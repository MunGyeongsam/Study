-- Player Entity
-- 플레이어 엔티티 (간단한 구현, 추후 ECS로 확장 가능)

local logger = require("00_common.logger")
local world = require("01_core.world")

local player = {}

-- 플레이어 초기 설정
function player.init(startX, startY)
    -- 월드에서 시작 위치 가져오기 (매개변수 우선)
    local worldStartX, worldStartY = world.getStartPosition()
    
    player.data = {
        -- 위치 정보
        position = {
            x = startX or worldStartX,
            y = startY or worldStartY
        },
        
        -- 이동 관련
        velocity = { x = 0, y = 0 },
        speed = 2,  -- 유닛/초 (화면 높이를 1초에 횡단)
        
        -- 렌더링
        radius = 0.1,  -- 월드 좌표상 반지름 (화면 높이 10 기준 3% 크기)
        color = {0, 1, 1, 1},  -- 사이안 (미래적/벡터 게임 느낌)
        
        -- 상태
        health = 100,
        invulnerable = false,
        
        -- 수집 상태
        powerUps = {},
        checkpointsSaved = {},
        
        -- 현재 구역 정보
        currentZone = nil,
        zoneHistory = {}
    }
    
    logger.info(string.format("🎮 Player initialized at (%.1f, %.1f)", 
        player.data.position.x, player.data.position.y))
    
    return player.data
end

-- 업데이트 (매 프레임 호출)
function player.update(dt, inputState)
    if not player.data then return end
    
    -- 입력 처리
    player.handleInput(dt, inputState)
    
    -- 물리 업데이트 (위치 갱신)
    player.updatePhysics(dt)
    
    -- 월드 상호작용 체크
    player.checkWorldInteractions()
    
    -- 구역 변경 감지
    player.updateCurrentZone()
end

-- 입력 처리
function player.handleInput(dt, inputState)
    if not inputState then return end
    
    local vel = player.data.velocity
    vel.x, vel.y = 0, 0  -- 매 프레임 초기화
    
    -- 키보드 입력 (PC 테스트용)
    if love.keyboard.isDown("w", "up") then
        vel.y = vel.y + 1
    end
    if love.keyboard.isDown("s", "down") then
        vel.y = vel.y - 1
    end
    if love.keyboard.isDown("a", "left") then
        vel.x = vel.x - 1
    end
    if love.keyboard.isDown("d", "right") then
        vel.x = vel.x + 1
    end
    
    -- 터치 입력 (모바일용, 추후 구현)
    if inputState.touchDirection then
        vel.x = vel.x + inputState.touchDirection.x
        vel.y = vel.y + inputState.touchDirection.y
    end
    
    -- 속도 정규화 (대각선 이동 보정)
    local magnitude = math.sqrt(vel.x^2 + vel.y^2)
    if magnitude > 0 then
        vel.x = (vel.x / magnitude) * player.data.speed
        vel.y = (vel.y / magnitude) * player.data.speed
    end
end

-- 물리 업데이트
function player.updatePhysics(dt)
    local pos = player.data.position
    local vel = player.data.velocity
    
    -- 위치 갱신
    pos.x = pos.x + vel.x * dt
    pos.y = pos.y + vel.y * dt
    
    -- 월드 경계 체크
    local worldLeft, worldBottom, worldRight, worldTop = world.getBounds()
    pos.x = math.max(worldLeft + player.data.radius, 
             math.min(worldRight - player.data.radius, pos.x))
    pos.y = math.max(worldBottom + player.data.radius,
             math.min(worldTop - player.data.radius, pos.y))
end

