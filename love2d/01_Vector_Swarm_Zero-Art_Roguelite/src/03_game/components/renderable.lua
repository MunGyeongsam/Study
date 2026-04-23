-- Renderable Component
-- 엔티티의 시각적 표현

local Renderable = {}

Renderable.name = "Renderable"

Renderable.defaults = {
    type = "circle",
    radius = 0.1,
    width = 0.2,
    height = 0.2,
    color = {0, 1, 1, 1},
    visible = true,
    rotation = 0,
    curveName = nil,
    curveRole = nil,
    curveNormalization = nil,
}

function Renderable.new(data)
    local d = data or {}
    local vis = d.visible
    if vis == nil then vis = Renderable.defaults.visible end
    return {
        type     = d.type     or Renderable.defaults.type,
        radius   = d.radius   or Renderable.defaults.radius,
        width    = d.width    or Renderable.defaults.width,
        height   = d.height   or Renderable.defaults.height,
        color    = d.color    or {0, 1, 1, 1},
        visible  = vis,
        rotation = d.rotation or 0,
        variant  = d.variant,  -- nil for normal, "swift"/"armored"/etc for variants
        curveName = d.curveName,
        curveRole = d.curveRole,
        curveNormalization = d.curveNormalization,
    }
end

return Renderable
