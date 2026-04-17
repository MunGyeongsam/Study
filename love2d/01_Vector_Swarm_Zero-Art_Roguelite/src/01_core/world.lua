-- World System
-- Arena-style world for stage-based gameplay.
-- Camera view is ~4.5x10 units; world is ~4x wider and ~3x taller.

local logger = require("00_common.logger")

local world = {}

-- Arena world size (camera sees ~4.5x10, world is comfortably larger)
world.size = {
    width  = 20,    -- ~4.4x camera width  → room to dodge sideways
    height = 30,    -- ~3x camera height   → enemies spawn above view
}

-- World center = player start position
world.center = {
    x = 0,
    y = 0,
}

-- Zones are not used for gameplay in Phase 2.
-- Stage number drives difficulty instead. Kept as empty table for API compat.
world.zones = {}

-- Fun elements placeholder (Phase 3+)
world.funElements = {
    powerUps    = {},
    checkpoints = {},
    secretPaths = {},
}

-- Init
function world.init()
    logger.info("[WORLD] Arena world initialized")
    logger.info(string.format("[WORLD] Size: %dx%d units, center: (%.0f, %.0f)",
        world.size.width, world.size.height, world.center.x, world.center.y))
end

-- Draw world grid and boundary
function world.drawGrid(gridSize, camera, showZones)
    local lg = love.graphics
    gridSize = gridSize or 2

    local pixelToWorld = 1
    if camera then
        pixelToWorld = camera:getUnitsPerPixel()
    else
        local _, screenHeight = lg.getDimensions()
        pixelToWorld = 10 / screenHeight
    end

    local r, g, b, a = lg.getColor()
    local lineWidth = lg.getLineWidth()

    local halfW = world.size.width  / 2
    local halfH = world.size.height / 2

    -- Grid lines (subtle)
    lg.setColor(0.2, 0.2, 0.2, 0.4)
    lg.setLineWidth(pixelToWorld * 0.5)

    for x = -halfW, halfW, gridSize do
        lg.line(x, -halfH, x, halfH)
    end
    for y = -halfH, halfH, gridSize do
        lg.line(-halfW, y, halfW, y)
    end

    -- Center axes (green)
    lg.setLineWidth(pixelToWorld * 2)
    lg.setColor(0, 0.8, 0, 0.6)
    lg.line(0, -halfH, 0, halfH)
    lg.line(-halfW, 0, halfW, 0)

    -- World boundary (red)
    lg.setColor(1, 0.2, 0.2, 0.8)
    lg.setLineWidth(pixelToWorld * 2)
    lg.rectangle("line", -halfW, -halfH, world.size.width, world.size.height)

    -- Start marker
    lg.setColor(0, 1, 0, 0.8)
    local markerSize = pixelToWorld * 6
    lg.circle("fill", world.center.x, world.center.y, markerSize)

    lg.setColor(r, g, b, a)
    lg.setLineWidth(lineWidth)
end

-- Gameplay API (simplified for arena phase, kept for compat)

function world.getZoneAtPosition(x, y)
    return nil, nil
end

function world.collectPowerUp(playerX, playerY, radius)
    return nil
end

function world.activateCheckpoint(playerX, playerY, radius)
    return nil
end

function world.discoverSecretPath(playerX, playerY, radius)
    return nil
end

function world.getSize()
    return world.size.width, world.size.height
end

function world.getBounds()
    local halfW = world.size.width  / 2
    local halfH = world.size.height / 2
    return -halfW, -halfH, halfW, halfH  -- left, bottom, right, top
end

function world.getStartPosition()
    return world.center.x, world.center.y
end

function world.resetFunElements()
    -- no-op in arena phase
end

function world.getProgressPercentage(playerY)
    return 0  -- not meaningful in arena mode
end

function world.getWorldStats()
    return {
        totalZones           = 0,
        powerUpsRemaining    = 0,
        checkpointsActivated = 0,
        secretsDiscovered    = 0,
        worldSize            = string.format("%d x %d", world.size.width, world.size.height),
    }
end

return world