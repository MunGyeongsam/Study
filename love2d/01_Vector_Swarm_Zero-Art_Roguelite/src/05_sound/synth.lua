-- Synth Engine — Zero-Art Procedural Audio
-- Generates SoundData from math functions: waveforms + ADSR + frequency sweep
-- No external files needed.

local sin   = math.sin
local abs   = math.abs
local floor = math.floor
local random = math.random
local pi2   = math.pi * 2

local synth = {}

-- ─── Waveforms ───────────────────────────────────────────────────
-- All return values in [-1, 1]

local function waveSin(phase)
    return sin(phase)
end

local function waveSquare(phase)
    return sin(phase) >= 0 and 1 or -1
end

local function waveSaw(phase)
    return 2 * ((phase / pi2) % 1) - 1
end

local function waveTriangle(phase)
    return abs(2 * ((phase / pi2) % 1) - 1) * 2 - 1
end

local function waveNoise()
    return random() * 2 - 1
end

local WAVES = {
    sin      = waveSin,
    square   = waveSquare,
    saw      = waveSaw,
    triangle = waveTriangle,
    noise    = waveNoise,
}

-- ─── ADSR Envelope ───────────────────────────────────────────────
-- Returns amplitude multiplier [0, 1] for time t within total duration
--   a = attack time, d = decay time, s = sustain level, r = release time
--   Total envelope length = a + d + sustain_hold + r
--   sustain_hold = duration - a - d - r

local function adsr(t, duration, env)
    local a = env.a or 0.01
    local d = env.d or 0.02
    local s = env.s or 0.5
    local r = env.r or 0.05

    local releaseStart = duration - r

    if t < a then
        -- Attack: 0 → 1
        return t / a
    elseif t < a + d then
        -- Decay: 1 → sustain level
        return 1 - (1 - s) * ((t - a) / d)
    elseif t < releaseStart then
        -- Sustain
        return s
    else
        -- Release: sustain → 0
        local rt = (t - releaseStart) / r
        if rt > 1 then rt = 1 end
        return s * (1 - rt)
    end
end

-- ─── Generate SoundData ──────────────────────────────────────────
-- params = {
--   duration   = 0.15,          -- seconds
--   sampleRate = 44100,         -- (optional, default 44100)
--   layers = {
--     { wave = "square",        -- waveform name
--       freq = 880,             -- start frequency (Hz)
--       freqEnd = 220,          -- end frequency (Hz, optional for sweep)
--       amp = 0.4,              -- layer amplitude
--       adsr = { a=0.01, d=0.03, s=0.3, r=0.05 },
--     },
--     ...
--   },
--   master = { volume = 0.5, clip = 0.95 },  -- optional
-- }

function synth.generate(params)
    local duration   = params.duration or 0.1
    local sampleRate = params.sampleRate or 44100
    local layers     = params.layers or {}
    local master     = params.master or {}
    local masterVol  = master.volume or 0.5
    local clip       = master.clip or 0.95

    local totalSamples = floor(duration * sampleRate)
    if totalSamples < 1 then totalSamples = 1 end

    local sd = love.sound.newSoundData(totalSamples, sampleRate, 16, 1)

    -- Pre-resolve wave functions
    local layerCount = #layers
    local waveFns    = {}
    local freqs      = {}
    local freqEnds   = {}
    local amps       = {}
    local envs       = {}
    local phases     = {}

    for i = 1, layerCount do
        local L = layers[i]
        waveFns[i]  = WAVES[L.wave] or waveSin
        freqs[i]    = L.freq or 440
        freqEnds[i] = L.freqEnd or freqs[i]
        amps[i]     = L.amp or 0.3
        envs[i]     = L.adsr or { a = 0.01, d = 0.02, s = 0.5, r = 0.05 }
        phases[i]   = 0
    end

    -- Fill samples
    for s = 0, totalSamples - 1 do
        local t = s / sampleRate
        local mix = 0

        for i = 1, layerCount do
            -- Frequency sweep (linear interpolation)
            local progress = t / duration
            local freq = freqs[i] + (freqEnds[i] - freqs[i]) * progress

            -- Accumulate phase (prevents discontinuities in sweep)
            phases[i] = phases[i] + pi2 * freq / sampleRate
            local waveVal = waveFns[i](phases[i])

            -- ADSR envelope
            local env = adsr(t, duration, envs[i])

            mix = mix + waveVal * amps[i] * env
        end

        -- Master volume + clipping
        mix = mix * masterVol
        if mix > clip then mix = clip
        elseif mix < -clip then mix = -clip end

        sd:setSample(s, mix)
    end

    return sd
end

-- ─── Note Frequency Table (A4 = 440Hz) ──────────────────────────
-- MIDI note number → Hz.  Middle C (C4) = 60
local function noteToFreq(note)
    return 440 * 2 ^ ((note - 69) / 12)
end

