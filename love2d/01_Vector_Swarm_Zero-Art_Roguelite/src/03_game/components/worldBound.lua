-- WorldBound Component
-- 엔티티를 월드 경계 안에 가둔다

local WorldBound = {}

WorldBound.name = "WorldBound"

WorldBound.defaults = {
    enabled = true,
}

function WorldBound.new(data)
    return {
        enabled = data and data.enabled ~= nil and data.enabled or true,
    }
end

return WorldBound
