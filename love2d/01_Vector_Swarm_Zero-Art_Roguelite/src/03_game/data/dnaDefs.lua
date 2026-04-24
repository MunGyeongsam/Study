-- ============================================================================
-- DNA Definitions — 적 변이 엔진 유전자 풀
-- ============================================================================
--
-- ◆ 이 파일의 역할
--   Stage 16+ Endless에서 사용하는 DNA 기반 적 변이 시스템의 데이터 정의.
--   5개 유전자(Body, Movement, Attack, Modifier, OnDeath)의 풀과 조합 규칙.
--
-- ◆ 구조
--   M.BODY_SHAPES    — 도형 12종 (기존 6 + 신규 6: triangle/star/reuleaux/astroid/superellipse/gear)
--   M.BODY_LAYERS    — 레이어 후보 목록 (shape × mode × scale × rot)
--   M.MOVEMENT_POOL  — 이동 AI 풀
--   M.ATTACK_POOL    — 탄막 패턴 풀
--   M.MODIFIER_POOL  — 방어/특수 변형 풀
--   M.ONDEATH_POOL   — 사망 반응 풀
--   M.PRESETS         — 기존 5종을 DNA로 표현
--   M.FORBIDDEN       — 금지 조합
--   M.generateDna()  — 시드 기반 DNA 생성
--   M.calcStats()    — DNA → 스탯 변환
--
-- ◆ 의존 관계
--   순수 데이터 + 생성 함수. require 의존 없음.
-- ============================================================================

local _floor = math.floor
local _random = math.random
local _min = math.min
local _max = math.max

local M = {}

-- ─── 도형 풀 (12종) ─────────────────────────────────────────────
-- 기존 basicShapes 6종 + 신규 6종
-- 신규 도형은 6B.1에서 basicShapes.lua에 렌더 함수 추가
M.BODY_SHAPES = {
    -- 기존
    "circle", "diamond", "arrow", "spiral_ring", "hexagon", "rectangle",
    -- 신규 (수학 곡선 기반)
    "triangle", "star", "reuleaux", "astroid", "superellipse", "gear",
}

-- ─── Body 레이어 후보 ───────────────────────────────────────────
-- { shape, mode, scaleRange={min,max}, rotChoices={...} }
-- 생성 시 scaleRange에서 랜덤, rotChoices에서 택 1
M.BODY_LAYERS = {
    -- circle
    { shape = "circle",      mode = "fill", scale = {0.6, 1.0}, rot = {0} },
    { shape = "circle",      mode = "line", scale = {0.8, 1.3}, rot = {0} },
    -- diamond
    { shape = "diamond",     mode = "fill", scale = {0.5, 1.0}, rot = {0, 45} },
    { shape = "diamond",     mode = "line", scale = {0.8, 1.3}, rot = {0, 45} },
    -- arrow
    { shape = "arrow",       mode = "fill", scale = {0.6, 1.0}, rot = {0, 180} },
    { shape = "arrow",       mode = "line", scale = {0.8, 1.2}, rot = {0, 90, 180, 270} },
    -- spiral_ring
    { shape = "spiral_ring", mode = "line", scale = {0.7, 1.0}, rot = {0} },
    -- hexagon
    { shape = "hexagon",     mode = "fill", scale = {0.4, 0.7}, rot = {0, 30} },
    { shape = "hexagon",     mode = "line", scale = {0.8, 1.2}, rot = {0, 30} },
    -- rectangle
    { shape = "rectangle",   mode = "fill", scale = {0.5, 0.8}, rot = {0, 45, 90} },
    { shape = "rectangle",   mode = "line", scale = {0.7, 1.1}, rot = {0, 45, 90} },
    -- triangle (신규)
    { shape = "triangle",    mode = "fill", scale = {0.5, 1.0}, rot = {0, 60, 180} },
    { shape = "triangle",    mode = "line", scale = {0.8, 1.3}, rot = {0, 60, 180} },
    -- star (신규)
    { shape = "star",        mode = "fill", scale = {0.5, 0.9}, rot = {0, 36} },
    { shape = "star",        mode = "line", scale = {0.8, 1.2}, rot = {0, 36} },
    -- reuleaux (신규 — 뛸로 삼각형, 정폭도형)
    { shape = "reuleaux",     mode = "fill", scale = {0.5, 0.9}, rot = {0, 120, 240} },
    { shape = "reuleaux",     mode = "line", scale = {0.8, 1.2}, rot = {0, 120, 240} },
    -- astroid (신규 — 4첨성, hypocycloid k=4)
    { shape = "astroid",      mode = "fill", scale = {0.5, 0.9}, rot = {0, 45} },
    { shape = "astroid",      mode = "line", scale = {0.8, 1.2}, rot = {0, 45} },
    -- superellipse (신규 — 스쿼클, Lamé curve n=4)
    { shape = "superellipse", mode = "fill", scale = {0.5, 0.8}, rot = {0, 45} },
    { shape = "superellipse", mode = "line", scale = {0.8, 1.2}, rot = {0, 45} },
    -- gear (신규)
    { shape = "gear",        mode = "fill", scale = {0.5, 0.8}, rot = {0} },
    { shape = "gear",        mode = "line", scale = {0.8, 1.2}, rot = {0} },
}

