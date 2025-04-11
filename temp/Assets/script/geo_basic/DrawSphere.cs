


using UnityEngine;

public class DrawSphere : DrawCylinder
{
    [SerializeField] float _radius2 = 1F;
    [SerializeField] int _numOfLongitude = 5;
    [SerializeField] int _numOfLatitude = 5;

    private BaseDrawMesh _cylinder = new DrawCylinder();

    protected override void Start0()
    {
        InitMemeber(_radius2, 2F * _radius2, _numOfLongitude, _numOfLatitude);
        Radius = (float r, float y) => Mathf.Sqrt(r * r - y * y);
    }
}