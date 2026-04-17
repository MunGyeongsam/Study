# 05. 탄막 시스템 가이드 — Vector Swarm

> 마지막 갱신: 2026-04-17 (Phase 2C 완료 기준)
> 이 문서는 현재 구현된 코드 기준으로 작성되었습니다.

---

## 1. 아키텍처 개요

탄막(Bullet)은 **ECS 외부**의 전용 오브젝트 풀에서 관리됩니다.
ECS 엔티티당 컴포넌트 오버헤드를 피하고, 수천 개 탄막을 zero-GC로 처리합니다.

```
┌─────────────────────────────────────────────────────────┐
│ ECS World                                               │
│  ┌─────────────┐   ┌───────────────┐   ┌─────────────┐ │
│  │ BulletEmitter│   │ PlayerWeapon  │   │ Collision   │ │
│  │ System       │   │ System        │   │ System      │ │
│  └──────┬───────┘   └──────┬────────┘   └──────┬──────┘ │
│         │  spawn()         │  spawn()          │ check  │
│         ▼                  ▼                   ▼        │
│  ┌──────────────────────────────────────────────────┐   │
│  │              BulletPool (공유 인스턴스)             │   │
│  │  active[1..N]  ←→  inactive[1..M]               │   │
│  │  layer: "enemy_bullet" | "player_bullet"         │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## 2. BulletPool (`src/03_game/systems/bulletPool.lua`)

### 2.1. 생성
```lua
local BulletPool = require("03_game.systems.bulletPool")
local pool = BulletPool.new(2000)   -- 2000개 사전 할당
```

### 2.2. 탄막 구조
```lua
{
    x = 0, y = 0,           -- 위치
    vx = 0, vy = 0,         -- 속도
    lifetime = 0,            -- 경과 시간
    maxLifetime = 5,         -- 최대 수명 (초)
    radius = 0.04,           -- 충돌 반경 (월드 유닛)
    color = {0.4, 0.8, 1, 1}, -- RGBA (0-1)
    active = false,
    layer = "enemy_bullet",  -- "enemy_bullet" | "player_bullet"
    damage = 1,              -- 데미지
}
```

### 2.3. API

| 메서드 | 설명 |
|--------|------|
| `pool:spawn(x, y, vx, vy, opts)` | 탄막 생성 (풀에서 추출). opts: maxLifetime, radius, color, layer, damage |
| `pool:update(dt, bounds)` | 이동 + 수명/경계 체크 + 죽은 탄막 재활용 |
| `pool:draw()` | 모든 활성 탄막 렌더링 |
| `pool:clear()` | 전체 재활용 (스테이지 클리어 등) |
| `pool:getStats()` | active, inactive, max, spawned, recycled, peakActive |

### 2.4. 성능 설계
- **사전 할당**: `new()` 시 모든 탄막 테이블을 미리 생성 → 런타임 메모리 할당 0
- **Swap-remove**: 비활성 탄막을 마지막 활성 요소와 교체 → O(1) 제거, 배열 연속성 유지
- **레이어 필드**: 충돌 시스템이 `layer` 값으로 아군/적 탄막 구분
- **월드 경계**: `update(dt, bounds)`에서 경계 밖 탄막 자동 재활용

---

## 3. BulletEmitter 시스템 (`src/03_game/systems/bulletEmitterSystem.lua`)

적 엔티티의 탄막 발사를 담당. ECS 컴포넌트(`BulletEmitter` + `Transform`)를 읽어 `BulletPool`에 spawn합니다.

### 3.1. 팩토리 패턴
```lua
local createBulletEmitterSystem = require("03_game.systems.bulletEmitterSystem")
local system = createBulletEmitterSystem(bulletPool, getPlayerPos)
```

클로저로 `bulletPool`과 `getPlayerPos` 함수를 주입받습니다.

### 3.2. BulletEmitter 컴포넌트

```lua
{
    active       = true,
    pattern      = "circle",    -- "circle" | "spiral" | "aimed" | "wave"
    emitRate     = 1.0,         -- 초당 발사 횟수
    bulletCount  = 8,           -- 한 번에 발사할 탄수
    bulletSpeed  = 1.5,         -- 탄속 (월드 유닛/초)
    bulletLifetime = 6,         -- 탄막 수명
    bulletRadius = 0.04,        -- 탄막 반경
    bulletColor  = {0.4, 0.8, 1, 1},
    angle        = 0,           -- spiral/wave용 현재 각도
    turnRate     = 1.0,         -- spiral/wave 회전 속도 (rad/s)
    timer        = 0,           -- 내부 타이머
}
```

### 3.3. 탄막 패턴 4종

#### circle — 원형 방사
모든 방향으로 균일하게 발사. `bulletCount`개를 360° 균등 분배.

#### spiral — 나선형
circle과 유사하나 매 발사마다 `angle`이 `turnRate`만큼 회전. 나선 무늬 생성.

#### aimed — 조준탄
플레이어 위치를 향해 발사. `bulletCount > 1`이면 ±0.15 rad 부채꼴 spread.

#### wave — 파동형
아래로 내려오면서 sin 파형 수평 오프셋. `bulletCount`개가 다른 위상으로 출발.

---

## 4. PlayerWeapon 시스템 (`src/03_game/systems/playerWeaponSystem.lua`)

플레이어 자동 공격. 가장 가까운 적을 `range` 내에서 찾아 자동 조준·발사.

```lua
local createPlayerWeaponSystem = require("03_game.systems.playerWeaponSystem")
local system = createPlayerWeaponSystem(bulletPool)
```

### PlayerWeapon 컴포넌트
```lua
{
    fireRate     = 4,      -- 초당 발사 횟수
    range        = 6,      -- 탐색 사거리 (월드 유닛)
    damage       = 1,      -- 탄 데미지
    bulletSpeed  = 8,      -- 탄속
    bulletCount  = 1,      -- 한 번에 발사할 탄수
    piercing     = false,  -- 관통 여부
    timer        = 0,      -- 내부 타이머
}
```

발사된 탄막은 `layer = "player_bullet"`, `damage = weapon.damage`로 설정.

---

## 5. 충돌 시스템

### 5.1. CollisionSystem — 적탄 ↔ 플레이어 (`src/03_game/systems/collisionSystem.lua`)

```lua
local createCollisionSystem = require("03_game.systems.collisionSystem")
local system = createCollisionSystem(bulletPool)
```

- **쿼리**: `{"PlayerTag", "Transform", "Collider", "Health"}` 엔티티
- **검사**: `enemy_bullet` 레이어만 대상
- **판정**: circle-circle (`dist² < (playerRadius + bulletRadius)²`)
- **히트**: 탄막 재활용 + `health.hp -= 1` + `iTimer = iFrames` (무적 시작)
- **프레임당 1히트**: iFrame 즉시 적용으로 다중 피격 방지

### 5.2. EnemyCollisionSystem — 아군탄 ↔ 적 (`src/03_game/systems/enemyCollisionSystem.lua`)

```lua
local createEnemyCollisionSystem = require("03_game.systems.enemyCollisionSystem")
local system = createEnemyCollisionSystem(bulletPool, onEnemyDeath)
```

- **쿼리**: `{"EnemyAI", "Transform", "Collider", "Health"}` 엔티티
- **검사**: `player_bullet` 레이어만 대상
- **히트**: 탄막 재활용 + `health.hp -= bullet.damage`
- **처치**: `health.hp <= 0` → `onEnemyDeath(ecs, x, y, xpValue)` 콜백 → `destroyEntity()`
- **XP 드롭**: 콜백에서 `EntityFactory.createXpOrb()` 호출

---

## 6. 적 타입 프리셋 (`entityFactory.lua`)

| 타입 | 색상 | 크기 | HP | 탄막 패턴 | 탄속 | 발사율 | XP |
|------|------|------|-------|---------|------|--------|-----|
| basic | 빨강 | 0.25 | 3 | circle(8발) | 1.5 | 0.8/s | 2 |
| spiral | 보라 | 0.3 | 5 | spiral(6발) | 1.2 | 1.5/s | 5 |
| aimed | 주황 | 0.2 | 2 | aimed(3발) | 2.5 | 1.0/s | 3 |
| wave | 청록 | 0.25 | 4 | wave(5발) | 1.0 | 1.2/s | 3 |

적 AI 타입: `drift` (기본 표류), `orbit` (spiral), `chase` (aimed), `drift` (wave)

---

## 7. ecsManager 통합

`ecsManager.init()` 시 자동 구성:

```lua
-- BulletPool 생성 (2000개)
ECSManager.bulletPool = BulletPool.new(2000)

