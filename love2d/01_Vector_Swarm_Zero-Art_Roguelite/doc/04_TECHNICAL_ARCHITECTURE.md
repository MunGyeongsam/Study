# 04. 기술 아키텍처 — Vector Swarm

> 마지막 갱신: 2026-04-19 (Phase 3A 완료 기준)
> 이 문서는 현재 코드베이스의 **실제 구현**을 기준으로 작성되었습니다.

---

## 1. 프로젝트 개요

- **엔진:** LÖVE2D 11.5 / Lua 5.1
- **플랫폼:** PC (Windows/macOS) 프로토타입, Mobile-First 목표
- **화면:** 432×960 (9:20 Portrait), 가변 해상도, VSync=1, MSAA=8

---

## 2. 폴더 구조

```
src/
├── main.lua               # LÖVE 콜백 (load/update/draw/input)
├── conf.lua               # LÖVE 창 설정 (432×960, 9:20 세로)
│
├── 00_common/             # 유틸리티 — 게임 의존성 없음
│   ├── global.lua           # 전역 함수 (log, clamp, lerp, setColor, …)
│   ├── logger.lua           # 4레벨 로깅 + 인게임 콘솔 (` 키)
│   ├── debug.lua            # 디버그 watch panel (F1)
│   ├── gridDebugDraw.lua    # 스크린 그리드 오버레이 (F4)
│   ├── kutil.lua            # 기타 유틸리티
│   └── math/
│       ├── vector.lua       # 2D 벡터 연산
│       └── matrix.lua       # 행렬 연산
│
├── 01_core/               # 엔진 레이어
│   ├── world.lua            # 월드 경계(120×250), 존 7개, 파워업/체크포인트
│   ├── ecs.lua              # ECS 코어 (엔티티/컴포넌트, componentIndex 캐시)
│   ├── system.lua           # 시스템 베이스 클래스 (성능 모니터링)
│   └── ecsManager.lua       # ECS 오케스트레이터 (시스템 등록, update/draw 분리)
│
├── 02_renderer/           # 카메라 & 렌더링
│   ├── camera.lua           # Unity 스타일 orthographic 카메라
│   └── cameraManager.lua    # game/debug 카메라 모드 (F5 토글)
│
├── 03_game/               # 게임 로직
│   ├── components/ (17종)   # 순수 데이터 ECS 컴포넌트
│   ├── systems/ (17종)      # ECS 시스템 + BulletPool + StageManager
│   ├── entities/            # 엔티티 팩토리 + Player 파사드
│   │   ├── entityFactory.lua  # createPlayer(), createEnemy(), createBoss(), createXpOrb()
│   │   └── player.lua         # ECS 파사드 (bind/update/getPosition)
│   ├── patterns/            # 탄막 패턴 (예정)
│   └── states/
│       ├── gameState.lua      # 게임 상태 (playing/game_over) + timeScale
│       └── levelUp.lua        # 레벨업 3택 1 카드 UI
│
└── 04_ui/                 # HUD, 모바일 레이아웃
    ├── uiManager.lua        # UI 총괄 (터치 소비 우선)
    ├── topHud.lua           # 상단 HUD
    ├── bottomControls.lua   # 하단 버튼 컨트롤
    └── mobileLayout.lua     # 모바일 레이아웃 (영역 분할)
```

---

## 3. 레이어 아키텍처

```
04_ui → 03_game → 02_renderer → 01_core → 00_common
```

- 상위 → 하위 `require`만 허용 (역방향 절대 금지)
- `main.lua`는 예외: 모든 레이어 `require` 가능 (진입점)
- `global.init()`은 `love.load()`에서 **최우선** 호출

### 모듈 패턴
```lua
local M = {}
function M.init() end
function M.update(dt) end
function M.draw() end
return M
```

---

## 4. ECS 아키텍처 (`01_core/`)

### 4.1. ECS 코어 (`ecs.lua`)
```lua
local ECS = require("01_core.ecs")
local ecs = ECS.new()

