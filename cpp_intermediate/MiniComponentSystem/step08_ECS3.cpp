// 목표
//   - 컴포넌트 타입을 등록
//   - 엔티티에 컴포넌트 추가 / 삭제 / 조회
//   - 각 컴포넌트는 타입별 배열로 저장
//   - 각 타입은 별도의 ComponentArray<T>로 저장됨


// 핵심 구조
//   - ComponentArray<T>
//     - 특정 타입 T의 컴포넌트를 엔티티 ID에 따라 저장하고 조회하는 클래스
//   - ComponentManager
//     - 다양한 타입의 컴포넌트를 등록하고 ComponentArray<T>를 관리
//     - T → ComponentType ID → ComponentArray<T> 매핑


#include <unordered_map>
#include <iostream>


using EntityID = std::uint32_t;
const EntityID MAX_ENTITIES = 5000;

class Entity {
public:
	EntityID id;

	explicit Entity(EntityID id_) : id(id_) {}

	bool operator==(const Entity& other) const {
		return id == other.id;
	}
};


#pragma once
#include <unordered_map>
#include <cassert>
#include <array>

template<typename T>
class ComponentArray {
public:
	void InsertData(Entity entity, T component) {
		assert(mEntityToIndexMap.find(entity) == mEntityToIndexMap.end() && "Component added to same entity more than once.");

		size_t newIndex = mSize;
		mEntityToIndexMap[entity] = newIndex;
		mIndexToEntityMap[newIndex] = entity;
		mComponentArray[newIndex] = component;
		++mSize;
	}

	void RemoveData(Entity entity) {
		assert(mEntityToIndexMap.find(entity) != mEntityToIndexMap.end() && "Removing non-existent component.");

		size_t indexOfRemovedEntity = mEntityToIndexMap[entity];
		size_t indexOfLastElement = mSize - 1;
		mComponentArray[indexOfRemovedEntity] = mComponentArray[indexOfLastElement];

		Entity entityOfLastElement = mIndexToEntityMap[indexOfLastElement];
		mEntityToIndexMap[entityOfLastElement] = indexOfRemovedEntity;
		mIndexToEntityMap[indexOfRemovedEntity] = entityOfLastElement;

		mEntityToIndexMap.erase(entity);
		mIndexToEntityMap.erase(indexOfLastElement);
		--mSize;
	}

	T& GetData(Entity entity) {
		assert(mEntityToIndexMap.find(entity) != mEntityToIndexMap.end() && "Retrieving non-existent component.");
		return mComponentArray[mEntityToIndexMap[entity]];
	}

	void EntityDestroyed(Entity entity) {
		if (mEntityToIndexMap.find(entity) != mEntityToIndexMap.end()) {
			RemoveData(entity);
		}
	}

private:
	std::array<T, MAX_ENTITIES> mComponentArray{};
	std::unordered_map<Entity, size_t> mEntityToIndexMap{};
	std::unordered_map<size_t, Entity> mIndexToEntityMap{};
	size_t mSize{};
};


#include <unordered_map>
#include <memory>
#include <typeindex>
#include <type_traits>
#include <cassert>

class ComponentManager {
public:
	template<typename T>
	void RegisterComponent() {
		std::type_index typeId = std::type_index(typeid(T));
		assert(componentTypes.find(typeId) == componentTypes.end() && "Component already registered.");

		componentTypes[typeId] = nextComponentType;
		componentArrays[typeId] = std::make_shared<ComponentArray<T>>();
		++nextComponentType;
	}

	template<typename T>
	void AddComponent(Entity entity, T component) {
		GetComponentArray<T>()->InsertData(entity, component);
	}

	template<typename T>
	void RemoveComponent(Entity entity) {
		GetComponentArray<T>()->RemoveData(entity);
	}

	template<typename T>
	T& GetComponent(Entity entity) {
		return GetComponentArray<T>()->GetData(entity);
	}

	template<typename T>
	ComponentType GetComponentType() {
		std::type_index typeId = std::type_index(typeid(T));
		assert(componentTypes.find(typeId) != componentTypes.end() && "Component not registered.");
		return componentTypes[typeId];
	}

	void EntityDestroyed(Entity entity) {
		for (auto const& pair : componentArrays) {
			pair.second->EntityDestroyed(entity);
		}
	}

private:
	std::unordered_map<std::type_index, ComponentType> componentTypes{};
	std::unordered_map<std::type_index, std::shared_ptr<IComponentArray>> componentArrays{};
	ComponentType nextComponentType = 0;

	template<typename T>
	std::shared_ptr<ComponentArray<T>> GetComponentArray() {
		std::type_index typeId = std::type_index(typeid(T));
		assert(componentTypes.find(typeId) != componentTypes.end() && "Component not registered.");
		return std::static_pointer_cast<ComponentArray<T>>(componentArrays[typeId]);
	}
};