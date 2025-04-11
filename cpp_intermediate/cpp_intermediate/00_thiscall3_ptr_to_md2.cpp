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

	// #1. ��� �����͸� ����Ű�� �����͵� std::invoke �� �� ����
	// p �� �Լ��� �ƴ� ��� �������� �ּ�(Ȥ�� offset)

	cout << pt.y << endl;

	invoke(p, pt) = 20;		// pt.*p = 10, pt.y = 10
	cout << pt.y << endl;
	auto a1 = invoke(p, pt);	// ok. auto a = obj.y �� �ǹ�
	auto& a2 = invoke(p, pt);	// ok. auto& a = obj.y �� �ǹ�
	a1 = 30;
	cout << a1 << endl;
	cout << pt.y << endl;

	a2 = 30;
	cout << a2 << endl;
	cout << pt.y << endl;
	
	// callable object : std::invoke �� ȣ�Ⱑ���� ��� ��
	//	- �Լ� ������
	//	- �Լ� ��ü
	//	- ���� ǥ����
	//	- ��� �Լ� ������
	//	- ��� ������ ������

	return 0;
}

//*/