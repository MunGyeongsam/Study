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
}

function Renderable.new(data)
    return {
        type    = data and data.type    or Renderable.defaults.type,
        radius  = data and data.radius  or Renderable.defaults.radius,
        width   = data and data.width   or Renderable.defaults.width,
        height  = data and data.height  or Renderable.defaults.height,
        color   = data and data.color   or Renderable.defaults.color,
        visible = data and data.visible ~= nil and data.visible or Renderable.defaults.visible,
    }
end

return Renderable
