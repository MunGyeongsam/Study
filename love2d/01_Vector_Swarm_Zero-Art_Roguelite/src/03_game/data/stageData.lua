-- ============================================================================
-- Stage Data — 스테이지 진행에 필요한 모든 "데이터 테이블"과 조회 함수
-- ============================================================================
--
-- ◆ 이 파일의 역할
--   stageManager.lua의 "로직"과 분리된 순수 데이터 + 조회 유틸리티.
--   게임 밸런스를 바꾸고 싶으면 여기 숫자만 수정하면 된다.
--
-- ◆ 수정 가이드 (Copilot / 사람 공용)
--
--   [보스 추가]
--     1. BOSS_STAGES 테이블에 {스테이지 번호 = "보스이름"} 추가
--     2. BOSS_SEQUENCE 배열 끝에 "보스이름" 추가 (Endless 순환용)
--     3. bossDefs.lua에 보스 스탯 정의 추가
--     4. bossRenderers.lua에 렌더 함수 추가
--
--   [스테이지 추가 (핸드 디자인)]
--     1. STAGE_DEFS[번호] = { waves, spawnDirs, types } 추가
--     2. 없는 스테이지는 자동 생성 (테마 풀 교대)
--
--   [적 타입 추가]
--     1. ALL_ENEMY_TYPES에 새 타입 추가
--     2. 특성에 맞게 MOBILITY_POOL 또는 FIREPOWER_POOL에 추가
--     3. entityFactory.lua에 createEnemy 로직 추가
--
--   [변형(Variant) 추가]
--     1. GUARANTEED_VARIANTS에 {스테이지 = "변형이름"} 추가
--     2. VARIANT_TIERS에 { stage, variant, baseChance } 추가
--     3. entityFactory.lua에서 변형 스탯 보너스 정의
--     4. variantOverlays.lua에 렌더 함수 추가
--
-- ◆ 의존 관계
--   이 파일은 어떤 모듈도 require 하지 않는다 (순수 데이터).
--   stageManager.lua가 이 파일을 require 한다.
-- ============================================================================

local _min   = math.min
local _floor = math.floor
local _random = math.random

local M = {}

-- ─── Boss Stage Mapping ──────────────────────────────────────────
-- key = 스테이지 번호, value = 보스 이름 (bossDefs.lua의 키와 일치해야 함)
M.BOSS_STAGES = {
    [3]  = "NULL",
    [6]  = "STACK",
    [9]  = "HEAP",
    [12] = "RECURSION",
    [15] = "OVERFLOW",
}

-- Endless 모드에서 순환하는 보스 순서
M.BOSS_SEQUENCE = { "NULL", "STACK", "HEAP", "RECURSION", "OVERFLOW" }

--- 해당 스테이지의 보스 타입 반환. 보스 스테이지가 아니면 nil.
--- hand-designed (1~15) + Endless (18, 21, 24, ... 매 3스테이지)
function M.getBossType(stage)
    if M.BOSS_STAGES[stage] then
        return M.BOSS_STAGES[stage]
    end
    -- Endless: every 3 stages after 15 (18, 21, 24, ...)
    if stage > 15 and (stage - 15) % 3 == 0 then
        local index = ((stage - 18) / 3) % #M.BOSS_SEQUENCE + 1
        return M.BOSS_SEQUENCE[_floor(index)]
    end
    return nil
end

--- Endless 라운드 번호 (0 = 아직 Endless 아님, 1+ = 라운드)
function M.getEndlessRound(stage)
    if stage <= 15 then return 0 end
    return _floor((stage - 16) / 15) + 1
end

--- Boss 스케일링 (HP, 속도, 미니언, 색상 변화). Endless 전용.
--- @return table|nil  round ≤ 0 이면 nil
function M.getBossScaling(stage)
    local round = M.getEndlessRound(stage)
    if round <= 0 then return nil end
    return {
        hpMult    = 2.0 ^ round,
        speedMult = 1.0 + round * 0.1,
        minionAdd = round,
        redShift  = _min(round * 0.15, 0.6),
        bulletSpeedMult = 1.0 + round * 0.1,
        emitRateMult    = _min(1.0 + round * 0.15, 2.0),
        round     = round,
    }
