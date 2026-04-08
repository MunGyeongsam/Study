#pragma once
#include "System.h"
#include "Components.h"
#include "Coordinator.h"
extern Coordinator gCoordinator;
class MovementSystem : public System {
public:
	void Update(float dt) {
		for (auto const& entity : entities) {
			auto* pos = gCoordinator.GetComponent<Position>(entity);
			auto* vel = gCoordinator.GetComponent<Velocity>(entity);
			if (!pos || !vel) {
				continue; // Skip if the entity does not have Position or Velocity components
			}
			pos->x += vel->dx * dt;
			pos->y += vel->dy * dt;
		}
	}
};