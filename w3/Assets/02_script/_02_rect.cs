using UnityEngine;

namespace _02_script
{
    public class _02_rect : DrawMeshBase
    {
        [SerializeField] private float width = 2F;
        [SerializeField] private float height = 1F;
        protected override Mesh CreateMesh()
        {
            return MeshUtil.Rect(width, height);
        }
    }
}