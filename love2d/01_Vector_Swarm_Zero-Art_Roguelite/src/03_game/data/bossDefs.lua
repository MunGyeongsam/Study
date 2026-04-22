-- ============================================================================
-- Boss Type Definitions — 보스 프리셋 데이터 (순수 데이터 — 로직 없음)
-- ============================================================================
--
-- ◆ 이 파일의 역할
--   보스별 스탯·패턴·AI 행동을 정의한다. entityFactory.createBoss()가 참조.
--   여기 숫자를 바꾸면 보스 난이도/패턴이 바뀐다.
--
-- ◆ 새 보스 추가 방법
--   1. 이 파일에 BOSS_TYPES["NAME"] = { ... } 추가 (아래 구조 참고)
--   2. stageData.lua → BOSS_STAGES + BOSS_SEQUENCE에 등록
--   3. bossRenderers.lua에 boss_name 렌더 함수 추가
--   4. bossSystem.lua에서 페이즈/AI 로직이 자동 적용됨 (패턴 기반)
--
-- ◆ 보스 구조 필드
--   color, radius, renderType   : 외형
--   hp, xpValue, maxPhase       : 전투 스탯
--   phaseThresholds             : HP% 기준 페이즈 전환 (예: {0.5} → 50%에서 2페이즈)
--   ai                          : 이동 행동 (behavior, speed, driftVx 등)
--   patterns[phase]             : 탄막 시퀀스 배열 (pattern, emitRate, bulletSpeed 등)
--
-- ◆ 의존 관계
--   순수 데이터. 어떤 모듈도 require 하지 않는다.
--   entityFactory.lua가 이 파일을 require 한다.
-- ============================================================================

