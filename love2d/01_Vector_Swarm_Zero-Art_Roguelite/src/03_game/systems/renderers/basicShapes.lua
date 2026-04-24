-- ============================================================================
-- Basic Shape Renderers — 기본 적 도형 12종
-- ============================================================================
--
-- ◆ 이 파일의 역할
--   ECS Renderable.type에 대응하는 기본 도형 렌더 함수.
--   renderSystem.lua의 dispatch 테이블에 자동 등록된다.
--
-- ◆ 새 도형 추가 방법
--   1. M.새이름 = function(x, y, r, renderable, transform) ... end 추가
--   2. 함수 이름이 곧 Renderable.type 값 (dispatch key)
--   3. entityFactory.lua에서 해당 적의 renderType을 새 이름으로 설정
--   4. renderSystem.lua 수정 불필요 (자동 등록)
--
-- ◆ 함수 시그니처
--   fn(x, y, r, renderable, transform)
--   x,y = 월드 좌표, r = renderable.radius
--   setColor()는 호출자(renderSystem)가 이미 적용한 상태
--
-- ◆ DNA 레이어 렌더 시그니처 (renderSystem에서 직접 호출)
--   fn_draw(x, y, r, mode)
--   mode = "fill" | "line"
--   shapes_draw 테이블에 등록. dispatch 테이블과 별도.
--
-- ◆ 의존 관계
--   love.graphics만 사용. renderSystem.lua가 require 한다.
-- ============================================================================

local lg = love.graphics
local cos = math.cos
local sin = math.sin
local pi = math.pi
local pi2 = math.pi * 2
local halfPi = math.pi / 2

-- Pre-built hexagon vertices (unit radius)
local hexVerts = {}
for i = 0, 5 do
    local a = (i / 6) * pi2 - halfPi
    hexVerts[i * 2 + 1] = cos(a)
    hexVerts[i * 2 + 2] = sin(a)
end

-- Pre-built triangle vertices (unit radius, point up)
local triVerts = {}
for i = 0, 2 do
    local a = (i / 3) * pi2 - halfPi
    triVerts[i * 2 + 1] = cos(a)
    triVerts[i * 2 + 2] = sin(a)
end

-- Pre-built star vertices (5-point, unit radius)
local starVerts = {}
local starOuter = 1.0
local starInner = 0.4
for i = 0, 9 do
    local a = (i / 10) * pi2 - halfPi
    local r = (i % 2 == 0) and starOuter or starInner
    starVerts[i * 2 + 1] = r * cos(a)
    starVerts[i * 2 + 2] = r * sin(a)
end

-- Pre-built gear vertices (8-tooth, unit radius)
local gearVerts = {}
local gearTeeth = 8
local gearOuter = 1.0
local gearInner = 0.7
for i = 0, gearTeeth * 4 - 1 do
    local a = (i / (gearTeeth * 4)) * pi2 - halfPi
    local step = i % 4
    local r
    if step == 0 or step == 1 then
        r = gearOuter
    else
        r = gearInner
    end
    gearVerts[i * 2 + 1] = r * cos(a)
    gearVerts[i * 2 + 2] = r * sin(a)
end

