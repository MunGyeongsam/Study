#include "pch.h";
#include "FixedArray2D.h"

namespace {
	using ka::FixedArray2D1st;
	using ka::FixedArray2D;
	using namespace std;

	template<typename T, int R, int C>
	void Print(T const(&arr)[R][C])
	{
		cout << R << " x " << C << endl;
		for (int i = 0; i < R; ++i)
		{
			for (int j = 0; j < C; ++j)
				cout << arr[i][j] << ' ';
			cout << endl;
		}
	}

}

TEST(FixedArray2D, FixedArray2D1st) {
	FixedArray2D1st<int, 2, 3> a;

	ASSERT_TRUE(a.Length() == 6);
	ASSERT_TRUE(a.Row() == 2);
	ASSERT_TRUE(a.Column() == 3);

	cout << a << endl;

	FixedArray2D1st<int, 2, 3> b(3);
	Print<int, 2, 3>(b);

	int ra[2][3] = { {1,2,3}, {1,2,3} };
	Print(ra);
}

TEST(FixedArray2D, Construct1) {

	FixedArray2D<int, 2,3> a;

	a[0][0] = 1;
	a[1][2] = 6;

	cout << a << endl;

	ASSERT_TRUE(a.Length() == 6);
	ASSERT_TRUE(a.Row() == 2);
	ASSERT_TRUE(a.Column() == 3);
	ASSERT_TRUE(a[0][0] == 1);
	ASSERT_TRUE(a[1][2] == 6);

	a = FixedArray2D<int, 2, 3>(3);
	cout << a << endl;
}