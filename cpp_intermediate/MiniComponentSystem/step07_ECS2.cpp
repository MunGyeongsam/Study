// ��ǥ
//   - Entity�� ID�� �ο��ϰ� ����
//   - EntityManager�� ���� ���� / ���� ����
//   - ������Ʈ �߰� / ���� / ��ȸ ����� ComponentManager�� ����


// ����
//   - Entity�� �׳� ��������, Signature�� ���� � Component ������ ������ ǥ���˴ϴ�.
//   - mSignatures�� ��ƼƼ�� ������Ʈ ������ �����ϴ� �ٽ��Դϴ�.
//   - CreateEntity()�� ��� ������ ID�� �ϳ� ������ ������ ������ŵ�ϴ�.
//   - DestroyEntity()�� ID�� ��ȯ�ϰ� �ñ״�ó�� �ʱ�ȭ�մϴ�.

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
		// Entity ID�� ��� ť�� �ʱ�ȭ
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