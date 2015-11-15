using UnityEngine;
using System.Collections;

public class DialogFrameTapControl : MonoBehaviour 
{
	//*************************************************************//
	private bool _iAmTouched;
	private GameObject _tutorialHandPrefab;
	private GameObject _tutorialHandInstant;
	//*************************************************************//
	void Start ()
	{
		_tutorialHandPrefab = ( GameObject ) Resources.Load ( "UI/hand" );
		_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, transform.position + Vector3.right * 3f + Vector3.up * 1f + Vector3.back * 1.5f, transform.rotation );

		_tutorialHandInstant.transform.parent = transform;
		_tutorialHandInstant.AddComponent < SimulateTapControl > ().scale = VectorTools.cloneVector3 ( _tutorialHandInstant.transform.localScale );
	}

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
		FLMissionScreenMapDialogManager.getInstance ().goToNextStep ();
		Destroy ( transform.parent.gameObject );
	}
}