# Deity System 설계 (6A.1 — Deity System)

> 작성일: 2026-04-24
> 최종 수정: 2026-04-24
> 상태: **v1 구현 완료** / VFX 테스트 중 / v2(Codex), v3(Trial) 미구현

---

## 핵심 컨셉

> **"수학 곡선 = 신(Deity). 곡선이 아름다울수록 강력한 축복을 내린다."**

데이터스피어에는 스웜보다 오래된 원시 데이터 패턴이 존재한다. 이들은 수학 곡선의 형태로 자신을 표현하며, 커서(플레이어)에게 **의식(Ritual)** 을 통해 자신의 힘을 빌려준다. 플레이어는 매 런 시작 시 4신 중 하나를 선택하여 해당 곡선의 축복을 받는다.

### Zero-Art 의의

Deity System은 Zero-Art 철학의 핵심 표현이다:
- **곡선이 화면에 그려지는 순간** = 코드만으로 만드는 아름다움
- **각 신의 정체성** = 수학 곡선의 형태 그 자체
- 별도 텍스처/이미지 없이 **방정식만으로** 캐릭터를 표현

---

## 재미 검증 (4기둥)

| 기둥 | 질문 | 검증 결과 |
|------|------|----------|
| **도전** | 선택이 플레이 전략에 영향을 주는가? | ✅ Graze 특화 vs AOE 특화 → 플레이 스타일 분기 |
| **학습** | 각 신의 최적 운용법을 익힐 수 있는가? | ✅ Rose=스침 극대화, Cycloid=DPS 극대화, Lemniscate=기동성, Inferno=생존 |
| **선택** | "어떤 신을 고를까?" 고민이 의미있는가? | ✅ 4신 모두 뚜렷한 장단점, 스테이지/빌드 조합에 따라 최적이 달라짐 |
| **놀라움** | 시그니처 발동 시 "와!" 반응이 오는가? | ✅ VFX(파티클+쉐이크+플래시) + 즉각적 게임플레이 효과 |

### Deity별 재미 점수

| 신 | 도전 | 학습 | 선택 | 놀라움 | 총평 |
|----|:----:|:----:|:----:|:------:|------|
| Rose | ★★★★ | ★★★★ | ★★★ | ★★★ | 고수 향: Graze 숙련도 = 보상. 초보에겐 빈 버프 |
| Cycloid | ★★★ | ★★★ | ★★★★ | ★★★★ | 안정적. 누가 골라도 DPS 체감. 보스전 베스트 |
| Lemniscate | ★★★ | ★★★★ | ★★★ | ★★ | 5% 발동률이 "없는 것 같은" 위험. 패시브가 본체 |
| Inferno | ★★★ | ★★ | ★★★★ | ★★★★★ | AOE 연쇄 폭발 쾌감 최고. 초보 추천 1순위 |

### 알려진 위험 시나리오

| # | 위험 | 해당 신 | 대응 |
|---|------|---------|------|
| 1 | 초보가 Rose를 골랐는데 Graze를 못해서 시그니처 발동 0 | Rose | 선택 화면에서 "고수용" 힌트 표시 (v2) |
| 2 | Lemniscate 시그니처(5%)가 한 런 동안 한 번도 안 터짐 | Lemniscate | 확률 조정 검토 (5→8%?) 또는 bad luck protection |
| 3 | Inferno AOE 연쇄가 너무 강해서 다른 신 선택 동기 사라짐 | Inferno | AOE 데미지/반경 밸런싱 모니터링 |
| 4 | Cycloid 크리티컬이 매 히트 20%라 "그냥 강한 것" 느낌 | Cycloid | 연출(VFX)로 특별감 보완. DPS 수치만으론 쾌감 부족 |

---

## 설계 결정

