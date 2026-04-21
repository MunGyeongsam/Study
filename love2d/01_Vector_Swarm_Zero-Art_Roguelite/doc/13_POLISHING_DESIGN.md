# 13. 폴리싱 설계 (Polishing Design)

> Phase 4: 메타 성장 → UX → 비주얼 마무리 → 성능 → 모바일 포팅
> 마지막 갱신: 2026-04-20
> 구현 상태: 4A ✅ / 4B ✅ / 4C ✅ / 4D 🔲 / 4E 🔲

---

## 1. 폴리싱 철학

### 1.1. 핵심 가치 기준 (우선순위순)

| 순위 | 가치 | Phase 4에서의 의미 |
|------|------|-------------------|
| 1 | **재미 (Fun)** | 메타 성장 = "한 판 더"의 이유. 없으면 리텐션 없음 |
| 2 | **쾌감 (Juice)** | 메뉴 전환, 보상 연출, 해금 이펙트 = 완성도의 체감 |
| 3 | **제약의 미학** | UI도 Zero-Art. 텍스트+벡터+수학으로만 메뉴를 구성 |

### 1.2. 판단 기준 (작업 우선순위)

```
1. "한 판 더" → 리텐션에 직결되는가?
2. "뭘 해야 하지?" → 첫 30초 경험이 개선되는가?
3. "좀 밋밋한데?" → 현재 가장 불편하거나 허전한 부분인가?
4. 기술적 필요 → 프로파일링 결과 병목인가?
```

### 1.3. 현재 게임 상태 진단

**강점 (이미 있는 것):**
- 탄막 회피 + 자동공격 + 대쉬/포커스 → 액션 코어 완성
- 적 5종 + 보스 5종 + 탄막 10+ 패턴 → 콘텐츠 볼륨 충분
- 레벨업 3택 + 감쇠 스택 → 인런 성장 있음
- Bloom + 파티클 + Screen Shake + Graze + BGM → Juice 레이어 완성

**약점 (지금 없는 것) → Phase 4에서 해소:**
- ~~죽으면 **완전 리셋**~~ → ✅ Data Fragment + 영구 강화 트리 (4A)
- ~~메뉴 없음~~ → ✅ TitleScene + PauseScene + GameOverScene (4B.1)
- ~~튜토리얼 없음~~ → ✅ 슬로모+글리치 4단계 인게임 힌트 (4B.2)
- ~~일시정지 없음~~ → ✅ PauseScene (ESC → Continue/Restart/Menu) (4B.1)

---

## 2. Phase 4A — 게임 루프 완성 (메타 성장) ✅

> 목표: 죽어도 쌓이는 보상 → "한 판 더"의 이유 만들기
> 상태: 전체 구현 완료 (upgradeTree.lua + achievementSystem.lua + saveData.lua)

### 2.1. Data Fragment (영구 재화)

#### 컨셉
- **Data Fragment** = 적/보스 처치 시 드롭되는 영구 재화
- 인런 XP(경험치)는 레벨업용, Data Fragment는 **영구 강화용**
- 세계관: 데이터스피어의 파편 — 적이 해체되며 남기는 정보 조각

#### 획득 공식
```
기본 드롭: enemy.xpValue × 0.5 (반올림)
보스 드롭: boss.maxHp × 0.3
스테이지 클리어 보너스: stage × 5
게임오버 시: 누적 fragment 확정 (세이브)
```

| 적 | xpValue | Fragment |
|----|---------|----------|
| Bit | 1 | 0~1 (50% 확률) |
| Node | 3 | 2 |
| Vector | 4 | 2 |
| Loop | 5 | 3 |
| Matrix | 6 | 3 |
| NULL boss | 50hp | 15 |
| STACK boss | 100hp | 30 |

#### 저장
- `love.filesystem.write/read` → JSON 또는 간단한 key=value
- 파일: `save/progress.dat`
- 저장 시점: 게임오버 결과 화면 표시 시

### 2.2. 영구 강화 트리 (Upgrade Tree)

#### 구조: 3갈래 트리

```
         [Core]
        /  |  \
   Attack  Def  Utility
```

