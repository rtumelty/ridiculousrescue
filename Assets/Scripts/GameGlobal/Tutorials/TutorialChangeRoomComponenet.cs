using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialChangeRoomComponenet : MonoBehaviour 
{
	//*************************************************************//
	public List < int[] > targetTiles;
	//*************************************************************//
	private GameObject _myFrameUICombo;
	//*************************************************************//
	private GameObject _slideArrowPrefab;
	private List < GameObject > _slideArrowInstants;
	private bool _finsihedStep = false;
	//*************************************************************//
	void Awake () 
	{
		_slideArrowPrefab = ( GameObject ) Resources.Load ( "UI/slideArrow" );
		print ("MMEEEEEEEEEEEEEEEEEEEE");
	}
	
	void Start ()
	{
		_slideArrowInstants = new List < GameObject > ();
		GameObject currentSiledArrow = ( GameObject ) Instantiate ( _slideArrowPrefab, new Vector3 ((float) targetTiles[0][0], transform.position.y, (float) targetTiles[0][1] - 0.5f ), Quaternion.identity );
		currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles = new List<int[]> ();
		currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles.Add ( targetTiles[1] );
		_slideArrowInstants.Add ( currentSiledArrow );
	}
	
	void Update () 
	{
		if ( _finsihedStep ) return;
		if ( gameObject.GetComponent < FLNavigateButton > ().amISelected ())
		{
			_finsihedStep = true;
			_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
			TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
			foreach ( GameObject slideArrow in _slideArrowInstants )
			{
				Destroy ( slideArrow );
			}
			
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
