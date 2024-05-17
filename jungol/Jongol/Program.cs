using Jongol;
using System;

namespace Jungol
{
    class Program
    {
        class TimePeriod
        {
            private double _seconds;

            public double Hours
            {
                get { return _seconds / 3600; }
                set
                {
                    if (value < 0 || value > 24)
                    {
                        System.Console.WriteLine($"{nameof(value)} must be between 0 and 24.");
                        return;
                    }
                    _seconds = value * 3600;
                }
            }
        }

        public class Tst: IDisposable
        {
            public void Test()
            {
                Console.WriteLine("Test");
            }
            public void Dispose()
            {
                Console.WriteLine("disposed");
            }
        }

        static void Main_()
        {
            //Basic.Test.Run();
            //SkillUp.Run();

            //Collections.Run();
            //Algorithm.Run();

            //Jongol.Days.Day01.Run();
            //Jongol.Days.Day02.Run();
            //Jongol.Days.Day03.Run();

            //_2020_1st_final.Run();
        }
    }
}
