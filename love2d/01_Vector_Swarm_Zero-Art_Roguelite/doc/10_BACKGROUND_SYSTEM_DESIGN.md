# 10. 배경 시스템 설계 (Background System Design)

> 절차적 프랙탈 배경 — Random Space Filling 기반

---

## 영감 & 참조

### Paul Bourke — Random Space Filling of the Plane
- **핵심 참조:** https://paulbourke.net/fractals/randomtile/index.html
- **프랙탈 갤러리:** https://paulbourke.net/fractals/
- **원 논문:** John Shier, "Filling Space with Random Fractal Non-Overlapping Simple Shapes" (Hyperseeing, 2011)

> Paul Bourke의 수학적 시각화 작업에 깊은 존경과 감사를 표합니다.
> 이 게임의 배경 시스템은 그의 Random Space Filling 알고리즘에서 영감을 받았습니다.
> 게임 내 크레딧에 반드시 명시합니다.

---

## 크레딧 계획

| 시점 | 내용 | Phase |
|------|------|-------|
| 게임 종료 화면 | "Background inspired by Paul Bourke's Random Space Filling" | 3C.4 (현재) |
| 크레딧 메뉴 | 정식 크레딧 (이름, URL, 참조 논문) | Phase 4 |
| README.md | 기술 참조 및 감사 | 3C.4 (현재) |
| 코드 주석 | 모듈 헤더에 출처 URL | 3C.4 (현재) |

---

## 핵심 가치 검증

| 가치 | 기여 |
|------|------|
| **재미 (Fun)** | 스테이지마다 다른 배경 = 시각적 신선함, 진행 동기 |
| **쾌감 (Juice)** | 어두운 프랙탈 + Bloom = 깊이감, 네온 분위기 극대화 |
| **제약의 미학** | 수학 함수로 절차 생성. 리소스 0, 코드만으로 무한 배경 |

---

## 알고리즘 개요

### Random Space Filling

평면을 겹치지 않는 도형(원)으로 채우는 알고리즘.

**면적 감소 함수:**
```
A(i) = A0 * i^(-c)
```
- `A0`: 초기 면적 (전체 영역과 리만 제타 함수로 결정)
- `c`: 감소율 파라미터 (1.1~1.5 사이가 적절)
- `i`: 반복 횟수

**의사 코드:**
```
choose c
calculate A0 from Riemann zeta relationship
seed random number generator

for i = 1 to N:
    radius = sqrt(A0 * i^(-c) / pi)
    repeat:
        choose random position (x, y)
        check overlap with all existing circles
    until no overlap
    add circle to plane
```

**겹침 판정 (원):**
```
overlap = distance(c1, c2) < r1 + r2
```

---

## 설계 결정

### 확정 사항

| 항목 | 결정 | 이유 |
|------|------|------|
| 렌더링 | Canvas 없이 직접 draw | 메모리 절약, 가시 영역만 렌더링 |
| 컬링 | 공간 해시 그리드 (O(1) 조회) | 정적 데이터에 최적, 구현 간단 |
| 데이터 | 메모리 테이블 + 시드 기반 | 생성 비용 무시 가능, 파일 불필요 |
| 확장성 | 시드 파라미터화 (파일/공유는 후순위) | YAGNI 원칙 |
| 생성 시점 | 혼합 (큰 원 즉시 + 작은 원 점진) | 쾌감: "공간 구축" 연출 |
| 도형 | 원(circle)만 먼저 | 검증 후 도형 확장 |
| 색상 | 스테이지별 색조 전환 (4단계 + lerp) | 4C.1에서 구현 |
| c 파라미터 | 1.2 시작, 런타임 조절 가능 | 테스트 후 튜닝 |
| 원 스타일 | filled / outline / 혼합 3종 테스트 | 직접 보고 판단 |

### 공간 해시 그리드 설계

```
월드: 120 × 250
셀 크기: 10 × 10
그리드: 12열 × 25행 = 300셀
카메라 영역: ~4.5 × 10 ≈ 1~2셀 조회 → O(1)
```

- 생성 시 각 원을 해당 셀에 등록
- 큰 원(여러 셀에 걸침)은 걸치는 모든 셀에 중복 등록
- 렌더 시 카메라 영역의 셀만 순회

---

## 구현 계획

### v1: 기본 구현
- [ ] `src/02_renderer/background.lua` 모듈
- [ ] Random Space Filling 알고리즘 (원만)
- [ ] 공간 해시 그리드
- [ ] 시드 기반 생성
- [ ] 점진 생성 연출 (큰 원 즉시 + 작은 원 프레임당 N개)
- [ ] filled / outline / 혼합 스타일 토글
- [ ] 런타임 c값 조절 (디버그 키)
- [ ] main.lua 통합 (camera.draw 내부, 월드 좌표계)

### v2: 확장 (후순위)
- [x] 스테이지별 색상/톤 변화
- [ ] 다형 도형 (삼각형, 사각형, 육각형)
- [ ] 보스 스테이지 특수 배경
- [ ] 배경 반응 효과 (플레이어 근처 원이 살짝 밝아지기 등)

### v3: 비전 (Phase 4+)
- [ ] 시드/배경 공유 시스템
- [ ] 커스텀 배경 에디터
- [ ] 기타 프랙탈 패턴 (paulbourke.net/fractals/ 참조)

---

## 참고: 유용한 프랙탈 패턴 후보 (paulbourke.net)

향후 배경 다양화 시 참고할 패턴들:

| 패턴 | URL | 메모 |
|------|-----|------|
| Random Space Filling | /fractals/randomtile/ | **v1 구현 대상** |
| DLA (확산 제한 응집) | /fractals/dla/ | 나뭇가지/번개 느낌 |
| Apollonian Gasket | /papers/apollony/ | 원 안의 원 — 재귀적 아름다움 |
| Voronoi | /fractals/randomtile/ (extension) | 셀 분할 배경 |
| Perlin Noise | /fractals/noise/ | 유기적 패턴, 구름/안개 |
