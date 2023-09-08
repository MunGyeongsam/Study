using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace jungol.LanguageCSharp
{
    internal class _01_VariableConstant
    {
        int _memberField;
        const int CONST_FIELD = 0;
        readonly int _readOnlyField;

        public _01_VariableConstant(int readOnly=0)
        {
            _readOnlyField = readOnly;
        }

        public void TestMethod(int param0)
        {
            int localVariable = param0 * 10;
            //_readOnlyField = localVariable;

            _memberField = localVariable;
        }
    }
}
