-- ============================================================================
-- ecsManager.lua — ECS 오케스트레이터
-- ============================================================================
--
-- ◆ 역할
--   ECS world 생성, 18개 시스템 등록/실행, bulletPool/stageManager 호출.
--   update(dt)는 로직 시스템, draw()는 렌더 시스템만 실행한다.
--
-- ◆ 시스템 실행 순서 (등록 순)
--   Input → Focus → Dash → EnemyAI → Movement → Boundary → LifeSpan
--   → BulletEmitter → PlayerWeapon → Collision → EnemyCollision
--   → XpCollection → Boss → [bulletPool] → [stageManager]
--   렌더: Render → PlayerRender → [bulletPool.draw]
--
-- ◆ 핵심 API
--   init(getPlayerPos), update(dt), draw()
--   createPlayer/createEnemy/createDnaEnemy/restart
--   getWorld/getBulletPool/getStageManager

local ECS = require("01_core.ecs")
local world = require("01_core.world")

local _random = math.random
local _pi     = math.pi
local _cos    = math.cos
local _sin    = math.sin
local _min    = math.min
local _max    = math.max
local _floor  = math.floor

-- Systems (03_game/systems/)
local InputSystem        = require("03_game.systems.inputSystem")
local MovementSystem     = require("03_game.systems.movementSystem")
local BoundarySystem     = require("03_game.systems.boundarySystem")
local LifeSpanSystem     = require("03_game.systems.lifespanSystem")
local RenderSystem       = require("03_game.systems.renderSystem")
local PlayerRenderSystem = require("03_game.systems.playerRenderSystem")

-- Bullet system (pool-based, not per-entity ECS)
local BulletPool                = require("03_game.systems.bulletPool")
local createBulletEmitterSystem = require("03_game.systems.bulletEmitterSystem")
local createCollisionSystem     = require("03_game.systems.collisionSystem")
local createEnemyCollisionSystem = require("03_game.systems.enemyCollisionSystem")
local achievementSystem           = require("03_game.states.achievementSystem")
local createPlayerWeaponSystem  = require("03_game.systems.playerWeaponSystem")
local createEnemyAISystem       = require("03_game.systems.enemyAISystem")
local DashSystem                = require("03_game.systems.dashSystem")
local FocusSystem               = require("03_game.systems.focusSystem")
local createXpCollectionSystem  = require("03_game.systems.xpCollectionSystem")
local StageManager              = require("03_game.systems.stageManager")
local createBossSystem          = require("03_game.systems.bossSystem")

-- Game state (for XP scaling by stage)
local gameState = require("03_game.states.gameState")
local upgradeTree = require("03_game.states.upgradeTree")
local deityDefs = require("03_game.data.deityDefs")

-- Entity factories (03_game/entities/)
local EntityFactory = require("03_game.entities.entityFactory")

local ECSManager = {}

function ECSManager.init(getPlayerPos)
    ECSManager.world = ECS.new()
    ECSManager.systems = {}
    ECSManager.systemsOrder = {}  -- 실행 순서 보장
    ECSManager.getPlayerPos = getPlayerPos or function() return 0, 0 end
    
    -- Bullet pool (shared by emitter system and render)
    ECSManager.bulletPool = BulletPool.new(2000)

    -- World bounds for bullet culling (use getBounds for consistency)
    local left, bottom, right, top = world.getBounds()
    ECSManager.bulletBounds = {
        minX = left, maxX = right,
        minY = bottom, maxY = top,
    }

    -- Stage manager (replaces enemy spawner)
    ECSManager.stageManager = StageManager.new(ECSManager, ECSManager.getPlayerPos)

    logInfo("[ECS] ECS Manager initialized")
    
    -- 기본 시스템들 등록
    ECSManager._registerBasicSystems()
    
    return true
end