| 카테고리 | 강화 항목 | 단계 | 단계당 효과 | 비용 (1→2→3→4→5) |
|---------|----------|------|-----------|-------------------|
| **Attack** | Base Damage | 5 | +1 기본 데미지 | 10, 25, 50, 100, 200 |
| | Fire Rate | 5 | +5% 발사속도 | 10, 25, 50, 100, 200 |
| | Multi Shot | 3 | +1 기본 탄 수 | 30, 80, 200 |
| **Defense** | Max HP | 5 | +1 최대 HP | 10, 25, 50, 100, 200 |
| | iFrame Duration | 3 | +0.2초 무적시간 | 20, 50, 120 |
| | Dash Cooldown | 3 | -0.3초 쿨타임 | 20, 50, 120 |
| **Utility** | Magnet Range | 5 | +15% 수집범위 | 8, 20, 40, 80, 160 |
| | XP Bonus | 5 | +10% XP 획득 | 8, 20, 40, 80, 160 |
| | Fragment Bonus | 3 | +15% Fragment 획득 | 30, 80, 200 |

#### 적용 방식
- `entityFactory.createPlayer()` 시점에 세이브 데이터 읽어서 기본값에 합산
- 인런 레벨업(levelUp.lua)은 그 위에 추가로 쌓임
- 예: 영구 강화 Damage Lv3(+3) + 인런 Damage 선택 2회 → 최종 +5

#### 설계 원칙
- **1시간 플레이 = 트리 1/3 해금** 목표 (빠른 보상 체감)
- **완전 해금은 불가능에 가깝게** (선택과 집중 = 빌드 다양성)
- Fragment Bonus를 Utility에 배치 → 초반에 투자하면 이후 가속

### 2.3. 해금 시스템 (4A.2)

#### 도전과제 → 해금

| 조건 | 해금 |
|------|------|
| Stage 3 최초 클리어 | 무기: Spread Shot (부채꼴 발사) |
| Stage 6 최초 클리어 | 무기: Piercing (관통탄) |
| 보스 5종 전부 처치 | 캐릭터: "Debugger" (작은 히트박스, 낮은 HP) |
| 누적 1000 처치 | 캐릭터: "Compiler" (높은 HP, 느린 이동) |
| 누적 Fragment 500 | 파시브: Start Boost (Stage 1 시작 시 랜덤 업그레이드 1개) |

#### 구현 방향
- 도전과제 진행도는 세이브 파일에 누적
- 해금 알림: 게임오버 결과 화면에서 "NEW!" 표시
- 무기/캐릭터 선택은 타이틀 메뉴에서

---

## 3. Phase 4B — 플레이어 경험 (UX) ✅

> Scene Stack 기반 구현 완료. 씨 8종 (sceneStack.lua + 7개 Scene 파일)
> 파일들: `src/03_game/scenes/*.lua`, `src/01_core/sceneStack.lua`

### 3.1. 화면 흐름도

**설계 → 구현 변경점**: 캐릭터/무기 선택은 4E(모바일 포팅) 시점으로 연기. Options는 미구현.

```
[타이틀 메뉴 (TitleScene)]
  ├── [PLAY] → replace(PlayScene) → [인게임]
  │                ├── [일시정지 (PauseScene)] → Continue / Restart / Menu
  │                ├── [레벨업 (LevelUpScene)] → 3택 선택 → 자동 pop
  │                └── [게임오버 (GameOverScene)]
  │                      ├── 도전과제 해금 알림 (있으면)
  │                      └── [R] Retry / [U] Upgrades / [ESC] Menu
  ├── [UPGRADES] → push(UpgradeScene) → 영구 강화 트리
  ├── [ACHIEVEMENTS] → push(AchievementScene) → 도전과제 + 해금 목록
  └── [CREDITS] → push(CreditsScene)
```

### 3.2. 타이틀 메뉴 (4B.1) ✅

#### 비주얼
- Zero-Art: 배경에 프랙탈 배경(background.lua 재활용) + 느린 회전
- "VECTOR SWARM" 로고: 벡터 도형 조합 (코드 드로잉)
- 메뉴 항목: 텍스트 + 좌우 네온 발광선

