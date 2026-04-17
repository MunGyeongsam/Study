-- XpOrb Component
-- 적 처치 시 드롭되는 경험치 오브

local XpOrb = {}

XpOrb.name = "XpOrb"

XpOrb.defaults = {
    value = 1,            -- XP 양
    magnetSpeed = 8,      -- 자석 끌림 속도
    attracted = false,    -- 자석에 끌리는 중
}

function XpOrb.new(data)
    return {
        value        = data and data.value        or XpOrb.defaults.value,
        magnetSpeed  = data and data.magnetSpeed  or XpOrb.defaults.magnetSpeed,
        attracted    = false,
    }
end

return XpOrb
