# CS (Computer Science) 기반 적 DNA 시스템 설계

> 작성일: 2026-04-21
> 최종 수정: 2026-04-24
> 상태: **Phase 6B 구현 완료** (`0fa7b64`) — 6A(CS 특수 적 + Deity) 계획 중
> 관련 문서: [09_ENEMY_DIVERSITY_DESIGN.md](09_ENEMY_DIVERSITY_DESIGN.md) (현행 적 시스템), [99_GLOSSARY.md](99_GLOSSARY.md) (용어집), [17_CURVE_STORY_ATLAS.md](17_CURVE_STORY_ATLAS.md) (곡선 52종 내러티브)

---

## 1. 동기 & 목표

### 현재 상태
- 기본 적 5종 (Bit, Node, Vector, Loop, Matrix) + 변형 4종 + 포메이션 5종
- 실질 조합: ~25종. Stage 16+ Endless에서 반복 체감

### 목표
- **적 100종+** 체감 가능한 다양성
- 스테이지 진행에 따라 **새로운 CS (Computer Science) 개념 기반 적**이 등장하는 진화 시스템
- 수작업 프리셋(1~15) + 변이 엔진(16+) 하이브리드

### 핵심 질문
> "스테이지가 올라갈수록 플레이어가 '이건 또 뭐지?'를 계속 경험할 수 있는가?"

---

## 2. 설계 철학: CS 진화론 (CS Evolution)

### 2.1 가치관 검증

> **판단 기준** (copilot-instructions 순서)
> 1. **재미 (Fun)** — 플레이어가 즐거운가?
> 2. **쾌감 (Juice)** — 시각·촉각 피드백이 기분 좋은가?
> 3. **제약의 미학 (Constraint Beauty)** — 코드만으로 아름다움을 만드는가?
>
> 성능(60 FPS (Frames Per Second))은 필수 조건.

### 2.2 왜 "CS 진화론"인가

현재 5종이 잘 먹히는 이유를 분석하면:

| 적 | CS 개념 | 형태가 행동을 설명 | 만났을 때 |
|----|---------|-------------------|----------|
| **Bit** | 최소 데이터 단위 | 작은 점 → 당연히 약하고 많음 | "1비트니까 무리로 오겠구나" |
| **Node** | 데이터 노드 | 다이아몬드(연결점) → 제자리에서 쏨 | "노드는 움직이면 안 되지" |
| **Vector** | 방향+크기 | 화살표 → 돌진 | "벡터니까 빠르겠다" |
| **Loop** | 반복 구조 | 이중원 → 회전 | "루프니까 돌겠지" |
| **Matrix** | 행렬 | 육각형(격자) → 격자 탄막 | "매트릭스면 패턴이 복잡하겠다" |

**핵심: 이름 → 형태 → 행동이 일관된다.**

이 원칙을 확장하면: **CS 개념이 곧 적의 정체성**. 이름만 들어도 형태와 행동이 예측되고, 예측이 맞으면 쾌감, 예측이 깨지면 놀라움.

### 2.3 왜 "랜덤 DNA 조합"이 아닌가 — 설계 과정 회고

초기 DNA (Deoxyribonucleic Acid — 게임 내 적 유전자 조합 비유) 설계에서 Body × Movement × Attack × Modifier × OnDeath의 **독립 랜덤 조합 (81,000종)**을 검토했으나, 가치관 기준으로 기각:

| | 랜덤 조합 | CS 개념 일체형 (선택) |
|---|---|---|
| **재미** | "새 적!" (단 대부분 밋밋) | **"어? Tree가 분열한다?!"** (이름이 행동을 예고) |
| **쾌감** | 인지 혼란 (형태와 행동 무관) | **형태=행동 일치** → 직관적 학습 |
| **미학** | 수학 곡선 다양성 | CS 개념 × 수학 시각화 = **"교과서가 살아움직임"** |
| **스케일** | 81K이지만 대부분 잡음 | CS 프리셋 × 변이 = **5,400+ (전부 의미있음)** |

**결론: CS 개념이 정체성을 잡아주고, DNA 변이 엔진이 무한 확장을 담당.**

### 2.4 CS 적 진화 계보

데이터스피어 세계관에서 스웜(Swarm)은 점점 복잡한 CS 개념으로 "진화"한다:

```
Stage 1~5     기초 자료형 (Primitive)      Bit, Node, Vector, Loop, Matrix
                                            (현행 — 심플한 기하학)

Stage 6~10    자료 구조 (Data Structure)    Linked List, Tree, Hash, Queue, Stack
                                            (복합 기하학 — 체인, 분기, 순간이동)

Stage 11~15   알고리즘 (Algorithm)          Sort, Search, Encrypt, Compress, Parse
                                            (변형 기하학 — 움직이면서 모양이 바뀜)

Stage 16+     창발 현상 (Emergence)         Neural, Fractal, Cellular, Quantum
                                            (유기적 기하학 — 프랙탈, 셀 오토마타)
```

---

## 3. 2계층 구조: CS 프리셋 + 변이 엔진

### 3.1 구조 개요

```
레이어 1: CS 적 프리셋 (수작업)
  └ 이름, 형태, 핵심 행동, 탄막, 서사 — CS 개념이 전부를 결정 (일체형)
  └ Stage 1~15에서 순차 등장

레이어 2: DNA 변이 엔진 (자동)
  └ CS 프리셋을 "베이스"로 받아서 1~2개 유전자 교체
  └ 금지 조합 필터 (CS 정체성 파괴 방지)
  └ 시드 기반 결정적 생성
  └ Stage 16+ Endless에서 무한 변이

결과:
  Stage 1~15  → 레이어 1만 (수작업 퀄리티, 15종)
  Stage 16+   → 레이어 1 × 레이어 2 (15종 × 변이 = 5,400+종)
```

