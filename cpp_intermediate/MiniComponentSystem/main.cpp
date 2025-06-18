#include <iostream>
#include "Coordinator.h"
#include "Components.h"
#include "DisplaySystem.h"
#include "MovementSystem.h"
#include "EventBus.h"

Coordinator gCoordinator;
int main() {
	gCoordinator.Init();
	// 컴포넌트 등록
	gCoordinator.RegisterComponent<Position>();
	gCoordinator.RegisterComponent<Velocity>();

	// 시스템 등록 및 시그니처 설정
	auto movementSystem = gCoordinator.RegisterSystem<MovementSystem>();
	Signature sig;
	sig.set(gCoordinator.GetComponentType<Position>());
	sig.set(gCoordinator.GetComponentType<Velocity>());
	gCoordinator.SetSystemSignature<MovementSystem>(sig);

	auto displaySystem = gCoordinator.RegisterSystem<DisplaySystem>();
	gCoordinator.SetSystemSignature<DisplaySystem>(sig);


	// 엔티티 생성 및 컴포넌트 추가
	Entity e1 = gCoordinator.CreateEntity();
	gCoordinator.AddComponent(e1, Position{ 0.0f, 0.0f });
	
	Entity e2 = gCoordinator.CreateEntity();
	gCoordinator.AddComponent(e2, Position{ 10.0f, 10.0f });
	gCoordinator.AddComponent(e2, Velocity{ 2.0f, 2.0f });

	// 업데이트 루프 (예: 3프레임 시뮬레이션)
	for (int i = 0; i < 3; ++i) {
		movementSystem->Update(1.0f); // 1초 단위
		auto* pos = gCoordinator.GetComponent<Position>(e1);
		std::cout << "Frame " << i << ": Position = (" << pos->x << ", " << pos->y << ")\n";

		displaySystem->Display();
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