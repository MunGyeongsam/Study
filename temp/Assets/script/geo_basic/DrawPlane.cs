using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPlane : BaseDrawMesh
{
    [SerializeField] float width = 10F;

    protected override void StepVertex(Mesh mesh)
    {
        float hw = width * 0.5F;

        mesh.vertices = new Vector3[]{

            new Vector3(-hw, 0, hw),
            new Vector3( hw, 0, hw),
            new Vector3(-hw, 0,-hw),
            new Vector3( hw, 0,-hw),

        };
    }

    protected override void StepUv(Mesh mesh)
    {
        mesh.uv = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
        };
    }

    protected override void StepColor(Mesh mesh)
    {
        mesh.colors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.gray,
        };
    }

    protected override void StepTriangle(Mesh mesh)
    {
        mesh.triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };
    }


    protected override void StepDrawSetting(Texture texture)
    {
        _draw = new DrawStandard(texture);
    }


    protected override void ChangeDraw(int n, Texture texture)
    {
        _draw = (n == 1) ? new DrawStandard(texture) : new DrawCustom(texture);
    }
}
