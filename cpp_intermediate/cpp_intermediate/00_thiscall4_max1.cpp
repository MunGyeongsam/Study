/*

#include <iostream>
#include <string>

using namespace std;

// #1. max ����

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

	// ������ �񱳰� �ƴ� ���ڼ��� ���ϰ� ����
	auto rt2 = mymax(s1.size(), s2.size());
	cout << rt2 << endl;

	//// ���� �� ���� ���̴� ��?
	//auto rt3 = mymax(s1, s2, [](auto& a, auto& b) {return a.size() < a.size(); });
	//						// #1. ���� ������ ��� - c++98 style	
	//auto rt4 = mymax(s1, s2, [](auto& a) {return a.size(); });
	//						// #2. ���� ������ ���. - python style


	return 0;
}

//*/