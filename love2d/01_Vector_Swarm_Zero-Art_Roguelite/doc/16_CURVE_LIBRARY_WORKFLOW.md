# 16_CURVE_LIBRARY_WORKFLOW.md

## 목적

CurveLab에서 수학 곡선을 빠르게 실험하고, 실제 게임(적 문양/shapeDefs)에 안전하게 이관하기 위한 운영 가이드.

이 문서는 프로젝트 외부(다른 LÖVE2D / Lua 프로젝트)에서도 재사용 가능하도록 작성한다.

스토리/업적/아이템 모티브 상세판은 `doc/17_CURVE_STORY_ATLAS.md`를 참조한다.

---

## 1) 데이터 설계 원칙

곡선 정의는 `src/03_game/data/curveDefs.lua`에만 둔다.
렌더/입력/필터 로직은 `src/03_game/scenes/curveLabScene.lua`에 둔다.

### 곡선 타입

- `polar`: `fn(t) -> r`
- `parametric`: `paramFn(t) -> x, y`
- `custom`: `customFn(steps) -> {x1,y1,...}`

### 공통 필드

- `name`: 곡선 표시 이름
- `formula`: UI 표시용 공식
- `fn`: 타입 문자열 또는 함수
- `tRange`: 샘플링 구간
- `defaultSteps`: 기본 정점 수

---

## 2) 자동 분류 메타데이터

곡선은 로드 시 아래 필드를 자동 부여한다.

- `complexity`: `defaultSteps` 기준 자동 1~3
  - `<=160 => 1`
  - `<=320 => 2`
  - `>320 => 3`
- `family`: 이름/타입 패턴으로 자동 추론
  - `rose`, `lemniscate`, `cycloid`, `trochoid`, `spiral`, `custom`, `parametric`, `polar`
- `closed`: open 예외 목록 + `tRange` span 기반 추론
- `discontinuous`: 특이점/분기 곡선 목록
- `enemyFriendly`: `closed and not discontinuous and complexity <= 2`

### open/discontinuous 예외 목록을 두는 이유

수식만으로 닫힘/불연속을 완전히 판별하기 어렵다.
실전에서는 예외 목록을 유지하는 것이 유지보수 비용이 가장 낮다.

---

## 3) CurveLab 필터 운영

Tab 키로 필터 순환:

- `ALL`
- `ENEMY`
- `CLOSED`
- `SIMPLE`
- `DISCONT`

HUD에는 다음을 표시한다.

- 현재 필터
- 필터 결과 개수 (`visible`)
- 현재 곡선 메타 (`family / complexity / enemyFriendly`)

필터가 빈 결과일 때도 안전하게 동작해야 한다.

---

## 4) 렌더 품질 규칙 (불연속/시임)

적 문양 용도에서는 곡선이 "수학적으로 정확"한 것보다 "시각적으로 안정"한 것이 우선이다.

### 적용 규칙

- 화면 좌표에서 큰 점프 구간은 세그먼트 분리 렌더
- 닫힌 단일 루프는 시작점 재연결로 시임 제거
- `fill`은 닫힌 단일 루프에서만 허용 (브리지 아티팩트 방지)

---

## 5) 곡선 추가 체크리스트

1. `curveDefs.lua`에 정의 추가
2. `defaultSteps`, `tRange`를 보수적으로 설정
3. 특이점이 있으면 `discontinuous`/open 예외 목록 반영
4. CurveLab에서 아래 항목 확인
   - `line/points/circle/fill` 모드
   - 회전 ON/OFF
   - `Tab` 필터 전환
5. ENEMY 필터에서 과밀/깨짐 없는지 확인

---

## 6) 적 문양 우선 곡선 선정 기준

우선 채택:

- 닫힘이 명확함 (`closed=true`)
- 분기/특이점 없음 (`discontinuous=false`)
- 정점 수 과다 아님 (`complexity<=2`)

보류:

- spiral/branch 계열
- `tan`, `1/cos(t)` 계열로 화면 점프가 큰 곡선

---

## 7) 곡선 아카이브 (소개/특징/비하인드)

실전에서 자주 쓰는 곡선을 "왜 채택했는지"까지 기록한다.
다음 프로젝트에서 곡선을 재선정할 때 이 섹션만 봐도 의사결정이 가능해야 한다.

### 7-1. Hero Curves (문양용 우선)

