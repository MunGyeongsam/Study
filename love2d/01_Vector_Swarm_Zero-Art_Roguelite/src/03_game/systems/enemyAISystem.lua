-- Enemy AI System
-- Reads EnemyAI + Transform + Velocity, sets velocity based on behavior.
-- Receives playerQuery function via closure to find player position.
-- Each behavior is a separate handler for maintainability.

local System = require("01_core.system")

local cos = math.cos
local sin = math.sin
local sqrt = math.sqrt
local atan2 = math.atan2
local floor = math.floor

-- ===== Behavior Handlers =====
-- Each handler receives (ai, transform, velocity, ecs, entityId, px, py, dt)

local function handleDrift(ai, _, velocity)
    velocity.vx = ai.driftVx
    velocity.vy = ai.driftVy
end

local function handleOrbit(ai, transform, velocity, _, _, _, _, dt)
    ai.orbitAngle = ai.orbitAngle + ai.orbitSpeed * dt
    local targetX = ai.orbitCenterX + cos(ai.orbitAngle) * ai.orbitRadius
    local targetY = ai.orbitCenterY + sin(ai.orbitAngle) * ai.orbitRadius
    local dx = targetX - transform.x
    local dy = targetY - transform.y
    local dist = sqrt(dx * dx + dy * dy)
    if dist > 0.01 then
        velocity.vx = (dx / dist) * ai.speed
        velocity.vy = (dy / dist) * ai.speed
    else
        velocity.vx = 0
        velocity.vy = 0
    end
end

local function handleChase(ai, transform, velocity, _, _, px, py)
    if not (px and py) then return end
    local dx = px - transform.x
    local dy = py - transform.y
    local dist = sqrt(dx * dx + dy * dy)
    if dist > 0.5 then
        velocity.vx = (dx / dist) * ai.chaseSpeed
        velocity.vy = (dy / dist) * ai.chaseSpeed
    else
        velocity.vx = 0
        velocity.vy = 0
    end
end

local function handleStationary(ai, _, velocity, ecs, entityId, _, _, dt)
    velocity.vx = 0
    velocity.vy = 0
    local renderable = ecs:getComponent(entityId, "Renderable")
    if renderable then
        renderable.rotation = (renderable.rotation or 0) + ai.spinSpeed * dt
    end
end

local function handleSwarm(ai, transform, velocity, _, _, px, py)
    if not (px and py) then return end
    local dx = px - transform.x
    local dy = py - transform.y
    local dist = sqrt(dx * dx + dy * dy)
    if dist > 0.01 then
        velocity.vx = (dx / dist) * ai.swarmSpeed
        velocity.vy = (dy / dist) * ai.swarmSpeed
    end
end

local function handleCharge(ai, transform, velocity, ecs, entityId, px, py, dt)
    ai.chargeTimer = ai.chargeTimer + dt
    local renderable = ecs:getComponent(entityId, "Renderable")

    if ai.chargePhase == "warn" then
        velocity.vx = 0
        velocity.vy = 0
        -- Lock direction on first frame
        if ai.chargeDirX == 0 and ai.chargeDirY == 0 and px and py then
            local dx = px - transform.x
            local dy = py - transform.y
            local dist = sqrt(dx * dx + dy * dy)
            if dist > 0.01 then
                ai.chargeDirX = dx / dist
                ai.chargeDirY = dy / dist
            else
                ai.chargeDirX = 0
                ai.chargeDirY = -1
            end
        end
        if renderable then
            renderable.visible = (floor(ai.chargeTimer * 10) % 2 == 0)
            renderable.rotation = atan2(ai.chargeDirY, ai.chargeDirX)
        end
        -- Transition to dash
        if ai.chargeTimer >= ai.chargeWarnTime then
            ai.chargePhase = "dash"
            ai.chargeTimer = 0
            if renderable then renderable.visible = true end
        end
    elseif ai.chargePhase == "dash" then
        velocity.vx = ai.chargeDirX * ai.chargeSpeed
        velocity.vy = ai.chargeDirY * ai.chargeSpeed
        if renderable then
            renderable.rotation = atan2(ai.chargeDirY, ai.chargeDirX)
        end
    end
end

-- Dispatch table: behavior name → handler function
local behaviorHandlers = {
    drift      = handleDrift,
    orbit      = handleOrbit,
    chase      = handleChase,
    stationary = handleStationary,
    swarm      = handleSwarm,
    charge     = handleCharge,
}

-- ===== System Factory =====

local function createEnemyAISystem(getPlayerPos)

    local EnemyAISystem = System.new("EnemyAI", {"EnemyAI", "Transform", "Velocity"},
        function(ecs, dt, entities)
            local px, py = getPlayerPos()

            for _, entityId in ipairs(entities) do
                local ai        = ecs:getComponent(entityId, "EnemyAI")
                local transform = ecs:getComponent(entityId, "Transform")
                local velocity  = ecs:getComponent(entityId, "Velocity")

                local handler = behaviorHandlers[ai.behavior]
                if handler then
                    handler(ai, transform, velocity, ecs, entityId, px, py, dt)
                end
            end
        end
    )

    return EnemyAISystem
end

return createEnemyAISystem
