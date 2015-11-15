using UnityEngine;
using System.Collections;

public class FLStorageContainerPanelControl : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private Material _myProgressBarMeterial;
	private TextMesh _maxText;
	private TextMesh _amountText;
	private Material _iconMaterial;
	private bool _popedUp = false;
	private Vector3 _initialPositionHidden;

	private float _countUpdate = 0f;
	//*************************************************************//
	void Start () 
	{
		_initialPositionHidden = VectorTools.cloneVector3 ( transform.localPosition );
		
		_myProgressBarMeterial = transform.Find ( "progressBar" ).renderer.material;
		_maxText = transform.Find ( "textMax" ).GetComponent < TextMesh > ();
		_amountText = transform.Find ( "textAmount" ).GetComponent < TextMesh > ();
		_iconMaterial = transform.Find ( "icon" ).renderer.material;
		
		_iconMaterial.mainTexture = myStorageContainerClass.iconTexture;
	}
	
	void Update () 
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		if (( ! _popedUp ) && ( FLUIControl.getInstance ().currentCameraAboveRoom () == FLNavigateButton.ROOM_ID_FACTORY ))
		{
			_popedUp = true;
			//iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.8f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPositionHidden + Vector3.back * 4.3f, "islocal", true ));
		}
		else if (( _popedUp ) && ( FLUIControl.getInstance ().currentCameraAboveRoom () != FLNavigateButton.ROOM_ID_FACTORY ))
		{
			_popedUp = false;
			//iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeInBack, "position", _initialPositionHidden, "islocal", true ));
		}

		switch ( myStorageContainerClass.type )
		{
		case FLStorageContainerClass.STORAGE_TYPE_METAL:
			_myProgressBarMeterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity ) / 5 )];
			_maxText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity.ToString ();
			break;
		case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
			_myProgressBarMeterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity ) / 5 )];
			_maxText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity.ToString ();
			break;
		case FLStorageContainerClass.STORAGE_TYPE_VINES:
			_myProgressBarMeterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity ) / 5 )];
			_maxText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity.ToString ();
			break;
		}

				
		_amountText.text = myStorageContainerClass.amount.ToString ();
	}
}
