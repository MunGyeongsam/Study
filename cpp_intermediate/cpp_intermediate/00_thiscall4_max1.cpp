/*
#include <iostream>
#include <string>
using namespace std;
// #1. max 도입
template<typename T>
const T& mymax(const T& t1, const T& t2)
{
	return t1 < t2 ? t2 : t1;
}
int main()
{
	string s1 = "abcd";
	string s2 = "xyz";
	auto rt1 = mymax(s1, s2);
	cout << rt1 << endl;
	// 사전순 비교가 아닌 글자수로 비교하고 싶음
	auto rt2 = mymax(s1.size(), s2.size());
	cout << rt2 << endl;
	//// 다음 중 좋아 보이는 것?
	//auto rt3 = mymax(s1, s2, [](auto& a, auto& b) {return a.size() < a.size(); });
	//						// #1. 이항 조건자 사용 - c++98 style	
	//auto rt4 = mymax(s1, s2, [](auto& a) {return a.size(); });
	//						// #2. 단항 조건자 사용. - python style
	return 0;
}
//*/