-- Stage Manager
-- Manages stage progression: waves → stage clear → next stage
-- Data-driven: STAGE_DEFS for hand-designed stages, formula for infinite stages.
-- Not an ECS system — called from ecsManager.update().

local world = require("01_core.world")
local EntityFactory = require("03_game.entities.entityFactory")
local gameState = require("03_game.states.gameState")

local _min    = math.min
local _max    = math.max
local _floor  = math.floor
local _random = math.random

local StageManager = {}
StageManager.__index = StageManager

-- Stage states
StageManager.STATE_WAVE_INTRO  = "wave_intro"
StageManager.STATE_SPAWNING    = "spawning"
StageManager.STATE_CLEARING    = "clearing"
StageManager.STATE_STAGE_CLEAR = "stage_clear"
StageManager.STATE_COLLECTING  = "collecting"    -- XP auto-vacuum phase
StageManager.STATE_NEXT_STAGE  = "next_stage"
StageManager.STATE_BOSS_INTRO  = "boss_intro"
StageManager.STATE_BOSS_ACTIVE = "boss_active"
StageManager.STATE_BOSS_CLEAR  = "boss_clear"

-- ===== Boss stage mapping =====
local BOSS_STAGES = {
    [3]  = "NULL",
    [6]  = "STACK",
    [9]  = "HEAP",
    [12] = "RECURSION",
    [15] = "OVERFLOW",
}

-- ===== Stage Definitions (hand-designed) =====
-- Stages beyond this table are auto-generated via _generateStageConfig().
local STAGE_DEFS = {
    [1] = { waves = 3, spawnDirs = {top=1.0},
            types = {"bit", "node"} },
    [2] = { waves = 4, spawnDirs = {top=0.8, left=0.1, right=0.1},
            types = {"bit", "node", "vector"} },
    [3] = { waves = 5, spawnDirs = {top=0.6, left=0.15, right=0.15, bottom=0.1},
            types = {"bit", "node", "vector"} },
    [4] = { waves = 5, spawnDirs = {top=0.5, left=0.2, right=0.2, bottom=0.1},
            types = {"bit", "node", "vector", "loop"} },
    [5] = { waves = 6, spawnDirs = {top=0.4, left=0.2, right=0.2, bottom=0.2},
            types = {"node", "vector", "loop", "matrix"} },
}

local ALL_ENEMY_TYPES = {"bit", "node", "vector", "loop", "matrix"}

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
        collectDuration = 1.5,  -- XP vacuum time
        collectTimer   = 0,

        -- Cumulative stats
        totalWaves = 0,

        active = true,

        -- Boss tracking
        bossEntityId = nil,
        bossType     = nil,
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
        waves     = _min(5 + _floor((s - 5) / 2), 8),
        spawnDirs = {top = 0.4, left = 0.2, right = 0.2, bottom = 0.2},
        types     = ALL_ENEMY_TYPES,
    }
end

-- Difficulty parameters for current stage
function StageManager:getDifficulty()
    local s = self.stage
    return {
        enemyCount      = _min(3 + (s - 1) * 2, 15),
        enemyHpMult     = 1.0 + (s - 1) * 0.15,
        enemySpeedMult  = _min(1.0 + (s - 1) * 0.1, 2.0),
        bulletSpeedMult = _min(1.0 + (s - 1) * 0.05, 1.5),
        spawnDelay      = _max(1.5, 3.0 - (s - 1) * 0.3),
    }
end

