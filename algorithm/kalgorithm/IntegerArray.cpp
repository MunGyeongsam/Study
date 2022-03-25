#include "pch.h"
#include "IntegerArray.h"
#include <cassert>

#pragma once

namespace ka
{
	//IntegerArray::IntegerArray()
	//	: m_lower(0)
	//	, m_upper(0)
	//	, m_data(new int[1])
	//{
	//	m_data[0] = 0;
	//}
	IntegerArray::IntegerArray()
		: IntegerArray(0, 0)
	{
	}

	IntegerArray::IntegerArray(int up, int low/*=0*/)
		: m_lower(low)
		, m_upper(up)
		, m_data(nullptr)
	{
		assert(up >= low);
		m_data = new int[up - low + 1];
		memset(m_data, 0, sizeof(int)*Size());
	}
	IntegerArray::IntegerArray(const int a[], int size)
		: m_lower(0)
		, m_upper(size-1)
		, m_data(new int[size])
	{
		for (int i = 0; i < size; ++i)
			m_data[i] = a[i];

		//memcpy(m_data, a, sizeof(int) * size);
		//memcpy_s(m_data, size * sizeof(int), a, size);
		//std::copy(a, a + size, m_data);
	}
	IntegerArray::IntegerArray(const IntegerArray& a)
		: m_lower(a.m_lower)
		, m_upper(a.m_upper)
		, m_data(nullptr)
	{
		int size = a.m_upper - a.m_lower + 1;
		m_data = new int[size];

		std::copy(a.m_data, a.m_data + size, m_data);
	}
	IntegerArray::~IntegerArray()
	{
		delete[] m_data;
		m_data = nullptr;
	}

	void IntegerArray::SetBounds(int up, int low /*= 0*/)
	{
		assert(up >= low);

		int newSize = up - low + 1;
		int* newData = new int[newSize];

		int lowMax = std::max(low, m_lower);
		int upMin = std::min(up, m_upper);

		for (int i = low; i < lowMax; ++i)
			newData[i - low] = 0;
		for (int i = lowMax; i <= upMin; ++i)
			newData[i - low] = m_data[i - m_lower];
		for (int i = up; i > upMin; --i)
			newData[i - low] = 0;

		delete[] m_data;

		m_lower = low;
		m_upper = up;
		m_data = newData;
	}
	IntegerArray& IntegerArray::operator= (const IntegerArray& a)
	{
		delete[] m_data;

		new(this) IntegerArray(a);

		//int size = a.m_upper - a.m_lower + 1;
		//m_data = new int[size];
		//m_lower = a.m_lower;
		//m_upper = a.m_upper;
		//
		//std::copy(a.m_data, a.m_data + size, m_data);

		return *this;
	}
	const int& IntegerArray::operator[] (int i) const
	{
		assert(i >= m_lower);
		assert(i <= m_upper);
		return m_data[i - m_lower];
	}
	int& IntegerArray::operator[] (int i)
	{
		assert(i >= m_lower);
		assert(i <= m_upper);
		return m_data[i - m_lower];
	}

	unsigned int IntegerArray::Size() const
	{
		return static_cast<unsigned int>(m_upper - m_lower) + 1U;
	}
	int IntegerArray::Lower() const
	{
		return m_lower;
	}
	int IntegerArray::Uppper() const
	{
		return m_upper;
	}
	std::ostream& operator<<(std::ostream& os, const IntegerArray& a)
	{
		os << "size : " << a.Size() << "(" << a.Lower() << " ~ " << a.Uppper() << ")" << std::endl;
		for (int i = a.Lower(); i <= a.Uppper(); ++i)
			os << "[" << i << "] : " << a[i] << std::endl;

		return os;
	}
}

