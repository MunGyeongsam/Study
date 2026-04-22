-- Upgrade Tree
-- 영구 강화 시스템: Data Fragment로 구매, 게임 시작 시 플레이어에 적용
-- 3가지 분기 (attack/defense/utility) × 각 3단계 = 9개 업그레이드
--
-- Usage:
--   upgradeTree.init(saveData)           -- love.load에서
--   upgradeTree.applyToPlayer(ecs, id)   -- 게임 시작 시 (after createPlayer)
--   upgradeTree.draw()                   -- 업그레이드 메뉴에서

local saveData = require("00_common.saveData")

local _floor = math.floor
local _min   = math.min
local _sin   = math.sin

local upgradeTree = {}

-- ===== 업그레이드 정의 =====
-- cost: 각 레벨별 비용 배열
-- maxLevel: 최대 레벨
-- apply: 플레이어 ECS 컴포넌트에 적용 (level = 현재 레벨)
local UPGRADES = {
    -- === 공격 (Attack) ===
    {
        id       = "atk_damage",
        name     = "Core Damage",
        desc     = "Base bullet damage +1 per level",
        branch   = "attack",
        maxLevel = 3,
        cost     = {10, 25, 50},
        apply    = function(ecs, playerId, level)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.bulletDamage = w.bulletDamage + level end
        end,
    },
    {
        id       = "atk_firerate",
        name     = "Overclock",
        desc     = "Attack speed +15% per level",
        branch   = "attack",
        maxLevel = 3,
        cost     = {15, 35, 60},
        apply    = function(ecs, playerId, level)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.fireRate = w.fireRate * (1 + 0.15 * level) end
        end,
    },
    {
        id       = "atk_range",
        name     = "Long Reach",
        desc     = "Auto-aim range +20% per level",
        branch   = "attack",
        maxLevel = 3,
        cost     = {10, 20, 40},
        apply    = function(ecs, playerId, level)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.range = w.range * (1 + 0.20 * level) end
        end,
    },

    -- === 방어 (Defense) ===
    {
        id       = "def_hp",
        name     = "Reinforced Core",
        desc     = "Max HP +1 per level",
        branch   = "defense",
        maxLevel = 3,
        cost     = {10, 25, 50},
        apply    = function(ecs, playerId, level)
            local h = ecs:getComponent(playerId, "Health")
            if h then
                h.maxHp = h.maxHp + level
                h.hp = h.hp + level
            end
        end,
    },
    {
        id       = "def_iframe",
        name     = "Phase Shield",
        desc     = "Invincibility time +0.3s per level",
        branch   = "defense",
        maxLevel = 3,
        cost     = {15, 30, 55},
        apply    = function(ecs, playerId, level)
            local h = ecs:getComponent(playerId, "Health")
            if h then h.iFrames = h.iFrames + 0.3 * level end
        end,
    },
    {
        id       = "def_dash",
        name     = "Quick Phase",
        desc     = "Dash cooldown -15% per level",
        branch   = "defense",
        maxLevel = 3,
        cost     = {10, 20, 40},
        apply    = function(ecs, playerId, level)
            local d = ecs:getComponent(playerId, "Dash")
            if d then d.cooldown = d.cooldown * (1 - 0.15 * level) end
        end,
    },

    -- === 유틸리티 (Utility) ===
    {
        id       = "util_magnet",
        name     = "Data Magnet",
        desc     = "XP pickup range +30% per level",
        branch   = "utility",
        maxLevel = 3,
        cost     = {10, 20, 40},
        apply    = function(ecs, playerId, level)
            local xp = ecs:getComponent(playerId, "PlayerXP")
            if xp then xp.magnetRange = xp.magnetRange * (1 + 0.30 * level) end
        end,
    },
    {
        id       = "util_speed",
        name     = "Turbo",
        desc     = "Move speed +10% per level",
        branch   = "utility",
        maxLevel = 3,
        cost     = {15, 30, 50},
        apply    = function(ecs, playerId, level)
            local v = ecs:getComponent(playerId, "Velocity")
            if v then v.speed = v.speed * (1 + 0.10 * level) end
        end,
    },
    {
        id       = "util_fragment",
        name     = "Data Miner",
        desc     = "Fragment drop rate +5% per level",
        branch   = "utility",
        maxLevel = 3,
        cost     = {10, 25, 40},
        apply    = nil,  -- handled in ecsManager onEnemyDeath
    },
}

