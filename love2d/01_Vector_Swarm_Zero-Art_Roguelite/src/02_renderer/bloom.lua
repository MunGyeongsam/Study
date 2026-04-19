-- Bloom Post-processing Module
-- Multi-pass: Scene → Bright Extract → Gaussian Blur → Additive Composite

local lg = love.graphics

local bloom = {}

-- State
local enabled = true
local sceneCanvas      -- full resolution scene capture
local brightCanvas     -- half-res bright pass
local blurCanvasH      -- half-res horizontal blur
local blurCanvasV      -- half-res vertical blur (final glow)
local thresholdShader  -- extracts bright pixels
local blurShader       -- 2-pass Gaussian blur

-- Tuning parameters
local threshold = 0.35    -- brightness cutoff (0-1)
local intensity = 0.8     -- glow strength multiplier
local blurRadius = 3      -- blur kernel offset scale

-- GLSL: Brightness threshold extraction
local thresholdShaderCode = [[
extern number threshold;

vec4 effect(vec4 color, Image tex, vec2 uv, vec2 px) {
    vec4 pixel = Texel(tex, uv) * color;
    float brightness = dot(pixel.rgb, vec3(0.2126, 0.7152, 0.0722));
    if (brightness < threshold) {
        return vec4(0.0, 0.0, 0.0, 0.0);
    }
    return pixel;
}
]]

-- GLSL: Single-direction Gaussian blur (9-tap)
local blurShaderCode = [[
extern vec2 direction;  // (1/w, 0) or (0, 1/h)

vec4 effect(vec4 color, Image tex, vec2 uv, vec2 px) {
    vec4 sum = vec4(0.0);

    // 9-tap Gaussian weights (sigma ~2)
    float weights[5] = float[](0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162);

    sum += Texel(tex, uv) * weights[0];
    for (int i = 1; i < 5; i++) {
        vec2 offset = direction * float(i);
        sum += Texel(tex, uv + offset) * weights[i];
        sum += Texel(tex, uv - offset) * weights[i];
    }
    return sum * color;
}
]]

function bloom.init()
    local w, h = lg.getDimensions()
    local halfW, halfH = math.floor(w / 2), math.floor(h / 2)

    -- Full-res scene capture
    sceneCanvas = lg.newCanvas(w, h)

    -- Half-res for blur (cheaper + softer glow)
    brightCanvas = lg.newCanvas(halfW, halfH)
    blurCanvasH  = lg.newCanvas(halfW, halfH)
    blurCanvasV  = lg.newCanvas(halfW, halfH)

    -- Compile shaders
    thresholdShader = lg.newShader(thresholdShaderCode)
    blurShader = lg.newShader(blurShaderCode)

    -- Set initial uniform
    thresholdShader:send("threshold", threshold)

    logInfo("[BLOOM] Initialized (%dx%d scene, %dx%d blur)", w, h, halfW, halfH)
end

--- Call before rendering the scene
function bloom.beginCapture()
    if not enabled then return end
    lg.setCanvas(sceneCanvas)
    lg.clear(0, 0, 0, 1)
end

--- Call after rendering the scene (before UI)
function bloom.endCapture()
    if not enabled then return end
    lg.setCanvas()
end

--- Draw the composited result (scene + glow)
function bloom.draw()
    if not enabled then return end

    local halfW, halfH = brightCanvas:getWidth(), brightCanvas:getHeight()

    -- Pass 1: Extract bright pixels → brightCanvas (half-res)
    lg.setCanvas(brightCanvas)
    lg.clear(0, 0, 0, 1)
    lg.setShader(thresholdShader)
    lg.setColor(1, 1, 1, 1)
    lg.draw(sceneCanvas, 0, 0, 0, halfW / sceneCanvas:getWidth(), halfH / sceneCanvas:getHeight())
    lg.setShader()
    lg.setCanvas()

    -- Pass 2: Horizontal blur → blurCanvasH
    lg.setCanvas(blurCanvasH)
    lg.clear(0, 0, 0, 1)
    lg.setShader(blurShader)
    blurShader:send("direction", {blurRadius / halfW, 0})
    lg.setColor(1, 1, 1, 1)
    lg.draw(brightCanvas)
    lg.setShader()
    lg.setCanvas()

    -- Pass 3: Vertical blur → blurCanvasV
    lg.setCanvas(blurCanvasV)
    lg.clear(0, 0, 0, 1)
    lg.setShader(blurShader)
    blurShader:send("direction", {0, blurRadius / halfH})
    lg.setColor(1, 1, 1, 1)
    lg.draw(blurCanvasH)
    lg.setShader()
    lg.setCanvas()

    -- Composite: Draw original scene
    lg.setColor(1, 1, 1, 1)
    lg.draw(sceneCanvas)

    -- Composite: Additive blend glow on top
    lg.setBlendMode("add")
    lg.setColor(intensity, intensity, intensity, 1)
    lg.draw(blurCanvasV, 0, 0, 0, sceneCanvas:getWidth() / halfW, sceneCanvas:getHeight() / halfH)
    lg.setBlendMode("alpha")

    -- Reset color
    lg.setColor(1, 1, 1, 1)
end

function bloom.toggle()
    enabled = not enabled
    logInfo("[BLOOM] %s", enabled and "ON" or "OFF")
end

function bloom.isEnabled()
    return enabled
end

function bloom.setThreshold(t)
    threshold = t
    if thresholdShader then
        thresholdShader:send("threshold", threshold)
    end
end

function bloom.setIntensity(i)
    intensity = i
end

function bloom.setBlurRadius(r)
    blurRadius = r
end

function bloom.getSettings()
    return threshold, intensity, blurRadius
end

--- Handle window resize
function bloom.resize(w, h)
    local halfW, halfH = math.floor(w / 2), math.floor(h / 2)
    sceneCanvas  = lg.newCanvas(w, h)
    brightCanvas = lg.newCanvas(halfW, halfH)
    blurCanvasH  = lg.newCanvas(halfW, halfH)
    blurCanvasV  = lg.newCanvas(halfW, halfH)
    logInfo("[BLOOM] Resized (%dx%d scene, %dx%d blur)", w, h, halfW, halfH)
end

return bloom
