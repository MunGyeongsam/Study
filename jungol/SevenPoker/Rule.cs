namespace SevenPoker
{
    class Rule
    {
        //기본 룰 : https://namu.wiki/w/%ED%8F%AC%EC%BB%A4
        //동일 족보 처리 : http://www.seenjoy.com/faq/faq_vew.asp?P_IDX=67&IDX=30
        
        // 아래는 7장의 카드에서 5장의 카드를 선택할 수 있는 모든 경우를 나타 냅니다.
        // 그 아래 주석처리된 함수 RecsvGen 와 Test 는 아래의 경우를 모두 출력해주는 코드입니다.
        // 주의 : 테스트 코드는 1을 기준으로 "0 1 2 3 4" 는 "1 2 3 4 5" 로 찍힙니다.)
        // 
        // 분석 시 어려움이 있을까하여 적어 봅니다.
        static int[,] allKindOfCombination =
        {
            {0, 1, 2, 3, 4},
            {0, 1, 2, 3, 5},
            {0, 1, 2, 3, 6},
            {0, 1, 2, 4, 5},
            {0, 1, 2, 4, 6},
            {0, 1, 2, 5, 6},
            {0, 1, 3, 4, 5},
            {0, 1, 3, 4, 6},
            {0, 1, 3, 5, 6},
            {0, 1, 4, 5, 6},
            {0, 2, 3, 4, 5},
            {0, 2, 3, 4, 6},
            {0, 2, 3, 5, 6},
            {0, 2, 4, 5, 6},
            {0, 3, 4, 5, 6},
            {1, 2, 3, 4, 5},
            {1, 2, 3, 4, 6},
            {1, 2, 3, 5, 6},
            {1, 2, 4, 5, 6},
            {1, 3, 4, 5, 6},
            {2, 3, 4, 5, 6}
        };
        //@{ generating of all kinds of card-combination
        //static int curr;
        //static int[,] allKinds = new int[21,5];
        //
        //
        //static void RecsvGen(int[] dst, int index, int depth, int digit)
        //{
        //    if (depth >= 5)
        //    {
        //        allKinds[curr, 0] = dst[0];
        //        allKinds[curr, 1] = dst[1];
        //        allKinds[curr, 2] = dst[2];
        //        allKinds[curr, 3] = dst[3];
        //        allKinds[curr, 4] = dst[4];
        //        ++curr;
        //        Console.WriteLine("curr : {0}", curr);
        //        Console.WriteLine(String.Join("", new List<int>(dst).ConvertAll(i => (i+1).ToString()).ToArray()));
        //        return;
        //    }
        //
        //    for(int i=index; i <= 7-digit; ++i)
        //    {
        //        dst[depth] = i;
        //        RecsvGen(dst, i+1,depth+1,digit-1);
        //    }
        //}
        //public static void Test()
        //{
        //    int[] dest = {0,0,0,0,0};
        //    RecsvGen(dest,0,0,5);
        //}
        //@}
        
        const int SCORE_ROYAL_STRAIGHT_FLUSH    = 9000;
        const int SCORE_BACK_STRAIGHT_FLUSH     = 8500;
        const int SCORE_STRAIGHT_FLUSH          = 8000;
        const int SCORE_FOUR_OF_A_KIND          = 7000;
        const int SCORE_FULL_HOUSE              = 6000;
        const int SCORE_FLUSH                   = 5000;
        const int SCORE_MOUNTAIN                = 4300;
        const int SCORE_BACK_STRAIGHT           = 4200;
        const int SCORE_STRAIGHT                = 4000;
        const int SCORE_THREE_OF_A_KIND         = 3000;
        const int SCORE_TWO_PAIR                = 2000;
        const int SCORE_ONE_PAIR                = 1000;
        const int SCORE_HIGH_CARDS              =    0;
        
        
        static int Score_ROYAL_STRAIGHT_FLUSH(Card[] fiveCards)
        {
            return SCORE_ROYAL_STRAIGHT_FLUSH + fiveCards[0].ScoreP;
        }
        static int Score_BACK_STRAIGHT_FLUSH(Card[] fiveCards)
        {
            return SCORE_BACK_STRAIGHT_FLUSH + fiveCards[0].ScoreP;
        }
        static int Score_STRAIGHT_FLUSH(Card[] fiveCards)
        {
            Card topCard = fiveCards[4];
            return SCORE_STRAIGHT_FLUSH + topCard.Score;
        }
        static int Score_FOUR_OF_A_KIND(Card[] fiveCards)
        {
            Enum.CardNo n0 = fiveCards[0].No;
            Enum.CardNo n1 = fiveCards[1].No;
            int add =(n0 == n1) ? fiveCards[0].ScoreN : fiveCards[1].ScoreN;

            return SCORE_FOUR_OF_A_KIND + add;
        }
        static int Score_FULL_HOUSE(Card[] fiveCards)
        {
            // fiveCards must be sorted
            int add = fiveCards[2].ScoreN;
            return SCORE_FULL_HOUSE + add;
        }
        static int Score_FLUSH(Card[] fiveCards)
        {
            Card topCard = fiveCards[4];
            return SCORE_FLUSH + topCard.Score;
        }
        static int Score_MOUNTAIN(Card[] fiveCards)
        {
            // A-K-Q-J-10
            Card topCard = fiveCards[4];
            return SCORE_MOUNTAIN + topCard.Score;
        }
        static int Score_BACK_STRAIGHT(Card[] fiveCards)
        {
            // A-2-3-4-5
            Card topCard = fiveCards[4];
            return SCORE_BACK_STRAIGHT + topCard.Score;
        }
        static int Score_STRAIGHT(Card[] fiveCards)
        {
            Card topCard = fiveCards[4];
            return SCORE_STRAIGHT + topCard.Score;
        }
        static int Score_THREE_OF_A_KIND(Card[] fiveCards)
        {
            // fiveCards must be sorted
            int add = fiveCards[2].ScoreN;
            return SCORE_THREE_OF_A_KIND + add;
        }
        static int Score_TWO_PAIR(Card[] fiveCards)
        {
            Enum.CardNo n4 = fiveCards[4].No;
            Enum.CardNo n3 = fiveCards[3].No;
            Enum.CardNo n1 = fiveCards[1].No;
            Enum.CardNo n0 = fiveCards[0].No;

            int addH = (n4 == n3) ? fiveCards[4].ScoreN : fiveCards[3].ScoreN;
            int addL = (n0 == n1) ? fiveCards[0].ScoreN : fiveCards[1].ScoreN;
            return SCORE_TWO_PAIR + (addH+200) + addL;
        }
        static int Score_ONE_PAIR(Card[] fiveCards)
        {
            Card card = null;
            for(int i=0; i<4; ++i)
            {
                if (fiveCards[i].No == fiveCards[i+1].No)
                {
                    card = fiveCards[i];
                    break;
                }
            }

            return SCORE_ONE_PAIR + card.ScoreN;
        }
        static int Score_HIGH_CARDS(Card[] fiveCards)
        {
            Card topCard = fiveCards[4];
            return SCORE_HIGH_CARDS + topCard.ScoreN;
        }


        static bool IsFlush(Card[] fiveCards)
        {
            Card c0 = fiveCards[0];
            return  (c0.Pattern == fiveCards[1].Pattern) &&
                    (c0.Pattern == fiveCards[2].Pattern) &&
                    (c0.Pattern == fiveCards[3].Pattern) &&
                    (c0.Pattern == fiveCards[4].Pattern) ;
        }
        static bool IsMountain(Card[] fiveCards)
        {
            return  (fiveCards[0].No == Enum.CardNo._A) &&
                    (fiveCards[1].No == Enum.CardNo._10) &&
                    (fiveCards[2].No == Enum.CardNo._J) &&
                    (fiveCards[3].No == Enum.CardNo._Q) &&
                    (fiveCards[4].No == Enum.CardNo._K) ;
        }
        static bool IsStraight(Card[] fiveCards)
        {
            Card c0 = fiveCards[0];
            return  (c0.No + 1 == fiveCards[1].No) &&
                    (c0.No + 2 == fiveCards[2].No) &&
                    (c0.No + 3 == fiveCards[3].No) &&
                    (c0.No + 4 == fiveCards[4].No) ;
        }
        static bool IsRoyalStraightFlush(Card[] fiveCards)
        {
            return IsFlush(fiveCards) && IsMountain(fiveCards);
        }
        static bool IsStraightFlush(Card[] fiveCards)
        {
            return IsFlush(fiveCards) && IsStraight(fiveCards);
        }

        static Enum.CardNo FindMultipleCard(int[] counts, int cnt)
        {
            for(Enum.CardNo no = Enum.CardNo._K; no >= Enum.CardNo._A; --no)
            {
                int i = (int)no;
                if(counts[i] == cnt)
                    return no;
            }

            return Enum.CardNo.INVALID;
        }

        static Enum.CardNo FindFourCard(int[] counts)
        {
            return FindMultipleCard(counts, 4);
        }
        static Enum.CardNo FindThreeCard(int[] counts)
        {
            return FindMultipleCard(counts, 3);
        }
        static Enum.CardNo Find_1stPair(int[] counts)
        {
            return FindMultipleCard(counts, 2);
        }
        static Enum.CardNo Find_2ndPair(int[] counts, Enum.CardNo firstPairNo)
        {
            int tmpCount = counts[(int)firstPairNo];
            counts[(int)firstPairNo] = 0;

            Enum.CardNo rt = FindMultipleCard(counts, 2);
            counts[(int)firstPairNo] = tmpCount;

            return rt;
        }

        static Enum.Title FiveCardsTitle(Card[] fiveCards)
        {
            if(IsFlush(fiveCards))
            {
                if(IsMountain(fiveCards))
                    return Enum.Title.ROYAL_STRAIGHT_FLUSH;
                if(IsStraight(fiveCards))
                {
                    if (Enum.CardNo._A == fiveCards[0].No)
                        return Enum.Title.BACK_STRAIGHT_FLUSH;
                    return Enum.Title.STRAIGHT_FLUSH;
                }

                // impossible below high titles when flush
                //  - FOUR_OF_A_KIND
                //  - FULL_HOUSE
                return Enum.Title.FLUSH;
            }
            
            int[] counts = new int[(int)Enum.CardNo._K+1];
            foreach(Card c in fiveCards)
                ++counts[c.nNo];
            
            if(Enum.CardNo.INVALID != FindFourCard(counts))
            {
                return Enum.Title.FOUR_OF_A_KIND;
            }
            
            if(Enum.CardNo.INVALID != FindThreeCard(counts))
            {
                if (Enum.CardNo.INVALID != Find_1stPair(counts))
                    return Enum.Title.FULL_HOUSE;

                //FLUSH, MOUNTAIN, BACK_STRAIGHT and STRAIGHT are not possible
                return Enum.Title.THREE_OF_A_KIND;
            }

            if (IsFlush(fiveCards))
            {
                return Enum.Title.FLUSH;
            }

            if (IsMountain(fiveCards))
            {
                return Enum.Title.MOUNTAIN;
            }

            if (IsStraight(fiveCards))
            {
                if (Enum.CardNo._A == fiveCards[0].No)
                    return Enum.Title.BACK_STRAIGHT;

                return Enum.Title.STRAIGHT;
            }
            
            Enum.CardNo fistPairNo = Find_1stPair(counts);
            if (Enum.CardNo.INVALID != fistPairNo)
            {
                if (Enum.CardNo.INVALID != Find_2ndPair(counts, fistPairNo))
                    return Enum.Title.TWO_PAIR;

                return Enum.Title.ONE_PAIR;
            }

            return Enum.Title.HIGH_CARDS;
        }
        static int FiveCardsScore(Enum.Title title, Card[] fiveCards)
        {
            switch(title)
            {
                case Enum.Title.ROYAL_STRAIGHT_FLUSH:
                    return Score_ROYAL_STRAIGHT_FLUSH(fiveCards);

                case Enum.Title.BACK_STRAIGHT_FLUSH:
                    return Score_BACK_STRAIGHT_FLUSH(fiveCards);

                case Enum.Title.STRAIGHT_FLUSH:
                    return Score_STRAIGHT_FLUSH(fiveCards);

                case Enum.Title.FOUR_OF_A_KIND:
                    return Score_FOUR_OF_A_KIND(fiveCards);

                case Enum.Title.FULL_HOUSE:
                    return Score_FULL_HOUSE(fiveCards);

                case Enum.Title.FLUSH:
                    return Score_FLUSH(fiveCards);

                case Enum.Title.MOUNTAIN:
                    return Score_MOUNTAIN(fiveCards);

                case Enum.Title.BACK_STRAIGHT:
                    return Score_BACK_STRAIGHT(fiveCards);

                case Enum.Title.STRAIGHT:
                    return Score_STRAIGHT(fiveCards);

                case Enum.Title.THREE_OF_A_KIND:
                    return Score_THREE_OF_A_KIND(fiveCards);

                case Enum.Title.TWO_PAIR:
                    return Score_TWO_PAIR(fiveCards);

                case Enum.Title.ONE_PAIR:
                    return Score_ONE_PAIR(fiveCards);

                case Enum.Title.HIGH_CARDS:
                    return Score_HIGH_CARDS(fiveCards);
            }
            return 0;
        }
        public static Result Title(Player player)
        {
            Card[] cards_7 = player.Cards;
            Card[] cards_5 = new Card[5];

            
            Result bestResult = new Result();
            Result currResult = new Result();
            for(int i=0; i<allKindOfCombination.GetLength(0); ++i)
            {
                cards_5[0] = cards_7[allKindOfCombination[i,0]];
                cards_5[1] = cards_7[allKindOfCombination[i,1]];
                cards_5[2] = cards_7[allKindOfCombination[i,2]];
                cards_5[3] = cards_7[allKindOfCombination[i,3]];
                cards_5[4] = cards_7[allKindOfCombination[i,4]];

                currResult.SetCard(cards_5);
                currResult.Title = FiveCardsTitle(cards_5);
                currResult.Score = FiveCardsScore(currResult.Title, cards_5);

                if (currResult.IsHigh(bestResult))
                    bestResult.Update(currResult);
            }


            return bestResult;
        }
    }
}
