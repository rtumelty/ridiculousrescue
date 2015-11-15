using UnityEngine;
using System.Collections;

public class GoldButtonControl : MonoBehaviour {
	//*************************************************************//
	private GameObject _buyGoldPrefab;
	private bool external = false;
	private bool fromDirectTouch = false;
	//*************************************************************//
	void Awake ()
	{
		_buyGoldPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/getGoldUIPanel" );
	}
	
	void OnMouseUp ()
	{
		//if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.MISSION_SCREEN ) return;
		if(GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE)
		{
			handleTouched (GameObject.Find("CameraResoultScreen").transform.Find ("background").transform.Find("uiPivot2").transform);
		}
		else
		{
			handleTouched();
		}
	}

	void Activate (Transform thisPoint = null)
	{
		_buyGoldPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/getGoldUIPanel" );
		external = true;
		handleTouched (thisPoint);
	}
	
	private void handleTouched (Transform thisPoint = null)
	{
		if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
		{
			print ("FFS2");
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
		}
		else if(Camera.main.transform.Find("UI").GetComponent<UIControl>() != null)
		{
			print ("WTF");

			Destroy(UIControl.currentPopupUI);
			UIControl.currentPopupUI = null;
			UIControl.getInstance().createPopup(UIControl.getInstance()._getGoldUI, true, "NULL", thisPoint);
			print ("AT THIS POINT " + thisPoint);
			thisPoint = null;
		}
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			print ("This");
			if ( GlobalVariables.MENU_FOR_TIP ) TipOnClickComponent.CURRENT_TIP.deActivate ();
			
			GameObject menuScreen = UIControl.getInstance ().createPopup ( _buyGoldPrefab, false );
			/*
			if ( GlobalVariables.TUTORIAL_MENU )
			{
				if ( menuScreen != null )
				{
					menuScreen.transform.Find ( "returnToLabButton" ).gameObject.SetActive ( false );
					menuScreen.transform.Find ( "settingsButton" ).position += Vector3.forward * 0.8f;
					menuScreen.transform.Find ( "exitButton" ).position += Vector3.forward * 0.4f;
				}
			}
			*/

		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			print ("Or this");
			print (_buyGoldPrefab);
			GameObject menuScreen = FLUIControl.getInstance ().createPopup ( _buyGoldPrefab, external );
			if(external == true)
			{
				menuScreen.transform.Find("textGeneral").GetComponent<GameTextControl>().myKey = "ui_sign_youneedgold";
			}
			external = false;
			/*if ( menuScreen != null )
			{
				menuScreen.transform.Find ( "returnToLabButton" ).gameObject.SetActive ( false );
				menuScreen.transform.Find ( "settingsButton" ).position += Vector3.forward * 0.8f;
				menuScreen.transform.Find ( "exitButton" ).position += Vector3.forward * 0.4f;
			}*/

		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			print ("Or this even");
			MNUIControl.getInstance ().createPopup ( _buyGoldPrefab );			
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			print ("Or this WHAT?");
			TRUIControl.getInstance ().createPopup ( _buyGoldPrefab );			
		}
	}
}
