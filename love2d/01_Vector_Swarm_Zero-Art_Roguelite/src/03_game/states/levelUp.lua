-- Level Up Manager
-- 레벨업 시 3택 1 선택 UI + 업그레이드 적용
-- gameState가 LEVEL_UP 상태일 때 화면에 옵션 표시, 선택 시 적용

local gameState = require("03_game.states.gameState")

local LevelUp = {}

-- ===== 업그레이드 옵션 풀 =====
local UPGRADE_POOL = {
    -- 무기 업그레이드
    {
        id = "fire_rate",
        name = "Fire Rate +20%",
        desc = "Attack speed increased",
        category = "weapon",
        apply = function(ecs, playerId)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.fireRate = w.fireRate * 1.2 end
        end,
    },
    {
        id = "bullet_count",
        name = "Multi Shot +1",
        desc = "Fire additional bullet",
        category = "weapon",
        apply = function(ecs, playerId)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.bulletCount = w.bulletCount + 1 end
        end,
    },
    {
        id = "bullet_damage",
        name = "Damage +1",
        desc = "Each bullet deals more damage",
        category = "weapon",
        apply = function(ecs, playerId)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.bulletDamage = w.bulletDamage + 1 end
        end,
    },
    {
        id = "bullet_speed",
        name = "Bullet Speed +25%",
        desc = "Bullets fly faster",
        category = "weapon",
        apply = function(ecs, playerId)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.bulletSpeed = w.bulletSpeed * 1.25 end
        end,
    },
    {
        id = "attack_range",
        name = "Range +20%",
        desc = "Auto-aim reaches further",
        category = "weapon",
        apply = function(ecs, playerId)
            local w = ecs:getComponent(playerId, "PlayerWeapon")
            if w then w.range = w.range * 1.2 end
        end,
    },
    -- 패시브 스킬
    {
        id = "max_hp",
        name = "Max HP +1",
        desc = "Increase maximum health",
        category = "passive",
        apply = function(ecs, playerId)
            local h = ecs:getComponent(playerId, "Health")
            if h then
                h.maxHp = h.maxHp + 1
                h.hp = h.hp + 1
            end
        end,
    },
    {
        id = "move_speed",
        name = "Move Speed +15%",
        desc = "Move faster",
        category = "passive",
        apply = function(ecs, playerId)
            local v = ecs:getComponent(playerId, "Velocity")
            if v then v.speed = v.speed * 1.15 end
        end,
    },
    {
        id = "magnet_range",
        name = "Magnet +30%",
        desc = "XP orb pickup range increased",
        category = "passive",
        apply = function(ecs, playerId)
            local xp = ecs:getComponent(playerId, "PlayerXP")
            if xp then xp.magnetRange = xp.magnetRange * 1.3 end
        end,
    },
    {
        id = "dash_cooldown",
        name = "Dash CD -20%",
        desc = "Dash recharges faster",
        category = "passive",
        apply = function(ecs, playerId)
            local d = ecs:getComponent(playerId, "Dash")
            if d then d.cooldown = d.cooldown * 0.8 end
        end,
    },
    {
        id = "focus_energy",
        name = "Focus +25%",
        desc = "More focus energy",
        category = "passive",
        apply = function(ecs, playerId)
            local f = ecs:getComponent(playerId, "Focus")
            if f then
                f.maxEnergy = f.maxEnergy * 1.25
                f.energy = f.energy + f.maxEnergy * 0.25
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
}

-- 캐시된 폰트
local titleFont = nil
local optionFont = nil
local descFont = nil

-- 랜덤으로 3개 옵션 선택 (중복 없이)
local function pickRandomOptions(count)
    local pool = {}
    for i, v in ipairs(UPGRADE_POOL) do
        pool[i] = v
    end
    -- Fisher-Yates shuffle
    for i = #pool, 2, -1 do
        local j = math.random(1, i)
        pool[i], pool[j] = pool[j], pool[i]
    end
    local result = {}
    for i = 1, math.min(count, #pool) do
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
    logInfo(string.format("[LEVELUP] Options: %s, %s, %s",
        state.options[1].name, state.options[2].name, state.options[3].name))
end

function LevelUp.isActive()
    return state.active
end

function LevelUp.select(index)
    if not state.active then return end
    if index < 1 or index > #state.options then return end

    local option = state.options[index]
    option.apply(state.ecs, state.playerId)
    logInfo(string.format("[LEVELUP] Selected: %s", option.name))

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

    for i, option in ipairs(state.options) do
        local cx = startX + (i - 1) * (cardW + gap)

        -- 카드 배경
        local isWeapon = option.category == "weapon"
        if isWeapon then
            lg.setColor(0.15, 0.1, 0.25, 0.9)
        else
            lg.setColor(0.1, 0.2, 0.15, 0.9)
        end
        lg.rectangle("fill", cx, cardY, cardW, cardH, 8, 8)

        -- 카드 테두리
        if isWeapon then
            lg.setColor(0.6, 0.3, 1, 0.8)
        else
            lg.setColor(0.3, 0.8, 0.4, 0.8)
        end
        lg.setLineWidth(2)
        lg.rectangle("line", cx, cardY, cardW, cardH, 8, 8)

        -- 번호
        lg.setFont(descFont)
        lg.setColor(0.5, 0.5, 0.5, 1)
        lg.print(tostring(i), cx + 8, cardY + 8)

        -- 이름
        lg.setFont(optionFont)
        lg.setColor(1, 1, 1, 1)
        local nameW = optionFont:getWidth(option.name)
        lg.print(option.name, cx + (cardW - nameW) / 2, cardY + cardH * 0.3)

        -- 설명
        lg.setFont(descFont)
        lg.setColor(0.7, 0.7, 0.7, 1)
        local descW = descFont:getWidth(option.desc)
        lg.print(option.desc, cx + (cardW - descW) / 2, cardY + cardH * 0.6)

        -- 카테고리 태그
        lg.setColor(0.5, 0.5, 0.5, 0.6)
        local tag = isWeapon and "[WEAPON]" or "[PASSIVE]"
        lg.print(tag, cx + 8, cardY + cardH - 20)
    end

    -- 안내 텍스트
    lg.setFont(descFont)
    lg.setColor(1, 1, 1, 0.6 + 0.4 * math.sin(love.timer.getTime() * 3))
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

return LevelUp
