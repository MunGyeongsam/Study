# 🗺️ World Size Options Analysis
> **Vector Swarm 월드 크기 옵션별 비교 분석**

---

## 현재 설계 vs 큰 월드 옵션 비교

### 📏 현재 설계 (화면 = 월드)
```lua
-- 현재 설계 (WORLD_COORDINATES_DESIGN.md 참조)
SCREEN_SIZE = {width = 432, height = 960}  -- 픽셀
WORLD_SIZE = {width = 10, height = 20}     -- 월드 유닛
PLAYER_AREA = {x = 0.5~9.5, y = 1.0~19.0} -- 플레이어 이동 가능 영역

-- 특징: 월드 = 화면 크기, 카메라 고정
```

**장점:**
- ✅ 터치 조작 최적화 (전체 화면이 플레이어 조작 영역)
- ✅ 성능 우수 (렌더링할 객체 수 제한적)
- ✅ 전통적 탄막 게임 스타일
- ✅ UI 레이아웃 단순

**단점:**
- ❌ 로그라이크 탐험 요소 제한
- ❌ 스테이지 다양성 부족
- ❌ 플레이어 전략적 이동 제한

---

## 🌍 옵션 1: 중형 월드 (화면의 2-3배)

### 월드 크기 설정
```lua
-- 중형 월드 설계
WORLD_SIZE = {width = 20, height = 40}     -- 화면의 2배 크기
SCREEN_VIEW = {width = 10, height = 20}    -- 화면에 보이는 영역
PLAYER_AREA = {
    left = 1, right = 19,                  -- 가로 이동 범위 확장
    top = 2, bottom = 38                   -- 세로 이동 범위 확장
}

-- 카메라 추적 설정
CAMERA_FOLLOW = {
    player_centered = true,                -- 플레이어 중심 추적
    smooth_factor = 0.1,                   -- 부드러운 추적 (0.1초 딜레이)
    dead_zone = {width = 3, height = 5},   -- 카메라 움직이지 않는 영역
    bounds = {                             -- 카메라 이동 제한
        left = 5, right = 15,              
        top = 10, bottom = 30
    }
}
```

### 카메라 시스템
```lua
-- 스무스 카메라 추적
function updateCamera(playerX, playerY, dt)
    local targetX = playerX
    local targetY = playerY
    
    -- Dead Zone 체크 (플레이어가 중앙 영역에 있으면 카메라 고정)
    local deadZoneLeft = CAMERA.x - CAMERA_FOLLOW.dead_zone.width / 2
    local deadZoneRight = CAMERA.x + CAMERA_FOLLOW.dead_zone.width / 2
    local deadZoneTop = CAMERA.y - CAMERA_FOLLOW.dead_zone.height / 2
    local deadZoneBottom = CAMERA.y + CAMERA_FOLLOW.dead_zone.height / 2
    
    if playerX >= deadZoneLeft and playerX <= deadZoneRight then
        targetX = CAMERA.x  -- Dead Zone 내부면 X축 카메라 고정
    end
    if playerY >= deadZoneTop and playerY <= deadZoneBottom then
        targetY = CAMERA.y  -- Dead Zone 내부면 Y축 카메라 고정
    end
    
    -- 부드러운 추적
    CAMERA.x = CAMERA.x + (targetX - CAMERA.x) * CAMERA_FOLLOW.smooth_factor * dt * 60
    CAMERA.y = CAMERA.y + (targetY - CAMERA.y) * CAMERA_FOLLOW.smooth_factor * dt * 60
    
    -- 카메라 경계 제한
    CAMERA.x = math.max(CAMERA_FOLLOW.bounds.left, 
               math.min(CAMERA_FOLLOW.bounds.right, CAMERA.x))
    CAMERA.y = math.max(CAMERA_FOLLOW.bounds.top, 
               math.min(CAMERA_FOLLOW.bounds.bottom, CAMERA.y))
end
```

