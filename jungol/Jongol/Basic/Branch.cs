using System;

namespace Jungol
{
    // 528	선택제어문 - 자가진단1
    // 529	선택제어문 - 자가진단2
    // 530	선택제어문 - 자가진단3
    // 531	선택제어문 - 자가진단4
    // 532	선택제어문 - 자가진단5
    // 533	선택제어문 - 자가진단6
    // 534	선택제어문 - 자가진단7
    // 535	선택제어문 - 자가진단8
    // 632	선택제어문 - 자가진단9
    // 120	선택제어문 - 형성평가1
    // 121	선택제어문 - 형성평가2
    // 122	선택제어문 - 형성평가3
    // 123	선택제어문 - 형성평가4
    // 124	선택제어문 - 형성평가5

    // 입력은 Console.ReadLine 보다
    // 한 줄의 문자열을 사용하는게 어울린다.
    static class Branch
    {

        public static void Test()
        {
            Util.Call(_528);
            Util.Call(_529);
            Util.Call(_530);
            Util.Call(_531);
            Util.Call(_532);
            Util.Call(_533);
            Util.Call(_534);
            Util.Call(_535);
            Util.Call(_632);
            Util.Call(_120);
            Util.Call(_121);
            Util.Call(_122);
            Util.Call(_123);
            Util.Call(_124);
        }


        //--------------------------------------------------
        // 528	선택제어문 - 자가진단1
        // 
        // 정수를 입력받아 첫 줄에 입력 받은 숫자를 출력하고 둘째 줄에 음수이면 “minus” 라고 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // -5
        //
        // 출력 예 :
        // -5
        // minus
        //
        //--------------------------------------------------
        static void _528()
        {
            string input = "-5";
            int n1 = Convert.ToInt32(input);
            
            Console.WriteLine("{0}", n1);
            if (n1 < 0)
                Console.WriteLine("minus");
        }



        //--------------------------------------------------
        // 529	선택제어문 - 자가진단2
        // 
        // “몸무게+100-키”를 비만수치 공식이라고 하자.
        // 키와 몸무게를 자연수로 입력받아 첫 번째 줄에 비만수치를 출력하고, 비만수치가 0보다 크면 다음줄에 비만("Obesity")이라는 메시지를 출력하는 프로그램을 작성하시오. (출력형식은 아래 출력 예를 참고하세요.)
        //
        // 입력 예 :
        // 155 60
        //
        // 출력 예 :
        // 5
        // Obesity
        //
        //--------------------------------------------------
        static void _529()
        {
            string input = "155 60";
            string[] arr = input.Split();

            int h = Convert.ToInt32(arr[0]);
            int w = Convert.ToInt32(arr[1]);

            int check = w + 100 - h;
            Console.WriteLine("{0}", check);
            if (check > 0)
                Console.WriteLine("Obesity");
        }



        //--------------------------------------------------
        // 530	선택제어문 - 자가진단3
        //
        // 나이를 입력받아 20살 이상이면 "adult"라고 출력하고 그렇지 않으면 몇 년후에 성인이 되는지를 "○ years later"라는 메시지를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 18
        //
        // 출력 예 :
        // 2 years later
        //
        //--------------------------------------------------
        static void _530()
        {
            string input = "18";
            int age = Convert.ToInt32(input);

            if (age >= 20)
                Console.WriteLine("adult");
            else
                Console.WriteLine("{0} years later", 20-age);
        }



        //--------------------------------------------------
        // 531	선택제어문 - 자가진단4
        //
        // 복싱체급은 몸무게가 50.80kg 이하를 Flyweight 61.23kg 이하를 Lightweight, 72.57kg 이하를 Middleweight, 88.45kg 이하를 Cruiserweight, 88.45kg 초과를 Heavyweight라고 하자.
        // 몸무게를 입력받아 체급을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 87.3
        //
        // 출력 예 :
        // Cruiserweight
        //
        //--------------------------------------------------
        static void _531()
        {
            string input = "87.3";
            float w = Convert.ToSingle(input);

            if (w <= 50.80f)
                Console.WriteLine("Flyweight ");
            else if (w <= 61.23f)
                Console.WriteLine("Lightweight");
            else if (w <= 72.57f)
                Console.WriteLine("Middleweight");
            else if (w <= 88.45f)
                Console.WriteLine("Cruiserweight");
            else
                Console.WriteLine("Heavyweight");
        }



