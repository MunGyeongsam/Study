# Project: Vector Swarm (현재 개발 상황)
> **"벡터 아트 로그라이트 탄막 슈터"**

## 1. 프로젝트 개요
- **장르:** 벡터 아트 로그라이트 탄막 슈터
- **플랫폼:** Mobile-First (Android/iOS) + Desktop - LÖVE2D 기반
- **화면 비율:** 모바일 9:16/9:20 Portrait + 가변 해상도 지원
- **개발 상태:** ECS 코어 구현 완료, 플레이어 ECS 전환 완료 (PC 프로토타입)
- **핵심 목표:** 
    - Unity 스타일 orthographic 카메라 시스템 구현 ✅
    - 픽셀 정확도 UI 시스템 ✅
    - 수학적 좌표계 (Y+ 상향) ✅
    - 모바일 최적화된 아키텍처 ✅

---

## 2. 현재 구현된 기능

### 2.1. Unity 스타일 카메라 시스템 ✅
- **Orthographic Size**: 카메라가 보는 월드의 절반 높이로 줌 제어
- **Viewport 제어**: 카메라가 화면의 어느 위치를 중심으로 할지 설정 가능
- **수학적 좌표계**: (0,0) 중심, Y+ 위쪽 방향
- **픽셀 정확도**: 줌과 무관하게 UI 요소들이 일정한 픽셀 크기 유지
- **CameraManager**: game 모드 (플레이어 추적) + debug 모드 (자유 이동/줌)

### 2.2. ECS (Entity-Component-System) ✅
- **ECS 코어**: 엔티티 생성/제거(ID 재활용), 컴포넌트 CRUD, pivot query 최적화
- **컴포넌트 8종**: Transform, Velocity, Collider, Renderable, LifeSpan, PlayerTag, Input, WorldBound
- **시스템 6종**: Input, Movement, Boundary, LifeSpan, Render, PlayerRender
- **ECS 매니저**: 시스템 등록/실행 순서, update/draw 분리
- **EntityFactory**: createPlayer(), createEnemy()

### 2.3. 플레이어 시스템 ✅
- **ECS 파사드 패턴**: player.bind(ecs, entityId) — ECS 엔티티에 래핑
- **InputSystem**: 키보드/터치 입력 → Velocity 반영
- **BoundarySystem**: 월드 경계 clamping
- **PlayerRenderSystem**: 삼각형 + 방향 표시 렌더링
- **월드 인터랙션**: 존 감지, 파워업, 체크포인트

### 2.4. 디버그/개발 도구 ✅
- **Logger 콘솔**: ` 키 토글, 4레벨 색상 구분 (DEBUG/INFO/WARN/ERROR)
- **Debug watch panel**: F1 토글, 키-값 실시간 모니터링
- **Screen grid**: F4 토글
- **Camera debug**: F5 토글 (마우스 드래그/휠로 자유 이동)

### 2.5. 모바일 UI ✅
- **uiManager**: 터치 입력 우선 소비
- **topHud**: 상단 점수/설정 바
- **bottomControls**: 하단 버튼 컨트롤
- **mobileLayout**: 영역 분할 (play/ui 영역 판별)

## 3. 입력 컨트롤

### 3.1. 현재 구현 (PC 프로토타입)

#### 키 바인딩
| 키 | 기능 |
|----|------|
| `` ` `` | Logger 콘솔 토글 |
| F1 | Debug watch panel 토글 |
| F2 | UI 표시 토글 |
| F3 | UI 디버그 모드 토글 |
| F4 | Screen grid 토글 |
| F5 | Camera 모드 전환 (game ↔ debug) |
| ESC | 게임 종료 |

#### 게임 조작
- **이동**: 터치 드래그 (play 영역) 또는 마우스
- **Debug 카메라**: F5로 전환 후 마우스 드래그/휠

### 3.2. 모바일 입력
- **터치 이동**: play 영역 터치/드래그
- **UI 터치**: uiManager가 터치 우선 소비
- mouse → touch 브릿지: `love.mousepressed` → `love.touchpressed` (PC 프로토타입)

---

## 4. 기술적 아키텍처

