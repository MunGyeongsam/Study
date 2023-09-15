using jungol.LanguageCSharp.Private_17_Indexer;
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

        class MineMap
        {
            public enum Level : short
            {
                None = 0,
                Beginner,       //  9 X  9 : 10
                Intermediate,   // 16 X 16 : 40
                Advanced,       // 16 X 39 : 99

                Num
            }

            public int Row { get; private set; }
            public int Col { get; private set; }
            public int Cnt { get; private set; }
            public Level Lv { get; private set; }

            char[] _map;

            public void Setup(Level lv)
            {
                if (Lv != lv)
                {
                    SetSize(lv);
                    Lv = lv;
                }
                else
                    Array.Fill(_map, '.');
            }

            private void SetSize(Level lv)
            {
                switch (lv)
                {
                    case Level.Beginner:
                        Row = 9;
                        Col = 9;
                        Cnt = 10;
                        break;
                    case Level.Intermediate:
                        Row = 16;
                        Col = 16;
                        Cnt = 40;
                        break;
                    case Level.Advanced:
                        Row = 16;
                        Col = 39;
                        Cnt = 99;
                        break;
                }

                _map = new char[Row *  Col];
                Array.Fill(_map, '.');
            }

            public char this[int i, int j]
            {
                get
                {
                    return _map[i * Col + j];
                }
                set
                {
                    _map[i * Col + j] = value;
                }
            }

            public void Print()
            {
                Console.WriteLine($"[{Row} x {Col}]");
                int index = 0;
                for(int i= 0; i < Row; i++)
                {
                    for(int j= 0; j < Col; j++)
                    {
                        Console.Write(_map[index++]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
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

            MineMap m = new MineMap();
            m.Setup(MineMap.Level.Beginner);

            m[0, 0] = '*';
            m[8, 8] = '*';

            m.Print();

            m.Setup(MineMap.Level.Intermediate);
            m[0, 0] = '*';
            m[8, 8] = '*';
            m.Print();

        }
    }
}
