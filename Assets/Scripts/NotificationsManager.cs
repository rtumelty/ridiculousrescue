using UnityEngine;
using System.Collections;
using System;

public class NotificationsManager : MonoBehaviour {
	static NotificationsManager instance;
	public static NotificationsManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject("_NotificationsManager");
				instance = go.AddComponent<NotificationsManager>();
			}

			return instance;
		}
	}


	[SerializeField] string returnTitle = "We miss you!"; 
	[SerializeField] string returnMessage = "Please come back, we'll be your friends...";
	[SerializeField] float messageDelayInDays = 3;
	
	static int lastNotificationId = 0;
	static int lastLivesNotificationId = 0;
	static int comeBackNotificationId = 0;

	// Use this for initialization
	void Awake () {
		if (instance != null)
				Destroy (this);
		else
				instance = this;

		DontDestroyOnLoad (this);
	}

	void OnEnable() {
		
		if (PlayerPrefs.HasKey ("lastNotificationId"))
			lastNotificationId = PlayerPrefs.GetInt ("lastNotificationId");
		
		if (PlayerPrefs.HasKey ("comeBackNotificationId")) {
			comeBackNotificationId = PlayerPrefs.GetInt ("comeBackNotificationId");
			AndroidNotificationManager.CanselLocalNotification(comeBackNotificationId);
		}
		
		int newNotificationTime = (int)Time.time + (int)(messageDelayInDays * 24 * 60 * 60);
		AndroidNotificationManager.ScheduleLocalNotification(returnTitle, returnMessage, newNotificationTime, ++lastNotificationId);
		comeBackNotificationId = lastNotificationId;
	}

	void OnDisable() {
		PlayerPrefs.SetInt ("lastNotificationId", lastNotificationId);
		PlayerPrefs.SetInt ("comeBackNotificationId", comeBackNotificationId);
	}
	
	public void ScheduleLivesNotification () {
		int time = (GameGlobalVariables.Stats.MAX_LIVES - GameGlobalVariables.Stats.LIVES_AVAILABLE)
			* GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION;

		AndroidNotificationManager.CanselLocalNotification (lastLivesNotificationId);
		AndroidNotificationManager.ScheduleLocalNotification ("Full lives!", 
		                                                      "You have full lives! Come back and play!", 
		                                                      time, ++lastNotificationId);
		lastLivesNotificationId = lastNotificationId;
	}
}
