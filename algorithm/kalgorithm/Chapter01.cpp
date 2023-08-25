#include "pch.h"
#include "Chapter01.h"

#include <iostream>
#include <algorithm>
#include <cmath>

namespace
{
	using namespace std;


	int Prob01_3n_1_Length_Recv(int n, int length = 1)
	{
		if (n == 1)
			return length;

		return Prob01_3n_1_Length_Recv((n & 0x1) ? (3 * n + 1) : (n >> 1), length + 1);
	}


	int Prob01_3n_1_Length(int n);
	void Prob01_3n_1_Impl(int from, int to);

	void Prob01_3n_1()
	{
		////Prob01_3n_1_Length(22);
		//Prob01_3n_1_Impl(1, 10);
		//Prob01_3n_1_Impl(100, 200);
		//Prob01_3n_1_Impl(201, 210);
		//Prob01_3n_1_Impl(900, 1000);

		//cout << Prob01_3n_1_Length_Recv(22) << endl;
	}

	void Prob01_3n_1_Impl(int from, int to)
	{
		int maxLength = 0;

		for (int i = from; i <= to; ++i)
		{
			maxLength = std::max(maxLength, Prob01_3n_1_Length(i));
			//maxLength = std::max(maxLength, Prob01_3n_1_Length_Recv(i));
		}

		cout << from << " " << to << " " << maxLength << endl;
	}

	int Prob01_3n_1_Length(int n)
	{
		int length = 1;

		//cout << "n : " << n << " ";

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

			//cout << n << " ";
		}

		//cout << endl;
		//cout << "length : " << length << endl;
		return length;
	}
}
void Chapter01::Test()
{
	Prob01_3n_1();
}