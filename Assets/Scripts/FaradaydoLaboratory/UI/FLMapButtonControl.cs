using UnityEngine;
using System.Collections;

public class FLMapButtonControl : MonoBehaviour
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
		if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null)
		{
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
		}
		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( true );
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