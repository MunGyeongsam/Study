
/*

struct Point
{
	int x;
	int y;

	// #1. ��� �Լ��� ȣ�� ����
	// thiscall ��� : ��ü�� �ּҰ� �߰��� ���޵Ǵ� �Լ�.
	void set(int a, int b)		// void set(Point* this, int a, int b)
	{
		x = a;					// this->x = a;
		y = b;					// this->y = b;
	}

	// #2. static ��� �Լ��� ȣ�����
	static void foo(int a)		// void foo(int a) �� ������, this �߰� �ȵ�.
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