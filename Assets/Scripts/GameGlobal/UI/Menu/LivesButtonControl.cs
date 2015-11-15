using UnityEngine;
using System.Collections;

public class LivesButtonControl : MonoBehaviour {
	//*************************************************************//
	public GameObject _buyLivesPrefab;
	//*************************************************************//

	void Awake ()
	{
		_buyLivesPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/getLivesUIPanel" );
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
		handleTouched ();
	}
	
	private void handleTouched ( Transform atThisPoint = null)
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

			UIControl.getInstance().createPopup(UIControl.getInstance()._getLivesUI, true, "NULL", atThisPoint);

		}
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( GlobalVariables.MENU_FOR_TIP ) TipOnClickComponent.CURRENT_TIP.deActivate ();
			
			GameObject menuScreen = UIControl.getInstance ().createPopup ( _buyLivesPrefab );
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
			print ("DUNIT");
			GameObject menuScreen = FLUIControl.getInstance ().createPopup ( _buyLivesPrefab );
			/*if ( menuScreen != null )
			{
				menuScreen.transform.Find ( "returnToLabButton" ).gameObject.SetActive ( false );
				menuScreen.transform.Find ( "settingsButton" ).position += Vector3.forward * 0.8f;
				menuScreen.transform.Find ( "exitButton" ).position += Vector3.forward * 0.4f;
			}*/
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.getInstance ().createPopup ( _buyLivesPrefab );			
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			TRUIControl.getInstance ().createPopup ( _buyLivesPrefab );			
		}
	}
}
