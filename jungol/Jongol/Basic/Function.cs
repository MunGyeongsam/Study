using System;

namespace Jungol
{
	// 571	함수1 - 자가진단1
	// 572	함수1 - 자가진단2
	// 573	함수1 - 자가진단3
	// 574	함수1 - 자가진단4
	// 575	함수1 - 자가진단5
	// 576	함수1 - 자가진단6
	// 577	함수1 - 자가진단7
	// 578	함수1 - 자가진단8
	// 170	함수1 - 형성평가1
	// 171	함수1 - 형성평가2
	// 172	함수1 - 형성평가3
	// 173	함수1 - 형성평가4
	// 174	함수1 - 형성평가5
	// 579	함수2 - 자가진단1
	// 580	함수2 - 자가진단2
	// 581	함수2 - 자가진단3
	// 582	함수2 - 자가진단4
	// 583	함수2 - 자가진단5
	// 584	함수2 - 자가진단6
	// 585	함수2 - 자가진단7
	// 586	함수2 - 자가진단8
	// 175	함수2 - 형성평가1
	// 176	함수2 - 형성평가2
	// 177	함수2 - 형성평가3
	// 178	함수2 - 형성평가4
	// 179	함수2 - 형성평가5
	// 180	함수2 - 형성평가6
	// 181	함수2 - 형성평가7
	// 587	함수3 - 자가진단1
	// 588	함수3 - 자가진단2
	// 589	함수3 - 자가진단3
	// 590	함수3 - 자가진단4
	// 591	함수3 - 자가진단5
	// 592	함수3 - 자가진단6
	// 231	함수3 - 형성평가1
	// 232	함수3 - 형성평가2
	// 233	함수3 - 형성평가3
	// 234	함수3 - 형성평가4
	// 235	함수3 - 형성평가5
	// 236	함수3 - 형성평가6

	static class Function
	{
		
		public static void Test()
        {
			Util.Call(_571);
			Util.Call(_572);
			Util.Call(_573);
			Util.Call(_574);
			Util.Call(_575);
			Util.Call(_576);
			Util.Call(_577);
			Util.Call(_578);
			Util.Call(_170);
			Util.Call(_171);
			Util.Call(_172);
			Util.Call(_173);
			Util.Call(_174);
			Util.Call(_579);
			Util.Call(_580);
			Util.Call(_581);
			Util.Call(_582);
			Util.Call(_583);
			Util.Call(_584);
			Util.Call(_585);
			Util.Call(_586);
			Util.Call(_175);
			Util.Call(_176);
			Util.Call(_177);
			Util.Call(_178);
			Util.Call(_179);
			Util.Call(_180);
			Util.Call(_181);
			Util.Call(_587);
			Util.Call(_588);
			Util.Call(_589);
			Util.Call(_590);
			Util.Call(_591);
			Util.Call(_592);
			Util.Call(_231);
			Util.Call(_232);
			Util.Call(_233);
			Util.Call(_234);
			Util.Call(_235);
			Util.Call(_236);
		}

		//--------------------------------------------------
		// 571	함수1 - 자가진단1
		//
		// 문자열 "~!@#$^&*()_+|" 를 출력하는 함수를 작성하고 정수를 입력받아 입력받은 수만큼 함수를 호출하는 프로그램을 작성하시오.
		// *문자열을 잘 확인하세요.
		//
		// 입력 예 :
		// 3
		//
		// 출력 예 :
		// ~!@#$^&*()_+|
		// ~!@#$^&*()_+|
		// ~!@#$^&*()_+|
		//
		//--------------------------------------------------
		static void _571_Impl(int n)
		{
			for (int i = 0; i < n; ++i)
				Console.WriteLine("~!@#$^&*()_+|");
		}
		static void _571()
		{
			string input = "3";
			int n = Convert.ToInt32(input);

			_571_Impl(n);
		}



		//--------------------------------------------------
		// 572	함수1 - 자가진단2
		//
		// 반지름의 길이를 전달받아 넓이를 출력하는 함수를 작성하고 반지름의 길이를 입력받아 함수를 호출하여 넓이를 출력하는 프로그램을 작성하시오. 
		// (원주율은 3.14로 하고 반올림하여 소수 둘째자리까지 출력한다.원의 넓이 = 반지름 * 반지름 * 원주율이다.)
		//
		// 입력 예 :
		// 10
		//
		// 출력 예 :
		// 314.00
		//
		//--------------------------------------------------
		static void _572_impl(float radius)
		{
			Console.WriteLine("{0:f2}", radius * radius * 3.14f);
		}
		static void _572()
		{
			string input = "10";
			float r = Convert.ToSingle(input);

			_572_impl(r);
		}



