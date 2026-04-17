-- Boundary System
-- WorldBound + Transform 컴포넌트를 가진 엔티티를 월드 경계 안에 가둔다.

local System = require("01_core.system")
local world  = require("01_core.world")

local BoundarySystem = System.new("Boundary", {"Transform", "WorldBound"},
    function(ecs, dt, entities)
        local worldLeft, worldBottom, worldRight, worldTop = world.getBounds()

        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local worldBound = ecs:getComponent(entityId, "WorldBound")

            if worldBound.enabled then
                -- Collider 반지름 고려 (있으면)
                local radius = 0
                local collider = ecs:getComponent(entityId, "Collider")
                if collider then
                    radius = collider.radius or 0
                end

                transform.x = math.max(worldLeft   + radius,
                              math.min(worldRight  - radius, transform.x))
                transform.y = math.max(worldBottom + radius,
                              math.min(worldTop    - radius, transform.y))
            end
        end
    end
)

return BoundarySystem
