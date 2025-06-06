// 3�ܰ� ��ǥ
// ��� �Լ� �����Ϳ� �� ȣ�� ��� ����
// ��ü ����� �ݹ� �ý��� ����
// Entity, Component, System �� �̺�Ʈ�� ���������� ó��

// ��� �Լ� ������
// class MyClass {
// public:
// 	void sayHello() { std::cout << "Hello\n"; }
// };
// 
// // ����
// void (MyClass::* funcPtr)() = &MyClass::sayHello;
// 
// // ȣ��
// (MyObject.*funcPtr)();


//���� ����
//����	����
//void (T::*)(Args...)		��� �Լ� �������� ����
//obj.*ptr					��� �Լ� �����͸� ��ü�� �Բ� ȣ��
//std::function				�پ��� �ݹ��� ���� ����
//ĸó��						����	��ü + ��� �Լ� �����͸� ���ٷ� ����


#include <iostream>
#include <functional>
#include <vector>

// ������ Entity Ŭ����
class Entity {
public:
	std::string name;
	Entity(const std::string& n) : name(n) {}
};

// �̺�Ʈ ������ ������ �� ������Ʈ
class HealthComponent {
public:
	int hp = 100;

	void onDamage(Entity& e, int amount) {
		hp -= amount;
		std::cout << e.name << " took " << amount << " damage. HP is now " << hp << "\n";
	}
};

// �ý���: �̺�Ʈ�� ó���� �� �ִ� �ܼ� �ݹ� �ý���
class EventSystem {
public:
	using Callback = void (HealthComponent::*)(Entity&, int);

	void registerCallback(HealthComponent* listener, Callback func) {
		callbacks.push_back([=](Entity& e, int amt) {
			(listener->*func)(e, amt);  // ��� �Լ� ������ ȣ��
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

	damageSystem.emit(knight, 30);  // ���: Knight took 30 damage. HP is now 70
	damageSystem.emit(knight, 20);  // ���: Knight took 20 damage. HP is now 50

	return 0;
}
