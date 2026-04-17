-- Focus Component
-- 포커스 모드 (슬로모 + 판정 축소 + 정밀 이동)

local Focus = {}

Focus.name = "Focus"

Focus.defaults = {
    active = false,       -- 포커스 활성 여부
    energy = 100,         -- 현재 에너지
    maxEnergy = 100,      -- 최대 에너지
    drainRate = 30,       -- 초당 에너지 소모
    rechargeRate = 15,    -- 초당 에너지 회복 (비활성 시)
    slowFactor = 0.4,     -- 시간 느려짐 배율 (0.4 = 40% 속도)
    moveSpeedMult = 0.5,  -- 이동 속도 배율
    colliderMult = 0.4,   -- 판정 축소 배율
}

function Focus.new(data)
    return {
        active        = false,
        energy        = data and data.energy        or Focus.defaults.energy,
        maxEnergy     = data and data.maxEnergy     or Focus.defaults.maxEnergy,
        drainRate     = data and data.drainRate     or Focus.defaults.drainRate,
        rechargeRate  = data and data.rechargeRate  or Focus.defaults.rechargeRate,
        slowFactor    = data and data.slowFactor    or Focus.defaults.slowFactor,
        moveSpeedMult = data and data.moveSpeedMult or Focus.defaults.moveSpeedMult,
        colliderMult  = data and data.colliderMult  or Focus.defaults.colliderMult,
    }
end

return Focus
