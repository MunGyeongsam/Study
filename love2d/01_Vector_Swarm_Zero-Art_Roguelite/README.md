# 🚀 Vector Swarm - Zero Art Roguelite

> **"수학적 패턴과 네온 그래픽의 조화, 모바일용 탄막 액션 게임"**

절차적 생성으로 만들어지는 아름다운 탄막 패턴과 직관적인 터치 컨트롤이 만나는 혁신적인 모바일 게임입니다.

## 🎮 게임 특징

### 🌟 **사용자 경험**
- **🎯 직관적 터치 컨트롤**: 드래그로 이동, 스와이프로 대쉬, 롱프레스로 포커스 모드
- **📱 모바일 최적화**: 갤럭시 노트 20 기준 9:20 세로 화면에서 최적의 플레이 경험
- **✨ Zero-Art 비주얼**: 모든 그래픽을 코드로 생성하여 수학적 아름다움 구현
- **⚡ 고성능 탄막**: 300-500개 이상의 탄환이 60FPS로 부드럽게 움직임

### 🛠️ **기술적 혁신**
- **🏗️ ECS 아키텍처**: 확장 가능한 Entity-Component-System 설계
- **🎨 실시간 패턴 생성**: 수학 함수 기반의 절차적 탄막 패턴
- **⚡ 고성능 렌더링**: SpriteBatch 기반 GPU 최적화
- **🔧 모듈화 설계**: Engine-Rendering-Game 레이어 분리

## 🚀 빠른 시작

### 📋 **필요 조건**
- [LOVE2D 11.5+](https://love2d.org/) (게임 실행용)
- [VS Code](https://code.visualstudio.com/) (개발용, 권장)

### 🎮 **게임 실행**

#### Windows
```bash
./run.bat
```

#### macOS
```bash
./run.sh
```

#### 개발 모드 (VS Code)
```bash
F5 키 또는 Ctrl+Shift+P → "LÖVE: Run"
```

### 🎯 **게임 컨트롤**
- **이동**: 드래그로 플레이어 이동
- **대쉬**: 빠른 스와이프로 순간이동 (쿨타임 3초)
- **포커스**: 롱프레스로 시간 느려짐 (에너지 소모)
- **디버그**: F1~F3 키로 디버그 정보 표시

## 🏗️ 기술 스택

### 🎯 **Core Engine**
- **LOVE2D 11.5**: 고성능 2D 게임 엔진
- **Lua**: 빠르고 유연한 스크립팅 언어
- **ECS Pattern**: 대규모 엔티티 관리를 위한 아키텍처

### 🎨 **Graphics & Effects**
- **SpriteBatch**: GPU 최적화 배치 렌더링
- **GLSL Shaders**: 네온 이펙트, Bloom, Motion Blur
- **Procedural Generation**: 수학 함수 기반 패턴 생성

### 📱 **Mobile Optimization**
- **Touch Input System**: 멀티터치 제스처 인식
- **Performance Profiling**: 60FPS 유지를 위한 최적화
- **Battery Efficiency**: 모바일 GPU 효율성 고려

## 📂 프로젝트 구조

```
├── src/                       # 메인 소스 코드
│   ├── core/                  # 🔧 Engine Layer (재사용 가능)
│   │   ├── ecs.lua           # ECS 아키텍처 엔진
│   │   ├── registry.lua      # 전역 객체 관리
│   │   └── resource.lua      # 리소스 로더 (핫 리로딩)
│   │
│   ├── renderer/             # 🎨 Rendering Layer
│   │   ├── batcher.lua       # 고성능 배치 렌더링
│   │   ├── camera.lua        # 카메라 & 화면 효과
│   │   └── post_effect.lua   # 포스트 프로세싱
│   │
│   ├── game/                 # 🎮 Game Layer
│   │   ├── systems/          # 게임 시스템 (움직임, 충돌 등)
│   │   ├── components/       # 데이터 컴포넌트
│   │   ├── patterns/         # 탄막 패턴 스크립트
│   │   └── states/           # 게임 상태 관리
│   │
│   └── lib/                  # 📚 공통 라이브러리
│       └── logger.lua        # 4레벨 로깅 시스템
│
├── assets/                   # 📦 게임 리소스
│   ├── shaders/             # GLSL 셰이더 파일
│   ├── data/                # 게임 데이터 (JSON/Lua)
│   └── sfx/                 # 사운드 에셋
│
├── doc/                     # 📖 개발 문서
│   └── VECTOR_SWARM_SPEC.md # 상세 게임 기획서
│
├── .vscode/                 # ⚙️ VS Code 설정
└── love-11.5-*/            # 🎮 LÖVE2D 실행 파일
```

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