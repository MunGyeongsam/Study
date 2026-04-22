-- GalleryScene
-- Enemy Gallery: 모든 적 도형을 격자로 나열하는 디버그/확인용 씬
-- 타이틀에서 G키로 진입. ESC로 복귀.
-- Page 1: 기존 5종 (base + 4 variants)
-- Page 2: 신규 도형 6종 (fill + line)
-- Page 3: DNA 적 랜덤 생성 (R키로 재생성)

local basicShapes = require("03_game.systems.renderers.basicShapes")
local dnaDefs     = require("03_game.data.dnaDefs")

local shapes_draw = basicShapes.shapes_draw

local lg = love.graphics
local _floor = math.floor
local _sin   = math.sin
local _rad   = math.rad

local GalleryScene = {}
GalleryScene.__index = GalleryScene

GalleryScene.name        = "GalleryScene"
GalleryScene.transparent = false
GalleryScene.drawBelow   = true

-- ─── Page definitions ────────────────────────────────────────────

-- Page 1: 기존 5종 × (base + 4 variants)
local BASE_TYPES = {
    { shape = "circle",      name = "bit",    color = {0, 1, 1} },
    { shape = "diamond",     name = "node",   color = {1, 0.5, 0} },
    { shape = "arrow",       name = "vector", color = {1, 0.2, 0.2} },
    { shape = "spiral_ring", name = "loop",   color = {0.5, 0.2, 1} },
    { shape = "hexagon",     name = "matrix", color = {0.2, 1, 0.5} },
}

local VARIANTS = { "base", "swift", "armored", "splitter", "shielded" }
local VARIANT_COLORS = {
    base     = nil,  -- use base color
    swift    = {0.3, 0.9, 1.0},
    armored  = {0.7, 0.7, 0.5},
    splitter = {1.0, 0.6, 0.3},
    shielded = {0.3, 0.6, 1.0},
}

-- Page 2: 신규 도형 6종 (fill + line)
local NEW_SHAPES = { "triangle", "star", "cross", "tear", "bowtie", "gear" }

-- ─── GalleryScene ────────────────────────────────────────────────

function GalleryScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
        _page = 1,
        _maxPage = 3,
        _timer = 0,
        _dnaEntries = {},
        _fonts = nil,
    }, GalleryScene)
end

function GalleryScene:enter(prev)
    self._timer = 0
    self._page = 1
    self._fonts = {
        title = lg.newFont(20),
        label = lg.newFont(11),
        hint  = lg.newFont(10),
    }
    self:_generateDna()
    logInfo("[GALLERY] GalleryScene entered")
end

function GalleryScene:exit()
    logInfo("[GALLERY] GalleryScene exited")
end

function GalleryScene:update(dt)
    self._timer = self._timer + dt
end

