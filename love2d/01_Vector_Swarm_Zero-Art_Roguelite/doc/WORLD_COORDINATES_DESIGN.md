# 🌐 World Coordinates & Stage Design
> **Vector Swarm 월드 좌표계 및 스테이지 크기 설계**

---

## 1. 좌표계 단위 정의 (Coordinate System Units)

### 1.1. 기준 해상도 및 다양한 디바이스 지원
```lua
-- 개발 기준 해상도 (갤럭시 노트20 기준)
REFERENCE_WIDTH = 432     -- 개발용 기준 너비
REFERENCE_HEIGHT = 960    -- 개발용 기준 높이
TARGET_ASPECT_RATIO = 9/20 -- 세로 모드 비율

-- 실제 모바일 디바이스 해상도 예시
TYPICAL_RESOLUTIONS = {
    {name = "Galaxy Note 20", width = 1080, height = 2400, dpi = 393},
    {name = "iPhone 13 Pro", width = 1170, height = 2532, dpi = 460},
    {name = "Pixel 6", width = 1080, height = 2400, dpi = 411},
    {name = "iPad mini", width = 1488, height = 2266, dpi = 326}
}

-- 화면 영역 분할 (비율 기반)
UI_TOP_RATIO = 0.05       -- 상단 UI 영역 (5%)
UI_BOTTOM_RATIO = 0.10    -- 하단 UI 영역 (10%)
PLAY_AREA_RATIO = 0.85    -- 실제 게임 플레이 영역 (85%)
```

### 1.2. 월드 단위 (World Units)
**논리적 게임 월드 좌표계** - 완전 해상도 독립적 설계
```lua
-- 월드 좌표계 (논리적 단위 - 모든 디바이스에서 동일)
WORLD_WIDTH = 10.0      -- 월드 가로 10 유닛
WORLD_HEIGHT = 20.0     -- 월드 세로 20 유닛 (9:20 비율 유지)

-- 월드 중심점
WORLD_CENTER_X = 5.0    -- 가로 중심
WORLD_CENTER_Y = 10.0   -- 세로 중심

-- 동적 월드-스크린 변환 비율 (실행시 계산)
function calculateWorldToScreenRatio()
    local screenWidth, screenHeight = love.graphics.getDimensions()
    local playAreaHeight = screenHeight * PLAY_AREA_RATIO
    
    return {
        x = screenWidth / WORLD_WIDTH,
        y = playAreaHeight / WORLD_HEIGHT
    }
end
```

### 1.3. 해상도 독립적 좌표 변환 함수
```lua
-- 현재 화면 정보 캐싱 (성능 최적화)
local screenInfo = {
    width = 0,
    height = 0,
    playAreaHeight = 0,
    playAreaYStart = 0,
    worldToScreenX = 1,
    worldToScreenY = 1,
    needsUpdate = true
}

-- 화면 크기 변경 시 호출 (해상도/회전 변경)
function updateScreenInfo()
    screenInfo.width, screenInfo.height = love.graphics.getDimensions()
    screenInfo.playAreaHeight = screenInfo.height * PLAY_AREA_RATIO
    screenInfo.playAreaYStart = screenInfo.height * UI_TOP_RATIO
    screenInfo.worldToScreenX = screenInfo.width / WORLD_WIDTH
    screenInfo.worldToScreenY = screenInfo.playAreaHeight / WORLD_HEIGHT
    screenInfo.needsUpdate = false
end

-- 월드 좌표 → 스크린 좌표
function worldToScreen(worldX, worldY)
    if screenInfo.needsUpdate then updateScreenInfo() end
    
    local screenX = worldX * screenInfo.worldToScreenX
    local screenY = worldY * screenInfo.worldToScreenY + screenInfo.playAreaYStart
    return screenX, screenY
end

-- 스크린 좌표 → 월드 좌표
function screenToWorld(screenX, screenY)
    if screenInfo.needsUpdate then updateScreenInfo() end
    
    local worldX = screenX / screenInfo.worldToScreenX
    local worldY = (screenY - screenInfo.playAreaYStart) / screenInfo.worldToScreenY
    return worldX, worldY
end

-- 터치 입력을 월드 좌표로 변환
function touchToWorld(touchX, touchY)
    return screenToWorld(touchX, touchY)
end

-- DPI 고려 터치 크기 (모바일 최적화)
function getDPIScale()
    local width, height = love.graphics.getDimensions()
    local referenceDPI = 400  -- 기준 DPI
    local estimatedDPI = math.sqrt(width^2 + height^2) / 6  -- 6인치 가정
    return estimatedDPI / referenceDPI
end
```

