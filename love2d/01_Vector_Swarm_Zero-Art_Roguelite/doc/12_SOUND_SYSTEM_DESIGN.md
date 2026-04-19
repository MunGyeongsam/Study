# 12. 사운드 시스템 설계 — Zero-Art Procedural Audio + 외부 파일 확장

> 작성: 2026-04-19
> 목표: Phase 3D 사운드 시스템 (SFX + BGM)

---

## 핵심 원칙

### Zero-Art 사운드
- **코드 = 사운드**: `love.sound.newSoundData`로 샘플 하나하나를 수학 함수로 생성
- 외부 에셋 파일 0개 — 빌드 시점에 아무것도 필요 없음
- sin, square, saw, noise 파형 조합 + ADSR 엔벨로프 + 주파수 sweep

### 하이브리드 확장 (아들 협업용)
- 동일한 사운드 슬롯(이름)에 대해 **외부 파일이 있으면 그걸 사용, 없으면 코드 생성**
- `ext_res/sounds/sfx/` 폴더에 `.wav` 또는 `.ogg`를 넣으면 자동으로 코드 생성 대신 로드
- 아들이 만든 BGM도 `ext_res/sounds/bgm/` 폴더에 넣으면 바로 사용
- **교체 방향**: 코드 생성 → 외부 파일 (점진적 교체 가능)

---

## 아키텍처

```
src/
├── 05_sound/
│   ├── synth.lua          -- 오실레이터 + 엔벨로프 엔진
│   ├── sfxDefs.lua        -- SFX 정의 (이름 → 합성 파라미터)
│   └── soundManager.lua   -- 통합 관리자 (생성/로드/재생)
│
├── ext_res/
│   └── sounds/            -- 선택적 외부 파일 (아들 협업용)
│       ├── sfx/           -- 외부 효과음 (player_shoot.wav 등)
│       └── bgm/           -- 외부 BGM (stage_normal.ogg 등)
```

### 모듈 책임

| 모듈 | 역할 |
|------|------|
| **synth.lua** | 저수준 오실레이터 (sin/square/saw/noise), ADSR 엔벨로프, 주파수 sweep, 믹서. `generate(params) → SoundData` |
| **sfxDefs.lua** | SFX 사전: `{ name = "player_shoot", generator = function(synth) ... end }`. 합성 레시피만 |
| **soundManager.lua** | 하이브리드 로더: (1) `sounds/sfx/{name}.wav` 있으면 파일 로드 → (2) 없으면 sfxDefs에서 코드 생성 → Source 캐시 + 풀링 재생 |

### 하이브리드 로드 순서

```lua
function soundManager.load(name, category)
    -- 1. 외부 파일 존재 확인
    local path = string.format("ext_res/sounds/%s/%s", category, name)
    for _, ext in ipairs({".ogg", ".wav"}) do
        local info = love.filesystem.getInfo(path .. ext)
        if info then
            return love.audio.newSource(path .. ext, "static")
        end
    end
    -- 2. 코드 생성 (sfxDefs에 레시피 있으면)
    local def = sfxDefs[name]
    if def then
        local soundData = def.generator(synth)
        return love.audio.newSource(soundData)
    end
    -- 3. 없으면 nil (무음)
    logWarn(string.format("[SND] Sound not found: %s/%s", category, name))
    return nil
end
```

---

## synth.lua — 오실레이터 엔진 설계

### 파형 함수 (Waveforms)

| 파형 | 수식 | 캐릭터 |
|------|------|--------|
| **sin** | `sin(2π * f * t)` | 깨끗한 톤 (레벨업, UI) |
| **square** | `sin(t) > 0 ? 1 : -1` | 레트로 8bit 느낌 (발사음) |
| **saw** | `2 * (f*t % 1) - 1` | 거친 톤 (경고, 보스) |
| **noise** | `random(-1, 1)` | 폭발, 충돌, 대쉬 |
| **triangle** | `abs(2*(f*t%1) - 1)*2 - 1` | 부드러운 레트로 (BGM 베이스) |

