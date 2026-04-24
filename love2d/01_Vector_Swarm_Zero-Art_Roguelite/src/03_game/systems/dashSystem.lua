-- Dash System
-- Dash 컴포넌트의 요청을 처리: 순간이동 + 무적 부여
-- InputSystem이 dash.requested = true + dirX/dirY 설정 → 이 시스템이 실행
-- onDash 이벤트는 콜백으로 외부에 전달 (playScene이 trailRenderer에 연결)

local System = require("01_core.system")
local world = require("01_core.world")

local onDashCallback = nil  -- playScene이 설정하는 콜백 (trailRenderer.onDash)

local sqrt = math.sqrt
local max  = math.max
local min  = math.min

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
                local mag = sqrt(dirX * dirX + dirY * dirY)
                if mag < 0.01 then
                    goto nextEntity
                end

                -- 방향 정규화
                dirX, dirY = dirX / mag, dirY / mag
                local transform = ecs:getComponent(entityId, "Transform")

                -- 출발 위치 기록
                local startX, startY = transform.x, transform.y

                -- 이동
                transform.x = transform.x + dirX * dash.distance
                transform.y = transform.y + dirY * dash.distance

                -- 월드 경계 clamp (BoundarySystem 전에 실행되므로 직접 제한)
                local wLeft, wBottom, wRight, wTop = world.getBounds()
                local collider = ecs:getComponent(entityId, "Collider")
                local cr = collider and collider.radius or 0
                transform.x = max(wLeft + cr, min(wRight - cr, transform.x))
                transform.y = max(wBottom + cr, min(wTop - cr, transform.y))

                -- 리본 트레일에 대쉬 궤적 전달 (콜백 경유)
                if onDashCallback then
                    onDashCallback(startX, startY, transform.x, transform.y)
                end

                -- 무적 부여
                local health = ecs:getComponent(entityId, "Health")
                if health.iTimer < dash.iFrames then
                    health.iTimer = dash.iFrames
                end

                -- 쿨타임 시작
                dash.cooldownTimer = dash.cooldown
                if playSound then playSound("dash") end
                logInfo(string.format("[DASH] Entity %d dashed (%.1f, %.1f) CD: %.1fs",
                    entityId, dirX, dirY, dash.cooldown))
            end

            ::nextEntity::
        end
    end
)

--- 대쉬 이벤트 콜백 설정 (playScene이 trailRenderer.onDash를 연결)
function DashSystem.setOnDashCallback(cb)
    onDashCallback = cb
end

return DashSystem
