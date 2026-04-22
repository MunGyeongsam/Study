-- Level Up Manager
-- 레벨업 시 3택 1 선택 UI + 업그레이드 적용
-- gameState가 LEVEL_UP 상태일 때 화면에 옵션 표시, 선택 시 적용

local gameState = require("03_game.states.gameState")

local _max    = math.max
local _min    = math.min
local _floor  = math.floor
local _random = math.random
local _sin    = math.sin
local _exp    = math.exp

-- Card entrance animation constants
local CARD_ANIM_K  = 6     -- exp decay speed (higher = snappier)
local CARD_STAGGER = 0.08  -- seconds between each card entrance

local LevelUp = {}

-- ===== 업그레이드 옵션 풀 =====
local UPGRADE_POOL = {
    -- 무기 업그레이드
    {
        id = "fire_rate",
        name = "Fire Rate",
        desc = "Attack speed increased",
        category = "weapon",
        baseValue = 0.20,  -- +20%
        apply = function(ecs, playerId, factor)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.fireRate = w.fireRate * (1 + 0.20 * factor) end
        end,
    },
    {
        id = "bullet_count",
        name = "Multi Shot",
        desc = "Fire 2 additional bullets",
        category = "weapon",
        baseValue = 2,
        maxPicks = 2,  -- 최대 2회 (1→3→5발) 이후 풀에서 제외
        apply = function(ecs, playerId, factor)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.bulletCount = w.bulletCount + 2 end  -- 고정 +2 (항상 홀수 유지, 감쇠 없음)
        end,
    },
    {
        id = "bullet_damage",
        name = "Damage",
        desc = "Each bullet deals more damage",
        category = "weapon",
        baseValue = 1,
        apply = function(ecs, playerId, factor)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then
                w.bulletDamage = w.bulletDamage + 1  -- 고정 +1 (감쇠 면제)
            end
        end,
    },
    {
        id = "bullet_speed",
        name = "Bullet Speed",
        desc = "Bullets fly faster",
        category = "weapon",
        baseValue = 0.25,
        apply = function(ecs, playerId, factor)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.bulletSpeed = w.bulletSpeed * (1 + 0.25 * factor) end
        end,
    },
    {
        id = "attack_range",
        name = "Range",
        desc = "Auto-aim reaches further",
        category = "weapon",
        baseValue = 0.20,
        apply = function(ecs, playerId, factor)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.range = w.range * (1 + 0.20 * factor) end
        end,
    },
    -- 패시브 스킬
    {
        id = "max_hp",
        name = "Max HP +1",
        desc = "Increase maximum health",
        category = "passive",
        baseValue = 1,
        apply = function(ecs, playerId, factor)
            local h = ecs:getComponent(playerId, "Health")
            if h then
                h.maxHp = h.maxHp + 1  -- HP는 항상 +1 (감쇠 없음)
                h.hp = h.hp + 1
            end
        end,
    },
    {
        id = "move_speed",
        name = "Move Speed",
        desc = "Move faster",
        category = "passive",
        baseValue = 0.15,
        apply = function(ecs, playerId, factor)
            local v = ecs:getComponent(playerId, "Velocity")
            if v then v.speed = v.speed * (1 + 0.15 * factor) end
        end,
    },
    {
        id = "magnet_range",
        name = "Magnet",
        desc = "XP orb pickup range increased",
        category = "passive",
        baseValue = 0.30,
        apply = function(ecs, playerId, factor)
            local xp = ecs:getComponent(playerId, "PlayerXP")
            if xp then xp.magnetRange = xp.magnetRange * (1 + 0.30 * factor) end
        end,
    },
    {
        id = "dash_cooldown",
        name = "Dash CD",
        desc = "Dash recharges faster",
        category = "passive",
        baseValue = 0.20,
        apply = function(ecs, playerId, factor)
            local d = ecs:getComponent(playerId, "Dash")
            if d then d.cooldown = d.cooldown * (1 - 0.20 * factor) end
        end,
    },
    {
        id = "focus_energy",
        name = "Focus",
        desc = "More focus energy",
        category = "passive",
        baseValue = 0.25,
        apply = function(ecs, playerId, factor)
            local f = ecs:getComponent(playerId, "Focus")
            if f then
                local bonus = f.maxEnergy * 0.25 * factor
                f.maxEnergy = f.maxEnergy + bonus
                f.energy = f.energy + bonus
                f.rechargeRate = f.rechargeRate + f.rechargeRate * 0.15 * factor
            end
        end,
    },
}

