using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _1, _2;
    [SerializeField] private SpriteRenderer sr;

    private Color _orgColor;

    public void Init(bool c)
    {
        _orgColor = c ? _1 : _2;
        sr.color = _orgColor;
    }

    public void Setcolor(Color c)
    {
        sr.color = c;
    }

    public void ResetColor()
    {
        sr.color = _orgColor;
    }
}
