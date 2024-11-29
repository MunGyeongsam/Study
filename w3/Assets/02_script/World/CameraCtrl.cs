using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField, Range(3F, 30F)]
    float _distance = 5F;

    [SerializeField, Range(1F, 10F)]
    float _zoomSpeed = 5F;
    [SerializeField, Range(0.1F, 2F)]
    float _rotSpeed = 0.5F;

    [SerializeField, Range(1F, 15F)]
    float _kd = 5F;

    [SerializeField, Range(0F, 45F)]
    float _patchMin = 10F;

    [SerializeField, Range(45F, 90F)]
    float _patchMax = 80F;

    float _pitch = 45F;
    float _yaw = 0F;

    Vector3 _mousePt;
    Vector3 _targ;
    Vector3 _lookat;

    WorldPosition _wtarg;
    WorldPosition _wlookat;

    Transform _transform;

    public Vector3 Target { set { _targ = value; } }
    public Vector3 Lookat { get { return _lookat; } }

    public WorldPosition W_Target { set { _wtarg = value; } }
    public WorldPosition W_Lookat { get { return _wlookat; } set { _wlookat = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _lookat = Vector3.zero;
        _targ = _lookat;

        _wlookat = WorldPosition.FromVector3(Vector3.zero);
        _wtarg = _wlookat;

        UpdateTransform();
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldPosition.ENABLED)
            W_Trace(Time.deltaTime);
        else
            Trace(Time.deltaTime);

        Zoom(Input.mouseScrollDelta.y);

        if (Input.GetMouseButtonDown(1))
        {
            _mousePt = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - _mousePt;
            _mousePt = Input.mousePosition;

            Rot(delta.x, delta.y);
        }

        UpdateTransform();
    }

    void UpdateTransform()
    {
        _transform.rotation = Quaternion.Euler(_pitch, _yaw, 0F);
        _transform.position = _lookat + _transform.forward * (-_distance);
    }


    void Trace(float dt)
    {
        _lookat = _lookat + (_targ - _lookat) * _kd * dt;
    }

    void W_Trace(float dt)
    {
        Vector3 delta = WorldPosition.FromTo(_wlookat, _wtarg);
        if (delta.sqrMagnitude <= 0.001F)
        {
            _wlookat = _wtarg;
        }
        else
        {
            _wlookat.Add(WorldPosition.FromTo(_wlookat, _wtarg) * _kd * dt);
        }

        WorldPosition.Base = _wlookat;

        _lookat = WorldPosition.FromBase(_wlookat);
    }

    void Zoom(float delta)
    {
        _distance += _zoomSpeed * delta;
        _distance = Mathf.Clamp(_distance, 3F, 30F);
    }


    void Rot(float dx, float dy)
    {
        float p = _rotSpeed * dy;
        float y = _rotSpeed * dx;

        _pitch += p;
        _yaw += y;

        _pitch = Mathf.Clamp(_pitch, _patchMin, _patchMax);

        while (_yaw < -360F)
            _yaw += 360F;
        while (_yaw > 360F)
            _yaw -= 360F;

        _transform.rotation = Quaternion.Euler(_pitch, _yaw, 0F);
    }
}
