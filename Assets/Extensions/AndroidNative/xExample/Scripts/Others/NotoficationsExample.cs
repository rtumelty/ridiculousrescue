////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;

public class NotoficationsExample : MonoBehaviour {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------




	private int NotificationId = 0;



	void Awake() {
		GoogleCloudMessageService.instance.addEventListener(GoogleCloudMessageService.CLOUD_MESSAGE_SERVICE_REGISTRATION_FAILED, OnRegFailed);
		GoogleCloudMessageService.instance.addEventListener(GoogleCloudMessageService.CLOUD_MESSAGE_SERVICE_REGISTRATION_RECIVED, OnRegstred);
		
		GoogleCloudMessageService.instance.addEventListener(GoogleCloudMessageService.CLOUD_MESSAGE_LOADED, OnMessageLoaded);
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	private void Toast() {
		AndroidToast.ShowToastNotification ("Hello Toast", AndroidToast.LENGTH_LONG);
	}

	private void Local() {
		NotificationId++;
		AndroidNotificationManager.ScheduleLocalNotification("Hello", "This is local notification", 5, NotificationId);


	
	}

	private void CanselLocal() {
		AndroidNotificationManager.CanselLocalNotification(NotificationId);
	}


	private void Reg() {
		GoogleCloudMessageService.instance.RgisterDevice();
	}

	private void LoadLastMessage() {
		GoogleCloudMessageService.instance.LoadLastMessage();
	}

	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnRegFailed() {
		AndroidNative.showMessage ("Reg Failed", "GCM Registration failed :(");
	}
	
	private void OnRegstred() {
		AndroidNative.showMessage ("Regstred", "GCM REG ID: " + GoogleCloudMessageService.instance.registrationId);
	}
	
	private void OnMessageLoaded() {
		AndroidNative.showMessage ("Message Loaded", "Last GCM Message: " + GoogleCloudMessageService.instance.lastMessage);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
