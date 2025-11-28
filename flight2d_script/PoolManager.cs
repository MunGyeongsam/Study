using UnityEngine;
using System.Collections;

public class PoolManager : MonoBehaviour {

	static PoolManager sPoolManager;

	public GameObject mPrefabBullet;
	public GameObject mPrefabEnemy01;
	public GameObject mPrefabEnemy02;
	public GameObject mPrefabEnemy03;
	public GameObject mPrefabBoom;
	public GameObject mPrefabBoomShip;

	Transform mPoolBullet;
	Transform mPoolEnemy01;
	Transform mPoolEnemy02;
	Transform mPoolEnemy03;
	Transform mPoolBoom;
	Transform mPoolBoomShip;

	PoolManager()
	{
		sPoolManager = this;
	}

	// Use this for initialization
	void Start () {
		mPoolBullet		= transform.Find ("PoolBullet");
		mPoolEnemy01	= transform.Find ("PoolEnemy01");
		mPoolEnemy02	= transform.Find ("PoolEnemy02");
		mPoolEnemy03	= transform.Find ("PoolEnemy03");
		mPoolBoom		= transform.Find ("PoolBoom");
		mPoolBoomShip	= transform.Find ("PoolBoomShip");
	}

	Transform BulletImpl(Transform t)
	{
		if (mPoolBullet.childCount > 0) {

			Transform child = mPoolBullet.GetChild (mPoolBullet.childCount - 1);
			child.SetParent(null);
			Bullet b = child.gameObject.GetComponent("Bullet") as Bullet;
			b.Reset(t);
			return child;

		} else {

			GameObject obj = Instantiate (mPrefabBullet, t.position, t.rotation) as GameObject;
			return obj.transform;
		}
	}
	Transform BoomImpl(Transform t)
	{
		if (mPoolBoom.childCount > 0) {

			Transform child = mPoolBoom.GetChild (mPoolBoom.childCount - 1);
			child.SetParent(null);
			Boom b = child.gameObject.GetComponent("Boom") as Boom;
			b.Reset(t);
			return child;

		} else {

			GameObject obj = Instantiate (mPrefabBoom, t.position, t.rotation) as GameObject;
			return obj.transform;
		}
	}
	Transform BoomShipImpl(Transform t)
	{
		if (mPoolBoomShip.childCount > 0) {

			Transform child = mPoolBoomShip.GetChild (mPoolBoomShip.childCount - 1);
			child.SetParent(null);
			BoomShip b = child.gameObject.GetComponent("BoomShip") as BoomShip;
			b.Reset(t);
			return child;

		} else {

			GameObject obj = Instantiate (mPrefabBoomShip, t.position, t.rotation) as GameObject;
			return obj.transform;
		}
	}
	Transform EnemyImpl(Transform t, Transform pool, GameObject prefab, int type)
	{
		if (pool.childCount > 0) {

			Transform child = pool.GetChild (pool.childCount - 1);
			child.SetParent(null);
			Enemy b = child.gameObject.GetComponent("Enemy") as Enemy;
			b.Reset(t);
			return child;

		} else {

			GameObject obj = Instantiate (prefab, t.position, t.rotation) as GameObject;
			Enemy b = obj.GetComponent ("Enemy") as Enemy;
			b.etype = type;
			return obj.transform;
		}
	}




	//---------------------------------------------------------------------
	// public
	static public void Hide(Bullet instance)
	{
		instance.transform.SetParent (sPoolManager.mPoolBullet);
	}
	static public void Hide(Enemy instance)
	{
		if (1 == instance.etype)
			instance.transform.SetParent (sPoolManager.mPoolEnemy01);
		if (2 == instance.etype)
			instance.transform.SetParent (sPoolManager.mPoolEnemy02);
		if (3 == instance.etype)
			instance.transform.SetParent (sPoolManager.mPoolEnemy03);

		GuidedMissileManager.RemoveEnemy (instance.transform);
	}
	static public void Hide(Boom instance)
	{
		instance.transform.SetParent (sPoolManager.mPoolBoom);
	}
	static public void Hide(BoomShip instance)
	{
		instance.transform.SetParent (sPoolManager.mPoolBoomShip);
	}


	static public Transform Bullet(Transform t)
	{
		return sPoolManager.BulletImpl (t);
	}

	static public Transform Boom(Transform t)
	{
		return sPoolManager.BoomImpl (t);
	}

	static public Transform BoomShip(Transform t)
	{
		return sPoolManager.BoomShipImpl (t);
	}

	static public Transform Enemy(Transform t, int kind)
	{
		if (1 == kind) {
			return sPoolManager.EnemyImpl (t, sPoolManager.mPoolEnemy01, sPoolManager.mPrefabEnemy01, kind);
		} else if (2 == kind) {
			return sPoolManager.EnemyImpl (t, sPoolManager.mPoolEnemy02, sPoolManager.mPrefabEnemy02, kind);
		}

		return sPoolManager.EnemyImpl (t, sPoolManager.mPoolEnemy03, sPoolManager.mPrefabEnemy03, kind);
	}
}
