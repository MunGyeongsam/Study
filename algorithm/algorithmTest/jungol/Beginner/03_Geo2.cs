using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _03_Geo2
    {
        public static void Run()
        {
            Util.Call(_1523);
            Util.Call(_5934);
            Util.Call(_1329);
            Util.Call(_5945);
            Util.Call(_5946);
            Util.Call(_5947);
            Util.Call(_1707);
            Util.Call(_5397);
            Util.Call(_1337);
            //Util.Call(_5398);
            //Util.Call(_2071);
            //Util.Call(_1331);
            //Util.Call(_1495);
            //Util.Call(_2074);
        }

        //--------------------------------------------------
        // 1523 별삼각형1
        //--------------------------------------------------
        static void Impl_1523_Line(int n)
        {
            for (int i = 0; i < n; ++i)
                Console.Write('*');
            Console.WriteLine();
        }
        static void Impl_1523_Tri1(int n)
        {
            for (int i = 1; i <= n; ++i)
                Impl_1523_Line(i);
        }
        static void Impl_1523_Tri2(int n)
        {
            for (int i = n; i > 0; --i)
                Impl_1523_Line(i);
        }
        static void Impl_1523_Tri3(int n)
        {
            for (int i = 1; i <= n; ++i)
            {
                string s1 = new string(' ', n - i);
                string s2 = new string('*', i * 2 - 1);
                Console.WriteLine("{0}{1}", s1, s2);
            }
        }
        static void Impl_1523(int n, int type)
        {
            if (1 == type)
                Impl_1523_Tri1(n);
            else if (2 == type)
                Impl_1523_Tri2(n);
            else if (3 == type)
                Impl_1523_Tri3(n);
        }
        static void _1523()
        {
            Impl_1523(5, 1);
            Console.WriteLine();
            Impl_1523(5, 2);
            Console.WriteLine();
            Impl_1523(5, 3);
            Console.WriteLine();
        }


        //--------------------------------------------------
        // 5934 별삼각형2
        //--------------------------------------------------
        static void Impl_5934(int n)
        {
            if (n < 1 || n > 99 || n % 2 == 0)
            {
                Console.WriteLine("INPUT ERROR!");
                return;
            }

            int N = n / 2 + 1;
            for(int i=0; i<N; ++i)
            {
                Console.Write(new string('-', i));
                Console.Write(new string('*', N - i));
                Console.WriteLine();
            }

            for(int j=2; j<=N; ++j)
            {
                Console.Write(new string('-', N-1));
                Console.Write(new string('*', j));
                Console.WriteLine();
            }
        }
        static void _5934()
        {
            Impl_5934(0);
            Console.WriteLine();
            Impl_5934(3);
            Console.WriteLine();
            Impl_5934(7);
            Console.WriteLine();
        }

        //--------------------------------------------------
        // 1329 별삼각형3
        //--------------------------------------------------
        static void Impl_1329(int n)
        {
            if (n % 2 == 0)
                return;

            int half = n / 2 + 1;

            for (int i = 0; i < half; ++i)
            {
                Console.WriteLine("{0}{1}", new string(' ', i), new string('*', i * 2 + 1));
            }

            for (int i = half; i < n; ++i)
            {
                Console.WriteLine("{0}{1}", new string(' ', n - i - 1), new string('*', (n - i - 1) * 2 + 1));
            }
        }
        static void _1329()
        {
            Impl_1329(7);
        }

        //--------------------------------------------------
        // 5945 숫자 삼각형1
        //--------------------------------------------------
        static void Impl_5945(int n)
        {
            int[][] arr = new int[n][];

            int num = 1;
            for (int i = 0; i < n; ++i)
            {
                arr[i] = new int[i + 1];
                for (int j = 0; j <= i; ++j)
                {
                    arr[i][j] = num++;
                }
                if (i % 2 == 1)
                    Array.Reverse(arr[i]);
            }
            UT.Util.PrintArray2(arr, 3);
        }

        static void _5945()
        {
            Impl_5945(5);
        }

        //--------------------------------------------------
        // 5946 숫자 삼각형2
        //--------------------------------------------------
        static void Impl_5946(int n)
        {
            if (n < 1 || n > 50 || n % 2 == 0)
            {
                Console.WriteLine("INPUT ERROR!");
                return;
            }

            int cnt = n * 2 - 1;
            for(int i = 0; i < n; ++i)
            {
                Console.Write(new string(' ', i*2));
                for(int j=0; j<cnt; ++j)
                {
                    Console.Write("{0} ", i);
                }
                cnt -= 2;
                Console.WriteLine();
            }
        }
        static void _5946()
        {
            Impl_5946(3);
            Console.WriteLine();
            Impl_5946(5);
            Console.WriteLine();
        }

        //--------------------------------------------------
        // 5947 숫자 삼각형3
        //--------------------------------------------------
        static void Impl_5947(int n)
        {
            if (n < 1 || n > 50 || n % 2 == 0)
            {
                Console.WriteLine("INPUT ERROR!");
                return;
            }

            int N = n / 2 + 1;
            for (int i = 1; i <= N; ++i)
            {
                for (int j = 1; j <= i; ++j)
                {
                    Console.Write("{0} ", j);
                }
                Console.WriteLine();
            }

            for (int i = N-1; i > 0; --i)
            {
                for (int j = 1; j <= i; ++j)
                {
                    Console.Write("{0} ", j);
                }
                Console.WriteLine();
            }
        }
        static void _5947()
        {
            Impl_5947(3);
            Console.WriteLine();
            Impl_5947(7);
            Console.WriteLine();
        }

        //--------------------------------------------------
        // 1707 달팽이사각형
        //--------------------------------------------------
        static void Impl_1707(int n)
        {
            int[,] arr = new int[n, n];

            int minRow = 0;
            int maxRow = n - 1;
            int minCol = 0;
            int maxCol = n - 1;

            int num = 1;
            int dir = 0; // 0 : →, 1 : ↓, 2 : ←, 3 : ↑
            while (num <= n * n)
            {

                if (0 == dir)
                { // 0 : →

                    for (int i = minCol; i <= maxCol; ++i)
                        arr[minRow, i] = num++;

                    ++minRow;

                }
                else if (1 == dir)
                { // 1 : ↓

                    for (int i = minRow; i <= maxRow; ++i)
                        arr[i, maxCol] = num++;

                    --maxCol;
                }
                else if (2 == dir)
                { // 2 : ←

                    for (int i = maxCol; i >= minCol; --i)
                        arr[maxRow, i] = num++;

                    --maxRow;

                }
                else
                { // 3 : ↑

                    for (int i = maxRow; i >= minRow; --i)
                        arr[i, minCol] = num++;

                    ++minCol;
                }

                ++dir;
                if (dir > 3)
                    dir = 0;

            }

            UT.Util.PrintArray(arr, 3);
        }
        static void _1707()
        {
            Impl_1707(3);
            Impl_1707(5);
        }

        //--------------------------------------------------
        // 5397 달팽이사각형 (reversed)
        //--------------------------------------------------
        static void Impl_5397(int n)
        {
            int[,] arr = new int[n, n];

            int minRow = 0;
            int maxRow = n - 1;
            int minCol = 0;
            int maxCol = n - 1;

            int num = 1;
            int dir = 2; // 0 : →, 1 : ↓, 2 : ←, 3 : ↑
            while (num <= n * n)
            {

                if (0 == dir)
                { // 0 : →

                    for (int i = minCol; i <= maxCol; ++i)
                        arr[maxRow, i] = num++;

                    --maxRow;

                }
                else if (1 == dir)
                { // 1 : ↓

                    for (int i = minRow; i <= maxRow; ++i)
                        arr[i, minCol] = num++;

                    ++minCol;
                }
                else if (2 == dir)
                { // 2 : ←

                    for (int i = maxCol; i >= minCol; --i)
                        arr[minRow, i] = num++;

                    ++minRow;

                }
                else
                { // 3 : ↑

                    for (int i = maxRow; i >= minRow; --i)
                        arr[i, maxCol] = num++;

                    --maxCol;
                }

                --dir;
                if (dir < 0)
                    dir = 3;

            }

            UT.Util.PrintArray(arr, 3);
        }
        static void _5397()
        {
            Impl_5397(3);
            Impl_5397(5);
        }


        //--------------------------------------------------
        // 1337 달팽이삼각형
        //--------------------------------------------------
        static void Impl_1337(int n)
        {
            int[,] arr = new int[n, n];

            int dir = 0; //0:↘ 1:← 2:↑
            int r = -1, c = -1;
            int num = 0;
            int cnt = n;

            while (cnt > 0)
            {

                for (int i = 0; i < cnt; ++i)
                {

                    if (0 == dir)
                    {
                        ++r; ++c;
                    }
                    else if (1 == dir)
                    {
                        --c;
                    }
                    else if (2 == dir)
                    {
                        --r;
                    }

                    arr[r, c] = num++;
                    if (num > 9)
                        num = 0;

                }

                ++dir;
                if (dir > 2)
                    dir = 0;

                --cnt;
            }

            for (int i = 0; i < n; ++i)
            {

                for (int j = 0; j <= i; ++j)
                    Console.Write("{0} ", arr[i, j]);
                Console.WriteLine();
            }
        }
        static void _1337()
        {
            Impl_1337(6);
        }

    }
}
