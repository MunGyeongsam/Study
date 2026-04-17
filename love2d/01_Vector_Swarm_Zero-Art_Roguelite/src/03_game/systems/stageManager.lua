-- Stage Manager
-- Manages stage progression: waves → stage clear → next stage
-- Not an ECS system — called from ecsManager.update().

local StageManager = {}
StageManager.__index = StageManager

-- Stage states
StageManager.STATE_WAVE_INTRO  = "wave_intro"   -- brief pause before wave starts
StageManager.STATE_SPAWNING    = "spawning"      -- enemies being spawned + fought
StageManager.STATE_CLEARING    = "clearing"      -- waiting for all enemies to die
StageManager.STATE_STAGE_CLEAR = "stage_clear"   -- "STAGE N CLEAR!" display
StageManager.STATE_NEXT_STAGE  = "next_stage"    -- brief pause before next stage

function StageManager.new(ecsManager, getPlayerPos)
    local mgr = setmetatable({
        ecsManager   = ecsManager,
        getPlayerPos = getPlayerPos,

        -- Progression
        stage          = 1,
        wave           = 0,        -- 0 = not started; incremented on spawn
        wavesPerStage  = 5,
        state          = StageManager.STATE_WAVE_INTRO,

        -- Wave spawning
        waveTimer      = 0,
        waveDelay      = 1.5,      -- pause between waves (seconds)
        clearTimer     = 0,
        clearDuration  = 2.0,      -- "STAGE CLEAR" display time

        -- Enemy type rotation
        enemyTypes = {"basic", "spiral", "aimed", "wave"},
        typeIndex  = 1,

        -- Cumulative stats
        totalWaves = 0,
        totalKills = 0,

        active = true,
    }, StageManager)
    return mgr
end

-- Difficulty parameters for current stage
function StageManager:getDifficulty()
    local s = self.stage
    return {
        enemyCount     = math.min(3 + (s - 1) * 2, 15),
        enemyHpMult    = 1.0 + (s - 1) * 0.15,
        enemySpeedMult = math.min(1.0 + (s - 1) * 0.1, 2.0),
        bulletSpeedMult = math.min(1.0 + (s - 1) * 0.05, 1.5),
        spawnDelay     = math.max(1.5, 3.0 - (s - 1) * 0.3),
    }
end

function StageManager:update(dt)
    if not self.active then return end

    local st = self.state

    if st == StageManager.STATE_WAVE_INTRO then
        self.waveTimer = self.waveTimer + dt
        local diff = self:getDifficulty()
        if self.waveTimer >= (self.wave == 0 and 1.0 or diff.spawnDelay) then
            self.waveTimer = 0
            self:_spawnWave()
            self.state = StageManager.STATE_SPAWNING
        end

    elseif st == StageManager.STATE_SPAWNING then
        -- Just wait for enemies to die — checked each frame
        if self:_countEnemies() == 0 then
            self.state = StageManager.STATE_CLEARING
        end

    elseif st == StageManager.STATE_CLEARING then
        -- All dead → is this the last wave of the stage?
        if self.wave >= self.wavesPerStage then
            self.state = StageManager.STATE_STAGE_CLEAR
            self.clearTimer = 0
            logInfo(string.format("[STAGE] Stage %d CLEAR!", self.stage))
        else
            -- Next wave
            self.state = StageManager.STATE_WAVE_INTRO
            self.waveTimer = 0
        end

    elseif st == StageManager.STATE_STAGE_CLEAR then
        self.clearTimer = self.clearTimer + dt
        -- Clear all bullets during celebration
        if self.clearTimer < 0.05 then
            self.ecsManager.bulletPool:clear()
        end
        if self.clearTimer >= self.clearDuration then
            self:_advanceStage()
        end

    elseif st == StageManager.STATE_NEXT_STAGE then
        -- Immediately start wave intro for new stage
        self.state = StageManager.STATE_WAVE_INTRO
        self.waveTimer = 0
    end
end

function StageManager:_spawnWave()
    self.wave = self.wave + 1
    self.totalWaves = self.totalWaves + 1

    local px, py = self.getPlayerPos()
    if not px then return end

    local diff = self:getDifficulty()
    local count = diff.enemyCount
    -- Early waves in a stage have fewer enemies (ramp within stage)
    local waveRatio = self.wave / self.wavesPerStage
    count = math.max(2, math.floor(count * (0.5 + 0.5 * waveRatio)))

    local viewTop = py + 6  -- just above visible area

    for i = 1, count do
        local offsetX = (math.random() - 0.5) * 8
        local offsetY = math.random() * 2
        local spawnX = px + offsetX
        local spawnY = viewTop + offsetY

        local enemyType = self.enemyTypes[self.typeIndex]
        self.typeIndex = (self.typeIndex % #self.enemyTypes) + 1

        self.ecsManager.createEnemy(spawnX, spawnY, enemyType, diff)
    end

    logInfo(string.format("[STAGE] Stage %d Wave %d/%d: %d enemies (HP:x%.2f Spd:x%.2f)",
        self.stage, self.wave, self.wavesPerStage, count,
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
    return {
        stage      = self.stage,
        wave       = self.wave,
        wavesPerStage = self.wavesPerStage,
        state      = self.state,
        totalWaves = self.totalWaves,
        enemies    = self:_countEnemies(),
    }
end

function StageManager:isClearing()
    return self.state == StageManager.STATE_STAGE_CLEAR
end

return StageManager
