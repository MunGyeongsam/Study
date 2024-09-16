using UnityEngine;

namespace _02_script
{
    public class _05_cylinder : DrawMeshBase
    {
        [SerializeField, Range(0, 10F)] private float radius = 1F;
        [SerializeField, Range(0, 10F)] private float height = 1F;
        [SerializeField, Range(3, 120)] private int numOfAngle = 4;
        [SerializeField, Range(1, 120)] private int numOfSlice = 4;
        
        protected override Mesh CreateMesh()
        {
            return MeshUtil.Cylinder(radius, height, numOfAngle, numOfSlice);
        }
    }
}