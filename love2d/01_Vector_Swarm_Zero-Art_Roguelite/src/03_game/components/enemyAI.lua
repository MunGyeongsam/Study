-- EnemyAI Component
-- Controls enemy movement behavior.
-- AI behaviors: drift (straight down), orbit (circle around point), chase (follow player)

local EnemyAI = {}

EnemyAI.name = "EnemyAI"

EnemyAI.defaults = {
    behavior  = "drift",   -- "drift", "orbit", "chase"
    speed     = 0.8,       -- movement speed
    -- orbit params
    orbitRadius = 2.0,
    orbitSpeed  = 1.0,     -- radians/sec
    orbitAngle  = 0,       -- current angle
    orbitCenterX = 0,
    orbitCenterY = 0,
    -- chase params
    chaseSpeed = 0.5,
    -- drift params
    driftVx = 0,
    driftVy = -0.5,        -- default: slow downward
    -- reward
    xpValue = 1,           -- XP dropped on death
}

function EnemyAI.new(data)
    local d = data or {}
    local def = EnemyAI.defaults
    return {
        behavior     = d.behavior     or def.behavior,
        speed        = d.speed        or def.speed,
        orbitRadius  = d.orbitRadius  or def.orbitRadius,
        orbitSpeed   = d.orbitSpeed   or def.orbitSpeed,
        orbitAngle   = d.orbitAngle   or def.orbitAngle,
        orbitCenterX = d.orbitCenterX or def.orbitCenterX,
        orbitCenterY = d.orbitCenterY or def.orbitCenterY,
        chaseSpeed   = d.chaseSpeed   or def.chaseSpeed,
        driftVx      = d.driftVx      or def.driftVx,
        driftVy      = d.driftVy      or def.driftVy,
        xpValue      = d.xpValue     or def.xpValue,
    }
end

return EnemyAI
