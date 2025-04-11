using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _07_cone : DrawMeshBase
{
    [SerializeField, Range(0, 10F)] private float radius = 1F;
    [SerializeField, Range(0, 10F)] private float height = 1F;
    [SerializeField, Range(3, 120)] private int numOfAngle = 4;

    private float prevRadius;
    private float prevHeight;
    private int prevNumOfAngle;

    protected override bool NeedToUpdateMesh()
    {
        if (prevRadius != radius || prevHeight != height || prevNumOfAngle != numOfAngle)
        {
            prevRadius = radius;
            prevHeight = height;
            prevNumOfAngle = numOfAngle;
            return true;
        }

        return false;
    }

    protected override Mesh CreateMesh()
    {
        return MeshUtil.Cone(radius, height, numOfAngle);
    }
}
