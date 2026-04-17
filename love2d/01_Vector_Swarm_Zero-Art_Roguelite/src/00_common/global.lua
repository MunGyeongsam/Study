-- Global Utilities
-- 편리한 전역 함수들

local logger = require("00_common.logger")

-- ===== 로깅 래퍼 (직접 할당) =====
log = logger.info
logInfo = logger.info
logDebug = logger.debug
logWarn = logger.warn
logError = logger.error

-- ===== 수학 유틸리티 =====

-- 거리 계산
function distance(x1, y1, x2, y2)
    local dx = x2 - x1
    local dy = y2 - y1
    return math.sqrt(dx * dx + dy * dy)
end

-- 값 클램핑
function clamp(value, min, max)
    return math.max(min, math.min(max, value))
end

-- 선형 보간
function lerp(a, b, t)
    return a + (b - a) * t
end

-- 각도 정규화 (0 ~ 2π)
function normalizeAngle(angle)
    while angle < 0 do angle = angle + 2 * math.pi end
    while angle >= 2 * math.pi do angle = angle - 2 * math.pi end
    return angle
end

-- 호도법을 도로 변환
function toDegrees(radians)
    return radians * 180 / math.pi
end

-- 도를 호도법으로 변환  
function toRadians(degrees)
    return degrees * math.pi / 180
end

-- ===== 색상 유틸리티 =====

-- 색상 설정 (0-255 값 자동 변환)
function setColor(r, g, b, a)
    r = r or 255
    g = g or 255
    b = b or 255
    a = a or 255
    love.graphics.setColor(r/255, g/255, b/255, a/255)
end

-- 흰색으로 초기화
function resetColor()
    love.graphics.setColor(1, 1, 1, 1)
end

-- ===== 좌표/벡터 유틸리티 =====

-- 벡터 정규화
function normalize(x, y)
    local length = math.sqrt(x * x + y * y)
    if length == 0 then return 0, 0 end
    return x / length, y / length
end

-- 벡터 내적
function dot(x1, y1, x2, y2)
    return x1 * x2 + y1 * y2
end

-- 두 점 사이의 각도
function angleBetween(x1, y1, x2, y2)
    return math.atan2(y2 - y1, x2 - x1)
end

-- ===== 화면 유틸리티 =====

-- 화면 중심점
function screenCenter()
    return love.graphics.getWidth() / 2, love.graphics.getHeight() / 2
end

-- 화면 크기
function screenSize()
    return love.graphics.getWidth(), love.graphics.getHeight()
end

-- ===== 디버그 유틸리티 =====

-- 간단한 텍스트 출력 (디버그용)
function drawText(text, x, y, color)
    if color then setColor(color[1], color[2], color[3], color[4])
    else resetColor() end
    love.graphics.print(text, x or 10, y or 10)
    resetColor()
end

-- 점 그리기  
function drawPoint(x, y, size, color)
    if color then setColor(color[1], color[2], color[3], color[4])
    else setColor(255, 0, 0) end
    love.graphics.circle("fill", x, y, size or 2)
    resetColor()
end

-- 선 그리기
function drawLine(x1, y1, x2, y2, color)
    if color then setColor(color[1], color[2], color[3], color[4])
    else setColor(255, 255, 255) end
    love.graphics.line(x1, y1, x2, y2)
    resetColor()
end

-- ===== 입력 유틸리티 =====

-- 마우스 월드 위치 (나중에 카메라 연동)
function mouseWorldPos()
    -- 카메라 인스턴스가 있을 때만 사용 가능
    -- 예: local cam = camera.new(); cam:mousepos()
    return love.mouse.getPosition()  -- 일단 기본 스크린 좌표 반환
end

-- 키 눌림 체크
function isKeyDown(key)
    return love.keyboard.isDown(key)
end

-- ===== 전역 초기화 =====
function initGlobals()
    log("Global utilities initialized")
end

return {
    init = initGlobals
}