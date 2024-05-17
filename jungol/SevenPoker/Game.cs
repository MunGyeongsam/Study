using System;

namespace SevenPoker
{
    class Game
    {
        static PlayerManager mPlayerMgr = new PlayerManager();
        static Dealer mDealer = new Dealer();
        static uint mGameCount = 0;
        static uint mTotalPlayer = 0;
        static uint[] mStatics =
        {
            0, //ROYAL_STRAIGHT_FLUSH    ,
            0, //BACK_STRAIGHT_FLUSH     ,
            0, //STRAIGHT_FLUSH          ,
            0, //FOUR_OF_A_KIND          ,
            0, //FULL_HOUSE              ,
            0, //FLUSH                   ,
            0, //MOUNTAIN                ,   //A-K-Q-J-10
            0, //BACK_STRAIGHT           ,   //A-2-3-4-5
            0, //STRAIGHT                ,
            0, //THREE_OF_A_KIND         ,
            0, //TWO_PAIR                ,
            0, //ONE_PAIR                ,
            0, //HIGH_CARDS              ,
        };
        static uint[] mWinCount =
        {
            0, //ROYAL_STRAIGHT_FLUSH    ,
            0, //BACK_STRAIGHT_FLUSH     ,
            0, //STRAIGHT_FLUSH          ,
            0, //FOUR_OF_A_KIND          ,
            0, //FULL_HOUSE              ,
            0, //FLUSH                   ,
            0, //MOUNTAIN                ,   //A-K-Q-J-10
            0, //BACK_STRAIGHT           ,   //A-2-3-4-5
            0, //STRAIGHT                ,
            0, //THREE_OF_A_KIND         ,
            0, //TWO_PAIR                ,
            0, //ONE_PAIR                ,
            0, //HIGH_CARDS              ,
        };
        
        static void NextGame(int numPlayer)
        {
            ++mGameCount;
            mTotalPlayer += (uint)numPlayer;

            mDealer.Reset();
            mDealer.Shuffle();

            Player[] players = mPlayerMgr.NewPlayer(numPlayer);
            
            for (int i = 0; i < 7; ++i)
            {
                for (int j = 0; j < numPlayer; ++j)
                    players[j].SetCard(i, mDealer.NextCard());
            }
            
            for (int i=0; i<numPlayer; ++i)
            {
                players[i].Show();
            }
            
            Console.WriteLine();
            Result highRecord = null;
            Result currRecord = null;
            Player winner = null;
            for (int i=0; i<numPlayer; ++i)
            {
                players[i].UpdateResult();
                currRecord = players[i].Result;

                ++mStatics[(int)currRecord.Title];

                if(highRecord == null || currRecord.IsHigh(highRecord))
                {
                    winner = players[i];
                    highRecord = winner.Result;
                }
            }
            for (int i=0; i<numPlayer; ++i)
            {
                players[i].Show();
            }
            if (null != highRecord)
            {
                ++mWinCount[(int)highRecord.Title];
                Console.WriteLine();
                Console.WriteLine("winner : {0} {1}", winner.Name, highRecord.ToString());
            }
            Console.WriteLine();

            Console.WriteLine("statics");
            Console.WriteLine("game count : {0,6}, totalPlayer : {1}", mGameCount, mTotalPlayer);
            for(int i=0; i<(int)Enum.Title.NUM; ++i)
                Console.WriteLine("--{0,20} : {1,8}({2,8:F4} %) -- win count : {3,5} ({4,8:F2} %)"
                    , (Enum.Title)i
                    , mStatics[i]
                    , (float)mStatics[i] / (float)mTotalPlayer * 100f
                    , mWinCount[i]
                    , (float)mWinCount[i] / (float)mStatics[i] * 100f
                    );

        }
        
        public static void Run()
        {
            Enum.DebugCheck();

            int numPlayer = 7;
            while (true)
            {
                Console.WriteLine("enter n to stop");
                string line = Console.ReadLine();
                Console.Clear();
                if (line == "n" || line == "N")
                    break;

                NextGame(numPlayer);
            }
        }
    }
}