-- 엔티티 생성/제거 (ID 재활용)
local id = ecs:createEntity()
ecs:destroyEntity(id)

-- 컴포넌트 CRUD
ecs:addComponent(id, "Transform", { x = 0, y = 0, angle = 0, scale = 1 })
local t = ecs:getComponent(id, "Transform")
ecs:removeComponent(id, "Transform")
ecs:hasComponent(id, "Transform")

-- 엔티티 쿼리 (pivot 최적화 + componentIndex 캐시)
local entities = ecs:queryEntities({"Transform", "Velocity"})

-- 통계
local stats = ecs:getStats()
-- stats.activeEntities, stats.componentTypes, stats.recycledIds
```

### 4.2. 시스템 베이스 클래스 (`system.lua`)
```lua
local System = require("01_core.system")

local MySystem = System.new("MySystem", {"Transform", "Velocity"},
    function(ecs, dt, entities)
        for _, entityId in ipairs(entities) do
            local t = ecs:getComponent(entityId, "Transform")
            local v = ecs:getComponent(entityId, "Velocity")
            -- 로직 처리
        end
    end
)
```

### 4.3. ECS 매니저 (`ecsManager.lua`)
```lua
local ecsManager = require("01_core.ecsManager")

ecsManager.init(getPlayerPos)     -- 월드 + 14시스템 등록
ecsManager.update(dt)             -- 로직 시스템만 실행
ecsManager.draw()                 -- 렌더 시스템만 실행
ecsManager.getWorld()             -- ECS 월드 참조
ecsManager.getBulletPool()        -- 공유 BulletPool 참조
ecsManager.restart()              -- 전체 리셋
ecsManager.getStats()             -- world + systems + bullets + spawner 통계
```

### 4.4. 컴포넌트 패턴 (`03_game/components/`)
```lua
local M = {}
M.name = "Transform"
M.defaults = { x = 0, y = 0, angle = 0, scale = 1 }
function M.new(data)
    local c = {}
    for k, v in pairs(M.defaults) do
        c[k] = (data and data[k]) or v
    end
    return c
end
return M
```

### 4.5. 컴포넌트 목록 (17종)

| 컴포넌트 | 주요 필드 | 용도 |
|----------|----------|------|
| Transform | x, y, angle, scale | 위치/회전 |
| Velocity | vx, vy, maxSpeed, friction | 속도/물리 |
| Collider | radius, type | 충돌체 |
| Renderable | color, radius, shape | 렌더링 속성 |
| LifeSpan | duration, elapsed | 수명 자동 제거 |
| PlayerTag | (태그) | 플레이어 식별 |
| Input | speed, enabled | 입력 설정 |
| WorldBound | (태그) | 월드 경계 제한 |
| Health | hp, maxHp, iFrames, iTimer, alive | HP/무적 |
| BulletEmitter | pattern, emitRate, bulletSpeed, … | 탄막 발사 |
| EnemyAI | aiType, speed, xpValue | 적 AI + XP 값 |
| PlayerWeapon | fireRate, range, damage, bulletSpeed | 자동 공격 |
| Dash | distance, cooldown, cooldownTimer | 대쉬 능력 |
| Focus | maxEnergy, energy, drainRate, active | 포커스 모드 |
| PlayerXP | xp, xpToNext, level, magnetRange | 경험치/레벨 |
| XpOrb | value, magnetRange, collectRadius | XP 오브 |
| BossTag | phases, currentPhase, patterns, … | 보스 상태/페이즈/텔레포트 |

---

## 5. 시스템 실행 순서

`ecsManager._registerBasicSystems()`에서 등록 순서:

```
love.update(scaledDt):
  ┌──────────────────────────────────────────────────┐
  │ 1. Input           키보드/터치 → Velocity         │
  │ 2. Focus           Space 홀드 → 슬로모+정밀이동   │
  │ 3. Dash            Shift → 순간이동+무적          │
  │ 4. EnemyAI         적 이동/행동 패턴               │
  │ 5. Movement        Velocity → Transform 갱신      │
  │ 6. Boundary        월드 경계 clamping              │
  │ 7. LifeSpan        수명 만료 엔티티 제거            │
  │ 8. BulletEmitter   적 탄막 패턴 발사 → BulletPool  │
  │ 9. PlayerWeapon    가장 가까운 적 자동 조준·발사     │
  │ 10. Collision      enemy_bullet ↔ Player 충돌      │
  │ 11. EnemyCollision player_bullet ↔ Enemy 충돌+XP   │
  │ 12. XpCollection   XP 오브 자석 수집               │
  │ 13. Boss           보스 인트로/페이즈/탄막/이동     │
  └──────────────────────────────────────────────────┘
  → bulletPool:update(scaledDt, bounds)
  → stageManager:update(scaledDt)

