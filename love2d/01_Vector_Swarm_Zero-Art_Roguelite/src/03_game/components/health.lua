-- Health Component
-- HP, invincibility frames, hit tracking.

local Health = {}

Health.name = "Health"

Health.defaults = {
    hp        = 3,      -- current hit points
    maxHp     = 3,
    iFrames   = 1.5,    -- invincibility duration after hit (seconds)
    iTimer    = 0,      -- current invincibility timer (counts down)
    hitCount  = 0,      -- total hits taken
    alive     = true,
}

function Health.new(data)
    local d = data or {}
    local def = Health.defaults
    local hp = d.hp or def.hp
    return {
        hp       = hp,
        maxHp    = d.maxHp or hp,
        iFrames  = d.iFrames or def.iFrames,
        iTimer   = 0,
        hitCount = 0,
        alive    = true,
    }
end

return Health
