using System;

namespace SevenPoker
{
    class Player
    {
        string mName = "";
        Card[] mCards = new Card[7];
        Result mResult;
        
        public Result Result {get { return mResult;} }
        public string Name {get { return mName;} }

        public Player(string name)
        {
            mName = name;
        }

        public void SetCard(int i, Card card)
        {
            mCards[i] = card;
        }

        public Card[] Cards
        {
            get {return mCards; }
        }

        public void UpdateResult()
        {
            Array.Sort(mCards, (a,b) => a.CompareTo(b));

            mResult = Rule.Title(this);
        }

        public void Show()
        {
            Console.Write("{0} :",mName);
            for(int i=0; i<7; ++i)
                Console.Write(" {0}", mCards[i].ToString());            
            if (mResult != null)
            {
                Console.Write(" => ");
                Console.WriteLine(mResult.ToString());
            }
            else
                Console.WriteLine();
        }
    }
}
