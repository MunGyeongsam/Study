// 4단계 목표
// rvalue / lvalue의 실제 구분과 활용
// std::move와 이동 생성자 / 이동 대입 연산자 적용
// 컴포넌트 등록 시 복사 대신 이동을 사용하는 방식 구현


// 핵심	개념 정리
// 개념								설명
// lvalue							이름이 있고, 주소를 참조할 수 있는 객체
// rvalue							일시적 값, 이름이 없는 임시 객체(예: MyData(5))
// std::move						lvalue를 rvalue로 캐스팅해, 이동 연산자를 호출 가능하게 함
// 이동 생성자						자원의 소유권을 이전하고, 원본은 비워 놓음
// 완벽 전달(perfect forwarding)		std::forward<T>(arg)로 원래의 값 특성을 유지하며 인자 전달



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

	manager.addComponent(c1);                // 복사
	std::cout << std::endl;

	manager.addComponent(std::move(c1));     // 이동
	std::cout << std::endl;

	manager.addComponent(HeavyComponent(std::vector<int>(500, 1))); // 임시 객체 → 이동
	std::cout << std::endl;

	return 0;
}
