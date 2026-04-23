-- curveDefs.lua
-- CurveLab 실험 곡선 정의 데이터
-- 새 곡선 추가 시 이 파일만 수정. curveLabScene.lua는 건드리지 않음.
--
-- 곡선 타입 3가지:
--   polar      fn(θ) → r  (기본)
--   parametric paramFn(t) → x, y
--   custom     customFn(steps) → {x1,y1,x2,y2,...}

local _sin   = math.sin
local _cos   = math.cos
local _exp   = math.exp
local _pi    = math.pi
local _sqrt  = math.sqrt
local _abs   = math.abs
local _floor = math.floor
local _min   = math.min

-- ─── 곡선 목록 ────────────────────────────────────────────────────

local M = {

    -- ── 극좌표 장미 & 기본형 ──────────────────────────────────────
    {
        name = "Butterfly (Fay)",
        formula = "r = e^cos(t) - 2cos(4t) + sin^5(t/12)",
        fn = function(t)
            return _exp(_cos(t)) - 2 * _cos(4 * t) + _sin(t / 12) ^ 5
        end,
        tRange = {0, 12 * _pi},
        defaultSteps = 500,
    },
    {
        name = "Rose 5/4",
        formula = "r = cos(5/4 * t)",
        fn = function(t) return _cos(5 / 4 * t) end,
        tRange = {0, 8 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Rose 3",
        formula = "r = cos(3t)",
        fn = function(t) return _cos(3 * t) end,
        tRange = {0, _pi},
        defaultSteps = 200,
    },
    {
        name = "Rose 5",
        formula = "r = cos(5t)",
        fn = function(t) return _cos(5 * t) end,
        tRange = {0, _pi},
        defaultSteps = 200,
    },
    {
        name = "Rose 7/3",
        formula = "r = cos(7/3 * t)",
        fn = function(t) return _cos(7 / 3 * t) end,
        tRange = {0, 6 * _pi},
        defaultSteps = 400,
    },
    {
        name = "Cardioid",
        formula = "r = 1 + cos(t)",
        fn = function(t) return 1 + _cos(t) end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Lemniscate",
        formula = "r^2 = cos(2t)",
        fn = function(t)
            local c = _cos(2 * t)
            if c < 0 then return 0 end
            return _sqrt(c)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 200,
    },
    {
        name = "Limacon (inner loop)",
        formula = "r = 0.5 + cos(t)",
        fn = function(t) return 0.5 + _cos(t) end,
        tRange = {0, 2 * _pi},
        defaultSteps = 150,
    },
    {
        name = "Logarithmic Spiral",
        formula = "r = 0.1 * e^(0.15t)",
        fn = function(t) return 0.1 * _exp(0.15 * t) end,
        tRange = {0, 6 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Lituus",
        formula = "r = 1 / sqrt(t)",
        fn = function(t)
            if t < 0.3 then return 0 end
            return 1 / _sqrt(t)
        end,
        tRange = {0.3, 6 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Folium (3-leaf)",
        formula = "r = cos(t) * sin(2t)",
        fn = function(t) return _cos(t) * _sin(2 * t) end,
        tRange = {0, 2 * _pi},
        defaultSteps = 200,
    },
    {
        name = "Wavy Circle (19:3)",
        formula = "r = 0.9 + 0.1 * sin(19/3 * t)",
        fn = function(t) return 0.9 + 0.1 * _sin(19 / 3 * t) end,
        tRange = {0, 6 * _pi},
        defaultSteps = 400,
    },
    {
        name = "Cissoid",
        formula = "r = sin^2(t) / cos(t)",
        fn = function(t)
            local c = _cos(t)
            if _abs(c) < 0.05 then return 0 end
            return _sin(t) * _sin(t) / c
        end,
        tRange = {-1.2, 1.2},
        defaultSteps = 150,
    },

    -- ── 파라메트릭 ────────────────────────────────────────────────
    {
        name = "Astroid",
        formula = "x = cos^3(t), y = sin^3(t)",
        fn = "parametric",
        paramFn = function(t)
            return _cos(t) ^ 3, _sin(t) ^ 3
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Deltoid",
        formula = "x = 2cos(t)+cos(2t), y = 2sin(t)-sin(2t)",
        fn = "parametric",
        paramFn = function(t)
            return (2 * _cos(t) + _cos(2 * t)) / 3,
                   (2 * _sin(t) - _sin(2 * t)) / 3
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Superellipse (n=4)",
        formula = "|x|^n + |y|^n = 1, n=4",
        fn = "parametric",
        paramFn = function(t)
            local c, s = _cos(t), _sin(t)
            local sc = c >= 0 and 1 or -1
            local ss = s >= 0 and 1 or -1
            return sc * _abs(c) ^ 0.5,
                   ss * _abs(s) ^ 0.5
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Epicycloid (k=3)",
        formula = "x=(R+r)cos(t)-r*cos((R+r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r = 1, 1 / 3
            return (R + r) * _cos(t) - r * _cos((R + r) * t / r),
                   (R + r) * _sin(t) - r * _sin((R + r) * t / r)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 150,
    },
    {
        name = "Nephroid",
        formula = "x = 3cos(t)-cos(3t), y = 3sin(t)-sin(3t)",
        fn = "parametric",
        paramFn = function(t)
            return (3 * _cos(t) - _cos(3 * t)) / 4,
                   (3 * _sin(t) - _sin(3 * t)) / 4
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Heart Curve",
        formula = "x = 16sin^3(t), y = 13cos-5cos2-2cos3-cos4",
        fn = "parametric",
        paramFn = function(t)
            local s, c = _sin(t), _cos(t)
            return 16 * s * s * s / 17,
                   (13 * c - 5 * _cos(2 * t) - 2 * _cos(3 * t) - _cos(4 * t)) / 17
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Cornoid",
        formula = "x = cos(t)(1-2sin^2), y = sin(t)(1+2cos^2)",
        fn = "parametric",
        paramFn = function(t)
            local c, s = _cos(t), _sin(t)
            return c * (1 - 2 * s * s),
                   s * (1 + 2 * c * c)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 150,
    },
    {
        name = "Hypocycloid (k=5)",
        formula = "x = 4cos(t)+cos(4t), y = 4sin(t)-sin(4t)",
        fn = "parametric",
        paramFn = function(t)
            return (4 * _cos(t) + _cos(4 * t)) / 5,
                   (4 * _sin(t) - _sin(4 * t)) / 5
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 100,
    },
    {
        name = "Lissajous (3:2)",
        formula = "x = cos(3t), y = sin(2t)",
        fn = "parametric",
        paramFn = function(t)
            return _cos(3 * t), _sin(2 * t)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 200,
    },
    {
        name = "Lissajous (5:4)",
        formula = "x = cos(5t), y = sin(4t)",
        fn = "parametric",
        paramFn = function(t)
            return _cos(5 * t), _sin(4 * t)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Spirograph",
        formula = "R=1, r=0.4, d=0.6",
        fn = "parametric",
        paramFn = function(t)
            local R, r, d = 1, 0.4, 0.6
            return (R - r) * _cos(t) + d * _cos((R - r) / r * t),
                   (R - r) * _sin(t) - d * _sin((R - r) / r * t)
        end,
        tRange = {0, 10 * _pi},
        defaultSteps = 500,
    },

    -- ── 커스텀 (정점 직접 생성) ────────────────────────────────────
    {
        name = "Reuleaux Triangle",
        formula = "3 arcs (constant width)",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local s3 = _sqrt(3)
            local pts = {}
            for i = 0, 2 do
                local a = i * 2 * _pi / 3 - _pi / 2
                pts[i + 1] = {_cos(a), _sin(a)}
            end
            local arcSteps = _floor(steps / 3)
            if arcSteps < 4 then arcSteps = 4 end
            local order = {{1, 2, 3}, {2, 3, 1}, {3, 1, 2}}
            for _, o in ipairs(order) do
                local center = pts[o[1]]
                local pFrom  = pts[o[2]]
                local pTo    = pts[o[3]]
                local aStart = math.atan2(pFrom[2] - center[2], pFrom[1] - center[1])
                local aEnd   = math.atan2(pTo[2] - center[2], pTo[1] - center[1])
                if aEnd < aStart then aEnd = aEnd + 2 * _pi end
                local arcR = s3
                for s = 0, arcSteps - 1 do
                    local t = aStart + (aEnd - aStart) * s / arcSteps
                    verts[#verts + 1] = center[1] + arcR * _cos(t)
                    verts[#verts + 1] = center[2] + arcR * _sin(t)
                end
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 60,
    },
    {
        name = "Vesica Piscis",
        formula = "Intersection of two circles",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local half = _floor(steps / 2)
            if half < 4 then half = 4 end
            local d = 0.5
            local aStart = math.asin(d)
            for s = 0, half - 1 do
                local a = -aStart + 2 * aStart * s / (half - 1)
                verts[#verts + 1] = d + _cos(a)
                verts[#verts + 1] = _sin(a)
            end
            for s = 0, half - 1 do
                local a = (_pi - aStart) + 2 * aStart * s / (half - 1)
                verts[#verts + 1] = -d + _cos(a)
                verts[#verts + 1] = _sin(a)
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 60,
    },
    {
        name = "Koch Edge (iter 2)",
        formula = "Recursive triangle subdivision",
        fn = "custom",
        customFn = function(steps)
            local function subdivide(pts, depth)
                if depth == 0 then return pts end
                local result = {}
                for i = 1, #pts - 1 do
                    local ax, ay = pts[i][1], pts[i][2]
                    local bx, by = pts[i + 1][1], pts[i + 1][2]
                    local dx, dy = bx - ax, by - ay
                    local p1x, p1y = ax + dx / 3, ay + dy / 3
                    local px = p1x + (dx / 3) * _cos(_pi / 3) - (dy / 3) * _sin(_pi / 3)
                    local py = p1y + (dx / 3) * _sin(_pi / 3) + (dy / 3) * _cos(_pi / 3)
                    local p2x, p2y = ax + dx * 2 / 3, ay + dy * 2 / 3
                    result[#result + 1] = {ax, ay}
                    result[#result + 1] = {p1x, p1y}
                    result[#result + 1] = {px, py}
                    result[#result + 1] = {p2x, p2y}
                end
                result[#result + 1] = pts[#pts]
                return subdivide(result, depth - 1)
            end
            local tri = {
                {0, -1},
                {_cos(-_pi / 6), _sin(-_pi / 6)},
                {_cos(-5 * _pi / 6), _sin(-5 * _pi / 6)},
                {0, -1},
            }
            local pts = subdivide(tri, 2)
            local verts = {}
            for _, p in ipairs(pts) do
                verts[#verts + 1] = p[1]
                verts[#verts + 1] = p[2]
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 0,
    },
    {
        name = "Fermat Spiral",
        formula = "r = +-sqrt(t)",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local half = _floor(steps / 2)
            if half < 10 then half = 10 end
            local tMax = 6 * _pi
            local invSqrtMax = 1 / _sqrt(tMax)
            for i = 0, half - 1 do
                local t = tMax * i / (half - 1)
                local r = _sqrt(t) * invSqrtMax
                verts[#verts + 1] = r * _cos(t)
                verts[#verts + 1] = r * _sin(t)
            end
            for i = half - 1, 0, -1 do
                local t = tMax * i / (half - 1)
                local r = -_sqrt(t) * invSqrtMax
                verts[#verts + 1] = r * _cos(t)
                verts[#verts + 1] = r * _sin(t)
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 300,
    },
    {
        name = "Maurer Rose (n=6, d=71)",
        formula = "r = sin(6t), lines at t=d*k degrees",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local n, d = 6, 71
            local count = _min(steps, 361)
            for k = 0, count - 1 do
                local t = k * d * _pi / 180
                local r = _sin(n * t)
                verts[#verts + 1] = r * _cos(t)
                verts[#verts + 1] = r * _sin(t)
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 361,
    },
}

return M
