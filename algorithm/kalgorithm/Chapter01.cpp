#include "pch.h"
#include "Chapter01.h"

#include <iostream>
#include <algorithm>
#include <cmath>

namespace
{
	using namespace std;

	static void Prob01_3n_1()
	{

	}

	static void Prob01_3n_1_Impl(int from, int to)
	{
		int maxLength = 0;

		for (int i = from; i <= to; ++i)
		{
			maxLength = std::max(maxLength, Prob01_3n_1_Length(i));
		}
	}

	static int Prob01_3n_1_Length(int n)
	{
		int length = 1;

		cout << "n : " << n << " ";

		while (n != 1)
		{
			if (n & 0x1) // odd
			{
				n = n * 3 + 1;
			}
			else
			{
				n >>= 1;
			}
			length++;

			cout << n << " ";
		}

		return length;
	}
}
void Chapter01::Test()
{

}