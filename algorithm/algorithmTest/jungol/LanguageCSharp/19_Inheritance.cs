using jungol.LanguageCSharp.Private_19;
using System;
namespace jungol.LanguageCSharp
{
    namespace Private_19
    {
        public class Animal
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Dog : Animal
        {
            public void HowOld()
            {
                Console.WriteLine("나이: {0}", this.Age);
            }
        }

        public class Bird : Animal
        {
            public void Fly()
            {
                Console.WriteLine("{0}가 날다", this.Name);
            }
        }



        public abstract class PureBase
        {
            // abstract C#키워드 
            public abstract int GetFirst();
            public abstract int GetNext();
        }

        public class DerivedA : PureBase
        {
            private int no = 1;

            // override C#키워드 
            public override int GetFirst()
            {
                return no;
            }

            public override int GetNext()
            {
                return ++no;
            }
        }

        public class DerivedB : PureBase
        {
            private int no = 11;

            // override C#키워드 
            public override int GetFirst()
            {
                return no;
            }

            public override int GetNext()
            {
                return ++no;
            }
        }
    }

    public class _19_Inheritance
    {
        public static void Test()
        {
            object o1 = new DerivedA();

            Console.WriteLine("o1 is PureBase : {0}", o1 is PureBase);
            Console.WriteLine("o1 is DerivedA : {0}", o1 is DerivedA);
            Console.WriteLine("o1 is DerivedB : {0}", o1 is DerivedB);

            var p = o1 as PureBase;
            var a = o1 as DerivedA;
            var b = o1 as DerivedB;

            Console.WriteLine(p == null);
            Console.WriteLine(a == null);
            Console.WriteLine(b == null);
        }
    }
}
