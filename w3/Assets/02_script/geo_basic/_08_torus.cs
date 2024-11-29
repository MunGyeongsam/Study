using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _08_torus : DrawMeshBase
{
    [SerializeField, Range(0, 10F)] private float radius = 1F;
    [SerializeField, Range(0, 10F)] private float tubeRadius = 0.3F;
    [SerializeField, Range(3, 120)] private int numOfAngle = 4;
    [SerializeField, Range(3, 120)] private int numOfTubeAngle = 4;

    private float prevRadius;
    private float prevTubeRadius;
    private int prevNumOfAngle;
    private int prevNumOfTubeAngle;

    protected override bool NeedToUpdateMesh()
    {
        if (prevRadius != radius || prevTubeRadius != tubeRadius || prevNumOfAngle != numOfAngle || prevNumOfTubeAngle != numOfTubeAngle)
        {
            prevRadius = radius;
            prevTubeRadius = tubeRadius;
            prevNumOfAngle = numOfAngle;
            prevNumOfTubeAngle = numOfTubeAngle;
            return true;
        }

        return false;
    }

    protected override Mesh CreateMesh()
    {
        return MeshUtil.Torus(radius, tubeRadius, numOfAngle, numOfTubeAngle);
    }
}
