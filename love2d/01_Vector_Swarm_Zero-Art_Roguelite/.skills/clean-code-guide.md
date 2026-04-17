---
name: clean-code-guide
role: "Lua 게임 개발에 맞게 변형한 SOLID & Clean Code & Lua 관용구 원칙"
description: |
  전통적인 SOLID 원칙, Clean Code 지침, Lua 관용구, 게임 디자인 패턴을
  Lua + LÖVE2D + ECS 게임 개발 환경에 맞게 변형한 가이드.
  각 원칙마다 원문, 변형 버전, 변경 근거를 함께 기술하여 학습 참고용으로도 활용 가능.
---

# SOLID & Clean Code for Lua Game Dev

> 이 문서는 Robert C. Martin의 SOLID 원칙과 Clean Code 철학을
> **Lua 5.1 + LÖVE2D + ECS 아키텍처** 환경에 맞게 재해석한 것이다.
> 각 원칙마다 [원문] → [우리 버전] → [변경 근거]를 함께 기록한다.

---

## SOLID 원칙

### S — Single Responsibility Principle (단일 책임 원칙)

**[원문]**
> "A class should have only one reason to change."
> 클래스는 변경의 이유가 하나뿐이어야 한다. — Robert C. Martin

**[우리 버전]**
> **모듈과 시스템은 하나의 관심사만 다룬다.**
> 단, 핫패스(매 프레임 실행 경로)에서는 성능을 위해 관련 로직을 인라인할 수 있다.

**[변경 근거]**
- Lua에는 class 키워드가 없다. 우리의 "클래스"는 **모듈 테이블**과 **ECS 시스템**이다.
- ECS가 이미 이 원칙을 강제한다: MovementSystem은 이동만, RenderSystem은 렌더링만.
- 하지만 게임 루프는 60fps 제약이 있어서, 함수 호출 오버헤드를 줄이기 위해
  핫패스에서는 여러 단계를 하나의 루프에 인라인하는 게 합리적이다.

```lua
-- ✅ 좋은 예: 시스템별 단일 책임
MovementSystem  → Transform + Velocity만 처리
LifeSpanSystem  → 수명 만료 엔티티만 제거
RenderSystem    → 그리기만

-- ⚠️ 허용: 핫패스 인라인 (성능상 이유가 명확할 때)
for _, id in ipairs(entities) do
    local t = ecs:getComponent(id, "Transform")
    local v = ecs:getComponent(id, "Velocity")
    -- 이동 + 경계 클램프를 한 루프에서 처리 (함수 호출 1회 절약)
    t.x = clamp(t.x + v.vx * dt, bounds.minX, bounds.maxX)
    t.y = clamp(t.y + v.vy * dt, bounds.minY, bounds.maxY)
end
```

---

### O — Open/Closed Principle (개방-폐쇄 원칙)

**[원문]**
> "Software entities should be open for extension, but closed for modification."
> 확장에는 열려 있고, 수정에는 닫혀 있어야 한다. — Bertrand Meyer

**[우리 버전]**
> **새 행동은 기존 코드 수정 없이 "새 컴포넌트 + 새 시스템"으로 추가한다.**

**[변경 근거]**
- OOP에서는 상속/인터페이스로 확장하지만, Lua에는 공식 상속이 없다.
- ECS의 **컴포지션(조합)** 모델이 이 원칙의 완벽한 구현이다.
  새 능력이 필요하면 컴포넌트를 추가하고, 그걸 처리하는 시스템을 등록하면 끝.
- 기존 MovementSystem을 건드리지 않고 HomingSystem을 추가할 수 있다.

```lua
-- ✅ 확장: "유도 미사일" 기능 추가 시
-- 기존 코드 수정 없이:
-- 1. 새 컴포넌트 정의
ecs:addComponent(missileId, "Homing", { targetId = playerId, turnRate = 3.0 })

-- 2. 새 시스템 등록
ecsManager.addSystem(System.new("Homing", {"Transform", "Velocity", "Homing"}, homingUpdateFn))

-- ❌ 나쁜 예: 기존 MovementSystem에 if문 추가
-- if ecs:hasComponent(id, "Homing") then ... end  ← 이러지 말 것
```

