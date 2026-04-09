-- Debug Info Display Module
-- 동적으로 디버그 정보를 추가/제거하고 렌더링하는 시스템

local Debug = {}

-- Private variables
local debugInfos = {}  -- 등록된 디버그 정보들 {key = {func, x, y, color}}
local debugMode = false
local consoleVisible = false
local defaultFont = nil
local lineHeight = 15

-- Configuration
local DEFAULT_X = 10
local DEFAULT_Y = 30
local DEFAULT_COLOR = {1, 1, 0, 1}  -- Yellow
local DEBUG_HEADER_COLOR = {0, 1, 0, 1}  -- Green

-- Initialize debug system
function Debug.init(font)
    defaultFont = font or love.graphics.getFont()
    lineHeight = defaultFont:getHeight() + 2
    
    -- Add default debug infos (모두 순차적으로 배치)
    Debug.addInfo("header", function() return "=== DEBUG MODE ===" end, DEBUG_HEADER_COLOR)
    Debug.addInfo("fps", function() return "FPS: " .. love.timer.getFPS() end)
    Debug.addInfo("time", function() return "Game Time: " .. string.format("%.2f", love.timer.getTime()) end)
end

-- Add debug info function
function Debug.addInfo(key, textFunc, color)
    color = color or DEFAULT_COLOR
    
    debugInfos[key] = {
        func = textFunc,
        color = color
    }
end

-- Remove debug info
function Debug.removeInfo(key)
    debugInfos[key] = nil
end

-- Clear all debug infos
function Debug.clearAll()
    debugInfos = {}
end

-- Toggle debug mode
function Debug.toggleMode()
    debugMode = not debugMode
    return debugMode
end

-- Set debug mode
function Debug.setMode(enabled)
    debugMode = enabled
end

-- Check if debug mode is on
function Debug.isEnabled()
    return debugMode
end

-- Toggle console visibility
function Debug.toggleConsole()
    consoleVisible = not consoleVisible
    return consoleVisible
end

-- Draw all debug infos
function Debug.draw()
    if not debugMode then return end
    
    -- Save current graphics state
    local r, g, b, a = love.graphics.getColor()
    local font = love.graphics.getFont()
    
    -- Set debug font
    love.graphics.setFont(defaultFont)
    
    -- 모든 디버그 정보를 순차적으로 배치
    local currentY = DEFAULT_Y
    for key, info in pairs(debugInfos) do
        local text = info.func()
        if text and text ~= "" then
            love.graphics.setColor(info.color[1], info.color[2], info.color[3], info.color[4] or 1)
            love.graphics.print(text, DEFAULT_X, currentY)
            currentY = currentY + lineHeight
        end
    end
    
    -- Restore graphics state
    love.graphics.setColor(r, g, b, a)
    love.graphics.setFont(font)
end

-- Utility functions for common debug patterns
function Debug.addGameVariable(varName, getValueFunc, format)
    format = format or "%.2f"
    Debug.addInfo(varName, function()
        local value = getValueFunc()
        if type(value) == "number" then
            return varName .. ": " .. string.format(format, value)
        else
            return varName .. ": " .. tostring(value)
        end
    end)
end

function Debug.addPosition(name, getPositionFunc)
    Debug.addInfo(name .. "_pos", function()
        local pos = getPositionFunc()
        return name .. ": (" .. string.format("%.0f", pos.x) .. ", " .. string.format("%.0f", pos.y) .. ")"
    end)
end

function Debug.addCounter(name, getCountFunc)
    Debug.addInfo(name .. "_count", function()
        return name .. ": " .. tostring(getCountFunc())
    end)
end

-- Set debug info position (사용하지 않음)
-- function Debug.setPosition(key, x, y)
--     if debugInfos[key] then
--         debugInfos[key].x = x
--         debugInfos[key].y = y
--     end
-- end

-- Set debug info color
function Debug.setColor(key, color)
    if debugInfos[key] then
        debugInfos[key].color = color
    end
end

return Debug