using UnityEngine;
using System.Collections;

public class TutorialCheckIfRediretorBuildedComponent : ObjectTapControl 
{
	//*************************************************************//
	private RedirectorComponent _myRedirectorComponent;
	private GameObject _myFrameUICombo;
	//*************************************************************//
	void Awake () 
	{
		_myRedirectorComponent = gameObject.GetComponent < RedirectorComponent > ();
	}
	
	void Update () 
	{
		if ( _alreadyTouched ) return;
		if ( _myRedirectorComponent.checkIfBuilded ())
		{
			_alreadyTouched = true;
			_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
			TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
			StartCoroutine ( "destroyOnComplete" );
		}
	}
	
	private IEnumerator destroyOnComplete ()
	{
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		yield return new WaitForSeconds ( 0.1f );
		
		Destroy ( _myFrameUICombo );
		
		TutorialsManager.getInstance ().goToNextStep ();
		Destroy ( this );
	}
}
