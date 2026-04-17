-- PlayerWeapon Component
-- Auto-fire weapon attached to the player.
-- PlayerWeaponSystem reads this to find nearest enemy and fire.

local PlayerWeapon = {}

PlayerWeapon.name = "PlayerWeapon"

PlayerWeapon.defaults = {
    fireRate     = 4,        -- shots per second
    bulletSpeed  = 4.0,      -- bullet travel speed
    bulletCount  = 1,        -- bullets per shot
    bulletDamage = 1,        -- damage per bullet
    bulletLifetime = 3,
    bulletRadius = 0.035,
    bulletColor  = {0.2, 1, 0.5, 1},  -- greenish
    range        = 6.0,      -- auto-aim range (world units)
    timer        = 0,        -- fire cooldown accumulator
}

function PlayerWeapon.new(data)
    local d = data or {}
    local def = PlayerWeapon.defaults
    return {
        fireRate       = d.fireRate       or def.fireRate,
        bulletSpeed    = d.bulletSpeed    or def.bulletSpeed,
        bulletCount    = d.bulletCount    or def.bulletCount,
        bulletDamage   = d.bulletDamage   or def.bulletDamage,
        bulletLifetime = d.bulletLifetime or def.bulletLifetime,
        bulletRadius   = d.bulletRadius   or def.bulletRadius,
        bulletColor    = d.bulletColor    or def.bulletColor,
        range          = d.range          or def.range,
        timer          = 0,
    }
end

return PlayerWeapon
