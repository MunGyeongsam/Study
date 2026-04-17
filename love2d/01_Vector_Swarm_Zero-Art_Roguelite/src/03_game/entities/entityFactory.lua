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

return EntityFactory
