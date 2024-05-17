using System;

namespace Jungol
{
    // 555	배열1 - 자가진단1
    // 556	배열1 - 자가진단2
    // 557	배열1 - 자가진단3
    // 558	배열1 - 자가진단4
    // 559	배열1 - 자가진단5
    // 560	배열1 - 자가진단6
    // 561	배열1 - 자가진단7
    // 562	배열1 - 자가진단8
    // 563	배열1 - 자가진단9
    // 150	배열1 - 형성평가1
    // 151	배열1 - 형성평가2
    // 152	배열1 - 형성평가3
    // 153	배열1 - 형성평가4
    // 154	배열1 - 형성평가5
    // 155	배열1 - 형성평가6
    // 156	배열1 - 형성평가7
    // 157	배열1 - 형성평가8
    // 158	배열1 - 형성평가9
    // 159	배열1 - 형성평가A

    // 입력은 필요하다고 생각되는 경우만 Console.ReadLine 을 사용하고
    // 가능한 문자열을 사용.
    static class Array1
	{
		
		public static void Test()
        {
            Util.Call(_555);
            Util.Call(_556);
            Util.Call(_557);
            Util.Call(_558);
            Util.Call(_559);
            Util.Call(_560);
            Util.Call(_561);
            Util.Call(_562);
            Util.Call(_563);
            Util.Call(_150);
            Util.Call(_151);
            Util.Call(_152);
            Util.Call(_153);
            Util.Call(_154);
            Util.Call(_155);
            Util.Call(_156);
            Util.Call(_157);
            Util.Call(_158);
            Util.Call(_159);
        }


        //--------------------------------------------------
        // 555	배열1 - 자가진단1
        //
        // 문자 10개를 저장할 수 있는 배열을 만들고 10개의 문자를 입력받아 입력받은 문자를 이어서 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // A B C D E F G H I J
        //
        // 출력 예 :
        // ABCDEFGHIJ
        //
        //--------------------------------------------------
        static void _555()
        {
            string input = "A B C D E F G H I J";
            string[] inputs = input.Split();

            char[] arr = new char[inputs.Length];

            for (int i = 0; i < inputs.Length; ++i)
                arr[i] = Convert.ToChar(inputs[i]);

            for (int i = 0; i < arr.Length; ++i)
                Console.Write(arr[i]);

            Console.WriteLine();
        }


        //--------------------------------------------------
        // 556	배열1 - 자가진단2
        //
        // 정수 10 개를 저장할 수 있는 배열을 만들어서 1 부터 10 까지를 대입하고 차례대로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        // 1 2 3 4 5 6 7 8 9 10
        //
        //--------------------------------------------------
        static void _556()
        {
            //cc dsfc vc int[] arr = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            for (int i = 0; i < 10; ++i) {

                Console.Write(arr[i]);
                Console.Write(' ');
            }
        }



        //--------------------------------------------------
        // 557	배열1 - 자가진단3
        //
        // 10개의 문자를 입력받아서 첫 번째 네 번째 일곱 번째 입력받은 문자를 차례로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // A B C D E F G H I J
        //
        // 출력 예 :
        // A D G
        //
        //--------------------------------------------------
        static void _557()
        {
            string input = "A B C D E F G H I J";
            string[] inputs = input.Split();

            char[] arr = new char[10];
            for (int i = 0; i < 10; ++i)
                arr[i] = Convert.ToChar(inputs[i]);

            Console.WriteLine("{0} {1} {2}", arr[0], arr[3], arr[6]);
        }



        //--------------------------------------------------
        // 558	배열1 - 자가진단4
        //
        // 100 개의 정수를 저장할 수 있는 배열을 선언하고 정수를 차례로 입력받다가 0 이 입력되면 0 을 제외하고 그 때까지 입력된 정수를 가장 나중에 입력된 정수부터 차례대로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3 5 10 55 0
        //
        // 출력 예 :
        // 55 10 5 3
        //
        //--------------------------------------------------
        static void _558()
        {
            string input = "3 5 10 55 0";
            string[] inputs = input.Split();

            int last = 0;
            int[] arr = new int[100];
            for (int i = 0; i < inputs.Length; ++i) {

                int n = Convert.ToInt32(inputs[i]);
                if (n == 0)
                    break;

                arr[i] = n;
                last = i;
            }

            for (int i = last; i >= 0; --i) {

                Console.Write(arr[i]);
                Console.Write(' ');
            }
        }



