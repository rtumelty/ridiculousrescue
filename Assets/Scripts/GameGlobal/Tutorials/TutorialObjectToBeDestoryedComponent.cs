using UnityEngine;
using System.Collections;

public class TutorialObjectToBeDestoryedComponent : MonoBehaviour 
{	
	//*************************************************************//
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	//*************************************************************//
	void Awake () 
	{
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );
	}
	
	void Start ()
	{
		_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * 1.7f + Vector3.up, Quaternion.identity );
		_jumpingArrowInstant.transform.parent = transform;
		_jumpingArrowInstant.transform.localScale *= TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor;
		
		GetComponent < SelectedComponenent > ().setSelectedForTutorial ( true, true, true, true, false );
	}
	
	void OnDestroy  () 
	{
		TutorialsManager.TutorialStepsClass currentTutorailStep = TutorialsManager.getInstance ().getCurrentTutorialStep ();
		int numberOfExistingObjects = 0;
		foreach ( GameObject objectTobeDestoyed in currentTutorailStep.objectsToBeDestroyed )
		{
			if ( objectTobeDestoyed != null ) numberOfExistingObjects++;
		}
		
		if ( currentTutorailStep.type == TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS_WITH_FRAME_TEXT )
		{
			if ( numberOfExistingObjects == 1 ) TutorialsManager.getInstance ().goToNextStepFully ();
		}
		else
		{
			if ( numberOfExistingObjects == 1 ) TutorialsManager.getInstance ().goToNextStep ();
		}
	}
	
	void OnMouseUp ()
	{
		gameObject.SendMessage ( "handleTouched", SendMessageOptions.DontRequireReceiver );
	}
}