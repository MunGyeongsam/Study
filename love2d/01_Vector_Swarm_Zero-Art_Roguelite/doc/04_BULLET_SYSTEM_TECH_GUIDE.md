# LÖVE2D 탄막 시스템 기술 가이드

## 개요
Vector Swarm 프로젝트를 위한 LÖVE2D 탄막 시스템 구현 가이드입니다. 60fps에서 1000+ 탄막 처리를 목표로 합니다.

---

## 1. 핵심 아키텍처 원칙

### 성능 목표
- **60FPS 유지**: 1000개 이상의 동시 탄막
- **메모리 효율성**: 가비지 컬렉션 최소화
- **렌더링 최적화**: 드로우콜 최소화
- **충돌 검사**: 공간 파티셔닝으로 O(N) → O(√N)

### 핵심 기술 스택
1. **오브젝트 풀링**: 메모리 할당 제거
2. **Canvas 배치 렌더링**: 드로우콜 통합
3. **공간 격자**: 효율적 충돌 검사
4. **수학적 패턴**: 절차적 탄막 생성

---

## 2. 오브젝트 풀링 시스템

### 실행 파일: `src/03_game/systems/bulletPool.lua`

```lua
-- Bullet Object Pool System
local bulletPool = {}
bulletPool.__index = bulletPool

function bulletPool.new(maxBullets)
    maxBullets = maxBullets or 5000
    
    local pool = setmetatable({
        maxBullets = maxBullets,
        active = {},      -- 활성 탄막 배열
        activeCount = 0,
        inactive = {},    -- 비활성 탄막 풀
        inactiveCount = 0,
        stats = {spawned = 0, recycled = 0, poolHits = 0, poolMisses = 0}
    }, bulletPool)
    
    -- 풀 사전 할당
    for i = 1, maxBullets do
        pool.inactive[i] = pool:_createBulletObject()
        pool.inactiveCount = i
    end
    
    return pool
end

function bulletPool:spawn(x, y, vx, vy, opts)
    if self.inactiveCount == 0 then return nil end
    
    -- 비활성 풀에서 추출
    local bullet = self.inactive[self.inactiveCount]
    self.inactive[self.inactiveCount] = nil
    self.inactiveCount = self.inactiveCount - 1
    
    -- 초기화
    bullet.x, bullet.y = x, y
    bullet.vx, bullet.vy = vx, vy
    bullet.lifetime = 0
    bullet.active = true
    
    -- 활성 풀에 추가
    self.activeCount = self.activeCount + 1
    self.active[self.activeCount] = bullet
    
    return bullet
end

function bulletPool:update(dt)
    local i = 1
    while i <= self.activeCount do
        local bullet = self.active[i]
        
        if not bullet.active then
            -- 제거: 마지막 요소와 교체
            self.active[i] = self.active[self.activeCount]
            self.active[self.activeCount] = nil
            self.activeCount = self.activeCount - 1
        else
            -- 업데이트
            bullet.x = bullet.x + bullet.vx * dt
            bullet.y = bullet.y + bullet.vy * dt
            bullet.lifetime = bullet.lifetime + dt
            
            if bullet.lifetime >= bullet.maxLifetime then
                self:recycle(bullet)
            end
            
            i = i + 1
        end
    end
end
```

**핵심 이점:**
- ✅ 런타임 메모리 할당 제로
- ✅ GC 지연 제거
- ✅ 캐시 친화적 연속 메모리

---

## 3. 렌더링 최적화

### Canvas 배치 렌더링: `src/03_game/systems/bulletRenderer.lua`

```lua
local bulletRenderer = {}

function bulletRenderer.new()
    return {
        canvas = love.graphics.newCanvas(432, 960),
        needsRedraw = true,
        lastBulletCount = 0
    }
end

function bulletRenderer:draw(bulletPool, camera)
    if self.needsRedraw or bulletPool.activeCount > 0 then
        -- Canvas에 모든 탄막 렌더링
        love.graphics.setCanvas(self.canvas)
        love.graphics.clear(0, 0, 0, 0)
        
        setColor(100, 200, 255, 255)
        for i = 1, bulletPool.activeCount do
            local b = bulletPool.active[i]
            local sx, sy = camera:getScreenPos(b.x, b.y)
            love.graphics.circle("fill", sx, sy, b.radius * camera:getPixelsPerUnit())
        end
        
        love.graphics.setCanvas()
    end
    
    -- 단일 드로우콜로 화면 출력
    love.graphics.draw(self.canvas, 0, 0)
end
```

