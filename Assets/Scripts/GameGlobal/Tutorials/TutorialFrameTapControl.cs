using UnityEngine;
using System.Collections;

public class TutorialFrameTapControl : MonoBehaviour 
{
	//*************************************************************//
	public bool screenTap = false;
	//*************************************************************//
	private bool _iAmTouched;
	private GameObject _tutorialHandPrefab;
	private GameObject _tutorialHandInstant;
	//*************************************************************//
	private GameObject _tileMarkPrefab;
	private GameObject _tileMarkInstant;
	//*************************************************************//
	void Start ()
	{
		_tileMarkPrefab = ( GameObject ) Resources.Load ( "Tile/tileMarkerCharacters" );

		_tutorialHandPrefab = ( GameObject ) Resources.Load ( "UI/hand" );
		if ( screenTap )
		{
			if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().waitBeforeShowHandAnttaptoContinue == 0f ) _tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( 5.4f, transform.position.y, 1.5f ), transform.rotation );

			_tileMarkInstant = ( GameObject ) Instantiate ( _tileMarkPrefab, new Vector3 ( 5f, 10f, 2.5f ), _tileMarkPrefab.transform.rotation );
			_tileMarkInstant.transform.parent = transform;
			SelectedComponenent currentSelectedComponenent = _tileMarkInstant.AddComponent < SelectedComponenent > ();
			currentSelectedComponenent.setSelectedForPulsingCharacterMark ( true, 1.5f );
		}
		else if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().waitBeforeShowHandAnttaptoContinue == 0f ) _tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, transform.position + Vector3.right * 3f + Vector3.up * 1f + Vector3.back * 1.5f, transform.rotation );

		if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().waitBeforeShowHandAnttaptoContinue == 0f )
		{
			_tutorialHandInstant.transform.parent = transform;
			_tutorialHandInstant.AddComponent < SimulateTapControl > ().scale = VectorTools.cloneVector3 ( _tutorialHandInstant.transform.localScale );
		}
		else
		{
			StartCoroutine ( "waitBeforeTurnOnHand" );
		}
	}

	private IEnumerator waitBeforeTurnOnHand ()
	{
		yield return new WaitForSeconds ( 4f );
		_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, transform.position + Vector3.right * 3f + Vector3.up * 1f + Vector3.back * 1.5f, transform.rotation );
		_tutorialHandInstant.transform.parent = transform;
		_tutorialHandInstant.AddComponent < SimulateTapControl > ().scale = VectorTools.cloneVector3 ( _tutorialHandInstant.transform.localScale );
	}

	void OnMouseUp ()
	{
		if ( _iAmTouched ) return;
		_iAmTouched = true;
		if (Camera.main.gameObject.GetComponent < StartSequenceStartCameraZoom >() != null)
	    {
			Camera.main.gameObject.GetComponent < StartSequenceStartCameraZoom >().putCamBack = true;
		}
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		TutorialsManager.getInstance ().disapeareTutorialBox ( transform.parent.gameObject );
		StartCoroutine ( "destroyOnComplete" );
		Destroy ( transform.parent.Find ( "tapText" ).gameObject );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		yield return new WaitForSeconds ( 0.1f );
		TutorialsManager.getInstance ().goToNextStep ( TutorialsManager.getInstance ().getCurrentTutorialStep ().repeatSequenceNumberBack );
		Destroy ( transform.parent.gameObject );
	}
}