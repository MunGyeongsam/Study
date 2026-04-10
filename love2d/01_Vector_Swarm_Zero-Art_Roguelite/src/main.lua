-- Vector Swarm - Zero Art Roguelite
-- Main game file

-- Global utilities first (전역 함수 최우선 초기화)
local global = require("00_common.global")

-- Load modules
local logger = require("00_common.logger")
local debug = require("00_common.debug")
local screenDebugDraw = require("00_common.grid_debug_draw")
local world = require("01_core.world")
local camera = require("02_renderer.camera")
local uiManager = require("04_ui.uiManager")  -- UI 시스템 추가

local mainCamera = nil  -- 전역 카메라 인스턴스 (필요 시 생성)

function love.load()
    -- Initialize global utilities first
    global.init()
    
    -- Initialize logging second
    logger.init()

    -- Set up multiple font sizes
    fonts = {
        small = love.graphics.newFont(12),    -- 작은 폰트 (디버그용)
        medium = love.graphics.newFont(16),   -- 중간 폰트 (기본)
        large = love.graphics.newFont(20),    -- 큰 폰트 (제목용)
        xlarge = love.graphics.newFont(24)    -- 매우 큰 폰트 (특별용)
    }
    love.graphics.setFont(fonts.medium)  -- 기본 폰트를 중간 크기로 설정

    -- Unity 스타일 카메라 생성
    local orthographicSize = 5
    mainCamera = camera.new(0, 0, orthographicSize)  -- 임시 위치
    
    -- Initialize core systems
    world.init()
    
    -- 🌍 카메라를 월드 시작 위치로 이동
    local startX, startY = world.getStartPosition()
    mainCamera:lookAt(startX, startY)
    logger.info(string.format("📍 Camera positioned at world start: (%.1f, %.1f)", startX, startY))
    
    -- Initialize UI system
    uiManager.init()
    
    -- UI 버튼 콜백 설정
    uiManager.setButtonCallbacks({
        onZoomIn = function()
            local newSize = mainCamera:getOrthographicSize() * 0.8
            mainCamera:setOrthographicSize(newSize)
            log(string.format("Zoom In - Orthographic Size: %.2f", newSize))
        end,
        onZoomOut = function()
            local newSize = mainCamera:getOrthographicSize() * 1.25
            mainCamera:setOrthographicSize(newSize)
            log(string.format("Zoom Out - Orthographic Size: %.2f", newSize))
        end,
        onReset = function()
            local startX, startY = world.getStartPosition()
            mainCamera:lookAt(startX, startY)
            mainCamera:setOrthographicSize(5)
            mainCamera:setViewportToCenter()
            log("Camera reset to world start position")
        end,
        onDebugToggle = function()
            uiManager.toggleDebugMode()
        end
    })
    
    log("All systems initialized successfully")
    log(string.format("Camera orthographic size: %.1f (viewing %.1f world units height)", 
         orthographicSize, orthographicSize * 2))
    log(string.format("Pixels per unit: %.1f", mainCamera:getPixelsPerUnit()))
    
end

function love.update(dt)
    -- UI 업데이트
    uiManager.update(dt)
    
    -- 게임 데이터를 UI에 전달
    local worldStats = world.getWorldStats()
    local currentX, currentY = mainCamera:pos()
    local progress = world.getProgressPercentage(currentY)
    
    uiManager.setGameData({
        score = 1250,  -- 예시 데이터
        lives = 3,
        level = math.floor(progress / 20) + 1,  -- 20%마다 레벨업
        fps = love.timer.getFPS(),
        progress = progress,  -- 월드 진행률
        powerUps = worldStats.powerUpsRemaining,
        secrets = worldStats.secretsDiscovered
    })
end

local function drawWorld()
    -- 월드 좌표계 그리드 (중심 0,0)
    world.drawGrid(2, mainCamera)  -- 2 유닛 간격 그리드, 카메라 정보 전달
end

function love.draw()
    -- 월드 렌더링 (카메라 적용)
    mainCamera:draw(drawWorld)
    
    -- UI 렌더링 (스크린 좌표계) 
    uiManager.draw()
    
    -- 디버그 정보 그리기 (기존 시스템)
    debug.draw()
    logger.drawConsole(fonts.small)  -- 인게임 디버그 콘솔 (작은 폰트)

    -- 스크린 좌표계 그리드 (참고용) - 주석 처리
    --screenDebugDraw.drawScreenGrid(50)
end

-- Debug console functions are now handled automatically by Logger

