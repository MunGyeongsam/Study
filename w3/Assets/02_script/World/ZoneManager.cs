using System.Collections.Generic;
using UnityEngine;


class ZoneManager
{
    Plane _plane;
    Vector3 _picked;

    int _layer;

    Dictionary<ZoneId, Zone> _zones;

    int _drawCount = 0;
    int _drawCountMax = 0;

    ZoneCuller _curll;

    public Vector3 Picked { get { return _picked; } }
    public Dictionary<ZoneId, Zone> Zones { get { return _zones; } }

    public ZoneManager(int nLayer)
    {
        _layer = nLayer;
        _zones = new Dictionary<ZoneId, Zone>(9, new ZoneIdCmp());

        _plane = new Plane(Vector3.up, Vector3.zero);
        _picked = Vector3.zero;

        _curll = new RectCuller();
        //_curll = new FrustumCuller();
    }

    void AddZoneImpl(ZoneId id)
    {
        if (_zones.ContainsKey(id))
            return;

        _zones.Add(id, new Zone(id.X, id.Z, _layer));
    }

    public void AddZone(ZoneId id)
    {
        AddZoneImpl(id);
    }

    public bool Pick()
    {
        float dist = 0F;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out dist))
        {
            _picked = ray.GetPoint(dist);
            _picked = ZoneConfig.Clamp(_picked);
            return ZoneConfig.PtInWorld(_picked);
        }

        return false;
    }

    public void Draw(Material[] materials)
    {
        _drawCount = _zones.Count;
        _drawCountMax = Mathf.Max(_drawCountMax, _drawCount);

        UI.AppendDbgInfo(string.Format("  - draw count : {0} / {1}", _drawCount, _drawCountMax));

        foreach (var z in _zones.Values)
            z.Draw(materials);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
            _curll = new RectCuller();
        else if (Input.GetKeyUp(KeyCode.F2))
            _curll = new FrustumCuller();

        _curll.Update(Zones, AddZone);
    }
}