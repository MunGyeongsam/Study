-- Vector Swarm - Zero Art Roguelite
-- Main game file — Scene Stack 기반 게임 루프

-- Global utilities first (전역 함수 최우선 초기화)
local global = require("00_common.global")

-- Load modules
local logger = require("00_common.logger")
local debug = require("00_common.debug")
local screenDebugDraw = require("00_common.gridDebugDraw")
local world = require("01_core.world")
local SceneStack = require("01_core.sceneStack")
local cameraManager = require("02_renderer.cameraManager")
local bloom = require("02_renderer.bloom")
local background = require("02_renderer.background")
local uiManager = require("04_ui.uiManager")
local player = require("03_game.entities.player")
local ecsManager = require("03_game.ecsManager")
local gameState = require("03_game.states.gameState")
local soundManager = require("05_sound.soundManager")
local saveData = require("00_common.saveData")

local fonts = nil
local sceneStack = nil   -- 글로벌 씬 스택
local showWorldGrid = false

function love.load()
    -- Initialize global utilities first
    global.init()
    
    -- Initialize logging second
    logger.init()

    -- Set up multiple font sizes
    fonts = {
        small = love.graphics.newFont(12),
        medium = love.graphics.newFont(16),
        large = love.graphics.newFont(20),
        xlarge = love.graphics.newFont(24)
    }
    love.graphics.setFont(fonts.medium)

    -- 카메라 매니저 초기화
    local orthographicSize = 5
    cameraManager.init(orthographicSize)

    -- Screen shake 글로벌 등록
    screenShake = cameraManager.shake

    -- 사운드 재생 글로벌 등록
    playSound = function(name) soundManager.play(name) end
    playBGM   = function(name) soundManager.playBGM(name) end
    stopBGM   = function() soundManager.stopBGM() end

    -- Bloom 포스트프로세싱 초기화
    bloom.init()

    -- 사운드 매니저 초기화
    soundManager.init()
    
    -- Initialize core systems
    world.init()

    -- Save data 로드
    saveData.load()
    
    -- ECS 시스템 초기화
    ecsManager.init(function() return player.getPosition() end)
    
    -- 초기 플레이어 (debug watcher용)
    local playerId = ecsManager.createPlayer(0, -12)
    player.bind(ecsManager.getWorld(), playerId)
    player.init(0, -12)

    -- Debug watchers
    debug.add("world info", function()
        local size = world.size
        return string.format("%10s : %d x %d", "World Size", size.width, size.height) 
    end)
    debug.add("player info", function()
        local x, y = player.getPosition()
        return string.format("%10s : (%.1f, %.1f)", "Player Pos", x, y) 
    end)
    debug.add("ECS stats", function()
        local stats = ecsManager.getStats()
        return string.format("%10s : %d entities", "ECS Total", stats.world.activeEntities) 
    end)
    debug.add("ECS components", function()
        local stats = ecsManager.getStats()
        local componentCounts = stats.world.componentTypes or {}
        local text = "Components: "
        for name, count in pairs(componentCounts) do
            text = text .. string.format("%s(%d) ", name, count)
        end
        return text
    end)
    debug.add("camera mode", function()
        local cam = cameraManager.getActive()
        local cx, cy = cam:pos()
        return string.format("%10s : %s (%.1f, %.1f) zoom=%.1f",
            "Camera", cameraManager.getMode(), cx, cy, cam:getOrthographicSize())
    end)
    debug.add("bullets", function()
        local stats = ecsManager.getStats()
        local b = stats.bullets
        return string.format("%10s : %d active / %d peak / %d spawned",
            "Bullets", b.active, b.peakActive, b.spawned)
    end)
    debug.add("player HP", function()
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
    end)
    debug.add("waves", function()
        local stats = ecsManager.getStats()
        local s = stats.stage
        return string.format("%10s : Stage %d Wave %d/%d [%s] (%d enemies)",
            "Stage", s.stage, s.wave, s.wavesPerStage, s.state, s.enemies)
    end)
    debug.add("dash/focus", function()
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
    end)
    debug.add("xp/level", function()
        local w = ecsManager.getWorld()
        local entities = w:queryEntities({"PlayerTag", "PlayerXP"})
        if #entities > 0 then
            local xp = w:getComponent(entities[1], "PlayerXP")
            return string.format("%10s : Lv.%d XP:%d/%d Mag:%.1f",
                "XP", xp.level, xp.xp, xp.xpToNext, xp.magnetRange)
        end
        return "XP : N/A"
    end)
    debug.add("debug keys", function()
        local PlayScene = require("03_game.scenes.playScene")
        return string.format("%10s : GOD[F7]:%s BLOOM[F6]:%s BG[F9]:%s", "debug",
            PlayScene.getGodMode() and "ON" or "off",
            bloom.isEnabled() and "ON" or "off",
            background.isEnabled() and "ON" or "off")
    end)
    debug.add("background", function()
        return string.format("%10s : c=%.2f %s %d circles [%s]",
            "BG", background.getC(), background.getStyle(), background.getCount(),
            background.isGenerating() and "generating..." or "done")
    end)
    debug.add("scene stack", function()
        local top = sceneStack:top()
        return string.format("%10s : depth=%d top=%s", "Scenes", sceneStack:size(), top and top.name or "none")
    end)

    debug.toggleConsole()

    cameraManager.getGameCamera():lookAt(0, -12)
    logger.info("[CAM] Positioned at player start: (0.0, -12.0)")
    
    -- Initialize UI system
    uiManager.init()
    uiManager.setButtonCallbacks({
        onZoomIn = function()
            local cam = cameraManager.getActive()
            cam:setOrthographicSize(cam:getOrthographicSize() * 0.8)
        end,
        onZoomOut = function()
            local cam = cameraManager.getActive()
            cam:setOrthographicSize(cam:getOrthographicSize() * 1.25)
        end,
        onReset = function()
            cameraManager.getActive():setOrthographicSize(orthographicSize)
        end,
        onDebugToggle = function()
            uiManager.toggleDebugMode()
        end
    })
    
    log("All systems initialized successfully")
    log(string.format("Camera orthographic size: %.1f (viewing %.1f world units height)", 
         orthographicSize, orthographicSize * 2))
    log(string.format("Pixels per unit: %.1f", cameraManager.getActive():getPixelsPerUnit()))
    
    background.init(1)

    -- Scene Stack 초기화 → TitleScene에서 시작
    sceneStack = SceneStack.new()
    local TitleScene = require("03_game.scenes.titleScene")
    sceneStack:push(TitleScene.new(sceneStack))
