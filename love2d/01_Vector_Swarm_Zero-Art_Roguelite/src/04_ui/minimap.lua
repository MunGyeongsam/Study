-- Minimap
-- 화면 우상단 월드 미니맵 오버레이
-- 플레이어(흰), 적(빨강), 보스(큰 빨강), 카메라 뷰포트(테두리)
-- 크기: 화면 너비의 20%, 월드 비율(2:3) 유지

local world         = require("01_core.world")
local mobileLayout  = require("04_ui.mobileLayout")

local _floor = math.floor
local _min   = math.min
local _max   = math.max

local minimap = {}

-- ===== Layout (해상도 종속, updateLayout에서 계산) =====
local MARGIN    = 6
local WIDTH_RATIO = 0.18     -- 화면 너비의 18%
local WORLD_ASPECT = 20 / 30 -- 2:3 (w:h)

local layout = {
    x = 0, y = 0,
    w = 0, h = 0,
    scaleX = 0, scaleY = 0,
}

-- World bounds (cached)
local wLeft, wBottom, wRight, wTop = -10, -15, 10, 15
local wWidth, wHeight = 20, 30

-- ===== Public API =====

function minimap.init()
    wLeft, wBottom, wRight, wTop = world.getBounds()
    wWidth  = wRight - wLeft
    wHeight = wTop - wBottom
    minimap.updateLayout()
end

function minimap.updateLayout()
    local screen = mobileLayout.getLayout()
    local sw, sh = screen.screenWidth, screen.screenHeight

    layout.w = _floor(sw * WIDTH_RATIO)
    layout.h = _floor(layout.w / WORLD_ASPECT)
    layout.x = sw - layout.w - MARGIN
    layout.y = screen.topAreaHeight + MARGIN

    layout.scaleX = layout.w / wWidth
    layout.scaleY = layout.h / wHeight
end

function minimap.draw(ecs, playerModule, cam)
    if not ecs then return end

    local lg = love.graphics

    -- Background
    lg.setColor(0, 0, 0, 0.45)
    lg.rectangle("fill", layout.x, layout.y, layout.w, layout.h)

    -- Border (neon cyan, thin)
    lg.setColor(0.2, 0.6, 0.8, 0.5)
    lg.setLineWidth(1)
    lg.rectangle("line", layout.x, layout.y, layout.w, layout.h)

    -- Scissor: clip dots inside minimap
    lg.setScissor(layout.x, layout.y, layout.w, layout.h)

    -- Camera viewport rectangle
    if cam then
        local orthoSize = cam:getOrthographicSize()
        local camX, camY = cam:pos()
        local viewH = orthoSize * 2
        local viewW = viewH * (love.graphics.getWidth() / love.graphics.getHeight())

        local vx1, vy1 = minimap._worldToMinimap(camX - viewW / 2, camY + viewH / 2)
        local vx2, vy2 = minimap._worldToMinimap(camX + viewW / 2, camY - viewH / 2)

        lg.setColor(0.3, 0.7, 1.0, 0.25)
        lg.rectangle("fill", vx1, vy1, vx2 - vx1, vy2 - vy1)
        lg.setColor(0.3, 0.7, 1.0, 0.5)
        lg.setLineWidth(1)
        lg.rectangle("line", vx1, vy1, vx2 - vx1, vy2 - vy1)
    end

    -- Enemies (red dots)
    local enemies = ecs:queryEntities({"Transform", "EnemyAI"})
    lg.setColor(1, 0.25, 0.25, 0.8)
    for _, eid in ipairs(enemies) do
        local t = ecs:getComponent(eid, "Transform")
        local mx, my = minimap._worldToMinimap(t.x, t.y)
        lg.circle("fill", mx, my, 2)
    end

    -- Bosses (larger red)
    local bosses = ecs:queryEntities({"Transform", "BossTag"})
    lg.setColor(1, 0.1, 0.1, 1)
    for _, eid in ipairs(bosses) do
        local t = ecs:getComponent(eid, "Transform")
        local mx, my = minimap._worldToMinimap(t.x, t.y)
        lg.circle("fill", mx, my, 4)
    end

    -- XP orbs (small green, optional)
    local orbs = ecs:queryEntities({"Transform", "XpOrb"})
    if #orbs > 0 then
        lg.setColor(0.2, 1, 0.4, 0.4)
        for _, eid in ipairs(orbs) do
            local t = ecs:getComponent(eid, "Transform")
            local mx, my = minimap._worldToMinimap(t.x, t.y)
            lg.points(mx, my)
        end
    end

    -- Player (white, larger)
    if playerModule then
        local px, py = playerModule.getPosition()
        if px and py then
            local mx, my = minimap._worldToMinimap(px, py)
            lg.setColor(1, 1, 1, 1)
            lg.circle("fill", mx, my, 3)
        end
    end

    lg.setScissor()
    resetColor()
end

-- ===== Internal =====

function minimap._worldToMinimap(wx, wy)
    local mx = layout.x + (wx - wLeft) * layout.scaleX
    local my = layout.y + (wTop - wy) * layout.scaleY  -- Y flip
    return mx, my
end

return minimap
