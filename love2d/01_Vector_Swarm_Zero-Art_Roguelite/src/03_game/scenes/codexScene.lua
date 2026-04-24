-- CodexScene
-- 곡선 도감: 전체 53+ 곡선 감상 + Deity 4신 특별 정보
-- 좌우 키로 1곡선씩 전환, 풀화면 레이아웃
-- drawBelow=true 오버레이 (타이틀 위에 push)
--
-- ◆ 접근: 타이틀 메뉴 CODEX 항목 또는 D키
-- ◆ 조작: ←/→ 페이지 전환, ESC 닫기
-- ◆ 애니메이션: 보통 속도 드로잉 → 완성 후 정지 → 2배 느린 드로잉 → 정지 → 반복
--
-- ◆ 확장 계획
--   업적 기반 잠금/해금 (achievementSystem 연동)
--   해금 시 화려한 연출 (파티클, 글로우 강화)

local lg = love.graphics

local curveDefs = require("03_game.data.curveDefs")
local deityDefs = require("03_game.data.deityDefs")

local _sin   = math.sin
local _cos   = math.cos
local _pi    = math.pi
local _floor = math.floor
local _min   = math.min
local _max   = math.max
local _abs   = math.abs

-- 드로잉 애니메이션 상수
local _DRAW_BASE_STEPS   = 200    -- 기준 step 수 (이 값일 때 기본 시간)
local _DRAW_SPEED_NORMAL = 1.5    -- 보통 속도 기준 (초, 200 steps 기준)
local _DRAW_SPEED_SLOW   = 3.0    -- 느린 속도 기준 (초, 200 steps 기준)
local _HOLD_DURATION     = 1.2    -- 완성 후 정지 시간 (초)

-- 페이지 전환 애니메이션
local _TRANSITION_SPEED = 5.0

-- Deity curveName → deity 역방향 룩업 테이블 (모듈 로드 시 1회 생성)
local _DEITY_BY_CURVE = {}
for _, d in ipairs(deityDefs.DEITIES) do
    _DEITY_BY_CURVE[d.curveName] = d
end

-- 일반 곡선 기본 색상 (family별, 파스텔 톤)
local _FAMILY_COLORS = {
    rose       = {0.95, 0.65, 0.72},  -- 파스텔 핑크
    lemniscate = {0.65, 0.90, 0.72},  -- 파스텔 민트
    cycloid    = {0.65, 0.78, 0.95},  -- 파스텔 스카이
    trochoid   = {0.78, 0.68, 0.92},  -- 파스텔 라벤더
    spiral     = {0.95, 0.78, 0.62},  -- 파스텔 피치
    polar      = {0.82, 0.72, 0.88},  -- 파스텔 라일락
    parametric = {0.62, 0.88, 0.85},  -- 파스텔 아쿠아
    custom     = {0.92, 0.75, 0.70},  -- 파스텔 살몬
}
local _DEFAULT_COLOR = {0.72, 0.75, 0.90}  -- 파스텔 페리윙클

local CodexScene = {}
CodexScene.__index = CodexScene

CodexScene.name        = "CodexScene"
CodexScene.transparent = false
CodexScene.drawBelow   = true

-- ─── Curve sampling ───────────────────────────────────────────────

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

function CodexScene.new(sceneStack)
    local self = setmetatable({}, CodexScene)
    self._sceneStack = sceneStack
    self._pages  = {}
    self._pageIndex = 1
    self._angle  = 0
    self._drawTimer = 0
    self._drawCycle = 0          -- 0=보통, 1=느림 (토글)
    self._transitionOffset = 0
    self._fonts  = nil
    return self
end

