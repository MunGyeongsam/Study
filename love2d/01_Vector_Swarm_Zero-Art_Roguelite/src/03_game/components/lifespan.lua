-- LifeSpan Component
-- 엔티티 자동 제거 타이머

local LifeSpan = {}

LifeSpan.name = "LifeSpan"

LifeSpan.defaults = {
    time = 10,
    destroyOffScreen = false,
}

function LifeSpan.new(data)
    return {
        time             = data and data.time             or LifeSpan.defaults.time,
        destroyOffScreen = data and data.destroyOffScreen ~= nil and data.destroyOffScreen or LifeSpan.defaults.destroyOffScreen,
    }
end

return LifeSpan
