-- LifeSpan Component
-- 엔티티 자동 제거 타이머

local LifeSpan = {}

LifeSpan.name = "LifeSpan"

LifeSpan.defaults = {
    time = 10,
    destroyOffScreen = false,
}

function LifeSpan.new(data)
    local d = data or {}
    local offScreen = d.destroyOffScreen
    if offScreen == nil then offScreen = LifeSpan.defaults.destroyOffScreen end
    return {
        time             = d.time or LifeSpan.defaults.time,
        destroyOffScreen = offScreen,
    }
end

return LifeSpan
