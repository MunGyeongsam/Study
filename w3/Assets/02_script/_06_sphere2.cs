using UnityEngine;

namespace _02_script
{
    public class _06_sphere2 : DrawMeshBase
    {
        [SerializeField, Range(0, 10F)] private float radius = 1F;
        [SerializeField, Range(3, 120)] private int numOfAngle = 4;
        
        protected override Mesh CreateMesh()
        {
            return MeshUtil.Sphere2(radius, numOfAngle);
        }
    }
}