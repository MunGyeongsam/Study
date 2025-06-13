// 목표
//   - Entity의 ID를 부여하고 저장
//   - EntityManager를 통해 생성 / 삭제 관리
//   - 컴포넌트 추가 / 제거 / 조회 기능은 ComponentManager와 협력


// 설명
//   - Entity는 그냥 숫자지만, Signature를 통해 어떤 Component 조합을 갖는지 표현됩니다.
//   - mSignatures는 엔티티와 컴포넌트 조합을 연결하는 핵심입니다.
//   - CreateEntity()는 사용 가능한 ID를 하나 꺼내고 개수를 증가시킵니다.
//   - DestroyEntity()는 ID를 반환하고 시그니처를 초기화합니다.

#include <queue>
#include <bitset>
#include <array>
#include <cassert>

using Entity = std::uint32_t;
const Entity MAX_ENTITIES = 5000;

using Signature = std::bitset<64>;

class EntityManager {
public:
	EntityManager() {
		// Entity ID를 모두 큐에 초기화
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
	uint32_t mLivingEntityCount{};
};