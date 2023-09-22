using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Challenges
{
    internal class Chapter02_Prob011_Hartal
    {
        public static void Run()
        {
            Solve(@"2
14
3
3
4
8
100
4
12
15
25
40");
        }
        static void Solve(string input)
        {
            string[] lines = input.Split('\n');

            int t = Convert.ToInt32(lines[0].TrimEnd());
            int il = 1;
            int n = 0;
            int cnt = 0;
            int[] args;

            for(int i=0; i<t; i++)
            {
                n = Convert.ToInt32(lines[il++].TrimEnd());
                cnt = Convert.ToInt32(lines[il++].TrimEnd());

                args = new int[cnt];

                for (int j=0; j<cnt; j++)
                {
                    args[j] = Convert.ToInt32(lines[il++].TrimEnd());
                }
                Impl_Hartal(n, args);
            }
        }
        static void Impl_Hartal(int n, int[] args)
        {
            var bitarr = new System.Collections.BitArray(n + 1);
            int tot = 0;
            foreach (int h in args)
            {
                for(int i=h; i<=n; i+=h)
                {
                    if (bitarr[i])
                        continue;

                    int rem = i % 7;
                    if (rem == 0 || rem == 6)
                        continue;

                    ++tot;
                    bitarr[i] = true;
                }
            }

            Console.WriteLine(tot);
        }
    }
}
