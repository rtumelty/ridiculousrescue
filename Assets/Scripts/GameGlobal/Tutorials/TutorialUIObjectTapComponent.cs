using UnityEngine;
using System.Collections;

public class TutorialUIObjectTapComponent : ObjectTapControl 
{
	//*************************************************************//
	public bool arrowFromAbove = false;
	public float moveUp = 0f;
	public bool dontShowArrow = false;
	//*************************************************************//
	private GameObject _myFrameUICombo;
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	private SelectedComponenent _mySelectedcomponent;
	//*************************************************************//
	void Awake () 
	{
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );		
		
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
	}
	
	void Start ()
	{
		if ( ! dontShowArrow )
		{
			_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * ( arrowFromAbove ? 0.9f + moveUp : -0.9f + moveUp ) * TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor + Vector3.up, Quaternion.identity );
			_jumpingArrowInstant.GetComponent < JumpingUIElement > ().typeFromAbove = arrowFromAbove;
			_jumpingArrowInstant.transform.parent = transform;
			_jumpingArrowInstant.transform.localScale *= TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor;
		}

		_mySelectedcomponent = GetComponent < SelectedComponenent > ();
		if ( _mySelectedcomponent ) _mySelectedcomponent.setSelectedForTutorial ( true, true, true, true, false );
		else
		{
			_mySelectedcomponent = gameObject.AddComponent < SelectedComponenent > ();
			_mySelectedcomponent.setSelectedForTutorial ( true, true, true, true, false );
		}
	}
	
	void OnMouseUp ()
	{
		if ( _alreadyTouched ) return;
		_alreadyTouched = true;
		
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		Destroy ( _jumpingArrowInstant );
		if ( _mySelectedcomponent ) GetComponent < SelectedComponenent > ().setSelectedForTutorial ( false );
		gameObject.SendMessage ( "handleTouched", false, SendMessageOptions.DontRequireReceiver );
		
		onStepComplete ();
	}
	
	private void onStepComplete ()
	{
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
		StartCoroutine ( "destroyOnComplete", false );
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