---

### L — Liskov Substitution Principle (리스코프 치환 원칙)

**[원문]**
> "Subtypes must be substitutable for their base types."
> 하위 타입은 상위 타입을 대체할 수 있어야 한다. — Barbara Liskov

**[우리 버전]**
> **같은 인터페이스 패턴(init/update/draw)을 노출하는 모듈은 서로 교체 가능해야 한다.**
> Lua의 덕 타이핑(duck typing)을 활용: "같은 함수가 있으면 같은 타입이다."

**[변경 근거]**
- Lua에는 타입 시스템도, 상속 계층도, 인터페이스도 없다.
- 대신 **구조적 호환성(structural compatibility)**으로 치환 원칙을 구현한다.
- 어떤 모듈이든 `init()`, `update(dt)`, `draw()`를 가지면 main.lua가 동일하게 호출할 수 있다.
- ECS에서는 더 단순: 같은 컴포넌트 조합을 가진 엔티티는 같은 시스템이 처리한다.

```lua
-- ✅ 구조적 호환: 두 모듈 모두 같은 패턴
-- player.lua
local player = {}
function player.init() ... end
function player.update(dt) ... end
function player.draw() ... end
return player

-- enemy.lua (player와 교체 가능한 구조)
local enemy = {}
function enemy.init() ... end
function enemy.update(dt) ... end
function enemy.draw() ... end
return enemy

-- main.lua에서 동일한 방식으로 호출 가능
```

---

### I — Interface Segregation Principle (인터페이스 분리 원칙)

**[원문]**
> "Clients should not be forced to depend on interfaces they do not use."
> 클라이언트는 사용하지 않는 인터페이스에 의존하면 안 된다. — Robert C. Martin

**[우리 버전]**
> **컴포넌트는 최소한의 관련 데이터만 담는다. 모듈 API는 필요한 함수만 노출한다.**

**[변경 근거]**
- Lua에 interface 키워드가 없으므로, 이 원칙은 **데이터 설계**에 적용한다.
- 비대한 컴포넌트 = 비대한 인터페이스. 쓰지 않는 필드가 메모리를 차지하고 혼란을 준다.
- ECS 컴포넌트 분리가 곧 인터페이스 분리다.

```lua
-- ❌ 비대한 컴포넌트 (ISP 위반)
ecs:addComponent(id, "Entity", {
    x = 0, y = 0,          -- 위치
    vx = 0, vy = 0,        -- 속도
    hp = 100,              -- 체력
    color = {1,1,1,1},     -- 렌더링
    radius = 0.1,          -- 충돌
})

-- ✅ 분리된 컴포넌트 (ISP 준수)
ecs:addComponent(id, "Transform", { x = 0, y = 0 })
ecs:addComponent(id, "Velocity",  { vx = 0, vy = 0 })
ecs:addComponent(id, "Health",    { hp = 100 })
ecs:addComponent(id, "Renderable",{ color = {1,1,1,1} })
ecs:addComponent(id, "Collider",  { radius = 0.1 })
-- → MovementSystem은 Health를 몰라도 되고,
--   RenderSystem은 Velocity를 몰라도 된다.
```

---

### D — Dependency Inversion Principle (의존성 역전 원칙)

**[원문]**
> "Depend upon abstractions, not concretions."
> 구체적인 것이 아니라 추상적인 것에 의존하라. — Robert C. Martin

**[우리 버전]**
> **레이어 규칙(상위→하위만 require)을 지키고,
> 역방향 통신이 필요하면 콜백/이벤트를 사용한다.**

**[변경 근거]**
- Lua에는 추상 클래스/인터페이스가 없어서 전통적인 DIP 구현이 불가능하다.
- 대신 우리의 **레이어 시스템(00→01→02→03→04)**이 의존 방향을 강제한다.
- 하위→상위 통신은 콜백 주입이나 이벤트 버스로 해결한다.
  이것이 Lua에서의 "추상에 의존"에 해당한다.

```lua
-- ❌ DIP 위반: 01_core가 03_game을 직접 require
-- 01_core/world.lua
local player = require("03_game.entities.player")  -- 역방향!

-- ✅ DIP 준수: 콜백으로 역전
-- main.lua (진입점)에서 연결
uiManager.setButtonCallbacks({
    onReset = function() player.reset() end  -- 04_ui → 03_game을 main이 중개
})
```

