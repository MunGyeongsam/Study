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
CurveLabScene.drawBelow   = false

local CURVES = require("03_game.data.curveDefs")
local MODES  = {"line", "points", "circle", "fill"}
local FILTERS = {
    {key = "all", label = "ALL", fn = function(_) return true end},
    {key = "enemy", label = "ENEMY", fn = function(c) return c.enemyFriendly end},
    {key = "closed", label = "CLOSED", fn = function(c) return c.closed end},
    {key = "simple", label = "SIMPLE", fn = function(c) return c.complexity <= 2 end},
    {key = "discontinuous", label = "DISCONT", fn = function(c) return c.discontinuous end},
}

local CURATION_OPTIONS = {
    {key = "enemy", label = "ENEMY", short = "1"},
    {key = "boss", label = "BOSS", short = "2"},
    {key = "both", label = "ENEMY+BOSS", short = "3"},
    {key = "overlay", label = "OVERLAY", short = "4"},
    {key = "bullet", label = "BULLET_CURVE", short = "5"},
}

local CURATION_LOG_FILE = "curve_curation.log"
local CURATION_SUMMARY_FILE = "curve_curation_summary.txt"
local TARGET_NORMALIZED_RADIUS = 1.0

local function _getCurationOption(choiceKey)
    for i = 1, #CURATION_OPTIONS do
        local opt = CURATION_OPTIONS[i]
        if opt.key == choiceKey then return opt end
    end
    return nil
end

local function _joinWithComma(parts)
    if #parts == 0 then return "NONE" end
    return table.concat(parts, ", ")
end

