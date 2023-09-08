using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.LanguageCSharp
{
    internal class _03_string
    {
        static public void Test1()
        {
            string s1 = "abc 123 k  KKK";
            string[] words = s1.Split(' ');

            string s2 = new string('*', 5);
            string s3 = s1 + s2;
            string s4 = s3.Replace('a', 'k');
            string s5 = s4.ToLower();               //ToUpper
            string s6 = s5.Substring(2, 5);
            string s7 = s6.Trim();                  //TrimStart, TrimEnd
            string s8 = s7.PadLeft(20, '-');        //PadRight
        }
    }
}
