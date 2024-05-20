using System;
using System.Collections.Generic;

namespace Jungol
{
    class SkillUp
    {
        public static void Run()
        {
            //Util.Call(_1303);           // 1303 숫자사각형1
            //Util.Call(_1856);           // 1856 숫자사각형2
            //Util.Call(_1304);           // 1304 숫자사각형3
            //Util.Call(_2046);           // 2046 숫자사각형4
            //Util.Call(_1307);           // 1307 문자사각형1
            //Util.Call(_1314);           // 1314 문자사각형2
            //Util.Call(_1707);           // 1707 달팽이사각형
            //Util.Call(_1331);           // 1331 문자마름모
            //Util.Call(_1495);           // 1495 대각선 지그재그
            //Util.Call(_2074);           // 2074 마방진
            //Util.Call(_1523);           // 1523 별삼각형1
            //Util.Call(_1719);           // 1719 별삼각형2
            //Util.Call(_1329);           // 1329 별삼각형3
            //Util.Call(_1641);           // 1641 숫자삼각형
            //Util.Call(_1337);           // 1337 달팽이삼각형
            //Util.Call(_1338);           // 1338 문자삼각형1
            //Util.Call(_1339);           // 1339 문자삼각형2
            //Util.Call(_2071);           // 2071 파스칼 삼각형
            //Util.Call(_1291);           // 1291 구구단
            //Util.Call(_1341);           // 1341 구구단2
            //Util.Call(_1009);           // 1009 각 자리수의 역과 합
            //Util.Call(_2812);           // 2812 각 자리수의 합
            //Util.Call(_1430);           // 1430 숫자의 개수
            //Util.Call(_1692);           // 1692 곱셈
            //Util.Call(_1402);           // 1402 약수 구하기
            //Util.Call(_2809);           // 2809 약수
            //Util.Call(_1071);           // 1071 약수와 배수
            //Util.Call(_1658);           // 1658 최대공약수와최소공배수
            //Util.Call(_1002);           // 1002 최대공약수, 최소공배수
            //Util.Call(_2255);           // 2255 섞기 수열
            //Util.Call(_2498);           // 2498 공약수
            //Util.Call(_2810);           // 2810 타일교체
            //Util.Call(_2811);           // 2811 소수와 합성수
            //Util.Call(_2813);           // 2813 소수의 개수
            //Util.Call(_1740);           // 1740 소수
            //Util.Call(_1901);           // 1901 소수 구하기
            //Util.Call(_2814);           // 2814 이진수
            //Util.Call(_2815);           // 2815 10진수를 2진수로
            //Util.Call(_1274);           // 1274 2진수를 10진수로...
            //Util.Call(_1534);           // 1534 10진수를 2 8 16진수로
            //Util.Call(_3106);           // 3106 진법 변환
            //Util.Call(_1097);           // 1097 앞뒤 같은 제곱
            //Util.Call(_2604);           // 2604 그릇
            //Util.Call(_2514);           // 2514 문자열 찾기
            //Util.Call(_2518);           // 2518 문자열변환
            //Util.Call(_1239);           // 1239 비밀편지
            //Util.Call(_1620);           // 1620 전화번호 속의 암호
            //Util.Call(_1516);           // 1516 단어 세기
            //Util.Call(_1535);           // 1535 단어집합(하)
            //Util.Call(_1566);           // 1566 소수문자열
            //Util.Call(_2085);           // 2085 윤년
            //Util.Call(_1031);           // 1031 빙고
            //Util.Call(_1112);           // 1112 줄자접기
            //Util.Call(_1311);           // 1311 카드게임
            //Util.Call(_1438);           // 1438 색종이(초)
            //Util.Call(_1733);           // 1733 오목
            //Util.Call(_1997);           // 1997 떡 먹는 호랑이
            //Util.Call(_2460);           // 2460 나는 학급회장이다.(투표)
            //Util.Call(_2101);           // 2101 연속부분최대곱
            //Util.Call(_1671);           // 1671 색종이(중)
            //Util.Call(_1124);           // 1124 색종이(고)
            //Util.Call(_1761);           // 1761 숫자 야구
            //Util.Call(_1998);           // 1998 수열
            //Util.Call(_2259);           // 2259 참외밭
            //Util.Call(_1836);           // 1836 연속부분합 찾기
            //Util.Call(_1102);           // 1102 스택 (stack)
            //Util.Call(_1697);           // 1697 큐(queue)
            //Util.Call(_1146);           // 1146 선택정렬
            //Util.Call(_1158);           // 1158 삽입정렬
            //Util.Call(_1157);           // 1157 버블정렬
            //Util.Call(_1814);           // 1814 삽입정렬 횟수 세기
            //Util.Call(_1295);           // 1295 이진탐색
            //Util.Call(_1221);           // 1221 후위표기법
            //Util.Call(_2858);           // 2858 쇠막대기
            //Util.Call(_1309);           // 1309 팩토리얼
            Util.Call(_1161);           // 1161 하노이의 탑
            //Util.Call(_1169);           // 1169 주사위 던지기1
            //Util.Call(_1175);           // 1175 주사위 던지기2
            //Util.Call(_1459);           // 1459 숫자고르기
            //Util.Call(_1021);           // 1021 장난감조립
            //Util.Call(_1147);           // 1147 주사위 쌓기
        }


