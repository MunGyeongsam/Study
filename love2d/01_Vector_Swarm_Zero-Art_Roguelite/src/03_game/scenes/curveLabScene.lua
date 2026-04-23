-- CurveLabScene
-- 극좌표 수학 곡선 실험용 디버그 씬
-- 타이틀에서 C키로 진입. ESC로 복귀.
-- ←/→ 곡선 전환, ↑/↓ 정점 수 증감, M 렌더모드 전환, +/- 스케일

local lg = love.graphics
local _sin   = math.sin
local _cos   = math.cos
local _exp   = math.exp
local _pi    = math.pi
local _sqrt  = math.sqrt
local _abs   = math.abs
local _floor = math.floor
local _min   = math.min

local CurveLabScene = {}
CurveLabScene.__index = CurveLabScene

CurveLabScene.name        = "CurveLabScene"
CurveLabScene.transparent = false
CurveLabScene.drawBelow   = true

-- ─── Curve definitions ───────────────────────────────────────────
-- 극좌표 곡선: name, formula(표시용), fn(θ→r), tRange, defaultSteps

local CURVES = {
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
        name = "Reuleaux Triangle",
        formula = "3 arcs (constant width)",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local R = 1.0  -- side length = sqrt(3)
            local s3 = _sqrt(3)
            -- 3 vertices of equilateral triangle
            local pts = {}
            for i = 0, 2 do
                local a = i * 2 * _pi / 3 - _pi / 2
                pts[i + 1] = {_cos(a), _sin(a)}
            end
            local arcSteps = _floor(steps / 3)
            if arcSteps < 4 then arcSteps = 4 end
            -- Arc from pt[2] to pt[3] centered on pt[1], etc.
            local order = {{1, 2, 3}, {2, 3, 1}, {3, 1, 2}}
            for _, o in ipairs(order) do
                local center = pts[o[1]]
                local pFrom  = pts[o[2]]
                local pTo    = pts[o[3]]
                local aStart = math.atan2(pFrom[2] - center[2], pFrom[1] - center[1])
                local aEnd   = math.atan2(pTo[2] - center[2], pTo[1] - center[1])
                -- ensure positive arc direction
                if aEnd < aStart then aEnd = aEnd + 2 * _pi end
                local arcR = s3  -- distance between vertices
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
        name = "Vesica Piscis",
        formula = "Intersection of two circles",
        fn = "custom",
        customFn = function(steps)
            local verts = {}
            local half = _floor(steps / 2)
            if half < 4 then half = 4 end
            local d = 0.5  -- half distance between centers
            -- Right arc (center at -d, 0)
            local aStart = math.asin(d)
            for s = 0, half - 1 do
                local a = -aStart + 2 * aStart * s / (half - 1)
                verts[#verts + 1] = d + _cos(a)
                verts[#verts + 1] = _sin(a)
            end
            -- Left arc (center at +d, 0)
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
            -- Koch subdivision of a single edge
            local function subdivide(pts, depth)
                if depth == 0 then return pts end
                local result = {}
                for i = 1, #pts - 1 do
                    local ax, ay = pts[i][1], pts[i][2]
                    local bx, by = pts[i + 1][1], pts[i + 1][2]
                    local dx, dy = bx - ax, by - ay
                    local p1x, p1y = ax + dx / 3, ay + dy / 3
                    local p2x, p2y = ax + dx * 2 / 3, ay + dy * 2 / 3
                    -- peak: 60 degree rotation
                    local px = p1x + (dx / 3) * _cos(_pi / 3) - (dy / 3) * _sin(_pi / 3)
                    local py = p1y + (dx / 3) * _sin(_pi / 3) + (dy / 3) * _cos(_pi / 3)
                    result[#result + 1] = {ax, ay}
                    result[#result + 1] = {p1x, p1y}
                    result[#result + 1] = {px, py}
                    result[#result + 1] = {p2x, p2y}
                end
                result[#result + 1] = pts[#pts]
                return subdivide(result, depth - 1)
            end
            -- Start: equilateral triangle
            local tri = {
                {0, -1},
                {_cos(-_pi / 6), _sin(-_pi / 6)},
                {_cos(-5 * _pi / 6), _sin(-5 * _pi / 6)},
                {0, -1},  -- close
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
        defaultSteps = 0,  -- ignored for custom
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

local MODES = {"line", "points"}

-- ─── CurveLabScene ───────────────────────────────────────────────

function CurveLabScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
        _curveIdx   = 1,
        _steps      = nil,  -- nil = use defaultSteps
        _mode       = 1,    -- index into MODES
        _scale      = 1.0,
        _timer      = 0,
        _verts      = nil,  -- cached {x1,y1,x2,y2,...}
        _fonts      = nil,
    }, CurveLabScene)
end

function CurveLabScene:enter(prev)
    self._timer = 0
    self._curveIdx = 1
    self._mode = 1
    self._scale = 1.0
    self._fonts = {
        title   = lg.newFont(16),
        formula = lg.newFont(13),
        info    = lg.newFont(11),
        hint    = lg.newFont(10),
    }
    self:_buildVerts()
    logInfo("[CURVELAB] CurveLabScene entered")
end

function CurveLabScene:exit()
    logInfo("[CURVELAB] CurveLabScene exited")
end

function CurveLabScene:_getCurve()
    return CURVES[self._curveIdx]
end

function CurveLabScene:_getSteps()
    return self._steps or self:_getCurve().defaultSteps
end

--- 현재 곡선의 정점 배열을 생성/캐시
function CurveLabScene:_buildVerts()
    local curve = self:_getCurve()
    local steps = self:_getSteps()
    local verts = {}

    if curve.fn == "custom" then
        verts = curve.customFn(steps)
    elseif curve.fn == "parametric" then
        local t0, t1 = curve.tRange[1], curve.tRange[2]
        for i = 0, steps - 1 do
            local t = t0 + (t1 - t0) * i / steps
            local x, y = curve.paramFn(t)
            verts[#verts + 1] = x
            verts[#verts + 1] = y
        end
    else
        -- polar: r = fn(θ)
        local t0, t1 = curve.tRange[1], curve.tRange[2]
        for i = 0, steps - 1 do
            local t = t0 + (t1 - t0) * i / steps
            local r = curve.fn(t)
            verts[#verts + 1] = r * _cos(t)
            verts[#verts + 1] = r * _sin(t)
        end
    end

    self._verts = verts
end

function CurveLabScene:update(dt)
    self._timer = self._timer + dt
end

-- ─── Draw ────────────────────────────────────────────────────────

function CurveLabScene:draw()
    local W, H = lg.getDimensions()
    local fonts = self._fonts
    local t = self._timer
    local curve = self:_getCurve()

    -- Background
    setColor(0, 0, 0, 242)
    lg.rectangle("fill", 0, 0, W, H)

    -- Title
    lg.setFont(fonts.title)
    local glow = 0.6 + 0.2 * _sin(t * 2)
    setColor(glow * 255, 76, 255, 255)
    local titleStr = string.format("CURVE LAB — %s (%d/%d)",
        curve.name, self._curveIdx, #CURVES)
    lg.printf(titleStr, 0, 12, W, "center")

    -- Formula
    lg.setFont(fonts.formula)
    setColor(178, 178, 204, 230)
    lg.printf(curve.formula, 0, 34, W, "center")

    -- Draw the curve
    local cx, cy = W / 2, H * 0.45
    local baseScale = _min(W, H) * 0.15 * self._scale
    local verts = self._verts

    if verts and #verts >= 4 then
        -- Build screen-space vertices
        local sv = {}
        for i = 1, #verts, 2 do
            sv[#sv + 1] = cx + verts[i] * baseScale
            sv[#sv + 1] = cy - verts[i + 1] * baseScale  -- Y up → screen Y down
        end

        local modeName = MODES[self._mode]

        -- Glow layer (slightly larger, dimmer)
        setColor(51, 128, 255, 64)
        lg.setLineWidth(3)
        lg.setPointSize(4)
        if modeName == "line" and #sv >= 6 then
            lg.line(sv)
        elseif modeName == "points" then
            lg.points(sv)
        end

        -- Main layer
        setColor(76, 204, 255, 255)
        lg.setLineWidth(1.5)
        lg.setPointSize(2)
        if modeName == "line" and #sv >= 6 then
            lg.line(sv)
        elseif modeName == "points" then
            lg.points(sv)
        end

        lg.setLineWidth(1)
        lg.setPointSize(1)
    end

    -- Crosshair at center
    setColor(76, 76, 76, 76)
    lg.line(cx - 20, cy, cx + 20, cy)
    lg.line(cx, cy - 20, cx, cy + 20)

    -- Info panel (bottom-left)
    lg.setFont(fonts.info)
    local infoY = H - 120
    local infoX = 16
    local lineH = 16

    setColor(128, 128, 153, 204)
    lg.print(string.format("vertices: %d", self:_getSteps()), infoX, infoY)
    infoY = infoY + lineH
    lg.print(string.format("mode: %s", MODES[self._mode]), infoX, infoY)
    infoY = infoY + lineH
    lg.print(string.format("scale: %.1fx", self._scale), infoX, infoY)
    infoY = infoY + lineH
    lg.print(string.format("points in array: %d", self._verts and #self._verts / 2 or 0), infoX, infoY)

    -- Hints (bottom center)
    lg.setFont(fonts.hint)
    local alpha = 0.4 + 0.15 * _sin(t * 2)
    setColor(128, 128, 128, alpha * 255)
    lg.printf(
        "LEFT/RIGHT: curve | UP/DOWN: vertices +/-10 | M: mode | +/-: scale | ESC: back",
        0, H - 24, W, "center"
    )

    resetColor()
end

-- ─── Input ───────────────────────────────────────────────────────

function CurveLabScene:keypressed(key)
    if key == "escape" then
        self._sceneStack:pop()
        return true
    elseif key == "left" then
        self._curveIdx = self._curveIdx - 1
        if self._curveIdx < 1 then self._curveIdx = #CURVES end
        self._steps = nil  -- reset to default
        self:_buildVerts()
        return true
    elseif key == "right" then
        self._curveIdx = self._curveIdx + 1
        if self._curveIdx > #CURVES then self._curveIdx = 1 end
        self._steps = nil
        self:_buildVerts()
        return true
    elseif key == "up" then
        local s = self:_getSteps() + 10
        if s > 2000 then s = 2000 end
        self._steps = s
        self:_buildVerts()
        return true
    elseif key == "down" then
        local s = self:_getSteps() - 10
        if s < 10 then s = 10 end
        self._steps = s
        self:_buildVerts()
        return true
    elseif key == "m" then
        self._mode = self._mode + 1
        if self._mode > #MODES then self._mode = 1 end
        return true
    elseif key == "=" or key == "kp+" then
        self._scale = self._scale + 0.2
        if self._scale > 5.0 then self._scale = 5.0 end
        return true
    elseif key == "-" or key == "kp-" then
        self._scale = self._scale - 0.2
        if self._scale < 0.2 then self._scale = 0.2 end
        return true
    end
    return false
end

-- macOS IME 한글 입력 시 keypressed 우회 대응
-- 한글 2벌식: ㅡ=m
local JAMO_TO_KEY = { ["ㅡ"] = "m" }
function CurveLabScene:textinput(text)
    local key = JAMO_TO_KEY[text] or text:lower()
    if key == "m" then
        return self:keypressed("m")
    end
    return false
end

function CurveLabScene:touchpressed(id, x, y, dx, dy, pressure)
    local W = lg.getDimensions()
    if x < W * 0.3 then
        self:keypressed("left")
    elseif x > W * 0.7 then
        self:keypressed("right")
    else
        self._sceneStack:pop()
    end
    return true
end

return CurveLabScene
