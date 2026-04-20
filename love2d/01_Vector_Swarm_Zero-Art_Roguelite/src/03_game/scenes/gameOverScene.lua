-- GameOverScene
-- 게임오버 결과 오버레이
-- drawBelow=true (전투 화면 보임), transparent=false (게임 정지)

local gameState   = require("03_game.states.gameState")

local GameOverScene = {}
GameOverScene.__index = GameOverScene

GameOverScene.name        = "GameOverScene"
GameOverScene.transparent = false
GameOverScene.drawBelow   = true

local _sceneStack = nil
local _playScene  = nil

function GameOverScene.new(sceneStack, playScene)
    _sceneStack = sceneStack
    _playScene  = playScene
    return setmetatable({}, GameOverScene)
end

function GameOverScene:enter(prev)
    logInfo("[GAMEOVER] GameOverScene entered")
end

function GameOverScene:exit()
    logInfo("[GAMEOVER] GameOverScene exited")
end

function GameOverScene:update(dt)
    -- gameState.draw()에서 타이머 업데이트가 필요할 수 있음
    gameState.update(dt, nil)
end

function GameOverScene:draw()
    -- gameState의 게임오버 UI가 이미 PlayScene:draw()에서 호출됨
    -- 여기서는 추가 오버레이 없음 (gameState.draw()가 PlayScene에 위임됨)
end

function GameOverScene:keypressed(key)
    if key == "r" and gameState.canRestart() then
        _sceneStack:pop()  -- GameOverScene 제거
        _playScene._gameOverPushed = false
        _playScene:restart()
        return true
    end

    if key == "u" and gameState.canRestart() then
        local UpgradeScene = require("03_game.scenes.upgradeScene")
        _sceneStack:push(UpgradeScene.new(_sceneStack))
        return true
    end

    if key == "escape" then
        _playScene:returnToTitle()
        return true
    end

    return false
end

function GameOverScene:touchpressed(id, x, y, dx, dy, pressure)
    if gameState.canRestart() then
        _sceneStack:pop()
        _playScene._gameOverPushed = false
        _playScene:restart()
        return true
    end
    return false
end

return GameOverScene
