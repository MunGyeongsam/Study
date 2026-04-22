-- Top HUD
-- 상단 영역 UI (점수, 설정, 상태 정보)

local logger     = require("00_common.logger")
local mobileLayout = require("04_ui.mobileLayout")
local mathUtil   = require("00_common.mathUtil")

local _max = math.max

local topHud = {}

-- 게임 데이터
local gameData = {
    score = 0,
    lives = 3,
    level = 1,
    fps = 0,
    -- Boss data
    bossActive = false,
    bossName = "",
    bossHp = 0,
    bossMaxHp = 1,
    bossPhase = 1,
    bossMaxPhase = 1,
    -- Smoothed display value
    bossDisplayHp = 0,
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

-- 프레임 업데이트 (보스 HP 바 슬라이딩 등)
function topHud.update(dt)
    if gameData.bossActive then
        gameData.bossDisplayHp = mathUtil.expDecay(gameData.bossDisplayHp, gameData.bossHp, 4, dt)
    end
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
    
    -- 스테이지/웨이브 표시
    local stageNum = gameData.stage or 1
    local stageText
    if stageNum > 15 then
        local endlessRound = math.floor((stageNum - 16) / 15) + 1
        stageText = string.format("ENDLESS +%d  Stage %d  Wave %d/%d",
            endlessRound, stageNum, gameData.wave or 0, gameData.wavesPerStage or 5)
    else
        stageText = string.format("Stage %d  Wave %d/%d",
            stageNum, gameData.wave or 0, gameData.wavesPerStage or 5)
    end
    lg.print(stageText, 10, 25)
    
    -- 라이프 표시
    local livesText = string.format("HP x %d", gameData.lives)
    lg.print(livesText, 10, 40)
    
    -- 레벨과 진행률 표시
    local levelText = string.format("Lv.%d", gameData.level)
    if gameData.progress then
        levelText = levelText .. string.format(" (%.0f%%)", gameData.progress)
    end
    lg.print(levelText, 150, 10)
    
    -- 수집 요소 표시
    if gameData.powerUps then
        local powerUpText = string.format("PWR:%d", gameData.powerUps)
        lg.print(powerUpText, 150, 25)
    end
    
    if gameData.secrets then
        local secretText = string.format("SEC:%d", gameData.secrets)
        lg.print(secretText, 200, 25)
    end
    
    -- FPS 표시 (가운데)
    local fpsText = string.format("FPS: %d", gameData.fps)
    local centerX = layout.screenWidth / 2
    local textWidth = font:getWidth(fpsText)
    lg.print(fpsText, centerX - textWidth/2, 10)
    
    -- 설정 버튼 그리기
    topHud.drawSettingsButton()

    -- 보스 HP바 그리기 (보스 활성 시)
    if gameData.bossActive then
        topHud.drawBossHpBar()
    end
    
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
    if data.stage then gameData.stage = data.stage end
    if data.wave then gameData.wave = data.wave end
    if data.wavesPerStage then gameData.wavesPerStage = data.wavesPerStage end
    -- Boss data
    if data.bossActive ~= nil then
        -- bossActive가 켜지는 순간 displayHp를 초기화
        if data.bossActive and not gameData.bossActive then
            gameData.bossDisplayHp = data.bossHp or gameData.bossHp
        end
        gameData.bossActive = data.bossActive
    end
    if data.bossName then gameData.bossName = data.bossName end
    if data.bossHp then gameData.bossHp = data.bossHp end
    if data.bossMaxHp then gameData.bossMaxHp = data.bossMaxHp end
    if data.bossPhase then gameData.bossPhase = data.bossPhase end
    if data.bossMaxPhase then gameData.bossMaxPhase = data.bossMaxPhase end
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

-- 보스 HP바 렌더링
function topHud.drawBossHpBar()
    local lg = love.graphics
    local layout = mobileLayout.getLayout()
    local w = layout.screenWidth

    local barW = w * 0.6
    local barH = 10
    local barX = (w - barW) / 2
    local barY = layout.topAreaHeight - barH - 4

    -- Background
    lg.setColor(0.2, 0.2, 0.2, 0.9)
    lg.rectangle("fill", barX - 1, barY - 1, barW + 2, barH + 2)

    -- HP fill
    local ratio = _max(0, gameData.bossDisplayHp / _max(1, gameData.bossMaxHp))
    -- Damage flash: show instant HP behind smoothed bar
    local instantRatio = _max(0, gameData.bossHp / _max(1, gameData.bossMaxHp))
    -- Color: green→yellow→red
    local r = ratio < 0.5 and 1 or (1 - (ratio - 0.5) * 2)
    local g = ratio > 0.5 and 1 or (ratio * 2)

    -- Instant (behind) bar — white flash
    if ratio > instantRatio + 0.005 then
        lg.setColor(1, 1, 1, 0.4)
        lg.rectangle("fill", barX, barY, barW * ratio, barH)
    end

    -- Smoothed bar
    lg.setColor(r, g, 0.1, 1)
    lg.rectangle("fill", barX, barY, barW * instantRatio, barH)

    -- Phase markers
    if gameData.bossMaxPhase > 1 then
        lg.setColor(1, 1, 1, 0.5)
        for i = 1, gameData.bossMaxPhase - 1 do
            local markerRatio = i / gameData.bossMaxPhase
            local mx = barX + barW * (1 - markerRatio)
            lg.rectangle("fill", mx - 1, barY, 2, barH)
        end
    end

    -- Boss name
    lg.setColor(1, 0.3, 0.3, 1)
    local name = gameData.bossName or "BOSS"
    local font = lg.getFont()
    local tw = font:getWidth(name)
    lg.print(name, (w - tw) / 2, barY - font:getHeight() - 2)

    lg.setColor(1, 1, 1, 1)
end

-- 설정 버튼 액션
function topHud.onSettingsPressed()
    -- TODO: 설정 메뉴 열기
    logger.info("Settings menu would open here")
end

return topHud