-- UI Manager
-- 모바일 친화적 UI 시스템 관리

local logger = require("00_common.logger")
local mobileLayout = require("04_ui.mobileLayout")
local topHud = require("04_ui.topHud")
local bottomControls = require("04_ui.bottomControls")
local minimap = require("04_ui.minimap")

local uiManager = {}

-- UI 상태
uiManager.isVisible = true
uiManager.debugMode = false

-- 초기화
function uiManager.init()
    logger.info("UI Manager initialized")
    
    -- 서브 시스템 초기화
    mobileLayout.init()
    topHud.init()
    bottomControls.init()
    minimap.init()
    
    -- 화면 크기 변경 감지
    uiManager.updateLayout()
end

-- 화면 크기 변경시 레이아웃 업데이트
function uiManager.updateLayout()
    mobileLayout.updateScreenInfo()
    topHud.updateLayout()
    bottomControls.updateLayout()
    minimap.updateLayout()
end

-- 업데이트
function uiManager.update(dt)
    if not uiManager.isVisible then return end
    
    bottomControls.update(dt)
end

-- 렌더링
function uiManager.draw()
    if not uiManager.isVisible then return end
    
    local lg = love.graphics
    
    -- 상태 저장
    local r, g, b, a = lg.getColor()
    local font = lg.getFont()
    
    -- UI 영역 표시 (디버그 모드시)
    if uiManager.debugMode then
        uiManager.drawLayoutGuides()
    end
    
    -- 상단 HUD 그리기
    topHud.draw()
    
    -- 하단 컨트롤 그리기
    bottomControls.draw()

    -- 미니맵 (setMinimapData가 호출된 경우에만)
    if uiManager._minimapEcs then
        minimap.draw(uiManager._minimapEcs, uiManager._minimapPlayer, uiManager._minimapCam)
    end
    
    -- 상태 복원
    lg.setColor(r, g, b, a)
    lg.setFont(font)
end

-- 레이아웃 가이드 그리기 (개발용)
function uiManager.drawLayoutGuides()
    local lg = love.graphics
    local layout = mobileLayout.getLayout()
    
    lg.setColor(1, 0, 0, 0.3)  -- 반투명 빨강
    
    -- 상단 UI 영역
    lg.rectangle("fill", 0, 0, layout.screenWidth, layout.topAreaHeight)
    
    -- 하단 UI 영역
    lg.rectangle("fill", 0, layout.bottomAreaStart, 
                 layout.screenWidth, layout.bottomAreaHeight)
    
    -- 게임 플레이 영역 테두리
    lg.setColor(0, 1, 0, 0.8)  -- 초록 테두리
    lg.setLineWidth(2)
    lg.rectangle("line", 0, layout.playAreaStart, 
                 layout.screenWidth, layout.playAreaHeight)
end

-- 터치 입력 처리
function uiManager.touchpressed(id, x, y, dx, dy, pressure)
    if not uiManager.isVisible then return false end
    
    -- 하단 컨트롤 터치 확인
    if bottomControls.touchpressed(id, x, y, dx, dy, pressure) then
        return true  -- 터치 이벤트 소비됨
    end
    
    -- 상단 HUD 터치 확인
    if topHud.touchpressed(id, x, y, dx, dy, pressure) then
        return true  -- 터치 이벤트 소비됨
    end
    
    return false  -- 게임 영역으로 터치 이벤트 전달
end

function uiManager.touchmoved(id, x, y, dx, dy, pressure)
    -- 모바일 UI는 일반적으로 touchmoved 사용하지 않음
    return false
end

function uiManager.touchreleased(id, x, y, dx, dy, pressure)
    if not uiManager.isVisible then return false end
    
    bottomControls.touchreleased(id, x, y, dx, dy, pressure)
    return false
end

-- UI 표시/숨김 토글
function uiManager.toggleVisibility()
    uiManager.isVisible = not uiManager.isVisible
    logger.info("UI visibility: " .. tostring(uiManager.isVisible))
end

-- 디버그 모드 토글
function uiManager.toggleDebugMode()
    uiManager.debugMode = not uiManager.debugMode
    logger.info("UI debug mode: " .. tostring(uiManager.debugMode))
end

-- 게임 데이터 설정 (점수, 상태 등)
function uiManager.setGameData(data)
    topHud.setGameData(data)
end

-- 미니맵 데이터 설정 (매 프레임 playScene에서 호출)
function uiManager.setMinimapData(ecs, playerModule, cam)
    uiManager._minimapEcs    = ecs
    uiManager._minimapPlayer = playerModule
    uiManager._minimapCam    = cam
end

-- 버튼 액션 콜백 설정
function uiManager.setButtonCallbacks(callbacks)
    bottomControls.setCallbacks(callbacks)
end

return uiManager