using UnityEngine;
using System.Collections;

public class FL_MOMNotEnoughResourcesGoMiningButtonControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		transform.parent.gameObject.AddComponent < HideUIElement > ();
		FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
		Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
		
		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( true, true, false, true );

		FLGlobalVariables.POPUP_UI_SCREEN = false;
	}
}
