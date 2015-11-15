using UnityEngine;
using System.Collections;

public class ButtonBackToPreviousMap : MonoBehaviour 
{
	//*************************************************************//
	public bool goRight = false;
	//*************************************************************//
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.TUTORIAL_MENU ) return;
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLMissionScreenDragRotateControl.getInstance ().blockDraging ();
		FLMissionRoomManager.getInstance ().changeWorldInDirection ( goRight ? Vector3.left : Vector3.right, 30f );
		//FLMissionRoomManager.getInstance ().setWorldRotation ();
	}
	
	void Update ()
	{
		if ( goRight )
		{
			if ( FLMissionRoomManager.getInstance ().getCurrentWorldID () == 1 )
			{
				GetComponent < AlphaApear > ().startFadeIn ();
				collider.enabled = true;
			}
			else if ( FLMissionRoomManager.getInstance ().getCurrentWorldID () == 2 )
			{
				GetComponent < AlphaApear > ().startFadeOut ();
				collider.enabled = false;
			}
			else
			{
				GetComponent < AlphaApear > ().startFadeIn ();
				collider.enabled = true;
			}
		}
		else
		{
			if ( FLMissionRoomManager.getInstance ().getCurrentWorldID () == 0 )
			{
				GetComponent < AlphaApear > ().startFadeOut ();
				collider.enabled = false;
			}
			else if ( FLMissionRoomManager.getInstance ().getCurrentWorldID () == 2 )
			{
				GetComponent < AlphaApear > ().startFadeIn ();
				collider.enabled = true;
			}
			else
			{
				GetComponent < AlphaApear > ().startFadeIn ();
				collider.enabled = true;
			}
		}
	}
}
