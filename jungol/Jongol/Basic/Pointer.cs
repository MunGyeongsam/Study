using System;

namespace Jungol
{
    // 619	포인터 - 자가진단1
    // 620	포인터 - 자가진단2
    // 621	포인터 - 자가진단3
    // 622	포인터 - 자가진단4
    // 623	포인터 - 자가진단5
    // 624	포인터 - 자가진단6
    // 625	포인터 - 자가진단7
    // 200	포인터 - 형성평가1
    // 201	포인터 - 형성평가2
    // 202	포인터 - 형성평가3
    // 203	포인터 - 형성평가4
    // 204	포인터 - 형성평가5

    // 포인터 연산은 C 또는 C++ 에서 메모리 관리 및 조작에 유용하고 흔하게 사용되지만
    // 
    // 프로그래머가 직접 메모리를 조작할 이유도 방법도 없으므로 C# 에서는 거의 사용하지 안음.
    //
    // 아래 풀이는 다소 억지스러운 감이 있지만 예제로서 방법정도를 소개하기 위함이다.
    // 
    // C# 에서 포인터를 사용하기 위해선 unsafe 블락으로 묶어야 하며
    // 주소를 고정하기 위해 fixed 를 사용해야 한다.
    // 
    // 빌드 옵션도 unsafe 속성을 활성화 시켜야 함.
    // google : /unsafe (C# Compiler Options)
    // https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/compiler-options/unsafe-compiler-option
    static class Pointer
	{
		
		public static void Test()
        {
            Util.Call(_619);
            Util.Call(_620);
            Util.Call(_621);
            Util.Call(_622);
            Util.Call(_623);
            Util.Call(_624);
            Util.Call(_625);
            Util.Call(_200);
            Util.Call(_201);
            Util.Call(_202);
            Util.Call(_203);
            Util.Call(_204);
        }

        //--------------------------------------------------
        // 619	포인터 - 자가진단1
        //
        // 정수형 변수와 포인터 변수를 선언하고 정수를 입력받아 포인터 변수를 이용하여 정수형 변수의 메모리 주소와 값을 출력하는 프로그램을 작성하시오.
        // 주소 출력은 "%#p"를 이용하시오.아래는 메모리 주소가 "0X11110000"이라고 가정했을 때이다.
        //
        // 입력 예 :
        // 20
        //
        // 출력 예 :
        // 0X11110000 20
        //
        //--------------------------------------------------
        static void _619()
        {
            int a = 20;
            unsafe
            {
                int* p = &a;
                Console.WriteLine("0x{0:X8} {1}", (uint)p, a);
            }
        }


        //--------------------------------------------------
        // 620	포인터 - 자가진단2
        //
        // 정수형 포인터를 이용하여 값을 입력받고 입력받은 값을 10으로 나눈 몫과 나머지를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 123
        //
        // 출력 예 :
        // 12...3
        //
        //--------------------------------------------------
        static void _620()
        {
            unsafe
            {
                int a = 0;
                int* p = &a;

                *p = 123;

                Console.WriteLine("{0}...{1}", *p / 10, *p % 10);
            }
        }



        //--------------------------------------------------
        // 621	포인터 - 자가진단3
        //
        // 포인터 변수 두 개를 이용하여 두 수를 입력받고 사칙 연산을 수행하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 35 6
        //
        // 출력 예 :
        // 35 + 6 = 41
        // 35 - 6 = 29
        // 35 * 6 = 210
        // 35 / 6 = 5
        //
        //--------------------------------------------------
        static void _621()
        {
            unsafe
            {
                int[] a = new int[2];

                fixed(int* pa = a)
                {
                    *(pa + 0) = 35;   //a[0] = 35; or *pa = 35;
                    *(pa + 1) = 6;    //a[1] = 6;

                    //Console.WriteLine("{0} + {1} = {2}", *pa, *(pa + 1), *pa + *(pa + 1));
                    //Console.WriteLine("{0} - {1} = {2}", *pa, *(pa + 1), *pa - *(pa + 1));
                    //Console.WriteLine("{0} * {1} = {2}", *pa, *(pa + 1), *pa * *(pa + 1));
                    //Console.WriteLine("{0} / {1} = {2}", *pa, *(pa + 1), *pa / *(pa + 1));
                }

                Console.WriteLine("{0} + {1} = {2}", a[0], a[1], a[0] + a[1]);
                Console.WriteLine("{0} - {1} = {2}", a[0], a[1], a[0] - a[1]);
                Console.WriteLine("{0} * {1} = {2}", a[0], a[1], a[0] * a[1]);
                Console.WriteLine("{0} / {1} = {2}", a[0], a[1], a[0] / a[1]);

            }

        }



