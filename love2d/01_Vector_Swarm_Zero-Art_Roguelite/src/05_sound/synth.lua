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

return synth
