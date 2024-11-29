using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public struct ZoneId
{
    private int _ix;
    private int _iz;

    public ZoneId(int ix, int iz)
    {
        _ix = ix;
        _iz = iz;
    }

    public int X { get { return _ix; } set { _ix = value; } }
    public int Z { get { return _iz; } set { _iz = value; } }


    public override int GetHashCode()
    {
        return ((_iz & 0xFFFF) << 16) + (_ix & 0xFFFF);
    }

    public bool Equals(ZoneId rhs)
    {
        return _ix == rhs._ix && _iz == rhs._iz;
    }
}

struct ZoneIdCmp : IEqualityComparer<ZoneId>
{
    public bool Equals(ZoneId lhs, ZoneId rhs)
    {
        return lhs.Equals(rhs);
    }
    public int GetHashCode(ZoneId p)
    {
        return p.GetHashCode();
    }
}

class Zone
{
    int _ix;
    int _iz;

    WorldPosition _wp;

    Mesh[] _mesh;
    Matrix4x4 _mat;

    public Bounds Box { get { return ZoneConfig.ZoneBounds(_ix, _iz); } }

    public Zone(int ix, int iz, int nLayer)
    {
        _ix = ix;
        _iz = iz;

        if (WorldPosition.ENABLED)
        {
            _wp = new WorldPosition(new ZoneId(ix, iz));
        }
        else
        {
            const float SIZE = ZoneConfig.ZONE_SIZE;
            _mat = Matrix4x4.Translate(new Vector3(ix * SIZE, 0F, iz * SIZE));
        }
        _mesh = Util.Zone.GetMeshs(nLayer);
    }

    public void Draw(Material[] materials)
    {
        Matrix4x4 mat = _mat;
        if (WorldPosition.ENABLED)
        {
            //Vector3 pos = WorldPosition.FromBase(_wp);
            //_mat = Matrix4x4.Translate(pos);

            Vector3 b = WorldPosition.Base.ApproximateVector;
            float s = ZoneConfig.ZONE_SIZE;
            float x = _ix * s - b.x;
            float z = _iz * s - b.z;

            _mat = Matrix4x4.Translate(new Vector3(x, 0F, z));
        }

        int n = _mesh.Length;
        for (int i = 0; i < n; ++i)
        {
            Graphics.DrawMesh(_mesh[i], mat, materials[i], i);
        }
    }
}