        //--------------------------------------------------
        // 622	포인터 - 자가진단4
        //
        // 5개짜리 정수형 배열을 선언하고 포인터 변수에 저장한 후 포인터 변수를 이용하여 입력을 받은 후 홀수번째 입력값을 출하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 1 15 23 9 12
        //
        // 출력 예 :
        // 1 23 12
        //
        //--------------------------------------------------
        static void _622()
        {
            int[] a = { 1, 15, 23, 9, 12 };

            unsafe
            {
                fixed (int* pa = a)
                {
                    Console.WriteLine("{0} {1} {2}", *pa, *(pa + 2), *(pa + 4));
                }
            }
        }



        //--------------------------------------------------
        // 623	포인터 - 자가진단5
        //
        // 실수 5개를 원소로 하는 배열을 선언하고 포인터 변수를 이용하여 입력과 출력을 실행하는 프로그램을 작성하시오  출력은 반올림하여 소수 첫째자리까지 출력하는 것으로 한다.
        //
        // 입력 예 :
        // 10 25.3 1.05 0.78 100
        //
        // 출력 예 :
        // 10.0 25.3 1.1 0.8 100.0
        //
        //--------------------------------------------------
        static void _623()
        {
            string input = "10 25.3 1.05 0.78 100";
            string[] arg = input.Split();

            double[] a = new double[arg.Length];

            unsafe
            {
                fixed (double* pa = a)
                {
                    *(pa + 0) = Convert.ToDouble(arg[0]);    //a[0] = Convert.ToInt32(arg[0]);
                    *(pa + 1) = Convert.ToDouble(arg[1]);    //a[1] = Convert.ToInt32(arg[1]);
                    *(pa + 2) = Convert.ToDouble(arg[2]);    //a[2] = Convert.ToInt32(arg[2]);
                    *(pa + 3) = Convert.ToDouble(arg[3]);    //a[3] = Convert.ToInt32(arg[3]);
                    *(pa + 4) = Convert.ToDouble(arg[4]);    //a[4] = Convert.ToInt32(arg[4]);

                    Console.Write("{0:f1} {1:f1} {2:f1} {3:f1} {4:f1}"
                        , *pa
                        , *(pa + 1)
                        , *(pa + 2)
                        , *(pa + 3)
                        , *(pa + 4));
                }
            }
        }



        //--------------------------------------------------
        // 624	포인터 - 자가진단6
        //
        // 배열의 크기를 입력받아 입력받은 크기만큼 실수 배열을 생성하고 배열의 원소를 입력받은 후 입력받은 자료 및 합과 평균을 반올림하여 소수 둘째자리까지 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 4
        // 15 23.6 100.35 0.388
        //
        // 출력 예 :
        // 15.00 23.60 100.35 0.39
        // hap : 139.34
        // avg : 34.83
        //
        //--------------------------------------------------
        static void _624()
        {
            string input_n = "4";
            int n = Convert.ToInt32(input_n);

            double[] a = new double[n];

            string input_s = "15 23.6 100.35 0.388";
            string[] args = input_s.Split();

            for (int i = 0; i < n; ++i)
                a[i] = Convert.ToDouble(args[i]);

            double sum = 0;
            for (int i = 0; i < n; ++i)
                sum += a[i];

            double avg = sum / n;

            for (int i = 0; i < n; ++i)
                Console.Write("{0:f2} ", a[i]);
            Console.WriteLine();

            Console.WriteLine("hap : {0:f2}", sum);
            Console.WriteLine("avg : {0:f2}", avg);
        }