---

## Clean Code 핵심 (Lua 게임 개발 변형)

### CC1 — 함수는 작게

**[원문]** 함수는 한 가지 일만 해야 하고, 짧아야 한다.

**[우리 버전]** 일반 함수는 짧게 유지한다. **단, 핫패스 함수는 성능이 우선이므로 길어질 수 있다.**

**[근거]** 함수 호출은 Lua에서 비용이 있다. 매 프레임 2000번 호출되는 시스템 업데이트 함수를
작은 함수 5개로 쪼개면 10,000번의 호출 오버헤드가 생긴다.

---

### CC2 — 의미 있는 이름

**[원문]** 변수명/함수명은 의도를 드러내야 한다.

**[우리 버전]** **그대로 적용.** 이건 언어와 무관하게 보편적이다.

```lua
-- ❌
local d = 0.016
local t = {}

-- ✅
local deltaTime = 0.016
local activeEntities = {}
```

---

### CC3 — 매직 넘버 금지

**[원문]** 의미 불명의 숫자 리터럴을 쓰지 마라.

**[우리 버전]** **그대로 적용.** 게임 수치는 특히 밸런싱 때 자주 바뀌므로 이름을 붙여야 한다.

```lua
-- ❌
if player.y > 125 then ...

-- ✅
local WORLD_TOP = 125
if player.y > WORLD_TOP then ...

-- 또는 world.lua에서 가져오기
if player.y > world.bounds.top then ...
```

---

### CC4 — DRY (Don't Repeat Yourself)

**[원문]** 중복 코드를 제거하라.

**[우리 버전]** **중복은 제거하되, 핫패스에서 과도한 추상화는 피한다.**

**[근거]** 범용 유틸 함수로 추출하면 DRY하지만, 핫패스에서 매번 범용 함수를 호출하면
불필요한 분기와 타입 체크가 성능을 먹는다. 핫패스 중복은 **의도적 인라인**으로 간주.

```lua
-- ✅ 일반 코드: DRY 적용
local function worldToScreen(x, y)
    return camera:worldToScreen(x, y)
end

-- ⚠️ 핫패스: 인라인 허용 (동일 연산이 반복되더라도)
for _, id in ipairs(entities) do
    local t = ecs:getComponent(id, "Transform")
    -- worldToScreen 호출 대신 직접 계산 (함수 호출 비용 절약)
    local sx = (t.x - camX) * ppu + halfW
    local sy = (camY - t.y) * ppu + halfH
end
```

---

### CC5 — 주석은 "왜"를 설명

**[원문]** 좋은 코드는 주석 없이 읽히지만, 필요하면 "왜"를 설명하라.

**[우리 버전]** **그대로 적용.** 특히 성능 트릭과 게임 수학 공식에는 "왜 이렇게 했는지" 필수.

```lua
-- ❌ "what" 주석 (코드만 봐도 안다)
-- x에 속도를 더한다
t.x = t.x + v.vx * dt

-- ✅ "why" 주석 (의도가 명확해진다)
-- 피벗 전략: 가장 작은 인덱스부터 순회하여 O(N_전체) → O(N_피벗)으로 축소
local pivotName = componentNames[1]
```

---

### CC6 — 에러 처리

**[원문]** 에러를 무시하지 마라. 명확하게 처리하라.

**[우리 버전]** **global.lua의 로깅 함수로 통일하고, 게임은 최대한 죽지 않게 한다.**

**[근거]** 게임은 서버와 다르다. 에러 하나로 크래시되면 플레이어 경험이 완전히 망가진다.
방어적으로 코딩하되, 문제는 반드시 로그에 남겨서 개발 중 잡을 수 있게 한다.

