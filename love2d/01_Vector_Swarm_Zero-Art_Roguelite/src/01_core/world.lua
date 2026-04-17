-- World System
-- 재미 극대화를 위한 확장된 월드 시스템

local logger = require("00_common.logger")

local world = {}

-- 🎮 재미 중심 월드 크기 (화면의 12x25배)
world.size = {
    width = 120,    -- 12배 확장 (화면 높이 10유닛 x 12)
    height = 250   -- 25배 확장 (화면 높이 10유닛 x 25) 
}

-- 월드 중심점 (플레이어 시작 위치)
world.center = {
    x = 0,    -- 중앙 (0,0 기준)
    y = -100  -- 하단에서 시작 (상향 진행)
}

-- 🗺️ 재미 구역 정의 (기획 기반)
world.zones = {
    -- 안전 시작 구역 (튜토리얼)
    safe_start = {
        bounds = {x1 = -20, y1 = -125, x2 = 20, y2 = -85},
        type = "safe",
        description = "플레이어 시작 및 학습 구역",
        color = {0, 1, 0, 0.1}  -- 초록
    },
    
    -- 초급 탐험 구역 
    exploration_easy = {
        bounds = {x1 = -40, y1 = -85, x2 = 40, y2 = -25},
        type = "exploration", 
        description = "기본 적과 아이템이 있는 탐험 구역",
        color = {0, 0, 1, 0.1}  -- 파랑
    },
    
    -- 중급 전투 구역
    combat_medium = {
        bounds = {x1 = -50, y1 = -25, x2 = 50, y2 = 35},
        type = "combat",
        description = "중간 난이도 탄막 패턴 구역", 
        color = {1, 1, 0, 0.1}  -- 노랑
    },
    
    -- 고급 위험 구역
    danger_zone = {
        bounds = {x1 = -60, y1 = 35, x2 = 60, y2 = 95},
        type = "danger",
        description = "고난이도 적과 보상이 있는 위험 구역",
        color = {1, 0.5, 0, 0.1}  -- 주황
    },
    
    -- 최종 보스 구역
    boss_chamber = {
        bounds = {x1 = -30, y1 = 95, x2 = 30, y2 = 125},
        type = "boss",
        description = "최종 보스와의 결전 구역",
        color = {1, 0, 0, 0.1}  -- 빨강
    },
    
    -- 숨겨진 보상 구역들
    secret_left = {
        bounds = {x1 = -60, y1 = -65, x2 = -40, y2 = 15},
        type = "secret",
        description = "숨겨진 파워업과 보너스",
        color = {1, 0, 1, 0.1}  -- 자주
    },
    
    secret_right = {
        bounds = {x1 = 40, y1 = -45, x2 = 60, y2 = 55},
        type = "secret", 
        description = "레어 아이템과 특별 능력",
        color = {1, 0, 1, 0.1}  -- 자주
    }
}

-- 🎯 재미 요소들 (랜덤 배치될 오브젝트)
world.funElements = {
    -- 파워업 포인트들
    powerUps = {
        {x = -10, y = -55, type = "speed", active = true},
        {x = 10, y = -35, type = "shield", active = true},
        {x = -30, y = 5, type = "multishot", active = true},
        {x = 30, y = 25, type = "laser", active = true},
        {x = 0, y = 65, type = "ultimate", active = true}
    },
    
    -- 체크포인트 (세이브 포인트)
    checkpoints = {
        {x = 0, y = -75, active = false},  -- 첫 번째 체크포인트
        {x = 0, y = -15, active = false},  -- 중간 체크포인트  
        {x = 0, y = 45, active = false},  -- 고급 체크포인트
        {x = 0, y = 85, active = false}  -- 보스 직전
    },
    
    -- 숨겨진 통로들
    secretPaths = {
        {x1 = -44, y1 = -55, x2 = -36, y2 = 5, discovered = false},
        {x1 = 36, y1 = -35, x2 = 44, y2 = 45, discovered = false}
    }
}

