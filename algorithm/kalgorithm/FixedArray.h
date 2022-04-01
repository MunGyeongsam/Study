#pragma once

#include <iostream>
#include <cassert>

namespace ka
{
	template<class T, size_t N>
	class FixedArray
	{
		friend std::ostream& operator<< (std::ostream& os, FixedArray const& a)
		{
			os << "size : " << a.Size() << std::endl;
			for (int i = 0; i < N; ++i)
				os << "[" << i << "] : " << a[i] << std::endl;
			return os;
		}

	public:
		enum {CNT = N};
		FixedArray()
		{
		}
		
		explicit FixedArray(const T& v)
		{
			for (size_t i = 0; i < N; ++i)
				m_data[i] = v;
		}

		explicit FixedArray(const T(&arr)[N])
		{
			for (size_t i = 0; i < N; ++i)
				m_data[i] = arr[i];
		}

		FixedArray(FixedArray const& rhs)
		{
			new(this)FixedArray(rhs.m_data);
		}

		T& operator[] (int i)
		{
			assert(i >= 0);
			assert(i < N);
			return m_data[i];
		}

		const T& operator[] (int i) const
		{
			assert(i >= 0);
			assert(i < N);
			return m_data[i];
		}

		FixedArray& operator= (FixedArray const& rhs)
		{
			new(this)FixedArray(rhs.m_data);
			return *this;
		}

		const size_t Size()const { return N; }

	private:
		T m_data[N];
	};
}

