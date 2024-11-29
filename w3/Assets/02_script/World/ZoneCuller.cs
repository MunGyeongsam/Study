using System;
using System.Collections.Generic;
using UnityEngine;

abstract class ZoneCuller
{
    protected List<ZoneId> _remList;

    protected abstract void PreUpdate();
    protected abstract void CalcRemoveZones(Dictionary<ZoneId, Zone> zones);
    protected abstract void AddZones(Action<ZoneId> addZone);

    protected ZoneCuller()
    {
        _remList = new List<ZoneId>(16);
    }

    void RemoveZones(Dictionary<ZoneId, Zone> zones)
    {
        foreach (var z in _remList)
            zones.Remove(z);

        _remList.Clear();
    }

    public void Update(Dictionary<ZoneId, Zone> zones, Action<ZoneId> addZone)
    {
        PreUpdate();

        CalcRemoveZones(zones);
        RemoveZones(zones);

        AddZones(addZone);
    }
}

class RectCuller : ZoneCuller
{
    CameraCtrl _camCtrl;
    Transform _camTransform;
    ZoneId _centerId;

    const int OFFSET = 3;

    public RectCuller() : base()
    {
        _camCtrl = Camera.main.GetComponent<CameraCtrl>();
        if (null == _camCtrl)
            _camTransform = Camera.main.transform;
    }

    protected override void PreUpdate()
    {
        if (WorldPosition.ENABLED)
        {
            Vector3 pt = _camCtrl != null ? _camCtrl.W_Lookat.ApproximateVector : _camTransform.position;
            _centerId = ZoneConfig.ToZone(pt);
        }
        else
        {
            Vector3 pt = _camCtrl != null ? _camCtrl.Lookat : _camTransform.position;
            _centerId = ZoneConfig.ToZone(pt);
        }
    }

    protected override void CalcRemoveZones(Dictionary<ZoneId, Zone> zones)
    {
        foreach (var id in zones.Keys)
        {
            int dx = id.X - _centerId.X;
            int dz = id.Z - _centerId.Z;

            if (Mathf.Abs(dx) > OFFSET || Mathf.Abs(dz) > OFFSET)
                _remList.Add(id);
        }
    }

    protected override void AddZones(Action<ZoneId> addZone)
    {
        Bounds outLine = ZoneConfig.Bounds(ZoneConfig.WORLD_SIZE_IN_ZONES, ZoneConfig.MIN_ZONE, ZoneConfig.MIN_ZONE);
        if (WorldPosition.ENABLED)
        {
            outLine.center -= WorldPosition.Base.ApproximateVector;
        }
        ZoneConfig.DrawBox(outLine, Color.green);

        ZoneId id = new ZoneId();
        for (int ix = _centerId.X - OFFSET; ix <= _centerId.X + OFFSET; ++ix)
        {
            id.X = ix;

            for (int iz = _centerId.Z - OFFSET; iz <= _centerId.Z + OFFSET; ++iz)
            {
                id.Z = iz;

                if (ZoneConfig.ZoneInWorld(ix, iz))
                {
                    //for debugging
                    Bounds box = ZoneConfig.ZoneBounds(ix, iz);
                    if (WorldPosition.ENABLED)
                    {
                        box.center -= WorldPosition.Base.ApproximateVector;
                    }
                    ZoneConfig.DrawBox(box, Color.yellow);

                    addZone(id);
                }
            }
        }
    }
}


class FrustumCuller : ZoneCuller
{
    Camera _camera;
    Plane[] _frustum;

    uint _chkCount = 0U;
    uint _chkCountMax = 0U;

    public FrustumCuller() : base()
    {
        _camera = Camera.main;
        _frustum = new Plane[6];
    }

    bool FrustumCheck(Bounds box)
    {
        return GeometryUtility.TestPlanesAABB(_frustum, box);
    }

    void QTreeRcsv(Action<ZoneId> addZone, int size, int ix, int iz)
    {
        Bounds box = ZoneConfig.Bounds(size, ix, iz);
        if (WorldPosition.ENABLED)
        {
            box.center -= WorldPosition.Base.ApproximateVector;
        }

        ++_chkCount;
        bool chk = FrustumCheck(box);

        ZoneConfig.DrawBox(box, chk ? Color.yellow : Color.white);

        if (!chk)
            return;

        if (size <= 1)
        {
            addZone(new ZoneId(ix, iz));
        }
        else
        {
            size >>= 1; // size = size / 2;

            QTreeRcsv(addZone, size, ix, iz);
            QTreeRcsv(addZone, size, ix, iz + size);
            QTreeRcsv(addZone, size, ix + size, iz + size);
            QTreeRcsv(addZone, size, ix + size, iz);
        }
    }

    protected override void PreUpdate()
    {
        GeometryUtility.CalculateFrustumPlanes(_camera, _frustum);
    }

    protected override void CalcRemoveZones(Dictionary<ZoneId, Zone> zones)
    {
        foreach (var id in zones.Keys)
        {
            Bounds box = ZoneConfig.ZoneBounds(id.X, id.Z);
            if (WorldPosition.ENABLED)
            {
                //box.center = WorldPosition.FromBase(WorldPosition.FromVector3(box.center));

                box.center -= WorldPosition.Base.ApproximateVector;
            }

            if (!FrustumCheck(box))
                _remList.Add(id);
        }
    }

    protected override void AddZones(Action<ZoneId> addZone)
    {
        _chkCount = 0U;
        QTreeRcsv(addZone, ZoneConfig.WORLD_SIZE_IN_ZONES, ZoneConfig.MIN_ZONE, ZoneConfig.MIN_ZONE);
        _chkCountMax = _chkCountMax > _chkCount ? _chkCountMax : _chkCount;

        UI.AppendDbgInfo(string.Format("  - frustum check count : {0} / {1}", _chkCount, _chkCountMax));
    }
}