-- Convenience: name → MIDI note (e.g. "C4"=60, "A4"=69, "F#3"=54)
local NOTE_NAMES = { C=0, D=2, E=4, F=5, G=7, A=9, B=11 }
local function parseNote(str)
    if type(str) == "number" then return str end
    local letter = str:sub(1,1):upper()
    local offset = NOTE_NAMES[letter] or 0
    local sharp = 0
    local rest = str:sub(2)
    if rest:sub(1,1) == "#" then sharp = 1; rest = rest:sub(2) end
    if rest:sub(1,1) == "b" then sharp = -1; rest = rest:sub(2) end
    local octave = tonumber(rest) or 4
    return (octave + 1) * 12 + offset + sharp
end

-- ─── Pattern Sequencer ───────────────────────────────────────────
-- Generates a loopable SoundData from a pattern definition.
--
-- pattern = {
--   bpm        = 120,
--   beatsPerBar = 4,
--   bars       = 4,              -- total bars to generate
--   sampleRate = 44100,
--   tracks = {
--     { -- track 1: melody
--       wave = "square",
--       amp  = 0.25,
--       adsr = { a=0.01, d=0.05, s=0.3, r=0.05 },
--       notes = {
--         { note="C4", beat=1, dur=0.5 },
--         { note="E4", beat=1.5, dur=0.5 },
--         ...
--       },
--     },
--     { -- track 2: bass
--       wave = "saw", amp = 0.2, ...
--       notes = { ... },
--     },
--     { -- track 3: drums (noise-based)
--       wave = "noise", amp = 0.3,
--       adsr = { a=0, d=0.03, s=0, r=0.02 },
--       notes = {
--         { beat=1, dur=0.05 },  -- kick-like
--         ...
--       },
--     },
--   },
--   master = { volume = 0.5, clip = 0.95 },
-- }

function synth.generateSequence(pattern)
    local bpm         = pattern.bpm or 120
    local beatsPerBar = pattern.beatsPerBar or 4
    local bars        = pattern.bars or 4
    local sampleRate  = pattern.sampleRate or 44100
    local master      = pattern.master or {}
    local masterVol   = master.volume or 0.5
    local clip        = master.clip or 0.95

    local secPerBeat  = 60 / bpm
    local totalBeats  = beatsPerBar * bars
    local totalSec    = totalBeats * secPerBeat
    local totalSamples = floor(totalSec * sampleRate)
    if totalSamples < 1 then totalSamples = 1 end

    local sd = love.sound.newSoundData(totalSamples, sampleRate, 16, 1)

    -- Build note events: { startSample, endSample, freq, waveFn, amp, env }
    local events = {}
    for _, track in ipairs(pattern.tracks or {}) do
        local waveFn = WAVES[track.wave] or waveSin
        local amp    = track.amp or 0.25
        local env    = track.adsr or { a = 0.01, d = 0.05, s = 0.3, r = 0.05 }
        local freqEnd = track.freqEnd  -- optional: all notes sweep to this factor

        for _, n in ipairs(track.notes or {}) do
            local beatStart = (n.beat or 1) - 1  -- 1-indexed → 0-indexed
            local dur       = (n.dur or 0.5) * secPerBeat
            local startSec  = beatStart * secPerBeat
            local noteFreq  = n.freq or (n.note and noteToFreq(parseNote(n.note))) or 440
            local noteFreqEnd = noteFreq
            if freqEnd then
                noteFreqEnd = noteFreq * freqEnd
            end
            if n.freqEnd then
                noteFreqEnd = n.freqEnd
            end

            events[#events + 1] = {
                s0   = floor(startSec * sampleRate),
                s1   = floor((startSec + dur) * sampleRate) - 1,
                freq = noteFreq,
                freqEnd = noteFreqEnd,
                dur  = dur,
                fn   = waveFn,
                amp  = n.amp or amp,
                env  = n.adsr or env,
            }
        end
    end

    -- Render all events into buffer
    for _, ev in ipairs(events) do
        local phase = 0
        local s0 = ev.s0
        local s1 = ev.s1
        if s1 >= totalSamples then s1 = totalSamples - 1 end
        local durSamples = s1 - s0 + 1
        if durSamples < 1 then durSamples = 1 end
        local durSec = ev.dur

        for s = s0, s1 do
            local t = (s - s0) / sampleRate
            local progress = t / durSec

            -- Frequency sweep within note
            local freq = ev.freq + (ev.freqEnd - ev.freq) * progress
            phase = phase + pi2 * freq / sampleRate

            local val = ev.fn(phase) * ev.amp * adsr(t, durSec, ev.env)

            -- Additive mix
            local prev = sd:getSample(s)
            local mixed = prev + val
            sd:setSample(s, mixed)
        end
    end

    -- Master pass: volume + clipping
    for s = 0, totalSamples - 1 do
        local val = sd:getSample(s) * masterVol
        if val > clip then val = clip
        elseif val < -clip then val = -clip end
        sd:setSample(s, val)
    end

    return sd
end

return synth
