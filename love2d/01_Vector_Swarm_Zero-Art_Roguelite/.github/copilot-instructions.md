# Copilot Instructions — Vector Swarm

## Running the Game

**macOS:**
```bash
./run.sh
```

**Windows:**
```bat
run.bat
```

**VS Code:** Press `F5` (uses bundled LÖVE 11.5 in `love-11.5-macos/` or `love-11.5-win64/`).

There is no test suite or linter. Validation is done by running the game.

---

## Architecture

The game is a **LÖVE2D 11.5 / Lua** project. Source lives entirely under `src/` and uses a **numbered layer system** — load order reflects dependency direction:

```
src/
├── main.lua          # LÖVE callbacks (load / update / draw / input)
├── conf.lua          # LÖVE window config (432×960 portrait, 9:20)
├── 00_common/        # Utilities — loaded first, no game dependencies
│   ├── global.lua    # Injects globals: log, clamp, lerp, setColor, …
│   ├── logger.lua    # 4-level logging + in-game console (` key)
│   ├── debug.lua     # In-game debug overlay (key-value watch panel)
│   ├── gridDebugDraw.lua  # Screen-space grid overlay (F4)
│   ├── kutil.lua     # Misc utilities
│   └── math/         # Vector / matrix helpers
├── 01_core/          # Engine layer
│   ├── world.lua     # World boundaries, zones, fun elements
│   ├── ecs.lua       # ECS core (entity/component management, componentIndex cache)
│   ├── system.lua    # System base class (performance monitoring)
│   └── ecsManager.lua # ECS orchestrator (system registration, update/draw split)
├── 02_renderer/      # Camera & rendering
│   ├── camera.lua    # Unity-style orthographic camera
│   └── cameraManager.lua  # Game/debug camera modes (F5 toggle)
├── 03_game/          # Game logic
│   ├── components/   # Pure-data ECS components (8 types)
│   │   ├── transform.lua, velocity.lua, collider.lua, renderable.lua
│   │   ├── lifespan.lua, playerTag.lua, input.lua, worldBound.lua
│   ├── systems/      # ECS systems (6 types)
│   │   ├── inputSystem.lua, movementSystem.lua, boundarySystem.lua
│   │   ├── lifespanSystem.lua, renderSystem.lua, playerRenderSystem.lua
│   ├── entities/     # Entity factories & façades
│   │   ├── entityFactory.lua  # createPlayer(), createEnemy()
│   │   └── player.lua         # ECS façade (bind, update, getPosition)
│   ├── patterns/     # Bullet patterns (planned)
│   └── states/       # Game states (planned)
└── 04_ui/            # HUD, mobile layout, button controls
    ├── uiManager.lua
    ├── topHud.lua
    ├── bottomControls.lua
    └── mobileLayout.lua
```

### Key cross-layer rules
- Higher-numbered layers may `require` lower-numbered layers; never the reverse.
- `00_common/global.lua` must be loaded **first** in `love.load()`; it defines globals used everywhere (`log`, `clamp`, `lerp`, `setColor`, etc.).
- `logger.init()` is called immediately after `global.init()`.

### ECS architecture (`01_core/`)
- **ecs.lua**: Entity lifecycle (create/destroy with ID recycling), component CRUD, `queryEntities()` with pivot optimization and `componentIndex` cache.
- **system.lua**: `System.new(name, requiredComponents, updateFn)` — base class with built-in performance timing.
- **ecsManager.lua**: Orchestrates all systems. `update(dt)` runs logic systems; `draw()` runs render systems separately. Systems registered via `addSystem()` execute in registration order.

### Component pattern (`03_game/components/`)
Each component file exports `{ name, defaults, new(data) }`:
```lua
local M = {}
M.name = "Transform"
M.defaults = { x = 0, y = 0, angle = 0, scale = 1 }
function M.new(data) ... end
return M
```

### Player ECS façade (`03_game/entities/player.lua`)
- `player.bind(ecs, entityId)` — binds to an ECS entity created by `entityFactory.createPlayer()`.
- `player.update(dt)` — handles world interactions (zone detection, power-ups) only.
- Rendering is handled by `PlayerRenderSystem`; input by `InputSystem`.

### Camera system (`02_renderer/camera.lua` + `cameraManager.lua`)
- Unity-style orthographic camera: `orthographicSize` = half-height in world units.
- World coordinate system is centered at `(0, 0)`; Y increases upward.
- `camera:draw(fn)` applies the transform; all world rendering happens inside that callback.
- `cameraManager` manages game camera (player-following) and debug camera (free pan/zoom).
- Mouse input is forwarded to touch callbacks (`love.mousepressed` → `love.touchpressed`) for PC prototyping.

### Touch / input pipeline
1. `love.touchpressed/moved/released` are the canonical input callbacks.
2. Mouse events are bridged to touch in `main.lua` (PC prototype only).
3. `uiManager` consumes touch first; unconsumed events reach game-play logic.
4. `mobileLayout.isTouchInArea(x, y, "play")` gates game-area input.

---

## Key Conventions

### Key bindings (current)
| Key | Function |
|-----|----------|
| `` ` `` | Logger console toggle |
| F1 | Debug watch panel toggle |
| F2 | UI visibility toggle |
| F3 | UI debug mode toggle |
| F4 | Screen grid toggle |
| F5 | Camera mode toggle (game ↔ debug) |
| F7 | God mode toggle |
| F8 | Stage skip (debug) |
| ESC | Quit game |