end

-- ─── Hand-Designed Stage Definitions ─────────────────────────────
-- waves: 웨이브 수, spawnDirs: 방향별 확률, types: 등장 적 풀
-- 이 테이블에 없는 스테이지는 _getStageConfig()에서 자동 생성됨.
M.STAGE_DEFS = {
    [1] = { waves = 3, spawnDirs = {top=1.0},
            types = {"bit", "node"} },
    [2] = { waves = 4, spawnDirs = {top=0.8, left=0.1, right=0.1},
            types = {"bit", "node", "vector"} },
    [3] = { waves = 5, spawnDirs = {top=0.6, left=0.15, right=0.15, bottom=0.1},
            types = {"bit", "node", "vector"} },
    [4] = { waves = 5, spawnDirs = {top=0.5, left=0.2, right=0.2, bottom=0.1},
            types = {"bit", "node", "vector", "loop"} },
    [5] = { waves = 6, spawnDirs = {top=0.4, left=0.2, right=0.2, bottom=0.2},
            types = {"node", "vector", "loop", "matrix"} },
}

-- ─── Enemy Type Pools ────────────────────────────────────────────
-- 자동 생성 스테이지에서 테마 기반 풀 선택에 사용.
-- 기동형(mobility): 이동으로 위협 → 대시·반응 테스트
-- 화력형(firepower): 탄막으로 위협 → 포커스·패턴 읽기 테스트
-- loop는 궤도(이동) + 나선탄(화력) → 양쪽 소속
M.ALL_ENEMY_TYPES  = {"bit", "node", "vector", "loop", "matrix"}
M.MOBILITY_POOL    = {"bit", "vector", "loop"}
M.FIREPOWER_POOL   = {"node", "loop", "matrix"}
M.THEME_MIX_RATIO  = 0.70  -- 70% 테마 풀, 30% 전체 풀

-- ─── Stage Ordinal (테마 교대용) ─────────────────────────────────
-- 보스 스테이지를 건너뛴 순번 → 홀수=기동, 짝수=화력

local function _countBossStagesUpTo(stage)
    local count = 0
    for s, _ in pairs(M.BOSS_STAGES) do
        if s <= stage then count = count + 1 end
    end
    return count
end

--- 보스 스테이지를 제외한 순서 번호.
--- 예: Stage 7 → 7 - 2 (boss@3,6) = 5th → 홀수 → mobility
function M.getNonBossOrdinal(stage)
    return stage - _countBossStagesUpTo(stage)
end

-- ─── Variant System ──────────────────────────────────────────────
-- "변형 적"은 기본 적에 특수 능력을 부여한 강화판.
-- swift=잔상, armored=두꺼운 링, splitter=분열, shielded=전면 방패

--- Wave 1에서 강제로 등장시킬 변형 (첫 만남 보장)
M.GUARANTEED_VARIANTS = {
    [4]  = "swift",
    [7]  = "armored",
    [10] = "splitter",
    [13] = "shielded",
}

-- 변형 등장 확률 테이블 (stage 이후부터 등장, 스테이지마다 +3% 스케일링, 최대 25%)
local VARIANT_TIERS = {
    { stage = 4,  variant = "swift",    baseChance = 0.15 },
    { stage = 7,  variant = "armored",  baseChance = 0.12 },
    { stage = 10, variant = "splitter", baseChance = 0.10 },
    { stage = 13, variant = "shielded", baseChance = 0.08 },
}
local VARIANT_CAP = 0.25

--- 현재 스테이지에서 랜덤 변형을 하나 뽑는다. 없으면 nil.
--- 높은 tier부터 검사해서 먼저 걸리면 반환 (한 적당 최대 1개 변형).
function M.pickVariant(stage)
    for i = #VARIANT_TIERS, 1, -1 do
        local vt = VARIANT_TIERS[i]
        if stage >= vt.stage then
            local scaled = vt.baseChance * (1 + (stage - vt.stage) * 0.03)
            local chance = _min(scaled, VARIANT_CAP)
            if _random() < chance then
                return vt.variant
            end
        end
    end
    return nil
end

return M
