using System.Diagnostics;

public class OmokLogic
{
    //표준 룰
    //https://m.blog.naver.com/dod000111/221796661958

    public struct Index
    {
        public int ix;
        public int iz;
    };

    enum Stone : byte
    {
        None,
        Black,
        White
    };

    const int SIZE = 19;
    Stone[,] _map = new Stone[SIZE, SIZE];
    bool _isGameOver = false;


    public bool IsGameOver { get { return _isGameOver; } }
    public void Reset()
    {
        System.Array.Clear(_map, 0, _map.Length);
        _isGameOver = false;
    }

    public bool IsValid(Index iPos, bool isBlack)
    {
        return _map[iPos.ix, iPos.iz] == Stone.None 
            && !Is3x3(iPos, isBlack?Stone.Black:Stone.White);
    }

    public void SetData(Index iPos, bool isBlack)
    {
        Debug.Assert(!_isGameOver);
        Debug.Assert(_map[iPos.ix, iPos.iz] == Stone.None);

        Stone stone = isBlack ? Stone.Black : Stone.White;
        _map[iPos.ix, iPos.iz] = stone;

        _isGameOver = IsFiveStone(iPos, stone);
    }

    public string ToString(Index iPos)
    {
        char row = (char)('S' - iPos.iz);
        string col = (iPos.ix + 1).ToString();
        Stone s = _map[iPos.ix, iPos.iz];

        return string.Format("[{0},{1}] {2}", row, col, s);
    }


    int CountN(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix + 1, c = iPos.iz; r < 19; ++r)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }

    int CountS(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix - 1, c = iPos.iz; r >= 0; --r)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }
    int CountE(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix, c = iPos.iz + 1; c < 19; ++c)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }
    int CountW(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix, c = iPos.iz - 1; c >= 0; --c)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }
    int CountNe(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix + 1, c = iPos.iz + 1; c < 19 && r < 19; ++r, ++c)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }
    int CountSe(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix - 1, c = iPos.iz + 1; c < 19 && r >= 0; --r, ++c)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }
    int CountNw(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix + 1, c = iPos.iz - 1; c >= 0 && r < 19; ++r, --c)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }
    int CountSw(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.ix - 1, c = iPos.iz - 1; c >= 0 && r >= 0; --r, --c)
        {
            if (_map[r, c] != stone)
                break;
            ++rt;
        }

        return rt;
    }

    bool IsFiveStone(Index ipos, Stone stone)
    {
        int cnt = 0;
        //horizontal
        cnt = CountE(ipos, stone) + CountW(ipos, stone) + 1;
        if (cnt == 5)
            return true;
        //vertical
        cnt = CountN(ipos, stone) + CountS(ipos, stone) + 1;
        if (cnt == 5)
            return true;
        //diagonal
        cnt = CountNe(ipos, stone) + CountSw(ipos, stone) + 1;
        if (cnt == 5)
            return true;
        cnt = CountNw(ipos, stone) + CountSe(ipos, stone) + 1;
        if (cnt == 5)
            return true;

        return false;
    }


    bool Is3x3(Index ipos, Stone stone)
    {
        //todo : 수정 필요
        int cnt = 0;
        if (3 == (CountE(ipos, stone) + CountW(ipos, stone) + 1))
            ++cnt;
        if (3 == (CountN(ipos, stone) + CountS(ipos, stone) + 1))
            ++cnt;
        if (3 == (CountNe(ipos, stone) + CountSw(ipos, stone) + 1))
            ++cnt;
        if (3 == (CountNw(ipos, stone) + CountSe(ipos, stone) + 1))
            ++cnt;

        return cnt >= 2;
    }
}
