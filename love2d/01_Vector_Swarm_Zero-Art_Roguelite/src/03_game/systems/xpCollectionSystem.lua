-- XP Collection System
-- XpOrb 엔티티를 플레이어 쪽으로 자석처럼 끌어당기고, 닿으면 수집.
-- 플레이어가 PlayerXP 컴포넌트를 통해 XP를 축적, 레벨업 판정.

local System = require("01_core.system")
local sqrt = math.sqrt
local floor = math.floor

local function createXpCollectionSystem(getPlayerPos)

    local XpCollectionSystem = System.new("XpCollection", {"XpOrb", "Transform", "Velocity"},
        function(ecs, dt, entities)
            if #entities == 0 then return end

            local px, py = getPlayerPos()

            -- 플레이어 엔티티에서 PlayerXP 가져오기
            local players = ecs:queryEntities({"PlayerTag", "PlayerXP"})
            if #players == 0 then return end
            local playerXP = ecs:getComponent(players[1], "PlayerXP")

            local magnetRange2 = playerXP.magnetRange * playerXP.magnetRange
            local collectRadius2 = 0.15 * 0.15  -- 수집 판정 반경

            for _, entityId in ipairs(entities) do
                local orb       = ecs:getComponent(entityId, "XpOrb")
                local transform = ecs:getComponent(entityId, "Transform")
                local velocity  = ecs:getComponent(entityId, "Velocity")

                local dx = px - transform.x
                local dy = py - transform.y
                local dist2 = dx * dx + dy * dy

                -- 수집 판정
                if dist2 < collectRadius2 then
                    playerXP.xp = playerXP.xp + orb.value
                    ecs:destroyEntity(entityId)

                    -- 레벨업 체크
                    if playerXP.xp >= playerXP.xpToNext then
                        playerXP.xp = playerXP.xp - playerXP.xpToNext
                        playerXP.level = playerXP.level + 1
                        playerXP.xpToNext = floor(playerXP.xpToNext * playerXP.xpGrowth)
                        playerXP.pendingLevelUp = true
                        logInfo(string.format("[XP] Level Up! Lv.%d (next: %d XP)", playerXP.level, playerXP.xpToNext))
                    end

                    goto nextOrb
                end

                -- 자석 범위 내 → 끌려오기
                if dist2 < magnetRange2 then
                    orb.attracted = true
                end

                if orb.attracted then
                    local dist = sqrt(dist2)
                    if dist > 0.01 then
                        local nx, ny = dx / dist, dy / dist
                        velocity.vx = nx * orb.magnetSpeed
                        velocity.vy = ny * orb.magnetSpeed
                    end
                end

                ::nextOrb::
            end
        end
    )

    return XpCollectionSystem
end

return createXpCollectionSystem
