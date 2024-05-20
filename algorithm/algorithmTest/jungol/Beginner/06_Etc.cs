using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _06_Etc
    {
        public static void Run()
        {
            Util.Call(_1438);
            Util.Call(_1671);
            Util.Call(_1997);
            Util.Call(_2259);
            Util.Call(_1761);
            Util.Call(_1031);
            Util.Call(_3427);
            Util.Call(_1836);
            Util.Call(_1733);
            Util.Call(_1311);
        }

        //--------------------------------------------------
        // 1438 색종이(초)
        //--------------------------------------------------
        static void Impl_1438(int cnt, params int[] points)
        {
            System.Diagnostics.Debug.Assert(points.Length == cnt * 2);

            bool[,] paper = new bool[100, 100];
            int area = 0;
            for (int i = 0; i < cnt; ++i)
            {
                int x = points[i * 2 + 0];
                int y = points[i * 2 + 1];

                for (int r = y; r < y + 10; ++r)
                {
                    for (int c = x; c < x + 10; ++c)
                    {
                        if (false == paper[r, c])
                        {
                            paper[r, c] = true;
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
            Impl_1438(3, 3, 7, 15, 7, 5, 2);
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
            for (int i = 0; i < 100; ++i)
            {
                for (int j = 0; j < 100; ++j)
                {
                    if (false == paper[i, j])
                        continue;

                    //위아래
                    if (i == 0 || i == 99 || !paper[i - 1, j] || !paper[i + 1, j])
                        ++rt;

                    //양옆
                    if (j == 0 || j == 99 || !paper[i, j - 1] || !paper[i, j + 1])
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
        // 1997 떡 먹는 호랑이
        //--------------------------------------------------
        static void Impl_1997(int D, int K)
        {
            System.Diagnostics.Debug.Assert(D >= 3);
            System.Diagnostics.Debug.Assert(D <= 30);
            int[] day1 = { 1, 0 };
            int[] day2 = { 0, 1 };
            int[] day3 = { 0, 0 };

            for (int i = 3; i <= D; ++i)
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
            for (int i = 1; i <= TO; ++i)
            {
                int rem = K - b * i;
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
            System.Diagnostics.Debug.Assert(6 * 2 == sixDirectionAndLength.Length);

            // 1 : East
            // 2 : West
            // 3 : South
            // 4 : North

            int n1 = 4;
            int n2 = 6;
            int n3 = 8;
            int n4 = 10;

            for (int i = 0; i < 6; ++i)
            {
                if ((sixDirectionAndLength[n1] == sixDirectionAndLength[n3]) &&
                     (sixDirectionAndLength[n2] == sixDirectionAndLength[n4]))
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

            int area = (W1 * H1) - (W2 * H2);

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
            Impl_2259(7, 3, 40, 1, 80, 4, 15, 2, 15, 4, 25, 2, 65);
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
            for (int i = 1; i <= 9 - 2; ++i)
            {
                for (int j = i + 1; j <= 9 - 1; ++j)
                {
                    for (int k = j + 1; k <= 9; ++k)
                    {
                        //Console.WriteLine("[{3,3}] {0} {1} {2}", i, j, k, index);
                        int[] ijks = new int[3 * 6]
                        {
                            i, j, k,
                            i, k, j,
                            j, i, k,
                            k, i, j,
                            j, k, i,
                            k, j, i,
                        };

                        for (int m = 0; m < ijks.Length; m += 3)
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
        // 1031 빙고
        //--------------------------------------------------
        static KeyValuePair<int, int> Impl_1031_FindRowColumn(int[,] map, int num)
        {
            for (int i = 0; i < map.GetLength(0); ++i)
            {
                for (int j = 0; j < map.GetLength(1); ++j)
                {
                    if (num == map[i, j])
                    {
                        return new KeyValuePair<int, int>(i, j);
                    }
                }
            }

            System.Diagnostics.Debug.Assert(false, "impossible");
            return new KeyValuePair<int, int>(-1, -1);
        }

        static int Impl_1031_lineCount(bool[,] map, int i, int j)
        {
            System.Diagnostics.Debug.Assert(map[i, j] == true);

            int rt = 0;
            int cnt = 0;
            // →
            for (int k = 0; k < 5; ++k)
            {
                if (false == map[k, j])
                    break;
                ++cnt;
            }
            if (cnt == 5)
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
            if (i == j)
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
                    if (false == map[k, 4 - k])
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
            string[] lines = input.Split(new char[] { '\n' });

            System.Diagnostics.Debug.Assert(lines.Length == 10);

            int[,] nmap = new int[5, 5];
            bool[,] bmap = new bool[5, 5];

            for (int i = 0; i < 5; ++i)
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
                string line = lines[i + 5].Trim();
                string[] words = line.Split();
                System.Diagnostics.Debug.Assert(words.Length == 5);

                for (int j = 0; j < 5; ++j)
                {
                    int n = Convert.ToInt32(words[j]);
                    KeyValuePair<int, int> rowCol = Impl_1031_FindRowColumn(nmap, n);
                    bmap[rowCol.Key, rowCol.Value] = true;
                    lineCount += Impl_1031_lineCount(bmap, rowCol.Key, rowCol.Value);
                    if (lineCount >= 3)
                    {
                        Console.WriteLine(i * 5 + j + 1);
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
        // 3427 볼 모으기(balls)
        //--------------------------------------------------
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
                if ( arr[i] == ch1)
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


        //--------------------------------------------------
        // 1836 연속부분합 찾기
        //--------------------------------------------------
        static void Impl_1836(int cnt, params int[] nums)
        {
            int maxSum = 0;
            for (int i = 0; i < cnt; ++i)
            {
                int sum = 0;
                for (int j = i; j < cnt; ++j)
                {
                    sum += nums[j];
                    if (sum > maxSum)
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
        // 1733 오목
        //--------------------------------------------------
        static void Impl_1733(string input)
        {
            int[,] map = new int[19, 19];

            string[] lines = input.Split(new char[] { '\n' });
            System.Diagnostics.Debug.Assert(lines.Length == 19);
            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i].Trim();
                string[] stones = line.Split();
                System.Diagnostics.Debug.Assert(stones.Length == 19);
                for (int j = 0; j < stones.Length; ++j)
                    map[i, j] = Convert.ToInt32(stones[j]);
            }

            bool[,] flags = new bool[19, 19];

            for (int i = 0; i < 19; ++i)
            {
                for (int j = 0; j < 19; ++j)
                {
                    if (0 == map[i, j])
                        continue;

                    int n = map[i, j];
                    int cnt = 0;

                    // →
                    cnt = 1;
                    for (int jj = j + 1; jj < 19; ++jj)
                    {
                        if (n != map[i, jj])
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
                    for (int ii = i + 1, jj = j + 1; ii < 19 && jj < 19; ++ii, ++jj)
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
            for (int i = 1; i < cards.Length; ++i)
                if (color != cards[i].color)
                    return false;

            return true;
        }
        static int[] Impl_1311_Counting(Impl_1311_Card[] cards)
        {
            int[] cnts = new int[10]; //the index of '0' is not used

            foreach (Impl_1311_Card c in cards)
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

        static int Impl_1311_FindCount(int[] cnts, int theCount, int skipIndex = 0)
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

        static int Impl_1311_HasTwoCards(int[] cnts, int skipIndex = 0)
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
                if (Impl_1311_IsStraight(cards))
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
            if (0 != chk)
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
    }
}
