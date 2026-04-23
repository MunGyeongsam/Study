-- Render System
-- Transform + Renderable을 가진 엔티티를 그린다
-- draw()에서만 실행되며, update()에서는 스킵된다.
-- Strategy Pattern: 렌더 타입별 함수를 디스패치 테이블로 관리

local System = require("01_core.system")

local basicShapes     = require("03_game.systems.renderers.basicShapes")
local bossRenderers   = require("03_game.systems.renderers.bossRenderers")
local variantOverlays = require("03_game.systems.renderers.variantOverlays")
local curveDefs       = require("03_game.data.curveDefs")

local lg = love.graphics
local _sin = math.sin
local _cos = math.cos
local _max = math.max
local _min = math.min

-- ─── Dispatch Table ──────────────────────────────────────────────
-- fn(x, y, r, renderable, transform)
-- 새 렌더 타입 추가 시 해당 모듈에 함수 추가 + 여기 등록만 하면 됨
local dispatch = {}

-- Basic shapes (12종)
for k, fn in pairs(basicShapes) do
    if type(fn) == "function" then
        dispatch[k] = fn
    end
end

-- Boss renderers (5종)
for k, fn in pairs(bossRenderers) do
    dispatch[k] = fn
end

-- DNA Body 레이어 렌더용 함수 테이블
local shapes_draw = basicShapes.shapes_draw

-- 라디안 변환 캐시
local _rad = math.rad

local CURVE_BY_NAME = {}
for i = 1, #curveDefs do
    local c = curveDefs[i]
    CURVE_BY_NAME[c.name] = c
end

local CURVE_POINT_CACHE = {}          -- raw sampled verts per curveName
local NORMALIZED_CURVE_CACHE = {}     -- center-offset + unit-radius normalized per curveName
local CURVE_RENDER_LOGGED_OK = setmetatable({}, {__mode = "k"})
local CURVE_RENDER_LOGGED_MISSING = setmetatable({}, {__mode = "k"})

local CURVE_OVERLAY_STYLE = {
    scale = 1.45,   -- gallery baseline
    alpha = 120,    -- gallery baseline
    lineWidth = 1.0 -- gallery baseline
}

-- 1 screen pixel = world 몇 단위? (프레임 시작 시 갱신)
local _pxInWorld = 1

local function _sampleCurveWorld(curve, steps)
    local verts = {}
    if not curve then return verts end

    if curve.fn == "custom" then
        return curve.customFn(steps)
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

local function _getCurvePoints(curveName)
    local cached = CURVE_POINT_CACHE[curveName]
    if cached then return cached end

    local curve = CURVE_BY_NAME[curveName]
    if not curve then return nil end
    local steps = curve.defaultSteps or 96
    local verts = _sampleCurveWorld(curve, steps)
    CURVE_POINT_CACHE[curveName] = verts
    return verts
end

