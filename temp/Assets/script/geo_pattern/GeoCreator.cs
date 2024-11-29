using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoCreator : MonoBehaviour
{
    [SerializeField] float coolTime = 1F;
    [SerializeField] float rangeX = 10F;
    [SerializeField] float rangeZ = 10F;

    [SerializeField] GeoContainer prefab;

    [SerializeField] Texture[] textures;

    IDraw[] _draws;

    float accum = 0F;

    // Start is called before the first frame update
    void Start()
    {
        int nTexture = textures.Length;
        _draws = new IDraw[nTexture];

        for(int i=0; i<nTexture; i++)
        {
            _draws[i] = new DrawStandard(textures[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        accum += Time.deltaTime;

        if (accum > coolTime)
        {
            accum -= coolTime;

            float x = Random.Range(-rangeX, rangeX);
            float z = Random.Range(-rangeZ, rangeZ);
            float life = Random.Range(3F, 15F);
            float angularSpeed = Random.Range(-180F, 180F);

            int idraw = Random.Range(0, _draws.Length);

            var name = GetName();

            var instance = Instantiate(prefab);
            instance.Init(name, _draws[idraw], angularSpeed, x, z, life);
        }
    }

    string GetName()
    {
        // todo 01 : 다양한 이름을 반환 할 수 있도록 구현하세요.
        //  이름 규칙은 GeoFactory.CreateGeo 함수를 참조하세요

        int r = Random.Range(0, 10);
        if (r < 1) return NameRect1();
        if (r < 2) return NameRect2();
        if (r < 3) return NameCircle1();
        if (r < 4) return NameCircle2();
        if (r < 5) return NameCube1();
        if (r < 6) return NameCube2();
        if (r < 7) return NameCylinder();
        if (r < 8) return NameSphere1();

        return NameSphere2();
    }

    string NameRect1()
    {
        float s = Random.Range(0.2F, 3.2F);
        return $"Rect {s}";
    }
    string NameRect2()
    {
        float w = Random.Range(0.2F, 3.2F);
        float h = Random.Range(0.2F, 3.2F);
        return $"Rect {w} {h}";
    }
    string NameCircle1()
    {
        int n = Random.Range(5, 32);
        float r = Random.Range(0.2F, 3.2F);
        return $"Circle {n} {r}";
    }
    string NameCircle2()
    {
        int n = Random.Range(5, 32);
        float w = Random.Range(0.2F, 3.2F);
        float h = Random.Range(0.2F, 3.2F);
        return $"Circle {n} {w} {h}";
    }
    string NameCube1()
    {
        float w = Random.Range(0.2F, 3.2F);
        float h = Random.Range(0.2F, 3.2F);
        float d = Random.Range(0.2F, 3.2F);
        return $"Cube {w} {h} {d}";
    }
    string NameCube2()
    {
        float w = Random.Range(0.2F, 3.2F);
        float h = Random.Range(0.2F, 3.2F);
        float d = Random.Range(0.2F, 3.2F);
        return $"Cube {w} {h} {d}";
    }
    string NameCylinder()
    {
        int n = Random.Range(5, 32);
        float r = Random.Range(0.2F, 3.2F);
        float h = Random.Range(0.2F, 3.2F);
        return $"Cylinder {n} {r} {h}";
    }
    string NameSphere1()
    {
        int n = Random.Range(5, 32);
        float r = Random.Range(0.2F, 3.2F);
        return $"Sphere {n} {r}";
    }
    string NameSphere2()
    {
        int n = Random.Range(5, 32);
        float w = Random.Range(0.2F, 3.2F);
        float h = Random.Range(0.2F, 3.2F);
        float d = Random.Range(0.2F, 3.2F);
        return $"Sphere {n} {w} {h} {d}";
    }
}
