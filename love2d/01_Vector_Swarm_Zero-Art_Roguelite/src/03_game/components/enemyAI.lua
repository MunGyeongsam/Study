-- EnemyAI Component
-- Controls enemy movement behavior.
-- AI behaviors: drift, orbit, chase, stationary, swarm, charge

local EnemyAI = {}

EnemyAI.name = "EnemyAI"

EnemyAI.defaults = {
    behavior  = "drift",   -- "drift", "orbit", "chase", "stationary", "swarm", "charge"
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
    -- swarm params (Bit: dash toward player, no stop)
    swarmSpeed = 0.8,
    -- charge params (Vector: warn → dash in fixed direction)
    chargeSpeed   = 4.0,
    chargeWarnTime = 0.8,   -- seconds of warning before dash
    chargeTimer   = 0,      -- internal timer
    chargePhase   = "warn", -- "warn" or "dash"
    chargeDirX    = 0,      -- locked direction
    chargeDirY    = 0,
    -- visual rotation speed (stationary Node)
    spinSpeed = 1.0,
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
        swarmSpeed   = d.swarmSpeed   or def.swarmSpeed,
        chargeSpeed  = d.chargeSpeed  or def.chargeSpeed,
        chargeWarnTime = d.chargeWarnTime or def.chargeWarnTime,
        chargeTimer  = 0,
        chargePhase  = "warn",
        chargeDirX   = 0,
        chargeDirY   = 0,
        spinSpeed    = d.spinSpeed    or def.spinSpeed,
        xpValue      = d.xpValue     or def.xpValue,
    }
end

return EnemyAI
