-- ============================================================================
-- entityFactory.lua — 엔티티 팩토리 (플레이어/적/보스/XP오브/DNA적)
-- ============================================================================
--
-- ◆ 역할
--   ECS 엔티티를 생성하고 컴포넌트를 조립한다.
--   적 타입 5종(bit/node/vector/loop/matrix) + 변형 4종(swift/armored/splitter/shielded)
--
-- ◆ 핵심 API
--   createPlayer(world, x, y, charId) → id
--   createEnemy(world, x, y, type, difficulty, variant) → id
--   createBoss(world, x, y, type, scaling) → id
--   createDnaEnemy(world, x, y, dna, difficulty) → id
--   createXpOrb(world, x, y, value) → id
--
-- ◆ 새 적 추가: 여기 + stageData.lua + basicShapes.lua
-- ◆ 새 변형 추가: 여기 + stageData.lua + variantOverlays.lua

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
local BOSS_TYPES    = require("03_game.data.bossDefs")
local ShapeDefs     = require("03_game.data.shapeDefs")

local _floor = math.floor
local _random = math.random

local EntityFactory = {}

local function pickCuratedCurve(role)
    local groups = ShapeDefs.groups or {}
    local pool = groups[role]
    if not pool or #pool == 0 then
        pool = groups.both
    end
    if not pool or #pool == 0 then
        pool = groups.enemy
    end
    if not pool or #pool == 0 then
        return nil
    end
    return pool[_random(#pool)]
end

-- ─── Player defaults ─────────────────────────────────────────────
local PLAYER = {
    speed    = 2,   maxSpeed  = 5,   damping = 1.0,
    radius   = 0.1, color     = {0, 1, 1, 1},
    hp       = 5,   iFrames   = 1.5,
    fireRate = 4,   bulletSpeed = 4, bulletCount = 1, range = 6,
    dashDist = 2.0, dashCooldown = 3.0, dashIFrames = 0.3,
}

-- ─── Character Presets (해금 시스템) ─────────────────────────────
local CHARACTER_PRESETS = {
    default = PLAYER,
    debugger = {
        speed    = 2.5, maxSpeed  = 6,   damping = 1.0,
        radius   = 0.06, color    = {0.4, 1, 0.4, 1},
        hp       = 3,   iFrames   = 1.0,
        fireRate = 5,   bulletSpeed = 5, bulletCount = 1, range = 6,
        dashDist = 2.5, dashCooldown = 2.5, dashIFrames = 0.3,
    },
    compiler = {
        speed    = 1.5, maxSpeed  = 3.5, damping = 1.0,
        radius   = 0.15, color    = {1, 0.6, 0.2, 1},
        hp       = 8,   iFrames   = 2.0,
        fireRate = 3,   bulletSpeed = 3.5, bulletCount = 1, range = 7,
        dashDist = 1.5, dashCooldown = 4.0, dashIFrames = 0.4,
    },
}

-- ─── Enemy max speed (scaled by difficulty) ──────────────────────
local ENEMY_MAX_SPEED = 3

-- ─── Variant Definitions ─────────────────────────────────────────
-- Each variant modifies base stats multiplicatively and sets a visual tag.
local VARIANT_DEFS = {
    swift = {
        speedMult  = 1.5,
        hpMult     = 0.8,
        scaleMult  = 0.8,
        xpMult     = 1.2,
    },
    armored = {
        speedMult  = 0.7,
        hpMult     = 2.5,
        scaleMult  = 1.3,
        xpMult     = 1.8,
    },
    splitter = {
        speedMult  = 1.0,
        hpMult     = 1.0,
        scaleMult  = 1.0,
        xpMult     = 1.3,
    },
    shielded = {
        speedMult  = 0.85,
        hpMult     = 1.5,
        scaleMult  = 1.1,
        xpMult     = 1.5,
        shieldArc  = 1.5708,  -- pi/2 = 90 degrees (±45° from facing)
    },
}

-- Enemy type presets (세계관 기반 6종 — Worm 제외 5종 구현)
local ENEMY_TYPES = {
    -- Bit: 접촉형 무리. 탄막 없이 몸통으로 돌진
    bit = {
        color       = {1.0, 1.0, 1.0, 1},
        radius      = 0.08,
        hp          = 1,
        xpValue     = 1,
        shape       = "circle",
        ai          = { behavior = "swarm", swarmSpeed = 0.8 },
        emitter     = nil,  -- 탄막 없음
    },
    -- Node: 정지 포탑. 맥동하는 원형 탄막 (타이밍 퍼즐)
    node = {
        color       = {1.0, 0.3, 0.2, 1},
        radius      = 0.15,
        hp          = 4,
        xpValue     = 3,
        shape       = "diamond",
        ai          = { behavior = "stationary", spinSpeed = 1.0 },
        emitter     = { pattern = "ring_pulse", emitRate = 1.2, bulletSpeed = 1.2, bulletCount = 8,
                        bulletLifetime = 3.5, bulletRadius = 0.04, bulletColor = {1, 0.4, 0.4, 1},
                        turnRate = 1.5 },
    },
    -- Vector: 경고 후 고속 직선 돌진. 탄막 없음
    vector = {
        color       = {1.0, 0.9, 0.1, 1},
        radius      = 0.12,
        hp          = 2,
        xpValue     = 4,
        shape       = "arrow",
        ai          = { behavior = "charge", chargeSpeed = 4.0, chargeWarnTime = 0.8 },
        emitter     = nil,  -- 몸통 돌진이 위협
    },
    -- Loop: 원형 궤도 + 나선 탄막
    loop = {
        color       = {0.8, 0.3, 1.0, 1},
        radius      = 0.18,
        hp          = 5,
        xpValue     = 5,
        shape       = "spiral_ring",
        ai          = { behavior = "orbit", orbitRadius = 1.5, orbitSpeed = 0.8, speed = 1.2 },
        emitter     = { pattern = "spiral", emitRate = 1.5, bulletSpeed = 1.0, bulletCount = 4,
                        bulletLifetime = 5, bulletRadius = 0.03, bulletColor = {0.8, 0.4, 1, 1},
                        turnRate = 2.0 },
    },
    -- Matrix: 십자/대각 교대 탄막. 틈새 읽기
    matrix = {
        color       = {0.2, 1.0, 0.8, 1},
        radius      = 0.16,
        hp          = 6,
        xpValue     = 6,
        shape       = "hexagon",
        ai          = { behavior = "drift", driftVx = 0, driftVy = -0.15 },
        emitter     = { pattern = "cross", emitRate = 0.8, bulletSpeed = 1.3, bulletCount = 4,
                        bulletLifetime = 4, bulletRadius = 0.035, bulletColor = {0.3, 1, 0.9, 1} },
    },
    -- === Legacy aliases (보스/스테이지 호환) ===
    basic   = nil,  -- → node로 대체
    spiral  = nil,  -- → loop로 대체
    aimed   = nil,  -- → vector로 대체
    wave    = nil,  -- → matrix로 대체
}

-- 플레이어 엔티티 생성 (characterId: "default", "debugger", "compiler")
function EntityFactory.createPlayer(world, x, y, characterId)
    local P = CHARACTER_PRESETS[characterId or "default"] or PLAYER
    local entityId = world:createEntity()

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 0,
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        speed = P.speed, maxSpeed = P.maxSpeed, damping = P.damping,
    }))

    world:addComponent(entityId, "Collider", Collider.new({
        radius = P.radius, layer = "player",
        mask = {"enemy", "bullet", "powerup"},
    }))

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = "circle", radius = P.radius,
        color = P.color,
    }))

    -- 플레이어 전용 컴포넌트
    world:addComponent(entityId, "Input", Input.new())
    world:addComponent(entityId, "PlayerTag", PlayerTag.new())
    world:addComponent(entityId, "WorldBound", WorldBound.new())
    world:addComponent(entityId, "Health", Health.new({ hp = P.hp, maxHp = P.hp, iFrames = P.iFrames }))
    world:addComponent(entityId, "PlayerWeapon", PlayerWeapon.new({
        fireRate = P.fireRate, bulletSpeed = P.bulletSpeed,
        bulletCount = P.bulletCount, range = P.range,
    }))
    world:addComponent(entityId, "Dash", Dash.new({
        distance = P.dashDist, cooldown = P.dashCooldown, iFrames = P.dashIFrames,
    }))
    world:addComponent(entityId, "Focus", Focus.new())
    world:addComponent(entityId, "PlayerXP", PlayerXP.new())

    logInfo(string.format("[ENTITY] Player created: %d", entityId))
    return entityId