love.draw():
  cameraManager.draw(function()
    ┌─────────────────────────────────────┐
    │ 14. Render         일반 엔티티 렌더링 │
    │     bulletPool:draw()               │
    │ 15. PlayerRender   플레이어 렌더링    │
    └─────────────────────────────────────┘
  end)
  → UI / Debug overlay / Logger console
```

---

## 6. 카메라 시스템 (`02_renderer/`)

### Unity 스타일 정사영 카메라
- `orthographicSize` = 카메라가 보는 월드의 **절반 높이**
- 수학적 좌표계: (0,0) 중심, Y+ 상향
- `camera:draw(fn)` 내부에서 월드 좌표 사용

```lua
local camera = require("02_renderer.camera")
local cam = camera.new(0, 0, 5)    -- 월드 (0,0), orthographicSize=5

cam:lookAt(x, y)                   -- 위치 설정
cam:setOrthographicSize(10)        -- 줌 제어
cam:draw(function() ... end)       -- 월드 좌표 렌더링

-- 좌표 변환
local sx, sy = cam:cameraCoords(wx, wy)   -- 월드 → 스크린
local wx, wy = cam:worldCoords(sx, sy)     -- 스크린 → 월드
local ppu = cam:getPixelsPerUnit()         -- 픽셀/유닛 비율
```

### cameraManager
- **game 모드**: 플레이어 추적 (기본)
- **debug 모드**: 마우스 드래그/휠 자유 이동 (F5 토글)

---

## 7. 월드 시스템 (`01_core/world.lua`)

```lua
local world = require("01_core.world")
world.init()

-- 월드 크기: 120 × 250 (중심: 0, -100)
local w, h = world.size.width, world.size.height  -- 120, 250
local cx, cy = world.center.x, world.center.y     -- 0, -100
```

- 7개 존 (axis-aligned 사각형)
- 파워업, 체크포인트, 비밀 장소 포함

---

## 8. Player 시스템

### EntityFactory (`03_game/entities/entityFactory.lua`)
```lua
EntityFactory.createPlayer(ecs, x, y)
-- 컴포넌트: Transform, Velocity(maxSpeed=5), Input(speed=5), Collider(r=0.2),
--   Renderable, PlayerTag, WorldBound, Health(5hp, iFrame=1.5s),
--   PlayerWeapon(fireRate=4, range=6), Dash(dist=2, cd=3s),
--   Focus(energy=100), PlayerXP

EntityFactory.createEnemy(ecs, x, y, enemyType)
-- 프리셋: basic(hp=3, xp=2), spiral(hp=5, xp=5), aimed(hp=2, xp=3), wave(hp=4, xp=3)

EntityFactory.createXpOrb(ecs, x, y, value)
-- XP 오브 엔티티 (자석 수집)

