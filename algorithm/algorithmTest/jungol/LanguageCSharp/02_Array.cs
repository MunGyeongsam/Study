using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace jungol.LanguageCSharp
{
    internal class _02_Array
    {
        public static void Test1()
        {
            int[] arr1 = new int[3] { 1, 2, 3 };
            int[] arr2 = new int[] { 1, 2, 3 };
            int[] arr3 = { 1, 2, 3 };


            Array.Sort(arr1);
            Array.Sort(arr1, (a, b) => b%2 - a%2);
            Array.Reverse(arr1);
            Array.IndexOf(arr1, 2);
            Array.LastIndexOf(arr1, -3);
        }

        public static void Test2()
        {
            int[,] arr1 = new int[3,4] { { 1, 2, 3, 4 }, { 1, 2, 3, 4 }, { 1, 2, 3, 4 } };
            int[,] arr2 = new int[,] { { 1, 2, 3, 4 }, { 1, 2, 3, 4 }, { 1, 2, 3, 4 } };
            int[,] arr3 = { 
                { 1, 2, 3, 4 }, 
                { 1, 2, 3, 4 }, 
                { 1, 2, 3, 4 }
            };

            Console.WriteLine(arr1.Rank);
            Console.WriteLine(arr1.Length);
            Console.WriteLine(arr1.GetLength(0));
            Console.WriteLine(arr1.GetLength(1));
        }

        public static void Test4()
        {
            int[][] arr1 = new int[3][] { new int[] { 1, 2, 3 }, new int[] { 3, 4 }, new int[] { 1, 2, 3, 4, 5 } };
        }
    }
}
