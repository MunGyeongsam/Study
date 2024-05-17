using System;

namespace Jungol
{
    // 626	파일입출력 - 자가진단1
    // 627	파일입출력 - 자가진단2
    // 628	파일입출력 - 자가진단3
    // 629	파일입출력 - 자가진단4
    // 630	파일입출력 - 자가진단5
    // 631	파일입출력 - 자가진단6
    // 205	파일입출력 - 형성평가1
    // 206	파일입출력 - 형성평가2
    // 207	파일입출력 - 형성평가3
    // 208	파일입출력 - 형성평가4
    // 209	파일입출력 - 형성평가5
    // 210	파일입출력 - 형성평가6
    // 211	파일입출력 - 형성평가7
    // 212	파일입출력 - 형성평가8
    // 213	파일입출력 - 형성평가9
    // 214	파일입출력 - 형성평가A
    static class FileIO
	{

        const string DIR = "../../Basic/text/";
		public static void Test()
        {
            Util.Call(_626);
            Util.Call(_627);
            Util.Call(_628);
            Util.Call(_629);
            Util.Call(_630);
            Util.Call(_631);
            Util.Call(_205);
            Util.Call(_206);
            Util.Call(_207);
            Util.Call(_208);
            Util.Call(_209);
            Util.Call(_210);
            Util.Call(_211);
            Util.Call(_212);
            Util.Call(_213);
            Util.Call(_214);
        }

