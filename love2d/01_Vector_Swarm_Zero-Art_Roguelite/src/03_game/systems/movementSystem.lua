-- Movement System
-- Transform + Velocity를 가진 엔티티의 위치를 업데이트

local System = require("01_core.system")

local MovementSystem = System.new("Movement", {"Transform", "Velocity"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local transform = ecs:getComponent(entityId, "Transform")
            local velocity  = ecs:getComponent(entityId, "Velocity")

            -- 속도에 따라 위치 업데이트
            transform.x = transform.x + velocity.vx * dt
            transform.y = transform.y + velocity.vy * dt

            -- 감속 적용
            velocity.vx = velocity.vx * velocity.damping
            velocity.vy = velocity.vy * velocity.damping
        end
    end
)

return MovementSystem
