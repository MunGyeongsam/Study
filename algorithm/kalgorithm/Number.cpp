#include "pch.h"
#include "Number.h"

namespace ka
{
	std::istream& operator >> (std::istream& s, Number& n)
	{
		const int LEN = 80;
		char input[LEN] = { 0, };

		s.getline(input, LEN);

		bool isMinus = input[0] == '-';

		int i = 0;
		while (input[i++])
			;

		IntegerArray& digit = n.m_digit;
		const int SIZE = isMinus ? (i - 2) : (i - 1);
		const int OFFSET = isMinus ? 1 : 0;

		n.m_currSize = SIZE;
		digit.SetBounds(SIZE);

		for (int i = isMinus ? 1 : 0; i < SIZE + OFFSET; ++i)
		{
			digit[SIZE - i + OFFSET] = int(input[i] - '0');
		}
		digit[0] = isMinus ? -1 : 1;

		return s;
	}

	std::ostream& operator << (std::ostream& s, const Number& n)
	{
		if (n.IsMinus())
			s << '-';
		for (int i = n.Length(); i > 0; --i)
			s << n.m_digit[i];

		return s;
	}

	Number::Number() : m_digit(1), m_currSize(1)
	{
		m_digit[0] = 1;
	}

	Number::Number(long i) : m_digit(20), m_currSize(0)
	{
		if (i < 0)
		{
			m_digit[0] = -1;
			i = -i;
		}
		else
			m_digit[0] = 1;

		do
		{
			m_digit[++m_currSize] = i % 10;
			i /= 10;
		} while (i > 0);
	}

	Number::Number(const Number& rhs)
		: m_digit(rhs.m_digit)
		, m_currSize(rhs.m_currSize)
	{
	}

	void AddPos(const Number& a, const Number& b)
	{
	}
}