```lua
-- ❌ 에러 무시
local t = ecs:getComponent(id, "Transform")
t.x = t.x + 1  -- t가 nil이면 크래시!

-- ❌ 과도한 에러 처리 (핫패스에서 비용)
local t = ecs:getComponent(id, "Transform")
if t == nil then
    logError("Transform missing!")
    return
end

-- ✅ 시스템 경계에서 한 번 검증, 내부에서는 신뢰
-- queryEntities가 이미 필요한 컴포넌트를 가진 엔티티만 반환하므로
-- 시스템 내부에서는 nil 체크 불필요
local entities = ecs:queryEntities({"Transform", "Velocity"})
for _, id in ipairs(entities) do
    local t = ecs:getComponent(id, "Transform")  -- nil일 수 없음
    local v = ecs:getComponent(id, "Velocity")    -- nil일 수 없음
    t.x = t.x + v.vx * dt
end
```

---

## 요약표: 원문 vs 우리 버전

| 원칙 | 원문 핵심 | 우리 버전 | 변경 정도 |
|------|-----------|-----------|-----------|
| **S** | 클래스 = 1책임 | 모듈/시스템 = 1관심사 (핫패스 인라인 허용) | 약간 변형 |
| **O** | 상속으로 확장 | 새 컴포넌트 + 새 시스템으로 확장 | ECS로 재해석 |
| **L** | 하위타입 치환 | 같은 패턴(init/update/draw) = 교체 가능 | 크게 변형 |
| **I** | 인터페이스 분리 | 컴포넌트 최소화, API 최소화 | ECS로 재해석 |
| **D** | 추상에 의존 | 레이어 규칙 + 콜백/이벤트 | 구조적 대체 |
| **CC1** | 함수는 작게 | 핫패스 제외, 일반 코드는 작게 | 조건부 예외 |
| **CC2** | 의미있는 이름 | 그대로 적용 | 동일 |
| **CC3** | 매직넘버 금지 | 그대로 적용 | 동일 |
| **CC4** | DRY | 핫패스 인라인은 의도적 중복 허용 | 조건부 예외 |
| **CC5** | 주석은 "왜" | 그대로 적용 (성능 트릭, 수학 공식 필수) | 동일 |
| **CC6** | 명확한 에러처리 | 경계에서 검증, 내부는 신뢰, 로그 필수 | 게임 특화 |

---

## Lua 관용구 (Lua Idioms)

