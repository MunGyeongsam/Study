using System;

namespace Jungol
{
    // 593	문자열1 - 자가진단1
    // 594	문자열1 - 자가진단2
    // 595	문자열1 - 자가진단3
    // 596	문자열1 - 자가진단4
    // 597	문자열1 - 자가진단5
    // 598	문자열1 - 자가진단6
    // 599	문자열1 - 자가진단7
    // 600	문자열1 - 자가진단8
    // 601	문자열1 - 자가진단9
    // 182	문자열1 - 형성평가1
    // 183	문자열1 - 형성평가2
    // 184	문자열1 - 형성평가3
    // 185	문자열1 - 형성평가4
    // 186	문자열1 - 형성평가5
    // 187	문자열1 - 형성평가6
    // 188	문자열1 - 형성평가7
    // 602	문자열2 - 자가진단1
    // 603	문자열2 - 자가진단2
    // 604	문자열2 - 자가진단3
    // 605	문자열2 - 자가진단4
    // 606	문자열2 - 자가진단5
    // 607	문자열2 - 자가진단6
    // 608	문자열2 - 자가진단7
    // 609	문자열2 - 자가진단8
    // 610	문자열2 - 자가진단9
    // 611	문자열2 - 자가진단A
    // 612	문자열2 - 자가진단B
    // 189	문자열2 - 형성평가1
    // 190	문자열2 - 형성평가2
    // 191	문자열2 - 형성평가3
    // 192	문자열2 - 형성평가4
    // 193	문자열2 - 형성평가5
    // 194	문자열2 - 형성평가6
    // 215	문자열2 - 형성평가7
    // 216	문자열2 - 형성평가8
    // 237	문자열2 - 형성평가9

    // 입력은 필요하다고 생각되는 경우만 Console.ReadLine 을 사용하고
    // 가능한 문자열을 사용.
    // 단, 필요하다고 판단이 되면 주석처리로 Console.ReadLine 버전을 예시.
    static class String
	{
		
		public static void Test()
        {
            Util.Call(_593);
            Util.Call(_594);
            Util.Call(_595);
            Util.Call(_596);
            Util.Call(_597);
            Util.Call(_598);
            Util.Call(_599);
            Util.Call(_600);
            Util.Call(_601);
            Util.Call(_182);
            Util.Call(_183);
            Util.Call(_184);
            Util.Call(_185);
            Util.Call(_186);
            Util.Call(_187);
            Util.Call(_188);
            Util.Call(_602);
            Util.Call(_603);
            Util.Call(_604);
            Util.Call(_605);
            Util.Call(_606);
            Util.Call(_607);
            Util.Call(_608);
            Util.Call(_609);
            Util.Call(_610);
            Util.Call(_611);
            Util.Call(_612);
            Util.Call(_189);
            Util.Call(_190);
            Util.Call(_191);
            Util.Call(_192);
            Util.Call(_193);
            Util.Call(_194);
            Util.Call(_215);
            Util.Call(_216);
            Util.Call(_237);
        }

        //--------------------------------------------------
        // 593	문자열1 - 자가진단1
        //
        // 33부터 127 까지의 숫자를 계속 입력받아 입력받은 숫자의 아스키코드에 해당하는 문자를 출력하다가 범위를 벗어나는 입력이 들어오면 종료하는 프로그램을 작성하시오.
        //
        // 입출력 예 :
        // ASCII code =? 66
        // B
        // ASCII code =? 122
        // z
        // ASCII code =? 0
        //
        //--------------------------------------------------
        static void _593()
        {
            //while (true) {
            //
            //    Console.Write("ASCII code =? ");
            //    string input = Console.ReadLine();
            //    int n = Convert.ToInt32(input);
            //
            //    if (n < 33 || n > 127)
            //        return;
            //
            //    Console.WriteLine((char)n);
            //}

            string input = "66 122 0";
            string[] args = input.Split();

            for (int i = 0; i < args.Length; ++i) {

                Console.Write("ASCII code =? ");
                Console.WriteLine(args[i]);

                int n = Convert.ToInt32(args[i]);
                if (n < 33 || n > 127)
                    return;

                Console.WriteLine((char)n);
            }
        }



