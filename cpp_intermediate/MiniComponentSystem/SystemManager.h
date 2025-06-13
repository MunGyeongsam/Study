// SystemManager.hpp
#pragma once

#include <memory>
#include <unordered_map>
#include <typeindex>
#include <cassert>
#include "System.h"
#include "Types.h"
#include "Entity.h"

class SystemManager {
public:
	// 시스템 등록
	template<typename T>
	std::shared_ptr<T> RegisterSystem() {
		const std::type_index typeName = typeid(T);
		assert(systems.find(typeName) == systems.end() && "System already registered");

		auto system = std::make_shared<T>();
		systems[typeName] = system;
		return system;
	}

	// 시스템의 관심 컴포넌트 시그니처 설정
	template<typename T>
	void SetSignature(Signature signature) {
		const std::type_index typeName = typeid(T);
		assert(systems.find(typeName) != systems.end() && "System not registered");

		signatures[typeName] = signature;
	}

	// Entity가 제거되면 모든 시스템에서 제거
	void EntityDestroyed(Entity entity) {
		for (auto& [type, system] : systems) {
			system->entities.erase(entity);
		}
	}

	// Entity의 Signature가 바뀌면, 어떤 시스템에 포함될지 다시 판단
	void EntitySignatureChanged(Entity entity, Signature entitySignature) {
		for (auto& [type, system] : systems) {
			const Signature& systemSig = signatures[type];
			if ((entitySignature & systemSig) == systemSig) {
				system->entities.insert(entity);
			}
			else {
				system->entities.erase(entity);
			}
		}
	}

private:
	std::unordered_map<std::type_index, std::shared_ptr<System>> systems;
	std::unordered_map<std::type_index, Signature> signatures;
};
