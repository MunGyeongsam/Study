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
	// #1. 멤버 함수 포인터
	// => 다음 중 에러는?
	// => 일반 함수 포인터에 멤버 함수를 담을 수 없다.
	// => 일반 함수 포인터에 static 멤버 함수를 담을 수 없다.

	void(*f1)(int) = &foo;		// ok
	//void(*f2)(int) = &X::mf1;	// error. this 가 추가되는 함수.
	void(*f3)(int) = &X::mf2;	// ok

	void (X:: * f2)(int) = &X::mf1;	// ok. 멤버함수의 주소를 담는 멤버 함수 포인터!.
	// &foo == foo
	// &X::mf1 != X::mf1

	// #2. pointer to member operator
	f1(10);		// ?	ok
	//f2(10);	// ?	error
	f3(10);

	X x;
	x.mf2(10);	// error : f2라는 멤버가 없음

	// .* : pointer to member operator
	//x.*f2(10);	// error : 연산자 우선순위
	(x.*f2)(10);	// ok

	return 0;
}
//*/