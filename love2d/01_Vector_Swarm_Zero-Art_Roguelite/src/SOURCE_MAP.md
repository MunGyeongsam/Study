# Vector Swarm — Source Map

> **용도**: Copilot / 개발자가 코드베이스를 빠르게 파악하기 위한 네비게이션 문서
> 마지막 갱신: 2026-04-24
> 총 파일: 90개 Lua 파일 (~12,000 lines)

---

## 아키텍처 한눈에 보기

```
진입점:  main.lua → love.load() → global/logger → ECS/Scene → 게임 시작
렌더링:  bloom.begin → camera.draw(ECS draw) → bloom.end → UI overlay
입력:    touch/mouse → sceneStack → top scene → uiManager or game logic
레이어:  00_common → 01_core → 02_renderer → 03_game → 04_ui / 05_sound
         (높은 번호가 낮은 번호를 require; 역방향 금지)
```

---

## 파일 인덱스

### 진입점

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `main.lua` (~290L) | LÖVE 콜백 진입점. Scene Stack 기반 게임 루프 | `love.load/update/draw/keypressed` |
| `conf.lua` (~75L) | LÖVE 윈도우 설정 (432×960, 9:20 세로) | `love.conf(t)` |

### 00_common/ — 유틸리티 (6파일, ~650L)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `global.lua` (~30L) | 전역 헬퍼 주입 (log, setColor 등) | `init()` → 전역 함수 생성 |
| `logger.lua` (~150L) | 4레벨 로깅 + 인게임 콘솔 (`` ` `` 키) | `init/debug/info/warn/error/drawConsole` |
| `debug.lua` (~60L) | 디버그 워치 패널 (F1) | `add(key, fn)/draw()` |
| `gridDebugDraw.lua` (~80L) | 스크린 그리드 오버레이 (F4) | `toggle()/draw()` |
| `mathUtil.lua` (~15L) | 수학 유틸 (exp decay) | `expDecay(cur, target, k, dt)` |
| `saveData.lua` (~280L) | JSON 영구 저장 (fragments, upgrades, 업적) | `load/save/addFragments/getUpgrades` |

### 01_core/ — 엔진 (4파일, ~540L)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `ecs.lua` (~230L) | ECS 코어: 엔티티/컴포넌트/쿼리 | `new/createEntity/addComponent/queryEntities` |
| `system.lua` (~70L) | 시스템 베이스 클래스 (성능 모니터링) | `System.new(name, required, updateFn)` |
| `sceneStack.lua` (~120L) | push/pop 씬 스택 (투명/drawBelow 지원) | `push/pop/replace/clear/update/draw` |
| `world.lua` (~120L) | 아레나 월드 (20×30, 중심 0,0) | `getBounds/getSize/getStartPosition` |

### 02_renderer/ — 그래픽 (5파일, ~1040L)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `camera.lua` (~200L) | Unity 스타일 정사영 카메라 (Y↑) | `draw(fn)/worldCoords/cameraCoords` |
| `cameraManager.lua` (~200L) | 게임/디버그 카메라 전환 (F5) | `init/update/draw/shake/toggle` |
| `bloom.lua` (~240L) | 블룸 후처리 (threshold + Gaussian) | `beginCapture/endCapture/draw` |
| `background.lua` (~350L) | Random Space Filling 배경 (Paul Bourke) | `init/update/draw/setStage` |
| `trailRenderer.lua` (~240L) | 플레이어 리본 트레일 (additive 메쉬) | `reset/update/draw/onDash` |

### 03_game/ — 게임 로직 (50+파일, ~7000L)

#### 오케스트레이터

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `ecsManager.lua` (~350L) | ECS 시스템 등록/실행 순서 관리 | `init/update/draw/createPlayer/createEnemy` |

#### entities/ — 엔티티 팩토리 (2파일)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `entityFactory.lua` (~500L) | 엔티티 생성 (플레이어/적/보스/XP/DNA적) | `createPlayer/createEnemy/createBoss/createDnaEnemy` |
| `player.lua` (~110L) | 플레이어 ECS 파사드 | `bind/update/getPosition/getStats` |

#### components/ — 순수 데이터 컴포넌트 (17파일, ~400L)

| 컴포넌트 | defaults 핵심 필드 |
|----------|-------------------|
| `transform` | x, y, angle, scale |
| `velocity` | vx, vy, speed, maxSpeed, damping |
| `collider` | radius, layer, mask |
| `renderable` | type, radius, color |
| `health` | hp, maxHp, iFrames, alive |
| `playerTag` | powerUps, zoneHistory |
| `input` | moveX, moveY, dash |
| `dash` | distance, cooldown, iFrames |
| `focus` | active, energy, maxEnergy |
| `enemyAI` | behavior, xpValue, speed |
| `bulletEmitter` | pattern, emitRate, bulletSpeed |
| `playerWeapon` | fireRate, bulletSpeed, bulletCount |
| `playerXP` | level, xp, xpToNext, magnetRange |
| `xpOrb` | value |
| `bossTag` | bossType, phase, maxPhase |
| `lifespan` | time, destroyOffScreen |
| `worldBound` | enabled |

#### data/ — 순수 데이터 테이블 (8파일, ~2300L)

| 파일 | 역할 | 핵심 데이터 |
|------|------|------------|
| `stageData.lua` (~175L) | 스테이지 정의 + 보스 배치 + 변형 확률 | `BOSS_STAGES/STAGE_DEFS/VARIANT_TIERS` |
| `bossDefs.lua` (~280L) | 보스 5종 프리셋 (스탯/패턴/AI) | `BOSS_TYPES["NULL"/"STACK"/...]` |
| `deityDefs.lua` (~100L) | Deity 4신 정의 (패시브+시그니처) | `DEITIES/applyStats/tryTrigger` |
| `dnaDefs.lua` (~450L) | DNA 변이 엔진 유전자 풀 (Stage 16+) | `generateDna/calcStats/BODY_SHAPES` |
| `formationDefs.lua` (~140L) | 대형 패턴 5종 (wedge, pincer 등) | `DEFS/getAvailable/getChance` |
| `curveDefs.lua` (~800L) | 수학 곡선 라이브러리 53종 | `polar/parametric/custom` 곡선 함수 |
| `shapeDefs.lua` (~360L) | 곡선 큐레이션 결과 (적/보스/오버레이 분류) | `groups/isUsable/getNormalization` |
| `stageStory.lua` (~70L) | 스토리 텍스트 (일반/보스/엔들리스) | `NORMAL_STORIES/BOSS_STORIES` |

#### scenes/ — 씬 스택 패턴 (12파일)

| 파일 | 역할 | drawBelow | transparent |
|------|------|-----------|-------------|
| `playScene.lua` (~600L) | 메인 전투 루프 (ECS+카메라+UI) | ✗ | ✗ |
| `titleScene.lua` (~60L) | 타이틀 메뉴 래퍼 | ✗ | ✗ |
| `deitySelectScene.lua` (~200L) | Deity 의식 선택 (2×2 곡선 카드 + 드로잉 애니메이션) | ✗ | ✗ |
| `codexScene.lua` (~300L) | Deity 도감 (풀화면 1신씩, 곡선+lore+스탯) | ✓ | ✗ |
| `pauseScene.lua` (~50L) | 일시정지 오버레이 | ✓ | ✗ |
| `levelUpScene.lua` (~40L) | 레벨업 3택 오버레이 (자동 pop) | ✓ | ✗ |
| `gameOverScene.lua` (~80L) | 게임오버 결과 화면 | ✗ | ✗ |
| `victoryScene.lua` (~180L) | 승리 연출 (글리치+통계) | ✗ | ✗ |
| `upgradeScene.lua` (~35L) | 영구 강화 트리 오버레이 | ✓ | ✗ |
| `achievementScene.lua` (~180L) | 업적 목록 | ✓ | ✗ |
| `creditsScene.lua` (~150L) | 크레딧 (Paul Bourke 등) | ✗ | ✗ |
| `galleryScene.lua` (~450L) | 적 갤러리 (3페이지: 기존/신규/DNA) | ✗ | ✗ |
| `curveLabScene.lua` (~1000L) | 곡선 실험실 (52+종 브라우저) | ✗ | ✗ |

#### states/ — 게임 상태 (7파일)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `gameState.lua` (~240L) | 상태 머신 (playing/game_over/victory 등) | `startPlaying/update/isPlaying/setTimeScale` |
| `levelUp.lua` (~410L) | 레벨업 3택 UI + 감쇠 수익 | `show/select/isActive/draw` |
| `upgradeTree.lua` (~410L) | 영구 강화 트리 (Data Fragment 상점) | `purchase/applyToPlayer/getLevel` |
| `achievementSystem.lua` (~220L) | 업적 추적 + 해금 관리 | `onEnemyKill/onBossDefeated/onStageClear` |
| `titleMenu.lua` (~280L) | 타이틀 메뉴 UI | `setCallbacks/draw/keypressed` |
| `pauseMenu.lua` (~150L) | 일시정지 메뉴 UI | `setCallbacks/draw/keypressed` |
| `tutorialHints.lua` (~280L) | 첫 플레이 힌트 (슬로모+글리치) | `init/update/draw/isActive` |

#### systems/ — ECS 시스템 (18파일, ~3000L)

**실행 순서** (ecsManager 등록 순):

| # | 파일 | 역할 | 필요 컴포넌트 |
|---|------|------|--------------|
| 1 | `inputSystem.lua` (~75L) | 키보드/터치 → 속도/대쉬/포커스 | Input |
| 2 | `focusSystem.lua` (~70L) | 포커스 슬로모 + 에너지 관리 | Focus, Input |
| 3 | `dashSystem.lua` (~80L) | 대쉬 순간이동 + 무적 | Dash, Input, Transform |
| 4 | `enemyAISystem.lua` (~230L) | 적 AI (drift/orbit/chase/charge/swarm) | Transform, EnemyAI, Velocity |
| 5 | `movementSystem.lua` (~25L) | 물리 (Velocity → Transform) | Transform, Velocity |
| 6 | `boundarySystem.lua` (~35L) | 월드 경계 클램핑 | Transform, WorldBound |
| 7 | `lifespanSystem.lua` (~40L) | 수명 만료 엔티티 제거 | Transform, LifeSpan |
| 8 | `bulletEmitterSystem.lua` (~140L) | 적 탄막 발사 (8 패턴) | Transform, BulletEmitter, EnemyAI |
| 9 | `playerWeaponSystem.lua` (~95L) | 플레이어 자동조준 발사 | PlayerTag, Transform, PlayerWeapon |
| 10 | `collisionSystem.lua` (~165L) | 플레이어 ↔ 적탄 충돌 + Graze | PlayerTag, Collider |
| 11 | `enemyCollisionSystem.lua` (~170L) | 플레이어탄 ↔ 적 충돌 + 처치 | Health, Collider, Transform |
| 12 | `xpCollectionSystem.lua` (~80L) | XP 오브 자석 수집 | PlayerTag, PlayerXP, Transform |
| 13 | `bossSystem.lua` (~300L) | 보스 생애주기 (페이즈/텔레포트/미니언) | BossTag, Transform, Health |
| — | `bulletPool.lua` (~260L) | 불릿 오브젝트 풀 (2000개, 레이어별) | (ECS 외부, ecsManager가 호출) |
| — | `stageManager.lua` (~820L) | 스테이지/웨이브/보스 진행 관리 | (ECS 외부, ecsManager가 호출) |
| R1 | `renderSystem.lua` (~250L) | 적/보스/XP 오브 렌더링 (Strategy) | Transform, Renderable |
| R2 | `playerRenderSystem.lua` (~105L) | 플레이어 렌더링 (외곽+방향) | PlayerTag, Transform, Health |

#### systems/renderers/ — 렌더 전략 (3파일, ~650L)

| 파일 | 역할 | 등록 방식 |
|------|------|----------|
| `basicShapes.lua` (~200L) | 적 도형 12종 (circle, diamond, arrow 등) | renderSystem에 자동 등록 |
| `bossRenderers.lua` (~250L) | 보스 외형 5종 + 절차 폴백 | renderSystem에 자동 등록 |
| `variantOverlays.lua` (~200L) | 변형 오버레이 4종 (swift/armored/splitter/shielded) | renderSystem에 자동 등록 |

### 04_ui/ — UI 시스템 (5파일, ~800L)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `uiManager.lua` (~170L) | UI 오케스트레이터 (터치 라우팅) | `init/update/draw/touchpressed` |
| `topHud.lua` (~200L) | 상단 HUD (점수/스테이지/보스HP바) | `setGameData/draw` |
| `bottomControls.lua` (~250L) | 하단 컨트롤 버튼 | `setCallbacks/draw/touchpressed` |
| `mobileLayout.lua` (~200L) | 반응형 레이아웃 (top 5%/play 85%/bottom 10%) | `getLayout/isTouchInArea` |
| `minimap.lua` (~150L) | 미니맵 오버레이 (우상단) | `draw(ecs, player, cam)` |

### 05_sound/ — 절차적 오디오 (3파일, ~1200L)

| 파일 | 역할 | 핵심 API |
|------|------|----------|
| `synth.lua` (~350L) | 오실레이터 엔진 (5파형, ADSR, 주파수 스윕) | `generate(params) → SoundData` |
| `soundManager.lua` (~500L) | SFX 풀링 + BGM 재생 (ext_res 우선, 합성 폴백) | `init/play/playBGM/stopBGM` |
| `sfxDefs.lua` (~350L) | SFX 레시피 6종 (발사/피격/처치/대쉬/레벨업/플레이어피격) | `M.DEFS` 테이블 |

---

## 주요 데이터 흐름

### 게임 루프 (매 프레임)
```
love.update(dt)
  → sceneStack:update(dt)
    → playScene:update(dt)
      → ecsManager.update(dt)
        → 시스템 1~13 순서대로 실행
        → bulletPool:update(dt)
        → stageManager:update(dt)
      → cameraManager.update(dt, playerPos)
      → gameState.update(dt, playerHealth)
      → background.update(dt)

love.draw()
  → sceneStack:draw()
    → playScene:draw()
      → bloom.beginCapture()
      → cameraManager.draw(function()
          background.draw(cam)
          ecsManager.draw()  -- renderSystem + playerRenderSystem
          trailRenderer.draw()
        end)
      → bloom.endCapture() + bloom.draw()
      → uiManager.draw()
      → 디버그 오버레이 (logger, debug, grid)
```

### 입력 파이프라인
```
love.touchpressed(id, x, y)
  → sceneStack:touchpressed(id, x, y)
    → top scene:touchpressed(id, x, y)
      → uiManager.touchpressed(id, x, y)  -- UI가 먼저 소비
      → mobileLayout.isTouchInArea(x, y, "play")  -- 게임영역 체크
      → inputSystem (게임 로직)
```

### 적 생성 흐름
```
stageManager:update(dt)
  → 웨이브 타이머 도달
  → ecsManager.createEnemy(x, y, type, difficulty, variant)
    → entityFactory.createEnemy(world, x, y, type, difficulty, variant)
      → world:createEntity() + addComponent(Transform, Velocity, Health, ...)
  → (Stage 16+) ecsManager.createDnaEnemy(x, y, dna, difficulty)
    → dnaDefs.generateDna(round)
    → entityFactory.createDnaEnemy(world, x, y, dna, difficulty)
```

---

## 수정 가이드 (Quick Reference)

### 새 적 타입 추가
1. `stageData.lua` → `ALL_ENEMY_TYPES` + 풀 등록
2. `entityFactory.lua` → `createEnemy()` 스탯 추가
3. `basicShapes.lua` → 렌더 함수 추가 (자동 등록)

### 새 보스 추가
1. `bossDefs.lua` → `BOSS_TYPES["NAME"]` 추가
2. `stageData.lua` → `BOSS_STAGES` + `BOSS_SEQUENCE` 등록
3. `bossRenderers.lua` → 렌더 함수 추가 (자동 등록)

### 새 변형 추가
1. `stageData.lua` → `GUARANTEED_VARIANTS` + `VARIANT_TIERS` 등록
2. `entityFactory.lua` → 변형 스탯 보너스 정의
3. `variantOverlays.lua` → 오버레이 렌더 함수 추가 (자동 등록)

### 새 씬 추가
1. `scenes/` 에 파일 생성 (name, enter, exit, update, draw, keypressed)
2. 진입 지점에서 `sceneStack:push(MyScene.new(sceneStack))`
3. 투명/drawBelow 필요하면 씬 테이블에 플래그 설정

### 새 ECS 시스템 추가
1. `systems/` 에 파일 생성: `System.new("Name", {"Comp1", "Comp2"}, updateFn)`
2. `ecsManager.lua` → `addSystem()` 호출 (순서 주의)

### 새 컴포넌트 추가
1. `components/` 에 파일 생성: `{name, defaults, new(data)}`
2. `ecsManager.lua` → `registerComponent()` 호출
