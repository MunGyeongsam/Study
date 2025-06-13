// EntityManager.hpp
#pragma once

#include <queue>
#include <array>
#include <bitset>
#include <cassert>
#include "Entity.h"
#include "Types.h"

class EntityManager {
public:
	EntityManager() {
		// 사용할 수 있는 모든 Entity ID를 큐에 삽입
		for (Entity entity = 0; entity < MAX_ENTITIES; ++entity) {
			mAvailableEntities.push(entity);
		}
	}

	Entity CreateEntity() {
		assert(mLivingEntityCount < MAX_ENTITIES && "Too many entities in existence.");
		Entity id = mAvailableEntities.front();
		mAvailableEntities.pop();
		++mLivingEntityCount;
		return id;
	}

	void DestroyEntity(Entity entity) {
		assert(entity < MAX_ENTITIES && "Entity out of range.");
		mSignatures[entity].reset();
		mAvailableEntities.push(entity);
		--mLivingEntityCount;
	}

	void SetSignature(Entity entity, Signature signature) {
		assert(entity < MAX_ENTITIES && "Entity out of range.");
		mSignatures[entity] = signature;
	}

	Signature GetSignature(Entity entity) const {
		assert(entity < MAX_ENTITIES && "Entity out of range.");
		return mSignatures[entity];
	}

private:
	std::queue<Entity> mAvailableEntities{};
	std::array<Signature, MAX_ENTITIES> mSignatures{};
	std::uint32_t mLivingEntityCount{};
};
