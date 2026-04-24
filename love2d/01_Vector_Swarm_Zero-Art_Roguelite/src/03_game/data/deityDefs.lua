-- deityDefs.lua
-- Deity System: 곡선 = 신(Deity) → 패시브 능력 부여
--
-- ◆ 구조
--   id         : 내부 식별자
--   name       : 표시 이름
--   curveName  : curveDefs.lua의 name 필드와 매칭 (곡선 렌더링용)
--   color      : 대표색 {r, g, b} (0-1)
--   lore       : 한 줄 설명 (선택 화면)
--   statBonuses: 수치 보너스 2개 — 게임 시작 시 applyStats()로 적용
--   signature  : 시그니처 능력 1개 — 전투 중 이벤트 트리거로 발동
--
-- ◆ 시그니처 trigger 종류
--   "on_graze"  : 탄막 스침 시
--   "on_hit"    : 적에게 데미지를 줄 때 (매 총알)
--   "on_kill"   : 적 처치 시
--
-- ◆ 새 Deity 추가
--   1. 이 파일에 DEITIES 배열에 추가
--   2. curveDefs.lua에 해당 곡선이 있는지 확인
--   3. deitySelectScene이 자동으로 인식 (수정 불필요)

local M = {}

-- ─── Deity 정의 ──────────────────────────────────────────────────

