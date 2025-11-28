using UnityEngine;
using System.Collections;

public class EnemySpot : MonoBehaviour {

	public GameObject[] mPrefabs;
	int mNum;
	float mMinX;
	float mMaxX;
	float mY;

	public float mFreq = 0.5f;
	float mAccum = 0.0f;

	// Use this for initialization
	void Start () {
		mNum = mPrefabs.Length;

		Vector3 l = transform.FindChild ("left").position;
		Vector3 r = transform.FindChild ("right").position;

		mMinX = l.x;
		mMaxX = r.x;
		mY = transform.position.y;

		//Debug.Log(System.Environment.Version);
	}
	
	// Update is called once per frame
	void Update () {
		mAccum += Time.deltaTime;

		if (mAccum >= mFreq) {
			mAccum -= mFreq;

			int index = Random.Range (1, mNum+1);
			Transform t = PoolManager.Enemy (transform, index);
			Enemy e = t.gameObject.GetComponent ("Enemy") as Enemy;
			//Debug.LogFormat("eid : {0}", e.GetInstanceID ());

			float x = Random.Range (mMinX, mMaxX);
			float speed = Random.Range (5.0f, 7.5f);
			float scale = Random.Range(0.8f, 1.0f);
			float r = Random.Range (0.3f, 0.5f);
			e.Init (x, mY, speed, scale, r, r, r);

			GuidedMissileManager.AddEnemy (t);
		}
	}
}
