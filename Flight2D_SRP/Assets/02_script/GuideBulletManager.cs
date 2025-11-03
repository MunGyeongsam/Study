using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBulletManager
{
    static readonly Dictionary<int, Transform> _enemies = new();

    public static void Add(Enemy e)
    {
        _enemies[e.GetInstanceID()] = e.transform;
    }

    public static void Rem(Enemy e)
    {
        _enemies.Remove(e.GetInstanceID());
    }

    public static (bool, Vector2) TargetPosition(int id)
    {
        if (_enemies.TryGetValue(id, out var t))
        {
            return (true, t.position);
        }

        return (false, Vector2.zero);
    }

    public static int FindTarget(Vector2 pos)
    {
        int rt = int.MinValue;
        float minSqDist = float.MaxValue;

        foreach(var e in _enemies)
        {
            float sqDist = ((Vector2)e.Value.position - pos).sqrMagnitude;

            if (sqDist < minSqDist)
            {
                rt = e.Key;
                minSqDist = sqDist;
            }
        }

        return rt;
    }
}
