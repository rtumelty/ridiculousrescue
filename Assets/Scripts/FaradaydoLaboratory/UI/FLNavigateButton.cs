using UnityEngine;
using System.Collections;

public class FLNavigateButton : MonoBehaviour 
{
	//*************************************************************//
	public static int CURRENT_ROOM_ID;
	//*************************************************************//	
	public const int ROOM_ID_FACTORY = 0;
	public const int ROOM_ID_MISSION = 1;
	public const int ROOM_ID_PLAY = 2;
	public const int ROOM_ID_GARAGE = 3;
	//*************************************************************//	
	public int myRoomID;
	//*************************************************************//	
	//private Vector3 _initialScale;
	private bool _selected = false;
	private Material _myBackMaterial;
	private bool _triggerLabVisit02 = false;
	private bool _labVisit02Triggered = false;
	//*************************************************************//	
	void Start () 
	{
		//_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		_myBackMaterial = renderer.material;
	}
	
	void Update () 
	{
		if ( ! _selected && FLUIControl.getInstance ().currentCameraAboveRoom () == myRoomID )
		{
			//iTween.ScaleTo ( gameObject, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _initialScale * 1.2f ));
			_myBackMaterial.mainTexture = FLUIControl.getInstance ().textureUIButtonSelectedUp;
			GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = FLUIControl.getInstance ().textureUIButtonSelectedUp;
			GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = FLUIControl.getInstance ().textureUIButtonSelectedDown;
			transform.Find ( "3DText" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
			transform.Find ( "3DText" ).Find ( "3DText(Clone)" ).renderer.material = GameGlobalVariables.FontMaterials.WHITE_TEXT;
			_selected = true;

			if ( _triggerLabVisit02 )
			{
				if ( _labVisit02Triggered ) return;
				GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );
			}
		}
		else if ( _selected && FLUIControl.getInstance ().currentCameraAboveRoom () != myRoomID )
		{
			_myBackMaterial.mainTexture = FLUIControl.getInstance ().textureUIButtonNotSelectedUp;
			GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = FLUIControl.getInstance ().textureUIButtonNotSelectedUp;
			GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = FLUIControl.getInstance ().textureUIButtonNotSelectedDown;
			transform.Find ( "3DText" ).renderer.material = GameGlobalVariables.FontMaterials.WHITE_TEXT;
			transform.Find ( "3DText" ).Find ( "3DText(Clone)" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
			//iTween.ScaleTo ( gameObject, iTween.Hash ( "time", 0.01f, "easetype", iTween.EaseType.linear, "scale", _initialScale ));
			_selected = false;

			if ( _triggerLabVisit02 )
			{
				if ( _labVisit02Triggered ) return;
				GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true );
			}
		}
	}
	
	public bool amISelected ()
	{
		return _selected;
	}

	public void triggerActionOnLab02VistTutorial ()
	{
		if ( _labVisit02Triggered ) return;

		_triggerLabVisit02 = true;

		_labVisit02Triggered = true;

		GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true );
	}
	
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched (bool fromMomClose = false)
	{
		if ( _triggerLabVisit02 )
		{
			GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );
		}

		FLUIControl.getInstance ().unselectCurrentGameElement ();
		FLUIControl.getInstance ().destoryCurrentUIElement ();
		
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		
		Vector3 positionToGo = Vector3.zero;
		switch ( myRoomID )
		{
			case ROOM_ID_FACTORY:
				if(fromMomClose == true)
				{
					print ("Step3");
					positionToGo = new Vector3 ( -6.5f + 12f, Camera.main.transform.position.y, 4f + 9.5f );
					Camera.main.orthographicSize = 5.3f;
				}
				else
				{
					positionToGo = new Vector3 ( -6.5f + 12f, Camera.main.transform.position.y, 4f + 9.5f );	
				}
				break;
			case ROOM_ID_MISSION:
				positionToGo = new Vector3 ( 5.5f + 12f, Camera.main.transform.position.y, 4f + 9.5f );
				break;
			case ROOM_ID_PLAY:
				positionToGo = new Vector3 ( 5.5f + 12f, Camera.main.transform.position.y, -4f + 9.5f );
				break;
			case ROOM_ID_GARAGE:
				FLUIControl.getInstance ().currentGaragePart = 0;
				positionToGo = new Vector3 ( 17.5f + 12f, Camera.main.transform.position.y, 4f + 9.5f );
				break;
		}
		
		iTween.MoveTo ( Camera.main.gameObject, iTween.Hash ( "time", 0.4f, "easetype", iTween.EaseType.easeInOutBack, "position", positionToGo ));
	}	
}
