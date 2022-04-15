#pragma once

#include <iostream>
#include <cassert>
#include "FixedArray.h"

namespace ka
{
	template<class T, int R, int C>
	class FixedArray2D1st
	{
		friend std::ostream& operator<< (std::ostream& os, FixedArray2D1st const& a)
		{
			os << "Length : " << a.Length() << std::endl;
			os << "Row : " << R << std::endl;
			os << "Column : " << C << std::endl;
			for (int r = 0, i = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					os << "[" << r << "][" << c << "] : " << a(r,c) << std::endl;
			}
			return os;
		}

	public:
		typedef T(&arr_t)[R][C];
		typedef const T(&carr_t)[R][C];
		FixedArray2D1st() {
			//do nothing!
		}
		FixedArray2D1st(T const& v)
		{
			for (int r = 0, i = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					m_data[r][c] = v;
			}
		}

		explicit FixedArray2D1st(const T(&arr)[R*C])
		{
			for (int r = 0, i = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					m_data[r][c] = arr[i++];
			}
		}

		explicit FixedArray2D1st(const T(&arr)[R][C])
		{
			for (int r = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					m_data[r][c] = arr[r][c];
			}
		}
		FixedArray2D1st(FixedArray2D1st const& rhs)
		{
			new(this)FixedArray2D1st(rhs.m_data);
		}

		FixedArray2D1st& operator= (FixedArray2D1st const& rhs)
		{
			new(this)FixedArray2D1st(rhs.m_data);
			return *this;
		}

		T& operator() (int i, int j)
		{

			assert(i >= 0);
			assert(i < R);
			assert(j >= 0);
			assert(j < C);
			return m_data[i][j];
		}
		const T& operator() (int i, int j) const
		{

			assert(i >= 0);
			assert(i < R);
			assert(j >= 0);
			assert(j < C);
			return m_data[i][j];
		}

		const size_t Length()const { return R * C; }
		const size_t Row()const { return R; }
		const size_t Column()const { return C; }

		operator arr_t() { return m_data; }
		operator carr_t()const { return m_data; }

	private:
		T m_data[R][C];
	};

	template<class T, int R, int C>
	class FixedArray2D
	{
		friend std::ostream& operator<< (std::ostream& os, FixedArray2D const& a)
		{
			os << "Length : " << a.Length() << std::endl;
			os << "Row : " << R << std::endl;
			os << "Column : " << C << std::endl;
			for (int r = 0, i = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					os << "[" << r << "][" << c << "] : " << a[r][c] << std::endl;
			}
			return os;
		}

	public:
		FixedArray2D()
		{
			//do nothing
		}
		explicit FixedArray2D(const T& v)
		{
			for (int r = 0, i = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					m_data[r][c] = v;
			}
		}

		template<int N>
		explicit FixedArray2D(const T(&arr)[N])
		{
			static_assert(N == R * C, "invalid");
			for (int r = 0, i = 0; r < R; ++r)
			{
				for (int c = 0; c < C; ++c)
					m_data[r][c] = arr[i++];
			}
		}
		
		explicit FixedArray2D(const T(&arr)[R][C])
		{
			for (int r = 0; r < R; ++r)
			{
				m_data[r] = rhs.m_data[r];
				//for (int c = 0; c < C; ++c)
				//	m_data[r][c] = arr[r][c];
			}
		}

		FixedArray2D(FixedArray2D const& rhs)
		{
			new(this)FixedArray2D(rhs.m_data);
		}

		FixedArray<T, C>& operator[] (int i)
		{
			assert(i >= 0);
			assert(i < R);
			return m_data[i];
		}

		const FixedArray<T, C>& operator[] (int i) const
		{
			assert(i >= 0);
			assert(i < R);
			return m_data[i];
		}

		FixedArray2D& operator= (FixedArray2D const& rhs)
		{
			for (int r = 0; r < R; ++r)
			{
				//m_data[r] = rhs.m_data[r];
				for (int c = 0; c < C; ++c)
					m_data[r][c] = rhs.m_data[r][c];
			}

			return *this;
		}

		const size_t Length()const { return R * C; }
		const size_t Row()const { return R; }
		const size_t Column()const { return C; }

	private:
		FixedArray<T,C> m_data[R];
	};
}