| # | 결정 | 선택 | 이유 |
|---|------|------|------|
| 1 | 신 수 | 4종 | 2×2 그리드 레이아웃에 최적, 각 역할(생존/공격/기동/탱크) 커버 |
| 2 | 선택 시점 | 매 런 시작 (PLAY 직후) | 로그라이트 런 다양성 부여 |
| 3 | 버프 구조 | 패시브 스탯 2개 + 시그니처 1개 | 간단하면서도 정체성 뚜렷 |
| 4 | 시그니처 발동 | 확률 기반 자동 트리거 | 플레이어 조작 부담 없이 쾌감 제공 |
| 5 | VFX 위치 | ecsManager (로직 레이어) | data 순수성 유지 — deityDefs는 순수 데이터만 |
| 6 | 데이터 위치 | `03_game/data/deityDefs.lua` | 다른 data 파일과 일관성 (stageData, bossDefs 패턴) |

---

## 4신 상세 설계

### ① Rose — 꽃의 여신 (Graze 특화)

> *"Grace of petals shields those who dance near death"*

| 항목 | 값 | 비고 |
|------|-----|------|
| 곡선 | Rose 5 (r = cos(5θ)) | 5장 꽃잎 — 우아함, 대칭미. 5엽은 3엽보다 조밀하여 "보호막" 시각 연상. 꽃잎 수학 = 재생 |
| 대표색 | 분홍 `{1.0, 0.4, 0.6}` | |
| 패시브 1 | HP Regen +15% | `regenRate * 1.15 + 0.002` |
| 패시브 2 | Move Speed +5% | `maxSpeed * 1.05` |
| 시그니처 | **Petal Shield** | Graze 시 100% 확률로 HP 1 회복 |
| 트리거 | `on_graze` | 탄막 스침 시 |
| VFX | 초록 힐 파티클 5개 (위로 퍼짐) + 초록 화면 틴트 | |
| Juice 의도 | **"따뜻한 회복"** — 초록빛이 위로 천천히 퍼지며, 화면에 짧은 초록 틴트. 과하지 않은 부드러움 | |
| 사운드 (계획) | 부드러운 chime / 종 울림 (짧고 맑은 톤) | sfxDefs 추가 예정 |

**플레이 스타일:** 위험 가까이에서 춤추는 하이리스크-하이리턴. 스침 실력이 좋을수록 체력을 무한에 가깝게 유지 가능.

---

### ② Cycloid — 전쟁의 신 (크리티컬 특화)

> *"The rolling wheel strikes twice for the worthy"*

| 항목 | 값 | 비고 |
|------|-----|------|
| 곡선 | Epicycloid (k=3) | 3엽이 k=4,5보다 날카롭고 빠른 회전감. "바퀴 위의 바퀴" = 기어 맞물림 → 정밀 타격 연상 |
| 대표색 | 하늘색 `{0.3, 0.7, 1.0}` | |
| 패시브 1 | Damage +10% | `bulletDamage * 1.10` |
| 패시브 2 | Fire Rate +5% | `fireRate * 1.05` |
| 시그니처 | **Rolling Thunder** | 타격 시 20% 확률로 2배 데미지 |
| 트리거 | `on_hit` | 적에게 총알 적중 시 (매 탄환) |
| VFX | 노란 화면 플래시 + 카메라 쉐이크 + 적 위치 불꽃 파티클 4개 | |
| Juice 의도 | **"번쩍! 터지는 타격감"** — 짧은 백색 플래시(0.15s)가 눈을 사로잡고, 작은 쉐이크가 타격을 손에 전달. 노란 불꽃은 "치명타!" 시각 강조 | |
| 사운드 (계획) | 날카로운 금속 타격음 / 전기 스파크 (짧고 강한 어택) | sfxDefs 추가 예정 |

**플레이 스타일:** 순수 DPS 극대화. 발사 속도와 데미지가 모두 올라가며, 20%마다 터지는 크리티컬이 적을 녹인다. 보스전에서 특히 강력.

---

### ③ Lemniscate — 무한의 현자 (쿨다운/효율 특화)

