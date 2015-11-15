using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialMoveCharacterComponent : ObjectTapControl 
{
	//*************************************************************//
	public List < int[] > targetTiles;
	public List < int[] > showTargetTiles;
	public int[] startPosition;
	public List < int[] > targetTiles02;
	public List < int[] > showTargetTiles02;
	public int[] startPosition02;
	public bool targetIsRescueToy = false;
	public float ifNoMoveAfterSecShowHand;
	//*************************************************************//
	private IComponent _myIComponent;
	private GameObject _myFrameUICombo;
	//*************************************************************//
	private GameObject _slideArrowPrefab;
	private List < GameObject > _slideArrowInstants;
	private bool _useTargetTiles02 = false;
	//*************************************************************//
	void Awake () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_slideArrowPrefab = ( GameObject ) Resources.Load ( "UI/slideArrow" );
		print ("MMEEEEEEEEEEEEEEEEEEEE");
	}
	
	IEnumerator Start ()
	{
		if ( startPosition02 != null && LevelControl.getInstance ().getCharacter ( GameElements.CHAR_CORA_1_IDLE ).position[0] == startPosition02[0] && LevelControl.getInstance ().getCharacter ( GameElements.CHAR_CORA_1_IDLE ).position[1] == startPosition02[1])
		{
			_useTargetTiles02 = true;
		}

		_slideArrowInstants = new List < GameObject > ();

		print ( ifNoMoveAfterSecShowHand );

		yield return new WaitForSeconds ( ifNoMoveAfterSecShowHand );

		if ( _useTargetTiles02 )
		{
			GameObject currentSiledArrow = ( GameObject ) Instantiate ( _slideArrowPrefab, new Vector3 ((float) startPosition02[0], 15f, (float) startPosition02[1] ), Quaternion.identity );
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles = showTargetTiles02;
			if ( showTargetTiles02.Count > 1 ) currentSiledArrow.GetComponent < SlideArrowUIElement > ().longPath = true;
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().fromLevelO1or02 = true;
			_slideArrowInstants.Add ( currentSiledArrow );
		}
		else
		{
			GameObject currentSiledArrow = ( GameObject ) Instantiate ( _slideArrowPrefab, new Vector3 ((float) startPosition[0], 15f, (float) startPosition[1] ), Quaternion.identity );
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles = showTargetTiles;
			if ( showTargetTiles.Count > 1 ) currentSiledArrow.GetComponent < SlideArrowUIElement > ().longPath = true;
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().fromLevelO1or02 = true;
			_slideArrowInstants.Add ( currentSiledArrow );
		}
	}

	public void finishExternaly ()
	{
		StartCoroutine ( "destroyOnComplete" );
	}
	
	void Update () 
	{
		if ( _alreadyTouched ) return;

		if ( _useTargetTiles02 )
		{
			if ( ToolsJerry.compareTiles ( _myIComponent.position, targetTiles02[0] ))
			{
				_alreadyTouched = true;
				_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
				if (( ! TutorialsManager.getInstance ().getCurrentTutorialStep ().dontDestroyThisTutorialBox && ! TutorialsManager.getInstance ().getCurrentTutorialStep ().preservePreviousTutorialBox ) || TutorialsManager.getInstance ().getCurrentTutorialStep ().destroyThisPreservedTutorialBoxOnEnd )
				{
					TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
				}
				
				StartCoroutine ( "destroyOnComplete" );}
		}
		else
		{
			if ( targetIsRescueToy )
			{
				if ( ToolsJerry.compareTiles ( GetComponent < RescuerComponent > ()._positionOfToBeRescued, targetTiles[0] ))
				{
					_alreadyTouched = true;
					_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
					if (( ! TutorialsManager.getInstance ().getCurrentTutorialStep ().dontDestroyThisTutorialBox && ! TutorialsManager.getInstance ().getCurrentTutorialStep ().preservePreviousTutorialBox ) || TutorialsManager.getInstance ().getCurrentTutorialStep ().destroyThisPreservedTutorialBoxOnEnd )
					{
						TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
					}
					
					StartCoroutine ( "destroyOnComplete" );
				}
			}
			else if ( ToolsJerry.compareTiles ( _myIComponent.position, targetTiles[0] ))
			{
				_alreadyTouched = true;
				_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
				if (( ! TutorialsManager.getInstance ().getCurrentTutorialStep ().dontDestroyThisTutorialBox && ! TutorialsManager.getInstance ().getCurrentTutorialStep ().preservePreviousTutorialBox ) || TutorialsManager.getInstance ().getCurrentTutorialStep ().destroyThisPreservedTutorialBoxOnEnd )
				{
					TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
				}

				StartCoroutine ( "destroyOnComplete" );
			}
		}
	}
	
	private IEnumerator destroyOnComplete ()
	{
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		yield return new WaitForSeconds ( 0.1f );
		
		if (( ! TutorialsManager.getInstance ().getCurrentTutorialStep ().dontDestroyThisTutorialBox && ! TutorialsManager.getInstance ().getCurrentTutorialStep ().preservePreviousTutorialBox ) || TutorialsManager.getInstance ().getCurrentTutorialStep ().destroyThisPreservedTutorialBoxOnEnd )
		{
			Destroy ( _myFrameUICombo );
		}

		foreach ( GameObject slideArrow in _slideArrowInstants )
		{
			Destroy ( slideArrow );
		} 
		
		TutorialsManager.getInstance ().goToNextStep ();
		Destroy ( this );
	}
	
	void OnMouseDown ()
	{
		gameObject.SendMessage ( "forceOnMouseDown", SendMessageOptions.DontRequireReceiver );
	}
}