        //--------------------------------------------------
        // 625	포인터 - 자가진단7
        //
        // 배열의 크기를 입력받아 입력받은 크기만큼 배열에 정수를 입력받고 내림차순으로 정렬하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 4
        // 15 23 100 38
        //
        // 출력 예 :
        // 100 38 23 15
        //
        //--------------------------------------------------
        static void _625()
        {
            string input_n = "4";
            int n = Convert.ToInt32(input_n);

            int[] a = new int[n];

            string input_s = "15 23 100 38";
            string[] args = input_s.Split();


            for (int i = 0; i < n; ++i)
                a[i] = Convert.ToInt32(args[i]);

            Array.Sort(a, new Comparison<int>( (n1, n2) => n2.CompareTo(n1)));
            Console.WriteLine(string.Join(" ", a));
        }



        //--------------------------------------------------
        // 200	포인터 - 형성평가1
        //
        // 문자와 실수를 각각 선언 하고 그 주소를 출력하는 프로그램을 작성하시오.
        //
        // 출력 예 :
        // 11111 22222
        //
        //--------------------------------------------------
        static void _200()
        {
            char a;
            double b;

            unsafe
            {
                Console.WriteLine("{0:X8} {1:X8}", (uint)&a, (uint)&b);
            }
        }



        //--------------------------------------------------
        // 201	포인터 - 형성평가2
        //
        // 정수형 변수를 선언하고 포인터 변수를 사용하여 100 이하의 수를 입력받은 후 입력받은 수만큼 ‘*’을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10
        //
        // 출력 예 :
        // **********
        //
        //--------------------------------------------------
        static void _201()
        {
            int n;
            unsafe
            {
                int* p = &n;
                *p = 10;
            }
            
            Console.WriteLine(new string('*', n));
        }



        //--------------------------------------------------
        // 202	포인터 - 형성평가3
        //
        // 세 개의 포인터 변수를 선언하고 메모리 공간을 확보하여 두 수를 입력받아 두 수의 차의 절대값을 저장한 후 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 15 25
        //
        // 출력 예 :
        // 10
        //
        //--------------------------------------------------
        static void _202()
        {
            string input = "15 25";
            string[] args = input.Split();

            int[] a = new int[2];

            unsafe
            {
                fixed (int* p = a)
                {
                    *p = Convert.ToInt32(args[0]);
                    *(p+1) = Convert.ToInt32(args[1]);
                }
            }

            Console.WriteLine(Math.Abs(a[0] - a[1]));
        }



        //--------------------------------------------------
        // 203	포인터 - 형성평가4
        //
        // 10개의 원소를 저장할 수 있는 배열을 선언한 후 포인터 변수를 이용하여 자료를 입력받아 홀수의 개수와 짝수의 개수를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3 5 10 52 1 97 36 25 13 29
        //
        // 출력 예 :
        // odd : 7
        // even : 3
        //
        //--------------------------------------------------
        static void _203()
        {
            string input = "3 5 10 52 1 97 36 25 13 29";
            string[] args = input.Split();

            int[] a = new int[10];

            int odd = 0;
            int even = 0;
            unsafe
            {
                for (int i = 0; i < 10; ++i) {
                    fixed (int* p = &a[i])
                        *p = Convert.ToInt32(args[i]);
                }
            }
            for (int i = 0; i < 10; ++i) {
                if (a[i] % 2 == 0)
                    ++even;
                else
                    ++odd;
            }

            Console.WriteLine("odd : {0}", odd);
            Console.WriteLine("even : {0}", even);
        }



        //--------------------------------------------------
        // 204	포인터 - 형성평가5
        //
        // 정수 n을 입력받아 n개의 정수형 동적배열을 생성하고 n개의 정수를 입력받아 최대값과 최소값을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 5
        // 15 90 8 36 25
        //
        // 출력 예 :
        // max : 90
        // min : 8
        //
        //--------------------------------------------------
        static void _204()
        {
            string input_n = "5";
            int n = Convert.ToInt32(input_n);

            int[] a = new int[n];

            string input_s = "15 90 8 36 25";
            string[] args = input_s.Split();
            
            unsafe
            {
                fixed(int* p = a)
                {
                    for (int i = 0; i < n; ++i)
                        *(p + i) = Convert.ToInt32(args[i]);

                    int max = *p;
                    int min = *p;

                    for (int i = 1; i < n; ++i) {
                        if (max < *(p + i))
                            max = *(p + i);
                        if (min > *(p + i))
                            min = *(p + i);
                    }

                    Console.WriteLine("max : {0}", max);
                    Console.WriteLine("min : {0}", min);
                }
            }
        }

    }
}