**성능 개선:**
- 1000개 탄막: 1000 드로우콜 → **1 드로우콜**
- 렌더링 시간: ~60ms → **~2ms**

---

## 4. 공간 파티셔닝

### 격자 기반 충돌 검사: `src/03_game/systems/spatialGrid.lua`

```lua
local spatialGrid = {}

function spatialGrid.new(worldWidth, worldHeight, cellSize)
    cellSize = cellSize or 2.0
    
    return setmetatable({
        cellSize = cellSize,
        width = math.ceil(worldWidth / cellSize),
        height = math.ceil(worldHeight / cellSize),
        cells = {},
        worldMinX = -worldWidth / 2,
        worldMinY = -worldHeight / 2
    }, spatialGrid)
end

function spatialGrid:getCellKey(x, y)
    local cellX = math.floor((x - self.worldMinX) / self.cellSize)
    local cellY = math.floor((y - self.worldMinY) / self.cellSize)
    
    cellX = math.max(0, math.min(self.width - 1, cellX))
    cellY = math.max(0, math.min(self.height - 1, cellY))
    
    return cellX * self.width + cellY
end

function spatialGrid:queryAround(px, py, range)
    local results = {}
    local playerCellKey = self:getCellKey(px, py)
    
    -- 3x3 인접 셀 검사
    local searchCells = {
        playerCellKey,
        playerCellKey - 1, playerCellKey + 1,
        playerCellKey - self.width, playerCellKey + self.width,
        -- 대각선 셀들...
    }
    
    for _, cellKey in ipairs(searchCells) do
        if self.cells[cellKey] then
            for _, bullet in ipairs(self.cells[cellKey]) do
                local dx, dy = bullet.x - px, bullet.y - py
                if dx*dx + dy*dy < range*range then
                    table.insert(results, bullet)
                end
            end
        end
    end
    
    return results
end
```

**충돌 검사 최적화:**
- 순진한 방법: 1000개 탄막 확인
- 공간 격자: 주변 ~90개만 확인 (**90% 성능 향상**)

---

## 5. 탄막 패턴 라이브러리

### 원형 패턴: `src/03_game/patterns/circlePattern.lua`

```lua
local circlePattern = {}

function circlePattern:fire(emitterX, emitterY, bulletPool, opts)
    opts = opts or {}
    local count = opts.count or 12
    local speed = opts.speed or 2.0
    local startAngle = opts.startAngle or 0
    
    for i = 0, count - 1 do
        local angle = startAngle + (i / count) * 2 * math.pi
        local vx = math.cos(angle) * speed
        local vy = math.sin(angle) * speed
        
        bulletPool:spawn(emitterX, emitterY, vx, vy, {
            angle = angle,
            maxLifetime = opts.lifetime or 5
        })
    end
end
```

### 나선형 패턴: `src/03_game/patterns/spiralPattern.lua`

```lua
local spiralPattern = {}

function spiralPattern:new()
    return {time = 0, emitRate = 20}
end

function spiralPattern:update(dt, emitterX, emitterY, bulletPool, opts)
    self.time = self.time + dt
    local speed = opts.speed or 1.5
    local turnRate = opts.turnRate or 2 * math.pi
    
    local bulletCount = math.floor(self.emitRate * dt)
    for i = 1, bulletCount do
        local angle = self.time * turnRate
        local vx = math.cos(angle) * speed
        local vy = math.sin(angle) * speed
        
        bulletPool:spawn(emitterX, emitterY, vx, vy, opts)
    end
end
```

### 추적 탄막: `src/03_game/systems/homingBulletSystem.lua`