-- 초기화
function world.init()
    logger.info("[WORLD] Expanded world system initialized")
    logger.info(string.format("World size: %.0fx%.0f units (%.0fx%.0f screen ratio)", 
        world.size.width, world.size.height, 
        world.size.width/10, world.size.height/10))  -- 10유닛 = 2배 화면 가정
    logger.info(string.format("Player starts at: (%.0f, %.0f)", world.center.x, world.center.y))
    
    -- 구역별 정보 출력
    local zoneCount = 0
    for name, zone in pairs(world.zones) do
        zoneCount = zoneCount + 1
    end
    logger.info(string.format("[WORLD] %d zones configured", zoneCount))
end

-- 🎨 확장된 월드 그리드 그리기 (구역별 색상 표시)
function world.drawGrid(gridSize, camera, showZones)
    local lg = love.graphics
    gridSize = gridSize or 5  -- 확장된 월드에서는 5유닛 간격이 적당
    showZones = showZones ~= false  -- 기본값 true
    
    -- 카메라의 orthographic size를 사용한 정확한 픽셀-월드 변환
    local pixelToWorld = 1
    if camera then
        pixelToWorld = camera:getUnitsPerPixel()
    else
        -- 카메라가 없는 경우 기본값 사용 (fallback)
        local _, screenHeight = lg.getDimensions()
        local defaultOrthographicSize = 5  -- 기본값
        pixelToWorld = (defaultOrthographicSize * 2) / screenHeight
    end
    
    -- 상태 저장
    local r, g, b, a = lg.getColor()
    local lineWidth = lg.getLineWidth()
    
    -- 🗺️ 구역 배경 그리기 (재미 요소 시각화)
    if showZones then
        world.drawZones(pixelToWorld)
    end
    
    -- 기본 그리드 그리기 (회색, 얇게)
    lg.setColor(0.2, 0.2, 0.2, 0.4)
    lg.setLineWidth(pixelToWorld * 0.5)  -- 0.5픽셀 두께
    
    -- 세로선들 (X축 방향)
    for x = -world.size.width/2, world.size.width/2, gridSize do
        lg.line(x, -world.size.height/2, x, world.size.height/2)
    end
    
    -- 가로선들 (Y축 방향)  
    for y = -world.size.height/2, world.size.height/2, gridSize do
        lg.line(-world.size.width/2, y, world.size.width/2, y)
    end
    
    -- 🎯 중요 축 강조 표시
    lg.setLineWidth(pixelToWorld * 2)  -- 2픽셀 두께
    
    -- 세로 중앙선 (X=0, 초록색)
    lg.setColor(0, 0.8, 0, 0.8)
    lg.line(0, -world.size.height/2, 0, world.size.height/2)
    
    -- 가로 중앙선 (Y=0, 초록색)
    lg.line(-world.size.width/2, 0, world.size.width/2, 0)
    
    -- 구역 경계선들 (노란색)
    lg.setColor(1, 1, 0, 0.6)
    lg.line(-world.size.width/2, -85, world.size.width/2, -85)  -- 시작-탐험 경계
    lg.line(-world.size.width/2, -25, world.size.width/2, -25)  -- 탐험-전투 경계  
    lg.line(-world.size.width/2, 35, world.size.width/2, 35)  -- 전투-위험 경계
    lg.line(-world.size.width/2, 95, world.size.width/2, 95)  -- 위험-보스 경계
    
    -- 🏠 플레이어 시작점 마커 (큰 초록 원)
    lg.setColor(0, 1, 0, 1)
    local markerSize = pixelToWorld * 8
    lg.circle("fill", world.center.x, world.center.y, markerSize)
    lg.setColor(0, 0.5, 0, 1)
    lg.circle("line", world.center.x, world.center.y, markerSize * 1.3)
    
    -- 시작점 레이블
    lg.setColor(1, 1, 1, 1)
    lg.push()
    lg.translate(world.center.x + markerSize * 2, world.center.y)
    
    local textScale = pixelToWorld * 0.8  -- 크기 대폭 줄임 (12 → 0.8)
    lg.scale(textScale, -textScale)  -- Y축 반전으로 텍스트 정상화
    lg.print("START")
    lg.pop()
    
    -- 🎯 재미 요소들 그리기
    world.drawFunElements(pixelToWorld)
    
    -- 상태 복원
    lg.setColor(r, g, b, a)
    lg.setLineWidth(lineWidth)
