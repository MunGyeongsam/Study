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
            //Prob04();
            //Prob05();
            Prob06();
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
                        ++t[tr - 1, tc - 1]; ++t[tr - 1, tc + 0]; ++t[tr - 1, tc + 1];
                        ++t[tr - 0, tc - 1]; ++t[tr - 0, tc + 1];
                        ++t[tr + 1, tc - 1]; ++t[tr + 1, tc + 0]; ++t[tr + 1, tc + 1];
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
            for (int i = 0; i < lines.Length; ++i)
            {
                line = lines[i].Trim();

                int n = Convert.ToInt32(line);

                if (n == 0)
                    return;

                float[] arr = new float[n];

                for (int j = 0; j < n; ++j)
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

            foreach (var line in lines)
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
            for (int i = 0; i < 5; i++)
            {
                int N = horizontal ? 1 : s;

                for (int j = 0; j < N; j++)
                {
                    var n = 0;
                    foreach (var c in num)
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

        //--------------------------------
        // 5. Graphical Editor
        //--------------------------------
        static void Prob05()
        {
            Prob05_Impl(@"I 5 6
L 2 3 A
S one.bmp
G 2 3 J
F 3 3 J
V 2 3 4 W
H 3 4 2 Z
S two.bmp
X");
        }
        static void Prob05_Impl(string input)
        {
            string[] lines = input.Split('\n');

            char[,] img = null;

            foreach (string line in lines)
            {
                string l = line.Trim();
                char CMD = l[0];

                switch (CMD)
                {
                    case 'I': img = Prob05_Impl_I(l); break;
                    case 'C': Prob05_Impl_C(img); break;
                    case 'L': Prob05_Impl_L(img, l); break;
                    case 'V': Prob05_Impl_V(img, l); break;
                    case 'H': Prob05_Impl_H(img, l); break;
                    case 'K': Prob05_Impl_K(img, l); break;
                    case 'F': Prob05_Impl_F(img, l); break;
                    case 'S': Prob05_Impl_S(img, l); break;

                    case 'X': return;
                }
            }
        }
        static void Prob05_Impl_Fill(char[,] img, char color, int x1 = -1, int y1 = -1, int x2 = -1, int y2 = -1)
        {
            int r1 = y1 < 0 ? 0 : y1 - 1;
            int c1 = x1 < 0 ? 0 : x1 - 1;

            int r2 = y2 < 0 ? img.GetLength(0) - 1 : y2 - 1;
            int c2 = x2 < 0 ? img.GetLength(1) - 1 : x2 - 1;

            int rstep = r1 < r2 ? 1 : -1;
            int cstep = c1 < c2 ? 1 : -1;

            for (int r = r1; ; r += rstep)
            {
                for (int c = c1; ; c += cstep)
                {
                    img[r, c] = color;
                    if (c == c2) break;
                }

                if (r == r2) break;
            }
        }
        static char[,] Prob05_Impl_I(string input)
        {
            string[] words = input.Split();
            int M = Convert.ToInt32(words[1]);
            int N = Convert.ToInt32(words[1]);

            var img = new char[N, M];
            Prob05_Impl_Fill(img, 'O');

            return img;
        }

        static void Prob05_Impl_C(char[,] img)
        {
            Prob05_Impl_Fill(img, 'O');
        }
        static void Prob05_Impl_L(char[,] img, string input)
        {
            string[] words = input.Split();

            int x = Convert.ToInt32(words[1]);
            int y = Convert.ToInt32(words[2]);
            char c = words[3][0];

            img[y - 1, x - 1] = c;
        }
        static void Prob05_Impl_V(char[,] img, string input)
        {
            string[] words = input.Split();

            int x = Convert.ToInt32(words[1]);
            int y1 = Convert.ToInt32(words[2]);
            int y2 = Convert.ToInt32(words[3]);
            char c = words[4][0];

            int step = y1 < y2 ? 1 : -1;

            for (int y = y1; ; y += step)
            {
                img[y - 1, x - 1] = c;

                if (y == y2)
                    break;
            }
        }
        static void Prob05_Impl_H(char[,] img, string input)
        {
            string[] words = input.Split();

            int x1 = Convert.ToInt32(words[1]);
            int x2 = Convert.ToInt32(words[2]);
            int y = Convert.ToInt32(words[3]);
            char c = words[4][0];

            int step = x1 < x2 ? 1 : -1;

            for (int x = x1; ; x += step)
            {
                img[y - 1, x - 1] = c;

                if (x == x2)
                    break;
            }
        }

        static void Prob05_Impl_K(char[,] img, string input)
        {
            string[] words = input.Split();

            int x1 = Convert.ToInt32(words[1]);
            int y1 = Convert.ToInt32(words[2]);
            int x2 = Convert.ToInt32(words[3]);
            int y2 = Convert.ToInt32(words[4]);
            char c = words[5][0];

            Prob05_Impl_Fill(img, c, x1, y1, x2, y2);
        }
        static void Prob05_Impl_F(char[,] img, string input)
        {
            string[] words = input.Split();

            int x = Convert.ToInt32(words[1]);
            int y = Convert.ToInt32(words[2]);
            char color = words[3][0];

            bool[,] lookup = new bool[img.GetLength(0), img.GetLength(1)];

            int r = y - 1;
            int c = x - 1;

            Prob05_Impl_F_Recv(img, lookup, r, c, img[r, c], color);
        }
        static void Prob05_Impl_F_Recv(char[,] img, bool[,] lookup, int r, int c, char org, char color)
        {
            if (r < 0 || r >= img.GetLength(0) || c < 0 || c >= img.GetLength(1))
                return;

            if (lookup[r, c])
                return;

            lookup[r, c] = true;
            if (img[r, c] == org)
            {
                img[r, c] = color;

                Prob05_Impl_F_Recv(img, lookup, r - 1, c, org, color);
                Prob05_Impl_F_Recv(img, lookup, r + 1, c, org, color);
                Prob05_Impl_F_Recv(img, lookup, r, c - 1, org, color);
                Prob05_Impl_F_Recv(img, lookup, r, c + 1, org, color);
            }
        }
        static void Prob05_Impl_S(char[,] img, string input)
        {
            string[] words = input.Split();
            Console.WriteLine(words[1]);

            for (int r = 0; r < img.GetLength(0); r++)
            {
                for (int c = 0; c < img.GetLength(1); c++)
                {
                    Console.Write(img[r, c]);
                }
                Console.WriteLine();
            }
        }



        //--------------------------------
        // 6. Interpreter
        //--------------------------------
        static void Prob06()
        {
            Prob06_Impl(@"2

299
492
495
399
492
495
399
283
279
689
078
100
000
000
000


299
492
495
399
492
495
399
283
279
689
078
100
000
000
000
");
        }
        static void Prob06_Impl(string input)
        {
            string[] lines = input.Split('\n');

            int n = Convert.ToInt32(lines[0].Trim());

            List<string> cmds = new List<string>();

            int li = 2;

            for (int i = 0; i < n; i++)
            {
                cmds.Clear();

                for (; li < lines.Length; li++)
                {
                    var line = lines[li].Trim();
                    if (line.Length == 0)
                    {
                        li += 2;
                        break;
                    }

                    cmds.Add(line);
                }

                Prob06_Impl_Interperter(cmds);
            }
        }
        static void Prob06_Impl_Interperter(List<string> cmds)
        {
            int[] regs = new int[10];

            int iIns = 0;
            int insCnt = 0;

            string cmd;
            int d, n, s, a;
            while(iIns < cmds.Count)
            {
                ++insCnt;
                cmd = cmds[iIns++];

                Console.WriteLine($"{cmd}, iIns({iIns}), insCnt({insCnt})");

                if (cmd == "100")
                    break;

                switch(cmd[0])
                {
                    case '2':
                        {
                            d = cmd[1] - '0';
                            n = cmd[2] - '0';
                            regs[d] = n;
                        } break;
                    case '3':
                        {
                            d = cmd[1] - '0';
                            n = cmd[2] - '0';
                            regs[d] += n;
                            regs[d] %= 1000;
                        }
                        break;
                    case '4':
                        {
                            d = cmd[1] - '0';
                            n = cmd[2] - '0';
                            regs[d] *= n;
                            regs[d] %= 1000;
                        }
                        break;
                    case '5':
                        {
                            d = cmd[1] - '0';
                            s = cmd[2] - '0';
                            regs[d] = regs[s];
                        }
                        break;
                    case '6':
                        {
                            d = cmd[1] - '0';
                            s = cmd[2] - '0';
                            regs[d] += regs[s];
                            regs[d] %= 1000;
                        }
                        break;
                    case '7':
                        {
                            d = cmd[1] - '0';
                            s = cmd[2] - '0';
                            regs[d] *= regs[s];
                            regs[d] %= 1000;
                        }
                        break;
                    case '8':
                        {
                            d = cmd[1] - '0';
                            a = cmd[2] - '0';
                            regs[d] = Convert.ToInt32(cmds[regs[a]]);
                        }
                        break;
                    case '9':
                        {
                            s = cmd[1] - '0';
                            a = cmd[2] - '0';
                            cmds[regs[a]] = string.Format("{0:0##}", regs[s]);
                        }
                        break;
                    case '0':
                        {
                            d = cmd[1] - '0';
                            s = cmd[2] - '0';

                            if (regs[s] != 0)
                            {
                                iIns = regs[d];
                            }
                        }
                        break;
                }
            }

            Console.WriteLine($"---- {insCnt}");
        }

    }
}
