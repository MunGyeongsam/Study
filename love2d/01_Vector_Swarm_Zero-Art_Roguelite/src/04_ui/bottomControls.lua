-- Bottom Controls
-- 하단 컨트롤 UI (줌, 리셋, 기타 버튼)

local logger = require("00_common.logger")
local mobileLayout = require("04_ui.mobileLayout")

local bottomControls = {}

-- 🔧 상수: 버튼 순서 (한 번만 생성, GC 부담 없음)
local BUTTON_ORDER = {"zoomOut", "zoomIn", "reset", "debug"}

-- 콜백 함수들
local callbacks = {
    onZoomIn = nil,
    onZoomOut = nil,
    onReset = nil,
    onDebugToggle = nil
}

-- UI 버튼들
local buttons = {
    zoomIn = {
        x = 0, y = 0, width = 0, height = 0,
        text = "+️",
        visible = true,
        pressed = false
    },
    zoomOut = {
        x = 0, y = 0, width = 0, height = 0,
        text = "-️",
        visible = true,
        pressed = false
    },
    reset = {
        x = 0, y = 0, width = 0, height = 0,
        text = "🏠",  -- 리셋 아이콘
        visible = true,
        pressed = false
    },
    debug = {
        x = 0, y = 0, width = 0, height = 0,
        text = "🔧",  -- 디버그 아이콘
        visible = true,
        pressed = false
    }
}

-- 초기화
function bottomControls.init()
    logger.info("Bottom controls initialized")
    bottomControls.updateLayout()
end

-- 레이아웃 업데이트
function bottomControls.updateLayout()
    local layout = mobileLayout.getLayout()
    local profile = mobileLayout.getDeviceProfile()
    
    local buttonSize = mobileLayout.getSafeTouchSize(50)
    local spacing = profile.buttonSpacing
    local totalWidth = (buttonSize * 4) + (spacing * 3)
    local startX = (layout.screenWidth - totalWidth) / 2
    local buttonY = layout.bottomAreaStart + (layout.bottomAreaHeight - buttonSize) / 2
    
    -- 버튼 위치 설정 (순서 보장, GC 부담 없음)
    for buttonIndex, buttonName in ipairs(BUTTON_ORDER) do
        local button = buttons[buttonName]
        if button then
            button.x = startX + ((buttonIndex - 1) * (buttonSize + spacing))
            button.y = buttonY
            button.width = buttonSize
            button.height = buttonSize
        end
    end
end

-- 업데이트
function bottomControls.update(dt)
    -- 버튼 프레스 애니메이션 처리
    for name, button in pairs(buttons) do
        if button.pressed then
            -- 프레스 효과 감소
            button.pressed = false
        end
    end
end

-- 렌더링
function bottomControls.draw()
    local lg = love.graphics
    local layout = mobileLayout.getLayout()
    
    -- 상태 저장
    local r, g, b, a = lg.getColor()
    local font = lg.getFont()
    
    -- 배경 (반투명 검은색)
    lg.setColor(0, 0, 0, 0.7)
    lg.rectangle("fill", 0, layout.bottomAreaStart, 
                 layout.screenWidth, layout.bottomAreaHeight)
    
    -- 각 버튼 그리기
    for name, button in pairs(buttons) do
        if button.visible then
            bottomControls.drawButton(button)
        end
    end
    
    -- 상태 복원
    lg.setColor(r, g, b, a)
    lg.setFont(font)
end

-- 버튼 그리기
function bottomControls.drawButton(button)
    local lg = love.graphics
    
    -- 버튼 색상 (프레스 상태에 따라 변경)
    if button.pressed then
        lg.setColor(0.5, 0.5, 0.5, 0.9)  -- 프레스됨: 어두운 회색
    else
        lg.setColor(0.3, 0.3, 0.3, 0.8)  -- 일반: 회색
    end
    
    -- 버튼 배경
    lg.rectangle("fill", button.x, button.y, button.width, button.height, 8, 8)
    
    -- 버튼 테두리
    lg.setColor(0.8, 0.8, 0.8, 1)
    lg.setLineWidth(2)
    lg.rectangle("line", button.x, button.y, button.width, button.height, 8, 8)
    
    -- 라벨 텍스트
    lg.setColor(1, 1, 1, 1)
    local font = lg.getFont()
    local textX = button.x + (button.width - font:getWidth(button.text)) / 2
    local textY = button.y + (button.height - font:getHeight()) / 2
    lg.print(button.text, textX, textY)
end

-- 터치 입력 처리
function bottomControls.touchpressed(id, x, y, dx, dy, pressure)
    -- 하단 영역인지 확인
    if not mobileLayout.isTouchInArea(x, y, "bottom") then
        return false
    end
    
    -- 각 버튼 휴릿 테스트
    for name, button in pairs(buttons) do
        if button.visible and bottomControls.isPointInButton(x, y, button) then
            button.pressed = true
            bottomControls.handleButtonPress(name)
            return true  -- 터치 이벤트 소비됨
        end
    end
    
    return false
end

function bottomControls.touchreleased(id, x, y, dx, dy, pressure)
    -- 모든 버튼의 프레스 상태 해제
    for name, button in pairs(buttons) do
        button.pressed = false
    end
end

-- 점이 버튼 영역에 있는지 확인
function bottomControls.isPointInButton(x, y, button)
    return x >= button.x and x <= (button.x + button.width) and
           y >= button.y and y <= (button.y + button.height)
end

-- 버튼 프레스 처리
function bottomControls.handleButtonPress(buttonName)
    logger.info("Button pressed: " .. buttonName)
    
    if buttonName == "zoomIn" and callbacks.onZoomIn then
        callbacks.onZoomIn()
    elseif buttonName == "zoomOut" and callbacks.onZoomOut then
        callbacks.onZoomOut()
    elseif buttonName == "reset" and callbacks.onReset then
        callbacks.onReset()
    elseif buttonName == "debug" and callbacks.onDebugToggle then
        callbacks.onDebugToggle()
    end
end

-- 콜백 함수 설정
function bottomControls.setCallbacks(callbackTable)
    if callbackTable.onZoomIn then callbacks.onZoomIn = callbackTable.onZoomIn end
    if callbackTable.onZoomOut then callbacks.onZoomOut = callbackTable.onZoomOut end
    if callbackTable.onReset then callbacks.onReset = callbackTable.onReset end
    if callbackTable.onDebugToggle then callbacks.onDebugToggle = callbackTable.onDebugToggle end
end

-- 버튼 표시/숨김 제어
function bottomControls.setButtonVisible(buttonName, visible)
    if buttons[buttonName] then
        buttons[buttonName].visible = visible
    end
end

return bottomControls