-- Lookup table by id
local upgradeById = {}
for _, u in ipairs(UPGRADES) do
    upgradeById[u.id] = u
end

-----------------------------------------------------------
-- Public API
-----------------------------------------------------------

function upgradeTree.getAll()
    return UPGRADES
end

function upgradeTree.getById(id)
    return upgradeById[id]
end

function upgradeTree.getLevel(id)
    return saveData.getUpgradeLevel(id)
end

function upgradeTree.getFragmentDropBonus()
    local level = saveData.getUpgradeLevel("util_fragment")
    return level * 0.05  -- +5% per level
end

function upgradeTree.canPurchase(id)
    local u = upgradeById[id]
    if not u then return false end
    local currentLevel = saveData.getUpgradeLevel(id)
    if currentLevel >= u.maxLevel then return false end
    local cost = u.cost[currentLevel + 1]
    return saveData.getFragments() >= cost
end

function upgradeTree.purchase(id)
    local u = upgradeById[id]
    if not u then return false end
    local currentLevel = saveData.getUpgradeLevel(id)
    if currentLevel >= u.maxLevel then return false end
    local cost = u.cost[currentLevel + 1]
    if not saveData.spendFragments(cost) then return false end
    saveData.setUpgradeLevel(id, currentLevel + 1)
    saveData.save()
    logInfo(string.format("[UPGRADE] Purchased %s Lv.%d (cost: %d)", u.name, currentLevel + 1, cost))
    return true
end

-- Apply all purchased upgrades to player entity
function upgradeTree.applyToPlayer(ecs, playerId)
    local applied = 0
    for _, u in ipairs(UPGRADES) do
        local level = saveData.getUpgradeLevel(u.id)
        if level > 0 and u.apply then
            u.apply(ecs, playerId, level)
            applied = applied + 1
        end
    end
    if applied > 0 then
        logInfo(string.format("[UPGRADE] Applied %d upgrades to player", applied))
    end
end

-- ===== UI (간단 버전 — 게임오버 후 표시) =====
local uiState = {
    active = false,
    selectedIndex = 1,
    scrollY = 0,
}

local titleFont = nil
local itemFont  = nil
local descFont  = nil

function upgradeTree.show()
    uiState.active = true
    uiState.selectedIndex = 1
end

function upgradeTree.hide()
    uiState.active = false
end

function upgradeTree.isActive()
    return uiState.active
end

local BRANCH_COLORS = {
    attack  = {1.0, 0.4, 0.3},
    defense = {0.3, 0.7, 1.0},
    utility = {0.4, 1.0, 0.5},
}

local BRANCH_NAMES = {
    attack  = "ATTACK",
    defense = "DEFENSE",
    utility = "UTILITY",
}

