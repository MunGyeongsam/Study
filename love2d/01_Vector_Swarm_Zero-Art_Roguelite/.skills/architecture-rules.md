---
name: architecture-rules
role: "Vector Swarm 소프트웨어 아키텍처 규칙 정의"
description: |
  프로젝트 전체에서 참조하는 아키텍처 규칙 세트.
  planner, code-reviewer, architect 등 다른 agent가 이 skill을 참조하여
  설계/리뷰/구현 시 아키텍처 일관성을 유지합니다.
---

# Vector Swarm Architecture Rules

## 1. 레이어 규칙 (Layer Dependency)

```
src/
├── 00_common/   # 유틸리티 — 게임 의존성 없음
├── 01_core/     # 엔진 (world, ECS)
├── 02_renderer/ # 카메라, 렌더링
├── 03_game/     # 게임 로직 (엔티티, 시스템, 상태)
└── 04_ui/       # HUD, 모바일 레이아웃
```

### 규칙
- **상위 → 하위 require만 허용**: `04_ui` → `03_game` → `02_renderer` → `01_core` → `00_common`
- **같은 레이어 내 require 허용**: `03_game/entities/player.lua` → `03_game/systems/...`
- **역방향 절대 금지**: `01_core`가 `03_game`을 require하면 위반
- **main.lua는 예외**: 모든 레이어를 require할 수 있음 (진입점)
- **scenes/는 예외**: `main.lua`와 동일한 컴포지터 역할. 여러 레이어를 조합하므로 모든 레이어를 require 가능

### 검증 방법
```lua
-- ✅ 올바른 의존성
local world = require("01_core.world")       -- 03_game → 01_core

-- ❌ 위반: 하위 레이어가 상위 참조
local player = require("03_game.entities.player")  -- 01_core에서 사용 금지
```

---

## 2. 모듈 패턴 일관성

### 표준 모듈 구조
```lua
local M = {}

function M.init()   end  -- 초기화 (선택)
function M.update(dt) end  -- 매 프레임 업데이트 (선택)
function M.draw()    end  -- 렌더링 (선택)

return M  -- 반드시 테이블 반환
```

### 규칙
- 모든 모듈은 **테이블을 반환**해야 함 (`return M`)
- 상태를 가진 모듈은 `init()` → `update(dt)` → `draw()` 패턴 준수
- 모듈 내부 상태는 **모듈 테이블 안에 캡슐화** (파일 스코프 local 변수 최소화)
- 생성자 패턴은 `M.new(...)` 사용

---

## 3. 성능 아키텍처

### 핫패스 규칙 (60fps, 1000+ 엔티티)
- `love.update()` / `love.draw()` 내부에서 **매 프레임 local 선언 최소화**
- **string.format** 사용 (`.."문자열"..` 연결 금지 — 루프 내)
- **오브젝트 풀링** 필수: 탄막, 적, 파티클 등 빈번한 생성/소멸 객체
- `math.sin/cos` 결과는 **룩업 테이블로 캐시** (반복 호출 시)

### 메모리 규칙
- GC 부하 최소화: 핫패스에서 테이블 생성 금지
- 풀에서 꺼내 재사용 → 사용 후 풀로 반환 패턴
- `collectgarbage("count")` 로 메모리 모니터링

---

## 4. ECS 설계 검증

### 컴포넌트 원칙
- 컴포넌트는 **순수 데이터** (함수 포함 금지)
- 하나의 관심사만 담당 (Transform ≠ Velocity)
- 컴포넌트 간 직접 참조 금지 (시스템을 통해서만 연결)

### 시스템 원칙
- 시스템은 **로직만** 담당 (데이터 보유 금지)
- 필요한 컴포넌트를 `requiredComponents`로 선언
- 시스템 간 직접 호출 금지 (ECSManager를 통해 순서 관리)
- Render 시스템은 `draw()`에서만 실행, 나머지는 `update(dt)`에서 실행

### 엔티티 생성 원칙
- 팩토리 함수는 `ECSManager`에 정의 (`createPlayer`, `createEnemy` 등)
- 컴포넌트 조합은 팩토리 함수 내에서만 결정

---

## 5. 새 모듈 배치 가이드