        //--------------------------------------------------
        // 532	선택제어문 - 자가진단5
        //
        // 두 개의 실수를 입력받아 모두 4.0 이상이면 "A", 모두 3.0 이상이면 "B", 아니면 "C" 라고 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 4.3 3.5
        // 4.0 2.9
        //
        // 출력 예 :
        // B
        // C
        // 
        //--------------------------------------------------
        static void _532()
        {
            //string input = "4.3 3.5";
            string input = "4.3 2.9";
            string[] arr = input.Split();

            float f1 = Convert.ToSingle(arr[0]);
            float f2 = Convert.ToSingle(arr[1]);

            if (f1 >= 4.0f && f2 >= 4.0f)
                Console.WriteLine("A");
            else if (f1 >= 3.0f && f2 >= 3.0f)
                Console.WriteLine("B");
            else
                Console.WriteLine("C");
        }



        //--------------------------------------------------
        // 533	선택제어문 - 자가진단6
        //
        // 남자는 'M' 여자는 'F'로 나타내기로 하고 18세 이상을 성인이라고 하자.
        // 성별('M', 'F')과 나이를 입력받아 "MAN"(성인남자), "WOMAN"(성인여자), "BOY"(미성년남자), "GIRL"(미성년여자)을 구분하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // F 15
        //
        // 출력 예 :
        // GIRL
        //
        //--------------------------------------------------
        static void _533()
        {
            string input = "F 15";
            string[] arr = input.Split();

            char sex = Convert.ToChar(arr[0]);
            int age = Convert.ToInt32(arr[1]);

            if (sex == 'M') {
                
                if (age >= 18)
                    Console.WriteLine("Man");
                else
                    Console.WriteLine("Boy");
            }
            else if (sex == 'F') {

                if (age >= 18)
                    Console.WriteLine("WOMAN");
                else
                    Console.WriteLine("GIRL");
            }
            else {

                Console.WriteLine("What?");
            }
        }



        //--------------------------------------------------
        // 534	선택제어문 - 자가진단7
        //
        // 영문 대문자를 입력받아 
        // 'A'이면 “Excellent”, 
        // 'B'이면 “Good”, 
        // 'C'이면 “Usually”, 
        // 'D'이면 “Effort”, 
        // 'F'이면 “Failure”, 
        // 그 외 문자이면 “error” 라고 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // B
        //
        // 출력 예 :
        // Good
        //
        //--------------------------------------------------
        static void _534()
        {
            string input = "B";
            char record = Convert.ToChar(input);

            if(record == 'A')
                Console.WriteLine("Excellent");
            else if (record == 'B')
                Console.WriteLine("Good");
            else if (record == 'C')
                Console.WriteLine("Usually");
            else if (record == 'D')
                Console.WriteLine("Effort");
            else if (record == 'F')
                Console.WriteLine("Failure");
            else
                Console.WriteLine("error");

            //switch (record) {
            //    case 'A':
            //        Console.WriteLine("Excellent");
            //        break;
            //    case 'B':
            //        Console.WriteLine("Good");
            //        break;
            //    case 'C':
            //        Console.WriteLine("Usually");
            //        break;
            //    case 'D':
            //        Console.WriteLine("Effort");
            //        break;
            //    case 'F':
            //        Console.WriteLine("Failure");
            //        break;
            //    default:
            //        Console.WriteLine("error");
            //        break;
            //}
        }



        //--------------------------------------------------
        // 535	선택제어문 - 자가진단8
        //
        // 4.5 이하의 평점을 입력받아 그 값에 따라 다음과 같이 출력하는 프로그램을 작성하시오.
        // (C, C++, Java는 switch ~case문 사용) (Python은 if ~else사용)
        // 단 입력는 0이상 4.5 이하이다.
        //
        // 4.0 이상 : "scholarship"
        // 3.0 이상 : "next semester"
        // 2.0 이상 : "seasonalsemester"
        // 2.0 미만 : "retake"​
        //
        // >> 실수를 정수형(int)으로 변환하여 분기한다. (예 - switch( (int)score ))
        //
        // 입력 예 :
        // 3.5
        //
        // 출력 예 :
        // next semester
        //
        //--------------------------------------------------
        static void _535()
        {
            string input = "3.5";
            float record = Convert.ToSingle(input);

            switch ((int)record) {
                case 4:
                    Console.WriteLine("scholarship");
                    break;
                case 3:
                    Console.WriteLine("next semester");
                    break;
                case 2:
                    Console.WriteLine("seasonalsemester");
                    break;
                default:
                    Console.WriteLine("retake");
                    break;
            }
        }