		//--------------------------------------------------
		// 573	함수1 - 자가진단3
		//
		// 100 미만의 정수만 입력된다. 정수 n을 입력받아 n x n 크기의 숫자사각형을 출력하는 프로그램을 작성하시오.
		// 이때 정수 n을 전달받아 숫자 정사각형을 출력하는 함수하고, 입력받은 정수 n을 함수로 전달하여 출력한다.
		//
		// 입력 예 :
		// 4
		//
		// 출력 예 :
		// 1 2 3 4
		// 5 6 7 8
		// 9 10 11 12
		// 13 14 15 16
		//
		//--------------------------------------------------
		static void _573_impl(int n)
		{
			int num = 1;
			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < n; ++j)
				{
					Console.Write("{0} ", num++);
				}
				Console.WriteLine();
			}
		}
		static void _573()
		{
			string input = "4";
			int n = Convert.ToInt32(input);

			_573_impl(n);
		}



		//--------------------------------------------------
		// 574	함수1 - 자가진단4
		//
		// 세 개의 정수를 전달받아 최대값을 구하여 리턴하는 함수를 작성하고 세 정수를 입력받아 최대값을 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// -10 115 33
		//
		// 출력 예 :
		// 115
		//
		//--------------------------------------------------
		static void _574_impl(int n1, int n2, int n3)
		{
			int max = Math.Max(n1, Math.Max(n2, n3));
			Console.WriteLine(max);
		}
		static void _574()
		{
			string input = "-10 115 33";
			string[] inputs = input.Split();

			int n1 = Convert.ToInt32(inputs[0]);
			int n2 = Convert.ToInt32(inputs[1]);
			int n3 = Convert.ToInt32(inputs[2]);

			_574_impl(n1, n2, n3);
		}



		//--------------------------------------------------
		// 575	함수1 - 자가진단5
		//
		// 10 이하의 두 정수를 받아서 첫 번째 수를 두 번째 수만큼 거듭제곱하여 나온 값을 리턴하는 함수를 작성하여 다음과 같이 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// 2 10
		//
		// 출력 예 :
		// 1024
		//
		//--------------------------------------------------
		static void _575_impl(int n, int pow)
		{
			// 10 의 10 승은... int 의 범위를 벗어나므로 long 으로함.
			long rt = 1;
			for (int i = 0; i < pow; ++i)
				rt *= n;

			Console.WriteLine(rt);
		}
		static void _575()
		{
			string input = "10 10";
			string[] inputs = input.Split();

			int n1 = Convert.ToInt32(inputs[0]);
			int n2 = Convert.ToInt32(inputs[1]);

			_575_impl(n1, n2);
		}



		//--------------------------------------------------
		// 576	함수1 - 자가진단6
		// 
		// 정수의 연산식을 입력받아 연산을 위한 함수를 호출하여 4칙연산(+ - * /)의 연산결과를 출력하는 프로그램을 작성하시오. 
		// ('/'의 경우는 정수 부분만 출력하고 4칙연산 이외의 연산 결과는 0으로 한다.)
		// 단 if~ else if~ else 문으로 작성하세요.
		//
		// 입력 예 :
		// 10 + 20
		// 
		// 출력 예 :
		// 10 + 20 = 30
		//
		//--------------------------------------------------
		static void _576_impl(string[] args)
		{
			int n1 = Convert.ToInt32(args[0]);
			int n2 = Convert.ToInt32(args[2]);
			string op = args[1];
			
			int rt = 0;
			if ("+" == op)
			{
				rt = n1 + n2;
			}
			else if ("-" == op)
			{
				rt = n1 - n2;
			}
			else if ("*" == op)
			{
				rt = n1 * n2;
			}
			else if ("/" == op)
			{
				rt = n1 / n2;
			}

			Console.WriteLine("{0} {1} {2} = {3}", n1, op, n2, rt);
		}
		static void _576()
		{
			string input = "10 + 20";
			string[] inputs = input.Split();
			

			_576_impl(inputs);
		}



		//--------------------------------------------------
		// 577	함수1 - 자가진단7
		//
		// 서로 다른 두 개의 정수가 주어진다. 두 정수를 입력받아 큰 수는 2로 나눈 몫을 저장하고 작은 수는 2를 곱하여 저장한 후 출력하는 프로그램을 작성하시오.(참조에 의한 호출을 이용한 함수를 작성하여 값을 수정하고 출력은 메인함수에서 한다.)
		//
		// 입력 예 :
		// 100 500
		//
		// 출력 예 :
		// 200 250
		//
		//--------------------------------------------------
		static void _577_impl(int n1, int n2)
		{
			if (n1 < n2)
			{
				n1 *= 2;
				n2 /= 2;
			}
			else
			{
				n1 /= 2;
				n2 *= 2;
			}
			Console.WriteLine("{0} {1}", n1, n2);
		}
		static void _577()
		{
			string input = "100 500";
			string[] inputs = input.Split();
			int n1 = Convert.ToInt32(inputs[0]);
			int n2 = Convert.ToInt32(inputs[1]);

			_577_impl(n1, n2);
		}



		//--------------------------------------------------
		// 578	함수1 - 자가진단8
		//
		// 10 이하의 두 개의 정수를 입력받아서 작은 수부터 큰 수까지의 구구단을 차례대로 출력하는 프로그램을 구조화하여 작성하시오.
		//
		// 입력 예 :
		// 3 5
		//
		// 출력 예 :
		// == 3dan ==
		// 3 * 1 =  3
		// 3 * 2 =  6
		// 3 * 3 =  9
		// 3 * 4 = 12
		// 3 * 5 = 15
		// 3 * 6 = 18
		// 3 * 7 = 21
		// 3 * 8 = 24
		// 3 * 9 = 27
		// 
		// == 4dan ==
		// 4 * 1 =  4
		// 4 * 2 =  8
		// 4 * 3 = 12
		//  … 
		// 5 * 8 = 40 
		// 5 * 9 = 45
		//
		//--------------------------------------------------
		static void _578_impl(int n1, int n2)
		{
			int from = Math.Min(n1, n2);
			int to = Math.Max(n1, n2);

			for (int i = from; i <= to; ++i)
			{
				Console.WriteLine("== {0}dan ==", i);
				for (int j=1; j<=9; ++j)
					Console.WriteLine("{0} x {1} = {2,2}", i, j, i*j);
				Console.WriteLine();
			}
		}
		static void _578()
		{
			string input = "3 5";
			string[] inputs = input.Split();
			int n1 = Convert.ToInt32(inputs[0]);
			int n2 = Convert.ToInt32(inputs[1]);

			_578_impl(n1, n2);
		}



		//--------------------------------------------------
		// 170	함수1 - 형성평가1
		//
		// ‘@’문자를 10개 출력하는 함수를 작성 한 후 함수를 세 번 호출하여 아래와 같이 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		//
		//
		// 출력 예 :
		// first
		// @@@@@@@@@@
		// second
		// @@@@@@@@@@
		// third
		// @@@@@@@@@@
		//
		//--------------------------------------------------
		static void _170_impl()
		{
			Console.WriteLine("@@@@@@@@@@");
		}
		static void _170()
		{
			Console.WriteLine("first");
			_170_impl();
			Console.WriteLine("second");
			_170_impl();
			Console.WriteLine("third");
			_170_impl();
		}



		//--------------------------------------------------
		// 171	함수1 - 형성평가2
		//
		// 1부터 전달받은 수까지의 합을 출력하는 함수를 작성하고 1000 이하의 자연수를 입력받아 작성한 함수로 전달하여 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// 100
		//
		// 출력 예 :
		// 5050
		//
		//--------------------------------------------------
		static void _171_impl(int n)
		{
			int sum = 0;
			for (int i = 1; i <= n; ++i)
				sum += i;

			Console.WriteLine(sum);
		}
		static void _171()
		{
			string input = "1000";
			int n = Convert.ToInt32(input);

			_171_impl(n);
		}



		//--------------------------------------------------
		// 172	함수1 - 형성평가3
		//
		// 100이하의 자연수를 입력받아 아래와 같은 사각형을 출력하는 프로그램을 작성하시오. (출력하는 부분은 함수로 작성하시오.)
		//
		// 입력 예 :
		// 3
		//
		// 출력 예 :
		// 1 2 3
		// 2 4 6
		// 3 6 9
		//
		//--------------------------------------------------
		static void _172_impl(int n)
		{
			for (int i = 1; i <= n; ++i)
			{
				for (int j = 1; j <= n; ++j)
					Console.Write("{0} ", i * j);
				Console.WriteLine();
			}
		}
		static void _172()
		{
			string input = "3";
			int n = Convert.ToInt32(input);

			_172_impl(n);
		}



		//--------------------------------------------------
		// 173	함수1 - 형성평가4
		//
		// 두 개의 정수를 입력받아 큰 수의 제곱에서 작은 수의 제곱을 뺀 결과값을 출력하는 프로그램을 작성하시오. (두 정수를 전달받아 제곱의 차를 리턴하는 함수를 이용할 것)
		//
		// 입력 예 :
		// 8 10
		//
		// 출력 예 :
		// 36
		//
		//--------------------------------------------------
		static void _173_impl(int n1, int n2)
		{
			int min = Math.Min(n1, n2);
			int max = Math.Max(n1, n2);

			Console.WriteLine(max * max - min * min);
		}
		static void _173()
		{
			string input = "8 10";
			string[] inputs = input.Split();
			int n1 = Convert.ToInt32(inputs[0]);
			int n2 = Convert.ToInt32(inputs[1]);

			_173_impl(n1, n2);
		}



		//--------------------------------------------------
		// 174	함수1 - 형성평가5
		// 
		// 3명 학생의 3과목 점수를 입력받아 각 과목별  학생별 총점을 출력하는 구조화된 프로그램을 작성하시오.
		//
		// 입력 예 :
		// 50 80 100
		// 96 88 66
		// 100 85 90
		//
		// 출력 예 :
		// 50 80 100 230
		// 96 88 66 250
		// 100 85 90 275
		// 246 253 256 755
		//
		//--------------------------------------------------
		static void _174_impl_init(int[,] arr, int row, string input)
		{
			string[] inputs = input.Split();
			for (int i = 0; i < 3; ++i)
				arr[row, i] = Convert.ToInt32(inputs[i]);
		}

		static void _174_impl_sum(int[,] arr)
		{
			for (int i = 0; i < 3; ++i)
				arr[i, 3] = arr[i, 0] + arr[i, 1] + arr[i, 2];
		}
		static void _174_impl_fill_last_row(int[,] arr)
		{
			for (int i = 0; i < 4; ++i)
				arr[3, i] = arr[0, i] + arr[1, i] + arr[2, i];
		}
		static void _174_impl_print(int[,] arr)
		{
			for (int i = 0; i < 4; ++i)
			{
				for (int j = 0; j < 4; ++j)
				{
					Console.Write("{0} ", arr[i, j]);
				}
				Console.WriteLine();
			}
		}

		static void _174()
		{
			string[] inputs = {
				"50 80 100",
				"96 88 66",
				"100 85 90"
			};

			int[,] arr = new int[4, 4];
			
			for (int i = 0; i < inputs.Length; ++i)
				_174_impl_init(arr, i, inputs[i]);

			_174_impl_sum(arr);
			_174_impl_fill_last_row(arr);
			_174_impl_print(arr);
		}



		//--------------------------------------------------
		// 579	함수2 - 자가진단1
		//
		// 10 이하의 자연수 n을 입력받고 n개의 정수를 입력받아 내림차순으로 정렬하여 출력하는 프로그램을 작성하시오. (배열을 전달하는 함수를 이용한다.)
		//
		// 입력 예 :
		// 4
		// 10 9 2 15
		//
		// 출력 예 :
		// 15 10 9 2
		//
		//--------------------------------------------------
		static void _579_impl(string count, string numbers)
		{
			int n = Convert.ToInt32(count);
			int[] arr = new int[n];
			
			string[] inputs = numbers.Split();
			for (int i = 0; i < n; ++i)
				arr[i] = Convert.ToInt32(inputs[i]);

			//@{
			Array.Sort(arr);
			Array.Reverse(arr);

			//Array.Sort(arr, delegate (int n1, int n2) { return n2.CompareTo(n1); });
			//@}

			Console.WriteLine(string.Join(" ", arr));
		}
		static void _579()
		{
			string inputCount = "4";
			string inputNums = "10 9 2 15";
			_579_impl(inputCount, inputNums);
		}



		//--------------------------------------------------
		// 580	함수2 - 자가진단2
		//
		// 2016년의 날짜를 두 개의 정수 월 일로 입력받아서 입력된 날짜가 존재하면 "OK!" 그렇지 않으면 "BAD!"라고 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// 2 30
		//
		// 출력 예 :
		// BAD!
		//
		//--------------------------------------------------
		static void _580_impl(string arg)
		{
			string[] inputs = arg.Split();

			int m = Convert.ToInt32(inputs[0]);
			int d = Convert.ToInt32(inputs[1]);

			int lastDay = DateTime.DaysInMonth(2016, m);
			if (d < 1 || d > lastDay)
				Console.WriteLine("BAD!");
			else
				Console.WriteLine("OK!");
		}
		static void _580()
		{
			string input = "2 30";

			_580_impl(input);
		}



		//--------------------------------------------------
		// 581	함수2 - 자가진단3
		//
		// 두 개의 정수를 입력받아 절대값이 더 큰 수를 출력하고 두 개의 실수를 입력받아 절대값이 작은 수를 출력하는 프로그램을 작성하시오. 실수는 소수점 이하 2자리까지 출력한다.
		//
		// 입력 예 :
		// -50 40
		// -12.34 5.67
		//
		// 출력 예 :
		// -50
		// 5.67
		//
		//--------------------------------------------------
		static void _581_impl(string integers, string reals)
		{
			string[] ia = integers.Split();
			string[] fa = reals.Split();

			int n1 = Convert.ToInt32(ia[0]);
			int n2 = Convert.ToInt32(ia[1]);

			float f1 = Convert.ToSingle(fa[0]);
			float f2 = Convert.ToSingle(fa[1]);

			int n = (Math.Abs(n1) > Math.Abs(n2)) ? n1 : n2;
			float f = (Math.Abs(f1) < Math.Abs(f2)) ? f1 : f2;

			Console.WriteLine(n);
			Console.WriteLine(f);
		}
		static void _581()
		{
			string integers = "-50 40";
			string realNumbers = "-12.34 5.67";

			_581_impl(integers, realNumbers);
		}



		//--------------------------------------------------
		// 582	함수2 - 자가진단4
		// 
		// 원의 넓이를 입력받아 반지름의 길이를 소수 둘째자리까지 출력하는 프로그램을 작성하시오. 
		// 원의 넓이 = 반지름 * 반지름 * 3.14 식을 이용하시오.
		//
		// 입력 예 :
		// 314
		//
		// 출력 예 :
		// 10.00
		//
		//--------------------------------------------------
		static void _582_impl(double area)
		{
			double r = Math.Sqrt(area / 3.14);
			Console.WriteLine("{0:f2}", r);
		}
		static void _582()
		{
			string input = "314";
			double area = Convert.ToDouble(input);

			_582_impl(area);
		}



		//--------------------------------------------------
		// 583	함수2 - 자가진단5
		// 세 개의 실수를 입력받아 가장 큰 수를 올림한 정수를 출력하고 가장 작은 수를 내림한 정수를 출력한 후 남은 수를 반올림한 정수를 출력하는 프로그램을 작성하시오.입력되는 실수는 -1000이상 1000이하이다.
		//
		//
		// 입력 예 :
		// 3.45 51.48 -100.1
		//
		// 출력 예 :
		// 52 -101 3
		//
		//--------------------------------------------------
		static void _583_impl(string input)
		{
			string[] inputs = input.Split();
			float f0 = Convert.ToSingle(inputs[0]);
			float f1 = Convert.ToSingle(inputs[1]);
			float f2 = Convert.ToSingle(inputs[2]);

			float min = Math.Min(f0, Math.Min(f1, f2));
			float max = Math.Max(f0, Math.Max(f1, f2));
			float rem = f0 + f1 + f2 - min - max;

			Console.WriteLine("{0} {1} {2}", Math.Ceiling(max), Math.Floor(min), (int)rem);
		}
		static void _583()
		{
			string input = "3.45 51.48 -100.1";

			_583_impl(input);
		}



		//--------------------------------------------------
		// 584	함수2 - 자가진단6
		// 
		// main() 함수 내에는 숫자를 사용하지 말고 1, 2, 3 세 개의 숫자를 조합하여 가능한 한 모든 합을 출력하는 프로그램을 작성하시오. 
		// 출력예와 같이 출력하시오.
		//
		// 입력 예 :
		// 
		//
		// 출력 예 :
		// 1 + 1 = 2
		// 1 + 2 = 3
		// 1 + 3 = 4
		// 2 + 1 = 3
		// 2 + 2 = 4
		// 2 + 3 = 5
		// 3 + 1 = 4
		// 3 + 2 = 5
		// 3 + 3 = 6
		//
		//--------------------------------------------------
        // C# 에서는 매크로 상수는 문법에 없어 멤버 상수로 대체
		const int ONE = 1;
		const int THREE = 3;
		static void _584()
		{
			for (int i = ONE; i <= THREE; ++i)
			{
				for (int j = ONE; j <= THREE; ++j)
					Console.WriteLine("{0} + {1} = {2}", i, j, i + j);
			}
		}



		//--------------------------------------------------
		// 585	함수2 - 자가진단7
		// 10개의 정수를 입력받아 버블정렬로 내림차순 정렬을 하면서 하나의 단계가 끝날 때마다 그 정렬결과를 출력하는 프로그램을 작성하시오.
		//
		//
		// 입력 예 :
		// 15 93 26 8 43 10 25 88 75 19
		//
		// 출력 예 : 
		// 93 26 15 43 10 25 88 75 19 8 
		// 93 26 43 15 25 88 75 19 10 8	
		// 93 43 26 25 88 75 19 15 10 8	
		// 93 43 26 88 75 25 19 15 10 8	
		// 93 43 88 75 26 25 19 15 10 8	
		// 93 88 75 43 26 25 19 15 10 8	
		// 93 88 75 43 26 25 19 15 10 8	
		// 93 88 75 43 26 25 19 15 10 8	
		// 93 88 75 43 26 25 19 15 10 8	
		//								  
		//--------------------------------------------------
		static void _585_impl(int[] arr)
		{
			for (int i = 0; i < arr.Length-1; ++i)
			{
				for (int j = 1; j < arr.Length-i; ++j)
				{
					if (arr[j] > arr[j - 1])
					{
						int tmp = arr[j];
						arr[j] = arr[j - 1];
						arr[j - 1] = tmp;
					}
				}
				Console.WriteLine(string.Join(" ", arr));
			}
		}
		static void _585()
		{
			string input = "15 93 26 8 43 10 25 88 75 19";
			string[] inputs = input.Split();
			int[] a = new int[inputs.Length];

			for (int i = 0; i < inputs.Length; ++i)
			{
				a[i] = Convert.ToInt32(inputs[i]);
			}

			_585_impl(a);
		}



		//--------------------------------------------------
		// 586	함수2 - 자가진단8
		// 
		// 정수 두 개를 입력받고  계산식을 매크로 함수로 작성하여 두 수의 차를 제곱한 값과 합을 세제곱한 값을 각각 출력하는 프로그램을 작성하시오. (거듭제곱은 '^'로 표시하기로 한다.)
		//
		// 입력 예 :
		// 5 10
		//
		// 출력 예 :
		// (5 - 10) ^ 2 = 25
		// (5 + 10) ^ 3 = 3375
		//
		//--------------------------------------------------
		static void _586_impl(int n1, int n2)
		{
			Console.WriteLine("({0} - {1}) ^ 2 = {2}", n1, n2, (int)Math.Pow((n1 - n2), 2) );
			Console.WriteLine("({0} + {1}) ^ 2 = {2}", n1, n2, (int)Math.Pow((n1 + n2), 3) );
		}
		static void _586()
		{
			string input = "5 10";
			string[] inputs = input.Split();

			int n1 = Convert.ToInt32(inputs[0]);
			int n2 = Convert.ToInt32(inputs[1]);

			_586_impl(n1, n2);
		}



		//--------------------------------------------------
		// 175	함수2 - 형성평가1
		//
		// 정수 N을 입력받고 다시 N개의 정수를 입력받아 내림차순으로 정렬하여 출력하는 프로그램을 작성하시오. 
		// (1 ≤ N ≤ 15, 입력과 출력, 정렬은 모두 함수를 이용할 것)
		//
		// 입력 예 :
		// 5
		// 12 35 1 48 9
		//
		// 출력 예 :
		// 48 35 12 9 1
		//
		//--------------------------------------------------
		static void _175_impl(int[] a)
		{
			// 방법 1.
			//Array.Sort(a);
			//Array.Reverse(a);

			// 방법 2.
			Array.Sort(a, new Comparison<int>((int n1, int n2) => n2.CompareTo(n1)));

			// 방법 3.
			//Array.Sort(a, delegate (int n1, int n2) { return n2.CompareTo(n1); });


			Console.WriteLine(string.Join(" ", a));
		}
		static void _175()
		{
			string input = "5";
			int n = Convert.ToInt32(input);
			int[] a = new int[n];

			input = "12 35 1 48 9";
			string[] inputs = input.Split();
			for (int i = 0; i < n; ++i)
				a[i] = Convert.ToInt32(inputs[i]);

			_175_impl(a);
		}



		//--------------------------------------------------
		// 176	함수2 - 형성평가2
		//
		// 두 개의 실수를 입력받아 각각의 제곱근을 구하고 두 제곱근 사이에 존재하는 정수의 개수를 출력하는 프로그램을 작성하시오. 
		// 단, 입력받는 두 실수는 양수이며 두 제곱근 사이라는 말은 두 제곱근을 포함한다.
		//
		// 입력 예 :
		// 12.0 34.789
		// 
		// 출력 예 :
		// 2
		//
		//--------------------------------------------------
		static void _176_impl(double a, double b)
		{
			double d1 = Math.Sqrt(a);
			double d2 = Math.Sqrt(b);

			//작은 수 내림
			uint n1 = (uint)Math.Ceiling(Math.Min(d1, d2));

			//큰 수 올림
			uint n2 = (uint)Math.Floor(Math.Max(d1, d2));

			//두 정수의 차 + 1
			Console.WriteLine(n2 - n1 + 1);
		}
		static void _176()
		{
			Console.WriteLine("==== NOT IMPLEMENTED");

			string input = "12.0 34.789";
			string[] inputs = input.Split();

			double a = Convert.ToDouble(inputs[0]);
			double b = Convert.ToDouble(inputs[1]);

			_176_impl(a, b);
		}



        //--------------------------------------------------
        // 177	함수2 - 형성평가3
        // 
        // 5개의 정수를 입력받아 각 정수의 절대값의 합을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 35 -20 10 0 55
        //
        // 출력 예 :
        // 120
        //
        //--------------------------------------------------
		static void _177()
		{
			string input = "35 -20 10 0 55";
            string[] args = input.Split();

            int sum = 0;
            for (int i = 0; i < args.Length; ++i) {
                int n = Convert.ToInt32(args[i]);
                sum += Math.Abs(n);
            }

            Console.WriteLine(sum);
		}



        //--------------------------------------------------
        // 178	함수2 - 형성평가4
        //
        // 정수 n을 입력받아 2n의 값을 출력하는 프로그램을 작성하시오. (1 ≤ n ≤ 20)
        //
        // 입력 예 :
        // 10
        //
        // 출력 예 :
        // 1024
        //
        //--------------------------------------------------
		static void _178()
		{
			string input = "10";
			int n = Convert.ToInt32(input);
            
            Console.WriteLine(1 << n);
        }



        //--------------------------------------------------
        // 179	함수2 - 형성평가5
        //
        // 세 개의 실수를 입력받아 합계와 평균을 구하여 평균을 반올림한 정수를 출력하고, 다음은 입력받은 수를 각각 먼저 반올림한 후 합계와 평균을 구하여 평균을 반올림한 한 결과를 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 153.74 34.59 109.5
        //
        // 출력 예 :
        // 99
        // 100
        //
        //--------------------------------------------------
        static void _179_impl(double d1, double d2, double d3)
        {
            //Math.Round(avg); // 반올림

            double sum = d1 + d2 + d3;
            double avg = Math.Round(sum / 3d);
            Console.WriteLine(avg);

            sum = Math.Round(d1) + Math.Round(d2) + Math.Round(d3);
            avg = Math.Round(sum / 3d);
            Console.WriteLine(avg);
		}
		static void _179()
        {
            string input = "153.74 34.59 109.5";
            string[] args = input.Split();

            _179_impl(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]), Convert.ToDouble(args[2]));
		}



        //--------------------------------------------------
        // 180	함수2 - 형성평가6
        //
        // 자료의 개수 7을 매크로 상수로 정의하여 자료의 개수만큼 정수를 입력받아 입력받은 순서대로 앞에서부터 마지막까지 가면서 바로 뒤의 숫자와 비교하여 크면 교환한다. 이러한 작업을 세 번 반복한 후 그 결과를 하나의 행에 공백으로 구분하여 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        // C# 에서는 매크로 상수는 문법에 없어 멤버 상수로 대체
        const int _180_NUM = 7;
        static void _180_impl(int[] arr)
		{
            for (int i = 0; i < 3; ++i) {

                for (int j = 0; j < _180_NUM - 1 - i; ++j) {

                    if (arr[j] > arr[j + 1]) {
                        int tmp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = tmp;
                    }
                }
            }

            Console.WriteLine(string.Join(" ", arr));
		}
		static void _180()
        {
            string input = "15 20 41 8 26 33 19";
            string[] args = input.Split();

            int[] arr = new int[_180_NUM];
            for (int i = 0; i < _180_NUM; ++i) {
                arr[i] = Convert.ToInt32(args[i]);
            }

            _180_impl(arr);
		}



        //--------------------------------------------------
        // 181	함수2 - 형성평가7
        //
        // 원주율을 3.141592로 매크로 상수로 정의하고 원의 넓이를 구하는 매크로 함수를 작성하여 반지름을 입력받아 원의 넓이를 출력하는 프로그램을 작성하시오. (소수 넷째자리에서 반올림)
        //
        // 입력 예 :
        // radius : 1.5
        //
        // 출력 예 :
        // area = 7.069
        //
        //--------------------------------------------------
        // C# 에서는 매크로 상수는 문법에 없어 멤버 상수로 대체
        const double _181_PI = 3.141592;

        // C# 에서는 매크로 함수를 지원하지 않으므로 일반 메소드로 대체
        static void _181_impl(double radius)
		{
            Console.WriteLine("area : {0:F3}", radius * radius * _181_PI);
		}
		static void _181()
		{
            Console.Write("radius : ");

            string input = "1.5";
            double radius = Convert.ToDouble(input);
            Console.WriteLine(radius);

            _181_impl(radius);
		}



        //--------------------------------------------------
        // 587	함수3 - 자가진단1
        //
        // 20 이하의 자연수 N을 입력받아 재귀함수를 이용해서 문자열 “recursive”를 N번 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // recursive
        // recursive
        // recursive
        //
        //--------------------------------------------------
        static void _587_impl(int n)
        {
            if (n <= 0)
                return;

            Console.WriteLine("recursive");
            _587_impl(n - 1);
        }
		static void _587()
		{
			string input = "3";
			int n = Convert.ToInt32(input);

			_587_impl(n);
		}



        //--------------------------------------------------
        // 588	함수3 - 자가진단2
        // 
        // 자연수 N을 입력받아 재귀함수를 이용하여 N부터 1까지 차례대로 출력하는 프로그램을 작성하시오. 
        // N은 50이하의 자연수이다.
        //
        // 입력 예 :
        // 5
        //
        // 출력 예 :
        // 5 4 3 2 1
        //
        //--------------------------------------------------
        static void _588_impl(int n)
		{
            if (n <= 0)
                return;

            Console.Write(n);
            Console.Write(' ');
            _588_impl(n - 1);

		}
		static void _588()
		{
			string input = "5";
			int n = Convert.ToInt32(input);

			_588_impl(n);
		}



        //--------------------------------------------------
        // 589	함수3 - 자가진단3
        // 
        // 100 이하의 자연수 N을 입력받아 재귀함수를 이용하여 1부터 N까지의 합을 구하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 100
        //
        // 출력 예 :
        // 5050
        //
        //--------------------------------------------------
        static int _589_impl(int n)
		{
            if (n > 1)
                return n + _589_impl(n - 1);
            else
                return n;
		}
		static void _589()
		{
			string input = "100";
			int n = Convert.ToInt32(input);

            int sum = _589_impl(n);
            Console.WriteLine(sum);
        }



        //--------------------------------------------------
        // 590	함수3 - 자가진단4
        //
        // 10 이하의 자연수 N을 입력받아 주사위를 N번 던져서 나올 수 있는 모든 경우를 출력하되 중복되는 경우에는 앞에서부터 작은 순으로 1개만 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 3
        //
        // 출력 예 :
        // 1 1 1
        // 1 1 2
        // ...
        // 1 1 6
        // 1 2 2
        // 1 2 3
        // ...
        // 5 6 6
        // 6 6 6
        //
        //--------------------------------------------------
        static void _590_impl(int[] a, int index, int n)
		{
            if (index == n) {
                Console.WriteLine(string.Join(" ", a));
                return;
            }

            int s = index == 0 ? 1 : a[index - 1];
            for (int i = s; i <= 6; ++i) {
                a[index] = i;

                _590_impl(a, index + 1, n);
            }
		}
		static void _590()
		{
			string input = "3";
			int n = Convert.ToInt32(input);

            int[] a = new int[n];

			_590_impl(a, 0, n);

        }



        //--------------------------------------------------
        // 591	함수3 - 자가진단5
        //
        // 첫 번째 수는 1이고 N번째 수는 (N/2)번째 수와 (N-1)번째 수의 합으로 구성된 수열이 있다. 
        // 50 이하의 자연수 N을 입력받아 재귀호출을 이용하여 이 수열에서 N번째 수를 출력하는 프로그램을 작성하시오.
        // (1 2 3 5 7 10 13 18 …)
        //
        // 입력 예 :
        // 8
        //
        // 출력 예 :
        // 18
        //
        //--------------------------------------------------
        static int _591_impl(int n)
		{
            if (n == 1)
                return 1;
            
            return _591_impl(n/2) + _591_impl(n-1);
		}
		static void _591()
		{
			string input = "8";
			int n = Convert.ToInt32(input);
            
			Console.WriteLine(_591_impl(n));
        }



        //--------------------------------------------------
        // 592	함수3 - 자가진단6
        //
        // 9자리 이하의 자연수를 입력받아 재귀함수를 이용하여 각 자리 숫자의 제곱의 합을 출력하는 프로그램을 작성하시오.
        // ( main()함수에 변수 하나, 재귀함수에 매개변수 하나만을 사용할 수 있다.)
        //
        // 입력 예 :
        // 12345
        //
        // 출력 예 :
        // 55
        //
        //--------------------------------------------------
        static int _592_impl(int n)
		{
            if (n < 10)
                return n * n;

            int d = n % 10;

            return d * d + _592_impl(n / 10);
		}
		static void _592()
		{
			string input = "12345";
			int n = Convert.ToInt32(input);

			Console.WriteLine(_592_impl(n));
		}



        //--------------------------------------------------
        // 231	함수3 - 형성평가1
        //
        // 자연수 N을 입력받아 1부터 N까지 출력을 하되 n-1번째는 n을 2로 나눈 몫이 되도록 하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 100
        //
        // 출력 예 :
        // 1 3 6 12 25 50 100
        //
        //--------------------------------------------------
        static void _231_impl(int n)
		{
            if (n <= 1) {
                Console.Write(n);
            } else {
                _231_impl(n / 2);

                Console.Write(' ');
                Console.Write(n);
            }
		}
		static void _231()
		{
			string input = "100";
			int n = Convert.ToInt32(input);

			_231_impl(n);
		}



        //--------------------------------------------------
        // 232	함수3 - 형성평가2
        //
        // 자연수 N을 입력받아 N이 홀수인 경우에는 1부터 N까지의 홀수를  짝수인 경우는 2부터 N까지의 짝수를 모두 출력하는 프로그램을 재귀함수로 작성하시오.
        //
        // 입력 예 :
        // 15
        //
        // 출력 예 :
        // 1 3 5 7 9 11 13 15
        //
        //--------------------------------------------------
        static void _232_impl(int n)
		{
            if (n <= 2) {
                Console.Write(n);
            }
            else {
                _232_impl(n - 2);

                Console.Write(' ');
                Console.Write(n);
            }
		}
		static void _232()
		{
			string input = "15";
			int n = Convert.ToInt32(input);

            _232_impl(n);
        }



        //--------------------------------------------------
        // 233	함수3 - 형성평가3
        // 
        // 자연수 N과 M을 입력받아서 주사위를 N번 던져서 나온 눈의 합이 M이 나올 수 있는 모든 경우를 출력하는 프로그램을 작성하시오. 
        // 단, N은 10 이하의 정수이다.
        //
        // 입력 예 :
        // 3 10
        //
        // 출력 예 :
        // 1 3 6
        // 1 4 5
        // 1 5 4
        // 1 6 3
        // 2 2 6
        // 2 3 5
        // …
        // 6 2 2
        // 6 3 1
        //
        //--------------------------------------------------
        static void _233_impl(int N, int n, int m, int[] a)
		{
            if (n == 0) {
                Console.WriteLine(string.Join(" ", a));
                return;
            }

            int sum = 0;
            for (int i = 0; i < N - n; ++i)
                sum += a[i];

            for (int i = 1; i <= 6; ++i) {
                if (i + sum + 6 * (n - 1) < m)
                    continue;

                if (n == 1 && sum + i != m)
                    continue;

                a[N - n] = i;
                _233_impl(N, n - 1, m, a);
            }
		}
		static void _233()
		{
			string input = "3 10";
            string[] args = input.Split();

            int n = Convert.ToInt32(args[0]);
            int m = Convert.ToInt32(args[1]);

            int[] a = new int[n];

            _233_impl(n, n, m, a);
        }



        //--------------------------------------------------
        // 234	함수3 - 형성평가4
        //
        // 첫 번째는 1, 두 번째는 2, 세 번째부터는 앞의 두 수의 곱을 100으로 나눈 나머지로 이루어진 수열이 있다. 
        // 100 이하의 자연수 N을 입력받아 재귀함수를 이용하여 N번째 값을 출력하는 프로그램을 작성하시오.
        //
        // 입력 예 :
        // 8
        //
        // 출력 예 :
        // 92
        //
        //--------------------------------------------------
        static int _234_impl(int n)
		{
            if (n <= 2)
                return n;
            else
                return (_234_impl(n - 2) * _234_impl(n - 1)) % 100;
		}
		static void _234()
		{
			string input = "8";
			int n = Convert.ToInt32(input);

			Console.WriteLine(_234_impl(n));
		}



        //--------------------------------------------------
        // 235	함수3 - 형성평가5
        //
        // 100만 이하의 자연수 N을 입력받아 짝수이면 2로  홀수이면 3으로 나누는 작업을 반복하다가 그 값이 1이 되면 그때까지 나누었던 작업의 횟수를 출력하는 프로그램을 재귀함수로 작성하시오.
        //
        // 입력 예 :
        // 100
        //
        // 출력 예 :
        // 6
        //
        //--------------------------------------------------
        static void _235_impl(int n, int cnt)
		{
            //Console.WriteLine("{0,3} : {1}", n, cnt);
            if (n == 1)
                Console.WriteLine(cnt);
            else if (n % 2 == 0)
                _235_impl(n / 2, cnt + 1);
            else
                _235_impl(n / 3, cnt + 1);
        }
		static void _235()
		{
			string input = "100";
			int n = Convert.ToInt32(input);

			_235_impl(n, 0);
		}



        //--------------------------------------------------
        // 236	함수3 - 형성평가6
        //
        // 3자리로 이루어진 자연수 3개를 입력받아 그 수들의 곱을 구한 후 그 결과값의 각 자리 숫자들중 0을 제외한 모든 수들의 곱을 구하여 출력하는 프로그램을 재귀함수로 작성하시오.
        //
        // 입력 예 :
        // 100 123 111
        //
        // 출력 예 :
        // 270
        //
        //--------------------------------------------------
        static void _236_impl(int n, int rt)
		{
            if (n < 10) {
                Console.WriteLine(rt * n);
                return;
            }

            int rem = n % 10;
            if (rem == 0)
                rem = 1;

            _236_impl(n / 10, rt * rem);

		}
		static void _236()
		{
			string input = "100 123 111";
            string[] args = input.Split();

            int a = Convert.ToInt32(args[0]);
            int b = Convert.ToInt32(args[1]);
            int c = Convert.ToInt32(args[2]);

            _236_impl(a*b*c, 1);
		}

	}
}
