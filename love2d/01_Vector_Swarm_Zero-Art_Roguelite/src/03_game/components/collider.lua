-- Collider Component
-- 충돌 판정 데이터

local Collider = {}

Collider.name = "Collider"

Collider.defaults = {
    type = "circle",
    radius = 0.1,
    layer = "default",
    mask = {},
}

function Collider.new(data)
    return {
        type   = data and data.type   or Collider.defaults.type,
        radius = data and data.radius or Collider.defaults.radius,
        layer  = data and data.layer  or Collider.defaults.layer,
        mask   = data and data.mask   or {},
    }
end

return Collider
