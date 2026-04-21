# 14. Scene Stack 아키텍처 — Vector Swarm

> 작성: 2026-04-20
> 상태: 구현 완료 (Phase 4B.1)

---

## 1. 기술 선택의 배경

### 1.1. 해결해야 할 문제

Phase 4에서 UI가 본격적으로 추가되면서 `main.lua`의 복잡도가 급격히 증가했다.

**현재 상태 (4B.1 완료 시점):**

```
love.keypressed(key):
  ├─ debug keys (F1~F4, `)        ── 모든 상태에서 작동
  ├─ if TITLE:
  │   ├─ if upgradeTree.keypressed(key) → return
  │   ├─ if ESC → quit
  │   └─ titleMenu.keypressed(key)
  ├─ if PAUSED:
  │   └─ pauseMenu.keypressed(key)
  ├─ if PLAYING:
  │   ├─ if levelUp.keypressed(key) → return
  │   ├─ if upgradeTree.keypressed(key) → return
  │   ├─ game keys (F5~F12, R, U, Shift)
  │   └─ if ESC → pause
  └─ debug camera keys (+, -, space)
```

**문제점:**

| # | 문제 | 영향 |
|---|------|------|
| 1 | **Input 라우팅이 if-else 수동 분기** | UI 추가마다 main.lua 수정 필요. 실수 유발 |
| 2 | **Draw 순서가 하드코딩** | 11개 레이어를 수동 순서 관리. 순서 실수 = draw bug |
| 3 | **상태 전이가 콜백 거미줄** | titleMenu → startGame → gameState → ecsManager → ... |
| 4 | **역방향 의존성** | levelUp이 gameState.setTimeScale(0) 직접 호출 |
| 5 | **동시 활성 UI 충돌** | upgradeTree + titleMenu 가 동시에 draw될 때 순서 실수 |
| 6 | **새 UI 추가 비용 O(n)** | 하나 추가할 때 if-else 분기 3곳(update, draw, keypressed) 수정 |

### 1.2. 검토한 후보 패턴

| 패턴 | 핵심 아이디어 | 적합성 | 판단 근거 |
|------|-------------|:------:|----------|
| Redux (단일 Store + Reducer) | 상태 불변성, 단방향 흐름, 순수 함수 | △ | Lua에 타입 없음 → 리듀서 안전성 불확실. 솔로 개발에서 보일러플레이트 과다 |
| MVP (Model-View-Presenter) | View↔Presenter 분리, 테스트 용이 | △ | 앱 UI에 최적. 게임은 매 프레임 update/draw 루프가 핵심이라 맞지 않음 |
| MVC | Model-View-Controller 고전 분리 | × | 게임 루프(update→draw)와 구조적 불일치. Controller 역할 모호 |
| FSM (Finite State Machine) | 상태 전이 명시적 정의 | ○ | 단일 상태만 가능. 오버레이(pause위에 upgrade) 불가 |
| **Scene Stack** | push/pop 기반 상태 스택, 오버레이 지원 | **◎** | 게임 UI의 표준 패턴. 정확히 현재 문제 해결. Lua에서 50줄 구현 |
| Event Bus (pub/sub) | 느슨한 결합, 메시지 기반 소통 | ○ | 단독 사용 시 디버깅 어려움. Scene Stack 보조로 향후 도입 가능 |

### 1.3. 선택: Scene Stack

**선택 이유:**
1. **게임 업계 검증 패턴** — Unity의 SceneManager, Godot의 SceneTree, Cocos2d의 Director가 동일 개념
2. **현재 pain point 직접 해결** — Input 라우팅, draw 순서, 상태 전이 모두 스택 구조로 자동화
3. **점진적 도입 가능** — 기존 모듈을 Scene 인터페이스로 래핑만 하면 됨 (리라이트 불필요)
4. **Lua 구현 간단** — 50~80줄. 외부 의존성 없음
5. **확장 비용 O(1)** — 새 UI 추가 시 Scene 파일 하나만 추가. main.lua 수정 없음

---

## 2. Scene이란?

### 2.1. 정의

**Scene(씬)은 "사용자가 지금 무엇을 하고 있는가"를 나타내는 독립적인 단위**다.

플레이어가 게임을 하는 동안 경험하는 모든 "상황"이 Scene이 될 수 있다:
- 타이틀 화면을 보고 있다 → TitleScene
- 전투를 하고 있다 → PlayScene
- 일시정지 메뉴를 보고 있다 → PauseScene
- 레벨업 선택을 하고 있다 → LevelUpScene

### 2.2. Scene의 경계 — 무엇이 Scene이고 무엇이 아닌가

Scene은 **사용자의 Input을 점유하는 단위**로 판단한다.

```
"이 화면에서 사용자가 방향키를 누르면 어떤 일이 벌어지는가?"
→ 다른 일이 벌어진다면, 그것은 다른 Scene이다.
```

| 상황 | Scene인가? | 이유 |
|------|:----------:|------|
| 타이틀 화면 | **O** | 방향키 = 메뉴 이동. 고유한 Input 맵 |
| 전투 플레이 | **O** | 방향키 = 플레이어 이동. 고유한 Input 맵 |
| 일시정지 | **O** | 방향키 = 메뉴 이동. 전투 Input 비활성 |
| 레벨업 3택 | **O** | 좌/우 = 카드 선택. 전투 Input 비활성 |
| 업그레이드 트리 | **O** | 방향키 = 항목 탐색. 기존 화면 위 오버레이 |
| 상단 HUD | **X** | 자체 Input 없음. PlayScene의 일부로 draw만 |
| 데미지 숫자 팝업 | **X** | 자체 Input 없음. PlayScene의 시각 효과 |
| 디버그 콘솔 | **X** | Scene 외부 시스템 (항상 최상위에 draw) |

**경계의 핵심 기준:**
1. **독자적인 Input 핸들링**이 있는가?
2. **다른 화면의 Input을 차단**하는가?
3. **독립적으로 enter/exit**할 수 있는가?

세 가지 중 2개 이상 해당하면 Scene으로 분리한다.

### 2.3. Scene이 소유하는 것과 소유하지 않는 것

```
Scene이 소유:                    Scene이 소유하지 않음:
├─ 자신의 UI 상태                 ├─ 전역 게임 데이터 (score, fragments)
│  (selectedIndex, timer)        ├─ ECS 월드 (엔티티/컴포넌트)
├─ 자신의 Input 핸들링            ├─ 세이브 데이터
├─ 자신의 Draw 로직               ├─ 카메라/사운드 시스템
└─ 전이 결정 (push/pop 트리거)    └─ 다른 Scene의 내부 상태
```

Scene은 **화면 전환의 주체**이지, **데이터의 소유자**가 아니다.
PlayScene은 ECS 월드를 "소유"하는 게 아니라 "참조"해서 update/draw를 위임한다.

### 2.4. Scene vs 기존 모듈

기존 `titleMenu.lua` 같은 모듈과 Scene의 관계:

```
titleMenu.lua (기존)               TitleScene (Scene 래퍼)
├─ draw()                         ├─ draw() → titleMenu.draw()
├─ keypressed(key)                ├─ keypressed(key) → titleMenu.keypressed(key)
├─ touchpressed(x, y)            ├─ touchpressed(...) → titleMenu.touchpressed(x, y)
└─ update(dt)                    ├─ update(dt) → titleMenu.update(dt)
                                  ├─ enter() → titleMenu.reset() + BGM 처리
                                  ├─ exit() → 정리 작업
                                  └─ drawBelow / transparent 속성
