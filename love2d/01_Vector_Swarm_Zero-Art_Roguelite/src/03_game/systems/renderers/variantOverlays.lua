-- ============================================================================
-- Variant Overlay Renderers — 변형 적 오버레이 4종
-- ============================================================================
--
-- ◆ 이 파일의 역할
--   기본 도형 위에 추가로 그려지는 변형(variant) 비주얼 효과.
--   renderSystem.lua에서 기본 도형 렌더 후 호출된다.
--
-- ◆ 새 변형 오버레이 추가 방법
--   1. M.변형이름 = function(x, y, r, renderable, ecs, entityId) ... end 추가
--   2. 함수 이름이 곧 Renderable.variant 값 (dispatch key)
--   3. stageData.lua → VARIANT_TIERS + GUARANTEED_VARIANTS에 등록
--   4. entityFactory.lua에서 변형 스탯 보너스 정의
--   5. renderSystem.lua 수정 불필요 (자동 등록)
--
-- ◆ 함수 시그니처
--   fn(x, y, r, renderable, ecs, entityId)
--   기본 도형과 달리 ecs와 entityId를 받음 (Velocity 조회 등에 필요)
--   setColor()는 호출자가 적용한 상태이나, 오버레이는 자체 색상 사용.
--
-- ◆ 의존 관계
--   love.graphics + ECS 조회(읽기 전용). renderSystem.lua가 require 한다.
-- ============================================================================

local lg = love.graphics
local sqrt = math.sqrt
local atan2 = math.atan2
local pi2 = math.pi * 2

local M = {}

-- Swift: 이동 방향 잔상 3개
function M.swift(x, y, r, renderable, ecs, entityId)
    local vel = ecs:getComponent(entityId, "Velocity")
    if not vel then return end
    local vx, vy = vel.vx or 0, vel.vy or 0
    local spd = vx * vx + vy * vy
    if spd <= 0.01 then return end
    spd = sqrt(spd)
    local nx, ny = vx / spd, vy / spd
    local c = renderable.color
    for g = 1, 3 do
        local dist = g * r * 1.2
        local gx = x - nx * dist
        local gy = y - ny * dist
        local ga = (4 - g) * 0.15
        setColor(c[1] * 255, c[2] * 255, c[3] * 255, ga * 255)
        lg.circle("fill", gx, gy, r * (1 - g * 0.15))
    end
end

-- Armored: 두꺼운 외곽 링
function M.armored(x, y, r, renderable, ecs, entityId)
    local c = renderable.color
    setColor(c[1] * 200, c[2] * 200, c[3] * 200, 220)
    lg.setLineWidth(r * 0.35)
    lg.circle("line", x, y, r * 1.1)
    lg.setLineWidth(1)
end

-- Splitter: 대시 외곽 원
function M.splitter(x, y, r, renderable, ecs, entityId)
    local c = renderable.color
    setColor(c[1] * 255, c[2] * 255, c[3] * 255, 180)
    lg.setLineWidth(r * 0.15)
    local segments = 8
    local rr = r * 1.15
    for s = 0, segments - 1, 2 do
        local a1 = (s / segments) * pi2
        local a2 = ((s + 1) / segments) * pi2
        lg.arc("line", "open", x, y, rr, a1, a2)
    end
    lg.setLineWidth(1)
end

-- Shielded: 전방 90° 보호 아크
function M.shielded(x, y, r, renderable, ecs, entityId)
    local vel = ecs:getComponent(entityId, "Velocity")
    local fvx = vel and vel.vx or 0
    local fvy = vel and vel.vy or 0
    if fvx * fvx + fvy * fvy < 0.001 then
        fvx, fvy = 0, -1
    end
    local facing = atan2(fvy, fvx)
    local arcHalf = 0.7854  -- pi/4
    local c = renderable.color
    -- Outer glow
    setColor(c[1] * 200, c[2] * 200, c[3] * 200, 100)
    lg.setLineWidth(r * 0.4)
    lg.arc("line", "open", x, y, r * 1.25, facing - arcHalf, facing + arcHalf)
    -- Inner bright
    setColor(c[1] * 255, c[2] * 255, c[3] * 255, 220)
    lg.setLineWidth(r * 0.2)
    lg.arc("line", "open", x, y, r * 1.2, facing - arcHalf, facing + arcHalf)
    lg.setLineWidth(1)
end

return M
