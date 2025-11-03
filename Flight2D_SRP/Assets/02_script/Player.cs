using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D = null!;
    [SerializeField] private float _speed = 5F;
    [SerializeField] private GlobalEnvironment _ge;

    Camera _mainCamera = null;
    Transform _transform = null;
    Vector2 _velocity = Vector2.zero;
    Vector2 _min = Vector2.zero;
    Vector2 _max = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _transform = transform;

        _max = _ge.WorldMax;
        _min = -_max;
    }

    private void FixedUpdate()
    {
        var vel = _rigidbody2D.velocity;
        _rigidbody2D.velocity = vel + (_velocity - vel) * Time.fixedDeltaTime * 10F;

    }
    // Update is called once per frame
    void Update()
    {
        if (!ControllByMouse())
            ControllByKeyboard();


        Clamp();
    }

    void Clamp()
    {
        Vector3 pos = _transform.position;

        pos.x = Mathf.Clamp(pos.x, _min.x, _max.x);
        pos.y = Mathf.Clamp(pos.y, _min.y, _max.y);

        _transform.position = pos;
    }

    bool ControllByMouse()
    {
        if (!Input.GetMouseButton(0))
        { 
            _velocity = Vector2.zero;
            return false;
        }

        var cursor = Input.mousePosition;
        Vector3 ptInWorld = _mainCamera.ScreenToWorldPoint(cursor);

        _velocity = (ptInWorld - _transform.position)*_speed;

        return true;
    }
    bool ControllByKeyboard()
    {
        bool l = Input.GetKey(KeyCode.LeftArrow);
        bool r = Input.GetKey(KeyCode.RightArrow);
        bool u = Input.GetKey(KeyCode.UpArrow);
        bool d = Input.GetKey(KeyCode.DownArrow);

        var dir = new Vector2(
            l ? -1F : (r ? 1F : 0F),
            d ? -1F : (u ? 1F : 0F)
            );
        _velocity = dir.normalized * _speed;

        return (l || r || u || d);
    }
}
