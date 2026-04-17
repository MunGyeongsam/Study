-- System Base Class
-- ECS 시스템의 기본 클래스

local System = {}

-- 새로운 시스템 생성
-- name: 시스템 이름 (디버그용)
-- requiredComponents: 처리할 엔티티가 가져야 할 컴포넌트 목록
-- updateFn: 실제 업데이트 로직 함수 (ecs, dt, entities) 형태
function System.new(name, requiredComponents, updateFn)
    local system = {
        name = name,
        requiredComponents = requiredComponents or {},
        updateFn = updateFn,
        enabled = true,
        
        -- 성능 모니터링
        updateTime = 0,
        callCount = 0,
        totalTime = 0
    }
    
    setmetatable(system, { __index = System })
    return system
end

-- 시스템 업데이트 실행
function System:update(ecs, dt)
    if not self.enabled then
        return
    end
    
    -- 성능 측정 시작
    local startTime = love.timer.getTime()
    
    -- 필요한 컴포넌트를 가진 엔티티들 쿼리
    local entities = ecs:queryEntities(self.requiredComponents)
    
    -- 업데이트 함수 실행
    if self.updateFn and #entities > 0 then
        self.updateFn(ecs, dt, entities)
    end
    
    -- 성능 측정 종료
    local endTime = love.timer.getTime()
    self.updateTime = endTime - startTime
    self.totalTime = self.totalTime + self.updateTime
    self.callCount = self.callCount + 1
end

-- 시스템 활성화/비활성화
function System:setEnabled(enabled)
    self.enabled = enabled
    logDebug(string.format("System '%s' %s", self.name, enabled and "enabled" or "disabled"))
end

-- 성능 통계 반환
function System:getStats()
    return {
        name = self.name,
        enabled = self.enabled,
        callCount = self.callCount,
        lastUpdateTime = self.updateTime,
        totalTime = self.totalTime,
        averageTime = self.callCount > 0 and (self.totalTime / self.callCount) or 0,
        requiredComponents = self.requiredComponents
    }
end

-- 성능 통계 초기화
function System:resetStats()
    self.updateTime = 0
    self.callCount = 0
    self.totalTime = 0
end

return System