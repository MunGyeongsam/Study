-- ============================================================================
-- Formation Definitions — 적 포메이션(편대) 패턴 정의
-- ============================================================================
--
-- ◆ 이 파일의 역할
--   여러 적이 대형을 이루어 동시에 등장하는 "포메이션" 패턴을 정의한다.
--   stageManager._spawnWave()가 확률적으로 포메이션 스폰을 시도할 때 사용.
--
-- ◆ 포메이션 구조
--   {
--     name       = "wedge",          -- 포메이션 이름 (로그용)
--     types      = {"bit"},          -- 사용하는 적 타입 (1개 또는 escort처럼 2개)
--     tier       = 1,                -- 등장 티어 (1=Stage 4+, 2=Stage 6+, 3=Stage 9+)
--     getOffsets = function()        -- {dx, dy} 배열 반환 (앵커 기준 상대 좌표)
--       return {{dx=0, dy=0}, ...}
--     end,
--     spawnMode  = "sides",          -- (선택) "sides"면 X축 확산, 기본은 상단 스폰
--   }
--
-- ◆ 새 포메이션 추가 방법
--   1. FORMATION_DEFS 배열 끝에 위 구조로 추가
--   2. tier 값으로 등장 시작 스테이지 제어:
--      tier 1 → Stage 4+
--      tier 2 → Stage 6+
--      tier 3 → Stage 9+
--   3. types에 들어간 적 타입이 해당 스테이지의 적 풀에 없으면 스킵됨
--      → stageData.lua의 STAGE_DEFS.types 또는 테마 풀 확인
--
-- ◆ getOffsets()가 함수인 이유
--   wedge/escort처럼 랜덤 수(3~5명, 4~6명)를 포함하는 포메이션이 있어서
--   매 스폰마다 다른 배치를 생성한다. 고정 배치면 단순 테이블로도 충분.
--
-- ◆ 의존 관계
--   이 파일은 어떤 모듈도 require 하지 않는다 (순수 데이터 + math만 사용).
--   stageManager.lua가 이 파일을 require 한다.
-- ============================================================================

local _min    = math.min
local _floor  = math.floor
local _random = math.random
local _cos    = math.cos
local _sin    = math.sin
local _pi     = math.pi

local M = {}

-- ─── Formation Patterns ──────────────────────────────────────────

M.DEFS = {
    -- ① Wedge (V자 대형): Bit 떼가 V자로 돌진
    --   tier 1 — Stage 4부터. 가장 기본적인 포메이션.
    {
        name  = "wedge",
        types = {"bit"},
        tier  = 1,
        getOffsets = function()
            local spacing = 0.35
            local offsets = {}
            local count = _random(7, 9)
            offsets[1] = {dx = 0, dy = 0}  -- leader center
            for i = 2, count do
                local row = _floor((i) / 2)
                local side = (i % 2 == 0) and 1 or -1
                offsets[i] = {dx = side * row * spacing, dy = row * spacing * 0.7}
            end
            return offsets
        end,
    },

    -- ② Pincer (집게 공격): Vector 2기가 좌우에서 동시 돌진
    --   tier 2 — Stage 6부터. spawnMode="sides"로 X축 확산.
    {
        name  = "pincer",
        types = {"vector"},
        tier  = 2,
        getOffsets = function()
            return {
                {dx = -4.0, dy = 0},     -- left
                {dx =  4.0, dy = 0},     -- right
            }
        end,
        spawnMode = "sides",
    },

    -- ③ Triangle Turrets: Node 3기가 삼각 대형으로 탄막
    --   tier 2 — Stage 6부터.
    {
        name  = "triangle",
        types = {"node"},
        tier  = 2,
        getOffsets = function()
            local r = 1.8
            local offsets = {}
            for i = 0, 2 do
                local angle = _pi / 2 + i * (2 * _pi / 3)
                offsets[i + 1] = {dx = _cos(angle) * r, dy = _sin(angle) * r}
            end
            return offsets
        end,
    },

    -- ④ Escort (호위 편대): 중앙 탱크(matrix/loop) + Bit 호위병
    --   tier 3 — Stage 9부터. types[1]=중앙, types[2]=호위.
    {
        name  = "escort",
        types = {"matrix", "bit"},
        tier  = 3,
        getOffsets = function()
            local guardCount = _random(4, 6)
            local offsets = {{dx = 0, dy = 0}}  -- center (tank type)
            for i = 1, guardCount do
                local angle = (i - 1) * (2 * _pi / guardCount)
                offsets[i + 1] = {dx = _cos(angle) * 0.8, dy = _sin(angle) * 0.8}
            end
            return offsets
        end,
    },

    -- ⑤ Spiral Array: Loop 다수가 호 위에 배치 → 나선탄 겹침
    --   tier 3 — Stage 9부터.
    {
        name  = "spiral_array",
        types = {"loop"},
        tier  = 3,
        getOffsets = function()
            local count = _random(3, 4)
            local r = 2.0
            local offsets = {}
            for i = 1, count do
                local angle = (i - 1) * (2 * _pi / count) + _pi / 4
                offsets[i] = {dx = _cos(angle) * r, dy = _sin(angle) * r}
            end
            return offsets
        end,
    },
}

-- ─── Availability & Chance ───────────────────────────────────────

--- 해당 스테이지에서 사용 가능한 포메이션 목록 반환.
--- Stage 1~3은 nil (포메이션 없음).
--- @return table|nil
function M.getAvailable(stage)
    local maxTier
    if stage <= 3 then return nil end
    if stage <= 5 then maxTier = 1
    elseif stage <= 8 then maxTier = 2
    else maxTier = 3 end

    local available = {}
    for _, f in ipairs(M.DEFS) do
        if f.tier <= maxTier then
            available[#available + 1] = f
        end
    end
    return #available > 0 and available or nil
end

--- 해당 스테이지에서 포메이션 스폰 확률 (0~0.6).
--- Stage 4부터 20%에서 시작, 스테이지당 +5%, 최대 60%.
function M.getChance(stage)
    if stage <= 3 then return 0 end
    return _min(0.6, 0.2 + (stage - 4) * 0.05)
end

return M
