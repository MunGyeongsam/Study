using System;
using System.Collections.Generic;
using System.Text;

using jungol.LanguageCSharp.Private_15_Event;

namespace jungol.LanguageCSharp
{
    namespace Private_15_Event
    {
        delegate int Delegate01(int a, int b);
        delegate void Delegate02(int a, int b);

        class A
        {
            public int _n;

            public Delegate01 _d1;
            public event Delegate01 _d2;

            public A(int n)
            {
                _n = n;
            }

            public static int Test01(int a, int b)
            {
                Console.WriteLine("-- Test01");
                return a + b;
            }
            public int Test02(int a, int b)
            {
                Console.WriteLine("-- Test02");
                return a + b;
            }

            public static void Test03(int a, int b)
            {
                Console.WriteLine("-- Test03");
            }
            public void Test04(int a, int b)
            {
                Console.WriteLine("-- Test04");
            }

            public static bool Test05(int a)
            {
                Console.WriteLine("-- Test05");
                return a > 0;
            }
        }
    }
    
    internal class _15_Event
    {
        public static void Test()
        {
            var a = new A(33);

            Console.WriteLine("-------------- Delegate01");
            Delegate01 d1 = new Delegate01(A.Test01);
            d1(1, 2);
            d1 += a.Test02;
            d1(1, 2);

            Console.WriteLine("-------------- Func");
            Func<int, int, int> f1 = A.Test01;
            f1(1, 2);
            f1 = a.Test02;
            f1(1, 2);

            Console.WriteLine("-------------- Delegate02");
            Delegate02 d2 = A.Test03;
            d2(1, 2);
            d2 = a.Test04;
            d2(1, 2);

            Console.WriteLine("-------------- Action");
            Action<int, int> a1 = A.Test03;
            a1(1, 2);
            a1 = a.Test04;
            a1(1, 2);

            Console.WriteLine("-------------- Delegate Member");
            a._d1 = A.Test01;
            a._d1 += A.Test01;
            a._d1 += a.Test02;
            a._d1(1, 2);


            Console.WriteLine("-------------- Event Member");
            //a._d2 = A.Test01;
            a._d2 += A.Test01;
            a._d2 += a.Test02;
            //a._d2(1, 2);
        }
    }
}