M.DEITIES = {
    -- ① Rose: 우아한 꽃의 여신 — Graze 특화
    {
        id        = "rose",
        name      = "Rose",
        curveName = "Rose 5",
        color     = {1.0, 0.4, 0.6},   -- 분홍
        lore      = "Grace of petals shields those who dance near death",
        statBonuses = {
            {
                stat  = "hpRegen",
                label = "HP Regen +15%",
                apply = function(ecs, playerId)
                    local h = ecs:getComponent(playerId, "Health")
                    if h then
                        h.regenRate = (h.regenRate or 0) * 1.15 + 0.002
                    end
                end,
            },
            {
                stat  = "moveSpeed",
                label = "Move Speed +5%",
                apply = function(ecs, playerId)
                    local v = ecs:getComponent(playerId, "Velocity")
                    if v then v.maxSpeed = v.maxSpeed * 1.05 end
                end,
            },
        },
        signature = {
            id      = "graze_heal",
            name    = "Petal Shield",
            desc    = "Graze restores 1 HP",
            trigger = "on_graze",
            chance  = 1.0,          -- 100% on graze
            execute = function(ecs, playerId, _context)
                local h = ecs:getComponent(playerId, "Health")
                if h and h.hp < h.maxHp then
                    h.hp = h.hp + 1
                end
            end,
        },
    },

    -- ② Cycloid: 회전하는 전쟁의 신 — 크리티컬 특화
    {
        id        = "cycloid",
        name      = "Cycloid",
        curveName = "Epicycloid (k=3)",
        color     = {0.3, 0.7, 1.0},   -- 하늘색
        lore      = "The rolling wheel strikes twice for the worthy",
        statBonuses = {
            {
                stat  = "damage",
                label = "Damage +10%",
                apply = function(ecs, playerId)
                    local w = ecs:getComponent(playerId, "PlayerWeapon")
                    if w then w.bulletDamage = w.bulletDamage * 1.10 end
                end,
            },
            {
                stat  = "fireRate",
                label = "Fire Rate +5%",
                apply = function(ecs, playerId)
                    local w = ecs:getComponent(playerId, "PlayerWeapon")
                    if w then w.fireRate = w.fireRate * 1.05 end
                end,
            },
        },
        signature = {
            id      = "critical_hit",
            name    = "Rolling Thunder",
            desc    = "20% chance to deal 2x damage",
            trigger = "on_hit",
            chance  = 0.20,
            execute = function(_ecs, _playerId, context)
                -- context.damage를 2배로 변경 (bulletEmitterSystem에서 사용)
                if context and context.damage then
                    context.damage = context.damage * 2
                end
            end,
        },
    },

    -- ③ Lemniscate: 무한의 현자 — 쿨다운/효율 특화
    {
        id        = "lemniscate",
        name      = "Lemniscate",
        curveName = "Lemniscate",
        color     = {0.6, 1.0, 0.4},   -- 연두
        lore      = "Infinity loops back; every end becomes a new beginning",
        statBonuses = {
            {
                stat  = "cooldown",
                label = "Cooldown -10%",
                apply = function(ecs, playerId)
                    local d = ecs:getComponent(playerId, "Dash")
                    if d then d.cooldown = d.cooldown * 0.90 end
                end,
            },
            {
                stat  = "xpGain",
                label = "XP Gain +10%",
                apply = function(ecs, playerId)
                    local xp = ecs:getComponent(playerId, "PlayerXP")
                    if xp then xp.xpMultiplier = (xp.xpMultiplier or 1.0) * 1.10 end
                end,
            },
        },
        signature = {
            id      = "kill_reset",
            name    = "Infinite Loop",
            desc    = "Kill has 5% chance to reset dash cooldown",
            trigger = "on_kill",
            chance  = 0.05,
            execute = function(ecs, playerId, _context)
                local d = ecs:getComponent(playerId, "Dash")
                if d then d.cooldownTimer = 0 end
            end,
        },
    },

    -- ④ Inferno: 악마의 곡선 — 탱크/AOE 특화
    {
        id        = "inferno",
        name      = "Inferno",
        curveName = "Devil's Curve",
        color     = {1.0, 0.3, 0.1},   -- 붉은 오렌지
        lore      = "From destruction, a ring of fire consumes all",
        statBonuses = {
            {
                stat  = "maxHp",
                label = "Max HP +2",
                apply = function(ecs, playerId)
                    local h = ecs:getComponent(playerId, "Health")
                    if h then
                        h.maxHp = h.maxHp + 2
                        h.hp    = h.hp + 2
                    end
                end,
            },
            {
                stat  = "defense",
                label = "I-Frame +0.2s",
                apply = function(ecs, playerId)
                    local h = ecs:getComponent(playerId, "Health")
                    if h then h.iFrameDuration = (h.iFrameDuration or 1.0) + 0.2 end
                end,
            },
        },
        signature = {
            id      = "kill_explosion",
            name    = "Devil's Pyre",
            desc    = "Kill has 10% chance to explode nearby enemies",
            trigger = "on_kill",
            chance  = 0.10,
            aoeDamage = 2,
            aoeRadius = 1.5,
            -- AOE 로직은 ecsManager._deityVFX.kill_explosion에서 처리
            -- (data 순수성 유지: ECS 로직은 data 레이어에 두지 않는다)
            execute = function(_ecs, _playerId, _context) end,
        },
    },
}

-- ─── Helper ──────────────────────────────────────────────────────

--- Deity ID로 정의 찾기
function M.getById(deityId)
    for _, d in ipairs(M.DEITIES) do
        if d.id == deityId then return d end
    end
    return nil
end

--- 스탯 보너스 적용 (게임 시작 시 호출)
function M.applyStats(ecs, playerId, deityId)
    local def = M.getById(deityId)
    if not def then return end
    for _, bonus in ipairs(def.statBonuses) do
        bonus.apply(ecs, playerId)
    end
    logInfo(string.format("[DEITY] Applied '%s' stat bonuses to player", def.name))
end

--- 시그니처 능력 트리거 체크 (전투 중 호출)
--- @return boolean 발동 여부
function M.tryTrigger(ecs, playerId, deityId, triggerType, context)
    local def = M.getById(deityId)
    if not def then return false end

    local sig = def.signature
    if sig.trigger ~= triggerType then return false end
    if math.random() > sig.chance then return false end

    sig.execute(ecs, playerId, context)
    return true
end

return M
