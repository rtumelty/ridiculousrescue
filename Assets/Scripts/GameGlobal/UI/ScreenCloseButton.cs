using UnityEngine;
using System.Collections;

public class ScreenCloseButton : MonoBehaviour 
{
	//*************************************************************//
	public string onCloseCreatePopupName = "NULL";
	//*************************************************************//
	void OnMouseUp ()
	{
		handleTouched ();
	}

	public void Close() {
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		print ("Press State 1");
		if(Camera.main.transform.Find("UI").GetComponent<UIControl>() != null)
		{
			print ("Press State 2");
			UIControl.getInstance().currencyHeaderBar.SetActive(false);
		}
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		transform.parent.gameObject.AddComponent < HideUIElement > ();
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			UIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
			Destroy ( UIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			GlobalVariables.POPUP_UI_SCREEN = false;
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
			UIControl.currentPopupUI = null;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
			Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
			FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			FLGlobalVariables.POPUP_UI_SCREEN = false;
			
			FLUIControl.currentReservingUIElmentObject = null;
			
			FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( false );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
			Destroy ( MNUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			MNGlobalVariables.POPUP_UI_SCREEN = false;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			TRUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
			Destroy ( TRUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
			TRUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			TRGlobalVariables.POPUP_UI_SCREEN = false;
		}
	}
}
