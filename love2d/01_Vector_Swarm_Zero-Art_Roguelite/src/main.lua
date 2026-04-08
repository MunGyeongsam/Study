-- Vector Swarm - Zero Art Roguelite
-- Main game file

-- Load modules
local Logger = require("lib.logger")
local Debug = require("lib.debug")

function love.load()
    -- Initialize logging first
    Logger.init()
    
    -- Called once at the beginning of the game
    Logger.info("Game started - hello vector_swarm")
    
    -- Debug: 실제 창 크기 확인
    local width, height = love.graphics.getDimensions()
    Logger.info("Window size: " .. width .. " x " .. height)
    Logger.addWindowInfo(width, height)  -- Add to console
    
    -- Set up multiple font sizes
    fonts = {
        small = love.graphics.newFont(12),    -- 작은 폰트 (디버그용)
        medium = love.graphics.newFont(16),   -- 중간 폰트 (기본)
        large = love.graphics.newFont(20),    -- 큰 폰트 (제목용)
        xlarge = love.graphics.newFont(24)    -- 매우 큰 폰트 (특별용)
    }
    love.graphics.setFont(fonts.medium)  -- 기본 폰트를 중간 크기로 설정
    
    -- Game variables
    message = "hello vector_swarm"
    windowInfo = "Window: " .. width .. "x" .. height
    gameTime = 0                    -- 게임 시간
    frameCount = 0                  -- 프레임 카운터
    playerPosition = {x = width/2, y = height/2}  -- 플레이어 위치
    gameSpeed = 1.0                 -- 게임 속도
    
    -- Debug console variables
    debugConsoleVisible = false
    
    -- Initialize debug system
    Debug.init(fonts.small)
    
    -- Register debug information
    Debug.addGameVariable("Game Time", function() return gameTime end, "%.2f")
    Debug.addGameVariable("Frame Count", function() return frameCount end, "%.0f")
    Debug.addGameVariable("Game Speed", function() return gameSpeed end, "%.1f")
    Debug.addPosition("Player", function() return playerPosition end)
end

function love.update(dt)
    -- Called every frame
    -- dt is the time since the last frame in seconds
    
    gameTime = gameTime + (dt * gameSpeed)  -- 게임 시간 업데이트
    frameCount = frameCount + 1
    
    -- Update window info in real time
    local width, height = love.graphics.getDimensions()
    windowInfo = "Window: " .. width .. "x" .. height .. " | FPS: " .. love.timer.getFPS()
    
    -- 디버그 모드일 때 플레이어 위치 업데이트 (간단한 움직임)
    if Debug.isEnabled() then
        playerPosition.x = playerPosition.x + math.sin(gameTime) * 50 * dt
        playerPosition.y = playerPosition.y + math.cos(gameTime) * 30 * dt
        
        -- 화면 경계 체크
        if playerPosition.x < 0 then playerPosition.x = width end
        if playerPosition.x > width then playerPosition.x = 0 end
        if playerPosition.y < 0 then playerPosition.y = height end
        if playerPosition.y > height then playerPosition.y = 0 end
    end
end

function love.draw()
    -- Called every frame for rendering
    
    -- Set text color to white
    love.graphics.setColor(1, 1, 1, 1)
    
    -- Draw the message at the center of the screen (큰 폰트)
    local width, height = love.graphics.getDimensions()
    love.graphics.setFont(fonts.large)
    local textWidth = fonts.large:getWidth(message)
    local textHeight = fonts.large:getHeight()
    
    love.graphics.print(message, 
                       (width - textWidth) / 2, 
                       (height - textHeight) / 2)
    
    -- Debug: 창 크기 정보를 화면 상단에 표시 (작은 폰트)
    love.graphics.setFont(fonts.small)
    love.graphics.print(windowInfo, 10, 10)
    
    -- 디버그 정보 그리기
    Debug.draw()
    
    -- 플레이어 위치에 점 표시 (디버그 맀드일 때)
    if Debug.isEnabled() then
        love.graphics.setColor(1, 0, 0, 1) -- 빨간색
        love.graphics.circle("fill", playerPosition.x, playerPosition.y, 5)
        love.graphics.setColor(1, 1, 1, 1) -- 색상 리셋
    end
    
    -- Draw debug console if visible
    if debugConsoleVisible then
        Logger.drawConsole(fonts.small, debugConsoleVisible)
    end
end

-- Debug console functions
function addDebugMessage(msg)
    local logMsg = Logger.debug(msg)  -- 표준 출력과 콘솔로 출력
    Logger.addToConsole(logMsg, 15)
end

function love.keypressed(key)
    -- Called when a key is pressed
    if key == "escape" then
        Logger.info("Game exiting...")
        love.event.quit()
    elseif key == "f1" then
        -- Toggle debug console
        debugConsoleVisible = not debugConsoleVisible
        if debugConsoleVisible then
            -- Add current time info when opening console
            local timeStr = "Console opened at: " .. love.timer.getTime()
            addDebugMessage(timeStr)
            Logger.info("Debug console toggled ON")
        else
            Logger.info("Debug console toggled OFF")
        end
    elseif key == "f2" then
        -- Test logging functions
        Logger.info("This is an INFO message")
        Logger.debug("This is a DEBUG message")
        Logger.warn("This is a WARN message")  -- NEW!
        Logger.error("This is an ERROR test (not a real error!)")
        addDebugMessage("Tested all log levels including WARN")
    elseif key == "f3" then
        -- Toggle debug mode
        local debugMode = Debug.toggleMode()
        local modeStr = debugMode and "ON" or "OFF"
        Logger.info("Debug mode toggled " .. modeStr)
        addDebugMessage("Debug mode: " .. modeStr)        
        if Debug.isEnabled() then
            -- 디버그 모드 시작시 플레이어 위치 리셋
            local width, height = love.graphics.getDimensions()
            playerPosition.x = width / 2
            playerPosition.y = height / 2
            gameSpeed = 2.0  -- 더 빠르게
        else
            gameSpeed = 1.0  -- 정상 속도
        end
    end
end

-- Close log file when game quits
function love.quit()
    Logger.close()
end