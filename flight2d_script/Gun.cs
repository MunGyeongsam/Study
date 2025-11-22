using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public float mFreq = 0.15f;
	public bool mGuided;


	float mAccum = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mAccum += Time.deltaTime;
		if (mAccum >= mFreq) {
			mAccum -= mFreq;

			Transform b = PoolManager.Bullet (transform);
			if (mGuided && Input.GetKey(KeyCode.LeftShift)) {
				//Debug.LogFormat("bid : {0}", b.GetInstanceID ());

				int targetID = GuidedMissileManager.FindTarget (transform.position);
				b.SendMessage ("SetTarget", targetID);
			}
		}
	}
}
