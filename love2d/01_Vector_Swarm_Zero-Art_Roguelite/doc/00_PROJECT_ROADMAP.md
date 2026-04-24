# Vector Swarm — 프로젝트 로드맵 & TODO

> 마지막 갱신: 2026-04-23
> 현재 위치: **Phase 5 완료** — Victory + Endless + 보스 연출 + 밸런싱. 다음: Phase 6 절차적 적 DNA 시스템

---

## 전체 타임라인

```
Phase 0  ███████████  기반 구축 (완료)
Phase 1  ███████████  MVP 게임 루프 (완료)
Phase 2  ███████████  게임플레이 확장 (완료)
Phase 3  ███████████  콘텐츠 & 비주얼 (완료)
Phase 4  ███████████  폴리싱 & 출시 (완료: 4A~4D ✅, 4E 모바일 보류)
Phase 5  ███████████  콘텐츠 확장 & 엔드게임 (완료)
Phase 6  ▓▓▓░░░░░░░░  절차적 적 DNA 시스템 ← 진행 중 (6B ✅, 6A.1 ✅, 나머지 🔲)
```

### Phase 4 우선순위 원칙

```
판단 기준 (순서대로):
1. "한 판 더" → 리텐션에 직결되는가?
2. "뭘 해야 하지?" → 첫 30초 경험이 개선되는가?
3. "좀 밋밋한데?" → 현재 가장 불편하거나 허전한 부분인가?
4. 기술적 필요 → 프로파일링 결과 병목인가?
```

---

## Phase 0: 기반 구축 ✅

| # | 작업 | 상태 | 커밋 |
|---|------|------|------|
| 0.1 | LÖVE2D 프로젝트 셋업 (Windows/macOS) | ✅ | `ef953bf` |
| 0.2 | Logger + Debug 시스템 | ✅ | `6e95018` |
| 0.3 | Camera 시스템 (Unity-style 정사영) | ✅ | `ee501e3` |
| 0.4 | World 좌표계 + 존 정의 | ✅ | `ee501e3` |
| 0.5 | 모바일 UI 레이아웃 (topHud, bottomControls) | ✅ | `48e8c2b` |
| 0.6 | ECS 코어 (ecs.lua, system.lua, ecsManager.lua) | ✅ | `053bd8b` |
| 0.7 | Player ECS 전환 (컴포넌트 기본 8종, 시스템 기본 6종) | ✅ | `053bd8b` |
| 0.8 | Copilot 에이전트 환경 구축 | ✅ | `b45c9aa` |
| 0.9 | 기획 문서 작성 (시장조사, 컨셉, 세계관, 기술) | ✅ | `0965730` `cdc741d` |

---

## Phase 1: MVP 게임 루프 ✅

> 목표: 이동 → 적 출현 → 탄막 회피 → 피격/사망 → 재시작

| # | 작업 | 상태 | 커밋 |
|---|------|------|------|
| 1.1 | BulletPool (2000개 사전할당, zero-GC, swap-remove) | ✅ | `60b1937` |
| 1.2 | BulletEmitter 컴포넌트 + BulletEmitterSystem | ✅ | `60b1937` |
| 1.3 | 탄막 패턴 4종 (circle, spiral, aimed, wave) | ✅ | `d9286e6` |
| 1.4 | 충돌 시스템 (플레이어 ↔ 불릿, circle-circle) | ✅ | `00955ed` |
| 1.5 | Health 컴포넌트 + 무적시간(iFrame) + 깜빡임 | ✅ | `00955ed` |
| 1.6 | EnemyAI 컴포넌트 (drift, orbit, chase) | ✅ | `d9286e6` |
| 1.7 | 적 타입 4종 프리셋 (basic, spiral, aimed, wave) | ✅ | `d9286e6` |
| 1.8 | 웨이브 스포너 (시간 기반, 스케일링) | ✅ | `d9286e6` |
| 1.9 | GameState (playing/game_over) + 점수(생존시간) | ✅ | `83ce770` |
| 1.10 | 게임오버 오버레이 + R키/터치 리스타트 | ✅ | `83ce770` |

---

## Phase 2: 게임플레이 확장 ✅

> 목표: 플레이어 공격 + 대쉬 + 경험치/레벨업 + 스테이지 진행
> 완료: 2A~2D 전부 완료 (컴포넌트 16종, 시스템 14종 + StageManager)

