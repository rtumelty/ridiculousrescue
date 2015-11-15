using UnityEngine;
using System.Collections;

public class TutorialObjectTapComponent : MonoBehaviour 
{
	//*************************************************************//
	public bool arrowFromAbove = true;
	public bool doNotInteract = false;
	//*************************************************************//
	private GameObject _myFrameUICombo;
	private TutorialsManager.TutorialStepsClass _myTutorialStep;
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	private bool _stepCompleted = false;
	private bool _mouseUpOnMe = false;
	private SelectedComponenent _mySelectedcomponent;
	private GameObject _tileMarkPrefab;
	private GameObject _tileMarkInstant;
	//*************************************************************//
	private GameObject _tutorialHandPrefab;
	private GameObject _tutorialHandInstant;
	//*************************************************************//
	void Awake ()
	{
		_tileMarkPrefab = ( GameObject ) Resources.Load ( "Tile/tileMarkerCharacters" );
		_tutorialHandPrefab = ( GameObject ) Resources.Load ( "UI/hand" );
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		_myTutorialStep = TutorialsManager.getInstance ().getCurrentTutorialStep ();
	}
	
	void Start ()
	{
		_myTutorialStep = TutorialsManager.getInstance ().getCurrentTutorialStep ();
		bool createJumpingArrow = true;
		switch ( _myTutorialStep.type )
		{
			case TutorialsManager.TUTORIAL_OBJECT_TYPE_TAP_ATTACK_BUTTON:
				createJumpingArrow = false;
				break;
		}
		
		if ( createJumpingArrow )
		{
			if ( _myTutorialStep.highLightWithHand )
			{
				_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, transform.position + Vector3.up + Vector3.back *  _myTutorialStep.handMove, _tutorialHandPrefab.transform.rotation );
				_tutorialHandInstant.transform.parent = transform.parent;
				_tutorialHandInstant.AddComponent < SimulateTapControl > ().scale = VectorTools.cloneVector3 ( _tutorialHandInstant.transform.localScale );

				_tileMarkInstant = ( GameObject ) Instantiate ( _tileMarkPrefab, transform.parent.position + Vector3.down * 0.25f + Vector3.forward * 0.5f, _tileMarkPrefab.transform.rotation );
				SelectedComponenent currentSelectedComponenent = _tileMarkInstant.AddComponent < SelectedComponenent > ();
				currentSelectedComponenent.setSelectedForPulsingCharacterMark ( true, 1.5f );
			}
			else
			{
				_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * ( arrowFromAbove ? 2.3f : -1f ) * TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor + Vector3.up, Quaternion.identity );
				_jumpingArrowInstant.GetComponent < JumpingUIElement > ().typeFromAbove = arrowFromAbove;
				_jumpingArrowInstant.transform.parent = transform;
				_jumpingArrowInstant.transform.localScale *= TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor;
			}
		}

		TutorialsManager.getInstance ().objectToBeDestoryedOnNextStep = _jumpingArrowInstant;
		
		_mySelectedcomponent = GetComponent < SelectedComponenent > ();
		if ( _mySelectedcomponent ) _mySelectedcomponent.setSelectedForTutorial ( true, true, true, true, false );
	}
	
	void OnMouseUp ()
	{
		if ( doNotInteract )
		{
			return;
		}

		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		_myTutorialStep = TutorialsManager.getInstance ().getCurrentTutorialStep ();
		Destroy ( _jumpingArrowInstant );
		Destroy ( _tutorialHandInstant );
		Destroy ( _tileMarkInstant );

		if ( _mySelectedcomponent ) GetComponent < SelectedComponenent > ().setSelectedForTutorial ( false );
		gameObject.SendMessage ( "handleTouched", SendMessageOptions.DontRequireReceiver );

		switch ( _myTutorialStep.type )
		{
			case TutorialsManager.TUTORIAL_OBJECT_TYPE_SELECT_CHARACTER_AND_TAP_OBJECT:
				callBackWhenCharacterAround ();
				_mouseUpOnMe = false;
				break;
			case TutorialsManager.TUTORIAL_OBJECT_TYPE_TAP_ATTACK_BUTTON:
				if ( gameObject.GetComponent < AttackUIComboControl > ().correctTapAlready ())
				{
					callBackWhenCharacterAround ();
					_mouseUpOnMe = false;
				}
				break;
			default:
				_mouseUpOnMe = true;
				break;
		}
	}
	
	void Update ()
	{
		if ( _stepCompleted ) return;
		if ( ! _mouseUpOnMe ) return;
		
		GameObject characterMoving = LevelControl.getInstance ().getSelectedCharacterObjectFromLevel ();
		if ( Vector3.Distance ( transform.parent.position, characterMoving.transform.position ) < 2f )
		{
			callBackWhenCharacterAround ();
		}
	}
	
	private void callBackWhenCharacterAround ()
	{
		_stepCompleted = true;
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );

		if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().forcePositionOnEnd != null )
		{
			GameObject characterMoving = LevelControl.getInstance ().getSelectedCharacterObjectFromLevel ();
			characterMoving.transform.Find ( "tile" ).GetComponent < IComponent > ().position = TutorialsManager.getInstance ().getCurrentTutorialStep ().forcePositionOnEnd;
		}

		StartCoroutine ( "destroyOnComplete", false );
	}
	
	public void externalCallForDestroy ()
	{
		Destroy ( _jumpingArrowInstant );
		Destroy ( _tutorialHandInstant );

		if ( _mySelectedcomponent ) GetComponent < SelectedComponenent > ().setSelectedForTutorial ( false );
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
		StartCoroutine ( "destroyOnComplete" );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		_myTutorialStep = TutorialsManager.getInstance ().getCurrentTutorialStep ();
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		yield return new WaitForSeconds ( 0.1f );
		
		Destroy ( _myFrameUICombo );
		
		switch ( _myTutorialStep.type )
		{
			case TutorialsManager.TUTORIAL_OBJECT_TYPE_TAP_ATTACK_BUTTON:
				Destroy ( this.gameObject );
				break;
			default:
				TutorialsManager.getInstance ().goToNextStep ();
				break;
		}
		
		Destroy ( this );
	}
}
