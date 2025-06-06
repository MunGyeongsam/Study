// 6단계 목표
// Entity라는 ID 기반의 객체 생성
// ComponentManager를 통해 컴포넌트를 관리
// System이라는 로직 집합을 도입(예: MovementSystem)
// 각 모듈의 역할과 관계를 이해하고 설계 구조를 파악



//	+--------------------+          +----------------------+
//	| Entity			 | <------> | ComponentManager     |
//	|  (ID only)         |          | -ID ↔ Components     |
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


// 이번 단계 요약
// 요소					역할
// Entity				ID만 갖는 존재, 데이터를 직접 보유하지 않음
// ComponentManager		ID별 컴포넌트를 저장 / 관리
// System				조건에 맞는 Entity의 Component를 사용해 동작 수행



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

	movement.update(1.0f); // 1초 경과

	std::cout << "Player Position: ("
		<< posManager.getComponent(player.id)->x << ", "
		<< posManager.getComponent(player.id)->y << ")\n";
}