        //--------------------------------------------------
        // 594	문자열1 - 자가진단2
        //
        // 문자열을 입력받은 뒤 그 문자열을 이어서 두 번 출력하는 프로그램을 작성하시오. 
        // 문자열의 길이는 100이하이다.
        //
        // 입력 예 :
        // ASDFG
        //
        // 출력 예 :
        // ASDFGASDFG
        //
        //--------------------------------------------------
        static void _594()
        {
            string input = "ASDFG";
            Console.WriteLine("{0}{0}", input);
        }



        //--------------------------------------------------
        // 595	문자열1 - 자가진단3
        //
        // 문자열을 “Hong Gil Dong”으로 초기화 한 후 3번부터 6번까지의 문자를 차례로 출력하시오.
        //
        // 출력 예 :
        // g Gi
        //
        //--------------------------------------------------
        static void _595()
        {
            string a = "Hong Gil Dong";
            Console.WriteLine(a.Substring(3, 4));
        }



        //--------------------------------------------------
        // 596	문자열1 - 자가진단4
        //
        // 문자열을 입력받고 정수를 입력 받아서 문자열의 맨 뒤부터 정수만큼 출력하는 프로그램을 작성하시오.
        //  만약 입력받은 정수가 문자열의 길이보다 크다면 맨 뒤부터 맨 처음까지 모두 출력한다.
        // (문자열 길이는 최대 100자 이하이다. )
        //
        // 입력 예 :
        // korea 3
        //
        // 출력 예 :
        // aer
        //
        //--------------------------------------------------
        static void _596()
        {
            string input = "korea 3";
            string[] args = input.Split();

            int n = Convert.ToInt32(args[1]);
            string s = args[0];

            if (n > s.Length)
                n = s.Length;

            for (int i = 1; i <= n; ++i)
                Console.Write(s[s.Length - i]);
        }



        //--------------------------------------------------
        // 597	문자열1 - 자가진단5
        //
        // 두 개의 문자열을 입력받아서 두 문자열의 길이의 합을 출력하는 프로그램을 작성하시오.
        // 각 문자열의 길이는 20자 미만이다.
        //
        // 입력 예 :
        // Korean
        // English
        //
        // 출력 예 :
        // 13
        //
        //--------------------------------------------------
        static void _597()
        {
            string s1 = "Korean";
            string s2 = "English";

            Console.WriteLine(s1.Length + s2.Length);
		}



		//--------------------------------------------------
		// 598	문자열1 - 자가진단6
		//
		// 문자를 입력받아 알파벳 문자인 경우에는 그대로 출력하고 숫자인 경우는 아스키코드값을 출력하는 작업을 반복하다가 기타의 문자가 입력되면 종료하는 프로그램을 작성하시오.

		// * 입출력예에서 진한글씨가 출력

		//
		// 입출력 예 :
		// A
		// A
		// 1
		// 49
		// @
		//
		//--------------------------------------------------
		static void _598()
		{
			//string input;
			//
			//while (true)
			//{
			//	input = Console.ReadLine();
			//
			//	if (char.IsNumber(input, 0))
			//	{
			//		//byte[] code = System.Text.ASCIIEncoding.ASCII.GetBytes(input.ToCharArray());
			//		//Console.WriteLine(code[0]);
			//		
			//		Console.WriteLine((int)input[i]);
			//	}
			//	else if (char.IsLetter(input, 0))
			//	{
			//		Console.WriteLine(input[0]);
			//	}
			//	else
			//	{
			//		break;
			//	}
			//}

			string input = "A1@";
			char[] chars = input.ToCharArray();
			for (int i = 0; i < chars.Length; ++i)
			{
				char ch = chars[i];
				if (char.IsNumber(ch))
				{
					//byte[] code = System.Text.ASCIIEncoding.ASCII.GetBytes(chars, i, 1);
					//Console.WriteLine(code[0]);

					Console.WriteLine((int)chars[i]);
				}
				else if (char.IsLetter(ch))
				{
					Console.WriteLine(ch);
				}
				else
				{
					break;
				}
			}
		}



