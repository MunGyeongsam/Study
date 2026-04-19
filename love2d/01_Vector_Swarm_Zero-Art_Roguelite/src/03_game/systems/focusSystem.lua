-- Focus System
-- 포커스 모드: 에너지 소모 → 슬로모(timeScale) + 판정 축소 + 이동 속도 감소
-- 에너지 부족 시 자동 해제, 비활성 시 회복

local System   = require("01_core.system")
local gameState = require("03_game.states.gameState")

local FocusSystem = System.new("Focus", {"Focus", "Collider", "Velocity"},
    function(ecs, dt, entities)
        -- dt는 timeScale 적용된 값 → 에너지 관리에는 실제 dt 사용
        local scale = gameState.getTimeScale()
        local realDt = (scale > 0.001) and (dt / scale) or dt

        for _, entityId in ipairs(entities) do
            local focus    = ecs:getComponent(entityId, "Focus")
            local collider = ecs:getComponent(entityId, "Collider")
            local velocity = ecs:getComponent(entityId, "Velocity")

            if focus.active then
                -- 에너지 소모 (실제 dt 사용)
                focus.energy = focus.energy - focus.drainRate * realDt
                if focus.energy <= 0 then
                    focus.energy = 0
                    focus.active = false
                end
            end

            if focus.active then
                -- 슬로모
                gameState.setTimeScale(focus.slowFactor)
                -- 판정 축소 (원본 반지름 저장)
                if not focus._origRadius then
                    focus._origRadius = collider.radius
                end
                collider.radius = focus._origRadius * focus.colliderMult
                -- 이동 속도 감소
                if not focus._origSpeed then
                    focus._origSpeed = velocity.speed
                end
                velocity.speed = focus._origSpeed * focus.moveSpeedMult
            else
                -- 에너지 회복 (실제 dt 사용)
                if focus.energy < focus.maxEnergy then
                    focus.energy = focus.energy + focus.rechargeRate * realDt
                    if focus.energy > focus.maxEnergy then
                        focus.energy = focus.maxEnergy
                    end
                end
                -- 원래 값 복원
                gameState.setTimeScale(1.0)
                if focus._origRadius then
                    collider.radius = focus._origRadius
                    focus._origRadius = nil
                end
                if focus._origSpeed then
                    velocity.speed = focus._origSpeed
                    focus._origSpeed = nil
                end
            end


        end
    end
)

return FocusSystem
