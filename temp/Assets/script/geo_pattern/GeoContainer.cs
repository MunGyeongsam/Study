using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoContainer : MonoBehaviour
{
    GeoBase _geoBase;
    IDraw _draw;
    float _life;
    float _yawSpeed;
    float _x;
    float _z;

    string _name;

    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(_x, 0F, _z);

        // todo 06 : Init 함수로 들어온 name 인자를 파싱해서 transfrom 설정을 해보세요.
        _geoBase.InitTransform(transform, _name);
    }

    void InitScaleRectCircle()
    {
        string[] w = _name.Split();

        if (w.Length == 1)
            return;

        if (w.Length == 2)
        {
            float s = float.Parse(w[1]);
            transform.localScale = new Vector3(s, s, s);
        }
        else
        {
            float sx = float.Parse(w[1]);
            float sz = float.Parse(w[2]);
            transform.localScale = new Vector3(sx, 1F, sz);
        }
    }
    void InitScaleCylinder()
    {
        string[] w = _name.Split();

        if (w.Length <= 2)
            return;

        if (w.Length == 3)
        {
            float s = float.Parse(w[2]);
            transform.localScale = new Vector3(s, s, s);
        }
        else
        {
            float sx = float.Parse(w[2]);
            float sy = float.Parse(w[3]);
            transform.localScale = new Vector3(sx, sy, sx);
        }
    }
    void InitScaleCube()
    {
        string[] w = _name.Split();

        if (w.Length == 1)
            return;

        if (w.Length == 2)
        {
            float s = float.Parse(w[1]);
            transform.localScale = new Vector3(s, s, s);
        }
        else if (w.Length == 4)
        {
            float sx = float.Parse(w[1]);
            float sy = float.Parse(w[2]);
            transform.localScale = new Vector3(sx, sy, sx);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // todo 05 : 남은 시간이 1초 미만이 된시점에 _draw 를 DrawCustom 으로 변경해서 곧 사라질 객체임을 알수 있도록 하세요.

        _life -= Time.deltaTime;
        if ( _life < 0 || _geoBase == null)
        {
            _draw = null;
            _geoBase = null;
            gameObject.SetActive( false );
            Destroy(gameObject);
            return;
        }

        transform.Rotate(Vector3.up, _yawSpeed*Time.deltaTime);

        if (_draw == null || _geoBase.Mesh == null || transform == null)
        {
            return;
        }

        _draw.Draw(_geoBase.Mesh, transform);
    }

    public void Init(string name, IDraw draw, float angularSpeed, float x, float z, float lifeTime = 5F)
    {
        _geoBase = GeoFactory.Get(name);
        _draw = draw;
        _life = lifeTime;
        _yawSpeed = angularSpeed;

        _x = x;
        _z = z;
        _name = name;
    }
}
