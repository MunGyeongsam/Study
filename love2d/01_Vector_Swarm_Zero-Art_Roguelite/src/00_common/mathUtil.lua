-- Math Utilities
-- 프레임 독립적 보간, 클램프 등 공통 수학 함수

local M = {}

local exp = math.exp

--- Exponential decay (frame-rate independent smoothing)
--- current가 target을 향해 부드럽게 수렴한다.
--- k: 감쇠 상수 (클수록 빠름, 1/k 초에 ~63% 도달)
--- 사용 예: value = expDecay(value, target, k, dt)
function M.expDecay(current, target, k, dt)
    return target + (current - target) * exp(-k * dt)
end

return M