        //--------------------------------------------------
        // 626	파일입출력 - 자가진단1
        // 
        // 두 개의 정수를 입력받아 작은 수부터 큰 수까지 모든 정수의 합을 구하여 출력하는 프로그램을 작성하시오.
        // * 표준입출력방식으로 작성하세요.
        //
        // 입력 예 :
        // 5 10
        //
        // 출력 예 :
        // 45
        //
        //--------------------------------------------------
        static void _626()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "626 input.txt"));
            string line = Console.ReadLine();
            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            char[] sep = { ' ' };
            string[] words = line.Split(sep);

            System.Diagnostics.Debug.Assert(words.Length == 2);

            if (words.Length >= 2) {
                int n1 = Convert.ToInt32(words[0]);
                int n2 = Convert.ToInt32(words[1]);
                int tot = 0;
                for (int i = Math.Min(n1, n2); i <= Math.Max(n1, n2); ++i) {
                    tot += i;
                }

                Console.SetOut(new System.IO.StreamWriter(DIR + "output_626.txt"));

                Console.WriteLine(tot);

                Console.Out.Close();
                Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
            }
        }



        //--------------------------------------------------
        // 627	파일입출력 - 자가진단2
        // 
        // 10개의 실수를 입력받아 첫 번째 입력 값과 마지막 입력 값의 평균을 반올림하여, 소수 첫째자리까지 출력하는 프로그램을 작성하시오.
        //
        // * 표준입출력방식으로 작성하세요.
        //
        // 입력 예 :
        // 15.3 123.5 0.69 85.12 3.0 51.9 100.1 1.58 5.5 10.5
        //
        // 출력 예 :
        // 12.9
        //
        //--------------------------------------------------
        static void _627()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "627 input.txt"));
            string line = Console.ReadLine();
            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            string[] words = line.Split(new char[] { ' ' });
            if (words.Length == 10) {
                double d1 = Convert.ToDouble(words[0]);
                double d2 = Convert.ToDouble(words[words.Length-1]);
                double av = (d1 + d2) / 2d;

                Console.SetOut(new System.IO.StreamWriter(DIR + "output_627.txt"));
                Console.WriteLine("{0:N1}", av);
                Console.Out.Close();

                Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
            }
        }



        //--------------------------------------------------
        // 628	파일입출력 - 자가진단3
        //
        // 10명의 학생 이름과 점수를 입력받아 이름과 점수, 등수를 입력받은 순서대로 출력하는 프로그램을 작성하시오.
        // 단, 출력시 "이름", "점수", "등수"는 한 칸의 공백으로 구분을 하며, Name은 4칸(%4s), Score는 5칸(%5d), Rank는 각 4칸(%4d)으로 출력한다.
        //
        // * 표준입출력방식으로 작성하세요.
        //
        // 입력 예 :
        // Hong 90
        // Lim 85
        // Park 88
        // Gong 75
        // Lee 100
        // Seo 90
        // Jang 75
        // Song 90
        // Kim 95
        // Sung 60
        //
        // 출력 예 :
        //Name Score Rank
        //Hong    90    3
        // Lim    85    7
        //Park    88    6
        //Gong    75    8
        // Lee   100    1
        // Seo    90    3
        //Jang    75    8
        //Song    90    3
        // Kim    95    2
        //Sung    60   10
        //
        //--------------------------------------------------
        static void _628()
        {
            string[] names = new string[10];
            int[] scores = new int[10];
            char[] SEP = new char[] { ' ' };

            Console.SetIn(new System.IO.StreamReader(DIR + "628 input.txt"));
            
            for (int i=0; i<10; ++i) {
                string line = Console.ReadLine();
                if (null == line) {
                    Console.WriteLine("need 10 line");
                    return;
                }

                string[] words = line.Split(SEP);
                names[i] = words[0];
                scores[i] = Convert.ToInt32(words[1]);
            }

            int[] ranks = new int[10];
            for (int i=0; i<10; ++i) {
                int score = scores[i];
                int rank = 0;
                for(int j=0; j<10; ++j) {
                    if (score < scores[j])
                        ++rank;
                }

                ranks[i] = rank + 1; // 1 base
            }
            
            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

             
            Console.SetOut(new System.IO.StreamWriter(DIR + "output_628.txt"));
            Console.WriteLine("{0,4} {1,5} {2,4}", "Name", "Score", "Rank");
            for (int i=0; i<10; ++i) {
                Console.WriteLine("{0,4} {1,5} {2,4}", names[i], scores[i], ranks[i]);
            }
            Console.Out.Close();

            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 629	파일입출력 - 자가진단4
        //
        // 공백을 포함한 문자열을 두 번 입력받아 길이가 작은 문자열과 긴 문자열의 순으로 출력하는 프로그램을 작성하시오.
        //
        // * 표준입출력방식으로 작성하세요.
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _629()
        {

            Console.SetIn(new System.IO.StreamReader(DIR + "629 input.txt"));

            string line1 = Console.ReadLine();
            string line2 = Console.ReadLine();
            
            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            Console.SetOut(new System.IO.StreamWriter(DIR + "output_629.txt"));

            if (line1.Length < line2.Length) {
                Console.WriteLine(line1);
                Console.WriteLine(line2);
            } else {
                Console.WriteLine(line2);
                Console.WriteLine(line1);
            }

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 630	파일입출력 - 자가진단5
        //
        // 100이하의 정수 N을 입력받은 후 공백이 포함된 N행의 문장을 입력받아서 역순으로 출력하는 프로그램을 작성하시오.
        // 각 문장의 길이는 100이하이다.
        //
        // * 표준입출력방식으로 작성하세요.
        //
        // 입력 예 :
        // 3
        // I love korea.
        // My name is chulgi.
        // I'm happy.
        //
        // 출력 예 :
        // I'm happy.
        // My name is chulgi.
        // I love korea.
        //
        //--------------------------------------------------
        static void _630()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "630 input.txt"));

            string line = Console.ReadLine();
            int N = Convert.ToInt32(line);

            string[] lines = new string[N];
            for (int i=0; i<N; ++i) {
                line = Console.ReadLine();
                lines[i] = line;
            }
            
            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));


            Console.SetOut(new System.IO.StreamWriter(DIR + "output_630.txt"));

            for (int i = N - 1; i >= 0; --i)
                Console.WriteLine(lines[i]);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 631	파일입출력 - 자가진단6
        //
        // 원의 둘레의 길이를 입력받아 반지름의 길이를 출력하는 프로그램을 작성하시오.
        // 단, 원주율은 3.14로 하고 출력은 소수 세째자리에서 반올림하여 둘째자리까지 출력 한다.
        // 둘레의 길이가 0 이면 종료한다.
        //
        // * 표준입출력방식으로 작성하세요.
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _631()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "631 input.txt"));
            Console.SetOut(new System.IO.StreamWriter(DIR + "output_631.txt"));

            double PI = 3.14d;
            for(;;) {
                string line = Console.ReadLine();
                double length = Convert.ToDouble(line);
                if (length == 0d)
                    break;

                double radius = length / (2d * PI);
                Console.WriteLine("{0:N2}", radius);                
            }

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));
        }



        //--------------------------------------------------
        // 205	파일입출력 - 형성평가1
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _205()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "205 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));


            string[] words = line.Split(new char[] { });
            System.Diagnostics.Debug.Assert(words.Length == 2);


            double d1 = Convert.ToDouble(words[0]);
            double d2 = Convert.ToDouble(words[1]);
            double sum = d1 + d2;


            Console.SetOut(new System.IO.StreamWriter(DIR + "output_205.txt"));

            Console.WriteLine("{0:N2} {1:N2} {2:N2}", d1, d2, sum);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 206	파일입출력 - 형성평가2
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _206()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "206 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));
            
            string[] words = line.Split(new char[] { });
            System.Diagnostics.Debug.Assert(words.Length == 3);

            int n1 = Convert.ToInt32(words[0]);
            int n2 = Convert.ToInt32(words[1]);
            int n3 = Convert.ToInt32(words[2]);
            int sum = n1 + n2 + n3;
            int quotient = sum / 3;
            int remain = sum % 3;

            Console.SetOut(new System.IO.StreamWriter(DIR + "output_206.txt"));

            Console.WriteLine("{0} {1}...{2}", sum, quotient, remain);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 207	파일입출력 - 형성평가3
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _207()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "207 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            string[] words = line.Split(new char[] { });
            System.Diagnostics.Debug.Assert(words.Length == 3);

            int n1 = Convert.ToInt32(words[0]);
            int n2 = Convert.ToInt32(words[1]);
            char op = Convert.ToChar(words[2]);
            int rt = 0;
            switch(op) {
                case '+': rt = n1 + n2; break;
                case '-': rt = n1 - n2; break;
                case '*': rt = n1 * n2; break;
                case '/': rt = n1 / n2; break;
                case '%': rt = n1 % n2; break;
            }



            Console.SetOut(new System.IO.StreamWriter(DIR + "output_207.txt"));

            Console.WriteLine("{0} {1} {2} = {3}", n1, op, n2, rt);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 208	파일입출력 - 형성평가4
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _208()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "208 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            int n = Convert.ToInt32(line);
            int rt = 1;
            for (int i=n; i>0; --i) {
                rt = rt * i;
            }

            Console.SetOut(new System.IO.StreamWriter(DIR + "output_208.txt"));

            Console.WriteLine(rt);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 209	파일입출력 - 형성평가5
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _209()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "209 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));
            string[] words = line.Split(new char[] {' '});



            Console.SetOut(new System.IO.StreamWriter(DIR + "output_209.txt"));

            int count = 0;
            for (int i=0; i<words.Length; ++i) {
                int n = Convert.ToInt32(words[i]);
                if (n == 0)
                    break;

                if (n % 3 == 0 && n % 5 == 0) {
                    Console.Write(n);
                    Console.Write(' ');
                    ++count;
                }
            }
            Console.WriteLine();
            Console.WriteLine(count);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 210	파일입출력 - 형성평가6
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _210()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "210 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            int n = Convert.ToInt32(line);
            int HALF_WIDTH = n;

            Console.SetOut(new System.IO.StreamWriter(DIR + "output_210.txt"));

            for (int i=1; i<=n; ++i) {

                Console.Write(new string(' ', (HALF_WIDTH - i) * 2));
                Console.Write('*');
                for (int j=1; j<(i*2-1); ++j) {
                    Console.Write(' ');
                    Console.Write('*');
                }
                Console.WriteLine();
            }

            for (int i=n-1; i>=1; --i) {
                Console.Write(new string(' ', (HALF_WIDTH - i) * 2));
                Console.Write('*');
                for (int j = 1; j < (i * 2 - 1); ++j) {
                    Console.Write(' ');
                    Console.Write('*');
                }
                Console.WriteLine();
            }

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 211	파일입출력 - 형성평가7
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        // 현재 전 세계 대부분의 나라에서 쓰는 그레고리력은 4년에 반드시 하루씩 윤날(2월 29일)을 추가하는 율리우스력을 보완한 것으로, 태양년과의 편차를 줄이기 위해 율리우스력의 400년에서 3일(세 번의 윤년)을 뺐다.
        // 
        // 그레고리력의 정확한 윤년 규칙은 다음과 같다.
        // 
        //  1. 서력 기원 연수가 4로 나누어떨어지는 해는 윤년으로 한다.(1988년, 1992년, 1996년, 2004년, 2008년, 2012년 …)
        //  2. 이 중에서 100으로 나누어떨어지는 해는 평년으로 한다.(1900년, 2100년, 2200년, 2300년, 2500년 …)
        //  3. 그중에 400으로 나누어떨어지는 해는 윤년으로 둔다.(1600년, 2000년, 2400년 …)
        //--------------------------------------------------

        static bool isLeapYear(int year)
        {
            if (year % 4 != 0)
                return false;

            if (year % 100 == 0 && year % 400 != 0)
                return false;

            return true;
        }
        static void _211()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "211 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));


            string[] words = line.Split(new char[] { });
            System.Diagnostics.Debug.Assert(words.Length == 2);

            int from = Convert.ToInt32(words[0]);
            int to = Convert.ToInt32(words[1]);

            int leapCount = 0;
            for (int i=from; i<=to; ++i) {
                if (isLeapYear(i))
                    ++leapCount;
            }


            Console.SetOut(new System.IO.StreamWriter(DIR + "output_211.txt"));

            Console.WriteLine(leapCount);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 212	파일입출력 - 형성평가8
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _212()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "212 input.txt"));

            string line = Console.ReadLine();
            int n = Convert.ToInt32(line);

            int[] scores_1 = new int[n];
            int[] scores_2 = new int[n];
            int[] scores_3 = new int[n];

            for (int i=0; i<n; ++i) {
                line = Console.ReadLine();
                string[] words = line.Split(new char[] { });
                System.Diagnostics.Debug.Assert(words.Length == 3);

                scores_1[i] = Convert.ToInt32(words[0]);
                scores_2[i] = Convert.ToInt32(words[1]);
                scores_3[i] = Convert.ToInt32(words[2]);
            }

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            int[] sum = new int[n];
            for (int i = 0; i < n; ++i) {
                sum[i] = scores_1[i] + scores_2[i] + scores_3[i];
            }
            Array.Sort(sum);
            Array.Reverse(sum);
            
            Console.SetOut(new System.IO.StreamWriter(DIR + "output_212.txt"));

            for (int i = 0; i < n; ++i) {
                double avg = sum[i] / 3d;
                Console.WriteLine("{0:n1}", avg);
            }

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 213	파일입출력 - 형성평가9
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _213()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "213 input.txt"));

            string line = Console.ReadLine();

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));

            string[] words = line.Split(new char[] { ' ' });
            string largest = words[0];
            for(int i=1; i<words.Length; ++i) {
                if (words[i].Length > largest.Length)
                    largest = words[i];
            }

            Console.SetOut(new System.IO.StreamWriter(DIR + "output_213.txt"));

            Console.WriteLine(line.Length);
            Console.WriteLine(largest);

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }



        //--------------------------------------------------
        // 214	파일입출력 - 형성평가A
        //
        //
        //
        // 입력 예 :
        //
        //
        // 출력 예 :
        //
        //
        //--------------------------------------------------
        static void _214()
        {
            Console.SetIn(new System.IO.StreamReader(DIR + "214 input.txt"));

            string[] words = new string[10];
            for (int i=0; i<words.Length; ++i) {
                words[i] = Console.ReadLine();
            }
            string line = Console.ReadLine();
            System.Diagnostics.Debug.Assert(line.Length == 1);
            char ch = Convert.ToChar(line);

            Console.In.Close();
            Console.SetIn(new System.IO.StreamReader(Console.OpenStandardInput()));


            Array.Sort(words);


            Console.SetOut(new System.IO.StreamWriter(DIR + "output_214.txt"));

            for(int i=0; i<words.Length; ++i) {
                if (words[i].IndexOf(ch) != -1) {
                    Console.WriteLine(words[i]);
                }
            }

            Console.Out.Close();
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()));
        }

    }
}
