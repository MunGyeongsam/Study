# API 가이드 - Vector Swarm 현재 구현

이 문서는 현재 구현된 시스템들의 실제 사용법을 정리한 것입니다.

## 1. 카메라 시스템 (02_renderer/camera.lua)

### 1.1. 기본 생성 및 설정
```lua
local camera = require("02_renderer.camera")

-- Unity 스타일 카메라 생성 (x, y, orthographicSize, rotation, viewportX, viewportY)
local mainCamera = camera.new(0, 0, 5)  -- 월드 (0,0)에서 시작, orthographic size 5

-- 카메라 위치 설정
mainCamera:lookAt(10, 5)    -- 특정 위치 보기
mainCamera:move(2, 1)       -- 상대적 이동
mainCamera:pos()            -- 현재 위치 반환

-- 줌 제어 
mainCamera:setOrthographicSize(10)      -- 직접 설정
mainCamera:zoom(2)                      -- 2배 줌인 (orthographic size 절반)
mainCamera:zoomTo(1.5)                  -- 1.5배 줌으로 설정

-- 회전
mainCamera:rotate(math.pi/4)            -- 45도 회전
mainCamera:rotateTo(0)                  -- 0도로 설정
```

### 1.2. Viewport 제어 (분할 화면, 미니맵 등)
```lua
-- 화면 중심 사용 (기본값)
mainCamera:setViewportToCenter()

-- 특정 화면 위치에 카메라 중심 설정
local w, h = love.graphics.getDimensions()
mainCamera:setViewport(w/4, h/2)        -- 좌측에 카메라 배치
mainCamera:setViewport(w*3/4, h/4)      -- 우상단에 미니맵

-- 현재 viewport 위치 확인
local vx, vy = mainCamera:getViewport()
```

### 1.3. 변환 계산 
```lua
-- 픽셀 정확도 계산
local pixelsPerUnit = mainCamera:getPixelsPerUnit()        -- 월드 1유닛이 몇 픽셀인지
local unitsPerPixel = mainCamera:getUnitsPerPixel()        -- 1픽셀이 몇 월드 유닛인지

-- 좌표 변환
local screenX, screenY = mainCamera:cameraCoords(worldX, worldY)  -- 월드 → 스크린
local worldX, worldY = mainCamera:worldCoords(screenX, screenY)   -- 스크린 → 월드

-- 입력 좌표 변환 (마우스/터치)
local mouseWorldX, mouseWorldY = mainCamera:mousepos()            -- 마우스 월드 좌표
-- 모바일에서는 touch 좌표를 worldCoords로 변환 가능
```

### 1.4. 렌더링
```lua
function love.draw()
    -- 월드 좌표계에서 그리기
    mainCamera:draw(function()
        -- 이 블록 안에서는 월드 좌표 사용
        love.graphics.circle("fill", 0, 0, 1)  -- 원점에 1유닛 반지름 원
        love.graphics.line(-5, 0, 5, 0)        -- X축 선
    end)
    
    -- 스크린 좌표계 UI (카메라 영향 없음)
    love.graphics.print("Score: 1000", 10, 10)
end
```

---

## 2. 월드 시스템 (01_core/world.lua)

### 2.1. 초기화 및 설정
```lua
local world = require("01_core.world")

-- 초기화 
world.init()

-- 월드 크기 정보 
local width, height = world.getSize()           -- 21, 21 (기본값)
local left, bottom, right, top = world.getBounds()   -- -10.5, -10.5, 10.5, 10.5
```

### 2.2. 그리드 렌더링
```lua
-- 카메라 내부에서 호출 (픽셀 정확도 적용)
mainCamera:draw(function()
    world.drawGrid(2, mainCamera)  -- 2유닛 간격 그리드, 카메라 정보 전달
end)

-- 카메라 없이 호출 (fallback 크기 사용)
world.drawGrid(1)  -- 기본 크기로 그리드
```

---

## 3. 로깅 시스템 (00_common/logger.lua) 

### 3.1. 기본 로깅
```lua
local logger = require("00_common.logger")

-- 초기화 (main.lua에서 한 번만)
logger.init()

-- 로깅 (문자열만 받음 — format은 호출자가 처리)
logger.info("[TAG] message")
logger.warn("[TAG] warning")
logger.error("[TAG] error")
logger.debug("[TAG] debug info")

-- 포매팅 예시
logger.info(string.format("[PLAYER] Position: (%.1f, %.1f)", x, y))
```

### 3.2. 인게임 콘솔
```lua
-- 콘솔 토글 (` 키에 바인딩, 기본값: 숨김)
logger.toggleConsole()