		//--------------------------------------------------
		// 599	문자열1 - 자가진단7
		// 
		// 문자열을 입력받아 알파벳 문자만 모두 대문자로 출력하는 프로그램을 작성하시오. 
		// 문자열의 길이는 100이하이다.
		//
		// 입력 예 :
		// 1988-Seoul-Olympic!!!
		//
		// 출력 예 :
		// SEOULOLYMPIC
		//
		//--------------------------------------------------
		static void _599()
        {
			string input = "1988-Seoul-Olympic!!!";
			char[] chars = input.ToCharArray();
			
			foreach (char ch in chars)
			{
				if (ch >= 'A' && ch <= 'Z')
					Console.Write(ch);
				else if (ch >= 'a' && ch <= 'z')
					Console.Write(char.ToUpper(ch));
			}
		}



		//--------------------------------------------------
		// 600	문자열1 - 자가진단8
		//
		// 공백을 포함한 100글자 이하의 문자열을 입력받아 문장을 이루는 단어의 개수를 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// My name is Kimchulsoo
		//
		// 출력 예 :
		// 4
		//
		//--------------------------------------------------
		static void _600()
        {
			string input = "My name is Kimchulsoo";
			Console.WriteLine(input.Split().Length);
		}



		//--------------------------------------------------
		// 601	문자열1 - 자가진단9
		//
		// 문자열을 입력 받아서 문자수만큼 오른쪽으로 한 바퀴 회전하여 출력하는 프로그램을 작성하시오. 문자열의 길이는 100이하이다.
		//
		// 입력 예 :
		// PROGRAM
		//
		// 출력 예 :
		// MPROGRA
		// AMPROGR
		// RAMPROG
		// GRAMPRO
		// OGRAMPR
		// ROGRAMP
		// PROGRAM
		//
		//--------------------------------------------------
		static void _601()
        {
			string input = "PROGRAM";

			for (int i = 0; i < input.Length; ++i)
			{
				int start = input.Length - 1 - i;
				int len1 = input.Length - start;
				Console.Write(input.Substring(start, len1));
				Console.Write(input.Substring(0, start));
				Console.WriteLine();
			}
		}



		//--------------------------------------------------
		// 182	문자열1 - 형성평가1
		//
		// 영문자 두 개를 입력 받아서 각각의 아스키코드의 합과 차를 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// A a
		//
		// 출력 예 :
		// 162 32
		//
		//--------------------------------------------------
		static void _182()
        {
			string input = "A a";
			string[] args = input.Split();

			int n1 = (int)args[0][0];
			int n2 = (int)args[1][0];
			Console.WriteLine("{0} {1}", n1 + n2, Math.Abs(n1 - n2));
		}



		//--------------------------------------------------
		// 183	문자열1 - 형성평가2
		//
		// 5개 이상 100개 이하의 문자로 된 단어를 입력받은 후 앞에서부터 5자를 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// AbCdEFG
		//
		// 출력 예 :
		// AbCdE
		//
		//--------------------------------------------------
		static void _183()
        {
			string input = "AbCdEFG";
			for (int i = 0; i < 5; ++i)
				Console.Write(input[i]);
        }



		//--------------------------------------------------
		// 184	문자열1 - 형성평가3
		//
		// 100개 이하의 문자열을 입력받아서 영문자와 숫자만 출력하되 영문자는 모두 소문자로 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// Hancom1234@cqclass.com
		//
		// 출력 예 :
		// hancom1234cqclasscom
		//
		//--------------------------------------------------
		static void _184()
        {
			string input = "Hancom1234@cqclass.com";
			char[] chs = input.ToCharArray();

			foreach (char ch in chs)
			{
				if (char.IsLetterOrDigit(ch))
					Console.Write(char.ToLower(ch));
			}
        }



		//--------------------------------------------------
		// 185	문자열1 - 형성평가4
		// 단어와 문자 한 개를 입력받아서 단어에서 입력받은 문자와 같은 문자를 찾아서 그 위치를 출력하는 프로그램을 작성하시오.
		// 단어에서 첫 번째 문자의 위치는 0으로 하고 찾는 문자가 여러 개일 때는 처음 나오는 위치를 출력한다.
		// 만약 찾는 문자가 없을 때는 "No"라고 출력한다.대소문자는 구별되며 단어는 100자 이하이다.
		//
		//
		// 입력 예 :
		// Jungol.co.kr o
		//
		// 출력 예 :
		// 4
		//
		//--------------------------------------------------
		static void _185()
        {
			string input = "Jungol.co.kr o";
			string[] args = input.Split();

			string str = args[0];
			char ch = args[1][0];

			int index = str.IndexOf(ch);
			if (index == -1)
				Console.WriteLine("No");
			else
				Console.WriteLine(index);
		}



