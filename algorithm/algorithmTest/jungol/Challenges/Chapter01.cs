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
            Prob03();
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

                    Console.WriteLine(arr[j]);
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


            Console.WriteLine("avg : {0:N}", avg);

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

            Console.WriteLine("sum : {0:N2}", sum);
        }
    }
}