function StageManager:update(dt)
    if not self.active then return end

    local st = self.state
    local config = self:_getStageConfig()

    if st == StageManager.STATE_WAVE_INTRO then
        -- Check if this is a boss stage
        if BOSS_STAGES[self.stage] then
            self:_spawnBoss()
            self.state = StageManager.STATE_BOSS_INTRO
            if playBGM then playBGM("boss") end
            return
        end

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
            self:_vacuumXpOrbs()  -- XP auto-collect trigger
            -- Fragment stage clear bonus
            local bonus = self.stage * 2
            gameState.addFragments(bonus)
            logInfo(string.format("[STAGE] Fragment bonus: +%d (stage %d)", bonus, self.stage))
        end
        if self.clearTimer >= self.clearDuration then
            self.state = StageManager.STATE_COLLECTING
            self.collectTimer = 0
        end

    elseif st == StageManager.STATE_COLLECTING then
        self.collectTimer = self.collectTimer + dt
        -- Keep XP orbs attracted during collection phase
        self:_vacuumXpOrbs()
        -- Check if all orbs collected or time expired
        local orbCount = self:_countXpOrbs()
        if orbCount == 0 or self.collectTimer >= self.collectDuration then
            self:_advanceStage()
        end

    -- === BOSS STATES ===
    elseif st == StageManager.STATE_BOSS_INTRO then
        -- BossSystem handles intro timer; we wait for introComplete
        if self.bossEntityId then
            local w = self.ecsManager.getWorld()
            if w then
                local bossTag = w:getComponent(self.bossEntityId, "BossTag")
                if bossTag and bossTag.introComplete then
                    self.state = StageManager.STATE_BOSS_ACTIVE
                    logInfo(string.format("[STAGE] Boss %s fight started!", self.bossType))
                end
            end
        end

    elseif st == StageManager.STATE_BOSS_ACTIVE then
        -- Check if boss is dead
        if self.bossEntityId then
            local w = self.ecsManager.getWorld()
            if w then
                local health = w:getComponent(self.bossEntityId, "Health")
                if not health or not health.alive then
                    self.state = StageManager.STATE_BOSS_CLEAR
                    self.clearTimer = 0
                    logInfo(string.format("[STAGE] Boss %s DEFEATED!", self.bossType))
                end
            end
        end

    elseif st == StageManager.STATE_BOSS_CLEAR then
        self.clearTimer = self.clearTimer + dt
        if self.clearTimer < 0.05 then
            self.ecsManager.bulletPool:clearLayer("enemy_bullet")
            self:_vacuumXpOrbs()  -- Boss XP burst auto-collect
            -- Fragment boss bonus: 5 + stage
            local bonus = 5 + self.stage
            gameState.addFragments(bonus)
            logInfo(string.format("[STAGE] Boss fragment bonus: +%d", bonus))
        end
        if self.clearTimer >= self.clearDuration + 1.0 then  -- extra second for boss
            -- Destroy boss entity from ECS
            if self.bossEntityId then
                local w = self.ecsManager.getWorld()
                if w then
                    w:destroyEntity(self.bossEntityId)
                    logInfo(string.format("[STAGE] Boss entity %d destroyed", self.bossEntityId))
                end
            end
            self.bossEntityId = nil
            self.bossType = nil
            -- Enter collecting phase (XP vacuum) before advancing
            self.state = StageManager.STATE_COLLECTING
            self.collectTimer = 0
            if playBGM then playBGM("stage") end
        end

    elseif st == StageManager.STATE_NEXT_STAGE then
        self.state = StageManager.STATE_WAVE_INTRO
        self.waveTimer = 0
    end
end

-- Spawn boss entity for current stage
function StageManager:_spawnBoss()
    local bossType = BOSS_STAGES[self.stage]
    if not bossType then return end

    local px, py = self.getPlayerPos()
    local spawnX = px or 0
    local spawnY = (py or 0) + 4  -- spawn above player

    local w = self.ecsManager.getWorld()
    self.bossEntityId = EntityFactory.createBoss(w, spawnX, spawnY, bossType)
    self.bossType = bossType
    self.bossRewardsApplied = false

    logInfo(string.format("[STAGE] Boss %s spawned at Stage %d", bossType, self.stage))
end

