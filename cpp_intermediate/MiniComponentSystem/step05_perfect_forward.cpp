// 5�ܰ� ��ǥ
// ������Ʈ�� ���� / �̵� ��� �����ϴ� �ϳ��� �Լ��� ����
// ���ø� + universal reference�� ����� �Ϻ� ����
// std::forward�� rvalue / lvalue�� Ư���� �����ϸ� ����

// �ٽ� �̷� ���
// ����						����
// T&&						���ø����� ���� ��� �� universal reference(lvalue& rvalue ��� ���� �� ����)
// std::forward<T>(arg)		arg�� �� Ư��(lvalue / rvalue)�� �״�� ������
// ����						�ϳ��� �Լ����� ����� �̵��� ��� ����ȭ�ϰ�, �ߺ��� ����


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

	template <typename T>
	void addComponent(T&& c) {
		std::cout << "Perfect forwarding version\n";
		components.emplace_back(std::forward<T>(c));  // ���� or �̵� �ڵ� ����
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
