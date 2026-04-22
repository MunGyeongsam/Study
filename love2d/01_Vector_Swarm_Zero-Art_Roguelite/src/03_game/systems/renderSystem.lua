-- Render System
-- Transform + Renderable을 가진 엔티티를 그린다
-- draw()에서만 실행되며, update()에서는 스킵된다.
-- Strategy Pattern: 렌더 타입별 함수를 디스패치 테이블로 관리

local System = require("01_core.system")

local basicShapes     = require("03_game.systems.renderers.basicShapes")
local bossRenderers   = require("03_game.systems.renderers.bossRenderers")
local variantOverlays = require("03_game.systems.renderers.variantOverlays")

local lg = love.graphics

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
local RenderSystem = System.new("Render", {"Transform", "Renderable"},
    function(ecs, dt, entities)
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

return RenderSystem
