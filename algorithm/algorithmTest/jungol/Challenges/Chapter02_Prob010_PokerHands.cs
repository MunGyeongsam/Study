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
        enum TitleType
        {

        }
        struct Counts
        {
            int Count1;
            int Count2;
            int Count3;
            int Count4;
            int Count5;

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
            return string.Format($"{_cards[0]} {_cards[1]} {_cards[2]} {_cards[3]} {_cards[4]}");
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
                for(int i=1; i<5; ++i)
                {
                    if (n != _cards[i].Number)
                    {
                        index++;
                        count[index] = 1;
                    }
                    else
                    {
                        ++count[index];
                    }
                }

                return count;
            }
        }

        public bool IsFourCard
        {
            get
            {
                int n1 = _cards[0].Number;
                int n2 = _cards[1].Number;

                int cnt = (n1 == n2) ? 2 : 1;
                for(int i=2; i<5; ++i)
                {
                    if (_cards[i].Number == n2)
                        ++cnt;
                    else
                        break;
                }

                return cnt == 4;
            }
        }

        public bool IsFullHouse
        {
            get
            {
                return false;
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
2H 3D 5S 9C KD 2D 3H 5C 9S KH");
        }

        static void Solve(string s)
        {
            string[] lines = s.Split('\n');

            var W = new PockerHand();
            var B = new PockerHand();
            foreach (string line in lines)
            {
                string[] words = line.TrimEnd().Split();
                B.Set(words[0], words[1], words[2], words[3], words[4]);
                W.Set(words[5], words[6], words[7], words[8], words[9]);
            }

        }
    }
}
