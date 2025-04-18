/*

#include <iostream>
#include <string>

using namespace std;

// #1. max ����

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

	// ���� �Լ� ����, ��� �Լ��� ������ �ʹ�?
	//auto rt2 = mymax(s1, s2, &std::string::size);
	//cout << rt1 << endl;

	// projection �� ���������ϸ� ���ڴ�.
	// auto rt3 = mymax(s1, s2);


	return 0;
}

//*/