```

**기존 모듈을 버리지 않는다.** Scene은 기존 모듈 위에 **통일된 인터페이스**(enter/exit/drawBelow/transparent)를 씌우는 얇은 래퍼다.

---

## 3. Scene Stack이란?

### 3.1. 개념

Scene Stack은 **Scene들을 스택(stack) 자료구조로 관리**하는 패턴이다.

```
일반 FSM:                    Scene Stack:
                            
  TITLE ──→ PLAYING          ┌───────────┐
    ↑          │             │ Upgrade   │ ← top (input 받음)
    └── GAME_OVER            ├───────────┤
                             │ TitleMenu │ ← draw만 (아래에 보임)
  한 번에 하나만 활성.         ├───────────┤
  오버레이 불가.              │  Playing  │ ← update도 가능
                             └───────────┘
                             위에 쌓고(push), 빼고(pop).
                             여러 겹 공존 가능.
```

### 3.2. 핵심 규칙

```
규칙 1: Input은 스택 최상위(top)만 받는다.
규칙 2: Draw는 아래부터 위로 순서대로 그린다.
규칙 3: Update는 각 Scene의 transparent 플래그에 따라 아래로 전파된다.
규칙 4: push/pop 시 enter/exit 콜백이 호출된다.
```

| 연산 | 동작 | 예시 |
|------|------|------|
| `push(scene)` | 스택 위에 새 씬 추가 | 플레이 중 ESC → `push(PauseScene)` |
| `pop()` | 최상위 씬 제거 | 일시정지에서 Continue → `pop()` |
| `replace(scene)` | 최상위를 교체 | 타이틀에서 PLAY → `replace(PlayScene)` |
| `clear()` | 스택 전체 비움 | 게임오버 → 타이틀 전환 시 |

### 3.3. Scene 인터페이스

모든 Scene은 동일한 인터페이스를 구현한다:

```lua
local MyScene = {}
MyScene.__index = MyScene

