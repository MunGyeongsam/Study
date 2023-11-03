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
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
        _screenHeight = _camera!.orthographicSize * 2F;
        _screenWidth = _screenHeight * _camera.aspect;
        _tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        
        float mh = hInTiles * _tileSize;
        float mw = wInTiles * _tileSize;

        float sx = _screenWidth / mw;
        float sy = _screenHeight / mh;
        float s = MathF.Min(sx, sy);

        _scaledTileSize = _tileSize * s;
        
        mh *= s;
        mw *= s;

        float offset = _scaledTileSize * 0.5F;

        _max = new(mw * 0.5F - offset, mh * 0.5F - offset);
        _min = -1F * _max;
        
        if (null != posPrefab)
        {
            var posTile = Instantiate(posPrefab
                , Vector3.zero
                , Quaternion.identity
                , this.transform);

            _posTile = posTile.transform;
            _posTile.localScale *= s;
        }

        BuildMap(s);

        _max.x -= offset;
        _max.y -= offset;
        _min.x += offset;
        _min.y += offset;
    }

    void BuildMap(float s)
    {
        Transform parent = this.transform;
        Quaternion rot = Quaternion.identity;
        Vector3 pos = new (_min.x, _min.y, 0F);
        Vector3 sv = new(s, s, s);

        for (int i = 0; i < hInTiles; ++i)
        {
            bool b = ((i & 0x1) == 1);
            pos.x = _min.x;
            for (int j = 0; j < wInTiles; ++j)
            {
                var g = Instantiate(tilePrefab, pos, rot, parent);
                g.transform.localScale = sv;
                g.name = string.Format($"tile {i}, {j}");
                g.Init(b);
                b = !b;
                pos.x += _scaledTileSize;
            }

            pos.y += _scaledTileSize;
        }
    }

    void UpdatePosTile()
    {
        if (!_posTile)
            return;
        
        Vector2 p = _camera.ScreenToWorldPoint(
            Input.mousePosition);

        //*
        p.x = Math.Clamp(p.x, _min.x, _max.x);
        p.y = Math.Clamp(p.y, _min.y, _max.y);
        
        Vector2 dist = p - _min;
        dist.x += _scaledTileSize * 0.5F;
        dist.y += _scaledTileSize * 0.5F;
        
        int rx = (int)(dist.x / _scaledTileSize);
        int ry = (int)(dist.y / _scaledTileSize);
        _posTile.position = _min + new Vector2(rx*_scaledTileSize, ry*_scaledTileSize);
        //*/
        
        //_posTile.position = p;
        
        
        /*
        if (!_posTile)
            return;
        
        Vector2 p = _camera.ScreenToWorldPoint(
            Input.mousePosition);

        //p.x += _scaledTileSize * .5F;
        //p.y += _scaledTileSize * .5F;

        p.x = Math.Clamp(p.x, _min.x, _max.x);
        p.y = Math.Clamp(p.y, _min.y, _max.y);
        Vector2 dist = p - _min;
        dist.x += _scaledTileSize * 0.5F;
        dist.y += _scaledTileSize * 0.5F;

        int rx = (int)(dist.x / _scaledTileSize);
        int ry = (int)(dist.y / _scaledTileSize);
        
        _posTile.position = _min + new Vector2(rx*_scaledTileSize, ry*_scaledTileSize);
        //*/
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosTile();
    }
}
