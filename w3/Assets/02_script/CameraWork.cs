using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    Camera _camera;
    Transform _cameraTransform;

    

    [SerializeField, Range(1F, 10F)] float _speed = 1F;
    [SerializeField, Range(1F, 10F)] float _zoomSpeed = 5F;
    [SerializeField, Range(0.1F, 2F)] float _rotSpeed = 0.5F;
    [SerializeField, Range(1F, 15F)] float _kd = 5F;

    [SerializeField, Range(0F, 45F)]
    float _patchMin = 10F;
    [SerializeField, Range(45F, 90F)]
    float _patchMax = 80F;

    float _pitch = 45F;
    float _yaw = 0F;
    float _distance;
    
    Vector3 _lookAt;
    Vector3 _lookAtTo;
    Vector3 _eye;
    
    Vector3 _mousePt;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _cameraTransform = Camera.main.transform;

        _eye = _cameraTransform.position;
        _lookAt = _eye;
        _lookAt.y = 0F;
        _lookAtTo = _lookAt;

        _distance = _eye.y;

        _cameraTransform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        MoveWithKey();
        MoveWithMouse();
        ZoomWithWheel();
        RotateWithMouse();

        UpdateLookAt();
    }

    void UpdateLookAt()
    {
        _lookAt = _lookAt + (_lookAtTo - _lookAt) * Time.deltaTime * _kd;

        _eye = _lookAt + _cameraTransform.rotation * Vector3.back * _distance;

        _cameraTransform.position = _eye;
    }

    void MoveWithMouse()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _lookAtTo = Vector3.zero;
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var ptInScreen = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(ptInScreen);
            var p = new Plane(Vector3.up, 0F);

            if (p.Raycast(ray, out float dist))
            {
                var ptInWorld = ray.GetPoint(dist);
                _lookAtTo = ptInWorld;
            }

            return;
        }
        
        if (Input.GetMouseButton(2)) // Middle mouse button
        {
            var ptInScreen = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(ptInScreen);
            var p = new Plane(Vector3.up, 0F);

            if (p.Raycast(ray, out float dist))
            {
                var ptInWorld = ray.GetPoint(dist);
                var delta = _lookAtTo - ptInWorld;
                _lookAtTo -= delta;
                _eye -= delta;
            }
        }
    }

    void MoveWithKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _eye.z += Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _eye.z -= Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _eye.x -= Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _eye.x += Time.deltaTime * _speed;
        }

        _cameraTransform.position = _eye;
    }

    void RotateWithMouse()
    {
        if (Input.GetMouseButton(1) == false)
            return;
        
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            _mousePt = Input.mousePosition;
            return;
        }
        
        Vector3 delta = Input.mousePosition - _mousePt;
        _mousePt = Input.mousePosition;
        
        float p = _rotSpeed * delta.y;
        float y = _rotSpeed * delta.x;

        _pitch += p;
        _yaw += y;

        _pitch = Mathf.Clamp(_pitch, _patchMin, _patchMax);

        while (_yaw < -360F)
            _yaw += 360F;
        while (_yaw > 360F)
            _yaw -= 360F;

        _cameraTransform.rotation = Quaternion.Euler(_pitch, _yaw, 0F);
    }

    void ZoomWithWheel()
    {
        float scroll = Input.mouseScrollDelta.y;
        _distance -= scroll * _zoomSpeed;
        _distance = Mathf.Clamp(_distance, 2F, 20F); // Adjust min and max zoom levels as needed
    }
}