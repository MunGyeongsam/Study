-- LifeSpan System
-- 수명이 다한 엔티티를 자동으로 제거
-- 적(EnemyAI)의 잔여 수명 < 2초 → 깜빡임 경고 (디스폰 예고)

local System = require("01_core.system")

local floor = math.floor

local BLINK_THRESHOLD = 2.0   -- seconds before death to start blinking
local BLINK_INTERVAL  = 0.15  -- blink toggle interval

local LifeSpanSystem = System.new("LifeSpan", {"LifeSpan"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local lifespan = ecs:getComponent(entityId, "LifeSpan")

            lifespan.time = lifespan.time - dt

            -- Despawn blink for enemies about to expire
            if lifespan.time <= BLINK_THRESHOLD and lifespan.time > 0 then
                local renderable = ecs:getComponent(entityId, "Renderable")
                if renderable and ecs:getComponent(entityId, "EnemyAI") then
                    -- Toggle visible at BLINK_INTERVAL
                    local phase = floor(lifespan.time / BLINK_INTERVAL)
                    renderable.visible = (phase % 2 == 0)
                end
            end

            if lifespan.time <= 0 then
                ecs:destroyEntity(entityId)
            end
        end
    end
)

return LifeSpanSystem
