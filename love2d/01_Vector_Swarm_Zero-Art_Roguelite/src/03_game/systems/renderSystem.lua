-- Render System
-- Transform + Renderable을 가진 엔티티를 그린다
-- draw()에서만 실행되며, update()에서는 스킵된다.
-- Strategy Pattern: 렌더 타입별 함수를 디스패치 테이블로 관리

local System = require("01_core.system")

local basicShapes     = require("03_game.systems.renderers.basicShapes")
local bossRenderers   = require("03_game.systems.renderers.bossRenderers")
local variantOverlays = require("03_game.systems.renderers.variantOverlays")

-- ─── Dispatch Table ──────────────────────────────────────────────
-- fn(x, y, r, renderable, transform)
-- 새 렌더 타입 추가 시 해당 모듈에 함수 추가 + 여기 등록만 하면 됨
local dispatch = {}

-- Basic shapes (6종)
for k, fn in pairs(basicShapes) do
    dispatch[k] = fn
end

-- Boss renderers (5종)
for k, fn in pairs(bossRenderers) do
    dispatch[k] = fn
end

-- ─── Main Render Loop ────────────────────────────────────────────
local RenderSystem = System.new("Render", {"Transform", "Renderable"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local transform  = ecs:getComponent(entityId, "Transform")
            local renderable = ecs:getComponent(entityId, "Renderable")

            if renderable.visible then
                -- Base color
                setColor(
                    renderable.color[1] * 255,
                    renderable.color[2] * 255,
                    renderable.color[3] * 255,
                    renderable.color[4] * 255)

                local x, y = transform.x, transform.y
                local r = renderable.radius

                -- Shape dispatch
                local fn = dispatch[renderable.type]
                if fn then
                    fn(x, y, r, renderable, transform)
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
