-- Trail System
-- 플레이어 리본 트레일: 매 프레임 위치 기록 → 법선 오프셋 → triangle strip 메쉬
-- 평소: 얇은 시안 리본 / 대쉬 시: 폭 + 밝기 부스트

local System = require("01_core.system")
local world  = require("01_core.world")

local lg    = love.graphics
local sqrt  = math.sqrt
local min   = math.min
local max   = math.max

-- ─── 설정 ────────────────────────────────────────────────────
local MAX_POINTS       = 40       -- ring buffer 최대 크기
local POINT_LIFETIME   = 0.3      -- 포인트 기본 수명 (초)
local SAMPLE_DIST_SQ   = 0.001    -- 최소 샘플 거리² (너무 가까우면 스킵)

local BASE_HALF_WIDTH  = 0.03     -- 평소 리본 반폭
local BOOST_HALF_WIDTH = 0.08     -- 대쉬 시 리본 반폭
local BOOST_DURATION   = 0.35     -- 대쉬 부스트 지속 시간

local COLOR_R, COLOR_G, COLOR_B = 0, 0.9, 0.9   -- 시안 (0~1 범위)
local BASE_ALPHA  = 0.3           -- 평소 최대 알파
local BOOST_ALPHA = 0.8           -- 대쉬 시 최대 알파

-- ─── 트레일 데이터 (모듈 스코프 — 플레이어 1명 전제) ────────
local points = {}   -- { {x, y, age, maxAge, boosted}, ... } 최신이 마지막
local head = 0      -- ring buffer 대신 단순 배열 + shift 방식 (40개라 충분)
local mesh = nil    -- love.graphics.Mesh (재사용)
local meshVerts = {} -- 정점 버퍼 (재사용)

local trailBoostTimer = 0  -- 대쉬 부스트 남은 시간

-- ─── 모듈 API ────────────────────────────────────────────────
local TrailSystem = {}

--- 초기화 (playScene 리셋 시 호출)
function TrailSystem.reset()
    points = {}
    head = 0
    trailBoostTimer = 0
    mesh = nil
    meshVerts = {}
end

--- 대쉬 발동 시 호출 — 부스트 시작 + 경로 포인트 삽입
function TrailSystem.onDash(startX, startY, endX, endY)
    trailBoostTimer = BOOST_DURATION
    -- 대쉬 궤적을 보간 포인트로 삽입 (시작→끝 균등 배치)
    local INTERP_COUNT = 8
    for i = 1, INTERP_COUNT do
        local t = (i - 1) / (INTERP_COUNT - 1)
        local px = startX + (endX - startX) * t
        local py = startY + (endY - startY) * t
        points[#points + 1] = {
            x = px, y = py,
            age = 0,
            maxAge = BOOST_DURATION + 0.15,
            boosted = true,
        }
        -- ring buffer 크기 제한
        if #points > MAX_POINTS then
            table.remove(points, 1)
        end
    end
end