-- Pre-built reuleaux triangle vertices (constant-width curve, 3 arcs)
-- Unit radius normalized: circumscribed circle radius ≈ 1.0
local reuleauxVerts = {}
do
    local sqrt3 = math.sqrt(3)
    local pts = {}
    for i = 0, 2 do
        local a = i * pi2 / 3 - halfPi
        pts[i + 1] = {cos(a), sin(a)}
    end
    local arcSteps = 8  -- per arc, 24 total
    local arcR = sqrt3
    local order = {{1, 2, 3}, {2, 3, 1}, {3, 1, 2}}
    for _, o in ipairs(order) do
        local center = pts[o[1]]
        local pFrom  = pts[o[2]]
        local pTo    = pts[o[3]]
        local aStart = math.atan2(pFrom[2] - center[2], pFrom[1] - center[1])
        local aEnd   = math.atan2(pTo[2] - center[2], pTo[1] - center[1])
        if aEnd < aStart then aEnd = aEnd + pi2 end
        for s = 0, arcSteps - 1 do
            local t = aStart + (aEnd - aStart) * s / arcSteps
            reuleauxVerts[#reuleauxVerts + 1] = center[1] + arcR * cos(t)
            reuleauxVerts[#reuleauxVerts + 1] = center[2] + arcR * sin(t)
        end
    end
    -- Normalize to unit radius (max distance from center = 1.0)
    local maxR = 0
    for i = 1, #reuleauxVerts, 2 do
        local d = math.sqrt(reuleauxVerts[i]^2 + reuleauxVerts[i+1]^2)
        if d > maxR then maxR = d end
    end
    if maxR > 0 then
        for i = 1, #reuleauxVerts do
            reuleauxVerts[i] = reuleauxVerts[i] / maxR
        end
    end
end
local reuleauxN = #reuleauxVerts / 2  -- 24

-- Pre-built astroid vertices (hypocycloid k=4: x=cos³θ, y=sin³θ)
local astroidVerts = {}
do
    local steps = 32
    for i = 0, steps - 1 do
        local t = i / steps * pi2
        astroidVerts[i * 2 + 1] = cos(t) ^ 3
        astroidVerts[i * 2 + 2] = sin(t) ^ 3
    end
end
local astroidN = 32

-- Pre-built superellipse vertices (Lamé curve n=4, "squircle")
-- |x|^n + |y|^n = 1 → parametric: x = sgn(cos t)|cos t|^(2/n), y = sgn(sin t)|sin t|^(2/n)
local superellipseVerts = {}
do
    local steps = 32
    local exp = 0.5  -- 2/n where n=4
    for i = 0, steps - 1 do
        local t = i / steps * pi2
        local c, s = cos(t), sin(t)
        local sc = c >= 0 and 1 or -1
        local ss = s >= 0 and 1 or -1
        superellipseVerts[i * 2 + 1] = sc * math.abs(c) ^ exp
        superellipseVerts[i * 2 + 2] = ss * math.abs(s) ^ exp
    end
end
local superellipseN = 32

-- Vertex counts for concave shapes (star-convex → triangle fan from center)
local starN  = 10
local gearN  = gearTeeth * 4  -- 32

--- Fill a star-convex polygon using triangle fan from origin (0,0)
--- Must be called inside lg.push/translate/scale context
local function _fillFanPoly(verts, nVerts)
    for i = 0, nVerts - 1 do
        local j = (i + 1) % nVerts
        lg.polygon("fill", 0, 0,
            verts[i*2+1], verts[i*2+2],
            verts[j*2+1], verts[j*2+2])
    end
end

local M = {}

function M.circle(x, y, r, renderable, transform)
    lg.circle("fill", x, y, r)
end

function M.rectangle(x, y, r, renderable, transform)
    local w = renderable.width or 0.2
    local h = renderable.height or 0.2
    lg.rectangle("fill", x - w/2, y - h/2, w, h)
end

function M.diamond(x, y, r, renderable, transform)
    local rot = renderable.rotation or 0
    lg.push()
    lg.translate(x, y)
    lg.rotate(rot)
    lg.polygon("fill", -r, 0, 0, r, r, 0, 0, -r)
    lg.pop()
end

function M.arrow(x, y, r, renderable, transform)
    local rot = renderable.rotation or 0
    lg.push()
    lg.translate(x, y)
    lg.rotate(rot)
    lg.polygon("fill", r*1.2, 0, -r*0.6, r*0.6, -r*0.6, -r*0.6)
    lg.setLineWidth(r * 0.4)
    lg.line(-r*1.2, 0, -r*0.4, 0)
    lg.setLineWidth(1)
    lg.pop()
end

function M.spiral_ring(x, y, r, renderable, transform)
    lg.setLineWidth(r * 0.25)
    lg.circle("line", x, y, r)
    lg.circle("line", x, y, r * 0.5)
    lg.setLineWidth(1)
end

function M.hexagon(x, y, r, renderable, transform)
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

-- ─── 신규 도형 6종 (DNA Body 레이어용) ──────────────────────────

function M.triangle(x, y, r, renderable, transform)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    lg.polygon("fill", triVerts[1], triVerts[2],
        triVerts[3], triVerts[4], triVerts[5], triVerts[6])
    lg.pop()
end

function M.star(x, y, r, renderable, transform)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    _fillFanPoly(starVerts, starN)
    lg.pop()
end

function M.gear(x, y, r, renderable, transform)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    _fillFanPoly(gearVerts, gearN)
    lg.pop()
end

function M.reuleaux(x, y, r, renderable, transform)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    lg.polygon("fill", unpack(reuleauxVerts))
    lg.pop()
end

function M.astroid(x, y, r, renderable, transform)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    _fillFanPoly(astroidVerts, astroidN)
    lg.pop()
end

function M.superellipse(x, y, r, renderable, transform)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    lg.polygon("fill", unpack(superellipseVerts))
    lg.pop()
end

-- ─── DNA 레이어 렌더용 함수 테이블 ──────────────────────────────
-- shapes_draw[shape](x, y, r, mode) — mode = "fill" | "line"
-- 기존 dispatch 함수와 별도: mode 파라미터로 fill/line 제어

M.shapes_draw = {}

M.shapes_draw.circle = function(x, y, r, mode)
    if mode == "line" then
        lg.setLineWidth(r * 0.2)
        lg.circle("line", x, y, r)
        lg.setLineWidth(1)
    else
        lg.circle("fill", x, y, r)
    end
end

M.shapes_draw.diamond = function(x, y, r, mode)
    if mode == "line" then
        lg.setLineWidth(r * 0.2)
        lg.polygon("line", x-r, y, x, y+r, x+r, y, x, y-r)
        lg.setLineWidth(1)
    else
        lg.polygon("fill", x-r, y, x, y+r, x+r, y, x, y-r)
    end
end

M.shapes_draw.arrow = function(x, y, r, mode)
    if mode == "line" then
        lg.setLineWidth(r * 0.2)
        lg.polygon("line", x+r*1.2, y, x-r*0.6, y+r*0.6, x-r*0.6, y-r*0.6)
        lg.line(x-r*1.2, y, x-r*0.4, y)
        lg.setLineWidth(1)
    else
        lg.polygon("fill", x+r*1.2, y, x-r*0.6, y+r*0.6, x-r*0.6, y-r*0.6)
        lg.setLineWidth(r * 0.4)
        lg.line(x-r*1.2, y, x-r*0.4, y)
        lg.setLineWidth(1)
    end
end

M.shapes_draw.spiral_ring = function(x, y, r, mode)
    lg.setLineWidth(r * 0.2)
    lg.circle("line", x, y, r)
    lg.circle("line", x, y, r * 0.5)
    lg.setLineWidth(1)
end

--- Convex polygon draw (hexagon, triangle, tear)
local function _drawPolygonVerts(x, y, r, verts, mode)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    if mode == "line" then
        lg.setLineWidth(1 / r * 0.15)
        lg.polygon("line", unpack(verts))
        lg.setLineWidth(1)
    else
        lg.polygon("fill", unpack(verts))
    end
    lg.pop()
end

--- Concave polygon draw (star, cross, gear) — fan fill from center
local function _drawConcaveVerts(x, y, r, verts, nVerts, mode)
    lg.push()
    lg.translate(x, y)
    lg.scale(r, r)
    if mode == "line" then
        lg.setLineWidth(1 / r * 0.15)
        lg.polygon("line", unpack(verts))
        lg.setLineWidth(1)
    else
        _fillFanPoly(verts, nVerts)
    end
    lg.pop()
end

M.shapes_draw.hexagon = function(x, y, r, mode)
    _drawPolygonVerts(x, y, r, hexVerts, mode)
end

M.shapes_draw.rectangle = function(x, y, r, mode)
    local w, h = r * 1.4, r * 1.0
    if mode == "line" then
        lg.setLineWidth(r * 0.2)
        lg.rectangle("line", x - w/2, y - h/2, w, h)
        lg.setLineWidth(1)
    else
        lg.rectangle("fill", x - w/2, y - h/2, w, h)
    end
end

M.shapes_draw.triangle = function(x, y, r, mode)
    _drawPolygonVerts(x, y, r, triVerts, mode)
end

M.shapes_draw.star = function(x, y, r, mode)
    _drawConcaveVerts(x, y, r, starVerts, starN, mode)
end

M.shapes_draw.gear = function(x, y, r, mode)
    _drawConcaveVerts(x, y, r, gearVerts, gearN, mode)
end

M.shapes_draw.reuleaux = function(x, y, r, mode)
    _drawPolygonVerts(x, y, r, reuleauxVerts, mode)
end

M.shapes_draw.astroid = function(x, y, r, mode)
    _drawConcaveVerts(x, y, r, astroidVerts, astroidN, mode)
end

M.shapes_draw.superellipse = function(x, y, r, mode)
    _drawPolygonVerts(x, y, r, superellipseVerts, mode)
end

return M
