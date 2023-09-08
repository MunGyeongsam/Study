using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace jungol.LanguageCSharp
{
    internal class _08_Yield
    {
        static IEnumerable<int> GetNumber(int from, int to, int step=1)
        {
            for(int i = from; i <= to; i+=step)
            {
                yield return i;
            }
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