### 2A. 플레이어 공격 시스템 ✅
| # | 작업 | 상태 | 커밋 |
|---|------|------|------|
| 2A.1 | 플레이어 자동공격 (가까운 적 자동 조준·발사) | ✅ | `aad435b` |
| 2A.2 | 플레이어 불릿 → 적 충돌 (적 HP + 처치) | ✅ | `aad435b` |
| 2A.3 | 적 처치 이펙트 (파편 파티클) | ✅ | — |
| 2A.4 | 처치 시 경험치 드롭 (자석식 수집) | ✅ | `f78b515` |

### 2B. 대쉬 & 포커스 ✅
| # | 작업 | 상태 | 커밋 |
|---|------|------|------|
| 2B.1 | 스와이프 대쉬 (순간이동 + 무적 + 쿨타임 3초) | ✅ | `94fc670` |
| 2B.2 | 대쉬 쿨타임 HUD 표시 | ✅ | `94fc670` |
| 2B.3 | 포커스 모드 (Space → 슬로모 + 정밀이동) | ✅ | `94fc670` |
| 2B.4 | 포커스 에너지 게이지 | ✅ | `94fc670` |

### 2C. 경험치 & 레벨업 ✅
| # | 작업 | 상태 | 커밋 |
|---|------|------|------|
| 2C.1 | XP 시스템 (적 처치 → 경험치 획득) | ✅ | `f78b515` |
| 2C.2 | 레벨업 시 3택 1 선택 UI | ✅ | `f78b515` |
| 2C.3 | 무기 업그레이드 옵션 (발사속도/탄수/관통 등) | ✅ | `f78b515` |
| 2C.4 | 패시브 스킬 옵션 (이속/체력/자석범위 등) | ✅ | `f78b515` |

### 2D. 스테이지 & 난이도 ✅
| # | 작업 | 상태 | 커밋 |
|---|------|------|------|
| 2D.1 | 웨이브 → 스테이지 진행 (kill-all 클리어 방식) | ✅ | `5eae6f1` |
| 2D.2 | 난이도 곡선 (적 수/HP/이속/탄속 스케일링) | ✅ | `5eae6f1` |
| 2D.3 | STAGE_DEFS 데이터 드리븐 + 무한 스테이지 | ✅ | `59f923e` |
| 2D.4 | 4방향 스폰 (top/left/right/bottom 확률 기반) | ✅ | `59f923e` |
| 2D.5 | 스테이지 디자인 가이드 문서 | ✅ | `59f923e` |

---

## Phase 3: 콘텐츠 & 비주얼 🔲

> 목표: 보스전 + 적 다양성 + 시각 효과 + 사운드

### 3A. 보스 시스템 🔄
| # | 작업 | 상태 |
|---|------|------|
| 3A.1 | BossTag 컴포넌트 + BossSystem | ✅ |
| 3A.2 | createBoss() 팩토리 + BOSS_TYPES | ✅ |
| 3A.3 | 보스 탄막 패턴 (페이즈별 순환) | ✅ |
| 3A.4 | StageManager 보스 스테이지 통합 (3,6,9,12,15) | ✅ |
| 3A.5 | 보스 HP바 HUD + 보스명 표시 | ✅ |
| 3A.6 | 보스 처치 보상 (탄막소거 + XP폭발 + HP회복 + 히트스톱) | ✅ |
| 3A.7 | 보스 등장/처치 연출 (MVP: 텍스트 + 플래시) | ✅ |
| 3A.8 | NULL 보스 프리셋 (Stage 3, HP50, 2페이즈) | ✅ |
| 3A.9 | STACK 보스 프리셋 (Stage 6, HP100, 3페이즈) | ✅ |
| 3A.10 | HEAP 보스 프리셋 (Stage 9) | ✅ |
| 3A.11 | RECURSION 보스 + 미니언 소환 (Stage 12) | ✅ |
| 3A.12 | OVERFLOW 보스 + 전패턴 통합 (Stage 15) | ✅ |
| 3A.13 | 보스 설계 문서 (doc/08) | ✅ |

### 3B. 적 다양성
| # | 작업 | 상태 |
|---|------|------|
| 3B.1 | 세계관 기반 적 6종 (Bit, Node, Vector, Loop, Matrix, Worm) | ✅ (5종, Worm 후순위) |
| 3B.2 | 적별 고유 탄막 패턴 + AI | ✅ (ring_pulse, cross) |
| 3B.3 | 고급 탄막 (orbit_shot, return_shot) | ✅ |
| 3B.4 | 호밍 탄막 (보스 전용, 보류) | 🔲 |

