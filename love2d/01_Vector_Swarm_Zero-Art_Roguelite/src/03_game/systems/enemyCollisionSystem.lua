-- Enemy Collision System
-- Checks enemy entities (EnemyAI + Transform + Collider + Health)
-- against player_bullet layer bullets in the shared BulletPool.
-- On hit: recycle bullet, apply damage, destroy enemy if HP <= 0.

local System = require("01_core.system")

local cos    = math.cos
local sin    = math.sin
local floor  = math.floor
local max    = math.max
local random = math.random
local atan2  = math.atan2
local abs    = math.abs
local pi     = math.pi
local pi2    = math.pi * 2
local SHIELD_HALF_ARC = 0.7854  -- pi/4 = 45 degrees (total 90° arc)

-- 적 사망 시 파편 파티클 스폰
local function spawnDeathDebris(bulletPool, x, y, color, radius)
    local count = 6 + random(0, 2)  -- 6~8개
    local baseRadius = radius * 0.3
    for i = 1, count do
        local angle = (i / count) * pi2 + (random() - 0.5) * 0.4
        local speed = 2 + random() * 2  -- 2~4
        local r = baseRadius * (0.5 + random() * 0.7)  -- 크기 변화
        bulletPool:spawn(x, y, cos(angle) * speed, sin(angle) * speed, {
            maxLifetime = 0.3 + random() * 0.2,  -- 0.3~0.5초
            radius      = r,
            color       = {color[1], color[2], color[3], 1},
            layer       = "debris",
            damage      = 0,
            damping     = 0.02,    -- 빠르게 감속 (0.02^dt ≈ 매 초 98% 감소)
            fadeAlpha   = true,
        })
    end
end

-- 보스 사망 시 XP 오브 버스트 스폰
local function spawnBossXpBurst(ecs, entityId, ex, ey, onEnemyDeath)
    local bossTag = ecs:getComponent(entityId, "BossTag")
    local orbCount = 25
    local enemyAI = ecs:getComponent(entityId, "EnemyAI")
    local xpPerOrb = max(1, floor(((enemyAI and enemyAI.xpValue) or 50) / orbCount + 0.5))
    for o = 1, orbCount do
        local angle = (o / orbCount) * pi2
        local dist = 0.3 + random() * 0.5
        onEnemyDeath(ecs, ex + cos(angle) * dist, ey + sin(angle) * dist, xpPerOrb)
    end
    bossTag.defeated = true
end

local function createEnemyCollisionSystem(bulletPool, onEnemyDeath, onSpawnEnemy)

    local EnemyCollisionSystem = System.new("EnemyCollision", {"EnemyAI", "Transform", "Collider", "Health"},
        function(ecs, dt, entities)
            if bulletPool.activeCount == 0 then return end

            for _, entityId in ipairs(entities) do
                local health = ecs:getComponent(entityId, "Health")
                if not health.alive then goto nextEnemy end

                local transform = ecs:getComponent(entityId, "Transform")
                local collider  = ecs:getComponent(entityId, "Collider")
                local ex, ey    = transform.x, transform.y
                local eRadius   = collider.radius

                local i = 1
                while i <= bulletPool.activeCount do
                    local b = bulletPool.active[i]

                    if b.layer ~= "player_bullet" then
                        i = i + 1
                        goto nextBullet
                    end

                    local dx = b.x - ex
                    local dy = b.y - ey
                    local dist2 = dx * dx + dy * dy
                    local minDist = eRadius + b.radius

                    if dist2 < minDist * minDist then
                        -- Shielded: check if bullet hits the front arc
                        local enemyAI = ecs:getComponent(entityId, "EnemyAI")
                        if enemyAI and enemyAI.variant == "shielded" then
                            local vel = ecs:getComponent(entityId, "Velocity")
                            local fvx = vel and vel.vx or 0
                            local fvy = vel and vel.vy or 0
                            -- If stationary, face downward (toward player)
                            if fvx * fvx + fvy * fvy < 0.001 then
                                fvx, fvy = 0, -1
                            end
                            local facingAngle = atan2(fvy, fvx)
                            -- Angle from enemy to bullet (incoming direction)
                            local hitAngle = atan2(dy, dx)
                            local diff = hitAngle - facingAngle
                            -- Normalize to [-pi, pi]
                            if diff > pi then diff = diff - pi2
                            elseif diff < -pi then diff = diff + pi2 end
                            if abs(diff) < SHIELD_HALF_ARC then
                                -- Deflect: recycle bullet, no damage
                                bulletPool:_recycle(i)
                                if playSound then playSound("enemy_hit") end
                                goto nextBullet
                            end
                        end

                        local dmg = b.damage or 1
                        bulletPool:_recycle(i)

                        health.hp = health.hp - dmg
                        health.hitCount = health.hitCount + 1

                        if health.hp <= 0 then
                            health.alive = false
                            local rend = ecs:getComponent(entityId, "Renderable")

                            -- Boss death
                            if ecs:getComponent(entityId, "BossTag") and onEnemyDeath then
                                spawnBossXpBurst(ecs, entityId, ex, ey, onEnemyDeath)
                                if rend then
                                    spawnDeathDebris(bulletPool, ex, ey, rend.color, eRadius)
                                end
                                if playSound then playSound("enemy_kill") end
                                goto nextEnemy
                            end

                            -- Normal enemy death: debris + XP + destroy
                            local enemyAI = ecs:getComponent(entityId, "EnemyAI")
                            if rend then
                                spawnDeathDebris(bulletPool, ex, ey, rend.color, eRadius)
                            end
                            if onEnemyDeath then
                                onEnemyDeath(ecs, ex, ey, enemyAI and enemyAI.xpValue or 1)
                            end
                            if playSound then playSound("enemy_kill") end

                            -- Splitter: spawn 2 mini copies on death
                            if enemyAI and enemyAI.variant == "splitter" and onSpawnEnemy then
                                for s = 1, 2 do
                                    local offset = (s - 1.5) * eRadius * 1.5
                                    onSpawnEnemy(ex + offset, ey, enemyAI.enemyType, nil, nil, 0.5)
                                end
                            end

                            ecs:destroyEntity(entityId)
                            goto nextEnemy
                        else
                            if playSound then playSound("enemy_hit") end
                        end
                    else
                        i = i + 1
                    end

                    ::nextBullet::
                end

                ::nextEnemy::
            end
        end
    )

    return EnemyCollisionSystem
end

return createEnemyCollisionSystem
