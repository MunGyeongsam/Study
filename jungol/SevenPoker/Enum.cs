using System.Diagnostics;

namespace SevenPoker
{
    static class Enum
    {
        public static void DebugCheck()
        {
            Debug.Assert(sPattern2String.Length == (int)Pattern.NUM);
            Debug.Assert(sNum2String.Length == (int)CardNo.NUM);
            Debug.Assert(sTitle2String.Length == (int)Title.NUM);
        }

        public enum Pattern
        {
            SPADE   ,
            DIAMOND ,
            HEART   ,
            CLUB    ,

            NUM
        }
        static string[] sPattern2String =
        {
            "♤",
            "◇",
            "♡",
            "♧",
        };

        public enum CardNo
        {
            INVALID = 0,

            _A  ,
            _2  ,
            _3  ,
            _4  ,
            _5  ,
            _6  ,
            _7  ,
            _8  ,
            _9  ,
            _10 ,
            _J  ,
            _Q  ,
            _K  ,

            NUM
        }
        static string[] sNum2String =
        {
            "Invalid",
            "A",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "J",
            "Q",
            "K",
        };

        public enum Title
        {
            ROYAL_STRAIGHT_FLUSH    ,
            BACK_STRAIGHT_FLUSH     ,
            STRAIGHT_FLUSH          ,
            FOUR_OF_A_KIND          ,
            FULL_HOUSE              ,
            FLUSH                   ,
            MOUNTAIN                ,   //A-K-Q-J-10
            BACK_STRAIGHT           ,   //A-2-3-4-5
            STRAIGHT                ,
            THREE_OF_A_KIND         ,
            TWO_PAIR                ,
            ONE_PAIR                ,
            HIGH_CARDS              ,

            NUM
        }
        static string[] sTitle2String =
        {
            "로티플",
            "빽스티플",
            "스티플",
            "포카드",
            "하우스",
            "플러쉬",
            "마운틴",
            "빽스트레잇",
            "스트레잇",
            "봉",
            "투페어",
            "원페어",
            "탑",
        };
        
        static public string Pattern2String(Pattern v)
        {
            return sPattern2String[(int)v];
        }
        static public string CardNo2String(CardNo v)
        {
            return sNum2String[(int)v];
        }
        static public string Title2String(Title v)
        {
            return sTitle2String[(int)v];
        }
    }
}
