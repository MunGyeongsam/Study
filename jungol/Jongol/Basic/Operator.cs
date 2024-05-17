using System;

namespace Jungol
{
    // 518	연산자 - 자가진단1
    // 519	연산자 - 자가진단2
    // 520	연산자 - 자가진단3
    // 521	연산자 - 자가진단4
    // 522	연산자 - 자가진단5
    // 523	연산자 - 자가진단6
    // 524	연산자 - 자가진단7
    // 525	연산자 - 자가진단8
    // 111	연산자 - 형성평가1
    // 112	연산자 - 형성평가2
    // 113	연산자 - 형성평가3
    // 114	연산자 - 형성평가4
    // 115	연산자 - 형성평가5

    // 연산자 문제의 성격상 입력은 Console.ReadLine 보다
    // 한 줄의 문자열을 사용하는게 어울린다.
    static class Operator
	{
		public static void Test()
        {
            Util.Call(_518);
            Util.Call(_519);
            Util.Call(_520);
            Util.Call(_521);
            Util.Call(_522);
            Util.Call(_523);
            Util.Call(_524);
            Util.Call(_525);
            Util.Call(_111);
            Util.Call(_112);
            Util.Call(_113);
            Util.Call(_114);
            Util.Call(_115);
        }

        //--------------------------------------------------
        // 518	연산자 - 자가진단1
        //
        // 세 개의 정수를 입력 받아서 합계와 평균을 출력하시오. (단 평균은 소수 이하를 버리고 정수부분만 출력한다.)
        //
        // 입력 예 : 
        // 10 25 33
        //
        // 출력 예 :
        // sum : 68
        // avg : 22
        //
        //--------------------------------------------------
        static void _518()
        {
            string input = "10 25 33";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);
            int n3 = Convert.ToInt32(arr[2]);

            int sum = n1 + n2 + n3;
            int avg = sum / 3;

            Console.WriteLine("sum : {0}", sum);
            Console.WriteLine("avg : {0}", avg);
        }



        //--------------------------------------------------
        // 519	연산자 - 자가진단2
        //
        // 정수 2개를 입력받아서 첫 번째 수에는 100을 증가시켜 저장하고  
        // 두 번째 수는 10으로 나눈 나머지를 저장한 후  두 수를 차례로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 20 35
        //
        // 출력 예 :
        // 120 5
        //
        //--------------------------------------------------
        static void _519()
        {
            string input = "20 35";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            n1 += 100;
            n2 %= 10;
            Console.WriteLine("{0} {1}", n1, n2);
        }



        //--------------------------------------------------
        // 520	연산자 - 자가진단3
        //
        // 한 개의 정수를 입력 받아서 후치증가 연산자를 사용하여 출력한 후 
        // 전치 증가 연산자를 사용하여 출력하는프로그램을 작성하시오.
        //
        // 입력 예 :
        // 5
        //
        // 출력 예 :
        // 5
        // 7
        //
        //--------------------------------------------------
        static void _520()
        {
            string input = "5";
            int n1 = Convert.ToInt32(input);

            Console.WriteLine("{0}", n1++);
            Console.WriteLine("{0}", ++n1);
        }



        //--------------------------------------------------
        // 521	연산자 - 자가진단4
        //
        // 두 개의 정수를 입력받아 각각 후치 증가 연산자와 
        // 전치 감소 연산자를 사용하여 두 수의 곱을 구한 후 
        // 각각의 값을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10 20
        //
        // 출력 예 :
        // 11 19 190
        //
        //--------------------------------------------------
        static void _521()
        {
            string input = "10 20";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            int mul = (n1++) * (--n2);

            Console.WriteLine("{0} {1} {2}", n1, n2, mul);
        }



        //--------------------------------------------------
        // 522	연산자 - 자가진단5
        //
        // 두 개의 정수를 입력받아서
        // 첫 번째 줄에는 두 정수의 값이 같으면 1 아니면 0을 출력하고
        // 두 번째 줄에는 같지 않으면 1 같으면 0을 출력하는 프로그램을 작성하시오.
        // (JAVA는 1이면 true, 0이면 false를 출력한다.)
        //
        // 입력 예 :
        // 5 5
        //
        // 출력 예 :
        // 1
        // 0
        //--------------------------------------------------
        static void _522()
        {
            string input = "5 5";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            //c# 에서는 궂이 1,0 보단 True, False 가 더 어울림
            Console.WriteLine("{0}", (n1 == n2));
            Console.WriteLine("{0}", (n1 != n2));
            //Console.WriteLine("{0}", Convert.ToInt32(n1 == n2));
            //Console.WriteLine("{0}", Convert.ToInt32(n1 != n2));
        }



        //--------------------------------------------------
        // 523	연산자 - 자가진단6
        //
        // 두 개의 정수를 입력받아서 다음과 같이 4가지 관계연산자의 결과를 출력하시오.
        // 이때 입력받은 두 정수를 이용하여 출력하시오.
        // (JAVA는 1이면 true, 0이면 false를 출력한다.)
        //
        // 입력 예 :
        // 4 5
        //
        // 출력 예 :
        // 4 > 5 --- 0
        // 4 < 5 --- 1
        // 4 >= 5 --- 0
        // 4 <= 5 --- 1
        //
        //--------------------------------------------------
        static void _523()
        {
            string input = "4 5";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            //c#은 1,0 보단 True, False 가 더 어울림
            Console.WriteLine("{0} > {1} --- {2}", n1, n2, n1 > n2);
            Console.WriteLine("{0} < {1} --- {2}", n1, n2, n1 < n2);
            Console.WriteLine("{0} >= {1} --- {2}", n1, n2, n1 >= n2);
            Console.WriteLine("{0} <= {1} --- {2}", n1, n2, n1 <= n2);
        }