--- 매 프레임 위치 기록 + 에이징
function TrailSystem.update(dt, playerX, playerY)
    -- 부스트 타이머 감소
    if trailBoostTimer > 0 then
        trailBoostTimer = trailBoostTimer - dt
        if trailBoostTimer < 0 then trailBoostTimer = 0 end
    end

    -- 기존 포인트 에이징 + 만료 제거
    local i = 1
    while i <= #points do
        local p = points[i]
        p.age = p.age + dt
        if p.age >= p.maxAge then
            table.remove(points, i)
        else
            i = i + 1
        end
    end

    -- 새 포인트 추가 (최소 거리 체크)
    if playerX then
        local addPoint = true
        if #points > 0 then
            local last = points[#points]
            local dx = playerX - last.x
            local dy = playerY - last.y
            if dx * dx + dy * dy < SAMPLE_DIST_SQ then
                addPoint = false
            end
        end
        if addPoint then
            local isBoosted = trailBoostTimer > 0
            points[#points + 1] = {
                x = playerX, y = playerY,
                age = 0,
                maxAge = isBoosted and (BOOST_DURATION + 0.1) or POINT_LIFETIME,
                boosted = isBoosted,
            }
            if #points > MAX_POINTS then
                table.remove(points, 1)
            end
        end
    end
end

--- 리본 메쉬 빌드 + 드로잉 (camera transform 내부에서 호출)
function TrailSystem.draw()
    local n = #points
    if n < 2 then return end

    -- 정점 배열 생성 (triangle strip: 포인트마다 좌/우 2정점)
    local vertCount = n * 2
    -- meshVerts 재사용 (크기 맞추기)
    for vi = #meshVerts + 1, vertCount do
        meshVerts[vi] = {0, 0, 0, 0, 1, 1, 1, 1}  -- x, y, u, v, r, g, b, a
    end

    local wLeft, wBottom, wRight, wTop = world.getBounds()

    for i = 1, n do
        local p = points[i]
        local fade = 1.0 - (p.age / p.maxAge)     -- 1→0 (꼬리로 갈수록 희미)

        -- 법선 계산: 양쪽 세그먼트 법선의 평균 (매끄러운 꺾임)
        local nx, ny
        if i == 1 then
            local dx = points[2].x - p.x
            local dy = points[2].y - p.y
            local m = sqrt(dx * dx + dy * dy)
            if m < 0.0001 then m = 1; dx = 0; dy = 1 end
            nx, ny = -dy / m, dx / m
        elseif i == n then
            local dx = p.x - points[n - 1].x
            local dy = p.y - points[n - 1].y
            local m = sqrt(dx * dx + dy * dy)
            if m < 0.0001 then m = 1; dx = 0; dy = 1 end
            nx, ny = -dy / m, dx / m
        else
            -- 이전 세그먼트 법선
            local ax = p.x - points[i - 1].x
            local ay = p.y - points[i - 1].y
            local am = sqrt(ax * ax + ay * ay)
            if am < 0.0001 then am = 1; ax = 0; ay = 1 end
            local n1x, n1y = -ay / am, ax / am
            -- 다음 세그먼트 법선
            local bx = points[i + 1].x - p.x
            local by = points[i + 1].y - p.y
            local bm = sqrt(bx * bx + by * by)
            if bm < 0.0001 then bm = 1; bx = 0; by = 1 end
            local n2x, n2y = -by / bm, bx / bm
            -- 법선 평균
            nx = (n1x + n2x) * 0.5
            ny = (n1y + n2y) * 0.5
            local nm = sqrt(nx * nx + ny * ny)
            if nm < 0.1 then
                -- 거의 반대 방향 (U턴) → 이전 세그먼트 법선 그대로 사용
                nx, ny = n1x, n1y
            else
                nx, ny = nx / nm, ny / nm
            end
        end

        -- 반폭 결정 (균일 폭 — 페이드아웃은 알파만)
        local halfW
        if p.boosted then
            halfW = BOOST_HALF_WIDTH * fade
        else
            halfW = BASE_HALF_WIDTH * fade
        end

        -- 알파 결정
        local alpha
        if p.boosted then
            alpha = BOOST_ALPHA * fade
        else
            alpha = BASE_ALPHA * fade
        end

        -- 좌/우 정점
        local li = (i - 1) * 2 + 1  -- left vert index
        local ri = li + 1             -- right vert index

        local lv = meshVerts[li]
        lv[1] = max(wLeft, min(wRight, p.x + nx * halfW))
        lv[2] = max(wBottom, min(wTop, p.y + ny * halfW))
        lv[3] = 0   -- u
        lv[4] = (i - 1) / (n - 1)  -- v
        lv[5] = COLOR_R
        lv[6] = COLOR_G
        lv[7] = COLOR_B
        lv[8] = alpha

        local rv = meshVerts[ri]
        rv[1] = max(wLeft, min(wRight, p.x - nx * halfW))
        rv[2] = max(wBottom, min(wTop, p.y - ny * halfW))
        rv[3] = 1
        rv[4] = (i - 1) / (n - 1)
        rv[5] = COLOR_R
        rv[6] = COLOR_G
        rv[7] = COLOR_B
        rv[8] = alpha
    end

    -- 메쉬 생성 또는 리사이즈
    if not mesh or mesh:getVertexCount() ~= vertCount then
        mesh = lg.newMesh(
            {
                {"VertexPosition", "float", 2},
                {"VertexTexCoord", "float", 2},
                {"VertexColor",    "float", 4},
            },
            vertCount,
            "strip",
            "stream"
        )
    end

    -- 정점 데이터 세팅
    for vi = 1, vertCount do
        mesh:setVertex(vi, meshVerts[vi])
    end

    -- 블렌드 모드 additive로 발광 효과
    local prevMode, prevAlphaMode = lg.getBlendMode()
    lg.setBlendMode("add", "alphamultiply")
    lg.setColor(1, 1, 1, 1)  -- 정점 컬러 사용
    lg.draw(mesh)
    lg.setBlendMode(prevMode, prevAlphaMode)
end

return TrailSystem