---

## 2. 스테이지 월드 크기 설계 (Stage World Size)

### 2.1. 재미 중심 월드 크기 (Fun-First World Design)

**핵심 철학**: 개발 복잡도 < 플레이어 재미

```lua
-- 🎯 재미를 위한 확장된 월드 크기
WORLD_SIZE = {
    width = 30.0,    -- 화면의 3배 가로 (탐험 공간 확보)
    height = 60.0    -- 화면의 3배 세로 (긴 여정감)
}

-- 🗺️ 구역별 재미 요소 설계
EXPLORATION_ZONES = {
    -- 안전 지대 (회복/준비 공간)
    safe_zones = {
        count = 5,
        size = {width = 8, height = 8},
        features = {"healing", "power_ups", "strategy_planning"}
    },
    
    -- 전투 지역 (다양한 탄막 패턴)  
    combat_areas = {
        count = 12,
        types = {"corridor", "arena", "maze", "trap_room"},
        difficulty_scaling = "progressive"
    },
    
    -- 보스 공간 (임팩트 있는 전투)
    boss_chambers = {
        count = 3,
        size = {width = 15, height = 20},
        special_mechanics = true
    },
    
    -- 숨겨진 지역 (발견의 재미)
    secret_areas = {
        count = 4,
        access = "hidden_paths",
        rewards = "unique_power_ups"
    }
}

-- 안전 삭제 영역 (성능 최적화)
CLEANUP_BOUNDS = {
    left = -1.0,     -- 화면 밖 1유닛까지
    right = 11.0,    -- 화면 밖 1유닛까지  
    top = -3.0,      -- 화면 위 3유닛까지
    bottom = 23.0    -- 화면 아래 3유닛까지
}
```

### 2.2. 로그라이크 탐험 시스템 (Roguelike Exploration)

```lua
-- 🎲 절차적 생성 월드 구조
WORLD_GENERATION = {
    -- 메인 경로 (반드시 지나가는 구간)
    main_path = {
        length = 40,           -- 40개 스테이지 연결
        branch_points = 8,     -- 8곳에서 갈래길 등장
        difficulty_curve = "exponential"
    },
    
    -- 선택적 경로 (플레이어 선택권)
    optional_paths = {
        risky_shortcuts = 3,   -- 위험하지만 빠른 길
        safe_detours = 4,      -- 안전하지만 긴 길  
        treasure_routes = 2,   -- 보상이 좋은 숨겨진 길
        boss_rush = 1          -- 연속 보스전 (고난이도)
    }
}

-- 🎯 다양한 스테이지 타입 (플레이 패턴 다양화)
STAGE_TYPES = {
    -- 기본 탄막 스테이지
    bullet_hell = {
        size = {width = 12, height = 15},
        patterns = ["spiral", "grid", "wave", "random"],
        duration = "2-3 minutes"
    },
    
    -- 좁은 복도 (정밀 조작)
    narrow_corridor = {
        size = {width = 4, height = 25}, 
        challenge = "precision_movement",
        reward_multiplier = 1.2
    },
    
    -- 넓은 아레나 (복잡한 패턴)
    open_arena = {
        size = {width = 20, height = 20},
        challenge = "pattern_reading", 
        multi_enemy_spawns = true
    },
    
    -- 미로형 스테이지 (탐험+전투)
    maze_chamber = {
        size = {width = 25, height = 25},
        walls = "procedural_generation",
        hidden_passages = 3,
        treasure_chance = 0.3
    },
    
    -- 수직 점프 (터치 액션)
    vertical_climb = {
        size = {width = 8, height = 40},
        gravity_reverse = true,
        platform_jumping = true,
        unique_mechanics = "anti_gravity_zones"
    },
    
    -- 타임 어택 (긴장감)
    time_attack = {
        size = {width = 15, height = 15},
        time_limit = 60,
        reward = "score_multiplier_x3",
        failure = "alternative_path_unlock"
    }
}
```

