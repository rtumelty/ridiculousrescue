using UnityEngine;
using System.Collections;

public class MNEscapeButton : MonoBehaviour 
{
	void OnMouseUp ()
	{
		if ( MNGlobalVariables.POPUP_UI_SCREEN || MNGlobalVariables.TUTORIAL_MENU ) return;
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		MNResoultScreen.getInstance ().startResoultScreen ();
	}
}