### 3C. 비주얼 이펙트
| # | 작업 | 상태 |
|---|------|------|
| 3C.1 | 네온 발광 셰이더 (Bloom/Glow) | ✅ |
| 3C.2 | 대쉬 잔상/트레일 효과 | ✅ |
| 3C.3 | 보스 등장 시 화면 흔들림 | ✅ |
| 3C.4 | 배경 시각화 (Random Space Filling, Paul Bourke) | ✅ (v1: 원, 공간해시, 시드) |
| 3C.5 | Graze 이펙트 (아슬아슬 회피 시각 피드백) | ✅ |

### 3D. 사운드
| # | 작업 | 상태 |
|---|------|------|
| 3D.1 | 절차적 사운드 시스템 (synth 엔진 + SFX 6종) | ✅ |
| 3D.2 | 효과음 게임 통합 (발사, 피격, 처치, 대쉬, 레벨업) | ✅ |
| 3D.3 | BGM + 보스전 BGM 전환 | ✅ |

---

## Phase 4: 폴리싱 & 출시 🔲

> 목표: 메타 성장으로 리텐션 확보 → UX 완성 → 비주얼 마무리 → 성능 검증 → 모바일 출시
>
> 순서 원칙: **재미(리텐션) → UX(접근성) → Juice(연출) → 최적화(필요시) → 포팅**

### 4A. 게임 루프 완성 (리텐션) — "한 판 더"
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 4A.1 | 메타 성장 (Data Fragment + 영구 강화 트리) | ✅ | 죽어도 쌈이는 보상 → 핵심 리텐션 |
| 4A.2 | 해금 시스템 (도전과제 → 신규 무기/캐릭터) | ✅ | 장기 목표 부여 |

### 4B. 플레이어 경험 (UX) — 게임의 "얼굴"
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 4B.1 | 메뉴 / 일시정지 / 옵션 화면 | ✅ | 타이틀, 일시정지 메뉴 (옵션은 4B.2+) |
| 4B.2 | 튜토리얼 (첫 30초 가이드) | ✅ | 슬로모+글리치 힌트 4단계 |
| 4B.3 | 크레딧 메뉴 (Paul Bourke 등) | ✅ | CreditsScene + Zero-Art 스타일 |

### 4C. 비주얼 폴리싱 — Juice 마무리
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 4C.1 | 구역 전환 연출 (배경색/톤 변화) | ✅ | 4단계 색조 + 0.8s lerp |
| 4C.2 | 보스 처치 연출 풀 버전 (탄막→XP, 글리치 텍스트) | ✅ | 기술부채 해소 |
| 4C.3 | 미니맵 (월드 내 위치 표시) | ✅ | 해상도 비례 크기, 우상단 오버레이 |

### 4D. 성능 & 안정성 — 프로파일링 후 판단
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 4D.1 | 밸런싱 & QA (전체 난이도 곡선 튜닝) | ✅ | 아래 세부 항목 참조 |
| 4D.2 | Canvas 배치 렌더링 (draw call 최적화) | 🔲 | 병목 확인 시만 |
| 4D.3 | 공간 파티셔닝 (uniform grid, 필요시) | 🔲 | 병목 확인 시만 |

#### 4D.1 밸런싱 & QA 세부
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 4D.1a | DPS 스케일링 억제 | ✅ | Multi Shot maxPicks=2 (5발 cap), xpGrowth 1.35 |
| 4D.1b | XP 오브 색상 구분 | ✅ | 초록→금색 {1.0, 0.85, 0.2} (플레이어 탄환과 분리) |
| 4D.1c | 보스 페이즈 전환 무적 | ✅ | 2초 무적 + 8Hz 깜빡임 + enemyCollision 가드 |
| 4D.1d | 보스 HP 레벨 스케일링 | ✅ | `baseHP × (1 + 0.15 × playerLv)`, NULL HP 50→150 |
| 4D.1e | 스테이지 클리어 스토리텔링 | ✅ | 일반=글리치 텍스트, 보스=팝업 씬. stageStory.lua 신규 |
| 4D.1f | 기타 밸런스 (Variant XP, Endless 탄막, Data Miner 등) | ✅ | 8개 항목 구현 완료 |

### 4E. 모바일 포팅
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 4E.1 | Android/iOS 빌드 + 터치 최적화 | 🔲 | 가상 조이스틱, 스와이프 대쉬, 튜토리얼 힌트 텍스트 터치용 분기 포함 |
| 4E.2 | 해상도 자동 대응 (PC/모바일/태블릿) | 🔲 | 디바이스 해상도 감지 → UI/카메라 자동 스케일링 |

