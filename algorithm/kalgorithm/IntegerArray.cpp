#include "pch.h"
#include "IntegerArray.h"
#include <cassert>

#pragma once

namespace ka
{
	IntegerArray::IntegerArray()
		: m_lower(0)
		, m_upper(0)
		, m_data(new int[1])
	{
	}
	IntegerArray::IntegerArray(int up, int low)
		: m_lower(low)
		, m_upper(up)
		, m_data(new int[up-low+1])
	{
		memset(m_data, 0, (up - low + 1));
	}
	IntegerArray::IntegerArray(int a[], int size)
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
		int oldSize = m_upper - m_lower + 1;

		int* newData = new int[newSize];

		int cnt = (newSize > oldSize) ? oldSize : newSize;
		std::copy(m_data, m_data + cnt, newData);

		if (newSize > oldSize)
		{
			memset(newData + oldSize, 0, newSize - oldSize);
		}

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
	int& IntegerArray::operator[] (int i) const
	{
		assert(i >= m_lower);
		assert(i <= m_upper);
		return m_data[i - m_lower];
	}
}

