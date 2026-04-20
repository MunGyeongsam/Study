-- Background System — Random Space Filling (Paul Bourke)
-- Ref: https://paulbourke.net/fractals/randomtile/index.html
--
-- Procedurally fills the world with non-overlapping circles using
-- a decreasing area function A(i) = A0 * i^(-c).
-- Uses a spatial hash grid for O(1) visibility culling.

local logger = require("00_common.logger")
local world  = require("01_core.world")

local M = {}

-- ─── Configuration ───────────────────────────────────────────────
local DEFAULT_C         = 1.2     -- area decay exponent
local MAX_CIRCLES       = 5000    -- total circles to generate
local BIG_THRESHOLD     = 200     -- first N circles placed immediately
local PER_FRAME         = 80      -- small circles placed per frame
local MAX_ATTEMPTS      = 200     -- max random placement tries per circle
local CELL_SIZE         = 2       -- spatial hash grid cell size (world units)
local MIN_RADIUS        = 0.01    -- skip circles smaller than this
local BASE_ALPHA        = 0.12    -- base opacity for circles

-- ─── State ───────────────────────────────────────────────────────
local circles   = {}      -- {x, y, radius} flat array
local grid      = {}      -- spatial hash: "cx,cy" -> list of circle indices
local count     = 0       -- circles placed so far
local nextIndex = 1       -- next circle index to place
local A0        = 0       -- initial area (calculated from c + world area)
local currentC  = DEFAULT_C
local seed      = 1
local rng       = nil     -- love.math RandomGenerator (seeded)

-- World bounds
local worldW, worldH = 20, 30
local worldMinX, worldMinY = -10, -15
local worldMaxX, worldMaxY = 10, 15

-- Draw style: "filled", "outline", "mixed"
local drawStyle = "mixed"
local enabled   = true
local generating = false  -- true while progressive generation is active

-- ─── Riemann Zeta approximation (partial sum) ────────────────────
local function zetaPartial(c, n)
    local s = 0
    for i = 1, n do
        s = s + i ^ (-c)
    end
    return s
end

-- ─── Spatial hash helpers ────────────────────────────────────────
local floor = math.floor
local _min  = math.min
local _max  = math.max

