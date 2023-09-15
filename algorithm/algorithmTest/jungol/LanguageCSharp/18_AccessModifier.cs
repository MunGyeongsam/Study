using System;
namespace jungol.LanguageCSharp
{

    namespace Private_18_AccessModifier
    {
        class Base
        {
            private int _nv;
            protected int _nt;
            public int _np;
        }

        class Drived : Base
        {
            public void Test()
            {
                //_nv = 3;
                _nt = 3;
                _np = 4;
            }
        }
    }
    public class _8_AccessModifier
    {
        public static void Test()
        {

        }
    }
}
