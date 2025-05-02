/*

#include <iostream>
#include <string>
#include <functional>

// #6. binary compare & projection 결합
template<typename T, typename Comp = std::less<void>, typename Proj = std::identity>
const T& mymax(const T& obj1, const T& obj2, Comp comp = {}, Proj proj = {})
{
	return std::invoke(comp, std::invoke(proj, obj1), std::invoke(proj, obj2)) ? obj2 : obj1;
}

int main()
{
	std::string s1 = "abcd";
	std::string s2 = "xyz";

	// #. 사용법 정리
	auto ret1 = mymax(s1, s2);
	auto ret2 = mymax(s1, s2, std::greater{});
	auto ret3 = mymax(s1, s2, {}, &std::string::size);
	auto ret4 = mymax(s1, s2, std::greater{}, &std::string::size);
	//      > 
	std::cout << ret1 << std::endl;
	std::cout << ret2 << std::endl;
	std::cout << ret3 << std::endl;
	std::cout << ret4 << std::endl;
}

//*/