-- Dash Component
-- 순간이동 대쉬 (무적 + 쿨타임)

local Dash = {}

Dash.name = "Dash"

Dash.defaults = {
    distance = 2.0,       -- 대쉬 이동 거리 (월드 유닛)
    cooldown = 3.0,       -- 쿨타임 (초)
    cooldownTimer = 0,    -- 현재 쿨타임 남은 시간
    iFrames = 0.3,        -- 대쉬 중 무적 시간
    requested = false,    -- 대쉬 요청 플래그
    dirX = 0,             -- 대쉬 방향
    dirY = 0,
}

function Dash.new(data)
    return {
        distance      = data and data.distance      or Dash.defaults.distance,
        cooldown      = data and data.cooldown       or Dash.defaults.cooldown,
        cooldownTimer = data and data.cooldownTimer  or Dash.defaults.cooldownTimer,
        iFrames       = data and data.iFrames        or Dash.defaults.iFrames,
        requested     = false,
        dirX          = 0,
        dirY          = 0,
    }
end

return Dash
