-- Render System
-- Transform + Renderable을 가진 엔티티를 그린다
-- draw()에서만 실행되며, update()에서는 스킵된다.

local System = require("01_core.system")

local lg = love.graphics
local cos = math.cos
local sin = math.sin
local pi2 = math.pi * 2

-- Pre-built hexagon vertices (unit radius)
local hexVerts = {}
for i = 0, 5 do
    local a = (i / 6) * pi2 - math.pi / 2  -- start from top
    hexVerts[i * 2 + 1] = cos(a)
    hexVerts[i * 2 + 2] = sin(a)
end

local RenderSystem = System.new("Render", {"Transform", "Renderable"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local renderable = ecs:getComponent(entityId, "Renderable")

            if renderable.visible then
                setColor(
                    renderable.color[1] * 255,
                    renderable.color[2] * 255,
                    renderable.color[3] * 255,
                    renderable.color[4] * 255
                )

                local x, y = transform.x, transform.y
                local r = renderable.radius

                if renderable.type == "circle" then
                    lg.circle("fill", x, y, r)

                elseif renderable.type == "rectangle" then
                    local w = renderable.width or 0.2
                    local h = renderable.height or 0.2
                    lg.rectangle("fill", x - w/2, y - h/2, w, h)

                elseif renderable.type == "diamond" then
                    -- Rotating diamond (square rotated 45°)
                    local rot = renderable.rotation or 0
                    lg.push()
                    lg.translate(x, y)
                    lg.rotate(rot)
                    lg.polygon("fill", -r, 0, 0, r, r, 0, 0, -r)
                    lg.pop()

                elseif renderable.type == "arrow" then
                    -- Arrow pointing in movement direction
                    local rot = renderable.rotation or 0
                    lg.push()
                    lg.translate(x, y)
                    lg.rotate(rot)
                    -- Arrow head (triangle)
                    lg.polygon("fill", r*1.2, 0, -r*0.6, r*0.6, -r*0.6, -r*0.6)
                    -- Tail line
                    lg.setLineWidth(r * 0.4)
                    lg.line(-r*1.2, 0, -r*0.4, 0)
                    lg.setLineWidth(1)
                    lg.pop()

                elseif renderable.type == "spiral_ring" then
                    -- Double concentric rings
                    lg.setLineWidth(r * 0.25)
                    lg.circle("line", x, y, r)
                    lg.circle("line", x, y, r * 0.5)
                    lg.setLineWidth(1)

                elseif renderable.type == "hexagon" then
                    -- Scaled hexagon
                    lg.push()
                    lg.translate(x, y)
                    lg.scale(r, r)
                    lg.polygon("fill",
                        hexVerts[1], hexVerts[2],
                        hexVerts[3], hexVerts[4],
                        hexVerts[5], hexVerts[6],
                        hexVerts[7], hexVerts[8],
                        hexVerts[9], hexVerts[10],
                        hexVerts[11], hexVerts[12])
                    lg.pop()
                end
            end
        end

        resetColor()
    end
)

return RenderSystem
