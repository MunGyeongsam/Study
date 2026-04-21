-- Tutorial Hints (첫 플레이 문맥 힌트 시스템)
-- 슬로모 + 딤 오버레이 + 글리치 텍스트 + 액션 완료로 진행
-- Phase: delay → entering → showing → completing → delay → ...

local saveData  = require("00_common.saveData")
local gameState = require("03_game.states.gameState")

local _random = math.random
local _char   = string.char
local _sin    = math.sin
local _min    = math.min
local _abs    = math.abs

local M = {}

-- ===== Hint definitions =====
local STEPS = {
    { id = "move",  text = "W A S D  --  MOVE",             slowScale = 0.15 },
    { id = "dash",  text = "SHIFT  --  DASH",                slowScale = 0.15 },
    { id = "focus", text = "SPACE  --  FOCUS  (SLOW TIME)",  slowScale = 0.15 },
    { id = "auto",  text = "WEAPON FIRES AUTOMATICALLY",     slowScale = 0.15, timed = 3.0 },
}

-- ===== Timing constants =====
local INITIAL_DELAY  = 0.5
local DELAY_BETWEEN  = 1.0
local ENTER_DURATION = 0.4
local EXIT_DURATION  = 0.3

-- ===== State =====
local active      = false
local currentStep = 0
local phase       = "idle"   -- idle | delay | entering | showing | completing
local timer       = 0
local glitchTimer = 0

-- Visual interpolation
local dimAlpha  = 0
local textAlpha = 0

-- Detection latches
local moveDetected  = false
local dashDetected  = false
local focusDetected = false

-- Font (created once)
local hintFont = nil

-- ===== Public API =====

function M.init()
    local stats = saveData.getStats()
    if stats.totalRuns == 0 and not stats.tutorialDone then
        active      = true
        currentStep = 0
        phase       = "delay"
        timer       = INITIAL_DELAY
        dimAlpha    = 0
        textAlpha   = 0
        moveDetected  = false
        dashDetected  = false
        focusDetected = false
        logInfo("[TUTORIAL] Hints activated (first play)")
    else
        active = false
    end
end

function M.isActive()
    return active
end

function M.update(dt, ecs)
    if not active then return end

    if phase == "delay" then
        timer = timer - dt
        if timer <= 0 then
            currentStep = currentStep + 1
            if currentStep > #STEPS then
                M._finish()
                return
            end
            phase = "entering"
            timer = 0
            logInfo(string.format("[TUTORIAL] Step %d: %s", currentStep, STEPS[currentStep].id))
        end

    elseif phase == "entering" then
        timer = timer + dt
        local p = _min(1, timer / ENTER_DURATION)
        local ep = p * p  -- ease-in

        dimAlpha  = ep * 0.35
        textAlpha = ep

        local step = STEPS[currentStep]
        gameState.setTimeScale(1.0 + (step.slowScale - 1.0) * ep)

        if p >= 1 then
            phase = "showing"
            timer = 0
            glitchTimer = 0
        end

    elseif phase == "showing" then
        timer = timer + dt
        glitchTimer = glitchTimer + dt
        dimAlpha  = 0.35
        textAlpha = 1

        local step = STEPS[currentStep]
        gameState.setTimeScale(step.slowScale)

        -- Check completion
        local completed = false
        if step.id == "move" then
            completed = M._checkMove(ecs)
        elseif step.id == "dash" then
            completed = M._checkDash(ecs)
        elseif step.id == "focus" then
            completed = M._checkFocus(ecs)
        elseif step.id == "auto" then
            completed = (timer >= step.timed)
        end

        if completed then
            phase = "completing"
            timer = 0
            glitchTimer = 0
        end

    elseif phase == "completing" then
        timer = timer + dt
        local p = _min(1, timer / EXIT_DURATION)

        dimAlpha  = 0.35 * (1 - p)
        textAlpha = 1 - p

        local step = STEPS[currentStep]
        gameState.setTimeScale(step.slowScale + (1.0 - step.slowScale) * p)

        if p >= 1 then
            gameState.setTimeScale(1.0)
            dimAlpha  = 0
            textAlpha = 0
            phase = "delay"
            timer = DELAY_BETWEEN
        end
    end
