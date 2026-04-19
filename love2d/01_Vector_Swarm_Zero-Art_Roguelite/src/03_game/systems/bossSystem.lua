-- Boss System
-- Manages boss lifecycle: intro → active (phase/pattern cycling) → defeat.
-- Controls BulletEmitter pattern switching based on phase and timer.
-- Handles boss movement patterns per phase.

local System = require("01_core.system")
local worldMod = require("01_core.world")
local EntityFactory = require("03_game.entities.entityFactory")

-- Helper: apply a pattern step to the BulletEmitter component
local function applyPattern(emitter, step)
    if not step then return end
    if step.pattern == "none" then
        emitter.active = false
    else
        emitter.pattern        = step.pattern
        emitter.emitRate       = step.emitRate or emitter.emitRate
        emitter.bulletSpeed    = step.bulletSpeed or emitter.bulletSpeed
        emitter.bulletCount    = step.bulletCount or emitter.bulletCount
        emitter.bulletLifetime = step.bulletLifetime or emitter.bulletLifetime
        emitter.bulletRadius   = step.bulletRadius or emitter.bulletRadius
        emitter.bulletColor    = step.bulletColor or emitter.bulletColor
        emitter.turnRate       = step.turnRate or emitter.turnRate
        emitter.active = true
    end
end

