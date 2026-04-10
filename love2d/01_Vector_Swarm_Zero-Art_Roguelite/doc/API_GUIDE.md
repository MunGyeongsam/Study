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

-- 로깅
logger.info("정보 메시지")
logger.warn("경고 메시지") 
logger.error("오류 메시지")
logger.debug("디버그 메시지")

-- 포매팅 지원
logger.info("플레이어 위치: (%.1f, %.1f)", playerX, playerY)
```

### 3.2. 인게임 콘솔
```lua
-- 콘솔 토글 (F1 키 등에 바인딩)
logger.toggleConsole()

-- 콘솔 렌더링 (love.draw에서 호출)
function love.draw()
    -- 게임 내용 그리기...
    
    -- 콘솔은 맨 마지막에
    logger.drawConsole(fonts.small)  -- fonts.small = 작은 폰트
end
```

---

## 4. 전역 유틸리티 (00_common/global.lua)

### 4.1. 초기화 및 전역 함수들
```lua
local global = require("00_common.global")

-- love.load에서 최우선 초기화
function love.load()
    global.init()  -- 전역 함수들 등록
    -- 다른 초기화들...
end

-- 이제 어디서나 사용 가능한 전역 함수들:
log("간단한 로깅")                     -- logger.info의 단축

-- 수학 함수들
local result = clamp(value, 0, 100)    -- 값을 0-100 사이로 제한
local interpolated = lerp(a, b, 0.5)   -- a와 b 사이 50% 지점
local dist = distance(x1, y1, x2, y2)  -- 두 점 사이 거리

-- 색상 함수들  
local r, g, b = randomColor()          -- 랜덤 RGB 색상
local r, g, b = hsl(hue, sat, light)   -- HSL → RGB 변환
```

---

## 5. 수학 라이브러리 (00_common/math/)

### 5.1. 벡터 연산 (vector.lua)
```lua
local Vector = require("00_common.math.vector")

-- 벡터 생성
local v1 = Vector(3, 4)        -- 또는 Vector.new(3, 4)
local v2 = Vector(1, 2)

-- 기본 연산
local v3 = v1 + v2             -- 덧셈  
local v4 = v1 - v2             -- 뺄셈
local v5 = v1 * 2              -- 스칼라 곱
local length = v1:len()        -- 길이 (5.0)
local normalized = v1:normalized()  -- 정규화된 벡터

-- 벡터 함수들
local dot = v1:dot(v2)         -- 내적
local angle = v1:angleTo(v2)   -- 두 벡터 사이 각도
local rotated = v1:rotated(math.pi/2)  -- 90도 회전
```

### 5.2. 행렬 연산 (matrix.lua)
```lua
local Matrix = require("00_common.math.matrix")

-- 변환 행렬 생성
local translate = Matrix.translate(10, 5)    -- 이동 행렬
local rotate = Matrix.rotate(math.pi/4)      -- 회전 행렬  
local scale = Matrix.scale(2, 1.5)           -- 스케일 행렬

-- 행렬 연산
local combined = translate * rotate * scale   -- 조합 변환
local transformed = combined * Vector(1, 1)   -- 벡터 변환
```

---

## 6. 실제 게임 루프 예제

```lua
-- main.lua 전체 구조 예제
local global = require("00_common.global")
local logger = require("00_common.logger") 
local world = require("01_core.world")
local camera = require("02_renderer.camera")

local mainCamera

function love.load()
    -- 1. 전역 유틸리티 최우선
    global.init()
    
    -- 2. 로깅 초기화
    logger.init()
    
    -- 3. 카메라 설정
    mainCamera = camera.new(0, 0, 5)
    
    -- 4. 기타 시스템들
    world.init()
    
    log("All systems initialized")
end

function love.update(dt)
    -- 게임 로직 업데이트
end

function love.draw()
    -- 월드 렌더링 
    mainCamera:draw(function()
        world.drawGrid(2, mainCamera)
        -- 게임 오브젝트들 그리기...
    end)
    
    -- UI 렌더링 (스크린 좌표)
    love.graphics.print("FPS: " .. love.timer.getFPS(), 10, 10)
    logger.drawConsole()
end

function love.keypressed(key)
    if key == "f1" then
        logger.toggleConsole()
    elseif key == "=" then
        -- 줌인
        local newSize = mainCamera:getOrthographicSize() * 0.8
        mainCamera:setOrthographicSize(newSize)
        log("Zoom: %.2f", newSize)
    end
end
```

---

## 7. 성능 고려사항

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

이 API 가이드는 현재 구현된 시스템의 실제 사용법을 정리한 것으로, 코드베이스와 정확히 일치합니다.