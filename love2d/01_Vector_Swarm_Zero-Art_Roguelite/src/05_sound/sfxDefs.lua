-- SFX Definitions — Zero-Art Procedural Sound Recipes
-- Each entry: name → function(synth) → SoundData
-- These are fallbacks; ext_res/sounds/sfx/{name}.wav takes priority.

local floor = math.floor
local sin   = math.sin
local pi2   = math.pi * 2

local sfxDefs = {}

-- 1. player_shoot: 짧은 square wave 하강 (레트로 발사음)
sfxDefs.player_shoot = function(synth)
    return synth.generate({
        duration = 0.08,
        layers = {
            { wave = "square", freq = 880, freqEnd = 220, amp = 0.35,
              adsr = { a = 0.005, d = 0.02, s = 0.2, r = 0.02 } },
            { wave = "noise", amp = 0.08,
              adsr = { a = 0, d = 0.01, s = 0, r = 0.01 } },
        },
        master = { volume = 0.5 },
    })
end

-- 2. enemy_hit: noise burst + 낮은 sin 펑
sfxDefs.enemy_hit = function(synth)
    return synth.generate({
        duration = 0.06,
        layers = {
            { wave = "noise", amp = 0.4,
              adsr = { a = 0, d = 0.02, s = 0, r = 0.02 } },
            { wave = "sin", freq = 220, freqEnd = 110, amp = 0.25,
              adsr = { a = 0, d = 0.03, s = 0, r = 0.02 } },
        },
        master = { volume = 0.4 },
    })
end

-- 3. enemy_kill: 상승 sin (팅! 느낌)
sfxDefs.enemy_kill = function(synth)
    return synth.generate({
        duration = 0.12,
        layers = {
            { wave = "sin", freq = 523, freqEnd = 1047, amp = 0.4,
              adsr = { a = 0.005, d = 0.03, s = 0.3, r = 0.04 } },
            { wave = "triangle", freq = 1047, freqEnd = 2093, amp = 0.15,
              adsr = { a = 0.005, d = 0.02, s = 0.1, r = 0.03 } },
        },
        master = { volume = 0.45 },
    })
end

-- 4. player_hit: noise + 낮은 saw 쿵
sfxDefs.player_hit = function(synth)
    return synth.generate({
        duration = 0.15,
        layers = {
            { wave = "noise", amp = 0.5,
              adsr = { a = 0, d = 0.03, s = 0.15, r = 0.06 } },
            { wave = "saw", freq = 110, freqEnd = 55, amp = 0.35,
              adsr = { a = 0, d = 0.05, s = 0.1, r = 0.05 } },
        },
        master = { volume = 0.5 },
    })
end

-- 5. dash: noise sweep high→low (슉~)
sfxDefs.dash = function(synth)
    return synth.generate({
        duration = 0.1,
        layers = {
            { wave = "noise", amp = 0.35,
              adsr = { a = 0, d = 0.03, s = 0.15, r = 0.04 } },
            { wave = "sin", freq = 2000, freqEnd = 200, amp = 0.2,
              adsr = { a = 0, d = 0.02, s = 0.1, r = 0.03 } },
        },
        master = { volume = 0.45 },
    })
end

