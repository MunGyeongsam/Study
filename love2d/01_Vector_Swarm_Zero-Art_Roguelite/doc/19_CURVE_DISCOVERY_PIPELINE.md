# 19_CURVE_DISCOVERY_PIPELINE.md

## 목적

수학 곡선을 **자동 생성 → 테스트 → 검수 → 저장**하는 파이프라인 설계 문서.
현재는 **설계만** 기록하고, 실제 구현은 여유가 생기면 또는 next project로 미룬다.

---

## 전략

### 1. 생성 (Generation)

#### 1-1. 기본 전략: 파라미터 공간 탐색
기존 곡선 family에서 **파라미터를 체계적으로 변형**해서 새로운 候補를 생성.

**Rose 계열 확장:**
```
- Rose k/n: k, n을 1~15 범위 조합 (기존: 3, 5, 5/4, 7/3, 8/3)
  → 미탐색 조합 샘플링 (예: 11/7, 9/5, etc.)
- 배제: 이미 생성된 것, 시각적 중복 (수렴 테스트)
```

**Epicycloid 계열 확장:**
```
- Epicycloid k=3~8 (기존: 3, 4, 5, 6)
  → k=7, 8 자동 추가
  → Hypotrochoid (d 값 변형)
```

**Roulette 계열 (Hypo/Epitrochoid):**
```
- 고정: R=7, r=3 (기존)
- 변형: d 값 1~6 자동 스캔
  → 기존 d=2,4 외에 d=1,3,5,6 테스트
```

**Custom 조합 (제한적):**
```
- sin/cos 고주파 변조: r = a + b*sin(k*t)
  - 매개변수 범위: a∈[0.7,1], b∈[0.05,0.3], k∈[10,30]
  - 샘플 수: 50~100개
```

---

### 2. 테스트 (Testing)

각 候補에 대해 **렌더링 & 메트릭 계산**.

#### 2-1. 렌더링 안정성 체크

```lua
function testCurveStability(curveSpec)
  local points = {}
  
  -- 고해상도 샘플링 (t=0..2π, 1000 포인트)
  for i=1, 1000 do
    local t = (i-1) * 2 * math.pi / 999
    local x, y = evaluateCurve(curveSpec, t)
    if x and y and not isNaN(x) and not isNaN(y) then
      table.insert(points, {x, y})
    end
  end
  
  -- 체크 1: 충분한 점이 렌더링되는가?
  if #points < 500 then
    return false, "insufficient_points"
  end
  
  -- 체크 2: 불연속 구간이 너무 많은가?
  local discontinuities = countDiscontinuities(points, threshold=12)
  if discontinuities > 20 then
    return false, "too_many_discontinuities"
  end
  
  -- 체크 3: 폐곡선인가? (enemyFriendly 판정용)
  local isClosed = isClosedLoop(points)
  
  return true, {
    pointCount = #points,
    discontinuities = discontinuities,
    isClosed = isClosed
  }
end
```

#### 2-2. 메트릭 계산

```lua
function computeMetrics(points, curveSpec)
  return {
    -- 복잡도 (점의 밀도로 평가)
    complexity = estimateComplexity(points),
    
    -- 대칭성 (회전 대칭 여부)
    symmetry = detectSymmetry(points),
    
    -- 시각적 다양성 (기존 곡선과의 유사도)
    novelty = compareToExisting(points, curveSpec),
    
    -- 게임 친화성 (폐곡선 & 단순 & 안정적)
    enemyFriendly = (isClosed and complexity <= 2 and discontinuities == 0),
    
    -- 렌더링 성능 (defaultSteps 추정)
    estimatedDefaultSteps = inferComplexity(points) * 160
  }
end
```

---

### 3. 검수 (Validation & Filtering)

#### 3-1. 자동 필터링

```lua
DISCOVERY_FILTERS = {
  stability = {
    rule = "pointCount >= 500 AND discontinuities <= 10",
    action = "keep"
  },
  novelty = {
    rule = "similarity_to_existing <= 0.7",
    action = "keep"
  },
  enemy_friendly = {
    rule = "closed AND complexity <= 2 AND discontinuities == 0",
    action = "tag_as_hero"
  }
}

-- 후보 정렬 (스코어링)
function rankCandidates(candidates)
  for _, c in ipairs(candidates) do
    c.score = (
      c.metrics.novelty * 0.4 +
      (1.0 - c.metrics.complexity / 3) * 0.3 +
      (c.metrics.enemyFriendly and 1.0 or 0.5) * 0.3
    )
  end
  table.sort(candidates, function(a, b) return a.score > b.score end)
  return candidates
end
```

#### 3-2. 시각적 검증 포인트

