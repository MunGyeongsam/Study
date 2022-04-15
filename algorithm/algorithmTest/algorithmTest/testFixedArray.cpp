#include "pch.h"
#include "FixedArray.h"

namespace {
	using ka::FixedArray;
	using namespace std;

	template<size_t N>
	struct Gen {
		static int GenNum() {
			return FixedArray<int, N>::CNT + FixedArray<int, N-1>::CNT;
			//FixedArray<int, N>::CNT;
			//return Gen<N - 1>::GenNum();
		}
	};
	template<>
	struct Gen<1> {
		static int GenNum() {
			return 1;
		}
	};
}

TEST(FixedArray, Construct1) {
	FixedArray<int, 10> a;

	cout << a << endl;

	for (int i = 0; i < (int)a.Size(); ++i)
		a[i] = i;

	ASSERT_TRUE(a.Size() == 10);
	ASSERT_TRUE(a[0] == 0);

	cout << a << endl;

	FixedArray<char, 5> b('*');
	cout << b << endl;
	//b = 'a';
	//cout << b << endl;

	cout << Gen<9999991>::GenNum() << endl;
}

TEST(FixedArray, Construct2) {

	int arr[] = { 3,4,5,6 };

	FixedArray<int, 4> a(arr);
	FixedArray<int, 4> b(a);

	ASSERT_TRUE(a.Size() == 4);
	ASSERT_TRUE(a[0] == 3);

	cout << a << endl;
	cout << b << endl;
}