### 3.2 변이 엔진 — CS 정체성을 유지하는 자동 확장

각 CS 적은 **고정 유전자**와 **변이 가능 유전자**로 나뉨:

```lua
-- 예: Tree 적의 DNA 정의
Tree = {
    -- 고정 (CS 정체성 = 절대 안 바뀜)
    body = {  -- 레이어 배열: 아래→위 순서로 렌더
        { shape="diamond", mode="fill", scale=0.6, rot=0 },
        { shape="diamond", mode="line", scale=1.0, rot=45 },
    },
    onDeath  = "split",             -- 죽으면 분열 (트리의 본질)

    -- 변이 가능 (Endless에서 1~2개 교체)
    movement = "drift",             -- 기본은 drift, 변이로 chase/orbit 등
    attack   = "fractal_burst",     -- 기본은 분열탄, 변이로 spiral/aimed 등
    modifier = "none",              -- 기본은 없음, 변이로 armored/phasing 등
}

-- Stage 18 변이 예시:
-- "Armored Tree" = Tree + modifier:armored (두꺼운 외곽선, 느리지만 안 죽음)
-- "Chasing Tree" = Tree + movement:chase (쫓아오는 나무!)
-- "Helix Tree"   = Tree + attack:helix (이중 나선 탄막 트리)
```

### 3.3 Body 유전자 — 레이어 조합 시스템

> **핵심 아이디어**: 기존 도형 12종 × fill/line × scale × rotation을 **레이어 배열**로 조합.
> 수학 곡선 없이도 수백 가지 구별 가능한 외형이 자동 생성된다.
> 나중에 수학 곡선이나 수작업 외형으로 **교체/보완** 가능.

#### 도형 풀 (12종)

| # | ID | 시각 | 출처 |
|---|-----|------|------|
| 1 | `circle` | ● | 기존 |
| 2 | `diamond` | ◆ | 기존 |
| 3 | `arrow` | ▶ | 기존 |
| 4 | `spiral_ring` | ◎ | 기존 |
| 5 | `hexagon` | ⬡ | 기존 |
| 6 | `rectangle` | ■ | 기존 |
| 7 | `triangle` | ▲ | 신규 |
| 8 | `star` | ★ | 신규 |
| 9 | `cross` | ✚ | 신규 |
| 10 | `tear` | 💧 | 신규 |
| 11 | `bowtie` | ⏳ | 신규 |
| 12 | `gear` | ⚙ | 신규 |

#### 레이어 파라미터

```lua
-- 레이어 1개 = { shape, mode, scale, rot }
{
    shape = "diamond",   -- 12종 중 택 1
    mode  = "line",      -- "fill" / "line"
    scale = 1.0,         -- 0.3 ~ 1.5 (기본 반지름 대비)
    rot   = 45,          -- 회전 (도)
}
```

#### 조합 규칙

1. 레이어 1~3개 (Endless round에 따라 증가)
2. **fill 레이어는 최대 1개** (겹치면 안 보임)
3. fill 레이어가 있으면 **가장 아래** (첫 번째)
4. 바깥 레이어 scale > 안쪽 레이어 scale
5. 같은 shape 연속 시 mode 또는 rot 달라야 함

#### 조합 예시

| 레이어 | 결과 | 느낌 |
|--------|------|------|
| `[circle-fill-1.0]` | ● | 기존 bit |
| `[diamond-fill-0.6-45°] + [diamond-line-1.0-0°]` | ◇◆ | 이중 다이아몬드 |
| `[hexagon-fill-0.5] + [hexagon-line-1.0-30°]` | ✡ | 다비드의 별 |
| `[circle-fill-0.7] + [hexagon-line-1.2]` | ●⬡ | 코어+외피 |
| `[triangle-fill-0.6] + [triangle-line-1.0-180°]` | ✡ | 겹친 삼각형 |
| `[star-fill-0.8] + [circle-line-1.2]` | ★○ | 별+보호막 |
| `[gear-line-1.0] + [circle-fill-0.4]` | ⚙● | 기계 코어 |
| `[arrow-fill-0.8] + [circle-line-1.1] + [circle-line-0.4]` | ▶◎ | 조준 표적 |

#### 렌더링 (런타임)

```lua
-- renderable.type이 table이면 레이어 렌더
-- string이면 기존 경로 (하위 호환 100%)
for _, layer in ipairs(body) do
    drawShape(layer.shape, x, y, r * layer.scale, layer.mode, layer.rot)
end
```

> **확장 경로**: 도형 풀에 수학 곡선(`astroid`, `deltoid` 등)을 추가하거나,
> CS 적의 body를 수작업 레이어 조합으로 교체하면 된다.

### 3.4 스케일링 수치

| 항목 | 수치 |
|------|:----:|
| 도형 풀 (기존 6 + 신규 6) | 12 |
| Body 파라미터 (mode × scale × rot) | ×24+ |
| Body 레이어 (1~3개) | ×조합 |
| Movement 풀 | 12 |
| Attack 풀 | 15 |
| Modifier 풀 | 6 |
| OnDeath 풀 | 5 |
| **Stage 16+ 자동 생성 가능 조합** | **수만+** |
| 전부 의미있는 조합 | ✅ (CS 정체성이 기본 잡아줌) |

---

## 4. CS 적 15종 상세 설계

### 4.1 기초 자료형 (Stage 1~5) — 구현 완료