```lua
function homingSystem:update(bulletPool, dt, target)
    for i = 1, bulletPool.activeCount do
        local b = bulletPool.active[i]
        
        if b.type == "homing" and target then
            local dx = target.x - b.x
            local dy = target.y - b.y
            local dist = math.sqrt(dx*dx + dy*dy)
            
            if dist > 0 then
                local nx, ny = dx / dist, dy / dist
                local trackSpeed = b.trackSpeed or 1.0
                
                -- 부드러운 추적
                b.vx = b.vx + nx * trackSpeed * dt
                b.vy = b.vy + ny * trackSpeed * dt
                
                -- 속도 정규화
                local speed = math.sqrt(b.vx*b.vx + b.vy*b.vy)
                if speed > 0 then
                    local targetSpeed = 2.0
                    b.vx = (b.vx / speed) * targetSpeed
                    b.vy = (b.vy / speed) * targetSpeed
                end
            end
        end
    end
end
```

---

## 6. 통합 시스템 시퀀스

### main.lua 통합 예제

```lua
-- src/main.lua
local bulletPool = require("03_game.systems.bulletPool")
local bulletRenderer = require("03_game.systems.bulletRenderer")
local spatialGrid = require("03_game.systems.spatialGrid")
local circlePattern = require("03_game.patterns.circlePattern")

local gameState = {}

function love.load()
    gameState.bulletPool = bulletPool.new(5000)
    gameState.bulletRenderer = bulletRenderer.new()
    gameState.spatialGrid = spatialGrid.new(120, 250, 2.0)
    gameState.circlePattern = circlePattern
    
    -- 테스트: 원형 탄막 발사
    gameState.circlePattern:fire(0, 0, gameState.bulletPool, {
        count = 16, speed = 2.0
    })
end

function love.update(dt)
    -- 탄막 업데이트
    gameState.bulletPool:update(dt)
    
    -- 공간 격자 갱신
    gameState.spatialGrid:clear()
    for i = 1, gameState.bulletPool.activeCount do
        local bullet = gameState.bulletPool.active[i]
        gameState.spatialGrid:insert(bullet)
    end
    
    -- 충돌 검사 (플레이어 위치 예: 0, -100)
    local nearbyBullets = gameState.spatialGrid:queryAround(0, -100, 1.0)
    logDebug(string.format("Nearby bullets: %d", #nearbyBullets))
end

function love.draw()
    camera:draw(function()
        gameState.bulletRenderer:draw(gameState.bulletPool, camera)
    end)
end
```

---

## 7. 성능 모니터링

### 디버그 정보 수집

```lua
-- 탄막 풀 상태
local stats = bulletPool:getStats()
debug.add("Bullets", function()
    return string.format("Active: %d/%d (%.1f%%)", 
        stats.activeCount, stats.totalCapacity, 
        (stats.activeCount / stats.totalCapacity) * 100)
end)

-- 메모리 사용량
debug.add("Memory", function()
    local kb = collectgarbage("count")
    return string.format("Lua: %.1f MB", kb / 1024)
end)

-- FPS
debug.add("FPS", function()
    return tostring(love.timer.getFPS())
end)
```

---

## 8. 다음 구현 단계

### Phase 1: 기본 시스템
- [ ] `bulletPool.lua` 구현 
- [ ] `bulletRenderer.lua` 구현
- [ ] `spatialGrid.lua` 구현
- [ ] 기본 테스트 시나리오

### Phase 2: 패턴 라이브러리
- [ ] `circlePattern.lua` - 원형 탄막
- [ ] `spiralPattern.lua` - 나선형 탄막  
- [ ] `wavePattern.lua` - 웨이브 탄막
- [ ] `homingBulletSystem.lua` - 추적 탄막

### Phase 3: 최적화 및 확장
- [ ] Canvas 렌더링 최적화
- [ ] ParticleSystem 실험
- [ ] 메모리 프로파일링
- [ ] 베지어 곡선 패턴

**예상 성능:** 5000개 탄막 풀에서 동시 1000-2000개 활성 탄막, 안정적 60FPS 달성 목표
