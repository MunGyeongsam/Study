-- VictoryScene
-- OVERFLOW 처치 후 Victory 오버레이
-- drawBelow=true (전투 화면 보임), transparent=false (게임 정지)
-- C: Endless 진입 / ESC: 타이틀 복귀

local gameState         = require("03_game.states.gameState")
local achievementSystem = require("03_game.states.achievementSystem")
local saveData          = require("00_common.saveData")
local ecsManager        = require("03_game.ecsManager")

local _sin   = math.sin
local _floor = math.floor
local _char  = string.char
local _random = math.random
local _min   = math.min
local _max   = math.max

-- Cached fonts (created once on first draw)
local fontTitle   = nil  -- 32pt
local fontGlitch  = nil  -- 28pt
local fontLore    = nil  -- 14pt
local fontStat    = nil  -- 16pt
local fontPrompt  = nil  -- 15pt
local fontToast   = nil  -- 13pt

local VictoryScene = {}
VictoryScene.__index = VictoryScene

VictoryScene.name        = "VictoryScene"
VictoryScene.transparent = false
VictoryScene.drawBelow   = true

function VictoryScene.new(sceneStack, playScene, stats)
    return setmetatable({
        _sceneStack = sceneStack,
        _playScene  = playScene,
        _stats      = stats or {},
        _timer      = 0,
        _toastTimer = 0,
    }, VictoryScene)
end

function VictoryScene:enter(prev)
    self._timer = 0
    self._toastTimer = 0

    -- Record first clear (achievementSystem.onVictory handles saveData internally)
    if not saveData.isGameCleared() then
        achievementSystem.onVictory()
        logInfo("[VICTORY] First clear recorded!")
    end

    logInfo("[VICTORY] VictoryScene entered")
end

function VictoryScene:exit()
    logInfo("[VICTORY] VictoryScene exited")
end

function VictoryScene:update(dt)
    self._timer = self._timer + dt
    self._toastTimer = self._toastTimer + dt
end

