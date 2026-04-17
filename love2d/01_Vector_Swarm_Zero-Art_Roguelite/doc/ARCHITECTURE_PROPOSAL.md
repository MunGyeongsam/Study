# Vector Swarm 아키텍처 현황

## 현재 구현 상태

ECS(Entity-Component-System) 아키텍처를 기반으로 한 레이어 구조.
Unity 스타일 카메라 시스템과 모바일-우선 입력 파이프라인을 갖추고 있습니다.

## 1. 실제 구조

```
src/
  main.lua               # LÖVE 콜백 (load/update/draw/input)
  conf.lua               # LÖVE 창 설정 (432×960, 9:20 세로)
  00_common/             # 공통 유틸리티 — 게임 의존성 없음
    global.lua             # 전역 함수 (log, clamp, lerp, setColor, …)
    logger.lua             # 4레벨 로깅 + 인게임 콘솔 (` 키)
    debug.lua              # 디버그 watch panel (F1)
    gridDebugDraw.lua      # 스크린 그리드 오버레이 (F4)
    kutil.lua              # 기타 유틸리티
    math/
      vector.lua           # 2D 벡터 연산
      matrix.lua           # 행렬 연산
  01_core/               # 엔진 레이어
    world.lua              # 월드 경계, 존, 재미 요소
    ecs.lua                # ECS 코어 (엔티티/컴포넌트, componentIndex 캐시)
    system.lua             # 시스템 베이스 클래스 (성능 모니터링)
    ecsManager.lua         # ECS 오케스트레이터 (시스템 등록, update/draw 분리)
  02_renderer/           # 카메라 & 렌더링
    camera.lua             # Unity 스타일 orthographic 카메라
    cameraManager.lua      # game/debug 카메라 모드 (F5 토글)
  03_game/               # 게임 로직
    components/            # 순수 데이터 ECS 컴포넌트 (8종)
      transform.lua          # 위치, 각도, 스케일
      velocity.lua           # 속도, 최대속력, 감속
      collider.lua           # 충돌체 (원형/AABB)
      renderable.lua         # 렌더링 속성 (색상, 반지름, 도형)
      lifespan.lua           # 수명 (자동 제거)
      playerTag.lua          # 플레이어 식별 태그
      input.lua              # 입력 설정 (속도, 활성화)
      worldBound.lua         # 월드 경계 제한
    systems/               # ECS 시스템 (6종)
      inputSystem.lua        # 키보드/터치 → Velocity
      movementSystem.lua     # Velocity → Transform
      boundarySystem.lua     # 월드 경계 clamping
      lifespanSystem.lua     # 수명 만료 엔티티 제거
      renderSystem.lua       # 일반 엔티티 렌더링
      playerRenderSystem.lua # 플레이어 전용 렌더링 (삼각형+방향)
    entities/              # 엔티티 팩토리 & 파사드
      entityFactory.lua      # createPlayer(), createEnemy()
      player.lua             # ECS 파사드 (bind, update, getPosition)
    patterns/              # 탄막 패턴 (예정)
    states/                # 게임 상태 (예정)
  04_ui/                 # HUD, 모바일 레이아웃
    uiManager.lua          # UI 총괄 (터치 소비 우선)
    topHud.lua             # 상단 HUD (점수, 설정)
    bottomControls.lua     # 하단 버튼 컨트롤
    mobileLayout.lua       # 모바일 레이아웃 (영역 분할)
```

## 2. 핵심 설계 원칙

### 2.1. 레이어 의존성
```
04_ui → 03_game → 02_renderer → 01_core → 00_common
```
- 상위 → 하위 require만 허용 (역방향 절대 금지)
- `main.lua`는 예외: 모든 레이어 require 가능 (진입점)

### 2.2. ECS 아키텍처
- **컴포넌트**: 순수 데이터 (`{ name, defaults, new(data) }` 패턴)
- **시스템**: `System.new(name, requiredComponents, updateFn)` — 로직 처리
- **엔티티**: 고유 ID (생성/파괴, ID 재활용)
- **ecsManager**: update(dt)에서 로직 시스템 실행, draw()에서 렌더 시스템만 실행

### 2.3. Unity 스타일 카메라
- Orthographic Size: 카메라가 보는 월드의 절반 높이
- Viewport Control: 화면 내 카메라 중심 위치
- 수학적 좌표계: (0,0) 중심, Y+ 상향
- cameraManager: game 모드 (플레이어 추적) + debug 모드 (자유 이동/줌)

### 2.4. 모듈 패턴
```lua
local M = {}
function M.init() end
function M.update(dt) end
function M.draw() end
return M
```

## 3. 시스템 실행 순서

```
love.update(dt):
  1. InputSystem      — 키보드/터치 → Velocity 컴포넌트
  2. MovementSystem   — Velocity → Transform 위치 갱신
  3. BoundarySystem   — 월드 경계 clamping
  4. LifeSpanSystem   — 수명 만료 엔티티 제거
  5. player.update()  — 존 감지, 파워업 (ECS 파사드)
  6. cameraManager    — 카메라 추적/업데이트