---

## 3. 실제 게임 객체 크기 기준 (Game Object Scaling)

### 3.1. 플레이어 중심 스케일링 (Player-Centric Scaling)
```lua
-- 재미를 위한 관대한 크기 설정
PLAYER_SIZE = 0.2           -- 플레이어 반지름 (시인성 우선)
PLAYER_VISUAL_SIZE = 0.25   -- 시각적 크기 (임팩트감)
PLAYER_HITBOX_SIZE = 0.15   -- 실제 충돌 크기 (관대한 판정)

-- 적과 탄환 크기 (플레이어 우위 설계)
BULLET_SMALL = 0.03         -- 작은 탄환 (다수 생성용)
BULLET_MEDIUM = 0.06        -- 중간 탄환 (기본)
BULLET_LARGE = 0.12         -- 큰 탄환 (파괴적)
BULLET_MASSIVE = 0.25       -- 거대 탄환 (보스용)

ENEMY_SMALL = 0.2           -- 작은 적
ENEMY_MEDIUM = 0.4          -- 중간 적  
ENEMY_LARGE = 0.8           -- 큰 적
ENEMY_BOSS = 2.0            -- 보스 (화면의 1/5 크기)

-- 🎮 터치 조작 최적화 (모바일 친화적)
TOUCH_AREA_SCALE = {
    hitbox = 1.5,           -- 터치 히트박스 50% 확대
    visual = 1.0,           -- 시각적 크기 그대로
    interaction = 2.0       -- 상호작용 범위 2배 확대
}

-- 🌟 특수 능력 범위 (재미 요소)
ABILITY_RANGES = {
    dash_distance = 3.0,    -- 대시 거리 (월드 유닛)
    focus_slow_radius = 5.0, -- 포커스 모드 영향 범위
    power_up_effect = 1.5,  -- 파워업 획득 범위
    combo_chain_range = 2.0 -- 콤보 연쇄 범위
}
```

### 4.2. 다양한 해상도/DPI 대응
```lua
-- 해상도 독립적 스케일링
function getAdaptiveScale()
    local width, height = love.graphics.getDimensions()
    
    -- 비율 유지 스케일 계산
    local scaleX = width / REFERENCE_WIDTH
    local scaleY = height / REFERENCE_HEIGHT
    local uniformScale = math.min(scaleX, scaleY)
    
    -- DPI 보정 (고해상도 디스플레이 대응)
    local dpiScale = getDPIScale()
    
    -- 모바일 터치 최적화 스케일
    local touchScale = math.max(0.8, math.min(2.0, dpiScale))
    
    return {
        uniform = uniformScale,      -- UI 레이아웃용
        dpi = dpiScale,             -- DPI 보정
        touch = touchScale,         -- 터치 영역 크기
        visual = uniformScale * 0.9 -- 시각적 요소 (여백 확보)
    }
end

-- 해상도별 최적화 설정
function getDeviceProfile()
    local width, height = love.graphics.getDimensions()
    local diagonal = math.sqrt(width^2 + height^2)
    
    if diagonal < 1500 then
        return {
            name = "small_phone",
            particleQuality = "low",
            maxEntities = 300,
            uiScale = 1.2  -- 터치 영역 확대
        }
    elseif diagonal < 2500 then
        return {
            name = "large_phone", 
            particleQuality = "medium",
            maxEntities = 500,
            uiScale = 1.0
        }
    else
        return {
            name = "tablet",
            particleQuality = "high", 
            maxEntities = 800,
            uiScale = 0.8  -- 터치 영역 축소
        }
    end
end
```

---

## 4. 몰입감 극대화 카메라 시스템 (Immersive Camera System)

