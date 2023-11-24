using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace jungol.LanguageCSharp
{
    class IndexerTest
    {
        int _i = 0;
        public int this[int i]
        {
            private get { return _i; }
            set { _i = value; }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
    internal class _08_Yield
    {
        static IEnumerable<int> GetNumber(int from, int to, int step=1)
        {
            for(int i = from; i <= to; i+=step)
            {
                yield return i;
            }

            IndexerTest a = new IndexerTest();
            //Console.WriteLine(a[0]);
            a[0] = 1;

            Action<int> ac = (int a) => { Console.WriteLine(a); };
        }

        static void Test01()
        {
            Console.WriteLine("0 ~ 10, 3");

            Console.WriteLine("wiith foreach");
            foreach (var i in GetNumber(1, 10, 3))
                Console.WriteLine(i);


            Console.WriteLine("wiith enumerator");
            var en = GetNumber(1, 5).GetEnumerator();
            while (en.MoveNext())
            {
                Console.WriteLine(en.Current);
            }
        }

        public static void Test()
        {
            Test01();
        }
    }
}
