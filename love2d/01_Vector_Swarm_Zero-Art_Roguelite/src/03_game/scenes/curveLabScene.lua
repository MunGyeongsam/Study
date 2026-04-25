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
local ShapeDefs = require("03_game.data.shapeDefs")
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
    {key = "overlay", label = "OVERLAY", short = "3"},
    {key = "bullet", label = "BULLET_CURVE", short = "4"},
}

local CURATION_LOG_FILE = "curve_curation.log"
local CURATION_SUMMARY_FILE = "curve_curation_summary.txt"
local CURATION_LUA_FILE = "shapeDefs_generated.lua"
local TARGET_NORMALIZED_RADIUS = 1.0

local GROUP_COLORS = {
    enemy    = {76, 204, 76},
    boss     = {255, 76, 76},
    overlay  = {76, 204, 255},
    bullet   = {204, 76, 255},
    excluded = {100, 100, 100},
    none     = {60, 60, 60},
}

local function _layoutRow(defs, y, W, h, gap)
    local totalW = -gap
    for i = 1, #defs do totalW = totalW + defs[i].w + gap end
    local x = math.floor((W - totalW) / 2)
    local rects = {}
    for i = 1, #defs do
        rects[i] = {id = defs[i].id, x = x, y = y, w = defs[i].w, h = h}
        x = x + defs[i].w + gap
    end
    return rects
end

