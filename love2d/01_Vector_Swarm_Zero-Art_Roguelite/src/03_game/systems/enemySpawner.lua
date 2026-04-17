-- Enemy Spawner
-- Time-based wave spawner. Spawns enemies near the player's visible area.
-- Not an ECS system — called from ecsManager.update().

local EnemySpawner = {}
EnemySpawner.__index = EnemySpawner

function EnemySpawner.new(ecsManager, getPlayerPos)
    local spawner = setmetatable({
        ecsManager   = ecsManager,
        getPlayerPos = getPlayerPos,
        timer        = 0,
        waveTimer    = 0,
        waveNumber   = 0,
        spawnInterval = 3.0,    -- seconds between waves
        maxEnemies   = 8,       -- max alive enemies at once
        enemyCount   = 0,       -- tracked externally (LifeSpan will destroy)
        active       = true,
        -- enemy type rotation
        enemyTypes   = {"basic", "spiral", "aimed", "wave"},
        typeIndex    = 1,
    }, EnemySpawner)
    return spawner
end

function EnemySpawner:update(dt)
    if not self.active then return end

    self.timer = self.timer + dt
    self.waveTimer = self.waveTimer + dt

    if self.waveTimer >= self.spawnInterval then
        self.waveTimer = self.waveTimer - self.spawnInterval
        self:_spawnWave()
    end
end

function EnemySpawner:_spawnWave()
    self.waveNumber = self.waveNumber + 1

    local px, py = self.getPlayerPos()
    if not px then return end

    -- Spawn 1-3 enemies above player view (camera sees ~5 units above/below)
    local count = math.min(1 + math.floor(self.waveNumber / 3), 3)
    local viewTop = py + 6  -- just above visible area

    for i = 1, count do
        -- Random X spread within ~4 units of player
        local offsetX = (math.random() - 0.5) * 8
        local offsetY = math.random() * 2  -- stagger vertically
        local spawnX = px + offsetX
        local spawnY = viewTop + offsetY

        -- Pick enemy type (rotate through types)
        local enemyType = self.enemyTypes[self.typeIndex]
        self.typeIndex = (self.typeIndex % #self.enemyTypes) + 1

        self.ecsManager.createEnemy(spawnX, spawnY, enemyType)
    end

    logInfo(string.format("[SPAWN] Wave %d: %d enemies", self.waveNumber, count))
end

function EnemySpawner:getStats()
    return {
        waveNumber = self.waveNumber,
        timer      = self.timer,
    }
end

return EnemySpawner
