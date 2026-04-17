-- Collision System
-- Checks player entities (PlayerTag + Transform + Collider + Health)
-- against all active bullets in the shared BulletPool.
--
-- Circle-circle distance check: dist² < (playerRadius + bulletRadius)²
-- On hit: recycle bullet, apply damage, start iFrames.

local System = require("01_core.system")

local function createCollisionSystem(bulletPool)

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

                -- Check all active bullets
                local i = 1
                while i <= bulletPool.activeCount do
                    local b = bulletPool.active[i]
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
                            logInfo("[COLLISION] Player destroyed!")
                        else
                            logInfo(string.format("[COLLISION] Player hit! HP: %d/%d", health.hp, health.maxHp))
                        end

                        -- Only one hit per frame (iFrames start immediately)
                        break
                    else
                        i = i + 1
                    end
                end

                ::nextEntity::
            end
        end
    )

    return CollisionSystem
end

return createCollisionSystem
