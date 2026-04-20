-- Pause Menu
-- 인게임 일시정지 오버레이: Continue / Restart / Main Menu

local _sin = math.sin

local pauseMenu = {}

local MENU_ITEMS = {
    { id = "continue", label = "CONTINUE" },
    { id = "restart",  label = "RESTART" },
    { id = "menu",     label = "MAIN MENU" },
}

local state = {
    selectedIndex = 1,
}

-- Callbacks (set from main.lua)
local callbacks = {
    onContinue = nil,
    onRestart  = nil,
    onMenu     = nil,
}

-- Cached fonts
local titleFont = nil
local menuFont  = nil
local hintFont  = nil

function pauseMenu.setCallbacks(cbs)
    callbacks = cbs or callbacks
end

function pauseMenu.reset()
    state.selectedIndex = 1
end

function pauseMenu.draw()
    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- Font caching
    if not titleFont then
        titleFont = lg.newFont(28)
        menuFont  = lg.newFont(18)
        hintFont  = lg.newFont(12)
    end

    -- Dim overlay
    lg.setColor(0, 0, 0, 0.75)
    lg.rectangle("fill", 0, 0, w, h)

    -- Title
    lg.setFont(titleFont)
    lg.setColor(1, 1, 1, 1)
    local title = "PAUSED"
    local tw = titleFont:getWidth(title)
    lg.print(title, (w - tw) / 2, h * 0.28)

    -- Separator
    lg.setColor(0.5, 0.5, 0.5, 0.3)
    lg.setLineWidth(1)
    lg.line(w * 0.3, h * 0.36, w * 0.7, h * 0.36)

    -- Menu items
    lg.setFont(menuFont)
    local itemH = 42
    local menuStartY = h * 0.40

    local t = love.timer.getTime()
    for i, item in ipairs(MENU_ITEMS) do
        local iy = menuStartY + (i - 1) * itemH
        local isSelected = (i == state.selectedIndex)

        if isSelected then
            local pulse = 0.6 + 0.4 * _sin(t * 4)
            lg.setColor(0.2, 0.5, 0.8, 0.2 * pulse)
            lg.rectangle("fill", w * 0.2, iy - 2, w * 0.6, itemH - 6, 4, 4)

            lg.setColor(0, 1, 1, pulse)
            lg.print(">", w * 0.22, iy + 6)

            lg.setColor(1, 1, 1, 1)
        else
            lg.setColor(0.6, 0.6, 0.6, 0.8)
        end

        local iw = menuFont:getWidth(item.label)
        lg.print(item.label, (w - iw) / 2, iy + 6)
    end

    -- Hint
    lg.setFont(hintFont)
    local alpha = 0.4 + 0.2 * _sin(t * 2)
    lg.setColor(0.5, 0.5, 0.5, alpha)
    local hint = "ESC: resume | UP/DOWN + ENTER"
    local hw = hintFont:getWidth(hint)
    lg.print(hint, (w - hw) / 2, h * 0.72)

    lg.setColor(1, 1, 1, 1)
end

local function executeSelection()
    local item = MENU_ITEMS[state.selectedIndex]
    if not item then return end

    if item.id == "continue" and callbacks.onContinue then
        callbacks.onContinue()
    elseif item.id == "restart" and callbacks.onRestart then
        callbacks.onRestart()
    elseif item.id == "menu" and callbacks.onMenu then
        callbacks.onMenu()
    end
end

function pauseMenu.keypressed(key)
    if key == "up" then
        state.selectedIndex = state.selectedIndex - 1
        if state.selectedIndex < 1 then state.selectedIndex = #MENU_ITEMS end
        return true
    elseif key == "down" then
        state.selectedIndex = state.selectedIndex + 1
        if state.selectedIndex > #MENU_ITEMS then state.selectedIndex = 1 end
        return true
    elseif key == "return" or key == "kpenter" then
        executeSelection()
        return true
    elseif key == "escape" then
        -- ESC in pause = resume
        if callbacks.onContinue then callbacks.onContinue() end
        return true
    end
    return false
end

function pauseMenu.touchpressed(x, y)
    local lg = love.graphics
    local w, h = lg.getDimensions()
    local itemH = 42
    local menuStartY = h * 0.40

    for i = 1, #MENU_ITEMS do
        local iy = menuStartY + (i - 1) * itemH
        if y >= iy - 2 and y <= iy + itemH - 6 and x >= w * 0.2 and x <= w * 0.8 then
            state.selectedIndex = i
            executeSelection()
            return true
        end
    end
    return false
end

return pauseMenu
