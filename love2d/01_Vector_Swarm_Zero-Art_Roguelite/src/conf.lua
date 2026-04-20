
local resolutionList = {
    -- Portrait mode (9:20 ratio) - 세로 모드
    {width = 216, height = 480},   -- Very small portrait
    {width = 270, height = 600},   -- Small portrait  
    {width = 324, height = 720},   -- Medium portrait (current)
    {width = 378, height = 840},   -- Large portrait
    {width = 432, height = 960},   -- Very large portrait
    {width = 540, height = 1200},  -- Extra large portrait
    
    -- Landscape mode (20:9 ratio) - 가로 모드
    {width = 800, height = 360},   -- Small landscape
    {width = 1000, height = 450},  -- Medium landscape
    {width = 1280, height = 576},  -- Large landscape
    {width = 1600, height = 720},  -- Very large landscape
    {width = 1920, height = 864},  -- Extra large landscape
    {width = 2560, height = 1152}, -- Ultra wide landscape
}

local resolutionIndex = 5
local resolution = resolutionList[resolutionIndex]

function love.conf(t)
    -- Note: logger unavailable at conf time; use print only for critical debug
    
    t.title = "Vector Swarm - Zero Art Roguelite"    -- The title of the window
    t.author = "Your Name"                           -- The author of the game
    t.version = "11.5"                               -- The LÖVE version this game was made for
    
    -- Window settings (Galaxy Note 20 ratio 9:20 - Portrait mode, scaled down)
    t.window.width = resolution.width                -- Window width (9:20 ratio - Portrait, smaller)
    t.window.height = resolution.height              -- Window height (9:20 ratio - Portrait, smaller)
    t.window.minwidth = resolution.width / 2        -- Minimum window width
    t.window.minheight = resolution.height / 2      -- Minimum window height
    t.window.resizable = true                        -- Let the window be resizable
    t.window.centered = true                         -- Center the window
    t.window.vsync = 1                               -- Enable vertical sync
    t.window.msaa = 8                                -- The number of samples to use with multi-sampled antialiasing
    t.window.fullscreen = false                      -- Enable fullscreen (boolean)
    t.window.fullscreentype = "desktop"              -- Choose between "desktop" fullscreen or "exclusive" fullscreen mode
    t.window.display = 1                             -- Index of the monitor to show the window in
    
    -- Console settings
    -- lovec.exe 사용 시 t.console=false (stdout이 터미널로 직접 감)
    -- love.exe 사용 시 t.console=true (별도 콘솔 창 띄움)
    t.console = false
    
    -- Modules to enable
    t.modules.audio = true                           -- Enable the audio module
    t.modules.data = true                            -- Enable the data module  
    t.modules.event = true                           -- Enable the event module
    t.modules.font = true                            -- Enable the font module
    t.modules.graphics = true                        -- Enable the graphics module
    t.modules.image = true                           -- Enable the image module
    t.modules.joystick = true                        -- Enable the joystick module
    t.modules.keyboard = true                        -- Enable the keyboard module
    t.modules.math = true                            -- Enable the math module
    t.modules.mouse = true                           -- Enable the mouse module
    t.modules.physics = true                         -- Enable the physics module
    t.modules.sound = true                           -- Enable the sound module
    t.modules.system = true                          -- Enable the system module
    t.modules.thread = true                          -- Enable the thread module
    t.modules.timer = true                           -- Enable the timer module
    t.modules.touch = true                           -- Enable the touch module
    t.modules.video = true                           -- Enable the video module
    t.modules.window = true                          -- Enable the window module
end