using System;

namespace SeotDa.OopApproach
{
    class Game
    {
        static Dealer mDealer = new Dealer();
        static Players mPlayers = new Players();

        static void NextGame(int numPlayer)
        {
            mDealer.Reset();
            //mDealer.Shuffle();

            Player[] players = mPlayers.NewPlayer(numPlayer);


            for (int i = 0; i < numPlayer; ++i)
                players[i].Card1 = mDealer.NextCard();
            for (int i = 0; i < numPlayer; ++i)
                players[i].Card2 = mDealer.NextCard();

            Rule.Result(players);
            for (int i=0; i<numPlayer; ++i)
            {
                players[i].Show();
            }
        }

        public static void Run()
        {

            int numPlayer = 10;
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
