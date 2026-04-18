-- Entity Factory
-- 엔티티 생성 헬퍼 함수 모음

local Transform     = require("03_game.components.transform")
local Velocity      = require("03_game.components.velocity")
local Collider      = require("03_game.components.collider")
local Renderable    = require("03_game.components.renderable")
local LifeSpan      = require("03_game.components.lifespan")
local Input         = require("03_game.components.input")
local PlayerTag     = require("03_game.components.playerTag")
local WorldBound    = require("03_game.components.worldBound")
local Health        = require("03_game.components.health")
local BulletEmitter = require("03_game.components.bulletEmitter")
local EnemyAI       = require("03_game.components.enemyAI")
local PlayerWeapon  = require("03_game.components.playerWeapon")
local Dash          = require("03_game.components.dash")
local Focus         = require("03_game.components.focus")
local PlayerXP      = require("03_game.components.playerXP")
local XpOrb         = require("03_game.components.xpOrb")
local BossTag       = require("03_game.components.bossTag")

local EntityFactory = {}

-- Enemy type presets
local ENEMY_TYPES = {
    basic = {
        color       = {1, 0.2, 0.2, 1},
        radius      = 0.15,
        hp          = 3,
        xpValue     = 2,
        ai          = { behavior = "drift", driftVx = 0, driftVy = -0.3 },
        emitter     = { pattern = "circle", emitRate = 0.8, bulletSpeed = 1.2, bulletCount = 6,
                        bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1, 0.4, 0.4, 1} },
    },
    spiral = {
        color       = {0.8, 0.2, 1, 1},
        radius      = 0.18,
        hp          = 5,
        xpValue     = 5,
        ai          = { behavior = "orbit", orbitRadius = 1.5, orbitSpeed = 0.8, speed = 1.2 },
        emitter     = { pattern = "spiral", emitRate = 1.5, bulletSpeed = 1.0, bulletCount = 4,
                        bulletLifetime = 5, bulletRadius = 0.03, bulletColor = {0.8, 0.4, 1, 1},
                        turnRate = 2.0 },
    },
    aimed = {
        color       = {1, 0.8, 0.1, 1},
        radius      = 0.12,
        hp          = 2,
        xpValue     = 3,
        ai          = { behavior = "chase", chaseSpeed = 0.4 },
        emitter     = { pattern = "aimed", emitRate = 1.2, bulletSpeed = 2.0, bulletCount = 3,
                        bulletLifetime = 3, bulletRadius = 0.035, bulletColor = {1, 0.9, 0.3, 1} },
    },
    wave = {
        color       = {0.2, 1, 0.4, 1},
        radius      = 0.14,
        hp          = 4,
        xpValue     = 3,
        ai          = { behavior = "drift", driftVx = 0.3, driftVy = -0.2 },
        emitter     = { pattern = "wave", emitRate = 1.0, bulletSpeed = 1.5, bulletCount = 5,
                        bulletLifetime = 4, bulletRadius = 0.035, bulletColor = {0.4, 1, 0.6, 1},
                        turnRate = 1.8 },
    },
}

-- 플레이어 엔티티 생성
function EntityFactory.createPlayer(world, x, y)
    local entityId = world:createEntity()

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 0,
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        speed = 2, maxSpeed = 5, damping = 1.0,  -- InputSystem이 매 프레임 직접 설정
    }))

    world:addComponent(entityId, "Collider", Collider.new({
        radius = 0.1, layer = "player",
        mask = {"enemy", "bullet", "powerup"},
    }))

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = "circle", radius = 0.1,
        color = {0, 1, 1, 1},
    }))

    -- 플레이어 전용 컴포넌트
    world:addComponent(entityId, "Input", Input.new())
    world:addComponent(entityId, "PlayerTag", PlayerTag.new())
    world:addComponent(entityId, "WorldBound", WorldBound.new())
    world:addComponent(entityId, "Health", Health.new({ hp = 5, maxHp = 5, iFrames = 1.5 }))
    world:addComponent(entityId, "PlayerWeapon", PlayerWeapon.new({
        fireRate = 4, bulletSpeed = 4, bulletCount = 1, range = 6,
    }))
    world:addComponent(entityId, "Dash", Dash.new({
        distance = 2.0, cooldown = 3.0, iFrames = 0.3,
    }))
    world:addComponent(entityId, "Focus", Focus.new())
    world:addComponent(entityId, "PlayerXP", PlayerXP.new())

    logInfo(string.format("[ENTITY] Player created: %d", entityId))
    return entityId