-- 콘솔 렌더링 (love.draw 맨 마지막에 호출)
logger.drawConsole(fonts.small)

-- 콘솔 상태 확인
logger.isConsoleVisible()
```

### 3.3. 로그 메시지 규칙
```lua
-- ASCII 태그 사용 (이모지/한글 금지 — 기본 폰트에서 깨짐)
logInfo("[CAM] Camera initialized")       -- O
logInfo("[ECS] System registered")        -- O
logInfo("📹 Camera initialized")         -- X (이모지 깨짐)
```

---

## 4. ECS 시스템 (01_core/)

### 4.1. ECS 코어 (ecs.lua)
```lua
local ECS = require("01_core.ecs")
local ecs = ECS.new()

-- 엔티티 생성/제거
local id = ecs:createEntity()
ecs:destroyEntity(id)

-- 컴포넌트 CRUD
ecs:addComponent(id, "Transform", {x = 0, y = 0, angle = 0, scale = 1})
local transform = ecs:getComponent(id, "Transform")
ecs:removeComponent(id, "Transform")
ecs:hasComponent(id, "Transform")

-- 엔티티 쿼리 (pivot 최적화 + componentIndex 캐시)
local entities = ecs:queryEntities({"Transform", "Velocity"})

-- 통계
local stats = ecs:getStats()
-- stats.entityCount, stats.componentCounts, stats.recycledCount
```

### 4.2. 시스템 베이스 클래스 (system.lua)
```lua
local System = require("01_core.system")

local MySystem = System.new("MySystem", {"Transform", "Velocity"}, function(ecs, entities, dt)
    for _, entityId in ipairs(entities) do
        local t = ecs:getComponent(entityId, "Transform")
        local v = ecs:getComponent(entityId, "Velocity")
        -- 로직 처리...
    end
end)

-- 성능 통계
local stats = MySystem:getStats()  -- { name, updateTime, entityCount }
```

### 4.3. ECS 매니저 (ecsManager.lua)
```lua
local ecsManager = require("01_core.ecsManager")

ecsManager.init()                            -- 월드 생성 + 기본 시스템 등록
ecsManager.update(dt)                        -- 로직 시스템만 실행 (렌더 제외)
ecsManager.draw()                            -- 렌더 시스템만 실행
ecsManager.getWorld()                        -- ECS 월드 참조 반환

-- 엔티티 생성 (EntityFactory 위임)
local playerId = ecsManager.createPlayer(x, y)
local enemyId = ecsManager.createEnemy(x, y, "basic")

-- 통계
local stats = ecsManager.getStats()          -- world + systems 통계
```

### 4.4. 컴포넌트 패턴 (03_game/components/)
```lua
-- 모든 컴포넌트 파일은 같은 패턴:
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

### 4.5. Player 파사드 (03_game/entities/player.lua)
```lua
local player = require("03_game.entities.player")

player.bind(ecs, entityId)         -- ECS 엔티티에 파사드 바인딩
player.init()                      -- 초기화 (디버그 정보 등록 등)
player.update(dt)                  -- 월드 인터랙션 (존 감지, 파워업)
player.getPosition()               -- x, y 반환
player.getCameraTarget()           -- 카메라 추적용 좌표
player.getStats()                  -- 디버그용 통계
player.reset()                     -- 상태 초기화
-- player.draw()는 비어있음 (PlayerRenderSystem이 처리)
```

---

## 5. 카메라 매니저 (02_renderer/cameraManager.lua)

```lua
local cameraManager = require("02_renderer.cameraManager")

-- 초기화 (카메라 2개 생성: game + debug)
cameraManager.init(orthographicSize)

-- 모드 전환 (F5)
cameraManager.toggle()             -- "game" ↔ "debug"
cameraManager.getMode()            -- 현재 모드 문자열

-- 업데이트
cameraManager.update(dt, targetX, targetY)  -- game 모드: 타겟 추적

-- 디버그 모드 조작
cameraManager.debugMove(dx, dy)    -- 마우스 드래그
cameraManager.wheelmoved(x, y)     -- 마우스 휠 줌

-- 렌더링 (카메라 변환 적용 후 콜백 실행)
cameraManager.draw(function()
    -- 월드 좌표계에서 그리기
end)

-- 활성 카메라 참조
local cam = cameraManager.getActive()
```

---

## 6. 디버그 도구