local function createBossSystem(bulletPool, getPlayerPos)

    -- === INTRO: wait before activating boss ===
    -- Returns true if intro is still in progress (caller should skip other logic)
    local function handleIntro(boss, emitter, dt)
        if boss.introComplete then return false end
        boss.introTimer = boss.introTimer + dt
        emitter.active = false
        if boss.introTimer >= boss.introDuration then
            boss.introComplete = true
            screenShake(0.12, 0.25)
            local phasePatterns = boss.patterns[boss.phase]
            applyPattern(emitter, phasePatterns and phasePatterns[1])
            logInfo(string.format("[BOSS] %s intro complete, phase %d active", boss.bossType, boss.phase))
        end
        return true
    end

    -- === PHASE CHECK: HP threshold triggers phase transition ===
    local function handlePhaseCheck(ecs, entityId, boss, health, emitter)
        boss.phaseChanged = false
        local hpRatio = health.hp / health.maxHp
        local threshold = boss.phaseThresholds[boss.phase]
        if threshold and hpRatio <= threshold and boss.phase < boss.maxPhase then
            boss.phase = boss.phase + 1
            boss.phaseChanged = true
            boss.patternIndex = 1
            boss.patternTimer = 0
            boss.minionTimer = 0
            screenShake(0.18, 0.3)

            if bulletPool then
                bulletPool:clearLayer("enemy_bullet")
            end
            health.iTimer = 0.5

            -- Clear existing minions on phase transition
            if boss.minion then
                local world = ecs
                local allEnemies = world:queryEntities({"EnemyAI"})
                for _, eid in ipairs(allEnemies) do
                    if eid ~= entityId then
                        world:destroyEntity(eid)
                    end
                end
                logInfo(string.format("[BOSS] %s minions cleared for phase %d", boss.bossType, boss.phase))
            end

            -- Phase-specific teleport interval (OVERFLOW)
            if boss.phaseTeleport then
                boss.teleportInterval = boss.phaseTeleport[boss.phase] or 0
                boss.teleportTimer = 0
                boss.teleporting = false
            end

            -- Phase-specific color change (OVERFLOW)
            if boss.phaseColors then
                local renderable = ecs:getComponent(entityId, "Renderable")
                if renderable and boss.phaseColors[boss.phase] then
                    renderable.color = boss.phaseColors[boss.phase]
                end
            end

            local phasePatterns = boss.patterns[boss.phase]
            applyPattern(emitter, phasePatterns and phasePatterns[1])

            logInfo(string.format("[BOSS] %s phase %d → %d (HP: %.0f%%)",
                boss.bossType, boss.phase - 1, boss.phase, hpRatio * 100))
        end
    end

    -- === PATTERN CYCLING: time-based sequence within current phase ===
    local function handlePatternCycling(boss, emitter, dt)
        boss.patternTimer = boss.patternTimer + dt
        local phasePatterns = boss.patterns[boss.phase]
        if not phasePatterns then return end

        local currentStep = phasePatterns[boss.patternIndex]
        if currentStep and boss.patternTimer >= currentStep.duration then
            boss.patternTimer = boss.patternTimer - currentStep.duration
            boss.patternIndex = boss.patternIndex + 1
            if boss.patternIndex > #phasePatterns then
                boss.patternIndex = 1
            end

            local nextStep = phasePatterns[boss.patternIndex]
            applyPattern(emitter, nextStep)
            if nextStep then
                emitter.timer = 0
                emitter.angle = 0
            end
        end
    end

    -- === MINION SPAWNING: periodic spawn up to max (RECURSION) ===
    local function handleMinionSpawn(ecs, entityId, boss, transform, dt)
        if not boss.minion then return end
        local config = boss.minion[boss.phase]
        if not config or config.max == 0 then return end

        boss.minionTimer = boss.minionTimer + dt
        if boss.minionTimer < config.interval then return end
        boss.minionTimer = boss.minionTimer - config.interval

        -- Count non-boss enemies
        local allEnemies = ecs:queryEntities({"EnemyAI"})
        local minionCount = #allEnemies - 1  -- subtract boss itself
        if minionCount >= config.max then return end

        -- Spawn minions up to max
        local toSpawn = config.max - minionCount
        local world = ecs
        for i = 1, toSpawn do
            local angle = (i / toSpawn) * math.pi * 2
            local spawnX = transform.x + math.cos(angle) * 2.0
            local spawnY = transform.y + math.sin(angle) * 2.0
            EntityFactory.createEnemy(world, spawnX, spawnY, config.type or "basic",
                { enemyHpMult = config.hpMult or 0.5 })
        end
        logInfo(string.format("[BOSS] %s spawned %d minions (phase %d)", boss.bossType, toSpawn, boss.phase))
    end

    -- === MOVEMENT: teleport (interval>0) / orbit (RECURSION) / drift (default) ===
    local function handleMovement(ecs, entityId, boss, transform, ai, health, dt)
        if not ai then return end
        local _, py = getPlayerPos()
        local left, _, right, _ = worldMod.getBounds()
        local margin = 2

        if boss.teleportInterval > 0 then
            -- Teleport mode (HEAP always, OVERFLOW P3-P4)
            local renderable = ecs:getComponent(entityId, "Renderable")
            boss.teleportTimer = boss.teleportTimer + dt
            local warnStart = boss.teleportInterval - boss.teleportWarning

            if boss.teleportTimer >= warnStart and not boss.teleporting then
                boss.teleporting = true
                logInfo(string.format("[BOSS] %s teleport warning", boss.bossType))
            end

            if boss.teleporting and renderable then
                renderable.visible = (math.floor(boss.teleportTimer * 10) % 2 == 0)
            end

            if boss.teleportTimer >= boss.teleportInterval then
                local _, bottom, _, top = worldMod.getBounds()
                local newX = left + margin + math.random() * (right - left - margin * 2)
                local newY = (py or 0) + 2.0 + math.random() * 3.0
                newY = math.min(newY, top - margin)
                newY = math.max(newY, bottom + margin)

                transform.x = newX
                transform.y = newY
                boss.teleportTimer = 0
                boss.teleporting = false
                health.iTimer = boss.teleportCooldown
                if renderable then renderable.visible = true end
                logInfo(string.format("[BOSS] %s teleported to (%.1f, %.1f)", boss.bossType, newX, newY))
            end
        elseif boss.bossType == "RECURSION" then
            -- Orbit around player
            local px, _ = getPlayerPos()
            ai.orbitCenterX = px or 0
            ai.orbitCenterY = (py or 0) + 2.0
        else
            -- Drift ping-pong (NULL, STACK, OVERFLOW P1-P2)
            if transform.x >= right - margin then
                ai.driftVx = -math.abs(ai.driftVx or 0.4)
            elseif transform.x <= left + margin then
                ai.driftVx = math.abs(ai.driftVx or 0.4)
            end

            local targetOffset = (boss.bossType == "STACK") and 3.0 or 3.5
            local vertSpeed    = (boss.bossType == "STACK") and 0.2 or 0.3
            local targetY = (py or 0) + targetOffset
            local yDiff = targetY - transform.y
            ai.driftVy = yDiff > 0.1 and vertSpeed or (yDiff < -0.1 and -vertSpeed or 0)
        end
    end

    -- === Main system loop ===
    local BossSystem = System.new("Boss", {"Transform", "EnemyAI", "Health", "BossTag", "BulletEmitter"},
        function(ecs, dt, entities)
            for _, entityId in ipairs(entities) do
                local boss      = ecs:getComponent(entityId, "BossTag")
                local health    = ecs:getComponent(entityId, "Health")
                local emitter   = ecs:getComponent(entityId, "BulletEmitter")
                local transform = ecs:getComponent(entityId, "Transform")
                local ai        = ecs:getComponent(entityId, "EnemyAI")

                if boss.defeated then goto nextBoss end
                if handleIntro(boss, emitter, dt) then goto nextBoss end

                handlePhaseCheck(ecs, entityId, boss, health, emitter)
                handlePatternCycling(boss, emitter, dt)
                handleMinionSpawn(ecs, entityId, boss, transform, dt)
                handleMovement(ecs, entityId, boss, transform, ai, health, dt)

                ::nextBoss::
            end
        end
    )

    return BossSystem
end

return createBossSystem
