-- Vector Swarm - Zero Art Roguelite
-- Main game file

-- Global utilities first (전역 함수 최우선 초기화)
local global = require("00_common.global")

local _floor = math.floor

-- Load modules
local logger = require("00_common.logger")
local debug = require("00_common.debug")
local screenDebugDraw = require("00_common.gridDebugDraw")
local world = require("01_core.world")
local cameraManager = require("02_renderer.cameraManager")
local bloom = require("02_renderer.bloom")
local background = require("02_renderer.background")
local uiManager = require("04_ui.uiManager")  -- UI 시스템 추가
local player = require("03_game.entities.player")  -- 플레이어 엔티티
local ecsManager = require("03_game.ecsManager")  -- ECS 오케스트레이터
local gameState = require("03_game.states.gameState")
local levelUp = require("03_game.states.levelUp")
local soundManager = require("05_sound.soundManager")
local saveData = require("00_common.saveData")

local fonts = nil       -- 폰트 테이블 (love.load에서 초기화)
local restartGame       -- forward declaration (콜백에서 참조)
local hitStopTimer = 0  -- hit-stop freeze (boss defeat)
local godMode = false   -- debug: player invincibility (F7)

function love.load()
    -- Initialize global utilities first
    global.init()
    
    -- Initialize logging second
    logger.init()

    -- Set up multiple font sizes
    fonts = {
        small = love.graphics.newFont(12),    -- 작은 폰트 (디버그용)
        medium = love.graphics.newFont(16),   -- 중간 폰트 (기본)
        large = love.graphics.newFont(20),    -- 큰 폰트 (제목용)
        xlarge = love.graphics.newFont(24)    -- 매우 큰 폰트 (특별용)
    }
    love.graphics.setFont(fonts.medium)  -- 기본 폰트를 중간 크기로 설정

    -- 카메라 매니저 초기화
    local orthographicSize = 5
    cameraManager.init(orthographicSize)

    -- Screen shake 글로벌 등록
    screenShake = cameraManager.shake

    -- 사운드 재생 글로벌 등록 (시스템들에서 직접 호출용)
    playSound = function(name) soundManager.play(name) end
    playBGM   = function(name) soundManager.playBGM(name) end
    stopBGM   = function() soundManager.stopBGM() end

    -- Bloom 포스트프로세싱 초기화
    bloom.init()

    -- 사운드 매니저 초기화 (SFX 사전 생성)
    soundManager.init()
    
    -- Initialize core systems
    world.init()

    -- Save data 로드 (영구 저장)
    saveData.load()
    
    -- ECS 시스템 초기화 (플레이어 위치 조회 콜백 전달)
    ecsManager.init(function() return player.getPosition() end)
    
    -- 플레이어 ECS 엔티티 생성 + 파사드 바인딩 (하단 시작)
    local playerId = ecsManager.createPlayer(0, -12)
    player.bind(ecsManager.getWorld(), playerId)
    player.init(0, -12)
    
    local startX, startY = player.getPosition()

    debug.add("world info", 
    function()
        local size = world.size
        return string.format("%10s : %d x %d", "World Size", size.width, size.height) 
    end);

    debug.add("player info", 
    function()
        local x, y = player.getPosition()
        return string.format("%10s : (%.1f, %.1f)", "Player Pos", x, y) 
    end);
    
    debug.add("ECS stats", 
    function()
        local stats = ecsManager.getStats()
        return string.format("%10s : %d entities", "ECS Total", stats.world.activeEntities) 
    end);
    
    debug.add("ECS components", 
    function()
        local stats = ecsManager.getStats()
        local componentCounts = stats.world.componentTypes or {}
        local text = "Components: "
        for name, count in pairs(componentCounts) do
            text = text .. string.format("%s(%d) ", name, count)
        end
        return text
    end);
    
    debug.add("camera mode",
    function()
        local cam = cameraManager.getActive()
        local cx, cy = cam:pos()
        return string.format("%10s : %s (%.1f, %.1f) zoom=%.1f",
            "Camera", cameraManager.getMode(), cx, cy, cam:getOrthographicSize())
    end);

    debug.add("bullets",
    function()
        local stats = ecsManager.getStats()
        local b = stats.bullets
        return string.format("%10s : %d active / %d peak / %d spawned",
            "Bullets", b.active, b.peakActive, b.spawned)
    end);

    debug.add("player HP",
    function()
        local w = ecsManager.getWorld()
        local entities = w:queryEntities({"PlayerTag", "Health"})
        if #entities > 0 then
            local h = w:getComponent(entities[1], "Health")
            local status = h.alive and "ALIVE" or "DEAD"
            local inv = h.iTimer > 0 and string.format(" [INV %.1f]", h.iTimer) or ""
            return string.format("%10s : %d/%d %s%s (hits: %d)",
                "Player HP", h.hp, h.maxHp, status, inv, h.hitCount)
        end
        return "Player HP : N/A"
    end);

    debug.add("waves",
    function()
        local stats = ecsManager.getStats()
        local s = stats.stage
        return string.format("%10s : Stage %d Wave %d/%d [%s] (%d enemies)",
            "Stage", s.stage, s.wave, s.wavesPerStage, s.state, s.enemies)
    end);

    debug.add("dash/focus",
    function()
        local w = ecsManager.getWorld()
        local entities = w:queryEntities({"PlayerTag", "Dash", "Focus"})
        if #entities > 0 then
            local dash = w:getComponent(entities[1], "Dash")
            local focus = w:getComponent(entities[1], "Focus")
            local dcd = dash.cooldownTimer > 0 and string.format("CD:%.1f", dash.cooldownTimer) or "READY"
            local fActive = focus.active and "ON" or "off"
            return string.format("%10s : Dash[%s] Focus[%s E:%.0f%%] TS:%.1fx",
                "Dash/Focus", dcd, fActive, focus.energy, gameState.getTimeScale())
        end
        return "Dash/Focus : N/A"
    end);

    debug.add("xp/level",
    function()
        local w = ecsManager.getWorld()
        local entities = w:queryEntities({"PlayerTag", "PlayerXP"})
        if #entities > 0 then
            local xp = w:getComponent(entities[1], "PlayerXP")
            return string.format("%10s : Lv.%d XP:%d/%d Mag:%.1f",
                "XP", xp.level, xp.xp, xp.xpToNext, xp.magnetRange)
        end
        return "XP : N/A"
    end);

    debug.add("debug keys", function()
        return string.format("%10s : GOD[F7]:%s BLOOM[F6]:%s BG[F9]:%s", "debug",
            godMode and "ON" or "off",
            bloom.isEnabled() and "ON" or "off",
            background.isEnabled() and "ON" or "off")
    end);

    debug.add("background", function()
        return string.format("%10s : c=%.2f %s %d circles [%s]",
            "BG", background.getC(), background.getStyle(), background.getCount(),
            background.isGenerating() and "generating..." or "done")
    end);

    debug.toggleConsole()   -- debug watch panel auto-show (dev)
    
    -- 🌍 카메라를 플레이어 시작 위치로 이동
    cameraManager.getGameCamera():lookAt(startX, startY)
    logger.info(string.format("[CAM] Positioned at player start: (%.1f, %.1f)", startX, startY))
    
    -- Initialize UI system
    uiManager.init()
    
    -- UI 버튼 콜백 설정
    uiManager.setButtonCallbacks({
        onZoomIn = function()
            local cam = cameraManager.getActive()
            local newSize = cam:getOrthographicSize() * 0.8
            cam:setOrthographicSize(newSize)
            log(string.format("Zoom In - Orthographic Size: %.2f", newSize))
        end,
        onZoomOut = function()
            local cam = cameraManager.getActive()
            local newSize = cam:getOrthographicSize() * 1.25
            cam:setOrthographicSize(newSize)
            log(string.format("Zoom Out - Orthographic Size: %.2f", newSize))
        end,
        onReset = function()
            cameraManager.getActive():setOrthographicSize(orthographicSize)
            log(string.format("Zoom Reset - Orthographic Size: %.2f", orthographicSize))
        end,
        onDebugToggle = function()
            uiManager.toggleDebugMode()
        end
    })
    
    log("All systems initialized successfully")
    log(string.format("Camera orthographic size: %.1f (viewing %.1f world units height)", 
         orthographicSize, orthographicSize * 2))
    log(string.format("Pixels per unit: %.1f", cameraManager.getActive():getPixelsPerUnit()))
    
    -- 배경 초기화 (스테이지 1)
    background.init(1)

    -- 게임 상태 초기화
    gameState.init()
    