-- fill 전용 레이어 인덱스 (조합 규칙: fill은 최대 1개, 가장 아래)
M._fillLayers = {}
M._lineLayers = {}
for i, layer in ipairs(M.BODY_LAYERS) do
    if layer.mode == "fill" then
        M._fillLayers[#M._fillLayers + 1] = i
    else
        M._lineLayers[#M._lineLayers + 1] = i
    end
end

-- ─── Movement 풀 ────────────────────────────────────────────────
-- id = enemyAI.aiType, 스탯 배수 포함
M.MOVEMENT_POOL = {
    { id = "drift",      speedMult = 1.0, hpMult = 1.0,  desc = "고정 방향 이동" },
    { id = "orbit",      speedMult = 0.8, hpMult = 1.2,  desc = "원형 궤도" },
    { id = "chase",      speedMult = 1.2, hpMult = 0.8,  desc = "플레이어 추적" },
    { id = "stationary", speedMult = 0.0, hpMult = 1.5,  desc = "제자리 고정" },
    { id = "swarm",      speedMult = 1.3, hpMult = 0.6,  desc = "군집 돌진" },
    { id = "charge",     speedMult = 1.5, hpMult = 0.7,  desc = "경고→돌진" },
}

-- ─── Attack 풀 ──────────────────────────────────────────────────
-- id = bulletEmitter.pattern
M.ATTACK_POOL = {
    { id = "none",        dpsMult = 0.0, desc = "탄막 없음 (접촉형)" },
    { id = "circle",      dpsMult = 1.0, desc = "등간격 원형" },
    { id = "aimed",       dpsMult = 1.2, desc = "플레이어 조준" },
    { id = "spiral",      dpsMult = 1.1, desc = "회전 나선" },
    { id = "cross",       dpsMult = 1.0, desc = "십자 교대" },
    { id = "ring_pulse",  dpsMult = 0.9, desc = "맥동 원형" },
    { id = "wave",        dpsMult = 0.8, desc = "사인파 하강" },
    { id = "orbit_shot",  dpsMult = 1.3, desc = "공전 궤도 탄" },
    { id = "return_shot", dpsMult = 1.3, desc = "부메랑 탄" },
}

-- ─── Modifier 풀 ────────────────────────────────────────────────
-- 기존 variant 시스템과 동일 구조
M.MODIFIER_POOL = {
    { id = "none",     speedMult = 1.0,  hpMult = 1.0, scaleMult = 1.0, xpMult = 1.0 },
    { id = "swift",    speedMult = 1.5,  hpMult = 0.8, scaleMult = 0.8, xpMult = 1.2 },
    { id = "armored",  speedMult = 0.7,  hpMult = 2.5, scaleMult = 1.3, xpMult = 1.8 },
    { id = "shielded", speedMult = 0.85, hpMult = 1.5, scaleMult = 1.1, xpMult = 1.5 },
}

-- ─── OnDeath 풀 ─────────────────────────────────────────────────
M.ONDEATH_POOL = {
    { id = "none",  xpMult = 1.0, desc = "기본 사망" },
    { id = "split", xpMult = 1.3, desc = "미니 2마리 분열" },
}

-- ─── 기존 5종 프리셋 (DNA 언어) ─────────────────────────────────
-- Stage 1~15에서는 이 프리셋 사용 안 함 (기존 코드 유지)
-- DNA 생성 시 "베이스"로 참조
M.PRESETS = {
    bit = {
        body     = {{ shape = "circle",      mode = "fill", scale = 1.0, rot = 0 }},
        movement = "swarm",
        attack   = "none",
        modifier = "none",
        onDeath  = "none",
        baseHp   = 1,
        baseSpeed = 0.8,
        baseRadius = 0.08,
        baseXp   = 1,
        color    = {0, 1, 1},
    },
    node = {
        body     = {{ shape = "diamond",     mode = "fill", scale = 1.0, rot = 0 }},
        movement = "stationary",
        attack   = "ring_pulse",
        modifier = "none",
        onDeath  = "none",
        baseHp   = 4,
        baseSpeed = 0,
        baseRadius = 0.15,
        baseXp   = 3,
        color    = {1, 0.5, 0},
    },
    vector = {
        body     = {{ shape = "arrow",       mode = "fill", scale = 1.0, rot = 0 }},
        movement = "charge",
        attack   = "none",
        modifier = "none",
        onDeath  = "none",
        baseHp   = 2,
        baseSpeed = 4.0,
        baseRadius = 0.12,
        baseXp   = 4,
        color    = {1, 0.2, 0.2},
    },
    loop = {
        body     = {{ shape = "spiral_ring", mode = "line", scale = 1.0, rot = 0 }},
        movement = "orbit",
        attack   = "spiral",
        modifier = "none",
        onDeath  = "none",
        baseHp   = 5,
        baseSpeed = 0.8,
        baseRadius = 0.18,
        baseXp   = 5,
        color    = {0.5, 0.2, 1},
    },
    matrix = {
        body     = {{ shape = "hexagon",     mode = "fill", scale = 1.0, rot = 0 }},
        movement = "drift",
        attack   = "cross",
        modifier = "none",
        onDeath  = "none",
        baseHp   = 6,
        baseSpeed = 0.15,
        baseRadius = 0.16,
        baseXp   = 6,
        color    = {0.2, 1, 0.5},
    },
}
M.PRESET_KEYS = { "bit", "node", "vector", "loop", "matrix" }

-- ─── 금지 조합 ──────────────────────────────────────────────────
-- {movement, attack} 쌍: 의미 없거나 짜증나는 조합
M.FORBIDDEN = {
    -- 고정+탄막없음 = 아무것도 안 하는 적
    { movement = "stationary", attack = "none" },
    -- 돌진+궤도탄 = 탄이 의미 없음 (빠르게 지나감)
    { movement = "charge",     attack = "orbit_shot" },
    -- 군집+부메랑 = 부메랑이 뒤의 아군에 가려짐
    { movement = "swarm",      attack = "return_shot" },
}

--- 금지 조합인지 확인
--- @param movementId string
--- @param attackId string
--- @return boolean
function M.isForbidden(movementId, attackId)
    for _, f in ipairs(M.FORBIDDEN) do
        if f.movement == movementId and f.attack == attackId then
            return true
        end
    end
    return false
end

-- ─── Body 레이어 생성 ───────────────────────────────────────────

--- 레이어 후보에서 실제 레이어 1개 생성 (scale 랜덤, rot 택 1)
--- @param layerDef table BODY_LAYERS 원소
--- @return table { shape, mode, scale, rot }
local function _instantiateLayer(layerDef)
    local sMin, sMax = layerDef.scale[1], layerDef.scale[2]
    -- 소수점 2자리로 반올림
    local s = sMin + _random() * (sMax - sMin)
    s = _floor(s * 100 + 0.5) / 100
    local rots = layerDef.rot
    local r = rots[_random(#rots)]
    return { shape = layerDef.shape, mode = layerDef.mode, scale = s, rot = r }
end

--- Body 레이어 배열 자동 생성
--- @param layerCount number 레이어 수 (1~3)
--- @return table body 레이어 배열
function M.generateBody(layerCount)
    local body = {}
    local usedShapes = {}

    -- 규칙: fill 레이어는 최대 1개, 가장 아래
    if layerCount >= 1 then
        -- 첫 레이어: fill (기본 몸통)
        local idx = M._fillLayers[_random(#M._fillLayers)]
        local layer = _instantiateLayer(M.BODY_LAYERS[idx])
        body[1] = layer
        usedShapes[layer.shape .. layer.mode] = true
    end

    -- 추가 레이어: line만
    for i = 2, layerCount do
        local attempts = 0
        local layer
        repeat
            local idx = M._lineLayers[_random(#M._lineLayers)]
            layer = _instantiateLayer(M.BODY_LAYERS[idx])
            attempts = attempts + 1
        until not usedShapes[layer.shape .. layer.mode] or attempts > 10
        -- 바깥 레이어는 안쪽보다 크게
        if body[i - 1] and layer.scale <= body[i - 1].scale then
            layer.scale = body[i - 1].scale + 0.2
        end
        body[i] = layer
        usedShapes[layer.shape .. layer.mode] = true
    end

    return body
end

-- ─── 색상 생성 ──────────────────────────────────────────────────
-- DNA 적은 기존 5종과 구별되는 색상 범위
local DNA_COLORS = {
    {1.0, 0.3, 0.6},  -- 핫핑크
    {0.6, 0.3, 1.0},  -- 보라
    {0.3, 0.8, 1.0},  -- 하늘
    {1.0, 0.7, 0.2},  -- 금색
    {0.3, 1.0, 0.6},  -- 민트
    {1.0, 0.4, 0.3},  -- 산호
    {0.8, 0.8, 0.2},  -- 올리브
    {0.4, 0.6, 1.0},  -- 라벤더
}

--- 랜덤 DNA 색상
--- @return table {r, g, b}
function M.randomColor()
    return DNA_COLORS[_random(#DNA_COLORS)]
end

--- DNA 조합에서 곡선 용도(role) 힌트 생성
--- 실제 curve 선택은 entityFactory에서 shapeDefs를 기반으로 수행한다.
--- @return string "enemy" | "boss" | "both"
local function _suggestCurveRole(movement, attack, modifier, round)
    if movement == "stationary" and attack ~= "none" and round >= 3 then
        return "boss"
    end

    if modifier == "armored" or modifier == "shielded" then
        return "both"
    end

    if round >= 4 and _random() < 0.2 then
        return "both"
    end

    return "enemy"
end

-- ─── DNA 생성 ───────────────────────────────────────────────────

--- 시드 기반 DNA 자동 생성
--- @param round number Endless 라운드 (1+)
--- @return table dna { body, movement, attack, modifier, onDeath, color, stats }
function M.generateDna(round)
    round = round or 1

    -- 레이어 수: Round 1~2 → 1~2개, Round 3+ → 2~3개
    local maxLayers = round >= 3 and 3 or 2
    local layerCount = _random(1, maxLayers)

    -- 변이 유전자 수: Round 1 → 1개, Round 3+ → 1~2개
    local mutationCount = round >= 3 and _random(1, 2) or 1

    -- 베이스 프리셋 선택
    local baseKey = M.PRESET_KEYS[_random(#M.PRESET_KEYS)]
    local base = M.PRESETS[baseKey]

    -- Body: 항상 새로 생성 (레이어 조합)
    local body = M.generateBody(layerCount)

    -- 나머지 유전자: 베이스에서 시작, mutationCount만큼 교체
    local movement = base.movement
    local attack   = base.attack
    local modifier = "none"
    local onDeath  = "none"

    -- 변이 가능 슬롯 (body는 이미 변이됨)
    local slots = { "movement", "attack", "modifier", "onDeath" }
    -- 셔플
    for i = #slots, 2, -1 do
        local j = _random(i)
        slots[i], slots[j] = slots[j], slots[i]
    end

    for m = 1, mutationCount do
        local slot = slots[m]
        if slot == "movement" then
            local pool = M.MOVEMENT_POOL
            local pick
            local attempts = 0
            repeat
                pick = pool[_random(#pool)]
                attempts = attempts + 1
            until (pick.id ~= movement and not M.isForbidden(pick.id, attack))
                  or attempts > 10
            movement = pick.id
        elseif slot == "attack" then
            local pool = M.ATTACK_POOL
            local pick
            local attempts = 0
            repeat
                pick = pool[_random(#pool)]
                attempts = attempts + 1
            until (pick.id ~= attack and not M.isForbidden(movement, pick.id))
                  or attempts > 10
            attack = pick.id
        elseif slot == "modifier" then
            local pool = M.MODIFIER_POOL
            modifier = pool[_random(2, #pool)].id  -- none 제외
        elseif slot == "onDeath" then
            local pool = M.ONDEATH_POOL
            onDeath = pool[_random(2, #pool)].id    -- none 제외
        end
    end

    -- 색상
    local color = M.randomColor()

    -- 스탯 산출
    local stats = M.calcStats(base, movement, attack, modifier, onDeath, round)
    local curveRole = _suggestCurveRole(movement, attack, modifier, round)

    return {
        baseType  = baseKey,
        body      = body,
        movement  = movement,
        attack    = attack,
        modifier  = modifier,
        onDeath   = onDeath,
        color     = color,
        stats     = stats,
        curveRole = curveRole,
        curveName = nil, -- entityFactory가 shapeDefs 기반으로 주입
    }
end

-- ─── 스탯 산출 ──────────────────────────────────────────────────

--- DNA 유전자 조합 → 최종 스탯 계산
--- @param base table 프리셋 (baseHp, baseSpeed, baseRadius, baseXp)
--- @param movementId string
--- @param attackId string
--- @param modifierId string
--- @param onDeathId string
--- @param round number Endless 라운드
--- @return table { hp, speed, radius, xp }
function M.calcStats(base, movementId, attackId, modifierId, onDeathId, round)
    -- Movement 배수 찾기
    local moveDef
    for _, m in ipairs(M.MOVEMENT_POOL) do
        if m.id == movementId then moveDef = m; break end
    end
    moveDef = moveDef or M.MOVEMENT_POOL[1]

    -- Modifier 배수 찾기
    local modDef
    for _, m in ipairs(M.MODIFIER_POOL) do
        if m.id == modifierId then modDef = m; break end
    end
    modDef = modDef or M.MODIFIER_POOL[1]

    -- OnDeath XP 배수 찾기
    local deathDef
    for _, d in ipairs(M.ONDEATH_POOL) do
        if d.id == onDeathId then deathDef = d; break end
    end
    deathDef = deathDef or M.ONDEATH_POOL[1]

    -- 라운드 스케일링
    local roundHpMult = 1.0 + (round - 1) * 0.3   -- Round 1: x1.0, Round 4: x1.9
    local roundSpdMult = 1.0 + (round - 1) * 0.05  -- Round 1: x1.0, Round 4: x1.15

    -- 최종 스탯
    local hp = _max(1, _floor(base.baseHp * moveDef.hpMult * modDef.hpMult * roundHpMult + 0.5))
    local speed = base.baseSpeed * moveDef.speedMult * modDef.speedMult * roundSpdMult
    local radius = base.baseRadius * modDef.scaleMult
    local xp = _max(1, _floor(base.baseXp * modDef.xpMult * deathDef.xpMult + 0.5))

    return {
        hp     = hp,
        speed  = speed,
        radius = radius,
        xp     = xp,
    }
end

return M
