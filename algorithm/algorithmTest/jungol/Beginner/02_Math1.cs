using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _02_Math1
    {
        public static void Run()
        {
            Util.Call(_1692);
            Util.Call(_1430);
            Util.Call(_1071);
            Util.Call(_1402);
            Util.Call(_2809);
            Util.Call(_1658);
            Util.Call(_1002);
            Util.Call(_5545);
        }

        //--------------------------------------------------
        // 1692 곱셈
        //--------------------------------------------------
        static void Impl_1692(int a, int b)
        {
            int mul = a * b;
            while (b > 0)
            {
                int rem = b % 10;
                Console.WriteLine(a * rem);
                b /= 10;
            }
            Console.WriteLine(mul);
        }
        static void _1692()
        {
            Impl_1692(472, 385);
        }

        //--------------------------------------------------
        // 1430 숫자의 개수
        //--------------------------------------------------
        static void Impl_1430(int a, int b, int c)
        {
            int mul = a * b * c;

            int[] cnts = new int[10];

            while (mul > 10)
            {

                int rem = mul % 10;
                ++cnts[rem];
                mul /= 10;
            }
            ++cnts[mul];
            foreach (int n in cnts)
                Console.WriteLine(n);
        }
        static void _1430()
        {
            Impl_1430(150, 266, 427);
        }

        //--------------------------------------------------
        // 1071 약수와 배수
        // 
        // 6
        // 2 3 5 12 18 24
        // 12
        // 
        // 17
        // 36
        //--------------------------------------------------
        static void Impl_1071(string n, string nums, string theNum)
        {
            int cnt = Convert.ToInt32(n);
            string[] words = nums.Split();
            int val = Convert.ToInt32(theNum);

            System.Diagnostics.Debug.Assert(cnt == words.Length);

            int sumOfDivisors = 0;
            int sumOfMultiples = 0;
            for (int i = 0; i < cnt; ++i)
            {
                int tmp = Convert.ToInt32(words[i]);

                //약수
                if ((tmp <= val) && (val % tmp == 0))
                    sumOfDivisors += tmp;

                //배수
                if ((tmp >= val) && (tmp % val == 0))
                    sumOfMultiples += tmp;
            }

            Console.WriteLine(sumOfDivisors);
            Console.WriteLine(sumOfMultiples);
        }
        static void _1071()
        {
            Impl_1071("6", "2 3 5 12 18 24", "12");
        }

        //--------------------------------------------------
        // 1402 약수 구하기
        //--------------------------------------------------
        static void Impl_1402(int n, int k)
        {
            if (k == 1)
            {
                Console.WriteLine(1);
                return;
            }

            int[] divds = new int[k];
            int index = 0;
            divds[index++] = 1;

            for (int i = 2; i < n; ++i)
            {
                if (n % i == 0)
                {
                    divds[index++] = i;

                    if (index == k)
                    {
                        Console.WriteLine(i);
                        return;
                    }
                }
            }

            Console.WriteLine(0);
        }
        static void _1402()
        {
            Impl_1402(6, 3);
            Impl_1402(2735, 1);
        }

        //--------------------------------------------------
        // 2809 약수
        //--------------------------------------------------
        static void Impl_2809(int n)
        {
            for (int i = 1; i <= n; ++i)
            {
                if (n % i == 0)
                    Console.Write("{0} ", i);
            }
            Console.WriteLine();
        }
        static void _2809()
        {
            Impl_2809(24);
        }

        //--------------------------------------------------
        // 1658 최대공약수와최소공배수
        //
        // 24 18
        // 
        // 6
        // 72
        //--------------------------------------------------
        static void Impl_1658(int n1, int n2)
        {
            int LCM = 1;
            int GCD = 1;
            //the least[lowest] common multiple (LCM)
            //the greatest common denominator (GCD)

            int min = Math.Min(n1, n2);
            int max = Math.Max(n1, n2);

            while (min > 1)
            {

                bool notFound = true;
                for (int i = 2; i <= min; ++i)
                {
                    if ((min % i == 0) && (max % i == 0))
                    {
                        GCD *= i;
                        min /= i;
                        max /= i;

                        notFound = false;
                    }
                }

                if (!notFound)
                    break;
            }


            LCM = GCD * min * max;

            Console.WriteLine(GCD);
            Console.WriteLine(LCM);
        }
        static void _1658()
        {
            Impl_1658(24, 18);
        }

        //--------------------------------------------------
        // 1002 최대공약수, 최소공배수
        // 
        // 3
        // 2 8 10
        // 
        // 2 40
        //--------------------------------------------------
        static void Impl_1002(string sCount, string sNums)
        {
            int LCM = 1;    //the least[lowest] common multiple (LCM)
            int GCD = 1;    //the greatest common denominator (GCD)


            int cnt = Convert.ToInt32(sCount);
            string[] words = sNums.Split();

            System.Diagnostics.Debug.Assert(cnt == words.Length);
            int[] nums = new int[cnt];
            for (int i = 0; i < cnt; ++i)
                nums[i] = Convert.ToInt32(words[i]);

            Array.Sort(nums);

            int min = nums[0];
            while (min > 1)
            {

                bool notFound = true;
                for (int i = 2; i <= min; ++i)
                {

                    bool isCommonDividor = true;
                    for (int j = 1; j < cnt; ++j)
                    {

                        if (nums[j] % i != 0)
                        {
                            isCommonDividor = false;
                            break;
                        }
                    }

                    if (isCommonDividor)
                    {

                        GCD *= i;
                        notFound = false;
                        min /= i;
                        nums[0] = min;
                        for (int j = 1; j < cnt; ++j)
                        {
                            nums[j] /= i;
                        }
                    }
                }

                if (notFound)
                    break;
            }

            LCM = GCD;
            for (int j = 0; j < cnt; ++j)
            {
                LCM *= nums[j];
            }

            Console.WriteLine("{0} {1}", GCD, LCM);

        }
        static void _1002()
        {
            Impl_1002("3", "2 8 10");
        }


        //--------------------------------------------------
        // 5545 연필 공장
        //
        // 첫 줄에 정수 P, V, K가 공백을 기준으로 나누어​ 입력된다
        // (0 < P < 10^6, 0 < V < 10^6, 0 < K < 10^9).
        //
        // 첫 줄에 네 개의 정수 A, B, C, D를 출력하시오.
        // A.도색과 광택이 완료된 연필의 수        
        // B.도색도 광택도 되지 않은 연필의 수        
        // C. 도색은 되었으나 광택이 되지 않은 연필의 수        
        // D. 광택은 되었으나 도색이 되지 않은 연필의 수​
        //--------------------------------------------------
        static int GCDEuclidean(int a, int b)
        {
            if (a < b)
                MyUtil.Swap(ref a, ref b);

            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }
            return a;
        }
        static void Impl_5545(int P, int V, int K)
        {
            int a = P + 1;
            int b = V + 1;
            int gcd = GCDEuclidean(a, b);
            int lcm = (int)((long)a * b / gcd);

            int A = K - (K / a + K / b - K / lcm);
            int B = K / lcm;
            int C = (K / b) - B;
            int D = (K / a) - B;

            Console.WriteLine($"{A} {B} {C} {D}");
        }
        static void Impl_5545_BruteForce(int P, int V, int K)
        {
            int a = P + 1;
            int b = V + 1;

            int A = 0;
            int B = 0;
            int C = 0;
            int D = 0;

            for (int i = 1; i <= K; ++i)
            {
                if ((i % a != 0) && (i % b != 0))
                    ++A;
                else if ((i % a == 0) && (i % b == 0))
                    ++B;
                else if ((i % a != 0) && (i % b == 0))
                    ++C;
                else if ((i % a == 0) && (i % b != 0))
                    ++D;
            }

            Console.WriteLine($"{A} {B} {C} {D}");
        }
        static void _5545()
        {
            Impl_5545(3, 5, 17);
            Impl_5545(999999, 999999, 999999999);

            //Impl_5545_BruteForce(3, 5, 17);
            //Impl_5545_BruteForce(999999, 999999, 999999999);
        }
    }
}
