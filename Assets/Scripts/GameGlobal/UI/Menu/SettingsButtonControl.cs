using UnityEngine;
using System.Collections;

public class SettingsButtonControl : MonoBehaviour 
{
	//*************************************************************//
	private GameObject _settingsPrefab;
	//*************************************************************//
	void Awake ()
	{
		_settingsPrefab = ( GameObject ) Resources.Load ( "UI/settingsUIScreen" );
	}
	
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			GlobalVariables.POPUP_UI_SCREEN = true;
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			if ( UIControl.currentPopupUI ) Destroy ( UIControl.currentPopupUI );
			
			GameObject settingsScreen = ( GameObject ) Instantiate ( _settingsPrefab, Camera.main.transform.position + Vector3.down * 3.8f, _settingsPrefab.transform.rotation );
			settingsScreen.transform.parent = Camera.main.transform;
			UIControl.currentPopupUI = settingsScreen;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLGlobalVariables.POPUP_UI_SCREEN = true;
			FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			if ( FLUIControl.currentPopupUI ) Destroy ( FLUIControl.currentPopupUI );
			
			GameObject settingsScreen = ( GameObject ) Instantiate ( _settingsPrefab, Camera.main.transform.position + Vector3.down * 4f, _settingsPrefab.transform.rotation );
			settingsScreen.transform.parent = Camera.main.transform;
			FLUIControl.currentPopupUI = settingsScreen;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNGlobalVariables.POPUP_UI_SCREEN = true;
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			if ( MNUIControl.currentPopupUI ) Destroy ( MNUIControl.currentPopupUI );
			
			GameObject settingsScreen = ( GameObject ) Instantiate ( _settingsPrefab, Camera.main.transform.position + Vector3.down * 4f, _settingsPrefab.transform.rotation );
			settingsScreen.transform.parent = Camera.main.transform;
			MNUIControl.currentPopupUI = settingsScreen;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			TRGlobalVariables.POPUP_UI_SCREEN = true;
			TRUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			if ( TRUIControl.currentPopupUI ) Destroy ( TRUIControl.currentPopupUI );
			
			GameObject settingsScreen = ( GameObject ) Instantiate ( _settingsPrefab, Camera.main.transform.position + Vector3.down * 4f, _settingsPrefab.transform.rotation );
			settingsScreen.transform.parent = Camera.main.transform;
			TRUIControl.currentPopupUI = settingsScreen;
		}
	}
}