end

-- 게임 리스타트
restartGame = function()
    -- Fragment 저장 (사망 시 영구 보존)
    local runFragments = gameState.getFragments()
    if runFragments > 0 then
        saveData.addFragments(runFragments)
    end
    local score = gameState.getScore()
    local stageInfo = gameState.getStageInfo()
    saveData.recordRun(score, stageInfo)
    saveData.save()

    -- ECS 월드 초기화 (엔티티 + 불릿 + 스포너)
    ecsManager.restart()

    -- 플레이어 재생성 + 바인딩 (하단 시작)
    local playerId = ecsManager.createPlayer(0, -12)
    player.bind(ecsManager.getWorld(), playerId)
    player.init(0, -12)

    -- 카메라 리셋
    local px, py = player.getPosition()
    cameraManager.getGameCamera():lookAt(px, py)
    cameraManager.getGameCamera():setOrthographicSize(5)

    -- 배경 재생성
    background.init(1)

    -- 게임 상태 리셋
    gameState.init()
    levelUp.reset()  -- 감쇠 스택 초기화

    logInfo("[GAME] Restarted!")
end

function love.update(dt)
    -- Hit-stop freeze (boss defeat moment)
    if hitStopTimer > 0 then
        hitStopTimer = hitStopTimer - dt
        return
    end

    -- 레벨업 선택 중이면 게임 로직 정지
    if levelUp.isActive() then
        return
    end

    -- 스테이지 클리어 중이면 제한된 시스템만 업데이트
    if ecsManager.stageManager:isClearing() then
        -- Detect boss defeat → apply rewards once
        local stageState = ecsManager.stageManager.state
        if stageState == "boss_clear" and not ecsManager.stageManager.bossRewardsApplied then
            ecsManager.stageManager.bossRewardsApplied = true
            -- Hit-stop + screen shake
            hitStopTimer = 0.2
            screenShake(0.35, 0.5)
            -- Recover player: HP full, dash/focus reset
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

        -- Collecting phase: XP orbs need movement + collection systems
        if stageState == "collecting" then
            -- Run movement + XP collection so orbs fly to player
            local moveSys = ecsManager.systems["Movement"]
            if moveSys then moveSys:update(ecsManager.getWorld(), dt) end
            local xpSys = ecsManager.systems["XpCollection"]
            if xpSys then xpSys:update(ecsManager.getWorld(), dt) end

            -- Check for pending level-ups during collection
            local w = ecsManager.getWorld()
            local xpEntities = w:queryEntities({"PlayerTag", "PlayerXP"})
            if #xpEntities > 0 then
                local playerXP = w:getComponent(xpEntities[1], "PlayerXP")
                if playerXP.pendingLevelUp then
                    playerXP.pendingLevelUp = false
                    levelUp.show(w, xpEntities[1])
                end
            end
        end

        ecsManager.stageManager:update(dt)
        return
    end

    -- 게임 오버 상태로 게임 로직 정지
    if gameState.isGameOver() then
        gameState.update(dt, nil)
        return
    end

    -- ECS 시스템 업데이트 (포커스 슬로모 적용)
    local scaledDt = dt * gameState.getTimeScale()
    ecsManager.update(scaledDt)

    -- God mode: keep player invincible
    if godMode then
        local playerId = player.getEntityId()
        if playerId then
            local health = ecsManager.getWorld():getComponent(playerId, "Health")
            if health then health.iTimer = 9999 end
        end
    end
    
    -- 🏃‍♂️ 플레이어 업데이트 (입력 처리 및 이동, 기존 OOP)
    player.update(dt, {})  -- 나중에 터치 입력 시스템 연결
    
    -- 📹 카메라 업데이트 (게임 모드면 플레이어 추적, 디버그 모드면 자유)
    local playerX, playerY = player.getCameraTarget()
    cameraManager.update(dt, playerX, playerY)
    
    -- 배경 점진 생성
    background.update(dt)

    -- UI 업데이트
    uiManager.update(dt)
    
    -- 플레이어와 월드 데이터를 UI에 전달
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
        -- Boss HUD data
        bossActive = stageStats.bossEntityId ~= nil and (stageStats.state == "boss_intro" or stageStats.state == "boss_active"),
        bossName = stageStats.bossType,
        bossHp = 0,
        bossMaxHp = 1,
        bossPhase = 1,
        bossMaxPhase = 1,
    })

    -- Update boss HP bar from ECS if boss is active
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

    -- 게임 상태 업데이트 (HP 검사 → 게임오버 판정)
    local w = ecsManager.getWorld()
    local playerEntities = w:queryEntities({"PlayerTag", "Health"})
    local playerHealth = #playerEntities > 0 and w:getComponent(playerEntities[1], "Health") or nil
    -- 스테이지 정보를 gameState에 전달
    gameState.setStageInfo(stageStats.stage, stageStats.wave, stageStats.wavesPerStage)
    gameState.setWaveReached(stageStats.totalWaves)
    gameState.update(dt, playerHealth)

    -- 레벨업 체크
    local xpEntities = w:queryEntities({"PlayerTag", "PlayerXP"})
    if #xpEntities > 0 then
        local playerXP = w:getComponent(xpEntities[1], "PlayerXP")
        if playerXP.pendingLevelUp then
            playerXP.pendingLevelUp = false
            levelUp.show(w, xpEntities[1])
        end
    end
