using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Rigidbody2D mRigidbody2D;
	Vector2 mVelocity;
	float mHP;

	public float mSpeed = 2.5f;
	public float mKd = 10.0f;

	Vector2 mTargetVelocity;

	// Use this for initialization
	void Start () {
		mRigidbody2D = GetComponent<Rigidbody2D> ();
		mVelocity.Set (0, 0);

		mHP = 100.0f;
		mTargetVelocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
		ScreenUI.SetHP(mHP);
		MouseControl ();
	}

	void MouseControl()
	{
		if (Input.GetMouseButton (0) == false) {
			
			mRigidbody2D.velocity = Vector2.zero;

		} else {
			
			Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//target.y = Screen.height - target.y;

			mTargetVelocity.x = target.x - transform.position.x;
			mTargetVelocity.y = target.y - transform.position.y;

			if (mTargetVelocity.SqrMagnitude () <= 0.01f) {
				mTargetVelocity.x = 0.0f;
				mTargetVelocity.y = 0.0f;
			} else {
				mTargetVelocity.Normalize ();
				mTargetVelocity.x *= mSpeed;
				mTargetVelocity.y *= mSpeed;
			}

			mRigidbody2D.velocity = mRigidbody2D.velocity + (mTargetVelocity - mRigidbody2D.velocity) * Time.deltaTime * mKd;
		}
	}

	void KeyboardControl()
	{
		float x = 0.0f;
		if (Input.GetKey (KeyCode.LeftArrow)) {
			x -= 1.0f;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			x += 1.0f;
		}
		float y = 0.0f;
		if (Input.GetKey (KeyCode.UpArrow)) {
			y += 1.0f;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			y -= 1.0f;
		} 

		mVelocity.Set (x, y);
		mVelocity.Normalize ();
		mRigidbody2D.velocity = mVelocity * mSpeed;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy") {
			mHP -= 5.0f;

			Enemy e = other.GetComponent ("Enemy") as Enemy;
			PoolManager.BoomShip (e.transform);
			PoolManager.Hide (e);
		}
	}
			
}
