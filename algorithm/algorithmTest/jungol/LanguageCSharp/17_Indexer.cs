using System;
using System.Collections.Generic;
using System.Text;


namespace jungol.LanguageCSharp
{
    using A = jungol.LanguageCSharp.Private_17_Indexer.A;

    namespace Private_17_Indexer
    {
        class A
        {
            int[] _data = new int[10];

            public void Set(params int[] data)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (i >= _data.Length)
                        break;

                    _data[i] = data[i];
                }
            }

            public int this[int i]
            {
                get { 
                    return _data[i];
                }
                set {
                    _data[i] = value;
                }
            }
        }
    }

    internal class _17_Indexer
    {
        public static void Test()
        {
            A a = new A();
            a.Set(1, 3, 2, 3);
            Console.WriteLine(a[0]);
            a[0] = 4;
            Console.WriteLine(a[0]);
        }
    }
}
