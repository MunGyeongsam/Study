using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    Color _initColor;
    public void SetColor(Color c)
    {
        _sr.color = c;
    }
    public void ResetColor()
    {
        _sr.color = _initColor;
    }
    public void InitColor(Color c)
    {
        _initColor = c;
        _sr.color = c;
    }
}