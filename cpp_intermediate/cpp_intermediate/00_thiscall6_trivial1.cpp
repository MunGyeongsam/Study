//*

#include <iostream>
#include <string>
#include <functional>

using namespace std;

struct identity
{
	template<typename T>
	constexpr T&& operator() (T&& obj) const noexcept
	{
		return std::forward<T>(obj);
	}
};

// identity f;
// f(10) -> 10
// f(n) -> n


// 위에 만든 identity 가 c++20 부터 표준에 도입.
template<typename T, typename Projection = std::identity>
const T& mymax(const T& t1, const T& t2, Projection proj = {})
{
	return invoke(proj, t1) < invoke(proj, t2) ? t2 : t1;
}

int main()
{
	string s1 = "abcd";
	string s2 = "xyz";

	std::identity f;
	auto& r = f(s1);	//s1

	cout << &s1 << endl;
	cout << &r << endl;

	auto rt1 = mymax(s1, s2);
	cout << rt1 << endl;

	auto rt2 = mymax(s1, s2, &std::string::size);
	cout << rt2 << endl;

	return 0;
}

//*/