-- Boundary System
-- WorldBound + Transform 컴포넌트를 가진 엔티티를 월드 경계 안에 가둔다.

local System = require("01_core.system")
local world  = require("01_core.world")

local max = math.max
local min = math.min

local BoundarySystem = System.new("Boundary", {"Transform", "WorldBound"},
    function(ecs, dt, entities)
        local worldLeft, worldBottom, worldRight, worldTop = world.getBounds()

        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local worldBound = ecs:getComponent(entityId, "WorldBound")

            if worldBound.enabled then
                local radius = 0
                local collider = ecs:getComponent(entityId, "Collider")
                if collider then
                    radius = collider.radius or 0
                end

                transform.x = max(worldLeft   + radius,
                              min(worldRight  - radius, transform.x))
                transform.y = max(worldBottom + radius,
                              min(worldTop    - radius, transform.y))
            end
        end
    end
)

return BoundarySystem