-- 월드 경계 (탄막 자동 재활용)
ECSManager.bulletBounds = {
    minX = -60, maxX = 60,
    minY = -225, maxY = 25,
}

-- update(dt) 내부:
-- 1. ECS 시스템 실행 (BulletEmitter, PlayerWeapon, Collision, EnemyCollision 포함)
-- 2. bulletPool:update(dt, bounds)  -- 이동 + 수명/경계 재활용
-- 3. enemySpawner:update(dt)       -- 웨이브 기반 적 스폰

-- draw() 내부:
-- 1. RenderSystem (적/XP 오브 등)
-- 2. bulletPool:draw()              -- 모든 활성 탄막 렌더링
-- 3. PlayerRenderSystem
```

---

## 8. 향후 확장 계획

### 구현 완료 ✅
- [x] BulletPool (2000개, zero-GC, swap-remove)
- [x] BulletEmitterSystem (4패턴)
- [x] CollisionSystem (적탄 ↔ 플레이어)
- [x] EnemyCollisionSystem (아군탄 ↔ 적)
- [x] PlayerWeaponSystem (자동 조준)

### 미구현 (Phase 3+)
- [ ] Canvas 배치 렌더링 (draw call 1000 → 1)
- [ ] 공간 파티셔닝 (spatialGrid, 충돌 검사 최적화)
- [ ] 고급 탄막 패턴 (호밍, 베지어 곡선)
- [ ] 탄막 파티클 이펙트
- [ ] 보스 전용 탄막 시퀀스
