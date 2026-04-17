-- Input System
-- Input + Velocity 컴포넌트를 가진 엔티티의 키보드/터치 입력 처리
-- 현재는 PlayerTag가 있는 엔티티만 키보드 입력을 받는다.

local System = require("01_core.system")

local InputSystem = System.new("Input", {"Input", "Velocity"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local input    = ecs:getComponent(entityId, "Input")
            local velocity = ecs:getComponent(entityId, "Velocity")

            -- 키보드 입력 읽기 (PlayerTag가 있는 엔티티만)
            if ecs:hasComponent(entityId, "PlayerTag") then
                input.moveX = 0
                input.moveY = 0

                if love.keyboard.isDown("w", "up") then
                    input.moveY = input.moveY + 1
                end
                if love.keyboard.isDown("s", "down") then
                    input.moveY = input.moveY - 1
                end
                if love.keyboard.isDown("a", "left") then
                    input.moveX = input.moveX - 1
                end
                if love.keyboard.isDown("d", "right") then
                    input.moveX = input.moveX + 1
                end
            end

            -- 입력 → Velocity 반영 (대각선 보정)
            local mx, my = input.moveX, input.moveY
            local magnitude = math.sqrt(mx * mx + my * my)
            if magnitude > 0 then
                velocity.vx = (mx / magnitude) * velocity.speed
                velocity.vy = (my / magnitude) * velocity.speed
            else
                velocity.vx = 0
                velocity.vy = 0
            end
        end
    end
)

return InputSystem
