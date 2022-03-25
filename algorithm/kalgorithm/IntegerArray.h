#pragma once

#include <iostream>

namespace ka
{
	// lower <= i <= upper
	class IntegerArray
	{
		typedef std::ostream ostream;
		friend ostream& operator<< (ostream& os, const IntegerArray& a);

	public:
		IntegerArray();
		IntegerArray(int up, int low = 0);
		IntegerArray(const int a[], int size);
		IntegerArray(const IntegerArray& a);
		template<int N> IntegerArray(int const (&a)[N]) : IntegerArray(a, N){}

		~IntegerArray();

		void SetBounds(int up, int low = 0);
		IntegerArray& operator= (const IntegerArray& a);
		int& operator[] (int i);
		const int& operator[] (int i) const;

		unsigned int Size() const;
		int Lower() const;
		int Uppper() const;

	private:
		int m_lower, m_upper;
		int* m_data;
	};
}

