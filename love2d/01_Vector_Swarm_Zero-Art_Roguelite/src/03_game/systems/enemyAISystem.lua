-- Enemy AI System
-- Reads EnemyAI + Transform + Velocity, sets velocity based on behavior.
-- Receives playerQuery function via closure to find player position.

local System = require("01_core.system")

local cos = math.cos
local sin = math.sin
local sqrt = math.sqrt

local function createEnemyAISystem(getPlayerPos)

    local EnemyAISystem = System.new("EnemyAI", {"EnemyAI", "Transform", "Velocity"},
        function(ecs, dt, entities)
            local px, py = getPlayerPos()

            for _, entityId in ipairs(entities) do
                local ai        = ecs:getComponent(entityId, "EnemyAI")
                local transform = ecs:getComponent(entityId, "Transform")
                local velocity  = ecs:getComponent(entityId, "Velocity")
                local behavior  = ai.behavior

                if behavior == "drift" then
                    velocity.vx = ai.driftVx
                    velocity.vy = ai.driftVy

                elseif behavior == "orbit" then
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

                elseif behavior == "chase" then
                    if px and py then
                        local dx = px - transform.x
                        local dy = py - transform.y
                        local dist = sqrt(dx * dx + dy * dy)
                        if dist > 0.5 then  -- stop when close
                            velocity.vx = (dx / dist) * ai.chaseSpeed
                            velocity.vy = (dy / dist) * ai.chaseSpeed
                        else
                            velocity.vx = 0
                            velocity.vy = 0
                        end
                    end
                end
            end
        end
    )

    return EnemyAISystem
end

return createEnemyAISystem