        //--------------------------------------------------
        // 559	배열1 - 자가진단5
        //
        // 1반부터 6반까지의 평균점수를 저장한 후 두 반의 반 번호를 입력받아 두 반 평균점수의 합을 출력하는 프로그램을 작성하시오. 
        // 반별 평균점수는 초기값으로 1반부터 차례로 85.6 79.5 83.1 80.0 78.2 75.0으로 초기화하고
        // ~출력은 소수 두 번째 자리에서 반올림하여 소수 첫째자리까지 한다.
        //
        // 입력 예 :
        // 1 3
        //
        // 출력 예 :
        // 168.7
        //
        //--------------------------------------------------
        static void _559()
        {
            double[] avgs = { 85.6, 79.5, 83.1, 80.0, 78.2, 75.0 };

            string input = "1 3";
            string[] inputs = input.Split();

            int n1 = Convert.ToInt32(inputs[0]);
            int n2 = Convert.ToInt32(inputs[1]);
            
            // 1반 : 0
            // 3반 : 2
            Console.WriteLine("{0:f1}", avgs[n1-1] + avgs[n2-1]);
        }



        //--------------------------------------------------
        // 560	배열1 - 자가진단6
        //
        // 10개의 정수를 입력받아 그 중 가장 작은 수를 출력하는 프로그램을 작성하시오.(입력받을 정수는 1000을 넘지 않는다.)
        //
        // 입력 예 :
        // 5 10 8 55 6 31 12 24 61 2
        //
        // 출력 예 :
        // 2
        //
        //--------------------------------------------------
        static void _560()
        {
            string input = "5 10 8 55 6 31 12 24 61 2";
            string[] inputs = input.Split();
            
            int[] arr = new int[10];
            for (int i = 0; i < 10; ++i)
                arr[i] = Convert.ToInt32(inputs[i]);

            int min = arr[0];
            for (int i = 1; i < 10; ++i)
                if (min > arr[i])
                    min = arr[i];

            Console.WriteLine(min);
        }



        //--------------------------------------------------
        // 561	배열1 - 자가진단7
        //
        // 10개의 정수를 입력받아 100 미만의 수 중 가장 큰 수와 100 이상의 수 중 가장 작은 수를 출력하는 프로그램을 작성하시오.
        // (입력되는 정수의 범위는 1이상 10,000 미만이다.만약 해당하는 수가 없을 때에는 100 을 출력한다.)
        //
        // 입력 예 :
        // 88 123 659 15 443 1 99 313 105 48
        //
        // 출력 예 :
        // 99 105
        //
        //--------------------------------------------------
        static void _561()
        {
            string input = "88 123 659 15 443 1 99 313 105 48";
            string[] inputs = input.Split();

            int[] arr = new int[10];
            for (int i = 0; i < 10; ++i)
                arr[i] = Convert.ToInt32(inputs[i]);

            int maxUnder100 = int.MinValue;
            int minOver100 = int.MaxValue;
            for (int i = 0; i < 10; ++i) {

                int n = arr[i];
                if (n < 100) {
                    if (n > maxUnder100)
                        maxUnder100 = n;
                }
                else if (n < minOver100) {

                    minOver100 = n;
                }
            }

            Console.WriteLine("{0} {1}", maxUnder100, minOver100);
        }



