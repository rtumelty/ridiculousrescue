using UnityEngine;
using System.Collections;

public class FLMissionGoToFactoryButtonControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
		Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		FLGlobalVariables.POPUP_UI_SCREEN = false;
			
		FLUIControl.currentReservingUIElmentObject = null;
		Destroy ( FLUIControl.currentPopupUI );
		FLUIControl.currentPopupUI = null;

		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( false );
		FLUIControl.getInstance ().changeRoomInDirection ( Vector3.right );
		//Vector3 positionToGo = new Vector3 ( -6.5f + 12f, Camera.main.transform.position.y, 4f + 9.25f );
		//Camera.main.transform.position = positionToGo;

		if ( ! FLGlobalVariables.TUTORIAL_MENU )
		{
			FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.Find ( "tile" ).gameObject.SendMessage ( "handleTouched" );
			TutorialsManager.getInstance ().turnOnDragMomObjectTutorial ();
		}
	}
}
