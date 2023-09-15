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
    }

    public class _9_Inheritance
    {
        public _9_Inheritance()
        {
        }
    }
}
