using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _01_Geo1
    {
        public static void Run()
        {
            Util.Call(_1291);
            Util.Call(_1341);
            Util.Call(_1303);
            Util.Call(_1856);
            Util.Call(_1304);
            Util.Call(_5931);
            Util.Call(_5932);
            Util.Call(_5933);
            Util.Call(_1307);
            Util.Call(_1314);
            Util.Call(_1338);
            Util.Call(_1339);
        }

        //--------------------------------------------------
        // 1291 구구단
        //--------------------------------------------------
        static void Impl_1291(int s, int e)
        {
            int step = (s < e) ? 1 : -1;

            for (int i = 1; i <= 9; ++i)
            {

                for (int j = s; ; j += step)
                {

                    Console.Write("{0} x {1} = {2,2}   ", j, i, j * i);

                    if (j == e)
                        break;
                }
                Console.WriteLine();
            }
        }
        static void _1291()
        {
            Impl_1291(4, 8);
        }

        //--------------------------------------------------
        // 1341 구구단2
        //--------------------------------------------------
        static void Impl_1341(int s, int e)
        {
            int step = (s < e) ? 1 : -1;
            e += step;

            for (int i = s; i != e; i += step)
            {

                for (int j = 1; j <= 9; j += 3)
                {
                    Console.Write("{0} x {1} = {2,2}", i, j + 0, i * (j + 0));
                    Console.Write("   {0} x {1} = {2,2}", i, j + 1, i * (j + 1));
                    Console.Write("   {0} x {1} = {2,2}", i, j + 2, i * (j + 2));
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        static void _1341()
        {
            Impl_1341(4, 3);
        }

        //--------------------------------------------------
        // 1303 숫자사각형1
        //--------------------------------------------------
        static void Impl_1303(int n, int m)
        {
            int num = 1;
            for (int row = 0; row < n; ++row)
            {
                for (int column = 0; column < m - 1; ++column)
                {
                    Console.Write(num++);
                    Console.Write(' ');
                }
                Console.Write(num++);
                Console.WriteLine();
            }
        }
        static void _1303()
        {
            Impl_1303(4, 5);
        }

        //--------------------------------------------------
        // 1856 숫자사각형2
        //--------------------------------------------------
        static void Impl_1856_toRight(int num, int count)
        {
            for (int i = 0; i < count - 1; ++i)
            {
                Console.Write(num++);
                Console.Write(' ');
            }
            Console.Write(num++);
            Console.WriteLine();
        }
        static void Impl_1856_toLeft(int num, int count)
        {
            num = num + (count - 1);
            for (int i = 0; i < count - 1; ++i)
            {
                Console.Write(num--);
                Console.Write(' ');
            }
            Console.Write(num--);
            Console.WriteLine();
        }
        static void Impl_1856(int n, int m)
        {
            int num = 1;

            bool toRight = true;
            for (int row = 0; row < n; ++row)
            {
                if (toRight)
                {
                    Impl_1856_toRight(num, m);
                }
                else
                {
                    Impl_1856_toLeft(num, m);
                }

                toRight = !toRight;
                num += m;
            }
        }
        static void _1856()
        {
            Impl_1856(4, 5);
        }

        //--------------------------------------------------
        // 1304 숫자사각형3
        //--------------------------------------------------
        static void Impl_1304(int n)
        {
            int num = 1;

            for (int i = 0; i < n; ++i)
            {

                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num + j * n);
                    Console.Write(' ');
                }
                Console.Write(num + n * (n - 1));
                Console.WriteLine();

                num++;
            }
        }
        static void _1304()
        {
            Impl_1304(4);
        }

        //--------------------------------------------------
        // 2046 숫자사각형4
        //--------------------------------------------------
        static void Impl_2046_Tri1(int n)
        {
            for (int i = 0; i < n; ++i)
            {

                int num = i + 1;
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num);
                    Console.Write(' ');
                }
                Console.WriteLine(num);
            }
        }
        static void Impl_2046_Tri2(int n)
        {
            bool toLeft = true;
            for (int i = 0; i < n; ++i)
            {

                int num = toLeft ? 1 : n;
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num);
                    Console.Write(' ');
                    if (toLeft)
                        ++num;
                    else
                        --num;
                }
                Console.WriteLine(num);

                toLeft = !toLeft;
            }
        }
        static void Impl_2046_Tri3(int n)
        {
            for (int i = 0; i < n; ++i)
            {

                int num = i + 1;
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num * (j + 1));
                    Console.Write(' ');
                }
                Console.WriteLine(num * n);
            }
        }
        static void Impl_2046(int n, int type)
        {
            if (1 == type)
                Impl_2046_Tri1(n);
            else if (2 == type)
                Impl_2046_Tri2(n);
            else if (3 == type)
                Impl_2046_Tri3(n);
        }
        static void _2046()
        {
            Impl_2046(5, 1);
            Console.WriteLine();
            Impl_2046(5, 2);
            Console.WriteLine();
            Impl_2046(5, 3);
        }


        //--------------------------------------------------
        // 5931 숫자사각형4-1
        //--------------------------------------------------
        static void _5931()
        {
            Impl_5931_Quadrangle1(5);
        }
        static void Impl_5931_Quadrangle1(int n)
        {
            for (int i = 0; i < n; ++i)
            {

                int num = i + 1;
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num);
                    Console.Write(' ');
                }
                Console.WriteLine(num);
            }
        }


        //--------------------------------------------------
        // 5932 숫자사각형4-2
        //--------------------------------------------------
        static void _5932()
        {
            Impl_5932_Quadrangle2(5);
        }
        static void Impl_5932_Quadrangle2(int n)
        {
            bool toLeft = true;
            for (int i = 0; i < n; ++i)
            {

                int num = toLeft ? 1 : n;
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num);
                    Console.Write(' ');
                    if (toLeft)
                        ++num;
                    else
                        --num;
                }
                Console.WriteLine(num);

                toLeft = !toLeft;
            }
        }

        //--------------------------------------------------
        // 5933 숫자사각형4-3
        //--------------------------------------------------
        static void _5933()
        {
            Impl_5933_Quadrangle3(5);
        }
        static void Impl_5933_Quadrangle3(int n)
        {
            for (int i = 0; i < n; ++i)
            {

                int num = i + 1;
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write(num * (j + 1));
                    Console.Write(' ');
                }
                Console.WriteLine(num * n);
            }
        }


        //--------------------------------------------------
        // 1307 문자사각형1
        //--------------------------------------------------
        static void Impl_1307(int n)
        {
            char ch = 'A';
            char[,] tbl = new char[n, n];
            for (int i = n - 1; i >= 0; --i)
            {

                for (int j = n - 1; j >= 0; --j)
                {
                    tbl[j, i] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
            }

            for (int i = 0; i < n; ++i)
            {

                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write("{0} ", tbl[i, j]);
                }
                Console.WriteLine(tbl[i, n - 1]);
            }
        }
        static void _1307()
        {
            Impl_1307(35);
        }


        //--------------------------------------------------
        // 1314 문자사각형2
        //--------------------------------------------------
        static void Impl_1314(int n)
        {

            char ch = 'A';
            char[] steps = { (char)(n * 2 - 1), (char)1 };

            for (int i = 0; i < n; ++i)
            {

                steps[0] = (char)((n - i) * 2 - 1);
                steps[1] = (char)(1 + 2 * i);

                char chtmp = ch;
                for (int j = 0; j < n; ++j)
                {

                    Console.Write("{0} ", chtmp);

                    int stepIndex = j % 2;
                    char step = steps[stepIndex];

                    chtmp += step;
                    while (chtmp > 'Z')
                        chtmp -= (char)('Z' - 'A' + 1); // 26
                }
                Console.WriteLine();

                ++ch;
                if (ch > 'Z')
                    ch = 'A';
            }
        }
        static void _1314()
        {
            Impl_1314(44);
        }

        //--------------------------------------------------
        // 1338 문자삼각형1
        //--------------------------------------------------
        static void Impl_1338(int n)
        {
            char[,] arr = new char[n, n];
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                    arr[i, j] = ' ';

            int cnt = n;
            char ch = 'A';
            while (cnt > 0)
            {

                for (int i = n - cnt, j = n - 1; i < n; ++i, --j)
                {
                    arr[i, j] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                --cnt;
            }

            Util.PrintArray(arr);
        }
        static void _1338()
        {
            Impl_1338(5);
        }

        //--------------------------------------------------
        // 1339 문자삼각형2
        //--------------------------------------------------
        static void Impl_1339(int n)
        {
            if (n % 2 == 0)
                return; // only odd number

            //count column
            int COL = n / 2 + 1;
            char[,] arr = new char[n, COL];

            int count = 0;
            for (int i = 0; i < COL; ++i)
                count += (n - i * 2);

            int rem = count % ('Z' - 'A' + 1);
            char lastC = (char)('A' + rem - 1);

            for (int i = 0; i < COL; ++i)
            {

                int cnt = n - i * 2;
                int startRow = n - i - 1;
                for (int j = 0; j < cnt; ++j)
                {

                    int row = startRow - j;
                    arr[row, i] = lastC--;
                    if (lastC < 'A')
                        lastC = 'Z';
                }
            }

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < COL; ++j)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void _1339()
        {
            Impl_1339(5);
        }

    }
}
