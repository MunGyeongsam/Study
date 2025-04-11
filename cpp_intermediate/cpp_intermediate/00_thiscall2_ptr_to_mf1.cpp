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
	// #1. ��� �Լ� ������
	// => ���� �� ������?
	// => �Ϲ� �Լ� �����Ϳ� ��� �Լ��� ���� �� ����.
	// => �Ϲ� �Լ� �����Ϳ� static ��� �Լ��� ���� �� ����.

	void(*f1)(int) = &foo;		// ok
	//void(*f2)(int) = &X::mf1;	// error. this �� �߰��Ǵ� �Լ�.
	void(*f3)(int) = &X::mf2;	// ok

	void (X:: * f2)(int) = &X::mf1;	// ok. ����Լ��� �ּҸ� ��� ��� �Լ� ������!.
	// &foo == foo
	// &X::mf1 != X::mf1

	// #2. pointer to member operator
	f1(10);		// ?	ok
	//f2(10);	// ?	error
	f3(10);

	X x;
	x.mf2(10);	// error : f2��� ����� ����

	// .* : pointer to member operator
	//x.*f2(10);	// error : ������ �켱����
	(x.*f2)(10);	// ok

	return 0;
}
//*/