function MyScene.new()
    return setmetatable({
        -- transparent: true면 아래 Scene도 update 받음
        transparent = false,
        -- drawBelow: true면 아래 Scene도 draw됨 (오버레이)
        drawBelow = true,
    }, MyScene)
end

function MyScene:enter(prevScene)
    -- push/replace로 활성화될 때 호출
end

function MyScene:exit()
    -- pop/replace로 제거될 때 호출
end

function MyScene:update(dt)
    -- 매 프레임 로직
end

function MyScene:draw()
    -- 매 프레임 렌더링
end

function MyScene:keypressed(key)
    -- 키 입력 (top일 때만 호출됨)
end

function MyScene:touchpressed(id, x, y, ...)
    -- 터치 입력 (top일 때만 호출됨)
end

return MyScene
```

### 3.4. transparent와 drawBelow

이 두 플래그가 Scene Stack의 핵심이다:

```
drawBelow = true, transparent = false (기본 오버레이)
→ 아래 씬이 보이지만(draw), 게임은 멈춤(update 안 함)
→ 용도: 일시정지, 레벨업 선택

drawBelow = false, transparent = false (전체화면 교체)
→ 아래 씬이 보이지도 않고, 업데이트도 안 됨
→ 용도: 타이틀 화면, 게임오버 화면

drawBelow = true, transparent = true (투명 오버레이)
→ 아래 씬이 보이고, 게임도 계속 진행됨
→ 용도: HUD, 데미지 숫자 팝업, 알림
```

---

## 4. 적용 설계

### 4.1. 현재 모듈 → Scene 매핑 (구현 완료)

| Scene 이름 | 래핑 모듈 | drawBelow | transparent | 비고 |
|-----------|-----------|:---------:|:-----------:|------|
| TitleScene | titleMenu.lua | false | false | 전체화면, BGM 정지 |
| PlayScene | ecsManager + player + bloom + bg | — | — | 스택 베이스. ECS 업데이트 + 렌더링 |
| PauseScene | pauseMenu.lua | true | false | Continue / Restart / Menu |
| LevelUpScene | levelUp.lua | true | false | 3택 선택 후 자동 pop |
| UpgradeScene | upgradeTree.lua | true | false | 타이틀/게임오버에서 push 가능 |
| GameOverScene | gameState.lua draw | true | false | 도전과제 해금 toast + R/U/ESC |
| CreditsScene | (자체 구현) | true | false | Zero-Art 스타일 크레딧 |
| AchievementScene | achievementSystem | true | false | 진행도 바 + 보상 정보 |

### 4.2. 게임 플로우 (Scene Stack 기준 — 구현 완료)

```
[앱 시작]
  stack: [ TitleScene ]

[PLAY 선택]
  → replace(PlayScene)
  stack: [ PlayScene ]

[ESC 누름]
  → push(PauseScene)
  stack: [ PlayScene, PauseScene ]
  ※ PlayScene.draw() 실행됨 (drawBelow=true)
  ※ PlayScene.update() 안 됨 (transparent=false)

[Continue]
  → pop()
  stack: [ PlayScene ]

[Pause → Main Menu]
  → clear + push(TitleScene)
  stack: [ TitleScene ]

[레벨업 발생]
  → push(LevelUpScene)
  stack: [ PlayScene, LevelUpScene ]

[선택 완료]
  → auto pop (levelUp.isActive() == false)
  stack: [ PlayScene ]

[UPGRADES (타이틀에서)]
  → push(UpgradeScene)
  stack: [ TitleScene, UpgradeScene ]

[ESC로 닫기]
  → pop()
  stack: [ TitleScene ]

[ACHIEVEMENTS (타이틀에서)]
  → push(AchievementScene)
  stack: [ TitleScene, AchievementScene ]

[CREDITS (타이틀에서)]
  → push(CreditsScene)
  stack: [ TitleScene, CreditsScene ]

[플레이어 사망]
  → push(GameOverScene)
  stack: [ PlayScene, GameOverScene ]

