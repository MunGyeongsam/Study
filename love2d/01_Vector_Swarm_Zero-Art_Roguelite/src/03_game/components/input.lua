-- Input Component
-- 엔티티의 입력 상태 (키보드, 터치 등)

local Input = {}

Input.name = "Input"

Input.defaults = {
    moveX = 0,        -- 이동 방향 X (-1 ~ 1)
    moveY = 0,        -- 이동 방향 Y (-1 ~ 1)
    dash = false,     -- 대쉬 요청 (keypressed → true → InputSystem에서 Dash 컴포넌트로 전달)
}

function Input.new(data)
    return {
        moveX = data and data.moveX or Input.defaults.moveX,
        moveY = data and data.moveY or Input.defaults.moveY,
        dash  = data and data.dash  or false,
    }
end

return Input
