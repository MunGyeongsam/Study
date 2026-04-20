-- GameOverScene
-- 게임오버 결과 오버레이
-- drawBelow=true (전투 화면 보임), transparent=false (게임 정지)

local gameState         = require("03_game.states.gameState")
local achievementSystem = require("03_game.states.achievementSystem")

local GameOverScene = {}
GameOverScene.__index = GameOverScene

GameOverScene.name        = "GameOverScene"
GameOverScene.transparent = false
GameOverScene.drawBelow   = true

function GameOverScene.new(sceneStack, playScene)
    return setmetatable({
        _sceneStack = sceneStack,
        _playScene  = playScene,
        _toastTimer = 0,
    }, GameOverScene)
end

function GameOverScene:enter(prev)
    self._toastTimer = 0
    logInfo("[GAMEOVER] GameOverScene entered")
end

function GameOverScene:exit()
    logInfo("[GAMEOVER] GameOverScene exited")
end

function GameOverScene:update(dt)
    -- gameState.draw()에서 타이머 업데이트가 필요할 수 있음
    gameState.update(dt, nil)
    self._toastTimer = self._toastTimer + dt
end

function GameOverScene:draw()
    -- Achievement unlock toast
    local pending = achievementSystem.getPendingUnlocks()
    if #pending > 0 and self._toastTimer < 5.0 then
        local lg = love.graphics
        local W, H = lg.getDimensions()
        local toastH = 30 * #pending + 15
        local toastY = H * 0.62

        -- Toast background
        local alpha = math.min(1, self._toastTimer / 0.5)
        if self._toastTimer > 4.0 then
            alpha = alpha * math.max(0, 1 - (self._toastTimer - 4.0))
        end
        lg.setColor(0.05, 0.12, 0.05, 0.85 * alpha)
        lg.rectangle("fill", W * 0.1, toastY, W * 0.8, toastH, 6, 6)
        lg.setColor(0.2, 0.8, 0.4, 0.7 * alpha)
        lg.setLineWidth(1)
        lg.rectangle("line", W * 0.1, toastY, W * 0.8, toastH, 6, 6)

        for i, ach in ipairs(pending) do
            local y = toastY + 8 + (i - 1) * 30
            lg.setColor(1.0, 0.9, 0.3, alpha)
            lg.printf(string.format("NEW! %s — %s", ach.name, ach.reward.name),
                W * 0.1 + 10, y, W * 0.8 - 20, "center")
        end
        lg.setColor(1, 1, 1, 1)
    end
end

function GameOverScene:keypressed(key)
    if key == "r" and gameState.canRestart() then
        self._sceneStack:pop()  -- GameOverScene 제거
        self._playScene._gameOverPushed = false
        self._playScene:restart()
        return true
    end

    if key == "u" and gameState.canRestart() then
        local UpgradeScene = require("03_game.scenes.upgradeScene")
        self._sceneStack:push(UpgradeScene.new(self._sceneStack))
        return true
    end

    if key == "escape" then
        self._playScene:returnToTitle()
        return true
    end

    return false
end

function GameOverScene:touchpressed(id, x, y, dx, dy, pressure)
    if gameState.canRestart() then
        self._sceneStack:pop()
        self._playScene._gameOverPushed = false
        self._playScene:restart()
        return true
    end
    return false
end

return GameOverScene
