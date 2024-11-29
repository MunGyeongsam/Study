using UnityEngine;

static class ZoneConfig
{
    public const int    WORLD_SIZE_IN_ZONES         = 1024 * 32;
    //public const int    WORLD_SIZE_IN_ZONES         = 2;

    public const int    MAX_ZONE                    = WORLD_SIZE_IN_ZONES / 2 - 1;
    public const int    MIN_ZONE                    = -MAX_ZONE - 1;

    public const int    ZONE_SIZE_IN_TILES          = 32;
    public const float  TILE_SIZE                   = 1F;
    public const float  ZONE_SIZE                   = ZONE_SIZE_IN_TILES * TILE_SIZE;
    public const float  WORLD_SIZE                  = WORLD_SIZE_IN_ZONES * ZONE_SIZE;

    static ZoneConfig()
    {
        Debug.Assert(Mathf.IsPowerOfTwo(WORLD_SIZE_IN_ZONES));
    }

    public static Vector3 WORLD_MAX { get { return new Vector3(WORLD_SIZE, 0F, WORLD_SIZE); } }
    public static Vector3 WORLD_MIN { get { return -WORLD_MAX; } }

    public static Bounds ZoneBounds(int ix, int iz)
    {
        return Bounds(1, ix, iz);
    }

    public static Bounds Bounds(int sizeInZones, int iLeft, int iBttm)
    {
        Bounds box = new Bounds();

        float lft = iLeft * ZONE_SIZE;
        float btm = iBttm * ZONE_SIZE;
        float size = sizeInZones * ZONE_SIZE;

        Vector3 min = new Vector3(lft, 0F, btm);
        Vector3 max = new Vector3(lft + size, 0F, btm + size);
        box.SetMinMax(min, max);

        return box;
    }

    public static ZoneId ToZone(Vector3 pos)
    {
        int iz = (int)Mathf.Floor(Mathf.Abs(pos.z) / ZONE_SIZE);
        int ix = (int)Mathf.Floor(Mathf.Abs(pos.x) / ZONE_SIZE);

        if (pos.z < 0F)
            iz = -iz - 1;
        if (pos.x < 0F)
            ix = -ix - 1;

        ZoneId rt = new ZoneId(ix, iz);

        return rt;
    }

    public static Vector3 Clamp(Vector3 pos)
    {
        float max = (MAX_ZONE + 1) * ZONE_SIZE;
        float min = -max;

        pos.x = Mathf.Clamp(pos.x, min, max);
        pos.z = Mathf.Clamp(pos.z, min, max);

        return pos;
    }

    public static bool PtInWorld(Vector3 pos)
    {
        float max = (MAX_ZONE + 1) * ZONE_SIZE;
        float min = -max;

        float x = pos.x;
        float z = pos.z;

        return ((x >= min && x <= max) && (z >= min && z <= max));
    }

    public static bool ZoneInWorld(int ix, int iz)
    {
        return (ix <= MAX_ZONE && ix >= MIN_ZONE) && (iz <= MAX_ZONE && iz >= MIN_ZONE);
    }


    public static void DrawBox(Bounds box, Color clr)
    {
        float l = box.min.x;
        float r = box.max.x;
        float b = box.min.z;
        float t = box.max.z;

        Debug.DrawLine(new Vector3(l, 0F, b), new Vector3(l, 0F, t), clr);
        Debug.DrawLine(new Vector3(l, 0F, t), new Vector3(r, 0F, t), clr);
        Debug.DrawLine(new Vector3(r, 0F, t), new Vector3(r, 0F, b), clr);
        Debug.DrawLine(new Vector3(r, 0F, b), new Vector3(l, 0F, b), clr);
    }
}