end

function love.update(dt)
    sceneStack:update(dt)
end

function love.draw()
    -- 씬 스택 렌더링 (아래 → 위)
    sceneStack:draw()

    -- 디버그 UI (씬 외부, 항상 최상위)
    debug.draw(10, 50, fonts.small)
    logger.drawConsole(fonts.small)
    screenDebugDraw.draw(50)
end

function love.keypressed(key)
    -- 디버그 키는 모든 상태에서 작동
    if key == "f1" then
        debug.toggleConsole()
        return
    elseif key == "`" then
        logger.toggleConsole()
        return
    elseif key == "f2" then
        uiManager.toggleVisibility()
        return
    elseif key == "f3" then
        uiManager.toggleDebugMode()
        return
    elseif key == "f4" then
        showWorldGrid = not showWorldGrid
        local PlayScene = require("03_game.scenes.playScene")
        PlayScene.setShowWorldGrid(showWorldGrid)
        screenDebugDraw.toggle()
        return
    end

    -- 나머지는 스택 최상위 씬에 위임
    sceneStack:keypressed(key)
end

function love.touchpressed(id, x, y, dx, dy, pressure)
    sceneStack:touchpressed(id, x, y, dx, dy, pressure)
end

function love.touchmoved(id, x, y, dx, dy, pressure)
    sceneStack:touchmoved(id, x, y, dx, dy, pressure)
end

function love.touchreleased(id, x, y, dx, dy, pressure)
    sceneStack:touchreleased(id, x, y, dx, dy, pressure)
end

-- PC: 마우스 → 터치 브릿지
function love.mousepressed(x, y, button, istouch, presses)
    if not istouch then
        love.touchpressed("mouse", x, y, 0, 0, 1)
    end
end

function love.mousemoved(x, y, dx, dy, istouch)
    if not istouch and love.mouse.isDown(1) then
        love.touchmoved("mouse", x, y, dx, dy, 1)
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