---

## Phase 5: 콘텐츠 확장 & 엔드게임 ✅

> 목표: 엔드게임 달성감 + 콘텐츠 다양성 + 커스텀 스테이지로 리플레이 가치 극대화
>
> 순서 원칙: **버그 수정 → 비주얼 다양성 → 엔드게임 → 커스텀 스테이지**

### 5A. 보스 & 적 비주얼/행동 다양화 — "원만 보이는 건 아니다"
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 5A.1 | 🐛 보스 월드바운드 이탈 버그 수정 | ✅ | createBoss에 WorldBound 추가 + 텔레포트 클램핑 (1e87e0d) |
| 5A.2 | 보스 고유 외형 (Zero-Art 기하학 도형) | ✅ | CS 자료구조 × 글리치: null=역삼각, stack=적층사각, heap=다이아+트리, recursion=프랙탈삼각, overflow=헥사+고스트 (f80cf8a) |
| 5A.3a | 포메이션 시스템 (5종 대형) | ✅ | Wedge, Pincer, Triangle, Escort, Spiral Array (`4663f70`) |
| 5A.3b | Swift 변형 (고속 소형) | ✅ | 속도×1.5, 크기×0.8 + 잔상 렌더 (`090fdbd`) |
| 5A.3c | Armored 변형 (고체력 대형) | ✅ | HP×2.5, 속도×0.7 + 두꺼운 외곽선 (`89f59e0`) |
| 5A.3d | Splitter 변형 (분열형) | ✅ | 사망시 미니 2마리 분열 + 점선 외곽 (`097dd9d`) |
| 5A.3e | Shielded 변형 (전방 방어) | ✅ | 전방 90° 피탄 무효 + 방패 아크 |
| 5A.3f | STAGE_DEFS 확장 (변형+포메이션 배치) | ✅ | 테마 교대 + 보장 등장 + 스케일링 확률 + 포메이션 변형 (4규칙) |
| 5A.4 | 보스 등장/페이즈 전환 비주얼 연출 강화 | ✅ | 인트로 스케일인+글리치 / 페이즈 히트스톱+틴트+펄스 / 처치 화이트아웃+쇼크웨이브 |
| 5A.5 | Bit 군집 분리 + 접촉 충돌 | ✅ | Spatial hash 분리력 + 몸통 접촉 즉사 + God mode 관찰 통합 (`2434349`) |

### 5B. 엔드게임 — Victory + Endless
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 5B.1 | Victory 상태 + Victory Scene (OVERFLOW 처치 후) | ✅ | 글리치 텍스트 + 통계 + C/ESC 선택 |
| 5B.2 | Endless 모드 (Stage 16+ 보스 루프, 매 3스테이지) | ✅ | D플랜: 5보스 순환 × HP1.5^R, Spd1.1^R, 미니언+R |
| 5B.3 | 첫 클리어 도전과제 ("SYSTEM.EXIT(0)") | ✅ | achievementSystem.onVictory() |
| 5B.4 | Endless 전용 배경 톤 변경 | ✅ | 다크레드 (0.45, 0.08, 0.08) |

### 5C. 커스텀 스테이지 — "내가 만든 스테이지"
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 5C.1 | JSON 기반 스테이지 로드 (Phase A) | 🔲 | ext_res/stages/custom.json → STAGE_DEFS 덮어쓰기 |
| 5C.2 | 인게임 스테이지 에디터 (Phase B) | 🔲 | 가로 16:9 별도 해상도, 마우스 기반 PC 도구 느낌 |

---

## Phase 6: 절차적 적 DNA 시스템 🔲

> 목표: Stage 16+ Endless에서 무한한 적 다양성 — 유전자 조합 + 수학 곡선 도형
>
> 설계 문서: `doc/15_PROCEDURAL_ENEMY_DNA_DESIGN.md`
>
> 순서 원칙: **6B(변이 엔진) → 6A(CS 특수 적) → 6C(탄막 궤적) → 6D(폴리싱)**

