-- BulletEmitter Component
-- Attached to entities (enemies) that fire bullets.
-- BulletEmitterSystem reads this to spawn bullets from BulletPool.

local BulletEmitter = {}

BulletEmitter.name = "BulletEmitter"

BulletEmitter.defaults = {
    pattern    = "circle",   -- "circle", "spiral", "aimed", "wave"
    emitRate   = 2,          -- shots per second
    bulletSpeed = 2.0,       -- bullet travel speed (world units/s)
    bulletCount = 8,         -- bullets per burst (circle pattern)
    bulletLifetime = 5,      -- seconds before bullet despawns
    bulletRadius = 0.04,     -- collision/visual radius
    bulletColor = {0.4, 0.8, 1, 1},  -- RGBA 0-1
    active     = true,       -- whether currently firing
    -- internal state
    timer      = 0,          -- accumulator for emit timing
    angle      = 0,          -- current rotation (spiral pattern)
    turnRate   = 1.5,        -- radians/s for spiral rotation
}

function BulletEmitter.new(data)
    return {
        pattern        = data and data.pattern        or BulletEmitter.defaults.pattern,
        emitRate       = data and data.emitRate       or BulletEmitter.defaults.emitRate,
        bulletSpeed    = data and data.bulletSpeed    or BulletEmitter.defaults.bulletSpeed,
        bulletCount    = data and data.bulletCount    or BulletEmitter.defaults.bulletCount,
        bulletLifetime = data and data.bulletLifetime or BulletEmitter.defaults.bulletLifetime,
        bulletRadius   = data and data.bulletRadius   or BulletEmitter.defaults.bulletRadius,
        bulletColor    = data and data.bulletColor    or BulletEmitter.defaults.bulletColor,
        active         = data and data.active ~= nil and data.active or BulletEmitter.defaults.active,
        timer          = 0,
        angle          = 0,
        turnRate       = data and data.turnRate       or BulletEmitter.defaults.turnRate,
    }
end

return BulletEmitter
