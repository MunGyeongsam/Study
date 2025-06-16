/*
#include <iostream>
#include <string>
using namespace std;
// #1. max 도입
template<typename T, typename Projection>
const T& mymax(const T& t1, const T& t2, Projection proj)
{
	return proj(t1) < proj(t2) ? t2 : t1;
	//return invoke(proj, t1) < invoke(proj, t2) ? t2 : t1;
}
int main()
{
	string s1 = "abcd";
	string s2 = "xyz";
	auto rt1 = mymax(s1, s2, [](auto& a) { return a.size(); });
	cout << rt1 << endl;
	// 람다 함수 말고, 멤버 함수를 보내고 싶다?
	//auto rt2 = mymax(s1, s2, &std::string::size);
	//cout << rt1 << endl;
	// projection 도 생각가능하면 좋겠다.
	// auto rt3 = mymax(s1, s2);
	return 0;
}
//*/