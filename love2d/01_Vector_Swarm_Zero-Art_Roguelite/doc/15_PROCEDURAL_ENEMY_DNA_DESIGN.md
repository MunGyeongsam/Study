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
