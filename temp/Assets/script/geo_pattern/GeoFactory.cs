using Assets.script.geo_pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeoFactory
{
    private static Dictionary<string, GeoBase> geos = new();

    public static GeoBase Get(string name)
    {
        // todo 04 : name 에서 key 로 쓰일 이름을 추출하세요.
        //   - ex1) "Rect 2.1 3" -> "Rect"
        //   - ex2) "Cirlce 13 4" -> "Cirlc-13"
        name = GetKey(name);

        if (!geos.ContainsKey(name))
        {
            geos[name] = CreateGeo(name);
        }

        return geos[name];
    }

    private static string GetKey(string name)
    {
        if (name.StartsWith("Circle"))
        {
            string[] words = name.Split();
            return words[0] + " " + words[1];
        }

        if (name.StartsWith("Cube"))
            return "Cube";

        if (name.StartsWith("Cylinder"))
        {
            string[] words = name.Split();
            return words[0] + " " + words[1];
        }

        if (name.StartsWith("Rect"))
            return "Rect";

        if (name.StartsWith("Sphere"))
        {
            string[] words = name.Split();
            return words[0] + " " + words[1];
        }

        return name;
    }

    private static GeoBase CreateGeo(string name)
    {
        GeoBase rt = null;
        if (name.StartsWith("Circle"))
            rt = CreateCirlce(name);
        if (name.StartsWith("Cube"))
            rt = CreateCube();
        if (name.StartsWith("Cylinder"))
            rt = CreateCylinder(name);
        if (name.StartsWith("Rect"))
            rt = CreateRect();
        if (name.StartsWith("Sphere"))
            rt = CreateSphere(name);

        rt.Build();

        return rt;
    }

    private static GeoCircle CreateCirlce(string name)
    {
        string[] words = name.Split();
        Debug.Assert(words.Length == 2);

        int numOfAngle = int.Parse(words[1]);

        return new GeoCircle(numOfAngle);
    }

    private static GeoCube CreateCube()
    {
        return new GeoCube();
    }

    private static GeoCylinder CreateCylinder(string name)
    {
        string[] words = name.Split();
        Debug.Assert(words.Length == 2);

        int numOfAngle = int.Parse(words[1]);

        return new GeoCylinder(numOfAngle);
    }

    private static GeoRect CreateRect()
    {
        return new GeoRect();
    }

    private static GeoSphere CreateSphere(string name)
    {
        string[] words = name.Split();
        Debug.Assert(words.Length == 2);

        int numOfAngle = int.Parse(words[1]);

        return new GeoSphere(numOfAngle);
    }
}
