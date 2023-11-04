using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _1, _2;
    [SerializeField] private SpriteRenderer sr;

    public void Init(bool c)
    {
        sr.color = c ? _1 : _2;
    }

    public void Setcolor(Color c)
    {
        sr.color = c;
    }
}
