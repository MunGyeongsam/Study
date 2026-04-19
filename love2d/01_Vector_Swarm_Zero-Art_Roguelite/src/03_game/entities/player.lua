-- Player Entity (ECS Façade)
-- ECS 엔티티를 감싸는 얇은 인터페이스.
-- main.lua에서 호출하는 API(getPosition, getCameraTarget, getStats, reset)를 유지하면서
-- 내부는 ecsManager/ECS 월드로 위임한다.

local world = require("01_core.world")

local player = {}

-- ECS 참조 (init 시 설정)
local ecsWorld    = nil   -- ECS.new() 인스턴스
local entityId    = nil   -- 플레이어 엔티티 ID

-- ECS 엔티티 ID 설정 (ecsManager에서 호출)
function player.bind(ecs, id)
    ecsWorld = ecs
    entityId = id
end

-- 초기화 (main.lua에서 호출)
function player.init(startX, startY)
    -- bind()가 먼저 호출된 경우: Transform 위치를 시작점으로 설정
    if ecsWorld and entityId then
        local worldStartX, worldStartY = world.getStartPosition()
        local transform = ecsWorld:getComponent(entityId, "Transform")
        if transform then
            transform.x = startX or worldStartX
            transform.y = startY or worldStartY
        end
        logInfo(string.format("[PLAYER] Initialized at (%.1f, %.1f)",
            transform.x, transform.y))
    end
end

-- 업데이트 — 입력/물리/경계는 ECS 시스템이 처리.
-- 월드 상호작용과 구역 감지만 여기서 처리한다.
function player.update(dt, inputState)
    if not ecsWorld or not entityId then return end

    local transform = ecsWorld:getComponent(entityId, "Transform")
    local tag       = ecsWorld:getComponent(entityId, "PlayerTag")
    if not transform or not tag then return end

    -- 월드 상호작용 체크
    local collider = ecsWorld:getComponent(entityId, "Collider")
    local radius = collider and collider.radius or 0.1

    local collectedPowerUp = world.collectPowerUp(transform.x, transform.y, radius)
    if collectedPowerUp then
        table.insert(tag.powerUps, collectedPowerUp)
        logInfo(string.format("[PLAYER] Collected: %s", collectedPowerUp))
    end

    local activatedCheckpoint = world.activateCheckpoint(transform.x, transform.y, radius * 2)
    if activatedCheckpoint then
        table.insert(tag.checkpointsSaved, activatedCheckpoint)
        logInfo(string.format("[PLAYER] Checkpoint %s activated!", activatedCheckpoint))
    end

    local discoveredSecret = world.discoverSecretPath(transform.x, transform.y, radius * 1.5)
    if discoveredSecret then
        logInfo(string.format("[PLAYER] Secret path %s discovered!", discoveredSecret))
    end

    -- 구역 변경 감지
    local zoneName, zoneData = world.getZoneAtPosition(transform.x, transform.y)
    if zoneName ~= tag.currentZone then
        if tag.currentZone then
            logInfo(string.format("[ZONE] Left: %s", tag.currentZone))
        end
        tag.currentZone = zoneName
        if zoneName then
            logInfo(string.format("[ZONE] Entered: %s (%s)", zoneName, zoneData.description))
            table.insert(tag.zoneHistory, zoneName)
        end
    end
end

-- 렌더링 — PlayerRenderSystem이 담당. 빈 함수로 유지.
-- 위치 정보 반환
function player.getPosition()
    if not ecsWorld or not entityId then return 0, 0 end
    local transform = ecsWorld:getComponent(entityId, "Transform")
    if not transform then return 0, 0 end
    return transform.x, transform.y
end

-- 엔티티 ID 반환
function player.getEntityId()
    return entityId
end

-- 카메라가 따라갈 대상 위치
function player.getCameraTarget()
    return player.getPosition()
end

-- 플레이어 상태 정보 반환 (UI용)
function player.getStats()
    if not ecsWorld or not entityId then
        return {
            health = 0, powerUps = {},
            currentZone = "none", progress = 0,
            zonesVisited = 0, checkpoints = 0,
        }
    end

    local transform = ecsWorld:getComponent(entityId, "Transform")
    local tag       = ecsWorld:getComponent(entityId, "PlayerTag")
    if not transform or not tag then
        return {
            health = 0, powerUps = {},
            currentZone = "none", progress = 0,
            zonesVisited = 0, checkpoints = 0,
        }
    end

    local progress = world.getProgressPercentage(transform.y)

    local health = ecsWorld:getComponent(entityId, "Health")
    local hp = health and health.hp or 0

    return {
        health = hp,
        powerUps = tag.powerUps,
        currentZone = tag.currentZone or "outside",
        progress = progress,
        zonesVisited = #tag.zoneHistory,
        checkpoints = #tag.checkpointsSaved,
    }
end

return player