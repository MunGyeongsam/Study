//1단계 목표
//Entity와 Component라는 기본 구조 만들기
//컴포넌트를 포인터로 다루는 법 익히기
//클래스와 구조체의 메모리 관계 이해
//개념				설명
//struct / class	C++에서 사용자 정의 타입
//포인터(T*)			메모리 주소를 저장, 객체를 동적으로 다룰 때 사용
//new / delete		힙 메모리 할당 / 해제
//생성자 / 소멸자		객체 생성과 파괴 시 자동 호출
//PositionComponent 말고 HealthComponent(int hp)를 추가해보세요.
//Entity 이름을 가져오는 getName() 함수를 만들어보세요.
//소멸자가 없으면 무슨 문제가 생길지 설명해보세요.
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
			delete comp;  // 컴포넌트 메모리 해제
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