        //--------------------------------------------------
        // 562	배열1 - 자가진단8
        //
        // 10개의 정수를 입력받아 배열에 저장한 후 짝수 번째 입력된 값의 합과 홀수 번째 입력된 값의 평균을 출력하는 프로그램을 작성하시오. 
        // 평균은 반올림하여 소수첫째자리까지 출력한다.
        //
        // 입력 예 :
        // 95 100 88 65 76 89 58 93 77 99
        //
        // 출력 예 :
        // sum : 446
        // avg : 78.8
        //
        //--------------------------------------------------
        static void _562()
        {
            string input = "95 100 88 65 76 89 58 93 77 99";
            string[] inputs = input.Split();

            int[] arr = new int[10];
            for (int i = 0; i < 10; ++i)
                arr[i] = Convert.ToInt32(inputs[i]);

            int sum1 = 0;
            int sum2 = 0;
            for (int i = 0; i < 10; i += 2) {

                sum2 += arr[i];     // 홀수 번째
                sum1 += arr[i + 1]; // 짝수 번째
            }

            Console.WriteLine("sum : {0}", sum1);
            Console.WriteLine("avg : {0:f1}", sum2 / 5f);
        }


        //--------------------------------------------------
        // 563	배열1 - 자가진단9
        //
        // 10개의 정수를 입력받아 배열에 저장한 후 내림차순으로 정렬하여 출력하시오.
        //
        // 입력 예 :
        // 95 100 88 65 76 89 58 93 77 99
        //
        // 출력 예 :
        // 100 99 95 93 89 88 77 76 65 58
        //
        //--------------------------------------------------
        static void _563()
        {
            string input = "95 100 88 65 76 89 58 93 77 99";
            string[] inputs = input.Split();

            int[] arr = new int[10];
            for (int i = 0; i < 10; ++i)
                arr[i] = Convert.ToInt32(inputs[i]);

            //Array.Sort(arr);                            // 1. 정렬
            //Array.Reverse(arr);                         // 2. 역순
            //Console.WriteLine(string.Join(" ", arr));   // 3. 문자열로 출력
            
            Array.Sort(arr, new Comparison<int>( (i1, i2) => i2.CompareTo(i1)));    // 1. 역순으로 정렬
            Console.WriteLine(string.Join(" ", arr));                               // 2. 문자열로 출력
        }



        //--------------------------------------------------
        // 150	배열1 - 형성평가1
        //
        // 10개의 문자를 입력받아 마지막으로 입력받은 문자부터 첫 번째 입력받은 문자까지 차례로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // A E C X Y Z c b z e
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _150()
        {
            string input = "A E C X Y Z c b z e";
            string[] inputs = input.Split();

            char[] arr = new char[10];
            for (int i = 0; i < 10; ++i)
                arr[i] = Convert.ToChar(inputs[i]);

            for (int i = arr.Length - 1; i >= 0; --i)
                Console.Write("{0} ", arr[i]);

            Console.WriteLine();
        }



        //--------------------------------------------------
        // 151	배열1 - 형성평가2
        //
        // 5개의 정수를 입력받은 후 첫 번째 세 번째 다섯 번째 입력받은 정수의 합을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 15 20 33 10 9
        //
        // 출력 예 :
        // 57
        //
        //--------------------------------------------------
        static void _151()
        {
            string input = "15 20 33 10 9";
            string[] inputs = input.Split();

            int[] arr = new int[5];
            for (int i = 0; i < 5; ++i)
                arr[i] = Convert.ToInt32(inputs[i]);

            int sum = arr[0] + arr[2] + arr[4];
            Console.WriteLine(sum);
        }



        //--------------------------------------------------
        // 152	배열1 - 형성평가3
        //
        // 10개의 정수를 입력받아 홀수 번째 입력받은 정수의 합과 짝수 번째 입력받은 정수의 합을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 10 20 25 66 83 7 22 90 1 100
        //
        // 출력 예 :
        // odd : 141
        // even : 283
        //
        //--------------------------------------------------
        static void _152()
        {
            string input = "10 20 25 66 83 7 22 90 1 100";
            string[] inputs = input.Split();

            int[] arr = new int[10];
            for (int i = 0; i < arr.Length; ++i)
                arr[i] = Convert.ToInt32(inputs[i]);

            int odd = 0;
            int even = 0;
            for (int i = 0; i < arr.Length; i += 2) {

                odd += arr[i];
                even += arr[i + 1];
            }

            Console.WriteLine("odd : {0}", odd);
            Console.WriteLine("even : {0}", even);
        }



