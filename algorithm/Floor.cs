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

    Stone[,] _map = new Stone[19, 19];
    List<GameObject> _stones = new List<GameObject>();

    Camera _mainCamera;

    public void Reset()
    {
        for (int i = 0; i < _map.GetLength(0); ++i)
            for (int j = 0; j < _map.GetLength(1); ++j)
                _map[i, j] = Stone.None;

        foreach (var s in _stones)
            Destroy(s);

        _curStone.gameObject.SetActive(false);
        _curStone = _black;
        _curStone.gameObject.SetActive(true);
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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(null != prefabStoneBlack);
        Debug.Assert(null != prefabStoneWhite);

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
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PickingFloor())
        {
            Vector3 pos = _curStone.transform.position;
            if (Input.GetMouseButtonUp(0) && AddStone(pos))
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
