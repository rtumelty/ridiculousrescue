using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialDragRedirectorBeforeRotateComponenet : MonoBehaviour 
{
	//*************************************************************//
	public List < int[] > targetTiles;
	//*************************************************************//
	private IComponent _myIComponent;
	private GameObject _myFrameUICombo;
	//*************************************************************//
	private GameObject _slideArrowPrefab;
	private List < GameObject > _slideArrowInstants;
	//*************************************************************//
	void Awake () 
	{
		if ( transform.Find ( "yesButton" ))
		{
			transform.Find ( "yesButton" ).renderer.material.mainTexture = UIControl.getInstance ().textureYesGreyedOutUp;
			transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureYesGreyedOutUp;
			transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureYesGreyedOutDown;
			transform.Find ( "yesButton" ).GetComponent < BoxCollider > ().enabled = false;
		}
		
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_slideArrowPrefab = ( GameObject ) Resources.Load ( "UI/slideArrow" );
		print ("MMEEEEEEEEEEEEEEEEEEEE");
	}
	
	void Start ()
	{
		_slideArrowInstants = new List < GameObject > ();
		foreach ( int[] target in targetTiles )
		{
			GameObject currentSiledArrow = ( GameObject ) Instantiate ( _slideArrowPrefab, transform.root.position + Vector3.up, Quaternion.identity );
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles = new List<int[]> ();
			currentSiledArrow.GetComponent < SlideArrowUIElement > ().targetTiles.Add ( target );
			_slideArrowInstants.Add ( currentSiledArrow );
		}
		
		InvokeRepeating ( "createGoodMark", 0f, 0.5f );
	}
	
	private void createGoodMark ()
	{
		foreach ( int[] target in targetTiles )
		{
			MessageCenter.getInstance ().createGoodMarkInPosition ( target );
		}
	}
	
	void Update () 
	{
		foreach ( int[] target in targetTiles )
		{
			if ( ToolsJerry.compareTiles ( _myIComponent.position, target ))
			{
#if UNITY_EDITOR
				if ( Input.GetMouseButtonUp ( 0 ))
				{
#else
				if (( Input.touchCount > 0 ) && ( Input.touches[0].phase == TouchPhase.Ended ))
				{
#endif		
					GlobalVariables.DRAGGING_OBJECT = false;
					_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
					TutorialsManager.getInstance ().disapeareTutorialBox ( _myFrameUICombo );
					StartCoroutine ( "destroyOnComplete" );
					SendMessage ( "externallyTurnOffMouseDownOnMe" );
					CancelInvoke ( "createGoodMark" );
				}
			}
		}
	}
	
	private IEnumerator destroyOnComplete ()
	{
		_myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		yield return new WaitForSeconds ( 0.1f );
		
		Destroy ( _myFrameUICombo );
		
		foreach ( GameObject slideArrow in _slideArrowInstants )
		{
			Destroy ( slideArrow );
		}
		
		TutorialsManager.getInstance ().goToNextStep ();
		
		Destroy ( this );
	}
	
	void OnMouseDown ()
	{
		SendMessage ( "handleTouched" );
	}
}
