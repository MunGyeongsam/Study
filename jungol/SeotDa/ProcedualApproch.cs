using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeotDa
{
    //0. 표준 섯다 룰 : http://free2cash.tistory.com/16
    //1. 카드 초기화(20장)
    //2. 카드 섞기
    //3. 카드 분배
    //4. 각 플레이어 카드 프린트
    class ProcedualApproch
    {

        static Random mRnd = new Random();
        static int mTop = 0;
        static string[] mStudents = {
            "채경훈",
            "송석진",
            "방현수",
            "박종현",
            "김우진",
            "한예랑",
            "임성택",
            "박유정",
            "장민제",
            "곽훈",
            "이태훈",
            "이하늘",
            "문준형",
            "정용재"};

        static void ShuffleStudents()
        {
            int LEN = mStudents.Length;
            for (int i = 0; i < 999; ++i)
            {
                int i0 = mRnd.Next(LEN);
                int i1 = mRnd.Next(LEN);
                while (i0 == i1)
                    i1 = mRnd.Next(LEN);

                string tmp = mStudents[i0];
                mStudents[i0] = mStudents[i1];
                mStudents[i1] = tmp;
            }
        }
        static string Student()
        {
            if (mTop >= mStudents.Length)
            {
                mTop = 0;
                ShuffleStudents();
            }

            return mStudents[mTop++];
        }
        static void Print(int[] deck)
        {
            for (int i = 0; i < deck.Length; ++i)
            {
                Console.WriteLine("[{0,2}] : {1,4}, {2}", i, deck[i], deck[i] % 10);
            }
            Console.WriteLine();
        }

        static bool Is38LightingDouble(int a, int b)
        {
            return (a == 2003 && b == 2008);
        }
        static bool IsLightingDouble(int a, int b)
        {
            return (a + b) > 4000;
        }
        static string DoubleString(int n)
        {
            if (n == 1)
                return "삥땡";
            if (n == 10)
                return "장땡";

            return string.Format("{0}땡", n);
        }

        static string GetTitle(int a, int b
            , out int score
            , ref bool _doubleK         //땡잡이
            , ref bool _lightDoubleK    //암행어사
            , ref bool _94              //94
            , ref bool _194             //멍94
            )
        {
            score = 0;

            int ra = a % 100;
            int rb = b % 100;
            int card1 = (ra < rb) ? a : b;
            int card2 = (ra < rb) ? b : a;
            int remain1 = Math.Min(ra, rb);
            int remain2 = Math.Max(ra, rb);

            if (Is38LightingDouble(card1, card2))
            {
                score = 3000;
                return "38 광땡";
            }

            //13,18
            if (IsLightingDouble(card1, card2))
            {
                score = 2000;
                return string.Format("1{0} 광땡", Math.Max(ra, rb));
            }

            //double
            if (ra == rb)
            {
                score = ra * 100;
                return DoubleString(ra);
            }

            if (remain1 == 1)
            {
                switch (remain2)
                {
                    case 2: score = 16; return "알이";
                    case 4: score = 15; return "독사";
                    case 9: score = 14; return "구삥";
                    case 10: score = 13; return "장삥";
                }
            }
            else if (remain1 == 4)
            {
                if (10 == remain2) { score = 12; return "장사"; }
                if (6 == remain2) { score = 11; return "쎄륙"; }
            }

            score = (a + b) % 10;

            //3,7 double killer
            if (card1 == 2003 && card2 == 1007)
            {
                _doubleK = true;
                return "땡잡이";
            }
            //4,7 lighting double killer
            if (card1 == 1004 && card2 == 1007)
            {
                _lightDoubleK = true;
                return "암행어사";
            }
            //9,4
            if (remain1 == 4 && remain2 == 9)
            {
                _94 = true;
                if (card1 + card2 > 2000)
                {
                    _194 = true;
                    return "멍구사";
                }
                return "구사";
            }

            int ggt = (a + b) % 10;
            if (9 == ggt)
                return "갑오";
            if (0 == ggt)
                return "망통";
            return string.Format("{0} 끗", ggt);
        }
        static void SeotDa()
        {
            //1. 카드 초기화(20장)
            int[] deck =
            {
            2001, 501,
            1002, 502,
            2003, 503,
            1004, 504,
            1005, 505,
            1006, 506,
            1007, 507,
            2008,1008,
            1009, 509,
            1010, 510,
        };
            //Print(deck);

            //2. 카드 섞기
            Random rnd = new Random();
            for (int i = 0; i < 300; ++i)
            {
                int i1 = rnd.Next(20);
                int i2 = rnd.Next(20);
                while (i1 == i2)
                    i2 = rnd.Next(20);

                int tmp = deck[i1];
                deck[i1] = deck[i2];
                deck[i2] = tmp;
            }
            //Print(deck);

            //3. 카드 분배
            int numPlayer = 10;
            int[] playerCard = new int[numPlayer * 2];
            int[] score = new int[numPlayer];
            string[] title = new string[numPlayer];
            bool _doubleK = false;//땡잡이
            bool _lightDoubleK = false;//암행어사
            bool _94 = false;//94
            bool _194 = false;//멍94

            int top = 0;
            for (int i = 0; i < numPlayer; ++i)
            {
                playerCard[i * 2 + 0] = deck[top++];
                playerCard[i * 2 + 1] = deck[top++];
            }


            //4. 각 플레이어 카드 프린트
            for (int i = 0; i < numPlayer; ++i)
            {
                int card1 = playerCard[i * 2 + 0];
                int card2 = playerCard[i * 2 + 1];

                title[i] = GetTitle(card1, card2, out score[i]
                    , ref _doubleK
                    , ref _lightDoubleK
                    , ref _94
                    , ref _194);
            }

            int highScore = 0;
            for (int i = 0; i < numPlayer; ++i)
            {
                highScore = Math.Max(highScore, score[i]);
            }

            for (int i = 0; i < numPlayer; ++i)
            {
                int card1 = playerCard[i * 2 + 0];
                int card2 = playerCard[i * 2 + 1];
                string result = (score[i] == highScore) ? "승" : "패";

                //bool _doubleK = false;//땡잡이
                //bool _lightDoubleK = false;//암행어사
                //bool _94 = false;//94
                //bool _194 = false;//멍94
                if (_194 && highScore < 1000)
                {
                    result = "다시";
                }
                else if (_94 && highScore < 100)
                {
                    result = "다시";
                }
                else if (_doubleK && 100 <= highScore && highScore <= 900)
                {
                    if (card1 + card2 == 2003 + 1007)
                        result = "승";
                    else
                        result = "패";
                }
                else if (_lightDoubleK && 2000 == highScore)
                {
                    if (card1 + card2 == 1004 + 1007)
                        result = "승";
                    else
                        result = "패";
                }

                Console.WriteLine("[{0}] : {1,4}, {2,4} -> {3}({6} {4}, {5})"
                        , i
                        , card1
                        , card2
                        , Student()
                        , title[i]
                        , score[i]
                        , result);
            }

            Console.WriteLine();
        }

        public static void Run()
        {
            ShuffleStudents();
            while (true)
            {
                Console.WriteLine("enter n to stop");
                string line = Console.ReadLine();
                Console.Clear();
                if (line == "n")
                    break;
                SeotDa();
            }
        }
    }
}