end

local showWorldGrid = false  -- 월드 그리드 표시 (F4 토글)

local function drawWorld()
    -- 배경 렌더링 (카메라 기반 컬링)
    local cam = cameraManager.getActive()
    background.draw(cam)

    -- 월드 좌표계 그리드 (디버그, F4 토글)
    if showWorldGrid then
        world.drawGrid(1, cam)
    end
    
    -- ECS 엔티티 렌더링 (플레이어 포함)
    ecsManager.draw()
end

function love.draw()
    -- Bloom: 씬 캡처 시작
    bloom.beginCapture()

    -- 월드 렌더링 (활성 카메라 적용)
    cameraManager.draw(drawWorld)
    
    -- Bloom: 씬 캡처 종료 + 합성 출력
    bloom.endCapture()
    bloom.draw()

    -- UI 렌더링 (스크린 좌표계) 
    uiManager.draw()
    
    -- 디버그 정보 그리기 (기존 시스템)
    debug.draw(10, 50, fonts.small)
    logger.drawConsole(fonts.small)  -- 인게임 디버그 콘솔

    -- 스크린 좌표계 그리드 (F4 토글)
    screenDebugDraw.draw(50)

    -- 게임 오버 오버레이 (스크린 좌표계, 최상위)
    gameState.draw()

    -- 스테이지 클리어 오버레이
    ecsManager.stageManager:draw()

    -- 레벨업 선택 UI (최상위)
    levelUp.draw()
