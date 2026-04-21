-- Enemy AI System
-- Reads EnemyAI + Transform + Velocity, sets velocity based on behavior.
-- Receives playerQuery function via closure to find player position.
-- Each behavior is a separate handler for maintainability.
-- Swarm behavior includes spatial-hash-based separation for 3000+ entities.

local System = require("01_core.system")

local cos = math.cos
local sin = math.sin
local sqrt = math.sqrt
local atan2 = math.atan2
local floor = math.floor

-- ===== Swarm Spatial Hash (rebuilt each frame, swarm entities only) =====
local SEPARATION_RADIUS = 0.25    -- 분리 반경 (world units)
local SEPARATION_FORCE  = 1.2     -- 분리력 강도
local CELL_SIZE         = 0.5     -- 그리드 셀 크기 (≥ SEPARATION_RADIUS)
local INV_CELL          = 1 / CELL_SIZE

-- Pre-allocated tables (reused each frame to avoid GC)
local swarmGrid   = {}  -- "cx,cy" → { {x,y,idx}, ... }
local swarmList   = {}  -- flat list of {x, y, entityId}
local swarmCount  = 0

local function swarmGridClear()
    for k in pairs(swarmGrid) do swarmGrid[k] = nil end
    swarmCount = 0
end

local function swarmGridInsert(x, y, entityId)
    swarmCount = swarmCount + 1
    local entry = swarmList[swarmCount]
    if entry then
        entry[1], entry[2], entry[3] = x, y, entityId
    else
        swarmList[swarmCount] = { x, y, entityId }
    end
    local cx = floor(x * INV_CELL)
    local cy = floor(y * INV_CELL)
    local key = cx * 100003 + cy  -- integer hash (no string concat)
    local cell = swarmGrid[key]
    if not cell then
        cell = {}
        swarmGrid[key] = cell
    end
    cell[#cell + 1] = swarmCount
end

-- Returns separation vector (sx, sy) for entity at (x, y)
local function swarmSeparation(x, y, selfIdx)
    local sx, sy = 0, 0
    local cx = floor(x * INV_CELL)
    local cy = floor(y * INV_CELL)
    local r2 = SEPARATION_RADIUS * SEPARATION_RADIUS

    for nx = cx - 1, cx + 1 do
        for ny = cy - 1, cy + 1 do
            local cell = swarmGrid[nx * 100003 + ny]
            if cell then
                for i = 1, #cell do
                    local idx = cell[i]
                    local other = swarmList[idx]
                    if other[3] ~= selfIdx then
                        local dx = x - other[1]
                        local dy = y - other[2]
                        local d2 = dx * dx + dy * dy
                        if d2 < r2 and d2 > 0.0001 then
                            local d = sqrt(d2)
                            local invD = 1 / d
                            -- 가까울수록 강한 반발 (1/d 비례)
                            local strength = (SEPARATION_RADIUS - d) * invD * SEPARATION_FORCE
                            sx = sx + dx * strength
                            sy = sy + dy * strength
                        end
                    end
                end
            end
        end
    end
    return sx, sy
end

-- ===== Behavior Handlers =====
-- Each handler receives (ai, transform, velocity, ecs, entityId, px, py, dt)

local function handleDrift(ai, _, velocity)
    velocity.vx = ai.driftVx
    velocity.vy = ai.driftVy
end

local function handleOrbit(ai, transform, velocity, _, _, _, _, dt)
    ai.orbitAngle = ai.orbitAngle + ai.orbitSpeed * dt
    local targetX = ai.orbitCenterX + cos(ai.orbitAngle) * ai.orbitRadius
    local targetY = ai.orbitCenterY + sin(ai.orbitAngle) * ai.orbitRadius
    local dx = targetX - transform.x
    local dy = targetY - transform.y
    local dist = sqrt(dx * dx + dy * dy)
    if dist > 0.01 then
        velocity.vx = (dx / dist) * ai.speed
        velocity.vy = (dy / dist) * ai.speed
    else
        velocity.vx = 0
        velocity.vy = 0
    end
end

local function handleChase(ai, transform, velocity, _, _, px, py)
    if not (px and py) then return end
    local dx = px - transform.x
    local dy = py - transform.y
    local dist = sqrt(dx * dx + dy * dy)
    if dist > 0.5 then
        velocity.vx = (dx / dist) * ai.chaseSpeed
        velocity.vy = (dy / dist) * ai.chaseSpeed
    else
        velocity.vx = 0
        velocity.vy = 0
    end
end

local function handleStationary(ai, _, velocity, ecs, entityId, _, _, dt)
    velocity.vx = 0
    velocity.vy = 0
    local renderable = ecs:getComponent(entityId, "Renderable")
    if renderable then
        renderable.rotation = (renderable.rotation or 0) + ai.spinSpeed * dt
    end
end

local function handleSwarm(ai, transform, velocity, _, entityId, px, py)
    if not (px and py) then return end
    -- Pursue player
    local dx = px - transform.x
    local dy = py - transform.y
    local dist = sqrt(dx * dx + dy * dy)
    local vx, vy = 0, 0
    if dist > 0.01 then
        vx = (dx / dist) * ai.swarmSpeed
        vy = (dy / dist) * ai.swarmSpeed
    end
    -- Add separation force (calculated from spatial hash)
    local sx, sy = swarmSeparation(transform.x, transform.y, entityId)
    velocity.vx = vx + sx
    velocity.vy = vy + sy
end

local function handleCharge(ai, transform, velocity, ecs, entityId, px, py, dt)
    ai.chargeTimer = ai.chargeTimer + dt
    local renderable = ecs:getComponent(entityId, "Renderable")

    if ai.chargePhase == "warn" then
        velocity.vx = 0
        velocity.vy = 0
        -- Lock direction on first frame
        if ai.chargeDirX == 0 and ai.chargeDirY == 0 and px and py then
            local dx = px - transform.x
            local dy = py - transform.y
            local dist = sqrt(dx * dx + dy * dy)
            if dist > 0.01 then
                ai.chargeDirX = dx / dist
                ai.chargeDirY = dy / dist
            else
                ai.chargeDirX = 0
                ai.chargeDirY = -1
            end
        end
        if renderable then
            renderable.visible = (floor(ai.chargeTimer * 10) % 2 == 0)
            renderable.rotation = atan2(ai.chargeDirY, ai.chargeDirX)
        end
        -- Transition to dash
        if ai.chargeTimer >= ai.chargeWarnTime then
            ai.chargePhase = "dash"
            ai.chargeTimer = 0
            if renderable then renderable.visible = true end
        end
    elseif ai.chargePhase == "dash" then
        velocity.vx = ai.chargeDirX * ai.chargeSpeed
        velocity.vy = ai.chargeDirY * ai.chargeSpeed
        if renderable then
            renderable.rotation = atan2(ai.chargeDirY, ai.chargeDirX)
        end
    end
end

-- Dispatch table: behavior name → handler function
local behaviorHandlers = {
    drift      = handleDrift,
    orbit      = handleOrbit,
    chase      = handleChase,
    stationary = handleStationary,
    swarm      = handleSwarm,
    charge     = handleCharge,
}

-- ===== System Factory =====

local function createEnemyAISystem(getPlayerPos)

    local EnemyAISystem = System.new("EnemyAI", {"EnemyAI", "Transform", "Velocity"},
        function(ecs, dt, entities)
            local px, py = getPlayerPos()

            -- Phase 1: Build swarm spatial hash (swarm entities only)
            swarmGridClear()
            for _, entityId in ipairs(entities) do
                local ai = ecs:getComponent(entityId, "EnemyAI")
                if ai.behavior == "swarm" then
                    local t = ecs:getComponent(entityId, "Transform")
                    swarmGridInsert(t.x, t.y, entityId)
                end
            end

            -- Phase 2: Update all AI behaviors
            for _, entityId in ipairs(entities) do
                local ai        = ecs:getComponent(entityId, "EnemyAI")
                local transform = ecs:getComponent(entityId, "Transform")
                local velocity  = ecs:getComponent(entityId, "Velocity")

                local handler = behaviorHandlers[ai.behavior]
                if handler then
                    handler(ai, transform, velocity, ecs, entityId, px, py, dt)
                end
            end
        end
    )

    return EnemyAISystem
end

return createEnemyAISystem
