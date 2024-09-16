using UnityEngine;

namespace _02_script
{
    public class _03_pyramid : DrawMeshBase
    {
        [SerializeField] private float size = 1F;
        
        protected override Mesh CreateMesh()
        {
            return MeshUtil.Pyramid(size);
        }
    }
}