local BOSS_TYPES = {
    NULL = {
        color    = {0.6, 0.6, 0.6, 1},
        radius   = 0.8,
        renderType = "boss_null",
        hp       = 50,
        xpValue  = 50,
        maxPhase = 2,
        phaseThresholds = {0.5},
        ai       = { behavior = "drift", driftVx = 0.4, driftVy = 0, speed = 0.4 },
        patterns = {
            -- Phase 1: learn
            [1] = {
                { pattern = "circle", emitRate = 0.4, bulletSpeed = 2.0, bulletCount = 8,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.7, 0.7, 0.7, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 2.0 },
                { pattern = "circle", emitRate = 0.5, bulletSpeed = 2.5, bulletCount = 12,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.7, 0.7, 0.7, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: serious
            [2] = {
                { pattern = "circle", emitRate = 0.6, bulletSpeed = 2.5, bulletCount = 8,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.9, 0.5, 0.5, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 1.5 },
                { pattern = "spiral", emitRate = 1.0, bulletSpeed = 2.0, bulletCount = 4,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.9, 0.5, 0.5, 1},
                  turnRate = 1.0, duration = 4.0 },
                { pattern = "none", duration = 1.5 },
            },
        },
    },
    STACK = {
        color    = {0.2, 0.8, 0.2, 1},
        radius   = 1.0,
        renderType = "boss_stack",
        hp       = 100,
        xpValue  = 120,
        maxPhase = 3,
        phaseThresholds = {0.66, 0.33},
        ai       = { behavior = "drift", driftVx = 0.3, driftVy = 0, speed = 0.3 },
        patterns = {
            -- Phase 1: slow concentric rings
            [1] = {
                { pattern = "circle", emitRate = 0.4, bulletSpeed = 1.8, bulletCount = 10,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.2, 0.9, 0.2, 1},
                  duration = 3.5 },
                { pattern = "none", duration = 2.0 },
                { pattern = "circle", emitRate = 0.5, bulletSpeed = 2.2, bulletCount = 14,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.2, 0.9, 0.2, 1},
                  duration = 3.5 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: rings + aimed
            [2] = {
                { pattern = "circle", emitRate = 0.6, bulletSpeed = 2.2, bulletCount = 12,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.4, 1.0, 0.4, 1},
                  duration = 3.0 },
                { pattern = "aimed", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 3,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 1.0, 0.3, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 3: dense rings + aimed burst
            [3] = {
                { pattern = "circle", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 16,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.6, 1.0, 0.6, 1},
                  duration = 3.0 },
                { pattern = "aimed", emitRate = 1.2, bulletSpeed = 3.5, bulletCount = 5,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 0.3, 1},
                  duration = 2.5 },
                { pattern = "none", duration = 1.0 },
            },
        },
    },
    HEAP = {
        color    = {0.9, 0.3, 0.1, 1},
        radius   = 0.9,
        renderType = "boss_heap",
        hp       = 160,
        xpValue  = 200,
        maxPhase = 3,
        phaseThresholds = {0.66, 0.33},
        ai       = { behavior = "drift", driftVx = 0.3, driftVy = 0, speed = 0.3 },
        -- Teleport params (consumed by bossSystem)
        teleportInterval = 5.0,
        teleportWarning  = 1.0,
        teleportCooldown = 0.3,
        patterns = {
            -- Phase 1: random-feel burst (fast circle, short pause, repeat)
            [1] = {
                { pattern = "circle", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 10,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.9, 0.4, 0.2, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.0 },
                { pattern = "circle", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 6,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 0.2, 1},
                  duration = 1.5 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 2: burst + wave (chaotic mix)
            [2] = {
                { pattern = "circle", emitRate = 0.9, bulletSpeed = 2.8, bulletCount = 12,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.4, 0.1, 1},
                  duration = 2.5 },
                { pattern = "wave", emitRate = 0.6, bulletSpeed = 2.5, bulletCount = 5,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {1.0, 0.6, 0.2, 1},
                  turnRate = 2.0, duration = 3.0 },
                { pattern = "none", duration = 1.0 },
            },
            -- Phase 3: dense spread + orbit_shot
            [3] = {
                { pattern = "circle", emitRate = 1.2, bulletSpeed = 3.0, bulletCount = 18,
                  bulletLifetime = 4, bulletRadius = 0.035, bulletColor = {1.0, 0.3, 0.1, 1},
                  duration = 2.5 },
                { pattern = "orbit_shot", emitRate = 0.8, bulletSpeed = 2.0, bulletCount = 6,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {1.0, 0.8, 0.2, 1},
                  orbitRadius = 1.2, orbitSpeed = 3.5, orbitTime = 1.0, turnRate = 0.5,
                  duration = 3.0 },
                { pattern = "none", duration = 0.8 },
            },
        },
    },
    RECURSION = {
        color    = {0.5, 0.1, 0.9, 1},
        radius   = 0.7,
        renderType = "boss_recursion",
        hp       = 200,
        xpValue  = 300,
        maxPhase = 3,
        phaseThresholds = {0.66, 0.33},
        ai       = { behavior = "orbit", speed = 0.5, orbitRadius = 3.5, orbitSpeed = 0.8 },
        -- Minion spawning params (consumed by bossSystem)
        minion = {
            [1] = { max = 0, interval = 0,   type = "basic", hpMult = 0.5 },
            [2] = { max = 3, interval = 8.0, type = "basic", hpMult = 0.5 },
            [3] = { max = 5, interval = 6.0, type = "basic", hpMult = 0.5 },
        },
        patterns = {
            -- Phase 1: slow spiral (learn the orbit movement)
            [1] = {
                { pattern = "spiral", emitRate = 0.6, bulletSpeed = 2.0, bulletCount = 4,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.6, 0.2, 1.0, 1},
                  turnRate = 1.2, duration = 4.0 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: faster spiral (+ minions provide circle pressure)
            [2] = {
                { pattern = "spiral", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 6,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.7, 0.2, 1.0, 1},
                  turnRate = 1.5, duration = 3.5 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 3: fast spiral + aimed bursts (+ more minions)
            [3] = {
                { pattern = "spiral", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 8,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.8, 0.3, 1.0, 1},
                  turnRate = 2.0, duration = 3.0 },
                { pattern = "aimed", emitRate = 1.2, bulletSpeed = 3.5, bulletCount = 3,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 1.0, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.0 },
            },
        },
    },
    OVERFLOW = {
        color    = {1.0, 0.2, 0.2, 1},
        radius   = 1.2,
        renderType = "boss_overflow",
        hp       = 300,
        xpValue  = 500,
        maxPhase = 4,
        phaseThresholds = {0.75, 0.50, 0.25},
        ai       = { behavior = "drift", driftVx = 0.3, driftVy = 0, speed = 0.3 },
        -- Phase-specific teleport (0 = drift, >0 = teleport interval)
        phaseTeleport = {
            [1] = 0, [2] = 0, [3] = 5.0, [4] = 3.0,
        },
        -- Phase-specific color changes
        phaseColors = {
            [1] = {1.0, 0.2, 0.2, 1},  -- red (NULL tribute)
            [2] = {0.2, 1.0, 0.2, 1},  -- green (STACK tribute)
            [3] = {0.9, 0.3, 0.9, 1},  -- purple (HEAP+RECURSION)
            [4] = {1.0, 1.0, 1.0, 1},  -- white (all combined)
        },
        teleportWarning  = 1.0,
        teleportCooldown = 0.3,
        -- Minions: P3-P4 only
        minion = {
            [1] = { max = 0, interval = 0,   type = "basic", hpMult = 0.5 },
            [2] = { max = 0, interval = 0,   type = "basic", hpMult = 0.5 },
            [3] = { max = 3, interval = 8.0, type = "basic", hpMult = 0.5 },
            [4] = { max = 3, interval = 8.0, type = "basic", hpMult = 0.5 },
        },
        patterns = {
            -- Phase 1: NULL tribute (circle only, comfortable)
            [1] = {
                { pattern = "circle", emitRate = 0.6, bulletSpeed = 2.5, bulletCount = 10,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.3, 0.3, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 2.0 },
                { pattern = "circle", emitRate = 0.7, bulletSpeed = 2.8, bulletCount = 14,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 0.3, 0.3, 1},
                  duration = 3.0 },
                { pattern = "none", duration = 2.0 },
            },
            -- Phase 2: STACK tribute (circle + aimed)
            [2] = {
                { pattern = "circle", emitRate = 0.7, bulletSpeed = 2.5, bulletCount = 14,
                  bulletLifetime = 5, bulletRadius = 0.04, bulletColor = {0.3, 1.0, 0.3, 1},
                  duration = 3.0 },
                { pattern = "aimed", emitRate = 1.0, bulletSpeed = 3.2, bulletCount = 4,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 1.0, 0.3, 1},
                  duration = 2.0 },
                { pattern = "none", duration = 1.5 },
            },
            -- Phase 3: HEAP+RECURSION (burst + wave, teleport + minions)
            [3] = {
                { pattern = "circle", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 14,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {0.9, 0.4, 0.9, 1},
                  duration = 2.5 },
                { pattern = "wave", emitRate = 0.8, bulletSpeed = 2.5, bulletCount = 5,
                  bulletLifetime = 5, bulletRadius = 0.035, bulletColor = {0.9, 0.5, 0.9, 1},
                  turnRate = 2.0, duration = 3.0 },
                { pattern = "none", duration = 1.0 },
            },
            -- Phase 4: OVERFLOW — rapid pattern cycling + return_shot
            [4] = {
                { pattern = "spiral", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 6,
                  bulletLifetime = 4, bulletRadius = 0.035, bulletColor = {1.0, 1.0, 1.0, 1},
                  turnRate = 2.0, duration = 2.0 },
                { pattern = "return_shot", emitRate = 0.7, bulletSpeed = 2.5, bulletCount = 8,
                  bulletLifetime = 3.5, bulletRadius = 0.04, bulletColor = {1.0, 0.5, 0.5, 1},
                  returnTime = 0.7, turnRate = 0.8, duration = 2.5 },
                { pattern = "circle", emitRate = 1.0, bulletSpeed = 3.0, bulletCount = 16,
                  bulletLifetime = 4, bulletRadius = 0.035, bulletColor = {1.0, 0.8, 0.8, 1},
                  duration = 2.0 },
                { pattern = "aimed", emitRate = 1.2, bulletSpeed = 3.5, bulletCount = 4,
                  bulletLifetime = 4, bulletRadius = 0.04, bulletColor = {1.0, 1.0, 0.5, 1},
                  duration = 1.5 },
                { pattern = "none", duration = 0.8 },
            },
        },
    },
}

return BOSS_TYPES
