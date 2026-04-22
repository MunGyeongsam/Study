-- BossTag Component
-- Marker + phase management for boss entities.
-- Tracks current phase, pattern cycling, and defeat sequence.

local BossTag = {}

BossTag.name = "BossTag"

BossTag.defaults = {
    bossType = "NULL",       -- "NULL"|"STACK"|"HEAP"|"RECURSION"|"OVERFLOW"
    phase = 1,               -- current phase (1-indexed)
    maxPhase = 2,            -- total phases
    phaseThresholds = {0.5}, -- HP ratio triggers (phase 1→2 at 50%)
    phaseChanged = false,    -- 1-frame flag on phase transition

    -- Pattern cycling
    patternIndex = 1,        -- current pattern in sequence
    patternTimer = 0,        -- time elapsed in current pattern step
    patterns = {},           -- per-phase pattern sequences (set by factory)

    -- Teleport (HEAP)
    teleportTimer    = 0,    -- counts up toward teleportInterval
    teleportInterval = 0,    -- seconds between teleports (0 = disabled)
    teleportWarning  = 1.0,  -- blink duration before teleport
    teleportCooldown = 0.3,  -- post-teleport invulnerability
    teleporting      = false,-- true during warning blink phase

    -- Minion spawning (RECURSION)
    minion           = nil,  -- per-phase config table: {[phase]={max, interval, type, hpMult}}
    minionTimer      = 0,    -- counts up toward current phase interval

    -- Phase-specific overrides (OVERFLOW)
    phaseTeleport    = nil,  -- {[phase]=interval} overrides teleportInterval on transition
    phaseColors      = nil,  -- {[phase]={r,g,b,a}} overrides Renderable.color on transition

    -- Intro/defeat
    introTimer = 0,          -- intro sequence timer
    introDuration = 1.5,     -- intro duration before active
    introComplete = false,   -- intro finished flag
    defeated = false,        -- kill confirmed flag
}

function BossTag.new(data)
    local d = data or {}
    local def = BossTag.defaults
    return {
        bossType        = d.bossType        or def.bossType,
        phase           = 1,
        maxPhase        = d.maxPhase        or def.maxPhase,
        phaseThresholds = d.phaseThresholds or {0.5},
        phaseChanged    = false,
        patternIndex    = 1,
        patternTimer    = 0,
        patterns        = d.patterns        or {},
        teleportTimer    = 0,
        teleportInterval = d.teleportInterval or def.teleportInterval,
        teleportWarning  = d.teleportWarning  or def.teleportWarning,
        teleportCooldown = d.teleportCooldown or def.teleportCooldown,
        teleporting      = false,
        minion           = d.minion or nil,
        minionTimer      = 0,
        phaseTeleport    = d.phaseTeleport or nil,
        phaseColors      = d.phaseColors or nil,
        speedMult        = d.speedMult or 1,
        bulletSpeedMult  = d.bulletSpeedMult or 1,
        emitRateMult     = d.emitRateMult or 1,
        minionAdd        = d.minionAdd or 0,
        introTimer      = 0,
        introDuration   = d.introDuration   or def.introDuration,
        introComplete   = false,
        defeated        = false,
    }
end

return BossTag
