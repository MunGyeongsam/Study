-- Player Weapon System
-- Auto-fires toward the nearest enemy within range.
-- Reads PlayerWeapon + Transform from player, queries EnemyAI entities for targeting.

local System = require("01_core.system")

local sqrt = math.sqrt
local atan2 = math.atan2
local cos = math.cos
local sin = math.sin
local pi2 = math.pi * 2

local function createPlayerWeaponSystem(bulletPool)

    local PlayerWeaponSystem = System.new("PlayerWeapon", {"PlayerTag", "Transform", "PlayerWeapon", "Health"},
        function(ecs, dt, entities)
            -- Cache enemy positions once per frame
            local enemies = ecs:queryEntities({"EnemyAI", "Transform"})

            for _, entityId in ipairs(entities) do
                local health = ecs:getComponent(entityId, "Health")
                if not health.alive then goto nextPlayer end

                local weapon    = ecs:getComponent(entityId, "PlayerWeapon")
                local transform = ecs:getComponent(entityId, "Transform")
                local px, py    = transform.x, transform.y

                weapon.timer = weapon.timer + dt
                local interval = 1.0 / weapon.fireRate

                if weapon.timer < interval then goto nextPlayer end
                weapon.timer = weapon.timer - interval

                -- Find nearest enemy within range
                local nearestId = nil
                local nearestDist2 = weapon.range * weapon.range
                for _, enemyId in ipairs(enemies) do
                    local et = ecs:getComponent(enemyId, "Transform")
                    local dx = et.x - px
                    local dy = et.y - py
                    local d2 = dx * dx + dy * dy
                    if d2 < nearestDist2 then
                        nearestDist2 = d2
                        nearestId = enemyId
                    end
                end

                if not nearestId then goto nextPlayer end

                -- Aim at nearest enemy
                local et = ecs:getComponent(nearestId, "Transform")
                local dx = et.x - px
                local dy = et.y - py
                local dist = sqrt(dx * dx + dy * dy)
                if dist < 0.01 then goto nextPlayer end
                local dirX, dirY = dx / dist, dy / dist

                local speed = weapon.bulletSpeed
                local opts = {
                    maxLifetime = weapon.bulletLifetime,
                    radius      = weapon.bulletRadius,
                    color       = weapon.bulletColor,
                    layer       = "player_bullet",
                    damage      = weapon.bulletDamage,
                }

                local count = weapon.bulletCount
                if count <= 1 then
                    bulletPool:spawn(px, py, dirX * speed, dirY * speed, opts)
                else
                    -- Spread pattern
                    local spread = 0.2
                    local baseAngle = atan2(dirY, dirX)
                    for i = 0, count - 1 do
                        local offset = -spread / 2 + (i / (count - 1)) * spread
                        local a = baseAngle + offset
                        bulletPool:spawn(px, py, cos(a) * speed, sin(a) * speed, opts)
                    end
                end

                if playSound then playSound("player_shoot") end

                ::nextPlayer::
            end
        end
    )

    return PlayerWeaponSystem
end

return createPlayerWeaponSystem