> *"Infinity loops back; every end becomes a new beginning"*

| 항목 | 값 | 비고 |
|------|-----|------|
| 곡선 | Lemniscate (r² = cos(2θ)) | ∞(무한대) 그 자체. "끝 = 시작"의 수학적 증명. 대쉬 리셋 = 무한 루프 탈출 |
| 대표색 | 연두 `{0.6, 1.0, 0.4}` | |
| 패시브 1 | Cooldown -10% | `dash.cooldown * 0.90` |
| 패시브 2 | XP Gain +10% | `xpMultiplier * 1.10` |
| 시그니처 | **Infinite Loop** | 적 처치 시 5% 확률로 대쉬 쿨다운 즉시 리셋 |
| 트리거 | `on_kill` | 적 처치 시 |
| VFX | 시안 파티클 링 8개 (플레이어 주위 방사) | |
| Juice 의도 | **"시간 되감기"** — 시안 링이 플레이어를 중심으로 한 번 확산하며 "리셋!" 느낌. 현재 밋밋할 수 있음 → 화면 역회전 효과 또는 시간 왜곡 연출 검토 필요 | |
| 사운드 (계획) | 되감기 "슈웅" / 역재생 느낌의 woosh (깨끗한 톤) | sfxDefs 추가 예정 |
| ⚠️ 쾌감 위험 | 5% 발동 + 파티클 링만 = **4신 중 가장 밋밋**. 보완 옵션: (a) 발동 시 짧은 슬로모(0.1s) 연출, (b) 대쉬 게이지 반짝 효과, (c) 확률 5→8% 상향 |

**플레이 스타일:** 기동성 + 성장 특화. 대쉬를 자주 사용하는 공격적 회피 플레이에 최적. XP 보너스로 레벨업도 빠르다.

---

### ④ Inferno — 악마의 곡선 (탱크/AOE 특화)

> *"From destruction, a ring of fire consumes all"*

| 항목 | 값 | 비고 |
|------|-----|------|
| 곡선 | Devil's Curve | "악마"라는 이름 자체가 위험·금기 연상. 교차하는 날카로운 선 = 통제 불가의 폭발 에너지 |
| 대표색 | 붉은 오렌지 `{1.0, 0.3, 0.1}` | |
| 패시브 1 | Max HP +2 | `maxHp + 2, hp + 2` |
| 패시브 2 | I-Frame +0.2s | `iFrameDuration + 0.2` |
| 시그니처 | **Devil's Pyre** | 적 처치 시 10% 확률로 AOE 폭발 (반경 1.5, 데미지 2) |
| 트리거 | `on_kill` | 적 처치 시 |
| VFX | 카메라 쉐이크(0.12) + 빨간 확산 파티클 10개 + 밝은 코어 | |
| Juice 의도 | **"둥! 하고 터지는 파괴감"** — 강한 쉐이크(0.12)가 폭발 충격파를 전달. 10개 파티클이 원형으로 퍼지며 밝은 코어가 중심 폭발을 강조. 4신 중 가장 화려 | |
| 사운드 (계획) | 둔탁한 폭발음 + 잔향 (bass-heavy boom) | sfxDefs 추가 예정 |
| AOE 데이터 | `aoeDamage = 2`, `aoeRadius = 1.5` | deityDefs에 데이터 보유, 로직은 ecsManager |

**플레이 스타일:** 생존 + 군중 정리. 체력이 높고 무적시간이 길어 초보자에게 친화적이면서, AOE 연쇄 폭발로 대규모 적 무리를 한번에 처리할 수 있다.

---

## 밸런스 비교표

