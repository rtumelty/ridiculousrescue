using UnityEngine;
using System.Collections;

public class URLRedirectionButtonControl : MonoBehaviour 
{
	//*************************************************************//	
	public string url;
	//*************************************************************//	
	private bool _leftApp = false;
	//*************************************************************//	
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( url == "https://www.facebook.com/ToyRescueStory" )
		{
			StartCoroutine ( "OpenFacebookPage" );
		}
		else
		{
			Application.OpenURL ( url );
		}
	}

	private IEnumerator OpenFacebookPage()
	{
		Application.OpenURL ( "fb://profile/1462958450605727" );
		//Application.OpenURL ( "fb://page/1462958450605727" );
		
		yield return new WaitForSeconds ( 1f );
		
		if ( _leftApp )
		{
			_leftApp = false;
		}
		else
		{
			Application.OpenURL( "https://www.facebook.com/ToyRescueStory" );
		}
	}
	
	void OnApplicationPause ()
	{
		_leftApp = true;
	}
}
