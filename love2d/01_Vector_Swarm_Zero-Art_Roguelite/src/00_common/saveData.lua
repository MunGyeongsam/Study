-- Save Data Module
-- love.filesystem 기반 영구 저장 (JSON 직렬화)
-- 저장 위치: love save directory / save/progress.dat
--
-- Usage:
--   saveData.load()                    -- love.load에서 호출
--   saveData.addFragments(n)           -- Fragment 획득
--   saveData.purchaseUpgrade(id)       -- 강화 구매
--   saveData.save()                    -- 게임 오버 시 호출

local _floor = math.floor

local saveData = {}

local SAVE_DIR  = "save"
local SAVE_FILE = "save/progress.dat"

-- Default data (첫 실행 시)
local function defaultData()
    return {
        fragments      = 0,          -- 보유 Data Fragment
        totalFragments = 0,          -- 누적 획득 Fragment
        upgrades       = {},         -- { [upgradeId] = level }
        bestScore      = 0,          -- 최고 생존시간
        bestStage      = 0,          -- 최고 도달 스테이지
        totalRuns      = 0,          -- 총 플레이 횟수
        totalKills     = 0,          -- 누적 적 처치 수
        bossesDefeated = {},         -- { ["NULL"]=true, ["STACK"]=true, ... }
        achievements   = {},         -- { ["stage3_clear"]=true, ... }
        selectedCharacter = "default",  -- 선택된 캐릭터 ID
        tutorialDone   = false,       -- 첫 플레이 튜토리얼 완료 여부
    }
end

local data = defaultData()

-- ===== Simple JSON serializer (no external deps) =====
local function serialize(tbl, indent)
    indent = indent or ""
    local nextIndent = indent .. "  "
    local parts = {}
    local isArray = (#tbl > 0)

    if isArray then
        for _, v in ipairs(tbl) do
            if type(v) == "table" then
                parts[#parts + 1] = nextIndent .. serialize(v, nextIndent)
            elseif type(v) == "string" then
                parts[#parts + 1] = nextIndent .. string.format("%q", v)
            else
                parts[#parts + 1] = nextIndent .. tostring(v)
            end
        end
        return "[\n" .. table.concat(parts, ",\n") .. "\n" .. indent .. "]"
    else
        local keys = {}
        for k in pairs(tbl) do keys[#keys + 1] = k end
        table.sort(keys, function(a, b) return tostring(a) < tostring(b) end)
        for _, k in ipairs(keys) do
            local v = tbl[k]
            local keyStr = string.format("%q", tostring(k))
            if type(v) == "table" then
                parts[#parts + 1] = nextIndent .. keyStr .. ": " .. serialize(v, nextIndent)
            elseif type(v) == "string" then
                parts[#parts + 1] = nextIndent .. keyStr .. ": " .. string.format("%q", v)
            elseif type(v) == "boolean" then
                parts[#parts + 1] = nextIndent .. keyStr .. ": " .. (v and "true" or "false")
            else
                parts[#parts + 1] = nextIndent .. keyStr .. ": " .. tostring(v)
            end
        end
        return "{\n" .. table.concat(parts, ",\n") .. "\n" .. indent .. "}"
    end
end

-- Simple JSON-like parser (handles our own output only)
local function deserialize(str)
    -- Use Lua load for simple cases — our format is safe (numbers, strings, booleans, tables)
    -- Convert JSON-like to Lua table literal
    local luaStr = str
        :gsub('"([^"]-)":', '["%1"]=')    -- "key": → ["key"]=

    local fn, err = load("return " .. luaStr)
    if fn then
        local ok, result = pcall(fn)
        if ok and type(result) == "table" then
            return result
        end
    end
    return nil
end

-----------------------------------------------------------
-- Public API
-----------------------------------------------------------

function saveData.load()
    if not love.filesystem.getInfo(SAVE_DIR) then
        love.filesystem.createDirectory(SAVE_DIR)
    end

    local info = love.filesystem.getInfo(SAVE_FILE)
    if info then
        local content = love.filesystem.read(SAVE_FILE)
        if content then
            local loaded = deserialize(content)
            if loaded then
                -- Merge with defaults (forward compatibility)
                local defaults = defaultData()
                for k, v in pairs(defaults) do
                    if loaded[k] == nil then
                        loaded[k] = v
                    end
                end
                data = loaded
                logInfo(string.format("[SAVE] Loaded: %d fragments, %d runs, best stage %d",
                    data.fragments, data.totalRuns, data.bestStage))
                return true
            end
        end
        logWarn("[SAVE] Failed to parse save file, using defaults")
    else
        logInfo("[SAVE] No save file found, starting fresh")
    end

    data = defaultData()
    return false
end

function saveData.save()
    if not love.filesystem.getInfo(SAVE_DIR) then
        love.filesystem.createDirectory(SAVE_DIR)
    end

    local content = serialize(data)
    local success, err = love.filesystem.write(SAVE_FILE, content)
    if success then
        logInfo(string.format("[SAVE] Saved: %d fragments, best stage %d",
            data.fragments, data.bestStage))
    else
        logError("[SAVE] Write failed: " .. tostring(err))
    end
    return success
end

-- Fragment 관련
function saveData.addFragments(amount)
    data.fragments = data.fragments + amount
    data.totalFragments = data.totalFragments + amount
end

function saveData.spendFragments(amount)
    if data.fragments >= amount then
        data.fragments = data.fragments - amount
        return true
    end
    return false
end

function saveData.getFragments()
    return data.fragments
end

-- 업그레이드 관련
function saveData.getUpgradeLevel(upgradeId)
    return data.upgrades[upgradeId] or 0
end

function saveData.setUpgradeLevel(upgradeId, level)
    data.upgrades[upgradeId] = level
end

function saveData.getUpgrades()
    return data.upgrades
end

-- 실행 기록
function saveData.recordRun(score, stage)
    data.totalRuns = data.totalRuns + 1
    if score > data.bestScore then
        data.bestScore = score
    end
    if stage > data.bestStage then
        data.bestStage = stage
    end
end

function saveData.getStats()
    return {
        fragments        = data.fragments,
        totalFragments   = data.totalFragments,
        bestScore        = data.bestScore,
        bestStage        = data.bestStage,
        totalRuns        = data.totalRuns,
        upgrades         = data.upgrades,
        totalKills       = data.totalKills,
        bossesDefeated   = data.bossesDefeated,
        achievements     = data.achievements,
        selectedCharacter = data.selectedCharacter,
        tutorialDone     = data.tutorialDone,
    }
end

-- 누적 킬 수 추가
function saveData.addKills(count)
    data.totalKills = data.totalKills + count
end

-- 보스 처치 기록
function saveData.recordBossDefeat(bossType)
    data.bossesDefeated[bossType] = true
end

-- 도전과제 해금
function saveData.unlockAchievement(achievementId)
    data.achievements[achievementId] = true
end

-- 캐릭터 선택
function saveData.setSelectedCharacter(charId)
    data.selectedCharacter = charId
end

function saveData.getSelectedCharacter()
    return data.selectedCharacter or "default"
end

-- 튜토리얼 완료
function saveData.setTutorialDone(val)
    data.tutorialDone = val
end

-- 디버그: 세이브 리셋
function saveData.reset()
    data = defaultData()
    saveData.save()
    logInfo("[SAVE] Data reset to defaults")
end

return saveData
