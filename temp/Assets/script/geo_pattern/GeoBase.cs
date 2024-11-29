using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoBase
{
    public Mesh Mesh => mesh;

    protected Mesh mesh;

    protected virtual int NumOfVertices { get; }

    protected virtual void StepVertex(Mesh mesh) { throw new System.Exception("to be implemented"); }
    protected virtual void StepUv(Mesh mesh) { throw new System.Exception("to be implemented"); }

    protected virtual void StepColor(Mesh mesh)
    {
        var colors = new Color[NumOfVertices];
        Array.Fill(colors, Color.white);

        // todo 02 : GeoBase �� ��� �޴� ��� Ŭ������ �� �Լ��� override �ؼ� ���� �ٸ纸����.
    }

    protected virtual void StepTriangle(Mesh mesh) { throw new System.Exception("to be implemented"); }

    public virtual void InitTransform(Transform t, string name) { }

    public void Build()
    {
        mesh = new Mesh();

        StepVertex(mesh);
        StepUv(mesh);
        StepColor(mesh);
        StepTriangle(mesh);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