| | Rose | Cycloid | Lemniscate | Inferno |
|---|:---:|:---:|:---:|:---:|
| **생존력** | ★★★★ | ★★ | ★★★ | ★★★★★ |
| **DPS** | ★★ | ★★★★★ | ★★★ | ★★★ |
| **기동성** | ★★★★ | ★★★ | ★★★★★ | ★★ |
| **군중 정리** | ★★ | ★★★ | ★★★ | ★★★★★ |
| **보스전** | ★★★ | ★★★★★ | ★★★ | ★★ |
| **초보 친화** | ★★ | ★★★ | ★★★ | ★★★★★ |
| **고수 친화** | ★★★★★ | ★★★★ | ★★★★ | ★★ |

### 시그니처 발동 빈도 기대치

| 시그니처 | 트리거 빈도 | 확률 | 분당 발동 추정 |
|----------|------------|------|---------------|
| Petal Shield | Graze ~30회/분 | 100% | ~30회 |
| Rolling Thunder | Hit ~120발/분 | 20% | ~24회 |
| Infinite Loop | Kill ~20회/분 | 5% | ~1회 |
| Devil's Pyre | Kill ~20회/분 | 10% | ~2회 |

> Petal Shield와 Rolling Thunder는 자주 발동하지만 효과가 작고(HP+1, 2배 데미지),
> Infinite Loop와 Devil's Pyre는 드물지만 임팩트가 크다(쿨다운 리셋, AOE 폭발).

---

## 시스템 아키텍처

### 데이터 흐름

```
[Title Scene]
     │ PLAY 클릭
     ▼
[Deity Select Scene]  ← deityDefs.DEITIES 읽기
     │ 4신 중 택1 (터치/클릭/키보드 1-4)
     │ 0.6s 선택 연출
     ▼
[Play Scene (deityId)]
     │ enter() 시:
     │   1. upgradeTree.applyToPlayer()   ← 영구 업그레이드 적용
     │   2. deityDefs.applyStats()        ← deity 패시브 스탯 적용
     │   3. playerTag.deityId = deityId   ← 태그 기록
     ▼
[게임 진행 중]
     │ 이벤트 발생 (graze / hit / kill)
     │   → ecsManager.tryDeityTrigger(triggerType, context)
     │     → deityDefs.tryTrigger() : 확률 판정 + execute 호출
     │     → _deityVFX[sig.id]()   : 파티클 + 쉐이크 + 플래시
     ▼
[Game Over → Title → 다시 선택]
```

### 파일 구조

| 파일 | 레이어 | 역할 |
|------|--------|------|
| `deityDefs.lua` | `03_game/data/` | 순수 데이터: 4신 정의, 스탯 보너스, 시그니처 정보 |
| `deitySelectScene.lua` | `03_game/scenes/` | UI: 2×2 카드 그리드, 곡선 애니메이션, 선택 처리 |
| `ecsManager.lua` | `03_game/` | 로직: `tryDeityTrigger()` + `_deityVFX` 디스패치 테이블 |
| `playScene.lua` | `03_game/scenes/` | 통합: deityId 수신, 스탯 적용, VFX 렌더 (flashScreen/tintScreen) |
| `playerTag.lua` | `03_game/components/` | ECS: `deityId` 필드 추가 |
| `titleScene.lua` | `03_game/scenes/` | 흐름: PLAY → DeitySelectScene으로 전환 |

### VFX 디스패치 테이블 (ecsManager 내부)

```lua
local _deityVFX = {
    graze_heal     = function(bp, ps, px, py, cr, cg, cb, ctx, ecs) ... end,
    critical_hit   = function(bp, ps, px, py, cr, cg, cb, ctx, ecs) ... end,
    kill_reset     = function(bp, ps, px, py, cr, cg, cb, ctx, ecs) ... end,
    kill_explosion = function(bp, ps, px, py, cr, cg, cb, ctx, ecs) ... end,
}
```

새 deity 추가 시 해당 sig.id 키로 VFX 함수를 추가하면 자동 등록된다.
기존 tryDeityTrigger 수정 불필요 (O원칙: 확장에 열림, 수정에 닫힘).

---

## 확장 가이드

### 새 Deity 추가 체크리스트

