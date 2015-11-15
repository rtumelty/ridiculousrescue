using UnityEngine;
using System.Collections;

public class FLMissionScreenBackButton : MonoBehaviour 
{
	private float _countUpdate = 0f;

	void OnMouseUp ()
	{
		if ( FLGlobalVariables.POPUP_UI_SCREEN || FLGlobalVariables.TUTORIAL_MENU || GameGlobalVariables.BLOCK_LAB_ENTERED || FLMissionRoomManager.AFTER_INTRO ) return;
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( false );
	}

	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;
		if(GameGlobalVariables.BLOCK_LAB_ENTERED == true)
		{
			gameObject.SetActive(false);
		}
	}
}
