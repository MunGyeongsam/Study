using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _05_String
    {
        public static void Run()
        {
            Util.Call(_2604);
            Util.Call(_2514);
            Util.Call(_2857);
            Util.Call(_1880);
            Util.Call(_1535);
            Util.Call(_4987);
            Util.Call(_5349);
            Util.Call(_1516);
            Util.Call(_3699);
        }

        //--------------------------------------------------
        // 2604 그릇
        //--------------------------------------------------
        static void Impl_2604(string s)
        {
            System.Diagnostics.Debug.Assert(s.Length >= 1);
            int height = 10;

            char prev = s[0];
            for (int i = 1; i < s.Length; ++i)
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
            for (int i = 0; i < s.Length - 2; ++i)
            {
                if (s[i + 1] == 'O' && s[i + 2] == 'I')
                {
                    if (s[i] == 'K')
                    {
                        ++kCount;
                        ++i;    // to skip 'O'
                    }
                    else if (s[i] == 'I')
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
        // 2857 세로읽기
        //--------------------------------------------------
        static void Impl_2857(string s)
        {
            string[] lines = s.Split('\n');
            for(int i=0; i<lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();
            }

            int row = lines.Length;
            int col = lines[0].Length;
            for(int i=1; i<row; ++i)
            {
                if (col < lines[i].Length)
                    col = lines[i].Length;
            }

            for(int c=0; c<col; ++c)
            {
                for(int r=0; r<row; ++r)
                {
                    if (c < lines[r].Length)
                        Console.Write(lines[r][c]);
                }
            }
            Console.WriteLine();
        }
        static void _2857()
        {
            Console.WriteLine();
            Impl_2857(@"ABCDE
abcde
01234
FGHIJ
fghij");

            Console.WriteLine();
            Impl_2857(@"AABCDD
afzz
09121
a8EWg6
P5h3kx");
            Console.WriteLine();
        }

        //--------------------------------------------------
        // 1880 암호풀기(Message Decoding)
        //--------------------------------------------------
        static void Impl_1880(string s)
        {
            string[] lines = s.Split('\n');
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();
            }

            System.Diagnostics.Debug.Assert(lines.Length == 2);
            System.Diagnostics.Debug.Assert(lines[0].Length == 26);

            foreach(char c in lines[1])
            {
                if (c == ' ')
                {
                    Console.Write(' ');
                    continue;
                }

                bool isUpper = char.IsUpper(c);
                int index = (char.ToLower(c)) - 'a';

                char ch = lines[0][index];
                if (isUpper)
                    Console.Write(char.ToUpper(ch));
                else
                    Console.Write(ch);
            }
        }
        static void _1880()
        {
            Impl_1880(@"eydbkmiqugjxlvtzpnwohracsf
Kifq oua zarxa suar bti yaagrj fa xtfgrj");
        }

        //--------------------------------------------------
        // 1535 단어집합(하)
        //--------------------------------------------------
        static void Impl_1535(HashSet<string> lookup, List<string> output, string input)
        {
            string[] words = input.Split();
            for (int i = 0; i < words.Length; ++i)
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
        // 4987 String 자료형(C++)
        //--------------------------------------------------
        static void Impl_4987(string s, string t)
        {

            while (true)
            {
                int index = s.IndexOf(t);
                if (index == -1)
                    break;

                s = s.Remove(index, t.Length);
            }
            Console.WriteLine(s);
        }
        static void _4987()
        {
            Impl_4987("whatthemomooofun", "moo");
            Impl_4987("ababababa", "aba");
        }

        //--------------------------------------------------
        // 5349 SubString (부분 문자열)
        //--------------------------------------------------
        static void Impl_5349(string s)
        {
            string[] words = s.Split();
            int wlen = words.Length;

            for (int i=(wlen % 2 == 0) ? wlen - 1 : wlen - 2; i>0; i-=2)
            {
                Console.Write(words[i] + " ");
            }
            Console.WriteLine();
        }
        static void _5349()
        {
            Impl_5349("Jungol is the best");
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
            for (int i = 1; i < words.Length; ++i)
            {
                if (curr != words[i])
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

            foreach (string line in inputs)
            {
                if (line == "END")
                    break;

                Impl_1516(line);
                Console.WriteLine();
            }
        }

        //--------------------------------------------------
        // 3699 변장
        //--------------------------------------------------
        static void Impl_3699(string s)
        {

            Impl_3699_GetCombinations(7, 1);
            Impl_3699_GetCombinations(7, 2);
            Impl_3699_GetCombinations(7, 3);
            Impl_3699_GetCombinations(7, 4);
            Impl_3699_GetCombinations(7, 5);
            Impl_3699_GetCombinations(7, 6);
            Impl_3699_GetCombinations(7, 7);
        }
        static int[,] Impl_3699_GetCombinations(int n, int r)
        {
            int numOfCombinations = Impl_3699_CalcCombination(n, r);

            Console.WriteLine("n: {0}, r: {1}, numOfCombinations: {2}", n, r, numOfCombinations);
            int[,] comb = new int[numOfCombinations, r];

            int[] arr = new int[r];
            int row = 0;
            Impl_3699_RecsvGenerateCombinations(n, r, arr, comb, ref row, 0, 0);

            Util.PrintArray(comb, 3, " ");
            return comb;
        }
        static void Impl_3699_RecsvGenerateCombinations(int n, int r, int[] arr, int[,] comb, ref int row, int index, int start)
        {
            if (index == r)
            {
                for (int i = 0; i < r; ++i)
                {
                    comb[row, i] = arr[i];
                    //Console.Write(arr[i] + " ");
                }
                row++;
                //Console.WriteLine();
                return;
            }

            for (int i = start; i < n; ++i)
            {
                arr[index] = i;
                Impl_3699_RecsvGenerateCombinations(n, r, arr, comb, ref row, index + 1, i + 1);
            }
        }
        static int Impl_3699_CalcCombination(int n, int r)
        {
            return Impl_3699_Factorial_Tail(n) / (Impl_3699_Factorial_Tail(r) * Impl_3699_Factorial_Tail(n - r));
        }
        static int Impl_3699_Factorial(int n)
        {
            if (n == 0)
                return 1;
            return n * Impl_3699_Factorial(n - 1); 
        }
        static int Impl_3699_Factorial_Tail(int n, int acc = 1)
        {
            if (n == 0)
                return acc;
            return Impl_3699_Factorial_Tail(n - 1, n * acc);
        }

        static void _3699()
        {
            Impl_3699("3\nhat headgear\nsunglasses eyewear\nturban headgear\n5\nmask face\nsunglasses face\nmakeup face\nmask face\nmakeup face\n");
        }
    }
}