### 6A. CS 특수 적 & Deity 시스템 — 수작업 프리셋 + 신 메카닉
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 6A.0-3 | **곡선 정규화** (상위 우선) | 🔲 | 무게중심 정렬 + 단위스케일 + AABB 바운딩 → shapeDefs 기초 (1-2h) |
| 6A.0 | shapeDefs.lua 수학 곡선 정점 생성기 | 🔲 | 정규화 후: Cardioid, Astroid, Rose 등 10+종 |
| 6A.1 | **Deity System v1** (Ritual Scene) | ✅ | 4신(Rose/Cycloid/Lemniscate/Inferno) + Passive + Signature VFX + Selection Scene → `doc/20_DEITY_SYSTEM_DESIGN.md` |
| 6A.2 | Deity System v2 (Codex Scene) | 🔲 | 신 정보 + 곡선 미리보기 + 스토리 연결 (선택사항, 1.5h) |
| 6A.3 | Deity System v3 (Trial Events) | 🔲 | 신별 도전과제 (Post-Phase 6) |
| 6A.4 | Linked List (체인 절단) | 🔲 | CS 특수 적 1/3 |
| 6A.5 | Tree (프랙탈 분열) | 🔲 | CS 특수 적 2/3 |
| 6A.6 | Hash (순간이동) | 🔲 | CS 특수 적 3/3 |

### 6A* (선행). 곡선 라이브러리 v2 & 발견 파이프라인 ✅
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 6A*.0 | 곡선 52종 확장 + 메타 자동분류 | ✅ | Rose 8/3 등 9종 추가, complexity/family/closed/discontinuous/enemyFriendly (`0fa7b64`의 상위개념) |
| 6A*.1 | Tab 필터 5개 (all/enemy/closed/simple/discontinuous) | ✅ | CurveLab 씬 확장 ||
| 6A*.2 | doc/16_CURVE_LIBRARY_WORKFLOW.md | ✅ | 설계 원칙 + 자동분류 규칙 + Hero Curves 아카이브 |
| 6A*.3 | doc/17_CURVE_STORY_ATLAS.md | ✅ | 52곡선 내러티브 + 신 4종 매핑 |
| 6A*.4 | doc/18_CURVE_MATH_SCIENCE_NOTES.md | ✅ | 52곡선 수학 배경 + 확실도 태깅 + 수식 레퍼런스 |
| 6A*.5 | doc/19_CURVE_DISCOVERY_PIPELINE.md | ✅ | 자동 발견 파이프라인 설계 (미래 작업용) |

### 6B. DNA 변이 엔진 — Stage 16+ 자동 생성 ✅
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 6B.0 | dnaDefs.lua 유전자 풀 + 생성 엔진 | ✅ | 5유전자, 24레이어후보, generateDna(), calcStats() (`0fa7b64`) |
| 6B.1 | Body 레이어 렌더러 + 신규 도형 6종 | ✅ | triangle/star/cross/tear/bowtie/gear + 오목 fill 수정 (`0fa7b64`) |
| 6B.2 | entityFactory.createDnaEnemy() | ✅ | DNA→ECS 엔티티 변환 (movement/attack/modifier 매핑) (`0fa7b64`) |
| 6B.3 | 스폰 연동 (ecsManager + stageManager) | ✅ | Stage 16+ DNA 적 확률 스폰 min(0.3+R×0.1, 0.7) (`0fa7b64`) |
| 6B.4 | 테스트 + 튜닝 | ✅ | Gallery Scene (G키) + Stage 16 실전 확인 (`0fa7b64`) |

### 6C. 탄막 궤적 다양화
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 6C.1 | Lissajous / Fibonacci spiral 궤적 | 🔲 | |

### 6D. 폴리싱
| # | 작업 | 상태 | 비고 |
|---|------|------|------|
| 6D.1 | DNA 적 밸런스 튜닝 | 🔲 | atk:none 비율, 라운드별 난이도 곡선 |

---

## 기술 부채 & 메모

