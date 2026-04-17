-- Player Render System
-- PlayerTag + Transform + Renderable을 가진 엔티티 전용 렌더링
-- 기본 원 + 외곽선 + 이동 방향 표시

local System = require("01_core.system")

local PlayerRenderSystem = System.new("PlayerRender", {"PlayerTag", "Transform", "Renderable"},
    function(ecs, dt, entities)
        local lg = love.graphics
        local prevLineWidth = lg.getLineWidth()
        local time = love.timer.getTime()

        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local renderable = ecs:getComponent(entityId, "Renderable")
            local velocity   = ecs:getComponent(entityId, "Velocity")
            local health     = ecs:getComponent(entityId, "Health")

            if not renderable.visible then
                goto continue
            end

            -- iFrame blink: skip every other 0.1s frame when invincible
            if health and health.iTimer > 0 then
                if math.floor(time * 10) % 2 == 0 then
                    goto continue
                end
            end

            local x, y = transform.x, transform.y
            local r = renderable.radius
            local c = renderable.color

            -- 본체
            setColor(c[1] * 255, c[2] * 255, c[3] * 255, c[4] * 255)
            lg.circle("fill", x, y, r)

            -- 외곽선 (더 진한 사이안) — 월드 좌표 기준 선 두께
            setColor(0, 178, 178, 255)
            lg.setLineWidth(r * 0.2)  -- 반지름의 20% (~2px at 96ppu)
            lg.circle("line", x, y, r * 1.2)

            -- 이동 방향 표시
            if velocity then
                local vx, vy = velocity.vx, velocity.vy
                local mag = math.sqrt(vx * vx + vy * vy)
                if mag > 0.01 then
                    local dirX = vx / mag
                    local dirY = vy / mag
                    setColor(255, 255, 255, 204)
                    lg.setLineWidth(r * 0.3)  -- 반지름의 30% (~3px at 96ppu)
                    lg.line(x, y, x + dirX * r * 2, y + dirY * r * 2)
                end
            end

            ::continue::
        end

        lg.setLineWidth(prevLineWidth)
        resetColor()
    end
)

return PlayerRenderSystem