-- ── Deity VFX 디스패치 테이블 (시그니처별 발동 이펙트) ──
-- 새 deity 추가 시 여기에 항목만 추가하면 됨 (O원칙: 확장에 열림, 수정에 닫힘)
-- @param bp     BulletPool
-- @param ps     PlayScene (flashScreen/tintScreen)
-- @param px,py  플레이어 월드 좌표
-- @param cr,cg,cb deity 대표색
-- @param ctx    트리거 context
local _deityVFX = {
    graze_heal = function(bp, ps, px, py, _cr, _cg, _cb, _ctx)
        -- 초록 힐 파티클 (위로 퍼지는 꽃잎) + 초록 틴트
        logDebug(string.format("[DEITY-VFX] graze_heal: pos=(%.1f,%.1f)", px, py))
        ps.tintScreen(0.2, 0.9, 0.3, 0.15)
        for i = 1, 5 do
            local angle = -_pi / 2 + (i - 3) * 0.4
            local speed = 1.0 + _random() * 0.8
            bp:spawn(px, py, _cos(angle) * speed, _sin(angle) * speed, {
                radius = 0.03, maxLifetime = 0.35,
                color = {0.3, 1.0, 0.4, 0.9}, layer = "debris",
                damping = 0.03, fadeAlpha = true,
            })
        end
    end,

    critical_hit = function(bp, ps, px, py, _cr, _cg, _cb, ctx)
        -- 노란 플래시 + 작은 쉐이크 + 적 위치 불꽃
        logDebug(string.format("[DEITY-VFX] critical_hit: dmg=%s ex=(%.1f,%.1f)",
            tostring(ctx and ctx.damage), ctx and ctx.enemyX or 0, ctx and ctx.enemyY or 0))
        ps.flashScreen(0.15)
        if screenShake then screenShake(0.06, 0.08) end
        local ex, ey = ctx and ctx.enemyX or px, ctx and ctx.enemyY or py
        for i = 1, 4 do
            local angle = _random() * _pi * 2
            local speed = 2.0 + _random() * 1.5
            bp:spawn(ex, ey, _cos(angle) * speed, _sin(angle) * speed, {
                radius = 0.04, maxLifetime = 0.2,
                color = {1.0, 0.9, 0.2, 1.0}, layer = "debris",
                damping = 0.04, fadeAlpha = true,
            })
        end
    end,

    kill_reset = function(bp, _ps, px, py, cr, cg, cb, _ctx)
        -- 시안 파티클 링 (플레이어 주위)
        logDebug(string.format("[DEITY-VFX] kill_reset: player=(%.1f,%.1f)", px, py))
        for i = 1, 8 do
            local angle = (i / 8) * _pi * 2
            local speed = 1.5
            bp:spawn(px, py, _cos(angle) * speed, _sin(angle) * speed, {
                radius = 0.025, maxLifetime = 0.3,
                color = {cr, cg, cb, 0.9}, layer = "debris",
                damping = 0.02, fadeAlpha = true,
            })
        end
    end,

    kill_explosion = function(bp, _ps, px, py, cr, cg, cb, ctx, ecs)
        local ex, ey = ctx and ctx.enemyX or px, ctx and ctx.enemyY or py

        -- AOE 데미지: 폭발 범위 내 적에게 데미지 (deityDefs에서 이관된 로직)
        local sig = deityDefs.getById("inferno").signature
        local enemies = ecs:queryEntities({"Transform", "Health", "EnemyAI"})
        local _aoeHitCount = 0
        for _, eId in ipairs(enemies) do
            local t = ecs:getComponent(eId, "Transform")
            if t then
                local dx, dy = t.x - ex, t.y - ey
                if dx * dx + dy * dy <= sig.aoeRadius * sig.aoeRadius then
                    local eh = ecs:getComponent(eId, "Health")
                    if eh then eh.hp = eh.hp - sig.aoeDamage; _aoeHitCount = _aoeHitCount + 1 end
                end
            end
        end
        logDebug(string.format("[DEITY-VFX] kill_explosion: pos=(%.1f,%.1f) aoeHits=%d", ex, ey, _aoeHitCount))

        -- VFX: 붉은 폭발 링 + 파편
        if screenShake then screenShake(0.12, 0.15) end
        for i = 1, 10 do
            local angle = (i / 10) * _pi * 2 + (_random() - 0.5) * 0.3
            local speed = 2.5 + _random() * 1.5
            bp:spawn(ex, ey, _cos(angle) * speed, _sin(angle) * speed, {
                radius = 0.035 + _random() * 0.02, maxLifetime = 0.3,
                color = {cr, cg, cb, 1.0}, layer = "debris",
                damping = 0.03, fadeAlpha = true,
            })
        end
        -- 중심 플래시 파티클 (밝은 코어)
        bp:spawn(ex, ey, 0, 0, {
            radius = 0.15, maxLifetime = 0.15,
            color = {1.0, 0.8, 0.3, 0.8}, layer = "debris",
            damping = 0, fadeAlpha = true,
        })
    end,
}

