# 🚀 Vector Swarm - Zero Art Roguelite

> **"수학적 패턴과 네온 그래픽의 조화, Zero-Art 탄막 로그라이트"**

코드만으로 그래픽과 사운드를 생성하는 **Zero-Art** 철학의 탄막 뱀서라이크.
적을 피하고, 처치하고, 레벨업하고, 영구 강화로 "한 판 더"를 반복하라.

## 🎮 게임 특징

### Core Loop
- **탄막 회피 + 자동 공격**: 가장 가까운 적을 자동 조준·발사. 플레이어는 이동과 회피에 집중
- **대쉬**: 순간이동 + 무적 (쿨타임 3초)
- **포커스**: 슬로모션 + 히트박스 축소 (에너지 소모)
- **인런 성장**: 적 처치 → XP 오브 → 레벨업 → 10종 업그레이드 3택

### Meta Growth
- **Data Fragment**: 적/보스 처치 시 획득하는 영구 재화
- **영구 강화 트리**: Attack / Defense / Utility 3갈래, 총 9종 강화
- **도전과제 & 해금**: 5종 도전과제 → 무기/캐릭터/패시브 해금
- **세이브 시스템**: 게임오버 시 자동 저장

### Content
- **적 5종**: Bit, Node, Vector, Loop, Matrix (각각 고유 AI + 탄막 패턴)
- **보스 5종**: NULL → STACK → HEAP → RECURSION → OVERFLOW (페이즈별 탄막 전환)
- **무한 스테이지**: Stage 16+ 무한 반복, 매 3스테이지마다 보스
- **튜토리얼**: 첫 플레이 시 슬로모+글리치 4단계 인게임 힌트

### Zero-Art
- **그래픽**: 모든 비주얼을 코드로 생성 (수학 함수 + 벡터 도형)
- **사운드**: 절차적 사운드 엔진 (5종 파형, ADSR, 주파수 스윕)
- **배경**: Random Space Filling 알고리즘 (Paul Bourke)
- **이펙트**: Bloom, Screen Shake, Graze, 파티클, 히트스톱

## 🚀 빠른 시작

### 게임 실행

**macOS:**
```bash
./run.sh
```

**Windows:**
```bat
run.bat
```

**VS Code:**
`F5` 키로 실행 (번들된 LÖVE 11.5 사용)

### 게임 컨트롤 (PC)
| 키 | 동작 |
|----|------|
| WASD / 방향키 | 이동 |
| Shift | 대쉬 (순간이동 + 무적) |
| Space (홀드) | 포커스 (슬로모 + 정밀이동) |
| 1 / 2 / 3 | 레벨업 옵션 선택 |
| ESC | 일시정지 / 종료 |

> ⚠️ 모바일 터치 조작은 개발 중. 현재 PC 키보드 권장.

## 🏗️ 기술 스택

- **LÖVE2D 11.5** — 2D 게임 프레임워크
- **Lua** — 스크립팅 언어
- **ECS** — Entity-Component-System 아키텍처 (ecs.lua, 17 컴포넌트, 17+ 시스템)
- **Scene Stack** — push/pop 기반 UI 상태 관리 (8개 씬)
- **Bloom Shader** — GLSL 기반 네온 발광

## 📂 프로젝트 구조

```
src/
├── main.lua              # LÖVE 콜백 (load/update/draw/input)
├── conf.lua              # 윈도우 설정 (432×960 세로)
├── 00_common/            # 유틸리티 (로거, 디버그, 세이브)
├── 01_core/              # 엔진 (ECS, Scene Stack, World)
├── 02_renderer/          # 카메라, Bloom, 배경
├── 03_game/              # 게임 로직
│   ├── components/       # ECS 컴포넌트 17종
│   ├── systems/          # ECS 시스템 17종+
│   ├── entities/         # 엔티티 팩토리 + 플레이어 파사드
│   ├── scenes/           # Scene Stack 씬 8종
│   └── states/           # 게임 상태 + 메타 시스템
├── 04_ui/                # HUD, 미니맵, 모바일 레이아웃
└── 05_sound/             # 절차적 사운드 (synth + sfxDefs)

doc/                      # 설계 문서 (00~14)
work_log/                 # 작업 일지
```

## 📊 개발 상태

```
Phase 0  ███████████  기반 구축              ✅
Phase 1  ███████████  MVP 게임 루프          ✅
Phase 2  ███████████  게임플레이 확장         ✅
Phase 3  ███████████  콘텐츠 & 비주얼        ✅
Phase 4  ████████░░░  폴리싱 & 출시          진행 중
```

Phase 4: 게임 루프 ✅ → UX ✅ → 비주얼 폴리싱 ✅ → 밸런싱 & 최적화 → 모바일 포팅

---

*Vector Swarm — LÖVE2D 11.5 / Lua / Zero-Art*

## 📊 개발 현황

### ✅ **완료된 기능**
- [x] 프로젝트 기본 구조 설정
- [x] LOVE2D 환경 구성 (9:20 해상도)
- [x] VS Code 통합 (F5 실행, 디버깅)
- [x] 4레벨 로깅 시스템 (DEBUG/INFO/WARN/ERROR)
- [x] 모듈화 아키텍처 설계
- [x] 기본 게임 루프 및 디버그 시스템

### 🚧 **진행 중인 작업**
- [ ] ECS 아키텍처 구현
- [ ] 터치 입력 시스템
- [ ] 기본 탄막 패턴 생성
- [ ] 플레이어 엔티티 시스템

### 🎯 **예정된 기능**
- [ ] 고급 터치 제스처 (스와이프 대쉬, 포커스 모드)
- [ ] 수학적 탄막 패턴 시스템
- [ ] 네온 이펙트 & 파티클 시스템
- [ ] 스테이지 및 난이도 시스템

## 🛠️ 개발 환경 설정

### VS Code 확장 프로그램 (권장)
```
- Love2D Dev Tools
- Lua Language Server
- Git Graph
- GitLens
```

### 디버깅 설정
```lua
-- F1: 디버그 콘솔 토글
-- F2: 로그 레벨 테스트
-- F3: 디버그 모드 토글
-- F5: 게임 실행 (VS Code)
```

### 커밋 규칙
```
feat: 새로운 기능
fix: 버그 수정
docs: 문서 업데이트
refactor: 코드 리팩토링
perf: 성능 최적화
```

## 🎯 MVP 개발 계획

### Week 1-2: Foundation
- ECS 아키텍처 구현
- 기본 터치 컨트롤
- 플레이어 엔티티 & 기본 탄막

### Week 3-4: Core Gameplay
- 고급 터치 제스처
- 수학적 탄막 패턴
- 충돌 검사 & 게임 오버

### Week 5+: Polish
- 네온 이펙트 & 사운드
- UI/UX 완성
- 성능 최적화 & 밸런싱

## 🤝 기여하기

이 프로젝트는 **모바일 탄막 게임의 혁신**을 목표로 합니다. 

### 관심 있는 분야
- 🎮 게임 디자인 & 밸런싱
- 🎨 절차적 그래픽 생성
- ⚡ 성능 최적화
- 📱 모바일 UX 개선

### 연락처
- 프로젝트 문의: [Issues](../../issues)
- 기능 제안: [Discussions](../../discussions)

---

**Made with ❤️ using LOVE2D & Lua**

> "코드로 만드는 아름다운 수학적 패턴의 세계"