### 4.1. 폴더 구조
```
src/
├── main.lua               # LÖVE 콜백 (load/update/draw/input)
├── conf.lua               # LÖVE 설정 (432×960, 9:20 세로)
├── 00_common/             # 유틸리티 — 게임 의존성 없음
│   ├── global.lua           # 전역 함수 (log, clamp, lerp, setColor, …)
│   ├── logger.lua           # 4레벨 로깅 + 인게임 콘솔
│   ├── debug.lua            # 디버그 watch panel
│   ├── gridDebugDraw.lua    # 스크린 그리드 오버레이
│   ├── kutil.lua            # 기타 유틸리티
│   └── math/                # 벡터/행렬 라이브러리
├── 01_core/               # 엔진 레이어
│   ├── world.lua            # 월드 경계, 존, 재미 요소
│   ├── ecs.lua              # ECS 코어
│   ├── system.lua           # 시스템 베이스 클래스
│   └── ecsManager.lua       # ECS 오케스트레이터
├── 02_renderer/           # 카메라 & 렌더링
│   ├── camera.lua           # Unity 스타일 orthographic 카메라
│   └── cameraManager.lua    # game/debug 카메라 모드
├── 03_game/               # 게임 로직
│   ├── components/          # 순수 데이터 ECS 컴포넌트 (8종)
│   ├── systems/             # ECS 시스템 (6종)
│   ├── entities/            # 엔티티 팩토리 + player 파사드
│   ├── patterns/            # 탄막 패턴 (예정)
│   └── states/              # 게임 상태 (예정)
└── 04_ui/                 # HUD, 모바일 레이아웃
    ├── uiManager.lua
    ├── topHud.lua
    ├── bottomControls.lua
    └── mobileLayout.lua
```

### 4.2. 핵심 설계 원칙
- **레이어 의존성**: `04_ui → 03_game → 02_renderer → 01_core → 00_common` (역방향 금지)
- **ECS 아키텍처**: 컴포넌트(순수 데이터) + 시스템(로직) + 엔티티(ID)
- **Unity 호환 카메라**: orthographicSize, 수학적 좌표계 (Y+ 상향)
- **모바일 우선**: 터치 입력 파이프라인, 9:20 세로 레이아웃

---

## 5. 향후 확장 계획

### 5.1. 단기 목표 (탄막 시스템)
- [ ] 오브젝트 풀링 (bulletPool)
- [ ] BulletPattern 컴포넌트 + BulletPatternSystem
- [ ] 기본 탄막 패턴 (원형, 나선형)
- [ ] 충돌 검사 시스템 (CollisionSystem)

### 5.2. 중기 목표 (게임플레이)
- [ ] 적 AI 시스템 (이동/공격 패턴)
- [ ] 게임 상태 머신 (title → playing → gameover)
- [ ] 사운드 시스템
- [ ] 경험치/레벨업 시스템

### 5.3. 장기 목표 (최적화/포팅)
- [ ] Canvas 배치 렌더링
- [ ] 공간 파티셔닝 (spatialGrid)
- [ ] 모바일 포팅 (Android/iOS)
- [ ] 절차적 콘텐츠 생성

---

## 6. 기술적 특징

### 6.1. Unity 개발자 친화적
- Orthographic Size 개념으로 직관적 줌 제어
- 수학적 좌표계 (3D 엔진과 동일)
- Component-based 확장 가능성

### 6.2. 픽셀 퍼펙트 
- UI 요소들이 줌과 무관하게 정확한 픽셀 크기 유지
- 깔끔한 픽셀 아트 스타일 지원
- 다양한 해상도 대응

### 6.3. 모듈 기반 확장성
- 각 시스템 독립적 구성
- 필요에 따른 점진적 기능 추가
- 간단한 테스트와 디버깅

---

## 8. 모바일 개발 고려사항 

### 8.1. LÖVE2D 모바일 지원
- **Android**: love-android-sdl2를 통한 네이티브 앱 배포
- **iOS**: love-ios-source를 통한 App Store 배포 가능
- **성능**: 네이티브에 가까운 성능으로 60+ FPS 유지 가능

