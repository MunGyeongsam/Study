-- Mobile Layout
-- 모바일 화면 레이아웃 계산 및 관리

local logger = require("00_common.logger")

local _floor = math.floor
local _sqrt  = math.sqrt
local _max   = math.max
local _min   = math.min

local mobileLayout = {}

-- 레이아웃 비율 (문서 기준)
local UI_TOP_RATIO = 0.05      -- 상단 UI 5%
local UI_BOTTOM_RATIO = 0.10   -- 하단 UI 10%
local PLAY_AREA_RATIO = 0.85   -- 게임 영역 85%

-- 현재 화면 정보
local screenInfo = {
    screenWidth = 0,
    screenHeight = 0,
    
    topAreaHeight = 0,
    playAreaStart = 0,
    playAreaHeight = 0,
    bottomAreaStart = 0,
    bottomAreaHeight = 0,
    
    dpiScale = 1.0,
    touchScale = 1.0,
    
    needsUpdate = true
}

-- 초기화
function mobileLayout.init()
    logger.info("Mobile layout system initialized")
    mobileLayout.updateScreenInfo()
end

-- 화면 정보 업데이트
function mobileLayout.updateScreenInfo()
    local lg = love.graphics
    screenInfo.screenWidth, screenInfo.screenHeight = lg.getDimensions()
    
    -- 영역별 크기 계산
    screenInfo.topAreaHeight = _floor(screenInfo.screenHeight * UI_TOP_RATIO)
    screenInfo.playAreaStart = screenInfo.topAreaHeight
    screenInfo.playAreaHeight = _floor(screenInfo.screenHeight * PLAY_AREA_RATIO)
    screenInfo.bottomAreaStart = screenInfo.playAreaStart + screenInfo.playAreaHeight
    screenInfo.bottomAreaHeight = screenInfo.screenHeight - screenInfo.bottomAreaStart
    
    -- DPI 및 터치 스케일 계산
    screenInfo.dpiScale = mobileLayout.calculateDPIScale()
    screenInfo.touchScale = mobileLayout.calculateTouchScale()
    
    screenInfo.needsUpdate = false
    
    logger.info(string.format("Screen layout updated: %dx%d", 
        screenInfo.screenWidth, screenInfo.screenHeight))
    logger.info(string.format("Areas - Top: %d, Play: %d, Bottom: %d", 
        screenInfo.topAreaHeight, screenInfo.playAreaHeight, screenInfo.bottomAreaHeight))
end

-- DPI 스케일 계산
function mobileLayout.calculateDPIScale()
    local referenceDPI = 400  -- 기준 DPI
    local diagonal = _sqrt(screenInfo.screenWidth^2 + screenInfo.screenHeight^2)
    local estimatedDPI = diagonal / 6  -- 6인치 가정
    return _max(0.8, _min(2.0, estimatedDPI / referenceDPI))
end

-- 터치 스케일 계산
function mobileLayout.calculateTouchScale()
    local diagonal = _sqrt(screenInfo.screenWidth^2 + screenInfo.screenHeight^2)
    
    -- 화면 크기별 터치 영역 조정
    if diagonal < 1500 then
        return 1.2  -- 작은 화면: 터치 영역 확대
    elseif diagonal < 2500 then
        return 1.0  -- 중간 화면: 기본 크기
    else
        return 0.8  -- 큰 화면(태블릿): 터치 영역 축소
    end
end

-- 현재 레이아웃 정보 반환 (읽기 전용 — 수정 금지)
function mobileLayout.getLayout()
    if screenInfo.needsUpdate then
        mobileLayout.updateScreenInfo()
    end
    return screenInfo
end

-- 터치 좌표가 특정 영역에 있는지 확인
function mobileLayout.isTouchInArea(x, y, area)
    local layout = mobileLayout.getLayout()
    
    if area == "top" then
        return y >= 0 and y <= layout.topAreaHeight
    elseif area == "play" then
        return y >= layout.playAreaStart and y <= (layout.playAreaStart + layout.playAreaHeight)
    elseif area == "bottom" then
        return y >= layout.bottomAreaStart and y <= layout.screenHeight
    end
    
    return false
end

-- 플레이 영역 좌표를 월드 좌표로 변환
function mobileLayout.playAreaToWorld(x, y)
    local layout = mobileLayout.getLayout()
    
    -- 플레이 영역 상대 좌표로 변환
    local relativeY = y - layout.playAreaStart
    
    return x, relativeY
end

-- 안전한 터치 크기 계산 (최소 44dp 보장)
function mobileLayout.getSafeTouchSize(baseSize)
    local layout = mobileLayout.getLayout()
    local minTouchSize = 44 * layout.dpiScale  -- 44dp 최소 크기
    
    return _max(baseSize * layout.touchScale, minTouchSize)
end

-- 디바이스 프로필 반환
function mobileLayout.getDeviceProfile()
    local layout = mobileLayout.getLayout()
    local diagonal = _sqrt(layout.screenWidth^2 + layout.screenHeight^2)
    
    if diagonal < 1500 then
        return {
            type = "small_phone",
            uiScale = 1.2,
            fontSize = "small",
            buttonSpacing = 8,
            maxParticles = 300
        }
    elseif diagonal < 2500 then
        return {
            type = "normal_phone",
            uiScale = 1.0,
            fontSize = "medium",
            buttonSpacing = 12,
            maxParticles = 500
        }
    else
        return {
            type = "tablet",
            uiScale = 0.8,
            fontSize = "large",
            buttonSpacing = 16,
            maxParticles = 800
        }
    end
end

-- 화면 크기 변경 이벤트 처리
function mobileLayout.onResize()
    screenInfo.needsUpdate = true
    logger.info("Screen resize detected")
end

return mobileLayout