end

-- 🎨 구역별 배경 그리기
function world.drawZones(pixelToWorld)
    local lg = love.graphics
    
    for name, zone in pairs(world.zones) do
        lg.setColor(zone.color[1], zone.color[2], zone.color[3], zone.color[4])
        
        local x1, y1 = zone.bounds.x1, zone.bounds.y1
        local x2, y2 = zone.bounds.x2, zone.bounds.y2
        
        -- 구역 배경 사각형
        lg.rectangle("fill", x1, y1, x2 - x1, y2 - y1)
        
        -- 구역 레이블
        lg.setColor(1, 1, 1, 0.8)
        local textScale = pixelToWorld * 0.8  -- 크기 대폭 줄임 (12 → 0.8)
        lg.push()
        lg.translate(x1 + 2, y1 + 2)
        lg.scale(textScale, -textScale)  -- Y축 반전으로 텍스트 정상화
        lg.print(zone.type:upper())
        lg.pop()
    end
end

-- 🎯 재미 요소들 그리기
function world.drawFunElements(pixelToWorld)
    local lg = love.graphics
    
    -- 파워업 그리기
    for i, powerUp in ipairs(world.funElements.powerUps) do
        if powerUp.active then
            -- 파워업별 색상
            local colors = {
                speed = {0, 1, 1},      -- 하늘색
                shield = {0, 0, 1},     -- 파랑  
                multishot = {1, 0.5, 0}, -- 주황
                laser = {1, 0, 1},      -- 자주
                ultimate = {1, 1, 0}    -- 노랑
            }
            
            local color = colors[powerUp.type] or {1, 1, 1}
            lg.setColor(color[1], color[2], color[3], 0.8)
            
            local size = pixelToWorld * 6
            lg.circle("fill", powerUp.x, powerUp.y, size)
            lg.setColor(1, 1, 1, 1)
            lg.circle("line", powerUp.x, powerUp.y, size * 1.2)
        end
    end
    
    -- 체크포인트 그리기
    for i, checkpoint in ipairs(world.funElements.checkpoints) do
        local color = checkpoint.active and {0, 1, 0} or {0.5, 0.5, 0.5}
        lg.setColor(color[1], color[2], color[3], 0.7)
        
        local size = pixelToWorld * 4
        lg.rectangle("fill", checkpoint.x - size, checkpoint.y - size, size * 2, size * 2)
    end
    
    -- 숨겨진 통로 그리기 (발견된 경우만)
    lg.setColor(1, 0, 1, 0.6)
    lg.setLineWidth(pixelToWorld * 3)
    for i, path in ipairs(world.funElements.secretPaths) do
        if path.discovered then
            lg.line(path.x1, path.y1, path.x2, path.y2)
        end
    end
end

-- 🎮 게임플레이 함수들

-- 특정 위치가 어떤 구역에 있는지 확인
function world.getZoneAtPosition(x, y)
    for name, zone in pairs(world.zones) do
        if x >= zone.bounds.x1 and x <= zone.bounds.x2 and
           y >= zone.bounds.y1 and y <= zone.bounds.y2 then
            return name, zone
        end
    end
    return nil, nil
end

-- 파워업 수집
function world.collectPowerUp(playerX, playerY, radius)
    radius = radius or 2  -- 수집 반지름
    
    for i, powerUp in ipairs(world.funElements.powerUps) do
        if powerUp.active then
            local distance = math.sqrt((powerUp.x - playerX)^2 + (powerUp.y - playerY)^2)
            if distance <= radius then
                powerUp.active = false  -- 수집됨
                logger.info("[WORLD] Power-up collected: " .. powerUp.type)
                return powerUp.type
            end
        end
    end
    return nil
