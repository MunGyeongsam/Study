-- PlayScene
-- 전투 플레이 씬: ECS 업데이트 + 월드 렌더링 + 게임플레이 Input
-- 기존 main.lua 게임 루프를 래핑한 Scene

local cameraManager = require("02_renderer.cameraManager")
local bloom         = require("02_renderer.bloom")
local background    = require("02_renderer.background")
local uiManager     = require("04_ui.uiManager")
local player        = require("03_game.entities.player")
local ecsManager    = require("03_game.ecsManager")
local gameState     = require("03_game.states.gameState")
local levelUp       = require("03_game.states.levelUp")
local upgradeTree   = require("03_game.states.upgradeTree")
local world         = require("01_core.world")

local _floor = math.floor

local PlayScene = {}
PlayScene.__index = PlayScene

-- Scene 속성
PlayScene.name        = "PlayScene"
PlayScene.transparent = false
PlayScene.drawBelow   = false

-- 게임 내 공유 상태 (static getter/setter 경유)
local _hitStopTimer  = 0
local _godMode       = false
local _showWorldGrid = false

--- Scene 생성
function PlayScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
        _gameOverPushed = false,
    }, PlayScene)
end

--- 외부에서 설정하는 공유 상태
function PlayScene.setGodMode(v) _godMode = v end
function PlayScene.getGodMode() return _godMode end
function PlayScene.setShowWorldGrid(v) _showWorldGrid = v end
function PlayScene.getShowWorldGrid() return _showWorldGrid end
function PlayScene.getHitStopTimer() return _hitStopTimer end

--- 게임 새로 시작 (타이틀 → 플레이, 리스타트 공용)
function PlayScene:_initGame()
    self._gameOverPushed = false
    ecsManager.restart()
    local playerId = ecsManager.createPlayer(0, -12)
    player.bind(ecsManager.getWorld(), playerId)
    player.init(0, -12)
    upgradeTree.applyToPlayer(ecsManager.getWorld(), playerId)

    local px, py = player.getPosition()
    cameraManager.getGameCamera():lookAt(px, py)
    cameraManager.getGameCamera():setOrthographicSize(5)

    background.init(1)
    background.setStage(1)
    gameState.startPlaying()
    levelUp.reset()
    _hitStopTimer = 0
end

--- 런 결과 저장 (returnToTitle, restart 공용)
function PlayScene:_saveRunResult()
    local saveData = require("00_common.saveData")
    local runFragments = gameState.getFragments()
    if runFragments > 0 then
        saveData.addFragments(runFragments)
    end
    local score = gameState.getScore()
    local stageInfo = gameState.getStageInfo()
    saveData.recordRun(score, stageInfo)
    saveData.save()
end

--- 타이틀로 복귀
function PlayScene:returnToTitle()
    self:_saveRunResult()
    self._sceneStack:clear()
    local TitleScene = require("03_game.scenes.titleScene")
    self._sceneStack:push(TitleScene.new(self._sceneStack))
end

--- 리스타트
function PlayScene:restart()
    self:_saveRunResult()
    self:_initGame()
    logInfo("[GAME] Restarted!")
end

-- ===== Scene 인터페이스 =====

function PlayScene:enter(prev)
    self:_initGame()
    logInfo("[GAME] PlayScene entered")
end

function PlayScene:exit()
    logInfo("[GAME] PlayScene exited")
end

