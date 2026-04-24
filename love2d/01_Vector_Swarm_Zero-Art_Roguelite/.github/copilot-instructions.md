# Copilot Instructions — Vector Swarm

## Core Values (판단 기준)

모든 설계·구현 결정에서 이 순서를 따른다:
1. **재미 (Fun)** — 플레이어가 즐거운가? No면 만들지 않는다
2. **쾌감 (Juice)** — 시각·촉각 피드백이 기분 좋은가? 밋밋하면 미완성
3. **제약의 미학 (Constraint Beauty)** — 코드만으로 아름다움을 만드는가? Zero-Art의 존재 이유

성능(60fps)은 가치가 아니라 필수 조건.

---

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
│   ├── global.lua    # Injects globals: log, setColor, resetColor
│   ├── logger.lua    # 4-level logging + in-game console (` key)
│   ├── debug.lua     # In-game debug overlay (key-value watch panel)
│   ├── gridDebugDraw.lua  # Screen-space grid overlay (F4)
│   └── saveData.lua  # love.filesystem JSON save/load (fragments, upgrades, tutorialDone)
├── 01_core/          # Engine layer (pure engine, no game deps)
│   ├── world.lua     # World boundaries, zones, fun elements
│   ├── ecs.lua       # ECS core (entity/component management, componentIndex cache)
│   ├── system.lua    # System base class (performance monitoring)
│   └── sceneStack.lua # Scene Stack engine (push/pop/replace/clear, transparent/drawBelow)
├── 02_renderer/      # Camera & rendering & post-processing
│   ├── camera.lua    # Unity-style orthographic camera
│   ├── cameraManager.lua  # Game/debug camera modes (F5 toggle)
│   ├── bloom.lua     # Bloom post-processing (threshold + Gaussian blur)
│   └── background.lua # Random Space Filling background (Paul Bourke)
├── 03_game/          # Game logic
│   ├── ecsManager.lua # ECS orchestrator (system registration, update/draw split)
│   ├── components/   # Pure-data ECS components (17 types)
│   │   ├── transform.lua, velocity.lua, collider.lua, renderable.lua
│   │   ├── lifespan.lua, playerTag.lua, input.lua, worldBound.lua
│   │   ├── health.lua, dash.lua, focus.lua, enemyAI.lua
│   │   ├── bulletEmitter.lua, playerWeapon.lua, playerXP.lua
│   │   ├── xpOrb.lua, bossTag.lua
│   ├── data/         # Pure data tables (no logic, no require)
│   │   ├── bossDefs.lua       # Boss presets (stats, patterns, AI)
│   │   ├── deityDefs.lua      # Deity system (4 gods, passive + signature abilities)
│   │   ├── stageData.lua      # Stage defs, enemy pools, variant tables, boss mapping
│   │   ├── formationDefs.lua  # Formation patterns (wedge, pincer, etc.)
│   │   ├── curveDefs.lua      # Math curves library (53 parametric/polar)
│   │   ├── shapeDefs.lua      # Curve curation (enemy/boss/overlay groups)
│   │   ├── dnaDefs.lua        # Procedural enemy DNA (Stage 16+)
│   │   └── stageStory.lua     # Stage story text (normal/boss/endless)
│   ├── systems/      # ECS systems (17 files)
│   │   ├── inputSystem.lua, movementSystem.lua, boundarySystem.lua
│   │   ├── lifespanSystem.lua, renderSystem.lua, playerRenderSystem.lua
│   │   ├── bulletEmitterSystem.lua, bulletPool.lua, collisionSystem.lua
│   │   ├── dashSystem.lua, focusSystem.lua, enemyAISystem.lua
│   │   ├── playerWeaponSystem.lua, enemyCollisionSystem.lua
│   │   ├── xpCollectionSystem.lua, bossSystem.lua
│   │   ├── stageManager.lua, enemySpawner.lua
│   │   └── renderers/        # Strategy Pattern render modules
│   │       ├── basicShapes.lua    # 6 basic enemy shapes (auto-registered)
│   │       ├── bossRenderers.lua  # 5 boss visuals (auto-registered)
│   │       └── variantOverlays.lua # 4 variant overlays (auto-registered)
│   ├── entities/     # Entity factories & façades
│   │   ├── entityFactory.lua  # createPlayer(), createEnemy(), createBoss(), createXpOrb()
│   │   └── player.lua         # ECS façade (bind, update, getPosition)
│   ├── scenes/       # Scene wrappers (Scene Stack pattern)
│   │   ├── playScene.lua      # Game loop scene (ECS + rendering + camera)
│   │   ├── titleScene.lua     # Title menu scene
│   │   ├── deitySelectScene.lua # Deity ritual selection (2×2 animated curves)
│   │   ├── pauseScene.lua     # Pause overlay scene (drawBelow)
│   │   ├── levelUpScene.lua   # Level-up overlay (auto-pop)
│   │   ├── upgradeScene.lua   # Upgrade tree overlay (auto-pop)
│   │   ├── gameOverScene.lua  # Game over result scene
│   │   ├── victoryScene.lua   # Victory cinematic (glitch + stats)
│   │   ├── galleryScene.lua   # Enemy gallery (3 pages)
│   │   ├── curveLabScene.lua  # Curve lab browser (53+ curves)
│   │   ├── creditsScene.lua   # Credits overlay scene (Paul Bourke attribution)
│   │   └── achievementScene.lua # Achievement list overlay
│   ├── patterns/     # Bullet patterns (planned)
│   └── states/       # Game states
│       ├── gameState.lua      # Game state machine (playing/game_over/level_up)
│   │   ├── levelUp.lua        # Level-up 3-choice UI + diminishing returns
│   │   ├── achievementSystem.lua # Achievement tracking + unlock management
│   │   ├── upgradeTree.lua    # Permanent upgrade tree (Data Fragments)
│   │   ├── titleMenu.lua      # Title screen menu (PLAY/UPGRADES/CREDITS)
│   │   ├── pauseMenu.lua      # In-game pause overlay
│   │   └── tutorialHints.lua  # First-play contextual hints (slowmo + glitch text)
├── 04_ui/            # HUD, mobile layout, button controls
│   ├── uiManager.lua
│   ├── topHud.lua
│   ├── bottomControls.lua
│   ├── mobileLayout.lua
│   └── minimap.lua   # World minimap overlay (resolution-proportional)
└── 05_sound/         # Zero-Art procedural audio
    ├── synth.lua     # Oscillator engine (5 waveforms, ADSR, freq sweep)
    ├── soundManager.lua # Hybrid loader (ext_res file → code gen fallback)
    └── sfxDefs.lua   # SFX recipes (6 sound effects)
