-- Bullet Pool
-- Pre-allocated object pool for zero-GC bullet management.
-- Bullets live outside ECS for performance (no per-entity component overhead).
--
-- Usage:
--   local pool = BulletPool.new(2000)
--   pool:spawn(x, y, vx, vy, { maxLifetime = 5, radius = 0.04 })
--   pool:update(dt, worldBounds)
--   pool:draw()
--   pool:clear()

local BulletPool = {}
BulletPool.__index = BulletPool

--- Create a new bullet pool with pre-allocated slots.
---@param maxBullets number  Maximum simultaneous bullets (default 2000)
function BulletPool.new(maxBullets)
    maxBullets = maxBullets or 2000

    local pool = setmetatable({
        maxBullets    = maxBullets,
        active        = {},          -- active[1..activeCount] = bullet
        activeCount   = 0,
        inactive      = {},          -- recycled bullet tables
        inactiveCount = 0,
        stats         = { spawned = 0, recycled = 0, peakActive = 0 },
    }, BulletPool)

    -- Pre-allocate all bullet tables
    for i = 1, maxBullets do
        pool.inactive[i] = pool:_createBullet()
    end
    pool.inactiveCount = maxBullets

    return pool
end

--- Spawn a bullet from the pool.
---@return table|nil  The bullet, or nil if pool exhausted
function BulletPool:spawn(x, y, vx, vy, opts)
    if self.inactiveCount == 0 then return nil end

    -- Pop from inactive
    local bullet = self.inactive[self.inactiveCount]
    self.inactive[self.inactiveCount] = nil
    self.inactiveCount = self.inactiveCount - 1

    -- Initialize
    opts = opts or {}
    bullet.x  = x
    bullet.y  = y
    bullet.vx = vx
    bullet.vy = vy
    bullet.lifetime    = 0
    bullet.maxLifetime = opts.maxLifetime or 5
    bullet.radius      = opts.radius or 0.04
    bullet.color       = opts.color or {0.4, 0.8, 1, 1}
    bullet.active      = true
    bullet.layer       = opts.layer or "enemy_bullet"
    bullet.damage      = opts.damage or 1

    -- Push to active
    self.activeCount = self.activeCount + 1
    self.active[self.activeCount] = bullet

    -- Stats
    self.stats.spawned = self.stats.spawned + 1
    if self.activeCount > self.stats.peakActive then
        self.stats.peakActive = self.activeCount
    end

    return bullet
end

--- Update all active bullets: move, age, recycle dead ones.
---@param dt number  Delta time
---@param bounds table|nil  {minX, maxX, minY, maxY} — recycle bullets outside bounds
function BulletPool:update(dt, bounds)
    local i = 1
    while i <= self.activeCount do
        local b = self.active[i]

        -- Move
        b.x = b.x + b.vx * dt
        b.y = b.y + b.vy * dt
        b.lifetime = b.lifetime + dt

        -- Check death conditions
        local dead = b.lifetime >= b.maxLifetime

        if not dead and bounds then
            dead = b.x < bounds.minX or b.x > bounds.maxX
                or b.y < bounds.minY or b.y > bounds.maxY
        end

        if dead then
            self:_recycle(i)
            -- Don't increment i: swapped element now at index i
        else
            i = i + 1
        end
    end
end

--- Draw all active bullets.
function BulletPool:draw()
    if self.activeCount == 0 then return end

    local lg = love.graphics
    for i = 1, self.activeCount do
        local b = self.active[i]
        lg.setColor(b.color[1], b.color[2], b.color[3], b.color[4])
        lg.circle("fill", b.x, b.y, b.radius)
    end
    lg.setColor(1, 1, 1, 1)  -- restore default color
end

--- Remove all active bullets (e.g. on stage clear).
function BulletPool:clear()
    while self.activeCount > 0 do
        self:_recycle(self.activeCount)
    end
end

--- Get pool statistics.
function BulletPool:getStats()
    return {
        active      = self.activeCount,
        inactive    = self.inactiveCount,
        max         = self.maxBullets,
        spawned     = self.stats.spawned,
        recycled    = self.stats.recycled,
        peakActive  = self.stats.peakActive,
    }
end

---------------------------------------------------------------
-- Internal
---------------------------------------------------------------

function BulletPool:_createBullet()
    return {
        x = 0, y = 0,
        vx = 0, vy = 0,
        lifetime = 0, maxLifetime = 5,
        radius = 0.04,
        color = {0.4, 0.8, 1, 1},
        active = false,
        layer = "enemy_bullet",
        damage = 1,
    }
end

--- Swap-remove: move last active into slot i, push removed to inactive.
function BulletPool:_recycle(i)
    local bullet = self.active[i]
    bullet.active = false

    -- Swap with last active
    self.active[i] = self.active[self.activeCount]
    self.active[self.activeCount] = nil
    self.activeCount = self.activeCount - 1

    -- Push to inactive
    self.inactiveCount = self.inactiveCount + 1
    self.inactive[self.inactiveCount] = bullet

    self.stats.recycled = self.stats.recycled + 1
end

return BulletPool
