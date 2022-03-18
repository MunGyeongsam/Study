#pragma once

#include <iostream>

namespace ka
{
	class IntegerArray
	{
		typedef std::ostream ostream;
		friend ostream operator<< (ostream& os, const IntegerArray& a);

	public:
		IntegerArray();
		IntegerArray(int up, int low = 0);
		IntegerArray(int a[], int size);
		IntegerArray(const IntegerArray& a);

		~IntegerArray();

		void SetBounds(int up, int low = 0);
		IntegerArray& operator= (const IntegerArray& a);
		int& operator[] (int i) const;

	private:
		int m_lower, m_upper;
		int* m_data;
	};
}

