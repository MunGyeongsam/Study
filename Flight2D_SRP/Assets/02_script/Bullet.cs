using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolElement
{
    [SerializeField] float _LifeSpan = 5F;


    float _accum = 0F;
    int _targetId = int.MinValue;

    Rigidbody2D _rigidbody2D;
    Transform _transform;

    float _speed = 0F;
    Vector2 _vel = Vector2.zero;

    private void FixedUpdate()
    {
        if (_targetId == int.MinValue)
            return;

        var(found, pos) = GuideBulletManager.TargetPosition(_targetId);

        if (!found)
        {
            _targetId = int.MinValue;
            return;
        }

        var dir = (pos - (Vector2)_transform.position).normalized;
        var tarRot = Toward(dir);
        var curRot = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(curRot, tarRot, 6F);

        _rigidbody2D.velocity = _transform.up * _speed;
    }

    private Quaternion Toward(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.back);
    }

    public void Reset(Transform t, float speed, int targetId = int.MinValue)
    {
        _targetId = targetId;

        _transform.position = t.position;
        _transform.rotation = t.rotation;

        _speed = speed;
        _rigidbody2D.velocity = _transform.up * speed;

        _accum = 0F;
    }

    protected override void OnCreateImpl()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    private void Update()
    {
        _accum += Time.deltaTime;

        if (_accum > _LifeSpan)
        {
            Release();
        }
    }
}
