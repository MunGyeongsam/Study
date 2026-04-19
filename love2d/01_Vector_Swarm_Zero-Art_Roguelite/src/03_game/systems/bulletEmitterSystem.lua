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

                    elseif pattern == "grid" then
                        -- Grid: 8-direction cross (cardinal + diagonal)
                        local count = emitter.bulletCount  -- typically 8
                        for i = 0, count - 1 do
                            local angle = (i / count) * pi2
                            bulletPool:spawn(ox, oy, cos(angle) * speed, sin(angle) * speed, opts)
                        end

                    elseif pattern == "ring_pulse" then
                        -- Ring Pulse: 원형 발사 + 속도 변조로 맥동 레이어 생성
                        -- 매 발사마다 회전 + 속도가 다른 층이 겹쳐 펄스 느낌
                        local count = emitter.bulletCount
                        local pulse = 0.4 + 0.6 * (0.5 + 0.5 * sin(emitter.angle * 3))
                        for i = 0, count - 1 do
                            local angle = emitter.angle + (i / count) * pi2
                            local s = speed * pulse
                            bulletPool:spawn(ox, oy, cos(angle) * s, sin(angle) * s, opts)
                        end
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "cross" then
                        -- Cross: 십자(+) ↔ 대각(×) 교대 발사. 틈새 읽기 재미
                        for i = 0, 3 do
                            local angle = emitter.angle + i * (pi2 / 4)
                            bulletPool:spawn(ox, oy, cos(angle) * speed, sin(angle) * speed, opts)
                        end
                        -- 다음 발사 시 45° 회전 (+ ↔ × 교대)
                        emitter.angle = (emitter.angle + math.pi / 4) % pi2

                    elseif pattern == "orbit_shot" then
                        -- Orbit Shot: 발사 지점 주위를 공전 → 일정 시간 후 접선 사출
                        local count = emitter.bulletCount
                        local orbitR = emitter.orbitRadius or 0.8
                        local orbitSpd = emitter.orbitSpeed or 4.0
                        local orbitT = emitter.orbitTime or 1.2
                        for i = 0, count - 1 do
                            local angle = emitter.angle + (i / count) * pi2
                            -- 공전 시작 위치에 spawn, 초기 속도는 접선 (사출 시 사용)
                            local startX = ox + cos(angle) * orbitR
                            local startY = oy + sin(angle) * orbitR
                            local tangent = orbitSpd * orbitR
                            bulletPool:spawn(startX, startY,
                                -sin(angle) * tangent, cos(angle) * tangent,
                                { maxLifetime = opts.maxLifetime, radius = opts.radius,
                                  color = opts.color, layer = "enemy_bullet",
                                  behavior = "orbit", originX = ox, originY = oy,
                                  orbitRadius = orbitR, orbitSpeed = orbitSpd,
                                  orbitAngle = angle, orbitTime = orbitT })
                        end
                        emitter.angle = (emitter.angle + emitter.turnRate * interval) % pi2

                    elseif pattern == "return_shot" then
                        -- Return Shot: 발사 → 감속 → 반전 가속. 부메랑 효과
                        local count = emitter.bulletCount
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
