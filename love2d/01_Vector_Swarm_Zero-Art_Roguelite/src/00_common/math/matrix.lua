
--[[
2D 변환용 3x3 행렬(Matrix) 모듈

좌표계 설명
 - LOVE2D/Lua 기본: (0,0)은 좌상단, x축 오른쪽(+), y축 아래쪽(+)
     ┌──────────────→ x (오른쪽이 +)
     │
     │
     ↓
     y (아래가 +)

기본 구조 및 용도
 - 3x3 행렬은 2D 변환(이동, 회전, 스케일, 전단 등)과 계층 구조(월드/로컬 변환)에 사용
 - 동차좌표(Homogeneous coordinates) 방식
 - 벡터 (x, y)는 (x, y, 1)로 확장하여 변환

행렬의 형태:
    | a b c |
    | d e f |
    | 0 0 1 |


예시

-- 변환 순서 주의!
-- 2D 그래픽스에서 "확대→회전→이동" 순서로 곱해야 직관적인 결과를 얻을 수 있음
-- (즉, m = 이동 * 회전 * 확대)


-- 아래 예시는 점(1,2)에 대해 2배 확대 → 90도(π/2) 회전 → (100,50)만큼 이동을 적용

-- 변환 전 (좌표계)
-- (0,0)
--   ┌──────────────→ x
--   │
--   │   ● (1,2)
--   ↓
--   y

local m = Matrix.new()
m = m:translate(100, 50) * Matrix.new():rotate(0) * Matrix.new():scale(2)
local x2, y2 = m:transformPoint(1, 2) -- x2=102, y2=54

-- 변환 후 (좌표계, 예시)
-- (0,0)
--   ┌──────────────→ x
--   │
--   │
--   │           ● (102, 54)
--   ↓
--   y

local m2 = Matrix.new()
m2 = m2:translate(100, 50) * Matrix.new():rotate(math.pi/2) * Matrix.new():scale(2)
local x3, y3 = m2:transformPoint(1, 2) -- x3=96, y3=52

-- 변환 후 (좌표계, 예시)
-- (0,0)
--   ┌──────────────→ x
--   │
--   │
--   │           ● (96, 52)
--   ↓
--   y

-- 또는 메서드 체이닝으로 쓸 경우 (뒤에서부터 적용됨)
-- m = Matrix.new():scale(2):rotate(math.pi/4):translate(100, 50)
-- 위 체이닝은 실제로는 m = 이동 * 회전 * 확대와 동일

-- 주요 함수
-- Matrix.new(), clone(), mul(), transformPoint(), translate(), scale(), rotate(), shear()
]]--

local _cos = math.cos
local _sin = math.sin

local Matrix = {}
Matrix.__index = Matrix


-- 새 3x3 행렬(Matrix) 생성
-- m: 9개 원소의 배열(생략 시 단위행렬 반환)
function Matrix.new(m)
    m = m or {
        1, 0, 0,
        0, 1, 0,
        0, 0, 1
    }
    return setmetatable({m = m}, Matrix)
end


-- 행렬 복사(clone)
function Matrix:clone()
    local m = self.m
    
    return Matrix.new({
        m[1], m[2], m[3],
        m[4], m[5], m[6],
        m[7], m[8], m[9]
    })
end


-- 행렬 곱셈(matrix multiplication)
-- self * other (좌측곱)
function Matrix:mul(other)
    local a, b = self.m, other.m
    local r = {}
    for row = 0, 2 do
        for col = 0, 2 do
            -- r[row, col] = sum(a[row, k] * b[k, col])
            r[row*3+col+1] = a[row*3+1]*b[col+1] + a[row*3+2]*b[col+4] + a[row*3+3]*b[col+7]
        end
    end
    return Matrix.new(r)
end

-- * 연산자 오버로딩: 행렬 곱셈(Matrix multiplication)
function Matrix.__mul(a, b)
    return a:mul(b)
end


-- 점(x, y)에 행렬 변환 적용
-- (x, y, 1) * self
function Matrix:transformPoint(x, y)
    local m = self.m
    local tx = m[1]*x + m[2]*y + m[3]
    local ty = m[4]*x + m[5]*y + m[6]
    return tx, ty
end


-- 이동(translation) 변환 적용
-- dx, dy: 이동 거리
function Matrix:translate(dx, dy)
    local t = Matrix.new({
        1, 0, dx,
        0, 1, dy,
        0, 0, 1
    })
    return self:mul(t)
end


-- 스케일(확대/축소, scale) 변환 적용
-- sx, sy: x, y축 스케일(생략 시 등방성)
function Matrix:scale(sx, sy)
    sy = sy or sx
    local s = Matrix.new({
        sx, 0, 0,
        0, sy, 0,
        0, 0, 1
    })
    return self:mul(s)
end


-- 회전(rotation) 변환 적용
-- angle: 회전 각도(라디안, +는 반시계 방향)
function Matrix:rotate(angle)
    local c, s = _cos(angle), _sin(angle)
    local r = Matrix.new({
        c, -s, 0,
        s,  c, 0,
        0,  0, 1
    })
    return self:mul(r)
end


-- 전단(shear) 변환 적용
-- kx: x축 전단, ky: y축 전단
function Matrix:shear(kx, ky)
    local sh = Matrix.new({
        1, kx or 0, 0,
        ky or 0, 1, 0,
        0, 0, 1
    })
    return self:mul(sh)
end

return Matrix
