using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] float speed;

    List<Node> _path;
    int _curr;

    Vector2 _pt0 = Vector2.zero;
    Vector2 _pt1 = Vector2.zero;
    Vector2 _vel = Vector2.zero;
    bool _move = false;

    float _accum = 0F;

    Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        //_move = false;
        //_accum = 0F;
    }

    private void FixedUpdate()
    {
        if (!_move)
            return;

        _accum += Time.fixedDeltaTime;

        Vector2 pos = _pt0 + _vel * _accum;
        Vector2 pos2pt1 = _pt1 - pos;

        if (Vector2.Dot(_vel, pos2pt1) < 0F)
        {
            ++_curr;
            if (_curr >= _path.Count)
            {
                pos = _pt1;
                _move = false;
                
                SetPath(_path);
            }
            else
            {
                _accum = ((pos-_pt0).magnitude - (_pt1 - _pt0).magnitude) / speed;
                _pt0 = _pt1;
                _pt1 = PathFinder.ToPos(_path[_curr]);
                _vel = (_pt1 - _pt0).normalized * speed;

                pos = _pt0 + _vel * _accum;
            }
        }

        _transform.position = pos;
    }

    public void SetPath(List<Node> path)
    {
        enabled = true;
        _path = path;
        _curr = 1;
        _accum = 0F;

        if (path.Count == 0)
            return;

        _pt0 = PathFinder.ToPos(path[0]);
        _move = _path.Count > 0;

        if (_move)
        {
            _pt1 = PathFinder.ToPos(path[1]);
            _vel = (_pt1 - _pt0).normalized * speed;
        }
    }
}