```

### Key cross-layer rules
- Higher-numbered layers may `require` lower-numbered layers; never the reverse.
- `00_common/global.lua` must be loaded **first** in `love.load()`; it defines globals used everywhere (`log`, `setColor`, etc.).
- `logger.init()` is called immediately after `global.init()`.

> **파일별 상세 API, 의존 관계, 데이터 흐름은 `src/SOURCE_MAP.md` 참조.**

### ECS architecture (`01_core/` + `03_game/ecsManager.lua`)
- **ecs.lua**: Entity lifecycle (create/destroy with ID recycling), component CRUD, `queryEntities()` with pivot optimization and `componentIndex` cache.
- **system.lua**: `System.new(name, requiredComponents, updateFn)` — base class with built-in performance timing.
- **ecsManager.lua** (`03_game/`): Orchestrates all systems. `update(dt)` runs logic systems; `draw()` runs render systems separately. Systems registered via `addSystem()` execute in registration order.

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

## Data-Driven Modification Patterns (수정 가이드)

확장이 잦은 작업을 유형별로 정리. 각 데이터/렌더러 파일 헤더에도 동일 가이드가 있다.

### 새 적(Enemy) 타입 추가
1. `03_game/data/stageData.lua` → `ALL_ENEMY_TYPES` + 해당 풀(`MOBILITY_POOL` 또는 `FIREPOWER_POOL`)에 등록
2. `03_game/entities/entityFactory.lua` → `createEnemy()` 에 타입별 스탯 추가
3. `03_game/systems/renderers/basicShapes.lua` → `M.타입이름 = function(x, y, r, renderable, transform)` 추가
4. `renderSystem.lua` 수정 불필요 (dispatch 자동 등록)

### 새 보스(Boss) 추가
1. `03_game/data/bossDefs.lua` → `BOSS_TYPES["NAME"] = { ... }` 추가
2. `03_game/data/stageData.lua` → `BOSS_STAGES[스테이지] = "NAME"` + `BOSS_SEQUENCE`에 추가
3. `03_game/systems/renderers/bossRenderers.lua` → `M.boss_name = function(...)` 추가
4. `renderSystem.lua`, `bossSystem.lua` 수정 불필요 (자동 등록 / 패턴 기반)

### 새 변형(Variant) 추가
1. `03_game/data/stageData.lua` → `GUARANTEED_VARIANTS[스테이지]` + `VARIANT_TIERS`에 등록
2. `03_game/entities/entityFactory.lua` → 변형 스탯 보너스 정의
3. `03_game/systems/renderers/variantOverlays.lua` → `M.변형이름 = function(x, y, r, renderable, ecs, entityId)` 추가
4. `renderSystem.lua` 수정 불필요 (자동 등록)

### 새 포메이션(Formation) 추가
1. `03_game/data/formationDefs.lua` → `M.DEFS` 배열에 `{name, types, tier, getOffsets()}` 추가
2. tier 값으로 등장 시작 스테이지 제어 (1=Stage4+, 2=Stage6+, 3=Stage9+)
3. `stageManager.lua` 수정 불필요

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
- `clean-code-guide` 스킬 기준으로 점검 (math 캐시, 로그 포맷, 모듈 패턴, 레이어 규칙 등)
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

### 자동 챙김 규칙
- 커밋 시 work_log에 해당 작업 내역이 반영되었는지 확인하고, 안 됐으면 같이 업데이트한다
- 기능(Phase/태스크) 완료 시 `doc/00_PROJECT_ROADMAP.md` 체크박스를 업데이트한다
- 세션에서 3회 이상 커밋했는데 `git push`를 안 했으면 알려준다
- **3커밋마다 또는 Phase 완료 시** `refactor-cleaner` 기준으로 변경 파일들 정리 (dead code, 중복, 캐시 누락 등)
