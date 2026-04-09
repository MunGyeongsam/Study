-- grid_debug_draw.lua
-- LOVE2D 화면 디버깅용 그리드 및 중앙 원점 좌표계 표시 유틸리티
-- drawGrid(centerX, centerY, gridSize, color)

local GridDebugDraw = {}

-- 화면에 그리드와 x/y축, 원점(0,0) 표시
function GridDebugDraw.drawGrid(centerX, centerY, gridSize, color)
    local lg = love.graphics
    local width, height = lg.getDimensions()
    gridSize = gridSize or 50
    color = color or {0.7, 0.7, 0.7, 0.5}

    -- Save state
    local r, g, b, a = lg.getColor()

    -- Draw grid lines
    lg.setColor(color)
    -- 세로선
    for x = centerX, width, gridSize do
        lg.line(x, 0, x, height)
    end
    for x = centerX, 0, -gridSize do
        lg.line(x, 0, x, height)
    end
    -- 가로선
    for y = centerY, height, gridSize do
        lg.line(0, y, width, y)
    end
    for y = centerY, 0, -gridSize do
        lg.line(0, y, width, y)
    end

    -- Draw axes (x: red, y: green)
    lg.setColor(1, 0, 0, 1) -- x축: 빨강
    lg.setLineWidth(2)
    lg.line(centerX, centerY, width, centerY)
    lg.setColor(0, 1, 0, 1) -- y축: 초록
    lg.line(centerX, centerY, centerX, height)
    lg.setLineWidth(1)

    -- Draw origin marker
    lg.setColor(1, 1, 0, 1)
    lg.circle("fill", centerX, centerY, 5)
    lg.setColor(0, 0, 0, 1)
    lg.circle("line", centerX, centerY, 7)
    lg.setColor(1, 1, 1, 1)

    -- Restore state
    lg.setColor(r, g, b, a)
end

return GridDebugDraw
