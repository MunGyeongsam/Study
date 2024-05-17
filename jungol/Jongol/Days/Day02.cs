using System;
using static System.Console;

namespace Jongol.Days
{
    class Day02
    {
        static void BuiltinTypes()
        {
            //int, uint, short, ushort, byte, sbyte, long, ulong
            //float, double, decimal
            //bool
            //string

            //unsafe
            {
                int a = 3;
                Console.WriteLine(3.GetType());
                Console.WriteLine(3u.GetType());
                Console.WriteLine(3.GetType());

                Console.WriteLine("{0:R}", Math.PI);
                double doubleVal = 0.91234582637;
                Console.WriteLine("{0:R2}", doubleVal);
            }

            //Console.WriteLine(sizeof(int));
            //Console.WriteLine(typeof(int));
            //Console.WriteLine(int.MaxValue);
            //Console.WriteLine(int.MinValue);

        }
        static void Writing()
        {
            //write
            string str = "Hello World!";

            System.Console.WriteLine(str);
            Console.WriteLine(str);
            WriteLine(str);

            Console.WriteLine("\n-------------------\n");

            Console.Write(str);
            Console.WriteLine(str);
            Console.Write(str);
            Console.Write(str);
            Console.WriteLine(str);

            Console.WriteLine("\n-------------------\n");
        }

        static void Reading()
        {
            //read
            int n = Console.Read();
            //while(-1 != (n = Read()))
            {
                Console.WriteLine("{0} : {1}", n, (char)n);
            }

            Console.WriteLine("\n-------------------\n");

            string l = Console.ReadLine();
            Console.WriteLine(l);
            for (int i = 0; i < l.Length; ++i)
                Console.WriteLine("[{0}] : {1}", i, l[i]);

            Console.WriteLine("\n-------------------\n");
            ConsoleKeyInfo ki = Console.ReadKey();
            Console.WriteLine("\n{0} {1} {2}", ki.Key, ki.KeyChar, ki.Modifiers);
        }


        enum EE { _e0 = 0x1, _e1 = 0x2, _e2 = 0x4 }
        // https://docs.microsoft.com/ko-kr/dotnet/standard/base-types/formatting-types
        // https://slaner.tistory.com/92
        static void Formating()
        {
            int n = 14;
            double d = Math.PI;
            float f = (float)Math.E;
            EE e = EE._e0;
            EE e2 = EE._e0 | EE._e2;

            Console.WriteLine("{0} {1} {0}", n, d, f);

            Console.WriteLine("\n\n");
            Console.WriteLine("'{0, 5}'", n);
            Console.WriteLine("'{0, -5}'", n);

            Console.WriteLine("\n\n");
            Console.WriteLine("'{0, 5:f3}'", n);
            Console.WriteLine("'{0, 5:d3}'", n);
            Console.WriteLine("'{0, 5:x3}'", n);
            Console.WriteLine("'{0, 5:c}'", n);
            Console.WriteLine("'{0, 5:c1}'", n);
            Console.WriteLine("'{0, 5:c2}'", n);


            Console.WriteLine("\n\n");
            Console.WriteLine();

            Console.WriteLine("\n\n");
            Console.WriteLine("'{0, 5:f}'", e);
            Console.WriteLine("'{0, 5:d}'", e);
            Console.WriteLine("'{0}'", e);
            Console.WriteLine("'{0}'", e.ToString("F"));
            Console.WriteLine("'{0}'", e.ToString("G"));

            Console.WriteLine("\n\n");
            Console.WriteLine("'{0, 5:f}'", e2);
            Console.WriteLine("'{0, 5:d}'", e2);
            Console.WriteLine("'{0, 5:g}'", e2);
            Console.WriteLine("'{0, 5:x}'", e2);
            Console.WriteLine("'{0, 5:X}'", e2);
            Console.WriteLine("'{0}'", e2);
            Console.WriteLine("'{0}'", e2.ToString("F"));
            Console.WriteLine("'{0}'", e2.ToString("G"));
        }
        public static void Run()
        {
            BuiltinTypes();
            //Writing();
            //Reading();
            //Formating();

            Console.WriteLine("\n------------------- press any key to exit\n");
            Console.ReadKey();
        }
    }
}