### 6.1. Debug Watch Panel (00_common/debug.lua)
```lua
local debug = require("00_common.debug")

-- watch 항목 등록 (문자열 반환 함수)
debug.add("FPS", function() return tostring(love.timer.getFPS()) end)
debug.add("Player", function() return string.format("(%.1f, %.1f)", x, y) end)
debug.remove("FPS")

-- 토글 (F1)
debug.toggleConsole()
debug.isConsoleVisible()

-- 렌더링 (love.draw에서 호출)
debug.draw(x, y, font)  -- x, y = 화면 위치, font = 폰트 객체
```

### 6.2. Grid Debug Draw (00_common/gridDebugDraw.lua)
```lua
local gridDebugDraw = require("00_common.gridDebugDraw")

gridDebugDraw.toggle()    -- F4 토글
gridDebugDraw.draw()      -- love.draw에서 호출
```

---

## 7. 전역 유틸리티 (00_common/global.lua)

```lua
local global = require("00_common.global")
global.init()  -- love.load에서 최우선 호출

-- 로깅 (어디서나 사용 가능)
log("message")          -- logger.info 단축
logInfo("message")      -- logger.info
logDebug("message")     -- logger.debug
logWarn("message")      -- logger.warn
logError("message")     -- logger.error

-- 수학
clamp(value, 0, 100)           -- 범위 제한
lerp(a, b, 0.5)               -- 선형 보간
distance(x1, y1, x2, y2)      -- 두 점 사이 거리
normalize(x, y)                -- 정규화 벡터 반환 (nx, ny)

-- 색상 (0-255 범위)
setColor(r, g, b, a)           -- love.graphics.setColor 래퍼
resetColor()                   -- 흰색으로 복원
```

---

## 8. 실제 게임 루프 예제

```lua
local global = require("00_common.global")
local logger = require("00_common.logger")
local debug = require("00_common.debug")
local world = require("01_core.world")
local ecsManager = require("01_core.ecsManager")
local cameraManager = require("02_renderer.cameraManager")
local uiManager = require("04_ui.uiManager")
local player = require("03_game.entities.player")

local fonts

function love.load()
    global.init()
    logger.init()
    
    fonts = {
        small = love.graphics.newFont(12),
        medium = love.graphics.newFont(16),
    }
    
    ecsManager.init()
    world.init()
    cameraManager.init(5)  -- orthographicSize = 5
    
    local playerId = ecsManager.createPlayer(0, -100)
    player.bind(ecsManager.getWorld(), playerId)
    player.init()
    
    uiManager.init()
end

function love.update(dt)
    ecsManager.update(dt)   -- 로직 시스템 실행
    player.update(dt)       -- 월드 인터랙션
    
    local tx, ty = player.getCameraTarget()
    cameraManager.update(dt, tx, ty)
    
    uiManager.update(dt)
end

function love.draw()
    cameraManager.draw(function()
        ecsManager.draw()       -- 렌더 시스템 실행
        world.draw(cameraManager.getActive())
    end)
    
    uiManager.draw()
    debug.draw(10, 10, fonts.small)
    logger.drawConsole(fonts.small)
end
```

---

## 9. 수학 라이브러리 (00_common/math/)

### 9.1. 벡터 연산 (vector.lua)
```lua
local Vector = require("00_common.math.vector")

local v1 = Vector(3, 4)
local v2 = Vector(1, 2)

local v3 = v1 + v2             -- 덧셈
local v4 = v1 * 2              -- 스칼라 곱
local length = v1:len()        -- 길이 (5.0)
local normalized = v1:normalized()
local dot = v1:dot(v2)         -- 내적
local rotated = v1:rotated(math.pi/2)
```

### 9.2. 행렬 연산 (matrix.lua)
```lua
local Matrix = require("00_common.math.matrix")

local translate = Matrix.translate(10, 5)
local rotate = Matrix.rotate(math.pi/4)
local combined = translate * rotate
```

---

## 10. 성능 고려사항

### 7.1. 픽셀 정확도 
```lua
-- ✅ 권장: 카메라 정보를 전달하여 정확한 픽셀 크기
world.drawGrid(gridSize, camera)

-- ❌ 비권장: 하드코딩된 크기 (줌에 따라 크기 변함)
love.graphics.setLineWidth(2)  -- 픽셀이 아닌 월드 좌표 기준
```

### 7.2. 좌표 변환 최적화
```lua
-- 프레임당 한 번만 계산하여 캐시
local pixelToWorld = camera:getUnitsPerPixel()

-- 여러 UI 요소에 같은 스케일 적용
local textScale = pixelToWorld * 16
-- 여러 텍스트에 textScale 사용...
```

이 API 가이드는 현재 구현된 시스템의 실제 사용법을 정리한 것입니다. (2026-04-17 업데이트)