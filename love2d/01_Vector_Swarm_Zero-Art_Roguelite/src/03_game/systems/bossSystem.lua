-- Boss System
-- Manages boss lifecycle: intro → active (phase/pattern cycling) → defeat.
-- Controls BulletEmitter pattern switching based on phase and timer.
-- Handles boss movement patterns per phase.

local System = require("01_core.system")
local worldMod = require("01_core.world")

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

    local BossSystem = System.new("Boss", {"Transform", "EnemyAI", "Health", "BossTag", "BulletEmitter"},
        function(ecs, dt, entities)
            for _, entityId in ipairs(entities) do
                local boss      = ecs:getComponent(entityId, "BossTag")
                local health    = ecs:getComponent(entityId, "Health")
                local emitter   = ecs:getComponent(entityId, "BulletEmitter")
                local transform = ecs:getComponent(entityId, "Transform")
                local ai        = ecs:getComponent(entityId, "EnemyAI")

                if boss.defeated then goto nextBoss end

                -- === INTRO PHASE ===
                if not boss.introComplete then
                    boss.introTimer = boss.introTimer + dt
                    emitter.active = false
                    if boss.introTimer >= boss.introDuration then
                        boss.introComplete = true
                        local phasePatterns = boss.patterns[boss.phase]
                        applyPattern(emitter, phasePatterns and phasePatterns[1])
                        logInfo(string.format("[BOSS] %s intro complete, phase %d active", boss.bossType, boss.phase))
                    end
                    goto nextBoss
                end

                -- === PHASE CHECK (HP threshold) ===
                boss.phaseChanged = false
                local hpRatio = health.hp / health.maxHp
                local threshold = boss.phaseThresholds[boss.phase]
                if threshold and hpRatio <= threshold and boss.phase < boss.maxPhase then
                    boss.phase = boss.phase + 1
                    boss.phaseChanged = true
                    boss.patternIndex = 1
                    boss.patternTimer = 0

                    -- Phase transition: clear bullets, brief invulnerability
                    if bulletPool then
                        bulletPool:clearLayer("enemy_bullet")
                    end
                    health.iTimer = 0.5

                    local phasePatterns = boss.patterns[boss.phase]
                    applyPattern(emitter, phasePatterns and phasePatterns[1])

                    logInfo(string.format("[BOSS] %s phase %d → %d (HP: %.0f%%)",
                        boss.bossType, boss.phase - 1, boss.phase, hpRatio * 100))
                end

                -- === PATTERN CYCLING (time-based within phase) ===
                boss.patternTimer = boss.patternTimer + dt
                local phasePatterns = boss.patterns[boss.phase]
                if phasePatterns then
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

                -- === BOSS MOVEMENT ===
                if ai then
                    local _, py = getPlayerPos()
                    local left, _, right, _ = worldMod.getBounds()
                    local margin = 2

                    -- Horizontal bounds ping-pong (shared by all bosses)
                    if transform.x >= right - margin then
                        ai.driftVx = -math.abs(ai.driftVx or 0.4)
                    elseif transform.x <= left + margin then
                        ai.driftVx = math.abs(ai.driftVx or 0.4)
                    end

                    -- Per-boss vertical behavior
                    local bossType = boss.bossType
                    if bossType == "NULL" then
                        -- Patrol upper area
                        local targetY = (py or 0) + 3.5
                        local yDiff = targetY - transform.y
                        ai.driftVy = yDiff > 0.1 and 0.3 or (yDiff < -0.1 and -0.3 or 0)
                    elseif bossType == "STACK" then
                        -- Patrol center-upper, slower vertical movement
                        local targetY = (py or 0) + 3.0
                        local yDiff = targetY - transform.y
                        ai.driftVy = yDiff > 0.1 and 0.2 or (yDiff < -0.1 and -0.2 or 0)
                    else
                        -- Default: stay above player
                        local targetY = (py or 0) + 3.5
                        local yDiff = targetY - transform.y
                        ai.driftVy = yDiff > 0.1 and 0.3 or (yDiff < -0.1 and -0.3 or 0)
                    end
                end

                ::nextBoss::
            end
        end
    )

    return BossSystem
end

return createBossSystem