[R 리스타트]
  → pop + playScene:restart()
  stack: [ PlayScene ]

[U 업그레이드 (게임오버에서)]
  → push(UpgradeScene)
  stack: [ PlayScene, GameOverScene, UpgradeScene ]
```

### 4.3. main.lua 변화 (Before → After)

**Before (현재):**
```lua
function love.keypressed(key)
    -- 디버그 키 (8줄)
    if gameState.isTitle() then
        if upgradeTree.keypressed(key) then return end
        if key == "escape" then love.event.quit() end
        titleMenu.keypressed(key)
        return
    end
    if gameState.isPaused() then
        pauseMenu.keypressed(key)
        return
    end
    if levelUp.keypressed(key) then return end
    if upgradeTree.keypressed(key) then return end
    -- game keys (20줄)
    if key == 'escape' then ... end
    -- debug camera (10줄)
end
```

**After (Scene Stack 적용):**
```lua
function love.keypressed(key)
    -- 디버그 키는 항상 작동 (씬 외부)
    if debugKeys.handle(key) then return end
    -- 나머지는 스택 최상위 씬에 위임
    sceneStack:keypressed(key)
end
```

**Before (love.draw):**
```lua
function love.draw()
    bloom.beginCapture()
    cameraManager.draw(drawWorld)
    bloom.endCapture()
    bloom.draw()
    uiManager.draw()
    debug.draw(10, 50, fonts.small)
    logger.drawConsole(fonts.small)
    screenDebugDraw.draw(50)
    gameState.draw()
    ecsManager.stageManager:draw()
    levelUp.draw()
    if gameState.isTitle() then titleMenu.draw() end
    if gameState.isPaused() then pauseMenu.draw() end
    upgradeTree.draw()
end
```

**After:**
```lua
function love.draw()
    -- 씬 스택이 아래부터 위로 순서대로 draw
    sceneStack:draw()
    -- 디버그 UI는 씬 외부 (항상 최상위)
    debug.draw(10, 50, fonts.small)
    logger.drawConsole(fonts.small)
    screenDebugDraw.draw(50)
end
```

---

## 5. SceneStack 구현 명세

### 5.1. 파일 위치

```
src/01_core/sceneStack.lua    -- 순수 엔진 레이어 (게임 의존성 없음)
```

### 5.2. API

```lua
local SceneStack = require("01_core.sceneStack")
local stack = SceneStack.new()

stack:push(scene)       -- 스택 위에 씬 추가, scene:enter() 호출
stack:pop()             -- 최상위 제거, scene:exit() 호출
stack:replace(scene)    -- 최상위 교체 (exit → enter)
stack:clear()           -- 전체 제거 (각각 exit)

stack:update(dt)        -- transparent 체인 따라 아래로 전파
stack:draw()            -- drawBelow 체인 따라 아래부터 위로 그림
stack:keypressed(key)   -- top만
stack:touchpressed(...) -- top만

stack:top()             -- 최상위 씬 반환
stack:size()            -- 스택 깊이
```

### 5.3. Update/Draw 전파 알고리즘

```
update 전파:
  top부터 아래로 탐색.
  transparent=true인 씬은 아래도 update.
  transparent=false인 씬에서 멈춤.

  예: [Play, LevelUp]
      LevelUp.transparent = false → Play.update() 안 함 ✓

draw 전파:
  top부터 아래로 탐색하며 "그려야 할 씬" 목록 수집.
  drawBelow=true인 씬은 아래도 목록에 추가.
  drawBelow=false인 씬에서 멈춤.
  수집된 목록을 아래부터 위로 순서대로 draw.

  예: [Play, Pause]
      Pause.drawBelow = true → Play도 그림.
      그리기 순서: Play → Pause (아래부터 위로).
```

---

## 6. 마이그레이션 — 완료

### 6.1. 원칙 (적용됨)

- **점진적 전환** — 씨 하나씩 Scene 래퍼로 이동.
- **회귀 테스트** — 씨 하나 이동할 때마다 전체 플로우 확인.
- **기존 모듈 유지** — titleMenu.lua 등은 그대로 두고, Scene이 래핑(wrap)만 함.

### 6.2. 단계 계획 → 완료

```
Step 1: sceneStack.lua 엔진 구현 (01_core/)                 ✅
Step 2: PlayScene 작성 (기존 게임 로직 래핑)                 ✅
Step 3: TitleScene 작성 (titleMenu.lua 래핑)               ✅
Step 4: main.lua를 sceneStack 기반으로 전환              ✅
Step 5: PauseScene, LevelUpScene, UpgradeScene,         ✅
        GameOverScene, CreditsScene, AchievementScene