### Global helpers (injected by `00_common/global.lua`)
Use these everywhere — do **not** use raw `print()` for debug output:
```lua
log("message")          -- alias for logger.info
logDebug("…")
logWarn("…")
logError("…")
setColor(r, g, b, a)    -- accepts 0-255; wraps love.graphics.setColor
resetColor()
clamp(v, min, max)
lerp(a, b, t)
distance(x1,y1, x2,y2)
normalize(x, y)         -- returns nx, ny
```

### Logger module API
```lua
local logger = require("00_common.logger")
logger.init()                        -- call in love.load
logger.debug/info/warn/error("msg")
logger.toggleConsole()               -- ` key toggle
logger.drawConsole(fonts.small)      -- call in love.draw
logger.close()                       -- call in love.quit
```

### Log message format
Use ASCII tags instead of emoji for log messages (no Korean font loaded):
```lua
logInfo("[CAM] Camera initialized")     -- not "📹 Camera initialized"
logInfo("[ECS] System registered")      -- not "🏗️ ECS ..."
logInfo("[PLAYER] Collected: shield")   -- not "🎁 Player collected: ..."
```

### Debug overlay
- `debug.add("label", fn)` registers a watch that displays `fn()` result each frame.
- Toggle with **F1**.
- **F2** toggles UI visibility; **F3** toggles UI debug mode.

### Module pattern
Every module returns a table. Stateful modules expose `.init()` / `.update(dt)` / `.draw()`:
```lua
local M = {}
function M.init() … end
function M.update(dt) … end
function M.draw() … end
return M
```

### World coordinate system
- World size: `120 × 250` world units; center `(0, −100)`.
- Camera `orthographicSize = 5` → 10 world units visible height.
- Zones are defined as axis-aligned rectangles in world space (see `01_core/world.lua`).

### Performance targets
- 60 FPS with 1000+ entities.
- Avoid `local` declarations inside `love.update()` hot paths.
- Prefer `string.format` over `..` concatenation in loops.
- Plan bullet systems around **object pooling** (`bulletPool` table, fixed-size arrays).
- Cache repeated `math.sin`/`math.cos` results in lookup tables.

### Commit message format
```
feat:     새로운 기능
fix:      버그 수정
docs:     문서 업데이트
refactor: 코드 리팩토링
perf:     성능 최적화
```

---

## Development Workflow (작업 흐름)

기능 구현 시 반드시 다음 순서를 따른다:

### 1. 계획 (Plan)
- 구현할 내용 정리 (무엇을, 왜, 어디에)
- 설계 결정이 필요하면 **선택지 + 각각의 근거**를 제시하고 사용자가 결정
- 결정 없이 임의로 진행하지 않는다

### 2. 구현 (Implement)
- 파일별로 나눠서 **단계적으로** 작성
- 각 단계마다 핵심 로직과 "왜 이렇게 했는지" 논리 설명

### 3. 코드 리뷰 (Review Together)
- 테스트 **전에** 완성된 코드를 같이 보면서 리뷰
- 전체 흐름, 핵심 로직, 설계 이유 설명
- 사용자의 질문을 받고 답변

### 4. 테스트 (Test)
- 게임 실행 (`F5` 또는 `run.bat` / `run.sh`)
- **체크리스트 기반 검증** (테스트 전에 체크리스트 먼저 작성)
- **회귀 테스트 (Regression Test)**: 리팩터링이 기존 기능을 건드렸을 때, 영향받는 기존 기능들을 먼저 테스트한 후 신규 기능 테스트

### 5. 검증 (Verify)
- 테스트 결과 확인
- 엣지 케이스 체크 (경계값, nil, 비정상 입력 등)
- 문제 발견 시 → 2번(구현)으로 돌아가 수정

### 6. 커밋 (Commit)
- 변경 사항 정리
- 한글 커밋 메시지 (prefix는 영문: `feat:`, `fix:`, etc.)

### 작업 로그 규칙
- 작업 시작 시 `work_log/` 에 당일 파일 생성 (없으면) 또는 기존 파일에 추가
- 파일 형식: `work_log/NNNN_YYYY-MM-DD_log.md` (번호는 순차 증가)
- 작업 중 핵심 결정/변경사항은 work_log에 바로 기록 (세션 메모 사용하지 않음)
- 커밋할 때 work_log도 함께 커밋하여 **기기 간 공유** 보장
- 기기 전환 시 코드 커밋이 어려우면, 최소한 work_log만이라도 커밋 (`docs: 작업 로그 중간 저장`)