love.draw():
  cameraManager.draw(function()
    1. RenderSystem       — 일반 엔티티 렌더링
    2. PlayerRenderSystem — 플레이어 삼각형+방향
    3. world.draw()       — 월드 디버그 그리기
  end)
  4. UI 렌더링 (스크린 좌표)
  5. Debug overlay / Logger console
```

## 4. 데이터 흐름

### 4.1. 엔티티 생성
```lua
-- EntityFactory.createPlayer(ecs, x, y)
entityId = ecs:createEntity()
ecs:addComponent(entityId, "Transform", Transform.new({x=x, y=y}))
ecs:addComponent(entityId, "Velocity", Velocity.new({maxSpeed=5}))
ecs:addComponent(entityId, "Input", Input.new({speed=5}))
ecs:addComponent(entityId, "PlayerTag", PlayerTag.new())
...
```

### 4.2. Player 파사드 바인딩
```lua
player.bind(ecsManager.getWorld(), playerId)
-- player.update(dt)는 ECS 외부 로직만 담당 (존 감지 등)
-- 입력은 InputSystem, 렌더링은 PlayerRenderSystem이 처리
```

## 5. 키 바인딩

| 키 | 기능 |
|----|------|
| `` ` `` | Logger 콘솔 토글 |
| F1 | Debug watch panel 토글 |
| F2 | UI 표시 토글 |
| F3 | UI 디버그 모드 토글 |
| F4 | Screen grid 토글 |
| F5 | Camera 모드 전환 (game ↔ debug) |
| ESC | 게임 종료 |

## 6. 향후 확장 방향

### 6.1. 탄막/불릿 시스템 (Phase 2C)
- 오브젝트 풀링 (bulletPool)
- BulletPattern 컴포넌트 + BulletPatternSystem
- Canvas 배치 렌더링
- 공간 파티셔닝 (spatialGrid)

### 6.2. 적 AI 시스템
- AIComponent + AISystem
- 이동/공격 패턴 정의

### 6.3. 게임 스테이트 머신
- title → playing → gameover 흐름
- states/ 폴더에 상태별 모듈

### 6.4. 충돌 시스템
- CollisionSystem (Transform + Collider 쿼리)
- 레이어 기반 충돌 필터링

### 6.5. 모바일 최적화
- 터치 인터페이스 고도화
- 배터리 효율 (불필요한 계산 최소화)
- 다양한 화면비 대응 (18:9 ~ 20:9)
    
    -- 스크린 좌표계 UI
    logger.drawConsole()
end

function love.keypressed(key)
    if key == "+" then
        -- 줌인 (orthographic size 감소)
        local newSize = mainCamera:getOrthographicSize() * 0.8
        mainCamera:setOrthographicSize(newSize)
    end
end
```

### 카메라 viewport 활용:
```lua
-- 화면 분할 예시
local w, h = love.graphics.getDimensions() 
mainCamera:setViewport(w/4, h/2)    -- 좌측에 카메라 배치
miniCamera:setViewport(w*3/4, h/4)  -- 우상단에 미니맵
```

이처럼 현재 구조는 **단순하면서도 강력한** 기반을 제공합니다.

## 4. 예시 흐름

1. **입력(Controller):**
   - 터치/스와이프 → core/input.lua → game/systems/TouchInputSystem
2. **로직(Model):**
   - 입력 결과로 ECS 컴포넌트(Transform, Velocity 등) 갱신
   - 시스템(Movement, Collision, Pattern 등)이 컴포넌트 처리
3. **출력(View):**
   - 렌더링 시스템이 ECS 상태를 기반으로 화면 그리기
   - UI 시스템이 점수/체력 등 HUD 표시

## 5. 상태(State) 관리
- game/states/IntroState.lua, PlayState.lua, GameOverState.lua 등으로 분리
- 각 상태에서 필요한 시스템만 활성화

## 6. 확장성/유지보수성
- 각 레이어/시스템/컴포넌트가 독립적이어서 기능 추가·수정이 용이
- 모바일 성능 최적화(ECS, 배치 렌더링, 상태 기반 시스템 관리)

---

이 구조는 문서의 "모바일 최적화", "ECS", "레이어 분리", "상태 관리", "확장성" 요구를 모두 충족합니다.

구체적 예시 코드나 각 시스템/컴포넌트 설계가 필요하면 언제든 말씀해 주세요!