### 4.1. 동적 카메라 (Dynamic Camera)
```lua
-- 🎬 영화적 카메라 움직임
CAMERA_SYSTEM = {
    -- 기본 추적 설정
    follow_player = true,
    smooth_factor = 0.08,        -- 부드러운 추적 (영화적 느낌)
    anticipation = 2.0,          -- 플레이어 움직임 예측
    
    -- 상황별 카메라 행동
    combat_zoom = 0.9,           -- 전투시 약간 줌인 (집중감)
    exploration_zoom = 1.1,      -- 탐험시 줌아웃 (시야 확보)
    boss_fight_zoom = 0.8,       -- 보스전 줌인 (긴장감)
    
    -- 드라마틱 효과
    screen_shake = {
        impact_intensity = 0.3,   -- 충돌시 흔들림
        explosion_intensity = 0.5, -- 폭발시 강한 흔들림
        boss_entrance = 1.0       -- 보스 등장시 극적 효과
    },
    
    -- 플레이어 사망시 특별 연출
    death_camera = {
        zoom_out_speed = 2.0,     -- 죽을때 줌아웃
        slow_motion = 0.3,        -- 슬로우모션 효과
        duration = 1.5            -- 연출 지속시간
    }
}

-- 📱 터치 인터페이스 고려 카메라 경계
CAMERA_BOUNDS = {
    -- 유연한 경계 (재미 우선)
    margin_x = 2.0,              -- 좌우 여유 공간
    margin_y = 3.0,              -- 상하 여유 공간
    
    -- 특수 상황 예외
    boss_fight_bounds = "none",   -- 보스전은 경계 무시
    chase_sequence = "extended", -- 추격전은 경계 확장
    exploration = "full_freedom" -- 탐험시 자유로운 이동
}
```

### 4.2. 뷰포트 변환
```lua
-- 카메라 매트릭스 적용
function applyCameraTransform()
    love.graphics.push()
    
    local centerX = SCREEN_WIDTH / 2
    local centerY = (PLAY_AREA_HEIGHT / 2) + PLAY_AREA_Y_START
    
    love.graphics.translate(centerX, centerY)
    love.graphics.scale(CAMERA.zoom, CAMERA.zoom)
    love.graphics.translate(-CAMERA.x * WORLD_TO_SCREEN_X + CAMERA.shake_x, 
                           -CAMERA.y * WORLD_TO_SCREEN_Y + CAMERA.shake_y)
end

function resetCameraTransform()
    love.graphics.pop()
end
```

---

## 5. 터치 입력 영역 관리 (Touch Input Areas)

### 5.1. 터치 영역 정의
```lua
-- 화면 영역별 터치 기능
TOUCH_AREAS = {
    -- 게임 플레이 영역 (드래그 이동)
    gameplay = {
        x = 0, y = PLAY_AREA_Y_START,
        width = SCREEN_WIDTH, 
        height = PLAY_AREA_HEIGHT,
        action = "move_player"
    },
    
    -- 포커스 모드 버튼 (하단 좌측)
    focus_button = {
        x = 20, y = SCREEN_HEIGHT - 80,
        width = 100, height = 60,
        action = "focus_mode"
    },
    
    -- 대쉬 영역 (스와이프 감지)
    dash_area = {
        x = 0, y = PLAY_AREA_Y_START,
        width = SCREEN_WIDTH,
        height = PLAY_AREA_HEIGHT,
        action = "dash_swipe"
    }
}
```

### 5.2. 터치 좌표 보정
```lua
-- 터치 입력을 월드 좌표로 정확히 변환
function processTouchInput(touchX, touchY)
    -- 플레이 영역 내부 터치만 처리
    if touchY >= PLAY_AREA_Y_START and 
       touchY <= (PLAY_AREA_Y_START + PLAY_AREA_HEIGHT) then
        
        local worldX, worldY = touchToWorld(touchX, touchY)
        
        -- 플레이어 이동 제한 적용
        worldX = math.max(PLAYER_BOUNDS.left, 
                 math.min(PLAYER_BOUNDS.right, worldX))
        worldY = math.max(PLAYER_BOUNDS.top, 
                 math.min(PLAYER_BOUNDS.bottom, worldY))
        
        return worldX, worldY
    end
    
    return nil  -- 플레이 영역 밖 터치는 무시
end
```

---

## 6. 플레이어 경험 최적화 고려사항 (Player Experience Optimization)