| 이름 | 분류 | 대표 수식 | 시각 특징 | 비하인드/메모 |
|---|---|---|---|---|
| Wavy Circle (19:3) | polar | `r = 0.9 + 0.1 sin(19/3 t)` | 원형 유지 + 잔물결 리듬 | "원형 실루엣 유지"와 "디테일"의 균형이 좋아 기본 문양으로 채택 |
| Reuleaux Triangle | custom | 3개의 원호 합성 | 둥근 삼각형, 강한 정체성 | 정폭도형 느낌이 있어서 보스/엘리트 구분에 잘 맞음 |
| Epicycloid (k=3/4/5/6) | parametric | `(R+r)cos t - r cos((R+r)t/r)` | 톱니/꽃잎 계열 실루엣 | `k`만 바꿔도 난이도 단계별로 패밀리 확장이 쉬움 |
| Hypotrochoid (R=7,r=3,d=2/4) | parametric | `(R-r)cos t + d cos((R-r)t/r)` | 기어형/로터형, 기계적 느낌 | 동일 수식에서 `d`만 조절하면 매끈/복잡 버전을 같이 만들 수 있음 |
| Cassini Oval | polar | `(x^2+y^2)^2 - 2a^2(x^2-y^2) + a^4 - b^4 = 0` | 타원~쌍루프 변형 | 파라미터 조절 시 "두 개의 코어" 같은 적 컨셉 제작에 유리 |
| Booth's Lemniscate | parametric | `x = cos t / (1+sin^2 t)` | 두툼한 8자 루프 | 일반 lemniscate보다 면적이 안정적이라 fill 렌더가 예쁨 |
| Gerono Lemniscate | parametric | `x = cos t, y = sin t cos t` | 깔끔한 무한대(∞) | 회전 애니메이션과 궁합이 좋아 "활성 상태" 연출에 적합 |
| Vesica Piscis | custom | 두 원의 교집합 | 눈/렌즈형 대칭 | 고대 문양 느낌이 강해 신비/고급 적 테마에 적합 |

### 7-2. 실전에서 주의할 곡선

| 이름 | 주의 이유 | 처리 전략 |
|---|---|---|
| Ophiuride | `tan` 계열 특이점 | `discontinuous=true`, 세그먼트 분리 렌더 |
| Conchoid of Nicomedes | `1/cos(t)` 근처 급점프 | `open` 처리 + range 제한 |
| Kampyle of Eudoxus | 본질적으로 분리된 branch | 문양용보다 패턴/연출용으로 분류 |
| Logarithmic Spiral / Lituus | 열린 곡선, 무한 확장 성질 | shapeDefs 이관 대상에서 제외 |
| Devil's Curve | 분기 구간이 많고 채움 난이도 높음 | custom branch 렌더 전용으로 유지 |

### 7-3. 이름/수식 네이밍 규칙 (재사용용)

- 이름은 `형태 + 핵심 파라미터` 조합으로 표기
  - 예: `Epicycloid (k=5)`, `Hypotrochoid (R=7,r=3,d=4)`
- 공식 문자열은 UI 표시를 위해 "핵심 형태"만 남긴다.
  - 너무 긴 식은 축약하고, 파라미터는 이름에서 보강한다.
- 같은 family는 접두를 통일한다.
  - `Rose`, `Epicycloid`, `Hypotrochoid`, `Lemniscate`

### 7-4. 추가 관련 숨은 이야기 (팀 메모)

- "Wavy Circle" 계열은 초반에는 단순 원처럼 보이는데, 회전/글로우를 켜면 디테일이 드러난다.
  - 플레이 중 가독성(외곽 인지)과 미학(디테일)을 동시에 가져갈 수 있다.
- `k`/`d` 파라미터를 고정하지 않고 프리셋 묶음으로 보관하면,
  - 새로운 수식을 찾지 않아도 "같은 패밀리의 다른 개체"를 빠르게 생성할 수 있다.
- 수학적으로 더 화려한 곡선이 있어도,
  - 실제 전투에서는 닫힘/면적 안정/가독성이 더 중요하다.
  - 그래서 `enemyFriendly` 자동 분류를 도입했다.

---

## 8) 이번 세션 기준 상태 요약

- 총 곡선 수: `52`
- 신규 추가(최근 배치):
  - `Rose 8/3`
  - `Booth's Lemniscate`
  - `Gerono Lemniscate`
  - `Epicycloid (k=4/5/6)`
  - `Hypotrochoid (R=7,r=3,d=2/4)`
  - `Epitrochoid (R=5,r=2,d=2)`
- CurveLab 필터: Tab 기반 5종 프리셋 동작

---

## 9) 다음 단계 (shapeDefs 이관)

1. `enemyFriendly=true` 곡선만 10~12개 선별
2. 정점 수 표준화 (`64/96/128`)
3. `shapeDefs.lua`로 사전 계산 정점 테이블 이관
4. CurveLab 이름과 shape key 매핑 테이블 유지