function CodexScene:enter(prev)
    self._angle = 0
    self._drawTimer = 0
    self._drawCycle = 0
    self._transitionOffset = 0
    self._pageIndex = 1

    self._fonts = {
        title   = lg.newFont(28),
        name    = lg.newFont(22),
        sigName = lg.newFont(16),
        lore    = lg.newFont(11),
        desc    = lg.newFont(12),
        stat    = lg.newFont(11),
        nav     = lg.newFont(14),
        formula = lg.newFont(10),
        tag     = lg.newFont(9),
    }

    -- 전체 곡선 페이지 빌드
    self._pages = {}
    for i = 1, #curveDefs do
        local c = curveDefs[i]
        local rawVerts = _sampleCurveWorld(c, c.defaultSteps or 200)
        local normVerts = _normalizeVerts(rawVerts)
        local deity = _DEITY_BY_CURVE[c.name]  -- nil이면 일반 곡선
        -- 실제 정점 쌍 수를 기준으로 duration 비율 계산 (custom 곡선 대응)
        local vertPairs = #normVerts / 2

        self._pages[i] = {
            curveDef     = c,
            deity        = deity,
            verts        = normVerts,
            vertPairs    = vertPairs,
            drawProgress = 0,
        }
    end

    logInfo(string.format("[CODEX] CodexScene entered (%d curves)", #self._pages))
end

function CodexScene:exit()
    logInfo("[CODEX] CodexScene exited")
end

-- ─── Update ───────────────────────────────────────────────────────

function CodexScene:update(dt)
    self._angle = self._angle + dt * 0.3
    self._drawTimer = self._drawTimer + dt

    -- 현재 페이지 드로잉 진행도 (속도 사이클: 보통 ↔ 느림, 정점 수에 비례)
    local page = self._pages[self._pageIndex]
    if page then
        local pairs = _max(page.vertPairs, 10)
        local ratio = pairs / _DRAW_BASE_STEPS
        -- complexity 3 곡선 (Butterfly 등): 복잡한 경로를 천천히 감상
        local complexity = page.curveDef.complexity or 2
        if complexity >= 3 then ratio = ratio * 1.6 end
        local baseDuration = self._drawCycle == 0 and _DRAW_SPEED_NORMAL or _DRAW_SPEED_SLOW
        local duration = baseDuration * ratio
        local totalCycle = duration + _HOLD_DURATION

        local cycleTime = self._drawTimer % totalCycle   -- 현재 사이클 내 위치
        if cycleTime < duration then
            -- 드로잉 중 (ease-out cubic)
            local t = _min(cycleTime / duration, 1)
            local inv = 1 - t
            page.drawProgress = 1 - inv * inv * inv
        else
            -- 홀드 중 (완성 상태)
            page.drawProgress = 1.0

            -- 사이클 전환 체크: 홀드 끝 → 다음 사이클로
            local holdElapsed = cycleTime - duration
            if holdElapsed >= _HOLD_DURATION - dt then
                -- 다음 사이클 시작
                self._drawCycle = 1 - self._drawCycle  -- 0↔1 토글
                self._drawTimer = 0
                page.drawProgress = 0
            end
        end
    end

    -- 전환 애니메이션 (0으로 수렴)
    if _abs(self._transitionOffset) > 0.001 then
        self._transitionOffset = self._transitionOffset * (1 - dt * _TRANSITION_SPEED)
        if _abs(self._transitionOffset) < 0.005 then
            self._transitionOffset = 0
        end
    end
end

-- ─── Draw ─────────────────────────────────────────────────────────

function CodexScene:draw()
    local W, H = lg.getDimensions()

    -- 어두운 오버레이
    lg.setColor(0, 0, 0, 0.92)
    lg.rectangle("fill", 0, 0, W, H)

    -- 타이틀
    lg.setFont(self._fonts.title)
    setColor(255, 255, 255, 200)
    lg.printf("CURVE CODEX", 0, H * 0.03, W, "center")

    -- 페이지 카운터
    lg.setFont(self._fonts.formula)
    setColor(140, 140, 160, 150)
    lg.printf(string.format("%d / %d", self._pageIndex, #self._pages), 0, H * 0.03 + 34, W, "center")

    -- 현재 페이지 그리기
    local page = self._pages[self._pageIndex]
    if not page then return end

    local slideX = self._transitionOffset * W * 0.3
    self:_drawPage(page, W, H, slideX)

    -- 네비게이션 힌트
    self:_drawNav(W, H)

    resetColor()
end

function CodexScene:_getColor(page)
    if page.deity then
        return page.deity.color[1], page.deity.color[2], page.deity.color[3]
    end
    local family = page.curveDef.family or "polar"
    local col = _FAMILY_COLORS[family] or _DEFAULT_COLOR
    return col[1], col[2], col[3]
end

function CodexScene:_drawPage(page, W, H, offsetX)
    local cr, cg, cb = self:_getColor(page)
    local progress = page.drawProgress

    -- ─── 곡선 렌더링 (화면 중앙 상단, 크게) ─────────────────────
    local curveRadius = _min(W, H) * 0.22
    local cx = W / 2 + offsetX
    local cy = H * 0.30
    local angle = self._angle

    local totalPairs = #page.verts / 2
    local visiblePairs = _floor(progress * totalPairs)

    if visiblePairs >= 2 then
        -- complexity에 따른 선 두께 (복잡한 곡선은 얇게)
        local complexity = page.curveDef.complexity or 2
        local glowWidth = complexity >= 3 and 4 or 6
        local mainWidth = complexity >= 3 and 1.5 or 2.5

        -- 곡선 글로우 (넓은 반투명)
        setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), 40)
        lg.setLineWidth(glowWidth)
        local glowVerts = self:_transformVerts(page.verts, visiblePairs, cx, cy, curveRadius, angle)
        if #glowVerts >= 4 then lg.line(glowVerts) end

        -- 곡선 메인
        setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(progress * 240))
        lg.setLineWidth(mainWidth)
        local mainVerts = self:_transformVerts(page.verts, visiblePairs, cx, cy, curveRadius, angle)
        if #mainVerts >= 4 then lg.line(mainVerts) end

        -- 펜 팁 글로우 (드로잉 중에만)
        if progress < 1 and #mainVerts >= 2 then
            local tipX = mainVerts[#mainVerts - 1]
            local tipY = mainVerts[#mainVerts]
            setColor(255, 255, 255, 220)
            lg.circle("fill", tipX, tipY, 4)
            setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), 120)
            lg.circle("fill", tipX, tipY, 10)
        end
    end

    -- ─── 텍스트 (곡선 완료 후 페이드인) ──────────────────────────
    local textAlpha = _max(0, (progress - 0.7) / 0.3)
    local textY = H * 0.50 + offsetX * 0.2

    -- 곡선 이름
    lg.setFont(self._fonts.name)
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 255))
    lg.printf(page.curveDef.name, 0, textY, W, "center")

    -- 수식
    lg.setFont(self._fonts.formula)
    setColor(160, 160, 180, _floor(textAlpha * 150))
    local formulaText = page.curveDef.formula or ""
    lg.printf(formulaText, 20, textY + 30, W - 40, "center")

    if page.deity then
        -- ─── Deity 특별 정보 ─────────────────────────────────────
        self:_drawDeityInfo(page, W, H, textY, textAlpha, cr, cg, cb)
    else
        -- ─── 일반 곡선 정보 ──────────────────────────────────────
        self:_drawCurveInfo(page, W, H, textY, textAlpha, cr, cg, cb)
    end

    lg.setLineWidth(1)
