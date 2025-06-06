//1�ܰ� ��ǥ
//Entity�� Component��� �⺻ ���� �����
//������Ʈ�� �����ͷ� �ٷ�� �� ������
//Ŭ������ ����ü�� �޸� ���� ����

//����				����
//struct / class	C++���� ����� ���� Ÿ��
//������(T*)			�޸� �ּҸ� ����, ��ü�� �������� �ٷ� �� ���
//new / delete		�� �޸� �Ҵ� / ����
//������ / �Ҹ���		��ü ������ �ı� �� �ڵ� ȣ��


//PositionComponent ���� HealthComponent(int hp)�� �߰��غ�����.
//Entity �̸��� �������� getName() �Լ��� ��������.
//�Ҹ��ڰ� ������ ���� ������ ������ �����غ�����.

#include <iostream>
#include <vector>
#include <string>

// Base Component class
struct Component {
	virtual ~Component() {}
	virtual void info() = 0;
};

// PositionComponent
struct PositionComponent : public Component {
	int x, y;
	PositionComponent(int x, int y) : x(x), y(y) {}

	void info() override {
		std::cout << "Position: (" << x << ", " << y << ")\n";
	}
};

// Entity class
class Entity {
	std::string name;
	std::vector<Component*> components;

public:
	Entity(const std::string& name) : name(name) {}

	~Entity() {
		for (auto comp : components)
			delete comp;  // ������Ʈ �޸� ����
	}

	void addComponent(Component* c) {
		components.push_back(c);
	}

	void showComponents() {
		std::cout << "Entity: " << name << "\n";
		for (auto comp : components)
			comp->info();
	}
};

int main() {
	Entity player("Player1");
	player.addComponent(new PositionComponent(10, 20));
	player.showComponents();

	return 0;
}