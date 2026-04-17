-- PlayerXP Component
-- 플레이어의 경험치, 레벨, 레벨업 상태

local PlayerXP = {}

PlayerXP.name = "PlayerXP"

PlayerXP.defaults = {
    xp = 0,
    level = 1,
    xpToNext = 10,       -- 다음 레벨까지 필요 XP
    xpGrowth = 1.4,      -- 레벨당 필요 XP 증가 배율
    magnetRange = 1.5,    -- XP 오브 자석 수집 범위 (월드 유닛)
    pendingLevelUp = false, -- 레벨업 대기 중 (UI 표시 필요)
}

function PlayerXP.new(data)
    return {
        xp             = data and data.xp             or PlayerXP.defaults.xp,
        level          = data and data.level          or PlayerXP.defaults.level,
        xpToNext       = data and data.xpToNext       or PlayerXP.defaults.xpToNext,
        xpGrowth       = data and data.xpGrowth       or PlayerXP.defaults.xpGrowth,
        magnetRange    = data and data.magnetRange    or PlayerXP.defaults.magnetRange,
        pendingLevelUp = false,
    }
end

return PlayerXP