		//--------------------------------------------------
		// 186	문자열1 - 형성평가5
		//
		// 두 개의 단어를 입력받아서 길이가 긴 단어의 문자 개수를 출력하는 프로그램을 작성하시오. 
		// 단어의 길이는 100자 이하다.
		//
		// 입력 예 :
		// excel powerpoint
		//
		// 출력 예 :
		// 10
		//
		//--------------------------------------------------
		static void _186()
        {
			string input = "excel powerpoint";
			string[] words = input.Split();

			int len1 = words[0].Length;
			int len2 = words[1].Length;
			Console.WriteLine(len1 > len2 ? len1 : len2);
		}



		//--------------------------------------------------
		// 187	문자열1 - 형성평가6
		//
		// 문자열(100자 이하)을 입력받은 후 정수를 입력받아 해당위치의 문자를 제거하고 출력하는 작업을 반복하다가 문자 1개가 남으면 종료하는 프로그램을 작성하시오.
		// 첫 번째 문자의 위치는 1이며 만약 입력받은 번호가 문자열의 길이 이상이면 마지막 문자를 제거한다.
		//  
		// * 입출력예에서 진한글씨가 출력
		//
		// 입출력 예 :
		// word
		// 3
		// wod
		// 1
		// od
		// 10
		// o
		//
		//--------------------------------------------------
		static void _187()
        {
			//string input = Console.ReadLine();
			//while (input.Length > 1)
			//{
			//	string input2 = Console.ReadLine();
			//	int n = Convert.ToInt32(input2);
			//	if (n > input.Length)
			//		input = input.Remove(input.Length - 1);
			//	else
			//		input = input.Remove(n - 1, 1);
			//
			//	Console.WriteLine(input);
			//}

			string input = "Word 3 1 10";
			string[] args = input.Split();
			string word = args[0];
			Console.WriteLine(word);

			for (int i = 1; i < args.Length; ++i)
			{
				int n = Convert.ToInt32(args[i]);
				Console.WriteLine(n);

				if (n > word.Length)
					word = word.Remove(word.Length - 1);
				else
					word = word.Remove(n - 1, 1);
			
				Console.WriteLine(word);
				if (word.Length == 1)
					break;
			}
		}



		//--------------------------------------------------
		// 188	문자열1 - 형성평가7
		//
		// 공백을 포함한 문자열을 입력받아 다음과 같이 분리하여 번호와 함께 출력하는 프로그램을 작성하시오. 
		// 문자열의 길이는 100자 이하이다.
		//
		// 입력 예 :
		// My name is Kimchulsoo
		//
		// 출력 예 :
		// 1. My
		// 2. name
		// 3. is
		// 4. Kimchulsoo
		//
		//--------------------------------------------------
		static void _188()
        {
			string input = "My name is Kimchulsoo";
			string[] words = input.Split();

			for (int i = 0; i < words.Length; ++i)
				Console.WriteLine("{0}. {1}", i + 1, words[i]);
		}



		//--------------------------------------------------
		// 602	문자열2 - 자가진단1
		// 5개의 단어를 입력받아 모든 단어를 입력받은 역순으로 출력하는 프로그램을 작성하시오. 
		// 각 단어의 길이는 30이하이다.
		//
		//
		// 입력 예 :
		// dog
		// cat
		// chick
		// calf
		// goat
		//
		// 출력 예 :
		// goat
		// calf
		// chick
		// cat
		// dog
		//
		//--------------------------------------------------
		static void _602()
        {
			//string[] inputs = new string[5];
			//for (int i = 0; i < 5; ++i)
			//{
			//	inputs[i] = Console.ReadLine();
			//}
			//
			//Console.WriteLine();
			//for (int i = inputs.Length - 1; i >= 0; --i)
			//	Console.WriteLine(inputs[i]);

			string[] inputs = {
				"dog",
				"cat",
				"chick",
				"calf",
				"goat",
			};

			for (int i = inputs.Length - 1; i >= 0; --i)
				Console.WriteLine(inputs[i]);
		}



