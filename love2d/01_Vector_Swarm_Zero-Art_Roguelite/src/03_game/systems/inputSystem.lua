-- Input System
-- Input + Velocity 컴포넌트를 가진 엔티티의 키보드/터치 입력 처리
-- 현재는 PlayerTag가 있는 엔티티만 키보드 입력을 받는다.

local System = require("01_core.system")

local sqrt = math.sqrt
local abs  = math.abs

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

                -- 대쉬: Shift 키 (이번 프레임 눌림 감지는 keypressed로)
                -- → DashSystem이 처리
                if ecs:hasComponent(entityId, "Dash") then
                    local dash = ecs:getComponent(entityId, "Dash")
                    if input.dash then
                        -- 이동 방향으로 대쉬 (입력 없으면 위쪽)
                        if abs(input.moveX) > 0.01 or abs(input.moveY) > 0.01 then
                            dash.dirX = input.moveX
                            dash.dirY = input.moveY
                        else
                            dash.dirX = 0
                            dash.dirY = 1
                        end
                        dash.requested = true
                        input.dash = false
                    end
                end

                -- 포커스: Space 키 (홀드)
                if ecs:hasComponent(entityId, "Focus") then
                    local focus = ecs:getComponent(entityId, "Focus")
                    focus.active = love.keyboard.isDown("space")
                end
            end

            -- 입력 → Velocity 반영 (대각선 보정)
            local mx, my = input.moveX, input.moveY
            local magnitude = sqrt(mx * mx + my * my)
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
