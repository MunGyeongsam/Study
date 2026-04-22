# CS (Computer Science) 기반 적 DNA 시스템 설계

> 작성일: 2026-04-21
> 최종 수정: 2026-04-22
> 상태: **설계 확정** — Phase 6A~6B 구현 대기
> 관련 문서: [09_ENEMY_DIVERSITY_DESIGN.md](09_ENEMY_DIVERSITY_DESIGN.md) (현행 적 시스템), [99_GLOSSARY.md](99_GLOSSARY.md) (용어집)

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
    body     = "fractal_branch",    -- 프랙탈 분기 외형
    onDeath  = "split",             -- 죽으면 분열 (트리의 본질)

    -- 변이 가능 (Endless에서 1~2개 교체)
    movement = "drift",             -- 기본은 drift, 변이로 chase/orbit 등
    attack   = "fractal_burst",     -- 기본은 분열탄, 변이로 spiral/aimed 등
    modifier = "none",              -- 기본은 없음, 변이로 armored/phasing 등
}

-- Stage 18 변이 예시:
-- "Armored Tree" = Tree + modifier:armored (두꺼운 가지, 느리지만 안 죽음)
-- "Chasing Tree" = Tree + movement:chase (쫓아오는 나무!)
-- "Helix Tree"   = Tree + attack:helix (이중 나선 탄막 트리)
```

### 3.3 스케일링 수치

| 항목 | 수치 |
|------|:----:|
| CS 적 프리셋 (수작업) | 15 |
| 변이 슬롯 (Movement, Attack, Modifier) | 3 |
| 각 슬롯의 선택지 | 6~15 |
| 동시 변이 수 (1~2개) | ×2 |
| **Stage 16+ 자동 생성 가능 조합** | **~5,400+** |
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

### 5.1 Movement (이동 패턴) — 12종

#### 기존 6종 (구현 완료)

| # | ID | 설명 | 난이도 |
|---|-----|------|:------:|
| 1 | `drift` | 고정 방향 이동 | ★ |
| 2 | `swarm` | 플레이어 돌진 (정지 안함) | ★ |
| 3 | `chase` | 플레이어 추적 (근접 시 정지) | ★★ |
| 4 | `orbit` | 플레이어 주위 원형 궤도 | ★★ |
| 5 | `stationary` | 제자리 고정 + 회전 | ★ |
| 6 | `charge` | 경고 → 고속 돌진 | ★★★ |

#### 신규 6종 (변이 전용 또는 신규 CS 적용)

| # | ID | 설명 | 수학적 배경 | 난이도 |
|---|-----|------|------------|:------:|
| 7 | `figure8` | 8자 궤적 | Lissajous(1:2) 경로 | ★★ |
| 8 | `pendulum` | 진자 운동 | 감쇠 진동 | ★★ |
| 9 | `levy` | 짧은 이동 → 갑자기 긴 도약 | Lévy flight | ★★★ |
| 10 | `mirror` | 플레이어 대칭 위치 복사 | 월드 중심 반전 | ★★★ |
| 11 | `gravity` | 타원 궤도 (플레이어=중력원) | 케플러 운동 | ★★★ |
| 12 | `bounce` | 벽 반사 (당구공) | 반사각 = 입사각 | ★★ |

### 5.2 Attack (탄막 패턴) — 15종

#### 기존 9종 (구현 완료)

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

#### 신규 6종

| # | ID | 설명 | 수학적 배경 | 난이도 |
|---|-----|------|------------|:------:|
| 9 | `fibonacci` | 황금각(137.5°) 간격 발사 | Fibonacci phyllotaxis | ★★ |
| 10 | `helix` | 이중 나선 | Double helix | ★★★ |
| 11 | `fractal_burst` | 일정 거리 후 3갈래 분열 (1세대) | Fractal branching | ★★★ |
| 12 | `freeze_ring` | 원형 → 정지 → 일제 추적 | Delayed homing | ★★★ |
| 13 | `pendulum_stream` | 진자 좌우 왕복 발사 | Pendulum sweep | ★★ |
| 14 | `gravity_well` | 고정 중력점에 의한 곡선 궤적 | Gravity simulation | ★★★ |

### 5.3 Modifier (방어 + Aura (패시브 범위 효과) 통합) — 6종

> 설계 원칙: "쾌감의 반대"가 되지 않도록 최소한만.

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

### 5.4 OnDeath (사망 반응) — 5종

> 설계 원칙: "잡았다!" 이후의 2차 이벤트. "죽여도 짜증"은 금지.

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

### 5.5 Body (외형) — 사전 계산 수학 곡선 시스템

> **핵심 전략: 런타임 0 비용.**
> 모든 곡선을 `shapeDefs.lua`에 **사전 계산된 정점 배열**로 저장.
> 렌더링 시에는 `love.graphics.polygon("line", verts)` 한 줄로 그린다.
> 적 1000마리 × 60fps에서도 sin/cos 연산 제로.

#### 구현 구조

```lua
-- src/03_game/data/shapeDefs.lua (사전 계산 정점 테이블)
local M = {}