end

-- 체크포인트 활성화
function world.activateCheckpoint(playerX, playerY, radius)
    radius = radius or 3  -- 활성화 반지름
    
    for i, checkpoint in ipairs(world.funElements.checkpoints) do
        if not checkpoint.active then
            local distance = math.sqrt((checkpoint.x - playerX)^2 + (checkpoint.y - playerY)^2)
            if distance <= radius then
                checkpoint.active = true
                logger.info("[WORLD] Checkpoint activated!")
                return i
            end
        end
    end
    return nil
end

-- 숨겨진 통로 발견
function world.discoverSecretPath(playerX, playerY, radius)
    radius = radius or 2  -- 발견 반지름
    
    for i, path in ipairs(world.funElements.secretPaths) do
        if not path.discovered then
            -- 통로의 중점과 거리 계산
            local midX = (path.x1 + path.x2) / 2
            local midY = (path.y1 + path.y2) / 2
            local distance = math.sqrt((midX - playerX)^2 + (midY - playerY)^2)
            
            if distance <= radius then
                path.discovered = true
                logger.info("[WORLD] Secret path discovered!")
                return i
            end
        end
    end
    return nil
end

-- 월드 크기 정보 반환 (카메라나 다른 시스템에서 사용)
function world.getSize()
    return world.size.width, world.size.height
end

-- 월드 경계 반환 (새로운 0,0 중심 크기)
function world.getBounds()
    return -world.size.width/2, -world.size.height/2, world.size.width/2, world.size.height/2  -- left, bottom, right, top
end

-- 플레이어 시작 위치 반환
function world.getStartPosition()
    return world.center.x, world.center.y
end

-- 🎲 재미 요소 리셋 (새 게임 시작시)
function world.resetFunElements()
    -- 모든 파워업 활성화
    for i, powerUp in ipairs(world.funElements.powerUps) do
        powerUp.active = true
    end
    
    -- 모든 체크포인트 비활성화
    for i, checkpoint in ipairs(world.funElements.checkpoints) do
        checkpoint.active = false
    end
    
    -- 모든 비밀 통로 숨김
    for i, path in ipairs(world.funElements.secretPaths) do
        path.discovered = false
    end
    
    logger.info("[WORLD] Fun elements reset for new game")
end

-- 🏆 게임 진행률 계산 (0,0 중심 기준)
function world.getProgressPercentage(playerY)
    -- Y가 -125에서 시작해서 125로 가므로 보정
    local normalizedY = (playerY + world.size.height/2)  -- 0~250 범위로 보정
    local progress = math.max(0, math.min(100, (normalizedY / world.size.height) * 100))
    return progress
end

-- 🌟 월드 통계 반환 (UI 표시용)
function world.getWorldStats()
    local activePowerUps = 0
    local activeCheckpoints = 0
    local discoveredPaths = 0
    local totalZones = 0
    
    -- 구역 수 계산
    for name, zone in pairs(world.zones) do
        totalZones = totalZones + 1
    end
    
    for i, powerUp in ipairs(world.funElements.powerUps) do
        if powerUp.active then activePowerUps = activePowerUps + 1 end
    end
    
    for i, checkpoint in ipairs(world.funElements.checkpoints) do
        if checkpoint.active then activeCheckpoints = activeCheckpoints + 1 end
    end
    
    for i, path in ipairs(world.funElements.secretPaths) do
        if path.discovered then discoveredPaths = discoveredPaths + 1 end
    end
    
    return {
        totalZones = totalZones,
        powerUpsRemaining = activePowerUps,
        checkpointsActivated = activeCheckpoints,
        secretsDiscovered = discoveredPaths,
        worldSize = string.format("%.0f x %.0f", world.size.width, world.size.height)
    }
end

return world