--- Deity 시그니처 트리거 헬퍼
--- 플레이어 엔티티를 자동 탐색하여 deity 시그니처 발동을 시도한다.
--- 발동 시 VFX 디스패치 테이블을 통해 이펙트도 함께 처리.
--- @param triggerType string "on_graze"|"on_hit"|"on_kill"
--- @param context table|nil 트리거 데이터 (damage, enemyX, enemyY 등)
--- @return boolean 발동 여부
function ECSManager.tryDeityTrigger(triggerType, context)
    local players = ECSManager.world:queryEntities({"PlayerTag"})
    if #players == 0 then return false end
    local playerId = players[1]
    local tag = ECSManager.world:getComponent(playerId, "PlayerTag")
    if not tag or not tag.deityId then return false end

    local fired = deityDefs.tryTrigger(ECSManager.world, playerId, tag.deityId, triggerType, context)
    if not fired then return false end

    logDebug(string.format("[DEITY] %s fired (deity=%s, trigger=%s)", 
        deityDefs.getById(tag.deityId).signature.id, tag.deityId, triggerType))

    -- VFX 디스패치
    local def = deityDefs.getById(tag.deityId)
    if not def then return true end
    local vfxFn = _deityVFX[def.signature.id]
    if vfxFn then
        local cr, cg, cb = def.color[1], def.color[2], def.color[3]
        local PlayScene = require("03_game.scenes.playScene")
        local pt = ECSManager.world:getComponent(playerId, "Transform")
        local px, py = pt and pt.x or 0, pt and pt.y or 0
        vfxFn(ECSManager.bulletPool, PlayScene, px, py, cr, cg, cb, context, ECSManager.world)
    end
    return true
end

-- 시스템 등록
function ECSManager.addSystem(system)
    if not system or not system.name then
        logError("ECS: Invalid system")
        return false
    end
    
    ECSManager.systems[system.name] = system
    table.insert(ECSManager.systemsOrder, system.name)
    
    logInfo(string.format("[ECS] System '%s' registered", system.name))
    return true
end

-- 시스템 제거
function ECSManager.removeSystem(systemName)
    if not ECSManager.systems[systemName] then
        return false
    end
    
    ECSManager.systems[systemName] = nil
    
    -- systemsOrder에서도 제거
    for i, name in ipairs(ECSManager.systemsOrder) do
        if name == systemName then
            table.remove(ECSManager.systemsOrder, i)
            break
        end
    end
    
    logInfo(string.format("[ECS] System '%s' removed", systemName))
    return true
end

-- 렌더 시스템 이름 목록 (draw에서만 실행, 한 번만 생성)
local renderSystems = { Render = true, PlayerRender = true }

-- 모든 시스템 업데이트 (Render 시스템 제외 — draw()에서만 실행)
function ECSManager.update(dt)
    if not ECSManager.world then
        return
    end

    -- 등록된 순서대로 시스템 실행 (렌더 계열 제외)
    for _, systemName in ipairs(ECSManager.systemsOrder) do
        if not renderSystems[systemName] then
            local system = ECSManager.systems[systemName]
            if system then
                system:update(ECSManager.world, dt)
            end
        end
    end

    -- Bullet pool update (movement + lifetime + culling)
    if ECSManager.bulletPool then
        ECSManager.bulletPool:update(dt, ECSManager.bulletBounds)
    end

    -- Stage manager (wave spawning + stage progression)
    if ECSManager.stageManager then
        ECSManager.stageManager:update(dt)
    end
end

-- 렌더링 (카메라 변환은 호출자가 적용 — main.lua의 drawWorld 내부에서 호출됨)
function ECSManager.draw()
    if not ECSManager.world then
        return
    end
    
    -- 렌더 시스템 실행 (카메라 변환은 이미 적용된 상태)
    local renderSystem = ECSManager.systems["Render"]
    if renderSystem then
        renderSystem:update(ECSManager.world, 0)
    end

    -- Bullet pool rendering (all active bullets)
    ECSManager.bulletPool:draw()

    -- 플레이어 전용 렌더 (외곽선, 방향 — 불릿 위에 그려짐)
    local playerRenderSystem = ECSManager.systems["PlayerRender"]
    if playerRenderSystem then
        playerRenderSystem:update(ECSManager.world, 0)
    end
