using UnityEngine;
using System.Collections;

public class Boom : PoolingBase {


	public Sprite[] mSprites;
	SpriteRenderer mSpriteRndr;
	float mAccum;
	int mIndex = 0;

	public float mFreq = 0.3f;

	// Use this for initialization
	void Start () {
		if (null == mSpriteRndr)
			mSpriteRndr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		mAccum += Time.deltaTime;
		if (mAccum >= mFreq) {
			Next ();
			mAccum -= mFreq;
		}
	}

	void Next() {
		if (mIndex < mSprites.Length) {
			mSpriteRndr.sprite = mSprites [mIndex++];
		} else {
			mIndex = 0;

			PoolManager.Hide (this);
		}
	}

	protected override void OnReset(Transform t)
	{
		mIndex = 0;
		if (null == mSpriteRndr)
			mSpriteRndr = GetComponent<SpriteRenderer> ();
		Next ();
	}
}
