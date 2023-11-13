using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject prefabTile2x2;
    [SerializeField] private GameObject prefabTile;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    [SerializeField] private int hInTiles;
    [SerializeField] private int wInTiles;

    Camera _mainCamera;
    Transform _cursor;
    Vector2 _min;
    Vector2 _max;
    Vector2 _leftBottom;
    Vector2 _leftTop;
    float _scaledTileSize;

    char[,] _map;
    bool[,] _mapProp;
    Tile[,] _mapTile;


    // Start is called before the first frame update
    void Start()
    {
        ReadMap("/04_Maps/map01.txt");

        var c = Camera.main;
        _mainCamera = c;

        //size of screen in world-space
        float sh = c.orthographicSize * 2F;
        float sw = sh * c.aspect;

        //size of tile in world-space without scale
        SpriteRenderer sr = prefabTile.GetComponent<SpriteRenderer>();
        float ts = sr.bounds.size.x;

        //size of map in world-space without scale
        float mw = ts * wInTiles;
        float mh = ts * hInTiles;

        //calculate scale
        float sx = sw / mw;
        float sy = sh / mh;
        float s = Mathf.Min(sx, sy);

        _scaledTileSize = s * ts;
        _leftBottom = new Vector2(mw * 0.5F, mh * 0.5F) * -s;
        _leftTop = new Vector2(mw * 0.5F * -s, mh * 0.5F * s);

        _min = _leftBottom + new Vector2(_scaledTileSize * 2F, _scaledTileSize * 2F);
        _max = -_min;

        _leftBottom.x += _scaledTileSize;
        _leftBottom.y += _scaledTileSize;

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        Transform parent = this.transform;

        var tmp = Instantiate(prefabTile2x2, pos, rot, parent);
        _cursor = tmp.transform;

        for (int i=0; i<hInTiles; ++i)
        {
            bool c1 = (i & 0x1) == 1;
            pos.x = 0F;
            for(int j=0; j<wInTiles; ++j)
            {
                var g=Instantiate(prefabTile, pos, rot, parent);
                var t = g.GetComponent<Tile>();

                _mapTile[i, j] = t;

                t.name = string.Format($"tile {i,2} x {j,2}");

                t.InitColor(c1 ? color1 : color2);
                char ch = _map[i, j];
                if (ch != '0')
                    t.SetColor(Char2Color(ch));

                pos.x += ts;

                c1 = !c1;
            }

            pos.y -= ts;
        }

        float dx = mw * s * 0.5F - ts * 0.5F * s;
        float dy = mh * s * 0.5F - ts * 0.5F * s;
        parent.position = new Vector3(-dx, dy, 0F);
        parent.localScale *= s;

        _cursor.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pt = MousePosInWorldSpace();
        UpdateCursor(pt);

        if (Input.GetMouseButtonDown(0))
        {
            SetProp(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SetProp(false);
        }
    }

    Vector2 MousePosInWorldSpace()
    {
        Vector2 pt = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pt.x = Mathf.Clamp(pt.x, _min.x, _max.x) + _scaledTileSize * 0.5F;
        pt.y = Mathf.Clamp(pt.y, _min.y, _max.y) + _scaledTileSize * 0.5F;
        return pt;
    }

    void UpdateCursor(Vector2 pt)
    {
        Vector2 dist = pt - _leftBottom;

        float dx = (int)(dist.x / _scaledTileSize) * _scaledTileSize;
        float dy = (int)(dist.y / _scaledTileSize) * _scaledTileSize;

        pt = _leftBottom + new Vector2(dx, dy);

        _cursor.position = pt;
    }

    void ReadMap(string path)
    {
        string fullPath = Application.dataPath + path;
        var lines = System.IO.File.ReadAllLines(fullPath);

        var words = lines[0].Trim().Split();

        wInTiles = System.Convert.ToInt32(words[0]);
        hInTiles = System.Convert.ToInt32(words[1]);

        _map = new char[hInTiles, wInTiles];
        _mapProp = new bool[hInTiles, wInTiles];
        _mapTile = new Tile[hInTiles, wInTiles];

        for (int r=0; r<hInTiles; ++r)
        {
            var line = lines[r + 1].Trim();
            for (int c=0; c<wInTiles; ++c)
            {
                char ch = line[c * 2];
                _map[r, c] = ch;
                _mapProp[r, c] = ch != '1';
            }
        }
    }

    Color Char2Color(char c)
    {
        switch (c)
        {
            case '1':
                return new Color(0.2F, 0.2F, 0.2F);
            case 'F':
            case 'f':
                return new Color(0.2F, 0.8F, 0.2F);
            case 'T':
            case 't':
                return new Color(0.8F, 0.2F, 0.2F);
        }

        return new Color(1F, 1F, 1F);
    }

    void SetProp(bool val)
    {
        Vector2 pt = _cursor.position;

        float dx = pt.x - _leftTop.x + _scaledTileSize * 0.5F;
        float dy = _leftTop.y - pt.y;

        int r = (int)(dy / _scaledTileSize);
        int c = (int)(dx / _scaledTileSize);


        if (val)
        {
            var clr = new Color(0.4F, 0.4F, 0.4F);
            _mapTile[r - 1, c - 1].SetColor(clr); _mapTile[r - 1, c - 0].SetColor(clr);
            _mapTile[r - 0, c - 1].SetColor(clr); _mapTile[r - 0, c - 0].SetColor(clr);
        }
        else
        {
            _mapTile[r - 1, c - 1].ResetColor(); _mapTile[r - 1, c - 0].ResetColor();
            _mapTile[r - 0, c - 1].ResetColor(); _mapTile[r - 0, c - 0].ResetColor();
        }
    }
}