end

-- Scale a value by multiplier if it exists, otherwise nil
local function scaled(value, mult)
    if value then return value * mult end
    return nil
end

-- 적 엔티티 생성 (difficulty: optional scaling, variant: optional "swift"/"armored"/etc)
function EntityFactory.createEnemy(world, x, y, enemyType, difficulty, variant)
    local preset = ENEMY_TYPES[enemyType] or ENEMY_TYPES.node
    local diff = difficulty or {}
    local hpMult = diff.enemyHpMult or 1.0
    local spdMult = diff.enemySpeedMult or 1.0
    local bspdMult = diff.bulletSpeedMult or 1.0

    -- Apply variant multipliers on top of difficulty
    local vdef = variant and VARIANT_DEFS[variant]
    if vdef then
        spdMult = spdMult * (vdef.speedMult or 1)
        hpMult  = hpMult * (vdef.hpMult or 1)
    end
    local radiusMult = vdef and vdef.scaleMult or 1
    local xpMult     = vdef and vdef.xpMult or 1

    local entityId = world:createEntity()
    local radius = preset.radius * radiusMult

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 5,
    }))
    world:addComponent(entityId, "Velocity", Velocity.new({
        vx = 0, vy = 0, speed = (preset.ai.speed or 1) * spdMult, maxSpeed = ENEMY_MAX_SPEED * spdMult, damping = 1.0,
    }))
    world:addComponent(entityId, "Collider", Collider.new({
        radius = radius, layer = "enemy",
        mask = {"player", "playerBullet"},
    }))
    world:addComponent(entityId, "Renderable", Renderable.new({
        type = preset.shape or "circle", radius = radius,
        color = preset.color,
        variant = variant,  -- visual tag for renderSystem
    }))
    world:addComponent(entityId, "LifeSpan", LifeSpan.new({
        time = 15, destroyOffScreen = true,
    }))

    local scaledHp = _floor(preset.hp * hpMult + 0.5)
    local ai = preset.ai
    world:addComponent(entityId, "EnemyAI", EnemyAI.new({
        behavior       = ai.behavior,
        speed          = (ai.speed or 1) * spdMult,
        orbitRadius    = ai.orbitRadius,
        orbitSpeed     = scaled(ai.orbitSpeed, spdMult),
        chaseSpeed     = scaled(ai.chaseSpeed, spdMult),
        driftVx        = scaled(ai.driftVx, spdMult),
        driftVy        = scaled(ai.driftVy, spdMult),
        swarmSpeed     = scaled(ai.swarmSpeed, spdMult),
        chargeSpeed    = scaled(ai.chargeSpeed, spdMult),
        chargeWarnTime = ai.chargeWarnTime,
        spinSpeed      = ai.spinSpeed,
        xpValue        = _floor((preset.xpValue or 1) * xpMult + 0.5),
        variant        = variant,  -- for systems that need variant info
    }))

    if preset.emitter then
        local em = preset.emitter
        world:addComponent(entityId, "BulletEmitter", BulletEmitter.new({
            pattern        = em.pattern,
            emitRate       = em.emitRate,
            bulletSpeed    = em.bulletSpeed * bspdMult,
            bulletCount    = em.bulletCount,
            bulletLifetime = em.bulletLifetime,
            bulletRadius   = em.bulletRadius,
            bulletColor    = em.bulletColor,
            turnRate       = em.turnRate,
        }))
    end

    world:addComponent(entityId, "Health", Health.new({
        hp = scaledHp, maxHp = scaledHp, iFrames = 0,
    }))

    if ai.behavior == "orbit" then
        local aiComp = world:getComponent(entityId, "EnemyAI")
        aiComp.orbitCenterX = x or 0
        aiComp.orbitCenterY = y or 5
    end

    logInfo(string.format("[ENTITY] Enemy created: %d (%s%s)", entityId,
        variant and (variant .. "-") or "", enemyType or "node"))
    return entityId
