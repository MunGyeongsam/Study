/*

#include <functional>
#include <iostream>

using namespace std;

struct Point
{
	int x;
	int y;
};

int main()
{
	int Point::* p = &Point::y;		// 4

	Point pt;

	pt.*p = 10;
	(&pt)->*p = 10;

	// #1. 멤버 데이터를 가르키는 포인터도 std::invoke 될 수 있음
	// p 는 함수가 아닌 멤버 데이터의 주소(혹은 offset)

	cout << pt.y << endl;

	invoke(p, pt) = 20;		// pt.*p = 10, pt.y = 10
	cout << pt.y << endl;
	auto a1 = invoke(p, pt);	// ok. auto a = obj.y 의 의미
	auto& a2 = invoke(p, pt);	// ok. auto& a = obj.y 의 의미
	a1 = 30;
	cout << a1 << endl;
	cout << pt.y << endl;

	a2 = 30;
	cout << a2 << endl;
	cout << pt.y << endl;
	
	// callable object : std::invoke 로 호출가능한 모든 것
	//	- 함수 포인터
	//	- 함수 객체
	//	- 람다 표현식
	//	- 멤버 함수 포인터
	//	- 멤버 데이터 포인터

	return 0;
}

//*/