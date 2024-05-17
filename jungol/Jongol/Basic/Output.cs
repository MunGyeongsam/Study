using System;

namespace Jungol
{
	// 501	출력 - 자가진단1	
	// 502	출력 - 자가진단2	
	// 503	출력 - 자가진단3	
	// 504	출력 - 자가진단4	
	// 505	출력 - 자가진단5	
	// 506	출력 - 자가진단6	
	// 507	출력 - 자가진단7	
	// 508	출력 - 자가진단8	
	// 101	출력 - 형성평가1	
	// 102	출력 - 형성평가2	
	// 103	출력 - 형성평가3	
	// 104	출력 - 형성평가4	
	// 105	출력 - 형성평가5	

	static class Output
	{
		
		public static void Test()
		{
			Util.Call(_501);
			Util.Call(_502);
			Util.Call(_503);
			Util.Call(_504);
			Util.Call(_505);
			Util.Call(_506);
			Util.Call(_507);
			Util.Call(_508);
			Util.Call(_101);
			Util.Call(_102);
			Util.Call(_103);
			Util.Call(_104);
			Util.Call(_105);
		}


		// 501	출력 - 자가진단1	
		static void _501()
		{
			Console.WriteLine("Fun Programming!");
		}

		// 502	출력 - 자가진단2	
		static void _502()
		{
			Console.WriteLine("Programming! It's fun.");
		}

		// 503	출력 - 자가진단3	
		static void _503()
		{
			Console.WriteLine("My name is Hong Gil Dong.");
			Console.WriteLine("I am 13 years old.");
		}

		// 504	출력 - 자가진단4	
		static void _504()
		{
			Console.WriteLine("(@) (@)");
			Console.WriteLine("(=^.^=)");
			Console.WriteLine("(-m-m-)");
			
			//Console.WriteLine(@"
			//(@) (@)
			//(=^.^=)
			//(-m-m-)
			//");
		}

		// 505	출력 - 자가진단5	
		static void _505()
		{
            //I can program well.
            //Dreams come true.
            Console.WriteLine("I can program well.");
            Console.WriteLine("Dreams come true.");
        }

		// 506	출력 - 자가진단6	
		static void _506()
		{
            //My height
            //170
            //My weight
            //68.600000


            Console.WriteLine("My height");
            Console.WriteLine(170);
            Console.WriteLine("My weight");
            Console.WriteLine("{0:f6}",68.6f);

        }

		// 507	출력 - 자가진단7	
		static void _507()
		{
            //다음과 같이 출력되는 프로그램을 작성하라.(공백으로 구분하여 출력)

            //5 Dan
            //5 * 2 = 10

            Console.WriteLine("{0} Dan", 5);
            Console.WriteLine("{0} * {1} = {2}", 5, 2, 5*2);
        }

		// 508	출력 - 자가진단8	
		static void _508()
		{
            //다음과 같이 출력되는 프로그램을 작성하라.
            //(각 요소들은 10칸씩 공간을 확보하여 오른쪽으로 정렬하여 출력한다.)

            //      item     count     price
            //       pen        20       100
            //      note         5        95
            //    eraser       110        97
            Console.WriteLine("{0,10}{1,10}{2,10}", "item", "count", "price");
            Console.WriteLine("{0,10}{1,10}{2,10}", "pen", 20, 100);
            Console.WriteLine("{0,10}{1,10}{2,10}", "note", 5, 95);
            Console.WriteLine("{0,10}{1,10}{2,10}", "eraser", 110, 97);
        }

		// 101	출력 - 형성평가1	
		static void _101()
		{
            //My name is Hong
            Console.WriteLine("My name is Hong");

        }

		// 102	출력 - 형성평가2	
		static void _102()
		{
            //My hometown
            //Flowering mountain
            Console.WriteLine("My hometown");
            Console.WriteLine("Flowering mountain");
        }

		// 103	출력 - 형성평가3	
		static void _103()
		{
            //TTTTTTTTTT
            //TTTTTTTTTT
            //    TT
            //    TT
            //    TT
            Console.WriteLine("{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}", 'T');
            Console.WriteLine("{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}", 'T');
            Console.WriteLine("{0,5}{0}", 'T');
            Console.WriteLine("{0,5}{0}", 'T');
            Console.WriteLine("{0,5}{0}", 'T');

        }

		// 104	출력 - 형성평가4	
		static void _104()
		{
            //다음 출력 예와 같이 출력되는 프로그램을 작성하시오.
            //합계와 평균은 수식을 이용하세요.

            //kor 90
            //mat 80
            //eng 100
            //sum 270
            //avg 90
            int sum = 90 + 80 + 100;
            int avg = sum / 3;
            Console.WriteLine("kor {0}", 90);
            Console.WriteLine("mat {0}", 80);
            Console.WriteLine("eng {0}", 100);
            Console.WriteLine("sum {0}", sum);
            Console.WriteLine("avg {0}", avg);
        }

		// 105	출력 - 형성평가5	
		static void _105()
		{
            //다음 출력 예와 같이 모든 단어를 15칸씩 오른쪽에 맞추어 출력되는 프로그램을 작성하시오.

            //          Seoul     10,312,545        +91,375
            //          Pusan      3,567,910         +5,868
            //        Incheon      2,758,296        +64,888
            //          Daegu      2,511,676        +17,230
            //        Gwangju      1,454,636        +29,774

            Console.WriteLine("{0,15}{1,15:#,#}{2,15:+#,#}", "  Seoul", 10312545, 91375);
            Console.WriteLine("{0,15}{1,15:#,#}{2,15:+#,#}", "  Pusan", 3567910, 5868);
            Console.WriteLine("{0,15}{1,15:#,#}{2,15:+#,#}", "Incheon", 2758296, 64888);
            Console.WriteLine("{0,15}{1,15:#,#}{2,15:+#,#}", "  Daegu", 2511676, 17230);
            Console.WriteLine("{0,15}{1,15:#,#}{2,15:+#,#}", "Gwangju", 1454636, 29774);

        }

    }
}