end

-- ─── DNA 기반 적 엔티티 생성 (Stage 16+ Endless) ─────────────────
-- dna = dnaDefs.generateDna(round) 결과물
function EntityFactory.createDnaEnemy(world, x, y, dna, difficulty)
    local diff = difficulty or {}
    local bspdMult = diff.bulletSpeedMult or 1.0

    local stats = dna.stats
    local modifier = dna.modifier or "none"

    local curveRole = dna.curveRole or "enemy"
    local curveName = dna.curveName or pickCuratedCurve(curveRole)
    local curveNorm = curveName and ShapeDefs.getNormalization(curveName) or nil

    local entityId = world:createEntity()
    local radius = stats.radius

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 5,
    }))
    world:addComponent(entityId, "Velocity", Velocity.new({
        vx = 0, vy = 0, speed = stats.speed, maxSpeed = ENEMY_MAX_SPEED, damping = 1.0,
    }))
    world:addComponent(entityId, "Collider", Collider.new({
        radius = radius, layer = "enemy",
        mask = {"player", "playerBullet"},
    }))
    -- Body = 레이어 배열 (table) → renderSystem이 레이어별 렌더
    world:addComponent(entityId, "Renderable", Renderable.new({
        type = dna.body,  -- table: [{shape,mode,scale,rot}, ...]
        radius = radius,
        color = dna.color,
        variant = modifier ~= "none" and modifier or nil,
        curveName = curveName,
        curveRole = curveRole,
        curveNormalization = curveNorm,
    }))
    world:addComponent(entityId, "LifeSpan", LifeSpan.new({
        time = 15, destroyOffScreen = true,
    }))

    -- AI: movement 유전자 → behavior 매핑
    local aiParams = {
        behavior  = dna.movement,
        speed     = stats.speed,
        xpValue   = stats.xp,
        variant   = modifier ~= "none" and modifier or nil,
    }
    -- Movement별 추가 파라미터
    if dna.movement == "drift" then
        aiParams.driftVx = 0
        aiParams.driftVy = -stats.speed
    elseif dna.movement == "orbit" then
        aiParams.orbitRadius = 1.5
        aiParams.orbitSpeed  = stats.speed
    elseif dna.movement == "chase" then
        aiParams.chaseSpeed = stats.speed
    elseif dna.movement == "swarm" then
        aiParams.swarmSpeed = stats.speed
    elseif dna.movement == "charge" then
        aiParams.chargeSpeed  = stats.speed * 3
        aiParams.chargeWarnTime = 0.8
    elseif dna.movement == "stationary" then
        aiParams.spinSpeed = 1.0
    end
    world:addComponent(entityId, "EnemyAI", EnemyAI.new(aiParams))

    -- Attack: attack 유전자 → bulletEmitter
    if dna.attack ~= "none" then
        -- 기본 탄막 파라미터 (패턴별 미세 조정)
        local emitRate = 1.5
        local bulletSpeed = 1.5 * bspdMult
        local bulletCount = 4
        local bulletLifetime = 4.0
        local bulletRadius = 0.04
        local bulletColor = {1, 0.3, 0.3, 1}

        if dna.attack == "aimed" then
            bulletCount = 1; bulletSpeed = 2.0 * bspdMult
        elseif dna.attack == "spiral" then
            bulletCount = 4; emitRate = 2.0
        elseif dna.attack == "cross" then
            bulletCount = 4; emitRate = 1.2
        elseif dna.attack == "ring_pulse" then
            bulletCount = 8; emitRate = 1.0
        elseif dna.attack == "wave" then
            bulletCount = 3; emitRate = 1.5
        elseif dna.attack == "orbit_shot" then
            bulletCount = 3; emitRate = 1.0; bulletLifetime = 6.0
        elseif dna.attack == "return_shot" then
            bulletCount = 2; emitRate = 0.8; bulletLifetime = 5.0
        end

        world:addComponent(entityId, "BulletEmitter", BulletEmitter.new({
            pattern        = dna.attack,
            emitRate       = emitRate,
            bulletSpeed    = bulletSpeed,
            bulletCount    = bulletCount,
            bulletLifetime = bulletLifetime,
            bulletRadius   = bulletRadius,
            bulletColor    = bulletColor,
        }))
    end

    world:addComponent(entityId, "Health", Health.new({
        hp = stats.hp, maxHp = stats.hp, iFrames = 0,
    }))

    if dna.movement == "orbit" then
        local aiComp = world:getComponent(entityId, "EnemyAI")
        aiComp.orbitCenterX = x or 0
        aiComp.orbitCenterY = y or 5
    end

    logInfo(string.format("[DNA][SPAWN] entity:%d base:%s mv:%s atk:%s mod:%s body:%d curve:%s role:%s",
        entityId,
        dna.baseType,
        dna.movement,
        dna.attack,
        dna.modifier,
        #dna.body,
        curveName or "none",
        curveRole))
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
        color = {1.0, 0.85, 0.2, 1},  -- 금색 XP 오브 (탄환과 구분)
    }))

    world:addComponent(entityId, "LifeSpan", LifeSpan.new({
        time = 15,  -- 15초 후 소멸
    }))

    world:addComponent(entityId, "XpOrb", XpOrb.new({
        value = value or 1,
    }))

    return entityId
