#include "pch.h";
#include "FixedArray2D.h"

namespace {
	using ka::FixedArray2D;
	using namespace std;
}

TEST(FixedArray2D, Construct1) {

	FixedArray2D<int, 2,3> a;

	a[0][0] = 1;
	a[1][2] = 6;

	cout << a << endl;

	ASSERT_TRUE(a.Count() == 6);
	ASSERT_TRUE(a.Row() == 2);
	ASSERT_TRUE(a.Column() == 3);
	ASSERT_TRUE(a[0][0] == 1);
	ASSERT_TRUE(a[1][2] == 6);

	a = FixedArray2D<int, 2, 3>(3);
	cout << a << endl;
}