-- ============================================================================
-- playScene.lua — 메인 전투 루프 씬
-- ============================================================================
--
-- ◆ 역할
--   ECS 업데이트 + 카메라 추적 + 블룸 렌더 + UI 오버레이.
--   스테이지 클리어/보스 연출/게임오버 판정을 처리한다.
--
-- ◆ 렌더 파이프라인
--   bloom.begin → cameraManager.draw(background + ECS + trail) → bloom.end → UI
--
-- ◆ 디버그 (F5=카메라, F7=갓모드, F8=스테이지스킵, F9=DNA스폰)
--
-- ◆ 주요 의존
--   ecsManager, cameraManager, bloom, background, uiManager, gameState

local cameraManager = require("02_renderer.cameraManager")
local bloom         = require("02_renderer.bloom")
local background    = require("02_renderer.background")
local uiManager     = require("04_ui.uiManager")
local player        = require("03_game.entities.player")
local ecsManager    = require("03_game.ecsManager")
local gameState     = require("03_game.states.gameState")
local levelUp       = require("03_game.states.levelUp")
local upgradeTree   = require("03_game.states.upgradeTree")
local deityDefs     = require("03_game.data.deityDefs")
local achievementSystem = require("03_game.states.achievementSystem")
local tutorialHints     = require("03_game.states.tutorialHints")
local trailRenderer     = require("02_renderer.trailRenderer")
local renderSystem      = require("03_game.systems.renderSystem")
local DashSystem        = require("03_game.systems.dashSystem")
local world         = require("01_core.world")

local _floor = math.floor
local _min   = math.min
local _max   = math.max
local _exp   = math.exp

local PlayScene = {}
PlayScene.__index = PlayScene

-- Scene 속성
PlayScene.name        = "PlayScene"
PlayScene.transparent = false
PlayScene.drawBelow   = false

-- 게임 내 공유 상태 (static getter/setter 경유)
local _hitStopTimer  = 0
local _godMode       = false
local _disableWeapon = false
local _showWorldGrid = false

-- Boss visual effect state
local _screenFlash   = 0      -- white flash alpha (decays to 0)
local _screenTint    = nil    -- {r,g,b, alpha} color tint (decays)
local _whiteout      = 0      -- whiteout alpha (for boss defeat)
local _whiteoutDecay = false  -- true = fading out phase
local _shockwave     = nil    -- {x,y, radius, maxRadius, alpha} expanding ring
local _phaseText     = nil    -- {text, timer, duration}
local _forceDnaToastTimer = 0
local _forceDnaToastText  = nil

--- Scene 생성
--- @param sceneStack table
--- @param deityId string|nil  선택된 Deity ID (nil이면 버프 없음)
function PlayScene.new(sceneStack, deityId)
    return setmetatable({
        _sceneStack = sceneStack,
        _gameOverPushed = false,
        _deityId = deityId,
    }, PlayScene)
end

--- 외부에서 설정하는 공유 상태
function PlayScene.setGodMode(v) _godMode = v end
function PlayScene.getGodMode() return _godMode end
function PlayScene.getDisableWeapon() return _disableWeapon end
function PlayScene.setShowWorldGrid(v) _showWorldGrid = v end
function PlayScene.getShowWorldGrid() return _showWorldGrid end
function PlayScene.getHitStopTimer() return _hitStopTimer end

function PlayScene.toggleForceDnaModeGlobal()
    local sm = ecsManager.getStageManager and ecsManager.getStageManager()
    if not sm then return nil end
    sm:setForceDnaSpawn(not sm:isForceDnaSpawn())
    return sm:isForceDnaSpawn()
end

function PlayScene:_toggleForceDnaMode()
    local isOn = PlayScene.toggleForceDnaModeGlobal()
    if isOn == nil then return false end

    _forceDnaToastTimer = 1.2
    _forceDnaToastText = isOn and "DNA FORCE MODE: ON" or "DNA FORCE MODE: OFF"
    return true
end

