using System.Diagnostics;

namespace SevenPoker
{
    class Result
    {
        Card[] mCards = new Card[5];

        public Enum.Title Title {get;set; }
        public int Score {get;set; }

        public Result()
        {
            Title = Enum.Title.HIGH_CARDS;
            Score = 0;
        }

        public void SetCard(Card[] cards)
        {
            Title = Enum.Title.HIGH_CARDS;
            Score = 0;

            for(int i=0; i<5; ++i)
                mCards[i] = cards[i];
        }

        public void Update(Result rhs)
        {
            SetCard(rhs.mCards);
            
            Title = rhs.Title;
            Score = rhs.Score;
        }


        bool IsHigh_TwoPair(Result other)
        {
            Debug.Assert(Title == Enum.Title.TWO_PAIR);
            Debug.Assert(other.Title == Enum.Title.TWO_PAIR);
            
            for(int i=0; i<4; ++i)
            {
                if (mCards[i].No != mCards[i+1].No)
                    return mCards[i].Score > other.mCards[i].Score;
            }

            return false;
        }
        bool IsHigh_OnePair(Result other)
        {
            Debug.Assert(Title == Enum.Title.ONE_PAIR);
            Debug.Assert(other.Title == Enum.Title.ONE_PAIR);

            Enum.CardNo no = mCards[0].No;
            for(int i=0; i<4; ++i)
            {
                if (mCards[i].No == mCards[i+1].No)
                {
                    no = mCards[i].No;
                    break;
                }
            }
            
            
            for(int i=4; i>=0; --i)
            {
                if (mCards[i].No != no && mCards[i].ScoreN != other.mCards[i].ScoreN)
                    return mCards[i].ScoreN > other.mCards[i].ScoreN;
            }
            
            for(int i=4; i>=0; --i)
            {
                if (mCards[i].No != no && mCards[i].ScoreP != other.mCards[i].ScoreP)
                    return mCards[i].ScoreP > other.mCards[i].ScoreP;
            }
            
            return false;
        }
        bool IsHigh_HighCards(Result other)
        {
            Debug.Assert(Title == Enum.Title.HIGH_CARDS);
            Debug.Assert(other.Title == Enum.Title.HIGH_CARDS);

            for(int i=4; i>=0; --i)
            {
                if (mCards[i].ScoreN != other.mCards[i].ScoreN)
                    return mCards[i].ScoreN > other.mCards[i].ScoreN;
            }
            
            for(int i=4; i>=0; --i)
            {
                if (mCards[i].ScoreP != other.mCards[i].ScoreP)
                    return mCards[i].ScoreP > other.mCards[i].ScoreP;
            }
            
            Debug.Assert(false, "Impossible!");
            return false;
        }

        public bool IsHigh(Result other)
        {
            if (Score == other.Score)
            {
                Debug.Assert(Title == other.Title);

                switch(Title)
                {
                    case Enum.Title.TWO_PAIR:
                        return IsHigh_TwoPair(other);
                    case Enum.Title.ONE_PAIR:
                        return IsHigh_OnePair(other);
                    case Enum.Title.HIGH_CARDS:
                        return IsHigh_HighCards(other);
                }
                return false;
            }

            return (Score > other.Score);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}"
                , mCards[0].ToString()
                , mCards[1].ToString()
                , mCards[2].ToString()
                , mCards[3].ToString()
                , mCards[4].ToString()
                , Enum.Title2String(Title)
                );
        }
    }
}