end

-- 플레이어 엔티티 생성 (EntityFactory 위임)
function ECSManager.createPlayer(x, y, characterId)
    if not ECSManager.world then
        logError("ECS: World not initialized")
        return nil
    end
    return EntityFactory.createPlayer(ECSManager.world, x, y, characterId)
end

-- 적 엔티티 생성 (EntityFactory 위임, difficulty/variant 포워딩)
function ECSManager.createEnemy(x, y, enemyType, difficulty, variant)
    if not ECSManager.world then
        logError("ECS: World not initialized")
        return nil
    end
    return EntityFactory.createEnemy(ECSManager.world, x, y, enemyType, difficulty, variant)
end

-- DNA 기반 적 엔티티 생성 (Stage 16+ Endless)
function ECSManager.createDnaEnemy(x, y, dna, difficulty)
    if not ECSManager.world then
        logError("ECS: World not initialized")
        return nil
    end
    return EntityFactory.createDnaEnemy(ECSManager.world, x, y, dna, difficulty)
end

-- ECS 통계 반환
function ECSManager.getStats()
    if not ECSManager.world then
        return { error = "World not initialized" }
    end
    
    local worldStats = ECSManager.world:getStats()
    local systemStats = {}
    
    for name, system in pairs(ECSManager.systems) do
        systemStats[name] = system:getStats()
    end
    
    return {
        world = worldStats,
        systems = systemStats,
        bullets = ECSManager.bulletPool:getStats(),
        stage = ECSManager.stageManager:getStats(),
    }
end

-- ECS 월드 참조 반환 (player.lua bind용)
function ECSManager.getWorld()
    return ECSManager.world
end

-- Bullet pool 참조 반환
function ECSManager.getBulletPool()
    return ECSManager.bulletPool
end

-- Stage manager 참조 반환
function ECSManager.getStageManager()
    return ECSManager.stageManager
end