| # | ID | CS 개념 | 형태 | AI (Artificial Intelligence) 행동 | 탄막 | 특수 능력 |
|---|-----|---------|------|---------|------|----------|
| 1 | `bit` | 최소 데이터 | ● 작은 원 | swarm (돌진) | 없음 (접촉) | 군집 분리 |
| 2 | `node` | 데이터 노드 | ◆ 다이아몬드 | stationary (고정) | ring_pulse | — |
| 3 | `vector` | 방향+크기 | ▶ 화살표 | charge (돌진) | aimed | — |
| 4 | `loop` | 반복 구조 | ◎ 이중원 | orbit (공전) | spiral | — |
| 5 | `matrix` | 행렬 | ⬡ 육각형 | drift (이동) | cross (격자) | — |

### 4.2 자료 구조 (Stage 6~10) — 1차 구현 대상

| # | ID | CS 개념 | 형태 | 수학적 시각화 | AI 행동 | 탄막 | 특수 능력 | 킬링 포인트 |
|---|-----|---------|------|-------------|---------|------|----------|------------|
| 6 | `linked_list` | 연결 리스트 | 노드 체인 (●─●─●─●) | 선분 연결 다관절 | drift (머리가 이끔) | aimed (머리만) | **체인 절단 → 두 리스트로 분리** | "어디를 끊을까?" 전략 |
| 7 | `tree` | 이진 트리 | 프랙탈 Y분기 | Fractal branching | drift | fractal_burst | **죽으면 2개 하위 트리 분열** | "잡을수록 늘어난다" 딜레마 |
| 8 | `hash` | 해시 테이블 | 점선 격자 + 키 심볼 | Voronoi scatter | **teleport** 사이클 | circle | **순간이동 (경고→이동→경직)** | "리듬에 맞춰 잡는 적" |
| 9 | `queue` | 큐 (FIFO (First In, First Out)) | 점선 줄 (→→→) | Linear chain | 한 줄 진입 | wave | **한 줄로 순서대로 진입, 먼저 온 놈부터** | "줄 서서 오는 위협" |
| 10 | `stack_enemy` | 스택 (LIFO (Last In, First Out)) | 세로 적층 (□□□) | Stacked blocks | stationary | ring_pulse (층별) | **위에서부터만 제거 가능** | "꼭대기부터 깎아라" |

#### Hash 적 밸런스 설계 (별도 주의)

순간이동은 **"예측 불가 + 피격 불가"가 동시**에 오면 짜증 유발. 양쪽 모두 해소:

```
[정지 3초: 탄막 발사] → [경고 0.5초: 깜빡임+수축] → [순간이동]
        ↑                                                    ↓
        └── [착지 0.5초: 팽창+무방비, 탄막 안 쏨] ←──────────┘
```

| 제약 | 효과 |
|------|------|
| 이동 전 경고 0.5s (깜빡임) | "지금 쏴!" 윈도우 |
| 착지 후 경직 0.5s | 도착 예측 사격 가능 |
| 착지 위치 ✕ 마커 (이동 전 표시) | 플레이어 근처 불가 (최소 거리 2.0) |
| 이동 간격 고정 3s | 예측 가능한 리듬 |
| 탄막은 정지 중에만 | 이동 중/경직 중 발사 안 함 |
| HP (Hit Points) 보통~낮음 | "안 맞는데 안 죽어" 더블 짜증 방지 |

### 4.3 알고리즘 (Stage 11~15) — 후순위

| # | ID | CS 개념 | 형태 | 수학적 시각화 | 킬링 포인트 (구상) |
|---|-----|---------|------|-------------|-------------------|
| 11 | `sort` | 정렬 | 높이 다른 막대들 | Bar chart swap | 서로 위치 교환하며 이동 |
| 12 | `search` | 탐색 | DFS (Depth-First Search)/BFS (Breadth-First Search) 경로선 | Graph traversal | 플레이어 향해 경로 탐색 시각화 |
| 13 | `encrypt` | 암호화 | 다중 회전 링 | Enigma rotor | 탄막 패턴이 주기적 변환 |
| 14 | `compress` | 압축 | 축소↔팽창 | RLE (Run-Length Encoding) 시각화 | 작아지면 빨라지고, 커지면 탄막 발사 |
| 15 | `parse` | 구문 분석 | 괄호 트리 `{[()]}` | AST (Abstract Syntax Tree) | 중첩 구조 — 바깥 껍질부터 제거 |

> **상세 설계는 4.2 자료 구조 구현 후 진행.**

### 4.4 Linked List 구현: 히스토리 버퍼 방식 (다관절)

Unity 스타일 parent-child Transform 계층은 우리 게임에 과함:
- 적 크기 0.15 = 14px. 관절마다 로컬 좌표계를 갖는 건 안 보임
- 우리 ECS (Entity Component System)는 플랫 구조. 계층 추가 = 아키텍처 대수술

대신 **위치 히스토리 원형 버퍼 (circular buffer)**:

```lua
-- Entity 1개만 ECS에 등록 (머리)
-- 매 프레임 위치 기록 → 세그먼트는 렌더링 + 충돌 전용
local history = {}  -- [1]=현재, [2]=1프레임전, [3]=2프레임전...
-- 세그먼트 N개 = history[1 + i*간격] 위치에 렌더

-- 피격 판정: 세그먼트 위치 배열 순회 (원형 히트박스)
-- 절단: 피격 지점 인덱스 기준으로 히스토리 분할 → 새 Entity 생성
```

장점:
- Entity 1개로 해결 (ECS 부하 없음)
- 히트 판정은 세그먼트 위치 배열 순회 (간단)
- 절단 시 분할 지점부터의 히스토리를 새 Entity에 복사
- **성능**: N 세그먼트 = N번 circle-circle 판정. Bit 군집보다 가벼움

---

## 5. DNA 유전자 풀 레퍼런스

