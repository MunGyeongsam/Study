using UnityEngine;

public class DrawMeshBase : MonoBehaviour
{
    private Mesh _mesh;
    private Material _mat;
    
    [SerializeField] private Texture texture;
    [SerializeField] private float yawSpeed = 90F;
    [SerializeField] private float pitchSpeed = 0F;
    [SerializeField] private float rollSpeed = 0F;
        
    void Start()
    {
        _mesh = CreateMesh();
        _mat = MeshUtil.CreateMaterial(texture);
    }
    
    protected virtual Mesh CreateMesh()
    {
        return MeshUtil.Rect(1, 1);
    }
    
    void Rotate()
    {
        transform.Rotate(Vector3.up, yawSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, pitchSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rollSpeed * Time.deltaTime);
    }

    protected virtual bool NeedToUpdateMesh()
    {
        return false;
    }
    void UpdateMesh()
    {
        if (NeedToUpdateMesh())
            _mesh = CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
        Rotate();
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
            _mat = MeshUtil.CreateMaterial(texture);
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            _mat = MeshUtil.CreateCustomMaterial(texture);
        
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _mat, 0);
        //Graphics.DrawMesh(_mesh, transform.position, Quaternion.identity, _mat, 0);
    }
}