using UnityEngine;
using System.Collections;

public class Bullet : PoolingBase {


	Rigidbody2D	mRigidbody2D;
	float mSpeed = 10.0f;
	float mLife = 0.0f;
	public float LIFESPAN = 2.5f;

	int mTargetID;
	Quaternion mTargetRotation;


	// Use this for initialization
	void Start () {
		mRigidbody2D = GetComponent<Rigidbody2D> ();
		Vector3 tmp = transform.TransformDirection(new Vector3(1,0));

		mRigidbody2D.velocity = new Vector2(mSpeed*tmp.x, mSpeed*tmp.y);
	}
	
	// Update is called once per frame
	void Update () {
		mLife += Time.deltaTime;
		if (mLife >= LIFESPAN) {
			PoolManager.Hide (this);
		} else if (0 != mTargetID) {
			UpdateVelocity ();
		}
	}


	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Enemy") {
			PoolManager.Hide (this);
			PoolManager.Boom (transform);

			Enemy e = other.GetComponent ("Enemy") as Enemy;
			e.AddDamage (13.0f);
		}
	}

	protected override void OnReset(Transform t)
	{
		mLife = 0.0f;
		mTargetID = 0;

		UpdateVelocity ();
	}

	public void SetTarget(int targetID)
	{
		//Debug.LogFormat ("targetID : {0}", targetID);
		mTargetID = targetID;
	}

	void UpdateVelocity()
	{
		if (0 != mTargetID) {
			TowardTarget ();

			//transform.rotation = mTargetRotation;
			//transform.rotation = Quaternion.Lerp (transform.rotation, mTargetRotation, Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, mTargetRotation, 5.5f);
		}

		Vector3 dir = transform.rotation * Vector3.right;
		mRigidbody2D.velocity = new Vector2 (mSpeed * dir.x, mSpeed * dir.y);
	}
	void TowardTarget()
	{
		Vector3 tagetPosition = Vector3.zero;
		bool check = GuidedMissileManager.TargetPosition(mTargetID, ref tagetPosition);

		if (check) {
			Vector3 fromTo = tagetPosition - transform.position;
			//fromTo.z = 0.0f;
			//fromTo.Normalize ();
			//mTargetRotation = Quaternion.LookRotation (-Vector3.forward, fromTo);

			float angle = Mathf.Atan2(fromTo.y,fromTo.x) * Mathf.Rad2Deg;
			mTargetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		} else {
			mTargetID = 0;
		}
	}
}
