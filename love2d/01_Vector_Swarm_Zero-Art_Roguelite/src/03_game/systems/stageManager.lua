-- Stage Manager
-- Manages stage progression: waves → stage clear → next stage
-- Data-driven: STAGE_DEFS for hand-designed stages, formula for infinite stages.
-- Not an ECS system — called from ecsManager.update().

local world = require("01_core.world")
local EntityFactory = require("03_game.entities.entityFactory")
local gameState = require("03_game.states.gameState")
local background = require("02_renderer.background")
local achievementSystem = require("03_game.states.achievementSystem")

local _min    = math.min
local _max    = math.max
local _floor  = math.floor
local _random = math.random
local _char   = string.char

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

-- ===== Stage Theme Pools (5A.3f Rule 1) =====
-- 기동형: 이동으로 위협 (대쉬·반응 테스트)
-- 화력형: 탄막으로 위협 (포커스·패턴 읽기 테스트)
-- loop는 궤도(이동) + 나선탄(화력) = 양쪽 소속
local MOBILITY_POOL  = {"bit", "vector", "loop"}
local FIREPOWER_POOL = {"node", "loop", "matrix"}
local THEME_MIX_RATIO = 0.7  -- 테마 풀 70%, 전체 풀 30%

-- Count boss stages up to (and including) a given stage
local function _countBossStagesUpTo(stage)
    local count = 0
    for s, _ in pairs(BOSS_STAGES) do
        if s <= stage then count = count + 1 end
    end
    return count
end

-- Get non-boss ordinal for a stage (skipping boss stages)
-- Stage 7 → 7 - 2 (bosses at 3,6) = 5th non-boss → odd → mobility
local function _getNonBossOrdinal(stage)
    return stage - _countBossStagesUpTo(stage)
end

-- ===== Formation Definitions =====
-- Each formation: { name, types (preferred), count, tier (1-3), offsets(anchorX,anchorY) → {{dx,dy},...} }
local _cos = math.cos
local _sin = math.sin
local _pi  = math.pi

local FORMATION_DEFS = {
    -- 1) Wedge (V-shape): Bit swarm rushes in V formation
    {
        name  = "wedge",
        types = {"bit"},
        tier  = 1,
        getOffsets = function()
            local spacing = 0.35
            local offsets = {}
            local count = _random(7, 9)
            offsets[1] = {dx = 0, dy = 0}  -- leader center
            for i = 2, count do
                local row = _floor((i) / 2)
                local side = (i % 2 == 0) and 1 or -1
                offsets[i] = {dx = side * row * spacing, dy = row * spacing * 0.7}
            end
            return offsets
        end,
    },
    -- 2) Pincer: Two Vectors charge from left and right simultaneously
    {
        name  = "pincer",
        types = {"vector"},
        tier  = 2,
        getOffsets = function()
            return {
                {dx = -4.0, dy = 0},     -- left
                {dx =  4.0, dy = 0},     -- right
            }
        end,
        spawnMode = "sides",  -- anchor is player Y, spread on X
    },
    -- 3) Triangle turrets: 3 Nodes in triangle formation
    {
        name  = "triangle",
        types = {"node"},
        tier  = 2,
        getOffsets = function()
            local r = 1.8  -- triangle radius
            local offsets = {}
            for i = 0, 2 do
                local angle = _pi / 2 + i * (2 * _pi / 3)  -- top, bottom-left, bottom-right
                offsets[i + 1] = {dx = _cos(angle) * r, dy = _sin(angle) * r}
            end
            return offsets
        end,
    },
    -- 4) Escort: center tank (matrix/loop) + surrounding bit guards
    {
        name  = "escort",
        types = {"matrix", "bit"},  -- first = center, rest = guards
        tier  = 3,
        getOffsets = function()
            local guardCount = _random(4, 6)
            local offsets = {{dx = 0, dy = 0}}  -- center (tank type)
            for i = 1, guardCount do
                local angle = (i - 1) * (2 * _pi / guardCount)
                offsets[i + 1] = {dx = _cos(angle) * 0.8, dy = _sin(angle) * 0.8}
            end
            return offsets
        end,
    },
    -- 5) Spiral Array: Loops arranged on arc for overlapping spiral bullets
    {
        name  = "spiral_array",
        types = {"loop"},
        tier  = 3,
        getOffsets = function()
            local count = _random(3, 4)
            local r = 2.0
            local offsets = {}
            for i = 1, count do
                local angle = (i - 1) * (2 * _pi / count) + _pi / 4
                offsets[i] = {dx = _cos(angle) * r, dy = _sin(angle) * r}
            end
            return offsets
        end,
    },
}

