using UnityEngine;
using System.Collections;

public class BG : MonoBehaviour {


	public RenderTexture mRT;
	Texture2D mTexture;
	public Renderer mRndr;
	// Use this for initialization
	void Start () {
		SpriteRenderer srndr = GetComponent<SpriteRenderer> ();
		mTexture = new Texture2D (400, 640, TextureFormat.ARGB32, false);
		srndr.sprite = Sprite.Create (mTexture, new Rect (0, 0, 400, 640), new Vector2 (0.5f, 0.5f));
	}
	void Update() {
		RenderTexture old = RenderTexture.active;
		RenderTexture.active = mRT;

		mTexture.ReadPixels (new Rect (0, 0, 400, 640), 0, 0);
		mTexture.Apply ();

		RenderTexture.active = old;

		//UnityEngine.UI.Toggle;
	}
		
	//void OnRenderObject () {
	//	Graphics.Blit (mRT, Camera.main.targetTexture);
	//}
}