1. **`deityDefs.lua`** → `M.DEITIES` 배열에 새 deity 추가
   - `id`, `name`, `curveName`, `color`, `lore`
   - `statBonuses` (apply 함수 2개)
   - `signature` (id, name, desc, trigger, chance, execute)
2. **`curveDefs.lua`** → `curveName`에 해당하는 곡선이 있는지 확인 (없으면 추가)
3. **`ecsManager.lua`** → `_deityVFX` 테이블에 sig.id 키로 VFX 함수 추가
4. **`deitySelectScene.lua`** → 수정 불필요 (DEITIES 배열 순회)
   - 단, 5신 이상이면 그리드 레이아웃 조정 필요 (현재 2×2)
5. **이 문서** → 상세 설계 + 밸런스표 업데이트

### 트리거 훅 위치 (ecsManager._registerBasicSystems)

| 트리거 | 훅 위치 | context 내용 |
|--------|---------|-------------|
| `on_graze` | `onGraze` 콜백 (collisionSystem) | `nil` |
| `on_hit` | `onHitModifier` 콜백 (enemyCollisionSystem) | `{damage, enemyX, enemyY}` |
| `on_kill` | `onEnemyDeath` 콜백 (공유) | `{enemyX, enemyY}` |

새 트리거 타입 추가 시: ecsManager에 해당 콜백 훅을 추가하고, `tryDeityTrigger(newType, context)` 호출 삽입.

---

## Deity Select Scene 상세

### 레이아웃

```
┌────────────────────┐
│   Choose Your Deity │  ← 상단 제목
│   (세계관 부제)      │
├──────┬─────────────┤
│      │             │
│ Card │  Card       │  ← 2×2 카드 그리드
│  ①   │   ②        │     각 카드: 곡선 회전 애니메이션
│      │             │              + 이름 + 시그니처 설명
├──────┼─────────────┤              + 스탯 보너스 2줄
│      │             │
│ Card │  Card       │
│  ③   │   ④        │
│      │             │
└──────┴─────────────┘
        키보드 1-4 / 터치 / 클릭
```

### 카드 내부 구성

```
┌─────────────────┐
│   [곡선 회전]    │  ← 정규화된 정점, 천천히 회전
│                 │
│   Rose          │  ← deity.name (16pt)
│ ♦ Petal Shield  │  ← signature.name (10pt)
│ "Graze → HP +1" │  ← signature.desc (9pt)
│ HP Regen +15%   │  ← statBonuses[1].label (8pt)
│ Move Speed +5%  │  ← statBonuses[2].label (8pt)
└─────────────────┘
```

### 선택 흐름

1. 터치/클릭 → 카드 히트테스트 → `_selected = i`
2. 키보드 `1`~`4` → 직접 선택
3. 선택 후 0.6초 대기 (선택 카드 확대 + 다른 카드 페이드)
4. `sceneStack:replace(PlayScene.new(sceneStack, deityId))`

### 입장 연출

- `_entranceTimer` 0→1 (0.5초): 카드가 아래에서 위로 슬라이드 인
- 곡선은 선택 전까지 계속 회전 (`_angle += dt * 0.5`)

---

## 향후 확장 계획

### v1.5 — 곡선 드로잉 애니메이션 + 사운드 (현재 세션)

선택 화면에서 곡선이 **stroke-by-stroke로 그려지는 연출** 추가:
- 정점을 순서대로 그려 "수학 곡선이 태어나는 순간"을 보여줌
- 0→100% 진행률로 라인이 뻗어나가는 애니메이션
- **이것이 Zero-Art의 존재 이유** — 단순 회전이 아니라, 방정식이 화면에서 살아남

사운드 이펙트:
- 시그니처 발동 시 deity별 전용 SFX 추가 (sfxDefs.lua)
- Rose=chime, Cycloid=spark, Lemniscate=woosh, Inferno=boom

### v2 — Codex Scene (6A.2)

