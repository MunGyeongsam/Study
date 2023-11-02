using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int wInTiles;
    [SerializeField] private int hInTiles;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private GameObject posPrefab;

    private float _screenWidth;
    private float _screenHeight;
    private float _tileSize;
    private Vector2 _min;
    private Vector2 _max;
    private float _scaledTileSize;
    private Transform _posTile;
    private Camera _camera;
    
    Vector3 startVertex;
    Vector3 mousePos;
    
    //private bool is
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
        _screenHeight = _camera!.orthographicSize * 2F;
        _screenWidth = _screenHeight * _camera.aspect;
        _tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        
        if (null != posPrefab)
        {
            var posTile = Instantiate(posPrefab
                , Vector3.zero
                , Quaternion.identity
                , this.transform);

            _posTile = posTile.transform;
        }

        BuildMap();
    }

    void BuildMap()
    {
        Transform parent = this.transform;
        Quaternion rot = Quaternion.identity;
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < hInTiles; ++i)
        {
            bool b = ((i & 0x1) == 1);
            pos.x = 0F;
            for (int j = 0; j < wInTiles; ++j)
            {
                var g = Instantiate(tilePrefab, pos, rot, parent);
                g.name = string.Format($"tile {i}, {j}");
                g.Init(b);
                b = !b;
                pos.x += _tileSize;
            }

            pos.y += _tileSize;
        }

        float mh = hInTiles * _tileSize;
        float mw = wInTiles * _tileSize;

        float sx = _screenWidth / mw;
        float sy = _screenHeight / mh;
        float s = MathF.Min(sx, sy);
        parent.localScale = new Vector3(s,s,s);

        _scaledTileSize = _tileSize * s;

        float minx = _tileSize * s * -0.5F;
        float maxx = _tileSize * s * wInTiles + minx;
        float miny = _tileSize * s * -0.5F;
        float maxy = _tileSize * s * hInTiles + miny;

        _min = new(minx + _scaledTileSize, miny + _scaledTileSize);
        _max = new(maxx - _scaledTileSize, maxy - _scaledTileSize);
        
        parent.localPosition = new Vector3((minx + maxx) * -.5F, (miny + maxy) * -.5F, 0F);

        _min += (Vector2) parent.localPosition;
        _max += (Vector2) parent.localPosition;
    }

    void UpdatePosTile()
    {
        if (!_posTile)
            return;
        
        Vector2 p = _camera.ScreenToWorldPoint(
            Input.mousePosition);

        //*
        //p.x += _scaledTileSize * .5F;
        //p.y += _scaledTileSize * .5F;

        p.x = Math.Clamp(p.x, _min.x, _max.x);
        p.y = Math.Clamp(p.y, _min.y, _max.y);
        Vector2 dist = p - _min;
        dist.x += _scaledTileSize * 0.5F;
        dist.y += _scaledTileSize * 0.5F;

        int rx = (int)(dist.x / _scaledTileSize);
        int ry = (int)(dist.y / _scaledTileSize);
        //*/
        
        _posTile.position = _min + new Vector2(rx*_scaledTileSize, ry*_scaledTileSize);
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosTile();
    }

    private void OnPostRender()
    {
        
    }
}
