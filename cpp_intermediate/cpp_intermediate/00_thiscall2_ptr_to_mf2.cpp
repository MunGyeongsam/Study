/*

#include <functional>

class X
{
public:
	void mf1(int a) {}				// void mf1(X* this, int a) {}
	static void mf2(int a) {}		// void mf2(int a) {}
};

void foo(int a) {}

int main()
{
	X x;

	// #1. �Ϲ� �Լ� ������ & ��� �Լ� ������

	void(*f1)(int) = &foo;
	void(X::*f2)(int) = &X::mf1;

	// #2. �Լ� �����͸� ����� �Լ� ȣ��
	f1(10);			// �Լ� ������ ���
	(x.*f2)(10);	// ��� �Լ� ������ ���


	// #3. ������ �����ϰ�
	// => c++17 std::invoke
	std::invoke(f1, 10);
	std::invoke(f2, x, 10);
	std::invoke(f2, &x, 10);

	return 0;
}
//*/