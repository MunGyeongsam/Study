using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    enum Stone : byte
    {
        None,
        Black,
        White
    };

    struct Index
    {
        public int row;
        public int col;
    };

    public GameObject prefabStoneBlack;
    public GameObject prefabStoneWhite;

    Transform _black;
    Transform _white;
    Transform _curStone;
    int _layerMask;         //for picking
    float _maxDist;         //for picking
    float _stoneY;
    bool _isGameOver = false;

    Stone[,] _map = new Stone[19, 19];
    List<GameObject> _stones = new List<GameObject>();

    Camera _mainCamera;
    UI _ui;

    int CountN(Index iPos, Stone stone)
    {
        int rt = 0;

        for (int r = iPos.row + 1, c = iPos.col; r < 19; ++r)
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

        for (int r = iPos.row - 1, c = iPos.col; r >= 0; --r)
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

        for (int r = iPos.row, c = iPos.col+1; c < 19; ++c)
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

        for (int r = iPos.row, c = iPos.col - 1; c >= 0; --c)
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

        for (int r = iPos.row+1, c = iPos.col + 1; c < 19 && r < 19; ++r, ++c)
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

        for (int r = iPos.row - 1, c = iPos.col + 1; c < 19 && r >= 0; --r, ++c)
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

        for (int r = iPos.row + 1, c = iPos.col - 1; c >= 0 && r < 19; ++r, --c)
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

        for (int r = iPos.row - 1, c = iPos.col - 1; c >= 0 && r >= 0; --r, --c)
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
    public void Reset()
    {
        System.Array.Clear(_map, 0, _map.Length);

        foreach (GameObject s in _stones)
            Destroy(s);
        _stones.Clear();

        _curStone.gameObject.SetActive(false);
        _curStone = _black;
        _curStone.gameObject.SetActive(true);

        _isGameOver = false;
    }

    Index ToIndex(Vector3 pos)
    {
        Index rt = new Index();

        float x = Mathf.Clamp(pos.x + 9F, 0F, 18F);
        float z = Mathf.Clamp(pos.z + 9F, 0F, 18F);

        rt.col = (int)(x + 0.5F);
        rt.row = (int)(z + 0.5F);

        return rt;
    }

    Vector3 ToVector3(Index index)
    {
        return new Vector3(index.col - 9F, _stoneY, index.row - 9F);
    }

    string ToString(Index index)
    {
        char row = (char)('S' - index.row);
        string col = (index.col+1).ToString();
        Stone s = _map[index.row, index.col];

        return string.Format("[{0},{1}] {2}", row, col, s);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(null != prefabStoneBlack);
        Debug.Assert(null != prefabStoneWhite);

        _ui = GameObject.FindObjectOfType<UI>();
        Debug.Assert(null != _ui);

        _black = Instantiate(prefabStoneBlack).transform;
        _white = Instantiate(prefabStoneWhite).transform;

        InitStone(_black);
        InitStone(_white);

        _curStone = _black;
        _stoneY = _curStone.position.y;
        _curStone.gameObject.SetActive(true);

        _mainCamera = Camera.main;
        _layerMask = 1 << LayerMask.NameToLayer("Floor");
        _maxDist = _mainCamera.farClipPlane;
    }

    void InitStone(Transform t)
    {
        t.gameObject.SetActive(false);
        Material mat = t.GetComponent<Renderer>().material;
        Color c = mat.color;
        c.a = 0.7F;
        mat.color = c;

        t.localScale *= 1.2F;
    }

    Vector3 Snap(Vector3 pt)
    {
        float x = pt.x;
        float z = pt.z;

        pt.x = (int)(x + ((x < 0F) ? -0.5F : 0.5F));
        pt.z = (int)(z + ((z < 0F) ? -0.5F : 0.5F));
        return pt;
    }
    bool PickingFloor()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, _maxDist, _layerMask))
        {
            Vector3 pos = hit.point;
            pos.y = _curStone.position.y;
            _curStone.position = Snap(pos);
            return true;
        }

        return false;
    }

    bool AddStone(Vector3 pos)
    {
        Index index = ToIndex(pos);
        if (_map[index.row, index.col] == Stone.None)
        {
            bool isBlack = (_curStone == _black);
            _map[index.row, index.col] = isBlack ? Stone.Black : Stone.White;

            GameObject prefab = isBlack ? prefabStoneBlack : prefabStoneWhite;
            _stones.Add(Instantiate(prefab, pos, Quaternion.identity));

            _isGameOver = IsFiveStone(index, isBlack ? Stone.Black : Stone.White);

            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver && PickingFloor())
        {
            Vector3 pos = _curStone.transform.position;
            if (Input.GetMouseButtonUp(0) && AddStone(pos))
            {
                if (_isGameOver)
                {
                    _ui.SetDbgText(string.Format("{0} Win", ToString(ToIndex(pos))));
                    _curStone.gameObject.SetActive(false);
                }
                else
                {
                    Transform newStone = (_curStone == _black) ? _white : _black;
                    _curStone.gameObject.SetActive(false);
                    _curStone = newStone;
                    _curStone.position = pos;
                    _curStone.gameObject.SetActive(true);
                }
            }
        }
    }
}