-- Register a circle index into all cells it overlaps
local function gridInsert(idx, cx, cy, r)
    local gx0 = floor((cx - r) / CELL_SIZE)
    local gy0 = floor((cy - r) / CELL_SIZE)
    local gx1 = floor((cx + r) / CELL_SIZE)
    local gy1 = floor((cy + r) / CELL_SIZE)
    for gx = gx0, gx1 do
        for gy = gy0, gy1 do
            local key = gx .. "," .. gy
            if not grid[key] then grid[key] = {} end
            grid[key][#grid[key] + 1] = idx
        end
    end
end

-- Check if a candidate circle overlaps any existing circle
local function overlaps(cx, cy, r)
    local gx0 = floor((cx - r) / CELL_SIZE)
    local gy0 = floor((cy - r) / CELL_SIZE)
    local gx1 = floor((cx + r) / CELL_SIZE)
    local gy1 = floor((cy + r) / CELL_SIZE)
    local checked = {}  -- avoid double-checking same circle
    for gx = gx0, gx1 do
        for gy = gy0, gy1 do
            local key = gx .. "," .. gy
            local cell = grid[key]
            if cell then
                for k = 1, #cell do
                    local idx = cell[k]
                    if not checked[idx] then
                        checked[idx] = true
                        local c = circles[idx]
                        local dx = cx - c.x
                        local dy = cy - c.y
                        local minDist = r + c.r
                        if dx * dx + dy * dy < minDist * minDist then
                            return true
                        end
                    end
                end
            end
        end
    end
    return false
end

-- Place a single circle (returns true if placed)
local sqrt = math.sqrt
local pi   = math.pi

local function placeCircle(i)
    local area = A0 * i ^ (-currentC)
    local r = sqrt(area / pi)
    if r < MIN_RADIUS then return false end

    for _ = 1, MAX_ATTEMPTS do
        local x = rng:random() * worldW + worldMinX
        local y = rng:random() * worldH + worldMinY
        -- Check bounds (circle fully inside world)
        if x - r >= worldMinX and x + r <= worldMaxX
        and y - r >= worldMinY and y + r <= worldMaxY then
            if not overlaps(x, y, r) then
                count = count + 1
                circles[count] = { x = x, y = y, r = r }
                gridInsert(count, x, y, r)
                return true
            end
        end
    end
    return false  -- couldn't place after MAX_ATTEMPTS
end

-- ─── Public API ──────────────────────────────────────────────────

function M.init(stageNum)
    circles   = {}
    grid      = {}
    count     = 0
    nextIndex = 1
    generating = true

    -- Seed from stage number for reproducible backgrounds
    seed = (stageNum or 1) * 12345 + 67890
    rng = love.math.newRandomGenerator(seed)

    -- World size from world module
    worldW = world.size.width
    worldH = world.size.height
    worldMinX = world.center.x - worldW / 2
    worldMinY = world.center.y - worldH / 2
    worldMaxX = world.center.x + worldW / 2
    worldMaxY = world.center.y + worldH / 2

    -- Calculate A0 from Riemann zeta partial sum
    -- Total area to fill = some fraction of world area (not 100%!)
    local fillRatio = 0.55
    local totalArea = worldW * worldH * fillRatio
    local zeta = zetaPartial(currentC, MAX_CIRCLES)
    A0 = totalArea / zeta

    -- Phase 1: Place big circles immediately
    for i = 1, _min(BIG_THRESHOLD, MAX_CIRCLES) do
        placeCircle(i)
        nextIndex = i + 1
    end

    if nextIndex > MAX_CIRCLES then
        generating = false
    end

    logger.info(string.format("[BG] Init: seed=%d, c=%.2f, big=%d placed, total target=%d",
        seed, currentC, count, MAX_CIRCLES))
end

function M.update(dt)
    if not generating then return end

    local placed = 0
    while nextIndex <= MAX_CIRCLES and placed < PER_FRAME do
        placeCircle(nextIndex)
        nextIndex = nextIndex + 1
        placed = placed + 1
    end

    if nextIndex > MAX_CIRCLES then
        generating = false
        logger.info(string.format("[BG] Generation complete: %d circles", count))
    end
end

function M.draw(camera)
    if not enabled or count == 0 then return end

    local lg = love.graphics

    -- Get camera visible bounds (world coords)
    local camX, camY = camera:pos()
    local orthoSize = camera:getOrthographicSize()
    local screenW, screenH = lg.getDimensions()
    local aspect = screenW / screenH
    local halfH = orthoSize
    local halfW = orthoSize * aspect

    local viewMinX = camX - halfW - 1  -- padding for partially visible circles
    local viewMaxX = camX + halfW + 1
    local viewMinY = camY - halfH - 1
    local viewMaxY = camY + halfH + 1

    -- Query spatial hash for visible cells
    local gx0 = floor(viewMinX / CELL_SIZE)
    local gy0 = floor(viewMinY / CELL_SIZE)
    local gx1 = floor(viewMaxX / CELL_SIZE)
    local gy1 = floor(viewMaxY / CELL_SIZE)

    local drawn = {}  -- avoid drawing same circle twice (multi-cell overlap)

    for gx = gx0, gx1 do
        for gy = gy0, gy1 do
            local key = gx .. "," .. gy
            local cell = grid[key]
            if cell then
                for k = 1, #cell do
                    local idx = cell[k]
                    if not drawn[idx] then
                        drawn[idx] = true
                        local c = circles[idx]
                        -- Alpha scales with radius (bigger = more subtle, smaller = brighter)
                        local alpha = BASE_ALPHA + (1 - _min(c.r / 2, 1)) * 0.08
                        lg.setColor(0.15, 0.35, 0.4, alpha)

                        if drawStyle == "filled" then
                            lg.circle("fill", c.x, c.y, c.r)
                        elseif drawStyle == "outline" then
                            lg.setLineWidth(0.02)
                            lg.circle("line", c.x, c.y, c.r)
                        else  -- mixed
                            if c.r > 0.3 then
                                lg.circle("fill", c.x, c.y, c.r)
                            else
                                lg.setLineWidth(0.015)
                                lg.circle("line", c.x, c.y, c.r)
                            end
                        end
                    end
                end
            end
        end
    end

    lg.setLineWidth(1)
end

-- ─── Controls ────────────────────────────────────────────────────

function M.toggle()
    enabled = not enabled
    logger.info(string.format("[BG] Background: %s", enabled and "ON" or "OFF"))
end

function M.cycleStyle()
    if drawStyle == "filled" then
        drawStyle = "outline"
    elseif drawStyle == "outline" then
        drawStyle = "mixed"
    else
        drawStyle = "filled"
    end
    logger.info(string.format("[BG] Style: %s", drawStyle))
end

function M.adjustC(delta)
    currentC = _max(1.05, _min(2.0, currentC + delta))
    logger.info(string.format("[BG] c = %.2f (regenerating...)", currentC))
    -- Regenerate with new c but same seed base
    local stageNum = floor((seed - 67890) / 12345)
    M.init(stageNum)
end

function M.getC()
    return currentC
end

function M.getCount()
    return count
end

function M.isGenerating()
    return generating
end

function M.isEnabled()
    return enabled
end

function M.getStyle()
    return drawStyle
end

return M
