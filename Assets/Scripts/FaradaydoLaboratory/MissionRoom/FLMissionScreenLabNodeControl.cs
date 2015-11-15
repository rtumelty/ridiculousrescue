using UnityEngine;
using System.Collections;

public class FLMissionScreenLabNodeControl : MonoBehaviour 
{
	//*************************************************************//
	private bool _checkIfMapDragged = false;
	//*************************************************************//
	void OnMouseUp ()
	{
		if ( _checkIfMapDragged )
		{
			if ( FLGlobalVariables.MAP_DRAGGED )
			{
				_checkIfMapDragged = false;
				return;
			}
		}

		if ( FLGlobalVariables.POPUP_UI_SCREEN || FLGlobalVariables.TUTORIAL_MENU || GameGlobalVariables.BLOCK_LAB_ENTERED || FLMissionRoomManager.AFTER_INTRO ) return;

		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}

	void OnMouseDown ()
	{
		_checkIfMapDragged = true;
	}
	
	private void handleTouched ()
	{
		FLMissionRoomManager.getInstance ().forceFinishShowMapAnimation ();
		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( false, true, false, false, true );
	}
}
