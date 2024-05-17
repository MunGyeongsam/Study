using System;

namespace SevenPoker
{
    class Dealer
    {
        int mTop = 0;
        Card[] mDeck = new Card[13 * 4];
        Random mRandom = new Random();

        public Dealer()
        {
            for(int i=0; i<mDeck.Length; ++i)
                mDeck[i] = new Card();
        }
        public void Reset()
        {
            mTop = 0;
            int index = 0;
            for(Enum.Pattern i = Enum.Pattern.SPADE; i<=Enum.Pattern.CLUB; ++i)
            {
                for(Enum.CardNo j=Enum.CardNo._A; j<=Enum.CardNo._K; ++j)
                {
                    Card c = mDeck[index++];
                    c.Pattern = i;
                    c.No = j;
                }
            }
        }

        public void Shuffle()
        {
            const int CNT = 90000;

            for(int i=0; i<CNT; ++i)
            {
                int i1 = mRandom.Next(mDeck.Length);
                int i2 = mRandom.Next(mDeck.Length);
                while(i1 == i2)
                    i2 = mRandom.Next(mDeck.Length);

                Card tmp = mDeck[i1];
                mDeck[i1] = mDeck[i2];
                mDeck[i2] = tmp;
            }
        }

        public Card NextCard()
        {
            return mDeck[mTop++];
        }

    }
}