> CS 적의 변이(Stage 16+)에 사용되는 유전자 후보 목록.
> 각 CS 적은 고정 유전자 + 변이 가능 슬롯으로 구성됨 (§3.2 참조).

### 5.1 Movement (이동 패턴) — 12종 (6종 구현, 6종 미구현)

#### 기존 6종 (✅ `dnaDefs.MOVEMENT_POOL`에 등록 + `enemyAISystem` 구현)

| # | ID | 설명 | 난이도 |
|---|-----|------|:------:|
| 1 | `drift` | 고정 방향 이동 | ★ |
| 2 | `swarm` | 플레이어 돌진 (정지 안함) | ★ |
| 3 | `chase` | 플레이어 추적 (근접 시 정지) | ★★ |
| 4 | `orbit` | 플레이어 주위 원형 궤도 | ★★ |
| 5 | `stationary` | 제자리 고정 + 회전 | ★ |
| 6 | `charge` | 경고 → 고속 돌진 | ★★★ |

#### 신규 6종 (후순위 — CS 특수 적 또는 6C/6D에서 구현 예정)

| # | ID | 설명 | 수학적 배경 | 난이도 |
|---|-----|------|------------|:------:|
| 7 | `figure8` | 8자 궤적 | Lissajous(1:2) 경로 | ★★ |
| 8 | `pendulum` | 진자 운동 | 감쇠 진동 | ★★ |
| 9 | `levy` | 짧은 이동 → 갑자기 긴 도약 | Lévy flight | ★★★ |
| 10 | `mirror` | 플레이어 대칭 위치 복사 | 월드 중심 반전 | ★★★ |
| 11 | `gravity` | 타원 궤도 (플레이어=중력원) | 케플러 운동 | ★★★ |
| 12 | `bounce` | 벽 반사 (당구공) | 반사각 = 입사각 | ★★ |

### 5.2 Attack (탄막 패턴) — 15종 (9종 구현, 6종 미구현)

#### 기존 9종 (✅ `dnaDefs.ATTACK_POOL`에 등록 + `bulletEmitterSystem` 구현)

| # | ID | 설명 | 난이도 |
|---|-----|------|:------:|
| 0 | `none` | 탄막 없음 (접촉형) | ★ |
| 1 | `circle` | 등간격 원형 | ★ |
| 2 | `aimed` | 플레이어 조준 | ★★ |
| 3 | `spiral` | 회전 나선 | ★★ |
| 4 | `cross` | 십자 → 대각 교대 | ★★ |
| 5 | `ring_pulse` | 맥동 속도 원형 | ★★ |
| 6 | `wave` | 사인파 하강 | ★★ |
| 7 | `orbit_shot` | 공전 궤도 탄 | ★★★ |
| 8 | `return_shot` | 부메랑 탄 | ★★★ |

#### 신규 6종 (후순위 — 6C에서 구현 예정)

| # | ID | 설명 | 수학적 배경 | 난이도 |
|---|-----|------|------------|:------:|
| 9 | `fibonacci` | 황금각(137.5°) 간격 발사 | Fibonacci phyllotaxis | ★★ |
| 10 | `helix` | 이중 나선 | Double helix | ★★★ |
| 11 | `fractal_burst` | 일정 거리 후 3갈래 분열 (1세대) | Fractal branching | ★★★ |
| 12 | `freeze_ring` | 원형 → 정지 → 일제 추적 | Delayed homing | ★★★ |
| 13 | `pendulum_stream` | 진자 좌우 왕복 발사 | Pendulum sweep | ★★ |
| 14 | `gravity_well` | 고정 중력점에 의한 곡선 궤적 | Gravity simulation | ★★★ |

### 5.3 Modifier (방어 + Aura (패시브 범위 효과) 통합) — 6종 (4종 구현, 2종 미구현)

> 설계 원칙: "쾌감의 반대"가 되지 않도록 최소한만.
>
> **구현 완료 (4종)**: none, armored, shielded → `dnaDefs.MODIFIER_POOL` 등록 + `variantOverlays` 렌더
> swift는 변형 시스템으로 구현 (`entityFactory` + `stageData`)

| # | ID | 설명 | 비주얼 |
|---|-----|------|--------|
| 0 | `none` | 기본 (방어 없음) | — |
| 1 | `armored` | HP ×2.5, 크기 ×1.3, 속도 ×0.7 | 두꺼운 외곽선 |
| 2 | `shielded` | 전방 90° 피탄 무효 | 전방 아크 |
| 3 | `phasing` | 주기적 투명화 무적 (0.5초/3초) | 깜빡임+반투명 |
| 4 | `gravity_aura` | 주변 플레이어 탄을 휘게 함 | 왜곡 링 |
| 5 | `time_warp` | 주변 플레이어 이속/탄속 -30% | 시공간 파문 |

#### 삭제한 것과 이유

| 삭제 후보 | 이유 |
|----------|------|
| `regen` (HP 회복) | "때려도 차오름" = 불쾌. Armored가 이미 대체 |
| `dodge` (총알 회피) | "맞추려는데 피함" = 짜증. Movement가 이 역할 |
| `reflect` (반사) | 내 총알이 돌아옴 = 불공정 |
| `darkness` (시야 차단) | Zero-Art에서 **아름다운 도형을 못 보게 하는 건 가치관 위반** |

### 5.4 OnDeath (사망 반응) — 5종 (2종 구현, 3종 미구현)

> 설계 원칙: "잡았다!" 이후의 2차 이벤트. "죽여도 짜증"은 금지.
>
> **구현 완료 (2종)**: none, split → `dnaDefs.ONDEATH_POOL` 등록 + `entityFactory` Splitter 변형

