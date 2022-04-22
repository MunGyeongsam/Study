
#include "pch.h";
#include "Number.h"

namespace {
	typedef ka::Number Number;
	using namespace std;
}

TEST(Number, Construct1) {
	Number a;

	cout << a << endl;

	cout << "input number : ";
	cin >> a;
	cout << a << endl;

	Number b(a);
	cout << b << endl;
}
