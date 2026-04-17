-- Dash System
-- Dash 컴포넌트의 요청을 처리: 순간이동 + 무적 부여
-- InputSystem이 dash.requested = true + dirX/dirY 설정 → 이 시스템이 실행

local System = require("01_core.system")

local DashSystem = System.new("Dash", {"Dash", "Transform", "Health"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local dash = ecs:getComponent(entityId, "Dash")

            -- 쿨타임 감소
            if dash.cooldownTimer > 0 then
                dash.cooldownTimer = dash.cooldownTimer - dt
                if dash.cooldownTimer < 0 then
                    dash.cooldownTimer = 0
                end
            end

            -- 대쉬 요청 처리
            if dash.requested then
                dash.requested = false

                if dash.cooldownTimer > 0 then
                    goto nextEntity
                end

                local dirX, dirY = dash.dirX, dash.dirY
                local mag = math.sqrt(dirX * dirX + dirY * dirY)
                if mag < 0.01 then
                    goto nextEntity
                end

                -- 방향 정규화 + 이동
                dirX, dirY = dirX / mag, dirY / mag
                local transform = ecs:getComponent(entityId, "Transform")
                transform.x = transform.x + dirX * dash.distance
                transform.y = transform.y + dirY * dash.distance

                -- 무적 부여
                local health = ecs:getComponent(entityId, "Health")
                if health.iTimer < dash.iFrames then
                    health.iTimer = dash.iFrames
                end

                -- 쿨타임 시작
                dash.cooldownTimer = dash.cooldown

                logInfo(string.format("[DASH] Entity %d dashed (%.1f, %.1f) CD: %.1fs",
                    entityId, dirX, dirY, dash.cooldown))
            end

            ::nextEntity::
        end
    end
)

return DashSystem
