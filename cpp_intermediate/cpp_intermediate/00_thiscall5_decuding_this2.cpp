/*
#include <iostream>

class MyArray
{
	int buff[5] = { 1,2,3,4,5 };
public:
	using iterator = int*;
	using const_iterator = const int*;

	iterator begin() { return buff; }
	//	iterator begin() const { return buff;} // error
	const_iterator begin() const { return buff; } // ok

	// �ٽ� : ��� �Լ� �ۼ��� "�����̸��� const, non-const �Լ�" �� ����� ��찡 �����ϴ�.
};

int main()
{
	MyArray arr1;		// container
	const MyArray arr2;	// const container

	auto it1 = arr1.begin();
	auto it2 = arr2.begin();
}

//*/