> 참고: [Olivine-Labs Lua Style Guide](https://github.com/Olivine-Labs/lua-style-guide),
> [lua-users.org LuaStyleGuide](http://lua-users.org/wiki/LuaStyleGuide),
> [Programming in Lua (PiL)](https://www.lua.org/pil/) by Roberto Ierusalimschy

### LI1 — Early Return (조기 반환)

검증 실패 시 즉시 return. 중첩 if를 피하고 함수의 "행복 경로(Happy Path)"를 먼저 보여준다.

```lua
-- ❌ 중첩 if (행복 경로가 가장 깊은 곳에 숨어있음)
local function process(entity)
    if entity then
        if entity.active then
            if entity.hp > 0 then
                -- ... 실제 로직 ...
            end
        end
    end
end

-- ✅ Early return (행복 경로가 맨 앞에)
local function process(entity)
    if not entity then return end
    if not entity.active then return end
    if entity.hp <= 0 then return end
    
    -- ... 실제 로직 ...
end
```

---

### LI2 — and/or 삼항 관용구

Lua에는 삼항 연산자(`?:`)가 없다. 대신 `and`/`or` 조합으로 간결하게 표현.

```lua
-- 기본값 설정 (nil/false일 때 대체)
local name = playerName or "Unknown"
local speed = config.speed or 2.0

-- 조건부 값 (주의: 첫 번째 값이 false일 수 있으면 사용 금지)
local label = isAlive and "Alive" or "Dead"
local color = isDamaged and {1,0,0,1} or {1,1,1,1}

-- ⚠️ 함정: a and b or c 에서 b가 false/nil이면 항상 c가 반환됨!
local bad = true and false or "oops"  -- → "oops" (false가 아니라 "oops"!)
```

---

### LI3 — 테이블 삽입 성능

`table.insert(t, v)` 대신 `t[#t+1] = v`가 빠르다. 함수 호출 오버헤드가 없으므로.

```lua
-- ❌ 느림 (table.insert는 함수 호출 + 테이블 룩업 필요)
table.insert(results, entityId)

-- ✅ 빠름 (직접 인덱스 접근)
results[#results + 1] = entityId

-- 핫패스에서는 이 차이가 누적된다.
-- 단, 일반 코드에서는 table.insert가 더 읽기 쉽다.
```

---

### LI4 — nil 검사 관용구

Lua에서 `nil`과 `false`만 falsy다. 나머지는 모두 truthy (0, "", {} 포함).

```lua
-- ❌ 불필요한 명시적 nil 비교
if entity ~= nil then ... end

-- ✅ 간결한 관용구
if entity then ... end
if not entity then return end

-- ⚠️ false와 nil을 구분해야 하는 경우만 명시적 비교
if value == nil then ...   -- nil만 (할당되지 않은 상태)
if value == false then ... -- false만 (명시적 거짓)
```

---

### LI5 — 모듈 패턴 (공식 권장)

Lua 커뮤니티와 PiL이 권장하는 모듈 패턴. `module()` 함수는 사용하지 않는다.

```lua
-- ✅ 표준 모듈 패턴 (Lua 커뮤니티 권장)
local M = {}

-- 비공개 함수 (local function)
local function helper()
    -- 모듈 내부에서만 접근 가능
end

-- 공개 API
function M.doSomething()
    helper()
end

return M  -- 반드시 테이블 반환

-- ❌ module() 함수 사용 금지 (전역 오염 + 환경 노출 문제)
-- module(..., package.seeall)  ← 절대 쓰지 말 것
```

---

### LI6 — 생성자 패턴 (OOP)

Lua에서 클래스를 흩내 낼 때의 관용적 패턴. 메타테이블을 이용한 상속 흩내내기.

```lua
local Camera = {}
Camera.__index = Camera

function Camera.new(x, y, orthoSize)
    local self = setmetatable({}, Camera)
    self.x = x
    self.y = y
    self.orthoSize = orthoSize
    return self
end

function Camera:lookAt(x, y)
    self.x = x
    self.y = y
end

return Camera

-- 사용:
-- local cam = Camera.new(0, 0, 5)
-- cam:lookAt(10, 20)  -- :은 self를 자동 전달
```

**참고:** 우리 프로젝트에서는 ECS가 주력이므로 OOP는 카메라, ECS 코어 등
소수 시스템 모듈에만 사용하고, 게임 엔티티는 컴포넌트 조합으로 표현한다.

---

### LI7 — 우리 프로젝트 네이밍 vs Lua 표준

Lua 생태계는 `snake_case`가 표준이지만, 우리는 **의도적으로** 다른 규칙을 쓴다:

| 대상 | Lua 표준 | 우리 규칙 | 이유 |
|------|----------|-----------|------|
| 파일명 | `snake_case.lua` | `camelCase.lua` | 레이어 번호와 시각적 구분 |
| 컴포넌트 | - | `PascalCase` | 컴포넌트는 타입 역할이므로 클래스명 관례 |
| 시스템명 | - | `PascalCase` | 동일 |
| 함수명 | `snake_case` | `camelCase` | 프로젝트 내 일관성 우선 |
| 상수 | `ALL_CAPS` | `ALL_CAPS` | Lua 표준과 동일 |

이 규칙은 프로젝트 전체에서 일관되면 문제없다. 중요한 건 **일관성**이지 특정 스타일이 아니다.

---

## 게임 디자인 패턴 (Game Programming Patterns)

> 참고: [Game Programming Patterns](https://gameprogrammingpatterns.com/) by Robert Nystrom (무료 웹북)
>
> 게임 개발 패턴의 바이블. 우리 프로젝트에서 사용 중이거나 도입 예정인 패턴들.

### 현재 사용 중인 패턴

#### GP1 — Component (컴포넌트)
**출처:** [14장 Component](https://gameprogrammingpatterns.com/component.html)

하나의 엔티티를 여러 도메인의 컴포넌트로 분해하여 결합도를 낮춘다.

```lua
-- 우리 구현: ECS 컴포넌트
-- 하나의 엔티티가 독립적인 데이터 조각들로 구성됨
ecs:addComponent(id, "Transform", { x = 0, y = 0 })
ecs:addComponent(id, "Velocity",  { vx = 0, vy = 0 })
ecs:addComponent(id, "Renderable",{ type = "circle", radius = 0.1 })
```

#### GP2 — Update Method (업데이트 메서드)
**출처:** [10장 Update Method](https://gameprogrammingpatterns.com/update-method.html)

각 객체가 프레임마다 호출되는 `update(dt)` 메서드를 가진다.

```lua
-- 우리 구현: ECS 시스템의 update
function System:update(ecs, dt)
    local entities = ecs:queryEntities(self.requiredComponents)
    self.updateFn(ecs, dt, entities)
end
```

#### GP3 — Game Loop (게임 루프)
**출처:** [9장 Game Loop](https://gameprogrammingpatterns.com/game-loop.html)

LÖVE2D가 제공하는 `love.load → love.update(dt) → love.draw` 순환.

---

### 도입 예정 패턴

#### GP4 — Object Pool (오브젝트 풀)
**출처:** [19장 Object Pool](https://gameprogrammingpatterns.com/object-pool.html)

탄막/적/파티클 등 대량으로 생성/소멸하는 객체를 미리 할당해두고 재사용.
GC 부하를 제거하여 60fps를 유지하는 핵심 패턴.

```lua
-- 예정 구현 개념
local bulletPool = {
    pool = {},       -- 사용 가능한 탄막
    active = {},     -- 활성 탄막
    maxSize = 5000   -- 최대 풀 크기
}
-- ECS의 recycledIds가 이미 이 패턴의 기초를 구현하고 있음
```

#### GP5 — Observer / Event Queue (관찰자 / 이벤트 큐)
**출처:** [4장 Observer](https://gameprogrammingpatterns.com/observer.html), [15장 Event Queue](https://gameprogrammingpatterns.com/event-queue.html)

레이어 간 역방향 통신 문제를 해결. 하위 레이어가 이벤트를 발행하고,
상위 레이어가 구독하는 방식으로 require 역전 없이 통신 가능.

```lua
-- 예정: 00_common/events.lua
local events = require("00_common.events")

-- 03_game에서 발행 (하위 레이어)
events.emit("player:damaged", { amount = 10 })

-- 04_ui에서 구독 (상위 레이어, 역방향 require 없음)
events.on("player:damaged", function(data)
    hud:showDamageEffect(data.amount)
end)
```

#### GP6 — State (상태 패턴)
**출처:** [7장 State](https://gameprogrammingpatterns.com/state.html)

게임 상태(메뉴, 플레이, 일시정지, 게임오버)를 상태 기계로 관리.

```lua
-- 예정: 03_game/states/
local states = {
    menu    = require("03_game.states.menu"),
    playing = require("03_game.states.playing"),
    paused  = require("03_game.states.paused"),
    gameover = require("03_game.states.gameover")
}
```

#### GP7 — Spatial Partition (공간 분할)
**출처:** [20장 Spatial Partition](https://gameprogrammingpatterns.com/spatial-partition.html)

충돌 검사를 O(N²)에서 O(N)으로 줄이는 격자 기반 공간 분할.
1000+ 탄막 충돌 처리에 필수.

#### GP8 — Data Locality (데이터 지역성)
**출처:** [17장 Data Locality](https://gameprogrammingpatterns.com/data-locality.html)

ECS의 컴포넌트별 배열 저장 방식(SoA)이 CPU 캐시 핬 불일치를 줄인다.
우리의 `components[componentName][entityId]` 구조가 이 방향.

---

### 패턴 적용 요약

| 패턴 | 상태 | 우리 구현체 |
|------|------|---------------|
| Component | ✅ 사용 중 | `ecs.lua` 컴포넌트 |
| Update Method | ✅ 사용 중 | `system.lua` update |
| Game Loop | ✅ 사용 중 | LÖVE2D 제공 |
| Object Pool | 📝 기초만 | ECS recycledIds |
| Observer/Event | 📝 계획 | `architecture-rules.md` 9번 참고 |
| State | 📝 계획 | `03_game/states/` |
| Spatial Partition | 📝 계획 | 충돌 시스템 때 |
| Data Locality | ✅ 부분 적용 | 컴포넌트별 저장소 |
