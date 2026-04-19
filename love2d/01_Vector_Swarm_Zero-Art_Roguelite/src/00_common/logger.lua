-- Logger Module
-- Log system with file output, stdout, and in-game console.
--
-- Usage:
--   logger.init()                  -- in love.load
--   logger.info("message")        -- DEBUG/INFO/WARN/ERROR
--   logger.toggleConsole()        -- ` key
--   logger.drawConsole(font)      -- in love.draw
--   logger.close()                -- in love.quit

local logger = {}

local _floor = math.floor
local _max   = math.max

-- Private variables
local originalPrint = print
local logFile = nil
local consoleMessages = {}
local consoleVisible = false

local MAX_CONSOLE_MESSAGES = 20

local LOG_LEVELS = {
    DEBUG = {name = "DEBUG", color = {0.7, 0.7, 0.7}},
    INFO  = {name = "INFO",  color = {0, 1, 0}},
    WARN  = {name = "WARN",  color = {1, 1, 0}},
    ERROR = {name = "ERROR", color = {1, 0, 0}},
}

-- Internal: write one log entry to all outputs
local function writeLog(level, msg)
    local timeStr = string.format("[%6.3f %6s] %s", love.timer.getTime(), level, msg)

    -- stdout (ERROR also goes to stderr)
    originalPrint(timeStr)
    io.stdout:flush()
    if level == "ERROR" then
        io.stderr:write(timeStr .. "\n")
        io.stderr:flush()
    end

    -- file
    if logFile then
        logFile:write(timeStr .. "\n")
        logFile:flush()
    end

    -- in-game console ring buffer
    consoleMessages[#consoleMessages + 1] = {
        text  = msg,
        level = level,
        time  = love.timer.getTime(),
    }
    if #consoleMessages > MAX_CONSOLE_MESSAGES then
        table.remove(consoleMessages, 1)
    end
end

-----------------------------------------------------------
-- Public API
-----------------------------------------------------------

function logger.init()
    consoleMessages = {}

    local err
    logFile, err = io.open("debug.log", "w")
    if logFile then
        logFile:write("=== Vector Swarm Debug Log Started ===\n")
        logFile:flush()
    else
        originalPrint("Error initializing log file: " .. tostring(err))
    end

    logger.info("Logger initialized successfully.")
    return logFile ~= nil
end

function logger.debug(msg) writeLog("DEBUG", msg) end
function logger.info(msg)  writeLog("INFO",  msg) end
function logger.warn(msg)  writeLog("WARN",  msg) end
function logger.error(msg) writeLog("ERROR", msg) end

function logger.getLevelColor(level)
    local data = LOG_LEVELS[level:upper()]
    return data and data.color or {1, 1, 1}
end

function logger.getConsoleMessages()
    return consoleMessages
end

function logger.close()
    logger.info("Logger shutting down...")
    if logFile then
        logFile:write("=== Vector Swarm Debug Log Ended ===\n")
        logFile:close()
        logFile = nil
    end
end

function logger.isInitialized()
    return logFile ~= nil
end

function logger.toggleConsole()
    consoleVisible = not consoleVisible
end

function logger.isConsoleVisible()
    return consoleVisible
end

-----------------------------------------------------------
-- In-game console rendering
-----------------------------------------------------------
function logger.drawConsole(font)
    if not consoleVisible then return end

    local lg = love.graphics
    local prevColor = {lg.getColor()}
    local prevFont  = lg.getFont()

    local width, height = lg.getDimensions()
    local consoleHeight = height * 0.5
    if font then lg.setFont(font) end
    local lineHeight = lg.getFont():getHeight() + 2

    -- background
    lg.setColor(0, 0, 0, 0.4)
    lg.rectangle("fill", 0, 0, width, consoleHeight)

    -- messages
    local y = 10
    local maxLines = _floor((consoleHeight - 20) / lineHeight)
    local startIdx = _max(1, #consoleMessages - maxLines + 1)

    for i = startIdx, #consoleMessages do
        if y + lineHeight > consoleHeight then break end
        local msg   = consoleMessages[i]
        local color = logger.getLevelColor(msg.level or "INFO")
        lg.setColor(color[1], color[2], color[3], 1)
        lg.print(msg.text or "", 10, y)
        y = y + lineHeight
    end

    -- key hints (bottom of console area)
    lg.setColor(1, 1, 0, 1)
    lg.print("`: Console | F1: Watch | F2: UI | F3: UI Debug | F4: Grid | F5: Camera | ESC: Quit",
             10, consoleHeight - lineHeight - 4)

    -- restore
    lg.setColor(unpack(prevColor))
    lg.setFont(prevFont)
end

return logger