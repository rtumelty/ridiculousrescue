using UnityEngine;
using System.Collections;

public class BlackOutTouchControl : MonoBehaviour 
{
	//*************************************************************//
	public string onCloseCreatePopupName = "NULL";
	//*************************************************************//
	private bool _iAmTouched = false;
	//*************************************************************//	
	void OnMouseUp ()
	{
		if ( _iAmTouched ) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		_iAmTouched = true;
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			GlobalVariables.POPUP_UI_SCREEN = false;
			if ( UIControl.currentPopupUI ) UIControl.currentPopupUI.AddComponent < HideUIElement > ();
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			
			if ( onCloseCreatePopupName == "bringBackResultScreen" )
			{
				Destroy ( UIControl.currentPopupUI );
				UIControl.currentPopupUI = null;
				
				ResoultScreen.getInstance ().camera.depth = 100;
				Camera.main.depth = 0;
			}
			else if ( onCloseCreatePopupName != "NULL" )
			{
				Destroy ( UIControl.currentPopupUI );
				UIControl.currentPopupUI = null;
				UIControl.getInstance ().createPopup (( GameObject ) Resources.Load ( onCloseCreatePopupName ));
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLGlobalVariables.POPUP_UI_SCREEN = false;
			if ( FLUIControl.currentPopupUI ) FLUIControl.currentPopupUI.AddComponent < HideUIElement > ();
			FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			
			FLUIControl.currentReservingUIElmentObject = null;
			
			FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( false );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNGlobalVariables.POPUP_UI_SCREEN = false;
			if ( MNUIControl.currentPopupUI ) MNUIControl.currentPopupUI.AddComponent < HideUIElement > ();
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			TRGlobalVariables.POPUP_UI_SCREEN = false;
			if ( TRUIControl.currentPopupUI ) TRUIControl.currentPopupUI.AddComponent < HideUIElement > ();
			TRUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}

		
		gameObject.AddComponent < AlphaDisapearAndDestory > ();
		
		Destroy ( GetComponent < BoxCollider > ());
	}
}
