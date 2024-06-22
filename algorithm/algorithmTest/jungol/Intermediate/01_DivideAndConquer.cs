using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Intermediate
{
    internal class _01_DivideAndConquer
    {
        public static void Run()
        {
            Util.Call(_3518);
            //Util.Call(_3519);
            //Util.Call(_4523);
            //Util.Call(_4524);
            //Util.Call(_4641);
            //Util.Call(_4642);
            //Util.Call(_3520);
            //Util.Call(_3517);
            //Util.Call(_7009);
            //Util.Call(_5805);
            //Util.Call(_5170);
            //Util.Call(_1219);
            //Util.Call(_4791);
            //Util.Call(_5804);
            //Util.Call(_1335);
            //Util.Call(_3560);
            //Util.Call(_1092);
        }

        //--------------------------------------------------
        // 3518 Tutorial : 퀵소트(Quick Sort 빠른정렬)
        //--------------------------------------------------
        static void Impl_3518(string input)
        {
            var lines = input.Split('\n');
            for(int i = 1; i < lines.Length; i++)
                lines[i] = lines[i].Trim();

            int n  = int.Parse(lines[0]);
            var words = lines[1].Split();

            System.Diagnostics.Debug.Assert(n == words.Length);

            var arr = new int[n];
            for(int i = 0; i < n; i++)
                arr[i] = int.Parse(words[i]);

            int low = 0;
            int high = arr.Length - 1;

            Impl_3518_QuickSort(arr, low, high);
        }
        static void Impl_3518_QuickSort(int[] arr, int low, int high)
        {
            if (low >= high)
                return;

            int pivot = Impl_3518_Partition(arr, low, high);

            Util.PrintArray(arr);

            Impl_3518_QuickSort(arr, low, pivot - 1);
            Impl_3518_QuickSort(arr, pivot + 1, high);
        }
        static int Impl_3518_Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low;

            for(int j=low; j<high-1; ++j)
            {
                if (arr[j] < pivot)
                {
                    ++i;
                    Util.Swap(ref arr[i], ref arr[j]);
                }
            }

            Util.Swap(ref arr[i + 1], ref arr[high]);

            return 0;
        }

        static void _3518()
        {
            string input = @"5
5 2 4 1 3";
            Impl_3518(input);
        }
    }
}
