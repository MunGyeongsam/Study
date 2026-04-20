-- LevelUpScene
-- 레벨업 3택 오버레이: levelUp.lua를 래핑
-- drawBelow=true (전투 화면 보임), transparent=false (게임 정지)

local levelUp = require("03_game.states.levelUp")

local LevelUpScene = {}
LevelUpScene.__index = LevelUpScene

LevelUpScene.name        = "LevelUpScene"
LevelUpScene.transparent = false
LevelUpScene.drawBelow   = true

function LevelUpScene.new(sceneStack, ecsWorld, playerEntityId)
    return setmetatable({
        _sceneStack = sceneStack,
        _ecsWorld = ecsWorld,
        _playerId = playerEntityId,
    }, LevelUpScene)
end

function LevelUpScene:enter(prev)
    levelUp.show(self._ecsWorld, self._playerId)
    logInfo("[LEVELUP] LevelUpScene entered")
end

function LevelUpScene:exit()
    logInfo("[LEVELUP] LevelUpScene exited")
end

function LevelUpScene:update(dt)
    -- 선택 완료 감지 → 자동 pop
    if not levelUp.isActive() then
        self._sceneStack:pop()
    end
end

function LevelUpScene:draw()
    levelUp.draw()
end

function LevelUpScene:keypressed(key)
    return levelUp.keypressed(key)
end

function LevelUpScene:touchpressed(id, x, y, dx, dy, pressure)
    return levelUp.touchpressed(x, y)
end

return LevelUpScene
