using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField, Range(3F, 30F)] float _bulletSpeed = 20F;
    [SerializeField, Range(1f/60F, 10F)] float _freq = 1F;
    [SerializeField] bool isGuide = false;

    private float _accum = 0F;

    private void FixedUpdate()
    {
        _accum += Time.fixedDeltaTime;

        while (_accum >= _freq)
        {
            Shoot();

            _accum -= _freq;
        }
    }
    void Shoot()
    {
        var e = ObjectPoolManager.Get("Bullet");

        int target = (isGuide) ? GuideBulletManager.FindTarget(transform.position) : int.MinValue;

        (e as Bullet).Reset(transform, _bulletSpeed, target);
    }
}
