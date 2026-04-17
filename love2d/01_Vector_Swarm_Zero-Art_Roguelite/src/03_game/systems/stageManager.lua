-- Stage Manager
-- Manages stage progression: waves → stage clear → next stage
-- Data-driven: STAGE_DEFS for hand-designed stages, formula for infinite stages.
-- Not an ECS system — called from ecsManager.update().

local world = require("01_core.world")

local StageManager = {}
StageManager.__index = StageManager

-- Stage states
StageManager.STATE_WAVE_INTRO  = "wave_intro"
StageManager.STATE_SPAWNING    = "spawning"
StageManager.STATE_CLEARING    = "clearing"
StageManager.STATE_STAGE_CLEAR = "stage_clear"
StageManager.STATE_NEXT_STAGE  = "next_stage"

-- ===== Stage Definitions (hand-designed) =====
-- Stages beyond this table are auto-generated via _generateStageConfig().
local STAGE_DEFS = {
    [1] = { waves = 3, spawnDirs = {top=1.0},
            types = {"basic"} },
    [2] = { waves = 4, spawnDirs = {top=0.8, left=0.1, right=0.1},
            types = {"basic", "aimed"} },
    [3] = { waves = 5, spawnDirs = {top=0.6, left=0.15, right=0.15, bottom=0.1},
            types = {"basic", "spiral", "aimed"} },
    [4] = { waves = 5, spawnDirs = {top=0.5, left=0.2, right=0.2, bottom=0.1},
            types = {"basic", "spiral", "aimed", "wave"} },
    [5] = { waves = 6, spawnDirs = {top=0.4, left=0.2, right=0.2, bottom=0.2},
            types = {"basic", "spiral", "aimed", "wave"} },
}

local ALL_ENEMY_TYPES = {"basic", "spiral", "aimed", "wave"}

function StageManager.new(ecsManager, getPlayerPos)
    local mgr = setmetatable({
        ecsManager   = ecsManager,
        getPlayerPos = getPlayerPos,

        -- Progression
        stage          = 1,
        wave           = 0,
        state          = StageManager.STATE_WAVE_INTRO,

        -- Wave spawning
        waveTimer      = 0,
        clearTimer     = 0,
        clearDuration  = 2.0,

        -- Cumulative stats
        totalWaves = 0,

        active = true,
    }, StageManager)
    return mgr
end

-- Get stage config: hand-designed or auto-generated
function StageManager:_getStageConfig()
    if STAGE_DEFS[self.stage] then
        return STAGE_DEFS[self.stage]
    end
    -- Auto-generate for infinite stages
    local s = self.stage
    return {
        waves     = math.min(5 + math.floor((s - 5) / 2), 8),
        spawnDirs = {top = 0.4, left = 0.2, right = 0.2, bottom = 0.2},
        types     = ALL_ENEMY_TYPES,
    }
end

-- Difficulty parameters for current stage
function StageManager:getDifficulty()
    local s = self.stage
    return {
        enemyCount      = math.min(3 + (s - 1) * 2, 15),
        enemyHpMult     = 1.0 + (s - 1) * 0.15,
        enemySpeedMult  = math.min(1.0 + (s - 1) * 0.1, 2.0),
        bulletSpeedMult = math.min(1.0 + (s - 1) * 0.05, 1.5),
        spawnDelay      = math.max(1.5, 3.0 - (s - 1) * 0.3),
    }
end

function StageManager:update(dt)
    if not self.active then return end

    local st = self.state
    local config = self:_getStageConfig()

    if st == StageManager.STATE_WAVE_INTRO then
        self.waveTimer = self.waveTimer + dt
        local diff = self:getDifficulty()
        local delay = (self.wave == 0) and 1.0 or diff.spawnDelay
        if self.waveTimer >= delay then
            self.waveTimer = 0
            self:_spawnWave(config)
            self.state = StageManager.STATE_SPAWNING
        end

    elseif st == StageManager.STATE_SPAWNING then
        if self:_countEnemies() == 0 then
            self.state = StageManager.STATE_CLEARING
        end

    elseif st == StageManager.STATE_CLEARING then
        if self.wave >= config.waves then
            self.state = StageManager.STATE_STAGE_CLEAR
            self.clearTimer = 0
            logInfo(string.format("[STAGE] Stage %d CLEAR!", self.stage))
        else
            self.state = StageManager.STATE_WAVE_INTRO
            self.waveTimer = 0
        end

    elseif st == StageManager.STATE_STAGE_CLEAR then
        self.clearTimer = self.clearTimer + dt
        if self.clearTimer < 0.05 then
            self.ecsManager.bulletPool:clear()
        end
        if self.clearTimer >= self.clearDuration then
            self:_advanceStage()
        end

    elseif st == StageManager.STATE_NEXT_STAGE then
        self.state = StageManager.STATE_WAVE_INTRO
        self.waveTimer = 0
    end
end

