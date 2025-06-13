/*

#include <iostream>
#include <string>
#include <functional>
#include <ranges>

// #1. C++20 std::ranges::max

int main()
{
	std::string s1 = "abcd";
	std::string s2 = "xyz";

	// g++ source.cpp -std=c++20 으로 빌드하면 됩니다.
	auto ret1 = std::ranges::max(s1, s2);
	auto ret2 = std::ranges::max(s1, s2, std::greater{});
	auto ret3 = std::ranges::max(s1, s2, {}, &std::string::size);
	auto ret4 = std::ranges::max(s1, s2, std::greater{}, &std::string::size);

	std::cout << ret1 << std::endl;
	std::cout << ret2 << std::endl;
	std::cout << ret3 << std::endl;
	std::cout << ret4 << std::endl;
}

//*/