using UnityEngine;
using System.Collections;

public class customScreenCloseButton : MonoBehaviour 
{
	//*************************************************************//
	public string onCloseCreatePopupName = "NULL";
	//*************************************************************//
	void OnMouseUp ()
	{
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
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GameGlobalVariables.Stats.METAL_IN_CONTAINERS > GetComponent<AddLivesButton>().myGoldCost)
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
	}
}
