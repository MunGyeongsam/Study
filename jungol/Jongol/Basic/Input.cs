using System;

namespace Jungol
{
    // 509	입력 - 자가진단1  
    // 510	입력 - 자가진단2 
    // 511	입력 - 자가진단3 
    // 512	입력 - 자가진단4 
    // 513	입력 - 자가진단5 
    // 514	입력 - 자가진단6 
    // 515	입력 - 자가진단7 
    // 516	입력 - 자가진단8 
    // 517	입력 - 자가진단9 
    // 106	입력 - 형성평가1 
    // 107	입력 - 형성평가2 
    // 108	입력 - 형성평가3 
    // 109	입력 - 형성평가4 
    // 110	입력 - 형성평가5 

    static class Input
	{
		
		public static void Test()
		{
            Util.Call(_509);
            Util.Call(_510);
            Util.Call(_511);
            Util.Call(_512);
            Util.Call(_513);
            Util.Call(_514);
            Util.Call(_515);
            Util.Call(_516);
            Util.Call(_517);
            Util.Call(_106);
            Util.Call(_107);
            Util.Call(_108);
            Util.Call(_109);
            Util.Call(_110);
        }



        // 509	입력 - 자가진단1 
        // 정수형 변수를 선언하고 -100을 대입하여 출력하는 프로그램을 작성하라.

        //-100
        static void _509()
        {
            int val = -100;
            Console.WriteLine(val);
        }


        // 510	입력 - 자가진단2 
        // 정수형 변수 2개를 선언하여 -1과 100을 대입한 후 아래와 같이 출력하는 프로그램을 작성하라.

        //-1
        //100
        static void _510()
        {            
            int n1 = -1;
            int n2 = 100;

            Console.WriteLine(n1);
            Console.WriteLine(n2);
        }


        // 511	입력 - 자가진단3 
        // 두 개의 정수형 변수를 선언하고 값을 대입하여 아래와 같이 출력되는 프로그램을 작성하라.
        
        // 55 - 10 = 45
        // 2008 - 1999 = 9
        static void _511()
        {
            int n1 = 55 - 10;
            int n2 = 2008 - 1999;

            Console.WriteLine("55 - 10 = {0}", n1);
            Console.WriteLine("2008 - 1999 = {0}", n2);
        }

        // 512	입력 - 자가진단4 
        // 다음 두 값을 변수에 저장하고 곱셈 계산식을 출력하는 프로그램을 작성하라.
        // 추의 무게 = 49, 중력의 비율 = 0.2683

        //49 * 0.268300 = 13.146700
        static void _512()
        {
            float w = 49f;
            float g = 13.1467f;

            Console.WriteLine("{0} * {1:f6} = {2:f6}", w, g, w*g);
        }

        // 513	입력 - 자가진단5 
        // 1야드(yd)는 91.44cm이고 1인치(in)는 2.54cm이다.
        // 2.1야드와 10.5인치를 각각 cm로 변환하여 다음 형식에 맞추어 소수 첫째자리까지 출력하시오.​

        // 2.1yd = 192.0cm
        //10.5in =  26.7cm
        static void _513()
        {
            float yd2cm = 91.44f;
            float in2cm = 2.54f;
            Console.WriteLine("{0,4:f1}yd = {1,5:f1}cm", 2.1f, 2.1f * yd2cm);
            Console.WriteLine("{0,4:f1}in = {1,5:f1}cm", 10.5f, 10.5f * in2cm);
        }

        // 514	입력 - 자가진단6 
        // 키를 입력받아 출력하는 프로그램을 작성하라.

        // height = 170
        // Your height is 170cm.
        static void _514()
        {
            Console.Write("height = ");
            //@{ 테스트를 위해 Console.ReadLine 을 문자열로 직접 입력
            //string input = Console.ReadLine();
            string input = "170";
            Console.WriteLine(input);
            //@}

            float height = Convert.ToSingle(input);
            Console.WriteLine("Your height is {0}cm.", height);
        }


        // 515	입력 - 자가진단7 
        // 두 개의 정수를 입력 받아 곱과 몫을 출력하시오.
        // (먼저 입력 받는 수가 항상 크며 입력되는 두 정수는 1이상 500이하이다.)

        //16 5
        //16 * 5 = 80
        //16 / 5 = 3
        static void _515()
        {
            //@{ 테스트를 위해 Console.ReadLine 을 문자열로 직접 입력
            //string input = Console.ReadLine();
            string input = "16 5";
            //@}

            string[] arr = input.Split();
            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            Console.WriteLine("{0} * {1} = {2}", n1, n2, n1 * n2);
            Console.WriteLine("{0} / {1} = {2}", n1, n2, n1 / n2);
        }


