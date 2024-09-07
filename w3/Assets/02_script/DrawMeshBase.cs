using UnityEngine;

public class DrawMeshBase : MonoBehaviour
{
    private Mesh _mesh;
    private Material _mat;
    
    [SerializeField] private Texture texture;
        
    void Start()
    {
        _mesh = CreateMesh();
        _mat = MeshUtil.CreateMaterial(texture);
    }
    
    protected virtual Mesh CreateMesh()
    {
        return MeshUtil.Rect(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            _mat = MeshUtil.CreateMaterial(texture);
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            _mat = MeshUtil.CreateCustomMaterial(texture);
        
        Graphics.DrawMesh(_mesh, transform.position, Quaternion.identity, _mat, 0);
    }
}