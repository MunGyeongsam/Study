-- Debug Info Display Module
-- 동적으로 디버그 정보를 추가/제거하고 렌더링하는 시스템


-- Minimal Debug Info Display
-- add(key, func): 문자열 반환 함수 등록
-- remove(key): 해당 디버그 정보 제거
-- draw(): 등록된 순서대로 출력

local Debug = {}

local debugList = {}   -- { {key=..., func=...}, ... } (순서 보장)
local debugIndex = {}  -- key -> index in debugList

-- 디버그 정보 추가 (key, 문자열 반환 함수)
function Debug.add(key, func)
    if debugIndex[key] then return end -- 중복 방지
    table.insert(debugList, {key=key, func=func})
    debugIndex[key] = #debugList
end

-- 디버그 정보 제거
function Debug.remove(key)
    local idx = debugIndex[key]
    if not idx then return end
    table.remove(debugList, idx)
    debugIndex[key] = nil
    -- 인덱스 재정렬
    for i, v in ipairs(debugList) do
        debugIndex[v.key] = i
    end
end

-- 디버그 정보 출력 (등록 순서)
function Debug.draw(x, y, lineHeight)
    x = x or 10
    y = y or 30
    lineHeight = lineHeight or 16
    for i, v in ipairs(debugList) do
        local text = v.func()
        if text and text ~= "" then
            love.graphics.print(text, x, y)
            y = y + lineHeight
        end
    end
end

return Debug