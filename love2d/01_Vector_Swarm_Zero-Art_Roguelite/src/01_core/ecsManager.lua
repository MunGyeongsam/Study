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

-- Entity factories (03_game/entities/)
local EntityFactory = require("03_game.entities.entityFactory")

local ECSManager = {}

function ECSManager.init()
    ECSManager.world = ECS.new()
    ECSManager.systems = {}
    ECSManager.systemsOrder = {}  -- 실행 순서 보장
    
    logInfo("[ECS] ECS Manager initialized")
    
    -- 기본 시스템들 등록 (다음 단계에서 구현)
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

    -- 플레이어 전용 렌더 (외곽선, 방향)
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
        systems = systemStats
    }
end

-- ECS 월드 참조 반환 (player.lua bind용)
function ECSManager.getWorld()
    return ECSManager.world
end

-- 기본 시스템들 등록 (실행 순서가 중요!)
function ECSManager._registerBasicSystems()
    -- 1. Input: 키보드/터치 → Velocity
    ECSManager.addSystem(InputSystem)
    -- 2. Movement: Velocity → Transform
    ECSManager.addSystem(MovementSystem)
    -- 3. Boundary: 월드 경계 clamping
    ECSManager.addSystem(BoundarySystem)
    -- 4. LifeSpan: 수명 만료 제거
    ECSManager.addSystem(LifeSpanSystem)
    -- 5-6. Render: draw()에서만 실행
    ECSManager.addSystem(RenderSystem)
    ECSManager.addSystem(PlayerRenderSystem)
end

return ECSManager