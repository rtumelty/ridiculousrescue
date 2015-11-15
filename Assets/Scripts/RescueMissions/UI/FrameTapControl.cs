using UnityEngine;
using System.Collections;

public class FrameTapControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		TutorialsManager.getInstance ().disapeareTutorialBox ( transform.parent.gameObject );
		StartCoroutine ( "destroyOnComplete" );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		yield return new WaitForSeconds ( 0.1f );
		Destroy ( transform.parent.gameObject );
		GlobalVariables.CHARACTER_BOX = false;
		MNGlobalVariables.CHARACTER_BOX = false;
		Destroy ( transform.parent.gameObject );
	}
}