EntityFactory.createBoss(ecs, x, y, bossType)
-- 보스 엔티티 (BossTag + 페이즈 + 탄막 순환)
-- bossType: "NULL"(S3), "STACK"(S6), "HEAP"(S9), "RECURSION"(S12), "OVERFLOW"(S15)
```

### Player 파사드 (`03_game/entities/player.lua`)
```lua
player.bind(ecs, entityId)    -- ECS 엔티티에 바인딩
player.init()                 -- 디버그 정보 등록
player.update(dt)             -- 월드 인터랙션 (존 감지, 파워업)
player.getPosition()          -- x, y 반환
player.getCameraTarget()      -- 카메라 추적용 좌표
player.getEntityId()          -- 바인딩된 엔티티 ID
player.getStats()             -- 디버그용 통계
player.reset()                -- 상태 초기화
```

---

## 9. 게임 상태 (`03_game/states/`)

### gameState.lua
```lua
gameState.init()                  -- 상태 초기화
gameState.update(dt, health)      -- 상태 갱신 (HP 체크 → game_over 전환)
gameState.isPlaying()             -- playing 상태인지
gameState.isGameOver()            -- game_over 상태인지
gameState.getScore()              -- 점수 (생존시간)
gameState.getTimeScale()          -- 시간 배율 (포커스=0.4, 레벨업=0, 기본=1.0)
gameState.setTimeScale(scale)     -- 시간 배율 설정
```

### levelUp.lua
- 10종 업그레이드 풀에서 랜덤 3개 카드 표시
- 무기 5종: 발사속도, 사거리, 데미지, 탄수, 관통
- 패시브 5종: 최대HP, 이동속도, 대쉬쿨, 자석범위, 최대에너지
- 감쇠 스택: 같은 업그레이드 반복 선택 시 `0.7^n` 감쇠 (예외: Multi Shot, Max HP)
- 키 1/2/3 또는 터치로 선택

---

## 10. 입력 파이프라인

### 터치/마우스
1. `love.touchpressed/moved/released` — 캐노니컬 입력 콜백
2. `love.mousepressed` → `love.touchpressed` 브릿지 (PC 프로토타입)
3. `uiManager`가 터치 우선 소비 → 미소비 이벤트가 게임으로
4. `mobileLayout.isTouchInArea(x, y, "play")`로 게임 영역 판별

### 키보드
| 키 | 기능 |
|----|------|
| WASD / 화살표 | 이동 |
| Shift | 대쉬 (순간이동 + 무적) |
| Space | 포커스 (슬로모 + 정밀이동) |
| 1/2/3 | 레벨업 카드 선택 |
| R | 게임오버 시 리스타트 |
| ESC | 종료 |

### 디버그 키
| 키 | 기능 |
|----|------|
| `` ` `` | Logger 콘솔 토글 |
| F1 | Debug watch panel 토글 |
| F2 | UI 표시 토글 |
| F3 | UI 디버그 모드 토글 |
| F4 | Screen grid 토글 |
| F5 | Camera 모드 전환 (game ↔ debug) |
| F7 | God mode 토글 (플레이어 무적) |
| F8 | Stage skip (디버그 스테이지 건너뛰기) |

---

## 11. 전역 유틸리티 (`00_common/global.lua`)

```lua
global.init()                      -- love.load에서 최우선 호출

-- 로깅 (어디서나 사용 가능)
log("msg")  logInfo("msg")  logDebug("msg")  logWarn("msg")  logError("msg")

-- 수학
clamp(value, min, max)
lerp(a, b, t)
distance(x1, y1, x2, y2)
normalize(x, y)                    -- 정규화 벡터 반환 (nx, ny)

-- 색상 (0-255 범위 입력)
setColor(r, g, b, a)               -- love.graphics.setColor 래퍼
resetColor()
```

### 로그 메시지 규칙
```lua
logInfo("[CAM] Camera initialized")    -- O (ASCII 태그)
logInfo("📹 Camera initialized")      -- X (이모지/한글 깨짐)
```

---

## 12. 디버그 도구

