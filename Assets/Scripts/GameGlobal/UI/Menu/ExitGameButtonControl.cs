using UnityEngine;
using System.Collections;

public class ExitGameButtonControl : MonoBehaviour 
{
	void OnMouseDown ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		//FlurryAnalytics.Instance ().LogEvent ( "Application quit" );
		GoogleAnalytics.instance.LogScreen ( "Application quit" );
		Application.Quit ();
	}
}
