#pragma once
#include <iostream>
#include "IntegerArray.h"

namespace ka
{

	class Number
	{
		friend std::istream& operator >>(std::istream& s, Number& n);
		friend std::ostream& operator <<(std::ostream& s, const Number& n);

		friend Number operator+ (const Number& n, const Number& m);
		friend Number operator- (const Number& n, const Number& m);
		friend Number operator* (const Number& n, const Number& m);
		friend Number operator- (const Number& n);

	public:
		//Number operator+ (const Number& n);

		Number();
		explicit Number(long i);
		Number(const Number& rhs);

		int Length() const { return m_currSize; }

	private:
		int m_currSize;
		IntegerArray m_digit;

		int IsZero() const { return m_digit[m_currSize] == 0; }
		bool IsMinus() const { return m_digit[0] < 0; }

		void AddPos(const Number& n, const Number& m);
		void SubPos(const Number& n, const Number& m);
	};
}

