-- AchievementScene
-- 도전과제 목록 오버레이: 잠김/해제 + 진행률 + 보상 표시
-- drawBelow=true, transparent=false

local achievementSystem = require("03_game.states.achievementSystem")

local AchievementScene = {}
AchievementScene.__index = AchievementScene

AchievementScene.name        = "AchievementScene"
AchievementScene.transparent = false
AchievementScene.drawBelow   = true

function AchievementScene.new(sceneStack)
    return setmetatable({
        _sceneStack = sceneStack,
        _timer = 0,
        _scroll = 0,
        _fonts = nil,
    }, AchievementScene)
end

function AchievementScene:enter(prev)
    self._timer = 0
    self._scroll = 0
    self._fonts = {
        title  = love.graphics.newFont(24),
        name   = love.graphics.newFont(16),
        desc   = love.graphics.newFont(12),
    }
    logInfo("[ACHIEVE] AchievementScene entered")
end

function AchievementScene:exit()
    logInfo("[ACHIEVE] AchievementScene exited")
end

function AchievementScene:update(dt)
    self._timer = self._timer + dt
end

function AchievementScene:draw()
    local lg = love.graphics
    local W, H = lg.getDimensions()

    -- Dim overlay
    lg.setColor(0, 0, 0, 0.88)
    lg.rectangle("fill", 0, 0, W, H)

    local fonts = self._fonts

    -- Title
    lg.setFont(fonts.title)
    local glow = 0.5 + 0.2 * math.sin(self._timer * 2)
    lg.setColor(0.4, 0.9, 1.0, glow)
    lg.printf("ACHIEVEMENTS", 0, 30, W, "center")
    lg.setColor(0.4, 0.9, 1.0, 1)
    lg.printf("ACHIEVEMENTS", 0, 30, W, "center")

    -- Separator
    lg.setColor(0.3, 0.6, 0.8, 0.4)
    lg.setLineWidth(1)
    lg.line(W * 0.15, 65, W * 0.85, 65)

    -- Achievement cards
    local all = achievementSystem.getAll()
    local cardH = 90
    local padding = 10
    local startY = 80
    local cardW = W * 0.85
    local cardX = (W - cardW) / 2

    for i, ach in ipairs(all) do
        local y = startY + (i - 1) * (cardH + padding)

        -- Card background
        if ach.unlocked then
            lg.setColor(0.05, 0.15, 0.1, 0.8)
        else
            lg.setColor(0.06, 0.06, 0.1, 0.8)
        end
        lg.rectangle("fill", cardX, y, cardW, cardH, 6, 6)

        -- Card border
        if ach.unlocked then
            lg.setColor(0.2, 0.8, 0.4, 0.6)
        else
            lg.setColor(0.3, 0.3, 0.4, 0.4)
        end
        lg.setLineWidth(1)
        lg.rectangle("line", cardX, y, cardW, cardH, 6, 6)

        -- Status icon
        local iconX = cardX + 12
        local iconY = y + 12
        if ach.unlocked then
            -- Checkmark circle
            lg.setColor(0.2, 1.0, 0.5, 0.9)
            lg.circle("fill", iconX + 10, iconY + 10, 10)
            lg.setColor(0, 0, 0, 1)
            lg.setFont(fonts.desc)
            lg.print("OK", iconX + 3, iconY + 4)
        else
            -- Lock circle
            lg.setColor(0.4, 0.4, 0.5, 0.6)
            lg.circle("line", iconX + 10, iconY + 10, 10)
            lg.setFont(fonts.desc)
            lg.setColor(0.4, 0.4, 0.5, 0.6)
            lg.print("?", iconX + 6, iconY + 4)
        end

        -- Name + Description
        local textX = cardX + 40
        lg.setFont(fonts.name)
        if ach.unlocked then
            lg.setColor(0.3, 1.0, 0.6, 1)
        else
            lg.setColor(0.7, 0.7, 0.8, 0.9)
        end
        lg.print(ach.name, textX, y + 10)

        lg.setFont(fonts.desc)
        lg.setColor(0.5, 0.5, 0.6, 0.8)
        lg.print(ach.desc, textX, y + 30)

        -- Progress bar
        local barX = textX
        local barY = y + 50
        local barW = cardW - 60
        local barH = 8
        local ratio = ach.target > 0 and (ach.progress / ach.target) or 0
        if ratio > 1 then ratio = 1 end

        -- Bar background
        lg.setColor(0.15, 0.15, 0.2, 0.6)
        lg.rectangle("fill", barX, barY, barW, barH, 3, 3)
        -- Bar fill
        if ach.unlocked then
            lg.setColor(0.2, 0.8, 0.4, 0.8)
        else
            lg.setColor(0.3, 0.5, 0.8, 0.7)
        end
        lg.rectangle("fill", barX, barY, barW * ratio, barH, 3, 3)
        -- Progress text
        lg.setColor(0.6, 0.6, 0.7, 0.8)
        lg.printf(string.format("%d / %d", ach.progress, ach.target),
            barX, barY + barH + 2, barW, "right")

        -- Reward
        local rewardText
        if ach.reward.type == "weapon" then
            rewardText = string.format("Weapon: %s%s", ach.reward.name,
                ach.unlocked and "" or " (COMING SOON)")
        elseif ach.reward.type == "character" then
            rewardText = string.format("Character: %s", ach.reward.name)
        elseif ach.reward.type == "passive" then
            rewardText = string.format("Passive: %s", ach.reward.name)
        end

        if rewardText then
            if ach.unlocked then
                lg.setColor(1.0, 0.9, 0.3, 0.9)
            else
                lg.setColor(0.5, 0.5, 0.5, 0.5)
            end
            lg.printf(rewardText, barX, barY + barH + 2, barW, "left")
        end
    end

    -- Hint
    lg.setFont(fonts.desc)
    local alpha = 0.4 + 0.2 * math.sin(self._timer * 2)
    lg.setColor(0.5, 0.5, 0.5, alpha)
    lg.printf("[ESC] Back", 0, H - 30, W, "center")

    lg.setColor(1, 1, 1, 1)
end

function AchievementScene:keypressed(key)
    if key == "escape" or key == "return" or key == "space" then
        self._sceneStack:pop()
        return true
    end
    return false
end

function AchievementScene:touchpressed(id, x, y, dx, dy, pressure)
    self._sceneStack:pop()
    return true
end

return AchievementScene
