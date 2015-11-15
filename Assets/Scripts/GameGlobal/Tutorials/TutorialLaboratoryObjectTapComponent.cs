using UnityEngine;
using System.Collections;

public class TutorialLaboratoryObjectTapComponent : MonoBehaviour 
{
	//*************************************************************//
	public bool arrowFromAbove = true;
	//*************************************************************//
	private GameObject _myFrameUICombo;
	//private GameObject _jumpingArrowPrefab;
	//private GameObject _jumpingArrowInstant;
	private SelectedComponenent _mySelectedcomponent;
	//*************************************************************//
	void Awake () 
	{
		//_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
	}
	
	void Start ()
	{
		//_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * ( arrowFromAbove ? 1.0f : -0.9f ) * 2f + Vector3.up * 2f, Quaternion.identity );
		//_jumpingArrowInstant.GetComponent < JumpingUIElement > ().typeFromAbove = arrowFromAbove;
		//_jumpingArrowInstant.transform.parent = transform;
		//_jumpingArrowInstant.transform.localScale *= 2f;

		_mySelectedcomponent = GetComponent < SelectedComponenent > ();
		if ( _mySelectedcomponent ) _mySelectedcomponent.setSelectedForTutorial ( true, true, true, true, false );
	}
	
	void OnMouseUp ()
	{
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		//Destroy ( _jumpingArrowInstant );
		if ( _mySelectedcomponent ) GetComponent < SelectedComponenent > ().setSelectedForTutorial ( false );
		gameObject.SendMessage ( "handleTouched", SendMessageOptions.DontRequireReceiver );
		onCompleteStep ();
	}
	
	private void onCompleteStep ()
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
