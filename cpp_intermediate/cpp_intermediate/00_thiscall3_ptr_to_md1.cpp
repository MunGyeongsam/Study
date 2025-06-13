/*

#include <iostream>
#include <stdio.h>

using namespace std;


void cout_operator(void*);
void cout_operator(bool);

struct Point
{
	//bool b1;
	//bool b11;
	//bool b12;
	int x;
	//bool b2;
	int y;
};

int main()
{
	int a = 3;

	// #1. pointer to member data
	
	//? = &a;
	//? = &Point::x;

	int* p1 = &a;
	//int(Point::* p2) = &Point::y;
	int Point::* p2 = &Point::y;		// 멤버 변수에 대한 포인터
										// y 라는 멤버가 Point 에서 얼마나 떨어진 곳에?
										// 4

	cout << p2 << endl;
	printf("p2 : %d\n", p2);


	//cout_operator(p1);
	//cout_operator(p2);
	// 
	//std::cout << "p1 : " << p1 << ", sizeof(p1) : " << sizeof(p1) << ", type : " << typeid(p1).name() << std::endl;
	//std::cout << "p2 : " << p2 << ", sizeof(p2) : " << sizeof(p2) << ", type : " << typeid(p2).name() << std::endl;


	// 생각해 봅시다
	//int& r1 = a;
	//int(Point::& r2) = &Point::y;


	// #2. using pointer to member data
	// 
	//*p1 = 10;	// ok
	//*p2 = 10;	// ?

	Point pt;
	pt.*p2 = 10;
	(&pt)->*p2 = 10;


	return 0;
}


// void* 에 멤버데이터 혹은 멤버함수의 주소를 담을 수 없음.
void cout_operator(void*)
{
	std::cout << "cout_operator(void*)" << std::endl;
}
void cout_operator(bool)
{
	std::cout << "cout_operator(bool)" << std::endl;
}

//*/