| # | ID | 설명 | 비주얼 |
|---|-----|------|--------|
| 0 | `none` | 기본 사망 (파편만) | 파편 파티클 |
| 1 | `split` | 미니 2마리 분열 | 현재 Splitter 변형 |
| 2 | `explode` | 사망 시 원형 탄막 8발 | 적색 폭발 링 |
| 3 | `chain` | 가까운 적 1마리에 데미지 전이 | 전기 아크 (**플레이어에게 유리!**) |
| 4 | `blackhole` | 2초간 중력장 (주변 탄/적 흡입) | 축소 소용돌이 |

#### 삭제한 것과 이유

| 삭제 후보 | 이유 |
|----------|------|
| `revive` (부활) | "왜 또 살아?" = 최악의 불쾌감. **절대 안 됨** |
| `poison` (독 장판) | 지속 데미지 = 짜증. 탄막 게임은 순간 피격이 핵심 |
| `buff_allies` (적 버프) | 효과가 안 보여서 인지 불가 |

### 5.5 Body (외형) — 레이어 조합 + 수학 곡선 확장

> **✅ Phase 6B 구현 완료** (`0fa7b64`): 레이어 조합 시스템 + 52종 수학 곡선 + CurveLab/Gallery 씨.
> **Phase 6A 계획**: CS 특수 적 외형으로 특정 곡선 전용 지정.

#### 실제 구현 구조 (3-파일 분담)

**1. `curveDefs.lua`** — 52종 곡선 정의 (polar/parametric/custom 수식)
```lua
M.curves.rose_3 = { type = "polar", fn = function(t) return cos(3*t) end, name = "Rose 3-petal" }
M.curves.astroid = { type = "parametric", fx = function(t) return cos(t)^3 end, fy = ... }
```

**2. `shapeDefs.lua`** — 곡선 정규화 메타데이터 (그룹 분류 + 단위원 스케일 + 바운드)
```lua
-- 곡선을 enemy/boss/bullet/overlay 그룹으로 분류
-- 각 곡선에 대해 사전 계산: centerOffset, maxRadius, scaleToUnitRadius, bounds
M.normalized.rose_3 = {
    centerOffset = {x=0, y=0},
    maxRadius = 1.0,
    scaleToUnitRadius = 1.0,
    bounds = {minX=-1, maxX=1, minY=-1, maxY=1}
}
```
- 그룹: enemy(25), boss(3), both(5), overlay(5), bullet(10), excluded(4)
- API: `isUsable(name)`, `getNormalization(name)`

**3. `renderSystem.lua`** — 실시간 곡선 샘플링 + 렌더링
```lua
-- _sampleCurveWorld(curveName, cx, cy, radius, segments)
-- → curveDefs에서 수식 읽기 → 정규화 메타 적용 → 월드 좌표 정점 배열 반환
-- _drawCurveOverlay() 로 오버레이 레이어 렌더링
```

> **설계 변경점**: 원래 계획은 "사전 계산 정점 배열"이었으나, 실제로는 **수식 기반 실시간 샘플링** 방식으로 구현.
> 장점: 곡선 추가 시 curveDefs에 수식만 등록하면 됨. 정점 수(segments)를 런타임에 조절 가능.
> 비용: 프레임당 sin/cos 호출 있으나, 캐시 및 세그먼트 수 제어로 60fps 유지.

#### 곡선 수식 레퍼런스 (curveDefs.lua에 등록된 52종 중 주요 곡선)

##### 기존 5종 (구현 완료 — basicShapes.lua)

| # | ID | 형상 | 렌더 방식 | CS 적 |
|---|-----|------|----------|-------|
| 1 | `circle` | ● | `lg.circle()` | Bit |
| 2 | `diamond` | ◆ | 정점 4개 정사각형 45° 회전 | Node |
| 3 | `arrow` | ▶ | 삼각형 + 꼬리선 | Vector |
| 4 | `spiral_ring` | ◎ | `lg.circle()` × 2 | Loop |
| 5 | `hexagon` | ⬡ | 정점 6개 정육각형 | Matrix |

##### 신규 곡선 — Tier 1: 닫힌 곡선 (정점 배열 직접 생성)

**① Reuleaux Triangle (뢸로 삼각형)**

```
     ╱╲
    ╱  ╲       정삼각형의 각 꼭짓점을 중심으로
   (    )      반대편 변 길이를 반지름으로 호를 그림
    ╲  ╱       → "맨홀 뚜껑" 형태 (정폭도형)
     ╲╱
```

- 수식: 정삼각형 꼭짓점 A,B,C 각각에서 `arc(center=A, r=|BC|, from=B, to=C)`
- 정점: 호 3개 × 16점 = **48 정점**
- CS 적 매핑: **Encrypt** — 에니그마 로터의 기어 느낌. 회전하면 곡선이 살아있는 듯
- 생성 코드:
  ```lua
  for i = 0, 2 do
      local cx, cy = cos(i*2π/3), sin(i*2π/3)  -- 꼭짓점
      for t = 0, 15 do
          local angle = (i*2π/3 + π) + (t/15) * (2π/3)  -- 120° 호
          verts[#verts+1] = cx + R * cos(angle)
          verts[#verts+1] = cy + R * sin(angle)
      end
  end
  ```

**② Astroid (별 곡선 — 4첨성)**

```
      *
    ·   ·
   ·     ·      안쪽으로 오목한 4꼭지 별.
    ·   ·       일반 별과 달리 꼭짓점이 뾰족하고
      *         변이 안으로 휜다.
```

