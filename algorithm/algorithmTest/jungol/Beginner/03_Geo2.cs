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
            Util.Call(_5398);
            Util.Call(_2071);
            Util.Call(_1331);
            Util.Call(_1495);
            Util.Call(_2074);
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

        //--------------------------------------------------
        // 5398 달팽이트리
        //--------------------------------------------------
        static void Impl_5398(int n)
        {
            int[,] arr = new int[n, n];
            
            int dir = 0; //0:↓ 1:→ 2:↖
            int r = -1, c = 0;
            int num = 0;
            int cnt = n;

            while (cnt > 0)
            {
                for (int i = 0; i < cnt; ++i)
                {
                    if (0 == dir)
                    {
                        ++r;
                    }
                    else if (1 == dir)
                    {
                        ++c;
                    }
                    else if (2 == dir)
                    {
                        --r; --c;
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

            for(r=0; r<n; ++r)
            {
                Console.Write(new string(' ', n - r - 1));
                for(c=0; c<=r; ++c)
                {
                    Console.Write("{0} ", arr[r, c]);
                }
                Console.WriteLine();
            }
        }
        static void _5398()
        {
            Impl_5398(25);
            Console.WriteLine();
            Impl_5398(6);
            Console.WriteLine();
        }

        //--------------------------------------------------
        // 2071 파스칼 삼각형
        //--------------------------------------------------
        static void Impl_2071_1(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); ++i)
            {
                for (int j = 0; j <= i; ++j)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Impl_2071_2(int[,] arr)
        {
            int n = arr.GetLength(1);
            for (int j = n - 1; j >= 0; --j)
            {

                for (int i = n - 1; i >= j; --i)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Impl_2071_3(int[,] arr)
        {
            int n = arr.GetLength(0);

            for (int i = n - 1; i >= 0; --i)
            {

                Console.Write(new string('-', n - i - 1));
                for (int j = 0; j <= i; ++j)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Impl_2071(int size, int type)
        {
            int[,] arr = new int[size, size];

            arr[0, 0] = 1;
            for (int i = 1; i < size; ++i)
            {

                arr[i, 0] = 1;
                for (int j = 1; j <= i; ++j)
                {
                    arr[i, j] = arr[i - 1, j - 1] + arr[i - 1, j];
                }
            }

            if (type == 1)
                Impl_2071_1(arr);
            else if (type == 2)
                Impl_2071_2(arr);
            else if (type == 3)
                Impl_2071_3(arr);

        }
        static void _2071()
        {
            int n = 6;
            Impl_2071(n, 1);
            Impl_2071(n, 2);
            Impl_2071(n, 3);
        }

        //--------------------------------------------------
        // 1331 문자마름모
        //--------------------------------------------------
        static void Impl_1331(int n)
        {
            const char S = ' ';
            int N = n * 2 - 1;
            char[,] arr = new char[N, N];

            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N; ++j)
                    arr[i, j] = S;
            }

            int cnt = n;
            char ch = 'A';
            for (int i = 0; i < n; ++i)
            {
                // ↙
                int r = i;
                int c = n - 1;
                for (int j = 0; j < cnt; ++j)
                {
                    arr[r++, c--] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                // ↘
                c = i + 1;
                for (int j = 0; j < cnt - 1; ++j)
                {
                    arr[r++, c++] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }

                // ↗
                r = N - 2 - i;
                c = n;
                for (int j = 0; j < cnt - 1; ++j)
                {
                    arr[r--, c++] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                // ↖
                c = N - 2 - i;
                for (int j = 0; j < cnt - 2; ++j)
                {
                    arr[r--, c--] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                --cnt;
            }

            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N - 1; ++j)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine(arr[i, N - 1]);
            }
        }
        static void _1331()
        {
            Impl_1331(15);
        }

        //--------------------------------------------------
        // 1495 대각선 지그재그
        //--------------------------------------------------
        static void Impl_1495(int n)
        {
            int[,] arr = new int[n, n];
            int cnt = n * 2 - 1;

            int r = 0;
            int c = 0;
            int num = 1;
            bool leftDown = true;
            for (int i = 0; i < cnt; ++i)
            {

                if (leftDown)
                {
                    // ↙
                    while (true)
                    {

                        arr[r, c] = num++;
                        if (r == n - 1)
                        {
                            ++c;
                            break;
                        }
                        if (c == 0)
                        {
                            ++r;
                            break;
                        }
                        ++r;
                        --c;
                    }

                }
                else
                {

                    while (true)
                    {
                        // ↗
                        arr[r, c] = num++;
                        if (c == n - 1)
                        {
                            ++r;
                            break;
                        }
                        if (r == 0)
                        {
                            ++c;
                            break;
                        }
                        --r;
                        ++c;
                    }

                }

                leftDown = !leftDown;
            }

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write("{0,2} ", arr[i, j]);
                }
                Console.WriteLine("{0,2}", arr[i, n - 1]);
            }
        }
        static void _1495()
        {
            Impl_1495(5);
        }

        //--------------------------------------------------
        // 2074 마방진
        //
        // 1. 첫 번째 행 가운데(1,2)에 1을 넣는다.
        // 2. 왼쪽 위로 이동하면 (0,1)인데 행의 위치가 처음을 벗어났으므로 마지막행(3,1)의 위치로 이동하여 2를 넣는다.
        // 3. 왼쪽 위로 이동하면 (2,0)인데 열의 위치가 처음을 벗어났으므로 마지막열(2,3)의 위치로 이동하여 3을 넣는다.
        // 4. 배열에 넣은 값(3)이 N의 배수이므로 바로 아래행으로(3,3) 이동하여 4를 넣는다.
        // 5. 왼쪽 위로 이동하여 (2,2) 5를 넣는다.
        // 6. 왼쪽 위로 이동하여 (1,1) 6를 넣는다.
        // 7. 배열에 넣은 값(6)이 N의 배수이므로 바로 아래행으로(2,1) 이동하여 7을 넣는다.
        // 8. 왼쪽 위로 이동하면 (1,0)인데 열의 위치가 처음을 벗어났으므로 마지막열(1,3)의 위치로 이동하여 8을 넣는다.
        // 9. 왼쪽 위로 이동하면 (0,2)인데 행의 위치가 처음을 벗어났으므로 마지막행(3,2)의 위치로 이동하여 9를 넣는다.
        //
        // 6 1 8
        // 7 5 3
        // 2 9 4
        //--------------------------------------------------
        static void Impl_2074(int n)
        {
            if (n % 2 == 0)
                return;

            int[,] arr = new int[n, n];

            int r = 0, c = n / 2;
            for (int i = 1; i <= n * n; ++i)
            {

                arr[r, c] = i;

                if (i % n == 0)
                {
                    ++r;
                }
                else
                {
                    --r;
                    --c;
                }

                if (r < 0)
                    r = n - 1;
                if (r >= n)
                    r = 0;
                if (c < 0)
                    c = n - 1;
                if (c >= n)
                    c = 0;
            }

            Util.PrintArray(arr, 3);
        }

        static void _2074()
        {
            Impl_2074(5);
        }

    }
}