-- Pick a spawn direction based on stage config probability table
function StageManager:_pickSpawnDirection(config)
    local dirs = config.spawnDirs
    local r = math.random()
    local cumulative = 0
    for dir, weight in pairs(dirs) do
        cumulative = cumulative + weight
        if r <= cumulative then
            return dir
        end
    end
    return "top"  -- fallback
end

-- Generate spawn position for a given direction
function StageManager:_getSpawnPosition(direction, px, py)
    local left, bottom, right, top = world.getBounds()
    local spawnX, spawnY

    if direction == "top" then
        spawnX = px + (math.random() - 0.5) * 8
        spawnY = py + 6 + math.random() * 2
    elseif direction == "bottom" then
        spawnX = px + (math.random() - 0.5) * 8
        spawnY = py - 6 - math.random() * 2
    elseif direction == "left" then
        spawnX = px - 3.5 - math.random() * 1.5
        spawnY = py + (math.random() - 0.5) * 8
    elseif direction == "right" then
        spawnX = px + 3.5 + math.random() * 1.5
        spawnY = py + (math.random() - 0.5) * 8
    end

    -- Clamp to world bounds (with small margin)
    spawnX = math.max(left + 0.5, math.min(right - 0.5, spawnX))
    spawnY = math.max(bottom + 0.5, math.min(top - 0.5, spawnY))

    return spawnX, spawnY
end

-- Pick enemy type from stage's type pool
function StageManager:_pickEnemyType(config, direction)
    local types = config.types
    -- Bottom spawns: avoid drift AI (would drift off-screen)
    if direction == "bottom" then
        local safe = {}
        for _, t in ipairs(types) do
            if t ~= "basic" and t ~= "wave" then  -- basic/wave use drift → skip
                safe[#safe + 1] = t
            end
        end
        if #safe > 0 then types = safe end
    end
    return types[math.random(#types)]
end

function StageManager:_spawnWave(config)
    self.wave = self.wave + 1
    self.totalWaves = self.totalWaves + 1

    local px, py = self.getPlayerPos()
    if not px then return end

    local diff = self:getDifficulty()
    local count = diff.enemyCount
    -- Ramp within stage: early waves fewer, last wave full count
    local waveRatio = self.wave / config.waves
    count = math.max(2, math.floor(count * (0.5 + 0.5 * waveRatio)))

    for i = 1, count do
        local direction = self:_pickSpawnDirection(config)
        local spawnX, spawnY = self:_getSpawnPosition(direction, px, py)
        local enemyType = self:_pickEnemyType(config, direction)

        self.ecsManager.createEnemy(spawnX, spawnY, enemyType, diff)
    end

    logInfo(string.format("[STAGE] Stage %d Wave %d/%d: %d enemies (HP:x%.2f Spd:x%.2f)",
        self.stage, self.wave, config.waves, count,
        diff.enemyHpMult, diff.enemySpeedMult))
end

function StageManager:_advanceStage()
    self.stage = self.stage + 1
    self.wave = 0
    self.state = StageManager.STATE_NEXT_STAGE
    logInfo(string.format("[STAGE] Advancing to Stage %d", self.stage))
end

-- Count alive enemies using ECS query (O(1) with componentIndex)
function StageManager:_countEnemies()
    local world = self.ecsManager.getWorld()
    if not world then return 0 end
    local enemies = world:queryEntities({"EnemyAI"})
    return #enemies
end

-- Draw stage clear overlay (called from main draw, screen coords)
function StageManager:draw()
    if self.state ~= StageManager.STATE_STAGE_CLEAR then return end

    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- Dim overlay
    lg.setColor(0, 0, 0, 0.4)
    lg.rectangle("fill", 0, 0, w, h)

    -- "STAGE N CLEAR!" text
    local alpha = math.min(1, self.clearTimer / 0.3)  -- fade in
    lg.setColor(0.2, 1, 0.4, alpha)
    local text = string.format("STAGE %d CLEAR!", self.stage)
    local font = lg.getFont()
    local tw = font:getWidth(text)
    local th = font:getHeight()
    -- Draw larger via scale
    local scale = 2.5
    lg.print(text, (w - tw * scale) / 2, h * 0.4 - th, 0, scale, scale)

    lg.setColor(1, 1, 1, alpha * 0.8)
    local sub = string.format("Waves: %d  Stage: %d", self.totalWaves, self.stage)
    local sw = font:getWidth(sub)
    lg.print(sub, (w - sw) / 2, h * 0.5)

    lg.setColor(1, 1, 1, 1)
end

function StageManager:getStats()
    local config = self:_getStageConfig()
    return {
        stage      = self.stage,
        wave       = self.wave,
        wavesPerStage = config.waves,
        state      = self.state,
        totalWaves = self.totalWaves,
        enemies    = self:_countEnemies(),
    }
end

function StageManager:isClearing()
    return self.state == StageManager.STATE_STAGE_CLEAR
end

return StageManager
