/*
struct Point
{
	int x;
	int y;
	// #1. 멤버 함수의 호출 원리
	// thiscall 사용 : 객체의 주소가 추가로 전달되는 함수.
	void set(int a, int b)		// void set(Point* this, int a, int b)
	{
		x = a;					// this->x = a;
		y = b;					// this->y = b;
	}
	// #2. static 멤버 함수의 호출원리
	static void foo(int a)		// void foo(int a) 로 컴파일, this 추가 안됨.
	{
		//x = a;
	}
};
int main()
{
	Point pt1;
	Point pt2;
	pt1.set(10, 20);	// Point::set(&pt1, 10, 20)
	pt2.set(10, 20);	// Point::set(&pt2, 10, 20)
	Point::foo(10);		// Point::foo(10);
	pt1.foo(10);		// Point::foo(10);
}
//*/