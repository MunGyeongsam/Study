-- ============================================================================
-- Basic Shape Renderers — 기본 적 도형 6종
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
-- ◆ 의존 관계
--   love.graphics만 사용. renderSystem.lua가 require 한다.
-- ============================================================================

local lg = love.graphics
local cos = math.cos
local sin = math.sin
local pi2 = math.pi * 2
local halfPi = math.pi / 2

-- Pre-built hexagon vertices (unit radius)
local hexVerts = {}
for i = 0, 5 do
    local a = (i / 6) * pi2 - halfPi
    hexVerts[i * 2 + 1] = cos(a)
    hexVerts[i * 2 + 2] = sin(a)
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

return M
