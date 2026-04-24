-- ============================================================================
-- sceneStack.lua — 씬 스택 엔진 (push/pop/replace/clear)
-- ============================================================================
--
-- ◆ 역할
--   push/pop 기반 상태 관리. Input은 top 씬만, Draw는 drawBelow 체인.
--   see doc/14_SCENE_STACK_ARCHITECTURE.md
--
-- ◆ 씬 인터페이스
--   name, transparent(=update 전파), drawBelow(=아래 씬도 그리기)
--   enter(prev), exit(), update(dt), draw()
--   keypressed(key), textinput(text)
--   touchpressed/moved/released(id, x, y, dx, dy, pressure)
--
-- ◆ 핵심 API
--   SceneStack.new() → stack
--   stack:push(scene) / pop() / replace(scene) / clear()
--   stack:update(dt) / draw() — transparent/drawBelow 체인 처리
--   stack:top() → 현재 씬 | nil

local SceneStack = {}
SceneStack.__index = SceneStack

function SceneStack.new()
    return setmetatable({
        _stack = {},
    }, SceneStack)
end

--- 스택 위에 씬 추가. scene:enter(prev) 호출.
function SceneStack:push(scene)
    local prev = self:top()
    self._stack[#self._stack + 1] = scene
    if scene.enter then scene:enter(prev) end
    logInfo(string.format("[SCENE] push '%s' (depth %d)", scene.name or "?", #self._stack))
end

--- 최상위 씬 제거. scene:exit() 호출.
function SceneStack:pop()
    local n = #self._stack
    if n == 0 then return nil end
    local removed = self._stack[n]
    self._stack[n] = nil
    if removed.exit then removed:exit() end
    logInfo(string.format("[SCENE] pop '%s' (depth %d)", removed.name or "?", #self._stack))
    return removed
end

--- 최상위를 교체. old:exit() → new:enter(old).
function SceneStack:replace(scene)
    local n = #self._stack
    if n == 0 then
        return self:push(scene)
    end
    local old = self._stack[n]
    if old and old.exit then old:exit() end
    self._stack[n] = scene
    if scene.enter then scene:enter(old) end
    logInfo(string.format("[SCENE] replace '%s' → '%s' (depth %d)",
        old and old.name or "?", scene.name or "?", #self._stack))
end

--- 스택 전체 비움. 위부터 exit() 호출.
function SceneStack:clear()
    for i = #self._stack, 1, -1 do
        local s = self._stack[i]
        if s and s.exit then s:exit() end
        self._stack[i] = nil
    end
    logInfo("[SCENE] clear (depth 0)")
end

--- 최상위 씬 반환.
function SceneStack:top()
    return self._stack[#self._stack]
end

--- 스택 깊이.
function SceneStack:size()
    return #self._stack
end

-- ===== update/draw 전파 =====

--- update: top부터 아래로 transparent 체인 따라 전파.
function SceneStack:update(dt)
    local stack = self._stack
    local n = #stack
    if n == 0 then return end

    -- transparent 체인 시작점 찾기 (top → 아래로)
    local start = n
    for i = n, 2, -1 do
        if not stack[i].transparent then break end
        start = i - 1
    end

    -- start부터 top까지 update
    for i = start, n do
        local s = stack[i]
        if s.update then s:update(dt) end
    end
end

--- draw: drawBelow 체인 따라 그려야 할 범위 결정, 아래부터 위로 그림.
function SceneStack:draw()
    local stack = self._stack
    local n = #stack
    if n == 0 then return end

    -- drawBelow 체인 시작점 찾기 (top → 아래로)
    local start = n
    for i = n, 2, -1 do
        if not stack[i].drawBelow then break end
        start = i - 1
    end

    -- start부터 top까지 draw (아래 → 위)
    for i = start, n do
        local s = stack[i]
        if s.draw then s:draw() end
    end
end

--- keypressed: top만 받음.
function SceneStack:keypressed(key)
    local s = self:top()
    if s and s.keypressed then return s:keypressed(key) end
    return false
end

--- textinput: IME를 통과한 문자 입력 (macOS 한글 등).
function SceneStack:textinput(text)
    local s = self:top()
    if s and s.textinput then return s:textinput(text) end
    return false
end

--- touchpressed: top만 받음.
function SceneStack:touchpressed(id, x, y, dx, dy, pressure)
    local s = self:top()
    if s and s.touchpressed then return s:touchpressed(id, x, y, dx, dy, pressure) end
    return false
end

--- touchmoved: top만 받음.
function SceneStack:touchmoved(id, x, y, dx, dy, pressure)
    local s = self:top()
    if s and s.touchmoved then return s:touchmoved(id, x, y, dx, dy, pressure) end
    return false
end

--- touchreleased: top만 받음.
function SceneStack:touchreleased(id, x, y, dx, dy, pressure)
    local s = self:top()
    if s and s.touchreleased then return s:touchreleased(id, x, y, dx, dy, pressure) end
    return false
end

return SceneStack
