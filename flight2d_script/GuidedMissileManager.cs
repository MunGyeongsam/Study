using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuidedMissileManager : MonoBehaviour {

	static GuidedMissileManager msThis;

	Dictionary<int, Transform> mTbl;

	// Use this for initialization
	void Start () {
		mTbl = new Dictionary<int, Transform> (16);

		msThis = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AddEnemyImpl(Transform t)
	{
		mTbl.Add (t.GetInstanceID (), t);
	}

	void RemoveEnemyImpl(Transform t)
	{
		mTbl.Remove (t.GetInstanceID ());
	}

	int FindTargetImpl(Vector3 bulletPosition)
	{
		int id = 0;
		float minDistance = float.MaxValue;
		float distance = 0.0f;
		foreach (Transform t in mTbl.Values) {
			distance = (t.position - bulletPosition).sqrMagnitude;
			if (distance < minDistance) {
				id = t.GetInstanceID ();
				minDistance = distance;
			}
		}
		return id;
	}
	bool TargetPositionImpl(int id, ref Vector3 targetPosition)
	{
		if (mTbl.ContainsKey(id))
		{
			Transform t = mTbl [id];
			targetPosition = t.position;
			return true;
		} else {
			return false;
		}
	}

	public static void AddEnemy(Transform t)
	{
		if (msThis)
			msThis.AddEnemyImpl (t);
	}
	public static void RemoveEnemy(Transform t)
	{
		if (msThis)
			msThis.RemoveEnemyImpl (t);
	}
	public static int FindTarget(Vector3 bulletPosition)
	{
		if (msThis)
			return msThis.FindTargetImpl (bulletPosition);
		return 0;
	}
	public static bool TargetPosition(int id, ref Vector3 targetPosition)
	{
		if (msThis)
			return msThis.TargetPositionImpl(id, ref targetPosition);
		return false;
	}
}