### Debug Watch Panel (`00_common/debug.lua`)
```lua
debug.add("FPS", function() return tostring(love.timer.getFPS()) end)
debug.remove("FPS")
debug.toggleConsole()              -- F1 토글
debug.draw(x, y, font)
```

### Logger 콘솔 (`00_common/logger.lua`)
```lua
logger.init()                      -- love.load에서 호출
logger.info/warn/error/debug(msg)
logger.toggleConsole()             -- ` 키 토글
logger.drawConsole(font)           -- love.draw 마지막에 호출
logger.close()                     -- love.quit에서 호출
```

---

## 13. 수학 라이브러리 (`00_common/math/`)

```lua
-- 벡터
local Vector = require("00_common.math.vector")
local v = Vector(3, 4)
v:len()  v:normalized()  v:dot(v2)  v:rotated(angle)
v1 + v2  v1 * scalar

-- 행렬
local Matrix = require("00_common.math.matrix")
Matrix.translate(x, y)  Matrix.rotate(angle)
```

---

## 14. 성능 고려사항

- **목표**: 60 FPS, 1000+ 엔티티 (탄막 + 적 + XP 오브)
- **BulletPool**: ECS 외부, 2000개 사전 할당, zero-GC swap-remove
- **componentIndex 캐시**: 쿼리 시 pivot 최적화
- **timeScale**: `scaledDt = dt * gameState.getTimeScale()`
- `local` 선언은 핫패스 밖에서
- `string.format` 선호 (루프 내 `..` 연결 지양)
- `math.sin/cos` → 필요 시 LUT 캐시

---

## 15. Spatial Hash Grid 패턴

프로젝트에서 반복적으로 사용되는 공간 분할 기법. 가까운 오브젝트 탐색을 O(n²) → O(n)으로 줄인다.

### 핵심 구조

```
┌───┬───┬───┐
│   │ ● │   │  셀 크기 = CELL_SIZE
├───┼───┼───┤  키 = floor(x/CELL_SIZE) * PRIME + floor(y/CELL_SIZE)
│ ● │ ★ │ ● │  ★ 기준 3×3 이웃 셀만 탐색
├───┼───┼───┤
│   │ ● │   │
└───┴───┴───┘
```

### 구현 패턴

```lua
local CELL_SIZE = 0.5
local INV_CELL  = 1 / CELL_SIZE
local grid = {}

-- 삽입: O(1)
local cx = floor(x * INV_CELL)
local cy = floor(y * INV_CELL)
local key = cx * 100003 + cy   -- 정수 키 (string concat 회피)
grid[key] = grid[key] or {}
grid[key][#grid[key] + 1] = entity

-- 이웃 탐색: 3×3 = 최대 9셀
for nx = cx - 1, cx + 1 do
    for ny = cy - 1, cy + 1 do
        local cell = grid[nx * 100003 + ny]
        if cell then
            for i = 1, #cell do ... end
        end
    end
end

-- 매 프레임 초기화: for k in pairs(grid) do grid[k] = nil end
```

### 설계 포인트

| 항목 | 설명 |
|------|------|
| 셀 크기 | ≥ 탐색 반경. 너무 크면 퇴화, 너무 작으면 빈 셀 낭비 |
| 키 해싱 | `cx * PRIME + cy` (정수 연산). `tostring` / `..` 절대 금지 |
| 메모리 | 테이블 재사용 + `nil` 초기화 (GC 최소화) |
| 자기 제외 | 저장된 entityId로 비교 (배열 인덱스 ≠ entityId 주의) |

### 프로젝트 적용 현황

| 위치 | 용도 | 셀 크기 | 탐색 반경 |
|------|------|---------|----------|
| `02_renderer/background.lua` | RSF 원 배치 중복 방지 | 가변 (원 반지름 기반) | 원 반지름 |
| `03_game/systems/enemyAISystem.lua` | Bit swarm 군집 분리력 | 0.5 | 0.25 |
| *(예정)* `playerWeaponSystem` | 최근접 적 탐색 최적화 | TBD | TBD |
