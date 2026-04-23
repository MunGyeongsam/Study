-- Vector Swarm - Zero Art Roguelite
-- Main game file ??Scene Stack 기반 게임 루프

-- Global utilities first (?�역 ?�수 최우??초기??
local global = require("00_common.global")

-- Load modules
local logger = require("00_common.logger")
local debugOverlay = require("00_common.debug")
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
local sceneStack = nil   -- 글로벌 ???�택
local showWorldGrid = false
local FORCE_DNA_TOGGLE_DEBOUNCE = 0.08
local _forceDnaLastToggleTime = -1

local function _tryToggleForceDnaMode()
    local now = love.timer.getTime()
    if now - _forceDnaLastToggleTime < FORCE_DNA_TOGGLE_DEBOUNCE then
        return true
    end

    local PlayScene = require("03_game.scenes.playScene")
    if PlayScene.toggleForceDnaModeGlobal() ~= nil then
        _forceDnaLastToggleTime = now
        return true
    end

    return false
end

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

    -- 카메??매니?� 초기??
    local orthographicSize = 5
    cameraManager.init(orthographicSize)

    -- Screen shake 글로벌 ?�록
    screenShake = cameraManager.shake

    -- ?�운???�생 글로벌 ?�록
    playSound = function(name) soundManager.play(name) end
    playBGM   = function(name) soundManager.playBGM(name) end
    stopBGM   = function() soundManager.stopBGM() end

    -- Bloom ?�스?�프로세??초기??
    bloom.init()

    -- ?�운??매니?� 초기??
    soundManager.init()
    
    -- Initialize core systems
    world.init()

    -- Save data 로드
    saveData.load()
    
    -- ECS ?�스??초기??
    ecsManager.init(function() return player.getPosition() end)
    
    -- 초기 ?�레?�어 (debug watcher??
    local playerId = ecsManager.createPlayer(0, -12)
    player.bind(ecsManager.getWorld(), playerId)
    player.init(0, -12)

    -- Debug watchers
    debugOverlay.add("world info", function()
        local size = world.size
        return string.format("%10s : %d x %d", "World Size", size.width, size.height) 
    end)
    debugOverlay.add("player info", function()
        local x, y = player.getPosition()
        return string.format("%10s : (%.1f, %.1f)", "Player Pos", x, y) 
    end)
    debugOverlay.add("ECS stats", function()
        local stats = ecsManager.getStats()
        return string.format("%10s : %d entities", "ECS Total", stats.world.activeEntities) 
    end)
    debugOverlay.add("ECS components", function()
        local stats = ecsManager.getStats()
        local componentCounts = stats.world.componentTypes or {}
        local text = "Components: "
        for name, count in pairs(componentCounts) do
            text = text .. string.format("%s(%d) ", name, count)
        end
        return text
    end)
    debugOverlay.add("camera mode", function()
        local cam = cameraManager.getActive()
        local cx, cy = cam:pos()
        return string.format("%10s : %s (%.1f, %.1f) zoom=%.1f",
            "Camera", cameraManager.getMode(), cx, cy, cam:getOrthographicSize())
    end)
    debugOverlay.add("bullets", function()
        local stats = ecsManager.getStats()
        local b = stats.bullets
        return string.format("%10s : %d active / %d peak / %d spawned",
            "Bullets", b.active, b.peakActive, b.spawned)
    end)
    debugOverlay.add("player HP", function()
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
    debugOverlay.add("waves", function()
        local stats = ecsManager.getStats()
        local s = stats.stage
        return string.format("%10s : Stage %d Wave %d/%d [%s] (%d enemies)",
            "Stage", s.stage, s.wave, s.wavesPerStage, s.state, s.enemies)
    end)
    debugOverlay.add("dash/focus", function()
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
    debugOverlay.add("xp/level", function()
        local w = ecsManager.getWorld()
        local entities = w:queryEntities({"PlayerTag", "PlayerXP"})
        if #entities > 0 then
            local xp = w:getComponent(entities[1], "PlayerXP")
            return string.format("%10s : Lv.%d XP:%d/%d Mag:%.1f",
                "XP", xp.level, xp.xp, xp.xpToNext, xp.magnetRange)
        end
        return "XP : N/A"
    end)
    debugOverlay.add("debug keys", function()
        local PlayScene = require("03_game.scenes.playScene")
        local sm = ecsManager.getStageManager and ecsManager.getStageManager()
        local dna = sm and sm:isForceDnaSpawn() and "ON" or "off"
        return string.format("%10s : GOD[F7]:%s+WPN:%s BLOOM[F6]:%s BG[F9]:%s DNA[F]:%s", "debug",
            PlayScene.getGodMode() and "ON" or "off",
            PlayScene.getDisableWeapon() and "OFF" or "on",
            bloom.isEnabled() and "ON" or "off",
            background.isEnabled() and "ON" or "off",
            dna)
    end)
    debugOverlay.add("background", function()
        return string.format("%10s : c=%.2f %s %d circles [%s]",
            "BG", background.getC(), background.getStyle(), background.getCount(),
            background.isGenerating() and "generating..." or "done")
    end)
    debugOverlay.add("scene stack", function()
        local top = sceneStack:top()
        return string.format("%10s : depth=%d top=%s", "Scenes", sceneStack:size(), top and top.name or "none")
    end)

    debugOverlay.toggleConsole()

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

    -- Scene Stack 초기????TitleScene?�서 ?�작
    sceneStack = SceneStack.new()
    local TitleScene = require("03_game.scenes.titleScene")
    sceneStack:push(TitleScene.new(sceneStack))
end

function love.update(dt)
    sceneStack:update(dt)
end

function love.draw()
    -- ???�택 ?�더�?(?�래 ????
    sceneStack:draw()

    -- ?�버�?UI (???��?, ??�� 최상??
    debugOverlay.draw(10, 50, fonts.small)
    logger.drawConsole(fonts.small)
    screenDebugDraw.draw(50)
end

function love.keypressed(key)
    -- ?�버�??�는 모든 ?�태?�서 ?�동
    if key == "f1" then
        debugOverlay.toggleConsole()
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
    elseif key == "f" then
        if _tryToggleForceDnaMode() then
            return
        end
    end

    -- ?�머지???�택 최상???�에 ?�임
    sceneStack:keypressed(key)
end

-- macOS IME(한글 등)에서 알파벳 키가 keypressed를 우회하는 문제 대응
function love.textinput(text)
    if text == "f" or text == "F" or text == "ㄹ" then
        if _tryToggleForceDnaMode() then
            return
        end
    end
    sceneStack:textinput(text)
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

-- PC: 마우?????�치 브릿지
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

-- 마우???? ?�버�?카메??�?
function love.wheelmoved(x, y)
    cameraManager.wheelmoved(x, y)
end

-- Close log file when game quits
function love.quit()
    logger.close()
end

-- ===== Error Handler: crash callstack -> debug.log =====
-- Backup Lua standard debug module (00_common.debug shadows it)
local _traceback = debug and debug.traceback or function(m) return tostring(m) end
local _defaultErrorHandler = love.errorhandler or love.errhand

function love.errorhandler(msg)
    local trace = _traceback(tostring(msg), 2)
    local crashMsg = "\n========== CRASH ==========\n" .. trace .. "\n============================\n"

    -- stderr + stdout
    pcall(function()
        io.stderr:write(crashMsg)
        io.stderr:flush()
    end)
    print(crashMsg)

    -- Write to debug.log if logger is alive
    pcall(function()
        logger.error("CRASH: " .. tostring(msg))
        logger.error(_traceback("", 2))
        logger.close()
    end)

    -- Fall back to LOVE default error screen (blue screen)
    if _defaultErrorHandler then
        return _defaultErrorHandler(msg)
    end
end

-- Screen resize callback
function love.resize(w, h)
    logger.info(string.format("Screen resized to %dx%d", w, h))
    uiManager.updateLayout()
    bloom.resize(w, h)
    log(string.format("New pixels per unit: %.1f", cameraManager.getActive():getPixelsPerUnit()))
end