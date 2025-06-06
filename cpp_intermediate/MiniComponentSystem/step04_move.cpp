// 4�ܰ� ��ǥ
// rvalue / lvalue�� ���� ���а� Ȱ��
// std::move�� �̵� ������ / �̵� ���� ������ ����
// ������Ʈ ��� �� ���� ��� �̵��� ����ϴ� ��� ����


// �ٽ�	���� ����
// ����								����
// lvalue							�̸��� �ְ�, �ּҸ� ������ �� �ִ� ��ü
// rvalue							�Ͻ��� ��, �̸��� ���� �ӽ� ��ü(��: MyData(5))
// std::move						lvalue�� rvalue�� ĳ������, �̵� �����ڸ� ȣ�� �����ϰ� ��
// �̵� ������						�ڿ��� �������� �����ϰ�, ������ ��� ����
// �Ϻ� ����(perfect forwarding)		std::forward<T>(arg)�� ������ �� Ư���� �����ϸ� ���� ����



#include <iostream>
#include <vector>
#include <string>

class HeavyComponent {
public:
	std::vector<int> bigData;

	HeavyComponent() = default;

	HeavyComponent(const std::vector<int>& data) : bigData(data) {
		std::cout << "Copy Constructor\n";
	}

	HeavyComponent(std::vector<int>&& data) : bigData(std::move(data)) {
		std::cout << "Move Constructor\n";
	}

	HeavyComponent(const HeavyComponent& other) : bigData(other.bigData) {
		std::cout << "Copy Constructor (HeavyComponent)\n";
	}

	HeavyComponent(HeavyComponent&& other) noexcept : bigData(std::move(other.bigData)) {
		std::cout << "Move Constructor (HeavyComponent)\n";
	}

	HeavyComponent& operator=(HeavyComponent&& other) noexcept {
		if (this != &other) {
			bigData = std::move(other.bigData);
			std::cout << "Move Assignment\n";
		}
		return *this;
	}
};


class ComponentManager {
public:
	std::vector<HeavyComponent> components;

	void addComponent(const HeavyComponent& c) {
		std::cout << "Adding by copy\n";
		components.push_back(c);
	}

	void addComponent(HeavyComponent&& c) {
		std::cout << "Adding by move\n";
		components.push_back(std::move(c));
	}
};


int main() {
	ComponentManager manager;

	HeavyComponent c1(std::vector<int>(1000, 42));  // big data
	std::cout << std::endl;

	manager.addComponent(c1);                // ����
	std::cout << std::endl;

	manager.addComponent(std::move(c1));     // �̵�
	std::cout << std::endl;

	manager.addComponent(HeavyComponent(std::vector<int>(500, 1))); // �ӽ� ��ü �� �̵�
	std::cout << std::endl;

	return 0;
}
