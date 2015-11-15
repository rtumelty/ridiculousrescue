using UnityEngine;
using System.Collections;

public class TutorialCheckIfRedirectorRotatedComponent : ObjectTapControl 
{
	//*************************************************************//
	private GameObject _myFrameUICombo;
	//*************************************************************//
	void OnMouseUp ()
	{
		if ( _alreadyTouched ) return;
		_alreadyTouched = true;
		
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
			
		transform.Find ( "yesButton" ).renderer.material.mainTexture = UIControl.getInstance ().textureYesUp;
		transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureYesUp;
		transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureYesDown;
		
		transform.Find ( "yesButton" ).GetComponent < BoxCollider > ().enabled = true;
		
		SendMessage ( "handleTouchedRotate" );
		TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
		StartCoroutine ( "destroyOnComplete" );
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
