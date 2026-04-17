-- LifeSpan System
-- 수명이 다한 엔티티를 자동으로 제거

local System = require("01_core.system")

local LifeSpanSystem = System.new("LifeSpan", {"LifeSpan"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local lifespan = ecs:getComponent(entityId, "LifeSpan")

            lifespan.time = lifespan.time - dt

            if lifespan.time <= 0 then
                ecs:destroyEntity(entityId)
            end
        end
    end
)

return LifeSpanSystem
