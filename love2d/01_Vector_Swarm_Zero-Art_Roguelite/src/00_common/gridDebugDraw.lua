-- gridDebugDraw.lua
-- LOVE2D 스크린 스페이스 기준 그리드 및 좌표축 표시 유틸리티
-- 좌측 상단 (0,0), 우측 하단 (width, height)

local gridDebugDraw = {}

local visible = false

function gridDebugDraw.toggle()
    visible = not visible
end

function gridDebugDraw.isVisible()
    return visible
end

-- 스크린 스페이스에 그리드와 좌상단 축 표시 (visible일 때만)
function gridDebugDraw.draw(gridSize, gridColor, axisLength)
    if not visible then return end
    local lg = love.graphics
    local width, height = lg.getDimensions()
    
    gridSize = gridSize or 50
    gridColor = gridColor or {0.3, 0.3, 0.3, 0.5}  -- 어두운 회색
    axisLength = axisLength or 100  -- 축 길이
    
    -- Save state
    local r, g, b, a = lg.getColor()
    local lineWidth = lg.getLineWidth()

    -- Draw grid lines
    lg.setColor(gridColor)
    lg.setLineWidth(1)
    
    -- 세로선 (X축 방향)
    for x = 0, width, gridSize do
        lg.line(x, 0, x, height)
    end
    
    -- 가로선 (Y축 방향)  
    for y = 0, height, gridSize do
        lg.line(0, y, width, y)
    end

    -- Draw coordinate axes at top-left corner
    lg.setLineWidth(2)
    
    -- X축 (오른쪽 방향, 빨간색)
    lg.setColor(1, 0, 0, 1)
    lg.line(10, 10, 10 + axisLength, 10)
    -- X축 화살표
    lg.line(10 + axisLength, 10, 10 + axisLength - 8, 6)
    lg.line(10 + axisLength, 10, 10 + axisLength - 8, 14)
    
    -- Y축 (아래쪽 방향, 초록색)
    lg.setColor(0, 1, 0, 1)
    lg.line(10, 10, 10, 10 + axisLength)
    -- Y축 화살표
    lg.line(10, 10 + axisLength, 6, 10 + axisLength - 8)
    lg.line(10, 10 + axisLength, 14, 10 + axisLength - 8)
    
    -- 원점 마커 (좌측 상단)
    lg.setColor(1, 1, 0, 1)  -- 노란색
    lg.circle("fill", 10, 10, 4)
    lg.setColor(0, 0, 0, 1)  -- 검은 테두리
    lg.circle("line", 10, 10, 4)
    
    -- 축 레이블
    lg.setColor(1, 1, 1, 1)  -- 흰색 텍스트
    lg.print("X", 10 + axisLength + 5, 5)
    lg.print("Y", 5, 10 + axisLength + 5)
    lg.print("(0,0)", 15, 15)
    
    -- 화면 크기 표시 (우하단)
    local sizeText = string.format("(%d,%d)", width, height)
    local font = lg.getFont()
    local textWidth = font:getWidth(sizeText)
    local textHeight = font:getHeight()
    lg.print(sizeText, width - textWidth - 10, height - textHeight - 10)

    -- Restore state
    lg.setColor(r, g, b, a)
    lg.setLineWidth(lineWidth)
end

return gridDebugDraw
