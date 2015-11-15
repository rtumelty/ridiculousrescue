using UnityEngine;
using System.Collections;

public class ManuButtonControl : MonoBehaviour 
{
	//*************************************************************//
	private GameObject _menuPrefab;
	//*************************************************************//
	void Awake ()
	{
		_menuPrefab = ( GameObject ) Resources.Load ( "UI/menuUIScreen" );
	}
	
	void OnMouseUp ()
	{
		//if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.MISSION_SCREEN ) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
		{
			print ("FFS2");
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
		}

		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( GlobalVariables.MENU_FOR_TIP ) TipOnClickComponent.CURRENT_TIP.deActivate ();

			GameObject menuScreen = UIControl.getInstance ().createPopup ( _menuPrefab );
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
			GameObject menuScreen = FLUIControl.getInstance ().createPopup ( _menuPrefab );
			if ( menuScreen != null )
			{
				menuScreen.transform.Find ( "returnToLabButton" ).gameObject.SetActive ( false );
				menuScreen.transform.Find ( "settingsButton" ).position += Vector3.forward * 0.8f;
				menuScreen.transform.Find ( "exitButton" ).position += Vector3.forward * 0.4f;
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.getInstance ().createPopup ( _menuPrefab );			
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			TRUIControl.getInstance ().createPopup ( _menuPrefab );			
		}
	}
}