### 8.2. 터치 입력 구현 계획
```lua
-- 터치 이벤트 처리 예시
function love.touchpressed(id, x, y, dx, dy, pressure)
    local worldX, worldY = mainCamera:worldCoords(x, y)
    -- 터치 위치에 따른 게임 로직
end

function love.touchmoved(id, x, y, dx, dy, pressure)
    -- 드래그 입력으로 카메라 이동
    mainCamera:move(-dx * camera:getUnitsPerPixel(), 
                    dy * camera:getUnitsPerPixel())  -- Y축 반전
end
```

### 8.3. 모바일 UI 레이아웃
```
┌─────────────────┐  ← 상단 안전 영역 (상태바, 노치 고려)
│  점수  | 설정    │  
├─────────────────┤  
│                 │  
│   게임 월드      │  ← 메인 게임 영역 (터치 입력)
│   (카메라 뷰)    │  
│                 │  
├─────────────────┤  
│ [확대] [축소]    │  ← 하단 UI (엄지손가락 접근성)
│      [리셋]      │  
└─────────────────┘  ← 하단 안전 영역 (홈 인디케이터)
```

### 8.4. 모바일 최적화 요소
- **배터리 효율**: 불필요한 계산 최소화, 절전 모드 지원
- **터치 반응성**: 30ms 이하 입력 지연시간 목표 
- **해상도 대응**: 다양한 화면비 (18:9, 19.5:9, 20:9) 지원
- **세로 모드 우선**: Portrait 기본, Landscape 옵션 지원

### 8.5. 성능 목표 (모바일)
- **프레임레이트**: 60 FPS (고사양), 30 FPS (저사양) 안정 유지
- **메모리 사용량**: 100MB 이하 (기본 동작)
- **배터리 수명**: 연속 플레이 2시간+ 목표
- **앱 크기**: APK 50MB 이하 (스토어 배포 고려)

---

## 7. 개발 히스토리 

### 7.1. 완료된 마일스톤
- ✅ **명명 규칙 통일**: PascalCase → camelCase로 모든 모듈 전환
- ✅ **카메라 시스템 구축**: 복잡한 멀티 인스턴스 → 단순한 Unity 스타일
- ✅ **월드 좌표계**: 수학적 좌표계 (Y+ 상향) 구현  
- ✅ **픽셀 정확도**: 줌과 무관한 일정한 UI 크기
- ✅ **Viewport 제어**: 분할 화면, 미니맵 등을 위한 카메라 위치 제어

### 7.2. 주요 학습사항
- **단순함의 힘**: 복잡한 ECS보다 직접적인 접근이 더 효과적
- **Unity 패턴 적용**: 기존 게임 엔진 경험 활용 가능
- **점진적 개발**: 기초를 탄탄히 한 후 기능 추가하는 방식의 중요성

이 문서는 **실제 구현 기준**으로 작성되어, 현재 코드베이스와 정확히 일치합니다.

## 1. 프로젝트 개요
- **장르:** 절차적 탄막 액션 (Procedural Bullet Hell)
- **플랫폼:** Mobile (Android) - LOVE2D 기반
- **화면 비율:** 9:20 Portrait (갤럭시 노트 20 기준)
- **개발 기간:** 1개월 (MVP 데모 기준)  
    - **MVP (Minimum Viable Product):** 최소 기능 제품 
    - 게임이 재미있는지 검증할 수 있는 플레이 가능한 최소 버전
- **핵심 목표:** 
    - 아트 리소스 제로 (Code-Only Visuals)
    - 모바일 최적화된 ECS 아키텍처
    - 터치 기반 직관적 컴트롤 시스템

---

## 2. 핵심 게임 메커니즘 (Core Gameplay)

### 2.1. 플레이어 컴트롤 (Touch Controls)
- **기본 이동:** 드래그로 플레이어 직접 이동 (정밀 컴트롤)
- **스와이프 대쉬:** 빠른 스와이프 → 해당 방향으로 순간이동 + 무적시간 (쿨타임 3초)
- **포커스 모드:** 롱프레스 → 시간 느려짐 + 정밀 조준 모드 (에너지 소모 3초 지속)
- **공격:** 자동 발사 시스템 (터치 조작 단순화)

