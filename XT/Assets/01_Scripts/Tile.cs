using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private GameObject _block;

    public void SetBlock(bool val)
    {
        _block.SetActive(val);
    }
    public void Init(Color c, bool isBlock)
    {
        _sr.color = c;
        SetBlock(isBlock);
    }
}