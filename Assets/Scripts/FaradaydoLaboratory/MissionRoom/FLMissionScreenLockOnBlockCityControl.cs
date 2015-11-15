using UnityEngine;
using System.Collections;

public class FLMissionScreenLockOnBlockCityControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.POPUP_UI_SCREEN || FLGlobalVariables.TUTORIAL_MENU ) return;
		
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		GameObject endMinesScreenPopup = ( GameObject ) Resources.Load ( "UI/Laboratory/endMinesUIScreen" );
		FLUIControl.getInstance ().createPopup ( endMinesScreenPopup );
	}
}