-- 게임 리스타트: 월드 초기화, 불릿 클리어, 스포너 리셋
function ECSManager.restart()
    -- 모든 불릿 제거
    ECSManager.bulletPool:clear()

    -- 모든 엔티티 제거
    local world = ECSManager.world
    local toDestroy = {}
    for entityId, _ in pairs(world.entities) do
        toDestroy[#toDestroy + 1] = entityId
    end
    for _, entityId in ipairs(toDestroy) do
        world:destroyEntity(entityId)
    end

    -- 스테이지 매니저 리셋
    ECSManager.stageManager = StageManager.new(ECSManager, ECSManager.getPlayerPos)

    logInfo("[ECS] World restarted")
end

-- 기본 시스템들 등록 (실행 순서가 중요!)
function ECSManager._registerBasicSystems()
    local getPlayerPos = ECSManager.getPlayerPos
    -- 1. Input: 키보드/터치 → Velocity + Dash/Focus 요청
    ECSManager.addSystem(InputSystem)
    -- 2. Focus: 슬로모 + 판정축소 + 에너지
    ECSManager.addSystem(FocusSystem)
    -- 3. Dash: 순간이동 + 무적
    ECSManager.addSystem(DashSystem)
    -- 4. EnemyAI: AI 행동 → Velocity
    ECSManager.addSystem(createEnemyAISystem(getPlayerPos))
    -- 5. Movement: Velocity → Transform
    ECSManager.addSystem(MovementSystem)
    -- 6. Boundary: 월드 경계 clamping
    ECSManager.addSystem(BoundarySystem)
    -- 7. LifeSpan: 수명 만료 제거
    ECSManager.addSystem(LifeSpanSystem)
    -- 8. BulletEmitter: 이미터 → BulletPool spawn
    ECSManager.addSystem(createBulletEmitterSystem(ECSManager.bulletPool, getPlayerPos))
    -- 9. PlayerWeapon: 자동 조준 + 발사
    local PlayScene = require("03_game.scenes.playScene")
    ECSManager.addSystem(createPlayerWeaponSystem(ECSManager.bulletPool, PlayScene.getDisableWeapon))
    -- 10. Collision: 플레이어 ↔ 적 불릿 충돌 + graze
    local onGraze = function(entityId, bx, by)
        -- 이펙트: 스침 파티클 2개 (짧은 선 느낌)
        for _ = 1, 2 do
            local angle = _random() * _pi * 2
            local speed = 1.5 + _random() * 1.5
            ECSManager.bulletPool:spawn(bx, by,
                _cos(angle) * speed, _sin(angle) * speed,
                { radius = 0.02, maxLifetime = 0.15,
                  color = {1, 1, 1, 0.8}, layer = "debris",
                  damping = 0.05, fadeAlpha = true })
        end
        -- 보상: 포커스 에너지 소량 회복
        local focus = ECSManager.world:getComponent(entityId, "Focus")
        if focus then
            focus.energy = _min(focus.energy + 0.3, focus.maxEnergy)
        end
        -- Deity: on_graze 시그니처 트리거
        ECSManager.tryDeityTrigger("on_graze", nil)
    end
    -- 11. Enemy death handler (shared by bullet collision + contact collision)
    local onEnemyDeath = function(ecs, x, y, xpValue)
        -- XP 스케일링: 스테이지가 올라갈수록 XP 보상 증가
        local stage = gameState.getStageInfo()
        local xpMult = 1.0 + (stage - 1) * 0.15
        local scaledXP = _max(1, _floor(xpValue * xpMult + 0.5))
        EntityFactory.createXpOrb(ecs, x, y, scaledXP)

        -- Fragment 드롭: 기본 10% + Data Miner 보너스
        local dropRate = 0.10 + upgradeTree.getFragmentDropBonus()
        if _random() < dropRate then
            gameState.addFragments(1)
        end

        -- Achievement: 킬 수 추적
        achievementSystem.onEnemyKill()
        -- Deity: on_kill 시그니처 트리거
        ECSManager.tryDeityTrigger("on_kill", {enemyX = x, enemyY = y})
    end
    ECSManager.addSystem(createCollisionSystem(ECSManager.bulletPool, {
        onGraze = onGraze,
        onContactKill = function(ecs, enemyId, ex, ey, xpValue)
            onEnemyDeath(ecs, ex, ey, xpValue)
        end,
    }))
    -- 12. EnemyCollision: 플레이어 불릿 ↔ 적 충돌 + XP 드롭
    -- Deity on_hit modifier: 크리티컬 등 데미지 변환
    local onHitModifier = function(dmg, enemyX, enemyY)
        local ctx = {damage = dmg, enemyX = enemyX, enemyY = enemyY}
        ECSManager.tryDeityTrigger("on_hit", ctx)
        return ctx.damage
    end
    ECSManager.addSystem(createEnemyCollisionSystem(ECSManager.bulletPool, onEnemyDeath, function(x, y, enemyType, difficulty, variant, scaleMult)
        local id = ECSManager.createEnemy(x, y, enemyType, difficulty, variant)
        if id and scaleMult then
            local t = ECSManager.world:getComponent(id, "Transform")
            if t then t.scale = t.scale * scaleMult end
            local c = ECSManager.world:getComponent(id, "Collider")
            if c then c.radius = c.radius * scaleMult end
            local h = ECSManager.world:getComponent(id, "Health")
            if h then h.maxHp = 1; h.hp = 1 end
            local ai = ECSManager.world:getComponent(id, "EnemyAI")
            if ai then ai.xpValue = 0 end
            local be = ECSManager.world:getComponent(id, "BulletEmitter")
            if be then be.enabled = false end
        end
        return id
    end, onHitModifier))
    -- 12. XpCollection: XP 오브 자석 수집 + 레벨업
    ECSManager.addSystem(createXpCollectionSystem(getPlayerPos))
    -- 12.5. Boss: 보스 라이프사이클 (페이즈 + 패턴 순환 + 이동)
    ECSManager.addSystem(createBossSystem(ECSManager.bulletPool, getPlayerPos))
    -- 13-14. Render: draw()에서만 실행
    ECSManager.addSystem(RenderSystem)
    ECSManager.addSystem(PlayerRenderSystem)
end

return ECSManager