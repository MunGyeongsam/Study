using System.Collections.Generic;

namespace _02_script.w3
{
    public struct Prop
    {
        private byte _flag;
        
        public bool LB { get { return (_flag & 0x01) != 0; } set { _flag = (byte)(value ? _flag | 0x01 : _flag & 0xFE); } }
        public bool RB { get { return (_flag & 0x02) != 0; } set { _flag = (byte)(value ? _flag | 0x02 : _flag & 0xFD); } }
        public bool LT { get { return (_flag & 0x04) != 0; } set { _flag = (byte)(value ? _flag | 0x04 : _flag & 0xFB); } }
        public bool RT { get { return (_flag & 0x08) != 0; } set { _flag = (byte)(value ? _flag | 0x08 : _flag & 0xF7); } }
        
        public bool IsEmpty => _flag == 0;
        public bool IsFull => _flag == 0x0F;
        public bool IsAny => _flag != 0;
        
        public Prop(bool lb, bool rb, bool lt, bool rt)
        {
            _flag = 0;
            LB = lb;
            RB = rb;
            LT = lt;
            RT = rt;
        }
        
        public void Clear()
        {
            _flag = 0;
        }
        
        public override int GetHashCode()
        {
            return _flag;
        }
        
        public bool Equals(Prop other)
        {
            return _flag == other._flag;
        }

        struct Comparer : IEqualityComparer<Prop>
        {
            public bool Equals(Prop lhs, Prop rhs)
            {
                return lhs.Equals(rhs);
            }

            public int GetHashCode(Prop prop)
            {
                return prop.GetHashCode();
            }
        }
    }
}