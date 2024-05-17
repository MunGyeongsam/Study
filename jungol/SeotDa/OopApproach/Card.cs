using System;
using System.Diagnostics;

namespace SeotDa.OopApproach
{
    class Card
    {
        public enum TYPE
        {
            INVALID = -1,
            LIGHTING,
            TEN,
            BAND,
        }

        public enum MONTH
        {
            INVALID = -1,
            JAN,
            FEB,
            MAR,
            APR,
            MAY,
            JUN,
            JUL,
            AUG,
            SEP,
            OCT,
        }

        static string[] sType =
        {
            "광",    // LIGHTING
            "장",    // TEN
            "띠",    // BAND
        };
        static string[] sMonth =
        {
            " 1월",   // JAN
            " 2월",   // FEB
            " 3월",   // MAR
            " 4월",   // APR
            " 5월",   // MAY
            " 6월",   // JUN
            " 7월",   // JUL
            " 8월",   // AUG
            " 9월",   // SEP
            "10월",   // OCT
        };

        MONTH mMon;
        TYPE mType;

        
        public MONTH Month
        {
            set
            {
                Debug.Assert(value >= MONTH.JAN && value <= MONTH.OCT);
                mMon = value;
            }
        }
        public int nMonth { get { return (int)mMon + 1; } }

        public TYPE Type
        {
            set
            {
                Debug.Assert(value >= TYPE.LIGHTING && value <= TYPE.BAND);
                mType = value;
            }
            get { return mType; }
        }
        
        public override string ToString()
        {
            return string.Format("{0}{1}", sMonth[(int)mMon], sType[(int)mType]);
        }
    }
}
