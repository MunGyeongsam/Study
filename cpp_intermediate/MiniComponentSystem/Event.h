#pragma once

struct Event {
	virtual ~Event() = default;
};

struct CollisionEvent : public Event {
	int entityA;
	int entityB;

	CollisionEvent(int a, int b) : entityA(a), entityB(b) {}
};

struct ScoreEvent : public Event {
	int entity;
	int scoreGained;


	ScoreEvent(int e, int s) : entity(e), scoreGained(s) {}
};

struct DamageEvent {
	int targetEntity;
	int damage;

	DamageEvent(int e, int d) : targetEntity(e), damage(d) {}
};