#### 구성 (실제 구현)
```
VECTOR SWARM
─────────────

▶ PLAY
  UPGRADES
  ACHIEVEMENTS
  CREDITS
```

> OPTIONS는 미구현. 향후 모바일 포팅(4E) 시 볼륨/해상도 설정 추가 예정.

### 3.3. 일시정지 (4B.1) ✅

- ESC → push(PauseScene) — Scene Stack 기반 오버레이
- drawBelow=true (게임 화면 보임), transparent=false (게임 정지)
- 옵션: Continue / Restart / Main Menu
- PauseScene이 pauseMenu.lua를 래핑

### 3.4. 게임오버 결과 화면 (4B.1)

현재 게임오버 화면 확장:

```
GAME OVER
─────────
Score: 142.3s
Stage: 7
Enemies: 234
Best: 201.5s

+45 Data Fragments  ← NEW
[NEW!] Spread Shot Unlocked  ← 조건 충족 시

[Retry]  [Menu]
```

### 3.5. 튜토리얼 (4B.2) ✅

#### 구현 방식: 슬로모 + 글리치 텍스트 4단계 힌트

설계(트리거 기반 5단계) → **구현(액션 감지 4단계)**로 변경.
이유: 탄막 속에서 텍스트가 묻히는 문제 → 슬로모션으로 주의 확보.

| 단계 | 메시지 | 완료 조건 | 슬로모 |
|------|--------|-----------|--------|
| 1 | **MOVE** — WASD to move | 이동 입력 감지 | 0.15× |
| 2 | **DASH** — SHIFT to dash | 대쉬 입력 감지 | 0.15× |
| 3 | **FOCUS** — SPACE to focus | 포커스 입력 감지 | 0.15× |
| 4 | **AUTO** — Auto-attack nearby | 3초 대기 | 0.15× |

- 활성 조건: `totalRuns == 0 and tutorialDone == false`
- 페이즈: idle → delay → entering(0.4s ease-in) → showing → completing(0.3s ease-out)
- 완료 시 `saveData.setTutorialDone(true)` → 이후 재표시 없음
- 파일: `src/03_game/states/tutorialHints.lua`

- 각 메시지는 1회만 표시 (세이브에 seen 플래그)
- 화면 하단 반투명 패널, 2~3초 후 페이드아웃
- `04_ui/tutorial.lua` 신규 모듈

### 3.6. 크레딧 (4B.3)

```
VECTOR SWARM
─────────────
Code & Design: [아빠 이름]
Music & Sound: [아들 이름]

─────────────
Background algorithm inspired by
Paul Bourke — "Random Space Filling"
paulbourke.net/fractals/randomtile/

Made with LÖVE2D 11.5
─────────────
Zero-Art: No sprites. Just math.
```

---

## 4. Phase 4C — 비주얼 폴리싱 ✅

### 4C.1. 구역 전환 연출 ✅

- 스테이지 번호에 따라 **배경 색조 변화**
- `background.lua` 의 색상 파라미터를 스테이지 테마와 연동
- 스테이지 전환 시 0.5초 크로스페이드

| Stage 범위 | 색조 | 분위기 |
|-----------|------|--------|
| 1~2 | 청록 (현재) | 차가운 데이터 |
| 3~5 | 보라 | 위험 증가 |
| 6~8 | 붉은 톤 | 긴장 고조 |
| 9+ | 흰/검 대비 | 최종 구역 |

### 4C.2. 보스 처치 연출 풀 버전 ✅

현재: 텍스트 + 플래시 + 히트스톱
추가:
- 보스 사망 시 **적 탄막 전부 → XP 오브 변환** (시각적 쾌감 극대화)
- 글리치 텍스트: 보스명이 1~2초간 랜덤 문자로 깜빡이다 사라짐
- HP바 파괴 애니메이션: 좌우로 갈라지며 사라짐

### 4C.3. 미니맵 ✅

