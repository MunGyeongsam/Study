using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundGen : MonoBehaviour {

	Camera mCamera;
	Mesh mMesh;
	Matrix4x4 mMatrix;
	public Material mMaterial;
	public RenderTexture mRenderTexture;

	Vector3 mPos;
	Vector3 mScale;
	Color mColor;

	struct E {
		public float x;
		public float y;
		public float r;

		public E(float x, float y, float r)
		{
			this.x = x;
			this.y = y;
			this.r = r;
		}

	}

	List<E>	mList;
	bool mCleared;


	Mesh CreateCircle(int nAngle)
	{
		Mesh mesh = new Mesh();

		Vector3[] vtx = new Vector3[nAngle+1];
		int[] triangle = new int[nAngle * 3];

		vtx [0].x = vtx [0].y = vtx [0].z = 0.0f;
		float angleStep = Mathf.PI * 2.0f / nAngle;
		float angle = 0.0f;
		for (int i = 1; i <= nAngle; ++i) {
			vtx [i].x = Mathf.Cos (angle);
			vtx [i].y = Mathf.Sin (angle);
			vtx [i].z = 0.0f;

			angle += angleStep;
		}

		int ti = 0;
		for (int i = 0; i < nAngle-1; ++i) {
			triangle [ti + 0] = 0;
			triangle [ti + 1] = i+1;
			triangle [ti + 2] = i+2;

			ti += 3;
		}
		triangle [ti + 0] = 0;
		triangle [ti + 1] = nAngle;
		triangle [ti + 2] = 1;

		mesh.vertices = vtx;
		mesh.triangles = triangle;

		mesh.RecalculateNormals();

		return mesh;
	}
	// Use this for initialization
	void Start () {

		mCamera = GameObject.Find ("BackgroundCamera").GetComponent ("Camera") as Camera;
		mList = new List<E> (128);

		mMatrix = Matrix4x4.identity;

		mPos = new Vector3 (0, 0, 0);
		mScale = new Vector3 (1, 1, 1);
		mColor = new Color (1,1,1,1);

		mMesh = CreateCircle(36);
	}

	void Clear()
	{
		RenderTexture old = RenderTexture.active;
		RenderTexture.active = mRenderTexture;

		GL.Begin(GL.TRIANGLES);
		GL.Clear(true, true, new Color(.1f, .1f, .1f, 1));
		GL.End();

		RenderTexture.active = old;

		mCleared = true;
	}

	void DrawCircle()
	{
		float c = Random.Range (0.2f, 0.7f);
		mColor.r = c;
		mColor.g = c;
		mColor.b = c;
		mMaterial.color = mColor;

		int cnt = 0;
		float x, y, r;

		for (int i = 0; i < 100; ++i) {

			x = Random.Range (-2.0f, 2.0f);
			y = Random.Range (-3.2f, 3.2f);
			r = Random.Range (0.05f, 1.0f);

			if (AddToList (x, y, r)) {

				mPos.x = x;
				mPos.y = y;
				mScale.x = mScale.y = mScale.z = r;
				mMatrix = Matrix4x4.TRS (mPos, Quaternion.identity, mScale);
				Graphics.DrawMesh (mMesh, mMatrix, mMaterial, 8);

				if (y + r > 3.2f) {

					mPos.y = y-6.4f;
					mMatrix = Matrix4x4.TRS (mPos, Quaternion.identity, mScale);
					Graphics.DrawMesh (mMesh, mMatrix, mMaterial, 8);
				}
				if (y - r < -3.2f) {

					mPos.y = y+6.4f;
					mMatrix = Matrix4x4.TRS (mPos, Quaternion.identity, mScale);
					Graphics.DrawMesh (mMesh, mMatrix, mMaterial, 8);
				}

				if ((++cnt) >= 20)
					break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mCleared) {
			DrawCircle ();
		} else {
			Clear ();
		}
	}

	bool AddToList(float x, float y, float r)
	{
		if (mList.Count == 0) {
			mList.Add (new E (x, y, r));
			return true;
		}

		bool rt = true;
		E e;
		float dx, dy, minr;
		for (int i = 0; i < mList.Count; ++i) {
			e = mList [i];
			dx = x - e.x;
			dy = y - e.y;
			minr = r + e.r;

			// 충돌검사
			if ((dx * dx + dy * dy) <= minr*minr) {
				rt = false;
				break;
			}

			if (e.y + e.r > 3.2f) {
				dy = y - (e.y-6.4f);
				if ((dx * dx + dy * dy) <= minr*minr) {
					rt = false;
					break;
				}
			}
			if (e.y - e.r < -3.2f) {
				dy = y - (e.y+6.4f);
				if ((dx * dx + dy * dy) <= minr*minr) {
					rt = false;
					break;
				}
			}
		}

		if (rt) {
			e.x = x;
			e.y = y;
			e.r = r;
			mList.Add (e);
		}

		return rt;
	}

}
