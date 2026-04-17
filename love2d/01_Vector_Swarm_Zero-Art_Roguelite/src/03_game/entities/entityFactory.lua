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

local EntityFactory = {}

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

    logInfo(string.format("[ENTITY] Player created: %d", entityId))
    return entityId
end

-- 적 엔티티 생성
function EntityFactory.createEnemy(world, x, y, enemyType)
    local entityId = world:createEntity()

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 5,
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        vx = 0, vy = -1, speed = 1, maxSpeed = 3, damping = 1.0,
    }))

    world:addComponent(entityId, "Collider", Collider.new({
        radius = 0.15, layer = "enemy",
        mask = {"player", "playerBullet"},
    }))

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = "circle", radius = 0.15,
        color = {1, 0.2, 0.2, 1},
    }))

    world:addComponent(entityId, "LifeSpan", LifeSpan.new({
        time = 30, destroyOffScreen = true,
    }))

    world:addComponent(entityId, "BulletEmitter", BulletEmitter.new({
        pattern     = "circle",
        emitRate    = 1,          -- 1 burst per second
        bulletSpeed = 1.5,
        bulletCount = 8,
        bulletLifetime = 4,
        bulletRadius   = 0.04,
        bulletColor    = {1, 0.4, 0.4, 1},  -- reddish
    }))

    logInfo(string.format("[ENTITY] Enemy created: %d", entityId))
    return entityId
end

return EntityFactory
