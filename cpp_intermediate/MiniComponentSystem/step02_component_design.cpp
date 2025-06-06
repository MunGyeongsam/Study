//2�ܰ� ��ǥ
//��� ������ ������ ���� ����
//�پ��� ������ ����(T*, T**, T&, T&&, const T*, T* const) ����
//Entity�� Component �� ������ �� �����ϰ� �����


//����						����							����
//��� ������ ������			int Entity::* ptr			��ü�� Ư�� ��� ������ ���� ������
//T*						int* p						int�� ����Ű�� ������
//T**						int** p						�����͸� ����Ű�� ������
//T&						int& r = x					������(lvalue reference)
//T &&						int&& r = std::move(x)		rvalue reference
//const T *					�б� ���� ������
//T* const					��� ������(��ü�� �Һ�, ����Ű�� ���� ���� ����)

//���� ����
//int Entity::* ptr�� int* ptr�� �������� �����غ�����.
//modifyMember �Լ��� float Entity::* �ε� �۵��ϵ��� ���ø����� ��������.
//T* const�� const T* �� ���̸� �ڵ� ������ �����غ�����.

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

// ��� ������ �����͸� �̿��� �Լ�
void modifyMember(Entity& entity, int Entity::* memberPtr, int newValue) {
	entity.*memberPtr = newValue;  // ��� �����͸� ���� ����
}

int main() {
	Entity player;

	int Entity::* hpPtr = &Entity::hp;      // Entity�� hp�� ���� ������
	int Entity::* manaPtr = &Entity::mana;  // Entity�� mana�� ���� ������

	player.print();  // ���: HP: 100, Mana: 50

	modifyMember(player, hpPtr, 80);       // HP ����
	modifyMember(player, manaPtr, 120);    // Mana ����

	player.print();  // ���: HP: 80, Mana: 120

	return 0;
}
