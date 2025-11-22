using UnityEngine;
using System.Collections;

public class ScreenUI : MonoBehaviour {

	static ScreenUI msThis;

	UnityEngine.UI.Text mTextHP;
	UnityEngine.UI.Text mTextScore;
	UnityEngine.UI.Text mTextFPS;
	UnityEngine.UI.Toggle mToggleBackground;
	float mScore = 0.0f;
	float mTargetScore = 0.0f;
	// Use this for initialization

	public GameObject	mBackgound01;
	public GameObject	mBackgound02;

	float[] mDeltaTimes;
	int mDetaTimesIndex;
	float m10DeltaTime;
	void Start () {
		msThis = this;

		RectTransform rct = transform as RectTransform;

		mTextHP = rct.Find ("HP").GetComponent("Text") as UnityEngine.UI.Text;
		mTextScore = rct.Find ("Score").GetComponent("Text") as UnityEngine.UI.Text;
		mTextFPS = rct.Find ("FPS").GetComponent("Text") as UnityEngine.UI.Text;
		mTextScore.text = "score : 0";

		mToggleBackground = rct.Find ("ToggleBG").GetComponent("Toggle") as UnityEngine.UI.Toggle;

		//mBGM.PlayDelayed (1.0f);

		mDeltaTimes = new float[10];
		mDetaTimesIndex = 0;
		m10DeltaTime = 0.0f;
	}

	void AddScoreImpl(float score)
	{
		mTargetScore += score;
	}

	void SetHPImpl(float hp)
	{
		mTextHP.text = "HP : " + hp.ToString ("F2");
	}


	void Update()
	{
		if (mTargetScore != mScore) {
			mScore = mScore + (mTargetScore - mScore) * Time.deltaTime * 13.0f;
			mTextScore.text = "score : " + mScore.ToString ("F2");
		}

		m10DeltaTime += Time.deltaTime;
		mDeltaTimes [mDetaTimesIndex++] = Time.deltaTime;

		int index = mDetaTimesIndex;
		if (index >= mDeltaTimes.Length)
			index = 0;
		m10DeltaTime -= mDeltaTimes [index];

		if (mDetaTimesIndex >= mDeltaTimes.Length)
			mDetaTimesIndex = 0;
			

		mTextFPS.text = (10.0f / m10DeltaTime).ToString("F2");
	}
	public static void AddScore(float score)
	{
		if (msThis)
			msThis.AddScoreImpl (score);
	}
	public static void SetHP(float hp)
	{
		if (msThis)
			msThis.SetHPImpl (hp);
	}


	public void MH_SwitchBackground()
	{
		bool isChecked = mToggleBackground.isOn;

		if (isChecked) {
			mBackgound01.SetActive (true);
			mBackgound02.SetActive (false);
		} else {
			mBackgound01.SetActive (false);
			mBackgound02.SetActive (true);
		}
			
	}
}
