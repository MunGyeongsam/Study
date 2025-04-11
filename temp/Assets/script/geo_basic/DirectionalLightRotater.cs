using UnityEngine;

public class DirectionalLightRotater : MonoBehaviour
{
    [SerializeField, Range(1F, 100F)] float pitchSpeed = 1F;
    [SerializeField, Range(1F, 100F)] float yawSpeed = 1F;
    private Transform _transform;

    private float _pitch = 0F;
    private float _yaw = 0F;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Rot();
    }

    void Rot()
    {
        float dt = Time.deltaTime;

        _pitch += dt * pitchSpeed;
        _yaw += dt * yawSpeed;

        _transform.rotation = Quaternion.Euler(_pitch, _yaw, 0F);
    }
}
