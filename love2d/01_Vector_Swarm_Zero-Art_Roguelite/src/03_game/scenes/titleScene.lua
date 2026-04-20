-- TitleScene
-- 타이틀 화면 씬: titleMenu.lua를 래핑

local titleMenu  = require("03_game.states.titleMenu")
local saveData   = require("00_common.saveData")

local TitleScene = {}
TitleScene.__index = TitleScene

TitleScene.name        = "TitleScene"
TitleScene.transparent = false
TitleScene.drawBelow   = false

local _sceneStack = nil

function TitleScene.new(sceneStack)
    _sceneStack = sceneStack
    local self = setmetatable({}, TitleScene)
    return self
end

function TitleScene:enter(prev)
    titleMenu.reset()
    -- 콜백 설정
    titleMenu.setCallbacks({
        onPlay = function()
            local PlayScene = require("03_game.scenes.playScene")
            _sceneStack:replace(PlayScene.new(_sceneStack))
        end,
        onUpgrades = function()
            local UpgradeScene = require("03_game.scenes.upgradeScene")
            _sceneStack:push(UpgradeScene.new(_sceneStack))
        end,
        onCredits = function()
            logInfo("[MENU] Credits (not yet implemented)")
        end,
    })
    if stopBGM then stopBGM() end
    logInfo("[TITLE] TitleScene entered")
end

function TitleScene:exit()
    logInfo("[TITLE] TitleScene exited")
end

function TitleScene:update(dt)
    titleMenu.update(dt)
end

function TitleScene:draw()
    titleMenu.draw()
end

function TitleScene:keypressed(key)
    if key == "escape" then
        love.event.quit()
        return true
    end
    return titleMenu.keypressed(key)
end

function TitleScene:touchpressed(id, x, y, dx, dy, pressure)
    return titleMenu.touchpressed(x, y)
end

return TitleScene
