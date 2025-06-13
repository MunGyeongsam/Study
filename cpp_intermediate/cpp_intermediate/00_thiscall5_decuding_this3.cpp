/*
#include <iostream>

class Object
{
public:
	// C++23 이전 스타일
	void foo() {}	// void foo(Object* obj)       {}
	void foo() const {} // void foo(const Object* obj) {}

	// C++23 스타일.!
	void goo(this Object& self) {}
	void goo(this const Object& self) {}

	// deducing this
	// C++23 부터는 템플릿을 사용하면 const, non-const 멤버 함수의 자동생성이 가능해 집니다.
	template<typename T>
	void hoo(this T& self)
	{}
};

int main()
{
	Object obj1;
	const Object obj2;

	obj1.foo();
	obj2.foo();

	obj1.goo();
	obj2.goo();

	obj1.hoo();
	obj2.hoo();
}

//*/