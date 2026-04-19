-- Collision System
-- Checks player entities (PlayerTag + Transform + Collider + Health)
-- against all active bullets in the shared BulletPool.
--
-- Circle-circle distance check: dist² < (playerRadius + bulletRadius)²
-- On hit: recycle bullet, apply damage, start iFrames.
-- Near-miss (graze): onGraze callback for reward/effect (Strategy pattern).

local System = require("01_core.system")

local GRAZE_MULT = 3.0  -- graze radius = collider radius × GRAZE_MULT

local function createCollisionSystem(bulletPool, callbacks)
    callbacks = callbacks or {}
    local onGraze = callbacks.onGraze  -- function(entityId, bulletX, bulletY) or nil

    local CollisionSystem = System.new("Collision", {"PlayerTag", "Transform", "Collider", "Health"},
        function(ecs, dt, entities)
            -- Tick iFrame timers (even when no bullets exist)
            for _, entityId in ipairs(entities) do
                local health = ecs:getComponent(entityId, "Health")
                if health.iTimer > 0 then
                    health.iTimer = health.iTimer - dt
                    if health.iTimer < 0 then health.iTimer = 0 end
                end
            end

            if bulletPool.activeCount == 0 then return end

            for _, entityId in ipairs(entities) do
                local health = ecs:getComponent(entityId, "Health")
                if not health.alive then goto nextEntity end
                if health.iTimer > 0 then goto nextEntity end  -- invincible

                local transform = ecs:getComponent(entityId, "Transform")
                local collider  = ecs:getComponent(entityId, "Collider")
                local px, py    = transform.x, transform.y
                local pRadius   = collider.radius
                local grazeR2   = (pRadius * GRAZE_MULT) ^ 2

                -- Check all active enemy bullets
                local i = 1
                while i <= bulletPool.activeCount do
                    local b = bulletPool.active[i]

                    -- Only check enemy bullets
                    if b.layer ~= "enemy_bullet" then
                        i = i + 1
                        goto nextBullet
                    end

                    local dx = b.x - px
                    local dy = b.y - py
                    local dist2 = dx * dx + dy * dy
                    local minDist = pRadius + b.radius
                    
                    if dist2 < minDist * minDist then
                        -- Hit! Recycle bullet
                        bulletPool:_recycle(i)
                        -- Don't increment i (swap-remove)

                        -- Apply damage
                        health.hp = health.hp - 1
                        health.hitCount = health.hitCount + 1
                        health.iTimer = health.iFrames

                        if health.hp <= 0 then
                            health.alive = false
                            screenShake(0.25, 0.4)
                            logInfo("[COLLISION] Player destroyed!")
                        else
                            screenShake(0.08, 0.15)
                            logInfo(string.format("[COLLISION] Player hit! HP: %d/%d", health.hp, health.maxHp))
                        end

                        if playSound then playSound("player_hit") end

                        -- Only one hit per frame (iFrames start immediately)
                        break
                    else
                        -- Graze check: near-miss, not yet grazed
                        if onGraze and not b.grazed and dist2 < grazeR2 then
                            b.grazed = true
                            onGraze(entityId, b.x, b.y)
                        end
                        i = i + 1
                    end

                    ::nextBullet::
                end

                ::nextEntity::
            end
        end
    )

    return CollisionSystem
end

return createCollisionSystem
