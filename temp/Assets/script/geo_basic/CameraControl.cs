using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField, Range(3F, 30F)]
    float _distance = 5F;

    [SerializeField, Range(1F, 10F)]
    float _zoomSpeed = 5F;
    [SerializeField, Range(0.1F, 2F)]
    float _rotSpeed = 0.5F;
    [SerializeField, Range(0.001F, 0.1F)]
    float _moveSpeed = 0.003F;

    [SerializeField, Range(1F, 15F)]
    float _kd = 5F;

    [SerializeField, Range(-90F, 45F)]
    float _pitchMin = 10F;

    [SerializeField, Range(45F, 90F)]
    float _pitchMax = 80F;

    float _pitch = 45F;
    float _yaw = 0F;

    Vector3 _mousePt;
    Vector3 _targ;
    Vector3 _lookat;

    Transform _transform;

    public Vector3 Target { set { _targ = value; } }
    public Vector3 Lookat { get { return _lookat; } }

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _lookat = Vector3.zero;
        _targ = _lookat;

        UpdateTransform();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom(Input.mouseScrollDelta.y);

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            _mousePt = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - _mousePt;
            _mousePt = Input.mousePosition;

            Rot(delta.x, delta.y);
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - _mousePt;
            _mousePt = Input.mousePosition;

            delta *= _moveSpeed;
            MoveLookat(delta.x, delta.y);
        }

        UpdateTransform();
    }

    void UpdateTransform()
    {
        _transform.rotation = Quaternion.Euler(_pitch, _yaw, 0F);
        _transform.position = _lookat + _transform.forward * (-_distance);
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

        _pitch = Mathf.Clamp(_pitch, _pitchMin, _pitchMax);

        while (_yaw < -360F)
            _yaw += 360F;
        while (_yaw > 360F)
            _yaw -= 360F;

        _transform.rotation = Quaternion.Euler(_pitch, _yaw, 0F);
    }

    void MoveLookat(float dx, float dy)
    {

        Vector3 right = _transform.right;
        right.y = 0F;
        right.Normalize();

        float uy = Mathf.Abs(_transform.up.y);
        float fy = Mathf.Abs(_transform.forward.y);

        Vector3 forward = uy < fy ? _transform.up : _transform.forward;
        forward.y = 0F;
        forward.Normalize();

        _lookat -= right * dx;
        _lookat -= forward * dy;
    }
}