### 6.1. 재미를 위한 성능 할당
```lua
-- 🎯 재미 우선 객체 수 (성능은 최적화로 해결)
TARGET_ENTITIES = {
    bullets_on_screen = 2000,        -- 화려한 탄막을 위해
    enemies_simultaneous = 50,       -- 박진감 있는 전투
    particles_explosion = 1000,      -- 임팩트 있는 폭발 효과
    power_up_effects = 200,          -- 시각적 만족감
    background_elements = 500        -- 몰입감 있는 배경
}

-- ⚡ 스마트 최적화 전략
OPTIMIZATION_STRATEGY = {
    -- 중요도 기반 LOD (Level of Detail)
    player_vicinity = "max_quality",     -- 플레이어 주변은 최고 품질
    mid_range = "medium_quality",        -- 중간 거리는 중품질
    far_distance = "low_quality",        -- 먼 거리는 저품질
    
    -- 동적 품질 조절
    fps_target = 60,
    quality_scaling = "adaptive",        -- FPS가 떨어지면 자동 품질 조절
    
    -- 재미 보장 최소값
    minimum_bullets = 500,              -- 최소한의 탄막 밀도 보장
    minimum_effects = 100               -- 최소한의 이펙트 보장
}
```

### 6.2. 몰입감을 위한 성능 관리
```lua
-- 🎮 플레이어 경험 우선 컬링
function getEntityImportance(entity, playerPos)
    local distance = getDistance(entity.pos, playerPos)
    local threat_level = entity.threat or 0
    local visual_impact = entity.effect_intensity or 1
    
    -- 재미 요소 가중치
    local importance = 0
    
    if entity.type == "bullet" then
        importance = math.max(0.5, 2.0 - distance * 0.2) -- 탄환은 항상 중요
    elseif entity.type == "enemy" then
        importance = math.max(0.8, 3.0 - distance * 0.3) -- 적은 더 중요
    elseif entity.type == "power_up" then
        importance = 5.0 - distance * 0.1 -- 파워업은 매우 중요
    elseif entity.type == "explosion" then
        importance = 10.0 -- 폭발 효과는 절대 컬링 금지
    end
    
    return importance * threat_level * visual_impact
end

-- 🌟 특별한 순간 성능 보장
SPECIAL_MOMENTS = {
    boss_entrance = {
        guaranteed_effects = true,    -- 모든 이펙트 강제 렌더링
        max_quality = true,          -- 최고 품질 보장
        duration = 3.0               -- 3초간 유지
    },
    
    player_death = {
        slow_motion = true,          -- 슬로우 모션
        particle_boost = 2.0,        -- 파티클 2배
        no_culling = true            -- 컬링 비활성화
    },
    
    power_up_moment = {
        screen_effects = true,       -- 화면 이펙트
        sound_emphasis = true,       -- 사운드 강조
        visual_feedback = "maximum"  -- 최대 시각 피드백
    }
}
```

---

## 7. 개발 로드맵: 재미 우선 구현 (Fun-First Implementation Roadmap)

### Phase 1: 탐험의 재미 (Week 1-2)
- [x] 확장된 월드 크기 시스템 (30x60 월드)
- [ ] 동적 카메라 추적 및 예측 시스템
- [ ] 기본 스테이지 타입 3개 (bullet_hell, corridor, arena)
- [ ] 플레이어 이동의 자유도 확보

### Phase 2: 선택의 재미 (Week 3-4)  
- [ ] 절차적 월드 생성 (메인 경로 + 갈래길)
- [ ] 6가지 스테이지 타입 모두 구현
- [ ] 위험/보상 밸런스 시스템
- [ ] 숨겨진 지역 및 비밀 요소

### Phase 3: 몰입의 재미 (Week 5-6)
- [ ] 영화적 카메라 효과 (줌, 흔들림, 슬로우모션)
- [ ] 보스 등장 연출 및 특별한 순간들
- [ ] 파워업 시각/청각 피드백 강화
- [ ] 플레이어 성취감 연출 시스템

### Phase 4: 지속적인 재미 (Week 7+)
- [ ] 로그라이크 진행 시스템
- [ ] 메타 진행 요소 (영구 업그레이드)
- [ ] 플레이어 선택에 따른 스토리 분기
- [ ] 엔드게임 콘텐츠 및 도전 모드

---

**핵심 개발 원칙**: "성능 문제는 최적화로 해결하자. 재미 없는 게임은 해결책이 없다."

모든 기술적 도전은 플레이어 경험을 위한 투자로, 복잡함보다는 **플레이어가 "와!" 하고 감탄할 순간들**을 만드는 것에 집중합니다.