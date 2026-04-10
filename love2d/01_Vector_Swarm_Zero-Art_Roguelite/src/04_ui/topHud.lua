-- Top HUD
-- 상단 영역 UI (점수, 설정, 상태 정보)

local logger = require("00_common.logger")
local mobileLayout = require("04_ui.mobileLayout")

local topHud = {}

-- 게임 데이터
local gameData = {
    score = 0,
    lives = 3,
    level = 1,
    fps = 0
}

-- UI 요소들
local elements = {
    settingsButton = {
        x = 0, y = 0, width = 0, height = 0,
        text = "⚙️",  -- 설정 아이콘
        visible = true
    }
}

-- 초기화
function topHud.init()
    logger.info("Top HUD initialized")
    topHud.updateLayout()
end

-- 레이아웃 업데이트
function topHud.updateLayout()
    local layout = mobileLayout.getLayout()
    local profile = mobileLayout.getDeviceProfile()
    
    -- 설정 버튼 위치 (우상단)
    local buttonSize = mobileLayout.getSafeTouchSize(32)
    elements.settingsButton.width = buttonSize
    elements.settingsButton.height = buttonSize
    elements.settingsButton.x = layout.screenWidth - buttonSize - 10
    elements.settingsButton.y = 10
end

-- 렌더링
function topHud.draw()
    local lg = love.graphics
    local layout = mobileLayout.getLayout()
    local profile = mobileLayout.getDeviceProfile()
    
    -- 상태 저장
    local r, g, b, a = lg.getColor()
    local font = lg.getFont()
    
    -- 배경 (반투명 검은색)
    lg.setColor(0, 0, 0, 0.7)
    lg.rectangle("fill", 0, 0, layout.screenWidth, layout.topAreaHeight)
    
    -- 텍스트 색상 설정
    lg.setColor(1, 1, 1, 1)  -- 흰색
    
    -- 점수 표시 (좌상단)
    local scoreText = string.format("Score: %d", gameData.score)
    lg.print(scoreText, 10, 10)
    
    -- 라이프 표시
    local livesText = string.format("♥ x %d", gameData.lives)
    lg.print(livesText, 10, 25)
    
    -- 레벨과 진행률 표시
    local levelText = string.format("Lv.%d", gameData.level)
    if gameData.progress then
        levelText = levelText .. string.format(" (%.0f%%)", gameData.progress)
    end
    lg.print(levelText, 150, 10)
    
    -- 수집 요소 표시
    if gameData.powerUps then
        local powerUpText = string.format("⚡:%d", gameData.powerUps)
        lg.print(powerUpText, 150, 25)
    end
    
    if gameData.secrets then
        local secretText = string.format("🔍:%d", gameData.secrets)
        lg.print(secretText, 200, 25)
    end
    
    -- FPS 표시 (가운데)
    local fpsText = string.format("FPS: %d", gameData.fps)
    local centerX = layout.screenWidth / 2
    local textWidth = font:getWidth(fpsText)
    lg.print(fpsText, centerX - textWidth/2, 10)
    
    -- 설정 버튼 그리기
    topHud.drawSettingsButton()
    
    -- 상태 복원
    lg.setColor(r, g, b, a)
    lg.setFont(font)
end

-- 설정 버튼 그리기
function topHud.drawSettingsButton()
    if not elements.settingsButton.visible then return end
    
    local lg = love.graphics
    local btn = elements.settingsButton
    
    -- 버튼 배경
    lg.setColor(0.3, 0.3, 0.3, 0.8)
    lg.rectangle("fill", btn.x, btn.y, btn.width, btn.height, 4, 4)
    
    -- 버튼 테두리
    lg.setColor(0.8, 0.8, 0.8, 1)
    lg.setLineWidth(2)
    lg.rectangle("line", btn.x, btn.y, btn.width, btn.height, 4, 4)
    
    -- 아이콘 텍스트
    lg.setColor(1, 1, 1, 1)
    local textX = btn.x + (btn.width - love.graphics.getFont():getWidth(btn.text)) / 2
    local textY = btn.y + (btn.height - love.graphics.getFont():getHeight()) / 2
    lg.print(btn.text, textX, textY)
end

-- 게임 데이터 업데이트
function topHud.setGameData(data)
    if data.score then gameData.score = data.score end
    if data.lives then gameData.lives = data.lives end
    if data.level then gameData.level = data.level end
    if data.fps then gameData.fps = data.fps end
end

-- 터치 입력 처리
function topHud.touchpressed(id, x, y, dx, dy, pressure)
    -- 상단 영역인지 확인
    if not mobileLayout.isTouchInArea(x, y, "top") then
        return false
    end
    
    -- 설정 버튼 클릭 확인
    if topHud.isPointInButton(x, y, elements.settingsButton) then
        logger.info("Settings button pressed")
        topHud.onSettingsPressed()
        return true
    end
    
    return false
end

-- 점이 버튼 영역에 있는지 확인
function topHud.isPointInButton(x, y, button)
    return x >= button.x and x <= (button.x + button.width) and
           y >= button.y and y <= (button.y + button.height)
end

-- 설정 버튼 액션
function topHud.onSettingsPressed()
    -- TODO: 설정 메뉴 열기
    logger.info("Settings menu would open here")
end

return topHud