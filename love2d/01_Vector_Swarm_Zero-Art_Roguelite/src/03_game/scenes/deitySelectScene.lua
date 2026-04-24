-- deitySelectScene.lua
-- Deity(신) 선택 전용 씬
-- PLAY 직후 표시, 4신 중 택1 → 선택 후 playScene으로 전환
-- 곡선 애니메이션 + 터치/클릭 선택

local lg = love.graphics
local lw = love.window

local curveDefs = require("03_game.data.curveDefs")
local deityDefs = require("03_game.data.deityDefs")

local _sin   = math.sin
local _cos   = math.cos
local _pi    = math.pi
local _floor = math.floor
local _min   = math.min
local _max   = math.max

-- 곡선 드로잉 애니메이션 상수
local _DRAW_DURATION = 1.2   -- 각 곡선 드로잉 소요 시간(초)
local _DRAW_STAGGER  = 0.15  -- 카드 간 시작 시차(초)

local DeitySelectScene = {}
DeitySelectScene.__index = DeitySelectScene

DeitySelectScene.name        = "DeitySelectScene"
DeitySelectScene.transparent = false
DeitySelectScene.drawBelow   = false

-- ─── Curve sampling (curveLabScene 패턴 재활용) ────────────────────

local function _sampleCurveWorld(curve, steps)
    local verts = {}
    if not curve then return verts end
    if curve.fn == "custom" then
        verts = curve.customFn(steps)
    elseif curve.fn == "parametric" then
        local t0, t1 = curve.tRange[1], curve.tRange[2]
        for i = 0, steps - 1 do
            local t = t0 + (t1 - t0) * i / steps
            local x, y = curve.paramFn(t)
            verts[#verts + 1] = x
            verts[#verts + 1] = y
        end
    else
        local t0, t1 = curve.tRange[1], curve.tRange[2]
        for i = 0, steps - 1 do
            local t = t0 + (t1 - t0) * i / steps
            local r = curve.fn(t)
            verts[#verts + 1] = r * _cos(t)
            verts[#verts + 1] = r * _sin(t)
        end
    end
    return verts
end

--- 곡선 정점을 중심 기준 정규화 (단위 반지름 = 1)
local function _normalizeVerts(verts)
    if #verts < 4 then return verts end
    local n = #verts / 2
    local sx, sy = 0, 0
    for i = 1, #verts, 2 do
        sx = sx + verts[i]
        sy = sy + verts[i + 1]
    end
    local cx, cy = sx / n, sy / n

    local maxR = 0
    for i = 1, #verts, 2 do
        local dx, dy = verts[i] - cx, verts[i + 1] - cy
        local r = (dx * dx + dy * dy) ^ 0.5
        if r > maxR then maxR = r end
    end

    local scale = maxR > 0 and (1 / maxR) or 1
    local out = {}
    for i = 1, #verts, 2 do
        out[#out + 1] = (verts[i] - cx) * scale
        out[#out + 1] = (verts[i + 1] - cy) * scale
    end
    return out
end

-- ─── Constructor ──────────────────────────────────────────────────

function DeitySelectScene.new(sceneStack)
    local self = setmetatable({}, DeitySelectScene)
    self._sceneStack = sceneStack
    self._cards      = {}
    self._angle      = 0
    self._selected   = nil       -- 선택된 카드 인덱스
    self._selectTimer = 0        -- 선택 후 연출 타이머
    self._SELECT_DELAY = 0.6     -- 선택 후 전환까지 대기
    self._entranceTimer = 0      -- 입장 연출 타이머
    self._drawTimer  = 0         -- 곡선 드로잉 타이머
    return self
end

function DeitySelectScene:enter(prev)
    self._angle = 0
    self._selected = nil
    self._selectTimer = 0
    self._entranceTimer = 0
    self._drawTimer = 0

    -- 폰트 캐시 (매 프레임 newFont 방지)
    self._fontTitle = love.graphics.newFont(24)
    self._fontSub   = love.graphics.newFont(12)
    self._fontName  = love.graphics.newFont(16)
    self._fontSig   = love.graphics.newFont(10)
    self._fontDesc  = love.graphics.newFont(9)
    self._fontStat  = love.graphics.newFont(8)

    -- 각 deity에 대해 곡선 정점 캐시
    self._cards = {}
    for i, deity in ipairs(deityDefs.DEITIES) do
        local curveDef = nil
        for _, c in ipairs(curveDefs) do
            if c.name == deity.curveName then
                curveDef = c; break
            end
        end

        local rawVerts = curveDef and _sampleCurveWorld(curveDef, curveDef.defaultSteps or 150) or {}
        local normVerts = _normalizeVerts(rawVerts)

        self._cards[i] = {
            deity    = deity,
            verts    = normVerts,
            hover    = false,
            pulse    = 0,
            drawProgress = 0,   -- 곡선 드로잉 진행도 (0→1)
        }
    end

    logInfo("[DEITY] DeitySelectScene entered")
end

function DeitySelectScene:exit()
    logInfo("[DEITY] DeitySelectScene exited")
end

-- ─── Update ───────────────────────────────────────────────────────

function DeitySelectScene:update(dt)
    self._angle = self._angle + dt * 0.5   -- 천천히 회전
    self._entranceTimer = _min(self._entranceTimer + dt * 2.0, 1.0)
    self._drawTimer = self._drawTimer + dt

    -- 카드별 드로잉 진행도 (시차 + ease-out cubic: 빠른 시작 → 부드러운 마무리)
    for i, card in ipairs(self._cards) do
        local stagger = (i - 1) * _DRAW_STAGGER
        local t = _max(0, _min((self._drawTimer - stagger) / _DRAW_DURATION, 1))
        local inv = 1 - t
        card.drawProgress = 1 - inv * inv * inv   -- ease-out cubic
    end

    -- 카드별 pulse
    for _, card in ipairs(self._cards) do
        if card.hover then
            card.pulse = _min(card.pulse + dt * 4, 1)
        else
            card.pulse = _max(card.pulse - dt * 4, 0)
        end
    end

    -- 선택 후 대기
    if self._selected then
        self._selectTimer = self._selectTimer + dt
        if self._selectTimer >= self._SELECT_DELAY then
            local deityId = self._cards[self._selected].deity.id
            local PlayScene = require("03_game.scenes.playScene")
            self._sceneStack:replace(PlayScene.new(self._sceneStack, deityId))
        end
    end
end

-- ─── Draw ─────────────────────────────────────────────────────────

function DeitySelectScene:draw()
    local W, H = lg.getDimensions()

    -- 배경
    lg.clear(0.02, 0.02, 0.06, 1)

    -- 입장 페이드
    local alpha = self._entranceTimer

    -- 타이틀
    lg.setFont(self._fontTitle)
    setColor(255, 255, 255, _floor(alpha * 220))
    lg.printf("Choose Your Deity", 0, H * 0.04, W, "center")

    -- 서브타이틀
    lg.setFont(self._fontSub)
    setColor(180, 180, 200, _floor(alpha * 160))
    lg.printf("A blessing for the journey ahead", 0, H * 0.04 + 30, W, "center")

    -- 카드 레이아웃: 2×2 그리드
    local cardW = W * 0.42
    local cardH = H * 0.38
    local gapX  = W * 0.04
    local gapY  = H * 0.03
    local startX = (W - cardW * 2 - gapX) / 2
    local startY = H * 0.14

    local positions = {
        {startX,              startY},
        {startX + cardW + gapX, startY},
        {startX,              startY + cardH + gapY},
        {startX + cardW + gapX, startY + cardH + gapY},
    }

    for i, card in ipairs(self._cards) do
        if positions[i] then
            self:_drawCard(card, i, positions[i][1], positions[i][2], cardW, cardH, alpha)
        end
    end

    resetColor()
end

function DeitySelectScene:_drawCard(card, index, x, y, w, h, alpha)
    local deity = card.deity
    local isSelected = (self._selected == index)
    local isOther    = (self._selected ~= nil and not isSelected)

    -- 선택 후 다른 카드 페이드아웃
    local cardAlpha = alpha
    if isOther then
        cardAlpha = cardAlpha * _max(0, 1 - self._selectTimer / self._SELECT_DELAY * 2)
    end
    if cardAlpha < 0.01 then return end

    -- 선택된 카드 확대 효과
    local scale = 1.0
    if isSelected then
        scale = 1.0 + self._selectTimer / self._SELECT_DELAY * 0.15
    end

    local cr, cg, cb = deity.color[1], deity.color[2], deity.color[3]
    local pulse = card.pulse

    -- 카드 배경
    local bgAlpha = (0.08 + pulse * 0.06) * cardAlpha
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(bgAlpha * 255))
    lg.rectangle("fill", x, y, w, h, 8, 8)

    -- 카드 테두리
    local borderAlpha = (0.3 + pulse * 0.4) * cardAlpha
    if isSelected then borderAlpha = cardAlpha end
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(borderAlpha * 255))
    lg.setLineWidth(isSelected and 3 or (1 + pulse))
    lg.rectangle("line", x, y, w, h, 8, 8)

    -- 곡선 렌더링 (드로잉 애니메이션)
    local curveRadius = _min(w, h * 0.55) * 0.38 * scale
    local cx, cy = x + w / 2, y + h * 0.38
    local angle = self._angle + index * _pi / 4   -- 각 카드 위상 차이

    local totalPairs = #card.verts / 2
    local visiblePairs = _floor(card.drawProgress * totalPairs)

    if visiblePairs >= 2 then
        setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(cardAlpha * 240))
        lg.setLineWidth(2)

        local screenVerts = {}
        local cosA, sinA = _cos(angle), _sin(angle)
        for j = 1, visiblePairs * 2, 2 do
            local vx, vy = card.verts[j], card.verts[j + 1]
            -- 회전 변환
            local rx = vx * cosA - vy * sinA
            local ry = vx * sinA + vy * cosA
            screenVerts[#screenVerts + 1] = cx + rx * curveRadius
            screenVerts[#screenVerts + 1] = cy + ry * curveRadius
        end

        if #screenVerts >= 4 then
            lg.line(screenVerts)
        end

        -- 펜 끝 글로우 (드로잉 진행 중일 때만)
        if card.drawProgress < 1 and #screenVerts >= 2 then
            local tipX = screenVerts[#screenVerts - 1]
            local tipY = screenVerts[#screenVerts]
            -- 밝은 코어
            setColor(255, 255, 255, _floor(cardAlpha * 200))
            lg.circle("fill", tipX, tipY, 3)
            -- 컬러 글로우
            setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(cardAlpha * 100))
            lg.circle("fill", tipX, tipY, 7)
        end
    end

    -- 텍스트 (곡선 드로잉 80% 이후 페이드인)
    local textAlpha = _max(0, (card.drawProgress - 0.8) / 0.2) * cardAlpha

    -- 이름
    lg.setFont(self._fontName)
    setColor(255, 255, 255, _floor(textAlpha * 230))
    lg.printf(deity.name, x, y + h * 0.70, w, "center")

    -- 시그니처 능력
    lg.setFont(self._fontSig)
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 200))
    lg.printf(deity.signature.name, x, y + h * 0.70 + 22, w, "center")

    -- 설명
    lg.setFont(self._fontDesc)
    setColor(200, 200, 220, _floor(textAlpha * 160))
    lg.printf(deity.signature.desc, x + 4, y + h * 0.70 + 38, w - 8, "center")

    -- 스탯 보너스
    lg.setFont(self._fontStat)
    setColor(160, 160, 180, _floor(textAlpha * 140))
    local statText = deity.statBonuses[1].label .. "  |  " .. deity.statBonuses[2].label
    lg.printf(statText, x + 4, y + h * 0.70 + 54, w - 8, "center")

    lg.setLineWidth(1)
end

-- ─── Input ────────────────────────────────────────────────────────

function DeitySelectScene:_hitTest(sx, sy)
    local W, H = lg.getDimensions()
    local cardW = W * 0.42
    local cardH = H * 0.38
    local gapX  = W * 0.04
    local gapY  = H * 0.03
    local startX = (W - cardW * 2 - gapX) / 2
    local startY = H * 0.14

    local positions = {
        {startX,              startY},
        {startX + cardW + gapX, startY},
        {startX,              startY + cardH + gapY},
        {startX + cardW + gapX, startY + cardH + gapY},
    }

    for i, pos in ipairs(positions) do
        if i <= #self._cards then
            if sx >= pos[1] and sx <= pos[1] + cardW
               and sy >= pos[2] and sy <= pos[2] + cardH then
                return i
            end
        end
    end
    return nil
end

function DeitySelectScene:touchpressed(id, x, y)
    if self._selected then return true end
    -- 드로잉 완료 전엔 선택 불가
    if self._cards[#self._cards] and self._cards[#self._cards].drawProgress < 1 then return false end
    local hit = self:_hitTest(x, y)
    if hit then
        self._selected = hit
        self._selectTimer = 0
        logInfo(string.format("[DEITY] Selected: %s", self._cards[hit].deity.name))
        return true
    end
    return false
end

function DeitySelectScene:mousepressed(x, y, button)
    if button == 1 then
        return self:touchpressed(nil, x, y)
    end
    return false
end

function DeitySelectScene:mousemoved(x, y)
    if self._selected then return end
    local hit = self:_hitTest(x, y)
    for i, card in ipairs(self._cards) do
        card.hover = (i == hit)
    end
end

function DeitySelectScene:keypressed(key)
    if self._selected then return true end
    -- 드로잉 완료 전엔 선택 불가
    if self._cards[#self._cards] and self._cards[#self._cards].drawProgress < 1 then return false end

    -- 숫자 키 1-4로 선택
    local num = tonumber(key)
    if num and num >= 1 and num <= #self._cards then
        self._selected = num
        self._selectTimer = 0
        logInfo(string.format("[DEITY] Selected via key: %s", self._cards[num].deity.name))
        return true
    end

    return false
end

return DeitySelectScene
