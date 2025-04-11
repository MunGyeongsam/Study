using UnityEngine;

namespace _02_script
{
    public class _02_cube : DrawMeshBase
    {
        
        [SerializeField] private float size = 1F;
        
        protected override Mesh CreateMesh()
        {
            return MeshUtil.Cube(size);
        }
    }
}