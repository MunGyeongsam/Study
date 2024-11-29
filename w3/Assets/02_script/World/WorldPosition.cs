using UnityEngine;
using static UnityEditor.PlayerSettings;

public struct WorldPosition
{
    public static bool ENABLED = true;

    const float SEGMENT_SIZE = 32F;
    const float SEG_PER_ZONE = ZoneConfig.ZONE_SIZE / SEGMENT_SIZE;

    static WorldPosition _base = new WorldPosition(0, 0);

    int _sx;
    int _sz;
    Vector3 _offset;

    public static WorldPosition Base { set { _base = value; _base._offset.y = 0F; } get { return _base; } }

    public WorldPosition(WorldPosition p) 
        : this(p._sx, p._sz, p._offset)
    {
    }

    public WorldPosition(Vector3 v)
    {
        _sx = _sz = 0;
        _offset = v;
        Normalize();
    }

    public WorldPosition(ZoneId zoneId)
    {
        float sx = zoneId.X * SEG_PER_ZONE;
        float sz = zoneId.Z * SEG_PER_ZONE;

        _sx = (int)sx;
        _sz = (int)sz;

        _offset = new Vector3((sx - _sx) * SEGMENT_SIZE, 0F, (sz - _sz) * SEGMENT_SIZE);
    }

    public WorldPosition(int sx, int sz)
    {
        _sx = sx;
        _sz = sz;
        _offset = Vector3.zero;
    }

    public WorldPosition(int sx, int sz, Vector3 offset)
    {
        _sx = sx;
        _sz = sz;
        _offset = offset;
    }

    void Normalize()
    {
        float fx = Mathf.Abs(_offset.x);
        float fz = Mathf.Abs(_offset.z);

        if (fx > SEGMENT_SIZE)
        {
            int n = (int)(_offset.x / SEGMENT_SIZE);
            _offset.x -= n * SEGMENT_SIZE;
            _sx += n;
        }
        if (fz > SEGMENT_SIZE)
        {
            int n = (int)(_offset.z / SEGMENT_SIZE);
            _offset.z -= n * SEGMENT_SIZE;
            _sz += n;
        }
    }

    public void Translate(Vector3 delta)
    {
        _offset += delta;
        Normalize();
    }

    public Vector3 ApproximateVector
    {
        get { return _offset + new Vector3(_sx * SEGMENT_SIZE, 0F, _sz * SEGMENT_SIZE); }
    }

    public void Subtract(WorldPosition rhs)
    {
        _sx -= rhs._sx;
        _sz -= rhs._sz;
        _offset -= rhs._offset;
        
        Normalize(); 
    }

    public void Subtract(Vector3 rhs)
    {
        _offset -= rhs;
        Normalize();
    }

    public void Add(WorldPosition rhs)
    {
        _sx += rhs._sx;
        _sz += rhs._sz;
        _offset += rhs._offset;

        Normalize();
    }

    public void Add(Vector3 rhs)
    {
        _offset += rhs;
        Normalize();
    }

    public override string ToString()
    {
        return string.Format("[{0}, {1}] x:{2}, y:{3}", _sx, _sz, _offset.x, _offset.z);
    }

    public static WorldPosition FromVector3(Vector3 ptInWorld)
    {
        return new WorldPosition(ptInWorld);
    }

    public static Vector3 FromTo(WorldPosition from, WorldPosition to)
    {
        Vector3 rt = to._offset - from._offset;

        rt.x += (to._sx - from._sx) * SEGMENT_SIZE;
        rt.z += (to._sz - from._sz) * SEGMENT_SIZE;

        return rt;
    }

    public static Vector3 FromBase(WorldPosition wp)
    {
        return FromTo(_base, wp);
    }

    public static WorldPosition AddToBase(Vector3 delta)
    {
        WorldPosition rt = _base;

        rt._offset += delta;
        rt.Normalize();

        return rt;
    }
}