end

-- Debug console functions are now handled automatically by Logger

function love.keypressed(key)
    -- 레벨업 선택 우선 처리
    if levelUp.keypressed(key) then return end

    if key == "f1" then
        debug.toggleConsole()   -- F1: 디버그 watch panel 토글
    elseif key == "`" then
        logger.toggleConsole()  -- ` (백틱): 로거 콘솔 토글
    elseif key == "f2" then
        uiManager.toggleVisibility()  -- F2키로 UI 토글
    elseif key == "f3" then
        uiManager.toggleDebugMode()  -- F3키로 UI 디버그 모드 토글
    elseif key == "f4" then
        showWorldGrid = not showWorldGrid
        screenDebugDraw.toggle()    -- F4: 그리드 토글 (월드+스크린)
    elseif key == "f5" then
        cameraManager.toggle()      -- F5: 게임/디버그 카메라 전환
    elseif key == "f6" then
        bloom.toggle()              -- F6: Bloom 토글
    elseif key == "f9" then
        background.toggle()         -- F9: 배경 토글
    elseif key == "f10" then
        background.cycleStyle()     -- F10: 배경 스타일 순환
    elseif key == "f11" then
        background.adjustC(-0.05)   -- F11: c값 감소
    elseif key == "f12" then
        background.adjustC(0.05)    -- F12: c값 증가
    elseif key == "f7" then
        godMode = not godMode       -- F7: 무적 모드 토글
        logInfo(string.format("[DEBUG] God mode: %s", godMode and "ON" or "OFF"))
    elseif key == "f8" then
        -- F8: 스테이지 스킵 (디버그용)
        local sm = ecsManager.getStageManager and ecsManager.getStageManager()
        if sm then sm:debugSkipStage() end
    elseif key == "r" then
        if gameState.canRestart() then
            restartGame()
        end
    elseif key == "lshift" or key == "rshift" then
        -- 대쉬 요청 → Input 컴포넌트로 전달
        if gameState.isPlaying() then
            local playerId = player.getEntityId()
            if playerId then
                local inp = ecsManager.getWorld():getComponent(playerId, "Input")
                if inp then
                    inp.dash = true
                end
            end
        end
    end
    if key == 'escape' then
        love.event.quit()  -- ESC키로 게임 종료
    end
    
    -- 디버그 카메라 모드: +/- 키로 줌 조절
    if cameraManager.isDebug() then
        if key == "=" or key == "+" then
            local cam = cameraManager.getActive()
            local newSize = cam:getOrthographicSize() * 0.8
            cam:setOrthographicSize(newSize)
        elseif key == "-" then
            local cam = cameraManager.getActive()
            local newSize = cam:getOrthographicSize() * 1.25
            cam:setOrthographicSize(newSize)
        elseif key == "space" then
            -- 플레이어 위치로 복귀
            local px, py = player.getPosition()
            cameraManager.getActive():lookAt(px, py)
        end
    end
end

-- 모바일 터치 입력 처리
function love.touchpressed(id, x, y, dx, dy, pressure)
    -- 레벨업 선택 우선 처리
    if levelUp.touchpressed(x, y) then return end

    -- 게임 오버 시 터치로 리스타트
    if gameState.canRestart() then
        restartGame()
        return
    end

    -- UI 터치 처리
    if uiManager.touchpressed(id, x, y, dx, dy, pressure) then
        return
    end
    
    -- 게임 플레이 영역 터치 처리
    local mobileLayout = require("04_ui.mobileLayout")
    if mobileLayout.isTouchInArea(x, y, "play") then
        local worldX, worldY = cameraManager.worldCoords(x, y)
        log(string.format("Touch at world: (%.1f, %.1f)", worldX, worldY))
    end
end

function love.touchmoved(id, x, y, dx, dy, pressure)
    -- UI 터치 이동 처리
    if uiManager.touchmoved(id, x, y, dx, dy, pressure) then
        return
    end
    
    -- 디버그 모드: 터치 드래그로 카메라 이동
    if cameraManager.isDebug() then
        cameraManager.debugMove(dx, dy)
    end
end

function love.touchreleased(id, x, y, dx, dy, pressure)
    uiManager.touchreleased(id, x, y, dx, dy, pressure)
end

-- PC: 마우스 입력
function love.mousepressed(x, y, button, istouch, presses)
    if not istouch then
        love.touchpressed("mouse", x, y, 0, 0, 1)
    end
end

function love.mousemoved(x, y, dx, dy, istouch)
    if not istouch and love.mouse.isDown(1) then
        -- 디버그 모드: 마우스 드래그로 카메라 자유 이동
        if cameraManager.isDebug() then
            cameraManager.debugMove(dx, dy)
        else
            love.touchmoved("mouse", x, y, dx, dy, 1)
        end
    end
end

function love.mousereleased(x, y, button, istouch, presses)
    if not istouch then
        love.touchreleased("mouse", x, y, 0, 0, 1)
    end
end

-- 마우스 휠: 디버그 카메라 줌
function love.wheelmoved(x, y)
    cameraManager.wheelmoved(x, y)
end

-- Close log file when game quits
function love.quit()
    logger.close()
end

-- 화면 리사이즈 콜백
function love.resize(w, h)
    logger.info(string.format("Screen resized to %dx%d", w, h))
    uiManager.updateLayout()
    bloom.resize(w, h)
    log(string.format("New pixels per unit: %.1f", cameraManager.getActive():getPixelsPerUnit()))
end