        //--------------------------------------------------
        // 632	선택제어문 - 자가진단9
        //
        // 3개의 정수를 입력받아 조건연산자를 이용하여 입력받은 수들 중 최소값을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 18 -5 10
        //
        // 출력 예 :
        // -5
        //
        //--------------------------------------------------
        static void _632()
        {
            string input = "18 -5 10";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);
            int n3 = Convert.ToInt32(arr[2]);


            int min = n1;
            if (min > n2)
                min = n2;
            if (min > n3)
                min = n3;

            //int min = n1 < n2 ? n1 : n2;
            //min = min < n3 ? min : n3;

            //int min = (n1 < n2 ? n1 : n2) < n3 ? (n1 < n2 ? n1 : n2) : n3;
            
            //int min = Math.Min(Math.Min(n1, n2), n3);

            Console.WriteLine(min);
        }



        //--------------------------------------------------
        // 120	선택제어문 - 형성평가1
        //
        // 두 개의 정수를 입력받아 큰 수에서 작은 수를 뺀 차를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 50 85
        //
        // 출력 예 :
        // 35
        //
        //--------------------------------------------------
        static void _120()
        {
            string input = "50 85";
            string[] arr = input.Split();

            int n1 = Convert.ToInt32(arr[0]);
            int n2 = Convert.ToInt32(arr[1]);

            if (n1 > n2)
                Console.WriteLine(n1 - n2);
            else
                Console.WriteLine(n2 - n1);
        }



        //--------------------------------------------------
        // 121	선택제어문 - 형성평가2
        //
        // 정수를 입력받아 0 이면 "zero" 양수이면 "plus" 음수이면 "minus" 라고 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 0
        //
        // 출력 예 :
        // zero
        //
        //--------------------------------------------------
        static void _121()
        {
            string input = "0";
            int n1 = Convert.ToInt32(input);

            if (n1 == 0)
                Console.WriteLine("zero");
            else if (n1 > 0)
                Console.WriteLine("plus");
            else
                Console.WriteLine("minus");
        }



        //--------------------------------------------------
        // 122	선택제어문 - 형성평가3
        //
        // 년도를 입력받아 윤년(leap year)인지 평년(common year)인지 판단하는 프로그램을 작성하시오.
        //
        // 400으로 나누어떨어지면 윤년이다.
        // 또는 4로 나누어떨어지고 100으로 나누어떨어지지 않으면 윤년이다.
        // 나머지는 모두 평년이다.
        //
        // 입력 예 :
        // 2008
        //
        // 출력 예 :
        // leap year
        //
        //--------------------------------------------------
        static void _122()
        {
            string input = "2008";
            int year = Convert.ToInt32(input);

            if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0)) {
                Console.WriteLine("leap year");
            }
        }



        //--------------------------------------------------
        // 123	선택제어문 - 형성평가4
        // 
        // 1번은 개, 2번은 고양이, 3번은 병아리로 정하고 번호를 입력하면 번호에 해당하는 동물을 영어로 출력하는 프로그램을 작성하시오. 
        // 해당 번호가 없으면 "I don't know."라고 출력한다.
        // 개-dog
        // 고양이-cat
        // 병아리-chick​
        //
        // 입력 예 :
        // Number? 2
        //
        // 출력 예 :
        // cat
        //
        //--------------------------------------------------
        static void _123()
        {
            Console.Write("Number? ");
            string input = "2";
            Console.WriteLine(input);
            int n = Convert.ToInt32(input);

            if (1 == n)
                Console.WriteLine("dog");
            else if (2 == n)
                Console.WriteLine("cat");
            else if (3 == n)
                Console.WriteLine("chick");
            else
                Console.WriteLine("I don't know.");
        }



        //--------------------------------------------------
        // 124	선택제어문 - 형성평가5
        //
        // 1~12사이의 정수를 입력받아 평년의 경우 입력받은 월을 입력받아 평년의 경우 해당 월의 날수를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 2
        //
        // 출력 예 :
        // 28
        //
        //--------------------------------------------------
        static void _124()
        {
            string input = "2";
            int month = Convert.ToInt32(input);

            switch (month) {
                case 2:
                    Console.WriteLine(28);
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    Console.WriteLine(30);
                    break;

                default:
                    Console.WriteLine(31);
                    break;
            }
        }

    }
}