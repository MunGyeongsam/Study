using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.LanguageCSharp
{
    enum Enum1
    {
        ITEM_0,
        ITEM_1, 
        ITEM_2,
        
        ITEM_4  = 4, 
        ITEM_5,

        ITEM_MAX
    }

    enum Fruit : short
    {
        Apple,
        Orange,
        Watermelon,
    }

    [Flags]
    enum WorkingWeek : long
    {
        Monday      = 1L << 0,
        Tuesday     = 1L << 1,
        Wednesday   = 1L << 2,
        Thursday    = 1L << 3,
        Friday      = 1L << 4,
        Saturday    = 1L << 5,
        Sunday      = 1L << 6,
    }

    static class Ext
    {
        public static string ShowFlag(this WorkingWeek workingWeek)
        {
            byte[] bytes = BitConverter.GetBytes((long)workingWeek);
            Array.Reverse(bytes);

            string bin = BitConverter.ToString(bytes);

            bin = Convert.ToString((long)workingWeek, 2);
            bin = bin.PadLeft(64, '0');
            return bin;
        }
    }

    internal class _04_Enum
    {
        public static void Test()
        {
            Console.WriteLine("-----------");
            Enum1 e1 = Enum1.ITEM_1;
            Console.WriteLine(e1);
            e1 = (Enum1)3;
            Console.WriteLine(e1);
            e1 = (Enum1)4;
            Console.WriteLine(e1);

            Console.WriteLine("-----------");
            Fruit e2 = Fruit.Apple;
            Console.WriteLine(e2);
            e2 = (Fruit)(2);
            Console.WriteLine(e2);


            Console.WriteLine("-----------");
            WorkingWeek e3 = WorkingWeek.Monday;
            Console.WriteLine($"{e3.ShowFlag()} {e3}");
            e3 |= WorkingWeek.Wednesday;
            Console.WriteLine($"{e3.ShowFlag()} {e3}");
            Console.WriteLine($"{e3 & WorkingWeek.Tuesday}");
            Console.WriteLine($"{e3 & WorkingWeek.Monday}");
            Console.WriteLine($"{e3.HasFlag(WorkingWeek.Tuesday)}");
        }
    }
}
