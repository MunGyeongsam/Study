using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.LanguageCSharp
{

    class MyList
    {
        int[] _data = { 1, 2, 3, 4, 5 };

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _data.Length; ++i)
                yield return _data[i];
        }
    }
    internal class _07_Loop
    {
        static void Test()
        {
            var ml = new MyList();
            foreach (var v in ml)
            {
                Console.WriteLine(v);
            }


            foreach (var s in "ho")
            {

            }
        }
    }
}