**장점:**
- ✅ 로그라이크 탐험 요소 추가
- ✅ 전략적 포지셔닝 가능
- ✅ 스테이지 디자인 다양성
- ✅ 여전히 모바일 친화적

**단점:**
- ⚠️ 카메라 시스템 복잡성
- ⚠️ 터치 조작 시 방향감각 혼동 가능
- ⚠️ 성능 최적화 필요 (컬링 시스템)

---

## 🌎 옵션 2: 대형 월드 (화면의 4-5배)

### 월드 크기 설정
```lua
-- 대형 월드 설계
WORLD_SIZE = {width = 40, height = 80}     -- 화면의 4배 크기
SECTOR_SIZE = {width = 10, height = 20}    -- 섹터 단위 관리
TOTAL_SECTORS = {x = 4, y = 4}             -- 4x4 섹터 그리드

-- 섹터별 특성
SECTOR_TYPES = {
    safe_zone = {enemy_spawn = 0.1},       -- 안전 지역 (적 적음)
    combat_zone = {enemy_spawn = 1.0},     -- 전투 지역 (적 많음)
    boss_zone = {enemy_spawn = 0.0, boss = true}, -- 보스 지역
    corridor = {width = 5, enemy_spawn = 0.5}     -- 좁은 통로
}
```

### 미니맵 시스템
```lua
-- 모바일용 미니맵 (우상단)
MINIMAP = {
    size = {width = 80, height = 80},      -- 80x80 픽셀
    position = {x = 340, y = 10},          -- 우상단 배치
    scale = 0.1,                           -- 월드 대비 축소 비율
    show_player = true,
    show_enemies = true,
    show_bounds = true
}

function drawMinimap()
    love.graphics.setColor(0.2, 0.2, 0.2, 0.8)  -- 반투명 배경
    love.graphics.rectangle("fill", MINIMAP.position.x, MINIMAP.position.y,
                           MINIMAP.size.width, MINIMAP.size.height)
    
    -- 플레이어 위치 표시
    local playerMapX = MINIMAP.position.x + (PLAYER.x / WORLD_SIZE.width) * MINIMAP.size.width
    local playerMapY = MINIMAP.position.y + (PLAYER.y / WORLD_SIZE.height) * MINIMAP.size.height
    
    love.graphics.setColor(0, 1, 0, 1)  -- 초록색 플레이어
    love.graphics.circle("fill", playerMapX, playerMapY, 3)
end
```

**장점:**
- ✅ 완전한 오픈 월드 탐험
- ✅ 복잡한 스테이지 구조 가능
- ✅ 긴 플레이 타임 보장
- ✅ 로그라이크 요소 극대화

**단점:**
- ❌ 모바일 성능 부담 증가
- ❌ 터치 네비게이션 복잡
- ❌ 개발 복잡도 높음
- ❌ 메모리 사용량 증가

---

## 📊 성능 비교 분석

### 렌더링 성능 (60 FPS 기준)
```lua
-- 화면 = 월드 (현재)
MAX_VISIBLE_ENTITIES = 1000        -- 화면 전체가 렌더링 대상
CULLING_OVERHEAD = 0               -- 컬링 필요 없음
MEMORY_USAGE = "Low"               -- 최소 메모리

-- 중형 월드 (2-3배)
MAX_VISIBLE_ENTITIES = 1000        -- 동일 (화면에 보이는 것만)
CULLING_OVERHEAD = 100             -- 화면 밖 객체 컬링 필요
MEMORY_USAGE = "Medium"            -- 전체 월드 데이터 보관

-- 대형 월드 (4-5배)
MAX_VISIBLE_ENTITIES = 1000        -- 동일
CULLING_OVERHEAD = 500             -- 섹터별 관리 + 컬링
MEMORY_USAGE = "High"              -- 섹터별 LOD 시스템 필요
```

