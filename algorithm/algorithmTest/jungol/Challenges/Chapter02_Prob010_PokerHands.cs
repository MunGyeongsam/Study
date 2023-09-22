using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace jungol.Challenges
{
    class Card
    {
        public enum PatternType
        {
            Club,
            Diamond,
            Heart,
            Spade
        }

        string _card;
        public void SetCard(string card) { _card=card; }

        public int Number { 
            get 
            {
                char n = _card[0];
                switch(n)
                {
                    case 'A': return 14;
                    case 'K': return 13;
                    case 'Q': return 12;
                    case 'J': return 11;
                    case 'T': return 10;
                    default:
                        return n - '0';
                }
            }
        }

        public PatternType Pattern
        {
            get
            {
                char c = _card[1];
                switch(c)
                {
                    case 'C': return PatternType.Club;
                    case 'D': return PatternType.Diamond;
                    case 'H': return PatternType.Heart;                    
                }
                return PatternType.Spade;
            }
        }

        public int Cmp(Card rhs)
        {
            int n1 = Number;
            int n2 = rhs.Number;

            return (n1 == n2) ? (Pattern - rhs.Pattern) : n1 - n2;
        }

        public override string ToString()
        {
            return _card;
        }
    }
    class PockerHand
    {
        public enum TitleType
        {
            HighCard,
            OnePair,
            TwoPair,
            Triple,
            Straight,
            Flush,
            FullHouse,
            FourCard,
            StraightFlush,

            NUM
        }
        struct Counts
        {
            int Count1;
            int Count2;
            int Count3;
            int Count4;
            int Count5;

            int Num1;
            int Num2;
            int Num3;
            int Num4;
            int Num5;


            public int this[int index]
            {
                get { 
                    switch (index)
                    {
                        case 0: return Count1;
                        case 1: return Count2;
                        case 2: return Count3;
                        case 3: return Count4;
                        case 4: return Count5;
                    }
                    return -1;
                }
                set {
                    switch (index)
                    {
                        case 0: Count1 = value; break;
                        case 1: Count2 = value; break;
                        case 2: Count3 = value; break;
                        case 3: Count4 = value; break;
                        case 4: Count5 = value; break;
                    }
                }
            }

            public int this[int i, int j]
            {
                get
                {
                    switch (i)
                    {
                        case 0: return (j==0) ? Count1 : Num1;
                        case 1: return (j==0) ? Count2 : Num2;
                        case 2: return (j==0) ? Count3 : Num3;
                        case 3: return (j==0) ? Count4 : Num4;
                        case 4: return (j==0) ? Count5 : Num5;
                    }
                    return -1;
                }
                set
                {
                    if (j == 0)
                    {
                        switch (i)
                        {
                            case 0: Count1 = value; break;
                            case 1: Count2 = value; break;
                            case 2: Count3 = value; break;
                            case 3: Count4 = value; break;
                            case 4: Count5 = value; break;
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0: Num1 = value; break;
                            case 1: Num2 = value; break;
                            case 2: Num3 = value; break;
                            case 3: Num4 = value; break;
                            case 4: Num5 = value; break;
                        }
                    }
                }
            }
        }
        Card[] _cards = new Card[5] {
            new Card(),
            new Card(),
            new Card(),
            new Card(),
            new Card()
        };

        public void Set(params string[] input)
        {
            for(int i=0; i<input.Length; i++)
            {
                _cards[i].SetCard(input[i]);
            }

            //Console.WriteLine(this);
            Array.Sort(_cards, (a, b) => a.Cmp(b));
            //Console.WriteLine(this);
        }

        public override string ToString()
        {
            return string.Format($"{_cards[0]} {_cards[1]} {_cards[2]} {_cards[3]} {_cards[4]} : {Title, -14}");
        }

        int TripleCmp(PockerHand rhs)
        {
            return _cards[2].Number - rhs._cards[2].Number;
        }

        int HighCardCmp(PockerHand rhs)
        {
            for (var i = 4; i >= 0; --i)
            {
                var diff = _cards[i].Number - rhs._cards[i].Number;
                if (0 == diff)
                    continue;

                return diff;
            }
            return 0;
        }

        int TwoPairCmp(PockerHand rhs)
        {
            var lcounting = this.Counting;
            var rcounting = rhs.Counting;

            System.Diagnostics.Debug.Assert(lcounting[2] != 0 && lcounting[3] == 0);
            System.Diagnostics.Debug.Assert(rcounting[2] != 0 && rcounting[3] == 0);

            //int lsum = 0;
            //int rsum = 0;
            //int lcnt = 0;
            //int rcnt = 0;
            //for (int i=0; i<3; ++i)
            //{
            //    lcnt += lcounting[i];
            //    rcnt += rcounting[i];
            //    lsum += lcounting[i, 1] * (lcnt == 1 ? 1 : (lcnt == 5 ? 10000 : 100));
            //    rsum += rcounting[i, 1] * (rcnt == 1 ? 1 : (rcnt == 5 ? 10000 : 100));
            //}
            //return lsum - rsum;

            int lnum = 0;
            int rnum = 0;

            //for highest pair
            for(int i=2; i>=0 && lnum == 0 && rnum == 0; --i)
            {
                if (lnum == 0 && lcounting[i] == 2)
                    lnum = lcounting[i, 1];
                if (rnum == 0 && rcounting[i] == 2)
                    rnum = rcounting[i, 1];
            }
            if (lnum != rnum)
                return lnum - rnum;

            //for lowest pair
            lnum = 0;
            rnum = 0;
            for (int i = 0; i < 3 && (lnum == 0 || rnum == 0); ++i)
            {
                if (lnum == 0 && lcounting[i] == 2)
                    lnum = lcounting[i, 1];
                if (rnum == 0 && rcounting[i] == 2)
                    rnum = rcounting[i, 1];
            }
            if (lnum != rnum)
                return lnum - rnum;

            //for highcard
            lnum = 0;
            rnum = 0;
            for (int i = 0; i < 3 && (lnum == 0 || rnum == 0); ++i)
            {
                if (lcounting[i] == 1)
                    lnum = lcounting[i, 1];
                if (rcounting[i] == 1)
                    rnum = rcounting[i, 1];
            }

            return lnum - rnum;
        }

        int PairCmp(PockerHand rhs)
        {
            var lcounting = this.Counting;
            var rcounting = rhs.Counting;

            int lnum = 0;
            int rnum = 0;

            System.Diagnostics.Debug.Assert(lcounting[3] != 0 && lcounting[4] == 0);
            System.Diagnostics.Debug.Assert(rcounting[3] != 0 && rcounting[4] == 0);

            //for pair
            for (int i = 3; i >= 0 && (lnum == 0 || rnum == 0); --i)
            {
                if (lnum == 0 && lcounting[i] == 2)
                    lnum = lcounting[i, 1];
                if (rnum == 0 && rcounting[i] == 2)
                    rnum = rcounting[i, 1];
            }
            if (lnum != rnum)
                return lnum - rnum;

            //for high-card
            int li = 2;
            int ri = 2;
            for (int i = 0; i < 3; ++i)
            {
                lnum = 0;
                rnum = 0;
                for (; li >= 0; --li)
                {
                    if (lcounting[li] == 1)
                    {
                        lnum = lcounting[li--, 1];
                        break;
                    }
                }
                for (; ri >= 0; --ri)
                {
                    if (lcounting[li] == 1)
                    {
                        lnum = lcounting[li--, 1];
                        break;
                    }
                }
                if (lnum != rnum)
                    return lnum - rnum;
            }

            return 0;
        }

        public int Cmp(PockerHand rhs)
        {
            var t1 = Title;
            var t2 = rhs.Title;

            if (t1 == t2)
            {
                switch(t1)
                {
                    case TitleType.StraightFlush:
                    case TitleType.FourCard:
                    case TitleType.Straight:
                    case TitleType.FullHouse:
                    case TitleType.Triple:
                        return TripleCmp(rhs);

                    case TitleType.Flush:
                    case TitleType.HighCard:
                        return HighCardCmp(rhs);

                    case TitleType.TwoPair:
                        return TwoPairCmp(rhs);

                    case TitleType.OnePair:
                        return PairCmp(rhs);

                    default: throw new IndexOutOfRangeException();
                }
            }

            return t1 - t2;
        }

        public TitleType Title
        {
            get
            {
                if (IsStraightFlush)
                    return TitleType.StraightFlush;

                if (IsFourCard)
                    return TitleType.FourCard;

                if (IsFullHouse)
                    return TitleType.FullHouse;

                if (IsFlush)
                    return TitleType.Flush;

                if (IsStraight)
                    return TitleType.Straight;

                if (IsTriple)
                    return TitleType.Triple;

                if (IsTwoPair)
                    return TitleType.TwoPair;

                if (IsOnePair)
                    return TitleType.OnePair;

                return TitleType.HighCard;
            }
        }

        public bool IsStraightFlush
        {
            get => IsStraight && IsFlush;
        }

        Counts Counting
        {
            get
            {
                Counts count = new Counts();

                int n = _cards[0].Number;
                int index = 0;
                count[index] = 1;
                count[index, 1] = n;
                for(int i=1; i<5; ++i)
                {
                    if (n != _cards[i].Number)
                    {
                        n = _cards[i].Number;
                        index++;
                        count[index] = 1;
                        count[index, 1] = n;
                    }
                    else
                    {
                        ++count[index];
                    }
                }

                return count;
            }
        }

        public bool IsOnePair
        {
            get
            {
                var cnts = Counting;
                int rt = 0;
                for (var i = 0; i < 5; ++i)
                {
                    if (cnts[i] == 0)
                        break;
                    if (cnts[i] == 2)
                        ++rt;
                }

                return rt == 1;
            }
        }

        public bool IsTwoPair
        {
            get
            {
                var cnts = Counting;
                int rt = 0;
                for(var i=0; i<5; ++i)
                {
                    if (cnts[i] == 0)
                        break;
                    if (cnts[i] == 2)
                        ++rt;
                }

                return rt == 2;
            }
        }

        public bool IsTriple
        {
            get
            {
                var cnts = Counting;
                for(var i=0; i<5; ++i)
                {
                    if (cnts[i] == 3)
                        return true;
                }
                return false;
            }
        }
        public bool IsFourCard
        {
            get
            {
                var cnts = Counting;
                return cnts[0] == 4 || cnts[1] == 4;
            }
        }

        public bool IsFullHouse
        {
            get
            {
                var cnts = Counting;
                var rt = (cnts[0] + cnts[1] == 5);

                return rt;
            }
        }

        public bool IsStraight
        {
            get
            {
                int n1 = _cards[0].Number;

                return n1 + 1 == _cards[1].Number
                    && n1 + 2 == _cards[2].Number
                    && n1 + 3 == _cards[3].Number
                    && n1 + 4 == _cards[4].Number;
            }
        }

        public bool IsFlush
        {
            get
            {
                Card.PatternType pattern = _cards[0].Pattern;

                return pattern == _cards[1].Pattern
                    && pattern == _cards[2].Pattern
                    && pattern == _cards[3].Pattern
                    && pattern == _cards[4].Pattern;
            }
        }
    }
    internal class Chapter02_Prob010_PokerHands
    {
        public static void Run()
        {
            Solve(@"2H 3D 5S 9C KD 2C 3H 4S 8C AH
2H 4S 4C 2D 4H 2S 8S AS QS 3S
2H 3D 5S 9C KD 2C 3H 4S 8C KH
2H 3D 5S 9C KD 2D 3H 5C 9S KH

5S 4D 3D 6C 2C 3H 2S 6H 5D 4S
TC JC QH KS AS JD AH TH KC QC
5C 6C 7D 8D 9S 8C 6D 5H 9C 7S
6S 4D 3D 2H 5H 4H 5D 3S 6D 2C
9C TS 8S 7D JS 7S 9H JH TH 8D
7D 9D JS TH 8C JH 8D TD 7C 9S
7S TD 8D 9S 6C 8C 9C TH 7D 6S
QD JH KC TS 9S KH TD JD QS 9D
8C JC TS 9D 7H 9C 8D 7D JD TC
7C 6S 8H TS 9D TD 9C 8S 7S 6C
4H 3D 5S 2S 6S 4S 5C 6D 3H 2H
TD JH 9C QC 8D JC 9D QS 8H TC
6S 4S 3C 7H 5S 3D 7C 4C 5H 6C
7C 6D 8H TH 9S 6H 9C TC 7D 8S
8H 5C 9C 7C 6S 8S 9D 6H 7H 5D
8S TD QD 9H JS JC TC QS 9D 8H
7C 6H 5S 4C 8S 5H 8D 4H 6S 7H
6D 7C 8H 9S TS 7D 8D TC 6H 9C
9D 8C TH JH 7D 8S JC TS 7C 9C
TC QC 9S KS JH 9C JS TS QS KD
TD QD 8C 9S JS 9H TC JD 8H QC
7H JH 9D TH 8H 7C TS JS 9S 8D
6D 2D 4S 3D 5S 2H 6H 5H 3S 4C
6D 7C TH 9H 8C 9C 7D 8D TC 6C
JH KS 2C 2D JC KH JS JD 2H 2S
9C 7H 6C 8C 5H 7D 9D 6D 5S 8D
6S 5D 7H 4C 3S 3H 7D 6H 4D 5H
8S 9H QS TC JD 8H QC 9D TD JC
3H TS TD 3D QC QS TC 3C 3S TH
3S 6D 5H 4D 7D 3D 6C 5C 7S 4S
7S 5S TS 9S AS AD 7D TD 9D 5D
3H 5C 4S 2H 6C 5D 2D 3S 4H 6S
4C 7S 8H 5D 6D 8S 4S 7D 5H 6C
7D TC 9S 6D 8H TH 6S 8C 7H 9C
QS KD TC 9C JC 9S JH TH QH KS
5S 3C 7H 6D 4C 6C 7S 5D 4S 3D
TC KD 9H KH 9S KC 9D KS TS 9C");
        }

        static void Solve(string s)
        {
            string[] lines = s.Split('\n');

            var W = new PockerHand();
            var B = new PockerHand();
            foreach (string line in lines)
            {
                string[] words = line.TrimEnd().Split();
                if (words.Length < 10)
                    continue;

                B.Set(words[0], words[1], words[2], words[3], words[4]);
                W.Set(words[5], words[6], words[7], words[8], words[9]);

                var rt = B.Cmp(W);

                Console.WriteLine(B);
                Console.Write(W);
                Console.Write(" == >");
                Console.WriteLine(rt > 0 ? "Black Win" : (rt < 0 ? "White Win" : "Tie"));
            }

        }

        static Random rnd = new Random();
        static string[] deck = new string[52];
        static void Reset(string[] a)
        {
            int i = 0;
            //C D H S
            char[] n = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
            char[] p = { 'C', 'D', 'H', 'S' };

            foreach (var pc in p)
            {
                foreach (var nc in n)
                {
                    a[i++] = string.Format($"{nc}{pc}");
                }
            }
        }
        static void Shuffle(string[] a, int cnt = 1000)
        {
            int i = 0;
            int j = 0;

            while (cnt-- > 0)
            {
                i = rnd.Next(a.Length);
                do
                {
                    j = rnd.Next(a.Length);
                } while (i == j);

                string tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
            }
        }

        public static void PrintTestCase(int cnt = 100)
        {
            Reset(deck);
            Shuffle(deck);

            var W = new PockerHand();
            var B = new PockerHand();

            int index = 0;
            for(int i = 0; i < cnt; )
            {
                string line = string.Format($"{deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]} {deck[index++]}");
                string[] words = line.TrimEnd().Split();
                B.Set(words[0], words[1], words[2], words[3], words[4]);
                W.Set(words[5], words[6], words[7], words[8], words[9]);

                int rt = B.Title == PockerHand.TitleType.HighCard ? 1 : B.Cmp(W);
                if (rt == 0)
                {
                    i++;
                    Console.WriteLine(line);
                }


                if (index >= deck.Length - 10)
                {
                    Shuffle(deck);
                    index = 0;
                }    
            }
        }
    }
}