-- 월드 상호작용 체크
function player.checkWorldInteractions()
    local pos = player.data.position
    
    -- 파워업 수집 체크
    local collectedPowerUp = world.collectPowerUp(pos.x, pos.y, player.data.radius)
    if collectedPowerUp then
        table.insert(player.data.powerUps, collectedPowerUp)
        logger.info("🎁 Player collected: " .. collectedPowerUp)
    end
    
    -- 체크포인트 활성화 체크
    local activatedCheckpoint = world.activateCheckpoint(pos.x, pos.y, player.data.radius * 2)
    if activatedCheckpoint then
        table.insert(player.data.checkpointsSaved, activatedCheckpoint)
        logger.info("🏁 Checkpoint " .. activatedCheckpoint .. " activated!")
    end
    
    -- 비밀 통로 발견 체크
    local discoveredSecret = world.discoverSecretPath(pos.x, pos.y, player.data.radius * 1.5)
    if discoveredSecret then
        logger.info("🔍 Secret path " .. discoveredSecret .. " discovered!")
    end
end

-- 현재 구역 업데이트
function player.updateCurrentZone()
    local pos = player.data.position
    local zoneName, zoneData = world.getZoneAtPosition(pos.x, pos.y)
    
    if zoneName ~= player.data.currentZone then
        -- 구역 변경됨
        if player.data.currentZone then
            logger.info("📍 Left zone: " .. player.data.currentZone)
        end
        
        player.data.currentZone = zoneName
        
        if zoneName then
            logger.info("📍 Entered zone: " .. zoneName .. " (" .. zoneData.description .. ")")
            table.insert(player.data.zoneHistory, zoneName)
        end
    end
end

-- 렌더링
function player.draw(camera)
    if not player.data then return end
    
    local lg = love.graphics
    local pos = player.data.position
    
    -- 상태 저장
    local r, g, b, a = lg.getColor()
    local lineWidth = lg.getLineWidth()
    
    -- 카메라 변환 적용 (카메라가 이미 변환을 처리하므로 월드 좌표 그대로 사용)
    
    -- 플레이어 본체 그리기
    lg.setColor(player.data.color)
    
    -- 픽셀 변환을 위한 크기 계산
    local pixelToWorld = 1
    if camera then
        pixelToWorld = camera:getUnitsPerPixel()
    end
    
    local drawRadius = player.data.radius
    
    -- 플레이어 원 그리기
    lg.circle("fill", pos.x, pos.y, drawRadius)
    
    -- 외곽선 (더 진한 사이안)
    lg.setColor(0, 0.7, 0.7, 1)
    lg.setLineWidth(pixelToWorld * 2)
    lg.circle("line", pos.x, pos.y, drawRadius * 1.2)
    
    -- 방향 표시 (작은 선)
    if player.data.velocity.x ~= 0 or player.data.velocity.y ~= 0 then
        local vel = player.data.velocity
        local magnitude = math.sqrt(vel.x^2 + vel.y^2)
        if magnitude > 0 then
            local dirX = vel.x / magnitude
            local dirY = vel.y / magnitude
            
            lg.setColor(1, 1, 1, 0.8)
            lg.setLineWidth(pixelToWorld * 3)
            lg.line(
                pos.x, pos.y,
                pos.x + dirX * drawRadius * 2,
                pos.y + dirY * drawRadius * 2
            )
        end
    end
    
    -- 상태 복원
    lg.setColor(r, g, b, a)
    lg.setLineWidth(lineWidth)
end

-- 위치 정보 반환
function player.getPosition()
    if not player.data then return 60, 25 end  -- 기본값
    return player.data.position.x, player.data.position.y
end

-- 카메라가 따라갈 대상 위치
function player.getCameraTarget()
    return player.getPosition()
end

-- 플레이어 상태 정보 반환 (UI용)
function player.getStats()
    if not player.data then 
        return {
            health = 0,
            powerUps = {},
            currentZone = "none",
            progress = 0
        }
    end
    
    local progress = world.getProgressPercentage(player.data.position.y)
    
    return {
        health = player.data.health,
        powerUps = player.data.powerUps,
        currentZone = player.data.currentZone or "outside",
        progress = progress,
        zonesVisited = #player.data.zoneHistory,
        checkpoints = #player.data.checkpointsSaved
    }
end

-- 플레이어 리셋 (새 게임)
function player.reset()
    local startX, startY = world.getStartPosition()
    player.init(startX, startY)
end

return player