-- 단위 크기(반지름 1.0) 기준, {x1,y1, x2,y2, ...} 플랫 배열
-- 렌더 시: love.graphics.scale(radius) 적용 후 polygon()

M.reuleaux = { --[[ 48개 좌표 ]] }
M.astroid  = { --[[ 32개 좌표 ]] }
M.koch_branch = { --[[ 재귀 2단계 Y분기 ]] }
-- ...

return M
```

```lua
-- renderSystem에서 사용 (런타임 비용 = polygon draw 1회)
local verts = shapeDefs[renderable.shape]
love.graphics.push()
love.graphics.translate(sx, sy)
love.graphics.rotate(transform.angle)
love.graphics.scale(r, r)  -- r = radius × pixelsPerUnit
love.graphics.polygon("line", verts)
love.graphics.pop()
```

#### 정점 생성 공식 (shapeDefs.lua 빌드 시 1회 실행)

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

### Phase 6A: CS 적 — 자료 구조 (1차 구현)

| # | 작업 | 내용 |
|---|------|------|
| 6A.0 | 인프라 | `shapeDefs.lua` 사전 계산 정점 생성기, 히스토리 버퍼, 새 적 타입 등록 흐름 |
| 6A.1 | **Linked List** | 체인 렌더링 + 절단 메카닉, Stage 6~7 등장 |
| 6A.2 | **Tree** | 프랙탈 분기 렌더 + 분열 메카닉, Stage 8~9 등장 |
| 6A.3 | **Hash** | 순간이동 사이클 + 착지마커, Stage 10~11 등장 |

### Phase 6B: 변이 엔진 (Stage 16+ Endless)

| # | 작업 | 내용 |
|---|------|------|
| 6B.1 | DNA 구조 정의 | 고정 유전자 / 변이 가능 슬롯 분리 |
| 6B.2 | 변이 생성기 | 시드 기반 결정적 생성 + 금지 조합 필터 |
| 6B.3 | 스탯 자동 산출 | DNA → HP/Speed/Radius/XP (Experience Points) 변환 공식 |
| 6B.4 | stageManager 연동 | Stage 16+ 에서 변이 적 스폰 |

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

| # | 질문 | 관련 | 우선순위 |
|---|------|------|:--------:|
| 1 | Linked List 세그먼트 수? (5? 8? 가변?) | 6A.1 | 높 |
| 2 | Tree 분열 세대 제한? (2세대까지? 3세대?) | 6A.2 | 높 |
| 3 | Hash 착지 최소 거리 2.0이 적절한가? | 6A.3 | 중 |
| 4 | Queue/Stack은 1차 구현 범위에 포함? | 6A | 중 |
| 5 | 변이 시 이름 생성 규칙? ("Armored Tree" 등) | 6B | 낮 |
| 6 | 금지 조합 목록? (shielded + stationary 등) | 6B | 높 |
| 7 | 스탯 자동 산출 공식? (트레이드오프) | 6B | 높 |
| 8 | gravity_well → 고정 중력점 방식으로 변경? | Attack | 높 |
| 9 | 종 도감(카탈로그) 시스템? | 메타 성장 | 낮 |

---

## 변경 이력

| 날짜 | 내용 |
|------|------|
| 2026-04-21 | 초안 작성: 논의 과정 + 8유전자 → 5유전자 축소 + 신선도 검토 |
| 2026-04-22 | **대규모 재설계**: DNA 랜덤 조합 → CS 기반 일체형 + 변이 하이브리드. 가치관 재검증. 1차 구현 대상 3종(Linked List, Tree, Hash) 확정. Linked List 히스토리 버퍼 구현 방식 확정. Hash 밸런스 설계. 용어집([99_GLOSSARY.md](99_GLOSSARY.md)) 신설 |
# 절차적 적 DNA 시스템 설계 (Procedural Enemy DNA)

> 작성일: 2026-04-21
> 상태: **설계 논의 중** — 구현 전 숙성 단계
> 관련 문서: [09_ENEMY_DIVERSITY_DESIGN.md](09_ENEMY_DIVERSITY_DESIGN.md) (현행 적 시스템)

---

## 1. 동기 & 목표

### 현재 상태
- 기본 적 5종 (Bit, Node, Vector, Loop, Matrix) + 변형 4종 + 포메이션 5종
- 실질 조합: ~25종. Stage 16+ Endless에서 반복 체감

### 목표
- **적 100종+** 체감 가능한 다양성
- 스테이지 진행에 따라 **새로운 적이 자동 생성**되는 절차적 시스템
- 수작업 프리셋(1~15) + 절차적 생성(16+) 하이브리드

### 핵심 질문
> "스테이지가 올라갈수록 플레이어가 '이건 또 뭐지?'를 계속 경험할 수 있는가?"

---

## 2. 접근법 비교 (논의 과정)

| | A. 수작업 확장 | B. 완전 절차적 | C. 하이브리드 (선택) |
|---|---|---|---|
| **방식** | 적 프리셋 100개 직접 정의 | 스테이지 시드로 전부 자동 | trait 풀 수작업 + 조합 자동 |
| **다양성 체감** | 높음 (수작업 튜닝) | 중간 (랜덤 느낌 위험) | 높음 (의미있는 조합만) |
| **작업량** | 매우 큼 | 적음 | 중간 |
| **스케일링** | 100개 넘기 어려움 | 무한 | 무한 |
| **밸런스 제어** | 쉬움 | 어려움 | 적당 |

**선택: C안 (하이브리드)**
- 이유: 각 trait을 수작업으로 재미 검증 → 조합은 시드 기반 자동
- 같은 스테이지는 같은 적 (결정적 시드) → 학습 가능

---

## 3. 가치관 기반 유전자 검토

> **판단 기준** (copilot-instructions 순서)
> 1. **재미 (Fun)** — 플레이어가 즐거운가?
> 2. **쾌감 (Juice)** — 시각·촉각 피드백이 기분 좋은가?
> 3. **제약의 미학 (Constraint Beauty)** — 코드만으로 아름다움을 만드는가?
>
> 성능(60fps)은 필수 조건.

### 검토 결과 종합

| 유전자 후보 | 재미 | 쾌감 | 미학 | 성능 위험 | 판정 |
|------------|:----:|:----:|:----:|:---------:|------|
| **Body (외형)** | ★★★ | ★★★ | ★★★ | 없음 | **핵심 — 최우선** |
| **Movement (이동)** | ★★★ | ★★☆ | ★★☆ | 없음 | **핵심 — 최우선** |
| **Attack (탄막)** | ★★★ | ★★★ | ★★★ | ⚠️ bulletPool | **핵심 — 최우선** |
| **Defense (방어)** | ★★☆ | ★☆☆ | ★☆☆ | 없음 | **최소한만** |
| **OnDeath (사망)** | ★★★ | ★★☆ | ★★☆ | ⚠️ 엔티티 급증 | **선별 채택** |
| **Social (집단)** | ★★☆ | ★☆☆ | ★★☆ | ⚠️ N² 연산 | **삭제** (ROI 낮음) |
| **Aura (패시브)** | ★★☆ | ★☆☆ | ★★☆ | ⚠️ 매 프레임 | **1~2종만** |
| **Visual Flair** | ★☆☆ | ★★★ | ★★★ | 없음 | **Body에 통합** |

### 검토 상세: 왜 이렇게 판정했나

#### Defense가 "최소한만"인 이유
- 방어 = 내 공격이 안 먹힘 = **쾌감의 반대**
- "때렸는데 왜 안 죽어?" = 스트레스
- armored/shielded는 이미 검증됨. dodge/reflect/absorb는 불쾌 유발 위험
- → 2~3종만, 나머지는 Movement로 "잡기 어려움" 표현이 더 재밌음

#### Social이 삭제된 이유
- 빠른 탄막 게임에서 적의 미묘한 사회적 행동(힐러, 버프 등)을 인지하기 어려움
- 유일하게 가치 있는 "linked"(연결선)는 후순위 독립 피처로 분리
- summoner는 OnDeath 또는 별도 AI로 통합 가능

#### Visual Flair가 Body에 통합된 이유
- 독립 유전자로 분리하면 "같은 삼각형인데 떨림만 다른" 변종이 늘어남 = 다양성 착시
- Body shape마다 기본 애니메이션(pulse/spin/flicker)을 내장하는 게 효과적

### 결론: 5 유전자 시스템

```
적 DNA = Body × Movement × Attack × Modifier × OnDeath