-- Pick a spawn direction based on stage config probability table
function StageManager:_pickSpawnDirection(config)
    local dirs = config.spawnDirs
    local r = _random()
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
        spawnX = px + (_random() - 0.5) * 8
        spawnY = py + 6 + _random() * 2
    elseif direction == "bottom" then
        spawnX = px + (_random() - 0.5) * 8
        spawnY = py - 6 - _random() * 2
    elseif direction == "left" then
        spawnX = px - 3.5 - _random() * 1.5
        spawnY = py + (_random() - 0.5) * 8
    elseif direction == "right" then
        spawnX = px + 3.5 + _random() * 1.5
        spawnY = py + (_random() - 0.5) * 8
    end

    -- Clamp to world bounds (with small margin)
    spawnX = _max(left + 0.5, _min(right - 0.5, spawnX))
    spawnY = _max(bottom + 0.5, _min(top - 0.5, spawnY))

    return spawnX, spawnY
end

-- Pick enemy type from stage's type pool
function StageManager:_pickEnemyType(config, direction)
    local types = config.types
    -- Bottom spawns: avoid drift-based AI (node=stationary, matrix=drift → skip)
    if direction == "bottom" then
        local safe = {}
        for _, t in ipairs(types) do
            if t ~= "node" and t ~= "matrix" then
                safe[#safe + 1] = t
            end
        end
        if #safe > 0 then types = safe end
    end
    return types[_random(#types)]
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
    count = _max(2, _floor(count * (0.5 + 0.5 * waveRatio)))

    for i = 1, count do
        local direction = self:_pickSpawnDirection(config)
        local spawnX, spawnY = self:_getSpawnPosition(direction, px, py)
        local enemyType = self:_pickEnemyType(config, direction)

        if enemyType == "bit" then
            -- Bit: swarm spawn (3~5 clustered around position)
            local swarmCount = _random(3, 5)
            for _ = 1, swarmCount do
                local ox = spawnX + (_random() - 0.5) * 0.6
                local oy = spawnY + (_random() - 0.5) * 0.6
                self.ecsManager.createEnemy(ox, oy, "bit", diff)
            end
        else
            self.ecsManager.createEnemy(spawnX, spawnY, enemyType, diff)
        end
    end

    logInfo(string.format("[STAGE] Stage %d Wave %d/%d: %d enemies (HP:x%.2f Spd:x%.2f)",
        self.stage, self.wave, config.waves, count,
        diff.enemyHpMult, diff.enemySpeedMult))
end

function StageManager:_advanceStage()
    self.stage = self.stage + 1
    self.wave = 0
    self.state = StageManager.STATE_NEXT_STAGE

    -- Player reset to bottom (0, -12)
    self:_resetPlayerToBottom()

    logInfo(string.format("[STAGE] Advancing to Stage %d", self.stage))
end

-- Count alive enemies using ECS query (O(1) with componentIndex)
function StageManager:_countEnemies()
    local world = self.ecsManager.getWorld()
    if not world then return 0 end
    local enemies = world:queryEntities({"EnemyAI"})
    return #enemies
end

-- Count remaining XP orbs
function StageManager:_countXpOrbs()
    local w = self.ecsManager.getWorld()
    if not w then return 0 end
    local orbs = w:queryEntities({"XpOrb"})
    return #orbs
end

-- Force all XP orbs to be attracted to player (instant vacuum)
local VACUUM_SPEED = 15  -- very fast magnet speed for vacuum
function StageManager:_vacuumXpOrbs()
    local w = self.ecsManager.getWorld()
    if not w then return end
    local orbs = w:queryEntities({"XpOrb"})
    for _, orbId in ipairs(orbs) do
        local orb = w:getComponent(orbId, "XpOrb")
        if orb then
            orb.attracted = true
            orb.magnetSpeed = VACUUM_SPEED
        end
    end
end

-- Reset player position to bottom center (0, -12)
local STAGE_START_Y = -12
function StageManager:_resetPlayerToBottom()
    local w = self.ecsManager.getWorld()
    if not w then return end
    local players = w:queryEntities({"PlayerTag", "Transform"})
    if #players > 0 then
        local transform = w:getComponent(players[1], "Transform")
        if transform then
            transform.x = 0
            transform.y = STAGE_START_Y
            logInfo(string.format("[STAGE] Player reset to (0, %d)", STAGE_START_Y))
        end
    end
end

-- Debug: skip current stage instantly (F8)
function StageManager:debugSkipStage()
    local w = self.ecsManager.getWorld()
    if not w then return end

    -- Destroy all enemies
    local enemies = w:queryEntities({"EnemyAI"})
    for _, eid in ipairs(enemies) do
        w:destroyEntity(eid)
    end

    -- Clear all enemy bullets
    local bp = self.ecsManager.getBulletPool and self.ecsManager.getBulletPool()
    if bp then bp:clear() end

    -- Destroy boss entity if active
    if self.bossEntityId then
        -- Entity may already be destroyed above (it has EnemyAI)
        self.bossEntityId = nil
    end

    -- Force advance
    self:_advanceStage()
    -- BGM: next stage could be boss or normal
    if BOSS_STAGES[self.stage] then
        if playBGM then playBGM("boss") end
    else
        if playBGM then playBGM("stage") end
    end
    logInfo(string.format("[DEBUG] Stage skipped → now Stage %d", self.stage))
end

-- Helper: draw centered overlay text with optional subtitle
local function drawOverlay(title, titleColor, alpha, subtitle, bgAlpha, yPos)
    local lg = love.graphics
    local w, h = lg.getDimensions()

    lg.setColor(0, 0, 0, bgAlpha or 0.4)
    lg.rectangle("fill", 0, 0, w, h)

    lg.setColor(titleColor[1], titleColor[2], titleColor[3], alpha)
    local font = lg.getFont()
    local tw = font:getWidth(title)
    local scale = 2.5
    lg.print(title, (w - tw * scale) / 2, (yPos or 0.4) * h - font:getHeight(), 0, scale, scale)

    if subtitle then
        lg.setColor(1, 1, 1, alpha * 0.8)
        local sw = font:getWidth(subtitle)
        lg.print(subtitle, (w - sw) / 2, h * 0.5)
    end

    lg.setColor(1, 1, 1, 1)
end

-- Draw stage clear overlay (called from main draw, screen coords)
function StageManager:draw()
    if self.state ~= StageManager.STATE_STAGE_CLEAR
       and self.state ~= StageManager.STATE_BOSS_INTRO
       and self.state ~= StageManager.STATE_BOSS_CLEAR then
        return
    end

    if self.state == StageManager.STATE_BOSS_INTRO then
        local alpha = self.bossEntityId and 1 or 0
        local title = string.format("BOSS: %s", self.bossType or "???")
        drawOverlay(title, {1, 0.2, 0.2}, alpha, nil, 0.3 * alpha, 0.4)
        return
    end

    if self.state == StageManager.STATE_BOSS_CLEAR then
        local alpha = _min(1, self.clearTimer / 0.3)
        local sub = string.format("%s — PURIFIED", self.bossType or "")
        drawOverlay("BOSS CLEAR!", {1, 0.85, 0.2}, alpha, sub, 0.4, 0.35)
        return
    end

    -- Stage clear
    local alpha = _min(1, self.clearTimer / 0.3)
    local title = string.format("STAGE %d CLEAR!", self.stage)
    local sub = string.format("Waves: %d  Stage: %d", self.totalWaves, self.stage)
    drawOverlay(title, {0.2, 1, 0.4}, alpha, sub, 0.4, 0.4)
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
        isBossStage = BOSS_STAGES[self.stage] ~= nil,
        bossType    = self.bossType,
        bossEntityId = self.bossEntityId,
    }
end

function StageManager:isClearing()
    return self.state == StageManager.STATE_STAGE_CLEAR
        or self.state == StageManager.STATE_BOSS_CLEAR
        or self.state == StageManager.STATE_COLLECTING
end

return StageManager