**수동 검증** (자동 필터 후):
1. **Top 10 후보 렌더링** → CurveLab에서 시각 확인
2. **기존 곡선과의 비교** → "이건 Rose 5와 너무 비슷한가?" 체크
3. **게임 내 usage 시뮬레이션** → 적 패턴으로 200프레임 실행해보기
4. **성능 프로파일링** → defaultSteps가 합리적인가?

---

### 4. 저장 (Export)

#### 4-1. 출력 형식

```lua
-- 생성된 곡선 후보 (curveDefs.lua 호환)
local DISCOVERED_CANDIDATES = {
  {
    name = "Rose 11/7 [AUTO]",
    type = "polar",
    formula = function(t) return math.cos((11/7) * t) end,
    tRange = {0, 14 * math.pi},  -- LCM(11, 7) * π
    defaultSteps = 240,
    metadata = {
      origin = "[AUTO] Parameter sweep Rose family",
      confidence = "medium",
      family = "rose",
      complexity = 2,
      closed = true,
      discontinuous = false,
      enemyFriendly = true,
      discoveryScore = 0.82
    }
  },
  -- ... more candidates
}

return DISCOVERED_CANDIDATES
```

#### 4-2. 통합 프로세스

```lua
-- Manual integration step:
-- 1. python discover.py > candidates.lua
-- 2. Review top 10 in CurveLab
-- 3. Copy-paste approved curves into curveDefs.lua
-- 4. Update totalCurves count
-- 5. Test game + commit
```

---

## MVP (Minimum Viable Pipeline)

### Phase 1: Basic Generator (1-2h)
- Rose k/n 파라미터 스캔
- 안정성 체크만 (NaN/infinity)
- Top 20 후보 출력

### Phase 2: Metrics & Filtering (1-2h)
- 복잡도 추정
- 유사도 계산 (기존 vs 新)
- 자동 스코어링

### Phase 3: Visual Integration (1h)
- CurveLab 내 "AUTO" 필터
- 후보 목록 미리보기
- 수동 승인 UI

---

## 구현 언어 선택

| 언어 | 장점 | 단점 | 추천 |
|------|------|------|------|
| **Python** | 수학 라이브러리 풍부, 빠른 프로토타이핑 | Lua 곡선 형식 변환 필요 | ⭐ 자동화 도구용 |
| **Lua** | curveDefs.lua와 동일 언어 | 과학 계산 라이브러리 부족 | ⭐ Game integration용 |
| **Bash** | Git 자동화 쉬움 | 수학 연산 약함 | 파이프라인만 |

**추천 조합:**
- **Python** (`tools/curve_discovery.py`): 생성 + 채점 + 출력
- **Lua** (CurveLab 확장): 시각적 검증 + 통합

---

## Next Project 포팅 체크리스트

새 프로젝트에서 이 파이프라인을 재사용할 때:

- [ ] `tools/curve_discovery.py` 복사 (수학 로직 재활용)
- [ ] 새 곡선 family 추가 (생성 규칙만 수정)
- [ ] 메트릭 기준 조정 (게임 장르별로)
- [ ] CurveLab UI 없으면 CSV 출력으로 변경
- [ ] doc/19 내용 프로젝트에 맞게 수정

---

## 트러블슈팅

### "후보 곡선이 기존 것과 겹치는가?"
```lua
function compareToExisting(newPoints, existingCurves)
  local maxSimilarity = 0
  for _, existing in ipairs(existingCurves) do
    local existingPoints = sampleCurve(existing, 1000)
    local similarity = hausdorffDistance(newPoints, existingPoints)
    maxSimilarity = math.max(maxSimilarity, similarity)
  end
  return maxSimilarity
end
```

### "복잡도 추정이 정확한가?"
→ defaultSteps를 범위로 제시: `[estimatedMin, estimatedMax]`
→ 실제 렌더링 후 사용자가 조정

### "성능 저하 우려"
→ 파이프라인은 게임 외 도구로 실행
→ 생성 ~ 검수 : 별도 CLI tool
→ 저장된 후보만 CurveLab에 로드

---

## 다음 단계

1. **Phase 6 마무리** (Deity System)
2. **여유 시간에 Phase 0 실행**:
   - `tools/curve_discovery.py` 작성 (Rose 계열만)
   - CurveLab "AUTO" 필터 추가
   - Top 10 후보 미리보기 + 수동 승인
3. **Post-Launch 또는 Sequel Project**:
   - Roulette 계열 확장
   - Custom 조합 탐색
   - 게임 내 "새로운 곡선 발견" 스토리 연결

---

## 참고 자료

- `doc/16_CURVE_LIBRARY_WORKFLOW.md`: Auto-classification rules (메트릭 기준 참고)
- `src/03_game/data/curveDefs.lua`: Formula reference (생성 규칙 모델)
- `src/03_game/scenes/curveLabScene.lua`: Rendering logic (테스트 환경 구축용)

