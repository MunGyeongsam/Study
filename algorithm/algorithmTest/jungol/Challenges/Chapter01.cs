using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Challenges
{
    internal class Chapter01
    {
        public static void Test()
        {
            //Prob01();
            //Prob02();
            //Prob03();
            Prob04();
        }

        //--------------------------------
        // 1. The 3n+1 Problem
        //--------------------------------
        static void Prob01()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 1000; ++i)
            {
                Prob01_3n_1(1, 10);
                Prob01_3n_1(100, 200);
                Prob01_3n_1(201, 210);
                Prob01_3n_1(900, 1000);
            }
            watch.Stop();
            Console.WriteLine($"elpased : {watch.ElapsedMilliseconds}");
        }

        static void Prob01_3n_1(int from, int to)
        {
            int maxval = 0;
            for (int i = from; i <= to; i++)
            {
                maxval = Math.Max(maxval, Prob01_3n_1_Length_Recv(i));
            }

            Console.WriteLine($"{from} {to} {maxval}");
        }

        static int Prob01_3n_1_Length(int n)
        {
            int rt = 1;

            while (n != 1)
            {
                n = (n & 0x1) == 1 ? (3 * n + 1) : (n >> 1);
                rt++;
            }

            return rt;
        }

        static int Prob01_3n_1_Length_Recv(int n, int length = 1)
        {
            if (n == 1) return length;

            return Prob01_3n_1_Length_Recv((n & 0x1) == 1 ? (3 * n + 1) : (n >> 1), length + 1);
        }


        //--------------------------------
        // 2. Minesweeper
        //--------------------------------
        static void Prob02()
        {
            Prob02_Impl(@"4 4
*...
....
.*..
....
3 5
**...
.....
.*...
0 0
");
        }
        static void Prob02_Impl(string input)
        {
            string[] lines = input.Split('\n');

            int id = 1;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                string[] words = line.Split(' ');

                int n = Convert.ToInt32(words[0]);
                int m = Convert.ToInt32(words[1]);

                if (n == 0 && m == 0)
                    break;

                char[,] map = new char[n, m];
                for (int r = 0; r < n; r++)
                {
                    line = lines[++i];
                    for (int c = 0; c < m; c++)
                    {
                        map[r, c] = line[c];
                    }
                }

                Prob02_Impl_Minesweeper(map, id++);
                //Util.PrintArray(map);
            }
        }
        static void Prob02_Impl_Minesweeper(char[,] map, int id)
        {
            Console.WriteLine($"Field #{id}:");

            int[,] t = new int[map.GetLength(0) + 2, map.GetLength(1) + 2];

            for (int r = 0; r < map.GetLength(0); ++r)
            {
                int tr = r + 1;
                for (int c = 0; c < map.GetLength(1); ++c)
                {
                    int tc = c + 1;

                    if (map[r, c] == '*')
                    {
                        ++t[tr - 1, tc - 1]; ++t[tr - 1, tc + 0];   ++t[tr - 1, tc + 1];
                        ++t[tr - 0, tc - 1];                        ++t[tr - 0, tc + 1];
                        ++t[tr + 1, tc - 1]; ++t[tr + 1, tc + 0];   ++t[tr + 1, tc + 1];
                    }
                }
            }

            for (int r = 0; r < map.GetLength(0); ++r)
            {
                int tr = r + 1;
                for (int c = 0; c < map.GetLength(1); ++c)
                {
                    int tc = c + 1;

                    if (map[r, c] == '*')
                        Console.Write(map[r, c]);
                    else
                        Console.Write(t[tr, tc]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        //--------------------------------
        // 3. The Trip
        //--------------------------------
        static void Prob03()
        {
            Prob03_Impl(@"3
10.00
20.00
30.00
4
15.00
15.01
3.00
3.01
0");
        }

        static void Prob03_Impl(string input)
        {
            string[] lines = input.Split('\n');
            string line;
            for(int i=0; i<lines.Length; ++i)
            {
                line = lines[i].Trim();

                int n = Convert.ToInt32(line);

                if (n == 0)
                    return;

                float[] arr = new float[n];

                for(int j=0; j<n; ++j)
                {
                    line = lines[++i].Trim();
                    arr[j] = Convert.ToSingle(line);

                    //Console.WriteLine(arr[j]);
                }

                Prob03_TheTrip(arr);
            }
        }

        static void Prob03_TheTrip(float[] arr)
        {
            float sum = 0F;
            foreach (var v in arr)
                sum += v;
            float avg = sum / arr.Length;

            //Console.WriteLine("avg : {0:N}", avg);

            sum = 0F;
            foreach (var v in arr)
            {
                if (v > avg)
                {
                    float t = v - avg;
                    t = MathF.Truncate(t * 100F) / 100F;
                    sum += t;
                }
            }

            Console.WriteLine("${0:N2}", sum);
        }


        //--------------------------------
        // 4. LCD Display
        //--------------------------------
        static void Prob04()
        {
            Prob04_Impl(@"2 12345
3 67890
1 8055
0 0");
        }

        static void Prob04_Impl(string input)
        {
            string[] lines = input.Split('\n');

            foreach(var line in lines)
            {
                string[] words = line.Split();
                int s = Convert.ToInt32(words[0]);

                if (s == 0) { break; }

                Prob04_Impl_LCD(s, words[1]);
                Console.WriteLine();
            }
        }

        static void Prob04_Impl_LCD(int s, string num)
        {
            const bool T = true;
            const bool F = false;
            bool[][,] tbl =
            {
                new bool[5,3]{  //0
                    {F, T, F},
                    {T, F, T},
                    {F, F, F},
                    {T, F, T},
                    {F, T, T} },
                new bool[5,3]{  //1
                    {F, F, F},
                    {F, F, T},
                    {F, F, F},
                    {F, F, T},
                    {F, F, F} },
                new bool[5,3]{  //2
                    {F, T, F},
                    {F, F, T},
                    {F, T, F},
                    {T, F, F},
                    {F, T, F} },
                new bool[5,3]{  //3
                    {F, T, F},
                    {F, F, T},
                    {F, T, F},
                    {F, F, T},
                    {F, T, F} },
                new bool[5,3]{  //4
                    {F, F, F},
                    {T, F, T},
                    {F, T, F},
                    {F, F, T},
                    {F, F, F} },
                new bool[5,3]{  //5
                    {F, T, F},
                    {T, F, F},
                    {F, T, F},
                    {F, F, T},
                    {F, T, F} },
                new bool[5,3]{  //6
                    {F, T, F},
                    {T, F, F},
                    {F, T, F},
                    {T, F, T},
                    {F, T, F} },
                new bool[5,3]{  //7
                    {F, T, F},
                    {F, F, T},
                    {F, F, F},
                    {F, F, T},
                    {F, F, F} },
                new bool[5,3]{  //8
                    {F, T, F},
                    {T, F, T},
                    {F, T, F},
                    {T, F, T},
                    {F, T, F} },
                new bool[5,3]{  //9
                    {F, T, F},
                    {T, F, T},
                    {F, T, F},
                    {F, F, T},
                    {F, T, F} },
            };


            var horizontal = true;
            for(int i=0; i<5; i++)
            {
                int N = horizontal ? 1 : s;

                for (int j = 0; j < N; j++)
                {
                    var n = 0;
                    foreach(var c in num)
                    {
                        n = c - '0';

                        var flags = tbl[n];
                        if (horizontal)
                        {
                            Prob04_Impl_DrawHorizontal(s, flags[i, 1]);
                        }
                        else
                        {
                            Prob04_Impl_DrawVertical(s, flags[i, 0], flags[i, 2]);
                        }
                        Console.Write(' ');
                    }

                    Console.WriteLine();
                }

                horizontal = !horizontal;
            }
        }

        static void Prob04_Impl_DrawHorizontal(int s, bool flag)
        {
            if (flag)
            {
                Console.Write(' '); Console.Write(new string('-', s)); Console.Write(' ');
            }
            else
            {
                Console.Write(new string(' ', s + 2));
            }
        }
        static void Prob04_Impl_DrawVertical(int s, bool flag1, bool flag2)
        {
            Console.Write(flag1 ? '|' : ' '); Console.Write(new string(' ', s)); Console.Write(flag2 ? '|' : ' ');
        }
    }
}
