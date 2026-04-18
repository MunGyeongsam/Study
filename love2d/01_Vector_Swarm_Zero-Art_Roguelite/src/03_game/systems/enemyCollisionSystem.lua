-- Enemy Collision System
-- Checks enemy entities (EnemyAI + Transform + Collider + Health)
-- against player_bullet layer bullets in the shared BulletPool.
-- On hit: recycle bullet, apply damage, destroy enemy if HP <= 0.

local System = require("01_core.system")

local cos = math.cos
local sin = math.sin
local pi2 = math.pi * 2
local random = math.random

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

local function createEnemyCollisionSystem(bulletPool, onEnemyDeath)

    local EnemyCollisionSystem = System.new("EnemyCollision", {"EnemyAI", "Transform", "Collider", "Health"},
        function(ecs, dt, entities)
            if bulletPool.activeCount == 0 then return end

            -- Collect player bullet indices for this frame
            -- (avoid checking enemy bullets against enemies)
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

                    -- Only check player bullets
                    if b.layer ~= "player_bullet" then
                        i = i + 1
                        goto nextBullet
                    end

                    local dx = b.x - ex
                    local dy = b.y - ey
                    local dist2 = dx * dx + dy * dy
                    local minDist = eRadius + b.radius

                    if dist2 < minDist * minDist then
                        local dmg = b.damage or 1
                        bulletPool:_recycle(i)

                        health.hp = health.hp - dmg
                        health.hitCount = health.hitCount + 1

                        if health.hp <= 0 then
                            health.alive = false
                            -- Boss: drop multiple XP orbs in burst pattern
                            local bossTag = ecs:getComponent(entityId, "BossTag")
                            if bossTag and onEnemyDeath then
                                local orbCount = 25
                                local xpPerOrb = math.floor((bossTag and ecs:getComponent(entityId, "EnemyAI").xpValue or 50) / orbCount + 0.5)
                                xpPerOrb = math.max(1, xpPerOrb)
                                for o = 1, orbCount do
                                    local angle = (o / orbCount) * math.pi * 2
                                    local dist = 0.3 + math.random() * 0.5
                                    onEnemyDeath(ecs, ex + math.cos(angle) * dist, ey + math.sin(angle) * dist, xpPerOrb)
                                end
                                -- Mark defeated (BossSystem reads this)
                                bossTag.defeated = true
                                -- 보스 사망 debris (큰 파편)
                                local rend = ecs:getComponent(entityId, "Renderable")
                                if rend then
                                    spawnDeathDebris(bulletPool, ex, ey, rend.color, eRadius)
                                end
                                -- Don't destroyEntity — let StageManager handle cleanup
                                goto nextEnemy
                            end
                            -- Normal enemy: debris + XP drop + destroy
                            local rend = ecs:getComponent(entityId, "Renderable")
                            if rend then
                                spawnDeathDebris(bulletPool, ex, ey, rend.color, eRadius)
                            end
                            if onEnemyDeath then
                                local enemyAI = ecs:getComponent(entityId, "EnemyAI")
                                onEnemyDeath(ecs, ex, ey, enemyAI and enemyAI.xpValue or 1)
                            end
                            ecs:destroyEntity(entityId)
                            goto nextEnemy
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
