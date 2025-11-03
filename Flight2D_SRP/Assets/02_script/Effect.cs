using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PoolElement
{
    [SerializeField] Sprite[] _sprites;

    SpriteRenderer _spriteRenderer;
    Transform _transform;

    int _index = 0;
    float _accum = 0F;
    [SerializeField] float _step = 1F / 30F;

    protected override void OnCreateImpl()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    private void Update()
    {
        _accum += Time.deltaTime;

        while(_accum >= _step)
        {
            _accum -= _step;
            ++_index;

            if (_index >= _sprites.Length)
            {
                Release();
                return;
            }
        }

        _spriteRenderer.sprite = _sprites[_index];
    }

    public void Reset(Transform t)
    {
        _index = 0;
        _spriteRenderer.sprite = _sprites[_index];

        _transform.position = t.position;
        _transform.localScale = t.localScale;

        _transform.rotation = Quaternion.Euler(0F, 0F, Random.Range(0F, 360F));
    }

    public void Reset(Vector3 pos, float s = 1F)
    {
        _index = 0;
        _spriteRenderer.sprite = _sprites[_index];

        _spriteRenderer.sprite = _sprites[_index];
        _transform.position = pos;
        _transform.localScale = new Vector3(s, s, s);

        _transform.rotation = Quaternion.Euler(0F, 0F, Random.Range(0F, 360F));
    }
}
