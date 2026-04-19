-- Global Utilities
-- 편리한 전역 함수들

local logger = require("00_common.logger")

-- ===== 로깅 래퍼 (직접 할당) =====
log = logger.info
logInfo = logger.info
logDebug = logger.debug
logWarn = logger.warn
logError = logger.error

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

-- ===== 전역 초기화 =====
function initGlobals()
    log("Global utilities initialized")
end

return {
    init = initGlobals
}