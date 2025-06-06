// 3단계 목표
// 멤버 함수 포인터와 그 호출 방식 이해
// 객체 기반의 콜백 시스템 구현
// Entity, Component, System 간 이벤트를 구조적으로 처리

// 멤버 함수 포인터
// class MyClass {
// public:
// 	void sayHello() { std::cout << "Hello\n"; }
// };
// 
// // 선언
// void (MyClass::* funcPtr)() = &MyClass::sayHello;
// 
// // 호출
// (MyObject.*funcPtr)();


//요점 정리
//개념	설명
//void (T::*)(Args...)		멤버 함수 포인터의 문법
//obj.*ptr					멤버 함수 포인터를 객체와 함께 호출
//std::function				다양한 콜백을 통합 관리
//캡처된						람다	객체 + 멤버 함수 포인터를 람다로 래핑


#include <iostream>
#include <functional>
#include <vector>

// 간단한 Entity 클래스
class Entity {
public:
	std::string name;
	Entity(const std::string& n) : name(n) {}
};

// 이벤트 리스너 역할을 할 컴포넌트
class HealthComponent {
public:
	int hp = 100;

	void onDamage(Entity& e, int amount) {
		hp -= amount;
		std::cout << e.name << " took " << amount << " damage. HP is now " << hp << "\n";
	}
};

// 시스템: 이벤트를 처리할 수 있는 단순 콜백 시스템
class EventSystem {
public:
	using Callback = void (HealthComponent::*)(Entity&, int);

	void registerCallback(HealthComponent* listener, Callback func) {
		callbacks.push_back([=](Entity& e, int amt) {
			(listener->*func)(e, amt);  // 멤버 함수 포인터 호출
			});
	}

	void emit(Entity& entity, int amount) {
		for (auto& cb : callbacks)
			cb(entity, amount);
	}

private:
	std::vector<std::function<void(Entity&, int)>> callbacks;
};

int main() {
	Entity knight("Knight");
	HealthComponent knightHealth;

	EventSystem damageSystem;
	damageSystem.registerCallback(&knightHealth, &HealthComponent::onDamage);

	damageSystem.emit(knight, 30);  // 출력: Knight took 30 damage. HP is now 70
	damageSystem.emit(knight, 20);  // 출력: Knight took 20 damage. HP is now 50

	return 0;
}
