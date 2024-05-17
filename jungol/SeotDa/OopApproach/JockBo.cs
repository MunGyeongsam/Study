using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace SeotDa.OopApproach
{
    //표준 섯다 룰 : http://free2cash.tistory.com/16
    class Rule
    {
        public const int SCORE_4_6  = 11;
        public const int SCORE_4_10 = 12;
        public const int SCORE_1_10 = 13;
        public const int SCORE_1_9  = 14;
        public const int SCORE_1_4  = 15;
        public const int SCORE_1_2  = 16;

        static Dictionary<int, string> mTitleMap = new Dictionary<int, string>();

        static void BuildTitleMap()
        {
            //todo : 이 함수를 완성하세요.

            //한번 설정했다면 다시 할 필요가 없음.
            if (mTitleMap.Count > 0)
                return;

            //1. 38,18,13  광땡

            //2. ~땡
            for (int i = 1; i <= 10; ++i)
            {
            }
            
            //3. 쎄륙 ~ 알리

            //4. ~끗
            mTitleMap[0] = "망통";
            mTitleMap[9] = "갑오";
            for (int i = 1; i <= 8; ++i)
            {
            }
        }
        public static string Title(int scroe)
        {
            BuildTitleMap();
            Debug.Assert(mTitleMap.ContainsKey(scroe));

            return mTitleMap[scroe];
        }
        //public static string Title(Card card1, Card card2)
        //{
        //    int m1 = Math.Min(card1.nMonth, card2.nMonth);
        //    int m2 = Math.Max(card1.nMonth, card2.nMonth);
        //
        //    if (card1.Type == Card.TYPE.LIGHTING && card2.Type == Card.TYPE.LIGHTING)
        //        return string.Format("{0}{1}광땡", m1, m2);
        //    
        //    if (m1 == m2)
        //        return string.Format("{0}땡", m1);
        //
        //    if (m1 == 1)
        //    {
        //        if (m2 == 2)
        //            return "알리";
        //        if (m2 == 4)
        //            return "독사";
        //        if (m2 == 9)
        //            return "구삥";
        //        if (m2 == 10)
        //            return "장삥";
        //    }
        //    else if (m1 == 4)
        //    {
        //        if (m2 == 10)
        //            return "장사";
        //        if (m2 == 6)
        //            return "쎄륙";
        //    }
        //
        //    int ggt = (m1 + m2) % 10;
        //    if (ggt == 9)
        //        return "갑오";
        //    if (ggt == 0)
        //        return "망통";
        //
        //    return string.Format("{0}끗", ggt);
        //}

        public static int Score(Card card1, Card card2)
        {
            int m1 = Math.Min(card1.nMonth, card2.nMonth);
            int m2 = Math.Max(card1.nMonth, card2.nMonth);

            //11000, 4000, 9000
            if (card1.Type == Card.TYPE.LIGHTING && card2.Type == Card.TYPE.LIGHTING)
                return (m1 + m2) * 1000;

            //100 ~ 1000
            if (m1 == m2)
                return m1 * 100;

            //11 ~ 16
            if (m1 == 1)
            {
                if (m2 == 2)
                    return SCORE_1_2;   //16
                if (m2 == 4)
                    return SCORE_1_4;   //15
                if (m2 == 9)
                    return SCORE_1_9;   //14
                if (m2 == 10)
                    return SCORE_1_10;  //13
            }
            else if (m1 == 4)
            {
                if (m2 == 10)
                    return SCORE_4_10;   //12
                if (m2 == 6)
                    return SCORE_4_6;    //11
            }
            
            //0 ~ 9
            int ggt = (m1 + m2) % 10;
            return ggt;
        }

        public static void Result(Player[] players)
        {
            int highScore = 0;
            foreach (Player p in players)
                highScore = Math.Max(highScore, p.Score);

            //13광땡, 18광땡
            if (highScore == (1+3)*1000 || highScore == (1+8)*1000)
            {
                //멍94
                bool hasTen94 = false;
                foreach (Player p in players)
                    hasTen94 |= p.IsTen94;

                if(hasTen94)
                {
                    foreach (Player p in players)
                    {
                        p.Result = "재";
                        if (p.IsTen94)
                            p.Title = "멍구사";
                    }
                    return;
                }


                //암행어사
                bool hasKiller = false;
                foreach (Player p in players)
                    hasKiller |= p.IsLightDoubleKiller;
                if(hasKiller)
                {
                    foreach (Player p in players)
                    {
                        if(p.IsLightDoubleKiller)
                        {
                            p.Title = "암행어사";
                            p.Result = "승";
                        }
                        else
                        {
                            p.Result = "패";
                        }
                    }
                    return;
                }
            }
            else if (highScore >= 100 && highScore <= 1000)
            {
                //94
                bool has94 = false;
                foreach (Player p in players)
                    has94 |= p.Is94;

                if (has94)
                {

                    foreach (Player p in players)
                    {
                        p.Result = "재";
                        if (p.Is94)
                            p.Title = "구사";
                    }
                    return;
                }

                //땡잡이
                bool hasKiller = false;
                foreach (Player p in players)
                    hasKiller |= p.IsDoubleKiller;
                if (hasKiller)
                {

                    foreach (Player p in players)
                    {
                        if (p.IsDoubleKiller)
                        {
                            p.Title = "땡잡이";
                            p.Result = "승";
                        }
                        else
                        {
                            p.Result = "패";
                        }
                    }
                    return;
                }
            }

            foreach (Player p in players)
                p.Result = (p.Score == highScore) ? "승" : "패";
        }
    }
}
