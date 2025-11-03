using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScale : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _srndr;

    private float _height = 0F;
    public float Height { get { return _height; } }

    // Start is called before the first frame update
    public void Init()
    {
        var ge = GlobalEnvironment.Instance;
        var worldMax = ge.WorldMax * 2F;

        var sz = _srndr.size;
        float sx = worldMax.x / sz.x;
        float sy = worldMax.y / sz.y;

        _height = sz.y * sy;

        transform.localScale = new Vector2(sx, sy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