end

function CodexScene:_drawDeityInfo(page, W, H, textY, textAlpha, cr, cg, cb)
    local deity = page.deity

    -- Lore
    lg.setFont(self._fonts.lore)
    setColor(200, 200, 220, _floor(textAlpha * 180))
    lg.printf("\"" .. deity.lore .. "\"", 30, textY + 52, W - 60, "center")

    -- 구분선
    local lineY = textY + 80
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 60))
    lg.setLineWidth(1)
    lg.line(W * 0.2, lineY, W * 0.8, lineY)

    -- Deity 뱃지
    lg.setFont(self._fonts.tag)
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 200))
    lg.printf("DEITY: " .. deity.name, 0, lineY + 6, W, "center")

    -- 시그니처 능력
    local sigY = lineY + 24
    lg.setFont(self._fonts.sigName)
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 230))
    lg.printf(deity.signature.name, 0, sigY, W, "center")

    lg.setFont(self._fonts.desc)
    setColor(220, 220, 240, _floor(textAlpha * 200))
    lg.printf(deity.signature.desc, 30, sigY + 24, W - 60, "center")

    -- 트리거 + 확률
    lg.setFont(self._fonts.formula)
    local triggerLabel = ({
        on_graze = "Trigger: Graze (bullet near-miss)",
        on_hit   = "Trigger: On Hit (each bullet)",
        on_kill  = "Trigger: On Kill (enemy defeated)",
    })[deity.signature.trigger] or deity.signature.trigger
    setColor(140, 140, 160, _floor(textAlpha * 130))
    lg.printf(triggerLabel, 0, sigY + 46, W, "center")
    local chanceText = string.format("Chance: %d%%", deity.signature.chance * 100)
    lg.printf(chanceText, 0, sigY + 62, W, "center")

    -- 구분선 2
    local line2Y = sigY + 82
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 40))
    lg.line(W * 0.25, line2Y, W * 0.75, line2Y)

    -- 패시브 스탯 보너스
    local statY = line2Y + 14
    lg.setFont(self._fonts.stat)
    for j, bonus in ipairs(deity.statBonuses) do
        setColor(180, 180, 200, _floor(textAlpha * 180))
        lg.printf("+ " .. bonus.label, 0, statY + (j - 1) * 20, W, "center")
    end
end