| 모듈 성격 | 배치 레이어 | 예시 |
|-----------|-------------|------|
| 수학/유틸/로깅 | `00_common/` | vector.lua, logger.lua |
| 월드, ECS 코어, 물리 | `01_core/` | ecs.lua, world.lua |
| 카메라, 셰이더, 파티클 렌더러 | `02_renderer/` | camera.lua |
| 엔티티, 시스템, 상태머신, 패턴 | `03_game/` | player.lua, movementSystem.lua |
| HUD, 버튼, 레이아웃 | `04_ui/` | topHud.lua, mobileLayout.lua |

### 판단 기준
- "이 모듈이 게임 로직을 알아야 하는가?" → Yes: `03_game` / No: `01_core` 이하
- "이 모듈이 화면에 직접 그리는가?" → UI: `04_ui` / 월드 렌더링: `02_renderer`
- "다른 프로젝트에서도 재사용 가능한가?" → Yes: `00_common`

---

## 6. API 네이밍 규칙

### 함수명
- **동사 + 명사** 형태: `getPosition()`, `createEntity()`, `destroyEntity()`
- **불린 반환**: `is`/`has` 접두사: `hasComponent()`, `isAlive()`
- **내부 함수**: `_` 접두사: `_registerBasicSystems()`, `_countComponentTypes()`

### 컴포넌트명
- **PascalCase**: `Transform`, `Velocity`, `Renderable`, `LifeSpan`

### 시스템명
- **PascalCase**: `Movement`, `LifeSpan`, `Render`

### 모듈 파일명
- **camelCase**: `ecsManager.lua`, `mobileLayout.lua`
- 예외: 약어는 소문자: `ecs.lua` (ECS → ecs)

---

## 7. 순환 의존성 방지

### 규칙
- 같은 레이어 내에서도 **A → B → A** 순환 require 금지
- Lua에서 순환 require 시 `nil` 반환 — 런타임 에러로 이어짐

### 해결 패턴
```lua
-- ❌ 순환: a.lua requires b.lua, b.lua requires a.lua

-- ✅ 해결 1: 공통 의존성 추출
-- c.lua에 공유 로직 분리, a와 b 모두 c만 참조

-- ✅ 해결 2: 지연 require (필요시에만)
function M.doSomething()
    local b = require("01_core.b")  -- 함수 호출 시점에 require
    b.action()
end
```

---

## 8. 전역 오염 방지

### 의도적 전역 (허용 목록)
`00_common/global.lua`가 주입하는 함수만 전역 허용:
- `log`, `logDebug`, `logWarn`, `logError`, `logInfo`
- `setColor`, `resetColor`
- `clamp`, `lerp`, `distance`, `normalize`

`main.lua`가 주입하는 전역 (엔진 서비스):
- `screenShake` — 카메라 쉐이크 (`cameraManager.shake`)
- `playSound`, `playBGM`, `stopBGM` — 사운드 재생 (`soundManager` 래퍼)

### 규칙
- 위 목록 외에 **전역 변수 생성 금지**
- 모든 변수는 `local` 선언 필수
- 모듈 간 데이터 공유는 **require를 통한 모듈 참조**로만

### 검증 방법
```lua
-- ❌ 전역 오염
myGlobalVar = 42

-- ✅ 올바른 사용
local myLocalVar = 42
```

---

## 9. 레이어 간 역방향 통신

하위 레이어 → 상위 레이어 알림이 필요한 경우 (예: 03_game → 04_ui 이벤트):

### 금지 패턴
```lua
-- ❌ 03_game에서 04_ui를 직접 require
local topHud = require("04_ui.topHud")
topHud.showDamage(10)
```

### 허용 패턴

#### 1. 콜백 주입 (현재 사용 중)
```lua
-- main.lua에서 상위→하위 콜백 설정
uiManager.setButtonCallbacks({
    onReset = function() player.reset() end
})
```

#### 2. 이벤트 시스템 (추후 도입 시)
```lua
-- 00_common/events.lua (이벤트 버스)
local events = require("00_common.events")

-- 03_game에서 발행
events.emit("player:damaged", { amount = 10 })

-- 04_ui에서 구독
events.on("player:damaged", function(data) ... end)
```

#### 3. 데이터 폴링
```lua
-- 04_ui가 03_game의 공개 API를 주기적으로 조회
local hp = player.getHealth()  -- 04_ui → 03_game (정방향 require, 합법)
```
