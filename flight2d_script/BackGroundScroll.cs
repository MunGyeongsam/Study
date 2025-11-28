using UnityEngine;
using System.Collections;

public class BackGroundScroll : MonoBehaviour {

	public float mSpeed = 0.0f;
	Vector3 mPosition;
	// Use this for initialization
	void Start () {
		mPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		mPosition.y -= Time.deltaTime * mSpeed;
		if (mPosition.y <= -Camera.main.orthographicSize*2.0f)
			mPosition.y += Camera.main.orthographicSize*4.0f;
		transform.position = mPosition;
	}
}
