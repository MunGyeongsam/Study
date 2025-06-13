#include <iostream>
#include "Coordinator.h"
#include "Components.h"
#include "MovementSystem.h"
#include "EventBus.h"

Coordinator gCoordinator;

int main() {
	gCoordinator.Init();

	// ������Ʈ ���
	gCoordinator.RegisterComponent<Position>();
	gCoordinator.RegisterComponent<Velocity>();

	// �ý��� ��� �� �ñ״�ó ����
	auto movementSystem = gCoordinator.RegisterSystem<MovementSystem>();
	Signature sig;
	sig.set(gCoordinator.GetComponentType<Position>());
	sig.set(gCoordinator.GetComponentType<Velocity>());
	gCoordinator.SetSystemSignature<MovementSystem>(sig);

	// ��ƼƼ ���� �� ������Ʈ �߰�
	Entity e1 = gCoordinator.CreateEntity();
	gCoordinator.AddComponent(e1, Position{ 0.0f, 0.0f });
	gCoordinator.AddComponent(e1, Velocity{ 1.0f, 1.5f });

	// ������Ʈ ���� (��: 3������ �ùķ��̼�)
	for (int i = 0; i < 3; ++i) {
		movementSystem->Update(1.0f); // 1�� ����
		auto& pos = gCoordinator.GetComponent<Position>(e1);
		std::cout << "Frame " << i << ": Position = (" << pos.x << ", " << pos.y << ")\n";
	}

	{
		EventBus eventBus;

		eventBus.subscribe<CollisionEvent>([](const CollisionEvent& e) {
			std::cout << "Collision detected: " << e.entityA << " vs " << e.entityB << "\n";
			});

		eventBus.subscribe<ScoreEvent>([](const ScoreEvent& e) {
			std::cout << "Entity " << e.entity << " gained " << e.scoreGained << " points.\n";
			});

		eventBus.publish(CollisionEvent{ 1, 2 });
		eventBus.publish(ScoreEvent{ 1, 50 });
	}

	return 0;
}