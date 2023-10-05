using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Challenges
{
    internal class Chapter02_Prob012_CryptKicker
    {
        public static void Run()
        {
            Solve(@"6
and
dick
jane
puff
spot
yertle
bjvg xsb hxsn xsb gymm xsb rqat xsb pnetfn
xxxx yyy zzzz www yyyy aaa bbbb ccc dddddd");
        }

        static Dictionary<char, char> dicPattern = new Dictionary<char, char>(16);
        static StringBuilder strPattern = new StringBuilder(16);

        private static string Pattern(string word)
        {
            dicPattern.Clear();
            strPattern.Clear();

            string s;
            char pc;
            int i = 0;

            Console.WriteLine($"word : {word}");
            foreach(var c in word)
            {
                if (dicPattern.ContainsKey(c))
                {
                    pc = dicPattern[c];
                    Console.WriteLine($"{c} : {pc}");
                }
                else
                {
                    s = (i++).ToString("X");
                    pc = s[0];
                    //Console.WriteLine($"{c} : {pc}");

                    dicPattern[c] = pc;
                }

                strPattern.Append(pc);
            }

            //Console.WriteLine($"str : {strPattern.ToString()}");
            
            return strPattern.ToString();
        }

        private static void Solve(string input)
        {
            // and      3 123
            // dick     4 1234
            // jane     4 1234
            // puff     4 1233
            // spot     4 1234
            // yertle   6 123452

            Console.WriteLine(int.MaxValue);



            string[] lines = input.Split('\n');

            int il = 0;

            StringBuilder tmp = new StringBuilder(16);

            int n = Convert.ToInt32(lines[il++].TrimEnd());
            string[] tbl = new string[n];

            var dicByLen = new Dictionary<int, List<int> >();
            var dicByPattern = new Dictionary<string, List<int> >();

            var dicWords = new HashSet<string>();

            for (int i=0; i<n; ++i)
            {
                string word = lines[il++].TrimEnd();
                tbl[i] = word;

                int len = word.Length;

                if (!dicByLen.ContainsKey(len))
                {
                    dicByLen[len] = new List<int>();
                }
                dicByLen[len].Add(i);

                string pt = Pattern(word);
                if (!dicByPattern.ContainsKey(pt))
                {
                    dicByPattern[pt] = new List<int>();
                }
                dicByPattern[pt].Add(i);
            }


            string line;
            dicWords.Clear();
            for (int i=il; i<lines.Length; ++i)
            {
                line = lines[i].TrimEnd();
                string[] words = line.Split();
                tmp.Clear();
                tmp.Append(line);

                foreach (var w in words)
                {
                    int len = w.Length;
                    string pt = Pattern(w);

                    var lst = dicByPattern[pt];
                }
            }

        }
    }
}
