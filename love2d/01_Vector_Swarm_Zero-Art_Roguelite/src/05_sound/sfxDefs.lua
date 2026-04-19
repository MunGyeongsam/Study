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

return sfxDefs
