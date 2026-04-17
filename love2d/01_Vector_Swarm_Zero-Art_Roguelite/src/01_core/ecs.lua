-- ECS Core System
-- Entity-Component-System 핵심 구현

local ECS = {}

function ECS.new()
    local ecs = {
        -- Entity 관리
        nextEntityId = 1,
        entities = {},          -- 활성 엔티티 ID 목록
        recycledIds = {},       -- 재사용 가능한 ID 풀
        
        -- Component 저장소
        components = {},        -- [componentName] = { [entityId] = data }
        entityComponents = {},  -- [entityId] = { [componentName] = true }
        
        -- 컴포넌트 인덱스 캐시
        -- queryEntities()가 매 프레임 전체 엔티티를 순회하지 않도록,
        -- 컴포넌트별로 어떤 엔티티가 갖고 있는지를 역방향 인덱스로 유지한다.
        -- 구조: [componentName] = { [entityId] = true, ... }
        -- addComponent/removeComponent 시점에 자동 업데이트되므로
        -- 쿼리 시 O(N_전체) 대신 O(N_최소셋) 으로 줄어든다.
        componentIndex = {},
        
        -- 통계
        stats = {
            activeEntities = 0,
            recycledIds = 0,
            totalCreated = 0,
            totalDestroyed = 0
        }
    }
    
    setmetatable(ecs, { __index = ECS })
    return ecs
end

-- 엔티티 생성
function ECS:createEntity()
    local entityId
    
    -- 재활용 ID가 있으면 사용, 없으면 새로 생성
    if #self.recycledIds > 0 then
        entityId = table.remove(self.recycledIds)
        self.stats.recycledIds = self.stats.recycledIds - 1
    else
        entityId = self.nextEntityId
        self.nextEntityId = self.nextEntityId + 1
    end
    
    -- 엔티티 등록
    self.entities[entityId] = true
    self.entityComponents[entityId] = {}
    
    self.stats.activeEntities = self.stats.activeEntities + 1
    self.stats.totalCreated = self.stats.totalCreated + 1
    
    return entityId
end

