// 6�ܰ� ��ǥ
// Entity��� ID ����� ��ü ����
// ComponentManager�� ���� ������Ʈ�� ����
// System�̶�� ���� ������ ����(��: MovementSystem)
// �� ����� ���Ұ� ���踦 �����ϰ� ���� ������ �ľ�



//	+--------------------+          +----------------------+
//	| Entity			 | <------> | ComponentManager     |
//	|  (ID only)         |          | -ID �� Components     |
//	+--------------------+          +----------------------+
//	
//	|
//	v
//	
//	+-------------------------- +
//	| System                    |
//	| -operates on Entities     |
//	|   with certain Components |
//	+-------------------------- +
//	


// �̹� �ܰ� ���
// ���					����
// Entity				ID�� ���� ����, �����͸� ���� �������� ����
// ComponentManager		ID�� ������Ʈ�� ���� / ����
// System				���ǿ� �´� Entity�� Component�� ����� ���� ����



#include <unordered_map>
#include <iostream>

using EntityID = std::size_t;

class Entity {
public:
	EntityID id;

	explicit Entity(EntityID id_) : id(id_) {}

	bool operator==(const Entity& other) const {
		return id == other.id;
	}
};


struct Position {
	float x, y;
};

struct Velocity {
	float dx, dy;
};

template <typename T>
class ComponentManager {
public:
	std::unordered_map<EntityID, T> components;

	template <typename U>
	void addComponent(EntityID entity, U&& component) {
		components.emplace(entity, std::forward<U>(component));
	}

	T* getComponent(EntityID entity) {
		auto it = components.find(entity);
		if (it != components.end())
			return &it->second;
		return nullptr;
	}
};


class MovementSystem {
public:
	ComponentManager<Position>& positions;
	ComponentManager<Velocity>& velocities;

	MovementSystem(ComponentManager<Position>& pos, ComponentManager<Velocity>& vel)
		: positions(pos), velocities(vel) {}

	void update(float dt) {
		for (auto& [id, pos] : positions.components) {
			if (Velocity* vel = velocities.getComponent(id)) {
				pos.x += vel->dx * dt;
				pos.y += vel->dy * dt;
			}
		}
	}
};


int main() {
	ComponentManager<Position> posManager;
	ComponentManager<Velocity> velManager;

	Entity player(1), enemy(2);

	posManager.addComponent(player.id, Position{ 0.f, 0.f });
	velManager.addComponent(player.id, Velocity{ 10.f, 0.f });

	posManager.addComponent(enemy.id, Position{ 100.f, 100.f });
	// enemy has no velocity

	MovementSystem movement(posManager, velManager);

	movement.update(1.0f); // 1�� ���

	std::cout << "Player Position: ("
		<< posManager.getComponent(player.id)->x << ", "
		<< posManager.getComponent(player.id)->y << ")\n";
}