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

-- Draw debug console (콘솔이 Logger 데이터를 직접 관리하므로 Logger에서 렌더링)
function Logger.drawConsole(font, isVisible)
    if not isVisible or not consoleMessages then return end
    
    -- Save current graphics state
    local r, g, b, a = love.graphics.getColor()
    local currentFont = love.graphics.getFont()
    
    local width, height = love.graphics.getDimensions()
    local consoleHeight = height * 0.5
    local consoleY = height - consoleHeight
    local lineHeight = font and (font:getHeight() + 2) or 15
    
    -- Console background
    love.graphics.setColor(0, 0, 0, 0.8)
    love.graphics.rectangle("fill", 0, consoleY, width, consoleHeight)
    
    -- Console text
    if font then love.graphics.setFont(font) end
    love.graphics.setColor(0, 1, 0, 1) -- Green
    
    local y = consoleY + 10
    local maxLines = math.floor((consoleHeight - 20) / lineHeight)
    local startIndex = math.max(1, #consoleMessages - maxLines + 1)
    
    for i = startIndex, #consoleMessages do
        if y + lineHeight > height then break end
        local msg = consoleMessages[i]
        local text = type(msg) == "table" and msg.text or msg
        love.graphics.print(text, 10, y)
        y = y + lineHeight
    end
    
    -- Instructions
    love.graphics.setColor(1, 1, 0, 1) -- Yellow
    love.graphics.print("F1: Toggle Console | F2: Test Logs | F3: Debug Mode | ESC: Exit", 
                       10, height - 25)
                       
    -- Restore graphics state
    love.graphics.setColor(r, g, b, a)
    love.graphics.setFont(currentFont)
end

return Logger