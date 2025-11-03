using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracerTest : MonoBehaviour
{
    Camera _c;
    Vector2 _max;
    Vector2 _min;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        var mc = Camera.main;

        float screenHeight = mc.orthographicSize * 2F;
        float screenWidth = screenHeight * mc.aspect;
        var size = _spriteRenderer.size;

        float scale = Mathf.Max(screenWidth / size.x
            , screenHeight / size.y);

        _c = Camera.main;
        var sz = _spriteRenderer.size * scale;
        sz *= 0.5F;
        sz.x = sz.x - screenWidth * 0.5F;
        sz.y = sz.y - screenHeight * 0.5F;

        _max = sz;
        _min = -_max;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ptInScreen = Input.mousePosition;
            var ptInWorld = _c.ScreenToWorldPoint(ptInScreen);
            ptInWorld.z = transform.position.z;

            ptInWorld.x = Mathf.Clamp(ptInWorld.x, _min.x, _max.x);
            ptInWorld.y = Mathf.Clamp(ptInWorld.y, _min.y, _max.y);

            transform.position = ptInWorld;
        }
    }
}
