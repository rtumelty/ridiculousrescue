using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialMoveCharacterByTapComponent : ObjectTapControl 
{
	//*************************************************************//
	public List < int[] > targetTiles;
	public Vector3 scale = new Vector3 ( 0.31f, 1.47f, 2.08f );
	//*************************************************************//
	private IComponent _myIComponent;
	private GameObject _myFrameUICombo;
	private GameObject _handPrefab;
	private GameObject _handInstant;
	private GameObject _tilePrefab;
	private GameObject _tileInstant;
	private bool _backToStartPosition = false;
	//*************************************************************//
	void Awake () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_handPrefab = ( GameObject ) Resources.Load ( "UI/hand" );
		_tilePrefab = ( GameObject ) Resources.Load ( "Tile/tileMarker" );
	}
	
	void Start ()
	{
		_handInstant = ( GameObject ) Instantiate ( _handPrefab, new Vector3 ((float) _myIComponent.position[0], 15f, (float) _myIComponent.position[1] - 0.5f ), _handPrefab.transform.rotation );
		_tileInstant = ( GameObject ) Instantiate ( _tilePrefab, new Vector3 ((float) targetTiles[0][0], 14f, (float) targetTiles[0][1] ), _tilePrefab.transform.rotation );
		scale = VectorTools.cloneVector3 ( _handPrefab.transform.localScale );

		iTween.ScaleFrom ( _handInstant, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "scale", scale * 1.3f, "oncomplete", "onComplete02", "oncompletetarget", this.gameObject ));
	}
	
	private void onComplete01 ()
	{
		if ( _backToStartPosition )
		{
			_handInstant.transform.position = new Vector3 ((float) _myIComponent.position[0], 15f, (float) _myIComponent.position[1] - 0.5f );
		}
		else
		{
			_handInstant.transform.position = new Vector3 ((float) targetTiles[0][0], 15f, (float) targetTiles[0][1] - 0.5f );
		}

		_backToStartPosition = ! _backToStartPosition;

		_handInstant.transform.localScale = VectorTools.cloneVector3 ( scale );
		iTween.ScaleFrom ( _handInstant, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "scale", scale * 1.3f, "oncomplete", "onComplete02", "oncompletetarget", this.gameObject ));
	}
	
	private void onComplete02 ()
	{
		iTween.ScaleTo ( _handInstant, iTween.Hash ( "time", 1.2f, "easetype", iTween.EaseType.linear, "scale", scale * 1.3f, "oncomplete", "onComplete01", "oncompletetarget", this.gameObject ));
	}

	void Update () 
	{
		if ( _alreadyTouched ) return;

		if ( ToolsJerry.compareTiles ( _myIComponent.position, targetTiles[0] ))
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
		Destroy ( _handInstant );
		Destroy ( _tileInstant );

		TutorialsManager.getInstance ().goToNextStep ();
		Destroy ( this );
	}
	
	void OnMouseUp ()
	{
		gameObject.SendMessage ( "handleTouched", SendMessageOptions.DontRequireReceiver );
	}
}