### ADSR 엔벨로프

```
Amplitude
   1 |  /\
     | /  \________
     |/            \
   0 +----+--+----+--> time
     A  D   S    R

A = Attack  (0 → 1)
D = Decay   (1 → sustainLevel)
S = Sustain (sustainLevel 유지)
R = Release (sustainLevel → 0)
```

### 주파수 모듈레이션

```lua
-- 주파수 sweep: startFreq → endFreq over duration
local freq = startFreq + (endFreq - startFreq) * (t / duration)

-- 비브라토: 주파수 주위 흔들림
local freq = baseFreq * (1 + vibratoDepth * sin(2 * pi * vibratoRate * t))
```

### generate() API

```lua
synth.generate({
    duration  = 0.15,       -- 초
    sampleRate = 44100,
    layers = {
        { wave = "square", freq = 880, freqEnd = 220, amp = 0.4,
          adsr = { a = 0.01, d = 0.03, s = 0.3, r = 0.05 } },
        { wave = "noise", amp = 0.15,
          adsr = { a = 0, d = 0, s = 1, r = 0.05 } },
    },
    master = { volume = 0.5, clip = 0.95 },  -- 클리핑 방지
})
--> returns love.sound.SoundData
```

---

## SFX 정의 (Phase 3D.2)

### 즉시 구현 (6종)

| # | 이름 | 트리거 | 합성 레시피 |
|---|------|--------|------------|
| 1 | `player_shoot` | 플레이어 발사 | square 880→220Hz, 0.08s, 짧은 decay |
| 2 | `enemy_hit` | 적 피격 | noise burst 0.06s + sin 440Hz 펑 |
| 3 | `enemy_kill` | 적 처치 | sin 523→1047Hz 상승, 0.12s (팅!) |
| 4 | `player_hit` | 플레이어 피격 | noise 0.15s + saw 110Hz 쿵 |
| 5 | `dash` | 대쉬 실행 | noise sweep (high→low), 0.1s |
| 6 | `level_up` | 레벨업 | 아르페지오 C5-E5-G5-C6, 각 0.08s sin |

### 추후 추가 (Phase 3D.2+)

| 이름 | 트리거 |
|------|--------|
| `boss_intro` | 보스 등장 |
| `boss_phase` | 보스 페이즈 전환 |
| `boss_clear` | 보스 처치 |
| `graze` | Graze 발동 |
| `focus_on` | 포커스 모드 진입 |
| `xp_collect` | 경험치 오브 수집 |
| `menu_select` | UI 선택 |

---

## BGM 설계 (Phase 3D.1, 3D.3)

### 접근법: 패턴 시퀀서

BGM도 코드로 생성하되, **실시간 시퀀서** 방식:

```lua
-- 시퀀서 패턴 (16스텝, 120 BPM)
local pattern = {
    bass  = { "C2", nil, nil, "C2", nil, nil, "G1", nil,
              "C2", nil, nil, "C2", nil, "Eb2", nil, nil },
    lead  = { "C4", nil, "Eb4", nil, "G4", nil, nil, "Bb4",
              nil, nil, "G4", nil, "Eb4", nil, "C4", nil },
    drums = { "K", nil, "H", nil, "S", nil, "H", nil,
              "K", nil, "H", "H", "S", nil, "H", nil },
}
```

### BGM 트랙 계획

| 트랙 | 상황 | 톤 |
|------|------|-----|
| `stage_normal` | 일반 스테이지 | 미니멀 신스, 낮은 BPM |
| `boss_fight` | 보스 전투 | 빠른 아르페지오, 높은 BPM |
| `game_over` | 게임오버 | 감속, 로우패스 |

> **아들 협업**: `sounds/bgm/stage_normal.ogg`를 만들어 넣으면 코드 생성 BGM 대신 사용.
> 코드 생성 BGM은 플레이스홀더 겸 프로토타입으로 유지.

---

## 게임 통합 계획