### 터치 조작 편의성
| 월드 크기 | 조작 복잡도 | 방향 감각 | 추천 UI |
|----------|------------|----------|---------|
| **화면 = 월드** | ⭐ 매우 단순 | ⭐⭐⭐ 명확 | 없음 필요 |
| **중형 월드** | ⭐⭐ 단순 | ⭐⭐ 보통 | Dead Zone 표시 |
| **대형 월드** | ⭐⭐⭐ 복잡 | ⭐ 혼란 | 미니맵/나침반 필수 |

---

## 🎯 권장사항 (Recommendation)

### MVP 단계: 현재 설계 유지
```lua
-- 1달 개발 기간 고려하여 단순한 설계로 시작
WORLD_SIZE = {width = 10, height = 20}  -- 화면 = 월드
FOCUS = "탄막 패턴과 터치 조작 완성도"
```

### 확장 단계: 중형 월드로 업그레이드
```lua
-- MVP 검증 후 로그라이크 요소 추가
WORLD_SIZE = {width = 20, height = 40}  -- 2배 확장
FEATURES = ["카메라 추적", "Dead Zone", "섹터 기반 난이도"]
```

### 최종 단계: 필요 시 대형 월드
```lua
-- 게임의 재미가 검증된 후에만 고려
WORLD_SIZE = {width = 40, height = 80}  -- 4배 확장
FEATURES = ["미니맵", "섹터 관리", "LOD 시스템"]
```

---

## ⚡ 즉시 구현 가능한 테스트 코드

### 중형 월드 프로토타입
```lua
-- main.lua에 추가 가능한 간단한 큰 월드 테스트
function love.load()
    -- 기존 코드...
    
    -- 월드 크기 테스트 모드
    WORLD_TEST_MODE = true
    if WORLD_TEST_MODE then
        WORLD_WIDTH = 20        -- 화면의 2배
        WORLD_HEIGHT = 40
        CAMERA_X = 10           -- 카메라 중심 위치
        CAMERA_Y = 20
    else
        WORLD_WIDTH = 10        -- 기본 크기  
        WORLD_HEIGHT = 20
        CAMERA_X = 5
        CAMERA_Y = 10
    end
end

function love.update(dt)
    -- 기존 코드...
    
    if WORLD_TEST_MODE then
        -- 플레이어 위치에 따라 카메라 이동
        CAMERA_X = playerPosition.x
        CAMERA_Y = playerPosition.y
        
        -- 월드 경계 제한
        playerPosition.x = math.max(1, math.min(WORLD_WIDTH-1, playerPosition.x))
        playerPosition.y = math.max(1, math.min(WORLD_HEIGHT-1, playerPosition.y))
    end
end

function love.draw()
    if WORLD_TEST_MODE then
        -- 카메라 변환 적용
        love.graphics.push()
        love.graphics.translate(SCREEN_WIDTH/2 - CAMERA_X*20, 
                               SCREEN_HEIGHT/2 - CAMERA_Y*20)
        
        -- 월드 그리드 그리기
        love.graphics.setColor(0.3, 0.3, 0.3, 1)
        for x = 0, WORLD_WIDTH do
            love.graphics.line(x*20, 0, x*20, WORLD_HEIGHT*20)
        end
        for y = 0, WORLD_HEIGHT do
            love.graphics.line(0, y*20, WORLD_WIDTH*20, y*20)
        end
        
        love.graphics.pop()
    end
    
    -- 기존 렌더링 코드...
end
```

---

## 🤔 질문과 결정사항

1. **개발 우선순위**: MVP에서는 탄막 패턴 완성도에 집중할까, 월드 탐험 요소에 집중할까?

2. **로그라이크 요소**: "Zero Art Roguelite"에서 로그라이크 요소가 얼마나 중요한가?

3. **모바일 최적화**: 큰 월드로 인한 성능 저하 vs 게임 플레이 다양성 중 무엇이 우선인가?

**추천**: MVP는 현재 설계(화면=월드)로 시작하고, 게임이 재미있다고 검증되면 중형 월드(2배 크기)로 확장하는 것이 효율적일 것 같습니다.

어떤 옵션이 가장 매력적으로 느껴지시나요?