-- Glitch text helper (same style as boss intro)
local function glitchText(text, intensity)
    local chars = {}
    for i = 1, #text do
        local c = text:sub(i, i)
        if c ~= " " and _random() < intensity then
            chars[#chars + 1] = _char(_random(33, 126))
        else
            chars[#chars + 1] = c
        end
    end
    return table.concat(chars)
end

function VictoryScene:draw()
    local lg = love.graphics
    local W, H = lg.getDimensions()
    local t = self._timer

    -- Dim overlay (fade in)
    local dimAlpha = _min(0.85, t * 1.5)
    lg.setColor(0, 0, 0, dimAlpha)
    lg.rectangle("fill", 0, 0, W, H)

    -- Phase 1: Glitch intro (0~2s)
    if t < 2.0 then
        local glitchIntensity = _max(0, 1.0 - t * 0.5)
        if not fontGlitch then fontGlitch = lg.newFont(28) end
        lg.setFont(fontGlitch)

        local title = "SYSTEM.EXIT(0)"
        local displayed = glitchText(title, glitchIntensity)

        -- Flicker alpha
        local alpha = 0.5 + 0.5 * _sin(t * 15)
        lg.setColor(0.2, 1.0, 0.4, alpha)
        local tw = fontGlitch:getWidth(displayed)
        lg.print(displayed, (W - tw) / 2, H * 0.25)
        return
    end

    -- Phase 2: Lore text + stats (2s+)
    local fadeIn = _min(1, (t - 2.0) * 2)

    -- Title: "VICTORY" in clean green
    if not fontTitle then fontTitle = lg.newFont(32) end
    lg.setFont(fontTitle)
    lg.setColor(0.2, 1.0, 0.4, fadeIn)
    local title = "SYSTEM.EXIT(0)"
    local tw = fontTitle:getWidth(title)
    lg.print(title, (W - tw) / 2, H * 0.15)

    -- Lore text
    if not fontLore then fontLore = lg.newFont(14) end
    lg.setFont(fontLore)
    local loreAlpha = _min(1, _max(0, (t - 2.5) * 2))
    lg.setColor(0.6, 0.8, 0.7, loreAlpha * fadeIn)

    local lore1 = "The resonance has stopped."
    local lore2 = "But only until the next cycle begins."
    local lw1 = fontLore:getWidth(lore1)
    local lw2 = fontLore:getWidth(lore2)
    lg.print(lore1, (W - lw1) / 2, H * 0.28)
    lg.print(lore2, (W - lw2) / 2, H * 0.32)

    -- Stats
    local statsAlpha = _min(1, _max(0, (t - 3.0) * 2))
    if not fontStat then fontStat = lg.newFont(16) end
    lg.setFont(fontStat)

    local stats = self._stats
    local lines = {
        string.format("TIME: %.1f sec", stats.score or 0),
        string.format("STAGE: %d", stats.stage or 15),
        string.format("KILLS: %d", stats.kills or 0),
        string.format("FRAGMENTS: +%d", stats.fragments or 0),
    }

    local baseY = H * 0.42
    for i, line in ipairs(lines) do
        local lineAlpha = _min(1, _max(0, (t - 3.0 - (i - 1) * 0.3) * 3))
        lg.setColor(0.8, 0.9, 0.85, lineAlpha * statsAlpha * fadeIn)
        local lw = fontStat:getWidth(line)
        lg.print(line, (W - lw) / 2, baseY + (i - 1) * 28)
    end

    -- Prompt (after 4.5s)
    if t >= 4.5 then
        local promptAlpha = 0.5 + 0.5 * _sin(love.timer.getTime() * 3)
        if not fontPrompt then fontPrompt = lg.newFont(15) end
        lg.setFont(fontPrompt)

        lg.setColor(0.3, 1.0, 0.5, promptAlpha)
        local continueText = "C: CONTINUE TO ENDLESS"
        local cw = fontPrompt:getWidth(continueText)
        lg.print(continueText, (W - cw) / 2, H * 0.68)

        lg.setColor(0.7, 0.7, 0.7, promptAlpha * 0.7)
        local returnText = "ESC: RETURN TO TITLE"
        local rw = fontPrompt:getWidth(returnText)
        lg.print(returnText, (W - rw) / 2, H * 0.74)
    end

    -- Achievement unlock toast
    local pending = achievementSystem.getPendingUnlocks()
    if #pending > 0 and self._toastTimer < 6.0 and self._toastTimer > 1.0 then
        local alpha = _min(1, (self._toastTimer - 1.0) / 0.5)
        if self._toastTimer > 5.0 then
            alpha = alpha * _max(0, 1 - (self._toastTimer - 5.0))
        end
        lg.setColor(0.05, 0.15, 0.05, 0.85 * alpha)
        local toastH = 30 * #pending + 15
        lg.rectangle("fill", W * 0.1, H * 0.82, W * 0.8, toastH, 6, 6)
        lg.setColor(0.2, 0.9, 0.4, 0.7 * alpha)
        lg.setLineWidth(1)
        lg.rectangle("line", W * 0.1, H * 0.82, W * 0.8, toastH, 6, 6)

        if not fontToast then fontToast = lg.newFont(13) end
        lg.setFont(fontToast)
        for i, ach in ipairs(pending) do
            local y = H * 0.82 + 8 + (i - 1) * 30
            lg.setColor(1.0, 0.9, 0.3, alpha)
            lg.printf(string.format("NEW! %s", ach.name),
                W * 0.1 + 10, y, W * 0.8 - 20, "center")
        end
    end

    lg.setColor(1, 1, 1, 1)
end

function VictoryScene:keypressed(key)
    -- Consume ALL keys to prevent F8/other keys propagating to PlayScene
    if self._timer < 4.5 then return true end

    if key == "c" then
        -- Continue to Endless
        gameState.continueToEndless()
        ecsManager.stageManager:continueEndless()
        self._sceneStack:pop()
        logInfo("[VICTORY] Continuing to Endless mode")
        return true
    end

    if key == "escape" then
        -- Return to title
        self._playScene:returnToTitle()
        return true
    end

    return true  -- consume all keys (prevent F8 leak to PlayScene)
end

function VictoryScene:touchpressed(id, x, y, dx, dy, pressure)
    if self._timer < 4.5 then return false end
    -- Touch = continue to Endless
    gameState.continueToEndless()
    ecsManager.stageManager:continueEndless()
    self._sceneStack:pop()
    return true
end

return VictoryScene
