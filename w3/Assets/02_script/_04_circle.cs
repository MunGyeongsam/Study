using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _04_circle : DrawMeshBase
{
    [SerializeField] private float radius = 1F;
    [SerializeField, Range(3, 120)] private int segments = 4;
    
    protected override Mesh CreateMesh()
    {
        return MeshUtil.Circle(radius, segments);
    }
}