Step 6: gameState에서 TITLE/PAUSED 상태 제거              ✅
```

### 6.3. gameState 역할 변화 (완료)

Scene Stack 도입 전:
```
gameState = 상태 머신(TITLE/PLAYING/PAUSED/GAME_OVER) + 점수 + timeScale
```

Scene Stack 도입 후 (현재):
```
gameState = 런 데이터 (score, fragments, timeScale)만 관리
sceneStack = 상태 전이 (어떤 화면이 활성인지) 담당
```

gameState.isTitle(), isPaused() 등은 Scene Stack이 대체했다.

> **참고: tutorialHints는 Scene이 아님**
> tutorialHints.lua는 PlayScene 내부에서 update/draw되는 모듈.
> 독립적 인풋 핸들링 없고 게임 재생 중 오버레이로 반투명 표시되므로 Scene 분리 불필요.

---

## 7. ECS와의 관계

ECS와 Scene Stack은 서로 다른 문제를 해결한다:

```
ECS  = "게임 내 엔티티(플레이어, 적, 총알)를 어떻게 관리할 것인가"
       → 데이터 지향 설계. 성능 최적화. 컴포넌트 조합.

Scene = "게임의 화면/상태(타이틀, 플레이, 일시정지)를 어떻게 전환할 것인가"
       → UI 상태 관리. Input 라우팅. Draw 순서.
```

둘은 직교(orthogonal) 관계:
- PlayScene 안에서 ECS가 동작한다.
- TitleScene에서는 ECS가 비활성이다.
- Scene이 ECS를 소유하는 게 아니라, PlayScene이 ecsManager를 **참조**한다.

```
┌─ SceneStack ─────────────────────────┐
│                                       │
│  ┌─ PlayScene ──────────────────┐    │
│  │  ┌─ ECS World ───────────┐   │    │
│  │  │ entities, components,  │   │    │
│  │  │ systems, bulletPool    │   │    │
│  │  └────────────────────────┘   │    │
│  │  camera, background, HUD     │    │
│  └───────────────────────────────┘    │
│                                       │
│  ┌─ PauseScene (overlay) ─┐          │
│  │  dim + menu options     │          │
│  └─────────────────────────┘          │
└───────────────────────────────────────┘
```

---

## 8. 향후 확장: Event Bus (선택적)

Scene Stack 도입 후에도 남는 문제가 있다면 Event Bus를 보조로 도입할 수 있다:

```lua
-- 현재 (역방향 의존):
-- levelUp.lua 안에서:
gameState.setTimeScale(0)   -- UI가 게임 상태를 직접 조작

-- Event Bus 적용 후:
events.emit("gameplay:freeze")      -- 의도만 선언
-- PlayScene이 구독해서 자신의 timeScale을 변경
```

단, Event Bus는 **디버깅이 어려워지는 단점**이 있으므로, Scene Stack 전환 후 실제로 필요한 시점에 도입한다.

---

## 9. 참고 자료

| 출처 | 설명 |
|------|------|
| [Game Programming Patterns — State](https://gameprogrammingpatterns.com/state.html) | 게임 상태 패턴 고전 참고서 |
| [HUMP Gamestate](https://hump.readthedocs.io/en/latest/gamestate.html) | LÖVE2D용 Scene Stack 라이브러리 (참고, 미사용) |
| Unity SceneManager | 씬 로드/언로드의 산업 표준 구현 |
| Cocos2d Director | push/pop 기반 씬 전환 패턴의 원조 |

---

## 부록 A: 기술 선택 비교 — ECS vs Scene Stack

| 구분 | ECS (Phase 1에서 선택) | Scene Stack (Phase 4에서 선택) |
|------|----------------------|------------------------------|
| 해결 문제 | 엔티티 수천 개 관리 + 성능 | UI 상태 전환 + Input 라우팅 |
| 도입 시점 | 프로젝트 초기 (핵심 게임 루프) | UI 복잡도 증가 시점 |
| 대안 | OOP 상속 / Component-only | if-else 수동 분기 / FSM |
| 대안 기각 이유 | 1000+ 엔티티 성능 + 조합 유연성 | FSM은 오버레이 불가, if-else는 O(n) |
| 구현 규모 | ~300줄 (ecs.lua + system.lua) | ~60줄 (sceneStack.lua) |
| 영향 범위 | 03_game/ 전체 | main.lua + 03_game/states/ |
