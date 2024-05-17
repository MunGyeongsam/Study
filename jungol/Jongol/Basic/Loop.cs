using System;

namespace Jungol
{

    // 536	반복제어문1 - 자가진단1
    // 537	반복제어문1 - 자가진단2
    // 538	반복제어문1 - 자가진단3
    // 539	반복제어문1 - 자가진단4
    // 540	반복제어문1 - 자가진단5
    // 633	반복제어문1 - 자가진단6
    // 125	반복제어문1 - 형성평가1
    // 126	반복제어문1 - 형성평가2
    // 127	반복제어문1 - 형성평가3
    // 128	반복제어문1 - 형성평가4
    // 129	반복제어문1 - 형성평가5
    // 541	반복제어문2 - 자가진단1
    // 542	반복제어문2 - 자가진단2
    // 543	반복제어문2 - 자가진단3
    // 544	반복제어문2 - 자가진단4
    // 545	반복제어문2 - 자가진단5
    // 546	반복제어문2 - 자가진단6
    // 547	반복제어문2 - 자가진단7
    // 548	반복제어문2 - 자가진단8
    // 130	반복제어문2 - 형성평가1
    // 131	반복제어문2 - 형성평가2
    // 132	반복제어문2 - 형성평가3
    // 133	반복제어문2 - 형성평가4
    // 134	반복제어문2 - 형성평가5
    // 135	반복제어문2 - 형성평가6
    // 136	반복제어문2 - 형성평가7
    // 137	반복제어문2 - 형성평가8
    // 138	반복제어문2 - 형성평가9
    // 139	반복제어문2 - 형성평가A
    // 549	반복제어문3 - 자가진단1
    // 634	반복제어문3 - 자가진단2
    // 550	반복제어문3 - 자가진단3
    // 551	반복제어문3 - 자가진단4
    // 552	반복제어문3 - 자가진단5
    // 553	반복제어문3 - 자가진단6
    // 554	반복제어문3 - 자가진단7
    // 140	반복제어문3 - 형성평가1
    // 141	반복제어문3 - 형성평가2
    // 142	반복제어문3 - 형성평가3
    // 143	반복제어문3 - 형성평가4
    // 144	반복제어문3 - 형성평가5
    // 145	반복제어문3 - 형성평가6
    // 146	반복제어문3 - 형성평가7
    // 147	반복제어문3 - 형성평가8
    // 148	반복제어문3 - 형성평가9
    // 149	반복제어문3 - 형성평가A

    // 입력은 필요하다고 생각되는 경우만 Console.ReadLine 을 사용하고
    // 가능한 문자열을 사용.
    static class Loop
	{
		
		public static void Test()
        {
            Util.Call(_536);
            Util.Call(_537);
            Util.Call(_538);
            Util.Call(_539);
            Util.Call(_540);
            Util.Call(_633);
            Util.Call(_125);
            Util.Call(_126);
            Util.Call(_127);
            Util.Call(_128);
            Util.Call(_129);
            Util.Call(_541);
            Util.Call(_542);
            Util.Call(_543);
            Util.Call(_544);
            Util.Call(_545);
            Util.Call(_546);
            Util.Call(_547);
            Util.Call(_548);
            Util.Call(_130);
            Util.Call(_131);
            Util.Call(_132);
            Util.Call(_133);
            Util.Call(_134);
            Util.Call(_135);
            Util.Call(_136);
            Util.Call(_137);
            Util.Call(_138);
            Util.Call(_139);
            Util.Call(_549);
            Util.Call(_634);
            Util.Call(_550);
            Util.Call(_551);
            Util.Call(_552);
            Util.Call(_553);
            Util.Call(_554);
            Util.Call(_140);
            Util.Call(_141);
            Util.Call(_142);
            Util.Call(_143);
            Util.Call(_144);
            Util.Call(_145);
            Util.Call(_146);
            Util.Call(_147);
            Util.Call(_148);
            Util.Call(_149);
        }

        //--------------------------------------------------
        // 536	반복제어문1 - 자가진단1
        //
        // 1부터 15까지 차례로 출력하는 프로그램을 작성하시오. while문을 이용하세요.
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        // 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15
        //
        //--------------------------------------------------
        static void _536()
        {
            for (int i = 1; i <= 15; ++i) {
                Console.Write(i);
                Console.Write(' ');
            }

            Console.WriteLine();
        }



        //--------------------------------------------------
        // 537	반복제어문1 - 자가진단2
        //
        // 100 이하의 양의 정수만 입력된다. while 문을 이용하여 1부터 입력받은 정수까지의 합을 구하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10
        //
        // 출력 예 :
        // 55
        //
        //--------------------------------------------------
        static void _537()
        {
            string input = "10";
            int n1 = Convert.ToInt32(input);

            int sum = 0;
            for (int i = 1; i <= 10; ++i) {
                sum += i;
            }

            Console.WriteLine(sum);
        }



