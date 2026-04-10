# Vector Swarm 아키텍처 현황 (단순화된 접근법)

## 현재 구현 상태

현재는 복잡한 ECS 시스템 대신 **단순하고 직접적인 아키텍처**로 구현되어 있습니다.
핵심은 Unity 스타일 카메라 시스템과 깔끔한 모듈 구조에 있습니다.

## 1. 실제 구조

```
src/
  00_common/         # 공통 유틸리티 모듈
    debug.lua          # 디버그 시스템
    global.lua         # 전역 유틸리티 함수들  
    grid_debug_draw.lua # 스크린 그리드 디버그
    kutil.lua          # 기타 유틸리티
    logger.lua         # 로깅 시스템
    math/              # 수학 라이브러리
      vector.lua       # 2D 벡터 연산
      matrix.lua       # 행렬 연산
  01_core/             # 핵심 게임 시스템
    world.lua          # 월드 좌표계와 그리드 시스템
  02_renderer/         # 렌더링 시스템
    camera.lua         # Unity 스타일 orthographic 카메라
  main.lua             # 메인 진입점
  conf.lua             # LÖVE2D 설정
```

## 2. 핵심 설계 원칙

### 2.1. Lua 표준 명명 규칙
- **모듈명**: camelCase (debug, logger, camera, world)
- **함수명**: camelCase 
- **상수**: UPPER_CASE
- PascalCase는 사용하지 않음

### 2.2. Unity 스타일 카메라 시스템
- **Orthographic Size**: 카메라가 보는 월드의 절반 높이
- **Viewport Control**: 카메라가 화면의 어느 위치를 중심으로 할지 제어
- **픽셀 정확도**: UI 요소들이 줌과 무관하게 정확한 픽셀 크기 유지

### 2.3. 수학적 좌표계
- **월드 좌표**: (0,0) 중심, Y+ 위방향
- **스크린 변환**: 카메라가 자동 처리
- **픽셀-월드 변환**: 카메라의 orthographic size 기반 정확 계산

## 3. 주요 시스템 설명

### 3.1. 카메라 시스템 (02_renderer/camera.lua)
```lua
-- Unity 스타일 생성
local camera = camera.new(x, y, orthographicSize, rotation, viewportX, viewportY)

-- 주요 기능
camera:setOrthographicSize(5)  -- 뷰 크기 조정
camera:setViewport(x, y)       -- 화면 중심 위치 설정
camera:getPixelsPerUnit()      -- 픽셀당 월드 유닛
camera:getUnitsPerPixel()      -- 월드 유닛당 픽셀

-- 렌더링
camera:draw(function()
    -- 월드 좌표계에서 그리기
end)
```

### 3.2. 월드 시스템 (01_core/world.lua)
```lua
-- 간단한 좌표계와 그리드 표시
world.init()                    -- 초기화
world.drawGrid(gridSize, camera) -- 픽셀 기준 정확한 그리드
world.getSize()                 -- 월드 크기
world.getBounds()               -- 월드 경계
```

### 3.3. 로깅 시스템 (00_common/logger.lua) 
```lua
-- 표준 로깅
logger.info("메시지")
logger.error("오류")
logger.toggleConsole()  -- F1 키로 인게임 콘솔

-- 전역 단축 함수 (global.lua에서 제공)
log("간단한 로깅")
```

### 3.4. 전역 유틸리티 (00_common/global.lua)
```lua
global.init()  -- love.load에서 최우선 호출

-- 제공되는 전역 함수들:
log()          -- logger.info 단축
clamp()        -- 수학적 클램핑
lerp()         -- 선형 보간
distance()     -- 거리 계산
randomColor()  -- 랜덤 색상
```

## 4. 현재 구현의 장점

### 4.1. 단순성
- 복잡한 ECS 없이 직접적인 시스템 설계
- 이해하기 쉽고 수정하기 용이한 구조
- 빠른 프로토타이핑 가능

### 4.2. Unity 호환성 
- Unity 2D 카메라와 동일한 orthographic size 개념
- Unity 개발자에게 친숙한 API
- 3D 게임 엔진 경험 활용 가능

### 4.3. 픽셀 정확도
- 줌과 무관한 일정한 UI 크기
- 깔끔한 픽셀 아트 스타일 지원
- 다양한 해상도 대응

## 5. 향후 확장 방향

### 5.1. 게임 객체 시스템
- 단순한 table 기반 엔티티
- 상속 대신 composition 패턴
- 필요시 간단한 컴포넌트 시스템 추가

### 5.2. 렌더링 최적화
- 배치(batch) 렌더링 시스템
- 간단한 오브젝트 풀링
- 화면 밖 컬링

### 5.3. 게임플레이 시스템
- 입력 시스템 (터치 우선/키보드 호환)
- 충돌 검사 시스템
- 탄막 패턴 시스템

---

## ECS 확장 고려사항 (미래)

현재는 단순한 구조이지만, 게임이 복잡해질 경우를 대비한 ECS 확장 계획:

* 03_game/components/Exp.lua      # 경험치 데이터
* 03_game/components/Level.lua    # 레벨/성장 단계  
* 03_game/components/Stat.lua     # 공격력, 체력 등 기본 능력치
* 03_game/systems/LevelUpSystem.lua # 경험치 획득 및 레벨업 처리

이때도 현재의 단순한 구조를 기반으로 점진적 확장이 가능합니다.

---

## 6. 모바일 포팅 고려사항

### 6.1. LÖVE2D 모바일 지원
현재 구조는 LÖVE2D의 멀티플랫폼 특성을 활용하여 최소한의 변경으로 모바일 포팅이 가능합니다:

- **Android**: love-android-sdl2 사용
- **iOS**: love-ios-source 사용  
- **입력 추상화**: 현재 키보드 입력을 터치로 쉽게 대체 가능

### 6.2. 터치 인터페이스 설계
```lua
-- 현재 키보드 입력:
love.keypressed() → camera:move()

-- 모바일 터치 입력 (계획):
love.touchmoved() → camera:move()
love.touchpressed() → UI interaction
```

### 6.3. 성능 최적화 전략
- **현재 장점**: 단순한 구조로 모바일 성능에 유리
- **픽셀 퍼펙트**: 다양한 모바일 해상도에 대응
- **배치 렌더링**: 필요시 쉽게 추가 가능한 구조

---

## 7. 실제 사용 예시

### main.lua의 현재 구조:
```lua
-- 전역 유틸리티 최우선 초기화
local global = require("00_common.global")

-- 주요 시스템들
local logger = require("00_common.logger")
local world = require("01_core.world")
local camera = require("02_renderer.camera")

local mainCamera

function love.load()
    global.init()  -- 전역 함수들 등록
    logger.init()
    
    -- Unity 스타일 카메라 생성
    mainCamera = camera.new(0, 0, 5)  -- orthographicSize = 5
    world.init()
end

function love.draw()
    mainCamera:draw(function()
        -- 월드 좌표계에서 그리기
        world.drawGrid(2, mainCamera)
    end)
    
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