function CodexScene:_drawCurveInfo(page, W, H, textY, textAlpha, cr, cg, cb)
    local c = page.curveDef

    -- Meta 태그 줄
    local tags = {}
    if c.family then tags[#tags + 1] = c.family end
    if c.closed then tags[#tags + 1] = "closed" else tags[#tags + 1] = "open" end
    if c.complexity then tags[#tags + 1] = "complexity " .. c.complexity end
    if c.discontinuous then tags[#tags + 1] = "discontinuous" end
    if c.enemyFriendly then tags[#tags + 1] = "enemy-ready" end

    local tagStr = table.concat(tags, "  ·  ")
    lg.setFont(self._fonts.tag)
    setColor(140, 140, 160, _floor(textAlpha * 140))
    lg.printf(tagStr, 20, textY + 52, W - 40, "center")

    -- 구분선
    local lineY = textY + 72
    setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), _floor(textAlpha * 40))
    lg.setLineWidth(1)
    lg.line(W * 0.25, lineY, W * 0.75, lineY)

    -- 타입 정보
    local infoY = lineY + 14
    lg.setFont(self._fonts.desc)
    setColor(180, 180, 200, _floor(textAlpha * 170))

    local fnType = c.fn
    if type(fnType) == "function" then fnType = "polar" end
    lg.printf("Type: " .. fnType, 0, infoY, W, "center")

    -- tRange
    if c.tRange then
        local rangeText = string.format("Range: [%.2f, %.2f]", c.tRange[1], c.tRange[2])
        lg.printf(rangeText, 0, infoY + 20, W, "center")
    end

    -- Steps
    if c.defaultSteps then
        setColor(140, 140, 160, _floor(textAlpha * 130))
        lg.setFont(self._fonts.formula)
        lg.printf(string.format("Steps: %d", c.defaultSteps), 0, infoY + 42, W, "center")
    end
end

function CodexScene:_transformVerts(verts, visiblePairs, cx, cy, radius, angle)
    local cosA, sinA = _cos(angle), _sin(angle)
    local out = {}
    for j = 1, visiblePairs * 2, 2 do
        local vx, vy = verts[j], verts[j + 1]
        local rx = vx * cosA - vy * sinA
        local ry = vx * sinA + vy * cosA
        out[#out + 1] = cx + rx * radius
        out[#out + 1] = cy + ry * radius
    end
    return out
end

function CodexScene:_drawNav(W, H)
    local total = #self._pages
    local idx = self._pageIndex

    -- 페이지 인디케이터 (하단 미니 도트, 53개니까 작게)
    local dotY = H * 0.94
    local maxDotsVisible = 15   -- 한 번에 보이는 최대 도트 수
    local dotSpacing = 10
    local halfWindow = _floor(maxDotsVisible / 2)
    local dotStart = _max(1, _min(idx - halfWindow, total - maxDotsVisible + 1))
    local dotEnd   = _min(total, dotStart + maxDotsVisible - 1)
    local dotCount = dotEnd - dotStart + 1
    local dotStartX = W / 2 - (dotCount - 1) * dotSpacing / 2

    for i = dotStart, dotEnd do
        local dx = dotStartX + (i - dotStart) * dotSpacing
        local cr, cg, cb = self:_getColor(self._pages[i])
        if i == idx then
            setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), 255)
            lg.circle("fill", dx, dotY, 4)
        else
            local dist = _abs(i - idx)
            local alpha = _max(40, 120 - dist * 15)
            setColor(_floor(cr * 255), _floor(cg * 255), _floor(cb * 255), alpha)
            lg.circle("fill", dx, dotY, 2)
        end
    end

    -- 좌우 화살표 힌트
    lg.setFont(self._fonts.nav)
    if idx > 1 then
        setColor(200, 200, 220, 140)
        lg.printf("<", 10, H * 0.91, 30, "left")
    end
    if idx < total then
        setColor(200, 200, 220, 140)
        lg.printf(">", W - 40, H * 0.91, 30, "right")
    end

    -- ESC 힌트 + 속도 표시
    setColor(120, 120, 140, 100)
    lg.setFont(self._fonts.formula)
    local speedLabel = self._drawCycle == 0 and "normal" or "slow"
    lg.printf("ESC close  |  " .. speedLabel, 0, H * 0.97, W, "center")
end

-- ─── Navigation ───────────────────────────────────────────────────

function CodexScene:_goToPage(newIndex)
    if newIndex < 1 or newIndex > #self._pages then return end
    local dir = newIndex > self._pageIndex and 1 or -1
    self._pageIndex = newIndex
    self._drawTimer = 0
    self._drawCycle = 0
    self._pages[newIndex].drawProgress = 0
    self._transitionOffset = dir
end

-- ─── Input ────────────────────────────────────────────────────────

function CodexScene:keypressed(key)
    if key == "escape" then
        self._sceneStack:pop()
        return true
    elseif key == "left" or key == "a" then
        self:_goToPage(self._pageIndex - 1)
        return true
    elseif key == "right" or key == "d" then
        self:_goToPage(self._pageIndex + 1)
        return true
    elseif key == "home" then
        self:_goToPage(1)
        return true
    elseif key == "end" then
        self:_goToPage(#self._pages)
        return true
    end
    return false
end

function CodexScene:touchpressed(id, x, y)
    local W = lg.getWidth()
    if x < W * 0.3 then
        self:_goToPage(self._pageIndex - 1)
        return true
    elseif x > W * 0.7 then
        self:_goToPage(self._pageIndex + 1)
        return true
    end
    return false
end

function CodexScene:mousepressed(x, y, button)
    if button == 1 then
        return self:touchpressed(nil, x, y)
    end
    return false
end

return CodexScene