-- 6. level_up: 아르페지오 C5-E5-G5-C6 (따다다단!)
sfxDefs.level_up = function(_synth)
    -- 아르페지오는 순차 노트 → synth.generate 대신 직접 생성
    local rate = 44100
    local noteLen = 0.08
    local notes = { 523, 659, 784, 1047 }  -- C5, E5, G5, C6
    local totalSamples = floor(#notes * noteLen * rate)
    local sd = love.sound.newSoundData(totalSamples, rate, 16, 1)

    local idx = 0

    for n = 1, #notes do
        local freq = notes[n]
        local noteSamples = floor(noteLen * rate)
        for s = 0, noteSamples - 1 do
            local t = s / rate
            local env = 1 - (t / noteLen)  -- linear decay
            env = env * env                 -- exponential feel
            local val = sin(pi2 * freq * t) * 0.35 * env
                      + sin(pi2 * freq * 2 * t) * 0.1 * env  -- octave shimmer
            if val > 0.95 then val = 0.95
            elseif val < -0.95 then val = -0.95 end
            if idx < totalSamples then
                sd:setSample(idx, val)
                idx = idx + 1
            end
        end
    end

    return sd
end

-- ═══════════════════════════════════════════════════════════════════
-- BGM Definitions (pattern sequencer)
-- Naming: bgm_{name} — soundManager detects "bgm_" prefix
-- ═══════════════════════════════════════════════════════════════════

-- Stage BGM: Am synthwave — 아르페지오 + 베이스 + 하이햇
-- Key: A minor, 4 bars, 120bpm
sfxDefs.bgm_stage = function(synth)
    -- ─── Arpeggio (square wave, 1 octave up) ─────────────────
    local arp = {}
    local arpNotes = { "A4", "C5", "E5", "A5", "E5", "C5" }  -- Am arp up-down
    local beat = 1
    for bar = 0, 3 do
        for i = 1, #arpNotes do
            arp[#arp + 1] = { note = arpNotes[i], beat = beat, dur = 0.4 }
            beat = beat + 0.5
        end
    end

    -- ─── Bass (saw wave, root + 5th) ─────────────────────────
    local bass = {}
    local bassPattern = {
        { "A2", 1 }, { "A2", 2 }, { "E2", 3 }, { "E2", 4 },     -- bar 1
        { "F2", 5 }, { "F2", 6 }, { "G2", 7 }, { "G2", 8 },     -- bar 2
        { "A2", 9 }, { "A2", 10 }, { "E2", 11 }, { "E2", 12 },  -- bar 3
        { "F2", 13 }, { "F2", 14 }, { "G2", 15 }, { "E2", 16 }, -- bar 4
    }
    for _, bp in ipairs(bassPattern) do
        bass[#bass + 1] = { note = bp[1], beat = bp[2], dur = 0.9 }
    end

    -- ─── Hi-hat (noise, every half beat) ─────────────────────
    local hat = {}
    for b = 1, 16, 0.5 do
        local accent = (b % 1 == 0) and 0.15 or 0.08
        hat[#hat + 1] = { beat = b, dur = 0.08, amp = accent }
    end

    -- ─── Pad (triangle, sustained chords) ────────────────────
    local pad = {}
    local padChords = {
        { "A3", 1, 4 }, { "C4", 1, 4 }, { "E4", 1, 4 },  -- Am (bars 1-2)
        { "F3", 9, 4 }, { "A3", 9, 4 }, { "C4", 9, 4 },  -- F  (bars 3-4, first half)
    }
    for _, pc in ipairs(padChords) do
        pad[#pad + 1] = { note = pc[1], beat = pc[2], dur = pc[3] * 0.95 }
    end

    return synth.generateSequence({
        bpm = 120, beatsPerBar = 4, bars = 4,
        tracks = {
            { wave = "square",   amp = 0.12, adsr = { a = 0.01, d = 0.04, s = 0.25, r = 0.03 }, notes = arp },
            { wave = "saw",      amp = 0.18, adsr = { a = 0.01, d = 0.05, s = 0.5,  r = 0.08 }, notes = bass },
            { wave = "noise",    amp = 0.15, adsr = { a = 0,    d = 0.02, s = 0,    r = 0.01 }, notes = hat },
            { wave = "triangle", amp = 0.08, adsr = { a = 0.1,  d = 0.1,  s = 0.4,  r = 0.15 }, notes = pad },
        },
        master = { volume = 0.55, clip = 0.95 },
    })
end

-- Boss BGM: 같은 키, 템포 빠르고 공격적
-- 베이스 옥타브 ↓, square 디스토션, 16th note hihat
sfxDefs.bgm_boss = function(synth)
    -- ─── Aggressive arpeggio (square, fast) ──────────────────
    local arp = {}
    local arpNotes = { "A4", "C5", "E5", "G5", "E5", "C5", "A4", "E4" }
    local beat = 1
    for bar = 0, 3 do
        for i = 1, #arpNotes do
            arp[#arp + 1] = { note = arpNotes[i], beat = beat, dur = 0.35 }
            beat = beat + 0.5
        end
    end

    -- ─── Heavy bass (square, octave lower than stage) ────────
    local bass = {}
    local bassPattern = {
        { "A1", 1 }, { "A1", 1.5 }, { "A1", 2 }, { "E1", 3 }, { "E1", 4 },   -- bar 1
        { "F1", 5 }, { "G1", 6 }, { "G1", 6.5 }, { "G1", 7 }, { "A1", 8 },   -- bar 2
        { "A1", 9 }, { "A1", 9.5 }, { "E1", 10 }, { "E1", 11 }, { "G1", 12 }, -- bar 3
        { "F1", 13 }, { "F1", 14 }, { "E1", 15 }, { "E1", 15.5 }, { "E1", 16 }, -- bar 4
    }
    for _, bp in ipairs(bassPattern) do
        bass[#bass + 1] = { note = bp[1], beat = bp[2], dur = 0.7 }
    end

    -- ─── Fast hi-hat + accented kick ─────────────────────────
    local hat = {}
    for b = 1, 16, 0.25 do
        local accent = (b % 1 == 0) and 0.18 or 0.06
        hat[#hat + 1] = { beat = b, dur = 0.04, amp = accent }
    end

    -- ─── Kick drum (sin, low freq burst) ─────────────────────
    local kick = {}
    for b = 1, 16, 2 do
        kick[#kick + 1] = { note = "C1", beat = b, dur = 0.15, freqEnd = 30 }
    end

    -- ─── Tension lead (saw, dissonant) ───────────────────────
    local lead = {}
    local leadNotes = {
        { "E5", 1, 2 }, { "D5", 3, 1 }, { "C5", 4, 1 },
        { "B4", 5, 2 }, { "A4", 7, 1 }, { "G#4", 8, 1 },
        { "A4", 9, 2 }, { "C5", 11, 1 }, { "B4", 12, 1 },
        { "E5", 13, 1.5 }, { "D5", 14.5, 1.5 },
    }
    for _, ln in ipairs(leadNotes) do
        lead[#lead + 1] = { note = ln[1], beat = ln[2], dur = ln[3] * 0.9 }
    end

    return synth.generateSequence({
        bpm = 145, beatsPerBar = 4, bars = 4,
        tracks = {
            { wave = "square",   amp = 0.11, adsr = { a = 0.005, d = 0.03, s = 0.2,  r = 0.02 }, notes = arp },
            { wave = "square",   amp = 0.20, adsr = { a = 0.005, d = 0.05, s = 0.5,  r = 0.05 }, notes = bass },
            { wave = "noise",    amp = 0.12, adsr = { a = 0,     d = 0.015, s = 0,   r = 0.01 }, notes = hat },
            { wave = "sin",      amp = 0.15, adsr = { a = 0.005, d = 0.05, s = 0.2,  r = 0.03 }, notes = kick },
            { wave = "saw",      amp = 0.09, adsr = { a = 0.02,  d = 0.06, s = 0.35, r = 0.08 }, notes = lead },
        },
        master = { volume = 0.50, clip = 0.95 },
    })
end

return sfxDefs
