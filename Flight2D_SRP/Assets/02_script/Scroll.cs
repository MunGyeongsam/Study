using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] BgScale _bgScale;

    private Transform _transform;
    private float _y = 0F;

    public void Init(float y)
    {
        _y = y;

        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    public void Move(float dy)
    {
        _y += dy;

        if (_y <= -_bgScale.Height)
        {
            _y += _bgScale.Height * 2F;
        }

        Vector3 pos = _transform.position;
        pos.y = _y;
        _transform.position = pos;
    }
}
