using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCylinder : BaseDrawMesh
{
    [SerializeField] float _radius = 1F;
    [SerializeField] float _height = 1F;
    [SerializeField] int _numOfAngle = 5;
    [SerializeField] int _numOfSlice = 5;

    public Func<float, float, float> Radius = (r, y) => r;

    public void InitMemeber(float radius, float height, int numOfAngle, int numOfSlice)
    {
        _radius = radius;
        _height = height;
        _numOfAngle = numOfAngle;
        _numOfSlice = numOfSlice;
    }

    protected override void StepVertex(Mesh mesh)
    {
        var vtx = new Vector3[(_numOfAngle * 2) * (_numOfSlice + 1)];

        float angleStep = Mathf.PI * 2F / _numOfAngle;
        float yStep = _height / _numOfSlice;

        Vector3 pos = Vector3.zero;
        pos.y = _height * 0.5F;

        float radius = 0F;
        int iv = 0;
        for (int i = 0; i < _numOfSlice + 1; i++)
        {
            radius = Radius(_radius, pos.y);

            float angle = 0F;
            pos.x = radius;
            pos.z = 0F;
            vtx[iv++] = pos;
            for (int j = 1; j<_numOfAngle; ++j)
            {
                angle += angleStep;
                pos.x = radius * Mathf.Cos(angle);
                pos.z = radius * Mathf.Sin(angle);

                vtx[iv++] = pos;
                vtx[iv++] = pos;
            }
            vtx[iv++] = vtx[i*(_numOfAngle*2)];

            pos.y -= yStep;
        }

        mesh.vertices = vtx;
    }

    protected override void StepUv(Mesh mesh)
    {
        var uvs = new Vector2[(_numOfAngle *2) * (_numOfSlice + 1)];

        float uStep = 1F / _numOfAngle;
        float vStep = 1F / _numOfSlice;

        Vector2 uv = Vector2.zero;

        float v = 1F;
        int iv = 0;
        for (int i = 0; i < _numOfSlice + 1; i++)
        {
            uv.y = v;
            float u = 0F;
            uvs[iv++] = uv;
            for (int j = 1; j < _numOfAngle; ++j)
            {
                u += uStep;
                uvs[iv++] = uv;
                uvs[iv++] = uv;
                uv.x = u;
            }
            uv.x = 1F;
            uvs[iv++] = uv;

            v -= vStep;
        }

        mesh.uv = uvs;
    }

    protected override void StepColor(Mesh mesh)
    {
        Color[] colors = new Color[(_numOfAngle * 2) * (_numOfSlice + 1)];

        Color color = Color.white;
        color.b = 1F;

        float rstep = 1F / _numOfSlice;
        float gstep = 1F / _numOfAngle;

        float r = 0F;

        int ic = 0;

        for (int i = 0; i < _numOfSlice + 1; i++)
        {
            float g = 0F;

            color.r = r;
            for (int j = 1; j < _numOfAngle; ++j)
            {
                g += gstep;
                color.g = g;

                colors[ic++] = color;
                colors[ic++] = color;
            }
            color.g = 1F;
            colors[ic++] = color;

            r += rstep;
        }

        Array.Fill(colors, Color.white);

        mesh.colors = colors;
    }
    protected override void StepTriangle(Mesh mesh)
    {
        int[] tris = new int[_numOfAngle * _numOfSlice * 6];

        int it = 0;
        for (int i = 0; i < _numOfSlice; i++)
        {
            int iStart = i * (_numOfAngle * 2);
            for (int j = 0; j < _numOfAngle; ++j)
            {
                int J = j * 2;
                tris[it++] = iStart + J;
                tris[it++] = iStart + J + 1;
                tris[it++] = iStart + _numOfAngle * 2 + J;

                tris[it++] = iStart + J + 1;
                tris[it++] = iStart + _numOfAngle * 2 + 1 + J;
                tris[it++] = iStart + _numOfAngle * 2 + 0 + J;
            }
        }

        mesh.triangles = tris;
    }

    protected override void StepDrawSetting(Texture texture)
    {
        _draw = new DrawCustom(texture);
    }
    protected override void ChangeDraw(int n, Texture texture)
    {
        _draw = (n == 2) ? new DrawStandard(texture) : new DrawCustom(texture);
    }
}
