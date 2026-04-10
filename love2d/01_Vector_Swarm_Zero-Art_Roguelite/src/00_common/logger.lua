-- Logger Module
-- 로그 시스템을 관리하는 독립적인 모듈
--
-- 📚 USAGE EXAMPLES:
--
-- 1. 초기화 및 종료:
--    local Logger = require("00_common.logger")
--    
--    function love.load()
--        Logger.init()  -- 로깅 시스템 초기화 (debug.log 파일 생성)
--    end
--    
--    function love.quit()
--        Logger.close()  -- 로그 파일 정리
--    end
--
-- 2. 로그 레벨별 사용법:
--    Logger.debug("Player position: " .. x .. "," .. y)     -- 회색 디버그 정보
--    Logger.info("Game started successfully")               -- 녹색 일반 정보  
--    Logger.warn("Performance issue detected: low FPS")     -- 노란색 경고
--    Logger.error("Failed to load texture: " .. filename)  -- 빨간색 에러
--
-- 3. 인게임 디버그 콘솔:
--    function love.keypressed(key)
--        if key == "f1" then
--            debugConsoleVisible = not debugConsoleVisible
--        end
--    end
--    
--    function love.draw()
--        if debugConsoleVisible then
--            Logger.drawConsole(fonts.small, debugConsoleVisible)
--        end
--    end
--
-- 4. 로깅 시스템 상태 확인:
--    if Logger.isInitialized() then
--        Logger.info("Logging system ready")
--    end
--
-- 📋 주요 기능:
-- - 파일 출력: debug.log 파일에 모든 로그 기록
-- - 콘솔 출력: 표준 출력 및 에러 출력  
-- - 인게임 콘솔: F1키로 토글 가능한 게임 내 콘솔
-- - 색상 구분: DEBUG(회색), INFO(녹색), WARN(노랑), ERROR(빨강)
-- - 타임스킬프: 게임 시작부터의 경과 시간 자동 기록
-- - 자동 콘솔 연동: 모든 로그가 자동으로 인게임 콘솔에 표시
--

local logger = {}

-- Private variables
local originalPrint = print
local logFile = nil
local consoleMessages = {}  -- Internal console messages

-- LOG LEVELS (color coding for console output)
local LOG_LEVELS = {
    DEBUG = {name = "DEBUG", color = {0.7, 0.7, 0.7}},  -- Gray
    INFO =  {name = "INFO",  color = {0, 1, 0}},        -- Green
    WARN =  {name = "WARN",  color = {1, 1, 0}},        -- Yellow  
    ERROR = {name = "ERROR", color = {1, 0, 0}}         -- Red
}

-- Initialize logging system
function logger.init()
    
    -- Initialize console messages
    consoleMessages = {
        {text = "=== DEBUG CONSOLE ===", level = "INFO", time = love.timer.getTime()},
        {text = "Game started - hello vector_swarm", level = "INFO", time = love.timer.getTime()},
        {text = "Press F1 to toggle console", level = "INFO", time = love.timer.getTime()},
        {text = "F2: Test logging functions", level = "INFO", time = love.timer.getTime()},
        {text = "F3: Toggle debug mode", level = "INFO", time = love.timer.getTime()}
    }
    -- Create log file that VS Code can monitor
    logFile, err = io.open("debug.log", "w")
    if logFile then
        logFile:write("=== Vector Swarm Debug Log Started ===\n")
        logFile:flush()
        table.insert(consoleMessages, {text = "Debug log initialized: debug.log", level = "INFO", time = love.timer.getTime()})
    else
        originalPrint("Error initializing log file: " .. tostring(err))
        table.insert(consoleMessages, {text = "[LOGGER ERROR] 로그 파일 초기화 실패: " .. tostring(err), level = "ERROR", time = love.timer.getTime()})
    end

    if logFile == nil then
        originalPrint("Error: Could not initialize log file!")
        table.insert(consoleMessages, {text = "[LOGGER ERROR] 로그 파일 생성 실패! (debug.log)", level = "ERROR", time = love.timer.getTime()})
    else
        logger.info("Logger initialized successfully.")
    end
    
    return logFile ~= nil
