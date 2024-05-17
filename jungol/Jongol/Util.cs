using System;
using System.Diagnostics;
using System.Reflection;

namespace Jungol
{
    static class Util
    {
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

        public static void Shuffle<T>(Random rnd, T[] arr, int CNT=-1)
        {
            if(CNT < 0)
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

        public static void PrintArray<T>(T[][] arr, int width = 1, string SEP = "")
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
    }
}
