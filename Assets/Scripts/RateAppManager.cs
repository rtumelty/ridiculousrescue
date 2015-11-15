using UnityEngine;
using System.Collections;
using System;

public class RateAppManager : MonoBehaviour {
	static RateAppManager _instance;
	
	public static RateAppManager Instance {
		get {
			if (_instance == null) {
				GameObject go = new GameObject("_RateAppManager");
				_instance = go.AddComponent<RateAppManager>() as RateAppManager;
			}
			return _instance;
		}
	}
	static bool dontAsk = false;

	static bool showingRateDialogue = false;

	public static bool ShowingRateDialogue {
		get {
			return showingRateDialogue;
		}
	}
	
	[SerializeField] int triggersToWaitOnRemind = 5;
	[SerializeField] int triggersToWait = 1;

	[SerializeField] string rateWindowTitle = "Rates Us!";
	[SerializeField] string rateMessage = "Like our game? Let us know!";
	[SerializeField] string rateUrl = "market://details?id=com.companyname.appname";

	// Use this for initialization
	void Awake () {
		if (_instance == null)
				_instance = this;
		else
				Destroy (this);

		DontDestroyOnLoad (gameObject);

		if (PlayerPrefs.HasKey ("alreadyRated"))
				dontAsk = PlayerPrefs.GetInt ("alreadyRated") > 0;
	}
	
	public void OpenRateWindow () {
		if (!dontAsk && !showingRateDialogue) {
			Debug.Log("AndroidNative Rate dialogue - triggers til shown: " + Instance.triggersToWait);
			if (Instance.triggersToWait-- <= 0) {
				#if UNITY_ANDROID && !UNITY_EDITOR
				showingRateDialogue = true;
				AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create (rateWindowTitle, rateMessage, rateUrl);
				rate.addEventListener(BaseEvent.COMPLETE, OnRatePopUpClose);
				#else
				Debug.Log("Android: Open rating popup");
				#endif
			}
		}
	}

	static void OnRatePopUpClose(CEvent e) {
		(e.dispatcher as AndroidRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, OnRatePopUpClose);
		AndroidDialogResult result = (AndroidDialogResult)e.data;
		Debug.Log("Result: " + result + " button pressed");

		switch (result) {
		case AndroidDialogResult.DECLINED:
		case AndroidDialogResult.RATED:
			dontAsk = true;
			PlayerPrefs.SetInt("alreadyRated", 1);
			break;
		default:
			Instance.triggersToWait = Instance.triggersToWaitOnRemind;
			break;
		}
		showingRateDialogue = false;
	}
}