end

-- 적 엔티티 생성 (difficulty: optional scaling from stageManager)
function EntityFactory.createEnemy(world, x, y, enemyType, difficulty)
    local preset = ENEMY_TYPES[enemyType] or ENEMY_TYPES.basic
    local diff = difficulty or {}
    local hpMult = diff.enemyHpMult or 1.0
    local spdMult = diff.enemySpeedMult or 1.0
    local bspdMult = diff.bulletSpeedMult or 1.0
    local entityId = world:createEntity()

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 5,
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        vx = 0, vy = 0, speed = (preset.ai.speed or 1) * spdMult, maxSpeed = 3 * spdMult, damping = 1.0,
    }))

    world:addComponent(entityId, "Collider", Collider.new({
        radius = preset.radius, layer = "enemy",
        mask = {"player", "playerBullet"},
    }))

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = "circle", radius = preset.radius,
        color = preset.color,
    }))

    world:addComponent(entityId, "LifeSpan", LifeSpan.new({
        time = 15, destroyOffScreen = true,
    }))

    local scaledHp = math.floor(preset.hp * hpMult + 0.5)
    world:addComponent(entityId, "EnemyAI", EnemyAI.new({
        behavior     = preset.ai.behavior,
        speed        = (preset.ai.speed or 1) * spdMult,
        orbitRadius  = preset.ai.orbitRadius,
        orbitSpeed   = preset.ai.orbitSpeed and (preset.ai.orbitSpeed * spdMult),
        chaseSpeed   = preset.ai.chaseSpeed and (preset.ai.chaseSpeed * spdMult),
        driftVx      = preset.ai.driftVx and (preset.ai.driftVx * spdMult),
        driftVy      = preset.ai.driftVy and (preset.ai.driftVy * spdMult),
        xpValue      = preset.xpValue or 1,
    }))
    world:addComponent(entityId, "BulletEmitter", BulletEmitter.new({
        pattern        = preset.emitter.pattern,
        emitRate       = preset.emitter.emitRate,
        bulletSpeed    = preset.emitter.bulletSpeed * bspdMult,
        bulletCount    = preset.emitter.bulletCount,
        bulletLifetime = preset.emitter.bulletLifetime,
        bulletRadius   = preset.emitter.bulletRadius,
        bulletColor    = preset.emitter.bulletColor,
        turnRate       = preset.emitter.turnRate,
    }))
    world:addComponent(entityId, "Health", Health.new({
        hp = scaledHp, maxHp = scaledHp, iFrames = 0,
    }))

    -- Set orbit center to spawn position
    if preset.ai.behavior == "orbit" then
        local ai = world:getComponent(entityId, "EnemyAI")
        ai.orbitCenterX = x or 0
        ai.orbitCenterY = y or 5
    end

    logInfo(string.format("[ENTITY] Enemy created: %d (%s)", entityId, enemyType or "basic"))
    return entityId
end

-- XP 오브 엔티티 생성
function EntityFactory.createXpOrb(world, x, y, value)
    local entityId = world:createEntity()

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 0,
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        speed = 0, maxSpeed = 10, damping = 1.0,
    }))

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = "circle", radius = 0.05,
        color = {0.2, 1.0, 0.4, 1},  -- 초록색 XP 오브
    }))

    world:addComponent(entityId, "LifeSpan", LifeSpan.new({
        maxLifetime = 15,  -- 15초 후 소멸
    }))

    world:addComponent(entityId, "XpOrb", XpOrb.new({
        value = value or 1,
    }))

    return entityId
end

