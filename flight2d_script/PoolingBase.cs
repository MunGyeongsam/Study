using UnityEngine;
using System.Collections;

public class PoolingBase : MonoBehaviour {

	static protected Transform mPool;

	public void Reset(Transform t)
	{
		transform.position = t.position;
		transform.rotation = t.rotation;

		OnReset (t);
	}

	protected virtual void OnReset(Transform t) {
	}
}
