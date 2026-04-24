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
        name = "Rose 8/3",
        formula = "r = cos(8/3 * t)",
        fn = function(t) return _cos(8 / 3 * t) end,
        tRange = {0, 6 * _pi},
        defaultSteps = 420,
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
        name = "Epicycloid (k=4)",
        formula = "x=(R+r)cos(t)-r*cos((R+r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r = 1, 1 / 4
            return (R + r) * _cos(t) - r * _cos((R + r) * t / r),
                   (R + r) * _sin(t) - r * _sin((R + r) * t / r)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 180,
    },
    {
        name = "Epicycloid (k=5)",
        formula = "x=(R+r)cos(t)-r*cos((R+r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r = 1, 1 / 5
            return (R + r) * _cos(t) - r * _cos((R + r) * t / r),
                   (R + r) * _sin(t) - r * _sin((R + r) * t / r)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 220,
    },
    {
        name = "Epicycloid (k=6)",
        formula = "x=(R+r)cos(t)-r*cos((R+r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r = 1, 1 / 6
            return (R + r) * _cos(t) - r * _cos((R + r) * t / r),
                   (R + r) * _sin(t) - r * _sin((R + r) * t / r)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 260,
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
        name = "Gerono Lemniscate",
        formula = "x = cos(t), y = sin(t)cos(t)",
        fn = "parametric",
        paramFn = function(t)
            return _cos(t), _sin(t) * _cos(t)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 220,
    },
    {
        name = "Booth's Lemniscate",
        formula = "x = cos(t)/(1+sin^2), y = sin(t)cos(t)/(1+sin^2)",
        fn = "parametric",
        paramFn = function(t)
            local s, c = _sin(t), _cos(t)
            local d = 1 + s * s
            return c / d, (s * c) / d
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 260,
    },
    {
        name = "Hypotrochoid (R=7,r=3,d=2)",
        formula = "x=(R-r)cos(t)+d*cos((R-r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r, d = 7, 3, 2
            local k = (R - r) / r
            local norm = (R - r) + d
            return ((R - r) * _cos(t) + d * _cos(k * t)) / norm,
                   ((R - r) * _sin(t) - d * _sin(k * t)) / norm
        end,
        tRange = {0, 6 * _pi},
        defaultSteps = 320,
    },
    {
        name = "Hypotrochoid (R=7,r=3,d=4)",
        formula = "x=(R-r)cos(t)+d*cos((R-r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r, d = 7, 3, 4
            local k = (R - r) / r
            local norm = (R - r) + d
            return ((R - r) * _cos(t) + d * _cos(k * t)) / norm,
                   ((R - r) * _sin(t) - d * _sin(k * t)) / norm
        end,
        tRange = {0, 6 * _pi},
        defaultSteps = 360,
    },
    {
        name = "Epitrochoid (R=5,r=2,d=2)",
        formula = "x=(R+r)cos(t)-d*cos((R+r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r, d = 5, 2, 2
            local k = (R + r) / r
            local norm = (R + r) + d
            return ((R + r) * _cos(t) - d * _cos(k * t)) / norm,
                   ((R + r) * _sin(t) - d * _sin(k * t)) / norm
        end,
        tRange = {0, 4 * _pi},
        defaultSteps = 320,
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
    -- ─── 곡선 추가 2026-04-23 (2차) ─────────────────────────────
    {
        name = "Cayley's Sextic",
        formula = "r = 4cos^3(t/3)",
        fn = function(t) return 4 * _cos(t / 3) ^ 3 end,
        tRange = {0, 6 * _pi},
        defaultSteps = 400,
    },
    {
        name = "Bifolium",
        formula = "r = sin(t)cos^2(t)",
        fn = function(t) return _sin(t) * _cos(t) * _cos(t) end,
        tRange = {0, 2 * _pi},
        defaultSteps = 200,
    },
    {
        name = "Quadrifolium",
        formula = "r = cos(2t)",
        fn = function(t) return _cos(2 * t) end,
        tRange = {0, 2 * _pi},
        defaultSteps = 200,
    },
    {
        name = "Freeth's Nephroid",
        formula = "r = (1 + 2sin(t/2)) / 3",
        fn = function(t) return (1 + 2 * _sin(t / 2)) / 3 end,
        tRange = {0, 4 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Ophiuride",
        formula = "r = (sin(t) - 0.5)tan(t)",
        fn = function(t)
            local c = _cos(t)
            if _abs(c) < 0.05 then return 0 end
            return (_sin(t) - 0.5) * _sin(t) / c
        end,
        tRange = {-1.2, 1.2},
        defaultSteps = 200,
    },
    {
        name = "Strophoid",
        formula = "x=(1-t^2)/(1+t^2), y=t(1-t^2)/(1+t^2)",
        fn = "parametric",
        paramFn = function(t)
            local d = 1 + t * t
            return (1 - t * t) / d, t * (1 - t * t) / d
        end,
        tRange = {-2.5, 2.5},
        defaultSteps = 200,
    },
    {
        name = "Bicorn",
        formula = "x=cos(t), y=sin^2(t)/(2+sin(t))",
        fn = "parametric",
        paramFn = function(t)
            local s = _sin(t)
            return _cos(t), s * s / (2 + s)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 150,
    },
    {
        name = "Ranunculoid (k=5)",
        formula = "x=6cos(t)-cos(6t), y=6sin(t)-sin(6t)",
        fn = "parametric",
        paramFn = function(t)
            return (6 * _cos(t) - _cos(6 * t)) / 7,
                   (6 * _sin(t) - _sin(6 * t)) / 7
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 200,
    },
    {
        name = "Hypotrochoid (R=5,r=3,d=5)",
        formula = "x=2cos(t)+5cos(2t/3), y=2sin(t)-5sin(2t/3)",
        fn = "parametric",
        paramFn = function(t)
            return (2 * _cos(t) + 5 * _cos(2 * t / 3)) / 7,
                   (2 * _sin(t) - 5 * _sin(2 * t / 3)) / 7
        end,
        tRange = {0, 6 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Kampyle of Eudoxus",
        formula = "x=sec(t), y=tan(t)sec(t)",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local half = _floor(steps / 2)
            if half < 10 then half = 10 end
            local tMax = _pi / 2 - 0.12
            -- right branch
            for i = 0, half - 1 do
                local t = -tMax + 2 * tMax * i / (half - 1)
                local c = _cos(t)
                verts[#verts + 1] = 1 / (c * 3)
                verts[#verts + 1] = _sin(t) / (c * c * 3)
            end
            -- left branch (mirrored)
            for i = half - 1, 0, -1 do
                local t = -tMax + 2 * tMax * i / (half - 1)
                local c = _cos(t)
                verts[#verts + 1] = -1 / (c * 3)
                verts[#verts + 1] = _sin(t) / (c * c * 3)
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 200,
    },
    {
        name = "Cassini Oval",
        formula = "(x^2+y^2)^2-2a^2(x^2-y^2)+a^4-b^4=0",
        fn = function(t)
            local a = 0.6
            local b = 1.0
            local a2 = a * a
            local a4 = a2 * a2
            local b4 = b * b * b * b
            local s2 = _sin(2 * t)
            local c2 = _cos(2 * t)
            local disc = b4 - a4 * s2 * s2
            if disc < 0 then return 0 end
            local r2 = a2 * c2 + _sqrt(disc)
            if r2 < 0 then return 0 end
            return _sqrt(r2)
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 360,
    },
    {
        name = "Epitrochoid (R=3,r=1,d=1)",
        formula = "x=(R+r)cos(t)-d*cos((R+r)t/r)",
        fn = "parametric",
        paramFn = function(t)
            local R, r, d = 3, 1, 1
            local k = (R + r) / r
            local norm = R + r + d
            return ((R + r) * _cos(t) - d * _cos(k * t)) / norm,
                   ((R + r) * _sin(t) - d * _sin(k * t)) / norm
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 300,
    },
    {
        name = "Conchoid of Nicomedes",
        formula = "r = a/cos(t) + b",
        fn = function(t)
            local a = 0.25
            local b = 0.65
            local c = _cos(t)
            if _abs(c) < 0.06 then return 0 end
            return a / c + b
        end,
        tRange = {-1.45, 1.45},
        defaultSteps = 320,
    },
    {
        name = "Devil's Curve",
        formula = "y^2 = x^2(x^2-1)/(x^2+1)",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local n = _floor(steps / 2)
            if n < 40 then n = 40 end
            local xMin, xMax = -2.0, 2.0

            -- upper branch
            for i = 0, n - 1 do
                local x = xMin + (xMax - xMin) * i / (n - 1)
                local x2 = x * x
                local v = x2 * (x2 - 1) / (x2 + 1)
                if v >= 0 then
                    verts[#verts + 1] = x / 2
                    verts[#verts + 1] = _sqrt(v) / 2
                end
            end

            -- lower branch (reverse)
            for i = n - 1, 0, -1 do
                local x = xMin + (xMax - xMin) * i / (n - 1)
                local x2 = x * x
                local v = x2 * (x2 - 1) / (x2 + 1)
                if v >= 0 then
                    verts[#verts + 1] = x / 2
                    verts[#verts + 1] = -_sqrt(v) / 2
                end
            end
            return verts
        end,
        tRange = {0, 0},
        defaultSteps = 320,
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
    -- #53: Teardrop (물방울형 비대칭 closed curve)
    -- m 파라미터로 뾰족함 조절: m=1 부드러운 방울, m=3 날카로운 첨단
    -- 적 용도: "물방울형 돌진 적" — 비대칭이 방향성 암시 (뾰족한 쪽이 앞)
    {
        name = "Teardrop",
        formula = "x=cos(t), y=sin(t)*sin(t/2)^m (m=2)",
        fn = "parametric",
        paramFn = function(t)
            local st2 = _sin(t * 0.5)
            return _cos(t), _sin(t) * st2 * st2
        end,
        tRange = {0, 2 * _pi},
        defaultSteps = 150,
    },
}

local OPEN_CURVES = {
    ["Logarithmic Spiral"] = true,
    ["Lituus"] = true,
    ["Cissoid"] = true,
    ["Ophiuride"] = true,
    ["Strophoid"] = true,
    ["Kampyle of Eudoxus"] = true,
    ["Conchoid of Nicomedes"] = true,
    ["Devil's Curve"] = true,
    ["Koch Edge (iter 2)"] = true,
    ["Fermat Spiral"] = true,
}

local DISCONTINUOUS_CURVES = {
    ["Cissoid"] = true,
    ["Ophiuride"] = true,
    ["Kampyle of Eudoxus"] = true,
    ["Conchoid of Nicomedes"] = true,
}

local function inferComplexity(defaultSteps)
    local s = defaultSteps or 0
    if s <= 160 then return 1 end
    if s <= 320 then return 2 end
    return 3
end

local function inferFamily(curve)
    local n = curve.name
    if n:find("Rose") then return "rose" end
    if n:find("Lemniscate") then return "lemniscate" end
    if n:find("Epicycloid") or n:find("Hypocycloid") then return "cycloid" end
    if n:find("Trochoid") or n:find("Spirograph") then return "trochoid" end
    if n:find("Spiral") then return "spiral" end
    if curve.fn == "custom" then return "custom" end
    if curve.fn == "parametric" then return "parametric" end
    return "polar"
end

local function inferClosed(curve)
    if OPEN_CURVES[curve.name] then return false end
    if curve.fn == "custom" and curve.tRange[1] == 0 and curve.tRange[2] == 0 then
        -- custom은 이름 기반으로 open 케이스만 제외하고 기본 closed로 본다.
        return true
    end
    local t0, t1 = curve.tRange[1], curve.tRange[2]
    local span = _abs(t1 - t0)
    if span < _pi * 1.95 then return false end
    return true
end

for i = 1, #M do
    local c = M[i]
    c.complexity = inferComplexity(c.defaultSteps)
    c.family = inferFamily(c)
    c.closed = inferClosed(c)
    c.discontinuous = DISCONTINUOUS_CURVES[c.name] or false
    c.enemyFriendly = c.closed and (not c.discontinuous) and c.complexity <= 2
end

return M