--- 게임 새로 시작 (타이틀 → 플레이, 리스타트 공용)
function PlayScene:_initGame()
    self._gameOverPushed = false
    self._victoryPushed  = false
    ecsManager.restart()
    local saveData = require("00_common.saveData")
    local charId = saveData.getSelectedCharacter()
    local playerId = ecsManager.createPlayer(0, -12, charId)
    player.bind(ecsManager.getWorld(), playerId)
    player.init(0, -12)
    upgradeTree.applyToPlayer(ecsManager.getWorld(), playerId)

    -- Deity 패시브 적용
    if self._deityId then
        deityDefs.applyStats(ecsManager.getWorld(), playerId, self._deityId)
        local tag = ecsManager.getWorld():getComponent(playerId, "PlayerTag")
        if tag then tag.deityId = self._deityId end
    end

    local px, py = player.getPosition()
    cameraManager.getGameCamera():lookAt(px, py)
    cameraManager.getGameCamera():setOrthographicSize(5)

    background.init(1)
    background.setStage(1)
    gameState.startPlaying()
    levelUp.reset()
    achievementSystem.resetSession()
    trailRenderer.reset()
    DashSystem.setOnDashCallback(trailRenderer.onDash)
    _hitStopTimer = 0
    _screenFlash  = 0
    _screenTint   = nil
    _whiteout     = 0
    _whiteoutDecay = false
    _shockwave    = nil
    _phaseText    = nil
    _forceDnaToastTimer = 0
    _forceDnaToastText = nil

    -- Reset runtime-tuned DNA curve thickness on each run start.
    do
        local cur = renderSystem.getCurveOverlayThickness and renderSystem.getCurveOverlayThickness() or 1.0
        renderSystem.adjustCurveOverlayThickness(1.0 - cur)
    end

    -- Start Boost: 해금되면 게임 시작 시 랜덤 업그레이드 1개 자동 적용
    if achievementSystem.isRewardUnlocked("start_boost") then
        levelUp.applyRandomUpgrade(ecsManager.getWorld(), playerId)
        logInfo("[BOOST] Start Boost applied")
    end

    -- Tutorial: 첫 플레이 시 문맥 힌트
    tutorialHints.init()
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
    -- Record Endless stage if past stage 15
    if stageInfo > 15 then
        saveData.recordEndlessStage(stageInfo)
    end
    achievementSystem.onRunEnd()
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
    -- === Visual effects update (always, even during hit-stop) ===
    if _screenFlash > 0.001 then
        _screenFlash = _screenFlash * _exp(-15 * dt)
        if _screenFlash < 0.001 then _screenFlash = 0 end
    end
    if _screenTint then
        _screenTint[4] = _screenTint[4] * _exp(-10 * dt)
        if _screenTint[4] < 0.01 then _screenTint = nil end
    end
    if _whiteout > 0.001 then
        if _whiteoutDecay then
            _whiteout = _whiteout * _exp(-4 * dt)
            if _whiteout < 0.01 then _whiteout = 0 end
        end
    end
    if _shockwave then
        _shockwave.radius = _shockwave.radius + dt * _shockwave.maxRadius * 1.5
        _shockwave.alpha = 1 - (_shockwave.radius / _shockwave.maxRadius)
        if _shockwave.alpha <= 0 then _shockwave = nil end
    end
    if _phaseText then
        _phaseText.timer = _phaseText.timer + dt
        if _phaseText.timer >= _phaseText.duration then _phaseText = nil end
    end
    if _forceDnaToastTimer > 0 then
        _forceDnaToastTimer = _max(0, _forceDnaToastTimer - dt)
    end

    -- Hit-stop freeze
    if _hitStopTimer > 0 then
        _hitStopTimer = _hitStopTimer - dt
        -- Start whiteout decay after hit-stop ends
        if _hitStopTimer <= 0 and _whiteout > 0 then
            _whiteoutDecay = true
        end
        return
    end

    -- 스테이지 클리어 중 제한 업데이트
    if ecsManager.stageManager:isClearing() then
        local stageState = ecsManager.stageManager.state
        if stageState == "boss_clear" and not ecsManager.stageManager.bossRewardsApplied then
            ecsManager.stageManager.bossRewardsApplied = true
            _hitStopTimer = 0.4
            screenShake(0.5, 0.7)
            -- Whiteout + shockwave at boss position
            _whiteout = 1.0
            _whiteoutDecay = false
            local bossId = ecsManager.stageManager.bossEntityId
            if bossId then
                local bw = ecsManager.getWorld()
                local bt = bw:getComponent(bossId, "Transform")
                if bt then
                    -- Convert boss world pos to screen for shockwave
                    local sx, sy = cameraManager.getActive():cameraCoords(bt.x, bt.y)
                    local sw, sh = love.graphics.getDimensions()
                    _shockwave = { x = sx, y = sy, radius = 0, maxRadius = _max(sw, sh) * 0.8, alpha = 1 }
                end
            end
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

    -- Victory
    if gameState.isVictory() and not self._victoryPushed then
        self._victoryPushed = true
        -- Enhanced whiteout for victory
        _whiteout = 1.0
        _whiteoutDecay = false
        _hitStopTimer = 0.6
        local VictoryScene = require("03_game.scenes.victoryScene")
        local stats = {
            score     = gameState.getScore(),
            stage     = ecsManager.stageManager.stage - 1,  -- OVERFLOW was stage 15
            kills     = achievementSystem.getSessionKills(),
            fragments = gameState.getFragments(),
        }
        self._sceneStack:push(VictoryScene.new(self._sceneStack, self, stats))
        return
    end
    if gameState.isVictory() then return end

    -- 일반 플레이
    local scaledDt = dt * gameState.getTimeScale()
    ecsManager.update(scaledDt)

    -- === Boss visual event detection ===
    local bossId = ecsManager.stageManager and ecsManager.stageManager.bossEntityId
    if bossId then
        local bw = ecsManager.getWorld()
        local boss = bw:getComponent(bossId, "BossTag")
        if boss then
            -- Intro flash: triggered once when intro completes
            if boss.introFlash then
                boss.introFlash = false
                _screenFlash = 0.8
            end
            -- Phase transition: hit-stop + color flash + phase text
            if boss.phaseHitStop then
                boss.phaseHitStop = false
                _hitStopTimer = 0.15
            end
            if boss.phaseFlash then
                boss.phaseFlash = false
                local rend = bw:getComponent(bossId, "Renderable")
                if rend then
                    _screenTint = { rend.color[1], rend.color[2], rend.color[3], 0.4 }
                end
                _phaseText = { text = string.format("PHASE %d", boss.phase), timer = 0, duration = 0.8 }
            end
        end
    end

    if _godMode then
        local pid = player.getEntityId()
        if pid then
            local health = ecsManager.getWorld():getComponent(pid, "Health")
            if health then
                health.hp = 999
                health.maxHp = 999
            end
        end
    end

    player.update(dt, {})

    -- 리본 트레일 위치 기록 (매 프레임)
    local px, py = player.getPosition()
    if px then trailRenderer.update(dt, px, py) end

    -- Tutorial hints (timeScale 오버라이드: ecsManager 이후 실행)
    tutorialHints.update(dt, ecsManager.getWorld())

    local playerX, playerY = player.getCameraTarget()
    cameraManager.update(dt, playerX, playerY)

    background.update(dt)
    uiManager.update(dt)
    uiManager.setMinimapData(ecsManager.getWorld(), player, cameraManager.getActive())

    -- UI 데이터 전달
    local worldStats = world.getWorldStats()
    local playerStats = player.getStats()
    local stageStats = ecsManager.getStats().stage

    -- 플레이어 HP 읽기
    local w = ecsManager.getWorld()
    local playerEntities = w:queryEntities({"PlayerTag", "Health"})
    local playerHealth = #playerEntities > 0 and w:getComponent(playerEntities[1], "Health") or nil
    local pHp    = playerHealth and playerHealth.hp or 0
    local pMaxHp = playerHealth and playerHealth.maxHp or 1

    uiManager.setGameData({
        score = _floor(gameState.getScore()),
        lives = pHp,
        maxLives = pMaxHp,
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
        bossName = (stageStats.bossType and stageStats.bossScaling and stageStats.bossScaling.round)
            and string.format("%s +%d", stageStats.bossType, stageStats.bossScaling.round)
            or stageStats.bossType,
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
        trailRenderer.draw()
    end)
    bloom.endCapture()
    bloom.draw()

    -- === Boss screen effects (screen-space, over bloom, under UI) ===
    local lg = love.graphics
    local sw, sh = lg.getDimensions()

    -- Shockwave ring
    if _shockwave then
        lg.setColor(1, 1, 1, _shockwave.alpha * 0.7)
        lg.setLineWidth(3)
        lg.circle("line", _shockwave.x, _shockwave.y, _shockwave.radius)
        lg.setLineWidth(1)
    end

    -- Screen flash (white)
    if _screenFlash > 0.01 then
        lg.setColor(1, 1, 1, _screenFlash)
        lg.rectangle("fill", 0, 0, sw, sh)
    end

    -- Screen tint (boss color)
    if _screenTint then
        lg.setColor(_screenTint[1], _screenTint[2], _screenTint[3], _screenTint[4])
        lg.rectangle("fill", 0, 0, sw, sh)
    end

    -- Whiteout (boss defeat)
    if _whiteout > 0.01 then
        lg.setColor(1, 1, 1, _whiteout)
        lg.rectangle("fill", 0, 0, sw, sh)
    end

    -- Phase text
    if _phaseText then
        local alpha = 1.0
        local t = _phaseText.timer
        if t < 0.1 then alpha = t / 0.1
        elseif t > _phaseText.duration - 0.2 then alpha = (_phaseText.duration - t) / 0.2 end
        lg.setColor(1, 0.3, 0.3, alpha)
        local font = lg.getFont()
        local text = _phaseText.text
        local tw = font:getWidth(text)
        local scale = 2.0
        lg.print(text, (sw - tw * scale) / 2, sh * 0.35, 0, scale, scale)
    end

    lg.setColor(1, 1, 1, 1)

    -- HUD
    uiManager.draw()

    -- Tutorial overlay
    tutorialHints.draw()

    -- 스테이지 클리어/게임오버 오버레이 (PlayScene 소속)
    gameState.draw()
    ecsManager.stageManager:draw()

    -- Force DNA spawn indicator (always visible)
    do
        local sm = ecsManager.getStageManager and ecsManager.getStageManager()
        if sm then
            local isOn = sm:isForceDnaSpawn()
            local pad = 10
            local text = string.format("DNA FORCE: %s [F]", isOn and "ON" or "OFF")
            local font = lg.getFont()
            local tw = font:getWidth(text)
            local th = font:getHeight()
            local bx, by = sw - tw - pad * 2 - 10, 10
            local bw, bh = tw + pad * 2, th + pad + 2

            if isOn then
                lg.setColor(0.92, 0.2, 0.2, 0.95)
            else
                lg.setColor(0.2, 0.22, 0.28, 0.85)
            end
            lg.rectangle("fill", bx, by, bw, bh, 6, 6)
            lg.setColor(1, 1, 1, isOn and 0.9 or 0.5)
            lg.rectangle("line", bx, by, bw, bh, 6, 6)
            lg.setColor(1, 1, 1, 1)
            lg.print(text, bx + pad, by + pad * 0.5 + 1)
        end
    end

    -- Toggle toast (brief center hint)
    if _forceDnaToastTimer > 0 and _forceDnaToastText then
        local a = _min(1, _forceDnaToastTimer / 0.25)
        local text = _forceDnaToastText
        local font = lg.getFont()
        local tw = font:getWidth(text)
        local th = font:getHeight()
        local bx = (sw - tw) * 0.5 - 12
        local by = sh * 0.18
        local bw = tw + 24
        local bh = th + 12

        lg.setColor(0.05, 0.05, 0.08, 0.78 * a)
        lg.rectangle("fill", bx, by, bw, bh, 8, 8)
        lg.setColor(1, 1, 1, 1 * a)
        lg.print(text, bx + 12, by + 6)
    end
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
        _disableWeapon = false  -- 밸런스 테스트: godMode에서도 무기 활성화
        logInfo(string.format("[DEBUG] God mode: %s (weapon: %s)", _godMode and "ON" or "OFF", _disableWeapon and "OFF" or "ON"))
        return true
    elseif key == "f8" then
        if not gameState.isVictory() then
            local sm = ecsManager.getStageManager and ecsManager.getStageManager()
            if sm then sm:debugSkipStage() end
        end
        return true
    elseif key == "[" or key == "9" then
        local w = renderSystem.adjustCurveOverlayThickness(-0.1)
        logInfo(string.format("[DNA][VIEW] Curve thickness: %.2f", w))
        return true
    elseif key == "]" or key == "0" then
        local w = renderSystem.adjustCurveOverlayThickness(0.1)
        logInfo(string.format("[DNA][VIEW] Curve thickness: %.2f", w))
        return true
    elseif key == "f" then
        self:_toggleForceDnaMode()
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

function PlayScene:textinput(text)
    -- IME 상태에서도 강제 모드 토글 가능하도록 fallback 처리
    if text == "f" or text == "F" or text == "ㄹ" then
        return self:_toggleForceDnaMode()
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
