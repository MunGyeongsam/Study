using UnityEngine;
using System.Collections;

public class Enemy : PoolingBase {

	public int etype = 1;
	float mHP;
	float mLife;
	const float LIFESPAN = 25.0f;
	GUIStyle mGUIStyle;
	GUIContent mGUIContent;

	Transform mHUD;
	Rect mRcHUD;
	Vector2 mHUDSize;

	// Use this for initialization
	void Start () {
		mHUD = transform.Find ("HUD");
		mRcHUD = new Rect ();
		mRcHUD.width = 100;
		mRcHUD.height = 100;

		if (null == mGUIStyle) {
			mGUIStyle = new GUIStyle ();
			mGUIContent = new GUIContent (mHP.ToString ("F2"));
		}
	}
	
	// Update is called once per frame
	void Update () {
		mLife += Time.deltaTime;
		if (mLife >= LIFESPAN) {
			PoolManager.Hide (this);
		}
	}

	void UpdateHUD()
	{
		mGUIContent.text = mHP.ToString ("F2");
		mHUDSize = mGUIStyle.CalcSize (mGUIContent);
	}

	public void Init(float x, float y, float speed, float s, float r, float g, float b)
	{
		Rigidbody2D rigidbody2d = GetComponent<Rigidbody2D> ();
		SpriteRenderer spriteRndr = GetComponent<SpriteRenderer> ();

		mHP = 50.0f * s;

		if (null == mGUIStyle) {
			mGUIStyle = new GUIStyle ();
			mGUIContent = new GUIContent (mHP.ToString ("F2"));
		}
		UpdateHUD ();
		mLife = 0.0f;

		transform.position = new Vector3(x,y,0);
		transform.localScale = new Vector3 (s, s, s);
		rigidbody2d.velocity = new Vector3 (0, -speed, 0);
	
		spriteRndr.color = new Color (r, g, b, 1.0f);

	}


	void OnGUI()
	{
		Vector3 pt = Camera.main.WorldToScreenPoint (mHUD.position);

		mRcHUD.x = pt.x - mHUDSize.x / 2.0f;
		mRcHUD.y = Screen.height - pt.y;
		GUI.Label (mRcHUD, mGUIContent);
		//mRcHUD.y += 20.0f;
		//GUI.Label (mRcHUD, string.Format("screen : {0} x {1}", Screen.width, Screen.height));
	}

	public void AddDamage(float dmg)
	{
		mHP -= dmg;
		UpdateHUD ();
		if (mHP <= 0.0f) {
			PoolManager.Hide (this);
			PoolManager.BoomShip (transform);

			ScreenUI.AddScore (100.0f * transform.localScale.x);
		}
	}
}
