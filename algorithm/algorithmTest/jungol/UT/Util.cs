using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace jungol.UT
{
    static class Util
    {
        static void PrintInt(int n)
        {
            string bin = Convert.ToString(n, 2);
            bin = bin.PadLeft(32, '0');

            byte[] bytes = BitConverter.GetBytes(n);
            string bin2 = BitConverter.ToString(bytes);

            Array.Reverse(bytes);
            string bin3 = BitConverter.ToString(bytes);

            Console.WriteLine("{1}, {2} ({3}) : {0}", n, bin, bin2, bin3);
        }

        static void PrintFloat(float a)
        {

            byte[] bytes = BitConverter.GetBytes(a);
            string bin2 = BitConverter.ToString(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte b = bytes[i];
                for (int j = 0; j < 8; ++j)
                {
                    sb.Insert(0, ((b >> j) & 1) == 1 ? "1" : "0");
                }
            }
            string s = sb.ToString();
            string exp = s.Substring(1, 8);
            string man = s.Substring(9);
            string bin = s.Substring(0, 1) + " " + s.Substring(1, 8) + " " + s.Substring(9); //sign exponent mantissa

            int e = Convert.ToInt32(exp, 2);
            int m = Convert.ToInt32(man, 2);

            Array.Reverse(bytes);
            string bin3 = BitConverter.ToString(bytes);

            Console.WriteLine("{1}, {2} ({3}) : {0}, exponent = {4}, mantissa = {5}", a, bin, bin2, bin3, e, m);
        }

        public static void PrintFunction()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");

            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
            Console.WriteLine("{0}.{1}", methodBase.DeclaringType.FullName, methodBase.Name);
            Console.WriteLine("---------------------------------------------------");
        }

        public static void Call(Action a)
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine("{0} {1}", a.Method.ReflectedType.FullName, a.Method.Name);

            Console.WriteLine("---------------------------------------------------");
            a();
        }

        public static void Shuffle<T>(Random rnd, T[] arr, int CNT = -1)
        {
            if (CNT < 0)
            {
                CNT = Math.Max(999, arr.Length * 10);
            }

            int LEN = arr.Length;
            for (int i = 0; i < CNT; ++i)
            {
                int i0 = rnd.Next(LEN);
                int i1 = rnd.Next(LEN);
                while (i0 == i1)
                    i1 = rnd.Next(LEN);

                T tmp = arr[i0];
                arr[i0] = arr[i1];
                arr[i1] = tmp;
            }
        }

        public static void PrintArray<T>(T[] arr)
        {
            PrintArray(arr, 1, " ");
        }
        public static void PrintArray<T>(T[,] arr)
        {
            PrintArray(arr, 1, " ");
        }
        public static void PrintArray<T>(T[] arr, int width)
        {
            PrintArray(arr, width, " ");
        }
        public static void PrintArray<T>(T[,] arr, int width)
        {
            PrintArray(arr, width, " ");
        }
        public static void PrintArray<T>(T[] arr, string SEP)
        {
            PrintArray(arr, 1, SEP);
        }
        public static void PrintArray<T>(T[,] arr, string SEP)
        {
            PrintArray(arr, 1, SEP);
        }

        public static void PrintArray<T>(T[] arr, int width, string SEP)
        {
            string FMT = string.Format("{{0,{0}}}{1}", width, SEP);
            //"{0,2} "
            Console.WriteLine();
            Console.WriteLine("[{0}]", arr.Length);

            for (int i = 0; i < arr.Length; ++i)
                Console.Write(FMT, arr[i]);
            Console.WriteLine();
        }

        public static void PrintArray<T>(T[,] arr, int width, string SEP)
        {
            string FMT = string.Format("{{0,{0}}}{1}", width, SEP);

            int ROW = arr.GetLength(0);
            int COL = arr.GetLength(1);

            Console.WriteLine();
            Console.WriteLine("[{0} x {1}]", ROW, COL);

            for (int i = 0; i < ROW; ++i)
            {
                for (int j = 0; j < COL; ++j)
                {
                    Console.Write(FMT, arr[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void PrintArray2<T>(T[][] arr, int width = 1, string SEP = "")
        {
            string FMT = string.Format("{{0,{0}}}{1}", width, SEP);

            int ROW = arr.Length;
            Console.WriteLine("ROW : {0}", ROW);
            for (int i = 0; i < ROW; ++i)
            {
                int COL = arr[i].Length;
                Console.Write("[{0, 2}] ({1}): ", i, COL);
                for (int j = 0; j < COL; ++j)
                {
                    Console.Write(FMT, arr[i][j]);
                }
                Console.WriteLine();
            }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}
