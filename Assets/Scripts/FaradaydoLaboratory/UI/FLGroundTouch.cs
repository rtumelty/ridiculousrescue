using UnityEngine;
using System.Collections;

public class FLGroundTouch : MonoBehaviour 
{
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLUIControl.getInstance ().unselectCurrentGameElement ();
		FLUIControl.getInstance ().destoryCurrentUIElement ();
	}
}
