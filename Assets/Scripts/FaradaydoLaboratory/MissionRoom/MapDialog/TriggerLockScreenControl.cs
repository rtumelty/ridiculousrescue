using UnityEngine;
using System.Collections;

public class TriggerLockScreenControl : MonoBehaviour 
{
	//*************************************************************//
	private GameObject _lockScreenPrefab;
	//*************************************************************//
	void Awake ()
	{
		_lockScreenPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/lockUIScreen" );
	}

	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLMissionScreenMapDialogManager.CURRENT_LOCKED_LEVEL_NODE = this.gameObject;
		GameObject lockScreen = FLUIControl.getInstance ().createPopup ( _lockScreenPrefab );
	}
}
