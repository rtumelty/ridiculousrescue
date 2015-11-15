using UnityEngine;
using System.Collections;

public class TapLockControl : MonoBehaviour 
{

	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLMissionScreenMapDialogManager.getInstance ().SendMessage ( "OnMouseUp" ); 
	}
}
