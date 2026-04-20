-- Title Menu
-- Zero-Art 스타일 타이틀 화면: 배경 + 로고 + 메뉴 항목
-- 게임 시작, 업그레이드, 옵션 진입

local saveData = require("00_common.saveData")
local achievementSystem = require("03_game.states.achievementSystem")

local _sin   = math.sin
local _cos   = math.cos
local _floor = math.floor

local titleMenu = {}

-- Menu items
local MENU_ITEMS = {
    { id = "play",         label = "PLAY" },
    { id = "upgrades",     label = "UPGRADES" },
    { id = "achievements", label = "ACHIEVEMENTS" },
    { id = "credits",      label = "CREDITS" },
}

-- Character definitions for selection
local CHARACTER_LIST = {
    { id = "default",  name = "DEFAULT",  desc = "Balanced stats",    locked = false },
    { id = "debugger", name = "DEBUGGER", desc = "Fast, small, fragile", locked = true, rewardId = "debugger" },
    { id = "compiler", name = "COMPILER", desc = "Slow, tough, heavy",  locked = true, rewardId = "compiler" },
}

local function getAvailableCharacters()
    local avail = {}
    for _, ch in ipairs(CHARACTER_LIST) do
        if not ch.locked or achievementSystem.isRewardUnlocked(ch.rewardId) then
            avail[#avail + 1] = ch
        end
    end
    return avail
end

local state = {
    selectedIndex = 1,
    timer = 0,           -- animation timer
    charIndex = 1,       -- selected character index in available list
}

-- Cached fonts
local logoFont    = nil
local menuFont    = nil
local subFont     = nil

-- Callbacks (set from main.lua)
local callbacks = {
    onPlay         = nil,
    onUpgrades     = nil,
    onAchievements = nil,
    onCredits      = nil,
}

function titleMenu.setCallbacks(cbs)
    callbacks = cbs or callbacks
end

function titleMenu.reset()
    state.selectedIndex = 1
    state.timer = 0
    -- Sync character selection from save
    local savedChar = saveData.getSelectedCharacter()
    local avail = getAvailableCharacters()
    state.charIndex = 1
    for i, ch in ipairs(avail) do
        if ch.id == savedChar then state.charIndex = i; break end
    end
end

function titleMenu.update(dt)
    state.timer = state.timer + dt
end

function titleMenu.draw()
    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- Font caching
    if not logoFont then
        logoFont = lg.newFont(32)
        menuFont = lg.newFont(20)
        subFont  = lg.newFont(12)
    end

    -- Background: dark with subtle pattern
    lg.setColor(0.02, 0.02, 0.06, 1)
    lg.rectangle("fill", 0, 0, w, h)

    -- Decorative lines (Zero-Art style)
    local t = state.timer
    for i = 1, 5 do
        local yy = h * (0.1 + i * 0.05) + _sin(t * 0.5 + i) * 10
        local alpha = 0.05 + 0.03 * _sin(t + i * 0.7)
        lg.setColor(0.2, 0.6, 1.0, alpha)
        lg.setLineWidth(1)
        lg.line(0, yy, w, yy)
    end

    -- Logo: "VECTOR SWARM"
    lg.setFont(logoFont)
    local logoText = "VECTOR SWARM"
    local lw = logoFont:getWidth(logoText)
    local logoY = h * 0.18

    -- Logo glow effect
    local glowAlpha = 0.3 + 0.15 * _sin(t * 2)
    lg.setColor(0, 0.7, 1.0, glowAlpha)
    lg.print(logoText, (w - lw) / 2 + 1, logoY + 1)
    lg.setColor(0, 1, 1, 1)
    lg.print(logoText, (w - lw) / 2, logoY)

    -- Subtitle
    lg.setFont(subFont)
    lg.setColor(0.5, 0.5, 0.6, 0.7)
    local sub = "ZERO-ART ROGUELITE"
    local sw = subFont:getWidth(sub)
    lg.print(sub, (w - sw) / 2, logoY + 42)

    -- Separator line
    lg.setColor(0, 0.8, 1.0, 0.3)
    lg.setLineWidth(1)
    local sepY = h * 0.35
    lg.line(w * 0.2, sepY, w * 0.8, sepY)

    -- Menu items
    lg.setFont(menuFont)
    local itemH = 45
    local menuStartY = h * 0.40

    for i, item in ipairs(MENU_ITEMS) do
        local iy = menuStartY + (i - 1) * itemH
        local isSelected = (i == state.selectedIndex)

        if isSelected then
            -- Selection indicator
            local pulse = 0.7 + 0.3 * _sin(t * 4)
            lg.setColor(0, 0.8, 1.0, 0.15 * pulse)
            lg.rectangle("fill", w * 0.15, iy - 4, w * 0.7, itemH - 8, 4, 4)

            -- Arrow
            lg.setColor(0, 1, 1, pulse)
            local arrowX = w * 0.2 - 20 + _sin(t * 3) * 3
            lg.print(">", arrowX, iy + 4)

            -- Text
            lg.setColor(0, 1, 1, 1)
        else
            lg.setColor(0.5, 0.6, 0.7, 0.8)
        end

        local tw = menuFont:getWidth(item.label)
        lg.print(item.label, (w - tw) / 2, iy + 4)
    end

    -- Character selector (only show if multiple characters available)
    local available = getAvailableCharacters()
    if #available > 1 then
        lg.setFont(subFont)
        local charY = menuStartY + #MENU_ITEMS * itemH + 10
        lg.setColor(0.3, 0.6, 0.7, 0.6)
        lg.printf("CHARACTER", 0, charY, w, "center")

        local ch = available[state.charIndex] or available[1]
        lg.setColor(0, 0.9, 0.8, 0.9)
        lg.printf(string.format("< %s >", ch.name), 0, charY + 16, w, "center")
        lg.setColor(0.4, 0.5, 0.6, 0.6)
        lg.printf(ch.desc, 0, charY + 32, w, "center")
    elseif #available == 1 then
        -- Single character — no selector, but show name if not default
        local ch = available[1]
        if ch.id ~= "default" then
            lg.setFont(subFont)
            local charY = menuStartY + #MENU_ITEMS * itemH + 10
            lg.setColor(0, 0.9, 0.8, 0.7)
            lg.printf(string.format("Character: %s", ch.name), 0, charY, w, "center")
        end
    end

    -- Fragment balance (bottom)
    lg.setFont(subFont)
    lg.setColor(0.4, 0.8, 1.0, 0.6)
    local frags = saveData.getFragments()
    if frags > 0 then
        local fragText = string.format("DATA FRAGMENTS: %d", frags)
        local fw = subFont:getWidth(fragText)
        lg.print(fragText, (w - fw) / 2, h * 0.75)
    end

    -- Stats
    local stats = saveData.getStats()
    if stats.totalRuns > 0 then
        lg.setColor(0.4, 0.4, 0.5, 0.5)
        local statText = string.format("Runs: %d | Best: Stage %d (%.0fs)",
            stats.totalRuns, stats.bestStage, stats.bestScore)
        local stw = subFont:getWidth(statText)
        lg.print(statText, (w - stw) / 2, h * 0.80)
    end

    -- Controls hint
    lg.setFont(subFont)
    local alpha = 0.4 + 0.2 * _sin(t * 2)
    lg.setColor(0.5, 0.5, 0.5, alpha)
    local hint = "UP/DOWN: select | ENTER: confirm"
    local hw = subFont:getWidth(hint)
    lg.print(hint, (w - hw) / 2, h * 0.92)

    lg.setColor(1, 1, 1, 1)
end

local function executeSelection()
    local item = MENU_ITEMS[state.selectedIndex]
    if not item then return end

    if item.id == "play" and callbacks.onPlay then
        callbacks.onPlay()
    elseif item.id == "upgrades" and callbacks.onUpgrades then
        callbacks.onUpgrades()
    elseif item.id == "achievements" and callbacks.onAchievements then
        callbacks.onAchievements()
    elseif item.id == "credits" and callbacks.onCredits then
        callbacks.onCredits()
    end
end

function titleMenu.keypressed(key)
    if key == "up" then
        state.selectedIndex = state.selectedIndex - 1
        if state.selectedIndex < 1 then state.selectedIndex = #MENU_ITEMS end
        return true
    elseif key == "down" then
        state.selectedIndex = state.selectedIndex + 1
        if state.selectedIndex > #MENU_ITEMS then state.selectedIndex = 1 end
        return true
    elseif key == "left" or key == "right" then
        -- Character cycling
        local avail = getAvailableCharacters()
        if #avail > 1 then
            if key == "left" then
                state.charIndex = state.charIndex - 1
                if state.charIndex < 1 then state.charIndex = #avail end
            else
                state.charIndex = state.charIndex + 1
                if state.charIndex > #avail then state.charIndex = 1 end
            end
            local ch = avail[state.charIndex]
            saveData.setSelectedCharacter(ch.id)
            saveData.save()
        end
        return true
    elseif key == "return" or key == "kpenter" or key == "space" then
        executeSelection()
        return true
    end
    return false
end

function titleMenu.touchpressed(x, y)
    local lg = love.graphics
    local w, h = lg.getDimensions()
    local itemH = 45
    local menuStartY = h * 0.40

    for i = 1, #MENU_ITEMS do
        local iy = menuStartY + (i - 1) * itemH
        if y >= iy - 4 and y <= iy + itemH - 4 and x >= w * 0.15 and x <= w * 0.85 then
            state.selectedIndex = i
            executeSelection()
            return true
        end
    end
    return false
end

return titleMenu
