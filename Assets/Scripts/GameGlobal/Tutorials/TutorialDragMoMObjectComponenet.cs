using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialDragMoMObjectComponenet : MonoBehaviour 
{
	//*************************************************************//
	public List < int[] > targetTiles;
	public bool fromTutorial = true;
	//*************************************************************//
	private GameObject _myFrameUICombo;
	//*************************************************************//
	private GameObject _slideArrowPrefab;
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	private List < GameObject > _slideArrowInstants;
	private SelectedComponenent _mySelectedcomponent;
	//*************************************************************//
	void Awake () 
	{
		print ("MMEEEEEEEEEEEEEEEEEEEE");
		_slideArrowPrefab = ( GameObject ) Resources.Load ( "UI/slideArrow" );
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
	}
	
	void Start ()
	{
		if ( fromTutorial )
		{
			_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * 1.2f * TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor + Vector3.up, Quaternion.identity );
			//_jumpingArrowInstant.transform.parent = transform;
			_jumpingArrowInstant.transform.localScale *= TutorialsManager.getInstance ().getCurrentTutorialStep ().arrowBigFactor;
		}

		/*
		_mySelectedcomponent = GetComponent < SelectedComponenent > ();
		if ( _mySelectedcomponent ) _mySelectedcomponent.setSelectedForTutorial ( true, true, true, true, false );
		else
		{
			_mySelectedcomponent = gameObject.AddComponent < SelectedComponenent > ();
			_mySelectedcomponent.setSelectedForTutorial ( true, true, true, true, false );
		}
		*/

		_slideArrowInstants = new List < GameObject > ();
		foreach ( int[] target in targetTiles )
		{
			GameObject currentSiledArrow = ( GameObject ) Instantiate ( _slideArrowPrefab, transform.position + Vector3.up, Quaternion.identity );
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles = new List<int[]> ();
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles.Add ( target );
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().fromTutorial = fromTutorial;

			currentSiledArrow.transform.parent = FLFactoryRoomManager.getInstance ().momsOnLevel[0].myMomPanelControl.transform;

			_slideArrowInstants.Add ( currentSiledArrow );
		}
	}
	
	void Update () 
	{
		foreach ( int[] target in targetTiles )
		{
			if ( ToolsJerry.compareTiles ( new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z )}, target ))
			{
#if UNITY_EDITOR
				if ( Input.GetMouseButtonUp ( 0 ))
				{
#else
				if (( Input.touchCount > 0 ) && ( Input.touches[0].phase == TouchPhase.Ended ))
				{
#endif				
					SendMessage ( "externallyStartProduction" );
					externallyFinishStep ();
				}
			}
		}
	}
		
	private void externallyFinishStep ()
	{
		foreach ( GameObject slideArrow in _slideArrowInstants )
		{
			Destroy ( slideArrow );
		}
			
		if ( fromTutorial )
		{
			_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
			TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
		}

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

	void OnDestroy ()
	{
		if ( ! fromTutorial )
		{
			TutorialsManager.getInstance ().goToNextStep ();
		}
	}
	
	void OnMouseDown ()
	{
		if ( _mySelectedcomponent ) _mySelectedcomponent.setSelectedForTutorial ( false );
		Destroy ( _jumpingArrowInstant );
		SendMessage ( "handleTouched" );
	}
}
