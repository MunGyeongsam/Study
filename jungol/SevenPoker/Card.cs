using System;

namespace SevenPoker
{


    class Card
    {

        public Enum.Pattern Pattern { get; set; }
        public Enum.CardNo No { get; set; }
        
        public int nPattern { get {return (int)Pattern; } }
        public int nNo { get {return (int)No; } }
        
        public int ScoreN { get { return nNo * 10;} }
        public int ScoreP {
            get 
            {
                //SPADE     : 4
                //DIAMOND   : 3
                //HEART     : 2
                //CLUB      : 1
                return 4-nPattern;
            }
        }
        public int Score { get { return ScoreN + ScoreP;} }

        String sPattern { get { return Enum.Pattern2String(Pattern); } }
        String sNumber { get { return Enum.CardNo2String(No); } }

        public override string ToString()
        {
            return string.Format("{1,2}({0})", sPattern, sNumber);
        }

        public int CompareTo(Card rhs)
        {
            return (No == rhs.No) ? Pattern.CompareTo(rhs.Pattern) : No.CompareTo(rhs.No);
        }
    }
}