end

function M.draw()
    if not active then return end
    if currentStep < 1 or currentStep > #STEPS then return end
    if textAlpha < 0.01 and dimAlpha < 0.01 then return end

    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- Dim overlay
    if dimAlpha > 0.01 then
        lg.setColor(0, 0, 0, dimAlpha)
        lg.rectangle("fill", 0, 0, w, h)
    end

    -- Font (lazy init)
    if not hintFont then
        hintFont = lg.newFont(22)
    end
    lg.setFont(hintFont)

    -- Build display text with glitch
    local step = STEPS[currentStep]
    local text = step.text

    if phase == "entering" then
        local p = _min(1, (timer or 0) / ENTER_DURATION)
        text = M._glitchText(text, 1 - p)
    elseif phase == "showing" then
        local pulse = _sin(glitchTimer * 4) * 0.05 + 0.02
        text = M._glitchText(text, pulse)
    elseif phase == "completing" then
        local p = _min(1, (timer or 0) / EXIT_DURATION)
        text = M._glitchText(text, p)
    end

    -- Text position: slightly above center
    local tw = hintFont:getWidth(text)
    local tx = (w - tw) / 2
    local ty = h * 0.38

    -- Glow shadow (4 offset draws)
    local pulseA = 0.25 + 0.15 * _sin(love.timer.getTime() * 3)
    lg.setColor(0.2, 0.6, 1.0, textAlpha * pulseA)
    lg.print(text, tx - 1, ty - 1)
    lg.print(text, tx + 1, ty + 1)
    lg.print(text, tx - 1, ty + 1)
    lg.print(text, tx + 1, ty - 1)

    -- Main text
    lg.setColor(0.4, 0.9, 1.0, textAlpha)
    lg.print(text, tx, ty)

    resetColor()
end

-- ===== Detection helpers =====

function M._checkMove(ecs)
    if moveDetected then return true end
    local entities = ecs:queryEntities({"PlayerTag", "Input"})
    if #entities > 0 then
        local input = ecs:getComponent(entities[1], "Input")
        if _abs(input.moveX) > 0.01 or _abs(input.moveY) > 0.01 then
            moveDetected = true
            return true
        end
    end
    return false
end

function M._checkDash(ecs)
    if dashDetected then return true end
    local entities = ecs:queryEntities({"PlayerTag", "Dash"})
    if #entities > 0 then
        local dash = ecs:getComponent(entities[1], "Dash")
        if dash.cooldownTimer > 0 then
            dashDetected = true
            return true
        end
    end
    return false
end

function M._checkFocus(ecs)
    if focusDetected then return true end
    local entities = ecs:queryEntities({"PlayerTag", "Focus"})
    if #entities > 0 then
        local focus = ecs:getComponent(entities[1], "Focus")
        if focus.active then
            focusDetected = true
            return true
        end
    end
    return false
end

-- ===== Internal =====

function M._finish()
    active    = false
    phase     = "idle"
    dimAlpha  = 0
    textAlpha = 0
    gameState.setTimeScale(1.0)

    -- Mark tutorial done in save data
    saveData.setTutorialDone(true)
    saveData.save()
    logInfo("[TUTORIAL] All hints complete!")
end

function M._glitchText(original, intensity)
    if intensity < 0.01 then return original end
    local chars = {}
    for i = 1, #original do
        local c = original:sub(i, i)
        if c ~= " " and _random() < intensity then
            chars[i] = _char(_random(33, 126))
        else
            chars[i] = c
        end
    end
    return table.concat(chars)
end

return M
