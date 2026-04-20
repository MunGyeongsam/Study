-- UpgradeScene
-- 업그레이드 트리 오버레이: upgradeTree.lua를 래핑
-- drawBelow=true, transparent=false

local upgradeTree = require("03_game.states.upgradeTree")

local UpgradeScene = {}
UpgradeScene.__index = UpgradeScene

UpgradeScene.name        = "UpgradeScene"
UpgradeScene.transparent = false
UpgradeScene.drawBelow   = true

function UpgradeScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
    }, UpgradeScene)
end

function UpgradeScene:enter(prev)
    upgradeTree.show()
    logInfo("[UPGRADE] UpgradeScene entered")
end

function UpgradeScene:exit()
    upgradeTree.hide()
    logInfo("[UPGRADE] UpgradeScene exited")
end

function UpgradeScene:update(dt)
    -- 닫힘 감지 → 자동 pop
    if not upgradeTree.isActive() then
        self._sceneStack:pop()
    end
end

function UpgradeScene:draw()
    upgradeTree.draw()
end

function UpgradeScene:keypressed(key)
    -- ESC/R로 닫기 시 upgradeTree가 hide() 호출 → update에서 pop됨
    return upgradeTree.keypressed(key)
end

function UpgradeScene:touchpressed(id, x, y, dx, dy, pressure)
    return upgradeTree.touchpressed(x, y)
end

return UpgradeScene