Modifier = Defense + Aura 통합 (소수 정예)
Visual Flair = Body에 내장
Social = 삭제
```

---

## 4. 유전자 풀 상세 설계

### 4.1 Body (외형) — 15종

> 설계 원칙: **수학 곡선 기반**, 코드만으로 그릴 수 있는 기하학적 아름다움.
> Zero-Art의 존재 이유.

#### 기존 5종 (구현 완료)

| # | ID | 형상 | 수학적 배경 | 사용처 |
|---|-----|------|------------|--------|
| 1 | `circle` | ● | 원 | Bit |
| 2 | `diamond` | ◆ | 45° 회전 정사각형 | Node |
| 3 | `arrow` | ▶ | 이등변삼각형 + 꼬리선 | Vector |
| 4 | `spiral_ring` | ◎ | 동심원 2겹 | Loop |
| 5 | `hexagon` | ⬡ | 정육각형 | Matrix |

#### 신규 10종 (수학 곡선 — 게임에서 잘 안 쓰이는 형태)

| # | ID | 형상 | 수학적 배경 | 수식 | 신선도 | 구현 방식 |
|---|-----|------|------------|------|:------:|----------|
| 6 | `reuleaux` | 곡선 삼각형 | Reuleaux triangle — 정폭도형(맨홀 뚜껑) | 3개 호(arc)의 교집합 | ★★★ | `lg.arc()` × 3 |
| 7 | `lissajous` | 8자/매듭 | Lissajous curve — 비율에 따라 무한 변형 | `x=sin(at), y=sin(bt+δ)` | ★★★ | line strip |
| 8 | `rose` | 꽃잎 곡선 | Rose curve — n에 따라 잎 수 변화 | `r = cos(nθ)` | ★★★ | polygon |
| 9 | `cardioid` | 심장 곡선 | Cardioid — 마이크 지향성, 커피잔 반사 | `r = 1 + cos(θ)` | ★★★ | polygon |
| 10 | `astroid` | 별 곡선 | Astroid (hypocycloid 4) — 오목한 별 | `x=cos³θ, y=sin³θ` | ★★★ | polygon |
| 11 | `lemniscate` | ∞ 곡선 | Lemniscate of Bernoulli — 무한대 | `r² = cos(2θ)` | ★★★ | polygon |
| 12 | `deltoid` | 삼각 곡선 | Deltoid (hypocycloid 3) — 오목한 삼각 | 매개변수 방정식 | ★★★ | polygon |
| 13 | `vesica` | 렌즈/눈 | Vesica Piscis — 두 원의 교집합 | `lg.arc()` × 2 | ★★★ | arc 2개 |
| 14 | `trefoil` | 세잎 매듭 | Trefoil curve — 겹치는 3잎 | 극좌표 매개변수 | ★★★ | polygon |
| 15 | `superellipse` | 둥근 사각 | Lamé curve — n에 따라 형태 변화 | `\|x\|ⁿ+\|y\|ⁿ=1` | ★★★ | polygon |

#### 신선도 검토 (왜 흔한 도형을 뺐나)

| 삭제 후보 | 이유 |
|----------|------|
| `triangle` ▲ | arrow가 이미 삼각형 변형. 모든 게임에 있음 |
| `square` ■ | diamond가 45° 회전 사각. 너무 흔함 |
| `star` ★ | 흔하고, astroid가 수학적으로 더 흥미로운 별 |
| `cross` ✚ | 흔함. 탄막 패턴(cross)과 혼동 |
| `ring` ○ | spiral_ring으로 대체 |

> **열린 질문:** 각 Body shape에 기본 내장 애니메이션을 어떤 조합으로 할 것인가?
> (예: reuleaux는 pulse, lissajous는 점 이동 애니메이션, rose는 잎 펼침 등)

---

### 4.2 Movement (이동 패턴) — 12종

> 설계 원칙: 이동 패턴 = "어떻게 피하지?"의 본질. 재미 기여도 1위.

#### 기존 6종 (구현 완료)

| # | ID | 설명 | 난이도 |
|---|-----|------|:------:|
| 1 | `drift` | 고정 방향 이동 | ★ |
| 2 | `swarm` | 플레이어 돌진 (정지 안함) | ★ |
| 3 | `chase` | 플레이어 추적 (근접 시 정지) | ★★ |
| 4 | `orbit` | 플레이어 주위 원형 궤도 | ★★ |
| 5 | `stationary` | 제자리 고정 + 회전 | ★ |
| 6 | `charge` | 경고 → 고속 돌진 | ★★★ |

#### 신규 6종

| # | ID | 설명 | 수학적 배경 | 난이도 | 신선도 |
|---|-----|------|------------|:------:|:------:|
| 7 | `figure8` | 8자 궤적 | Lissajous(1:2) 경로 | ★★ | ★★★ |
| 8 | `pendulum` | 진자 운동 | 감쇠 진동, 상단 고정점 기준 스윙 | ★★ | ★★★ |
| 9 | `levy` | 짧은 이동 반복 → 갑자기 긴 도약 | Lévy flight (예측 불가) | ★★★ | ★★★ |
| 10 | `mirror` | 플레이어 대칭 위치 복사 | 월드 중심 기준 반전 | ★★★ | ★★★ |
| 11 | `gravity` | 타원 궤도 (플레이어=중력원) | 케플러 운동, 가까울수록 가속 | ★★★ | ★★★ |
| 12 | `bounce` | 벽 반사 (당구공) | 반사각 = 입사각 | ★★ | ★★☆ |

#### 삭제한 것

| 후보 | 삭제 이유 |
|------|----------|
| `patrol` | drift로 대체 가능. 행동 차이 미미 |
| `zigzag` | pendulum의 하위호환. 수학적 배경 없음 |

> **열린 질문:**
> - `mirror`는 Shielded와 결합 시 공략 불가능한 조합이 되지 않는지?
> - `levy`의 랜덤 도약이 "불공정"하게 느껴지지 않는지? (경고 연출 필요?)
> - `gravity` 궤도의 이심률 범위? (너무 타원이면 화면 밖으로 나감)

---

### 4.3 Attack (탄막 패턴) — 15종

> 설계 원칙: 탄막 = 게임 그 자체. 수학적 아름다움의 정수.

#### 기존 9종 (구현 완료, `none` 포함)

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

#### 신규 6종

| # | ID | 설명 | 수학적 배경 | 난이도 | 신선도 |
|---|-----|------|------------|:------:|:------:|
| 9 | `fibonacci` | 황금각(137.5°) 간격 발사 | 해바라기 씨앗 배열 (Fibonacci phyllotaxis) | ★★ | ★★★ |
| 10 | `helix` | 이중 나선 — DNA처럼 두 줄기 꼬임 | 이중 나선 (double helix) | ★★★ | ★★★ |
| 11 | `fractal_burst` | 탄이 일정 거리 후 3갈래 분열 (1세대만) | 프랙탈 분기 (fractal branching) | ★★★ | ★★★ |
| 12 | `freeze_ring` | 원형 발사 → 공중 정지 → 일제히 플레이어 방향 가속 | 지연 추적 (delayed homing) | ★★★ | ★★★ |
| 13 | `pendulum_stream` | 발사 각도가 진자처럼 좌우 왕복 | 부채꼴 스윕 (pendulum sweep) | ★★ | ★★★ |
| 14 | `gravity_well` | 탄끼리 중력 → 곡선 궤적 | N-body 간이 시뮬레이션 | ★★★ | ★★★ |

#### 삭제한 것

| 후보 | 삭제 이유 |
|------|----------|
| `shotgun` | aimed의 spread 변형. 독립 패턴으로 부족 |
| `grid` | circle과 사실상 동일 (코드 내 alias) |
| `laser` | 구현 복잡도 높음 + 즉사기는 탄막 게임 문법에 안 맞음 → 후순위 |
| `homing` | freeze_ring이 "지연 호밍"으로 상위호환. 순수 호밍은 밸런스 위험 |
| `mine` | 설치물 관리 = 새 시스템 필요. ROI 낮음 → 후순위 |

> **열린 질문:**
> - `fractal_burst`의 분열 세대를 1로 제한했는데, 2세대까지 허용하면 재밌는가? (성능 위험)
> - `gravity_well`의 N-body 연산이 bulletPool 2000개에서 감당 가능한가?
>   - 대안: 탄끼리가 아니라 "고정 중력점" 1~2개만 배치
> - `freeze_ring`의 정지 시간이 너무 길면 답답, 너무 짧으면 의미 없음. 최적 값은?

---

### 4.4 Modifier (방어 + 오오라 통합) — 6종

> 설계 원칙: "쾌감의 반대"가 되지 않도록 최소한만.
> "잡기 어려운 이유"는 Movement로 표현하는 게 더 재밌음.

| # | ID | 설명 | 비주얼 | 출처 |
|---|-----|------|--------|------|
| 0 | `none` | 기본 (방어 없음) | - | - |
| 1 | `armored` | HP ×2.5, 크기 ×1.3, 속도 ×0.7 | 두꺼운 외곽선 | Defense (검증됨) |
| 2 | `shielded` | 전방 90° 피탄 무효 | 전방 아크 | Defense (검증됨) |
| 3 | `phasing` | 주기적 투명화 무적 (0.5초/3초) | 깜빡임+반투명 | Defense (신규) |
| 4 | `gravity_aura` | 주변 플레이어 탄을 휘게 함 (명중 어려움) | 왜곡 링 | Aura (신규) |
| 5 | `time_warp` | 주변 플레이어 이속 -30%, 탄속 -30% | 시공간 파문 | Aura (신규) |

#### 왜 이것만 남겼나

| 삭제 후보 | 이유 |
|----------|------|
| `regen` (HP 회복) | "때려도 계속 차오름" = 불쾌. Armored가 이미 "안 죽는" 역할 |
| `dodge` (총알 회피) | "맞추려는데 피함" = 극도로 짜증. Movement가 이 역할 대체 |
| `reflect` (총알 반사) | 내 총알이 나한테 돌아옴 = 불공정 체감 |
| `absorb` (HP 흡수) | reflect와 동일한 불쾌감 |
| `slow_field` | time_warp으로 통합 (이속+탄속 동시) |
| `darkness` | Zero-Art에서 **아름다운 도형을 못 보게 하는 건 가치관 위반** |
| `magnet` (XP 방해) | 간접적 짜증. 플레이어가 원인을 인지하기 어려움 |
| `empower` (적 버프) | Social과 동일한 문제 — 인지 어려움 |

> **열린 질문:**
> - `gravity_aura`의 탄 궤적 왜곡이 **시각적으로 보여야** 의미 있음. 탄이 휘는 게 보이는가?
> - `time_warp` 범위 내에서 탄막도 느려지면 오히려 쉬워질 수 있음 (적 탄막도 느려짐?)
>   - 플레이어만 느려지고 적/탄막은 정상 → "포커스의 역전" 컨셉

---

### 4.5 OnDeath (사망 반응) — 5종

> 설계 원칙: "잡았다!" 이후의 2차 이벤트. 전략적 처치 순서 유발.
> 단, "죽여도 짜증"은 금지.

| # | ID | 설명 | 비주얼 | 재미 체크 |
|---|-----|------|--------|----------|
| 0 | `none` | 기본 사망 (파편만) | 파편 파티클 | 기본 |
| 1 | `split` | 미니 2마리 분열 | 현재 Splitter | ✅ 검증됨 |
| 2 | `explode` | 사망 시 원형 탄막 8발 | 적색 폭발 링 | ✅ "피했다!" 추가 쾌감 |
| 3 | `chain` | 가장 가까운 적 1마리에 데미지 전이 | 전기 아크 | ✅ **플레이어에게 유리** → 기분 좋음! |
| 4 | `blackhole` | 사망 위치에 2초간 중력장 (주변 탄/적 흡입) | 축소 소용돌이 | ✅ 전장 변화 → 전략적 처치 유도 |

#### 왜 이것만 남겼나

| 삭제 후보 | 이유 |
|----------|------|
| `poison` (독 장판) | 지속 데미지 = 짜증. 탄막 게임은 순간 피격이 핵심 |
| `buff_allies` (적 버프) | Social과 동일 — 효과가 안 보여서 인지 불가 |
| `revive` (부활) | "왜 또 살아?" = 최악의 불쾌감. 절대 안 됨 |
| `spawn_orb` (추가 보상) | OnDeath가 아니라 일반 드롭 시스템으로 처리 |

> **열린 질문:**
> - `chain` 데미지량은 어느 정도? 즉사? 비율? 
> - `blackhole` 중력장이 **내 탄까지 흡입**하면 양날검이 될 수 있음 (전략 깊이 vs 짜증)
> - `explode`의 탄막이 너무 많으면 "잡는 게 손해" → 최소 수량으로 유지

---

## 5. 조합 수 & 스케일링

### 수치

| 유전자 | 풀 크기 |
|--------|:------:|
| Body | 15 |
| Movement | 12 |
| Attack | 15 (none 포함) |
| Modifier | 6 (none 포함) |
| OnDeath | 5 (none 포함) |
| **총 조합** | **81,000** |

### 스테이지별 해금 곡선

```
Stage 1~3:   Body 5 × Move 3 × Attack 4 = 60 (수작업 프리셋)
Stage 4~6:   + Move 2 × Attack 2 × Modifier 2 = ~600
Stage 7~9:   + Body 3 × Move 2 × Attack 2 = ~3,000
Stage 10~12: + Modifier 2 × OnDeath 2 = ~10,000
Stage 13~15: + 나머지 해금 = 풀 개방
Stage 16+:   Endless — 시드 기반 절차적 생성, 매 스테이지 2~3 신종
```

### 자동 생성 로직 (구상)

```lua
-- 스테이지 시드 → 결정적 유전자 조합
function generateSpecies(stageNum, index)
    local seed = stageNum * 1000 + index
    math.randomseed(seed)  -- 결정적 시드
    
    local dna = {
        body     = pick(UNLOCKED_BODIES[stageNum]),
        movement = pick(UNLOCKED_MOVEMENTS[stageNum]),
        attack   = pick(UNLOCKED_ATTACKS[stageNum]),
        modifier = pick(UNLOCKED_MODIFIERS[stageNum]),
        onDeath  = pick(UNLOCKED_ONDEATHS[stageNum]),
    }
    
    -- 밸런스 제약: 금지 조합 필터
    dna = applyConstraints(dna)
    
    -- 스탯 자동 산출: DNA → HP/Speed/XP/Radius
    dna.stats = calculateStats(dna)
    
    return dna
