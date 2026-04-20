-- Achievement System
-- 도전과제 추적 + 해금 관리
-- saveData를 통해 영구 저장, 런 중 이벤트로 진행도 갱신

local saveData = require("00_common.saveData")

local achievementSystem = {}

-- ─── Achievement Definitions ─────────────────────────────────────
-- reward.type: "weapon" (미구현→COMING SOON), "character", "passive"
local ACHIEVEMENTS = {
    {
        id          = "stage3_clear",
        name        = "First Contact",
        desc        = "Clear Stage 3",
        condition   = "stage_clear",
        target      = 3,
        reward      = { type = "weapon", id = "spread_shot", name = "Spread Shot" },
    },
    {
        id          = "stage6_clear",
        name        = "Deep Dive",
        desc        = "Clear Stage 6",
        condition   = "stage_clear",
        target      = 6,
        reward      = { type = "weapon", id = "piercing", name = "Piercing" },
    },
    {
        id          = "all_bosses",
        name        = "Exterminator",
        desc        = "Defeat all 5 boss types",
        condition   = "all_bosses",
        target      = 5,
        reward      = { type = "character", id = "debugger", name = "Debugger" },
    },
    {
        id          = "total_kills_1000",
        name        = "Massacre",
        desc        = "Defeat 1000 enemies total",
        condition   = "total_kills",
        target      = 1000,
        reward      = { type = "character", id = "compiler", name = "Compiler" },
    },
    {
        id          = "total_fragments_500",
        name        = "Data Hoarder",
        desc        = "Collect 500 fragments total",
        condition   = "total_fragments",
        target      = 500,
        reward      = { type = "passive", id = "start_boost", name = "Start Boost" },
    },
}

-- Pending unlocks from this session (reset each run)
local pendingUnlocks = {}

-- Session kill counter (flushed to saveData on game over)
local sessionKills = 0

-- ─── Public API ──────────────────────────────────────────────────

--- Get all achievement definitions with current progress/status
function achievementSystem.getAll()
    local result = {}
    local stats = saveData.getStats()
    local completed = stats.achievements or {}
    local bossesDefeated = stats.bossesDefeated or {}

    for _, def in ipairs(ACHIEVEMENTS) do
        local entry = {
            id       = def.id,
            name     = def.name,
            desc     = def.desc,
            reward   = def.reward,
            target   = def.target,
            unlocked = completed[def.id] == true,
            progress = 0,
        }

        -- Calculate progress
        if def.condition == "stage_clear" then
            entry.progress = math.min(stats.bestStage or 0, def.target)
        elseif def.condition == "total_kills" then
            entry.progress = math.min(stats.totalKills or 0, def.target)
        elseif def.condition == "total_fragments" then
            entry.progress = math.min(stats.totalFragments or 0, def.target)
        elseif def.condition == "all_bosses" then
            local count = 0
            for _ in pairs(bossesDefeated) do count = count + 1 end
            entry.progress = math.min(count, def.target)
        end

        result[#result + 1] = entry
    end
    return result
end

--- Get specific achievement definition
function achievementSystem.getById(id)
    for _, def in ipairs(ACHIEVEMENTS) do
        if def.id == id then return def end
    end
    return nil
end

--- Check if a specific achievement is unlocked
function achievementSystem.isUnlocked(id)
    local stats = saveData.getStats()
    local completed = stats.achievements or {}
    return completed[id] == true
end

--- Reset session state (call at game start)
function achievementSystem.resetSession()
    pendingUnlocks = {}
    sessionKills = 0
end

--- Track enemy kill during run
function achievementSystem.onEnemyKill()
    sessionKills = sessionKills + 1
end

--- Get session kill count
function achievementSystem.getSessionKills()
    return sessionKills
end

--- Called on boss defeat — record boss type
function achievementSystem.onBossDefeated(bossType)
    saveData.recordBossDefeat(bossType)
    logInfo(string.format("[ACHIEVE] Boss defeated: %s", bossType))
end

--- Called on stage clear — check stage milestones
function achievementSystem.onStageClear(stage)
    -- Stage clear achievements are checked via bestStage in saveData
    -- (recordRun updates bestStage)
    logInfo(string.format("[ACHIEVE] Stage %d cleared", stage))
end

--- Flush session kills to persistent storage + check all achievements
--- Call this on game over / return to title
function achievementSystem.onRunEnd()
    -- Flush kills
    if sessionKills > 0 then
        saveData.addKills(sessionKills)
        logInfo(string.format("[ACHIEVE] Session kills flushed: +%d", sessionKills))
    end

    -- Check all achievements against current persistent data
    pendingUnlocks = {}
    local stats = saveData.getStats()
    local completed = stats.achievements or {}
    local bossesDefeated = stats.bossesDefeated or {}

    for _, def in ipairs(ACHIEVEMENTS) do
        if not completed[def.id] then
            local met = false

            if def.condition == "stage_clear" then
                met = (stats.bestStage or 0) >= def.target
            elseif def.condition == "total_kills" then
                met = (stats.totalKills or 0) >= def.target
            elseif def.condition == "total_fragments" then
                met = (stats.totalFragments or 0) >= def.target
            elseif def.condition == "all_bosses" then
                local count = 0
                for _ in pairs(bossesDefeated) do count = count + 1 end
                met = count >= def.target
            end

            if met then
                saveData.unlockAchievement(def.id)
                pendingUnlocks[#pendingUnlocks + 1] = def
                logInfo(string.format("[ACHIEVE] UNLOCKED: %s — %s", def.name, def.reward.name))
            end
        end
    end

    if #pendingUnlocks > 0 then
        saveData.save()
    end
end

--- Get newly unlocked achievements from this run (for game over toast)
function achievementSystem.getPendingUnlocks()
    return pendingUnlocks
end

--- Check if a reward is available (unlocked)
function achievementSystem.isRewardUnlocked(rewardId)
    local stats = saveData.getStats()
    local completed = stats.achievements or {}
    for _, def in ipairs(ACHIEVEMENTS) do
        if def.reward.id == rewardId and completed[def.id] then
            return true
        end
    end
    return false
end

return achievementSystem
