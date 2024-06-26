﻿using System;

namespace Jungol
{
    // 526	디버깅 - 자가진단1
    // 527	디버깅 - 자가진단2
    // 116	디버깅 - 형성평가1
    // 117	디버깅 - 형성평가2
    // 118	디버깅 - 형성평가3
    // 119	디버깅 - 형성평가4

    static class Debugging
    {
		public static void Test()
        {
            Util.Call(_526);
            Util.Call(_527);
            Util.Call(_116);
            Util.Call(_117);
            Util.Call(_118);
            Util.Call(_119);
        }

        //--------------------------------------------------
        // 526	디버깅 - 자가진단1
        //
        // 2개의 실수(double)를 입력 받아서 두 수의 곱을 정수로 변환한 결과값과 
        // 두 수를 각각 정수로 변환하여 곱을 구한 결과값을 출력하는 프로그램을 작성하고 
        // 프로그램 내용에 관한 설명을 주석으로 표시하시오.
        //
        // 입력 예 :
        // 3.4 5.65
        //
        // 출력 예 :
        // 19 15
        //
        //--------------------------------------------------
        static void _526()
        {
            string input1 = "3.4 5.65";
            string[] arr1 = input1.Split();
            double d1 = Convert.ToDouble(arr1[0]);
            double d2 = Convert.ToDouble(arr1[1]);


            // 3.4 * 5.65 => 19.21
            // (int)19.21 => 19
            int n1 = (int)(d1 * d2);

            // (int)3.4 => 3
            // (int)5.65 => 5
            // 3 * 5 => 15
            int n2 = (int)d1 * (int)d2;

            Console.WriteLine("{0} {1}", n1, n2);
            Console.WriteLine(d1 * d2);
        }



        //--------------------------------------------------
        // 527	디버깅 - 자가진단2
        //
        // 2개의 정수를 입력받아서 첫 번째 수를 두 번째 수로 나눈 몫을 출력하고 
        // 첫 번째 수를 실수로 변환하여두 번째 수로 나눈 값을 구한 후 
        // 반올림하여 소수 둘째자리까지 출력하는 프로그램을 작성하고 
        // 프로그램내용에 관한 설명을 주석으로 표시하시오.
        //
        // 입력 예 :
        // 11 3
        //
        // 출력 예 :
        // 3 3.67
        //
        //--------------------------------------------------
        static void _527()
        {
            string input = "11 3";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            // 정수형의 / 연사자는 몫 연산자 이다.
            int rt1 = n1 / n2;

            // '실수 와 정수' 간의 연산은 정수가 암묵적인 실수로의 형변환이 일어나서 '실수 와 실수' 간의 연산으로 바뀐다.
            // 실수형에서 / 연산자는 나누기 연산자 이다.
            double rt2 = (double)n1 / n2;

            Console.WriteLine("{0} {1:f2}", rt1, rt2);
        }



        //--------------------------------------------------
        // 116	디버깅 - 형성평가1
        //
        // 정수로 된 3과목의 점수를 입력받아 평균을 구한 후 
        // 반올림하여 소수 첫째자리까지 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 70 95 65
        //
        // 출력 예 :
        // 76.7
        //
        //--------------------------------------------------
        static void _116()
        {
            string input = "70 95 65";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);
            int n3 = Convert.ToInt32(arr[2]);

            int sum = n1 + n2 + n3;
            float avg = sum / 3f;
            Console.WriteLine("{0:f1}", avg);
        }



        //--------------------------------------------------
        // 117	디버깅 - 형성평가2
        //
        // 실수로 된 3과목의 점수를 입력받아 총점은 정수부분의 합계를 출력하고 
        // 평균은 실수의 평균을 구한 뒤 정수부분만 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 70.5 95.5 68.5
        //
        // 출력 예 :
        // sum 233
        // avg 78
        //
        //--------------------------------------------------
        static void _117()
        {
            string input = "70.5 95.5 68.5";
            string[] arr = input.Split();

            float f1 = Convert.ToSingle(arr[0]);
            float f2 = Convert.ToSingle(arr[1]);
            float f3 = Convert.ToSingle(arr[2]);

            // 70.5 -> 70
            // 95.5 -> 90
            // 68.5 -> 68
            // 70 + 90 + 68 -> 233
            int sum = (int)f1 + (int)f2 + (int)f3;

            // 70.5 + 95.5 + 68.5 -> 234.5
            // 234.5 / 3f -> 78.16666667
            // (int)78.16666667 -> 78
            float avg = (f1 + f2 + f3) / 3f;

            Console.WriteLine("sum {0}", sum);
            Console.WriteLine("avg {0}", (int)avg);
        }



        //--------------------------------------------------
        // 118	디버깅 - 형성평가3
        //
        // 아래의 프로그램을 작성하여 디버깅을 하면서 디버깅 창에서
        // [1] 위치에서의 a의 값이 얼마인지 구하여 그 값을 입력하시오. 
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _118()
        {
            int a = 5;
            a += 10;
            a = a - 1; //-----------[1]

            Console.WriteLine("{0}", 1); // 여기에서 출력될 1을 [1]위치에서의 a 값으로 바꾸어 준다.

            //[1] 의 위치에선 아직 15 이다.
            //[1] 의 라인을 지나야 비로서 14가 된다.
            //그러므로 답은 15
            //Console.WriteLine("{0}", 15); 
        }



        //--------------------------------------------------
        // 119	디버깅 - 형성평가4
        //
        // 다음의 프로그램을 작성하여 같은 방법으로 [1],[2],[3]
        // 위치에서 디버깅 창에 표시된 a의 값을 각각 입력하시오. 
        // (PC의 시간이 맞는지 확인하세요. 시간이 다르면 결과가 틀릴 수 있습니다.)
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _119()
        {
            int a = 0;

            DateTime saveNow = DateTime.Now;
            a += saveNow.Year;                          //------[1]
            a += saveNow.Month;                         //------[2]
            a += saveNow.Day;

            Console.WriteLine("{0} {1} {2}", 1, 2, 3);  //------[3]
            // 위 문장에서 출력될 값들을 각각 [1][2][3]위치에서의 a의 값으로 바꾸어 준다.

            //현재시간을 기준으로 하면 아래와 같다.
            //saveNow = {2017-06-24 오전 3:08:14}
            //Console.WriteLine("{0} {1} {2}", 0, 2017, 2047);  //------[3]
        }
    }
}