end

function EntityFactory.createBoss(world, x, y, bossType, scaling)
    local preset = BOSS_TYPES[bossType] or BOSS_TYPES.NULL
    local entityId = world:createEntity()

    -- Endless scaling: HP and speed multipliers
    local hpMult    = scaling and scaling.hpMult or 1
    local speedMult = scaling and scaling.speedMult or 1
    local bulletSpeedMult = scaling and scaling.bulletSpeedMult or 1
    local emitRateMult    = scaling and scaling.emitRateMult or 1
    local redShift  = scaling and scaling.redShift or 0
    local playerLvMult = scaling and scaling.playerLvMult or 1
    local scaledHp  = _floor(preset.hp * hpMult * playerLvMult)

    world:addComponent(entityId, "Transform", Transform.new({
        x = x or 0, y = y or 5, scale = 0,  -- scale 0: intro scale-in animation
    }))

    world:addComponent(entityId, "Velocity", Velocity.new({
        vx = 0, vy = 0, speed = (preset.ai.speed or 0.4) * speedMult, maxSpeed = 2 * speedMult, damping = 1.0,
    }))

    world:addComponent(entityId, "Collider", Collider.new({
        radius = preset.radius, layer = "enemy",
        mask = {"player", "playerBullet"},
    }))

    -- Endless color shift: blend toward red
    local baseColor = preset.color
    local bossColor = baseColor
    if redShift > 0 then
        bossColor = {
            baseColor[1] + (1.0 - baseColor[1]) * redShift,
            baseColor[2] * (1 - redShift),
            baseColor[3] * (1 - redShift),
            baseColor[4] or 1,
        }
    end

    world:addComponent(entityId, "Renderable", Renderable.new({
        type = preset.renderType or "circle", radius = preset.radius,
        color = bossColor,
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
        hp = scaledHp, maxHp = scaledHp, iFrames = 0,
    }))

    -- Start with emitter inactive — BossSystem controls pattern switching
    local firstPattern = preset.patterns[1] and preset.patterns[1][1] or {}
    world:addComponent(entityId, "BulletEmitter", BulletEmitter.new({
        pattern        = firstPattern.pattern or "circle",
        emitRate       = firstPattern.emitRate or 0.5,
        bulletSpeed    = (firstPattern.bulletSpeed or 2.0) * bulletSpeedMult,
        bulletCount    = firstPattern.bulletCount or 8,
        bulletLifetime = firstPattern.bulletLifetime or 4,
        bulletRadius   = firstPattern.bulletRadius or 0.04,
        bulletColor    = firstPattern.bulletColor or {0.7, 0.7, 0.7, 1},
        turnRate       = firstPattern.turnRate or 1.5,
        active         = false,  -- BossSystem activates after intro
    }))

    world:addComponent(entityId, "WorldBound", WorldBound.new({ enabled = true }))

    world:addComponent(entityId, "BossTag", BossTag.new({
        bossType         = bossType or "NULL",
        maxPhase         = preset.maxPhase,
        phaseThresholds  = preset.phaseThresholds,
        patterns         = preset.patterns,
        teleportInterval = preset.teleportInterval or 0,
        teleportWarning  = preset.teleportWarning or 1.0,
        teleportCooldown = preset.teleportCooldown or 0.3,
        minion           = preset.minion or nil,
        phaseTeleport    = preset.phaseTeleport or nil,
        phaseColors      = preset.phaseColors or nil,
        speedMult        = speedMult,
        bulletSpeedMult  = scaling and scaling.bulletSpeedMult or 1,
        emitRateMult     = scaling and scaling.emitRateMult or 1,
        minionAdd        = scaling and scaling.minionAdd or 0,
    }))

    local label = bossType or "NULL"
    if scaling and scaling.round then
        label = string.format("%s +%d", label, scaling.round)
    end
    logInfo(string.format("[ENTITY] Boss created: %d (%s) HP:%d", entityId, label, scaledHp))
    return entityId
end

return EntityFactory