-- 엔티티 제거
function ECS:destroyEntity(entityId)
    if not self.entities[entityId] then
        logWarn(string.format("ECS: Attempting to destroy non-existent entity %d", entityId))
        return false
    end
    
    -- 모든 컴포넌트 제거 (pairs 순회 중 테이블 수정 방지)
    local componentList = self.entityComponents[entityId]
    if componentList then
        local toRemove = {}
        for componentName, _ in pairs(componentList) do
            toRemove[#toRemove + 1] = componentName
        end
        for _, componentName in ipairs(toRemove) do
            self:removeComponent(entityId, componentName)
        end
    end
    
    -- 엔티티 제거 및 ID 재활용
    self.entities[entityId] = nil
    self.entityComponents[entityId] = nil
    table.insert(self.recycledIds, entityId)
    
    self.stats.activeEntities = self.stats.activeEntities - 1
    self.stats.recycledIds = self.stats.recycledIds + 1
    self.stats.totalDestroyed = self.stats.totalDestroyed + 1
    
    return true
end

-- 컴포넌트 추가
function ECS:addComponent(entityId, componentName, data)
    if not self.entities[entityId] then
        logError(string.format("ECS: Cannot add component to non-existent entity %d", entityId))
        return false
    end
    
    -- 컴포넌트 저장소 초기화 (필요 시)
    if not self.components[componentName] then
        self.components[componentName] = {}
    end
    
    -- 컴포넌트 데이터 저장
    self.components[componentName][entityId] = data or {}
    self.entityComponents[entityId][componentName] = true
    
    -- 인덱스 캐시 업데이트: 이 컴포넌트를 가진 엔티티 목록에 추가
    if not self.componentIndex[componentName] then
        self.componentIndex[componentName] = {}
    end
    self.componentIndex[componentName][entityId] = true
    
    return true
end

-- 컴포넌트 가져오기
function ECS:getComponent(entityId, componentName)
    if not self.entities[entityId] then
        return nil
    end
    
    if not self.components[componentName] then
        return nil
    end
    
    return self.components[componentName][entityId]
end

-- 컴포넌트 제거
function ECS:removeComponent(entityId, componentName)
    if not self.entities[entityId] then
        return false
    end
    
    -- 컴포넌트 데이터 제거
    if self.components[componentName] then
        self.components[componentName][entityId] = nil
    end
    
    if self.entityComponents[entityId] then
        self.entityComponents[entityId][componentName] = nil
    end
    
    -- 인덱스 캐시 업데이트: 이 컴포넌트의 엔티티 목록에서 제거
    if self.componentIndex[componentName] then
        self.componentIndex[componentName][entityId] = nil
    end
    
    return true
end

-- 컴포넌트 존재 확인
function ECS:hasComponent(entityId, componentName)
    if not self.entities[entityId] then
        return false
    end
    
    return self.entityComponents[entityId] and self.entityComponents[entityId][componentName] or false
end

-- 컴포넌트 조합으로 엔티티 쿼리 (인덱스 캐시 활용)
--
-- 최적화 전략:
--   1. 요청된 컴포넌트 중 가장 적은 엔티티를 가진 인덱스를 "피벗"으로 선택
--   2. 피벗 인덱스의 엔티티들만 순회하며 나머지 컴포넌트 보유 여부 확인
--   → 전체 엔티티 순회(O(N_전체)) 대신 O(N_피벗) 으로 축소
--   → 예: 엔티티 2000개, Transform 2000개, LifeSpan 50개
--          LifeSpan이 피벗 → 50개만 검사
function ECS:queryEntities(componentNames)
    local numRequired = #componentNames
    if numRequired == 0 then
        return {}
    end
    
    -- 1. 가장 작은 인덱스를 가진 컴포넌트를 피벗으로 선택
    local pivotName = componentNames[1]
    local pivotIndex = self.componentIndex[pivotName]
    if not pivotIndex then
        return {}  -- 해당 컴포넌트를 가진 엔티티가 0개
    end
    
    local smallestSize = self:_indexSize(pivotName)
    for i = 2, numRequired do
        local name = componentNames[i]
        local idx = self.componentIndex[name]
        if not idx then
            return {}  -- 하나라도 0개면 결과도 0
        end
        local size = self:_indexSize(name)
        if size < smallestSize then
            smallestSize = size
            pivotName = name
            pivotIndex = idx
        end
    end
    
    -- 2. 피벗 인덱스 순회, 나머지 컴포넌트 확인
    local results = {}
    for entityId, _ in pairs(pivotIndex) do
        if self.entities[entityId] then  -- 아직 활성 엔티티인지 확인
            local hasAll = true
            for i = 1, numRequired do
                local name = componentNames[i]
                if name ~= pivotName then
                    local idx = self.componentIndex[name]
                    if not idx or not idx[entityId] then
                        hasAll = false
                        break
                    end
                end
            end
            if hasAll then
                results[#results + 1] = entityId
            end
        end
    end
    
    return results
end

-- 내부 함수: 인덱스 크기 계산 (피벗 선택용)
function ECS:_indexSize(componentName)
    local idx = self.componentIndex[componentName]
    if not idx then return 0 end
    local count = 0
    for _ in pairs(idx) do
        count = count + 1
    end
    return count
end

-- 통계 정보 반환
function ECS:getStats()
    return {
        activeEntities = self.stats.activeEntities,
        recycledIds = self.stats.recycledIds,
        totalCreated = self.stats.totalCreated,
        totalDestroyed = self.stats.totalDestroyed,
        componentTypes = self:_countComponentTypes()
    }
end

-- 내부 함수: 컴포넌트 타입별 카운트
function ECS:_countComponentTypes()
    local counts = {}
    for componentName, componentData in pairs(self.components) do
        local count = 0
        for _, _ in pairs(componentData) do
            count = count + 1
        end
        counts[componentName] = count
    end
    return counts
end

return ECS