		//--------------------------------------------------
		// 603	문자열2 - 자가진단2
		//
		// 공백을 포함한 문장을 입력 받아서 홀수 번째 단어를 차례로 출력하는 프로그램을 작성하시오.
		// 문장의 길이는 100자 이하이다.
		//
		// 입력 예 :
		// I like you better than him.
		//
		// 출력 예 :
		// I
		// you
		// than
		//
		//--------------------------------------------------
		static void _603()
        {
			string input = "I like you better than him.";
			string[] words = input.Split();

			for (int i = 0; i < words.Length; i += 2)
				Console.WriteLine(words[i]);
        }



		//--------------------------------------------------
		// 604	문자열2 - 자가진단3
		//
		// 20개 이하의 문자로 이루어진 10개의 단어와 한 개의 문자를 입력받아서 마지막으로 입력받은 문자로 끝나는 단어를 모두 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// champion
		// tel
		// pencil
		// olympiad
		// class
		// information
		// jungol
		// lesson
		// book
		// lion
		// l
		//
		// 출력 예 :
		// tel
		// pencil
		// jungol
		//
		//--------------------------------------------------
		static void _604()
        {
			string[] inputs = {
				"champion",
				"tel",
				"pencil",
				"olympiad",
				"class",
				"information",
				"jungol",
				"lesson",
				"book",
				"lion",
				"l"
			};
			
			for (int i = 0; i < 10; ++i)
			{
				if (inputs[i].EndsWith(inputs[10]))
					Console.WriteLine(inputs[i]);
			}
        }



		//--------------------------------------------------
		// 605	문자열2 - 자가진단4
		//
		// 문자열을 선언하고 다음과 같이 "Hong Gil Dong"이라는 이름을 복사하여 저장한 후 출력하는 프로그램을 작성하시오.
		//
		// 출력 예 :
		// Hong Gil Dong
		//
		//--------------------------------------------------
		static void _605()
        {
			string s = "Hong Gil Dong";
			Console.WriteLine(s);
		}



		//--------------------------------------------------
		// 606	문자열2 - 자가진단5
		//
		// 20개 이하의 문자열로 된 이름을 입력받아서 그 뒤에 "fighting"을 붙여서 저장하고 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// Korea
		//
		// 출력 예 :
		// Koreafigting
		//
		//--------------------------------------------------
		static void _606()
        {
			string input = "Korea";
			input += "figting";
			Console.WriteLine(input);
        }



		//--------------------------------------------------
		// 607	문자열2 - 자가진단6
		//
		// 두 개의 문자열을 입력받아 첫 번째 문자열의 앞부분 두자를 두 번째 문자열의 앞부분에 복사하고 다시 뒷부분에 이어 붙여서 저장한 후 두 번째 문자열을 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// ABCDE QWERTY
		//
		// 출력 예 :
		// ABERTYAB
		//
		//--------------------------------------------------
		static void _607()
        {
			string input = "ABCDE QWERTY";
			string[] args = input.Split();

			System.Text.StringBuilder sb = new System.Text.StringBuilder(args[1]);

			sb[0] = args[0][0];
			sb[1] = args[0][1];

			sb.Append(args[0][0]);
			sb.Append(args[0][1]);
			
			Console.WriteLine(sb.ToString());
		}



		//--------------------------------------------------
		// 608	문자열2 - 자가진단7
		//
		// 100개 이하의 문자로 구성된 한 개의 문자열을 입력받아서 그 문자열에 문자 'c'와 문자열 "ab"의 포함여부를 "Yes", "No"로 구분하여 출력예처럼 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// abdef
		//
		// 출력 예 :
		// No Yes
		//
		//--------------------------------------------------
		static void _608()
		{
			string input = "abdef";

			int index_c = input.IndexOf('c');
			int index_ab = input.IndexOf("ab");
			Console.WriteLine("{0} {1}", index_c == -1 ? "No" : "Yes", index_ab == -1 ? "No" : "Yes");
		}



		//--------------------------------------------------
		// 609	문자열2 - 자가진단8
		//
		// 세 개의 단어를 입력받아 사전 순으로 가장 먼저 나오는 단어를 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// cat dog cow
		//
		// 출력 예 :
		// cat
		//
		//--------------------------------------------------
		static void _609()
		{
			string input = "cat dog cow";
			string[] words = input.Split();

			string min = words[0];
			for (int i = 1; i < words.Length; ++i)
				if (min.CompareTo(words[i]) > 0)
					min = words[i];

			Console.WriteLine(min);
		}



