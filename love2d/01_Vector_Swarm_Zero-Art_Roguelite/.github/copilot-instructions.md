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
│   ├── logger.lua    # 4-level logging system (DEBUG/INFO/WARN/ERROR)
│   ├── debug.lua     # In-game debug overlay (key-value watch panel)
│   └── math/         # Vector / matrix helpers
├── 01_core/          # Engine layer (world boundaries, future ECS)
├── 02_renderer/      # Camera (orthographic, Unity-style)
├── 03_game/          # Game logic — entities, systems, patterns, states
└── 04_ui/            # HUD, mobile layout, button controls
```

### Key cross-layer rules
- Higher-numbered layers may `require` lower-numbered layers; never the reverse.
- `00_common/global.lua` must be loaded **first** in `love.load()`; it defines globals used everywhere (`log`, `clamp`, `lerp`, `setColor`, etc.).
- `logger.init()` is called immediately after `global.init()`.

### Camera system (`02_renderer/camera.lua`)
- Unity-style orthographic camera: `orthographicSize` = half-height in world units.
- World coordinate system is centered at `(0, 0)`; Y increases upward.
- `camera:draw(fn)` applies the transform; all world rendering happens inside that callback.
- Mouse input is forwarded to touch callbacks (`love.mousepressed` → `love.touchpressed`) for PC prototyping.

### Touch / input pipeline
1. `love.touchpressed/moved/released` are the canonical input callbacks.
2. Mouse events are bridged to touch in `main.lua` (PC prototype only).
3. `uiManager` consumes touch first; unconsumed events reach game-play logic.
4. `mobileLayout.isTouchInArea(x, y, "play")` gates game-area input.

---

## Key Conventions

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
logger.drawConsole(fonts.small)      -- call in love.draw
logger.close()                       -- call in love.quit
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
