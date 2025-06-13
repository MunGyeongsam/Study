//2단계 목표
//멤버 데이터 포인터 사용법 이해
//다양한 포인터 선언(T*, T**, T&, T&&, const T*, T* const) 경험
//Entity와 Component 간 구조를 더 유연하게 만들기


//개념						예시							설명
//멤버 데이터 포인터			int Entity::* ptr			객체의 특정 멤버 변수에 대한 포인터
//T*						int* p						int를 가리키는 포인터
//T**						int** p						포인터를 가리키는 포인터
//T&						int& r = x					참조자(lvalue reference)
//T &&						int&& r = std::move(x)		rvalue reference
//const T *					읽기 전용 포인터
//T* const					상수 포인터(자체는 불변, 가리키는 값은 변경 가능)

//연습 문제
//int Entity::* ptr과 int* ptr의 차이점을 설명해보세요.
//modifyMember 함수를 float Entity::* 로도 작동하도록 템플릿으로 만들어보세요.
//T* const와 const T* 의 차이를 코드 예제로 실험해보세요.

#include <iostream>
#include <string>

class Entity {
public:
	int hp = 100;
	int mana = 50;

	void print() const {
		std::cout << "HP: " << hp << ", Mana: " << mana << "\n";
	}
};

// 멤버 데이터 포인터를 이용한 함수
void modifyMember(Entity& entity, int Entity::* memberPtr, int newValue) {
	entity.*memberPtr = newValue;  // 멤버 포인터를 통해 접근
}

int main() {
	Entity player;

	int Entity::* hpPtr = &Entity::hp;      // Entity의 hp에 대한 포인터
	int Entity::* manaPtr = &Entity::mana;  // Entity의 mana에 대한 포인터

	player.print();  // 출력: HP: 100, Mana: 50

	modifyMember(player, hpPtr, 80);       // HP 수정
	modifyMember(player, manaPtr, 120);    // Mana 수정

	player.print();  // 출력: HP: 80, Mana: 120

	return 0;
}
