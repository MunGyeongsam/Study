using _02_script.MVC.Controller;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : PoolElement
{
    [SerializeField] Slider _lifeBar;
    
    Rigidbody2D _rigidbody2D;
    Transform _transform;
    Vector2 _halfSize;
    int _Hp = 100;
    float _maxHp = 100F;
    float _hpYScale = 1F;

    protected override void OnCreateImpl()
    { 
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
        transform.rotation = Quaternion.Euler(0F, 0F, 180F);
        _rigidbody2D.velocity = transform.up * Random.Range(3F, 10F);

        _halfSize = GetComponent<SpriteRenderer>().size * 0.5F;
        _hpYScale = _lifeBar.transform.localScale.y;
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

        _maxHp = Mathf.Ceil(100F * scale);
        _Hp = (int)_maxHp;
        _lifeBar.value = 1F;

        var hpScale = _lifeBar.transform.localScale;
        hpScale.y = _hpYScale / scale;
        _lifeBar.transform.localScale = hpScale;
    }

    // Update is called once per frame
    void Update()
    {
        GlobalEnvironment.Instance.ControlPhysics(_rigidbody2D);
        
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

            _Hp -= 20;
            _lifeBar.value = _Hp / _maxHp;
            if (_Hp <= 0)
            {
                //GlobalEnvironment.Instance.UI.AddScore(_maxHp);
                
                ScoreController.Instance.AddScore(_maxHp);
                Release();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            //GlobalEnvironment.Instance.UI.AddScore(_maxHp);
            ScoreController.Instance.AddScore(_maxHp);
            
            collision.GetComponent<Player>().TakeDamage(_Hp);
            Release();
        }
    }
}
