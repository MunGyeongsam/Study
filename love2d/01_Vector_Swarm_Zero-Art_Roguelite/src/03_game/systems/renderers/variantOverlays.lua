-- Variant Overlay Renderers
-- 변형 4종 비주얼: swift, armored, splitter, shielded
-- fn(x, y, r, renderable, ecs, entityId)

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