        //--------------------------------------------------
        // 153	배열1 - 형성평가4
        //
        // 100개의 정수를 입력받을 수 있는 배열을 선언한 후 정수를 차례로 입력 받다가 -1이 입력되면 입력을 중단하고 -1을 제외한 마지막 세 개의 정수를 출력하는 프로그램을 작성하시오. (입력받은 정수가 -1을 제외하고 3개 미만일 경우에는 -1을 제외하고 입력받은 정수를 모두 출력한다.)
        //
        // 입력 예 :
        // 30 20 10 60 80 -1
        //
        // 출력 예 :
        // 10 60 80
        //
        //--------------------------------------------------
        static void _153()
        {
            string input = "30 20 10 60 80 -1";
            string[] inputs = input.Split();

            int[] arr = new int[100];
            int last = 0;
            for (int i = 0; i < arr.Length; ++i) {

                int n = Convert.ToInt32(inputs[i]);
                if (n == -1)
                    break;

                arr[i] = n;
                last = i;
            }

            int first = Math.Max(0, last - 2);

            for (int i = first; i <= last; ++i) {
                Console.Write("{0} ", arr[i]);
            }
            Console.WriteLine(); 
        }



        //--------------------------------------------------
        // 154	배열1 - 형성평가5
        // 
        // 6명의 몸무게를 입력받아 몸무게의 평균을 출력하는 프로그램을 작성하시오. 
        // 출력은 반올림하여 소수 첫째자리까지로 한다
        //
        // 입력 예 :
        // 23.2 39.6 66.4 50.0 45.6 48.0
        //
        // 출력 예 :
        // 45.5
        //
        //--------------------------------------------------
        static void _154()
        {
            string input = "23.2 39.6 66.4 50.0 45.6 48.0";
            string[] inputs = input.Split();

            float[] arr = new float[6];
            for (int i = 0; i < arr.Length; ++i) {

                arr[i] = Convert.ToSingle(inputs[i]);
            }

            float tot = 0f;
            for (int i = 0; i < arr.Length; ++i) {

                tot += arr[i];
            }
            Console.WriteLine("{0:f1}", tot / arr.Length);
        }



        //--------------------------------------------------
        // 155	배열1 - 형성평가6
        //
        // 6개의 문자배열을 만들고 {'J' 'U' 'N' 'G' 'O' 'L'} 으로 초기화 한 후 문자 한 개를 입력받아 배열에서의 위치를 출력하는 프로그램을 작성하시오.
        // 첫 번째 위치는 0번이며 배열에 없는 문자가 입력되면 "없는 문자입니다." 라는 메시지를 출력하고 끝내는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // L
        // B
        //
        // 출력 예 :
        // 5
        // 없는 문자입니다.
        //
        //--------------------------------------------------
        static void _155()
        {
            char[] arr = { 'J', 'U', 'N', 'G', 'O', 'L' };


            string input = "L B";
            string[] inputs = input.Split();

            for (int i = 0; i < inputs.Length; ++i) {

                char ch = Convert.ToChar(inputs[i]);
                int index = Array.IndexOf(arr, ch);

                if (index == -1)
                    Console.WriteLine("없는 문자입니다.");
                else
                    Console.WriteLine(index);
            }
        }



        //--------------------------------------------------
        // 156	배열1 - 형성평가7
        //
        // 세 자리 이하의 정수를 차례로 입력 받다가 999가 입력되면 프로그램을 종료하고 그 때까지 입력된 최대값과 최소값을 출력하는 프로그램을 작성하시오.입력받는 정수는 100개 이하이다.
        //
        // 입력 예 :
        // 45 19 123 58 10 -55 16 -1 999
        //
        // 출력 예 :
        // max : 123 
        // min : -55
        //
        //--------------------------------------------------
        static void _156()
        {
            string input = "45 19 123 58 10 -55 16 -1 999";
            string[] inputs = input.Split();
            
            System.Collections.Generic.List<int> arr = new System.Collections.Generic.List<int>();

            for (int i = 0; i < inputs.Length; ++i) {

                int n = Convert.ToInt32(inputs[i]);

                if (n == 999)
                    break;

                arr.Add(n);
            }

            int max = int.MinValue;
            int min = int.MaxValue;
            for (int i = 0; i < arr.Count; ++i) {

                int n = arr[i];

                if (n > max)
                    max = n;
                if (n < min)
                    min = n;
            }

            Console.WriteLine("max : {0}", max);
            Console.WriteLine("min : {0}", min);
        }



