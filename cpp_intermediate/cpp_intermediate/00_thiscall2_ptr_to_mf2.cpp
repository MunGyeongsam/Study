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

	// #1. 일반 함수 포인터 & 멤버 함수 포인터

	void(*f1)(int) = &foo;
	void(X::*f2)(int) = &X::mf1;

	// #2. 함수 포인터를 사용한 함수 호출
	f1(10);			// 함수 포인터 사용
	(x.*f2)(10);	// 멤버 함수 포인터 사용


	// #3. 사용법을 동일하게
	// => c++17 std::invoke
	std::invoke(f1, 10);
	std::invoke(f2, x, 10);
	std::invoke(f2, &x, 10);

	return 0;
}
//*/