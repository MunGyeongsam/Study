using System;

namespace FinalExam
{
    class Exe
    {
        //1291 : 구구단
        //
        //문제
        //원하는 구구단의 범위를 입력받아 해당 구간의 구구단을 출력하는 프로그램을 작성하시오.
        //
        //
        //<처리조건>
        //(1) 구간의 처음과 끝을 입력받는다.
        //(2) 입력된 구간은 반드시 처음 입력 값이 끝의 입력 값보다 작아야 하는 것은 아니다.
        //
        //즉 입력된 구간의 범위는 증가하거나 감소하는 순서 그대로 출력되어야 한다.

        //입력 예
        //4 3
        //

        //출력 예
        //4 * 1 =  4   3 * 1 =  3
        //4 * 2 =  8   3 * 2 =  6
        //4 * 3 = 12   3 * 3 =  9
        //4 * 4 = 16   3 * 4 = 12
        //4 * 5 = 20   3 * 5 = 15
        //4 * 6 = 24   3 * 6 = 18
        //4 * 7 = 28   3 * 7 = 21
        //4 * 8 = 32   3 * 8 = 24
        //4 * 9 = 36   3 * 9 = 27
        static void GuguDan(int s, int e)
        {
            //
            //1. 중첩된 반복구문 활용 여부
            //2. 인제에 따른 정확한 "iterator 섹션"
            int i = 0, j = 0;

            for (i = 1; i <= 9; i++) //행
            {
                for (j = 4; j >= 3; j--) //열
                {
                    Console.Write(j + "*" + i + "=" + i * j + "\t");
                }
                Console.WriteLine();
            }

        }


        //1307 : 문자사각형1
        //
        //문제
        //정사각형의 한 변의 길이 n을 입력받은 후 다음과 같은 문자로 된 정사각형 형태로 출력하는 프로그램을 작성하시오.
        //
        //
        //< 처리조건 >
        //문자의 진행 순서는 맨 오른쪽 아래에서 위쪽으로 'A'부터 차례대로 채워나가는 방법으로 아래 표와 같이 왼쪽 위까지 채워 넣는다.
        //
        //'Z' 다음에는 다시 'A'부터 반복된다.

        //입력형식
        //정사각형 한 변의 길이 n(n의 범위는 1이상 100 이하의 정수)을 입력받는다.

        //출력형식
        //위의 형식과 같이 한변의 길이가 n인 문자 사각형을 출력한다.문자 사이는 공백으로 구분하여 출력한다.

        //입력 예
        //4

        //출력 예
        //P L H D
        //O K G C
        //N J F B
        //M I E A
        static void LetterRectangle(int n)
        {
            //1. 2차원 배열의 사용 여부
            //2. 문자의 ++연산자 사용
            //3. 경계조건 검사'Z' 또는 'A'
            char ch = 'A';
            char[,] tbl = new char[n, n];
            for (int i = n - 1; i >= 0; --i)
            {

                for (int j = n - 1; j >= 0; --j)
                {
                    tbl[j, i] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
            }

            for (int i = 0; i < n; ++i)
            {

                for (int j = 0; j < n - 1; ++j)
                {
                    Console.Write("{0} ", tbl[i, j]);
                }
                Console.WriteLine(tbl[i, n - 1]);
            }
        }



        //1338 : 문자삼각형1
        //
        //문제
        //삼각형의 높이 N을 입력받아서 아래와 같이 문자 'A'부터 차례대로 왼쪽 대각선으로 채워서 삼각형 모양을 출력하는 프로그램을 작성하시오.
        //
        //<처리조건>
        //(1) 오른쪽 위부터 왼쪽 아래쪽으로 이동하면서 문자 'A'부터 차례대로 채워나간다.
        //(2) N번 행까지 채워지면 다시 오른쪽 둘째 행부터 왼쪽 아래로 채워나간다.
        //(3) 삼각형이 모두 채워질 때까지 반복하면서 채워 나간다. (문자 'Z'다음에는 'A'부터 다시 시작한다.)

        //입력형식
        //삼각형의 높이 N(N의 범위는 100 이하의 양의 정수)을 입력받는다.

        //출력형식
        //주어진 형태대로 높이가 N인 문자삼각형을 출력한다. 문자 사이는 한 개의 공백으로 구분한다.

        //입력 예
        //5

        //출력 예
        //        A
        //      B F
        //    C G J
        //  D H K M
        //E I L N O
        static void LetterTriangle(int n)
        {
            //1. 배열의 사용여부
            //2. 반복구문에서 대각으로의 진행여부
            char[,] arr = new char[n, n];
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                    arr[i, j] = ' ';

            int cnt = n;
            char ch = 'A';
            while (cnt > 0)
            {

                for (int i = n - cnt, j = n - 1; i < n; ++i, --j)
                {
                    arr[i, j] = ch++;
                    if (ch > 'Z')
                        ch = 'A';
                }
                --cnt;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write("{0} ", arr[i, j]);
                Console.WriteLine();
            }
        }

        //1523 : 별삼각형1
        //문제
        //삼각형의 높이 n과 종류 m을 입력받은 후 다음과 같은 삼각형 형태로 출력하는 프로그램을 작성하시오.
        //
        //*        *****       *
        //**       ****       ***
        //***      ***       *****
        //****     **       *******
        //*****    *       *********
        //종류1    종류2   종류3   

        //입력 예
        //3 2

        //출력 예
        //***
        //**
        //*
        static void Line(int n)
        {
            //별로 채워진 한 라인을 출력하도록 구현
            for (int i = 0; i < n; ++i)
                Console.Write('*');
            Console.WriteLine();
        }
        static void Tri1(int n)
        {
            //type 1
            for (int i = 1; i <= n; ++i)
                Line(i);
        }
        static void Tri2(int n)
        {
            //type 2
            for (int i = n; i > 0; --i)
                Line(i);
        }
        static void Tri3(int n)
        {
            //type 3
            //string 생성자를 이용해서 빈칸 및 별 문자열을 출력하도록 구현
            for (int i = 1; i <= n; ++i)
            {
                string s1 = new string(' ', n - i);
                string s2 = new string('*', i * 2 - 1);
                Console.WriteLine("{0}{1}", s1, s2);
            }
        }

        static void StarTriangle(int n, int type)
        {
            if (1 == type)
                Tri1(n);
            else if (2 == type)
                Tri2(n);
            else if (3 == type)
                Tri3(n);
        }


        static void Main()
        {
            Console.WriteLine("---- 구구단");
            GuguDan(7, 2);
            Console.WriteLine();

            Console.WriteLine("---- 문자 사각형");
            LetterRectangle(15);
            Console.WriteLine();

            Console.WriteLine("---- 숫자 삼각형");
            LetterTriangle(5);
            Console.WriteLine();


            Console.WriteLine("---- 별삼각형 1");
            StarTriangle(5, 1);
            Console.WriteLine();

            Console.WriteLine("---- 별삼각형 2");
            StarTriangle(5, 2);
            Console.WriteLine();

            Console.WriteLine("---- 별삼각형 3");
            StarTriangle(5, 3);
            Console.WriteLine();
        }
    }
}