        //--------------------------------------------------
        // 157	배열1 - 형성평가8
        //
        // 100 개 이하의 정수를 입력받다가 0 이 입력되면 그 때까지 입력된 정수 중 5의 배수의 개수와 합계 평균을 출력하는 프로그램을 작성하시오.
        // 평균은 소수점 이하 1자리까지 출력한다.
        //
        // 입력 예 :
        // 35 10 23 100 64 51 5 0
        //
        // 출력 예 :
        // max : 123 
        // min : -55
        //
        //--------------------------------------------------
        static void _157()
        {
            string input = "35 10 23 100 64 51 5 0";
            string[] inputs = input.Split();

            int[] arr = new int[100];
            int last = 0;
            for (int i = 0; i < arr.Length; ++i) {

                int n = Convert.ToInt32(inputs[i]);
                if (n == 0)
                    break;

                arr[i] = n;
                last = i;
            }

            int sum = 0;
            int cnt = 0;
            for (int i = 0; i <= last; ++i) {

                int n = arr[i];

                if (n % 5 == 0) {
                    sum += n;
                    ++cnt;
                }
            }

            Console.WriteLine("Multiples of 5 : {0}", cnt);
            Console.WriteLine("sum : {0}", sum);
            Console.WriteLine("avg : {0:f1}", sum / (float)cnt);
        }



        //--------------------------------------------------
        // 158	배열1 - 형성평가9
        // 
        // 100개 이하의 정수를 입력받다가 0 이 입력되면 0 을 제외하고 그 때까지 입력 받은 개수를 출력한 후 입력받은 정수를 차례로 출력하되 그 수가 홀수이면 2배한 값을 짝수인 경우에는 2로 나눈 몫을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 8 10 5 15 100 0
        //
        // 출력 예 :
        // 5
        // 4 5 10 30 50
        //
        //--------------------------------------------------
        static void _158()
        {
            string input = "8 10 5 15 100 0";
            string[] inputs = input.Split();

            int[] arr = new int[100];
            int cnt = 0;
            for (int i = 0; i < arr.Length; ++i) {

                int n = Convert.ToInt32(inputs[i]);
                if (n == 0)
                    break;

                arr[i] = n;
                cnt++;
            }

            Console.WriteLine(cnt);

            for (int i = 0; i < cnt; ++i) {

                int n = arr[i];
                if (n % 2 == 1)
                    n *= 2;
                else
                    n /= 2;
                Console.Write("{0} ", n); 
            }
        }



        //--------------------------------------------------
        // 159	배열1 - 형성평가A
        //
        // 20 이하의 정수 n을 입력받고 n명의 점수를 입력받아 높은 점수부터 차례로 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 5
        // 35 10 35 100 64
        // 
        // 출력 예 :
        // 100
        // 64
        // 35
        // 35
        // 10
        //
        //--------------------------------------------------
        static void _159()
        {
            //@{ Console.ReadLine 을 이용해 구현한 버전

            //int n = Convert.ToInt32(Console.ReadLine());
            //int[] arr = new int[n];
            //
            //string[] scores = Console.ReadLine().Split();
            //for (int i = 0; i < n; ++i)
            //    arr[i] = Convert.ToInt32(scores[i]);
            //
            //Array.Sort(arr);                    // 1. 정렬            
            //for (int i = n - 1; i >= 0; --i)    // 2. 역순 출력
            //    Console.WriteLine(arr[i]);

            //@}


            string input_n = "5";
            int n = Convert.ToInt32(input_n);

            int[] arr = new int[n];

            string input_scores = "35 10 35 100 64";
            string[] scores = input_scores.Split();
            for (int i = 0; i < n; ++i)
                arr[i] = Convert.ToInt32(scores[i]);
            
            Array.Sort(arr);                    // 1. 정렬            
            for (int i = n - 1; i >= 0; --i)    // 2. 역순 출력
                Console.WriteLine(arr[i]);
        }
    }
}
