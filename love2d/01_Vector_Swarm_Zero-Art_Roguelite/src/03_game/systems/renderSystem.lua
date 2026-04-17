-- Render System
-- Transform + Renderable을 가진 엔티티를 그린다
-- draw()에서만 실행되며, update()에서는 스킵된다.

local System = require("01_core.system")

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

                if renderable.type == "circle" then
                    love.graphics.circle("fill", transform.x, transform.y, renderable.radius)
                elseif renderable.type == "rectangle" then
                    local w = renderable.width or 0.2
                    local h = renderable.height or 0.2
                    love.graphics.rectangle("fill", transform.x - w/2, transform.y - h/2, w, h)
                end
            end
        end

        resetColor()
    end
)

return RenderSystem