--- 정규화된 커브 포인트 캐시 (center offset + scaleToUnitRadius 적용)
--- 한 번만 계산, draw 시 lg.translate/scale로 변환
local function _getNormalizedCurvePoints(curveName, norm)
    local cached = NORMALIZED_CURVE_CACHE[curveName]
    if cached then return cached end

    local verts = _getCurvePoints(curveName)
    if not verts or #verts < 6 then return nil end

    local cox = norm.centerOffset and norm.centerOffset.x or 0
    local coy = norm.centerOffset and norm.centerOffset.y or 0
    local scaleR = norm.scaleToUnitRadius or 1

    local points = {}
    for i = 1, #verts, 2 do
        points[#points + 1] = (verts[i] - cox) * scaleR
        points[#points + 1] = (verts[i + 1] - coy) * scaleR
    end

    NORMALIZED_CURVE_CACHE[curveName] = points
    return points
end

local function _drawCurveOverlay(renderable, x, y, r, entityId)
    local curveName = renderable.curveName
    local norm = renderable.curveNormalization
    if not curveName or not norm then
        if entityId and not CURVE_RENDER_LOGGED_MISSING[renderable] then
            CURVE_RENDER_LOGGED_MISSING[renderable] = true
            logWarn(string.format("[DNA][RENDER] entity:%d curve overlay skipped (curve:%s norm:%s role:%s)",
                entityId,
                curveName or "nil",
                norm and "ok" or "nil",
                renderable.curveRole or "unknown"))
        end
        return
    end

    local verts = _getNormalizedCurvePoints(curveName, norm)
    if not verts or #verts < 6 then
        if entityId and not CURVE_RENDER_LOGGED_MISSING[renderable] then
            CURVE_RENDER_LOGGED_MISSING[renderable] = true
            logWarn(string.format("[DNA][RENDER] entity:%d curve overlay skipped (curve points invalid: %s)",
                entityId,
                curveName))
        end
        return
    end

    if entityId and not CURVE_RENDER_LOGGED_OK[renderable] then
        CURVE_RENDER_LOGGED_OK[renderable] = true
        logInfo(string.format("[DNA][RENDER] entity:%d curve:%s role:%s points:%d", entityId,
            curveName,
            renderable.curveRole or "unknown",
            #verts / 2))
    end

    setColor(120, 200, 255, CURVE_OVERLAY_STYLE.alpha)
    lg.push()
    lg.translate(x, y)
    lg.scale(r)
    lg.setLineWidth(CURVE_OVERLAY_STYLE.lineWidth * _pxInWorld / r)
    lg.line(verts)
    lg.pop()
    lg.setLineWidth(1)
end

--- DNA Body 레이어 배열 렌더 (table 타입일 때)
--- @param layers table Body 레이어 배열 [{shape, mode, scale, rot}, ...]
--- @param x number 월드 X
--- @param y number 월드 Y
--- @param r number 기본 반지름
local function _drawBodyLayers(layers, x, y, r)
    for _, layer in ipairs(layers) do
        local drawFn = shapes_draw[layer.shape]
        if drawFn then
            local lr = r * (layer.scale or 1.0)
            local rot = layer.rot or 0
            if rot ~= 0 then
                lg.push()
                lg.translate(x, y)
                lg.rotate(_rad(rot))
                drawFn(0, 0, lr, layer.mode or "fill")
                lg.pop()
            else
                drawFn(x, y, lr, layer.mode or "fill")
            end
        end
    end
end

-- ─── Main Render Loop ────────────────────────────────────────────
local _abs = math.abs

local RenderSystem = System.new("Render", {"Transform", "Renderable"},
    function(ecs, dt, entities)
        -- 프레임당 1회: 1 screen pixel이 월드 몇 단위인지 계산
        local wx0 = lg.inverseTransformPoint(0, 0)
        local wx1 = lg.inverseTransformPoint(1, 0)
        _pxInWorld = _abs(wx1 - wx0)
        if _pxInWorld < 1e-6 then _pxInWorld = 1 end

        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local renderable = ecs:getComponent(entityId, "Renderable")

            if renderable.visible then
                -- Base color
                local c = renderable.color
                setColor(
                    c[1] * 255,
                    c[2] * 255,
                    c[3] * 255,
                    (c[4] or 1) * 255)

                local x, y = transform.x, transform.y
                local r = renderable.radius

                -- Shape dispatch: string → 기존 경로, table → DNA 레이어
                local rType = renderable.type
                if type(rType) == "table" then
                    _drawBodyLayers(rType, x, y, r)
                    _drawCurveOverlay(renderable, x, y, r, entityId)
                else
                    local fn = dispatch[rType]
                    if fn then
                        fn(x, y, r, renderable, transform)
                    end
                end

                -- Variant overlay
                local vari = renderable.variant
                if vari then
                    local overlay = variantOverlays[vari]
                    if overlay then
                        overlay(x, y, r, renderable, ecs, entityId)
                    end
                end
            end
        end

        resetColor()
    end
)

function RenderSystem.adjustCurveOverlayThickness(delta)
    CURVE_OVERLAY_STYLE.lineWidth = _max(0.5, _min(3.0, CURVE_OVERLAY_STYLE.lineWidth + delta))
    return CURVE_OVERLAY_STYLE.lineWidth
end

function RenderSystem.getCurveOverlayThickness()
    return CURVE_OVERLAY_STYLE.lineWidth
end

return RenderSystem
