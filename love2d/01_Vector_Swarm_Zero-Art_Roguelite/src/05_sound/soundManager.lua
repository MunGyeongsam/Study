-- Sound Manager — Hybrid Loader (ext_res file → code generation fallback)
-- Manages SFX pooling + BGM playback.
-- Usage:
--   soundManager.init()
--   soundManager.play("player_shoot")
--   soundManager.playBGM("stage_normal")

local synth   = require("05_sound.synth")
local sfxDefs = require("05_sound.sfxDefs")

local M = {}

-- ─── Configuration ───────────────────────────────────────────────
local POOL_SIZE    = 4       -- max simultaneous Sources per SFX
local EXT_RES_DIR  = "ext_res/sounds"
local EXT_FORMATS  = { ".ogg", ".wav" }

-- ─── State ───────────────────────────────────────────────────────
local sfxSources = {}    -- { name → { Source, Source, ... } }
local sfxIndex   = {}    -- { name → next index to play }
local bgmSource  = nil   -- current BGM Source
local bgmName    = nil

local volumes = {
    sfx = 0.7,
    bgm = 0.4,
}

local enabled = true

-- ─── Internal: load a single SFX (ext_res file or code gen) ─────
local function loadSFX(name)
    -- 1. Try external file
    for _, ext in ipairs(EXT_FORMATS) do
        local path = EXT_RES_DIR .. "/sfx/" .. name .. ext
        local info = love.filesystem.getInfo(path)
        if info then
            logInfo(string.format("[SND] Loaded ext_res: %s", path))
            return love.audio.newSource(path, "static")
        end
    end

    -- 2. Code generation from sfxDefs
    local def = sfxDefs[name]
    if def then
        local soundData = def(synth)
        logInfo(string.format("[SND] Generated: %s (%d samples)", name, soundData:getSampleCount()))
        return love.audio.newSource(soundData, "static")
    end

    logWarn(string.format("[SND] Not found: %s", name))
    return nil
end

-- ─── Internal: load a BGM (ext_res file or nil) ─────────────────
local function loadBGM(name)
    -- 1. Try external file (stream mode for BGM)
    for _, ext in ipairs(EXT_FORMATS) do
        local path = EXT_RES_DIR .. "/bgm/" .. name .. ext
        local info = love.filesystem.getInfo(path)
        if info then
            logInfo(string.format("[SND] BGM loaded: %s", path))
            return love.audio.newSource(path, "stream")
        end
    end

    -- 2. Code generation (future: pattern sequencer)
    local def = sfxDefs["bgm_" .. name]
    if def then
        local soundData = def(synth)
        logInfo(string.format("[SND] BGM generated: %s", name))
        return love.audio.newSource(soundData)
    end

    logWarn(string.format("[SND] BGM not found: %s", name))
    return nil
end

-- ─── Public API ──────────────────────────────────────────────────

function M.init()
    -- Pre-generate all SFX defined in sfxDefs (pooled)
    for name, _ in pairs(sfxDefs) do
        -- Skip bgm_ prefixed entries
        if not name:find("^bgm_") then
            local sources = {}
            local template = loadSFX(name)
            if template then
                sources[1] = template
                -- Clone for pooling
                for i = 2, POOL_SIZE do
                    sources[i] = template:clone()
                end
            end
            sfxSources[name] = sources
            sfxIndex[name] = 1
        end
    end

    logInfo(string.format("[SND] Sound Manager initialized (%d SFX)", M.getSFXCount()))
end

function M.play(name)
    if not enabled then return end

    local pool = sfxSources[name]
    if not pool or #pool == 0 then return end

    local idx = sfxIndex[name]
    local source = pool[idx]

    -- Round-robin through pool
    sfxIndex[name] = (idx % #pool) + 1

    source:stop()
    source:setVolume(volumes.sfx)
    source:play()
end

function M.playBGM(name)
    if bgmName == name and bgmSource and bgmSource:isPlaying() then return end

    M.stopBGM()
    bgmSource = loadBGM(name)
    if bgmSource then
        bgmSource:setLooping(true)
        bgmSource:setVolume(volumes.bgm)
        bgmSource:play()
        bgmName = name
    end
end

function M.stopBGM()
    if bgmSource then
        bgmSource:stop()
        bgmSource = nil
        bgmName = nil
    end
end

function M.setVolume(category, vol)
    volumes[category] = vol
    if category == "bgm" and bgmSource then
        bgmSource:setVolume(vol)
    end
end

function M.getVolume(category)
    return volumes[category] or 0
end

function M.setEnabled(on)
    enabled = on
    if not on then
        M.stopBGM()
    end
end

function M.isEnabled()
    return enabled
end

function M.getSFXCount()
    local n = 0
    for _ in pairs(sfxSources) do n = n + 1 end
    return n
end

return M
