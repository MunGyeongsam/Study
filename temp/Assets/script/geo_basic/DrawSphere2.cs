using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSphere2 : BaseDrawMesh
{
    [SerializeField] float radius;
    [SerializeField] int numOfAngle;

    protected override void StepVertex(Mesh mesh)
    {
        int numOfSlice = numOfAngle / 2;
        //numOfSlice = numOfAngle - 1;
        int numOfVtx = (numOfAngle * 2 * 2) * numOfSlice;

        Vector3[] vtx = new Vector3[numOfVtx];

        float y = radius;
        float yStep = 2 * radius / numOfSlice;

        float vStepInRadians = Mathf.PI / numOfSlice;
        float uStepInRadians = 2*Mathf.PI / numOfAngle;

        float vAngle = 0F;
        float uAngle = 0F;

        Vector3 pt = Vector3.zero;
        pt.y = y;

        int iv = 0;

        float r = 0F;

        for (int i=0; i<numOfSlice; i++)
        {
            r = radius * Mathf.Sin(vAngle);
            y = radius * Mathf.Cos(vAngle);

            uAngle = 0F;

            pt.y = y;

            for (int i2 = 0; i2 < 2; i2++)
            {
                pt.x = r;
                pt.z = 0F;
                vtx[iv++] = pt;

                uAngle = uStepInRadians;

                for (int j = 1; j < numOfAngle; j++)
                {
                    pt.x = r * Mathf.Cos(uAngle);
                    pt.z = r * Mathf.Sin(uAngle);

                    vtx[iv++] = pt;
                    vtx[iv++] = pt;
                    uAngle += uStepInRadians;
                }

                pt.x = r;
                pt.z = 0F;
                vtx[iv++] = pt;

                pt.y = radius * Mathf.Cos(vAngle + vStepInRadians);
                r = radius * Mathf.Sin(vAngle + vStepInRadians);
            }

            vAngle += vStepInRadians;
        }

        Debug.Assert(iv == numOfVtx);

        mesh.vertices = vtx;
    }
    protected override void StepUv(Mesh mesh)
    {
        int numOfSlice = numOfAngle / 2;
        //numOfSlice = numOfAngle - 1;
        int numOfVtx = (numOfAngle * 2 * 2) * numOfSlice;

        var uvs = new Vector2[numOfVtx];

        float uStep = 1F / numOfAngle;
        float vStep = 1F / numOfSlice;

        Vector2 uv = Vector2.zero;

        float v = 1F;
        int iv = 0;
        for (int i = 0; i < numOfSlice; i++)
        {
            uv.y = v;

            for (int i2 = 0; i2 < 2; ++i2)
            {
                float u = 0F;
                uvs[iv++] = uv;
                for (int j = 1; j < numOfAngle; ++j)
                {
                    u += uStep;
                    uvs[iv++] = uv;
                    uvs[iv++] = uv;
                    uv.x = u;
                }
                uv.x = 1F;
                uvs[iv++] = uv;

                uv.y += vStep;
            }

            v -= vStep;
        }

        Debug.Assert(iv == numOfVtx);
        mesh.uv = uvs;
    }
    protected override void StepColor(Mesh mesh)
    {
        int numOfSlice = numOfAngle / 2;
        int numOfVtx = (numOfAngle * 2 * 2) * numOfSlice;

        Color[] colors = new Color[numOfVtx];
        for (int i = 0; i < numOfVtx; i++)
            colors[i] = Color.white;

        mesh.colors = colors;
    }
    protected override void StepTriangle(Mesh mesh)
    {
        int numOfSlice = numOfAngle / 2;
        //numOfSlice = numOfAngle - 1;
        int[] tris = new int[numOfAngle * numOfSlice * 6];


        int it = 0;
        for (int i = 0; i < numOfSlice; i++)
        {
            int iStart = i * (numOfAngle * 4);
            for (int j = 0; j < numOfAngle; ++j)
            {
                int J = j * 2;
                tris[it++] = iStart + J;
                tris[it++] = iStart + J + 1;
                tris[it++] = iStart + numOfAngle * 2 + J;

                tris[it++] = iStart + J + 1;
                tris[it++] = iStart + numOfAngle * 2 + 1 + J;
                tris[it++] = iStart + numOfAngle * 2 + 0 + J;
            }
        }
        Debug.Assert(it == numOfAngle * numOfSlice * 6);

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
