-- Bullet Emitter System
-- Reads BulletEmitter + Transform components from ECS entities,
-- spawns bullets into the shared BulletPool.
--
-- This system receives bulletPool via closure, NOT as an ECS component.

local System = require("01_core.system")

local sin = math.sin
local cos = math.cos
local pi2 = math.pi * 2

local function createBulletEmitterSystem(bulletPool, getPlayerPos)

    local BulletEmitterSystem = System.new("BulletEmitter", {"Transform", "BulletEmitter"},
        function(ecs, dt, entities)
            for _, entityId in ipairs(entities) do
                local transform = ecs:getComponent(entityId, "Transform")
                local emitter   = ecs:getComponent(entityId, "BulletEmitter")

                if not emitter.active then goto continue end

                emitter.timer = emitter.timer + dt
                local interval = 1.0 / emitter.emitRate

                if emitter.timer >= interval then
                    emitter.timer = emitter.timer - interval

                    local pattern = emitter.pattern
                    local ox, oy  = transform.x, transform.y
                    local speed   = emitter.bulletSpeed
                    local opts    = {
                        maxLifetime = emitter.bulletLifetime,
                        radius      = emitter.bulletRadius,
                        color       = emitter.bulletColor,
                        layer       = "enemy_bullet",
                    }

                    if pattern == "circle" then
                        local count = emitter.bulletCount
                        for i = 0, count - 1 do
                            local angle = (i / count) * pi2
                            bulletPool:spawn(ox, oy, cos(angle) * speed, sin(angle) * speed, opts)
                        end

                    elseif pattern == "spiral" then
                        local count = emitter.bulletCount
                        for i = 0, count - 1 do
                            local angle = emitter.angle + (i / count) * pi2
                            bulletPool:spawn(ox, oy, cos(angle) * speed, sin(angle) * speed, opts)
                        end
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "aimed" then
                        -- Aimed at player position
                        local tx, ty = getPlayerPos()
                        if tx and ty then
                            local dx = tx - ox
                            local dy = ty - oy
                            local dist = math.sqrt(dx * dx + dy * dy)
                            if dist > 0 then
                                dx, dy = dx / dist, dy / dist
                            else
                                dx, dy = 0, -1
                            end
                            -- Spread: emit bulletCount aimed bullets with slight angle offset
                            local count = emitter.bulletCount
                            if count <= 1 then
                                bulletPool:spawn(ox, oy, dx * speed, dy * speed, opts)
                            else
                                local spread = 0.3  -- total spread in radians
                                local baseAngle = math.atan2(dy, dx)
                                for i = 0, count - 1 do
                                    local offset = -spread / 2 + (i / (count - 1)) * spread
                                    local a = baseAngle + offset
                                    bulletPool:spawn(ox, oy, cos(a) * speed, sin(a) * speed, opts)
                                end
                            end
                        end

                    elseif pattern == "wave" then
                        -- Sinusoidal wave: bullets travel downward with sine offset
                        local count = emitter.bulletCount
                        local baseAngle = emitter.angle
                        for i = 0, count - 1 do
                            local a = baseAngle + (i / count) * pi2
                            local vx = sin(a) * speed * 0.3
                            local vy = -speed
                            bulletPool:spawn(ox, oy, vx, vy, opts)
                        end
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2
                    end
                end

                ::continue::
            end
        end
    )

    return BulletEmitterSystem
end

return createBulletEmitterSystem
