-- BulletEmitter Component
-- Attached to entities (enemies) that fire bullets.
-- BulletEmitterSystem reads this to spawn bullets from BulletPool.

local BulletEmitter = {}

BulletEmitter.name = "BulletEmitter"

BulletEmitter.defaults = {
    pattern    = "circle",   -- "circle", "spiral", "aimed", "wave", "grid",
                             -- "ring_pulse", "cross", "orbit_shot", "return_shot"
                             -- (planned: "homing")
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
    local d = data or {}
    local def = BulletEmitter.defaults
    return {
        pattern        = d.pattern        or def.pattern,
        emitRate       = d.emitRate       or def.emitRate,
        bulletSpeed    = d.bulletSpeed    or def.bulletSpeed,
        bulletCount    = d.bulletCount    or def.bulletCount,
        bulletLifetime = d.bulletLifetime or def.bulletLifetime,
        bulletRadius   = d.bulletRadius   or def.bulletRadius,
        bulletColor    = d.bulletColor    or {0.4, 0.8, 1, 1},
        active         = d.active ~= nil and d.active or def.active,
        timer          = 0,
        angle          = 0,
        turnRate       = d.turnRate       or def.turnRate,
    }
end

return BulletEmitter
