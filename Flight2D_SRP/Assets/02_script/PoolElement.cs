using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolElement : MonoBehaviour
{
    public UnityEngine.Pool.IObjectPool<PoolElement> Pool { set; private get;}

    public void Release()
    {
        Pool.Release(this);
    }

    public void OnCreate()
    {
        OnCreateImpl();
    }
    public void OnGet()
    {
        gameObject.SetActive(true);
        OnGetImpl();
    }
    public void OnRelease()
    {
        gameObject.SetActive(false);
        OnReleaseImpl();
    }
    public void OnDestroy()
    {
        OnDestroyImpl();
        Destroy(gameObject);
    }

    protected virtual void OnCreateImpl() { }
    protected virtual void OnGetImpl() { }
    protected virtual void OnReleaseImpl() { }
    protected virtual void OnDestroyImpl() { }
}
