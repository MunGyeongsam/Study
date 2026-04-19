-- Player Render System
-- PlayerTag + Transform + Renderable을 가진 엔티티 전용 렌더링
-- 기본 원 + 외곽선 + 이동 방향 표시 + 대쉬 잔상

local System = require("01_core.system")

local lg = love.graphics
local sqrt = math.sqrt
local sin = math.sin
local pi = math.pi
local floor = math.floor

-- ─── 헬퍼: 대쉬 고스트 트레일 ─────────────────────────────────
local function drawGhosts(ghosts)
    for gi = 1, #ghosts do
        local g = ghosts[gi]
        local fade = g.timer / g.maxTime
        setColor(0, 230, 230, fade * 150)
        lg.circle("fill", g.x, g.y, g.radius * (0.6 + 0.4 * fade))
        setColor(0, 255, 255, fade * 80)
        lg.setLineWidth(g.radius * 0.15)
        lg.circle("line", g.x, g.y, g.radius * (0.8 + 0.6 * fade))
    end
end

-- ─── 헬퍼: 이동 방향 표시 ──────────────────────────────────────
local function drawDirection(x, y, r, velocity)
    local vx, vy = velocity.vx, velocity.vy
    local mag = sqrt(vx * vx + vy * vy)
    if mag > 0.01 then
        setColor(255, 255, 255, 204)
        lg.setLineWidth(r * 0.3)
        lg.line(x, y, x + vx / mag * r * 2, y + vy / mag * r * 2)
    end
end

-- ─── 헬퍼: 포커스 모드 아우라 ──────────────────────────────────
local function drawFocusAura(ecs, entityId, x, y, r, time)
    local focus = ecs:getComponent(entityId, "Focus")
    if not focus.active then return end
    local pulse = 0.6 + 0.4 * sin(time * 6)
    setColor(180, 100, 255, pulse * 180)
    lg.setLineWidth(r * 0.15)
    lg.circle("line", x, y, r * 2.0)
    local collider = ecs:getComponent(entityId, "Collider")
    if collider then
        setColor(255, 200, 255, 150)
        lg.setLineWidth(r * 0.1)
        lg.circle("line", x, y, collider.radius)
    end
end

-- ─── 헬퍼: 대쉬 쿨타임 링 ─────────────────────────────────────
local function drawDashCooldown(dash, x, y, r)
    if dash.cooldownTimer <= 0 then return end
    local ratio = dash.cooldownTimer / dash.cooldown
    setColor(255, 80, 80, ratio * 200)
    lg.setLineWidth(r * 0.15)
    lg.arc("line", "open", x, y, r * 1.5, -pi/2, -pi/2 + (1 - ratio) * pi * 2)
end

-- ─── 메인 시스템 ───────────────────────────────────────────────
local PlayerRenderSystem = System.new("PlayerRender", {"PlayerTag", "Transform", "Renderable"},
    function(ecs, dt, entities)
        local prevLineWidth = lg.getLineWidth()
        local time = love.timer.getTime()

        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local renderable = ecs:getComponent(entityId, "Renderable")
            local dash = ecs:hasComponent(entityId, "Dash") and ecs:getComponent(entityId, "Dash") or nil

            -- 대쉬 잔상은 항상 그림 (iFrame/visible 무관)
            if dash and #dash.ghosts > 0 then
                drawGhosts(dash.ghosts)
            end

            -- 본체 가시성 판정
            if not renderable.visible then
                -- skip body rendering
            else
                local health = ecs:getComponent(entityId, "Health")
                local isBlinking = health and health.iTimer > 0 and floor(time * 10) % 2 == 0

                if not isBlinking then
                    local x, y = transform.x, transform.y
                    local r = renderable.radius
                    local c = renderable.color

                    -- 본체 + 외곽선
                    setColor(c[1] * 255, c[2] * 255, c[3] * 255, c[4] * 255)
                    lg.circle("fill", x, y, r)
                    setColor(0, 178, 178, 255)
                    lg.setLineWidth(r * 0.2)
                    lg.circle("line", x, y, r * 1.2)

                    -- 이동 방향
                    local velocity = ecs:getComponent(entityId, "Velocity")
                    if velocity then drawDirection(x, y, r, velocity) end

                    -- 포커스 아우라
                    if ecs:hasComponent(entityId, "Focus") then
                        drawFocusAura(ecs, entityId, x, y, r, time)
                    end

                    -- 대쉬 쿨타임 링
                    if dash then drawDashCooldown(dash, x, y, r) end
                end
            end
        end

        lg.setLineWidth(prevLineWidth)
        resetColor()
    end
)

return PlayerRenderSystem