function love.keypressed(key)
    if key == "f1" then
        logger.toggleConsole()  -- F1키로 디버그 콘솔 토글
    elseif key == "f2" then
        uiManager.toggleVisibility()  -- F2키로 UI 토글
    elseif key == "f3" then
        uiManager.toggleDebugMode()  -- F3키로 UI 디버그 모드 토글
    end
    if key == 'escape' then
        love.event.quit()  -- ESC키로 게임 종료
    end
    
    -- Orthographic Size 조정 (Unity 스타일) - PC 프로토타입용
    if key == "=" or key == "+" then
        local newSize = mainCamera:getOrthographicSize() * 0.8  -- 줌인 (크기 축소)
        mainCamera:setOrthographicSize(newSize)
        log(string.format("Zoom In - Orthographic Size: %.2f", newSize))
    end
    if key == "-" then
        local newSize = mainCamera:getOrthographicSize() * 1.25  -- 줌아웃 (크기 확대)
        mainCamera:setOrthographicSize(newSize)
        log(string.format("Zoom Out - Orthographic Size: %.2f", newSize))
    end
    
    -- 카메라 이동 테스트 (PC 프로토타입)
    -- TODO: 모바일에서는 터치 드래그로 대체 예정
    local moveSpeed = 1
    if key == "w" then mainCamera:move(0, moveSpeed) end
    if key == "s" then mainCamera:move(0, -moveSpeed) end
    if key == "a" then mainCamera:move(-moveSpeed, 0) end
    if key == "d" then mainCamera:move(moveSpeed, 0) end
    if key == "space" then mainCamera:lookAt(0, 0) end  -- 원점으로 복귀 (더블탭으로 대체 가능)
    
    -- Viewport 테스트 (화살표 키)
    -- TODO: 모바일에서는 UI 버튼으로 대체 예정
    local w, h = love.graphics.getDimensions()
    if key == "up" then mainCamera:setViewport(w/2, h/4) end      -- 상단 
    if key == "down" then mainCamera:setViewport(w/2, h*3/4) end  -- 하단
    if key == "left" then mainCamera:setViewport(w/4, h/2) end    -- 좌측
    if key == "right" then mainCamera:setViewport(w*3/4, h/2) end -- 우측
    if key == "return" then mainCamera:setViewportToCenter() end  -- 중심 복귀
end

-- 모바일 터치 입력 처리
function love.touchpressed(id, x, y, dx, dy, pressure)
    -- UI 터치 처리
    if uiManager.touchpressed(id, x, y, dx, dy, pressure) then
        return  -- UI에서 터치를 소비했으면 게임 로직으로 전달하지 않음
    end
    
    -- 게임 플레이 영역 터치 처리
    local mobileLayout = require("04_ui.mobileLayout")
    if mobileLayout.isTouchInArea(x, y, "play") then
        local worldX, worldY = mainCamera:worldCoords(x, y)
        log(string.format("Touch at world: (%.1f, %.1f)", worldX, worldY))
    end
end

function love.touchmoved(id, x, y, dx, dy, pressure)
    -- UI 터치 이동 처리
    if uiManager.touchmoved(id, x, y, dx, dy, pressure) then
        return  -- UI에서 처리됨
    end
    
    -- 게임 플레이 영역에서 터치 드래그로 카메라 이동
    local mobileLayout = require("04_ui.mobileLayout")
    if mobileLayout.isTouchInArea(x, y, "play") then
        -- 터치 드래그로 카메라 이동
        mainCamera:move(-dx * mainCamera:getUnitsPerPixel(), 
                        dy * mainCamera:getUnitsPerPixel())  -- Y축 반전
    end
end

function love.touchreleased(id, x, y, dx, dy, pressure)
    -- UI 터치 릴리즈 처리
    uiManager.touchreleased(id, x, y, dx, dy, pressure)
end

-- PC 프로토타입용 마우스 입력을 터치로 시뮬레이션
function love.mousepressed(x, y, button, istouch, presses)
    if not istouch then  -- 실제 마우스 클릭인 경우만
        love.touchpressed("mouse", x, y, 0, 0, 1)
    end
end

function love.mousemoved(x, y, dx, dy, istouch)
    if not istouch and love.mouse.isDown(1) then  -- 마우스 드래그
        love.touchmoved("mouse", x, y, dx, dy, 1)
    end
end

function love.mousereleased(x, y, button, istouch, presses)
    if not istouch then  -- 실제 마우스 릴리즈인 경우만
        love.touchreleased("mouse", x, y, 0, 0, 1)
    end
end

-- Close log file when game quits
function love.quit()
    logger.close()
end

-- 화면 리사이즈 콜백
function love.resize(w, h)
    log(string.format("Screen resized to: %dx%d", w, h))
    -- UI 레이아웃 업데이트
    uiManager.updateLayout()
end
function love.resize(w, h)
    logger.info(string.format("Screen resized to %dx%d", w, h))
    
    -- Orthographic Size는 그대로, 픽셀당 유닛만 다시 계산됨 
    log(string.format("New pixels per unit: %.1f", mainCamera:getPixelsPerUnit()))
    --World.onResize()  -- 월드 좌표계 업데이트
end