-- ===== Boss Type Presets =====
local BOSS_TYPES = {
    NULL = {
        color    = {0.6, 0.6, 0.6, 1},
        radius   = 0.8,
        hp       = 50,
        xpValue  = 50,
        maxPhase = 2,
        phaseThresholds = {0.5},
        ai       = { behavior = "drift", driftVx = 0.4, driftVy = 0, speed = 0.4 },
        patterns = {
            -- Phase 1: learn
            [1] = {
                { pattern = "circle", emitRate = 0.4, bulletSpeed = 2.0, bulletCount = 8,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.7, 0.7, 0.7, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 2.0 },
                { pattern = "circle", emitRate = 0.5, bulletSpeed = 2.5, bulletCount = 12,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.7, 0.7, 0.7, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: serious
            [2] = {
                { pattern = "circle", emitRate = 0.6, bulletSpeed = 2.5, bulletCount = 8,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.9, 0.5, 0.5, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 1.5 },
                { pattern = "spiral", emitRate = 1.0, bulletSpeed = 2.0, bulletCount = 4,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.9, 0.5, 0.5, 1},
                  turnRate = 1.0, duration = 4.0 },
                { pattern = "none", duration = 1.5 },
            },
        },
    },
    STACK = {
        color    = {0.2, 0.8, 0.2, 1},
        radius   = 1.0,
        hp       = 100,
        xpValue  = 120,
        maxPhase = 3,
        phaseThresholds = {0.66, 0.33},
        ai       = { behavior = "drift", driftVx = 0.3, driftVy = 0, speed = 0.3 },
        patterns = {
            -- Phase 1: slow concentric rings
            [1] = {
                { pattern = "circle", emitRate = 0.4, bulletSpeed = 1.8, bulletCount = 10,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.2, 0.9, 0.2, 1},
                  duration = 3.5 },
                { pattern = "none", duration = 2.0 },
                { pattern = "circle", emitRate = 0.5, bulletSpeed = 2.2, bulletCount = 14,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.2, 0.9, 0.2, 1},
                  duration = 3.5 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: rings + aimed
            [2] = {
                { pattern = "circle", emitRate = 0.6, bulletSpeed = 2.2, bulletCount = 12,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.4, 1.0, 0.4, 1},
                  duration = 3.0 },
                { pattern = "aimed", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 3,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 1.0, 0.3, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 3: dense rings + aimed burst
            [3] = {
                { pattern = "circle", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 16,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.6, 1.0, 0.6, 1},
                  duration = 3.0 },
                { pattern = "aimed", emitRate = 1.2, bulletSpeed = 3.5, bulletCount = 5,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 0.3, 1},
                  duration = 2.5 },
                { pattern = "none", duration = 1.0 },
            },
        },
    },
    HEAP = {
        color    = {0.9, 0.3, 0.1, 1},
        radius   = 0.9,
        hp       = 160,
        xpValue  = 200,
        maxPhase = 3,
        phaseThresholds = {0.66, 0.33},
        ai       = { behavior = "drift", driftVx = 0.3, driftVy = 0, speed = 0.3 },
        -- Teleport params (consumed by bossSystem)
        teleportInterval = 5.0,
        teleportWarning  = 1.0,
        teleportCooldown = 0.3,
        patterns = {
            -- Phase 1: random-feel burst (fast circle, short pause, repeat)
            [1] = {
                { pattern = "circle", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 10,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.9, 0.4, 0.2, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.0 },
                { pattern = "circle", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 6,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 0.2, 1},
                  duration = 1.5 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 2: burst + wave (chaotic mix)
            [2] = {
                { pattern = "circle", emitRate = 0.9, bulletSpeed = 2.8, bulletCount = 12,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.4, 0.1, 1},
                  duration = 2.5 },
                { pattern = "wave", emitRate = 0.6, bulletSpeed = 2.5, bulletCount = 5,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {1.0, 0.6, 0.2, 1},
                  turnRate = 2.0, duration = 3.0 },
                { pattern = "none", duration = 1.0 },
            },
            -- Phase 3: dense spread + aimed (full chaos)
            [3] = {
                { pattern = "circle", emitRate = 1.2, bulletSpeed = 3.0, bulletCount = 18,
                  bulletLifetime = 4, bulletRadius = 0.035, bulletColor = {1.0, 0.3, 0.1, 1},
                  duration = 2.5 },
                { pattern = "aimed", emitRate = 1.5, bulletSpeed = 3.5, bulletCount = 4,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.8, 0.2, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 0.8 },
            },
        },
    },
    RECURSION = {
        color    = {0.5, 0.1, 0.9, 1},
        radius   = 0.7,
        hp       = 200,
        xpValue  = 300,
        maxPhase = 3,
        phaseThresholds = {0.66, 0.33},
        ai       = { behavior = "orbit", speed = 0.5, orbitRadius = 3.5, orbitSpeed = 0.8 },
        -- Minion spawning params (consumed by bossSystem)
        minion = {
            [1] = { max = 0, interval = 0,   type = "basic", hpMult = 0.5 },
            [2] = { max = 3, interval = 8.0, type = "basic", hpMult = 0.5 },
            [3] = { max = 5, interval = 6.0, type = "basic", hpMult = 0.5 },
        },
        patterns = {
            -- Phase 1: slow spiral (learn the orbit movement)
            [1] = {
                { pattern = "spiral", emitRate = 0.6, bulletSpeed = 2.0, bulletCount = 4,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.6, 0.2, 1.0, 1},
                  turnRate = 1.2, duration = 4.0 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: faster spiral (+ minions provide circle pressure)
            [2] = {
                { pattern = "spiral", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 6,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.7, 0.2, 1.0, 1},
                  turnRate = 1.5, duration = 3.5 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 3: fast spiral + aimed bursts (+ more minions)
            [3] = {
                { pattern = "spiral", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 8,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.8, 0.3, 1.0, 1},
                  turnRate = 2.0, duration = 3.0 },
                { pattern = "aimed", emitRate = 1.2, bulletSpeed = 3.5, bulletCount = 3,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 1.0, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.0 },
            },
        },
    },
}
function EntityFactory.createBoss(world, x, y, bossType)
    local preset = BOSS_TYPES[bossType] or BOSS_TYPES.NULL
    local entityId = world:createEntity()

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 5,
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        vx = 0, vy = 0, speed = preset.ai.speed or 0.4, maxSpeed = 2, damping = 1.0,
    }))

    world:addComponent(entityId, "Collider", Collider.new({
        radius = preset.radius, layer = "enemy",
        mask = {"player", "playerBullet"},
    }))

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = "circle", radius = preset.radius,
        color = preset.color,
    }))

    world:addComponent(entityId, "EnemyAI", EnemyAI.new({
        behavior    = preset.ai.behavior,
        speed       = preset.ai.speed or 0.4,
        driftVx     = preset.ai.driftVx or 0,
        driftVy     = preset.ai.driftVy or 0,
        orbitRadius = preset.ai.orbitRadius or 3.5,
        orbitSpeed  = preset.ai.orbitSpeed or 0.8,
        xpValue     = preset.xpValue or 50,
    }))

    world:addComponent(entityId, "Health", Health.new({
        hp = preset.hp, maxHp = preset.hp, iFrames = 0,
    }))

    -- Start with emitter inactive — BossSystem controls pattern switching
    local firstPattern = preset.patterns[1] and preset.patterns[1][1] or {}
    world:addComponent(entityId, "BulletEmitter", BulletEmitter.new({
        pattern        = firstPattern.pattern or "circle",
        emitRate       = firstPattern.emitRate or 0.5,
        bulletSpeed    = firstPattern.bulletSpeed or 2.0,
        bulletCount    = firstPattern.bulletCount or 8,
        bulletLifetime = firstPattern.bulletLifetime or 4,
        bulletRadius   = firstPattern.bulletRadius or 0.04,
        bulletColor    = firstPattern.bulletColor or {0.7, 0.7, 0.7, 1},
        turnRate       = firstPattern.turnRate or 1.5,
        active         = false,  -- BossSystem activates after intro
    }))

    world:addComponent(entityId, "BossTag", BossTag.new({
        bossType         = bossType or "NULL",
        maxPhase         = preset.maxPhase,
        phaseThresholds  = preset.phaseThresholds,
        patterns         = preset.patterns,
        teleportInterval = preset.teleportInterval or 0,
        teleportWarning  = preset.teleportWarning or 1.0,
        teleportCooldown = preset.teleportCooldown or 0.3,
        minion           = preset.minion or nil,
    }))

    logInfo(string.format("[ENTITY] Boss created: %d (%s) HP:%d", entityId, bossType or "NULL", preset.hp))
    return entityId
end

return EntityFactory