end
```

> **열린 질문:**
> - 금지 조합은 뭐가 있을까? (예: shielded + stationary = 후방 공격 불가?)
> - 스탯 자동 산출 공식? (공격력 높으면 HP 낮게 등 트레이드오프)
> - 신종 등장 시 연출? ("New: ▲ Zigzag Burster" 플래시 표시?)
> - 세이브에 종 카탈로그 기록? (도감 시스템?)

---

## 6. 주목할 조합 예시

이 시스템의 가치를 증명하는 **다른 게임에서 본 적 없는** 조합들:

| 조합 | Body + Move + Attack | 체감 |
|------|---------------------|------|
| 수학 곡선 생명체 | `lissajous` + `figure8` + `fibonacci` | 8자로 날아다니며 황금각 탄막 |
| 대칭 퍼즐 적 | `reuleaux` + `mirror` + `freeze_ring` | 내 반대편에서 지연 호밍 |
| 중력 테마 적 | `cardioid` + `gravity` + `gravity_well` | 타원 궤도 + 휘는 탄 |
| 프랙탈 적 | `deltoid` + `levy` + `fractal_burst` | 순간이동 + 분열탄 |
| 무한 적 | `lemniscate` + `bounce` + `helix` | ∞자 외형, 벽 반사, 이중 나선탄 |
| 톱니 터렛 | `superellipse` + `stationary` + `pendulum_stream` | 둥근 사각 포탑, 진자 스윕 |

---

## 7. 구현 단계 (초안)

> 확정 전. 논의 후 조정 예정.

### Phase A: Body 확장 (비주얼 우선)
- 신규 10종 도형 렌더링 함수 구현
- 각 도형별 기본 애니메이션 내장
- 기존 적 5종 → 15종 Body로 확장

### Phase B: Movement 확장
- 신규 6종 AI 행동 구현
- 기존 enemyAISystem에 핸들러 추가

### Phase C: Attack 확장
- 신규 6종 탄막 패턴 구현
- bulletEmitterSystem 핸들러 추가
- bulletPool 확장 (분열탄, 지연 호밍 등 behavior 추가)

### Phase D: DNA 조합 엔진
- DNA 데이터 구조 정의
- 수작업 프리셋 (Stage 1~15)
- 절차적 생성 (Stage 16+)
- 금지 조합 필터
- 스탯 자동 산출 공식

### Phase E: Modifier + OnDeath
- phasing, gravity_aura, time_warp
- explode, chain, blackhole

---

## 8. 미결정 사항 목록

| # | 질문 | 관련 유전자 | 우선순위 |
|---|------|-----------|:--------:|
| 1 | Body별 기본 애니메이션 조합은? | Body + Visual | 높 |
| 2 | mirror + shielded 조합이 공략 불가능하지 않은가? | Move + Mod | 높 |
| 3 | levy의 랜덤 도약에 경고 연출 필요한가? | Movement | 중 |
| 4 | gravity 궤도의 이심률 범위는? | Movement | 중 |
| 5 | fractal_burst 분열 2세대까지 허용? | Attack | 중 |
| 6 | gravity_well N-body가 성능 가능한가? | Attack | 높 |
| 7 | freeze_ring 정지 시간 최적값? | Attack | 중 |
| 8 | gravity_aura의 탄 왜곡이 시각적으로 보이는가? | Modifier | 높 |
| 9 | time_warp가 적 탄막도 느리게 하면 역효과? | Modifier | 높 |
| 10 | chain 데미지량? 즉사 vs 비율? | OnDeath | 낮 |
| 11 | blackhole이 내 탄도 흡입? | OnDeath | 중 |
| 12 | 금지 조합 목록? | DNA 엔진 | 높 |
| 13 | 스탯 자동 산출 공식? (트레이드오프) | DNA 엔진 | 높 |
| 14 | 신종 등장 연출 방식? | UX | 낮 |
| 15 | 종 도감(카탈로그) 시스템? | 메타 성장 | 낮 |
| 16 | Worm(다관절)은 이 시스템에 통합 가능한가? | Body | 낮 |

---

## 변경 이력

| 날짜 | 내용 |
|------|------|
| 2026-04-21 | 초안 작성: 논의 과정 + 8유전자 → 5유전자 축소 + 신선도 검토 |