local function _buildRecommendationText(curve)
    if not curve then return "추천: N/A" end

    local tags = {}
    if curve.enemyFriendly then
        tags[#tags + 1] = "Enemy"
    end
    if curve.closed then
        tags[#tags + 1] = "BossShape"
    end
    if curve.discontinuous then
        tags[#tags + 1] = "Overlay/Bullet"
    end
    if curve.complexity == 1 then
        tags[#tags + 1] = "Simple"
    elseif curve.complexity == 3 then
        tags[#tags + 1] = "Complex"
    end

    if curve.family == "rose" or curve.family == "cycloid" or curve.family == "trochoid" then
        tags[#tags + 1] = "PatternStrong"
    end

    return "추천: " .. _joinWithComma(tags)
end

local function _sampleCurveWorld(curve, steps)
    local verts = {}
    if not curve then return verts end

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
        local t0, t1 = curve.tRange[1], curve.tRange[2]
        for i = 0, steps - 1 do
            local t = t0 + (t1 - t0) * i / steps
            local r = curve.fn(t)
            verts[#verts + 1] = r * _cos(t)
            verts[#verts + 1] = r * _sin(t)
        end
    end

    return verts
end

local function _computeBounds(verts)
    if #verts < 2 then
        return {xMin = 0, xMax = 0, yMin = 0, yMax = 0}
    end
    local xMin, xMax = verts[1], verts[1]
    local yMin, yMax = verts[2], verts[2]
    for i = 3, #verts, 2 do
        local x = verts[i]
        local y = verts[i + 1]
        if x < xMin then xMin = x end
        if x > xMax then xMax = x end
        if y < yMin then yMin = y end
        if y > yMax then yMax = y end
    end
    return {xMin = xMin, xMax = xMax, yMin = yMin, yMax = yMax}
end

local function _computeNormalizationData(curve)
    local steps = curve.defaultSteps or 180
    local verts = _sampleCurveWorld(curve, steps)
    if #verts < 2 then
        return {
            centerX = 0, centerY = 0,
            maxRadius = 0,
            scaleToTarget = 1,
            targetRadius = TARGET_NORMALIZED_RADIUS,
            bounds = {xMin = 0, xMax = 0, yMin = 0, yMax = 0},
            normalizedBounds = {xMin = 0, xMax = 0, yMin = 0, yMax = 0},
        }
    end

    local n = #verts / 2
    local sx, sy = 0, 0
    for i = 1, #verts, 2 do
        sx = sx + verts[i]
        sy = sy + verts[i + 1]
    end
    local cx = sx / n
    local cy = sy / n

    local maxR = 0
    local normalized = {}
    local scale = 1

    for i = 1, #verts, 2 do
        local dx = verts[i] - cx
        local dy = verts[i + 1] - cy
        local r = (dx * dx + dy * dy) ^ 0.5
        if r > maxR then maxR = r end
    end

    if maxR > 0 then
        scale = TARGET_NORMALIZED_RADIUS / maxR
    end

    for i = 1, #verts, 2 do
        normalized[#normalized + 1] = (verts[i] - cx) * scale
        normalized[#normalized + 1] = (verts[i + 1] - cy) * scale
    end

    return {
        centerX = cx,
        centerY = cy,
        maxRadius = maxR,
        scaleToTarget = scale,
        targetRadius = TARGET_NORMALIZED_RADIUS,
        bounds = _computeBounds(verts),
        normalizedBounds = _computeBounds(normalized),
    }
end

local function _getCurationPanelLayout(W, H)
    local panelW = 190
    local panelH = 168
    local panelX = W - panelW - 14
    local panelY = H - panelH - 54
    local rowH = 24
    local optionRects = {}
    for i = 1, #CURATION_OPTIONS do
        optionRects[#optionRects + 1] = {
            key = CURATION_OPTIONS[i].key,
            x = panelX + 8,
            y = panelY + 34 + (i - 1) * rowH,
            w = panelW - 16,
            h = rowH - 2,
        }
    end
    return {
        x = panelX,
        y = panelY,
        w = panelW,
        h = panelH,
        optionRects = optionRects,
    }
end

local function _appendFirstPoint(points)
    points[#points + 1] = points[1]
    points[#points + 1] = points[2]
end

-- 화면 좌표 배열을 불연속 구간 기준으로 분리한다.
local function _buildSegments(sv)
    local segments = {}
    if #sv < 4 then return segments end

    local total, edges = 0, 0
    for i = 1, #sv - 2, 2 do
        local dx = sv[i + 2] - sv[i]
        local dy = sv[i + 3] - sv[i + 1]
        total = total + (dx * dx + dy * dy) ^ 0.5
        edges = edges + 1
    end

    local avg = (edges > 0) and (total / edges) or 0
    local threshold = avg * 4
    if threshold < 12 then threshold = 12 end

    local seg = {sv[1], sv[2]}
    for i = 3, #sv, 2 do
        local dx = sv[i] - sv[i - 2]
        local dy = sv[i + 1] - sv[i - 1]
        local dist = (dx * dx + dy * dy) ^ 0.5
        if dist > threshold then
            if #seg >= 4 then segments[#segments + 1] = seg end
            seg = {sv[i], sv[i + 1]}
        else
            seg[#seg + 1] = sv[i]
            seg[#seg + 1] = sv[i + 1]
        end
    end
    if #seg >= 4 then segments[#segments + 1] = seg end
    return segments
end

-- 양 끝점이 충분히 가까우면 닫힌 곡선으로 간주한다.
local function _isClosedLoop(sv)
    if #sv < 6 then return false end
    local total, edges = 0, 0
    for i = 1, #sv - 2, 2 do
        local dx = sv[i + 2] - sv[i]
        local dy = sv[i + 3] - sv[i + 1]
        total = total + (dx * dx + dy * dy) ^ 0.5
        edges = edges + 1
    end
    if edges == 0 then return false end
    local avg = total / edges
    local dx = sv[#sv - 1] - sv[1]
    local dy = sv[#sv] - sv[2]
    local endDist = (dx * dx + dy * dy) ^ 0.5
    return endDist <= avg * 2.2
end

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
        _filterMode = 1,
        _filtered   = CURVES,
        _curationByName = {},
        _curationCount = 0,
        _curationExported = false,
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
    self._filterMode = 1
    self:_rebuildFilter()
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
    return self._filtered[self._curveIdx]
end

function CurveLabScene:_getCurveCount()
    return #self._filtered
end

function CurveLabScene:_rebuildFilter()
    local f = FILTERS[self._filterMode]
    local list = {}
    for i = 1, #CURVES do
        local c = CURVES[i]
        if f.fn(c) then list[#list + 1] = c end
    end
    self._filtered = list
    if #list == 0 then
        self._curveIdx = 1
        self._verts = {}
        self._centroid = {x = 0, y = 0}
        return
    end
    self._curveIdx = ((self._curveIdx - 1) % #list) + 1
end

function CurveLabScene:_getSteps()
    local c = self:_getCurve()
    if not c then return self._steps or 120 end
    return self._steps or c.defaultSteps
end

function CurveLabScene:_appendCurationLog(line)
    logInfo(line)
    local ok = love.filesystem.append(CURATION_LOG_FILE, line .. "\n")
    if not ok then
        logWarn("[CURATION] Failed to append curation log file")
    end
end

function CurveLabScene:_recordCuration(choiceKey)
    local curve = self:_getCurve()
    if not curve then return end

    local opt = _getCurationOption(choiceKey)
    if not opt then return end

    local prev = self._curationByName[curve.name]
    self._curationByName[curve.name] = choiceKey
    if not prev then
        self._curationCount = self._curationCount + 1
    end

    local line = string.format("[CURATION] %s -> %s", curve.name, opt.label)
    self:_appendCurationLog(line)

    if self._curationCount >= #CURVES and not self._curationExported then
        self:_exportCurationSummary()
        self._curationExported = true
    end
end

function CurveLabScene:_exportCurationSummary()
    local groups = {
        enemy = {},
        boss = {},
        both = {},
        overlay = {},
        bullet = {},
        excluded = {},
    }

    local normalizeKeys = {enemy = true, boss = true, both = true}
    local normalizedLines = {}

    for i = 1, #CURVES do
        local curve = CURVES[i]
        local choiceKey = self._curationByName[curve.name]
        if choiceKey and groups[choiceKey] then
            groups[choiceKey][#groups[choiceKey] + 1] = curve.name
            if normalizeKeys[choiceKey] then
                local n = _computeNormalizationData(curve)
                normalizedLines[#normalizedLines + 1] = string.format(
                    "%s | %s | center=(%.4f, %.4f) | maxR=%.4f | scale=%.4f | bounds=(%.4f,%.4f,%.4f,%.4f) | normalizedBounds=(%.4f,%.4f,%.4f,%.4f)",
                    curve.name,
                    choiceKey,
                    n.centerX, n.centerY,
                    n.maxRadius,
                    n.scaleToTarget,
                    n.bounds.xMin, n.bounds.xMax, n.bounds.yMin, n.bounds.yMax,
                    n.normalizedBounds.xMin, n.normalizedBounds.xMax, n.normalizedBounds.yMin, n.normalizedBounds.yMax
                )
            end
        else
            groups.excluded[#groups.excluded + 1] = curve.name
        end
    end

    local usableCount = #groups.enemy + #groups.boss + #groups.both + #groups.overlay + #groups.bullet

    local lines = {
        "# Curve Curation Summary",
        string.format("Curated: %d / %d", self._curationCount, #CURVES),
        string.format("Usable: %d / %d", usableCount, #CURVES),
        string.format("Excluded(Unclassified): %d / %d", #groups.excluded, #CURVES),
        "Rule: Unclassified curves are excluded and considered unusable.",
        "",
        "[USABLE]",
        "- enemy + boss + both + overlay + bullet only",
        "",
        "[GROUP] Enemy",
    }

    for i = 1, #groups.enemy do lines[#lines + 1] = "- " .. groups.enemy[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Boss"
    for i = 1, #groups.boss do lines[#lines + 1] = "- " .. groups.boss[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Enemy+Boss"
    for i = 1, #groups.both do lines[#lines + 1] = "- " .. groups.both[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Overlay"
    for i = 1, #groups.overlay do lines[#lines + 1] = "- " .. groups.overlay[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Bullet Curve"
    for i = 1, #groups.bullet do lines[#lines + 1] = "- " .. groups.bullet[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[EXCLUDED] Unclassified"
    for i = 1, #groups.excluded do lines[#lines + 1] = "- " .. groups.excluded[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[NORMALIZATION] target radius = 1.0 (for enemy/boss/enemy+boss)"
    for i = 1, #normalizedLines do lines[#lines + 1] = normalizedLines[i] end

    local content = table.concat(lines, "\n") .. "\n"
    local ok = love.filesystem.write(CURATION_SUMMARY_FILE, content)
    if ok then
        self:_appendCurationLog("[CURATION] Summary exported -> " .. CURATION_SUMMARY_FILE)
        self:_appendCurationLog(string.format("[CURATION] Usable=%d Excluded=%d", usableCount, #groups.excluded))
    else
        logWarn("[CURATION] Failed to export summary")
    end
end

--- 정점 배열 재계산 + 무게중심 갱신
function CurveLabScene:_buildVerts()
    local curve = self:_getCurve()
    if not curve then
        self._verts = {}
        self._centroid = {x = 0, y = 0}
        return
    end
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

-- 화면 좌표 정점 배열을 modeName에 따라 렌더 (Glow/Main 공용)
local function _drawLayer(sv, segments, closeLoop, modeName, cx, cy, pointR)
    if modeName == "line" then
        for i = 1, #segments do
            local seg = segments[i]
            if #seg >= 4 then
                if closeLoop and #segments == 1 and #seg >= 6 then
                    local closed = {}
                    for j = 1, #seg do closed[j] = seg[j] end
                    _appendFirstPoint(closed)
                    lg.line(closed)
                else
                    lg.line(seg)
                end
            end
        end
    elseif modeName == "points" and #sv >= 2 then
        lg.points(sv)
    elseif modeName == "circle" then
        for i = 1, #sv, 2 do
            lg.circle("fill", sv[i], sv[i + 1], pointR)
        end
    elseif modeName == "fill" and #sv >= 6 then
        -- fill은 닫힌 단일 루프에서만 수행 (불연속 브리지 방지)
        if not (closeLoop and #segments == 1) then return end
        local seg = segments[1]
        for i = 1, #seg - 2, 2 do
            lg.polygon("fill", cx, cy, seg[i], seg[i + 1], seg[i + 2], seg[i + 3])
        end
        lg.polygon("fill", cx, cy, seg[#seg - 1], seg[#seg], seg[1], seg[2])
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
    local curveName = curve and curve.name or "(no curves in filter)"
    lg.printf(string.format("CURVE LAB  %s  (%d/%d | total %d)", curveName, self._curveIdx, self:_getCurveCount(), #CURVES),
        0, 12, W, "center")

    -- Formula
    lg.setFont(self._fonts.formula)
    setColor(178, 178, 204, 230)
    lg.printf(curve and curve.formula or "", 0, 34, W, "center")

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
    if curve and verts and #verts >= 4 then
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

        local segments = _buildSegments(sv)
        local closeLoop = _isClosedLoop(sv)

        local modeName = MODES[self._mode]

        -- Glow pass
        setColor(51, 128, 255, 64)
        lg.setLineWidth(3)
        lg.setPointSize(4)
        _drawLayer(sv, segments, closeLoop, modeName, cx, cy, 4)

        -- Main pass
        if modeName == "fill" then
            setColor(76, 204, 255, 128)
            _drawLayer(sv, segments, closeLoop, "fill", cx, cy, 0)
            setColor(76, 204, 255, 255)
            lg.setLineWidth(1.5)
            if closeLoop and #segments == 1 and #sv >= 6 then
                local outline = {}
                local seg = segments[1]
                for i = 1, #seg do outline[i] = seg[i] end
                _appendFirstPoint(outline)
                lg.line(outline)
            end
        else
            setColor(76, 204, 255, 255)
            lg.setLineWidth(1.5)
            lg.setPointSize(2)
            _drawLayer(sv, segments, closeLoop, modeName, cx, cy, 3)
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
    local infoY = H - 240
    local lineH = 16
    setColor(128, 128, 153, 204)
    lg.print(string.format("filter:   %s",         FILTERS[self._filterMode].label),           infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("visible:  %d",         self:_getCurveCount()),                       infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("mode:     %s",         MODES[self._mode]),                        infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("vertices: %d",         self:_getSteps()),                         infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("scale:    %.1fx",      self._scale),                              infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("rotate:   %s",         self._rotating and "ON" or "OFF"),         infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("angle:    %.2f",       self._rotAngle),                           infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("centroid:(%.2f,%.2f)", self._centroid.x, self._centroid.y),       infoX, infoY); infoY = infoY + lineH
    lg.print("select:   1 enemy  2 boss  3 both", infoX, infoY); infoY = infoY + lineH
    lg.print("          4 overlay 5 bullet", infoX, infoY); infoY = infoY + lineH
    if curve then
        lg.print(string.format("meta:     %s / c%d / %s", curve.family, curve.complexity, curve.enemyFriendly and "enemy" or "non-enemy"), infoX, infoY)
        infoY = infoY + lineH
        lg.print(_buildRecommendationText(curve), infoX, infoY)
        infoY = infoY + lineH
        local pickedKey = self._curationByName[curve.name]
        local picked = _getCurationOption(pickedKey)
        lg.print(string.format("picked:   %s", picked and picked.label or "NONE"), infoX, infoY)
        infoY = infoY + lineH
        lg.print(string.format("curated:  %d / %d", self._curationCount, #CURVES), infoX, infoY)
        infoY = infoY + lineH
        lg.print("note: unclassified = unusable", infoX, infoY)
    end

    -- DNA curation panel (bottom-right)
    local panel = _getCurationPanelLayout(W, H)
    setColor(8, 14, 24, 220)
    lg.rectangle("fill", panel.x, panel.y, panel.w, panel.h, 6, 6)
    setColor(64, 128, 200, 180)
    lg.setLineWidth(1)
    lg.rectangle("line", panel.x, panel.y, panel.w, panel.h, 6, 6)

    lg.setFont(self._fonts.info)
    setColor(188, 216, 255, 240)
    lg.print("DNA CURATION", panel.x + 10, panel.y + 8)

    local pickedKey = curve and self._curationByName[curve.name] or nil
    for i = 1, #CURATION_OPTIONS do
        local opt = CURATION_OPTIONS[i]
        local r = panel.optionRects[i]
        local isPicked = (pickedKey == opt.key)
        if isPicked then
            setColor(48, 142, 255, 168)
            lg.rectangle("fill", r.x, r.y, r.w, r.h, 4, 4)
            setColor(120, 220, 255, 220)
            lg.rectangle("line", r.x, r.y, r.w, r.h, 4, 4)
            setColor(230, 250, 255, 255)
        else
            setColor(116, 136, 170, 220)
        end
        lg.print(string.format("%s) %s", opt.short, opt.label), r.x + 6, r.y + 4)
    end

    setColor(140, 160, 190, 220)
    lg.print("F5: export summary", panel.x + 10, panel.y + panel.h - 20)

    -- Hint bar
    lg.setFont(self._fonts.hint)
    setColor(128, 128, 128, (0.4 + 0.15 * _sin(t * 2)) * 255)
    lg.printf("L/R: curve | TAB: filter | 1:enemy 2:boss 3:both 4:overlay 5:bullet | F5:export | ESC: back | top tap: back",
        0, H - 24, W, "center")

    resetColor()
end

-- ─── Input ───────────────────────────────────────────────────────

function CurveLabScene:keypressed(key, scancode)
    if key == "escape" then
        self._sceneStack:pop()
    elseif key == "tab" then
        self._filterMode = self._filterMode % #FILTERS + 1
        self._steps = nil
        self:_rebuildFilter()
        self:_buildVerts()
    elseif key == "left" then
        local n = self:_getCurveCount()
        if n == 0 then return true end
        self._curveIdx = (self._curveIdx - 2) % n + 1
        self._steps = nil
        self:_buildVerts()
    elseif key == "right" then
        local n = self:_getCurveCount()
        if n == 0 then return true end
        self._curveIdx = self._curveIdx % n + 1
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
    elseif key == "1" or key == "kp1" then
        self:_recordCuration("enemy")
    elseif key == "2" or key == "kp2" then
        self:_recordCuration("boss")
    elseif key == "3" or key == "kp3" then
        self:_recordCuration("both")
    elseif key == "4" or key == "kp4" then
        self:_recordCuration("overlay")
    elseif key == "5" or key == "kp5" then
        self:_recordCuration("bullet")
    elseif key == "f5" then
        self:_exportCurationSummary()
        self._curationExported = true
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
    local W, H = lg.getDimensions()
    local panel = _getCurationPanelLayout(W, H)

    -- Touch selection for DNA curation options.
    if x >= panel.x and x <= panel.x + panel.w and y >= panel.y and y <= panel.y + panel.h then
        for i = 1, #panel.optionRects do
            local r = panel.optionRects[i]
            if x >= r.x and x <= r.x + r.w and y >= r.y and y <= r.y + r.h then
                self:_recordCuration(r.key)
                return true
            end
        end
        return true
    end

    -- Top-edge tap is an explicit back action.
    if y < H * 0.12 then
        self._sceneStack:pop()
    elseif x < W * 0.3 then
        self:keypressed("left")
    elseif x > W * 0.7 then
        self:keypressed("right")
    else
        -- Center tap is consumed intentionally to avoid accidental scene close.
        return true
    end
    return true
end

return CurveLabScene