        //--------------------------------------------------
        // 1303 숫자사각형1
        //--------------------------------------------------
        static void Impl_1303(int n, int m)
        {
            int num = 1;
            for (int row = 0; row < n; ++row) {
                for (int column = 0; column < m - 1; ++column) {
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
            for (int i = 0; i < count - 1; ++i) {
                Console.Write(num++);
                Console.Write(' ');
            }
            Console.Write(num++);
            Console.WriteLine();
        }
        static void Impl_1856_toLeft(int num, int count)
        {
            num = num + (count - 1);
            for (int i = 0; i < count - 1; ++i) {
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
            for (int row = 0; row < n; ++row) {
                if (toRight) {
                    Impl_1856_toRight(num, m);
                }
                else {
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

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j < n - 1; ++j) {
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
            for (int i = 0; i < n; ++i) {

                int num = i + 1;
                for (int j = 0; j < n - 1; ++j) {
                    Console.Write(num);
                    Console.Write(' ');
                }
                Console.WriteLine(num);
            }
        }
        static void Impl_2046_Tri2(int n)
        {
            bool toLeft = true;
            for (int i = 0; i < n; ++i) {

                int num = toLeft ? 1 : n;
                for (int j = 0; j < n - 1; ++j) {
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
            for (int i = 0; i < n; ++i) {

                int num = i + 1;
                for (int j = 0; j < n - 1; ++j) {
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
        // 1307 문자사각형1
        //--------------------------------------------------
        static void Impl_1307(int n)
        {
            char ch = 'A';
            char[,] tbl = new char[n, n];
            for (int i = n - 1; i >= 0; --i) {

                for (int j = n - 1; j >= 0; --j) {
                    tbl[j, i] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
            }

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j < n - 1; ++j) {
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

            for (int i = 0; i < n; ++i) {

                steps[0] = (char)((n - i) * 2 - 1);
                steps[1] = (char)(1 + 2 * i);

                char chtmp = ch;
                for (int j = 0; j < n; ++j) {

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
            while (num <= n * n) {

                if (0 == dir) { // 0 : →

                    for (int i = minCol; i <= maxCol; ++i)
                        arr[minRow, i] = num++;

                    ++minRow;

                }
                else if (1 == dir) { // 1 : ↓

                    for (int i = minRow; i <= maxRow; ++i)
                        arr[i, maxCol] = num++;

                    --maxCol;
                }
                else if (2 == dir) { // 2 : ←

                    for (int i = maxCol; i >= minCol; --i)
                        arr[maxRow, i] = num++;

                    --maxRow;

                }
                else { // 3 : ↑

                    for (int i = maxRow; i >= minRow; --i)
                        arr[i, minCol] = num++;

                    ++minCol;
                }

                ++dir;
                if (dir > 3)
                    dir = 0;

            }


            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n - 1; ++j) {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine(arr[i, n - 1]);
            }
        }
        static void _1707()
        {
            Impl_1707(3);

        }



        //--------------------------------------------------
        // 1331 문자마름모
        //--------------------------------------------------
        static void Impl_1331(int n)
        {
            const char S = ' ';
            int N = n * 2 - 1;
            char[,] arr = new char[N, N];

            for (int i = 0; i < N; ++i) {
                for (int j = 0; j < N; ++j)
                    arr[i, j] = S;
            }

            int cnt = n;
            char ch = 'A';
            for (int i = 0; i < n; ++i) {
                // ↙
                int r = i;
                int c = n - 1;
                for (int j = 0; j < cnt; ++j) {
                    arr[r++, c--] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                // ↘
                c = i + 1;
                for (int j = 0; j < cnt - 1; ++j) {
                    arr[r++, c++] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }

                // ↗
                r = N - 2 - i;
                c = n;
                for (int j = 0; j < cnt - 1; ++j) {
                    arr[r--, c++] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                // ↖
                c = N - 2 - i;
                for (int j = 0; j < cnt - 2; ++j) {
                    arr[r--, c--] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                --cnt;
            }

            for (int i = 0; i < N; ++i) {
                for (int j = 0; j < N - 1; ++j) {
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
            for (int i = 0; i < cnt; ++i) {

                if (leftDown) {
                    // ↙
                    while (true) {

                        arr[r, c] = num++;
                        if (r == n - 1) {
                            ++c;
                            break;
                        }
                        if (c == 0) {
                            ++r;
                            break;
                        }
                        ++r;
                        --c;
                    }

                }
                else {

                    while (true) {
                        // ↗
                        arr[r, c] = num++;
                        if (c == n - 1) {
                            ++r;
                            break;
                        }
                        if (r == 0) {
                            ++c;
                            break;
                        }
                        --r;
                        ++c;
                    }

                }

                leftDown = !leftDown;
            }

            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n - 1; ++j) {
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
            for (int i = 1; i <= n * n; ++i) {

                arr[r, c] = i;

                if (i % n == 0) {
                    ++r;
                }
                else {
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
            for (int i = 1; i <= n; ++i) {
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
            Impl_1523(5, 2);
            Impl_1523(5, 3);
        }



        //--------------------------------------------------
        // 1719 별삼각형2
        //--------------------------------------------------
        static void Impl_1719_Tri1(int n)
        {
            int width = n / 2 + 1;
            for (int i = 1; i <= width; ++i) {
                Console.WriteLine(new string('*', i));
            }
            for (int i = 1; i < width; ++i) {
                Console.WriteLine(new string('*', width - i));
            }
        }
        static void Impl_1719_Tri2(int n)
        {
            int width = n / 2 + 1;
            for (int i = 1; i <= width; ++i) {
                Console.WriteLine("{0}{1}", new string(' ', width - i), new string('*', i));
            }
            for (int i = 1; i < width; ++i) {
                Console.WriteLine("{0}{1}", new string(' ', i), new string('*', width - i));
            }
        }
        static void Impl_1719_Tri3(int n)
        {
            int half = n / 2;
            for (int i = 0; i < half; ++i)
                Console.WriteLine("{0}{1}", new string(' ', i), new string('*', n - i * 2));

            for (int i = half; i < n; ++i)
                Console.WriteLine("{0}{1}", new string(' ', n - i - 1), new string('*', (i - half) * 2 + 1));
        }
        static void Impl_1719_Tri4(int n)
        {
            int half = n / 2;
            for (int i = 0; i < half; ++i)
                Console.WriteLine("{0}{1}", new string(' ', i), new string('*', half - i + 1));

            for (int i = half; i < n; ++i)
                Console.WriteLine("{0}{1}", new string(' ', half), new string('*', (i - half) + 1));
        }
        static void Impl_1719(int n, int type)
        {
            if (n % 2 == 0)
                return;
            Console.WriteLine("--- size : {0}, type : {1}", n, type);
            if (type == 1)
                Impl_1719_Tri1(n);
            else if (type == 2)
                Impl_1719_Tri2(n);
            else if (type == 3)
                Impl_1719_Tri3(n);
            else if (type == 4)
                Impl_1719_Tri4(n);
        }
        static void _1719()
        {
            Impl_1719(5, 1);
            Impl_1719(5, 2);
            Impl_1719(5, 3);
            Impl_1719(5, 4);
        }



        //--------------------------------------------------
        // 1329 별삼각형3
        //--------------------------------------------------
        static void Impl_1329(int n)
        {
            if (n % 2 == 0)
                return;

            int half = n / 2 + 1;

            for (int i = 0; i < half; ++i) {
                Console.WriteLine("{0}{1}", new string(' ', i), new string('*', i * 2 + 1));
            }

            for (int i = half; i < n; ++i) {
                Console.WriteLine("{0}{1}", new string(' ', n - i - 1), new string('*', (n - i - 1) * 2 + 1));
            }
        }
        static void _1329()
        {
            Impl_1329(7);
        }



        //--------------------------------------------------
        // 1641 숫자삼각형
        //--------------------------------------------------
        static void Impl_1641_Tri1(int n)
        {
            int min = 0;
            int max = 0;
            int num = 1;
            bool toLeft = true;
            for (int i = 1; i <= n; ++i) {

                min = max + 1;
                max += i;

                if (toLeft) {
                    num = min;
                    for (int j = 0; j < i; ++j)
                        Console.Write("{0} ", num++);
                }
                else {
                    num = max;
                    for (int j = 0; j < i; ++j)
                        Console.Write("{0} ", num--);
                }
                Console.WriteLine();

                toLeft = !toLeft;
            }
        }
        static void Impl_1641_Tri2(int n)
        {
            for (int i = 0; i < n; ++i) {
                Console.Write(new string(' ', i * 2));
                for (int j = 0; j < (n - i) * 2 - 1; ++j)
                    Console.Write("{0} ", i);
                Console.WriteLine();
            }
        }
        static void Impl_1641_Tri3(int n)
        {
            int half = n / 2 + 1;

            for (int i = 1; i <= half; ++i) {

                for (int j = 1; j <= i; ++j)
                    Console.Write("{0} ", j);
                Console.WriteLine();
            }
            for (int i = half - 1; i > 0; --i) {

                for (int j = 1; j <= i; ++j)
                    Console.Write("{0} ", j);
                Console.WriteLine();
            }

        }
        static void Impl_1641(int size, int type)
        {
            if (size % 2 == 0)
                return;

            if (1 == type)
                Impl_1641_Tri1(size);
            else if (2 == type)
                Impl_1641_Tri2(size);
            else if (3 == type)
                Impl_1641_Tri3(size);
        }
        static void _1641()
        {
            Impl_1641(5, 1);
            Impl_1641(5, 2);
            Impl_1641(5, 3);
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

            while (cnt > 0) {

                for (int i = 0; i < cnt; ++i) {

                    if (0 == dir) {
                        ++r; ++c;
                    }
                    else if (1 == dir) {
                        --c;
                    }
                    else if (2 == dir) {
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

            for (int i = 0; i < n; ++i) {

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
            while (cnt > 0) {

                for (int i = n - cnt, j = n - 1; i < n; ++i, --j) {
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

            for (int i = 0; i < COL; ++i) {

                int cnt = n - i * 2;
                int startRow = n - i - 1;
                for (int j = 0; j < cnt; ++j) {

                    int row = startRow - j;
                    arr[row, i] = lastC--;
                    if (lastC < 'A')
                        lastC = 'Z';
                }
            }

            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < COL; ++j) {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void _1339()
        {
            Impl_1339(5);
        }



        //--------------------------------------------------
        // 2071 파스칼 삼각형
        //--------------------------------------------------
        static void Impl_2071_1(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); ++i) {
                for (int j = 0; j <= i; ++j) {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Impl_2071_2(int[,] arr)
        {
            int n = arr.GetLength(1);
            for (int j = n - 1; j >= 0; --j) {

                for (int i = n - 1; i >= j; --i) {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Impl_2071_3(int[,] arr)
        {
            int n = arr.GetLength(0);

            for (int i = n - 1; i >= 0; --i) {

                Console.Write(new string('-', n - i - 1));
                for (int j = 0; j <= i; ++j) {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Impl_2071(int size, int type)
        {
            int[,] arr = new int[size, size];

            arr[0, 0] = 1;
            for (int i = 1; i < size; ++i) {

                arr[i, 0] = 1;
                for (int j = 1; j <= i; ++j) {
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
        // 1291 구구단
        //--------------------------------------------------
        static void Impl_1291(int s, int e)
        {
            int step = (s < e) ? 1 : -1;

            for (int i = 1; i <= 9; ++i) {

                for (int j = s; ; j += step) {

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

            for (int i = s; i != e; i += step) {

                for (int j = 1; j <= 9; j += 3) {
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
        // 1009 각 자리수의 역과 합
        //--------------------------------------------------
        static void Impl_1009(int n)
        {
            int sum = 0;
            while (n > 10) {
                int rem = n % 10;
                sum += rem;
                Console.Write(rem);
                n /= 10;
            }
            sum += n;
            Console.WriteLine("{0} {1}", n, sum);
        }
        static void _1009()
        {
            int[] inputs = { 453, 123456, 0 };
            foreach (int i in inputs) {
                if (i <= 0)
                    break;

                Impl_1009(i);
            }
        }



        //--------------------------------------------------
        // 2812 각 자리수의 합
        //--------------------------------------------------
        static void Impl_2812(int n)
        {
            while (n > 10) {

                int sum = 0;
                while (n > 0) {
                    sum += n % 10;
                    n /= 10;
                }

                Console.WriteLine(sum);
                n = sum;
            }
            Console.WriteLine(n);
        }
        static void _2812()
        {
            Impl_2812(1234567891);
        }



        //--------------------------------------------------
        // 1430 숫자의 개수
        //--------------------------------------------------
        static void Impl_1430(int a, int b, int c)
        {
            int mul = a * b * c;

            int[] cnts = new int[10];

            while (mul > 10) {

                int rem = mul % 10;
                ++cnts[rem];
                mul /= 10;
            }
            ++cnts[mul];
            foreach (int n in cnts)
                Console.WriteLine(n);
        }
        static void _1430()
        {
            Impl_1430(150, 266, 427);
        }



        //--------------------------------------------------
        // 1692 곱셈
        //--------------------------------------------------
        static void Impl_1692(int a, int b)
        {
            int mul = a * b;
            while (b > 0) {
                int rem = b % 10;
                Console.WriteLine(a * rem);
                b /= 10;
            }
            Console.WriteLine(mul);
        }
        static void _1692()
        {
            Impl_1692(472, 385);
        }



        //--------------------------------------------------
        // 1402 약수 구하기
        //--------------------------------------------------
        static void Impl_1402(int n, int k)
        {
            if (k == 1) {
                Console.WriteLine(1);
                return;
            }

            int[] divds = new int[k];
            int index = 0;
            divds[index++] = 1;

            for (int i = 2; i < n; ++i) {
                if (n % i == 0) {
                    divds[index++] = i;

                    if (index == k) {
                        Console.WriteLine(i);
                        return;
                    }
                }
            }

            Console.WriteLine(0);
        }
        static void _1402()
        {
            Impl_1402(6, 3);
            Impl_1402(2735, 1);
        }



        //--------------------------------------------------
        // 2809 약수
        //--------------------------------------------------
        static void Impl_2809(int n)
        {
            for (int i = 1; i <= n; ++i) {
                if (n % i == 0)
                    Console.Write("{0} ", i);
            }
            Console.WriteLine();
        }
        static void _2809()
        {
            Impl_2809(24);
        }



        //--------------------------------------------------
        // 1071 약수와 배수
        // 
        // 6
        // 2 3 5 12 18 24
        // 12
        // 
        // 17
        // 36
        //--------------------------------------------------
        static void Impl_1071(string n, string nums, string theNum)
        {
            int cnt = Convert.ToInt32(n);
            string[] words = nums.Split();
            int val = Convert.ToInt32(theNum);

            System.Diagnostics.Debug.Assert(cnt == words.Length);

            int sumOfDivisors = 0;
            int sumOfMultiples = 0;
            for (int i = 0; i < cnt; ++i) {
                int tmp = Convert.ToInt32(words[i]);

                //약수
                if ((tmp <= val) && (val % tmp == 0))
                    sumOfDivisors += tmp;

                //배수
                if ((tmp >= val) && (tmp % val == 0))
                    sumOfMultiples += tmp;
            }

            Console.WriteLine(sumOfDivisors);
            Console.WriteLine(sumOfMultiples);
        }
        static void _1071()
        {
            Impl_1071("6", "2 3 5 12 18 24", "12");
        }




        //--------------------------------------------------
        // 1658 최대공약수와최소공배수
        //
        // 24 18
        // 
        // 6
        // 72
        //--------------------------------------------------
        static void Impl_1658(int n1, int n2)
        {
            int LCM = 1;
            int GCD = 1;
            //the least[lowest] common multiple (LCM)
            //the greatest common denominator (GCD)

            int min = Math.Min(n1, n2);
            int max = Math.Max(n1, n2);

            while (min > 1) {

                bool notFound = true;
                for (int i = 2; i <= min; ++i) {
                    if ((min % i == 0) && (max % i == 0)) {
                        GCD *= i;
                        min /= i;
                        max /= i;

                        notFound = false;
                    }
                }

                if (!notFound)
                    break;
            }


            LCM = GCD * min * max;

            Console.WriteLine(GCD);
            Console.WriteLine(LCM);
        }
        static void _1658()
        {
            Impl_1658(24, 18);
        }



        //--------------------------------------------------
        // 1002 최대공약수, 최소공배수
        // 
        // 3
        // 2 8 10
        // 
        // 2 40
        //--------------------------------------------------
        static void Impl_1002(string sCount, string sNums)
        {
            int LCM = 1;    //the least[lowest] common multiple (LCM)
            int GCD = 1;    //the greatest common denominator (GCD)


            int cnt = Convert.ToInt32(sCount);
            string[] words = sNums.Split();

            System.Diagnostics.Debug.Assert(cnt == words.Length);
            int[] nums = new int[cnt];
            for (int i = 0; i < cnt; ++i)
                nums[i] = Convert.ToInt32(words[i]);

            Array.Sort(nums);

            int min = nums[0];
            while (min > 1) {

                bool notFound = true;
                for (int i = 2; i <= min; ++i) {

                    bool isCommonDividor = true;
                    for (int j = 1; j < cnt; ++j) {

                        if (nums[j] % i != 0) {
                            isCommonDividor = false;
                            break;
                        }
                    }

                    if (isCommonDividor) {

                        GCD *= i;
                        notFound = false;
                        min /= i;
                        nums[0] = min;
                        for (int j = 1; j < cnt; ++j) {
                            nums[j] /= i;
                        }
                    }
                }

                if (notFound)
                    break;
            }

            LCM = GCD;
            for (int j = 0; j < cnt; ++j) {
                LCM *= nums[j];
            }

            Console.WriteLine("{0} {1}", GCD, LCM);

        }
        static void _1002()
        {
            Impl_1002("3", "2 8 10");
        }



        //--------------------------------------------------
        // 2255 섞기 수열
        //--------------------------------------------------
        static bool Impl_2255_IsSorted(int[] arr)
        {
            for(int i=1; i<arr.Length; ++i) {
                if (arr[i] < arr[i - 1])
                    return false;
            }

            return true;
        }
        static void Impl_2255(string sCount, string sNums)
        {
            int cnt = Convert.ToInt32(sCount);
            string[] words = sNums.Split();

            System.Diagnostics.Debug.Assert(cnt == words.Length);

            int[] arr = new int[cnt];
            for(int i=0; i<cnt; ++i) {
                arr[i] = Convert.ToInt32(words[i]);
            }

            int[] shuffleIndex = new int[cnt];
            for (int i = 0; i < cnt; ++i) {
                shuffleIndex[i] = arr[i] - 1;
            }

            int shuffleCount = 1;
            int[] tmp = new int[cnt];
            while (false == Impl_2255_IsSorted(arr)) {

                Array.Copy(arr, tmp, cnt);
                for (int i = 0; i < cnt; ++i) {
                    arr[i] = tmp[shuffleIndex[i]];
                }
                ++shuffleCount;
            }

            Console.WriteLine(shuffleCount);
        }
        static void _2255()
        {
            Impl_2255("6", "3 2 5 6 1 4");
        }



        //--------------------------------------------------
        // 2498 공약수
        //--------------------------------------------------
        static bool Impl_2498_IsRelativelyPrime(int n1, int n2)
        {
            //서로소
            int min = Math.Min(n1, n2);

            for(int i=2; i<=min; ++i)
            {
                if (n1 % i == 0 && n2 % i == 0)
                    return false; // 공약수 존재
            }

            return true;
        }
        static void Impl_2498(int GCD, int LCM)
        {
            int LCM_D_GCD = LCM / GCD;
            
            System.Collections.Generic.Stack<int> list1 = new System.Collections.Generic.Stack<int>();
            System.Collections.Generic.Stack<int> list2 = new System.Collections.Generic.Stack<int>();
            list1.Push(1);
            list2.Push(LCM_D_GCD);
            

            for(int i=2; i<LCM_D_GCD; ++i)
            {
                if (i >= list2.Peek())
                    break;

                int t = LCM_D_GCD / i;
                if (t*i == LCM_D_GCD && Impl_2498_IsRelativelyPrime(i, t))
                {
                    list1.Push(i);
                    list2.Push(t);
                }
            }
            
            //1. 두수의 차가 가장 작은 약수의 합이 가장 작음을 가정 한 코드
            Console.WriteLine("{0} {1}", GCD * list1.Peek(), GCD * list2.Peek());


            ////2. 두수의 차가 가장 작은 약수의 합이 가장 작음을 모르면 아래와 같이 구함
            //int min = int.MaxValue;
            //int _1 = 0;
            //int _2 = 0;
            //while(list1.Count > 0)
            //{
            //    int l1 = list1.Pop();
            //    int l2 = list2.Pop();
            //
            //    if (min > l1 + l2)
            //    {
            //        min = l1 + l2;
            //        _1 = l1;
            //        _2 = l2;
            //    }
            //}
            //Console.WriteLine("{0} {1}", GCD * _1, GCD*_2);

        }
        static void _2498()
        {
            Impl_2498(6, 180);
            Impl_2498(2, 86486400);
        }



        //--------------------------------------------------
        // 2810 타일교체
        //--------------------------------------------------
        static void Impl_2810(int width, int height)
        {
            //최소 공약수
            int min = Math.Min(width, height);

            int GCD = 1;

            while(min > 1)
            {
                int tmp = min;

                bool notFound = true;
                for(int i=2; i<=min; ++i)
                {
                    if (width % i == 0 && height % i == 0)
                    {
                        width /= i;
                        height /= i;

                        min /= i;
                        GCD *= i;

                        notFound = false;
                        break;
                    }
                }
                if (notFound)
                    break;
            }

            //original width == width * GCD
            //original height == height * GCD
            Console.WriteLine( width * height );
        }
        static void _2810()
        {
            Impl_2810(24, 30);
        }



        //--------------------------------------------------
        // 2811 소수와 합성수
        //--------------------------------------------------
        static bool Impl_2811_IsPrimeNumber(int n)
        {
            for(int i=2; i*i < n; ++i)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }
        static void Impl_2811(string str)
        {
            string[] arr = str.Split();
            foreach(string w in arr)
            {
                int n = int.Parse(w);
                if (1 == n)
                    Console.WriteLine("number one");
                else if (Impl_2811_IsPrimeNumber(n))
                    Console.WriteLine("prime number");
                else
                    Console.WriteLine("composite number");
            }
        }
        static void _2811()
        {
            Impl_2811("3 10 1 55 127");
        }



        //--------------------------------------------------
        // 2813 소수의 개수
        //--------------------------------------------------
        static void Impl_2813_eratos(int from, int to)
        {
            System.Collections.BitArray lookup = new System.Collections.BitArray(to+1, true);
            lookup[0] = lookup[1] = false;
            
            int cnt = 0;
            for (int i = 2; i < to; ++i)
            {
                if (lookup[i])
                {
                    if (i>=from)
                        ++cnt;
                    uint ui = (uint)i;
                    for (uint j = ui * ui; j <= to; j += ui)
                        lookup[(int)j] = false;
                }
            }

            Console.WriteLine(cnt);
        }
        static void _2813()
        {
            //Impl_2813_eratos(10, int.MaxValue-1);
            Impl_2813_eratos(10, 100);
        }



        //--------------------------------------------------
        // 1740 소수
        //--------------------------------------------------
        static void Impl_1740(int from, int to)
        {
            System.Collections.BitArray lookup = new System.Collections.BitArray(to+1, true);
            lookup[0] = lookup[1] = false;

            int sum = 0;
            int min = 0;

            for(int i=2; i<=to; ++i)
            {
                if (lookup[i])
                {
                    if(i >= from)
                    {
                        if (min == 0)
                            min = i;
                        sum += i;
                    }
                    uint ui = (uint)i;
                    for(uint j=ui*ui; j<=(uint)to; j+=ui)
                    {
                        lookup[(int)j] = false;
                    }
                }
            }

            Console.WriteLine(sum);
            Console.WriteLine(min);
        }
        static void _1740()
        {
            Impl_1740(60, 100);
        }



        //--------------------------------------------------
        // 1901 소수 구하기
        //--------------------------------------------------
        static bool Impl_1901_IsPrimeNumber(int n)
        {
            for(int i=2; i*i <= n; ++i)
                if (n % i == 0)
                    return false;

            return true;
        }
        static void Impl_1901_NearestPrimeNumber(int n)
        {
            if (n <= 2)
                Console.WriteLine(2);

            int prime1 = 2;
            for (int i=n; i>=2; --i)
            {
                if (Impl_1901_IsPrimeNumber(i))
                {
                    prime1 = i;
                    break;
                }
            }

            int prime2 = prime1;
            for(int i=n+1; i <= n+(n-prime1); ++i)
            {
                if (Impl_1901_IsPrimeNumber(i))
                {
                    prime2 = i;
                    break;
                }
            }

            if (prime2 == prime1)
                Console.WriteLine(prime1);
            else if (prime2-n == n-prime1)
                Console.WriteLine("{0} {1}", prime1, prime2);
        }
        static void Impl_1901(int cnt, int[] src)
        {
            System.Diagnostics.Debug.Assert(cnt == src.Length);
            for(int i=0; i<cnt; ++i)
                Impl_1901_NearestPrimeNumber(src[i]);
        }
        static void _1901()
        {
            Impl_1901(2, new int[] {8, 15});
        }



        //--------------------------------------------------
        // 2814 이진수
        //--------------------------------------------------
        static void Impl_2814(string binary)
        {
            System.Diagnostics.Debug.Assert(binary.Length <= 30);
            
            //int rt_easy = Convert.ToInt32(binary, 2);
            //Console.WriteLine(rt_easy);

            int rt = 0;
            for(int i=binary.Length-1; i>=0; --i)
            {
                if (binary[i] == '1')
                    rt += (1 << i);
            }
            Console.WriteLine(rt);
        }
        static void _2814()
        {
            Impl_2814("10101");
        }



        //--------------------------------------------------
        // 2815 10진수를 2진수로
        //--------------------------------------------------
        static void Impl_2815(int n)
        {
            System.Collections.Generic.List<char> list = new System.Collections.Generic.List<char>();
            while(n != 0)
            {
                if ((n & 0x1) == 1)
                    list.Add('1');
                else
                    list.Add('0');

                n >>= 1;
            }
            list.Reverse();
            char[] arr = list.ToArray();
            for(int i=0; i<arr.Length; ++i)
                Console.Write(arr[i]);
            Console.WriteLine();
        }
        static void _2815()
        {
            Impl_2815(26);
            Impl_2815(21);
        }



        //--------------------------------------------------
        // 1274 2진수를 10진수로...
        //--------------------------------------------------
        static void Impl_1274(string binary)
        {
            System.Diagnostics.Debug.Assert(binary.Length == 8);

            char msb = binary[0];
            int dec = 0;
            if (msb == '0')
            {
                for(int i=binary.Length - 1; i>0; --i)
                {
                    if (binary[i] == '1')
                    {
                        //dec += (int)Math.Pow(2, 7-i);
                        dec += 1 << (7-i); 
                    }
                }
            }
            else
            {
                //1의 보수 : 1's complement
                char[] data = binary.ToCharArray();
                for(int i=1; i<binary.Length; ++i)
                    data[i] = data[i] == '0' ? '1' : '0';

                //2의 보수 : 2's complement
                for(int i=binary.Length-1; i>0; --i)
                {
                    if(data[i] == '0')
                    {
                        data[i] = '1';
                        break;
                    }
                    else
                        data[i] = '0';
                }


                for (int i = binary.Length - 1; i > 0; --i)
                {
                    if (data[i] == '1')
                    {
                        //dec += (int)Math.Pow(2, 7-i);
                        dec += 1 << (7 - i);
                    }
                }
                dec *= -1;
            }

            Console.WriteLine("{0} : {1}", binary, dec);

        }
        static void _1274()
        {
            Impl_1274("00000101");  //5
            Impl_1274("10011000");  //-104
        }



        //--------------------------------------------------
        // 1534 10진수를 2 8 16진수로
        //--------------------------------------------------
        static Stack<int> Impl_1534_stack(int dec, int fromBase)
        {
            Stack<int> stack = new Stack<int>();
            while (dec > 0)
            {
                int s = dec / fromBase;
                int r = dec - s * fromBase; // dec % fromBase

                stack.Push(r);
                dec = s;
            }

            return stack;
        }
        static void Impl_1534_2or8(int dec, int fromBase)
        {
            Stack<int> stack = Impl_1534_stack(dec, fromBase);

            Console.Write("{0,8} : ", dec);
            while(stack.Count > 0)
                Console.Write(stack.Pop());

            Console.WriteLine();
        }
        static void Impl_1534_16(int dec)
        {
            Stack<int> stack = Impl_1534_stack(dec, 16);
            char[] chs = {'A', 'B', 'C', 'D', 'E', 'F'};

            Console.Write("{0,8} : ", dec);
            while (stack.Count > 0)
            {
                int n = stack.Pop();
                if (n < 10)
                    Console.Write(n);
                else
                    Console.Write(chs[n-10]);
            }
            Console.WriteLine();
        }
        static void Impl_1534(int dec, int fromBase)
        {
            if (16 == fromBase)
            {
                Impl_1534_16(dec);
            }
            else
            {
                System.Diagnostics.Debug.Assert(fromBase == 2 || fromBase == 8);
                Impl_1534_2or8(dec, fromBase);
            }
        }
        static void _1534()
        {
            Impl_1534(27, 2);
            Impl_1534(27, 8);
            Impl_1534(27, 16);
        }



        //--------------------------------------------------
        // 3106 진법 변환
        //--------------------------------------------------
        static void Impl_3106(int fromBase, int num, int toBase)
        {
            System.Diagnostics.Debug.Assert(fromBase >= 2);
            System.Diagnostics.Debug.Assert(toBase <= 36);


            Stack<int> stack = new Stack<int>();
            while (num > 0)
            {
                int s = num / toBase;
                int r = num - s * toBase; // num % fromBase

                stack.Push(r);
                num = s;
            }
            
            while (stack.Count > 0)
            {
                int n = stack.Pop();
                if (n < 10)
                    Console.Write(n);
                else
                    Console.Write((char)('A' + (char)(n-10)));
            }
            Console.WriteLine();
        }
        static int Impl_3106_2nd(int fromBase, string num)
        {
            int rt = 0;
            int pow = 1;    // Math.Pow(fromBase, 0);
            for(int i=num.Length-1; i>=0; --i)
            {
                char c = num[i];

                int n = 0;
                if (char.IsDigit(c))
                {
                    n = (int)(c - '0');
                }
                else
                {
                    System.Diagnostics.Debug.Assert(char.IsUpper(c));
                    n = (int)(c-'A') + 10;
                }

                rt += n * pow;
                pow *= fromBase;
            }

            System.Diagnostics.Debug.Assert(rt == Convert.ToInt32(num, fromBase));
            return rt;
        }
        static void _3106()
        {
            string[] inputs =
            {
                "2 10110 10",
                "10 2543 16",
                "16 ABC 8",
                "0",
            };

            for(int i=0; i<inputs.Length; ++i)
            {
                string input = inputs[i];
                if ("0" == input)
                    break;
                string[] words = input.Split();
                int fromBase = Convert.ToInt32(words[0]);

                int num = Convert.ToInt32(words[1], fromBase);
                //int num = Impl_3106_2nd(fromBase, words[1]);

                int toBase = Convert.ToInt32(words[2]);

                Impl_3106(fromBase, num, toBase);
            }
        }



        //--------------------------------------------------
        // 1097 앞뒤 같은 제곱
        //--------------------------------------------------
        static bool Impl_1097_IsSameFrontAndBack(int n, int theBase)
        {
            List<int> list = new List<int>();
            while(n > 0)
            {
                int s = n / theBase;
                int r = n - s*theBase;
                list.Add(r);
                n = s;
            }
            
            int mid = list.Count / 2;
            for(int i=0; i<mid; ++i)
            {
                if (list[i] != list[list.Count - i - 1])
                    return false;
            }

            return true;
        }

        static void Impl_1097_Print(int n, int theBase)
        {
            Stack<int> stack = new Stack<int>();
            while (n > 0)
            {
                int s = n / theBase;
                int r = n - s * theBase;
                stack.Push(r);
                n = s;
            }

            while(stack.Count > 0)
            {
                int digit = stack.Pop();
                if (digit < 10)
                    Console.Write(digit);
                else
                    Console.Write((char)('A' + digit - 10));
            }
        }
        static void Impl_1097(int theBase)
        {
            System.Diagnostics.Debug.Assert(theBase >= 2 && theBase <= 20);
            for(int i=1; i<=300; ++i)
            {
                if (Impl_1097_IsSameFrontAndBack(i*i, theBase))
                {
                    Impl_1097_Print(i, theBase);
                    Console.Write(" ");
                    Impl_1097_Print(i*i, theBase);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
        static void _1097()
        {
            Impl_1097(10);
        }



        //--------------------------------------------------
        // 2604 그릇
        //--------------------------------------------------
        static void Impl_2604(string s)
        {
            System.Diagnostics.Debug.Assert(s.Length >= 1);
            int height = 10;

            char prev = s[0];
            for(int i=1; i<s.Length; ++i)
            {
                if (prev == s[i])
                    height += 5;
                else
                    height += 10;

                prev = s[i];
            }
            Console.WriteLine(height);
        }
        static void _2604()
        {
            Impl_2604("((((");
            Impl_2604("()()()))(");
        }



        //--------------------------------------------------
        // 2514 문자열 찾기
        //--------------------------------------------------
        static void Impl_2514(string s)
        {
            int kCount = 0;
            int iCount = 0;
            for (int i=0; i<s.Length-2; ++i)
            {
                if (s[i+1] == 'O' && s[i+2] == 'I')
                {
                    if(s[i] == 'K')
                    {
                        ++kCount;
                        ++i;    // to skip 'O'
                    }
                    else if(s[i] == 'I')
                    {
                        ++iCount;
                        ++i;    // to skip 'O'
                    }
                }
            }
            Console.WriteLine(kCount);
            Console.WriteLine(iCount);
        }
        static void _2514()
        {
            Impl_2514("KOIOIOI");
            Console.WriteLine();
            Impl_2514("KORKDIOIDXHKOIOIOIOPKOI");
            Console.WriteLine();
        }



        //--------------------------------------------------
        // 2518 문자열변환
        //--------------------------------------------------
        static void Impl_2518(string input)
        {
            string[] lines = input.Split(new char[] {'\n'});

            int changeCount = Convert.ToInt32(lines[0]);
            char[,] chageList = new char[changeCount, 2];

            for(int i=0; i<changeCount; ++i)
            {
                chageList[i, 0] = lines[i + 1][0];
                chageList[i, 1] = lines[i + 1][2];
            }

            int characterCount = Convert.ToInt32(lines[changeCount+1]);
            char[] output = new char[characterCount];
            for(int i=0; i<characterCount; ++i)
            {
                char c = lines[i+changeCount+2][0];

                for(int j=0; j<changeCount; ++j)
                {
                    if (c == chageList[j,0])
                    {
                        c = chageList[j,1];
                        break;
                    }
                }
                output[i] = c;
            }

            Console.WriteLine(new string(output));
        }
        static void _2518()
        {
            //aBC5144aba
            string multiLines = @"3
A a
0 5
5 4
10
A
B
C
0
1
4
5
a
b
A";
            Impl_2518(multiLines);
        }



        //--------------------------------------------------
        // 1239 비밀편지
        //--------------------------------------------------
        static int Impl_1239_dif(string lhs, string rhs)
        {
            System.Diagnostics.Debug.Assert(6 == lhs.Length);
            System.Diagnostics.Debug.Assert(6 == rhs.Length);

            int cnt = 0;
            for(int i=0; i<6; ++i)
                if(lhs[i] != rhs[i])
                    ++cnt;

            return cnt;
        }
        static void Impl_1239(int n, string msg)
        {
            string[] codes =
            {
                "000000",//A
                "001111",//B
                "010011",//C
                "011100",//D
                "100110",//E
                "101001",//F
                "110101",//G
                "111010",//H
            };

            char[] output = new char[n];
            System.Diagnostics.Debug.Assert(6*n == msg.Length);
            for (int i=0; i<n; ++i)
            {
                string s = msg.Substring(i*6, 6);

                int index = -1;
                for(int j=0; j<codes.Length; ++j)
                {
                    int difCnt = Impl_1239_dif(s, codes[j]);
                    if (1 >= difCnt)
                    {
                        index = j;
                        break;
                    }
                }
                if (index == -1)
                {
                    Console.WriteLine(i + 1);//1 base
                    return;
                }

                output[i] = (char)('A' + (char)index);
            }
            Console.WriteLine(new string(output));
        }
        static void _1239()
        {
            Impl_1239(3, "001111000000011100");
            Impl_1239(5, "011111000000111111000000111111");
        }



        //--------------------------------------------------
        // 1620 전화번호 속의 암호
        //--------------------------------------------------
        static void Impl_1620(string number, int add, int index)
        {
            string[] words = number.Split(new char[] {'-'});
            if (index > words.Length)
            {
                Console.WriteLine("INPUT ERROR");
                return;
            }
            for(int i=0; i<words.Length; ++i)
            {
                if (words[i].Length > 4)
                {
                    Console.WriteLine("INPUT ERROR");
                    return;
                }
                foreach(char c in words[i])
                {
                    if (char.IsDigit(c) == false)
                    {
                        Console.WriteLine("INPUT ERROR");
                        return;
                    }
                }
            }

            add %= 10;
            string word = words[index-1];
            for(int i=0; i<(4-word.Length); ++i)
                Console.Write(add);
            for(int i=0; i<word.Length; ++i)
            {
                int n = (int)(word[i]-'0');
                Console.Write((n+add) % 10);
            }
            Console.WriteLine();   
        }
        static void _1620()
        {
            Impl_1620("111-2222-3412-5432", 2, 4);
            Impl_1620("11111-22-33", 5, 1);
        }



        //--------------------------------------------------
        // 1516 단어 세기
        //--------------------------------------------------
        static void Impl_1516(string line)
        {
            string[] words = line.Split();
            Array.Sort(words);

            int cnt = 1;
            string curr = words[0];
            for(int i=1; i<words.Length; ++i)
            {
                if(curr != words[i])
                {
                    Console.WriteLine("{0} : {1}", curr, cnt);
                    curr = words[i];
                    cnt = 1;
                }
                else
                    ++cnt;
            }
            Console.WriteLine("{0} : {1}", curr, cnt);
        }
        static void _1516()
        {
            string[] inputs =
            {
                "I AM DOG DOG DOG DOG A AM I",
                "I AM OLYMPIAD JUNGOL JUNGOL OLYMPIAD",
                "END"
            };

            foreach(string line in inputs)
            {
                if (line == "END")
                    break;

                Impl_1516(line);
                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 1535 단어집합(하)
        //--------------------------------------------------
        static void Impl_1535(HashSet<string> lookup, List<string> output, string input)
        {
            string[] words = input.Split();
            for(int i=0; i<words.Length; ++i)
            {
                if (false == lookup.Contains(words[i]))
                {
                    lookup.Add(words[i]);
                    output.Add(words[i]);
                }
            }

            Console.WriteLine(string.Join(" ", output.ToArray()));
        }
        static void _1535()
        {
            string[] inputs =
            {
                "I am a boy",
                "I am a girl",
                "END"
            };

            HashSet<string> lookup = new HashSet<string>();
            List<string> output = new List<string>();
            foreach (string line in inputs)
            {
                if (line == "END")
                    break;

                Impl_1535(lookup, output, line);
                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 1566 소수문자열
        //--------------------------------------------------
        static bool Impl_1566_IsPrimeNumber(int n)
        {
            //0,1 is not a prime number
            if (n < 2)
                return false;

            for(int i=2; i*i<=n; ++i)
                if ((n % i) == 0)
                    return false;

            return true;
        }
        static void Impl_1566(string s)
        {
            Dictionary<char,int> lookup = new Dictionary<char, int>();
            foreach(char c in s)
            {
                if (lookup.ContainsKey(c))
                    ++lookup[c];
                else
                    lookup.Add(c, 1);
            }

            List<char> output = new List<char>();
            foreach(char c in lookup.Keys)
            {
                int n = lookup[c];
                if (Impl_1566_IsPrimeNumber(n))
                    output.Add(c);
            }

            if (output.Count > 0)
                Console.WriteLine(new string(output.ToArray()));
            else
                Console.WriteLine("NONE");
        }
        static void _1566()
        {
            Impl_1566("AABAAB");
            Impl_1566("AABAABAABAABAABAABAABAAB");
            Impl_1566("AABAABAACCCBAABAABAABBADDB");
        }



        //--------------------------------------------------
        // 2085 윤년
        //--------------------------------------------------
        static void Impl_2085(int currYear, int birthYear)
        {
            //@{ 이해하기 쉬운 반복문 이용
            //int cnt = 0;
            //int from = (1 + (birthYear / 4)) * 4;
            //
            //for(int i=from; i<=currYear; i+=4)
            //{
            //    if (i % 100 == 0 && i%400 != 0)
            //        continue;
            //    ++cnt;
            //}
            //
            //Console.WriteLine(cnt);
            //@}

            //@{ 큰 수의 경우 좀더 빠른 방법이 필요함
            int _1 = currYear / 4 - birthYear / 4;
            int _2 = currYear / 100 - birthYear / 100;
            int _3 = currYear / 400 - birthYear / 400;
            Console.WriteLine(_1-_2+_3);
            //@}
        }
        static void _2085()
        {
            Impl_2085(2004, 1980);
            Impl_2085(2100000000, 1);
            Impl_2085(2100000000, 4);
        }



        //--------------------------------------------------
        // 1031 빙고
        //--------------------------------------------------
        static KeyValuePair<int,int> Impl_1031_FindRowColumn(int[,] map, int num)
        {
            for(int i=0; i<map.GetLength(0); ++i)
            {
                for(int j=0; j<map.GetLength(1); ++j)
                {
                    if (num == map[i,j])
                    {
                        return new KeyValuePair<int,int>(i,j);
                    }
                }
            }

            System.Diagnostics.Debug.Assert(false, "impossible");
            return new KeyValuePair<int, int>(-1,-1);
        }

        static int Impl_1031_lineCount(bool[,] map, int i, int j)
        {
            System.Diagnostics.Debug.Assert(map[i,j] == true);

            int rt = 0;
            int cnt = 0;
            // →
            for(int k=0; k<5; ++k)
            {
                if(false == map[k,j])
                    break;
                ++cnt;
            }
            if(cnt == 5)
                ++rt;
            // ↓
            cnt = 0;
            for (int k = 0; k < 5; ++k)
            {
                if (false == map[i, k])
                    break;
                ++cnt;
            }
            if (cnt == 5)
                ++rt;
            // ↘
            if (i==j)
            {
                cnt = 0;
                for (int k = 0; k < 5; ++k)
                {
                    if (false == map[k, k])
                        break;
                    ++cnt;
                }
                if (cnt == 5)
                    ++rt;
            }
            // ↙
            if (i + j == 4)
            {
                cnt = 0;
                for (int k = 0; k < 5; ++k)
                {
                    if (false == map[k, 4-k])
                        break;
                    ++cnt;
                }
                if (cnt == 5)
                    ++rt;
            }

            return rt;
        }
        static void Impl_1031(string input)
        {
            string[] lines = input.Split(new char[] {'\n' });

            System.Diagnostics.Debug.Assert(lines.Length == 10);

            int[,] nmap = new int[5,5];
            bool[,] bmap = new bool[5,5];

            for(int i=0; i<5; ++i)
            {
                string line = lines[i].Trim();
                string[] words = line.Split();
                System.Diagnostics.Debug.Assert(words.Length == 5);

                nmap[i, 0] = Convert.ToInt32(words[0]);
                nmap[i, 1] = Convert.ToInt32(words[1]);
                nmap[i, 2] = Convert.ToInt32(words[2]);
                nmap[i, 3] = Convert.ToInt32(words[3]);
                nmap[i, 4] = Convert.ToInt32(words[4]);
            }

            int lineCount = 0;
            for (int i = 0; i < 5; ++i)
            {
                string line = lines[i+5].Trim();
                string[] words = line.Split();
                System.Diagnostics.Debug.Assert(words.Length == 5);
                
                for(int j=0; j<5; ++j)
                {
                    int n = Convert.ToInt32(words[j]);
                    KeyValuePair<int, int> rowCol = Impl_1031_FindRowColumn(nmap, n);
                    bmap[rowCol.Key, rowCol.Value] = true;
                    lineCount += Impl_1031_lineCount(bmap, rowCol.Key, rowCol.Value);
                    if (lineCount >= 3)
                    {
                        Console.WriteLine(i*5 + j + 1);
                        return;
                    }
                }
            }

        }
        static void _1031()
        {
            string input = @"11 12 2 24 10
16 1 13 3 25
6 20 5 21 17
19 4 8 14 9
22 15 7 23 18
5 10 7 16 2
4 22 8 17 13
3 18 1 6 25
12 19 23 14 21
11 24 9 20 15";
            Impl_1031(input);
        }



        //--------------------------------------------------
        // 1112 줄자접기
        //--------------------------------------------------
        static void Impl_1112(int len, int[,] RBY)
        {
            int left = 0;
            int right = len*4;
            for (int i = 0; i < 3; ++i)
            {
                RBY[i, 0] *= 4;
                RBY[i, 1] *= 4;
            }
            for (int i=0; i<3; ++i)
            {
                int min = Math.Min(RBY[i, 0], RBY[i, 1]);
                int max = Math.Max(RBY[i, 0], RBY[i, 1]);

                int mid = (min + max) / 2;
                int halfLen = (right-left) / 2;
                if (mid < halfLen)
                {
                    left = mid;

                    for (int j = i + 1; j < 3; ++j)
                    {
                        if (RBY[j, 0] < left)
                            RBY[j, 0] = left + (left - RBY[j, 0]);

                        if (RBY[j, 1] < left)
                            RBY[j, 1] = left + (left - RBY[j, 1]);
                    }
                }
                else
                {
                    right = mid;
                }
            }

            Console.WriteLine("{0:f1}", (right-left) / 4f);
        }
        static void _1112()
        {
            Impl_1112(10, new int[3, 2] { { 2, 7 }, { 5, 4 }, { 10, 3 } });
        }



        //--------------------------------------------------
        // 1311 카드게임
        //--------------------------------------------------
        struct Impl_1311_Card
        {
            public char color;
            public int num;
            public Impl_1311_Card(char C, int N)
            {
                color = C;
                num = N;
            }
        }

        static bool Impl_1311_IsFlush(Impl_1311_Card[] cards)
        {
            char color = cards[0].color;
            for(int i=1; i<cards.Length; ++i)
                if (color != cards[i].color)
                    return false;

            return true;
        }
        static int[] Impl_1311_Counting(Impl_1311_Card[] cards)
        {
            int[] cnts = new int[10]; //the index of '0' is not used

            foreach(Impl_1311_Card c in cards)
                ++cnts[c.num];

            return cnts;
        }
        static bool Impl_1311_IsStraight(Impl_1311_Card[] cards)
        {
            for (int i = 1; i < cards.Length; ++i)
                if (cards[i - 1].num + 1 != cards[i].num)
                    return false;

            return true;
        }

        static int Impl_1311_FindCount(int[] cnts, int theCount, int skipIndex=0)
        {
            for (int i = 1; i < cnts.Length; ++i)
            {
                if (skipIndex != i && theCount == cnts[i])
                    return i;
            }
            return 0;
        }
        static int Impl_1311_IsFourCards(int[] cnts)
        {
            return Impl_1311_FindCount(cnts, 4);
        }

        static int Impl_1311_HasThreeCards(int[] cnts)
        {
            return Impl_1311_FindCount(cnts, 3);
        }

        static int Impl_1311_HasTwoCards(int[] cnts, int skipIndex=0)
        {
            return Impl_1311_FindCount(cnts, 2, skipIndex);
        }

        static void Impl_1311(Impl_1311_Card[] cards)
        {
            System.Diagnostics.Debug.Assert(cards.Length == 5);
            Array.Sort(cards, (Impl_1311_Card l, Impl_1311_Card r) =>
            {
                return l.num - r.num;
            });

            int rt = 0;
            if (Impl_1311_IsFlush(cards))
            {
                if(Impl_1311_IsStraight(cards))
                    rt = cards[4].num + 900;
                else
                    rt = cards[4].num + 600;
                Console.WriteLine(rt);
                return;
            }

            if (Impl_1311_IsStraight(cards))
            {
                rt = cards[4].num + 500;
                Console.WriteLine(rt);
                return;
            }

            int[] cnts = Impl_1311_Counting(cards);
            int chk = Impl_1311_IsFourCards(cnts);
            if (0 != chk)
            {
                rt = chk + 800;
                Console.WriteLine(rt);
                return;
            }

            chk = Impl_1311_HasThreeCards(cnts);
            if (chk != 0)
            {
                int chk2 = Impl_1311_HasTwoCards(cnts);
                if (chk2 != 0)
                    rt = chk * 10 + chk2 + 700;
                else
                    rt = chk + 400;
                Console.WriteLine(rt);
                return;
            }

            chk = Impl_1311_HasTwoCards(cnts);
            if (0!=chk)
            {
                int chk2 = Impl_1311_HasTwoCards(cnts, chk);
                if (chk2 != 0)
                    rt = chk2 * 10 + chk + 300;
                else
                    rt = chk + 200;
            }
            else
            {
                rt = cards[4].num + 100;
            }

            Console.WriteLine(rt);
            return;
        }
        static void _1311()
        {
            Impl_1311_Card[] cards =
            {
                new Impl_1311_Card('B', 3 ),
                new Impl_1311_Card('B', 7 ),
                new Impl_1311_Card('R', 1 ),
                new Impl_1311_Card('B', 2 ),
                new Impl_1311_Card('Y', 7 ),
            };
            Impl_1311(cards);
        }



        //--------------------------------------------------
        // 1438 색종이(초)
        //--------------------------------------------------
        static void Impl_1438(int cnt, params int[] points)
        {
            System.Diagnostics.Debug.Assert(points.Length == cnt*2);

            bool [,] paper = new bool[100,100];
            int area = 0;
            for(int i=0; i<cnt; ++i)
            {
                int x = points[i * 2 + 0];
                int y = points[i * 2 + 1];

                for(int r=y; r<y+10; ++r)
                {
                    for(int c=x; c<x+10; ++c)
                    {
                        if (false == paper[r,c])
                        {
                            paper[r,c] = true;
                            ++area;
                        }
                    }
                }
            }

            Console.WriteLine(area);
        }
        static void _1438()
        {
            //Impl_1438(3, new int[] { 3, 7, 15, 7, 5, 2 });
            Impl_1438(3, 3, 7, 15, 7, 5, 2 );
        }



        //--------------------------------------------------
        // 1733 오목
        //--------------------------------------------------
        static void Impl_1733(string input)
        {
            int[,] map = new int[19,19];

            string[] lines = input.Split(new char[] {'\n'});
            System.Diagnostics.Debug.Assert(lines.Length == 19);
            for (int i=0; i<lines.Length; ++i)
            {
                string line = lines[i].Trim();
                string[] stones = line.Split();
                System.Diagnostics.Debug.Assert(stones.Length == 19);
                for(int j=0; j<stones.Length; ++j)
                    map[i,j] = Convert.ToInt32(stones[j]);
            }

            bool[,] flags = new bool[19,19];

            for(int i=0; i<19; ++i)
            {
                for(int j=0; j<19; ++j)
                {
                    if (0 == map[i,j])
                        continue;

                    int n = map[i,j];
                    int cnt = 0;

                    // →
                    cnt = 1;
                    for(int jj = j+1; jj<19; ++jj)
                    {
                        if (n != map[i,jj])
                            break;
                        ++cnt;
                        if (cnt >= 6)
                            break;
                    }
                    if (cnt == 5)
                    {
                        Console.WriteLine(n);
                        Console.WriteLine("{0} {1}", i + 1, j + 1);
                        return;
                    }

                    // ↓
                    cnt = 1;
                    for (int ii = i + 1; ii < 19; ++ii)
                    {
                        if (n != map[ii, j])
                            break;
                        ++cnt;
                        if (cnt >= 6)
                            break;
                    }
                    if (cnt == 5)
                    {
                        Console.WriteLine(n);
                        Console.WriteLine("{0} {1}", i + 1, j + 1);
                        return;
                    }

                    // ↘
                    cnt = 1;
                    for (int ii = i + 1, jj = j+1; ii < 19 && jj < 19; ++ii, ++jj)
                    {
                        if (n != map[ii, jj])
                            break;
                        ++cnt;
                        if (cnt >= 6)
                            break;
                    }
                    if (cnt == 5)
                    {
                        Console.WriteLine(n);
                        Console.WriteLine("{0} {1}", i + 1, j + 1);
                        return;
                    }
                }
            }
        }
        static void _1733()
        {
            string input = @"0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 1 2 0 0 2 2 2 1 0 0 0 0 0 0 0 0 0 0
                             0 0 1 2 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0
                             0 0 0 1 2 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 1 2 2 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 1 1 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 2 1 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                             0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0";
            Impl_1733(input);
        }



        //--------------------------------------------------
        // 1997 떡 먹는 호랑이
        //--------------------------------------------------
        static void Impl_1997(int D, int K)
        {
            System.Diagnostics.Debug.Assert(D >= 3);
            System.Diagnostics.Debug.Assert(D <= 30);
            int[] day1 = { 1, 0 };
            int[] day2 = { 0, 1 };
            int[] day3 = { 0, 0 };

            for(int i=3; i<=D; ++i)
            {
                day3[0] = day2[0] + day1[0];
                day3[1] = day2[1] + day1[1];

                day1[0] = day2[0];
                day1[1] = day2[1];

                day2[0] = day3[0];
                day2[1] = day3[1];
            }

            int a = day3[0];
            int b = day3[1];


            int TO = K / b;
            for(int i=1; i<=TO; ++i)
            {
                int rem = K - b*i;
                if (rem % a == 0)
                {
                    int A = rem / a;
                    int B = i;
                    if (B >= A)
                    {
                        Console.WriteLine(A);
                        Console.WriteLine(B);
                        return;
                    }
                }
            }

            Console.WriteLine("error -----> {0,2} : {1,6} {2,6}", D, a, b);
        }
        static void _1997()
        {
            Impl_1997(6, 41);
            Impl_1997(7, 218);
        }



        //--------------------------------------------------
        // 2460 나는 학급회장이다.(투표)
        //--------------------------------------------------
        static void Impl_2460(string input)
        {
            string[] lines = input.Split(new char[] {'\n' });
            int n = Convert.ToInt32(lines[0].Trim());

            int[] scores = new int[3];
            int[] counts3 = new int[3];
            int[] counts2 = new int[3];

            System.Diagnostics.Debug.Assert(n == lines.Length - 1);
            for(int i=1; i<=n; ++i)
            {
                string line = lines[i].Trim();
                string[] words = line.Split();

                for(int j=0; j<3; ++j)
                {
                    int score = Convert.ToInt32(words[j]);
                    scores[j] += score;
                    if (score == 3)
                        ++counts3[j];
                    else if (score == 2)
                        ++counts2[j];
                }
            }
            
            int index = 0;
            int highScore = int.MinValue;
            int count3 = 0;
            int count2 = 0;

            for(int i=0; i<3; ++i)
            {
                bool found = false;
                if(highScore < scores[i])
                    found = true;
                else if (highScore == scores[i])
                {
                    if (count3 < counts3[i])
                        found = true;
                    else if (count3 == counts3[i])
                    {
                        if (count2 < counts2[i])
                            found = true;
                        else if (count2 == counts2[i])
                            index = 0;
                    }
                }
                if (found)
                {
                    index = i + 1;
                    highScore = scores[i];
                    count3 = counts3[i];
                    count2 = counts2[i];
                }
            }

            Console.WriteLine("{0} {1}", index, highScore);
        }
        static void _2460()
        {
            string input = @"6
3 1 2
2 3 1
3 1 2
1 2 3
3 1 2
1 2 3";
            Impl_2460(input);

            input = @"6
1 2 3
3 1 2
2 3 1
1 2 3
3 1 2
2 3 1";
            Impl_2460(input);
        }



        //--------------------------------------------------
        // 2101 연속부분최대곱
        //
        // 풀이
        //  - 모든 경우의 수를 나열
        //  - 반복 시 오른쪽만을 고려
        //--------------------------------------------------
        static void Impl_2101(int n, double[] reals)
        {
            double high = reals[0];

            for (int i = 0; i < n; ++i)
            {
                for (int j=i; j<n; ++j)
                {
                    double curr = 1d;
                    for (int k = i; k <= j; ++k)
                    {
                        curr *= reals[k];
                    }
                    if (curr > high)
                        high = curr;
                }
            }


            Console.WriteLine("{0:F3}", high);
        }
        static void _2101()
        {
            double [] inputs = new double[8]
            {
                1.1,
                0.7,
                1.3,
                0.9,
                1.4,
                0.8,
                0.7,
                1.4,
            };
            Impl_2101(8, inputs);
        }



        //--------------------------------------------------
        // 1671 색종이(중)
        //
        // 풀이
        //  - 정수임을 이용하여 색칠(레스터라이제이션) 후
        //  - 공백과 만나는 점들의 수가 둘레임
        //  - 단, 꼭짓점은 두면이 공백과 만날 수 있음에 주의하자
        //--------------------------------------------------
        static void Impl_1671(int cnt, params int[] points)
        {
            System.Diagnostics.Debug.Assert(points.Length == cnt * 2);

            bool[,] paper = new bool[100, 100];
            for (int i = 0; i < cnt; ++i)
            {
                int x = points[i * 2 + 0];
                int y = points[i * 2 + 1];

                for (int r = y; r < y + 10; ++r)
                    for (int c = x; c < x + 10; ++c)
                        paper[r, c] = true;
            }

            int rt = 0;
            for(int i=0; i<100; ++i)
            {
                for(int j=0; j<100; ++j)
                {
                    if (false == paper[i,j])
                        continue;

                    //위아래
                    if (i == 0 || i == 99 || !paper[i-1,j] || !paper[i+1,j])
                        ++rt;

                    //양옆
                    if (j == 0 || j == 99 || !paper[i, j-1] || !paper[i, j+1])
                        ++rt;
                }
            }

            Console.WriteLine(rt);
        }
        static void _1671()
        {
            Impl_1671(4, 3, 7, 5, 2, 15, 7, 13, 14);
        }



        //--------------------------------------------------
        // 1124 색종이(고)
        //
        // 풀이 설명
        //  - 각 색종이의 교차 영역을 포함하는 직사각형 중에 찾자
        //--------------------------------------------------

        //WindowsBase.dll 을 참조에 추가고 System.Windows.Rect 를 사용하면 편함
        struct Rect
        {
            public int X1
            {
                get; set;
            }
            public int Y1
            {
                get; set;
            }
            public int X2
            {
                get; set;
            }
            public int Y2
            {
                get; set;
            }

            public bool IsValid
            {
                get
                {
                    return (X1 <= X2) && (Y1 <= Y2);
                }
            }
        }
        static Rect Impl_1124_IntersectRect(int x1, int y1, int x2, int y2, int SIZE = 10)
        {
            Rect rc = new Rect();
            
            rc.X1 = Math.Min(x1, x2);
            rc.Y1 = Math.Max(x1, x2);
            rc.X2 = Math.Min(y1, y2) + SIZE;
            rc.Y2 = Math.Max(y1, y2) + SIZE;

            return rc;
        }

        static int Impl_1124_MaxArea(bool[,] paper, Rect rc, int SIZE = 10)
        {
            int area = SIZE*SIZE;

            int width = 0;
            int height = 0;

            //
            // y
            // ^
            // |
            // |
            // -------> x
            //

            //1. virtical
            width = rc.X2 - rc.X1;
            int minY = 0;
            int maxY = paper.GetLength(0);
            for(int x=rc.X1; x<rc.X2; ++x)
            {
                //down
                for(int y=rc.Y1-1; y>=0; --y)
                {
                    if(paper[x,y] == false)
                    {
                        minY = Math.Max(minY, y);
                        break;
                    }
                }
                //up
                for (int y = rc.Y2; y < paper.GetLength(0); ++y)
                {
                    if (paper[x, y] == false)
                    {
                        maxY = Math.Min(maxY, y);
                        break;
                    }
                }
            }
            area = Math.Max(width * (maxY - minY), area);

            //2. horizontal
            height = rc.Y2 - rc.Y1;
            int minX = 0;
            int maxX = paper.GetLength(1);
            for (int y=rc.Y1; y<rc.Y2; ++y)
            {
                //left
                for (int x = rc.X1 - 1; x >= 0; --x)
                {
                    if (paper[x, y] == false)
                    {
                        minX = Math.Max(minX, x);
                        break;
                    }
                }

                //right
                for (int x = rc.X2; x < paper.GetLength(1); ++x)
                {
                    if (paper[x, y] == false)
                    {
                        maxX = Math.Min(maxX, x);
                        break;
                    }
                }
            }
            area = Math.Max(height * (maxX - minX), area);

            return area;
        }

        static void Impl_1124(int cnt, params int[] points)
        {
            System.Diagnostics.Debug.Assert(points.Length == cnt * 2);

            bool[,] paper = new bool[100, 100];
            for (int i = 0; i < cnt; ++i)
            {
                int x = points[i * 2 + 0];
                int y = points[i * 2 + 1];

                for (int r = y; r < y + 10; ++r)
                    for (int c = x; c < x + 10; ++c)
                        paper[r, c] = true;
            }
            
            int area = int.MinValue;
            for(int i=0; i<cnt; ++i)
            {
                int x1 = points[i * 2 + 0];
                int y1 = points[i * 2 + 1];

                for (int j=0; j<cnt; ++j)
                {
                    if (i == j)
                        continue;

                    int y2 = points[j * 2 + 1];
                    int x2 = points[j * 2 + 0];

                    Rect rc = Impl_1124_IntersectRect(x1, y1, x2, y2);

                    if (false == rc.IsValid)
                        continue;

                    area = Math.Max(area, Impl_1124_MaxArea(paper, rc));
                }
            }

            Console.WriteLine(area);
        }
        static void _1124()
        {
            Impl_1124(3, 3, 7, 15, 7, 5, 2);
        }



        //--------------------------------------------------
        // 1761 숫자 야구
        // 
        // 풀이
        //  - 모든 경우의 배열을 나열해보자 패턴은 아래와 같다.
        //  - 아래의 84개의 3자리 숫자가 있고
        //  - 각 숫자마다 6가지 나열의 경우가 있다
        //  - 총 84 * 6 = 504 개의 경우가 존재
        //
        // 123 134 145 156 167 178 189
        // 124 135 146 157 168 179
        // 125 136 147 158 169
        // 126 137 148 159
        // 127 138 149
        // 128 139
        // 129 
        //
        // 234 245 256 267 278 289
        // 235 246 257 268 279 
        // 236 247 258 269 
        // 237 248 259 
        // 238 249 
        // 239 
        //
        // 345 356 367 378 389 
        // 346 357 368 379 
        // 347 358 369 
        // 348 359 
        // 349 
        //
        // 456 467 478 489 
        // 457 468 479 
        // 458 469 
        // 459 
        //
        // 567 578 589 
        // 568 579 
        // 569 
        //
        // 678 689 
        // 679 
        //
        // 789
        //--------------------------------------------------
        struct stImpl_1761_QNA
        {
            int n1;
            int n2;
            int n3;
            int strike;
            int ball;

            public stImpl_1761_QNA(string str)
            {
                n1 = 0;
                n2 = 0;
                n3 = 0;
                strike = 0;
                ball = 0;
                Build(str);
            }
            public void Build(string str)
            {
                string[] words = str.Split();
                System.Diagnostics.Debug.Assert(words[0].Length == 3);
                System.Diagnostics.Debug.Assert(words[1].Length == 1);
                System.Diagnostics.Debug.Assert(words[2].Length == 1);

                n1 = Convert.ToInt32(words[0][0].ToString());
                n2 = Convert.ToInt32(words[0][1].ToString());
                n3 = Convert.ToInt32(words[0][2].ToString());

                strike = Convert.ToInt32(words[1]);
                ball = Convert.ToInt32(words[2]);
            }

            public bool Check(int n1, int n2, int n3)
            {
                int s = 0;
                int b = 0;

                if (this.n1 == n1)
                    ++s;
                else if (this.n2 == n1 || this.n3 == n1)
                    ++b;

                if (this.n2 == n2)
                    ++s;
                else if (this.n1 == n2 || this.n3 == n2)
                    ++b;

                if (this.n3 == n3)
                    ++s;
                else if (this.n1 == n3 || this.n2 == n3)
                    ++b;

                return (strike == s) && (ball == b);
            }
        }
        static void Impl_1761(int n, stImpl_1761_QNA[] inputs)
        {
            System.Diagnostics.Debug.Assert(n == inputs.Length);
            
            int cnt = 0;
            for(int i=1; i<=9-2; ++i)
            {
                for(int j=i+1; j<=9-1; ++j)
                {
                    for(int k=j+1; k<=9; ++k)
                    {
                        //Console.WriteLine("[{3,3}] {0} {1} {2}", i, j, k, index);
                        int[] ijks = new int[3*6]
                        {
                            i, j, k,
                            i, k, j,
                            j, i, k,
                            k, i, j,
                            j, k, i,
                            k, j, i,
                        };

                        for(int m=0; m<ijks.Length; m+=3)
                        {
                            int n1 = ijks[m + 0];
                            int n2 = ijks[m + 1];
                            int n3 = ijks[m + 2];
                            

                            bool isOk = true;
                            for (int l = 0; l < n; ++l)
                            {
                                if (false == inputs[l].Check(n1, n2, n3))
                                {
                                    isOk = false;
                                    break;
                                }
                            }

                            if (isOk)
                            {
                                ++cnt;
                                //Console.WriteLine("[{3,3}] {0} {1} {2}", n1, n2, n3, cnt);
                            }
                        }
                    } //for(k)
                } //for(j)
            } //for(i)

            Console.WriteLine(cnt);
        }
        static void _1761()
        {
            stImpl_1761_QNA[] inputs = new stImpl_1761_QNA[]
            {
                new stImpl_1761_QNA("123 1 1"),
                new stImpl_1761_QNA("356 1 0"),
                new stImpl_1761_QNA("327 2 0"),
                new stImpl_1761_QNA("489 0 1"),
            };
            Impl_1761(4, inputs);
        }



        //--------------------------------------------------
        // 1998 수열
        //--------------------------------------------------
        static bool Impl_1998_GreaterOrEqual(int n1, int n2)
        {
            return n1 >= n2;
        }
        static bool Impl_1998_LessOrEqual(int n1, int n2)
        {
            return n1 <= n2;
        }
        static void Impl_1998(int cnt, params int[] nums)
        {
            if (cnt <= 2)
            {
                Console.WriteLine(cnt);
                return;
            }

            int maxLength = int.MinValue;
            int length = 1;
            bool ge = true;
            for(int i=1; i<cnt; ++i)
            {
                if (ge)
                {
                    if (nums[i-1] > nums[i])
                    {
                        ge = false;
                        if (maxLength < length)
                            maxLength = length;
                        length = 2;

                        for(int j = i-2; j>=0; --j)
                        {
                            if(nums[j] == nums[j+1])
                                ++length;
                            else
                                break;
                        }
                    }
                    else
                    {
                        ++length;
                    }
                }
                else
                {
                    if(nums[i-1] < nums[i])
                    {
                        ge = true;
                        if (maxLength < length)
                            maxLength = length;
                        length = 2;

                        for (int j = i - 2; j >= 0; --j)
                        {
                            if (nums[j] == nums[j + 1])
                                ++length;
                            else
                                break;
                        }
                    }
                    else
                    {
                        ++length;
                    }
                }
            }
            if (maxLength < length)
                maxLength = length;

            Console.WriteLine(maxLength);
        }
        static void _1998()
        {
            Impl_1998(9, 1, 2, 2, 4, 4, 5, 7, 7, 2);
            Impl_1998(9, 4, 1, 3, 3, 2, 2, 9, 2, 3);
            Impl_1998(11, 1, 5, 3, 6, 4, 7, 1, 3, 2, 9, 5);
        }



        //--------------------------------------------------
        // 2259 참외밭
        //
        // 풀이
        //  - 0도 90도, 180도, 270도 4가지 회전의 경우 각 변의 방향은
        //   - N W S E S E
        //   - W S E S E N
        //   - S E S E N W
        //   - E S E N W S
        //   - S E N W S E
        //   - E N W S E S
        //   이상 6개중 하나이며 시작과 끝을 연결하여(원형띄를 생각하자)
        //   두개의 방향이 연속된 위치를 찾는다.
        //   그 위치를 기준으로 큰 사각형에서 작은 사각형의 너비를 빼는 방향으로 접근하면
        //      앞에 두 변이 큰 사각형의 두변이고
        //      가운데 두 변이 작은 사각형의 두변이 된다
        //--------------------------------------------------
        static void Impl_2259(int nPerUnitArea, params int[] sixDirectionAndLength)
        {
            System.Diagnostics.Debug.Assert(6*2 == sixDirectionAndLength.Length);

            // 1 : East
            // 2 : West
            // 3 : South
            // 4 : North

            int n1 = 4;
            int n2 = 6;
            int n3 = 8;
            int n4 = 10;
            
            for(int i=0; i<6; ++i)
            {
                if ( (sixDirectionAndLength[n1] == sixDirectionAndLength[n3]) && 
                     (sixDirectionAndLength[n2] == sixDirectionAndLength[n4]) )
                {
                    //FOUND
                    break;
                }
                
                n1 += 2; if (n1 >= 12) n1 -= 12;
                n2 += 2; if (n2 >= 12) n2 -= 12;
                n3 += 2; if (n3 >= 12) n3 -= 12;
                n4 += 2; if (n4 >= 12) n4 -= 12;
            }

            int nw = n1 - 4; if (nw < 0) nw += 12;
            int nh = n1 - 2; if (nh < 0) nh += 12;

            int W1 = sixDirectionAndLength[nw + 1];
            int H1 = sixDirectionAndLength[nh + 1];
            int W2 = sixDirectionAndLength[n2 + 1];
            int H2 = sixDirectionAndLength[n3 + 1];

            int area = (W1*H1) - (W2*H2);

            Console.WriteLine(area * nPerUnitArea);
        }
        static void _2259()
        {
            Impl_2259(7, 4, 50, 2, 160, 3, 30, 1, 60, 3, 20, 1, 100);
            Impl_2259(7, 1, 100, 4, 50, 2, 160, 3, 30, 1, 60, 3, 20);
            Impl_2259(7, 3, 20, 1, 100, 4, 50, 2, 160, 3, 30, 1, 60);
            Impl_2259(7, 1, 60, 3, 20, 1, 100, 4, 50, 2, 160, 3, 30);
            Impl_2259(7, 3, 30, 1, 60, 3, 20, 1, 100, 4, 50, 2, 160);
            Impl_2259(7, 2, 160, 3, 30, 1, 60, 3, 20, 1, 100, 4, 50);
        }



        //--------------------------------------------------
        // 1836 연속부분합 찾기
        //--------------------------------------------------
        static void Impl_1836(int cnt, params int[] nums)
        {
            int maxSum = 0;
            for(int i=0; i<cnt; ++i)
            {
                int sum = 0;
                for(int j=i; j<cnt; ++j)
                {
                    sum += nums[j];
                    if(sum > maxSum)
                    {
                        maxSum = sum;
                    }
                }
            }
            Console.WriteLine(maxSum);
        }
        static void _1836()
        {
            Impl_1836(4, 1, 2, -2, 4);
        }



        //--------------------------------------------------
        // 1102 스택 (stack)
        //
        // 풀이 : 문제는 C/C++ 를 가정하여 List 를 이용한 구현을 원하지만
        //       C# 에서는 Stack은 기본적으로 제공됨.
        //--------------------------------------------------
        static void Impl_1102(string multiLine)
        {
            string[] lines = multiLine.Split(new char[] {'\n'});

            Stack<int> stack = new Stack<int>();
            //List<int> list = new List<int>();

            int cnt = Convert.ToInt32(lines[0]);
            for(int i=1; i<=cnt; ++i)
            {
                string s = lines[i].Trim();
                char cmd = s[0];

                switch(cmd)
                {
                    case 'i':
                    {
                        string[] words = s.Split();
                        int n = Convert.ToInt32(words[1]);
                        //list.Add(n);

                        stack.Push(n);
                    }break;
                    case 'c':
                    {
                        //Console.WriteLine(list.Count);
                        Console.WriteLine(stack.Count);
                    }
                    break;
                    case 'o':
                    {
                        //if (list.Count == 0)
                        //    Console.WriteLine("empty");
                        //else
                        //{
                        //    int index = list.Count - 1;
                        //    int n = list[index];
                        //    list.RemoveAt(index);
                        //    Console.WriteLine(n);
                        //}
                        if (stack.Count == 0)
                            Console.WriteLine("empty");
                        else
                            Console.WriteLine(stack.Pop());
                    }
                    break;
                }
            }
        }
        static void _1102()
        {
            string ml = @"7
i 7
i 5
c
o
o
o
c";
            Impl_1102(ml);
        }



        //--------------------------------------------------
        // 1697 큐(queue)
        //
        // 풀이 : C# 에서는 Queue 가 기본적으로 제공 되지만
        //       구지 구현한다면 List 보단 LinkedList 를 이용한다
        //--------------------------------------------------
        static void Impl_1697(string multiLine)
        {
            string[] lines = multiLine.Split(new char[] { '\n' });

            //Queue<int> queue = new Queue<int>();
            LinkedList<int> list = new LinkedList<int>();

            int cnt = Convert.ToInt32(lines[0]);
            for (int i = 1; i <= cnt; ++i)
            {
                string s = lines[i].Trim();
                char cmd = s[0];

                switch (cmd)
                {
                    case 'i':
                    {
                        string[] words = s.Split();
                        int n = Convert.ToInt32(words[1]);

                        //queue.Enqueue(n);

                        list.AddLast(n);
                    }
                    break;
                    case 'c':
                    {
                        //Console.WriteLine(queue.Count);

                        Console.WriteLine(list.Count);
                    }
                    break;
                    case 'o':
                    {
                        //if (queue.Count == 0)
                        //    Console.WriteLine("empty");
                        //else
                        //    Console.WriteLine(queue.Dequeue());

                        if (list.Count == 0)
                            Console.WriteLine("empty");
                        else
                        {
                            Console.WriteLine(list.First.Value);
                            list.RemoveFirst();
                        }
                    }
                    break;
                }
            }
        }
        static void _1697()
        {
            string ml = @"7
i 7
i 5
c
o
o
o
c";
            Impl_1697(ml);
        }



        //--------------------------------------------------
        // 1146 선택정렬
        //--------------------------------------------------
        static void Impl_1146(int n, params int[] nums)
        {
            for(int i=0; i<n-1; ++i)
            {
                int min = nums[i];
                int theIndex = -1;
                for(int j=i+1; j<n; ++j)
                {
                    if (min > nums[j])
                    {
                        min = nums[j];
                        theIndex = j;
                    }
                }

                if (theIndex != -1)
                {
                    int tmp = nums[i];
                    nums[i] = nums[theIndex];
                    nums[theIndex] = tmp;
                }
                
                Console.Write("[{0}] : ", i);
                for(int ii = 0; ii<nums.Length; ++ii)
                    Console.Write("{0} ", nums[ii]);
                Console.WriteLine();
            }
        }
        static void _1146()
        {
            Impl_1146(5, 6, 4, 8, 3, 1);
        }



        //--------------------------------------------------
        // 1158 삽입정렬
        //
        // 풀이 : 삽입정렬을 구현하려면 배열보단 LinkedList가 유리
        //       배열은 삽입 위치를 찾고 값의 뒤로 이동이 필요하나
        //       링크드리스트는 중간에 추가 삭제에 이동이 필요 없다.
        //--------------------------------------------------
        static void Impl_1158(int n, params int[] nums)
        {
            if (n<=0)
                return;

            //LinkedList<int> list = new LinkedList<int>();
            //
            //list.AddFirst(nums[0]);
            //for(int i=1; i<n; ++i)
            //{
            //    bool bAdded = false;
            //    LinkedListNode<int> node = list.First;
            //    while(null != node)
            //    {
            //        if (node.Value > nums[i])
            //        {
            //            list.AddBefore(node, nums[i]);
            //            bAdded = true;
            //            break;
            //        }
            //        node = node.Next;
            //    }
            //
            //    if (false == bAdded)
            //    {
            //        list.AddLast(nums[i]);
            //    }
            //
            //
            //    Console.Write("[{0}] : ", i);
            //    node = list.First;
            //    while (null != node)
            //    {
            //        Console.Write("{0} ", node.Value);
            //        node = node.Next;
            //    }
            //    Console.WriteLine();
            //}

            for(int i=1; i<n; ++i)
            {
                int cur = nums[i];

                for(int j=0; j<i; ++j)
                {
                    if(cur < nums[j])
                    {
                        //뒤로 한칸씩 이동
                        for(int k=i; k>j; --k)
                            nums[k] = nums[k-1];

                        //삽입
                        nums[j] = cur;
                        break;
                    }
                }

                Console.Write("[{0}] : ", i);
                for (int ii = 0; ii < nums.Length; ++ii)
                    Console.Write("{0} ", nums[ii]);
                Console.WriteLine();
            }
        }
        static void _1158()
        {
            Impl_1158(5, 5, 4, 3, 7, 6);
        }



        //--------------------------------------------------
        // 1157 버블정렬
        //--------------------------------------------------
        static void Impl_1157(int n, params int[] nums)
        {
            for(int i=0; i<n-1; ++i)
            {
                for(int j=1; j<n-i; ++j)
                {
                    if(nums[j-1] > nums[j])
                    {
                        int tmp = nums[j];
                        nums[j] = nums[j-1];
                        nums[j-1] = tmp;
                    }
                }
                Console.Write("[{0}] : ", i);
                for (int ii = 0; ii < nums.Length; ++ii)
                    Console.Write("{0} ", nums[ii]);
                Console.WriteLine();
            }
        }
        static void _1157()
        {
            Impl_1157(4, 62, 23, 32, 15);
        }



        //--------------------------------------------------
        // 1814 삽입정렬 횟수 세기
        //--------------------------------------------------
        static void Impl_1814(int n, params int[] nums)
        {
            if (n <= 0)
                return;

            int moveCount = 0;
            for (int i = 1; i < n; ++i)
            {
                int cur = nums[i];

                for (int j = 0; j < i; ++j)
                {
                    if (cur < nums[j])
                    {
                        //뒤로 한칸씩 이동
                        for (int k = i; k > j; --k)
                        {
                            ++moveCount;
                            nums[k] = nums[k - 1];
                        }

                        //삽입
                        nums[j] = cur;
                        break;
                    }
                }
            }
            Console.WriteLine(moveCount);
        }
        static void _1814()
        {
            Impl_1814(4, 20, 40, 30, 10);
            Impl_1814(3, -1, 1, 0);
        }



        //--------------------------------------------------
        // 1295 이진탐색
        //--------------------------------------------------
        static int Impl_1295_find(int n, int[] nums)
        {
            if (nums.Length <= 0)
                return -1;

            //[l, r) : 반개구간임 즉, l 포함, r 미포함
            int l = 0;
            int r = nums.Length;
            int m = (r-l) / 2;

            do
            {
                if(nums[m] == n)
                    return m;

                if(nums[m] < n)
                    l = m + 1;
                else
                    r = m;
                m = l + (r-l) / 2;
            }while(r > l);
            
            return -1;
        }
        static void Impl_1295(string multiLines)
        {
            string[] lines = multiLines.Split(new char[] {'\n' });

            int n1 = Convert.ToInt32(lines[0]);
            int[] nums = new int[n1];

            string[] words = lines[1].Split();
            for(int i=0; i<n1; ++i)
                nums[i] = Convert.ToInt32(words[i]);

            int n2 = Convert.ToInt32(lines[2]);

            words = lines[3].Split();
            for(int i=0; i<n2; ++i)
            {
                int n = Convert.ToInt32(words[i]);
                int index = Impl_1295_find(n, nums);
                Console.WriteLine(index + 1);
            }
        }
        static void _1295()
        {
            string ml = @"7 
2 4 9 10 14 23 32 
3 
6 23 9";
            Impl_1295(ml);
        }



        //--------------------------------------------------
        // 1221 후위표기법
        //--------------------------------------------------
        static void Impl_1221(int n, string exp)
        {
            string[] words = exp.Split();
            
            int lhs = Convert.ToInt32(words[0]);
            int rhs = Convert.ToInt32(words[1]);
            for(int i=2; i<n; ++i)
            {
                string op = words[i];
                switch(op)
                {
                    case "+":
                    {
                        lhs += rhs;
                    }
                    break;
                    case "-":
                    {
                        lhs -= rhs;
                    }
                    break;
                    case "*":
                    {
                        lhs *= rhs;
                    }
                    break;
                    case "/":
                    {
                        lhs /= rhs;
                    }
                    break;
                }

                if (i+1 < n)
                {
                    rhs = Convert.ToInt32(words[++i]);
                }
            }

            Console.WriteLine(lhs);
        }
        static void _1221()
        {
            Impl_1221(3, "2 3 +");
            Impl_1221(3, "9 3 /");
            Impl_1221(5, "5 8 + 2 *");
        }



        //--------------------------------------------------
        // 2858 쇠막대기
        //
        // 풀이 - 새로 만나는 쇠막대의 끝은 바로 전에 등장한 쇠막대의 시작과 연결된 막대임
        //--------------------------------------------------
        static void Impl_2858(string input)
        {
            Stack<int> left = new Stack<int>();
            List<int> laser = new List<int>();
            List<int> stick = new List<int>();

            for(int i=0; i<input.Length; ++i)
            {
                char curr = input[i];
                if (curr == '(')
                {
                    char next = input[i+1];
                    if (next == ')')
                        laser.Add(i++);
                    else
                        left.Push(i);
                }
                else
                {
                    stick.Add(left.Pop());
                    stick.Add(i);
                }
            }

            System.Diagnostics.Debug.Assert(left.Count == 0);
            System.Diagnostics.Debug.Assert(stick.Count % 2 == 0);

            int totalCout = 0;
            for(int i=0; i<stick.Count; i+=2)
            {
                int l = stick[i];
                int r = stick[i+1];

                int count = 1;
                for(int j=0; j<laser.Count; ++j)
                {
                    int p = laser[j];
                    if (p > r)
                        break;
                    if (p > l)
                        ++count;
                }
                totalCout += count;
            }
            Console.WriteLine(totalCout);
        }
        static void _2858()
        {
            Impl_2858("()(((()())(())()))(())");
            Impl_2858("(((()(()()))(())()))(()())");
        }



        //--------------------------------------------------
        // 1309 팩토리얼
        //--------------------------------------------------
        static ulong Impl_1309_Factorial(ulong n)
        {
            if (n == 1ul)
            {
                Console.WriteLine("1! = 1");
                return 1ul;
            }
            else
            {
                Console.WriteLine("{0}! = {0} * {1}!", n, n-1);
                return n*Impl_1309_Factorial(n-1);
            }

            //return (n == 0ul) ? 1ul : n*Impl_1309_Factorial(n-1);
        }
        static void Impl_1309(int n)
        {
            Console.WriteLine(Impl_1309_Factorial((ulong)n));
        }
        static void _1309()
        {
            Impl_1309(4);
        }



        //--------------------------------------------------
        // 1161 하노이의 탑
        //--------------------------------------------------
        class HanoiTower
        {
            int Layer { get; set; }
            bool ShowGraph {get; set; }

            int[,] columns;


            public HanoiTower(int n)
            {
                ShowGraph = false;
                Layer = n;
                columns = new int[3, n];

                for (int i = 0; i < n; ++i)
                    columns[0, i] = n - i;
            }


            public void Solve(bool showGraph)
            {
                ShowGraph = showGraph;
                Move(0, 2, 0);
            }


            private int Height(int c)
            {
                for (int i = 0; i < Layer; ++i)
                    if (columns[c, i] == 0)
                        return i;

                return Layer;
            }
            private int TopLayer(int c)
            {
                return Height(c) - 1;
            }

            private int FindOtherColumn(int from, int to)
            {
                if (0 != from && 0 != to)
                    return 0;
                else if (1 != from && 1 != to)
                    return 1;
                else
                    return 2;
            }

            private int BottomLayer(int c, int plate)
            {
                int bottom = -1;
                for (int i = TopLayer(c); i >= 0; --i)
                {
                    if (columns[c, i] < plate)
                        bottom = i;
                }

                return bottom;
            }

            private int Pop(int c)
            {
                int h = Height(c);
                System.Diagnostics.Debug.Assert(h > 0);

                int rt = columns[c, h - 1];
                columns[c, h - 1] = 0;

                return rt;
            }

            private void Push(int c, int n)
            {
                int h = Height(c);
                System.Diagnostics.Debug.Assert(h < Layer);

                columns[c, h] = n;
            }

            private void Draw()
            {
                char C = 'C';
                char S = ' ';
                char V = ' ';

                int layer = Layer;

                System.Text.StringBuilder sb = new System.Text.StringBuilder(layer * 2 * 3 + 2);
                for (int i = layer - 1; i >= 0; --i)
                {
                    sb.Clear();
                    for (int j = 0; j < 3; ++j)
                    {
                        int n = columns[j, i];
                        sb.Append(new string(S, layer - n));
                        sb.Append(new string(C, n * 2));
                        sb.Append(new string(S, layer - n));
                        if (j < 2)
                            sb.Append(V);
                    }
                    Console.WriteLine(sb);
                }
                Console.WriteLine(new string('-', layer * 2 * 3 + 2));
                Console.WriteLine();
            }

            int maxDepth = 0;
            int depth = 0;
            int moveCount = 0;
            private void Move(int from, int to, int layer)
            {
                ++depth;
                if (depth > maxDepth)
                    maxDepth = depth;

                int remainColumn = FindOtherColumn(from, to);
                if (TopLayer(from) == layer)
                {
                    int plate = Pop(from);
                    Push(to, plate);

                    ++moveCount;


                    if (ShowGraph)
                    {
                        System.Threading.Thread.Sleep(50);
                        Console.Clear();
                        Draw();
                    }
                    Console.WriteLine("{0} : {1} -> {2} (depth : {3} / {4}, move count : {5})", plate, from + 1, to + 1, depth, maxDepth, moveCount);

                    int bLayer = BottomLayer(remainColumn, plate);
                    if (bLayer >= 0)
                        Move(remainColumn, to, bLayer);
                }
                else
                {
                    Move(from, remainColumn, layer + 1);
                    Move(from, to, layer);
                }
                --depth;
            }
        }
        static void Impl_1161(int n, bool showGraph)
        {
            HanoiTower h = new HanoiTower(n);

            h.Solve(showGraph);

        }
        static void _1161()
        {
            //for(int i=1; i<=15; ++i)
            //{
            //    Impl_1161(i, true);
            //    System.Threading.Thread.Sleep(5000);
            //}
            
            Impl_1161(12, false);
        }



        //--------------------------------------------------
        // 1169 주사위 던지기1
        //--------------------------------------------------
        static void Impl_1169_1_rcv(int n, int di, int[] nums)
        {
            if (di >= n)
            {
                for(int i=0; i<n; ++i)
                    Console.Write("{0} ", nums[i]);
                Console.WriteLine();
                return;
            }

            for (int i = 1; i <= 6; ++i)
            {
                nums[di] = i;
                Impl_1169_1_rcv(n, di + 1, nums);
            }
        }
        static void Impl_1169_1(int n)
        {
            int[] nums = new int[n];
            Impl_1169_1_rcv(n, 0, nums);
        }


        static void Impl_1169_2_rcv(int n, int di, int[] nums, HashSet<int> flags)
        {
            if (di >= n)
            {
                int[] tmp = new int[n];
                Array.Copy(nums, tmp, n);
                Array.Sort(tmp);

                int hash = 0;
                int digit = 1;
                for (int i = 0; i < n; ++i)
                {
                    hash += tmp[i] * digit;
                    digit *= 10;
                }

                if (flags.Contains(hash))
                    return;
                
                flags.Add(hash);
                for (int i = 0; i < n; ++i)
                    Console.Write("{0} ", tmp[i]);
                Console.WriteLine();
                return;
            }

            for (int i = 1; i <= 6; ++i)
            {
                nums[di] = i;
                Impl_1169_2_rcv(n, di + 1, nums, flags);
            }
        }
        static void Impl_1169_2(int n)
        {
            int[] nums = new int[n];
            HashSet<int> flags = new HashSet<int>();
            Impl_1169_2_rcv(n, 0, nums, flags);
        }


        static void Impl_1169_3_rcv(int n, int di, int[] nums)
        {
            if (di >= n)
            {
                for (int i = 0; i < n; ++i)
                    Console.Write("{0} ", nums[i]);
                Console.WriteLine();
                return;
            }

            for (int i = 1; i <= 6; ++i)
            {
                int index = Array.IndexOf<int>(nums, i);
                if (index < 0 || index >= di)
                {
                nums[di] = i;
                Impl_1169_3_rcv(n, di + 1, nums);
                }
            }
        }
        static void Impl_1169_3(int n)
        {
            int[] nums = new int[3];

            Impl_1169_3_rcv(n, 0, nums);
        }
        static void Impl_1169(int n, int t)
        {
            if (t == 1)
                Impl_1169_1(n);
            else if (t == 2)
                Impl_1169_2(n);
            else if (t == 3)
                Impl_1169_3(n);
            Console.WriteLine();
        }
        static void _1169()
        {
            Impl_1169(3, 1);
            Impl_1169(3, 2);
            Impl_1169(3, 3);
        }



        //--------------------------------------------------
        // 1175 주사위 던지기2
        //--------------------------------------------------
        static void Impl_1175_rcv(int n, int sum, int di, int[] nums)
        {
            if (di >= n)
            {
                int tot = 0;
                foreach(int num in nums)
                {
                    tot += num;
                }

                if (tot != sum)
                    return;

                for(int i=0; i<n; ++i)
                {
                    Console.Write("{0} ", nums[i]);
                }
                Console.WriteLine();
            }
            else
            {
                for(int i=1; i<=6; ++i)
                {
                    nums[di] = i;
                    Impl_1175_rcv(n, sum, di+1, nums);
                }
            }
        }
        static void Impl_1175(int n, int sum)
        {
            int[] nums = new int[n];
            Impl_1175_rcv(n, sum, 0, nums);
        }
        static void _1175()
        {
            Impl_1175(3, 10);
        }



        //--------------------------------------------------
        // 1459 숫자고르기
        //
        // 풀이 - 끝과 시작이 연결된 경우를 찾아보자
        //--------------------------------------------------
        static bool Impl_1459_IsConnected(int[] nums, int i)
        {
            int si = i+1;
            List<int> tbl = new List<int>();
            tbl.Add(si);

            while(tbl.IndexOf(nums[i]) == -1)
            {
                i = nums[i]-1;
                tbl.Add(i+1);
            }
            
            return nums[i] == si;
        }
        static void Impl_1459(int n, params int[] nums)
        {
            System.Diagnostics.Debug.Assert(n == nums.Length);


            List<int> list = new List<int>();
            for(int i=0; i<n; ++i)
            {
                if(Impl_1459_IsConnected(nums,i))
                {
                    list.Add(i+1);
                }
            }

            Console.WriteLine(list.Count);
            for(int i=0; i<list.Count; ++i)
                Console.WriteLine(list[i]);
        }
        static void _1459()
        {
            Impl_1459(7, 3, 1, 1, 5, 5, 4, 6);
        }



        //--------------------------------------------------
        // 1021 장난감조립
        //--------------------------------------------------
        struct stImpl_1021_NoCount
        {
            public int No {get; set; }
            public int Count {get; set; }
        }
        static void Impl_1021_rcsv(Dictionary<int,int> rt, int key, Dictionary<int, List<stImpl_1021_NoCount>> dic)
        {
            List<stImpl_1021_NoCount> list = dic[key];
            foreach(stImpl_1021_NoCount e in list)
            {
                int no = e.No;
                if (dic.ContainsKey(no))
                {
                    for(int i=0; i<e.Count; ++i)
                        Impl_1021_rcsv(rt, no, dic);
                }
                else
                {
                    if(rt.ContainsKey(e.No))
                    {
                        rt[e.No] += e.Count;
                    }
                    else
                    {
                        rt[e.No] = e.Count;
                    }
                }
            }
        }
        static void Impl_1021(string multiLines)
        {
            Dictionary<int, List<stImpl_1021_NoCount>> dic = new Dictionary<int, List<stImpl_1021_NoCount>>();

            string[] lines = multiLines.Split(new char[] { '\n' });
            int nComplete = Convert.ToInt32(lines[0]);
            int nLines = Convert.ToInt32(lines[1]);

            for (int i = 0; i < nLines; ++i)
            {
                string line = lines[i + 2];
                string[] words = line.Split();

                int key = Convert.ToInt32(words[0]);
                int no = Convert.ToInt32(words[1]);
                int cnt = Convert.ToInt32(words[2]);


                if (dic.ContainsKey(key))
                {
                    List<stImpl_1021_NoCount> list = dic[key];
                    list.Add(new stImpl_1021_NoCount { No = no, Count = cnt });
                }
                else
                {
                    List<stImpl_1021_NoCount> list = new List<stImpl_1021_NoCount>();
                    list.Add(new stImpl_1021_NoCount { No = no, Count = cnt });
                    dic[key] = list;
                }
            }


            Dictionary<int, int> result = new Dictionary<int, int>();
            Impl_1021_rcsv(result, nComplete, dic);


            foreach (KeyValuePair<int, int> entry in result)
            {
                Console.WriteLine("{0} {1}", entry.Key, entry.Value);
            }
        }
        static void _1021()
        {
            string input = @"7
8
5 1 2
5 2 2
7 5 2
6 5 2
6 3 3
6 4 4
7 6 3
7 4 5";
            Impl_1021(input);
        }



        //--------------------------------------------------
        // 1147 주사위 쌓기
        //--------------------------------------------------
        static int Impl_1147_MaxSide(int topi, int row, int[,] arr)
        {
            int[,] sides = {
                {1,3,2,4 }, // 0
                {0,5,2,4 }, // 1
                {0,5,1,3 }, // 2
                {0,5,2,4 }, // 3
                {0,5,1,3 }, // 4
                {1,3,2,4 }, // 5
            };

            int max = int.MinValue;
            for (int i=0; i<4; ++i)
            {
                max = Math.Max(max, arr[row, sides[topi, i]]);
            }

            return max;
        }

        static int Impl_1147_TopI(int bottom, int row, int[,] arr)
        {
            int[] theI = { 5,3,4,1,2,0 };
            for(int i=0; i<6; ++i)
            {
                if (arr[row,i] == bottom)
                    return theI[i];
            }

            System.Diagnostics.Debug.Assert(false, "invalid");
            return -1;
        }

        static void Impl_1147(string input)
        {
            string[] lines = input.Split(new char[] {'\n'});

            int cnt = Convert.ToInt32(lines[0]);

            int[,] arr = new int[cnt, 6];

            for(int i=0; i<cnt; ++i)
            {
                string[] words = lines[i+1].Split();
                for(int j=0; j<6; ++j)
                    arr[i,j] = Convert.ToInt32(words[j]);
            }


            int maxSum = 0;
            for(int i=0; i<6; ++i)
            {
                int top = arr[0,i];
                int cur = 0;
                int topi = i;
                for(int j=0; j<cnt; ++j)
                {
                    cur += Impl_1147_MaxSide(topi, j, arr);
                    
                    if(j < cnt-1)
                    {
                        topi = Impl_1147_TopI(top, j+1, arr);
                        top = arr[j+1, topi];
                    }
                }

                if (maxSum < cur)
                    maxSum = cur;
            }

            Console.WriteLine(maxSum);
        }
        static void _1147()
        {
            string input = @"5
2 3 1 6 5 4
3 1 2 4 6 5
5 6 4 1 3 2
1 3 6 2 4 5
4 1 6 5 2 3";
            Impl_1147(input);
        }


        static void impl_3427(int n, string src)
        {
            char[] arr = src.ToCharArray();
            System.Diagnostics.Debug.Assert(n == arr.Length);
            
            char ch1 = 'R';
            char ch2 = 'B';
            int cnt1 = 0;
            int lastIndexOfCh2 = Array.LastIndexOf(arr, ch2);
            for (int i = lastIndexOfCh2 - 1; i >= 0; --i)
            {
                if (arr[i] == ch1)
                {
                    arr[lastIndexOfCh2] = ch1;
                    arr[i] = ch2;
                    lastIndexOfCh2 = i;
                    ++cnt1;
                }
            }

            ch1 = 'B';
            ch2 = 'R';
            lastIndexOfCh2 = Array.LastIndexOf(arr, ch2);
            int cnt2 = 0;
            for (int i = lastIndexOfCh2 - 1; i >= 0; --i)
            {
                if (arr[i] == ch1)
                {
                    arr[lastIndexOfCh2] = ch1;
                    arr[i] = ch2;
                    lastIndexOfCh2 = i;
                    ++cnt2;
                }
            }

            //Console.WriteLine("{0}, {1}", cnt1, cnt2);
            Console.WriteLine(Math.Min(cnt1, cnt2));

        }
        static void _3427()
        {
            impl_3427(9, "RBBBRBRRR");
            impl_3427(8, "BBRBBBBR");
        }

    }


}
