/*
#include <iostream>

class Object
{
public:
	// C++23 ���� ��Ÿ��
	void foo() {}	// void foo(Object* obj)       {}
	void foo() const {} // void foo(const Object* obj) {}

	// C++23 ��Ÿ��.!
	void goo(this Object& self) {}
	void goo(this const Object& self) {}

	// deducing this
	// C++23 ���ʹ� ���ø��� ����ϸ� const, non-const ��� �Լ��� �ڵ������� ������ ���ϴ�.
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