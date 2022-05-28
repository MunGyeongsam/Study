using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Index = OmokLogic.Index;

public class OmokBoard : MonoBehaviour
{
    public GameObject prefabStoneBlack;
    public GameObject prefabStoneWhite;

    Transform _black;
    Transform _white;
    Transform _curStone;
    int _layerMask;         //for picking
    float _maxDist;         //for picking
    float _stoneY;
    Index _iPos;

    OmokLogic _logic = new OmokLogic();
    List<GameObject> _stones = new List<GameObject>();

    UI _ui;
    Camera _mainCamera;

    bool IsBlackTurn { get { return _curStone == _black; } }
    bool IsGameOver { get { return _logic.IsGameOver; } }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(null != prefabStoneBlack);
        Debug.Assert(null != prefabStoneWhite);

        _mainCamera = Camera.main;
        _ui = GameObject.FindObjectOfType<UI>();
        Debug.Assert(null != _ui);

        _layerMask = 1 << LayerMask.NameToLayer("Floor");
        _maxDist = _mainCamera.farClipPlane;

        _black = Instantiate(prefabStoneBlack).transform;
        _white = Instantiate(prefabStoneWhite).transform;

        InitStone(_black);
        InitStone(_white);

        _curStone = _black;
        _stoneY = _curStone.position.y;
        _curStone.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver || !Picking())
            return;

        if (!_logic.IsValid(_iPos, IsBlackTurn))
            return;

        Vector3 pos = ToVector3(_iPos);
        _curStone.position = pos;

        if (Input.GetMouseButtonUp(0))
        {
            AddStone(pos);
            _curStone.gameObject.SetActive(false);

            if (IsGameOver)
            {
                _ui.SetDbgText(string.Format("{0} Win!", _logic.ToString(_iPos)));
            }
            else
            {
                TurnOver();
            }
        }
    }

    void TurnOver()
    {
        Transform newStone = IsBlackTurn ? _white : _black;
        newStone.position = _curStone.position;

        _curStone = newStone;
        _curStone.gameObject.SetActive(true);
    }

    void AddStone(Vector3 pos)
    {
        GameObject prefab = IsBlackTurn ? prefabStoneBlack : prefabStoneWhite;
        _stones.Add(Instantiate(prefab, pos, Quaternion.identity));
        _logic.SetData(_iPos, IsBlackTurn);
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

    public void Reset()
    {
        _logic.Reset();
        foreach (GameObject s in _stones)
            Destroy(s);
        _stones.Clear();

        _curStone.gameObject.SetActive(false);
        _curStone = _black;
        _curStone.gameObject.SetActive(true);
    }

    Index ToIndex(Vector3 pos)
    {
        Index rt = new Index();

        float x = Mathf.Clamp(pos.x + 9F, 0F, 18F);
        float z = Mathf.Clamp(pos.z + 9F, 0F, 18F);

        rt.ix = (int)(x + 0.5F);
        rt.iz = (int)(z + 0.5F);

        return rt;
    }

    Vector3 ToVector3(Index index)
    {
        return new Vector3(index.ix - 9F, _stoneY, index.iz - 9F);
    }

    bool Picking()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _maxDist, _layerMask))
        {
            _iPos = ToIndex(hit.point);
            return true;
        }

        return false;
    }
}