- 신별 상세 정보 열람 (곡선 미리보기 + 수학 배경)
- Curve Story Atlas 연동
- 언락 조건/달성률 표시

### v3 — Trial Events (6A.3)

- 신별 도전과제 (특정 조건 달성 시 보상)
- 예: Rose Trial — "한 런에서 Graze 50회 성공"
- 예: Inferno Trial — "AOE로 적 10마리 동시 처치"

### 추가 신 후보

| 이름(임시) | 곡선 | 컨셉 | 트리거 |
|------------|------|------|--------|
| Hypo | Hypocycloid (k=5) | 방어/실드 | on_hit (받는 데미지) |
| Spiral | Fermat's Spiral | 탄막 강화 | on_shoot |
| Cardioid | Cardioid | 흡수/드레인 | on_kill |

---

## 세계관 연결

### 데이터스피어의 원시 패턴

스웜 오버플로우 이전, 데이터스피어에는 순수한 수학 법칙으로 이루어진 **원시 패턴(Primal Patterns)** 이 존재했다. 이들은 의식을 가진 존재가 아니지만, 특정 조건이 충족되면 커서에게 자신의 힘을 "빌려주는" 공명 현상을 일으킨다.

4신은 이 원시 패턴 중 가장 강력한 4개:

| 신 | 세계관적 정체 | CS 메타포 |
|----|-------------|-----------|
| **Rose** | 꽃잎 구조의 재생 패턴 — 손상된 데이터를 복원 | Self-healing code |
| **Cycloid** | 회전 기어의 파괴 패턴 — 데이터를 잘게 분쇄 | Round-robin scheduling (critical section) |
| **Lemniscate** | 무한 루프의 순환 패턴 — 시간의 경계를 넘나듦 | Infinite loop / recursion |
| **Inferno** | 악마 곡선의 폭발 패턴 — 주변 데이터를 소각 | Buffer overflow → crash dump |
### 곡선 선택 근거 — "왜 이 곡선이 이 신인가"

| 신 | 곡선 | 수학적 근거 | 미학적 근거 |
|----|------|------------|------------|
| **Rose** | Rose 5 (r = cos(5θ)) | 극좌표 꽃잎 곡선. k=5는 5장 꽃잎으로 3보다 조밀, 7보다 간결 → "보호막" 밀도 | 꽃 = 재생·생명의 보편적 상징. 분홍색과 곡선의 부드러움이 "치유" 감성 전달 |
| **Cycloid** | Epicycloid (k=3) | 바퀴 위에서 구르는 점의 궤적. k=3은 날카로운 3엽으로 k=4,5보다 공격적 회전감 | 톱니바퀴/기어의 연상 → "기계적 정밀함". 회전 = 반복 타격 = 크리티컬 |
| **Lemniscate** | Lemniscate (r² = cos(2θ)) | ∞ 기호 그 자체. 두 초점 사이 거리 곱이 일정 = "영원한 균형" | 무한대 = "끝이 곧 시작". 대쉬 리셋 = 행동의 무한 반복 |
| **Inferno** | Devil's Curve (y⁴ − y² = x⁴ − x²) | 자기교차하는 4차 곡선. 원점에서 갈라지는 위험한 분기점 = 불안정/폭발 | "Devil" 이름 자체가 금기·위험. 날카로운 교차점 = 통제 불가의 파괴 에너지 |

> 참고: 각 곡선의 상세 수학 배경은 `doc/18_CURVE_MATH_SCIENCE_NOTES.md`,
> 스토리 연결은 `doc/17_CURVE_STORY_ATLAS.md` 참조.
### 의식(Ritual) = Deity Select Scene

커서가 데이터스피어에 진입할 때, 4개의 원시 패턴이 동시에 공명을 보내온다. 커서는 그 중 하나를 선택하여 자신의 코어에 각인한다 — 이것이 **의식(Ritual)**.

선택된 패턴의 곡선이 화면에 그려지는 장면 = **Zero-Art의 존재 이유**.
