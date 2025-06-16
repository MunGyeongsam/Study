//*
#include <iostream>
#include <type_traits>
class MyArray
{
	int buff[5] = { 1,2,3,4,5 };
public:
	using iterator = int*;
	using const_iterator = const int*;
	template<typename T>
	std::conditional_t<std::is_const_v<T>, const_iterator, iterator>  // auto
		begin(this T& self)
	{
		return self.buff;
	}
};
int main()
{
	MyArray arr1;		// container
	const MyArray arr2;	// const container
	auto it1 = arr1.begin();
	auto it2 = arr2.begin();
}
//*/