using UnityEngine;
using System.Collections;

public class TutorialsOkBuonConrol : MonoBehaviour 
{
	private bool _iAmTouched;

	IEnumerator Start ()
	{
		renderer.enabled = false;
		collider.enabled = false;
		transform.Find ( "3DText" ).renderer.enabled = false;
		yield return new WaitForSeconds ( 3f );
		renderer.enabled = true;
		collider.enabled = true;
		transform.Find ( "3DText" ).renderer.enabled = true;

	}

	void OnMouseUp ()
	{
		if ( _iAmTouched ) return;
		_iAmTouched = true;
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		TutorialsManager.getInstance ().disapeareTutorialBox ( transform.parent.parent.gameObject );
		StartCoroutine ( "destroyOnComplete" );
		Destroy ( transform.parent.parent.Find ( "tapText" ).gameObject );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		yield return new WaitForSeconds ( 0.1f );
		TutorialsManager.getInstance ().goToNextStep ( TutorialsManager.getInstance ().getCurrentTutorialStep ().repeatSequenceNumberBack );
		Destroy ( transform.parent.parent.gameObject );
	}
}
