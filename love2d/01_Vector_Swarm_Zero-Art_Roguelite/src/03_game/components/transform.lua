-- Transform Component
-- 엔티티의 위치, 회전, 크기

local Transform = {}

Transform.name = "Transform"

Transform.defaults = {
    x = 0,
    y = 0,
    angle = 0,
    scale = 1,
}

function Transform.new(data)
    return {
        x     = data and data.x     or Transform.defaults.x,
        y     = data and data.y     or Transform.defaults.y,
        angle = data and data.angle or Transform.defaults.angle,
        scale = data and data.scale or Transform.defaults.scale,
    }
end

return Transform
