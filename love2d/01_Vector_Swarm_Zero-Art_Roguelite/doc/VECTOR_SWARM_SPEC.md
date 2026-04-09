# 🚀 Project: Vector Swarm (가제)
> **"수학적 패턴과 네온 그래픽의 조화, 기술 기반 탄막 액션 시뮬레이터"**

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
**Engine Layer (src/core/ecs.lua)** 기반의 효율적 아키텍처:

#### Components (Data-Only Structures):
```lua
-- src/game/components/
Transform = {x, y, angle}           -- 세로 화면 좌표계 최적화
Velocity = {vx, vy, speed, accel}   -- 물리 이동 데이터
TouchCollider = {radius, layer}     -- 모바일 터치 충돌
LifeSpan = {time, maxTime}          -- 자동 소거 관리
BulletPattern = {type, params}      -- 탄막 패턴 데이터
```

#### Systems (Logic Processing):
```lua
-- src/game/systems/
TouchInputSystem    -- 드래그, 스와이프, 롱프레스 입력 처리
MovementSystem      -- Velocity → Transform 위치 갱신
CollisionSystem     -- 공간 분할 기반 효율적 충돌 검사
PatternSystem       -- 탄막 패턴 생성 및 관리
RenderSystem        -- Renderer Layer와 연동하여 배치 렌더링
```

### 3.3. 고성능 렌더링 파이프라인 (High-Performance Rendering)
**Rendering Layer (src/renderer/)** 를 통한 모바일 GPU 최적화:

#### 배치 렌더링 (src/renderer/batcher.lua):
- **SpriteBatch 집약**: 수백 개 탄환을 단일 Draw Call로 처리
- **인스턴싱**: 동일 형태 탄환의 위치/회전/색상만 변경하여 렌더링
- **컴링**: 화면 밖 객체 자동 제거로 성능 향상

#### 세로 화면 카메라 (src/renderer/camera.lua):
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