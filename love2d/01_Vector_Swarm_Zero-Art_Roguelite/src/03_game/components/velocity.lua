-- Velocity Component
-- 엔티티의 속도와 감속

local Velocity = {}

Velocity.name = "Velocity"

Velocity.defaults = {
    vx = 0,
    vy = 0,
    speed = 2,
    maxSpeed = 5,
    damping = 0.9,
}

function Velocity.new(data)
    return {
        vx       = data and data.vx       or Velocity.defaults.vx,
        vy       = data and data.vy       or Velocity.defaults.vy,
        speed    = data and data.speed    or Velocity.defaults.speed,
        maxSpeed = data and data.maxSpeed or Velocity.defaults.maxSpeed,
        damping  = data and data.damping  or Velocity.defaults.damping,
    }
end

return Velocity
