using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : PoolElement
{
    Rigidbody2D _rigidbody2D;
    Transform _transform;
    Vector2 _halfSize;
    int _Hp = 100;

    protected override void OnCreateImpl()
    { 
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
        transform.rotation = Quaternion.Euler(0F, 0F, 180F);
        _rigidbody2D.velocity = transform.up * Random.Range(3F, 10F);

        _halfSize = GetComponent<SpriteRenderer>().size * 0.5F;
    }
    public void Reset(float x, float scale)
    {
        gameObject.SetActive(true);
        var wmax = GlobalEnvironment.Instance.WorldMax;
        var wmin = GlobalEnvironment.Instance.WorldMin;

        var pos = _transform.position;
        pos.x = x;
        pos.y = wmax.y + scale * _halfSize.y;

        _transform.localScale = Vector3.one * scale;
        _transform.position = pos;
        _rigidbody2D.velocity = transform.up * Random.Range(3F, 10F);

        _Hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = _transform.position;

        if (pos.y + _halfSize.y < GlobalEnvironment.Instance.WorldMin.y)
        {
            Release();
        }
    }

    protected override void OnGetImpl()
    {
        base.OnGetImpl();

        GuideBulletManager.Add(this);
    }

    protected override void OnReleaseImpl()
    {
        base.OnReleaseImpl();

        var eff = ObjectPoolManager.Get("BoomFlight");
        (eff as Effect).Reset(_transform);

        GuideBulletManager.Rem(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            collision.GetComponent<Bullet>().Release();
            var eff = ObjectPoolManager.Get("BoomBullet") as Effect;

            eff.Reset(collision.transform.position, 1F);

            _Hp -= 10;
            if (_Hp <= 0)
            {
                Release();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            Release();
            GlobalEnvironment.Instance.OnTakeDamage();
        }
    }
}