		//--------------------------------------------------
		// 610	문자열2 - 자가진단9
		//
		// 5개의 문자열을 입력받아 문자열 크기(아스키코드) 역순으로 정렬하여 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// Jungol
		// Korea
		// information
		// Monitor
		// class
		//
		// 출력 예 :
		// information
		// class
		// Monitor
		// Korea
		// Jungol
		//
		//--------------------------------------------------
		static void _610()
		{
			string[] inputs = {
				"Jungol",
				"Korea",
				"information",
				"Monitor",
				"class",
			};

			Console.WriteLine((int)'A');    //65
			Console.WriteLine((int)'a');    //97
			Console.WriteLine();
			Array.Sort<string>(inputs, (a, b) =>
			{
				byte[] ascii_a = System.Text.ASCIIEncoding.ASCII.GetBytes(a);
				byte[] ascii_b = System.Text.ASCIIEncoding.ASCII.GetBytes(b);

				int LEN = ascii_a.Length < ascii_b.Length ? ascii_a.Length : ascii_b.Length;
				for (int i = 0; i < LEN; ++i)
				{
					if (ascii_a[i] != ascii_b[i])
						return ascii_b[i] - ascii_a[i];
				}

				return ascii_b.Length - ascii_a.Length;
			});
			for (int i = 0; i < inputs.Length; ++i)
			{
				Console.WriteLine(inputs[i]);
			}
		}



		//--------------------------------------------------
		// 611	문자열2 - 자가진단A
		//
		// 한 개의 문자열을 입력받아서 첫 줄에는 정수로 변환하여 2배한 값을 출력하고, 다음 줄에는 실수로 변환한 값을 반올림하여 소수 둘째자리까지 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// 50.1034	///< 50.1*34 는 오타로 보임.
		//
		// 출력 예 :
		// 100
		// 50.10
		//
		//--------------------------------------------------
		static void _611()
		{
			string input = "50.1034";
			double d = Convert.ToDouble(input);
			Console.WriteLine(((int)d) * 2);
			Console.WriteLine("{0:f2}", d);
		}



		//--------------------------------------------------
		// 612	문자열2 - 자가진단B
		//
		// 5개의 정수를 입력받아 모두 붙여서 문자열로 저장한 후 세 자씩 나누어서 출력하는 프로그램을 작성하시오.
		//
		// 입력 예 :
		// 12 5963 58 1 45678
		//
		// 출력 예 :
		// 125
		// 963
		// 581
		// 456
		// 78
		//
		//--------------------------------------------------
		static void _612()
		{
			string input = "12 5963 58 1 45678";
			string[] args = input.Split();

			int[] integers = new int[args.Length];
			for (int i = 0; i < integers.Length; ++i)
				integers[i] = Convert.ToInt32(args[i]);

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < integers.Length; ++i)
				sb.Append(integers[i].ToString());

			for (int i = 0; i < sb.Length; i += 3)
			{
				int TO = Math.Min(i + 3, sb.Length);
				for (int j = i; j < TO; ++j)
					Console.Write(sb[j]);
				Console.WriteLine();
			}
		}



		//--------------------------------------------------
		// 189	문자열2 - 형성평가1
		//
		// 공백을 포함한 문자열을 입력받아 각 단어로 분리하여 문자열 배열에 저장한 후 입력순서의 반대 순서로 출력하는 프로그램을 작성하시오. 
		// 문자열의 길이는 100자 이하이다.
		//
		// 입력 예 :
		// C++ Programing jjang!!
		//
		// 출력 예 :
		// jjang!!
		// Programing
		// C++
		//
		//--------------------------------------------------
		static void _189()
		{
			string input = "C++ Programing jjang!!";
			string[] words = input.Split();

			for (int i = words.Length - 1; i >= 0; --i)
				Console.WriteLine(words[i]);
		}



