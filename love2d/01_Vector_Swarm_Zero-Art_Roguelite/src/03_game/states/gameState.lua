-- Game State Manager
-- Manages game states: title, playing, paused, game_over
-- Tracks score (survival time) and handles restart.

local GameState = {}

local _sin = math.sin

-- States
GameState.TITLE     = "title"
GameState.PLAYING   = "playing"
GameState.PAUSED    = "paused"
GameState.GAME_OVER = "game_over"
GameState.VICTORY   = "victory"

-- Cached fonts (created once on first draw)
local titleFont = nil
local scoreFont = nil

-- State
local state = {
    current     = GameState.TITLE,
    score       = 0,        -- survival time in seconds
    bestScore   = 0,        -- best survival time
    waveReached = 0,
    stage       = 1,
    wave        = 0,
    wavesPerStage = 5,
    gameOverTimer = 0,      -- time since game over (for UI delay)
    restartDelay = 1.0,     -- seconds before restart is allowed
    timeScale    = 1.0,     -- 시간 배율 (포커스 슬로모용)
    fragments   = 0,        -- 이번 런에서 획득한 Data Fragment
}

function GameState.update(dt, playerHealth)
    if state.current == GameState.PLAYING then
        state.score = state.score + dt

        -- Check death
        if playerHealth and not playerHealth.alive then
            state.current = GameState.GAME_OVER
            state.gameOverTimer = 0
            state.timeScale = 1.0  -- 포커스 슬로모 해제
            if stopBGM then stopBGM() end
            if state.score > state.bestScore then
                state.bestScore = state.score
            end
            logInfo(string.format("[GAME] Game Over! Score: %.1fs (Best: %.1fs)", state.score, state.bestScore))
        end

    elseif state.current == GameState.GAME_OVER then
        state.gameOverTimer = state.gameOverTimer + dt
    end
end

function GameState.draw()
    if state.current ~= GameState.GAME_OVER then return end

    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- Dim overlay
    lg.setColor(0, 0, 0, 0.7)
    lg.rectangle("fill", 0, 0, w, h)

    -- Game Over text
    lg.setColor(1, 0.2, 0.2, 1)
    local title = "GAME OVER"
    if not titleFont then titleFont = lg.newFont(36) end
    lg.setFont(titleFont)
    local tw = titleFont:getWidth(title)
    lg.print(title, (w - tw) / 2, h * 0.3)

    -- Score
    lg.setColor(1, 1, 1, 1)
    if not scoreFont then scoreFont = lg.newFont(20) end
    lg.setFont(scoreFont)
    local scoreText = string.format("%.1f sec", state.score)
    local sw = scoreFont:getWidth(scoreText)
    lg.print(scoreText, (w - sw) / 2, h * 0.42)

    -- Best
    lg.setColor(1, 0.8, 0, 1)
    local bestText = string.format("Best: %.1f sec", state.bestScore)
    local bw = scoreFont:getWidth(bestText)
    lg.print(bestText, (w - bw) / 2, h * 0.48)

    -- Wave
    lg.setColor(0.7, 0.7, 0.7, 1)
    local waveText = string.format("Stage %d - Wave %d", state.stage, state.waveReached)
    local ww = scoreFont:getWidth(waveText)
    lg.print(waveText, (w - ww) / 2, h * 0.54)

    -- Fragment 획득 표시
    if state.fragments > 0 then
        lg.setColor(0.4, 0.8, 1.0, 1)
        local fragText = string.format("+ %d DATA FRAGMENTS", state.fragments)
        local fw = scoreFont:getWidth(fragText)
        lg.print(fragText, (w - fw) / 2, h * 0.60)
    end

    -- Restart prompt (after delay)
    if state.gameOverTimer >= state.restartDelay then
        local alpha = 0.5 + 0.5 * _sin(love.timer.getTime() * 3)
        lg.setColor(1, 1, 1, alpha)
        local restartText = "R: restart | U: upgrades"
        local rw = scoreFont:getWidth(restartText)
        lg.print(restartText, (w - rw) / 2, h * 0.70)
    end

    lg.setColor(1, 1, 1, 1)
end

function GameState.canRestart()
    return state.current == GameState.GAME_OVER
       and state.gameOverTimer >= state.restartDelay
end

function GameState.isPlaying()
    return state.current == GameState.PLAYING
end

function GameState.isGameOver()
    return state.current == GameState.GAME_OVER
end

function GameState.isVictory()
    return state.current == GameState.VICTORY
end

function GameState.triggerVictory()
    if state.current ~= GameState.PLAYING then return end
    state.current = GameState.VICTORY
    state.timeScale = 1.0
    if state.score > state.bestScore then
        state.bestScore = state.score
    end
    logInfo(string.format("[GAME] VICTORY! Score: %.1fs, Stage %d", state.score, state.stage))
end

function GameState.continueToEndless()
    if state.current ~= GameState.VICTORY then return end
    state.current = GameState.PLAYING
    logInfo("[GAME] Continuing to Endless mode")
end

function GameState.getScore()
    return state.score
end

function GameState.setStageInfo(stage, wave, wavesPerStage)
    state.stage = stage or state.stage
    state.wave = wave or state.wave
    state.wavesPerStage = wavesPerStage or state.wavesPerStage
end

function GameState.getStageInfo()
    return state.stage, state.wave, state.wavesPerStage
end

function GameState.setWaveReached(wave)
    state.waveReached = wave
end

function GameState.addFragments(amount)
    state.fragments = state.fragments + amount
end

function GameState.getFragments()
    return state.fragments
end

function GameState.getState()
    return {
        current   = state.current,
        score     = state.score,
        bestScore = state.bestScore,
        waveReached = state.waveReached,
        timeScale   = state.timeScale,
        fragments   = state.fragments,
    }
end

function GameState.setTimeScale(scale)
    state.timeScale = scale
end

function GameState.getTimeScale()
    return state.timeScale
end

function GameState.isTitle()
    return state.current == GameState.TITLE
end

function GameState.isPaused()
    return state.current == GameState.PAUSED
end

function GameState.pause()
    if state.current == GameState.PLAYING then
        state.preTimeScale = state.timeScale  -- 포커스 슬로모 보존
        state.current = GameState.PAUSED
        state.timeScale = 0
        logInfo("[GAME] Paused")
    end
end

function GameState.resume()
    if state.current == GameState.PAUSED then
        state.current = GameState.PLAYING
        state.timeScale = state.preTimeScale or 1.0  -- 포커스 슬로모 복원
        state.preTimeScale = nil
        logInfo("[GAME] Resumed")
    end
end

function GameState.toTitle()
    state.current = GameState.TITLE
    state.timeScale = 0
    if stopBGM then stopBGM() end
    logInfo("[GAME] Returned to title")
end

function GameState.startPlaying()
    state.current = GameState.PLAYING
    state.score = 0
    state.gameOverTimer = 0
    state.timeScale = 1.0
    state.stage = 1
    state.wave = 0
    state.waveReached = 0
    state.fragments = 0
    if playBGM then playBGM("stage") end
    logInfo("[GAME] Started playing")
end

return GameState
