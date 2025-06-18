#pragma once

#include "System.h"
#include "Components.h"
#include "Coordinator.h"

extern Coordinator gCoordinator;
class DisplaySystem : public System {
public:
	void Display() {
		for (auto const& entity : entities) {
			auto* pos = gCoordinator.GetComponent<Position>(entity);
			auto* vel = gCoordinator.GetComponent<Velocity>(entity);

			std::cout << "Entity ID: " << entity << "\n";
			if(pos)
				std::cout << "  - Position: (" << pos->x << ", " << pos->y << ")\n";
			if(vel)
				std::cout << "  - Velocity: (" << vel->dx << ", " << vel->dy << ")\n";
		}
	}
};