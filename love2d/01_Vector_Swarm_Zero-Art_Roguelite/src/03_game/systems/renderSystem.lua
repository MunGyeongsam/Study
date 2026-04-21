-- Render System
-- Transform + Renderable을 가진 엔티티를 그린다
-- draw()에서만 실행되며, update()에서는 스킵된다.

local System = require("01_core.system")

local lg = love.graphics
local cos = math.cos
local sin = math.sin
local pi2 = math.pi * 2
local halfPi = math.pi / 2
local floor = math.floor
local getTime = love.timer.getTime

-- Pre-built hexagon vertices (unit radius)
local hexVerts = {}
for i = 0, 5 do
    local a = (i / 6) * pi2 - halfPi  -- start from top
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

                -- ===== BOSS RENDER TYPES =====

                elseif renderable.type == "boss_null" then
                    -- Inverted triangle + dashed outline flicker (null = unstable existence)
                    local t = getTime()
                    local flicker = floor(t * 6) % 3  -- 0,1,2 cycle
                    local alpha = renderable.color[4] * (flicker == 0 and 0.4 or 1.0)
                    setColor(
                        renderable.color[1] * 255,
                        renderable.color[2] * 255,
                        renderable.color[3] * 255,
                        alpha * 255)
                    -- Filled inverted triangle
                    lg.push()
                    lg.translate(x, y)
                    lg.polygon("fill", 0, r, -r * 0.87, -r * 0.5, r * 0.87, -r * 0.5)
                    -- Dashed outer ring (segments)
                    lg.setLineWidth(r * 0.08)
                    local segments = 8
                    for i = 0, segments - 1 do
                        if i % 2 == 0 then
                            local a1 = (i / segments) * pi2
                            local a2 = ((i + 0.7) / segments) * pi2
                            lg.arc("line", "open", 0, 0, r * 1.3, a1, a2)
                        end
                    end
                    lg.setLineWidth(1)
                    lg.pop()

                elseif renderable.type == "boss_stack" then
                    -- 3 stacked rectangles with vertical wobble (stack push/pop)
                    local t = getTime()
                    local wobble = sin(t * 3) * r * 0.12
                    local layerH = r * 0.5
                    local layerW = r * 1.4
                    lg.push()
                    lg.translate(x, y)
                    for i = -1, 1 do
                        local ly = i * (layerH + r * 0.1) + wobble * i
                        local layerAlpha = 1.0 - math.abs(i) * 0.15
                        setColor(
                            renderable.color[1] * 255,
                            renderable.color[2] * 255,
                            renderable.color[3] * 255,
                            renderable.color[4] * layerAlpha * 255)
                        lg.rectangle("fill", -layerW / 2, ly - layerH / 2, layerW, layerH, r * 0.1, r * 0.1)
                    end
                    -- Outline
                    setColor(
                        renderable.color[1] * 255,
                        renderable.color[2] * 255,
                        renderable.color[3] * 255,
                        renderable.color[4] * 255)
                    lg.setLineWidth(r * 0.08)
                    lg.rectangle("line", -layerW / 2 - r * 0.1, -r * 1.0, layerW + r * 0.2, r * 2.0, r * 0.12, r * 0.12)
                    lg.setLineWidth(1)
                    lg.pop()

                elseif renderable.type == "boss_heap" then
                    -- Diamond + inscribed tree triangle (heap tree node)
                    lg.push()
                    lg.translate(x, y)
                    -- Outer diamond (filled)
                    lg.polygon("fill", 0, -r, r, 0, 0, r, -r, 0)
                    -- Inner tree structure (lines)
                    setColor(0, 0, 0, 200)
                    lg.setLineWidth(r * 0.1)
                    -- Root to children
                    lg.line(0, -r * 0.5, -r * 0.4, r * 0.15)
                    lg.line(0, -r * 0.5, r * 0.4, r * 0.15)
                    -- Children to leaves
                    lg.line(-r * 0.4, r * 0.15, -r * 0.6, r * 0.55)
                    lg.line(-r * 0.4, r * 0.15, -r * 0.2, r * 0.55)
                    lg.line(r * 0.4, r * 0.15, r * 0.2, r * 0.55)
                    lg.line(r * 0.4, r * 0.15, r * 0.6, r * 0.55)
                    -- Nodes
                    setColor(
                        renderable.color[1] * 255,
                        renderable.color[2] * 255,
                        renderable.color[3] * 255,
                        renderable.color[4] * 255)
                    lg.circle("fill", 0, -r * 0.5, r * 0.15)
                    lg.circle("fill", -r * 0.4, r * 0.15, r * 0.12)
                    lg.circle("fill", r * 0.4, r * 0.15, r * 0.12)
                    lg.setLineWidth(1)
                    lg.pop()

                elseif renderable.type == "boss_recursion" then
                    -- Sierpinski-style fractal triangle (self-reference, rotating)
                    local t = getTime()
                    local rot = t * 0.5  -- slow rotation
                    lg.push()
                    lg.translate(x, y)
                    lg.rotate(rot)
                    -- Outer triangle
                    local function tri(cx, cy, sz)
                        local h = sz * 0.866  -- sqrt(3)/2
                        lg.polygon("fill",
                            cx, cy - h * 0.67,
                            cx - sz * 0.5, cy + h * 0.33,
                            cx + sz * 0.5, cy + h * 0.33)
                    end
                    -- Level 0: full triangle
                    tri(0, 0, r * 2)
                    -- Level 1: cut out center (draw 3 smaller in darker shade)
                    setColor(0, 0, 0, 180)
                    local sz1 = r
                    local h1 = sz1 * 0.866
                    tri(0, 0, sz1)  -- center cutout
                    -- Re-color the 3 sub-triangles
                    setColor(
                        renderable.color[1] * 255,
                        renderable.color[2] * 255,
                        renderable.color[3] * 255,
                        renderable.color[4] * 255)
                    tri(0, -h1 * 0.33, sz1)               -- top
                    tri(-sz1 * 0.5, h1 * 0.33, sz1)       -- bottom-left
                    tri(sz1 * 0.5, h1 * 0.33, sz1)        -- bottom-right
                    lg.setLineWidth(1)
                    lg.pop()

                elseif renderable.type == "boss_overflow" then
                    -- Hexagon core + glitching ghost shapes of previous bosses
                    local t = getTime()
                    lg.push()
                    lg.translate(x, y)
                    -- Core hexagon (filled)
                    lg.push()
                    lg.scale(r, r)
                    lg.polygon("fill",
                        hexVerts[1], hexVerts[2],
                        hexVerts[3], hexVerts[4],
                        hexVerts[5], hexVerts[6],
                        hexVerts[7], hexVerts[8],
                        hexVerts[9], hexVerts[10],
                        hexVerts[11], hexVerts[12])
                    lg.pop()
                    -- Ghost shapes: cycle through previous boss silhouettes
                    local ghostAlpha = 0.25 + 0.15 * sin(t * 4)
                    setColor(255, 255, 255, ghostAlpha * 255)
                    lg.setLineWidth(r * 0.06)
                    local phase = floor(t * 2) % 4
                    local gr = r * 1.2  -- ghost radius slightly larger
                    local offset = sin(t * 5) * r * 0.08  -- jitter
                    if phase == 0 then
                        -- NULL ghost: inverted triangle
                        lg.polygon("line", offset, gr, -gr * 0.87 + offset, -gr * 0.5, gr * 0.87 + offset, -gr * 0.5)
                    elseif phase == 1 then
                        -- STACK ghost: rectangle
                        lg.rectangle("line", -gr * 0.7 + offset, -gr * 0.7, gr * 1.4, gr * 1.4, r * 0.1, r * 0.1)
                    elseif phase == 2 then
                        -- HEAP ghost: diamond
                        lg.polygon("line", offset, -gr, gr + offset, 0, offset, gr, -gr + offset, 0)
                    else
                        -- RECURSION ghost: triangle
                        lg.polygon("line", offset, -gr, -gr * 0.87 + offset, gr * 0.5, gr * 0.87 + offset, gr * 0.5)
                    end
                    lg.setLineWidth(1)
                    lg.pop()
                end
            end
        end

        resetColor()
    end
)

return RenderSystem
