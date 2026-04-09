--[[

좌표계 설명
 - LOVE2D/Lua 기본: (0,0)은 좌상단, x축 오른쪽(+), y축 아래쪽(+)
     ┌──────────────→ x (오른쪽이 +)
     │
     │
     ↓
     y (아래가 +)
 - 왼손 좌표계(2D)
 - 각도(angle)는 +x축(오른쪽)을 기준으로 반시계 방향이 양수(라디안)

2D 벡터(Vector) 모듈

- 벡터 생성: local v = Vector.new(x, y)
- 벡터 복사: local v2 = v:clone()
- 벡터 연산: v + u, v - u, v * 2, v / 2
- 벡터 내적: v:dot(u)
- 벡터 외적: Vector.cross(v, u) -- 스칼라 반환
- 벡터 길이: v:length(), v:lengthSquared() (빠른 비교)
- 정규화: v:normalize() (새 벡터 반환), v:makeUnit() (자기 자신을 단위 벡터로)
- 수직 벡터: v:perpendicular()
- 각도: v:angle()

-- 예시
local v = Vector.new(3, 4)
print(v:length()) -- 5
local u = Vector.new(1, 0)
print(Vector.cross(v, u)) -- 4
v:makeUnit() -- v가 (0.6, 0.8)로 변경
]]--

local Vector = {}
Vector.__index = Vector

function Vector.new(x, y)
    return setmetatable({x = x or 0, y = y or 0}, Vector)
end

function Vector:clone()
    return Vector.new(self.x, self.y)
end

function Vector:unpack()
    return self.x, self.y
end

function Vector.__add(a, b)
    return Vector.new(a.x + b.x, a.y + b.y)
end

function Vector.__sub(a, b)
    return Vector.new(a.x - b.x, a.y - b.y)
end

function Vector.__mul(a, b)
    if type(a) == "number" then
        return Vector.new(a * b.x, a * b.y)
    elseif type(b) == "number" then
        return Vector.new(a.x * b, a.y * b)
    else
        error("Vector can only be multiplied by a number")
    end
end

function Vector.__div(a, b)
    return Vector.new(a.x / b, a.y / b)
end

function Vector:dot(b)
    return self.x * b.x + self.y * b.y
end

function Vector:lengthSquared()
    return self.x * self.x + self.y * self.y
end

Vector.sqlength = Vector.lengthSquared

function Vector:length()
    return math.sqrt(self:lengthSquared())
end

function Vector:normalize(epsilon)
    epsilon = epsilon or 1e-8
    local lenSq = self:lengthSquared()
    if lenSq < epsilon then
        return Vector.new(0, 0)
    end
    local len = math.sqrt(lenSq)
    if math.abs(len - 1) < epsilon then
        return self:clone()
    end
    return self / len
end


-- 벡터에 수직인 벡터(perpendicular vector) 반환
-- (x, y) → (-y, x)
-- 예시:
--   v = (2, 1)
--   v:perpendicular() → (-1, 2)
--

-- 변환 전 (LOVE2D 화면 좌표계)
-- (0,0)
--   ┌──────────────→ x
--   │   
--   │    ● (2,1)
--   │   /
--   │  /
--   ↓ y
--
-- 변환 후 (수직 벡터)
-- (0,0)
--   ┌──────────────→ x
--   │   
--   │● (-1,2)
--   │ \
--   │  \
--   ↓ y
--
-- ※ (2,1)은 오른쪽 아래, (-1,2)는 왼쪽 아래 방향입니다. (-1,2)는 +x축이 아니라 -x축(왼쪽) 방향임에 주의!
function Vector:perpendicular()
    return Vector.new(-self.y, self.x)
end



-- 벡터의 각도(라디안) 반환
-- 기준: +x축(오른쪽), 반시계 방향이 양수
-- LOVE2D의 왼손 좌표계 기준 (y축 아래로 증가)
-- 예시:
--   (1, 0) → 0 라디안 (0°)   → 오른쪽
--   (0, 1) → +π/2 (90°)      → 아래
--   (-1, 0) → π (180°)       → 왼쪽
--   (0, -1) → -π/2 (-90°)    → 위
function Vector:angle()
    return math.atan2(self.y, self.x)
end

-- In-place normalization: 자기 자신을 단위 벡터로 변환
function Vector:makeUnit(epsilon)
    epsilon = epsilon or 1e-8
    local lenSq = self:lengthSquared()
    if lenSq < epsilon then
        self.x, self.y = 0, 0
        return self
    end
    local len = math.sqrt(lenSq)
    if math.abs(len - 1) < epsilon then
        return self
    end
    self.x = self.x / len
    self.y = self.y / len
    return self
end

-- 두 2D 벡터의 외적(크로스 프로덕트)
-- cross(a, b)는 스칼라(면적, 방향성) 값을 반환
-- 양수: b가 a의 반시계 방향, 음수: 시계 방향, 0: 평행
function Vector.cross(a, b)
    return a.x * b.y - a.y * b.x
end

return Vector