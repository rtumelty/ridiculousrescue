using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TRStoneControl : MonoBehaviour 
{
	//*************************************************************//
	public bool doNotProduceButton = false;
	//*************************************************************//
	private float _countFrame;
	private int _frameID;
	private float _additionalXDistance = 0f;
	private Material _myMaterial;
	private GameObject _drillingButtonInstance;
	private GameObject _tutorialHandPrefab;
	private GameObject _tutorialHandInstant;
	private TRBozControl bozReference;
	//*************************************************************//
	void Start () 
	{
		_tutorialHandPrefab = ( GameObject ) Resources.Load ( "UI/hand" );

		_myMaterial = renderer.material;
		bozReference = GameObject.Find ("Boz").transform.Find ("tile").transform.Find ("side").GetComponent < TRBozControl > ();
		if ( ! doNotProduceButton )
		{
			produceButton ();
		}
	}

	private void produceButton ()
	{
		GameObject drillingButtonPrefab = ( GameObject ) Resources.Load ( "UI/drillingButton" );
		_drillingButtonInstance = ( GameObject ) Instantiate ( drillingButtonPrefab, transform.position + Vector3.up * 10f + Vector3.forward * 2f, drillingButtonPrefab.transform.rotation );
		_drillingButtonInstance.transform.Find ( "button" ).GetComponent < TRDrillingButtonControl > ().followStone = this.transform;

		if ( TREventsManager.getInstance ().getCurrentTutorialID () == TREventsManager.TUTORIAL_ID_TAP_ROCK )
		{
			_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( _drillingButtonInstance.transform.position.x + 0.2f, _drillingButtonInstance.transform.position.y + 1f, _drillingButtonInstance.transform.position.z - 3.0f ), _tutorialHandPrefab.transform.rotation );
			_tutorialHandInstant.transform.parent = _drillingButtonInstance.transform;
			_tutorialHandInstant.AddComponent < SimulateTapControl > ().scale = VectorTools.cloneVector3 ( _tutorialHandInstant.transform.localScale );
			_tutorialHandInstant.GetComponent < SimulateTapControl > ().timeFactor = 0.1f;
		}
	}
	
	void Update () 
	{
		_countFrame += Time.deltaTime;

		if ( _countFrame >= 1f / 24f )
		{
			_countFrame = 0f;
			_frameID++;
			if ( _frameID >= TRSpeedAndTrackOMetersManager.getInstance ().stoneTextures.Length ) _frameID = 0;
			_myMaterial.mainTexture = TRSpeedAndTrackOMetersManager.getInstance ().stoneTextures[_frameID];
		}

		transform.Translate ( Vector3.right * Time.deltaTime * TRSpeedAndTrackOMetersManager.getInstance ().getSpeed ());

		if ( transform.position.x <= ( TRSpeedAndTrackOMetersManager.getInstance ().getPositionOfTrainPart01 () + Vector3.right * 2f ).x + _additionalXDistance )
		{
			transform.position = new Vector3 (( TRSpeedAndTrackOMetersManager.getInstance ().getPositionOfTrainPart01 () + Vector3.right * 2f ).x + _additionalXDistance, transform.position.y, transform.position.z );
			TRSpeedAndTrackOMetersManager.getInstance ().slowDown ( this.gameObject );
		}

		if ( _drillingButtonInstance == null )
		{
			if ( TREventsManager.getInstance ().stonesOnLevel.IndexOf ( this.gameObject ) == 0 )
			{
				produceButton ();
			}
		}
	}

	void OnTriggerEnter ( Collider other )
	{
		if ( other.gameObject.tag == TRGlobalVariables.Tags.STONE )
		{
			_additionalXDistance = transform.position.x - ( TRSpeedAndTrackOMetersManager.getInstance ().getPositionOfTrainPart01 () + Vector3.right * 2f ).x;
		}
	}

	void OnTriggerExit ( Collider other )
	{
		_additionalXDistance = 0f;
	}
	void OnDestroy ()
	{
		if(bozReference != null)
		{
			bozReference.stillClicking = true;
		}
		SoundManager.getInstance ().playSound (SoundManager.BOZ_WIN);
	}

	public void removeAdditionalX ()
	{
		_additionalXDistance = 0f;
	}
}
