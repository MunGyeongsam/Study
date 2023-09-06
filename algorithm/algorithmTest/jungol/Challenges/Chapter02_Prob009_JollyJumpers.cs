using System;
using System.Collections;
using System.Collections.Generic;

namespace jungol.Challenges
{
    // 9. Jolly Jumpers
    public class Chapter02_Prob009_JollyJumpers
    {
        public static void Run()
        {
            Solve(@"4 1 4 2 3
5 1 4 2 -1 6");
        }

        static void Solve(string input)
        {
            string[] lines = input.Split('\n');

            foreach(var l in lines)
            {
                Check(l);
            }
        }

        static void Check(string line)
        {
            line = line.TrimEnd();
            if (line.Length == 0)
                return;

            string[] words = line.Split();

            int n = Convert.ToInt32(words[0]);

            BitArray flags = new BitArray(n);
            flags[0] = true;//not used

            int n1 = Convert.ToInt32(words[1]);
            int n2 = 0;
            int delta = 0;

            for(int i=2; i<words.Length; ++i)
            {
                n2 = Convert.ToInt32(words[i]);

                delta = Math.Abs(n1 - n2);
                if (delta < flags.Count)
                    flags[delta] = true;

                n1 = n2;
            }

            bool isJolly = true;
            for(int i=1; i<n; ++i)
            {
                if (flags[i] == false)
                {
                    isJolly = false;
                    break;
                }
            }

            Console.WriteLine(isJolly ? "Jolly" : "Not Jolly");
        }
    }
}