        //--------------------------------------------------
        // 538	반복제어문1 - 자가진단3
        //
        // 한 개의 정수를 입력받아 양수(positive integer)인지 음수(negative number)인지 출력하는 작업을 반복하다가 0이 입력되면 종료하는 프로그램을 작성하시오.
        //
        // 입력 출력 예 :
        // number? 10
        // positive integer
        // number? -10
        // negative number
        // number? 0
        //
        //--------------------------------------------------
        static void _538()
        {
            //   무한 반복문과 함께 Console.ReadLine 의 사용이 어울리나,
            //   테스트를 위해 아래와 유한한 for loop로 시뮬레이션 한다.

            //while (true)
            //{
            //    Console.Write("number ? ");
            //    string input = Console.ReadLine();
            //    int n = Convert.ToInt32(input);
            //
            //    // exit loop
            //    if (0 == n)
            //        break;
            //
            //    else if (0 < n)
            //        Console.WriteLine("positive integer");
            //    else
            //        Console.WriteLine("negative integer");
            //}


            string inputs = "10 -10 0";
            string[] arr = inputs.Split();
            for (int i = 0; i < arr.Length; ++i)
            {
                int n = Convert.ToInt32(arr[i]);

                // exit loop
                if (0 == n)
                    break;

                else if (0 < n)
                    Console.WriteLine("positive integer");
                else
                    Console.WriteLine("negative integer");
            }

        }



        //--------------------------------------------------
        // 539	반복제어문1 - 자가진단4
        //
        // 정수를 계속 입력받다가 100 이상의 수가 입력이 되면 마지막 입력된 수를 포함하여 합계와 평균을 출력하는 프로그램을 작성하시오.
        // (평균은 반올림하여 소수 첫째자리까지 출력한다.)
        //
        // 입력 예 :
        // 1 2 3 4 5 6 7 8 9 10 100
        //
        // 출력 예 :
        // 155
        // 14.1
        //
        //--------------------------------------------------
        static void _539()
        {
            string inputs = "1 2 3 4 5 6 7 8 9 10 100";
            string[] arr = inputs.Split();

            int sum = 0;
            for (int i = 0; i < arr.Length; ++i) {

                int n = Convert.ToInt32(arr[i]);

                sum += n;

                if (n >= 100) {

                    Console.WriteLine(sum);
                    Console.WriteLine("{0:f1}", (double)sum / (i+1));
                    
                    break;
                }
            }
        }



        //--------------------------------------------------
        // 540	반복제어문1 - 자가진단5
        //
        // 정수를 입력받아서 3의 배수가 아닌 경우에는 아무 작업도 하지 않고 3의 배수인 경우에는 3으로 나눈몫을 출력하는 작업을 반복하다가 -1이 입력되면 종료하는 프로그램을 작성하시오.
        //
        // 입력 출력 예 :
        // 5   : 입력값
        // 12  : 입력값
        // 4   : 실행값
        // 21  : 입력값
        // 7   : 실행값
        // 100 : 입력값
        // -1  : 입력값
        //
        //--------------------------------------------------
        static void _540()
        {
            //   무한 반복문과 함께 Console.ReadLine 의 사용이 어울리나,
            //   테스트를 위해 아래와 유한한 for loop로 시뮬레이션 한다.

            //while (true)
            //{
            //    string input = Console.ReadLine();
            //    int n = Convert.ToInt32(input);
            //
            //    // exit loop
            //    if (-1 == n)
            //        break;
            //
            //    if (n % 3 == 0)
            //        Console.WriteLine(n / 3);
            //}

            string inputs = "5 12 21 100 -1";
            string[] arr = inputs.Split();
            for (int i = 0; i < arr.Length; ++i) {
                int n = Convert.ToInt32(arr[i]);

                // exit loop
                if (-1 == n)
                    break;

                Console.WriteLine(n);
                if (n % 3 == 0)
                    Console.WriteLine(n / 3);
            }
        }