- 수식: `x = cos³(θ)`, `y = sin³(θ)`
- 정점: θ를 0~2π로 **32 등분 = 32 정점**
- CS 적 매핑: **Compress** — 수축/팽창하는 별. 안쪽으로 쪼그라든 느낌 = 압축
- 생성 코드:
  ```lua
  for i = 0, 31 do
      local t = i / 32 * 2 * math.pi
      verts[#verts+1] = math.cos(t)^3
      verts[#verts+1] = math.sin(t)^3
  end
  ```

**③ Deltoid (삼각 곡선 — 3첨성)**

```
      △
     ╱ ╲        astroid의 3꼭지 버전.
    ╱   ╲       변이 안쪽으로 살짝 휜
    ‾‾‾‾‾       부드러운 삼각형.
```

- 수식: `x = 2cos(θ)+cos(2θ)`, `y = 2sin(θ)-sin(2θ)` (hypocycloid k=3)
- 정점: **36 정점**
- CS 적 매핑: **Tree** — 3갈래 분기의 원형. 부드러운 삼각 = 유기적 나무 느낌
- 생성 코드:
  ```lua
  for i = 0, 35 do
      local t = i / 36 * 2 * math.pi
      verts[#verts+1] = 2*math.cos(t) + math.cos(2*t)
      verts[#verts+1] = 2*math.sin(t) - math.sin(2*t)
  end
  -- 반지름 3 기준이므로 1/3로 정규화
  ```

**④ Cardioid (심장 곡선)**

```
      ♡
     ╱ ╲        위가 오목하고 아래가 뾰족한 물방울.
    (   )       "방향"이 있는 곡선 → 이동 방향 표시에 적합.
     ╲ ╱
      V
```

- 수식: `r = 1 + cos(θ)` (극좌표)
- 정점: **40 정점**
- CS 적 매핑: **Search** — "어디로 갈까?" 탐색하는 적. 뾰족한 끝이 이동 방향을 가리킴
- 생성 코드:
  ```lua
  for i = 0, 39 do
      local t = i / 40 * 2 * math.pi
      local r = 1 + math.cos(t)
      verts[#verts+1] = r * math.cos(t)
      verts[#verts+1] = r * math.sin(t)
  end
  -- 반지름 2 기준이므로 1/2로 정규화
  ```

**⑤ Rose Curve (꽃잎 곡선)**

```
    ·  *  ·
   · * * * ·     n=3이면 3잎, n=5이면 5잎.
    ·  *  ·      n에 따라 무한 변형 가능.
```

- 수식: `r = cos(nθ)` (n=3이면 3잎, n=5이면 5잎)
- 정점: **48~60 정점** (잎 수에 비례)
- CS 적 매핑: **Neural** — 뉴런의 수상돌기 느낌. 잎 수로 연결 노드 수 표현
- 생성 코드:
  ```lua
  local n = 3  -- 잎 수
  for i = 0, 47 do
      local t = i / 48 * 2 * math.pi
      local r = math.cos(n * t)
      verts[#verts+1] = r * math.cos(t)
      verts[#verts+1] = r * math.sin(t)
  end
  ```

**⑥ Superellipse (둥근 사각 — Lamé curve)**

```
    ╭──────╮
    │      │     n=2이면 원, n=4이면 둥근 사각,
    │      │     n→∞이면 정사각형.
    ╰──────╯     n으로 "디지털 느낌" 조절 가능.
```

- 수식: `|x|ⁿ + |y|ⁿ = 1` (n=4 추천)
- 정점: **32 정점**
- CS 적 매핑: **Stack** — 적층 블록의 개별 층. 각진 직사각형보다 살짝 부드러움
- 생성 코드:
  ```lua
  local n = 4
  for i = 0, 31 do
      local t = i / 32 * 2 * math.pi
      local c, s = math.cos(t), math.sin(t)
      local sign_c = c >= 0 and 1 or -1
      local sign_s = s >= 0 and 1 or -1
      verts[#verts+1] = sign_c * math.abs(c)^(2/n)
      verts[#verts+1] = sign_s * math.abs(s)^(2/n)
  end
  ```

**⑦ Lemniscate (∞ 곡선 — 무한대)**

```
     ╲    ╱
      ╳──╳       무한대 기호. 중심에서 교차.
     ╱    ╲      "끝없는 반복"을 시각적으로 표현.
```

- 수식: `r² = cos(2θ)` (극좌표, θ에 따라 r이 허수가 되는 구간은 건너뜀)
- 정점: **40 정점** (두 루프 각각 20점)
- CS 적 매핑: **Loop** 변이 후보 — 현재 이중원보다 더 "무한 반복" 느낌
- 생성 코드:
  ```lua
  -- 상반부 루프 (0 ~ π/4, 7π/4 ~ 2π)
  -- 하반부 루프 (3π/4 ~ 5π/4)
  for i = 0, 19 do
      local t = -math.pi/4 + i/20 * math.pi/2
      local r = math.sqrt(math.cos(2*t))
      verts[#verts+1] = r * math.cos(t)
      verts[#verts+1] = r * math.sin(t)
  end
  -- 하반부도 동일하게 반전
  ```

**⑧ Vesica Piscis (렌즈/눈)**

```
      ╱╲
     (  )        두 원의 교집합 = 렌즈 형태.
      ╲╱         "감시하는 눈" — 알고리즘이 탐색하는 느낌.
```

- 수식: 두 원(중심 거리 = 반지름)의 교차 영역
- 정점: 호 2개 × 20점 = **40 정점**
- CS 적 매핑: **Search** 대안 — DFS/BFS의 "탐색 시야"

**⑨ Koch Edge (코흐 눈송이 가지)**

```
     /\           선분 → 삼각 돌기 → 재귀.
    /  \          재귀 2단계면 13 정점.
   /    \         Tree 적의 가지 형태로 직접 사용.
  ──    ──
```

