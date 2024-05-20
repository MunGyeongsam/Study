using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _04_Math2
    {
        public static void Run()
        {
            Util.Call(_1009);
            Util.Call(_2811);
            Util.Call(_1901);
            Util.Call(_1740);
            Util.Call(_2813);
            Util.Call(_2814);
            Util.Call(_1534);
            Util.Call(_3106);
            Util.Call(_4977);
        }

        //--------------------------------------------------
        // 1009 각 자리수의 역과 합
        //--------------------------------------------------
        static void Impl_1009(int n)
        {
            int sum = 0;
            while (n > 10)
            {
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
            foreach (int i in inputs)
            {
                if (i <= 0)
                    break;

                Impl_1009(i);
            }
        }

        //--------------------------------------------------
        // 2811 소수와 합성수
        //--------------------------------------------------
        static bool Impl_2811_IsPrimeNumber(int n)
        {
            for (int i = 2; i * i < n; ++i)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }
        static void Impl_2811(string str)
        {
            string[] arr = str.Split();
            foreach (string w in arr)
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
        // 1901 소수 구하기
        //--------------------------------------------------
        static bool Impl_1901_IsPrimeNumber(int n)
        {
            for (int i = 2; i * i <= n; ++i)
                if (n % i == 0)
                    return false;

            return true;
        }
        static void Impl_1901_NearestPrimeNumber(int n)
        {
            if (n <= 2)
                Console.WriteLine(2);

            int prime1 = 2;
            for (int i = n; i >= 2; --i)
            {
                if (Impl_1901_IsPrimeNumber(i))
                {
                    prime1 = i;
                    break;
                }
            }

            int prime2 = prime1;
            for (int i = n + 1; i <= n + (n - prime1); ++i)
            {
                if (Impl_1901_IsPrimeNumber(i))
                {
                    prime2 = i;
                    break;
                }
            }

            if (prime2 == prime1)
                Console.WriteLine(prime1);
            else if (prime2 - n == n - prime1)
                Console.WriteLine("{0} {1}", prime1, prime2);
        }
        static void Impl_1901(int cnt, int[] src)
        {
            System.Diagnostics.Debug.Assert(cnt == src.Length);
            for (int i = 0; i < cnt; ++i)
                Impl_1901_NearestPrimeNumber(src[i]);
        }
        static void _1901()
        {
            Impl_1901(2, new int[] { 8, 15 });
        }

        //--------------------------------------------------
        // 1740 소수
        //--------------------------------------------------
        static void Impl_1740(int from, int to)
        {
            System.Collections.BitArray lookup = new System.Collections.BitArray(to + 1, true);
            lookup[0] = lookup[1] = false;

            int sum = 0;
            int min = 0;

            for (int i = 2; i <= to; ++i)
            {
                if (lookup[i])
                {
                    if (i >= from)
                    {
                        if (min == 0)
                            min = i;
                        sum += i;
                    }
                    uint ui = (uint)i;
                    for (uint j = ui * ui; j <= (uint)to; j += ui)
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
        // 2813 소수의 개수
        //--------------------------------------------------
        static void Impl_2813_eratos(int from, int to)
        {
            System.Collections.BitArray lookup = new System.Collections.BitArray(to + 1, true);
            lookup[0] = lookup[1] = false;

            int cnt = 0;
            for (int i = 2; i < to; ++i)
            {
                if (lookup[i])
                {
                    if (i >= from)
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
        // 2814 이진수
        //--------------------------------------------------
        static void Impl_2814(string binary)
        {
            System.Diagnostics.Debug.Assert(binary.Length <= 30);

            //int rt_easy = Convert.ToInt32(binary, 2);
            //Console.WriteLine(rt_easy);

            int rt = 0;
            for (int i = binary.Length - 1; i >= 0; --i)
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
            while (stack.Count > 0)
                Console.Write(stack.Pop());

            Console.WriteLine();
        }
        static void Impl_1534_16(int dec)
        {
            Stack<int> stack = Impl_1534_stack(dec, 16);
            char[] chs = { 'A', 'B', 'C', 'D', 'E', 'F' };

            Console.Write("{0,8} : ", dec);
            while (stack.Count > 0)
            {
                int n = stack.Pop();
                if (n < 10)
                    Console.Write(n);
                else
                    Console.Write(chs[n - 10]);
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


            Impl_1534(27, 2);
            Impl_1534(625, 2);
            Impl_1534(123456, 2);
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
                    Console.Write((char)('A' + (char)(n - 10)));
            }
            Console.WriteLine();
        }
        static int Impl_3106_2nd(int fromBase, string num)
        {
            int rt = 0;
            int pow = 1;    // Math.Pow(fromBase, 0);
            for (int i = num.Length - 1; i >= 0; --i)
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
                    n = (int)(c - 'A') + 10;
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

            for (int i = 0; i < inputs.Length; ++i)
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
        // 4977 실수의 이진수
        //--------------------------------------------------
        static void Impl_4977(string input)
        {
            input = input.Trim();

            int p = input.IndexOf('.');
            int n = int.Parse(input.Substring(0, p));
            float f = float.Parse(input.Substring(p));

            StringBuilder sb = new StringBuilder();
            sb.Capacity = 12;   //7(100 : 1100100) + 1(소수점) + 4(소수점이하 4자리)

            while (n > 0)
            {
                sb.Insert(0, n % 2);
                n /= 2;
            }

            sb.Append('.');
            int cnt = 4;
            while (f > 0 && cnt > 0)
            {
                f *= 2;
                if (f >= 1)
                {
                    sb.Append(1);
                    f -= 1;
                }
                else
                {
                    sb.Append(0);
                }
                --cnt;
            }
            sb.Append('0', cnt);

            Console.WriteLine(sb.ToString());
        }

        static void _4977()
        {
            Impl_4977("127.625");
            Impl_4977("1.123456");
            Impl_4977("5.625");
            Impl_4977("3.141592654");
        }
    }
}
