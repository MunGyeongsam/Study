/*

#include <iostream>
#include <string>

using namespace std;

// #2. binary compare

//template<typename T, typename Compare = std::less<void> >
//const T& mymax2(const T& t1, const T& t2, Compare cmp = {})
//{
//	//return t1 < t2 ? t2 : t1;
//	//return cmp(t1, t2) ? t2 : t1;
//	return std::invoke(cmp, t1, t2) ? t2 : t1;
//}

template<typename T, typename Compare>
const T& mymax(const T& t1, const T& t2, Compare cmp)
{
	//return t1 < t2 ? t2 : t1;
	//return cmp(t1, t2) ? t2 : t1;
	return std::invoke(cmp, t1, t2) ? t2 : t1;
}

int main()
{
	string s1 = "abcd";
	string s2 = "xyz";

	auto rt1 = mymax(s1, s2, [](auto& l, auto& r) { return l.size() < r.size(); });
	cout << rt1 << endl;

	//auto rt2 = mymax2(s1, s2);
	//cout << rt2 << endl;
	//
	//auto rt3 = mymax2(s1, s2, [](auto& l, auto& r) { return l.size() < r.size(); });
	//cout << rt3 << endl;


	return 0;
}

//*/