- 수식: 선분을 3등분 → 중간 1/3을 정삼각형 돌기로 치환 (재귀)
- 정점: **재귀 1단계 = 5정점, 2단계 = 17정점**
- CS 적 매핑: **Tree** 가지 세그먼트 — Linked List 세그먼트 외형으로도 활용

**⑩ Epicycloid (외전원 곡선)**

```
     * * *
    *     *      원이 다른 원 바깥을 굴러가는 궤적.
     * * *       k=3이면 3잎 꽃, k=5이면 5잎.
```

- 수식: `x=(R+r)cos(θ)-r·cos((R+r)θ/r)`, `y=(R+r)sin(θ)-r·sin((R+r)θ/r)`
- 정점: **48 정점**
- CS 적 매핑: **Encrypt** — 에니그마 로터 기어. 동심 회전 링의 궤적
- 탄막 궤적으로도 활용: 탄이 epicycloid 경로를 따라 이동

##### 신규 곡선 — Tier 2: 탄막/이동 궤적 전용

| ID | 수식 | 적용 | 비고 |
|-----|------|------|------|
| `lissajous` | `x=sin(at), y=sin(bt+δ)` | figure8 이동 패턴 | a:b 비율로 궤적 변형 |
| `fibonacci_spiral` | `θ += 137.5°, r += step` | fibonacci 탄막 | 황금각 발사 |
| `clifford_attractor` | `xₙ₊₁=sin(ayₙ)+c·cos(axₙ)` | Endless 배경 파티클 | 사전 계산 → 경로 배열 |
| `l_system` | 재귀 문자열 치환 | Tree 분기 경로 | `F→F[+F][-F]` 형태 |

##### CS 적 ↔ 곡선 최종 매핑

| CS 적 | 기본 외형 | 곡선 근거 | 왜 이 곡선인가 |
|-------|----------|----------|---------------|
| Linked List | 선분 연결 체인 | 직선 + circle 노드 | 연결 리스트 = 노드와 포인터(선) |
| Tree | **Deltoid** (몸통) + Koch (가지) | 3갈래 분기 곡선 | 이진 트리의 프랙탈적 분기 구조 |
| Hash | 점선 격자 + **Reuleaux** | 기어 모양 | 해시 함수의 "돌리면 뭐가 나올지 모르는" 느낌 |
| Queue | 점선 줄 + **Vesica** (개별 노드) | 렌즈 형태 | 대기열의 "순서를 기다리는 눈" |
| Stack | **Superellipse** (적층 블록) | 둥근 사각 | 데이터 블록이 쌓인 형태 |
| Sort | **Astroid** (막대 끝) | 오목별 | 교환 시 별처럼 반짝이는 연출 |
| Search | **Cardioid** | 방향 있는 물방울 | 탐색 방향을 뾰족한 끝으로 표시 |
| Encrypt | **Epicycloid** + Reuleaux | 기어/로터 | 에니그마 암호기의 회전 기어 |
| Compress | **Astroid** (수축/팽창) | 오목별 스케일 변화 | 압축 = 줄어드는 느낌 |
| Parse | Koch 중첩 괄호 | 재귀 구조 | `{[()]}` 중첩 = 프랙탈 |

> **구현 순서**: Phase 6A.0에서 `shapeDefs.lua` 생성기를 먼저 만들고,
> 각 CS 적 구현 시 해당 곡선을 등록하는 방식으로 점진 확장.

---

## 6. "컴퓨터 과학 교과서가 공격해 온다"

이 시스템의 가치를 증명하는, **다른 게임에서 본 적 없는** 장면들:

| CS 적 | 장면 | 플레이어 반응 |
|-------|------|-------------|
| **Linked List** | 체인이 화면을 가로지르며 진입. 중간을 끊자 두 뱀이 흩어진다 | "어디를 끊어야 효율적이지?" |
| **Tree** | 프랙탈 나무가 죽으며 가지가 두 갈래로 분열. 분열체도 분열 | "잡을수록 늘어나... 빨리 처리해야!" |
| **Hash** | 적이 깜빡이더니 사라지고, 화면 반대편에 ✕ 마커와 함께 재출현 | "3초 리듬... 지금이다!" |
| **Queue** | 한 줄로 줄 서서 등장. FIFO — 먼저 온 놈부터 처리 | "줄이 끝이 없어..." |
| **Stack** | 블록이 세로로 쌓여있다. 위에서부터만 깎을 수 있다 | "꼭대기부터 하나씩!" |
| **Sort** (미래) | 막대들이 서로 위치를 교환하며 움직인다 | "뭐야 이 패턴..." |
| **Encrypt** (미래) | 동심 링이 각자 다른 속도로 회전하며 탄막 패턴이 변한다 | "에니그마...?" |

---

## 7. 구현 계획

> **구현 순서: 6B → 6A → 6C → 6D**
> DNA 변이 엔진(6B)으로 외형 다양성을 먼저 확보한 뒤,
> CS 특수 적(6A)의 고유 메카닉을 점진 추가.

### Phase 6B: DNA 변이 엔진 (Stage 16+ Endless) — ✅ 구현 완료 (`0fa7b64`)

> **전략**: 기존 Stage 1~15 코드는 일절 안 건드림. Stage 16+에서만 DNA 엔진 가동.
> Body 유전자 = 기존 도형 12종의 레이어 조합으로 외형 다양성 확보.
> 나중에 수학 곡선이나 CS 특수 적 외형으로 교체/보완 가능.

