# Vector Swarm 아키텍처 제안 (MVP 기준)


## 성장 시스템(레벨/능력치) 확장 고려

MVP 단계에서 최소한의 성장 시스템(경험치, 레벨, 능력치 등)을 구조에 포함:

* game/components/Exp.lua      # 경험치 데이터
* game/components/Level.lua    # 레벨/성장 단계
* game/components/Stat.lua     # 공격력, 체력 등 기본 능력치
* game/systems/LevelUpSystem.lua # 경험치 획득 및 레벨업 처리

이렇게 설계하면, 추후 스킬, 특성, 장비 등 다양한 성장 요소를 컴포넌트/시스템으로 쉽게 확장 가능

---
## 1. 전체 구조

```
src/
  core/           # 엔진/플랫폼 레이어 (ECS, 입력, 리소스)
  renderer/       # 렌더링/셰이더/배치
  game/           # 게임 도메인 (시스템, 컴포넌트, 패턴, 상태)
    systems/
    components/
    patterns/
    states/
  ui/             # UI 시스템 및 HUD
lib/              # 외부 라이브러리
assets/           # 셰이더, 데이터, 사운드
```

## 2. 아키텍처 계층

- **Engine Layer (core/):**
  - ECS 엔진, 입력 추상화, 리소스 관리
- **Rendering Layer (renderer/):**
  - 배치 렌더링, 셰이더, 카메라
- **Game Layer (game/):**
  - 시스템(로직), 컴포넌트(데이터), 패턴, 상태
- **UI Layer (ui/):**
  - 점수, 체력, 대시 등 HUD 및 터치 UI

## 3. 시스템 중심 MVC 변형

- **Model:** ECS의 엔티티/컴포넌트 (game/components/)
- **View:** 렌더링 시스템, UI (renderer/, ui/)
- **Controller:** 입력 시스템, 상태 전환 (core/input.lua, game/systems/TouchInputSystem)

## 4. 예시 흐름

1. **입력(Controller):**
   - 터치/스와이프 → core/input.lua → game/systems/TouchInputSystem
2. **로직(Model):**
   - 입력 결과로 ECS 컴포넌트(Transform, Velocity 등) 갱신
   - 시스템(Movement, Collision, Pattern 등)이 컴포넌트 처리
3. **출력(View):**
   - 렌더링 시스템이 ECS 상태를 기반으로 화면 그리기
   - UI 시스템이 점수/체력 등 HUD 표시

## 5. 상태(State) 관리
- game/states/IntroState.lua, PlayState.lua, GameOverState.lua 등으로 분리
- 각 상태에서 필요한 시스템만 활성화

## 6. 확장성/유지보수성
- 각 레이어/시스템/컴포넌트가 독립적이어서 기능 추가·수정이 용이
- 모바일 성능 최적화(ECS, 배치 렌더링, 상태 기반 시스템 관리)

---

이 구조는 문서의 "모바일 최적화", "ECS", "레이어 분리", "상태 관리", "확장성" 요구를 모두 충족합니다.

구체적 예시 코드나 각 시스템/컴포넌트 설계가 필요하면 언제든 말씀해 주세요!