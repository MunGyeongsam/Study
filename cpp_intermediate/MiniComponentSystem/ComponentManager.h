#pragma once

#include <unordered_map>
#include <memory>
#include <bitset>
#include <cassert>
#include <typeindex>
#include "Entity.h"
#include "Types.h"

// Component type ID generator
class ComponentTypeManager {
public:
	template <typename T>
	static ComponentType GetTypeID() {
		static const ComponentType typeId = nextId++;
		return typeId;
	}

private:
	static inline ComponentType nextId = 0;
};

// Interface for generic component arrays
class IComponentArray {
public:
	virtual ~IComponentArray() = default;
	virtual void EntityDestroyed(Entity entity) = 0;
};

// Concrete component array
template <typename T>
class ComponentArray : public IComponentArray {
public:
	void InsertData(Entity entity, T component) {
		assert(entityToIndex.find(entity) == entityToIndex.end() && "Component already exists!");
		std::size_t index = size;
		entityToIndex[entity] = index;
		indexToEntity[index] = entity;
		data[index] = component;
		++size;
	}

	void RemoveData(Entity entity) {
		assert(entityToIndex.find(entity) != entityToIndex.end() && "No such component!");
		std::size_t index = entityToIndex[entity];
		std::size_t lastIndex = size - 1;
		data[index] = data[lastIndex];

		Entity lastEntity = indexToEntity[lastIndex];
		entityToIndex[lastEntity] = index;
		indexToEntity[index] = lastEntity;

		entityToIndex.erase(entity);
		indexToEntity.erase(lastIndex);
		--size;
	}

	T& GetData(Entity entity) {
		assert(entityToIndex.find(entity) != entityToIndex.end() && "No such component!");
		return data[entityToIndex[entity]];
	}

	void EntityDestroyed(Entity entity) override {
		if (entityToIndex.find(entity) != entityToIndex.end()) {
			RemoveData(entity);
		}
	}

private:
	std::array<T, MAX_ENTITIES> data{};
	std::unordered_map<Entity, std::size_t> entityToIndex{};
	std::unordered_map<std::size_t, Entity> indexToEntity{};
	std::size_t size = 0;
};

// Component manager
class ComponentManager {
public:
	template <typename T>
	void RegisterComponent() {
		const char* typeName = typeid(T).name();
		assert(componentTypes.find(typeName) == componentTypes.end() && "Component already registered!");

		componentTypes[typeName] = ComponentTypeManager::GetTypeID<T>();
		componentArrays[typeName] = std::make_shared<ComponentArray<T>>();
	}

	template <typename T>
	void AddComponent(Entity entity, T component) {
		GetComponentArray<T>()->InsertData(entity, component);
	}

	template <typename T>
	void RemoveComponent(Entity entity) {
		GetComponentArray<T>()->RemoveData(entity);
	}

	template <typename T>
	T& GetComponent(Entity entity) {
		return GetComponentArray<T>()->GetData(entity);
	}

	template <typename T>
	ComponentType GetComponentType() {
		const char* typeName = typeid(T).name();
		assert(componentTypes.find(typeName) != componentTypes.end() && "Component not registered!");
		return componentTypes[typeName];
	}

	void EntityDestroyed(Entity entity) {
		for (auto const& pair : componentArrays) {
			auto const& array = pair.second;
			array->EntityDestroyed(entity);
		}
	}

private:
	std::unordered_map<const char*, ComponentType> componentTypes{};
	std::unordered_map<const char*, std::shared_ptr<IComponentArray>> componentArrays{};

	template <typename T>
	std::shared_ptr<ComponentArray<T>> GetComponentArray() {
		const char* typeName = typeid(T).name();
		assert(componentTypes.find(typeName) != componentTypes.end() && "Component not registered!");
		return std::static_pointer_cast<ComponentArray<T>>(componentArrays[typeName]);
	}
};
