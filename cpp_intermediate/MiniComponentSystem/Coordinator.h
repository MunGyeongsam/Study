// Coordinator.hpp
#pragma once
#include "EntityManager.h"
#include "ComponentManager.h"
#include "SystemManager.h"
#include "Types.h"
#include "EventBus.h"
class Coordinator {
public:
	void Init() {
		entityManager = std::make_unique<EntityManager>();
		componentManager = std::make_unique<ComponentManager>();
		systemManager = std::make_unique<SystemManager>();
		eventBus = std::make_unique<EventBus>();
	}
	EventBus& GetEventBus() {
		return *eventBus;
	}
	// ----------------------------
	// Entity 관련
	// ----------------------------
	Entity CreateEntity() {
		return entityManager->CreateEntity();
	}
	void DestroyEntity(Entity entity) {
		entityManager->DestroyEntity(entity);
		componentManager->EntityDestroyed(entity);
		systemManager->EntityDestroyed(entity);
	}
	// ----------------------------
	// Component 관련
	// ----------------------------
	template<typename T>
	void RegisterComponent() {
		componentManager->RegisterComponent<T>();
	}
	template<typename T>
	void AddComponent(Entity entity, T component) {
		componentManager->AddComponent<T>(entity, component);
		auto signature = entityManager->GetSignature(entity);
		signature.set(componentManager->GetComponentType<T>(), true);
		entityManager->SetSignature(entity, signature);
		systemManager->EntitySignatureChanged(entity, signature);
	}
	template<typename T>
	void RemoveComponent(Entity entity) {
		componentManager->RemoveComponent<T>(entity);
		auto signature = entityManager->GetSignature(entity);
		signature.set(componentManager->GetComponentType<T>(), false);
		entityManager->SetSignature(entity, signature);
		systemManager->EntitySignatureChanged(entity, signature);
	}
	template<typename T>
	T* GetComponent(Entity entity) {
		return componentManager->GetComponent<T>(entity);
	}
	template<typename T>
	ComponentType GetComponentType() {
		return componentManager->GetComponentType<T>();
	}
	// ----------------------------
	// System 관련
	// ----------------------------
	template<typename T>
	std::shared_ptr<T> RegisterSystem() {
		return systemManager->RegisterSystem<T>();
	}
	template<typename T>
	void SetSystemSignature(Signature signature) {
		systemManager->SetSignature<T>(signature);
	}
private:
	std::unique_ptr<EntityManager> entityManager;
	std::unique_ptr<ComponentManager> componentManager;
	std::unique_ptr<SystemManager> systemManager;
	std::unique_ptr<EventBus> eventBus;
};
#pragma once