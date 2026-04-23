-- CurveLabScene
-- 극좌표 수학 곡선 실험용 디버그 씬
-- 타이틀에서 C키로 진입. ESC로 복귀.
-- L/R: 곡선 전환, U/D: 정점 수 증감, M: 렌더모드, R: 회전 토글, +/-: 스케일
-- 곡선 데이터: 03_game/data/curveDefs.lua

local lg   = love.graphics
local _sin = math.sin
local _cos = math.cos
local _pi  = math.pi
local _min = math.min

local CurveLabScene = {}
CurveLabScene.__index = CurveLabScene

CurveLabScene.name        = "CurveLabScene"
CurveLabScene.transparent = false
CurveLabScene.drawBelow   = true

local CURVES = require("03_game.data.curveDefs")
local MODES  = {"line", "points", "circle", "fill"}

-- ─── Constructor ─────────────────────────────────────────────────

function CurveLabScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
        _curveIdx   = 1,
        _steps      = nil,   -- nil = use curve.defaultSteps
        _mode       = 1,     -- index into MODES
        _scale      = 1.0,
        _timer      = 0,
        _verts      = nil,   -- cached world-space vertices {x1,y1,...}
        _fonts      = nil,
        _rotating   = false,
        _rotAngle   = 0,
        _centroid   = {x = 0, y = 0},
    }, CurveLabScene)
end

function CurveLabScene:enter(prev)
    self._timer    = 0
    self._curveIdx = 1
    self._mode     = 1
    self._scale    = 1.0
    self._rotating = false
    self._rotAngle = 0
    self._centroid = {x = 0, y = 0}
    self._steps    = nil
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

-- ─── Helpers ─────────────────────────────────────────────────────

function CurveLabScene:_getCurve()
    return CURVES[self._curveIdx]
end

function CurveLabScene:_getSteps()
    return self._steps or self:_getCurve().defaultSteps
end

