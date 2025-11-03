using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public struct Info
    {
        public string Name;
        public PoolElement Prefab;
        public int MinCount;
        public int MaxCount;
    }

    [SerializeField] private Info[] _infos;
    [SerializeField] private Transform _parent = null;

    readonly Dictionary<string, IObjectPool<PoolElement>> _tbl = new();

    static ObjectPoolManager _instance;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        foreach(var i in _infos)
        {
            _tbl[i.Name] = new ObjectPool<PoolElement>(
                () => Create(i)
                , (PoolElement e) => e.OnGet()
                , (PoolElement e) => e.OnRelease()
                , (PoolElement e) => e.OnDestroy()
                , false
                , i.MinCount
                , i.MaxCount
                );
        }
    }

    PoolElement Create(Info info)
    {
        var rt = Instantiate<PoolElement>(info.Prefab, _parent);
        rt.OnCreate();
        rt.Pool = _tbl[info.Name];

        return rt;
    }

    public static PoolElement Get(string name)
    {
        var dic = _instance._tbl;
        return dic[name].Get();
    }
}
