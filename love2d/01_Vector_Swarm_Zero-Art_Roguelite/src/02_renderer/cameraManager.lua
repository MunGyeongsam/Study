-- Camera Manager
-- 게임 카메라와 디버그 카메라를 관리하고 전환한다.
--
-- 모드:
--   "game"  — 플레이어를 자동 추적 (기본)
--   "debug" — 마우스 드래그로 자유 이동, 휠로 줌

local camera = require("02_renderer.camera")

local cameraManager = {}

local gameCamera  = nil
local debugCamera = nil
local mode = "game"  -- "game" | "debug"

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

-- 업데이트: 게임 모드일 때 플레이어 추적
function cameraManager.update(dt, targetX, targetY)
    if mode == "game" and targetX and targetY then
        gameCamera:lookAt(targetX, targetY)
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

-- 월드 좌표 변환 (현재 활성 카메라 기준)
function cameraManager.worldCoords(screenX, screenY)
    return cameraManager.getActive():worldCoords(screenX, screenY)
end

-- 렌더링 (활성 카메라로 월드 그리기)
function cameraManager.draw(drawFn)
    cameraManager.getActive():draw(drawFn)
end

return cameraManager