		//--------------------------------------------------
		// 190	문자열2 - 형성평가2
		//
		// "flower" "rose" "lily" "daffodil" "azalea" 5개의 단어를 초기화한 후 한 개의 문자를 입력받아서 입력받은 문자가 두 번째나 세 번째에 포함된 단어를 모두 출력하고 마지막 줄에 출력한 단어의 개수를 출력하는 프로그램을 작성하시오.
		// 해당되는 단어가 없으면 "0"만 첫 줄에 출력한다.
		//
		// 입력 예 :
		// l
		//
		// 출력 예 :
		// flower
		// lily
		// 2
		//
		//--------------------------------------------------
		static void _190()
		{
			string[] inputs = {
				"flower",
				"rose",
				"lily",
				"daffodil",
				"azalea",
			};

			char c = 'l';

			int cnt = 0;
			for (int i = 0; i < inputs.Length; ++i)
			{
				if (inputs[i][1] == c || inputs[i][2] == c)
				{
					Console.WriteLine(inputs[i]);
					++cnt;
				}
			}
			Console.WriteLine(cnt);
		}



		//--------------------------------------------------
		// 191	문자열2 - 형성평가3
		//
		// 단어를 입력받다가 "0"을 입력받으면 입력을 종료하고 그 때까지 입력받은 단어의 개수를 출력하고 홀수 번째 입력받은 단어를 한 줄에 1개씩 출력하는 프로그램을 작성하시오. 
		// 단어의 개수는 50개를 넘지 않고, 단어의 길이는 100자 이하이다.
		//
		// 입력 예 :
		// keyboard
		// mouse
		// monitor
		// 0
		//
		// 출력 예 :
		// 3
		// keyboard
		// monitor
		//
		//--------------------------------------------------
		static void _191()
        {
			//string input;
			//int cnt = 0;
			//
			//string[] words = new string[50];
			//for (int i = 0; i < words.Length; ++i)
			//{
			//	input = Console.ReadLine();
			//	if ("0" == input)
			//		break;
			//
			//	words[cnt++] = input;
			//}
			//Console.WriteLine(cnt);
			//for (int i = 0; i < cnt; i += 2)
			//	Console.WriteLine(words[i]);
			
			int cnt = 0;

			string[] words = {
				"keyboard",
				"mouse",
				"monitor",
				"0",
			};

			Console.WriteLine(cnt);
			for (int i = 0; i < cnt; i += 2)
				Console.WriteLine(words[i]);
		}



		//--------------------------------------------------
		// 192	문자열2 - 형성평가4
		//
		// 10 이하의 정수 n을 입력받고 n개의 문자열을 입력받은 후 그 크기를 비교하여 가장 작은 문자열부터 차례로 출력하는 프로그램을 작성하시오. 
		// 문자열의 길이는 100자 이하다.사전순(아스키코드순)으로 뒤에 나오는 것을 큰 것으로 한다.
		//
		// 입력 예 :
		// 3
		// notebook
		// pencil
		// eraser
		//
		// 출력 예 :
		// eraser
		// notebook
		// pencil
		//
		//--------------------------------------------------
		static void _192()
        {
			//string input = Console.ReadLine();
			//int n = Convert.ToInt32(input);
			//
			//string[] inputs = new string[n];
			//for (int i = 0; i < n; ++i)
			//	inputs[i] = Console.ReadLine();
			//
			//Console.WriteLine();
			//Array.Sort(inputs);
			//for (int i = 0; i < n; ++i)
			//	Console.WriteLine(inputs[i]);

			
			string[] inputs = {
				"notebook",
				"pencil",
				"erazer"
			};
			
			Array.Sort(inputs);
			for (int i = 0; i < inputs.Length; ++i)
				Console.WriteLine(inputs[i]);

		}



		//--------------------------------------------------
		// 193	문자열2 - 형성평가5
		//
		// 5개의 단어(각 단어는 100자 이하)를 입력받은 후 문자와 문자열(100자 이하)을 한 개씩 입력받아 나중에 입력받은 문자나 문자열이 포함된 단어를 모두 출력하는 프로그램을 작성하시오. 찾는 단어가 없으면 “none”이라고 출력한다.
		//
		// 입력 예 :
		// banana
		// apple
		// melon
		// tomato
		// pear
		// n
		// to
		//
		// 출력 예 :
		// banana
		// melon
		// tomato
		//
		//--------------------------------------------------
		static void _193()
        {
			string[] inputs = {
				"banana",
				"apple",
				"melon",
				"tomato",
				"pear",
			};
			char c = 'n';
			string s = "to";

			int cnt = 0;
			for (int i = 0; i < inputs.Length; ++i)
			{
				if (inputs[i].IndexOf(c) != -1 || inputs[i].IndexOf(s) != -1)
				{
					++cnt;
					Console.WriteLine(inputs[i]);
				}
			}
			if (cnt == 0)
				Console.WriteLine("none");
        }



