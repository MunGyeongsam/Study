using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeotDa.OopApproach
{
    class Player
    {
        string mName;

        Card mCard1;
        Card mCard2;

        public Player(string name)
        {
            mName = name;
            Score = 0;
            Result = "";
        }

        public string Title { get; set; }
        public string Result { get; set; }
        public int Score { get; set; }
        public Card Card1
        {
            set { mCard1 = value; }
        }
        public Card Card2
        {
            set
            {
                mCard2 = value;
                Score = Rule.Score(mCard1, mCard2);
                //Title = Rule.Title(mCard1, mCard2);

                Title = Rule.Title(Score);

            }
        }
        public bool IsTen
        {
            get { return mCard1.Type == Card.TYPE.TEN && mCard2.Type == Card.TYPE.TEN; }
        }
        public bool Is94
        {
            get
            {
                int m1 = Math.Min(mCard1.nMonth, mCard2.nMonth);
                int m2 = Math.Max(mCard1.nMonth, mCard2.nMonth);
                return (m1 == 4 && m2 == 9);
            }
        }
        public bool IsTen94
        {
            get { return IsTen && Is94; }
        }
        public bool IsLightDoubleKiller
        {
            get
            {
                int m1 = Math.Min(mCard1.nMonth, mCard2.nMonth);
                int m2 = Math.Max(mCard1.nMonth, mCard2.nMonth);
                return (IsTen && m1 == 4 && m2 == 7);
            }
        }
        public bool IsDoubleKiller
        {
            get
            {
                int m1 = Math.Min(mCard1.nMonth, mCard2.nMonth);
                int m2 = Math.Max(mCard1.nMonth, mCard2.nMonth);
                return (IsTen && m1 == 3 && m2 == 7);
            }
        }


        public void Show()
        {
            Card c1;
            Card c2;
            if (mCard1.nMonth < mCard2.nMonth)
            {
                c1 = mCard1;
                c2 = mCard2;
            }
            else
            {
                c1 = mCard1;
                c2 = mCard2;
            }

            Console.WriteLine("{0}({5}) : {1} + {2} = {3,5}({4})"
                , mName
                , mCard1.ToString()
                , mCard2.ToString()
                , Score
                , Title
                , Result);
        }
    }
}