### soundManager API

```lua
local sm = require("05_sound.soundManager")

sm.init()                    -- love.load에서 호출 (SFX 전부 사전 생성/로드)
sm.play("player_shoot")     -- 즉시 재생 (풀링: 같은 소리 최대 4개 동시)
sm.play("enemy_kill")
sm.setVolume("sfx", 0.8)    -- 카테고리별 볼륨
sm.setVolume("bgm", 0.5)
sm.playBGM("stage_normal")  -- 루프 재생
sm.stopBGM()
```

### 트리거 위치

| 이벤트 | 파일 | 위치 |
|--------|------|------|
| 플레이어 발사 | playerWeaponSystem.lua | 탄 생성 직후 |
| 적 피격 | enemyCollisionSystem.lua | HP 감소 시 |
| 적 처치 | enemyCollisionSystem.lua | health.hp <= 0 |
| 플레이어 피격 | collisionSystem.lua | 충돌 판정 시 |
| 대쉬 | dashSystem.lua | 대쉬 실행 시 |
| 레벨업 | levelUp.lua | show() 호출 시 |

### 성능 고려

- SFX: `init()` 시 전부 사전 생성 → `"static"` Source. 재생 시 GC 0
- BGM: `"stream"` 모드 (외부 파일) 또는 큰 SoundData 루프 (코드 생성)
- 동시 재생 제한: 같은 SFX 최대 4개 (Source 풀링)
- 생성 시간: SFX 1개당 < 1ms (0.1초 * 44100 = 4410 샘플)

---

## 파일 구조 (외부 파일 확장용)

```
src/ext_res/sounds/
├── sfx/
│   ├── player_shoot.wav    -- 아들이 만든 발사음 (있으면 우선 사용)
│   ├── enemy_hit.wav
│   └── ...
└── bgm/
    ├── stage_normal.ogg    -- 아들이 만든 BGM
    └── boss_fight.ogg
```

> `ext_res/sounds/sfx/`, `ext_res/sounds/bgm/` 폴더는 `.gitkeep`으로 빈 상태 커밋.
> 아들이 파일을 넣으면 자동으로 코드 생성 대신 해당 파일 사용.

---

## 구현 순서

### Step 1: synth.lua — 오실레이터 엔진
- 파형 5종 (sin, square, saw, noise, triangle)
- ADSR 엔벨로프
- 주파수 sweep
- 멀티 레이어 믹싱
- `generate(params) → SoundData`

### Step 2: soundManager.lua — 하이브리드 로더
- 외부 파일 우선 → 코드 생성 폴백
- Source 풀링 (같은 SFX 최대 4개 동시)
- 카테고리별 볼륨 (sfx, bgm)
- `init()`, `play(name)`, `playBGM(name)`, `stopBGM()`

### Step 3: sfxDefs.lua — SFX 6종 레시피
- player_shoot, enemy_hit, enemy_kill, player_hit, dash, level_up

### Step 4: 게임 통합
- 6개 트리거 위치에 `soundManager.play()` 삽입
- main.lua에 `soundManager.init()` 추가

### Step 5 (후속): BGM 시퀀서
- 패턴 기반 실시간 합성 or 긴 SoundData 프리렌더
- 보스전 BGM 전환

---

## 설계 결정 필요 사항

| 결정 | 선택지 | 추천 |
|------|--------|------|
| 샘플 레이트 | 22050 vs 44100 | **44100** (품질 우선, SFX는 짧아서 메모리 무관) |
| 비트 수 | 8 vs 16 | **16** (노이즈 플로어 낮음) |
| SFX 풀 크기 | 2~8 | **4** (동시 발사음 4개면 충분) |
| BGM 방식 | 프리렌더 vs 실시간 | **프리렌더** (Step 5에서 결정, 우선 SFX만) |
| ext_res/ 폴더 위치 | src/ 안 vs 밖 | **src/ext_res/sounds/** (LÖVE 루트 안, 코드와 리소스 분리) |