function upgradeTree.draw()
    if not uiState.active then return end

    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- Font caching
    if not titleFont then
        titleFont = lg.newFont(24)
        itemFont  = lg.newFont(16)
        descFont  = lg.newFont(12)
    end

    -- Background
    lg.setColor(0, 0, 0, 0.85)
    lg.rectangle("fill", 0, 0, w, h)

    -- Title
    lg.setFont(titleFont)
    lg.setColor(0.4, 0.8, 1.0, 1)
    local title = "UPGRADE TREE"
    local tw = titleFont:getWidth(title)
    lg.print(title, (w - tw) / 2, h * 0.04)

    -- Fragment balance
    lg.setFont(itemFont)
    lg.setColor(0.4, 0.8, 1.0, 0.9)
    local fragText = string.format("DATA FRAGMENTS: %d", saveData.getFragments())
    local fw = itemFont:getWidth(fragText)
    lg.print(fragText, (w - fw) / 2, h * 0.10)

    -- Draw upgrades by branch
    local cardW = w * 0.88
    local cardH = 52
    local startY = h * 0.15
    local y = startY
    local idx = 0

    local lastBranch = nil
    for _, u in ipairs(UPGRADES) do
        idx = idx + 1

        -- Branch header
        if u.branch ~= lastBranch then
            lastBranch = u.branch
            local bc = BRANCH_COLORS[u.branch] or {1, 1, 1}
            lg.setFont(itemFont)
            lg.setColor(bc[1], bc[2], bc[3], 1)
            lg.print(BRANCH_NAMES[u.branch] or u.branch, (w - cardW) / 2, y)
            y = y + 22
        end

        local cx = (w - cardW) / 2
        local cy = y
        local level = saveData.getUpgradeLevel(u.id)
        local isMax = level >= u.maxLevel
        local canBuy = upgradeTree.canPurchase(u.id)
        local isSelected = (idx == uiState.selectedIndex)

        -- Card background
        local bc = BRANCH_COLORS[u.branch] or {1, 1, 1}
        if isSelected then
            lg.setColor(bc[1] * 0.3, bc[2] * 0.3, bc[3] * 0.3, 0.9)
        else
            lg.setColor(0.1, 0.1, 0.15, 0.8)
        end
        lg.rectangle("fill", cx, cy, cardW, cardH, 6, 6)

        -- Border
        if isSelected then
            lg.setColor(bc[1], bc[2], bc[3], 1)
        else
            lg.setColor(bc[1] * 0.5, bc[2] * 0.5, bc[3] * 0.5, 0.5)
        end
        lg.setLineWidth(isSelected and 2 or 1)
        lg.rectangle("line", cx, cy, cardW, cardH, 6, 6)

        -- Name + level
        lg.setFont(itemFont)
        lg.setColor(1, 1, 1, 1)
        local levelStr = isMax and " [MAX]" or string.format(" Lv.%d/%d", level, u.maxLevel)
        lg.print(u.name .. levelStr, cx + 10, cy + 6)

        -- Description
        lg.setFont(descFont)
        lg.setColor(0.7, 0.7, 0.7, 1)
        lg.print(u.desc, cx + 10, cy + 28)

        -- Cost (right side)
        if not isMax then
            local cost = u.cost[level + 1]
            lg.setFont(descFont)
            if canBuy then
                lg.setColor(0.4, 1.0, 0.5, 1)
            else
                lg.setColor(0.5, 0.5, 0.5, 0.7)
            end
            local costStr = string.format("COST: %d", cost)
            local costW = descFont:getWidth(costStr)
            lg.print(costStr, cx + cardW - costW - 10, cy + 8)
        end

        y = y + cardH + 4
    end

    -- Controls hint
    lg.setFont(descFont)
    local alpha = 0.6 + 0.4 * _sin(love.timer.getTime() * 3)
    lg.setColor(1, 1, 1, alpha)
    local hint = "UP/DOWN: select | ENTER: buy | ESC: back"
    local hw = descFont:getWidth(hint)
    lg.print(hint, (w - hw) / 2, h * 0.92)

    lg.setColor(1, 1, 1, 1)
end

function upgradeTree.keypressed(key)
    if not uiState.active then return false end

    if key == "up" then
        uiState.selectedIndex = _min(#UPGRADES, uiState.selectedIndex - 1)
        if uiState.selectedIndex < 1 then uiState.selectedIndex = 1 end
        return true
    elseif key == "down" then
        uiState.selectedIndex = uiState.selectedIndex + 1
        if uiState.selectedIndex > #UPGRADES then uiState.selectedIndex = #UPGRADES end
        return true
    elseif key == "return" or key == "kpenter" then
        local u = UPGRADES[uiState.selectedIndex]
        if u then
            upgradeTree.purchase(u.id)
        end
        return true
    elseif key == "escape" or key == "r" then
        upgradeTree.hide()
        return true
    end

    return false
end

function upgradeTree.touchpressed(x, y)
    if not uiState.active then return false end

    local lg = love.graphics
    local w, h = lg.getDimensions()
    local cardW = w * 0.88
    local cardH = 52
    local startY = h * 0.15
    local yy = startY
    local idx = 0
    local lastBranch = nil

    for _, u in ipairs(UPGRADES) do
        idx = idx + 1
        if u.branch ~= lastBranch then
            lastBranch = u.branch
            yy = yy + 22
        end
        local cx = (w - cardW) / 2
        if x >= cx and x <= cx + cardW and y >= yy and y <= yy + cardH then
            uiState.selectedIndex = idx
            upgradeTree.purchase(u.id)
            return true
        end
        yy = yy + cardH + 4
    end

    -- Tap outside = close
    upgradeTree.hide()
    return true
end

return upgradeTree
