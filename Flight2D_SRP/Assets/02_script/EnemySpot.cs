using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpot : MonoBehaviour
{
    [SerializeField] Transform _t1;
    [SerializeField, Range(0.2F, 10F)] float _freq = 1F;

    [SerializeField] string[] _poolNames;

    private float _accum = 0F;

    private void FixedUpdate()
    {
        _accum += Time.fixedDeltaTime;

        while(_accum >= _freq)
        {
            Spawn();

            _accum -= _freq;
        }
    }

    // Update is called once per frame
    void Spawn()
    {
        int i = Random.Range(0, _poolNames.Length);

        var e = ObjectPoolManager.Get(_poolNames[i]);

        float s = Random.Range(0.2F, 1.2F);
        float x = Random.Range(GlobalEnvironment.Instance.WorldMin.x, GlobalEnvironment.Instance.WorldMax.x);
        (e as Enemy).Reset(x, s);
    }
}
