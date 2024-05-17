using System;
using System.Collections.Generic;

namespace SeotDa.OopApproach
{
    class Dealer
    {
        const int CARD_NUM = 20;
        Random mRnd = new Random();
        List<Card> mDeck = new List<Card>();


        public void Reset()
        {
            //todo : 이 함수를 완성하세요.

            //1. mDeck 의 모든 요소를 지운다.
            //2. mDeck 에 20장의 카드를 채운다
        }

        public Card NextCard()
        {
            Card rt = null;
            //todo : 이 함수를 완성하세요.
            
            //1. mDeck 의 갯수보다 작은 임의의 수를 설정
            //2. rt 에 해당 인덱스의 값을 저장
            //3. 해당 인덱스를 지운다.
            return rt;
        }

    }
    //class Dealer
    //{
    //    const int CARD_NUM = 20;
    //    Random mRnd = new Random();
    //    Card[] mDeck = new Card[CARD_NUM];
    //    int mTop = 0;
    //
    //    public Dealer()
    //    {
    //        for(int i=0; i<20; i+=2 )
    //        {
    //            mDeck[i+0] = new Card();
    //            mDeck[i+1] = new Card();
    //        }
    //    }
    //
    //    public void Reset()
    //    {
    //        for (int i = 0; i < 10; ++i)
    //        {
    //            Card c1 = mDeck[i * 2 + 0];
    //            Card c2 = mDeck[i * 2 + 1];
    //
    //            Card.MONTH month = (Card.MONTH)i;
    //
    //            c1.Month = month;
    //            c2.Month = month;
    //
    //            if(month == Card.MONTH.JAN || month == Card.MONTH.MAR || month == Card.MONTH.AUG)
    //                c1.Type = Card.TYPE.LIGHTING;
    //            else
    //                c1.Type = Card.TYPE.TEN;
    //
    //            c2.Type = Card.TYPE.BAND;
    //        }
    //    }
    //
    //    public void Shuffle()
    //    {
    //        mTop = 0;
    //
    //        for (int i = 0; i < 300; ++i)
    //        {
    //            int i1 = mRnd.Next(CARD_NUM);
    //            int i2 = mRnd.Next(CARD_NUM);
    //            while (i1 == i2)
    //                i2 = mRnd.Next(CARD_NUM);
    //
    //            Card tmp = mDeck[i1];
    //            mDeck[i1] = mDeck[i2];
    //            mDeck[i2] = tmp;
    //        }
    //    }
    //
    //
    //    public Card NextCard()
    //    {
    //        return mDeck[mTop++];
    //    }
    //    
    //}
}
