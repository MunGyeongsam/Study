-- TitleScene
-- 타이틀 화면 씬: titleMenu.lua를 래핑

local titleMenu  = require("03_game.states.titleMenu")
local saveData   = require("00_common.saveData")

local TitleScene = {}
TitleScene.__index = TitleScene

TitleScene.name        = "TitleScene"
TitleScene.transparent = false
TitleScene.drawBelow   = false

function TitleScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
    }, TitleScene)
end

function TitleScene:enter(prev)
    titleMenu.reset()
    -- 콜백 설정
    titleMenu.setCallbacks({
        onPlay = function()
            local DeitySelectScene = require("03_game.scenes.deitySelectScene")
            self._sceneStack:replace(DeitySelectScene.new(self._sceneStack))
        end,
        onUpgrades = function()
            local UpgradeScene = require("03_game.scenes.upgradeScene")
            self._sceneStack:push(UpgradeScene.new(self._sceneStack))
        end,
        onAchievements = function()
            local AchievementScene = require("03_game.scenes.achievementScene")
            self._sceneStack:push(AchievementScene.new(self._sceneStack))
        end,
        onCredits = function()
            local CreditsScene = require("03_game.scenes.creditsScene")
            self._sceneStack:push(CreditsScene.new(self._sceneStack))
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
    --logInfo("[TITLE] Key pressed: " .. key)
    if key == "escape" then
        love.event.quit()
        return true
    elseif key == "g" then
        local GalleryScene = require("03_game.scenes.galleryScene")
        self._sceneStack:push(GalleryScene.new(self._sceneStack))
        return true
    elseif key == "c" then
        local CurveLabScene = require("03_game.scenes.curveLabScene")
        self._sceneStack:push(CurveLabScene.new(self._sceneStack))
        return true
    end
    return titleMenu.keypressed(key)
end

-- macOS IME 한글 입력 시 keypressed 우회 대응
-- 한글 2벌식: ㅎ=g, ㅊ=c
local JAMO_TO_KEY = { ["ㅎ"] = "g", ["ㅊ"] = "c" }
function TitleScene:textinput(text)
    local key = JAMO_TO_KEY[text] or text:lower()
    if key == "g" or key == "c" then
        return self:keypressed(key)
    end
    return false
end

function TitleScene:touchpressed(id, x, y, dx, dy, pressure)
    return titleMenu.touchpressed(x, y)
end

return TitleScene