| # | 작업 | 내용 | 파일 | 상태 |
|---|------|------|------|:----:|
| 6B.0 | DNA 구조 + 유전자 풀 | 5개 유전자 풀, 기존 적 5종 프리셋, Body 레이어 풀, 조합 규칙, 금지 필터 | `dnaDefs.lua` | ✅ |
| 6B.1 | Body 레이어 렌더러 + 신규 도형 6종 | `renderSystem.lua` 레이어 배열 지원 + `basicShapes.lua` 도형 6종 추가 | `renderSystem.lua`, `basicShapes.lua` | ✅ |
| 6B.2 | 변이 엔진 | 시드 기반 결정적 DNA 생성 — body 레이어 자동 조합 + 4유전자 선택 + 스탯 자동 산출 | `dnaDefs.lua` | ✅ |
| 6B.3 | 스폰 연동 | `entityFactory.createDnaEnemy(dna)` 새 경로 + `stageManager` Stage 16+에서 호출 | `entityFactory.lua`, `stageManager.lua` | ✅ |
| 6B.4 | 테스트 + 튜닝 | F8 스킵으로 Stage 16~20 검증, 외형 구별 + 밸런스 | 미세 조정 | ✅ |

> **추가 구현** (문서 작성 시 미계획):
> - `curveDefs.lua` — 52종 수학 곡선 (polar/parametric/custom)
> - `shapeDefs.lua` — 곡선 정규화 메타 + 그룹 분류 (enemy/boss/bullet/overlay)
> - `curveLabScene.lua` — CurveLab 실시간 곡선 탐험 씨
> - `galleryScene.lua` — Gallery 곡선 도감 씨

### Phase 6A: CS 적 — 자료 구조 (6B 이후) — 계획 중

> 6B의 DNA 인프라 위에 CS 특수 적의 **고유 메카닉** 추가.
> Body는 수학 곡선(`curveDefs.lua`)으로 교체/보완.

| # | 작업 | 내용 | 상태 |
|---|------|------|:----:|
| 6A.0 | 인프라 | shapeDefs + curveDefs 정규화, CurveLab/Gallery 씨 | ✅ `0fa7b64` |
| 6A.1 | **Deity System** | 신(deity) 선택 → 곡선별 패시브 능력 부여 | 📋 설계 중 |
| 6A.2 | **Linked List** | 체인 렌더링 + 절단 메카닉, Stage 6~7 등장 | ⏳ |
| 6A.3 | **Tree** | 프랙탈 분기 렌더 + 분열 메카닉, Stage 8~9 등장 | ⏳ |
| 6A.4 | **Hash** | 순간이동 사이클 + 착지마커, Stage 10~11 등장 | ⏳ |

### Phase 6C: CS 적 — 알고리즘 (후순위)

| # | 작업 | 내용 |
|---|------|------|
| 6C.1~5 | Sort, Search, Encrypt, Compress, Parse | §4.3 상세 설계 후 구현 |

### Phase 6D: Modifier + OnDeath (후순위)

| # | 작업 | 내용 |
|---|------|------|
| 6D.1 | Modifier 신규 3종 | phasing, gravity_aura, time_warp |
| 6D.2 | OnDeath 신규 3종 | explode, chain, blackhole |

---

## 8. 미결정 사항

| # | 질문 | 관련 | 우선순위 | 상태 |
|---|------|------|:--------:|:----:|
| 1 | Body 레이어 풀 — 14px에서 구별 안 되는 도형 조합은? | 6B.1 | 높 | ✅ Gallery 테스트로 검증 완료 |
| 2 | Body sides 범위 3~8? 3~12? (원과 구별 한계) | 6B.0 | 높 | ⏳ |
| 3 | 동시 변이 유전자 수 — Round 1: 1개, Round 3+: 2개? | 6B.2 | 높 | ⏳ |
| 4 | 금지 조합 목록? (shielded + stationary 등) | 6B.2 | 높 | ✅ 3쌍 구현: `isForbidden()` |
| 5 | 스탯 자동 산출 공식? (트레이드오프) | 6B.2 | 높 | ✅ `calcStats()` 구현 완료 |
| 6 | Endless 라운드별 레이어 수 증가? (R1: 1~2, R3+: 2~3) | 6B.2 | 중 | ✅ `generateBody(layerCount)` 라운드 연동 |
| 7 | Linked List 세그먼트 수? (5? 8? 가변?) | 6A.2 | 중 | ⏳ |
| 8 | Tree 분열 세대 제한? (2세대까지? 3세대?) | 6A.3 | 중 | ⏳ |
| 9 | Hash 착지 최소 거리 2.0이 적절한가? | 6A.4 | 낮 | ⏳ |
| 10 | 변이 시 이름 생성 규칙? ("Armored Tree" 등) | 6B | 낮 | ⏳ |

---

## 변경 이력

| 날짜 | 내용 |
|------|------|
| 2026-04-21 | 초안 작성: 논의 과정 + 8유전자 → 5유전자 축소 + 신선도 검토 |
| 2026-04-22 | **대규모 재설계**: DNA 랜덤 조합 → CS 기반 일체형 + 변이 하이브리드. 가치관 재검증. 1차 구현 대상 3종(Linked List, Tree, Hash) 확정. Linked List 히스토리 버퍼 구현 방식 확정. Hash 밸런스 설계. 용어집([99_GLOSSARY.md](99_GLOSSARY.md)) 신설 |
| 2026-04-24 | **문서 정리**: Phase 6B 구현 완료 반영 (`0fa7b64`). 구 초안(4/21) 삭제. §5 유전자 풀 구현 상태 마킹. §5.5 shapeDefs 실제 구현 구조(curveDefs + 정규화 메타 + 실시간 샘플링)로 갱신. §7 커밋 해시 + 상태 열 추가. §8 해결된 미결정 4건 체크. CurveLab/Gallery/52곡선 반영 |