-- 상태
local state = {
    active = false,
    options = {},      -- 현재 표시 중인 3개 옵션
    selectedIndex = 0, -- 선택된 인덱스 (1-3), 0 = 미선택
    playerId = nil,
    ecs = nil,
    showTime = 0,      -- wall-clock time when show() was called
}

-- 감쇠 스택: 업그레이드별 선택 횟수 추적
local pickCounts = {}  -- { [id] = count }

-- 감쇠 계수: n번째 선택 시 효과 배율
-- 1회: 100%, 2회: 70%, 3회: 50%, 4회: 35%, ...
local function getDiminishingFactor(pickCount)
    if pickCount <= 0 then return 1.0 end
    return 0.7 ^ pickCount  -- 매번 30% 감소
end

-- 캐시된 폰트
local titleFont = nil
local optionFont = nil
local descFont = nil

-- 랜덤으로 3개 옵션 선택 (중복 없이, maxPicks 도달 시 제외)
local function pickRandomOptions(count)
    local pool = {}
    for _, v in ipairs(UPGRADE_POOL) do
        if not v.maxPicks or (pickCounts[v.id] or 0) < v.maxPicks then
            pool[#pool + 1] = v
        end
    end
    -- Fisher-Yates shuffle
    for i = #pool, 2, -1 do
        local j = _random(1, i)
        pool[i], pool[j] = pool[j], pool[i]
    end
    local result = {}
    for i = 1, _min(count, #pool) do
        result[i] = pool[i]
    end
    return result
end

function LevelUp.show(ecs, playerId)
    state.active = true
    state.options = pickRandomOptions(3)
    state.selectedIndex = 0
    state.playerId = playerId
    state.ecs = ecs
    gameState.setTimeScale(0)  -- 게임 일시정지
    state.showTime = love.timer.getTime()

    -- 옵션 이름에 현재 효과량 표시
    for _, opt in ipairs(state.options) do
        local count = pickCounts[opt.id] or 0
        local factor = getDiminishingFactor(count)
        if opt.id == "bullet_count" then
            opt.displayName = opt.name .. " +2"
        elseif opt.id == "max_hp" or opt.id == "bullet_damage" then
            opt.displayName = opt.name .. " +1"
        else
            local pct = _floor(opt.baseValue * factor * 100 + 0.5)
            if opt.id == "dash_cooldown" then
                opt.displayName = opt.name .. string.format(" -%d%%", pct)
            else
                opt.displayName = opt.name .. string.format(" +%d%%", pct)
            end
        end
    end

    logInfo(string.format("[LEVELUP] Options: %s, %s, %s",
        state.options[1].displayName, state.options[2].displayName, state.options[3].displayName))
end

function LevelUp.isActive()
    return state.active
end

function LevelUp.select(index)
    if not state.active then return end
    if index < 1 or index > #state.options then return end

    local option = state.options[index]

    -- 감쇠 계산
    local count = pickCounts[option.id] or 0
    local factor = getDiminishingFactor(count)
    option.apply(state.ecs, state.playerId, factor)

    -- 선택 횟수 기록
    pickCounts[option.id] = count + 1

    if playSound then playSound("level_up") end

    logInfo(string.format("[LEVELUP] Selected: %s (pick #%d, factor: %.0f%%)",
        option.displayName or option.name, count + 1, factor * 100))

    state.active = false
    state.options = {}
    state.selectedIndex = 0
    gameState.setTimeScale(1.0)  -- 게임 재개
end

function LevelUp.draw()
    if not state.active then return end

    local lg = love.graphics
    local w, h = lg.getDimensions()

    -- 폰트 캐싱
    if not titleFont then
        titleFont  = lg.newFont(28)
        optionFont = lg.newFont(18)
        descFont   = lg.newFont(13)
    end

    -- 반투명 배경
    lg.setColor(0, 0, 0, 0.7)
    lg.rectangle("fill", 0, 0, w, h)

    -- 타이틀
    lg.setFont(titleFont)
    lg.setColor(1, 1, 0.3, 1)
    local title = "LEVEL UP!"
    local tw = titleFont:getWidth(title)
    lg.print(title, (w - tw) / 2, h * 0.15)

    -- 3개 옵션 카드
    local cardW = w * 0.28
    local cardH = h * 0.25
    local gap = w * 0.03
    local totalW = cardW * 3 + gap * 2
    local startX = (w - totalW) / 2
    local cardY = h * 0.35

    -- entrance animation: per-card stagger with exp decay
    local elapsed = love.timer.getTime() - state.showTime
    local slideDist = cardH * 0.3

    for i, option in ipairs(state.options) do
        local cx = startX + (i - 1) * (cardW + gap)

        -- per-card animation progress
        local cardElapsed = elapsed - (i - 1) * CARD_STAGGER
        local t = 1  -- fully visible by default
        if cardElapsed < 0 then
            t = 0
        elseif cardElapsed < 1.0 then
            t = 1 - _exp(-CARD_ANIM_K * cardElapsed)
        end
        local offsetY = (1 - t) * slideDist
        local alpha = t
        local cy = cardY + offsetY

        -- 카드 배경
        local isWeapon = option.category == "weapon"
        if isWeapon then
            lg.setColor(0.15, 0.1, 0.25, 0.9 * alpha)
        else
            lg.setColor(0.1, 0.2, 0.15, 0.9 * alpha)
        end
        lg.rectangle("fill", cx, cy, cardW, cardH, 8, 8)

        -- 카드 테두리
        if isWeapon then
            lg.setColor(0.6, 0.3, 1, 0.8 * alpha)
        else
            lg.setColor(0.3, 0.8, 0.4, 0.8 * alpha)
        end
        lg.setLineWidth(2)
        lg.rectangle("line", cx, cy, cardW, cardH, 8, 8)

        -- 번호
        lg.setFont(descFont)
        lg.setColor(0.5, 0.5, 0.5, alpha)
        lg.print(tostring(i), cx + 8, cy + 8)

        -- 이름 (감쇠 효과량 포함)
        lg.setFont(optionFont)
        lg.setColor(1, 1, 1, alpha)
        local displayName = option.displayName or option.name
        local nameW = optionFont:getWidth(displayName)
        lg.print(displayName, cx + (cardW - nameW) / 2, cy + cardH * 0.3)

        -- 설명
        lg.setFont(descFont)
        lg.setColor(0.7, 0.7, 0.7, alpha)
        local descW = descFont:getWidth(option.desc)
        lg.print(option.desc, cx + (cardW - descW) / 2, cy + cardH * 0.6)

        -- 카테고리 태그
        lg.setColor(0.5, 0.5, 0.5, 0.6 * alpha)
        local tag = isWeapon and "[WEAPON]" or "[PASSIVE]"
        lg.print(tag, cx + 8, cy + cardH - 20)
    end

    -- 안내 텍스트
    lg.setFont(descFont)
    lg.setColor(1, 1, 1, 0.6 + 0.4 * _sin(love.timer.getTime() * 3))
    local hint = "Press 1, 2, or 3 to choose"
    local hw = descFont:getWidth(hint)
    lg.print(hint, (w - hw) / 2, h * 0.75)

    lg.setColor(1, 1, 1, 1)
end

function LevelUp.keypressed(key)
    if not state.active then return false end
    if key == "1" then LevelUp.select(1); return true end
    if key == "2" then LevelUp.select(2); return true end
    if key == "3" then LevelUp.select(3); return true end
    return false
end

function LevelUp.touchpressed(x, y)
    if not state.active then return false end

    local lg = love.graphics
    local w, h = lg.getDimensions()

    local cardW = w * 0.28
    local cardH = h * 0.25
    local gap = w * 0.03
    local totalW = cardW * 3 + gap * 2
    local startX = (w - totalW) / 2
    local cardY = h * 0.35

    for i = 1, 3 do
        local cx = startX + (i - 1) * (cardW + gap)
        if x >= cx and x <= cx + cardW and y >= cardY and y <= cardY + cardH then
            LevelUp.select(i)
            return true
        end
    end
    return false
end

function LevelUp.reset()
    state.active = false
    state.options = {}
    state.selectedIndex = 0
    pickCounts = {}
end

--- Apply a single random upgrade silently (for Start Boost passive)
function LevelUp.applyRandomUpgrade(ecs, playerId)
    local options = pickRandomOptions(1)
    if #options > 0 then
        local opt = options[1]
        local count = pickCounts[opt.id] or 0
        local factor = getDiminishingFactor(count)
        opt.apply(ecs, playerId, factor)
        pickCounts[opt.id] = count + 1
        logInfo(string.format("[LEVELUP] Start Boost: %s applied (factor: %.0f%%)", opt.name, factor * 100))
    end
end

return LevelUp