### 2.2. 세로 화면 탄막 시스템 (Vertical Bullet Hell)
- **화면 레이아웃:** 위에서 아래로 내려오는 탄막 패턴 (9:20 Portrait 최적화)
- **기하학적 패턴:** 원형, 나선형, 조준형, 파동형 등 수학적 함수 기반 탁막 생성
- **엔티티 밀도:** 모바일 성능에 맞는 현실적 목표 (100-500+ 객체, 60 FPS 보장)
- **상호작용:** 탁환은 플레이어에게 대미지를 입히거나, 대쉬/포커스 능력으로 회피 가능

### 2.3. 모바일 UI 레이아웃
```
┌─────────────────┐  ← 상단 5% (점수, 생명력)
│  SCORE: 1250    │
│  ♥♥♥ DASH:●●●   │
├─────────────────┤  
│                 │
│      적/탁막     │  ← 중상단 20% (적 등장 구역)
│                 │
│   ✦ ✧ ✦ ✧ ✦    │
│ ✧ ○ ✦ ○ ✧ ○ ✦ │  ← 중앙 50% (메인 게임 플레이 구역)
│   ✦ ✧ ✦ ✧ ✦    │
│                 │
│       🚀        │  ← 중하단 15% (플레이어 안전 구역)
│                 │
├─────────────────┤
│ PAUSE    FOCUS   │  ← 하단 10% (터치 UI)
│                 │
└─────────────────┘
```

---

## 3. 기술적 아키텍처 (Technical Architecture)

### 3.1. 레이어별 폴더 구조 (Layered Architecture)
확장 가능하고 유지보수가 용이한 **엔진-렌더링-게임** 레이어 분리 설계:

```
├── main.lua                # Entry Point (LOVE2D 콜백 연결)
├── conf.lua                # 엔진 설정 (윈도우 크기, VSync, 고성능 모드 등)
│
├── src/                    # 소스 코드 메인
│   ├── core/               # [Engine Layer] 재사용 가능한 엔진 라이브러리
│   │   ├── ecs.lua         # 핵심 ECS 엔진 (Entity/Component/System 관리)
│   │   ├── registry.lua    # 전역 객체 등록 및 관리
│   │   ├── resource.lua    # 리소스 로더 (자동 캐싱, 핫 리로딩 대응)
│   │   └── input.lua       # 입력 추상화 (키보드, 게임패드 통합)
│   │
│   ├── renderer/           # [Rendering Layer] 렌더링 최적화 및 셰이더
│   │   ├── batcher.lua     # 수만 개 탄환을 위한 Draw Call Batching
│   │   ├── camera.lua      # 화면 흔들림(Shake), 줌, 이동 관리
│   │   └── post_effect.lua # Bloom, Glow, Blur 셰이더 체인 관리
│   │
│   └── game/               # [Game Layer] 실제 게임 로직
│       ├── systems/        # 각 도메인별 시스템 (Move, Collision, Pattern)
│       ├── components/     # 데이터 정의 (Transform, Velocity, Collider)
│       ├── patterns/       # 탄막 발사 로직 (Lua 스크립트 기반)
│       └── states/         # GameState 관리 (Intro, Play, GameOver)
│
├── assets/                 # [Resource Layer] 리소스 파일
│   ├── shaders/            # 핵심 비주얼을 담당할 GLSL 파일들
│   ├── data/               # 적 패턴, 밸런스 설정 (JSON 혹은 Lua 테이블)
│   └── sfx/                # 최소한의 사운드 에셋
│
└── lib/                    # 외부 오픈소스 라이브러리 (hump, lume 등)
```

### 3.2. 모바일 최적화 ECS 구조 (Mobile-Optimized ECS)
**Engine Layer (src/01_core/ecs.lua)** 기반의 효율적 아키텍처:

#### Components (Data-Only Structures):
```lua
-- src/03_game/components/
Transform = {x, y, angle}           -- 세로 화면 좌표계 최적화
Velocity = {vx, vy, speed, accel}   -- 물리 이동 데이터
TouchCollider = {radius, layer}     -- 모바일 터치 충돌
LifeSpan = {time, maxTime}          -- 자동 소거 관리
BulletPattern = {type, params}      -- 탄막 패턴 데이터
```