-- Formation availability by stage tier
-- Returns formations available for the given stage
local function _getAvailableFormations(stage)
    local maxTier
    if stage <= 3 then return nil end       -- no formations stages 1-3
    if stage <= 5 then maxTier = 1          -- wedge only
    elseif stage <= 8 then maxTier = 2      -- + pincer, triangle
    else maxTier = 3 end                    -- + escort, spiral_array

    local available = {}
    for _, f in ipairs(FORMATION_DEFS) do
        if f.tier <= maxTier then
            available[#available + 1] = f
        end
    end
    return #available > 0 and available or nil
end

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
    -- Auto-generate for infinite stages (6+)
    -- Rule 1: theme-based type pool (mobility/firepower alternating)
    local s = self.stage
    local ordinal = _getNonBossOrdinal(s)
    local isMobility = (ordinal % 2 == 1)
    local themePool = isMobility and MOBILITY_POOL or FIREPOWER_POOL

    return {
        waves     = _min(5 + _floor((s - 5) / 2), 8),
        spawnDirs = {top = 0.4, left = 0.2, right = 0.2, bottom = 0.2},
        types     = themePool,       -- theme pool for _pickEnemyType
        allTypes  = ALL_ENEMY_TYPES, -- fallback for 30% mix
        isMobility = isMobility,
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
            self.stageClearProcessed = false
            logInfo(string.format("[STAGE] Stage %d CLEAR!", self.stage))
        else
            self.state = StageManager.STATE_WAVE_INTRO
            self.waveTimer = 0
        end

    elseif st == StageManager.STATE_STAGE_CLEAR then
        self.clearTimer = self.clearTimer + dt
        if not self.stageClearProcessed then
            self.stageClearProcessed = true
            self.ecsManager.bulletPool:clear()
            self:_vacuumXpOrbs()  -- XP auto-collect trigger
            -- Fragment stage clear bonus
            local bonus = self.stage * 2
            gameState.addFragments(bonus)
            logInfo(string.format("[STAGE] Fragment bonus: +%d (stage %d)", bonus, self.stage))
            -- Achievement: stage clear
            achievementSystem.onStageClear(self.stage)
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
                    self.bossClearProcessed = false
                    logInfo(string.format("[STAGE] Boss %s DEFEATED!", self.bossType))
                end
            end
        end

    elseif st == StageManager.STATE_BOSS_CLEAR then
        self.clearTimer = self.clearTimer + dt
        if not self.bossClearProcessed then
            self.bossClearProcessed = true
            -- Convert enemy bullets → XP orbs (visual payoff)
            local w = self.ecsManager.getWorld()
            local positions = self.ecsManager.bulletPool:harvestLayer("enemy_bullet")
            local converted = 0
            if w and #positions > 0 then
                for _, pos in ipairs(positions) do
                    EntityFactory.createXpOrb(w, pos.x, pos.y, 1)
                    converted = converted + 1
                end
                logInfo(string.format("[BOSS] %d bullets → XP orbs", converted))
            end
            self:_vacuumXpOrbs()  -- Boss XP burst auto-collect
            -- Fragment boss bonus: 5 + stage
            local bonus = 5 + self.stage
            gameState.addFragments(bonus)
            logInfo(string.format("[STAGE] Boss fragment bonus: +%d", bonus))
            -- Achievement: boss defeated
            achievementSystem.onBossDefeated(self.bossType)
            achievementSystem.onStageClear(self.stage)
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
-- Rule 1: themed stages use 70% theme pool, 30% all pool
function StageManager:_pickEnemyType(config, direction)
    local types
    -- Auto-generated stages have allTypes for 30% mix
    if config.allTypes and _random() >= THEME_MIX_RATIO then
        types = config.allTypes
    else
        types = config.types
    end
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

-- ===== Variant System (5A.3f Rules 2~3) =====

-- Rule 2: Guaranteed first encounter — wave 1 of these stages forces one variant enemy
local GUARANTEED_VARIANTS = {
    [4]  = "swift",
    [7]  = "armored",
    [10] = "splitter",
    [13] = "shielded",
}

-- Rule 3: Scaling variant chances (replaces fixed probability)
-- chance = baseChance × (1 + (stage - firstStage) × 0.03), cap 25% each
local VARIANT_TIERS = {
    { stage = 4,  variant = "swift",    baseChance = 0.15 },
    { stage = 7,  variant = "armored",  baseChance = 0.12 },
    { stage = 10, variant = "splitter", baseChance = 0.10 },
    { stage = 13, variant = "shielded", baseChance = 0.08 },
}
local VARIANT_CAP = 0.25  -- per-variant cap

local function _pickVariant(stage)
    for i = #VARIANT_TIERS, 1, -1 do
        local vt = VARIANT_TIERS[i]
        if stage >= vt.stage then
            local scaled = vt.baseChance * (1 + (stage - vt.stage) * 0.03)
            local chance = _min(scaled, VARIANT_CAP)
            if _random() < chance then
                return vt.variant
            end
        end
    end
    return nil
end

-- Spawn a formation group at anchor position
-- Rule 4: formation enemies can have variants
-- Returns the number of enemies spawned
function StageManager:_spawnFormation(formation, px, py, diff, guaranteedVariant)
    local left, bottom, right, top = world.getBounds()
    local offsets = formation.getOffsets()
    local types = formation.types

    -- Determine anchor position
    local anchorX, anchorY
    if formation.spawnMode == "sides" then
        -- Pincer: anchor at player position, offsets spread on X (horizontal charge)
        anchorX = px
        anchorY = py + (_random() - 0.5) * 2
    else
        -- Default: spawn from top area above player
        anchorX = px + (_random() - 0.5) * 4
        anchorY = py + 5 + _random() * 2
    end

    local spawned = 0
    for i, off in ipairs(offsets) do
        local sx = anchorX + off.dx
        local sy = anchorY + off.dy
        -- Clamp to world bounds
        sx = _max(left + 0.5, _min(right - 0.5, sx))
        sy = _max(bottom + 0.5, _min(top - 0.5, sy))

        -- Determine type for this slot
        local enemyType
        if formation.name == "escort" then
            enemyType = (i == 1) and types[1] or types[2]  -- center = tank, rest = bit
        else
            enemyType = types[1]
        end

        -- Variant: leader gets guaranteed variant, others roll individually
        local variant
        if i == 1 and guaranteedVariant then
            variant = guaranteedVariant
        else
            variant = _pickVariant(self.stage)
        end

        self.ecsManager.createEnemy(sx, sy, enemyType, diff, variant)
        spawned = spawned + 1
    end

    logInfo(string.format("[FORMATION] %s: %d enemies at (%.1f, %.1f)",
        formation.name, spawned, anchorX, anchorY))
    return spawned
end

-- Formation chance: increases with stage, caps at 60%
local function _formationChance(stage)
    if stage <= 3 then return 0 end
    return _min(0.6, 0.2 + (stage - 4) * 0.05)
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

    local formationSpawned = 0

    -- Rule 2: check guaranteed variant for this wave
    local guaranteedVariant = nil
    if self.wave == 1 then
        guaranteedVariant = GUARANTEED_VARIANTS[self.stage]
    end

    -- Try formation spawn (stage 4+)
    local available = _getAvailableFormations(self.stage)
    if available and _random() < _formationChance(self.stage) then
        local formation = available[_random(#available)]
        -- Check if formation types are in this stage's pool
        local typeOk = true
        for _, ft in ipairs(formation.types) do
            local found = false
            for _, ct in ipairs(config.types) do
                if ft == ct then found = true; break end
            end
            if not found then typeOk = false; break end
        end
        if typeOk then
            formationSpawned = self:_spawnFormation(formation, px, py, diff, guaranteedVariant)
            if guaranteedVariant and formationSpawned > 0 then
                guaranteedVariant = nil  -- consumed by formation leader
            end
        end
    end

    -- Spawn remaining enemies randomly
    local remaining = _max(0, count - formationSpawned)

    for i = 1, remaining do
        local direction = self:_pickSpawnDirection(config)
        local spawnX, spawnY = self:_getSpawnPosition(direction, px, py)
        local enemyType = self:_pickEnemyType(config, direction)

        -- First enemy of wave gets guaranteed variant (if applicable)
        local variant
        if i == 1 and guaranteedVariant then
            variant = guaranteedVariant
            guaranteedVariant = nil  -- consumed
        else
            variant = _pickVariant(self.stage)
        end

        if enemyType == "bit" then
            -- Bit: swarm spawn (3~5 clustered around position)
            local swarmCount = _random(3, 5)
            for _ = 1, swarmCount do
                local ox = spawnX + (_random() - 0.5) * 0.6
                local oy = spawnY + (_random() - 0.5) * 0.6
                self.ecsManager.createEnemy(ox, oy, "bit", diff, variant)
            end
        else
            self.ecsManager.createEnemy(spawnX, spawnY, enemyType, diff, variant)
        end
    end

    -- Log wave info with theme
    local themeName = config.isMobility == true and "MOB" or (config.isMobility == false and "FP" or "HAND")
    local fmtStr = formationSpawned > 0
        and "[STAGE] Stage %d [%s] Wave %d/%d: %d enemies (+%d formation) (HP:x%.2f Spd:x%.2f)"
        or  "[STAGE] Stage %d [%s] Wave %d/%d: %d enemies (HP:x%.2f Spd:x%.2f)"
    if formationSpawned > 0 then
        logInfo(string.format(fmtStr,
            self.stage, themeName, self.wave, config.waves, count, formationSpawned,
            diff.enemyHpMult, diff.enemySpeedMult))
    else
        logInfo(string.format(fmtStr,
            self.stage, themeName, self.wave, config.waves, count,
            diff.enemyHpMult, diff.enemySpeedMult))
    end

    -- Log guaranteed variant spawn
    if self.wave == 1 and GUARANTEED_VARIANTS[self.stage] then
        logInfo(string.format("[STAGE] Guaranteed variant: %s", GUARANTEED_VARIANTS[self.stage]))
    end
end

function StageManager:_advanceStage()
    self.stage = self.stage + 1
    self.wave = 0
    self.state = StageManager.STATE_NEXT_STAGE

    -- Player reset to bottom (0, -12)
    self:_resetPlayerToBottom()

    -- Background color transition
    background.setStage(self.stage)

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
        local bossName = self.bossType or "???"
        -- Glitch effect during intro: random chars that settle into boss name
        local introT = 0
        if self.bossEntityId then
            local w = self.ecsManager.getWorld()
            local bossTag = w:getComponent(self.bossEntityId, "BossTag")
            if bossTag then introT = bossTag.introTimer end
        end
        local glitchIntensity = _max(0, 1 - introT / 1.2)  -- settles by 1.2s
        local glitchChars = {}
        local display = "< " .. bossName .. " >"
        for i = 1, #display do
            local ch = display:sub(i, i)
            if ch ~= " " and ch ~= "<" and ch ~= ">" and _random() < glitchIntensity then
                glitchChars[i] = _char(_random(33, 126))
            else
                glitchChars[i] = ch
            end
        end
        local title = table.concat(glitchChars)
        drawOverlay(title, {1, 0.2, 0.2}, alpha, nil, 0.3 * alpha, 0.4)
        return
    end

    if self.state == StageManager.STATE_BOSS_CLEAR then
        local t = self.clearTimer
        local alpha = _min(1, t / 0.3)

        -- Glitch text: boss name scrambles for ~1.5s, then shows "PURIFIED"
        local bossName = self.bossType or "???"
        local title
        if t < 1.5 then
            -- Random glitch characters
            local glitchChars = {}
            local glitchIntensity = _max(0, 1 - t / 1.5)  -- fades from 1→0
            for i = 1, #bossName do
                if _random() < glitchIntensity then
                    glitchChars[i] = _char(_random(33, 126))
                else
                    glitchChars[i] = bossName:sub(i, i)
                end
            end
            title = table.concat(glitchChars)
        else
            title = "PURIFIED"
        end

        local sub = t >= 1.5 and string.format("%s — BOSS CLEAR!", bossName) or nil
        local titleColor = t < 1.5 and {1, 0.3, 0.3} or {0.2, 1, 0.5}
        drawOverlay(title, titleColor, alpha, sub, 0.4, 0.35)
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
