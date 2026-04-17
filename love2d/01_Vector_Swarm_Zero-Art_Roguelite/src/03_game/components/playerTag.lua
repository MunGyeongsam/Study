-- PlayerTag Component
-- 플레이어 엔티티 식별 + 게임 상태 데이터

local PlayerTag = {}

PlayerTag.name = "PlayerTag"

PlayerTag.defaults = {
    powerUps = {},
    checkpointsSaved = {},
    currentZone = nil,
    zoneHistory = {},
}

function PlayerTag.new(data)
    return {
        powerUps         = data and data.powerUps         or {},
        checkpointsSaved = data and data.checkpointsSaved or {},
        currentZone      = data and data.currentZone      or nil,
        zoneHistory      = data and data.zoneHistory      or {},
    }
end

return PlayerTag
