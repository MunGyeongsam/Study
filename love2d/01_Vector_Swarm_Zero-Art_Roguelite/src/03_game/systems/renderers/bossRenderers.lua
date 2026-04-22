-- Boss Renderers
-- 보스 전용 렌더 5종: NULL, STACK, HEAP, RECURSION, OVERFLOW
-- fn(x, y, r, renderable, transform) — r은 원본, 내부에서 scale 적용

local lg = love.graphics
local cos = math.cos
local sin = math.sin
local pi2 = math.pi * 2
local halfPi = math.pi / 2
local floor = math.floor
local abs = math.abs
local getTime = love.timer.getTime

-- Pre-built hexagon vertices (unit radius, boss_overflow에서 사용)
local hexVerts = {}
for i = 0, 5 do
    local a = (i / 6) * pi2 - halfPi
    hexVerts[i * 2 + 1] = cos(a)
    hexVerts[i * 2 + 2] = sin(a)
end

local M = {}

-- NULL: 역삼각형 + 점멸 + 대시 외곽
function M.boss_null(x, y, r, renderable, transform)
    r = r * (transform.scale or 1)
    local t = getTime()
    local flicker = floor(t * 6) % 3
    local alpha = renderable.color[4] * (flicker == 0 and 0.4 or 1.0)
    setColor(
        renderable.color[1] * 255,
        renderable.color[2] * 255,
        renderable.color[3] * 255,
        alpha * 255)
    lg.push()
    lg.translate(x, y)
    lg.polygon("fill", 0, r, -r * 0.87, -r * 0.5, r * 0.87, -r * 0.5)
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
end

-- STACK: 3단 직사각형 + 수직 흔들림 + 외곽선
function M.boss_stack(x, y, r, renderable, transform)
    r = r * (transform.scale or 1)
    local t = getTime()
    local wobble = sin(t * 3) * r * 0.12
    local layerH = r * 0.5
    local layerW = r * 1.4
    lg.push()
    lg.translate(x, y)
    for i = -1, 1 do
        local ly = i * (layerH + r * 0.1) + wobble * i
        local layerAlpha = 1.0 - abs(i) * 0.15
        setColor(
            renderable.color[1] * 255,
            renderable.color[2] * 255,
            renderable.color[3] * 255,
            renderable.color[4] * layerAlpha * 255)
        lg.rectangle("fill", -layerW / 2, ly - layerH / 2, layerW, layerH, r * 0.1, r * 0.1)
    end
    setColor(
        renderable.color[1] * 255,
        renderable.color[2] * 255,
        renderable.color[3] * 255,
        renderable.color[4] * 255)
    lg.setLineWidth(r * 0.08)
    lg.rectangle("line", -layerW / 2 - r * 0.1, -r * 1.0, layerW + r * 0.2, r * 2.0, r * 0.12, r * 0.12)
    lg.setLineWidth(1)
    lg.pop()
end

-- HEAP: 다이아몬드 + 내접 트리 구조
function M.boss_heap(x, y, r, renderable, transform)
    r = r * (transform.scale or 1)
    lg.push()
    lg.translate(x, y)
    lg.polygon("fill", 0, -r, r, 0, 0, r, -r, 0)
    setColor(0, 0, 0, 200)
    lg.setLineWidth(r * 0.1)
    lg.line(0, -r * 0.5, -r * 0.4, r * 0.15)
    lg.line(0, -r * 0.5, r * 0.4, r * 0.15)
    lg.line(-r * 0.4, r * 0.15, -r * 0.6, r * 0.55)
    lg.line(-r * 0.4, r * 0.15, -r * 0.2, r * 0.55)
    lg.line(r * 0.4, r * 0.15, r * 0.2, r * 0.55)
    lg.line(r * 0.4, r * 0.15, r * 0.6, r * 0.55)
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
end

-- RECURSION: 시에르핀스키 프랙탈 삼각형 (회전)
function M.boss_recursion(x, y, r, renderable, transform)
    r = r * (transform.scale or 1)
    local t = getTime()
    local rot = t * 0.5
    lg.push()
    lg.translate(x, y)
    lg.rotate(rot)
    local function tri(cx, cy, sz)
        local h = sz * 0.866
        lg.polygon("fill",
            cx, cy - h * 0.67,
            cx - sz * 0.5, cy + h * 0.33,
            cx + sz * 0.5, cy + h * 0.33)
    end
    tri(0, 0, r * 2)
    setColor(0, 0, 0, 180)
    local sz1 = r
    local h1 = sz1 * 0.866
    tri(0, 0, sz1)
    setColor(
        renderable.color[1] * 255,
        renderable.color[2] * 255,
        renderable.color[3] * 255,
        renderable.color[4] * 255)
    tri(0, -h1 * 0.33, sz1)
    tri(-sz1 * 0.5, h1 * 0.33, sz1)
    tri(sz1 * 0.5, h1 * 0.33, sz1)
    lg.setLineWidth(1)
    lg.pop()
end

-- OVERFLOW: 육각형 코어 + 이전 보스 고스트 순환
function M.boss_overflow(x, y, r, renderable, transform)
    r = r * (transform.scale or 1)
    local t = getTime()
    lg.push()
    lg.translate(x, y)
    -- Core hexagon
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
    local gr = r * 1.2
    local offset = sin(t * 5) * r * 0.08
    if phase == 0 then
        lg.polygon("line", offset, gr, -gr * 0.87 + offset, -gr * 0.5, gr * 0.87 + offset, -gr * 0.5)
    elseif phase == 1 then
        lg.rectangle("line", -gr * 0.7 + offset, -gr * 0.7, gr * 1.4, gr * 1.4, r * 0.1, r * 0.1)
    elseif phase == 2 then
        lg.polygon("line", offset, -gr, gr + offset, 0, offset, gr, -gr + offset, 0)
    else
        lg.polygon("line", offset, -gr, -gr * 0.87 + offset, gr * 0.5, gr * 0.87 + offset, gr * 0.5)
    end
    lg.setLineWidth(1)
    lg.pop()
end

return M