#### Systems (Logic Processing):
```lua
-- src/03_game/systems/
TouchInputSystem    -- 드래그, 스와이프, 롱프레스 입력 처리
MovementSystem      -- Velocity → Transform 위치 갱신
CollisionSystem     -- 공간 분할 기반 효율적 충돌 검사
PatternSystem       -- 탁막 패턴 생성 및 관리
RenderSystem        -- Renderer Layer와 연동하여 배치 렌더링
```

### 3.3. 고성능 렌더링 파이프라인 (High-Performance Rendering)
**Rendering Layer (src/02_renderer/)** 를 통한 모바일 GPU 최적화:

#### 배치 렌더링 (src/02_renderer/batcher.lua):
- **SpriteBatch 집약**: 수백 개 탄환을 단일 Draw Call로 처리
- **인스턴싱**: 동일 형태 탄환의 위치/회전/색상만 변경하여 렌더링
- **컴링**: 화면 밖 객체 자동 제거로 성능 향상

#### 세로 화면 카메라 (src/02_renderer/camera.lua):
- **9:20 비율 최적화**: 세로 화면에 맞는 뷰포트 관리
- **스크린 쉐이크**: 충돌/폭발 시 화면 진동 효과
- **스무스 팔로우**: 플레이어 추적 카메라 (옵션)

---

## 4. 데이터 및 리소스 전략

### 4.1. Zero-Art 리소스
- 이미지 파일 없이 모든 비주얼을 **절차적 생성(Procedural Generation)**으로 처리.
- **색상 체계:** 고대비 형광색(Neon)과 검은색 배경의 조합으로 시인성 확보.

### 4.2. 터치 기반 패턴 시스템 (Touch-Based Pattern System)
- 탁막 패턴을 외부 `.lua` 파일로 관리하여, 모바일 터치 인터렉션에 최적화된 개발 환경.

```lua
-- 세로 화면 패턴 예시 (vertical_spiral.lua)
return {
    bullets_per_shot = 8,        -- 모바일 성능 고려 감소
    angle_step = 0.3,            -- 세로 화면에 맞는 각도
    vertical_speed = 200,        -- 위에서 아래로 내려오는 속도  
    color = {0, 1, 0.8},        -- 네온 민트 (모바일 시인성 최적화)
    touch_dodge_hint = true      -- 터치 회피 가이드 표시
}
```

---

## 5. MVP 및 개발 단계 (MVP & Development Phases)

### 5.1. MVP 범위 (Minimum Viable Product Scope)
플레이 가능한 최소 기능 - 모바일 탄막 게임의 핵심 재미 검증:

#### ✅ MVP 핵심 기능 (Week 1-2)
- **터치 컨트롤:** 드래그로 플레이어 이동 (정밀 컨트롤)
- **기본 탄막:** 2-3가지 패턴 (원형, 직선, 나선형)
- **충돌 검사:** 플레이어-탄환 충돌 및 게임 오버
- **기본 UI:** 점수, 생명력 표시
- **ECS 기반:** Entity-Component-System 기본 아키텍처
- **성능 목표:** 100+ 엔티티, 60 FPS 안정

#### 🚧 MVP 후 확장 기능 (Week 3-4)
- **고급 컨트롤:** 스와이프 대쉬 (순간이동 + 무적시간)
- **포커스 모드:** 롱프레스 → 시간 느려짐
- **복잡 패턴:** 파동형, 조준형 고급 탄막
- **성능 향상:** 300-500+ 엔티티
- **스테이지 시스템:** 난이도 진행

#### ✨ 완성도 향상 (Week 5+)
- **사운드 & 파티클:** 절차적 생성 이펙트
- **네온 이펙트:** 모바일 배터리 고려 최적화
- **UI/UX 완성:** 메뉴, 옵션, 일시정지
- **밸런싱:** 난이도 곡선 및 점수 시스템 완성

### 5.2. 개발 우선순위 (Priority Matrix)
```
핵심 기능     │ 사용자 경험  │ 완성도
──────────────┼──────────────┼──────────
1월 MVP     │ 2월 확장    │ 3월+ 향상
ECS 기본     │ 대쉬/포커스   │ 사운드
기본 탄막     │ UI/UX      │ 네온 이펙트
충돌 검사   │ 카메라 이펙트 │ 밸런싱
```