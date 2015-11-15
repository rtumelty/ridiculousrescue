using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class ChartboostCall : MonoBehaviour 
{
	//*************************************************************//
	private static ChartboostCall _meInstance;
	public static ChartboostCall getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//

	[SerializeField] GameObject touchBlocker;

	void Awake ()
	{
		if (_meInstance == null) {
						_meInstance = this;
						DontDestroyOnLoad (gameObject);

			if (touchBlocker != null) {
				touchBlocker = Instantiate(touchBlocker) as GameObject;
				DontDestroyOnLoad(touchBlocker);
				touchBlocker.SetActive(false);
			}
#if UNITY_ANDROID && ! UNITY_EDITOR
		// Initialize the Chartboost plugin
//		Chartboost.init("5407330089b0bb6d2d15c6e5", "a0bd3f78f81c781a7d647bb8ce493e93076adaad");
		Debug.Log ("Chartboost: Pre-Cache ad");
		Chartboost.cacheInterstitial(CBLocation.LevelComplete);
#endif
				}
	}

	void OnEnable() 
	{
		#if UNITY_ANDROID && ! UNITY_EDITOR
		Chartboost.didCloseInterstitial += OnFinishedInterstitial;
		Chartboost.didDismissInterstitial += OnFinishedInterstitial;
		Chartboost.didClickInterstitial += OnFinishedInterstitial;
		#endif
	}

	public void Update() 
	{
		#if UNITY_ANDROID && ! UNITY_EDITOR
		// Handle the Android back button (only if impressions are set to not use activities)
		if (Input.GetKeyUp(KeyCode.Escape)) 
		{
			// Check if Chartboost wants to respond to it
		}
		#endif
	}
	
	void OnDisable() 
	{
		#if UNITY_ANDROID && ! UNITY_EDITOR
		Chartboost.didCloseInterstitial -= OnFinishedInterstitial;
		Chartboost.didDismissInterstitial -= OnFinishedInterstitial;
		Chartboost.didClickInterstitial -= OnFinishedInterstitial;
		#endif
	}

	public void showAdd ()
	{
		StartCoroutine(_showAdd());
	}

	IEnumerator _showAdd() {
		Debug.Log ("Chartboost - attempting to show ad");
		AndroidNative.HidePreloader ();
		yield return new WaitForSeconds (Time.deltaTime);
		if (Chartboost.hasInterstitial (CBLocation.LevelComplete)) {
			Debug.Log ("Chartboost ad cached - showing");
			Time.timeScale = 0;

			if (touchBlocker != null) {
				touchBlocker.transform.position = Camera.main.transform.position - Vector3.up;
				touchBlocker.SetActive(true);
			}

			Chartboost.showInterstitial (CBLocation.LevelComplete);
		} else {
			Chartboost.cacheInterstitial(CBLocation.LevelComplete);
			Debug.Log ("Chartboost - No ad cached");
		}
	}
	
	void OnGUI() 
	{
		#if UNITY_ANDROID && ! UNITY_EDITOR
		// Disable user input for GUI when impressions are visible
		// This is only necessary on Android if we have disabled impression activities
		//   by having called Chartboost.init(ID, SIG, false), as that allows touch
		//   events to leak through Chartboost impressions
		GUI.enabled = !Chartboost.isImpressionVisible();
		#endif
	}

	void OnFinishedInterstitial(CBLocation location) {
		Time.timeScale = 1;
		
		if (touchBlocker != null) {
			touchBlocker.SetActive(false);
		}
		Debug.Log ("Cache ad after displayed");
		Chartboost.cacheInterstitial (CBLocation.LevelComplete);
		return;
	}
}
