// 5단계 목표
// 컴포넌트를 복사 / 이동 모두 지원하는 하나의 함수로 통합
// 템플릿 + universal reference를 사용한 완벽 전달
// std::forward로 rvalue / lvalue의 특성을 유지하며 전달
// 핵심 이론 요약
// 개념						설명
// T&&						템플릿에서 나올 경우 → universal reference(lvalue& rvalue 모두 받을 수 있음)
// std::forward<T>(arg)		arg의 값 특성(lvalue / rvalue)을 그대로 전달함
// 목적						하나의 함수에서 복사와 이동을 모두 최적화하고, 중복을 줄임
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
		components.emplace_back(std::forward<T>(c));  // 복사 or 이동 자동 결정
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