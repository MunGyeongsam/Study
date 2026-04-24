-- ============================================================================
-- cameraManager.lua — 카메라 컨트롤러 (게임/디버그 모드)
-- ============================================================================
--
-- ◆ 역할
--   게임 카메라(플레이어 추적) ↔ 디버그 카메라(자유 이동) 전환 (F5).
--   스크린 셰이크, 월드 바운드 클램핑을 처리한다.
--
-- ◆ 모드
--   "game"  — 플레이어 자동 추적 (exp smoothing k=12)
--   "debug" — 마우스 드래그 + 휠 줌
--
-- ◆ 핵심 API
--   init(orthoSize), update(dt, targetX, targetY), draw(drawFn)
--   toggle(), shake(intensity, duration), worldCoords(sx, sy)

local camera   = require("02_renderer.camera")
local mathUtil = require("00_common.mathUtil")
local worldMod = require("01_core.world")

local cameraManager = {}

local gameCamera  = nil
local debugCamera = nil
local mode = "game"  -- "game" | "debug"

-- Camera follow damping (k: higher = snappier)
local CAM_FOLLOW_K = 12

-- Screen shake state
local shakeIntensity = 0
local shakeDuration  = 0
local shakeTimer     = 0
local shakeOffsetX   = 0
local shakeOffsetY   = 0
local random = math.random
local max = math.max
local min = math.min
local cos, sin, pi2 = math.cos, math.sin, math.pi * 2

function cameraManager.init(orthographicSize)
    orthographicSize = orthographicSize or 5
    gameCamera  = camera.new(0, 0, orthographicSize)
    debugCamera = camera.new(0, 0, orthographicSize)
    mode = "game"
    logInfo("[CAM] Camera Manager initialized (mode: game)")
end

-- 모드 전환
function cameraManager.toggle()
    if mode == "game" then
        -- 디버그 전환 시 현재 게임 카메라 위치/줌을 복사
        local gx, gy = gameCamera:pos()
        debugCamera:lookAt(gx, gy)
        debugCamera:setOrthographicSize(gameCamera:getOrthographicSize())
        mode = "debug"
    else
        mode = "game"
    end
    logInfo(string.format("[CAM] Camera mode: %s", mode))
end

function cameraManager.getMode()
    return mode
end

function cameraManager.isDebug()
    return mode == "debug"
end

-- 활성 카메라 반환
function cameraManager.getActive()
    if mode == "debug" then
        return debugCamera
    end
    return gameCamera
end

-- 게임 카메라 직접 접근 (UI 콜백 등에서 필요)
function cameraManager.getGameCamera()
    return gameCamera
end

-- 업데이트: 게임 모드일 때 플레이어 추적 (exponential smoothing) + 월드 클램핑
function cameraManager.update(dt, targetX, targetY)
    if mode == "game" and targetX and targetY then
        local cx, cy = gameCamera:pos()
        local nx = mathUtil.expDecay(cx, targetX, CAM_FOLLOW_K, dt)
        local ny = mathUtil.expDecay(cy, targetY, CAM_FOLLOW_K, dt)

        -- Clamp to world bounds (camera edges never exceed world)
        local left, bottom, right, top = worldMod.getBounds()
        local orthoSize = gameCamera:getOrthographicSize()
        local sw, sh = love.graphics.getDimensions()
        local halfViewW = orthoSize * (sw / sh)
        local halfViewH = orthoSize

        local minX, maxX = left + halfViewW, right - halfViewW
        local minY, maxY = bottom + halfViewH, top - halfViewH

        if minX < maxX then nx = max(minX, min(maxX, nx)) else nx = (left + right) * 0.5 end
        if minY < maxY then ny = max(minY, min(maxY, ny)) else ny = (bottom + top) * 0.5 end

        gameCamera:lookAt(nx, ny)
    end

    -- Shake decay
    if shakeTimer > 0 then
        shakeTimer = shakeTimer - dt
        if shakeTimer <= 0 then
            shakeTimer = 0
            shakeOffsetX, shakeOffsetY = 0, 0
        else
            local ratio = shakeTimer / shakeDuration
            local mag = shakeIntensity * ratio
            local angle = random() * pi2
            shakeOffsetX = cos(angle) * mag
            shakeOffsetY = sin(angle) * mag
        end
    end
end

-- 디버그 카메라 마우스 드래그
function cameraManager.debugMove(dx, dy)
    if mode ~= "debug" then return end
    local upp = debugCamera:getUnitsPerPixel()
    debugCamera:move(-dx * upp, dy * upp)  -- Y축 반전
end

-- 디버그 카메라 마우스 휠 줌
function cameraManager.wheelmoved(x, y)
    if mode ~= "debug" then return end
    local factor = y > 0 and 0.8 or 1.25  -- 위로 = 줌인, 아래로 = 줌아웃
    local newSize = debugCamera:getOrthographicSize() * factor
    debugCamera:setOrthographicSize(newSize)
    logDebug(string.format("[CAM] Debug zoom: %.2f", newSize))
end

-- Screen shake 트리거 (intensity: 월드 유닛, duration: 초)
-- 현재 shake보다 강한 shake만 덮어씀
function cameraManager.shake(intensity, duration)
    if intensity > shakeIntensity * (shakeTimer / max(shakeDuration, 0.001)) then
        shakeIntensity = intensity
        shakeDuration  = duration
        shakeTimer     = duration
    end
end

-- 월드 좌표 변환 (현재 활성 카메라 기준)
function cameraManager.worldCoords(screenX, screenY)
    return cameraManager.getActive():worldCoords(screenX, screenY)
end

-- 렌더링 (활성 카메라로 월드 그리기 + shake 오프셋 적용)
function cameraManager.draw(drawFn)
    local cam = cameraManager.getActive()
    if shakeTimer > 0 then
        local ox, oy = cam:pos()
        cam:lookAt(ox + shakeOffsetX, oy + shakeOffsetY)
        cam:draw(drawFn)
        cam:lookAt(ox, oy)
    else
        cam:draw(drawFn)
    end
end

return cameraManager