        //--------------------------------------------------
        // 633	반복제어문1 - 자가진단6
        //
        // 아래와 같이 나라 이름을 출력하고 숫자를 입력받아 해당하는 나라의 수도를 출력하는 작업을 반복하다가 해당하는 번호 이외의 숫자가 입력되면 "none"라고 출력한 후 종료하는 프로그램을 작성하시오.
        //
        // * 각 나라의 수도 : 
        //      대한민국 = 서울(Seoul)
        //      미국 = 워싱턴(Washington)
        //      일본 = 동경(Tokyo)
        //      중국 = 북경(Beijing)
        // 
        // * 입출력시 모양은 "입·출력예"와 같이 하시오.​
        //
        // 입력 출력 예 :
        // 1. Korea
        // 2. USA
        // 3. Japan
        // 4. China
        // number? 1
        // 
        // Seoul
        // 
        // 1. Korea
        // 2. USA
        // 3. Japan
        // 4. China
        // number? 5
        // 
        // none
        //
        //--------------------------------------------------
        static void _633()
        {
            //   무한 반복문과 함께 Console.ReadLine 의 사용이 어울리나,
            //   테스트를 위해 아래와 유한한 for loop로 시뮬레이션 한다.

            //while (true) {
            //    Console.WriteLine("1. Korea");
            //    Console.WriteLine("2. USA");
            //    Console.WriteLine("3. Japan");
            //    Console.WriteLine("4. China");
            //
            //    Console.Write("number? ");
            //    string input = Console.ReadLine();
            //
            //    Console.WriteLine();
            //    if (input == "1")
            //        Console.WriteLine("Seoul");
            //    else if (input == "2")
            //        Console.WriteLine("Washington");
            //    else if (input == "3")
            //        Console.WriteLine("Tokyo");
            //    else if (input == "4")
            //        Console.WriteLine("Beijing");
            //    else {
            //        Console.WriteLine("none");
            //        break;
            //    }
            //    Console.WriteLine();
            //}

            string inputs = "1 3 5";
            string[] arr = inputs.Split();
            for (int i = 0; i < arr.Length; ++i) {

                Console.WriteLine("1. Korea");
                Console.WriteLine("2. USA");
                Console.WriteLine("3. Japan");
                Console.WriteLine("4. China");

                Console.Write("number? ");
                string input = arr[i];
                Console.WriteLine(input);

                Console.WriteLine();
                if (input == "1")
                    Console.WriteLine("Seoul");
                else if (input == "2")
                    Console.WriteLine("Washington");
                else if (input == "3")
                    Console.WriteLine("Tokyo");
                else if (input == "4")
                    Console.WriteLine("Beijing");
                else {
                    Console.WriteLine("none");
                    break;
                }
                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 125	반복제어문1 - 형성평가1
        //
        // 정수를 입력받아 1부터 입력받은 정수까지를 차례대로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 5
        //
        // 출력 예 :
        // 1 2 3 4 5
        //
        //--------------------------------------------------
        static void _125()
        {
            string input = "5";
            int n = Convert.ToInt32(input);

            for (int i = 1; i <= n; ++i) {
                Console.Write(i);
                Console.Write(' ');
            }
        }



        //--------------------------------------------------
        // 126	반복제어문1 - 형성평가2
        // 
        // 정수를 입력받다가 0 이 입력되면 그 때까지 입력받은 홀수의 개수와 짝수의 개수를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 9 7 10 5 33 65 0
        //
        // 출력 예 :
        // odd : 5
        // even : 1
        //
        //--------------------------------------------------
        static void _126()
        {
            string inputs = "9 7 10 5 33 65 0";
            string[] arr = inputs.Split();

            int odd = 0;
            int even = 0;
            for (int i = 0; i < arr.Length; ++i) {
                int n = Convert.ToInt32(arr[i]);
                if (n == 0)
                    break;

                if (n % 2 == 0)
                    ++even;
                else
                    ++odd;
            }

            Console.WriteLine("odd : {0}", odd);
            Console.WriteLine("even : {0}", even);
        }



        //--------------------------------------------------
        // 127	반복제어문1 - 형성평가3
        //
        // 0 부터 100 까지의 점수를 계속 입력받다가 범위를 벗어나는 수가 입력되면 그 이전까지 입력된 자료의 합계와 평균을 출력하는 프로그램을 작성하시오.
        // (평균은 반올림하여 소수 첫째자리까지 출력한다.)
        //
        // 입력 예 :
        // 55 100 48 36 0 101
        //
        // 출력 예 :
        // sum : 239
        // avg : 47.8
        //
        //--------------------------------------------------
        static void _127()
        {
            string inputs = "55 100 48 36 0 101";
            string[] arr = inputs.Split();

            int sum = 0;
            int cnt = 0;
            for (int i = 0; i < arr.Length; ++i) {
                int n = Convert.ToInt32(arr[i]);
                if (n < 0 || n > 100)
                    break;

                sum += n;
                ++cnt;
            }

            float avg = sum / (float)cnt;
            Console.WriteLine("sum : {0}", sum);
            Console.WriteLine("avg : {0:f1}", avg);
        }



        //--------------------------------------------------
        // 128	반복제어문1 - 형성평가4
        //
        // 0 이 입력될 때까지 정수를 계속 입력받아 3의 배수와 5의 배수를 제외한 수들의 개수를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 1 2 3 4 5 6 7 8 9 10 0
        //
        // 출력 예 :
        // 5
        //
        //--------------------------------------------------
        static void _128()
        {
            string inputs = "1 2 3 4 5 6 7 8 9 10 0";
            string[] arr = inputs.Split();
            
            int cnt = 0;
            for (int i = 0; i < arr.Length; ++i) {

                int n = Convert.ToInt32(arr[i]);
                if (n %3 == 0 || n %5 == 0)
                    continue;
                
                ++cnt;
            }
            
            Console.WriteLine(cnt);
        }



        //--------------------------------------------------
        // 129	반복제어문1 - 형성평가5
        //
        // 삼각형의 밑변의 길이와 높이를 입력받아 넓이를 출력하고, "Continue? "에서 하나의 문자를 입력받아 그 문자가 'Y' 나 'y' 이면 작업을 반복하고 다른 문자이면 종료하는 프로그램을 작성하시오.
        // (넓이는 반올림하여 소수 첫째자리까지 출력한다.)​
        //
        // 입력 출력 예 :
        // Base = 11
        // Height = 5
        // Triangle width = 27.5
        // Continue? Y
        // Base = 10
        // Height = 10
        // Triangle width = 50.0
        // Continue? N
        //
        //--------------------------------------------------
        static void _129()
        {

            //   무한 반복문과 함께 Console.ReadLine 의 사용이 어울리나,
            //   테스트를 위해 아래와 유한한 for loop로 시뮬레이션 한다.

            //while (true) {
            //
            //    string input;
            //    int width;
            //    int height;
            //
            //    Console.Write("Base = ");
            //    input = Console.ReadLine();
            //    width = Convert.ToInt32(input);
            //    Console.Write("Height = ");
            //    input = Console.ReadLine();
            //    height = Convert.ToInt32(input);
            //    Console.WriteLine("Triangle area = {0:f1}", width * height * 0.5f);
            //    Console.Write("Continue? ");
            //    input = Console.ReadLine();
            //    if (input != "y" && input != "Y")
            //        break;
            //}

            string inputs = "11 5 Y, 10 10 N";
            char[] sep = { ',' };
            string[] arr = inputs.Split(sep);

            for (int i = 0; i < arr.Length; ++i) {

                string input;
                int width;
                int height;

                string line = arr[i];
                line = line.TrimStart();
                string[] arr2 = line.Split();
                
                Console.Write("Base = ");
                input = arr2[0];
                Console.WriteLine(input);
                width = Convert.ToInt32(input);
                
                Console.Write("Height = ");
                input = arr2[1];
                Console.WriteLine(input);
                height = Convert.ToInt32(input);

                Console.WriteLine("Triangle area = {0:f1}", width * height * 0.5f);
                Console.Write("Continue? ");
                input = arr2[2];
                Console.WriteLine(input);
                if (input != "y" && input != "Y")
                    break;
            }
        }



        //--------------------------------------------------
        // 541	반복제어문2 - 자가진단1
        //
        // 문자를 입력받아서 입력받은 문자를 20번 반복하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // A
        //
        // 출력 예 :
        // AAAAAAAAAAAAAAAAAAAA
        //
        //--------------------------------------------------
        static void _541()
        {
            string input = "A";
            char ch = Convert.ToChar(input);

            for (int i = 0; i < 20; ++i)
                Console.Write(ch);
            Console.WriteLine();
        }



        //--------------------------------------------------
        // 542	반복제어문2 - 자가진단2
        // 
        // 10부터 20까지의 숫자를 차례대로 출력하는 프로그램을 작성하시오.for문을 사용하세요
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        // 10 11 12 13 14 15 16 17 18 19 20
        //
        //--------------------------------------------------
        static void _542()
        {
            for (int i = 10; i <= 20; ++i) {

                Console.Write(i);
                Console.Write(' ');
            }
            Console.WriteLine();
        }



        //--------------------------------------------------
        // 543	반복제어문2 - 자가진단3
        // 
        // 하나의 정수를 입력받아 1부터 입력받은 정수까지의 짝수를 차례대로 출력하는 프로그램을 작성하시오.
        // 입력되는 정수는 50이하이다.
        //
        // 입력 예 :
        // 10
        //
        // 출력 예 :
        // 2 4 6 8 10
        //
        //--------------------------------------------------
        static void _543()
        {
            string input = "10";
            int n = Convert.ToInt32(input);

            // 문제를 그대로 코드로 옮겨보면 아래와 같지만
            //for (int i = 1; i <= n; ++i) {
            //
            //    if (i % 2 == 0) {
            //        Console.Write(i);
            //        Console.Write(' ');
            //    }
            //}
            
            // 2부터 시작하는 짝수는 아래와 같은 코드가 더 낫다.
            for (int i = 2; i <= n; i += 2) {
                Console.Write(i);
                Console.Write(' ');
            }

            Console.WriteLine();
        }



        //--------------------------------------------------
        // 544	반복제어문2 - 자가진단4
        //
        // 100 이하의 정수를 입력받아서 입력받은 정수부터 100까지의 합을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 95
        //
        // 출력 예 :
        // 585
        //
        //--------------------------------------------------
        static void _544()
        {
            string input = "95";
            int n = Convert.ToInt32(input);

            int sum = 0;
            for (int i = n; i <= 100; ++i) {

                sum += i;
            }

            Console.WriteLine(sum);
        }



        //--------------------------------------------------
        // 545	반복제어문2 - 자가진단5
        //
        // 10개의 정수를 입력받아 3의 배수의 개수와 5의 배수의 개수를 각각 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10 15 36 99 100 19 46 88 87 13
        //
        // 출력 예 :
        // Multiples of 3 : 4
        // Multiples of 5 : 3
        //
        //--------------------------------------------------
        static void _545()
        {
            string inputs = "10 15 36 99 100 19 46 88 87 13";
            string[] arr = inputs.Split();

            int x3 = 0;
            int x5 = 0;
            for (int i = 0; i < arr.Length; ++i) {

                int n = Convert.ToInt32(arr[i]);

                if (n % 3 == 0)
                    x3++;

                if (n % 5 == 0)
                    x5++;
            }
            Console.WriteLine("Multiples of 3 : {0}", x3);
            Console.WriteLine("Multiples of 5 : {0}", x5);
        }


        //--------------------------------------------------
        // 546	반복제어문2 - 자가진단6
        //
        // 10 이하의 과목수 n을 입력받은 후 n개 과목의 점수를 입력받아서 평균을 구하여 출력하고 평균이 80점이상이면 "pass", 80점 미만이면 "fail"이라고 출력하는 프로그램을 작성하시오.
        // 평균은 반올림하여 소수 첫째자리까지 출력한다.​
        //
        // 입력 예 :
        // 4
        // 75 80 85 90
        //
        // 출력 예 :
        // avg : 82.5
        // pass
        //
        //--------------------------------------------------
        static void _546()
        {
            string input_n = "4";
            string input_score = "75 80 85 90";
            string[] arr = input_score.Split();

            int n = Convert.ToInt32(input_n);
            int sum = 0;
            for (int i = 0; i < n; ++i) {

                sum += Convert.ToInt32(arr[i]);
            }

            float avg = sum / (float)n;
            Console.WriteLine("avg : {0:f1}", avg);
            if (avg >= 80f)
                Console.WriteLine("pass");
            else
                Console.WriteLine("fail");
        }



        //--------------------------------------------------
        // 547	반복제어문2 - 자가진단7
        //
        // 아래와 같이 출력되는 프로그램을 작성하시오.
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        // 2 3 4 5 6
        // 3 4 5 6 7
        // 4 5 6 7 8
        // 5 6 7 8 9
        // 6 7 8 9 10
        //
        //--------------------------------------------------
        static void _547()
        {
            for (int i = 2; i <= 6; ++i) {

                for (int j = 0; j < 5; ++j) {

                    Console.Write(i + j);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 548	반복제어문2 - 자가진단8
        //
        // 구구단의 일부를 다음과 같이 출력하는 프로그램을 작성하시오.
        // 곱셈의 결과는 오른쪽으로 맞추어 출력을 하고 결과값 사이의 공백은 3칸으로 한다.
        // 출력형식 예) 2□*□1□=□□2□□□ (□는 공백을 나타내는 것임)​
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        // 2 * 1 =  2   2 * 2 =  4   2 * 3 =  6   2 * 4 =  8   2 * 5 = 10
        // 3 * 1 =  3   3 * 2 =  6   3 * 3 =  9   3 * 4 = 12   3 * 5 = 15
        // 4 * 1 =  4   4 * 2 =  8   4 * 3 = 12   4 * 4 = 16   4 * 5 = 20
        //
        //--------------------------------------------------
        static void _548()
        {
            for (int i = 2; i <= 4; ++i) {

                for (int j = 1; j <= 5; ++j)
                    Console.Write("{0} * 1 ={1,3}   ", i, i * j);

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 130	반복제어문2 - 형성평가1
        //
        // 10 이하의 자연수 n을 입력받아 "JUNGOL​"을 n번 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 4
        //
        // 출력 예 :
        // JUNGOL
        // JUNGOL
        // JUNGOL
        // JUNGOL
        //
        //--------------------------------------------------
        static void _130()
        {
            string input = "4";
            int n = Convert.ToInt32(input);
            
            for (int i = 0; i < n; ++i)
                Console.WriteLine("JUNGOL");
        }



        //--------------------------------------------------
        // 131	반복제어문2 - 형성평가2
        //
        // 00 이하의 두 개의 정수를 입력받아 작은 수부터 큰 수까지 차례대로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10 5
        //
        // 출력 예 :
        // 5 6 7 8 9 10
        //
        //--------------------------------------------------
        static void _131()
        {
            string input = "10 5";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            int MIN, MAX;
            if (n1 < n2) {
                MIN = n1;
                MAX = n2;
            }
            else {
                MIN = n2;
                MAX = n1;
            }

            for (int i = MIN; i <= MAX; ++i) {

                Console.Write(i);
                Console.Write(' ');
            }
            Console.WriteLine();
        }



        //--------------------------------------------------
        // 132	반복제어문2 - 형성평가3
        //
        // 정수를 입력받아서 1부터 입력받은 정수까지의 5의 배수의 합을 구하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 20
        //
        // 출력 예 :
        // 50
        //
        //--------------------------------------------------
        static void _132()
        {
            string input = "20";
            int n = Convert.ToInt32(input);

            int sum = 0;
            for (int i = 5; i <= n; i += 5)
                sum += i;

            Console.WriteLine(sum);
        }



        //--------------------------------------------------
        // 133	반복제어문2 - 형성평가4
        //
        // 100 이하의 자연수 n을 입력받고 n개의 정수를 입력받아 평균을 출력하는 프로그램을 작성하시오. 
        // (평균은 반올림하여 소수 둘째자리까지 출력하도록 한다.)
        //
        // 입력 예 :
        // 3
        // 99 65 30
        //
        // 출력 예 :
        // 64.67
        //
        //--------------------------------------------------
        static void _133()
        {
            string input_n = "3";
            string input_score = "99 65 30";
            string[] arr = input_score.Split();

            int sum = 0;
            int n = Convert.ToInt32(input_n);
            for (int i = 0; i < n; ++i)
                sum += Convert.ToInt32(arr[i]);

            float avg = sum / (float)n;
            Console.WriteLine("{0:f2}", avg);
        }



        //--------------------------------------------------
        // 134	반복제어문2 - 형성평가5
        //
        // 10개의 정수를 입력받아 입력받은 수들 중 짝수의 개수와 홀수의 개수를 각각 구하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10 20 30 55 66 77 88 99 100 15
        //
        // 출력 예 :
        // even : 6
        // odd : 4
        //
        //--------------------------------------------------
        static void _134()
        {
            string inputs = "10 20 30 55 66 77 88 99 100 15";
            string[] arr = inputs.Split();

            int odd = 0;
            int even = 0;
            for (int i = 0; i < arr.Length; ++i) {
                int n = Convert.ToInt32(arr[i]);

                if (n % 2 == 0)
                    ++even;
                else
                    ++odd;
            }

            Console.WriteLine("even : {0}", even);
            Console.WriteLine("odd : {0}", odd);
        }



        //--------------------------------------------------
        // 135	반복제어문2 - 형성평가6
        //
        // 두 개의 정수를 입력받아 두 정수 사이(두 정수를 포함)에 3의 배수이거나 5의 배수인 수들의 합과 평균을 출력하는 프로그램을 작성하시오.
        // (평균은 반올림하여 소수 첫째자리까지 출력한다.)
        //
        // 입력 예 :
        // 10 15
        //
        // 출력 예 :
        // sum : 37
        // avg : 12.3
        //
        //--------------------------------------------------
        static void _135()
        {
            string input = "10 15";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            int sum = 0;
            int cnt = 0;
            for (int i = n1; i <= n2; ++i) {

                if ((i % 3 == 0) || (i % 5 == 0)) {

                    sum += i;
                    ++cnt;
                }
            }

            float avg = sum / (float)cnt;
            Console.WriteLine("sum : {0}", sum);
            Console.WriteLine("avg : {0:f1}", avg);
        }



        //--------------------------------------------------
        // 136	반복제어문2 - 형성평가7
        //
        // 한 개의 자연수를 입력받아 그 수의 배수를 차례로 10개 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 5
        //
        // 출력 예 :
        // 5 10 15 20 25 30 35 40 45 50
        //
        //--------------------------------------------------
        static void _136()
        {
            string input = "5";
            int n = Convert.ToInt32(input);
            
            for (int i = 1; i <= 10; ++i) {

                Console.Write(n * i);
                Console.Write(' ');
            }

            Console.WriteLine();
        }



        //--------------------------------------------------
        // 137	반복제어문2 - 형성평가8
        //
        // 행과 열의 수를 입력받아 다음과 같이 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3 4
        //
        // 출력 예 :
        // 1 2 3 4
        // 2 4 6 8
        // 3 6 9 12
        //
        //--------------------------------------------------
        static void _137()
        {
            string input = "3 4";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);
            
            for (int i = 1; i <= n1; ++i) {

                for (int j = 1; j <= n2; ++j) {

                    Console.Write(i*j);
                    Console.Write(' ');
                }

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 138	반복제어문2 - 형성평가9
        //
        // 정수를 입력받아 다음과 같이 순서쌍을 출력하는 프로그램을 작성하시오.
 
        // * 주의 
        //  ')'와 '('사이에 공백이 1칸 있다.
        // (1, _1) 처럼 출력한다 : '_'는 공백
        //
        // 입력 예 :
        // 4
        //
        // 출력 예 :
        // (1, 1) (1, 2) (1, 3) (1, 4)
        // (2, 1) (2, 2) (2, 3) (2, 4)
        // (3, 1) (3, 2) (3, 3) (3, 4)
        // (4, 1) (4, 2) (4, 3) (4, 4)
        //
        //--------------------------------------------------
        static void _138()
        {
            string input = "4";
            int n = Convert.ToInt32(input);


            for (int i = 1; i <= n; ++i) {

                for (int j = 1; j <= n; ++j) {

                    Console.Write("({0}, {1}) ", i, j);
                }

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 139	반복제어문2 - 형성평가A
        //
        // 2부터 9까지의 수 중 2개를 입력받아 입력받은 수 사이의 구구단을 출력하는 프로그램을 작성하시오.
        // 단 반드시 먼저 입력된 수의 구구단부터 아래의 형식에 맞게 출력하여야 한다.
        // 구구단 사이의 공백은 3칸이다.
        //
        // 입력 예 :
        // 5 3
        //
        // 출력 예 :
        // 5 * 1 =  5   4 * 1 =  4   3 * 1 =  3
        // 5 * 2 = 10   4 * 2 =  8   3 * 2 =  6
        // 5 * 3 = 15   4 * 3 = 12   3 * 3 =  9
        // 5 * 4 = 20   4 * 4 = 16   3 * 4 = 12
        // 5 * 5 = 25   4 * 5 = 20   3 * 5 = 15
        // 5 * 6 = 30   4 * 6 = 24   3 * 6 = 18
        // 5 * 7 = 35   4 * 7 = 28   3 * 7 = 21
        // 5 * 8 = 40   4 * 8 = 32   3 * 8 = 24
        // 5 * 9 = 45   4 * 9 = 36   3 * 9 = 27
        //
        //--------------------------------------------------
        static void _139()
        {
            string input = "5 3";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            int step = (n1 > n2) ? -1 : 1;

            for (int i = 1; i <= 9; ++i) {

                ///< do ~ while
                int j = n1-step;
                do {
                    j += step;
                    Console.Write("{0} * {1} = {2,2}   ", j, i, j * i);
                } while (j != n2);

                ///< while
                //while (true) {
                //
                //    Console.Write("{0} * {1} = {2,2}   ", j, i, j*i);
                //
                //    if (j == n2)
                //        break;
                //
                //    j += step;
                //}

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 549	반복제어문3 - 자가진단1
        //
        // 자연수 n을 입력받고 1부터 홀수를 차례대로 더해나가면서 합이 n 이상이면 그 때까지 더해진 홀수의 개수와 그 합을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 100
        //
        // 출력 예 :
        // 10 100
        //
        //--------------------------------------------------
        static void _549()
        {
            string input = "100";
            int n = Convert.ToInt32(input);

            int sum = 0;
            int cnt = 0;
            int cur = 1;

            while( sum < n) {

                sum += cur;
                ++cnt;

                cur += 2; // 1, 3, 5, ...
            }

            Console.WriteLine("{0} {1}", cnt, sum);
        }



        //--------------------------------------------------
        // 634	반복제어문3 - 자가진단2
        //
        // 자연수 n을 입력받아서 n줄만큼 다음과 같이 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 5
        //
        // 출력 예 :
        // *
        // **
        // ***
        // ****
        // *****
        //
        //--------------------------------------------------
        static void _634()
        {
            string input = "5";
            int n = Convert.ToInt32(input);

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j <= i; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 550	반복제어문3 - 자가진단3
        //
        // 자연수 n을 입력받아서 다음과 같이 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // ***
        // **
        // *
        // *
        // **
        // ***
        //
        //--------------------------------------------------
        static void _550()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            for (int i = n - 1; i >= 0; --i) {

                for (int j = 0; j <= i; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j <= i; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 551	반복제어문3 - 자가진단4
        //
        // 자연수 n을 입력받아서 다음과 같이 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // ***
        //  **
        //   *
        //
        //--------------------------------------------------
        static void _551()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            for (int i = 0; i < n; ++i) {

                int j = 0;
                for (; j < i; ++j)
                    Console.Write(' ');

                for (; j < n; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 552	반복제어문3 - 자가진단5
        //
        // 자연수 n을 입력받아서 다음과 같이 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // *****
        //  ***
        //   *
        //
        //--------------------------------------------------
        static void _552()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j < i; ++j)
                    Console.Write(' ');

                for (int j = 0; j < (n - i) * 2 - 1; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 553	반복제어문3 - 자가진단6
        //
        // 자연수 n을 입력받아 다음과 같이 영문자를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // ABC
        // DE
        // F
        //
        //--------------------------------------------------
        static void _553()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            char c = 'A';
            for (int i = 0; i < n; ++i) {

                for (int j = 0; j < n-i; ++j)
                    Console.Write(c++);

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 554	반복제어문3 - 자가진단7
        //
        // 자연수 n을 입력받아서 n개의 줄에 n+1개의 숫자 혹은 문자로 채워서 다음과 같이 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // 1 2 3 A
        // 4 5 B C
        // 6 D E F
        //
        //--------------------------------------------------
        static void _554()
        {
            string input = "3";
            int N = Convert.ToInt32(input);

            int n = 1;
            char c = 'A';

            for (int i = 0; i < N; ++i) {

                for (int j = 0; j < N - i; ++j)
                    Console.Write(n++);
                for (int j = 0; j <= i; ++j)
                    Console.Write(c++);

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 140	반복제어문3 - 형성평가1
        //
        // 정수 20 개를 입력받아서 그 합과 평균을 출력하되 0 이 입력되면 20개 입력이 끝나지 않았더라도 그 때까지 입력된 합과 평균을 출력하는 프로그램을 작성하시오. 
        // 평균은 소수부분은 버리고 정수만 출력한다.(0이 입력된 경우 0을 제외한 합과 평균을 구한다.)
        //
        // 입력 예 :
        // 5 9 6 8 4 3 0
        //
        // 출력 예 :
        // 35 5
        //
        //--------------------------------------------------
        static void _140()
        {
            string inputs = "5 9 6 8 4 3 0";
            string[] arr = inputs.Split();

            int sum = 0;
            int cnt = 0;
            for (int i = 0; i < arr.Length; ++i) {
                int n = Convert.ToInt32(arr[i]);

                if (0 == n)
                    break;

                sum += n;
                ++cnt;
            }

            int avg = (cnt == 0) ? 0 : sum / cnt;
            Console.WriteLine("{0} {1}", sum, avg);
        }



        //--------------------------------------------------
        // 141	반복제어문3 - 형성평가2
        // 
        // 1부터 100까지의 정수 중 한 개를 입력받아 100 보다 작은 배수들을 차례로 출력하다가 10 의 배수가 출력되면 프로그램을 종료하도록 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 7
        //
        // 출력 예 :
        // 7 14 21 28 35 42 49 56 63 70
        //
        //--------------------------------------------------
        static void _141()
        {
            string input = "7";
            int n = Convert.ToInt32(input);

            int i = 1;
            int mul = 0;
            while (true) {

                mul = n * i++;
                if (mul >= 100)
                    break;

                Console.Write(mul);
                Console.Write(' ');

                if (mul % 10 == 0)
                    break;
            }
        }



        //--------------------------------------------------
        // 142	반복제어문3 - 형성평가3
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 출력되는 프로그램을 작성하시오.
        // 주의! '*'과 '*'사이에 공백이 없고 줄사이에도 빈줄이 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // *
        // **
        // ***
        // **
        // *
        // 
        //--------------------------------------------------
        static void _142()
        {
            string input = "3";
            int n = Convert.ToInt32(input);
            
            for (int i = 0; i < n; ++i) {

                for (int j = 0; j <= i; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }

            for (int i = n-1; i >= 1; --i) {

                for (int j = 0; j < i; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }

        }



        //--------------------------------------------------
        // 143	반복제어문3 - 형성평가4
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 출력되는 프로그램을 작성하시오.
        // 주의! '*'과 '*'사이에 공백이 없고 줄사이에도 빈줄이 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // *****
        //  ***
        //   *
        //  ***
        // *****
        //
        //--------------------------------------------------
        static void _143()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j < i; ++j)
                    Console.Write(' ');

                for (int j = 0; j < (n - i) * 2 - 1; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }

            for (int i = n-2; i >= 0; --i) {

                for (int j = 0; j < i; ++j)
                    Console.Write(' ');

                for (int j = 0; j < (n - i) * 2 - 1; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 144	반복제어문3 - 형성평가5
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 출력되는 프로그램을 작성하시오.
        // 주의! '*'과 '*'사이에 공백이 없고 줄사이에도 빈줄이 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        //     *
        //   ***
        // *****
        //
        //--------------------------------------------------
        static void _144()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            int width = n * 2 - 1;

            for (int i = 1; i <= n; ++i) {

                int cnt = (n - i) * 2;
                for (int j = 0; j < cnt; ++j)
                    Console.Write(' ');

                for (int j = 0; j < width - cnt; ++j)
                    Console.Write('*');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 145	반복제어문3 - 형성평가6
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 출력되는 프로그램을 작성하시오.
        // 주의! '*'과 '*'사이에 공백이 없고 줄사이에도 빈줄이 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        //   1
        //  12
        // 123
        //
        //--------------------------------------------------
        static void _145()
        {
            string input = "3";
            int n = Convert.ToInt32(input);
            
            for (int i = 0; i < n; ++i) {

                int cnt = (n - i) - 1;
                for (int j = 0; j < cnt; ++j)
                    Console.Write(' ');

                cnt = n - cnt;
                int num = 1;
                for (int j = 0; j < cnt; ++j)
                    Console.Write(num++);

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 146	반복제어문3 - 형성평가7
        //
        // 자연수 n(n≤6)을 입력받아 "출력 예"와 같이 공백으로 구분하여 출력되는 프로그램을 작성하시오.
        // 주의! 문자는 공백으로 구분하되 줄사이에 빈줄은 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // A B C
        // D E 0
        // F 1 2
        // 
        //--------------------------------------------------
        static void _146()
        {
            string input = "3";
            int N = Convert.ToInt32(input);

            int n = 0;
            char c = 'A';

            for (int i = 0; i < N; ++i) {

                for (int j = 0; j < N - i; ++j)
                    Console.Write(c++);

                for (int j = 0; j < i; ++j)
                    Console.Write(n++);

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 147	반복제어문3 - 형성평가8
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 공백으로 구분하여 출력되는 프로그램을 작성하시오. 
        // 주의! 숫자는 공백으로 구분하되 줄사이에 빈줄은 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // 1 2 3
        //   4 5
        //     6
        //
        //--------------------------------------------------
        static void _147()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            int num = 1;
            for (int i = 0; i < n; ++i) {

                int j = 0;
                for (; j < i; ++j)
                    Console.Write(' ');

                for (; j < n; ++j)
                    Console.Write(num++);

                Console.WriteLine();
            }
        }
        

        //--------------------------------------------------
        // 148	반복제어문3 - 형성평가9
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 공백으로 구분하여 출력되는 프로그램을 작성하시오.
        // 주의! '#'은 공백으로 구분하되 줄사이에 빈줄은 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // #
        // # #
        // # # #
        //   # #
        //     #
        //
        //--------------------------------------------------
        static void _148()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            for (int i = 0; i < n; ++i) {

                for (int j = 0; j <= i; ++j)
                    Console.Write('#');

                Console.WriteLine();
            }

            for (int i = 1; i < n; ++i) {

                int j = 0;
                for (; j < i; ++j)
                    Console.Write(' ');

                for (; j < n; ++j)
                    Console.Write('#');

                Console.WriteLine();
            }
        }



        //--------------------------------------------------
        // 149	반복제어문3 - 형성평가A
        //
        // 자연수 n을 입력받아 "출력 예"와 같이 n x n크기에 공백으로 구분하여 출력되는 프로그램을 작성하시오. 
        // 10 미만의 홀수만 출력하시오.주의! 숫자는 공백으로 구분하되 줄사이에 빈줄은 없다.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // 1 3 5
        // 7 9 1
        // 3 5 7
        //
        //--------------------------------------------------
        static void _149()
        {
            string input = "3";
            int n = Convert.ToInt32(input);

            int num = 1;
            for (int i = 0; i < n; ++i) {

                for (int j = 0; j < n; ++j) {

                    Console.Write(num);

                    num += 2;
                    if (num > 10)
                        num = 1;
                }

                Console.WriteLine();
            }
        }

    }
}