- 화면 우상단, 화면 너비의 18%, 세로비 2:3 (월드 비율 동일)
- 표시: 플레이어(흰), 적(빨강), 보스(큰 빨강), XP오브(초록), 카메라 뷰포트(시안)
- Scissor 클리핑으로 범위 밖 렌더 방지
- uiManager에서 setMinimapData(ecs, player, cam)으로 데이터 전달
- 파일: `src/04_ui/minimap.lua`

---

## 5. Phase 4D — 성능 & 안정성

### 5.1. 밸런싱 (4D.1)

#### 체감 난이도 곡선 목표

```
난이도
  ▲
  │           ★ Boss3        ★ Boss6
  │        ╱ ╲            ╱ ╲
  │      ╱    ╲         ╱    ╲
  │    ╱       ╲      ╱       ╲
  │  ╱          ╲   ╱
  │╱              ╲╱
  └──────────────────────────────→ Stage
     1  2  3  4  5  6  7  8  9
```

- 보스 전: 긴장 상승 (적 수/속도 증가)
- 보스 후: 잠시 이완 (다음 사이클 준비)
- 영구 강화 반영: 강화 레벨이 높을수록 초반 쉽고 후반 동일

#### 밸런싱 파라미터 (조절 대상)

| 파라미터 | 현재 값 | 조절 범위 | 영향 |
|----------|---------|-----------|------|
| stageManager spawnDelay | 스테이지별 가변 | 0.5~3.0초 | 압박감 |
| enemyHpMult | 1.0 + stage×0.1 | 1.0~3.0 | 생존 시간 |
| bulletSpeedMult | 1.0 + stage×0.05 | 1.0~2.0 | 회피 난이도 |
| 플레이어 기본 HP | 5 | 3~7 | 초보 허용 범위 |
| XP xpToNext 성장률 | ×1.4 | ×1.2~×1.6 | 레벨업 빈도 |

### 5.2. 성능 프로파일링 (4D.2, 4D.3)

#### 기준선
- **목표**: 60fps @ 1000+ 엔티티 + 2000 탄환
- **측정 도구**: debug.lua 워치 패널 (이미 있음) + `love.timer.getDelta()`
- **병목 후보**: bulletPool:update() (O(n) 매 프레임), 충돌 체크 (O(n×m))

#### 최적화 판단 흐름

```
FPS < 55?
  ├── Yes → bulletPool이 주범?
  │    ├── Yes → Canvas 배치 렌더링 (4D.2)
  │    └── No → 충돌 체크가 주범?
  │         ├── Yes → 공간 파티셔닝 (4D.3)
  │         └── No → 개별 프로파일링
  └── No → 최적화 불필요. 다음 작업으로.
```

---

## 6. Phase 4E — 모바일 포팅

### 6.1. 빌드 타겟

| 플랫폼 | 방법 | 비고 |
|--------|------|------|
| Android | love-android (Gradle) | APK 생성 |
| iOS | love-ios (Xcode) | 개발자 계정 필요 |
| Desktop | 현재 그대로 | 이미 동작 |

### 6.2. 터치 최적화 체크리스트

- [ ] 터치 영역 크기 검증 (최소 44pt)
- [ ] 멀티터치: 이동 + 대쉬 동시 입력
- [ ] 화면 해상도 대응 (mobileLayout 이미 있음)
- [ ] 배터리 소모 테스트 (BGM + 렌더링)
- [ ] 세이브/로드 경로 (love.filesystem 모바일 경로)
- [ ] 앱 아이콘 (벡터 기반 생성)

---

## 7. 구현 순서 요약

```
4A.1 메타 성장 (Data Fragment + 강화 트리)
  → 4A.2 해금 시스템
  → 4B.1 메뉴 / 일시정지 / 결과 화면
  → 4B.2 튜토리얼
  → 4B.3 크레딧
  → 4C.1 구역 전환 연출
  → 4C.2 보스 처치 연출
  → 4D.1 밸런싱 & QA
  → 4D.2~3 성능 최적화 (필요시)
  → 4E.1 모바일 포팅
```

각 항목은 구현 전 이 문서의 해당 섹션을 참조하여 설계 결정을 따른다.