--- DNA 적 16마리 생성 (Page 3용)
function GalleryScene:_generateDna()
    self._dnaEntries = {}
    for round = 1, 4 do
        for i = 1, 4 do
            local dna = dnaDefs.generateDna(round)
            dna._round = round
            self._dnaEntries[#self._dnaEntries + 1] = dna
        end
    end
    logInfo("[GALLERY] DNA entries regenerated (16 samples)")
end

-- ─── Drawing helpers ─────────────────────────────────────────────

--- 도형 1개를 스크린 좌표에 렌더
--- @param shape string shapes_draw key
--- @param cx number 스크린 X 중심
--- @param cy number 스크린 Y 중심
--- @param radius number 픽셀 반지름
--- @param mode string "fill" | "line"
--- @param color table {r,g,b}
--- @param rot number 회전 (도)
local function drawShape(shape, cx, cy, radius, mode, color, rot)
    local fn = shapes_draw[shape]
    if not fn then return end
    setColor(color[1] * 255, color[2] * 255, color[3] * 255, 255)
    if rot and rot ~= 0 then
        lg.push()
        lg.translate(cx, cy)
        lg.rotate(_rad(rot))
        fn(0, 0, radius, mode)
        lg.pop()
    else
        fn(cx, cy, radius, mode)
    end
end

--- 텍스트를 셀 아래 중앙에
local function drawLabel(font, text, cx, cy)
    lg.setFont(font)
    local tw = font:getWidth(text)
    lg.print(text, cx - tw / 2, cy)
end

--- variant 오버레이 시각 힌트 (간이: 링/대시/아크 표시)
local function drawVariantHint(variant, cx, cy, radius, baseColor)
    if variant == "swift" then
        -- 잔상 3개
        for g = 1, 3 do
            local dist = g * radius * 0.6
            local ga = (4 - g) * 0.12
            setColor(baseColor[1] * 255, baseColor[2] * 255, baseColor[3] * 255, ga * 255)
            lg.circle("fill", cx - dist, cy, radius * (1 - g * 0.15))
        end
    elseif variant == "armored" then
        setColor(baseColor[1] * 200, baseColor[2] * 200, baseColor[3] * 200, 220)
        lg.setLineWidth(radius * 0.25)
        lg.circle("line", cx, cy, radius * 1.15)
        lg.setLineWidth(1)
    elseif variant == "splitter" then
        setColor(baseColor[1] * 255, baseColor[2] * 255, baseColor[3] * 255, 180)
        lg.setLineWidth(radius * 0.12)
        local pi2 = math.pi * 2
        for s = 0, 7, 2 do
            local a1 = (s / 8) * pi2
            local a2 = ((s + 1) / 8) * pi2
            lg.arc("line", "open", cx, cy, radius * 1.15, a1, a2)
        end
        lg.setLineWidth(1)
    elseif variant == "shielded" then
        local arcHalf = 0.7854
        setColor(baseColor[1] * 200, baseColor[2] * 200, baseColor[3] * 200, 150)
        lg.setLineWidth(radius * 0.25)
        lg.arc("line", "open", cx, cy, radius * 1.2, -math.pi/2 - arcHalf, -math.pi/2 + arcHalf)
        lg.setLineWidth(1)
    end
end

--- DNA body 레이어 배열을 스크린 좌표에 렌더
local function drawDnaBody(layers, cx, cy, radius, color)
    for _, layer in ipairs(layers) do
        local fn = shapes_draw[layer.shape]
        if fn then
            local lr = radius * (layer.scale or 1.0)
            local rot = layer.rot or 0
            setColor(color[1] * 255, color[2] * 255, color[3] * 255, 255)
            if rot ~= 0 then
                lg.push()
                lg.translate(cx, cy)
                lg.rotate(_rad(rot))
                fn(0, 0, lr, layer.mode or "fill")
                lg.pop()
            else
                fn(cx, cy, lr, layer.mode or "fill")
            end
        end
    end
end

-- ─── Page drawing ────────────────────────────────────────────────

function GalleryScene:_drawPage1()
    local W, H = lg.getDimensions()
    local fonts = self._fonts

    -- 5 rows (types) × 5 cols (base + 4 variants)
    local cols = #VARIANTS
    local rows = #BASE_TYPES
    local cellW = W / (cols + 1)
    local cellH = (H - 120) / (rows + 1)
    local radius = math.min(cellW, cellH) * 0.25
    local startY = 80

    -- Column headers
    lg.setFont(fonts.label)
    for c, vName in ipairs(VARIANTS) do
        local cx = cellW * (c - 1) + cellW
        setColor(150, 150, 170, 200)
        drawLabel(fonts.label, vName, cx, startY - 20)
    end

    -- Grid
    for r, base in ipairs(BASE_TYPES) do
        local cy = startY + cellH * (r - 1) + cellH * 0.4

        -- Row label
        setColor(150, 150, 170, 200)
        drawLabel(fonts.label, base.name, cellW * 0.3, cy + radius + 4)

        for c, vName in ipairs(VARIANTS) do
            local cx = cellW * (c - 1) + cellW
            local color = VARIANT_COLORS[vName] or base.color

            -- variant hint (behind)
            if vName ~= "base" then
                drawVariantHint(vName, cx, cy, radius, color)
            end

            -- shape
            drawShape(base.shape, cx, cy, radius, "fill", color, 0)
        end
    end
end

function GalleryScene:_drawPage2()
    local W, H = lg.getDimensions()
    local fonts = self._fonts

    -- 6 shapes × 2 modes (fill, line) = 12 cells
    local cols = 2
    local rows = #NEW_SHAPES
    local cellW = W / 3
    local cellH = (H - 120) / (rows + 1)
    local radius = math.min(cellW, cellH) * 0.28
    local startY = 80

    -- Column headers
    lg.setFont(fonts.label)
    local headers = { "fill", "line" }
    for c, hdr in ipairs(headers) do
        local cx = cellW * c
        setColor(150, 150, 170, 200)
        drawLabel(fonts.label, hdr, cx, startY - 20)
    end

    -- Color for new shapes
    local newColor = {0.8, 0.6, 1.0}

    for r, shapeName in ipairs(NEW_SHAPES) do
        local cy = startY + cellH * (r - 1) + cellH * 0.4

        -- Row label
        setColor(150, 150, 170, 200)
        drawLabel(fonts.label, shapeName, cellW * 0.35, cy + radius + 4)

        -- fill
        drawShape(shapeName, cellW * 1, cy, radius, "fill", newColor, 0)
        -- line
        drawShape(shapeName, cellW * 2, cy, radius, "line", newColor, 0)
    end
end

function GalleryScene:_drawPage3()
    local W, H = lg.getDimensions()
    local fonts = self._fonts

    -- 4×4 grid of DNA enemies
    local cols = 4
    local rows = 4
    local cellW = W / (cols + 0.5)
    local cellH = (H - 140) / (rows + 0.5)
    local radius = math.min(cellW, cellH) * 0.22
    local startY = 80

    -- Round headers on left
    for r = 1, rows do
        local cy = startY + cellH * (r - 1) + cellH * 0.35
        setColor(100, 100, 120, 180)
        drawLabel(fonts.label, string.format("R%d", r), cellW * 0.2, cy - 6)
    end

    for r = 1, rows do
        for c = 1, cols do
            local idx = (r - 1) * cols + c
            local dna = self._dnaEntries[idx]
            if dna then
                local cx = cellW * (c - 1) + cellW * 0.8
                local cy = startY + cellH * (r - 1) + cellH * 0.35

                -- Body layers
                drawDnaBody(dna.body, cx, cy, radius, dna.color)

                -- Label: movement/attack
                setColor(150, 150, 170, 160)
                local info = string.format("%s/%s", dna.movement, dna.attack)
                drawLabel(fonts.hint, info, cx, cy + radius + 2)

                -- Modifier indicator
                if dna.modifier ~= "none" then
                    setColor(200, 200, 100, 180)
                    drawLabel(fonts.hint, dna.modifier, cx, cy + radius + 13)
                end

                -- Layer count
                setColor(100, 100, 100, 120)
                drawLabel(fonts.hint, string.format("%dL", #dna.body), cx, cy - radius - 12)
            end
        end
    end
end

-- ─── Main draw ───────────────────────────────────────────────────

function GalleryScene:draw()
    local W, H = lg.getDimensions()
    local fonts = self._fonts
    local t = self._timer

    -- Background
    lg.setColor(0, 0, 0, 0.92)
    lg.rectangle("fill", 0, 0, W, H)

    -- Title
    lg.setFont(fonts.title)
    local pageNames = { "EXISTING ENEMIES", "NEW SHAPES (6B)", "DNA MUTATIONS" }
    local title = string.format("ENEMY GALLERY — %s (%d/%d)",
        pageNames[self._page] or "", self._page, self._maxPage)
    local glow = 0.6 + 0.2 * _sin(t * 2)
    lg.setColor(0, glow, 1.0, 1)
    lg.printf(title, 0, 16, W, "center")

    -- Page content
    if self._page == 1 then
        self:_drawPage1()
    elseif self._page == 2 then
        self:_drawPage2()
    elseif self._page == 3 then
        self:_drawPage3()
    end

    -- Bottom hints
    lg.setFont(fonts.hint)
    local alpha = 0.5 + 0.2 * _sin(t * 2)
    lg.setColor(0.5, 0.5, 0.5, alpha)
    local hint = "LEFT/RIGHT: page | ESC: back"
    if self._page == 3 then
        hint = "LEFT/RIGHT: page | R: regenerate | ESC: back"
    end
    lg.printf(hint, 0, H - 28, W, "center")

    resetColor()
end

-- ─── Input ───────────────────────────────────────────────────────

function GalleryScene:keypressed(key)
    if key == "escape" then
        self._sceneStack:pop()
        return true
    elseif key == "left" then
        self._page = self._page - 1
        if self._page < 1 then self._page = self._maxPage end
        return true
    elseif key == "right" then
        self._page = self._page + 1
        if self._page > self._maxPage then self._page = 1 end
        return true
    elseif key == "r" and self._page == 3 then
        self:_generateDna()
        return true
    end
    return false
end

function GalleryScene:touchpressed(id, x, y, dx, dy, pressure)
    local W = lg.getDimensions()
    if x < W * 0.3 then
        self._page = self._page - 1
        if self._page < 1 then self._page = self._maxPage end
    elseif x > W * 0.7 then
        self._page = self._page + 1
        if self._page > self._maxPage then self._page = 1 end
    else
        self._sceneStack:pop()
    end
    return true
end

return GalleryScene
