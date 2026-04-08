-- Logger Module
-- 로그 시스템을 관리하는 독립적인 모듈

local Logger = {}

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
function Logger.init()
    -- Create log file that VS Code can monitor
    logFile = io.open("debug.log", "w")
    if logFile then
        logFile:write("=== Vector Swarm Debug Log Started ===\n")
        logFile:flush()
    end
    
    -- Initialize console messages
    consoleMessages = {
        "=== DEBUG CONSOLE ===",
        "Game started - hello vector_swarm",
        "Press F1 to toggle console",
        "F2: Test logging functions",
        "F3: Toggle debug mode"
    }
    
    return logFile ~= nil
end

-- Internal logging function
local function writeLog(level, msg)
    local timeStr = string.format("[%.3f %s] %s", love.timer.getTime(), level, msg)
    
    -- Standard output
    originalPrint(timeStr)
    io.stdout:flush()
    
    -- Write to log file
    if logFile then
        logFile:write(timeStr .. "\n")
        logFile:flush()
    end
    
    return timeStr
end

-- Public logging functions
function Logger.debug(msg)
    return writeLog("DEBUG", msg)
end

function Logger.info(msg)
    return writeLog("INFO", msg)
end

function Logger.warn(msg)
    return writeLog("WARN", msg)
end

function Logger.error(msg)
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
    
    return timeStr
end

-- Get log level color for UI rendering
function Logger.getLevelColor(level)
    local levelData = LOG_LEVELS[level:upper()]
    return levelData and levelData.color or {1, 1, 1}
end

-- Add message to debug console (for in-game console)
function Logger.addToConsole(msg, maxMessages)
    maxMessages = maxMessages or 15
    table.insert(consoleMessages, {
        text = msg,
        time = love.timer.getTime(),
        level = "INFO"
    })
    
    -- Keep only last N messages
    if #consoleMessages > maxMessages then
        table.remove(consoleMessages, 1)
    end
end

-- Get console messages for rendering
function Logger.getConsoleMessages()
    return consoleMessages
end

-- Add window size info to console
function Logger.addWindowInfo(width, height)
    table.insert(consoleMessages, "Window size: " .. width .. " x " .. height)
end

-- Close logging system
function Logger.close()
    Logger.info("Logger shutting down...")
    if logFile then
        logFile:write("=== Vector Swarm Debug Log Ended ===\n")
        logFile:close()
        logFile = nil
    end
end

-- Check if logging is initialized
function Logger.isInitialized()
    return logFile ~= nil
end

return Logger