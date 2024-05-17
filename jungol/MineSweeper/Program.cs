using System;

namespace MineSweeper
{
    /*
     * 마인크래프트
     * 
     * 1. 맵을 프린트
     * 2. 맵을 생성
     * 3. 풀이
     * */
    class Exe
    {
        static Random random = new Random();

        static void PrintArray(int[,] arr)
        {
            int ROW = arr.GetLength(0);
            int COL = arr.GetLength(1);

            Console.WriteLine("[{0} x {1}]", ROW, COL);
            Console.Write("     ");
            for (int j = 0; j < COL; ++j)
                Console.Write("{0} ", j%10);
            Console.WriteLine();

            for (int i = 0; i < ROW; ++i)
            {
                Console.Write("{0,2} : ", i);
                for (int j = 0; j < COL; ++j)
                {
                    int a = arr[i, j];
                    if (a == 0) // 공백
                        Console.Write("{0} ", '.');
                    else if (a == 9)    // 폭탄
                        Console.Write("{0} ", '*');
                    else
                        Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }

        static int[,] MakeMap(int R, int C, int CNT)
        {
            int[,] map = new int[R, C];

            for (int i = 0; i < CNT; ++i)
            {
                int r = random.Next(0, R);
                int c = random.Next(0, C);
                while (map[r, c] == 9)
                {
                    r = random.Next(0, R);
                    c = random.Next(0, C);
                }
                map[r, c] = 9;
            }

            return map;
        }
        static void Solve(int[,] map)
        {

        }
        static void Main()
        {
            int[,] levelInfo =
            {
                { 9,  9, 10},   // 초급
                {16, 16, 40},   // 중급
                {16, 39, 99},   // 고급
            };

            do
            {
                Console.WriteLine("1 : 초급( 9 x  9 : 10");
                Console.WriteLine("2 : 중급(16 x 16 : 40");
                Console.WriteLine("3 : 고급(16 x 30 : 99");
                Console.Write("번호를 선택 하세요 : ");
                string line = Console.ReadLine();
                int level = Convert.ToInt32(line);
                if (level < 1 || level > 3)
                    continue;

                int r = Convert.ToInt32(levelInfo[level-1, 0]);
                int c = Convert.ToInt32(levelInfo[level-1, 1]);
                int n = Convert.ToInt32(levelInfo[level-1, 2]);
                int[,] m = MakeMap(r, c, n);
                Solve(m);

                PrintArray(m);

                Console.WriteLine("계속 하시겠습니까? (Y or N)");
                line = Console.ReadLine();
                if ("N" == line || "n" == line)
                    break;
                Console.Clear();
            } while (true);
        }
    }
}

/*
namespace MineSweeper
{
    class Program
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);

        static void PrintMap(char[,] map)
        {
            int ROW = map.GetLength(0);
            int COL = map.GetLength(1);

            Console.WriteLine("[{0} x {1}]", ROW, COL);
            Console.WriteLine();
            
            Console.Write("    ");
            for (int j=0; j<COL; ++j)
            {
                Console.Write("{0} ", j % 10);
            }
            Console.WriteLine();
            Console.WriteLine();

            for (int i=0; i<ROW; ++i)
            {
                Console.Write("{0}   ", i % 10);
                for (int j=0; j<COL; ++j)
                {
                    Console.Write("{0} ", map[i, j]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        static char[,] Make(int ROW, int COL, int percentage)
        {
            char[,] map = new char[ROW, COL];

            int cnt = (int)(ROW * COL * percentage / 100F);

            for(int i=0; i<cnt; ++i)
            {
                while(true)
                {
                    int r = rnd.Next(ROW);
                    int c = rnd.Next(COL);

                    if (map[r,c] != '*')
                    {
                        map[r, c] = '*';
                        break;
                    }
                }
            }

            return map;
        }

        static void Solve(char[,] map)
        {
            int ROW = map.GetLength(0);
            int COL = map.GetLength(1);

            int[,] tmp = new int[ROW + 2, COL + 2];

            for(int i=0; i<ROW; ++i)
            {
                int ti = i + 1;
                for (int j=0; j<COL; ++j)
                {
                    int tj = j + 1;
                    if (map[i,j] == '*')
                    {
                        ++tmp[ti - 1, tj - 1];
                        ++tmp[ti - 1, tj    ];
                        ++tmp[ti - 1, tj + 1];

                        ++tmp[ti    , tj - 1];
                        ++tmp[ti    , tj + 1];

                        ++tmp[ti + 1, tj - 1];
                        ++tmp[ti + 1, tj    ];
                        ++tmp[ti + 1, tj + 1];
                    }
                }
            }

            for (int i = 0; i < ROW; ++i)
            {
                int ti = i + 1;
                for (int j = 0; j < COL; ++j)
                {
                    int tj = j + 1;
                    if (map[i, j] == '*')
                    {
                        tmp[ti, tj] = 0;
                    }
                }
            }
            
            for (int i = 0; i < ROW; ++i)
            {
                int ti = i + 1;
                for (int j = 0; j < COL; ++j)
                {
                    int tj = j + 1;
                    if (tmp[ti, tj] > 0)
                        map[i, j] = tmp[ti, tj].ToString()[0];
                }
            }
        }

        static void Main()
        {
            char[,] m = Make(10, 20, 30);
            PrintMap(m);
            Solve(m);
            PrintMap(m);
        }
    }
}
*/