end

-- Internal logging function
local function writeLog(level, msg)
    local timeStr = string.format("[%6.3f %6s] %s", love.timer.getTime(), level, msg)
    
    -- Standard output
    originalPrint(timeStr)
    io.stdout:flush()
    
    -- Write to log file
    if logFile then
        logFile:write(timeStr .. "\n")
        logFile:flush()
    end
    
    -- Add to console messages (auto console integration)
    local maxMessages = 20
    table.insert(consoleMessages, {
        text = msg,
        level = level,
        time = love.timer.getTime()
    })
    
    -- Keep only last N messages
    if #consoleMessages > maxMessages then
        table.remove(consoleMessages, 1)
    end
    
    return timeStr
end

-- Public logging functions
function logger.debug(msg)
    writeLog("DEBUG", msg)
end

function logger.info(msg)
    writeLog("INFO", msg)
end

function logger.warn(msg)
    writeLog("WARN", msg)
end

function logger.error(msg)
    local timeStr = string.format("[%.3f ERROR] %s", love.timer.getTime(), msg)
    
    -- ERROR goes to stderr
    io.stderr:write(timeStr .. "\n")
    io.stderr:flush()
    originalPrint(timeStr)  -- Also print to console
    
    -- Write to log file
    if logFile then
        logFile:write(timeStr .. "\n")  
        logFile:flush()
    end
    
    -- Add to console messages
    local maxMessages = 20
    table.insert(consoleMessages, {
        text = msg,
        level = "ERROR",
        time = love.timer.getTime()
    })
    
    -- Keep only last N messages
    if #consoleMessages > maxMessages then
        table.remove(consoleMessages, 1)
    end
    
    return timeStr
end

-- Get log level color for UI rendering
function logger.getLevelColor(level)
    local levelData = LOG_LEVELS[level:upper()]
    return levelData and levelData.color or {1, 1, 1}
end

-- Get console messages for rendering
function logger.getConsoleMessages()
    return consoleMessages
end

-- Close logging system
function logger.close()
    logger.info("Logger shutting down...")
    if logFile then
        logFile:write("=== Vector Swarm Debug Log Ended ===\n")
        logFile:close()
        logFile = nil
    end
end

-- Check if logging is initialized
function logger.isInitialized()
    return logFile ~= nil
end

-- Draw debug console (콘솔이 Logger 데이터를 직접 관리하므로 Logger에서 렌더링)
function logger.drawConsole(font, isVisible)
    if not isVisible or not consoleMessages then return end
    
    -- Save current graphics state
    local r, g, b, a = love.graphics.getColor()
    local currentFont = love.graphics.getFont()
    
    local width, height = love.graphics.getDimensions()
    local consoleHeight = height * 0.5
    local consoleY = 0
    local lineHeight = font and (font:getHeight() + 2) or 15
    
    -- Console background
    love.graphics.setColor(0, 0, 0, 0.4)
    love.graphics.rectangle("fill", 0, consoleY, width, consoleHeight)
    
    -- Console text with level-based colors
    if font then love.graphics.setFont(font) end
    
    local y = consoleY + 10
    local maxLines = math.floor((consoleHeight - 20) / lineHeight)
    local startIndex = math.max(1, #consoleMessages - maxLines + 1)
    
    for i = startIndex, #consoleMessages do
        if y + lineHeight > height then break end
        local msg = consoleMessages[i]
        
        -- Apply color based on log level
        local level = type(msg) == "table" and msg.level or "INFO"
        local color = Logger.getLevelColor(level)
        love.graphics.setColor(color[1], color[2], color[3], 1)
        
        local text = type(msg) == "table" and msg.text or msg
        love.graphics.print(text, 10, y)
        y = y + lineHeight
    end
    
    -- Instructions
    love.graphics.setColor(1, 1, 0, 1) -- Yellow
    love.graphics.print("F1: Toggle Console | F2: Test Logs | F3: Debug Mode | F4: Camera | F5: Shake | ESC: Exit", 
                       10, height - 25)
                       
    -- Restore graphics state
    love.graphics.setColor(r, g, b, a)
    love.graphics.setFont(currentFont)
end

return logger