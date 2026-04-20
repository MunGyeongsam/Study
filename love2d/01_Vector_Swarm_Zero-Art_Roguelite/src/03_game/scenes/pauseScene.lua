-- PauseScene
-- 일시정지 오버레이: pauseMenu.lua를 래핑
-- drawBelow=true (아래 PlayScene 보임), transparent=false (게임 정지)

local pauseMenu = require("03_game.states.pauseMenu")
local gameState = require("03_game.states.gameState")

local PauseScene = {}
PauseScene.__index = PauseScene

PauseScene.name        = "PauseScene"
PauseScene.transparent = false
PauseScene.drawBelow   = true

local _sceneStack = nil
local _playScene  = nil

function PauseScene.new(sceneStack, playScene)
    _sceneStack = sceneStack
    _playScene  = playScene
    return setmetatable({}, PauseScene)
end

function PauseScene:enter(prev)
    pauseMenu.reset()
    pauseMenu.setCallbacks({
        onContinue = function()
            gameState.resume()
            _sceneStack:pop()
        end,
        onRestart = function()
            _sceneStack:pop()  -- PauseScene 제거
            _playScene:restart()
        end,
        onMenu = function()
            _playScene:returnToTitle()
        end,
    })
    logInfo("[PAUSE] PauseScene entered")
end

function PauseScene:exit()
    logInfo("[PAUSE] PauseScene exited")
end

function PauseScene:update(dt)
    -- 게임 정지 상태 — 아무것도 업데이트하지 않음
end

function PauseScene:draw()
    pauseMenu.draw()
end

function PauseScene:keypressed(key)
    return pauseMenu.keypressed(key)
end

function PauseScene:touchpressed(id, x, y, dx, dy, pressure)
    return pauseMenu.touchpressed(x, y)
end

return PauseScene
