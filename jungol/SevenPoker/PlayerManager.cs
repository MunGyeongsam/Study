using System;

namespace SevenPoker
{
    class PlayerManager
    {
        Random mRnd = new Random();
        int mTop = 0;
        string[] mStudents = {
            "김다영",
            "김대형",
            "김명석",
            "김진환",
            "박보운",
            "박정민",
            "서영오",
            "서지은",
            "신광희",
            "안진교",
            "양승현",
            "이기훈",
            "이승민",
            "이재홍",
            "이재훈",
            "이한결",
            "임영빈",
            "장범진",
            "장  준",
            "정지수",
            "조원도",
            "주정현",
            "홍성현",
        };


        void Shuffle()
        {

            int LEN = mStudents.Length;
            for (int i = 0; i < 999; ++i)
            {
                int i0 = mRnd.Next(LEN);
                int i1 = mRnd.Next(LEN);
                while (i0 == i1)
                    i1 = mRnd.Next(LEN);

                string tmp = mStudents[i0];
                mStudents[i0] = mStudents[i1];
                mStudents[i1] = tmp;
            }
        }

        string NextName()
        {
            if (mTop >= mStudents.Length)
            {
                mTop = 0;
                Shuffle();
            }

            return mStudents[mTop++];
        }
        public PlayerManager()
        {
            Shuffle();
        }

        public Player[] NewPlayer(int n)
        {

            Player[] players = new Player[n];

            for (int i = 0; i < n; ++i)
                players[i] = new Player(NextName());

            return players;
        }
    }
}