function PlayScene:update(dt)
    -- Hit-stop freeze
    if _hitStopTimer > 0 then
        _hitStopTimer = _hitStopTimer - dt
        return
    end

    -- 스테이지 클리어 중 제한 업데이트
    if ecsManager.stageManager:isClearing() then
        local stageState = ecsManager.stageManager.state
        if stageState == "boss_clear" and not ecsManager.stageManager.bossRewardsApplied then
            ecsManager.stageManager.bossRewardsApplied = true
            _hitStopTimer = 0.4
            screenShake(0.5, 0.7)
            local w = ecsManager.getWorld()
            local pEnts = w:queryEntities({"PlayerTag", "Health"})
            if #pEnts > 0 then
                local pHealth = w:getComponent(pEnts[1], "Health")
                pHealth.hp = pHealth.maxHp
                pHealth.iTimer = 0
                local dash = w:getComponent(pEnts[1], "Dash")
                if dash then dash.cooldownTimer = 0 end
                local focus = w:getComponent(pEnts[1], "Focus")
                if focus then focus.energy = focus.maxEnergy end
            end
            logInfo("[BOSS] Rewards applied: HP full, dash/focus reset")
        end

        if stageState == "collecting" then
            local moveSys = ecsManager.systems["Movement"]
            if moveSys then moveSys:update(ecsManager.getWorld(), dt) end
            local xpSys = ecsManager.systems["XpCollection"]
            if xpSys then xpSys:update(ecsManager.getWorld(), dt) end

            local w = ecsManager.getWorld()
            local xpEntities = w:queryEntities({"PlayerTag", "PlayerXP"})
            if #xpEntities > 0 then
                local playerXP = w:getComponent(xpEntities[1], "PlayerXP")
                if playerXP.pendingLevelUp then
                    playerXP.pendingLevelUp = false
                    local LevelUpScene = require("03_game.scenes.levelUpScene")
                    self._sceneStack:push(LevelUpScene.new(self._sceneStack, w, xpEntities[1]))
                end
            end
        end

        ecsManager.stageManager:update(dt)
        return
    end

    -- 게임오버
    if gameState.isGameOver() then
        gameState.update(dt, nil)
        -- canRestart 가능해지면 GameOverScene push (한 번만)
        if gameState.canRestart() and not self._gameOverPushed then
            self._gameOverPushed = true
            local GameOverScene = require("03_game.scenes.gameOverScene")
            self._sceneStack:push(GameOverScene.new(self._sceneStack, self))
        end
        return
    end

    -- 일반 플레이
    local scaledDt = dt * gameState.getTimeScale()
    ecsManager.update(scaledDt)

    if _godMode then
        local pid = player.getEntityId()
        if pid then
            local health = ecsManager.getWorld():getComponent(pid, "Health")
            if health then health.iTimer = 9999 end
        end
    end

    player.update(dt, {})

    local playerX, playerY = player.getCameraTarget()
    cameraManager.update(dt, playerX, playerY)

    background.update(dt)
    uiManager.update(dt)

    -- UI 데이터 전달
    local worldStats = world.getWorldStats()
    local playerStats = player.getStats()
    local stageStats = ecsManager.getStats().stage

    uiManager.setGameData({
        score = _floor(gameState.getScore()),
        lives = 3,
        level = playerStats.zonesVisited + 1,
        fps = love.timer.getFPS(),
        stage = stageStats.stage,
        wave = stageStats.wave,
        wavesPerStage = stageStats.wavesPerStage,
        progress = playerStats.progress,
        powerUps = #playerStats.powerUps,
        secrets = worldStats.secretsDiscovered,
        currentZone = playerStats.currentZone or "outside",
        checkpoints = playerStats.checkpoints,
        bossActive = stageStats.bossEntityId ~= nil and (stageStats.state == "boss_intro" or stageStats.state == "boss_active"),
        bossName = stageStats.bossType,
        bossHp = 0, bossMaxHp = 1, bossPhase = 1, bossMaxPhase = 1,
    })

    if stageStats.bossEntityId then
        local bw = ecsManager.getWorld()
        local bossHealth = bw:getComponent(stageStats.bossEntityId, "Health")
        local bossTag = bw:getComponent(stageStats.bossEntityId, "BossTag")
        if bossHealth and bossTag then
            uiManager.setGameData({
                bossHp = bossHealth.hp,
                bossMaxHp = bossHealth.maxHp,
                bossPhase = bossTag.phase,
                bossMaxPhase = bossTag.maxPhase,
            })
        end
    end

    -- 게임 상태 업데이트
    local w = ecsManager.getWorld()
    local playerEntities = w:queryEntities({"PlayerTag", "Health"})
    local playerHealth = #playerEntities > 0 and w:getComponent(playerEntities[1], "Health") or nil
    gameState.setStageInfo(stageStats.stage, stageStats.wave, stageStats.wavesPerStage)
    gameState.setWaveReached(stageStats.totalWaves)
    gameState.update(dt, playerHealth)

    -- 레벨업 체크
    local xpEntities = w:queryEntities({"PlayerTag", "PlayerXP"})
    if #xpEntities > 0 then
        local playerXP = w:getComponent(xpEntities[1], "PlayerXP")
        if playerXP.pendingLevelUp then
            playerXP.pendingLevelUp = false
            local LevelUpScene = require("03_game.scenes.levelUpScene")
            self._sceneStack:push(LevelUpScene.new(self._sceneStack, w, xpEntities[1]))
        end
    end
