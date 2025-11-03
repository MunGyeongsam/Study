using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GlobalEnvironment _ge;
    [SerializeField] private float _Kd = 10F;

    Transform _transform;

    bool _isInit = false;
    Vector2 _min = Vector2.zero;
    Vector2 _max = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        Init();
    }

    void Init()
    {
        if (_isInit) return;
        _isInit = true;

        var mc = Camera.main;
        float dy = mc.orthographicSize;
        float dx = dy * mc.aspect;

        _max = _ge.WorldMax - new Vector2(dx, dy);
        _min = -_max;
    }

    // Update is called once per frame
    void Update()
    {
        Trace(Time.deltaTime);
    }

    void Trace(float dt)
    {
        var pos = _transform.position;
        var targ = _target.position;

        float x = Mathf.Clamp(targ.x, _min.x, _max.x);
        float y = Mathf.Clamp(targ.y, _min.y, _max.y);

        pos.x = pos.x + (x - pos.x) * _Kd * dt;
        pos.y = pos.y + (y - pos.y) * _Kd * dt;

        _transform.position = pos;
    }
}
