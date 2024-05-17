using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jongol.Days
{
    class Day03
    {
        //built-in types
        //suffix : u, l, ul, f, d m
        static string DigitString(int size)
        {
            int digit = size * 8;
            StringBuilder s = new StringBuilder(digit);
            for (int i = 1; i <= digit; ++i)
                s.Append((i % 10).ToString());
            string sdigit = new string(s.ToString().Reverse().ToArray());

            return sdigit;
        }
        static void BuiltinType_int()
        {
            int a = 3;
            string sval = "8055";

            Console.WriteLine("\n-------- int");
            Console.WriteLine("sizeof : {0} bytes", sizeof(int));
            Console.WriteLine("typeof : {0}", typeof(int));
            Console.WriteLine("GetType : {0}", a.GetType());

            string sdigit = DigitString(sizeof(int));

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", int.MinValue);
            Console.WriteLine(" 0x{0:X}", int.MinValue);
            Console.WriteLine(" {0,32}", sdigit);
            Console.WriteLine(" {0,32}\n", Convert.ToString(int.MinValue, 2));

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", int.MaxValue);
            Console.WriteLine(" 0x{0:X}", int.MaxValue);
            Console.WriteLine(" {0,32}", sdigit);
            Console.WriteLine(" {0,32}\n", Convert.ToString(int.MaxValue, 2));

            Console.WriteLine("val : {0}", a);
            a = int.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToInt32(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_uint()
        {
            uint a = 3;
            string sval = "8055";

            Console.WriteLine("\n-------- uint");
            Console.WriteLine("sizeof : {0} bytes", sizeof(uint));
            Console.WriteLine("typeof : {0}", typeof(uint));
            Console.WriteLine("GetType : {0}", a.GetType());

            string sdigit = DigitString(sizeof(uint));

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", uint.MinValue);
            Console.WriteLine(" 0x{0:X}", uint.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", uint.MaxValue);
            Console.WriteLine(" 0x{0:X}", uint.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = uint.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToUInt32(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_short()
        {
            short a = 3;
            string sval = "8055";

            Console.WriteLine("\n-------- short");
            Console.WriteLine("sizeof : {0} bytes", sizeof(short));
            Console.WriteLine("typeof : {0}", typeof(short));
            Console.WriteLine("GetType : {0}", a.GetType());

            string sdigit = DigitString(sizeof(short));

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", short.MinValue);
            Console.WriteLine(" 0x{0:X}", short.MinValue);
            Console.WriteLine(" {0,16}", sdigit);
            Console.WriteLine(" {0,16}\n", Convert.ToString(short.MinValue, 2));

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", short.MaxValue);
            Console.WriteLine(" 0x{0:X}", short.MaxValue);
            Console.WriteLine(" {0,16}", sdigit);
            Console.WriteLine(" {0,16}\n", Convert.ToString(short.MaxValue, 2));

            Console.WriteLine("val : {0}", a);
            a = short.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToInt16(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_ushort()
        {
            ushort a = 3;
            string sval = "8055";

            Console.WriteLine("\n-------- ushort");
            Console.WriteLine("sizeof : {0} bytes", sizeof(ushort));
            Console.WriteLine("typeof : {0}", typeof(ushort));
            Console.WriteLine("GetType : {0}", a.GetType());

            string sdigit = DigitString(sizeof(ushort));

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", ushort.MinValue);
            Console.WriteLine(" 0x{0:X}", ushort.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", ushort.MaxValue);
            Console.WriteLine(" 0x{0:X}", ushort.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = ushort.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToUInt16(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_sbyte()
        {
            sbyte a = 3;
            //string sval = "8055";
            string sval = "55";

            Console.WriteLine("\n-------- sbyte");
            Console.WriteLine("sizeof : {0} bytes", sizeof(sbyte));
            Console.WriteLine("typeof : {0}", typeof(sbyte));
            Console.WriteLine("GetType : {0}", a.GetType());

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", sbyte.MinValue);
            Console.WriteLine(" 0x{0:X}", sbyte.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", sbyte.MaxValue);
            Console.WriteLine(" 0x{0:X}", sbyte.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = sbyte.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToSByte(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_byte()
        {
            byte a = 3;
            //string sval = "8055";
            string sval = "55";

            Console.WriteLine("\n-------- byte");
            Console.WriteLine("sizeof : {0} bytes", sizeof(byte));
            Console.WriteLine("typeof : {0}", typeof(byte));
            Console.WriteLine("GetType : {0}", a.GetType());

            string sdigit = DigitString(sizeof(byte));

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", byte.MinValue);
            Console.WriteLine(" 0x{0:X}", byte.MinValue);
            Console.WriteLine(" {0,8}", sdigit);
            Console.WriteLine(" {0,8}\n", Convert.ToString(byte.MinValue, 2));

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", byte.MaxValue);
            Console.WriteLine(" 0x{0:X}", byte.MaxValue);
            Console.WriteLine(" {0,8}", sdigit);
            Console.WriteLine(" {0,8}\n", Convert.ToString(byte.MaxValue, 2));

            Console.WriteLine("val : {0}", a);
            a = byte.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToByte(sval);
            Console.WriteLine("val : {0}", a);
        }

        static void BuiltinType_long()
        {
            long a = 3;
            string sval = "8055";

            Console.WriteLine("\n-------- long");
            Console.WriteLine("sizeof : {0} bytes", sizeof(long));
            Console.WriteLine("typeof : {0}", typeof(long));
            Console.WriteLine("GetType : {0}", a.GetType());

            string sdigit = DigitString(sizeof(long));

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", long.MinValue);
            Console.WriteLine(" 0x{0:X}", long.MinValue);
            Console.WriteLine(" {0,64}", sdigit);
            Console.WriteLine(" {0,64}\n", Convert.ToString(long.MinValue, 2));

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", long.MaxValue);
            Console.WriteLine(" 0x{0:X}", long.MaxValue);
            Console.WriteLine(" {0,64}", sdigit);
            Console.WriteLine(" {0,64}\n", Convert.ToString(long.MaxValue, 2));

            Console.WriteLine("val : {0}", a);
            a = long.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToInt64(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_ulong()
        {
            ulong a = 3;
            string sval = "8055";

            Console.WriteLine("\n-------- ulong");
            Console.WriteLine("sizeof : {0} bytes", sizeof(ulong));
            Console.WriteLine("typeof : {0}", typeof(ulong));
            Console.WriteLine("GetType : {0}", a.GetType());

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", ulong.MinValue);
            Console.WriteLine(" 0x{0:X}", ulong.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", ulong.MaxValue);
            Console.WriteLine(" 0x{0:X}", ulong.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = ulong.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToUInt64(sval);
            Console.WriteLine("val : {0}", a);
        }

        static void BuiltinType_float()
        {
            float a = 3.2f;
            string sval = "8055.7";

            Console.WriteLine("\n-------- float");
            Console.WriteLine("sizeof : {0} bytes", sizeof(float));
            Console.WriteLine("typeof : {0}", typeof(float));
            Console.WriteLine("GetType : {0}", a.GetType());

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", float.MinValue);
            Console.WriteLine(" {0:F}", float.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", float.MaxValue);
            Console.WriteLine(" {0:F}", float.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = float.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToSingle(sval);
            Console.WriteLine("val : {0}", a);
        }

        static void BuiltinType_double()
        {
            double a = 3.2f;
            string sval = "8055.7";

            Console.WriteLine("\n-------- double");
            Console.WriteLine("sizeof : {0} bytes", sizeof(double));
            Console.WriteLine("typeof : {0}", typeof(double));
            Console.WriteLine("GetType : {0}", a.GetType());

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", double.MinValue);
            Console.WriteLine(" {0:F}", double.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", double.MaxValue);
            Console.WriteLine(" {0:F}", double.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = double.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToDouble(sval);
            Console.WriteLine("val : {0}", a);
        }

        static void BuiltinType_decimal()
        {
            decimal a = 3.2m;
            string sval = "8055.7";

            Console.WriteLine("\n-------- decimal");
            Console.WriteLine("sizeof : {0} bytes", sizeof(decimal));
            Console.WriteLine("typeof : {0}", typeof(decimal));
            Console.WriteLine("GetType : {0}", a.GetType());

            Console.WriteLine("MinValue");
            Console.WriteLine(" {0}", decimal.MinValue);
            Console.WriteLine(" {0:F}", decimal.MinValue);

            Console.WriteLine("MaxValue");
            Console.WriteLine(" {0}", decimal.MaxValue);
            Console.WriteLine(" {0:F}", decimal.MaxValue);

            Console.WriteLine("val : {0}", a);
            a = decimal.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToDecimal(sval);
            Console.WriteLine("val : {0}", a);
        }
        static void BuiltinType_bool()
        {
            bool a = true;
            string sval = "false";

            Console.WriteLine("\n-------- bool");
            Console.WriteLine("sizeof : {0} bytes", sizeof(bool));
            Console.WriteLine("typeof : {0}", typeof(bool));
            Console.WriteLine("GetType : {0}", a.GetType());
            
            Console.WriteLine("val : {0}", a);
            a = bool.Parse(sval);
            Console.WriteLine("val : {0}", a);
            a = Convert.ToBoolean(sval);
            Console.WriteLine("val : {0}", a);
        }

        static void BuiltinTypes()
        {
            //for interger
            BuiltinType_int();
            BuiltinType_uint();
            BuiltinType_short();
            BuiltinType_ushort();
            BuiltinType_sbyte();
            BuiltinType_byte();
            BuiltinType_long();

            //for realnumber
            BuiltinType_float();
            BuiltinType_double();
            BuiltinType_decimal();

            //bool
            BuiltinType_bool();
        }


        public static void Run()
        {
            BuiltinTypes();

            //string s = DigitString(sizeof(int));
            //Console.WriteLine(s);
            //Console.WriteLine("{0,32}", Convert.ToString( 1, 2));
            //Console.WriteLine("{0,32}", Convert.ToString(-1, 2));
            //Console.WriteLine("{0,32}", Convert.ToString( 2, 2));
            //Console.WriteLine("{0,32}", Convert.ToString(-2, 2));


            ////suffix : u, l, ul, f, d m
            //Console.WriteLine("{0}", 3.GetTypeCode());
            //Console.WriteLine("{0}", 3u.GetTypeCode());
            //Console.WriteLine("{0}", 3l.GetTypeCode());
            //Console.WriteLine("{0}", 3ul.GetTypeCode());
            //Console.WriteLine("{0}", 3f.GetTypeCode());
            //Console.WriteLine("{0}", 3d.GetTypeCode());
            //Console.WriteLine("{0}", 3m.GetTypeCode());
            //
            //Console.WriteLine("{0}", 3.GetTypeCode());
            //Console.WriteLine("{0}", 3U.GetTypeCode());
            //Console.WriteLine("{0}", 3L.GetTypeCode());
            //Console.WriteLine("{0}", 3UL.GetTypeCode());
            //Console.WriteLine("{0}", 3F.GetTypeCode());
            //Console.WriteLine("{0}", 3D.GetTypeCode());
            //Console.WriteLine("{0}", 3M.GetTypeCode());
        }
    }
}
