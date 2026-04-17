-- ECS Manager
-- 전체 ECS 월드와 시스템들을 관리

local ECS = require("01_core.ecs")

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
local createPlayerWeaponSystem  = require("03_game.systems.playerWeaponSystem")
local createEnemyAISystem       = require("03_game.systems.enemyAISystem")
local DashSystem                = require("03_game.systems.dashSystem")
local FocusSystem               = require("03_game.systems.focusSystem")
local createXpCollectionSystem  = require("03_game.systems.xpCollectionSystem")
local EnemySpawner              = require("03_game.systems.enemySpawner")

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

    -- World bounds for bullet culling
    local world = require("01_core.world")
    local halfW = world.size.width  / 2
    local halfH = world.size.height / 2
    ECSManager.bulletBounds = {
        minX = -halfW, maxX = halfW,
        minY = world.center.y - halfH, maxY = world.center.y + halfH,
    }

    -- Enemy spawner
    ECSManager.enemySpawner = EnemySpawner.new(ECSManager, ECSManager.getPlayerPos)

    logInfo("[ECS] ECS Manager initialized")
    
    -- 기본 시스템들 등록
    ECSManager._registerBasicSystems()
    
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

-- 모든 시스템 업데이트 (Render 시스템 제외 — draw()에서만 실행)
function ECSManager.update(dt)
    if not ECSManager.world then
        return
    end
    
    -- 렌더 시스템 이름 목록 (draw에서만 실행)
    local renderSystems = { Render = true, PlayerRender = true }

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
    ECSManager.bulletPool:update(dt, ECSManager.bulletBounds)

    -- Enemy spawner
    ECSManager.enemySpawner:update(dt)
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
function ECSManager.createPlayer(x, y)
    if not ECSManager.world then
        logError("ECS: World not initialized")
        return nil
    end
    return EntityFactory.createPlayer(ECSManager.world, x, y)
end

-- 적 엔티티 생성 (EntityFactory 위임)
function ECSManager.createEnemy(x, y, enemyType)
    if not ECSManager.world then
        logError("ECS: World not initialized")
        return nil
    end
    return EntityFactory.createEnemy(ECSManager.world, x, y, enemyType)
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
        spawner = ECSManager.enemySpawner:getStats(),
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

    -- 스포너 리셋
    ECSManager.enemySpawner = EnemySpawner.new(ECSManager, ECSManager.getPlayerPos)

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
    ECSManager.addSystem(createPlayerWeaponSystem(ECSManager.bulletPool))
    -- 10. Collision: 플레이어 ↔ 적 불릿 충돌
    ECSManager.addSystem(createCollisionSystem(ECSManager.bulletPool))
    -- 11. EnemyCollision: 플레이어 불릿 ↔ 적 충돌 + XP 드롭
    local onEnemyDeath = function(ecs, x, y, xpValue)
        EntityFactory.createXpOrb(ecs, x, y, xpValue)
    end
    ECSManager.addSystem(createEnemyCollisionSystem(ECSManager.bulletPool, onEnemyDeath))
    -- 12. XpCollection: XP 오브 자석 수집 + 레벨업
    ECSManager.addSystem(createXpCollectionSystem(getPlayerPos))
    -- 13-14. Render: draw()에서만 실행
    ECSManager.addSystem(RenderSystem)
    ECSManager.addSystem(PlayerRenderSystem)
end

return ECSManager