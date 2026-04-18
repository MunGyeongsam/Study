-- WorldBound Component
-- 엔티티를 월드 경계 안에 가둔다

local WorldBound = {}

WorldBound.name = "WorldBound"

WorldBound.defaults = {
    enabled = true,
}

function WorldBound.new(data)
    local d = data or {}
    local en = d.enabled
    if en == nil then en = WorldBound.defaults.enabled end
    return {
        enabled = en,
    }
end

return WorldBound