end

function PlayScene:draw()
    -- Bloom + 월드 렌더링
    bloom.beginCapture()
    cameraManager.draw(function()
        local cam = cameraManager.getActive()
        background.draw(cam)
        if _showWorldGrid then
            world.drawGrid(1, cam)
        end
        ecsManager.draw()
    end)
    bloom.endCapture()
    bloom.draw()

    -- HUD
    uiManager.draw()

    -- 스테이지 클리어/게임오버 오버레이 (PlayScene 소속)
    gameState.draw()
    ecsManager.stageManager:draw()
end

function PlayScene:keypressed(key)
    -- F-keys (게임 내 디버그)
    if key == "f5" then
        cameraManager.toggle()
        return true
    elseif key == "f6" then
        bloom.toggle()
        return true
    elseif key == "f9" then
        background.toggle()
        return true
    elseif key == "f10" then
        background.cycleStyle()
        return true
    elseif key == "f11" then
        background.adjustC(-0.05)
        return true
    elseif key == "f12" then
        background.adjustC(0.05)
        return true
    elseif key == "f7" then
        _godMode = not _godMode
        logInfo(string.format("[DEBUG] God mode: %s", _godMode and "ON" or "OFF"))
        return true
    elseif key == "f8" then
        local sm = ecsManager.getStageManager and ecsManager.getStageManager()
        if sm then sm:debugSkipStage() end
        return true
    end

    -- ESC → 일시정지 (플레이 중에만)
    if key == "escape" and gameState.isPlaying() then
        gameState.pause()
        local PauseScene = require("03_game.scenes.pauseScene")
        self._sceneStack:push(PauseScene.new(self._sceneStack, self))
        return true
    end

    -- Shift → 대쉬
    if key == "lshift" or key == "rshift" then
        if gameState.isPlaying() then
            local pid = player.getEntityId()
            if pid then
                local inp = ecsManager.getWorld():getComponent(pid, "Input")
                if inp then inp.dash = true end
            end
        end
        return true
    end

    -- 디버그 카메라
    if cameraManager.isDebug() then
        if key == "=" or key == "+" then
            cameraManager.getActive():setOrthographicSize(cameraManager.getActive():getOrthographicSize() * 0.8)
            return true
        elseif key == "-" then
            cameraManager.getActive():setOrthographicSize(cameraManager.getActive():getOrthographicSize() * 1.25)
            return true
        elseif key == "space" then
            local px, py = player.getPosition()
            cameraManager.getActive():lookAt(px, py)
            return true
        end
    end

    return false
end

function PlayScene:touchpressed(id, x, y, dx, dy, pressure)
    -- UI 터치
    if uiManager.touchpressed(id, x, y, dx, dy, pressure) then
        return true
    end
    -- 게임 영역 터치
    local mobileLayout = require("04_ui.mobileLayout")
    if mobileLayout.isTouchInArea(x, y, "play") then
        local worldX, worldY = cameraManager.worldCoords(x, y)
        log(string.format("Touch at world: (%.1f, %.1f)", worldX, worldY))
    end
    return false
end

function PlayScene:touchmoved(id, x, y, dx, dy, pressure)
    if uiManager.touchmoved(id, x, y, dx, dy, pressure) then
        return true
    end
    if cameraManager.isDebug() then
        cameraManager.debugMove(dx, dy)
    end
    return false
end

function PlayScene:touchreleased(id, x, y, dx, dy, pressure)
    uiManager.touchreleased(id, x, y, dx, dy, pressure)
end

return PlayScene
