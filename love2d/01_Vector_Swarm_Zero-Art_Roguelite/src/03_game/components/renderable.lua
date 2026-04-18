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
    local d = data or {}
    local vis = d.visible
    if vis == nil then vis = Renderable.defaults.visible end
    return {
        type    = d.type    or Renderable.defaults.type,
        radius  = d.radius  or Renderable.defaults.radius,
        width   = d.width   or Renderable.defaults.width,
        height  = d.height  or Renderable.defaults.height,
        color   = d.color   or Renderable.defaults.color,
        visible = vis,
    }
end

return Renderable
