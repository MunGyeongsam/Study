-- Bullet Emitter System
-- Reads BulletEmitter + Transform components from ECS entities,
-- spawns bullets into the shared BulletPool.
--
-- This system receives bulletPool via closure, NOT as an ECS component.

local System = require("01_core.system")

local sin   = math.sin
local cos   = math.cos
local sqrt  = math.sqrt
local atan2 = math.atan2
local pi    = math.pi
local pi2   = pi * 2

-- 원형 배치 공통 헬퍼: count발을 baseAngle부터 등간격 발사
local function spawnRing(pool, ox, oy, count, speed, baseAngle, opts)
    for i = 0, count - 1 do
        local angle = baseAngle + (i / count) * pi2
        pool:spawn(ox, oy, cos(angle) * speed, sin(angle) * speed, opts)
    end
end

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
                    local count   = emitter.bulletCount
                    local opts    = {
                        maxLifetime = emitter.bulletLifetime,
                        radius      = emitter.bulletRadius,
                        color       = emitter.bulletColor,
                        layer       = "enemy_bullet",
                    }

                    if pattern == "circle" then
                        spawnRing(bulletPool, ox, oy, count, speed, 0, opts)

                    elseif pattern == "spiral" then
                        spawnRing(bulletPool, ox, oy, count, speed, emitter.angle, opts)
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "aimed" then
                        local tx, ty = getPlayerPos()
                        if tx and ty then
                            local dx = tx - ox
                            local dy = ty - oy
                            local dist = sqrt(dx * dx + dy * dy)
                            if dist > 0 then
                                dx, dy = dx / dist, dy / dist
                            else
                                dx, dy = 0, -1
                            end
                            if count <= 1 then
                                bulletPool:spawn(ox, oy, dx * speed, dy * speed, opts)
                            else
                                local spread = 0.3
                                local baseAngle = atan2(dy, dx)
                                for i = 0, count - 1 do
                                    local offset = -spread / 2 + (i / (count - 1)) * spread
                                    local a = baseAngle + offset
                                    bulletPool:spawn(ox, oy, cos(a) * speed, sin(a) * speed, opts)
                                end
                            end
                        end

                    elseif pattern == "wave" then
                        for i = 0, count - 1 do
                            local a = emitter.angle + (i / count) * pi2
                            bulletPool:spawn(ox, oy, sin(a) * speed * 0.3, -speed, opts)
                        end
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "grid" then
                        spawnRing(bulletPool, ox, oy, count, speed, 0, opts)

                    elseif pattern == "ring_pulse" then
                        local pulse = 0.4 + 0.6 * (0.5 + 0.5 * sin(emitter.angle * 3))
                        spawnRing(bulletPool, ox, oy, count, speed * pulse, emitter.angle, opts)
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "cross" then
                        spawnRing(bulletPool, ox, oy, 4, speed, emitter.angle, opts)
                        emitter.angle = (emitter.angle + pi / 4) % pi2

                    elseif pattern == "orbit_shot" then
                        local orbitR   = emitter.orbitRadius or 0.8
                        local orbitSpd = emitter.orbitSpeed or 4.0
                        local orbitT   = emitter.orbitTime or 1.2
                        for i = 0, count - 1 do
                            local angle = emitter.angle + (i / count) * pi2
                            local tangent = orbitSpd * orbitR
                            bulletPool:spawn(
                                ox + cos(angle) * orbitR, oy + sin(angle) * orbitR,
                                -sin(angle) * tangent, cos(angle) * tangent,
                                { maxLifetime = opts.maxLifetime, radius = opts.radius,
                                  color = opts.color, layer = "enemy_bullet",
                                  behavior = "orbit", originX = ox, originY = oy,
                                  orbitRadius = orbitR, orbitSpeed = orbitSpd,
                                  orbitAngle = angle, orbitTime = orbitT })
                        end
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "return_shot" then
                        local retTime = emitter.returnTime or 0.8
                        for i = 0, count - 1 do
                            local angle = emitter.angle + (i / count) * pi2
                            bulletPool:spawn(ox, oy,
                                cos(angle) * speed, sin(angle) * speed,
                                { maxLifetime = opts.maxLifetime, radius = opts.radius,
                                  color = opts.color, layer = "enemy_bullet",
                                  behavior = "return", returnTime = retTime,
                                  damping = 0.15 })
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