		//--------------------------------------------------
		// 194	문자열2 - 형성평가6
		//
		// 두 개의 문자열 A와 B 한 개의 정수 n을 입력받아서 A에 B를 연결하고, 변경된 문자열 A에서 n개의 문자를 B에 복사한 후 A와 B를 출력하는 프로그램을 작성하시오. (1≤n,A,B≤100)
		//
		// 입력 예 :
		// banana apple 3
		//
		// 출력 예 :
		// bananaapple
		// banle
		//
		//--------------------------------------------------
		static void _194()
        {
			string input = "banana apple 3";
			string[] inputs = input.Split();

			string A = inputs[0];
			string B = inputs[1];
			int n = Convert.ToInt32(inputs[2]);

			Console.WriteLine(A + B);

			System.Text.StringBuilder sb = new System.Text.StringBuilder(B);
			for (int i = 0; i < n; ++i)
			{
				sb[i] = A[i];
			}
			Console.WriteLine(sb.ToString());
		}



		//--------------------------------------------------
		// 215	문자열2 - 형성평가7
		//
		// 두 개의 문자열을 입력받아 앞에서부터 정수로 변환 가능한 부분을 변환한 후 두 수의 곱을 출력하는 프로그램을 작성하시오. 
		// 각 문자열의 길이는 100이하이다.
		//
		// 입력 예 :
		// 123.45  67@12 
		//
		// 출력 예 :
		// 8241
		//
		//--------------------------------------------------
		static void _215()
        {
			string input = "123.45 67@12";
			string[] words = input.Split();

			int n1 = 0;
			string s = words[0];
			for (int i = 0; i < s.Length; ++i)
			{
				if (false == char.IsDigit(s[i]))
				{
					n1 = Convert.ToInt32(s.Substring(0, i));
					break;
				}
			}
			int n2 = 0;
			s = words[1];
			for (int i = 0; i < s.Length; ++i)
			{
				if (false == char.IsDigit(s[i]))
				{
					n2 = Convert.ToInt32(s.Substring(0, i));
					break;
				}
			}

			Console.WriteLine(n1 * n2);
		}



		//--------------------------------------------------
		// 216	문자열2 - 형성평가8
		//
		// 한 개의 단어를 입력받아서 거꾸로 뒤집어 출력하는 작업을 반복하다가 "END"라고 입력이 되면 종료하는 프로그램을 작성하시오. 
		// 입력받는 단어의 길이는 20이하이다.
		//
		// 입력 예 :
		// Jungol
		// jjang
		// END
		//
		// 출력 예 :
		// lognuJ
		// gnajj
		//
		//--------------------------------------------------
		static void _216()
        {
			string[] inputs = {
				"Jungol",
				"jjang",
				"END",
			};

			foreach (string s in inputs)
			{
				if ("END" == s)
					break;

				for (int i = s.Length - 1; i >= 0; --i)
					Console.Write(s[i]);
				Console.WriteLine();
			}
        }



		//--------------------------------------------------
		// 237	문자열2 - 형성평가9
		//
		// 정수, 실수, 문자열을 차례로 입력받아서 새로운 문자열에 출력한 후 전체의 길이를 2등분하여 출력하는 프로그램을 작성하시오. 실수는 반올림하여 소수 셋째자리까지 출력하는 것으로 하고, 새로운 문자열의 길이가 홀수일 때는 첫 번째 줄에 한 개를 더 출력한다. 각 문자열의 길이는 30자 이내이다.
		//
		// 입력 예 :
		// 12345 5.0123 fighting
		//
		// 출력 예 :
		// 123455.01
		// 2fighting
		// 
		//--------------------------------------------------
		static void _237()
        {
			string input = "12345 5.0123 fighting";
			string[] inputs = input.Split();

			double d = Convert.ToDouble(inputs[1]);
			inputs[1] = d.ToString("f3");

			string o = inputs[0] + inputs[1] + inputs[2];
			int n = o.Length / 2;
			if (n * 2 != o.Length)
				n += 1;

			Console.WriteLine(o.Substring(0, n));
			Console.WriteLine(o.Substring(n));

		}


    }
}