--- 정점 배열 재계산 + 무게중심 갱신
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
        -- polar: r = fn(theta)
        local t0, t1 = curve.tRange[1], curve.tRange[2]
        for i = 0, steps - 1 do
            local t = t0 + (t1 - t0) * i / steps
            local r = curve.fn(t)
            verts[#verts + 1] = r * _cos(t)
            verts[#verts + 1] = r * _sin(t)
        end
    end

    self._verts = verts

    -- 무게중심 계산 (회전 기준점)
    local n = #verts / 2
    if n > 0 then
        local sx, sy = 0, 0
        for i = 1, #verts, 2 do
            sx = sx + verts[i]
            sy = sy + verts[i + 1]
        end
        self._centroid = {x = sx / n, y = sy / n}
    else
        self._centroid = {x = 0, y = 0}
    end
end

--- 화면 좌표 정점 배열을 modeName에 따라 렌더 (Glow/Main 공용)
local function _drawLayer(sv, modeName, cx, cy, pointR)
    if modeName == "line" and #sv >= 6 then
        lg.line(sv)
    elseif modeName == "points" and #sv >= 2 then
        lg.points(sv)
    elseif modeName == "circle" then
        for i = 1, #sv, 2 do
            lg.circle("fill", sv[i], sv[i + 1], pointR)
        end
    elseif modeName == "fill" and #sv >= 6 then
        for i = 1, #sv - 2, 2 do
            lg.polygon("fill", cx, cy, sv[i], sv[i + 1], sv[i + 2], sv[i + 3])
        end
        lg.polygon("fill", cx, cy, sv[#sv - 1], sv[#sv], sv[1], sv[2])
    end
end

-- ─── Update ──────────────────────────────────────────────────────

function CurveLabScene:update(dt)
    self._timer = self._timer + dt
    if self._rotating then
        self._rotAngle = self._rotAngle + dt * 1.2
        if self._rotAngle > 2 * _pi then
            self._rotAngle = self._rotAngle - 2 * _pi
        end
    end
end

-- ─── Draw ────────────────────────────────────────────────────────

function CurveLabScene:draw()
    local W, H  = lg.getDimensions()
    local t     = self._timer
    local curve = self:_getCurve()
    local cx    = W / 2
    local cy    = H * 0.45
    local baseScale = _min(W, H) * 0.15 * self._scale

    -- Background
    setColor(0, 0, 0, 242)
    lg.rectangle("fill", 0, 0, W, H)

    -- Title
    lg.setFont(self._fonts.title)
    setColor((0.6 + 0.2 * _sin(t * 2)) * 255, 76, 255, 255)
    lg.printf(string.format("CURVE LAB  %s  (%d/%d)", curve.name, self._curveIdx, #CURVES),
        0, 12, W, "center")

    -- Formula
    lg.setFont(self._fonts.formula)
    setColor(178, 178, 204, 230)
    lg.printf(curve.formula, 0, 34, W, "center")

    -- Crosshair
    setColor(76, 76, 76, 76)
    lg.line(cx - 20, cy, cx + 20, cy)
    lg.line(cx, cy - 20, cx, cy + 20)

    -- Guide circles: unit circle (white) + typical enemy size (gold)
    lg.setLineWidth(1)
    setColor(255, 255, 255, 25)
    lg.circle("line", cx, cy, baseScale)
    setColor(255, 200, 64, 20)
    lg.circle("line", cx, cy, baseScale * 0.3)

    local verts = self._verts
    if verts and #verts >= 4 then
        -- World -> screen: centroid-based rotation
        local sv   = {}
        local cosR = _cos(self._rotAngle)
        local sinR = _sin(self._rotAngle)
        local ox   = self._centroid.x
        local oy   = self._centroid.y
        for i = 1, #verts, 2 do
            local dx = verts[i]     - ox
            local dy = verts[i + 1] - oy
            sv[#sv + 1] = cx + (dx * cosR - dy * sinR + ox) * baseScale
            sv[#sv + 1] = cy - (dx * sinR + dy * cosR + oy) * baseScale
        end

        local modeName = MODES[self._mode]

        -- Glow pass
        setColor(51, 128, 255, 64)
        lg.setLineWidth(3)
        lg.setPointSize(4)
        _drawLayer(sv, modeName, cx, cy, 4)

        -- Main pass
        if modeName == "fill" then
            setColor(76, 204, 255, 128)
            _drawLayer(sv, "fill", cx, cy, 0)
            setColor(76, 204, 255, 255)
            lg.setLineWidth(1.5)
            if #sv >= 6 then lg.polygon("line", sv) end
        else
            setColor(76, 204, 255, 255)
            lg.setLineWidth(1.5)
            lg.setPointSize(2)
            _drawLayer(sv, modeName, cx, cy, 3)
        end

        -- First-vertex dot (rotation indicator, visible only while rotating)
        if self._rotating and #sv >= 2 then
            setColor(255, 120, 80, 220)
            lg.circle("fill", sv[1], sv[2], 4)
        end

        -- Centroid dot (rotation pivot, always visible)
        local scx = cx + ox * baseScale
        local scy = cy - oy * baseScale
        setColor(255, 220, 60, 200)
        lg.circle("fill", scx, scy, 3)
        setColor(255, 220, 60, 80)
        lg.circle("line", scx, scy, 7)

        lg.setLineWidth(1)
        lg.setPointSize(1)
    end

    -- Info panel (bottom-left)
    lg.setFont(self._fonts.info)
    local infoX = 16
    local infoY = H - 144
    local lineH = 16
    setColor(128, 128, 153, 204)
    lg.print(string.format("mode:     %s",         MODES[self._mode]),                        infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("vertices: %d",         self:_getSteps()),                         infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("scale:    %.1fx",      self._scale),                              infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("rotate:   %s",         self._rotating and "ON" or "OFF"),         infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("angle:    %.2f",       self._rotAngle),                           infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("centroid:(%.2f,%.2f)", self._centroid.x, self._centroid.y),       infoX, infoY)

    -- Hint bar
    lg.setFont(self._fonts.hint)
    setColor(128, 128, 128, (0.4 + 0.15 * _sin(t * 2)) * 255)
    lg.printf("L/R: curve | U/D: verts | M: mode | R: rotate | +/-: scale | ESC: back",
        0, H - 24, W, "center")

    resetColor()
end

-- ─── Input ───────────────────────────────────────────────────────

function CurveLabScene:keypressed(key, scancode)
    if key == "escape" then
        self._sceneStack:pop()
    elseif key == "left" then
        self._curveIdx = (self._curveIdx - 2) % #CURVES + 1
        self._steps = nil
        self:_buildVerts()
    elseif key == "right" then
        self._curveIdx = self._curveIdx % #CURVES + 1
        self._steps = nil
        self:_buildVerts()
    elseif key == "up" then
        self._steps = math.min(self:_getSteps() + 10, 2000)
        self:_buildVerts()
    elseif key == "down" then
        self._steps = math.max(self:_getSteps() - 10, 10)
        self:_buildVerts()
    elseif key == "m" or scancode == "m" then
        self._mode = self._mode % #MODES + 1
    elseif key == "r" or scancode == "r" then
        self._rotating = not self._rotating
    elseif key == "=" or key == "kp+" then
        self._scale = math.min(self._scale + 0.2, 5.0)
    elseif key == "-" or key == "kp-" then
        self._scale = math.max(self._scale - 0.2, 0.2)
    else
        return false
    end
    return true
end

-- macOS IME 한글 2벌식: ㅡ->m, ㄱ->r
local JAMO_TO_KEY = {["ㅡ"] = "m", ["ㄱ"] = "r"}
function CurveLabScene:textinput(text)
    local key = JAMO_TO_KEY[text]
    if key then return self:keypressed(key) end
    return false
end

function CurveLabScene:touchpressed(id, x, y)
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