        // 516	입력 - 자가진단8 
        // 실수 2개와 한 개의 문자를 입력 받아 출력하되 실수는 반올림하여 소수 둘째자리까지 출력하는 프로그램을작성하시오.

        //12.2536
        //526.129535
        //A
        
        //12.25
        //526.13
        //A
        static void _516()
        {
            float f1, f2;
            char ch;

            string input;

            //input = Console.ReadLine();
            input = "12.2536";
            f1 = Convert.ToSingle(input);

            //input = Console.ReadLine();
            input = "526.129535";
            f2 = Convert.ToSingle(input);

            //input = Console.ReadLine();
            input = "A";
            ch = Convert.ToChar(input);

            Console.WriteLine("{0:f2}", f1);
            Console.WriteLine("{0:f2}", f2);
            Console.WriteLine("{0}", ch);

        }

        // 517	입력 - 자가진단9 
        // 두 개의 float형 실수와 한 개의 double형 실수를 입력 받아 소수 3째 자리까지 출력하는 프로그램을 작성하시오.
        static void _517()
        {
            float f1, f2;
            double d1;

            string input;

            //input = Console.ReadLine();
            input = "1.2568";
            f1 = Convert.ToSingle(input);

            //input = Console.ReadLine();
            input = "95.21438";
            f2 = Convert.ToSingle(input);

            //input = Console.ReadLine();
            input = "526.851364";
            d1 = Convert.ToDouble(input);

            Console.WriteLine("{0:f3}", f1);
            Console.WriteLine("{0:f3}", f2);
            Console.WriteLine("{0:f3}", d1);
        }

        // 106	입력 - 형성평가1 
        // 세 개의 정수형 변수를 선언하고 각 변수에 10 20 30을 대입한 후 그 변수를 이용하여 출력 예와 같이 출력하는 프로그램을 작성하시오.
        // 10 + 20 = 30
        static void _106()
        {
            int n1 = 10;
            int n2 = 20;
            int n3 = 30;

            Console.WriteLine("{0} + {1} = {2}", n1, n2, n3);
        }


        // 107	입력 - 형성평가2 
        // 실수형 변수를 2개 선언한 후 각각에 80.5  22.34를 대입한 후 두 수의 합을 구하여 각각의 숫자를 10칸씩 오른쪽에 맞추어 소수 둘째자리까지 출력하는 프로그램을 작성하시오.
        static void _107()
        {
            float f1 = 80.5f;
            float f2 = 22.34f;
            float sum = f1 + f2;

            Console.WriteLine("{0,10:f2}{1,10:f2}{2,10:f2}", f1, f2, sum);
        }


        // 108	입력 - 형성평가3 
        // 정수형 변수 한 개를 선언하여 50을 대입하고 실수형 변수 한 개를 선언하여 100.12를 대입한 후 다음과 같이 출력되는 프로그램을 작성하시오.
        // 100.12 * 50 = 5006
        static void _108()
        {
            int n = 50;
            float f = 100.12f;
            Console.WriteLine("{0} * {1} = {2}", f, n, f * n);
        }


        // 109	입력 - 형성평가4 
        // 세 개의 정수를 입력받아 합과 평균을 출력하는 프로그램을 작성하시오.(단 평균은 소수 이하를 버림하여 정수 부분만 출력한다.)
        static void _109()
        {

            //string input = Console.ReadLine();
            string input = "20 50 100";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);
            int n3 = Convert.ToInt32(arr[2]);
            int sum = n1 + n2 + n3;
            int avg = sum / 3;
            Console.WriteLine("sum = {0}", sum);
            Console.WriteLine("avg = {0}", avg);
        }
        // 110	입력 - 형성평가5 
        // 실수의 yard(야드)를 입력받아 cm(센티미터)로 환산하여 입력값과 환산한 값을 출력 예와 같이 소수 둘째자리에서 반올림하여 첫째자리까지 출력하는 프로그램을 작성하시오. (단 1야드 = 91.44cm로 한다.) 
        // 실수는 "double"로 하세요.
        // 입력 : yard? 10.1
        // 출력 : 10.1yard = 923.5cm
        static void _110()
        {
            Console.Write("yard? ");

            //string input = Console.ReadLine();
            string input = "10.1";
            Console.WriteLine(input);

            float yard = Convert.ToSingle(input);

            Console.WriteLine("{0}yard = {1:f1}cm", yard, yard * 91.44f);
        }

    }
}