- [x] ~~world.lua 존 텍스트 한글 인코딩 깨짐~~ (존 시스템 미사용으로 자연 해소)
- [ ] 해상도 고정(432×960) → 디바이스 해상도 자동 감지 + 레터박스/스케일링 전환 필요 (4E.2)
- [x] ~~구역 전환 연출~~ → 4C.1로 이동
- [x] ~~보스 처치 연출 풀 버전~~ → 4C.2로 이동
- [x] ~~ecsManager 레이어 위반~~ (01_core/ → 03_game/ 이동)
- [x] ~~emoji 렌더링 (topHud)~~ (ASCII 태그로 교체)
- [x] ~~late require (ecsManager, background)~~ (모듈 상단으로 이동)
- [x] ~~magic number (entityFactory)~~ (PLAYER/ENEMY_MAX_SPEED 상수 추출)
- [x] ~~main.lua if-else 상태 분기 폭발~~ (Scene Stack 아키텍처 도입, `ae1b1c4`)
- [x] ~~🐛 보스 WorldBound 누락~~ — createBoss()에 WorldBound 추가 + 텔레포트 클램핑 (5A.1, `1e87e0d`)
- [x] ~~🐛 보스 텔레포트 X좌표 클램핑 누락~~ — bossSystem 텔레포트 시 반경 고려 클램핑 (5A.1, `1e87e0d`)
- [x] 적 비주얼 획일화 — basicShapes.lua 6종 도형 구현 완료 (circle, rectangle, diamond, arrow, spiral_ring, hexagon)
- [ ] 디버그 단축키 정리 — F5~F12 매핑 충돌/포화 상태, 체계적 재배치 필요 (F10 중복 등)
- [x] ~~macOS 한글 IME 키 입력 누락~~ — love.textinput + JAMO_TO_KEY 매핑으로 해결 (ㅎ→g, ㅊ→c 등)
- [ ] playerWeaponSystem 최근접 적 탐색 최적화 — 현재 전체 순회(O(n)), 공간 분할(spatial hash/grid) 도입 필요

---

## 커밋 히스토리 (주요)

| 날짜 | 커밋 | 내용 |
|------|------|------|
| Day 1 | `053bd8b` | ECS 코어 + Player 전환 + Logger 정비 |
| Day 1 | `b07b06b` | 문서 5종 현행화 |
| Day 2 | `60b1937` | Phase 1.1-1.2: 불릿 풀 + 이미터 |
| Day 2 | `b2c9d5c` | 코드 리뷰 수정 |
| Day 2 | `00955ed` | Phase 1.4-1.5: 충돌 + HP + iFrame |
| Day 2 | `d9286e6` | Phase 1.6-1.8: 적 AI + 패턴 + 스포너 |
| Day 2 | `83ce770` | Phase 1.9-1.10: 게임오버 + 리스타트 |
| Day 3 | `aad435b` | Phase 2A: 플레이어 자동공격 + 적 충돌 |
| Day 3 | `94fc670` | Phase 2B: 대쉬 & 포커스 시스템 |
| Day 3 | `d7460e0` | 코드 리뷰 리팩터 |
| Day 3 | `f78b515` | Phase 2C: XP & 레벨업 시스템 |
| Day 3 | `66e3d60` | 플레이 가이드 문서 |
| Day 3 | `c796e12` | 문서 정리 (12개 → 7개) |
| Day 3 | `97bb50b` | 월드 축소 (120×250 → 20×30) + bulletBounds 수정 |
| Day 3 | `da09926` | planner 에이전트 보강 + planning-guide 스킬 |
| Day 3 | `c7bb33b` | planner 에이전트/스킬 리뷰 보완 |
| Day 3 | `5eae6f1` | Phase 2D: 스테이지 & 난이도 시스템 |
| Day 3 | `59f923e` | STAGE_DEFS + 4방향 스폰 + 스테이지 디자인 문서 |
| Day 4 | | 보스 설계 문서 `doc/08_BOSS_SYSTEM_DESIGN.md` 작성 |
| Day 4 | | Phase 3A MVP: BossTag + BossSystem + 페이즈 + 보상 시퀀스 |
| Day 4 | | NULL 보스 (Stage 3) + STACK 보스 (Stage 6) 구현 |
| Day 4 | | 무적 모드 (F7) 디버그 기능 추가 |
| Day 4 | | 코드 리뷰 2회 (refactor-cleaner) |
| Day 4 | `ede88c9` | Phase 3A MVP: 보스 시스템 (NULL + STACK) |
| Day 4 | `80c6690` | HEAP 보스 + 텔레포트 + F8 스테이지 스킵 |
| Day 4 | `98ff572` | RECURSION 보스 + 미니언 소환 |
| Day 4 | `34e0045` | OVERFLOW 최종 보스 (4Phase 폭주) |
| Day 4 | `126731a` | 코드 전체 점검 — 크리티컬 버그 2건 수정 |
| Day 5 | `258e2ac` | 자동 챙김 규칙 추가 |
| Day 5 | `8fb064b` | 공유 참조 버그 4건 + angle 오버플로 + lovec 콘솔 설정 |
| Day 5 | `bdab36c` | 적 처치 파편 파티클 (2A.3) |
| Day 5 | `d066a67` | 밸런스 패스 — 감쇠 스택 + XP 스케일링 |
| Day 5 | `2d17115` | ecs._indexSize O(1) + mobileLayout GC 제거 |
