using UnityEngine;
using System.Collections;

public class LoadingIndicator : MonoBehaviour {
	static LoadingIndicator _instance;
	bool coroutineActive = false;
	
	public static LoadingIndicator Instance {
		get {
			if (_instance == null) {
				GameObject go = new GameObject("_LoadingIndicator");
				_instance = go.AddComponent<LoadingIndicator>() as LoadingIndicator;
			}
			return _instance;
		}
	}
	void Awake() {
		if (_instance == null)
			_instance = this;
		else
			Destroy (this);
		DontDestroyOnLoad (gameObject);
	}

	IEnumerator OnLevelWasLoaded(int level) {
		Debug.Log ("Level loaded: " + level + ", coroutine active: " + coroutineActive);
		if (!coroutineActive) {
			if (level == 5) coroutineActive = true;

			switch (level) {
			case 3:
			case 7:
				yield return new WaitForSeconds(Time.deltaTime);
				AndroidNativeUtility.HidePreloader ();
				break;
			case 5:
				yield return new WaitForSeconds(Time.deltaTime);
				AndroidNativeUtility.HidePreloader ();

				Debug.Log ("Map level loaded - attempting to open rate dialogue");
				RateAppManager.Instance.OpenRateWindow ();


				if (GameGlobalVariables.SHOW_ADS) {
					if (LevelControl.LEVEL_ID > 4) {
							Debug.Log ("Level id: " + LevelControl.LEVEL_ID);
							ChartboostCall.getInstance ().showAdd ();
					}
				}

				break;
			}
			yield return new WaitForSeconds(1);
			coroutineActive = false;
		}
	}
}
