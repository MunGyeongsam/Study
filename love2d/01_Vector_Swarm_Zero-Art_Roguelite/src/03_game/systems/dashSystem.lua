-- Dash System
-- Dash 컴포넌트의 요청을 처리: 순간이동 + 무적 부여
-- InputSystem이 dash.requested = true + dirX/dirY 설정 → 이 시스템이 실행

local System = require("01_core.system")

local GHOST_COUNT    = 4
local GHOST_DURATION = 0.4

local DashSystem = System.new("Dash", {"Dash", "Transform", "Health"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local dash = ecs:getComponent(entityId, "Dash")

            -- 고스트 타이머 감소 (매 프레임)
            local ghosts = dash.ghosts
            local i = 1
            while i <= #ghosts do
                ghosts[i].timer = ghosts[i].timer - dt
                if ghosts[i].timer <= 0 then
                    ghosts[i] = ghosts[#ghosts]
                    ghosts[#ghosts] = nil
                else
                    i = i + 1
                end
            end

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

                -- 방향 정규화
                dirX, dirY = dirX / mag, dirY / mag
                local transform = ecs:getComponent(entityId, "Transform")
                local renderable = ecs:getComponent(entityId, "Renderable")
                local radius = renderable and renderable.radius or 0.15

                -- 출발 위치 기록
                local startX, startY = transform.x, transform.y

                -- 이동
                transform.x = transform.x + dirX * dash.distance
                transform.y = transform.y + dirY * dash.distance

                -- 고스트 생성 (출발→도착 사이 균등 배치)
                local endX, endY = transform.x, transform.y
                for g = 1, GHOST_COUNT do
                    local t = (g - 1) / (GHOST_COUNT - 1)  -- 0, 0.33, 0.67, 1.0
                    ghosts[#ghosts + 1] = {
                        x       = startX + (endX - startX) * t,
                        y       = startY + (endY - startY) * t,
                        timer   = GHOST_DURATION,
                        maxTime = GHOST_DURATION,
                        radius  = radius,
                    }
                end

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
