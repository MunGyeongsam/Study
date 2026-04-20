-- CreditsScene
-- 크레딧 오버레이: Zero-Art 스타일 텍스트
-- drawBelow=true (타이틀 배경 보임), transparent=false

local CreditsScene = {}
CreditsScene.__index = CreditsScene

CreditsScene.name        = "CreditsScene"
CreditsScene.transparent = false
CreditsScene.drawBelow   = true

local CREDITS = {
    { size = "xlarge", text = "VECTOR SWARM" },
    { size = "small",  text = "Zero-Art Roguelite" },
    { size = "small",  text = "" },
    { size = "medium", text = "Code & Design" },
    { size = "small",  text = "gsmun" },
    { size = "small",  text = "" },
    { size = "medium", text = "Background Algorithm" },
    { size = "small",  text = "Inspired by Paul Bourke" },
    { size = "small",  text = "\"Random Space Filling\"" },
    { size = "small",  text = "paulbourke.net/fractals/randomtile/" },
    { size = "small",  text = "" },
    { size = "medium", text = "Reference" },
    { size = "small",  text = "John Shier — Filling Space with" },
    { size = "small",  text = "Random Fractal Non-Overlapping Shapes" },
    { size = "small",  text = "" },
    { size = "medium", text = "Made with" },
    { size = "small",  text = "LOVE2D 11.5" },
    { size = "small",  text = "" },
    { size = "small",  text = "---" },
    { size = "small",  text = "" },
    { size = "medium", text = "Zero-Art" },
    { size = "small",  text = "No sprites. Just math." },
    { size = "small",  text = "" },
    { size = "small",  text = "[ESC] Back" },
}

-- Line height per font size
local LINE_H = { small = 18, medium = 24, large = 28, xlarge = 32 }

function CreditsScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
        _timer = 0,
        _fonts = nil,
    }, CreditsScene)
end

function CreditsScene:enter(prev)
    self._timer = 0
    self._fonts = {
        small  = love.graphics.newFont(12),
        medium = love.graphics.newFont(16),
        large  = love.graphics.newFont(20),
        xlarge = love.graphics.newFont(24),
    }
    logInfo("[CREDITS] CreditsScene entered")
end

function CreditsScene:exit()
    logInfo("[CREDITS] CreditsScene exited")
end

function CreditsScene:update(dt)
    self._timer = self._timer + dt
end

function CreditsScene:draw()
    local lg = love.graphics
    local W, H = lg.getDimensions()

    -- Dim overlay
    lg.setColor(0, 0, 0, 0.85)
    lg.rectangle("fill", 0, 0, W, H)

    -- Calculate total height
    local fonts = self._fonts
    local totalH = 0
    for _, line in ipairs(CREDITS) do
        totalH = totalH + (LINE_H[line.size] or 18)
    end

    local startY = (H - totalH) / 2
    local y = startY

    for _, line in ipairs(CREDITS) do
        local font = fonts[line.size] or fonts.small
        lg.setFont(font)

        local lh = LINE_H[line.size] or 18

        if line.text == "---" then
            -- Decorative line
            lg.setColor(0.4, 0.4, 0.4, 0.6)
            lg.setLineWidth(1)
            lg.line(W * 0.3, y + lh / 2, W * 0.7, y + lh / 2)
        elseif line.text ~= "" then
            -- Title/header glow
            if line.size == "xlarge" then
                local glow = 0.5 + 0.3 * math.sin(self._timer * 2)
                lg.setColor(0.3, 0.8, 1.0, glow)
            elseif line.size == "medium" then
                lg.setColor(0.6, 0.8, 0.9, 0.9)
            else
                lg.setColor(0.5, 0.6, 0.65, 0.8)
            end
            lg.printf(line.text, 0, y, W, "center")
        end

        y = y + lh
    end
end

function CreditsScene:keypressed(key)
    if key == "escape" or key == "return" or key == "space" then
        self._sceneStack:pop()
        return true
    end
    return false
end

function CreditsScene:touchpressed(id, x, y, dx, dy, pressure)
    self._sceneStack:pop()
    return true
end

return CreditsScene