        //--------------------------------------------------
        // 524	연산자 - 자가진단7
        // 
        // 2개의 정수를 입력 받아서 논리곱과 논리합의 결과를 출력하는 프로그램을 작성하시오.
        // (수가 0 이 아닌 경우 참으로, 0 인 경우 거짓으로 간주합니다.)
        //
        // ​hint : 정수 a를 입력받은 후 boolean c = (a != 0);을 실행하면 c에 a의 논리값이 저장된다.
        //
        // 입력 예 :
        // 2 0
        //
        // 출력 예 :
        // 0 1
        //
        //--------------------------------------------------
        static void _524()
        {
            string input = "2 0";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            bool b1 = (n1 != 0);
            bool b2 = (n2 != 0);

            //c#은 1,0 보단 True, False 가 더 어울림
            Console.WriteLine("{0} {1}", (b1 && b2), (b1 || b2));
        }



        //--------------------------------------------------
        // 525	연산자 - 자가진단8
        //
        // 3개의 정수를 입력 받아 첫 번째 수가 가장 크면 1 아니면 0을 출력하고 세 개의 수가 모두 같으면 1 아니면 0을 출력하는 프로그램을 작성하시오.
        // (JAVA는 1이면 true, 0이면 false를 출력한다.)
        //
        // 입력 예 :
        // 10 9 9
        //
        // 출력 예 :
        // 1 0
        //
        //--------------------------------------------------
        static void _525()
        {
            string input = "10 9 9";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);
            int n3 = Convert.ToInt32(arr[2]);

            //c#은 1,0 보단 True, False 가 더 어울림
            Console.WriteLine("{0} {1}", (n1 > n2 && n1 > n3), (n1 == n2 && n2 == n3));
        }



        //--------------------------------------------------
        // 111	연산자 - 형성평가1
        //
        // 국어 영어 수학 컴퓨터 과목의 점수를 정수로 입력받아서 총점과 평균을 구하는 프로그램을 작성하시오.
        // (단 평균의 소수점 이하는 버림 한다.)
        //
        // 입력 예 :
        // 70 95 63 100
        //
        // 출력 예 :
        // sum 328
        // avg 82
        //
        //--------------------------------------------------
        static void _111()
        {
            string input = "70 95 63 100";
            string[] arr = input.Split();

            int kor = Convert.ToInt32(arr[0]);
            int eng = Convert.ToInt32(arr[1]);
            int mat = Convert.ToInt32(arr[2]);
            int com = Convert.ToInt32(arr[3]);

            int sum = kor + eng + mat + com;
            int avg = sum / 4;
            
            Console.WriteLine("sum {0}", sum);
            Console.WriteLine("avg {0}", avg);
        }



        //--------------------------------------------------
        // 112	연산자 - 형성평가2
        //
        // 두 정수를 입력받아서 나눈 몫과 나머지를 다음과 같은 형식으로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 35 10
        //
        // 출력 예 :
        // 35 / 10 = 3...5
        //
        //--------------------------------------------------
        static void _112()
        {
            string input = "35 10";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            Console.WriteLine("{0} / {1} = {2}...{3}", n1, n2, n1/n2, n1%n2);
        }



        //--------------------------------------------------
        // 113	연산자 - 형성평가3
        //
        // 직사각형의 가로와 세로의 길이를 정수형 값으로 입력받은 후 
        // 가로의 길이는 5 증가시키고 세로의 길이는 2배하여 저장한 후 
        // 가로의 길이 세로의 길이 넓이를 차례로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 20 15
        //
        // 출력 예 :
        // width = 25
        // length = 30
        // area = 750
        //
        //--------------------------------------------------
        static void _113()
        {
            string input = "20 15";
            string[] arr = input.Split();

            int width = Convert.ToInt32(arr[0]);
            int length = Convert.ToInt32(arr[1]);

            width += 5; // width = width + 5;
            length *= 2; // length = length * 2;

            Console.WriteLine("width = {0}", width);
            Console.WriteLine("length = {0}", length);
            Console.WriteLine("width = {0}", width* length);
        }



        //--------------------------------------------------
        // 114	연산자 - 형성평가4
        //
        // 두 정수를 입력받아 첫 번째 수는 전치증가연산자를 사용하고 
        // 두 번째 수는 후치감소연산자를 사용하여 출력하고 
        // 바뀐 값을 다시 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10 15
        //
        // 출력 예 :
        // 11 15
        // 11 14
        //
        //--------------------------------------------------
        static void _114()
        {
            string input = "10 15";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            Console.WriteLine("{0} {1}", ++n1, n2--);
            Console.WriteLine("{0} {1}", n1, n2);
        }



        //--------------------------------------------------
        // 115	연산자 - 형성평가5
        //
        // 민수와 기영이의 키와 몸무게를 입력받아 민수가 키도 크고 몸무게도 크면 1 그렇지 않으면 0을 출력하는 프로그램을 작성하시오.
        // (JAVA는 1 이면 true, 0 이면 false를 출력한다.)
        //
        // 입력 예 :
        // 150 35
        // 145 35
        //
        // 출력 예 :
        // 0
        //
        //--------------------------------------------------
        static void _115()
        {
            string input1 = "150 35";
            string[] arr1 = input1.Split();
            int h1 = Convert.ToInt32(arr1[0]);
            int w1 = Convert.ToInt32(arr1[1]);

            string input2 = "145 35";
            string[] arr2 = input2.Split();
            int h2 = Convert.ToInt32(arr2[0]);
            int w2 = Convert.ToInt32(arr2[1]);

            Console.WriteLine("{0}", (h1 > h2 && w1 > w2));
        }



    }
}