--- 태그셋을 대문자 콤마 문자열로 (예: "ENEMY, BOSS")
local function _tagsToString(tags)
    if not tags then return "NONE" end
    local parts = {}
    -- 정렬된 순서로 출력
    local order = {"enemy", "boss", "overlay", "bullet"}
    for _, k in ipairs(order) do
        if tags[k] then parts[#parts + 1] = string.upper(k) end
    end
    if #parts == 0 then return "NONE" end
    return table.concat(parts, ", ")
end

--- 태그셋의 첫 번째 태그 색상 반환
local function _tagsColor(tags)
    if not tags then return GROUP_COLORS.none end
    local order = {"enemy", "boss", "overlay", "bullet"}
    for _, k in ipairs(order) do
        if tags[k] then return GROUP_COLORS[k] end
    end
    return GROUP_COLORS.none
end

--- 태그셋에 하나라도 태그가 있는지
local function _hasTags(tags)
    if not tags then return false end
    for _ in pairs(tags) do return true end
    return false
end

local function _joinWithComma(parts)
    if #parts == 0 then return "NONE" end
    return table.concat(parts, ", ")
end

local function _buildRecommendationText(curve)
    if not curve then return "recommand: N/A" end

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
    local panelW = 200
    local panelH = 146
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
        _rotAngleByName = {},
        _directionalByName = {},
        _centroid   = {x = 0, y = 0},
        _filterMode = 1,
        _filtered   = CURVES,
        _normalizedView = false,
        _normData   = nil,
        _normSource = nil,
        _groupTags  = nil,
        _toolbarBtns = {},
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
    self._rotAngleByName = {}
    self._directionalByName = {}
    self._centroid = {x = 0, y = 0}
    self._steps    = nil
    self._filterMode = 1
    self._normalizedView = false
    self._normData = nil
    self._normSource = nil
    self._groupTags = nil
    self._toolbarBtns = {}

    -- Sync curation from shapeDefs tags (multi-tag)
    self._curationByName = {}
    self._curationCount = 0
    for i = 1, #CURVES do
        local tags = ShapeDefs.getTags(CURVES[i].name)
        if _hasTags(tags) then
            -- deep copy tag set
            local copy = {}
            for k, v in pairs(tags) do copy[k] = v end
            self._curationByName[CURVES[i].name] = copy
            self._curationCount = self._curationCount + 1
        end
    end
    self._curationExported = false

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

function CurveLabScene:_recordCuration(tagKey)
    local curve = self:_getCurve()
    if not curve then return end

    local tags = self._curationByName[curve.name]
    local hadTags = _hasTags(tags)

    if not tags then
        tags = {}
        self._curationByName[curve.name] = tags
    end

    -- Toggle the tag
    if tags[tagKey] then
        tags[tagKey] = nil
    else
        tags[tagKey] = true
    end

    local hasTags = _hasTags(tags)
    if not hadTags and hasTags then
        self._curationCount = self._curationCount + 1
    elseif hadTags and not hasTags then
        self._curationCount = self._curationCount - 1
    end

    local line = string.format("[CURATION] %s -> %s", curve.name, _tagsToString(tags))
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
        overlay = {},
        bullet = {},
        excluded = {},
    }

    local normalizeKeys = {enemy = true, boss = true}
    local normalizedLines = {}

    for i = 1, #CURVES do
        local curve = CURVES[i]
        local tags = self._curationByName[curve.name]
        if _hasTags(tags) then
            for tagKey, _ in pairs(tags) do
                if groups[tagKey] then
                    groups[tagKey][#groups[tagKey] + 1] = curve.name
                end
                if normalizeKeys[tagKey] then
                    -- avoid duplicate normalization lines
                    local already = false
                    for j = 1, #normalizedLines do
                        if normalizedLines[j]:sub(1, #curve.name) == curve.name then
                            already = true; break
                        end
                    end
                    if not already then
                        local n = _computeNormalizationData(curve)
                        normalizedLines[#normalizedLines + 1] = string.format(
                            "%s | %s | center=(%.4f, %.4f) | maxR=%.4f | scale=%.4f | bounds=(%.4f,%.4f,%.4f,%.4f) | normalizedBounds=(%.4f,%.4f,%.4f,%.4f)",
                            curve.name,
                            _tagsToString(tags),
                            n.centerX, n.centerY,
                            n.maxRadius,
                            n.scaleToTarget,
                            n.bounds.xMin, n.bounds.xMax, n.bounds.yMin, n.bounds.yMax,
                            n.normalizedBounds.xMin, n.normalizedBounds.xMax, n.normalizedBounds.yMin, n.normalizedBounds.yMax
                        )
                    end
                end
            end
        else
            groups.excluded[#groups.excluded + 1] = curve.name
        end
    end

    local usableCount = 0
    local counted = {}
    for _, gKey in ipairs({"enemy", "boss", "overlay", "bullet"}) do
        for _, name in ipairs(groups[gKey]) do
            if not counted[name] then
                counted[name] = true
                usableCount = usableCount + 1
            end
        end
    end

    local lines = {
        "# Curve Curation Summary (Multi-Tag)",
        string.format("Curated: %d / %d", self._curationCount, #CURVES),
        string.format("Usable: %d / %d", usableCount, #CURVES),
        string.format("Excluded(Untagged): %d / %d", #groups.excluded, #CURVES),
        "Rule: Curves with no tags are excluded and considered unusable.",
        "",
        "[USABLE]",
        "- enemy + boss + overlay + bullet (multi-tag possible)",
        "",
        "[GROUP] Enemy",
    }

    for i = 1, #groups.enemy do lines[#lines + 1] = "- " .. groups.enemy[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Boss"
    for i = 1, #groups.boss do lines[#lines + 1] = "- " .. groups.boss[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Overlay"
    for i = 1, #groups.overlay do lines[#lines + 1] = "- " .. groups.overlay[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[GROUP] Bullet Curve"
    for i = 1, #groups.bullet do lines[#lines + 1] = "- " .. groups.bullet[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[EXCLUDED] Untagged"
    for i = 1, #groups.excluded do lines[#lines + 1] = "- " .. groups.excluded[i] end
    lines[#lines + 1] = ""
    lines[#lines + 1] = "[NORMALIZATION] target radius = 1.0 (for enemy/boss)"
    for i = 1, #normalizedLines do lines[#lines + 1] = normalizedLines[i] end

    local content = table.concat(lines, "\n") .. "\n"
    local ok = love.filesystem.write(CURATION_SUMMARY_FILE, content)
    if ok then
        self:_appendCurationLog("[CURATION] Summary exported -> " .. CURATION_SUMMARY_FILE)
        self:_appendCurationLog(string.format("[CURATION] Usable=%d Excluded=%d", usableCount, #groups.excluded))
    else
        logWarn("[CURATION] Failed to export summary")
    end

    -- Generate shapeDefs Lua source
    self:_exportShapeDefsLua(groups)
end

function CurveLabScene:_exportShapeDefsLua(groups)
    local L = {}
    local function w(s) L[#L + 1] = s end
    local function wf(...) L[#L + 1] = string.format(...) end

    w("-- shapeDefs.lua")
    w("-- Auto-generated by CurveLab export.")
    w("-- Unclassified curves are intentionally excluded.")
    w("")
    w("local M = {}")
    w("")
    wf("M.targetRadius = %.1f", TARGET_NORMALIZED_RADIUS)
    w("")
    w("M.groups = {")

    local groupOrder = {"enemy", "boss", "overlay", "bullet", "excluded"}
    for _, gKey in ipairs(groupOrder) do
        local list = groups[gKey]
        if list and #list > 0 then
            wf("    %s = {", gKey)
            for i = 1, #list do
                wf("        %q,", list[i])
            end
            w("    },")
        end
    end
    w("}")
    w("")

    -- M.usable
    w("M.usable = {}")
    for _, gKey in ipairs({"enemy", "boss", "overlay", "bullet"}) do
        if groups[gKey] and #groups[gKey] > 0 then
            wf("for i = 1, #M.groups.%s do M.usable[M.groups.%s[i]] = true end", gKey, gKey)
        end
    end
    w("")

    -- M.normalized (enemy/boss curves)
    w("M.normalized = {")
    local normalizeKeys = {enemy = true, boss = true}
    local normalized = {}  -- avoid duplicates
    for i = 1, #CURVES do
        local curve = CURVES[i]
        local tags = self._curationByName[curve.name]
        if _hasTags(tags) and not normalized[curve.name] then
            local needNorm = false
            for k, _ in pairs(tags) do
                if normalizeKeys[k] then needNorm = true; break end
            end
            if needNorm then
                normalized[curve.name] = true
                local n = _computeNormalizationData(curve)
                local usage = _tagsToString(tags):lower()
                wf("    [%q] = {", curve.name)
                wf("        usage = %q,", usage)
                wf("        centerOffset = { x = %.4f, y = %.4f },", n.centerX, n.centerY)
                wf("        maxRadius = %.4f,", n.maxRadius)
                wf("        scaleToUnitRadius = %.4f,", n.scaleToTarget)
                wf("        bounds = { xMin = %.4f, xMax = %.4f, yMin = %.4f, yMax = %.4f },",
                    n.bounds.xMin, n.bounds.xMax, n.bounds.yMin, n.bounds.yMax)
                wf("        normalizedBounds = { xMin = %.4f, xMax = %.4f, yMin = %.4f, yMax = %.4f },",
                    n.normalizedBounds.xMin, n.normalizedBounds.xMax, n.normalizedBounds.yMin, n.normalizedBounds.yMax)
                w("    },")
            end
        end
    end
    w("}")
    w("")

    -- Reverse lookup + API
    w("-- Reverse lookup: curveName → tag set {enemy=true, boss=true, ...}")
    w("M._tagLookup = {}")
    w("for groupName, list in pairs(M.groups) do")
    w("    for i = 1, #list do")
    w("        local name = list[i]")
    w("        if not M._tagLookup[name] then")
    w("            M._tagLookup[name] = {}")
    w("        end")
    w("        M._tagLookup[name][groupName] = true")
    w("    end")
    w("end")
    w("")
    w("function M.isUsable(curveName)")
    w("    return M.usable[curveName] == true")
    w("end")
    w("")
    w("--- Returns tag set table (e.g. {enemy=true, boss=true}) or empty table.")
    w("function M.getTags(curveName)")
    w("    return M._tagLookup[curveName] or {}")
    w("end")
    w("")
    w("--- Backward compat: returns first tag name or \"none\".")
    w("function M.getGroup(curveName)")
    w("    local tags = M._tagLookup[curveName]")
    w("    if not tags then return \"none\" end")
    w("    for k, _ in pairs(tags) do return k end")
    w("    return \"none\"")
    w("end")
    w("")
    w("function M.getNormalization(curveName)")
    w("    return M.normalized[curveName]")
    w("end")
    w("")
    w("return M")

    local luaContent = table.concat(L, "\n") .. "\n"
    local ok = love.filesystem.write(CURATION_LUA_FILE, luaContent)
    if ok then
        local savePath = love.filesystem.getSaveDirectory()
        self:_appendCurationLog(string.format("[CURATION] Lua export -> %s/%s", savePath, CURATION_LUA_FILE))
    else
        logWarn("[CURATION] Failed to export Lua file")
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
    self:_updateCurveInfo()
end

function CurveLabScene:_updateCurveInfo()
    local curve = self:_getCurve()
    if not curve then
        self._groupTags = nil
        self._normData = nil
        self._normSource = nil
        return
    end
    self._groupTags = ShapeDefs.getTags(curve.name)
    local stored = ShapeDefs.getNormalization(curve.name)
    if stored then
        self._normData = stored
        self._normSource = "stored"
    else
        local computed = _computeNormalizationData(curve)
        self._normData = {
            centerOffset = { x = computed.centerX, y = computed.centerY },
            maxRadius = computed.maxRadius,
            scaleToUnitRadius = computed.scaleToTarget,
            bounds = computed.bounds,
            normalizedBounds = computed.normalizedBounds,
        }
        self._normSource = "computed"
    end
end

function CurveLabScene:_savePerCurveState()
    local curve = self:_getCurve()
    if not curve then return end
    self._rotAngleByName[curve.name] = self._rotAngle
end

function CurveLabScene:_loadPerCurveState()
    local curve = self:_getCurve()
    if not curve then return end
    self._rotAngle = self._rotAngleByName[curve.name] or 0
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

    -- Category badge + NORM indicator
    if curve then
        local gc = _tagsColor(self._groupTags)
        setColor(gc[1], gc[2], gc[3], 230)
        local badge = _tagsToString(self._groupTags)
        if self._normalizedView then badge = badge .. "  |  NORM" end
        lg.setFont(self._fonts.info)
        lg.printf(badge, 0, 52, W, "center")
    end

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
        -- World -> screen: centroid-based rotation (+ optional normalization)
        local sv   = {}
        local cosR = _cos(self._rotAngle)
        local sinR = _sin(self._rotAngle)
        local ox   = self._centroid.x
        local oy   = self._centroid.y
        if self._normalizedView and self._normData then
            local co = self._normData.centerOffset
            local s  = self._normData.scaleToUnitRadius
            for i = 1, #verts, 2 do
                local nx = (verts[i] - co.x) * s
                local ny = (verts[i + 1] - co.y) * s
                sv[#sv + 1] = cx + (nx * cosR - ny * sinR) * baseScale
                sv[#sv + 1] = cy - (nx * sinR + ny * cosR) * baseScale
            end
        else
            for i = 1, #verts, 2 do
                local dx = verts[i]     - ox
                local dy = verts[i + 1] - oy
                sv[#sv + 1] = cx + (dx * cosR - dy * sinR + ox) * baseScale
                sv[#sv + 1] = cy - (dx * sinR + dy * cosR + oy) * baseScale
            end
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
        local scx, scy
        if self._normalizedView and self._normData then
            scx = cx
            scy = cy
        else
            scx = cx + ox * baseScale
            scy = cy - oy * baseScale
        end
        setColor(255, 220, 60, 200)
        lg.circle("fill", scx, scy, 3)
        setColor(255, 220, 60, 80)
        lg.circle("line", scx, scy, 7)

        lg.setLineWidth(1)
        lg.setPointSize(1)
    end

    -- ─── Right-side Toolbar (vertical column) ─────────────────────
    do
        local btnW = 40
        local pairW = btnW * 2 + 4
        local btnH = 26
        local btnGap = 4
        local colX = W - pairW - 14
        local curY = 70
        local btnFont = self._fonts.hint
        lg.setFont(btnFont)

        self._toolbarBtns = {}
        local isDir = curve and self._directionalByName[curve.name] or false

        local rows = {
            {{id = "prev", w = btnW}, {id = "next", w = btnW}},
            {{id = "filter", w = pairW}},
            {{id = "mode", w = pairW}},
            {{id = "norm", w = pairW}},
            {{id = "rotate", w = pairW}},
            {{id = "directional", w = pairW}},
            {{id = "rot_ccw", w = btnW}, {id = "rot_cw", w = btnW}},
            {{id = "scale_dn", w = btnW}, {id = "scale_up", w = btnW}},
            {{id = "export", w = pairW}},
        }

        local labels = {
            prev        = "<<",
            next        = ">>",
            filter      = FILTERS[self._filterMode].label,
            mode        = MODES[self._mode],
            norm        = self._normalizedView and "NORM *" or "NORM",
            rotate      = self._rotating and "ROT *" or "ROT",
            directional = isDir and "DIR *" or "DIR",
            rot_ccw     = "-90",
            rot_cw      = "+90",
            scale_dn    = "-",
            scale_up    = "+",
            export      = "EXPORT",
        }

        local actives = {
            norm        = self._normalizedView,
            rotate      = self._rotating,
            directional = isDir,
        }

        for _, row in ipairs(rows) do
            local rx = colX
            for _, def in ipairs(row) do
                local b = {id = def.id, x = rx, y = curY, w = def.w, h = btnH}
                local active = actives[b.id] or false
                local label = labels[b.id] or b.id

                if active then
                    setColor(48, 142, 255, 200)
                else
                    setColor(24, 36, 56, 220)
                end
                lg.rectangle("fill", b.x, b.y, b.w, b.h, 4, 4)

                if active then
                    setColor(120, 220, 255, 220)
                else
                    setColor(80, 140, 220, 160)
                end
                lg.rectangle("line", b.x, b.y, b.w, b.h, 4, 4)

                setColor(220, 240, 255, active and 255 or 180)
                lg.printf(label, b.x, b.y + (b.h - btnFont:getHeight()) / 2, b.w, "center")

                self._toolbarBtns[#self._toolbarBtns + 1] = b
                rx = rx + def.w + 4
            end
            curY = curY + btnH + btnGap
        end
    end

    -- ─── Shortcut Legend (top-left) ──────────────────────────────
    do
        lg.setFont(self._fonts.hint)
        setColor(100, 110, 130, 160)
        local lx, ly, lh = 10, 70, 13
        lg.print("L/R  curve",   lx, ly); ly = ly + lh
        lg.print("TAB  filter",  lx, ly); ly = ly + lh
        lg.print("M    mode",    lx, ly); ly = ly + lh
        lg.print("N    norm",    lx, ly); ly = ly + lh
        lg.print("R    rotate",  lx, ly); ly = ly + lh
        lg.print("D    direct",  lx, ly); ly = ly + lh
        lg.print("Q/E  rot 90",  lx, ly); ly = ly + lh
        lg.print("+/-  scale",   lx, ly); ly = ly + lh
        lg.print("U/D  verts",   lx, ly); ly = ly + lh
        lg.print("1-4  curation", lx, ly); ly = ly + lh
        lg.print("ESC  back",    lx, ly)
    end

    -- Info panel (bottom-left)
    lg.setFont(self._fonts.info)
    local infoX = 16
    local infoY = H - 260
    local lineH = 16
    do
        local gc = _tagsColor(self._groupTags)
        setColor(gc[1], gc[2], gc[3], 230)
        lg.print(string.format("group:    %s", _tagsToString(self._groupTags)), infoX, infoY); infoY = infoY + lineH
    end
    setColor(128, 128, 153, 204)
    lg.print(string.format("norm:     %s", self._normSource or "none"), infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("filter:   %s", FILTERS[self._filterMode].label), infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("visible:  %d", self:_getCurveCount()), infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("mode:     %s", MODES[self._mode]), infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("verts:    %d", self:_getSteps()), infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("scale:    %.1fx", self._scale), infoX, infoY); infoY = infoY + lineH
    lg.print(string.format("angle:    %.0f deg", math.deg(self._rotAngle)), infoX, infoY); infoY = infoY + lineH
    do
        local isDir = curve and self._directionalByName[curve.name] or false
        if isDir then setColor(255, 180, 80, 230) end
        lg.print(string.format("direct:   %s", isDir and "YES" or "no"), infoX, infoY); infoY = infoY + lineH
        setColor(128, 128, 153, 204)
    end
    lg.print(string.format("centroid:(%.2f,%.2f)", self._centroid.x, self._centroid.y), infoX, infoY); infoY = infoY + lineH
    if self._normData then
        local nd = self._normData
        local co = nd.centerOffset
        setColor(128, 153, 128, 204)
        lg.print(string.format("maxR:     %.4f", nd.maxRadius), infoX, infoY); infoY = infoY + lineH
        lg.print(string.format("scale1R:  %.4f", nd.scaleToUnitRadius), infoX, infoY); infoY = infoY + lineH
        lg.print(string.format("offset:  (%.4f,%.4f)", co.x, co.y), infoX, infoY); infoY = infoY + lineH
    end
    setColor(128, 128, 153, 204)
    if curve then
        lg.print(string.format("meta:     %s / c%d / %s", curve.family, curve.complexity, curve.enemyFriendly and "enemy" or "non-enemy"), infoX, infoY)
        infoY = infoY + lineH
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

    local pickedTags = curve and self._curationByName[curve.name] or nil
    for i = 1, #CURATION_OPTIONS do
        local opt = CURATION_OPTIONS[i]
        local r = panel.optionRects[i]
        local isPicked = pickedTags and pickedTags[opt.key] or false
        if isPicked then
            setColor(48, 142, 255, 168)
            lg.rectangle("fill", r.x, r.y, r.w, r.h, 4, 4)
            setColor(120, 220, 255, 220)
            lg.rectangle("line", r.x, r.y, r.w, r.h, 4, 4)
            setColor(230, 250, 255, 255)
        else
            setColor(116, 136, 170, 220)
        end
        lg.print(string.format("%s) %s %s", opt.short, isPicked and "[x]" or "[ ]", opt.label), r.x + 6, r.y + 4)
    end

    -- Hint bar
    lg.setFont(self._fonts.hint)
    setColor(128, 128, 128, (0.4 + 0.15 * _sin(t * 2)) * 255)
    lg.printf(string.format("curated: %d/%d | ESC: back", self._curationCount, #CURVES),
        0, H - 24, W, "center")

    resetColor()
end

-- ─── Input ───────────────────────────────────────────────────────

function CurveLabScene:keypressed(key, scancode)
    if key == "escape" then
        self._sceneStack:pop()
    elseif key == "tab" then
        self:_savePerCurveState()
        self._filterMode = self._filterMode % #FILTERS + 1
        self._steps = nil
        self:_rebuildFilter()
        self:_buildVerts()
        self:_loadPerCurveState()
    elseif key == "left" then
        local n = self:_getCurveCount()
        if n == 0 then return true end
        self:_savePerCurveState()
        self._curveIdx = (self._curveIdx - 2) % n + 1
        self._steps = nil
        self:_buildVerts()
        self:_loadPerCurveState()
    elseif key == "right" then
        local n = self:_getCurveCount()
        if n == 0 then return true end
        self:_savePerCurveState()
        self._curveIdx = self._curveIdx % n + 1
        self._steps = nil
        self:_buildVerts()
        self:_loadPerCurveState()
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
    elseif key == "n" or scancode == "n" then
        self._normalizedView = not self._normalizedView
    elseif key == "d" or scancode == "d" then
        local curve = self:_getCurve()
        if curve then
            self._directionalByName[curve.name] = not self._directionalByName[curve.name]
        end
    elseif key == "q" or scancode == "q" then
        self._rotAngle = self._rotAngle - _pi / 2
        if self._rotAngle < 0 then self._rotAngle = self._rotAngle + 2 * _pi end
    elseif key == "e" or scancode == "e" then
        self._rotAngle = self._rotAngle + _pi / 2
        if self._rotAngle >= 2 * _pi then self._rotAngle = self._rotAngle - 2 * _pi end
    elseif key == "=" or key == "kp+" then
        self._scale = math.min(self._scale + 0.2, 5.0)
    elseif key == "-" or key == "kp-" then
        self._scale = math.max(self._scale - 0.2, 0.2)
    elseif key == "1" or key == "kp1" then
        self:_recordCuration("enemy")
    elseif key == "2" or key == "kp2" then
        self:_recordCuration("boss")
    elseif key == "3" or key == "kp3" then
        self:_recordCuration("overlay")
    elseif key == "4" or key == "kp4" then
        self:_recordCuration("bullet")
    elseif key == "f5" then
        self:_exportCurationSummary()
        self._curationExported = true
    else
        return false
    end
    return true
end

-- macOS IME 한글 2벌식: ㅡ->m, ㄱ->r, ㅜ->n, ㅂ->q, ㄷ->e
local JAMO_TO_KEY = {["ㅡ"] = "m", ["ㄱ"] = "r", ["ㅜ"] = "n", ["ㅂ"] = "q", ["ㄷ"] = "e", ["ㅇ"] = "d"}
function CurveLabScene:textinput(text)
    local key = JAMO_TO_KEY[text]
    if key then return self:keypressed(key) end
    return false
end

function CurveLabScene:_handleToolbarAction(id)
    if id == "prev" then
        self:keypressed("left")
    elseif id == "next" then
        self:keypressed("right")
    elseif id == "filter" then
        self:keypressed("tab")
    elseif id == "mode" then
        self:keypressed("m")
    elseif id == "norm" then
        self._normalizedView = not self._normalizedView
    elseif id == "rotate" then
        self._rotating = not self._rotating
    elseif id == "directional" then
        local curve = self:_getCurve()
        if curve then
            self._directionalByName[curve.name] = not self._directionalByName[curve.name]
        end
    elseif id == "rot_ccw" then
        self._rotAngle = self._rotAngle - _pi / 2
        if self._rotAngle < 0 then self._rotAngle = self._rotAngle + 2 * _pi end
    elseif id == "rot_cw" then
        self._rotAngle = self._rotAngle + _pi / 2
        if self._rotAngle >= 2 * _pi then self._rotAngle = self._rotAngle - 2 * _pi end
    elseif id == "scale_dn" then
        self._scale = math.max(self._scale - 0.2, 0.2)
    elseif id == "scale_up" then
        self._scale = math.min(self._scale + 0.2, 5.0)
    elseif id == "export" then
        self:_exportCurationSummary()
        self._curationExported = true
    end
end

function CurveLabScene:touchpressed(id, x, y)
    local W, H = lg.getDimensions()

    -- Toolbar buttons
    for i = 1, #self._toolbarBtns do
        local b = self._toolbarBtns[i]
        if x >= b.x and x <= b.x + b.w and y >= b.y and y <= b.y + b.h then
            self:_handleToolbarAction(b.id)
            return true
        end
    end

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

