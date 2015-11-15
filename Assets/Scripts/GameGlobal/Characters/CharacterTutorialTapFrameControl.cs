using UnityEngine;
using System.Collections;

public class CharacterTutorialTapFrameControl : MonoBehaviour 
{
	//*************************************************************//
	private bool _iAmTouched;
	//*************************************************************//
	void OnMouseUp ()
	{
		if ( _iAmTouched ) return;
		_iAmTouched = true;
		
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		TutorialsManager.getInstance ().disapeareTutorialBox ( transform.parent.gameObject );
		StartCoroutine ( "destroyOnComplete" );
		Destroy ( transform.parent.Find ( "tapText" ).gameObject );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		yield return new WaitForSeconds ( 0.1f );
		foreach ( CharacterTapTutorialControl tutorialtapcomponent in FLLevelControl.getInstance ().tutorialTapComponentsOnLevel )
		{
			tutorialtapcomponent.createQuestionMark ();
		}
		
		Destroy ( transform.parent.gameObject );
	}
}
