using jungol.Beginner;
using jungol.Challenges;
using jungol.LanguageCSharp;
using System;

namespace jungol
{
    static class MyUtil
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
    class Program
    {
        static void Main()
        {
            //int a1 = 1;
            //int a2 = 2;
            //char c1 = 'A';
            //char c2 = 'B';
            //MyUtil.Swap(ref a1, ref a2);
            //MyUtil.Swap(ref c1, ref c2);
            //Console.WriteLine($"a1 : {a1}, a2 : {a2}");
            //Console.WriteLine($"c1 : {c1}, c2 : {c2}");

            //_01_Geo1.Run();
            //_02_Math1.Run();
            //_03_Geo2.Run();
            //_04_Math2.Run();
            //_05_String.Run();
            //_06_Etc.Run();
            //_07_DataStructure.Run();
            _08_Recursive.Run();
        }
    }
}
