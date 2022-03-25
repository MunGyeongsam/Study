
#include "pch.h";
#include "IntegerArray.h"

namespace {
	typedef ka::IntegerArray IntegerArray;
	using namespace std;
}

TEST(IntegerArray, Construct1) {
	IntegerArray a;

	EXPECT_TRUE(a.Size() == 1);
	EXPECT_TRUE(a.Lower() == 0);
	EXPECT_TRUE(a.Uppper() == 0);
	EXPECT_EQ(a[0], 0);
	cout << a << endl;
}

TEST(IntegerArray, Construct2)
{
	//IntegerArray a(3, 8); //assert
	IntegerArray a(8, 3);
	ASSERT_EQ(a.Size(), 6);
	cout << a << endl;
}

TEST(IntegerArray, Construct3)
{
	int rowArray[] = { 3,4,5 };
	IntegerArray a(rowArray, _countof(rowArray));

	ASSERT_EQ(a[0], 3);
	ASSERT_EQ(a[2], 5);
	cout << a << endl;
}

TEST(IntegerArray, Construct4)
{
	IntegerArray a(5, 1);
	for (int i = 1; i <= 5; ++i)
		a[i] = i * i;
	IntegerArray b(a);

	for (int i = a.Lower(); i <= a.Uppper(); ++i)
		ASSERT_EQ(a[i], b[i]);

	cout << a << endl;
	cout << b << endl;
}

TEST(IntegerArray, Construct5)
{
	int rowArray[] = { 3,4,5 };
	IntegerArray a(rowArray);

	ASSERT_EQ(a[0], 3);
	ASSERT_EQ(a[2], 5);
	cout << a << endl;
}

TEST(IntegerArray, Assignment)
{
	int rowArray[] = { 3,4,5 };
	IntegerArray a(rowArray);
	IntegerArray b;

	ASSERT_EQ(b[0], 0);
	b = a;

	ASSERT_EQ(a.Size(), b.Size());
	for (int i = a.Lower(); i <= a.Uppper(); ++i)
		ASSERT_EQ(a[i], b[i]);
}

TEST(IntegerArray, SetBounds)
{
	int rowArray[] = { 0,1,2,3,4,5,6,7,8 };
	IntegerArray a(rowArray);

	ASSERT_EQ(a[0], 0);
	ASSERT_EQ(a[1], 1);
	ASSERT_EQ(a[2], 2);
	ASSERT_EQ(a.Lower(), 0);
	ASSERT_EQ(a.Uppper(), 8);
	ASSERT_EQ(a.Size(), 9);

	IntegerArray b(a);
	b.SetBounds(6, 1);
	cout << b << endl;

	b.SetBounds(8, 0);
	cout << b << endl;

	b = a;
	b.SetBounds(10, 3);
	cout << b << endl;
	b.SetBounds(7, 5);
	cout << b << endl;
}