using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DragComponent : MonoBehaviour 
{
	//*************************************************************//	
#if UNITY_EDITOR
	private Vector3 _lastMousePosition;
#endif
	private bool _mouseDownOnMe = false;
	private GameObject _tileMarkerPrefab;
	private GameObject _tileMarkerInstant;
	private GameObject _tileMarkerArrow;
	private GameObject _tileMarkerArrowInstant;
	private IComponent _myIComponent;
	private Color _cantMoveMarkerColor = new Color ( 1f, 0.2f, 0.1f, 0.3f );
	private Color _canMoveMarkerColor = new Color ( 0.2f, 1f, 0.1f, 0.3f );
	private int[] _lastPositionOnPlaced;
	private bool _blockMove = false;
	//*************************************************************//	
	void Awake () 
	{
		_lastPositionOnPlaced = new int[2] { -1, -1 };
		
		collider.enabled = true;
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_tileMarkerArrow = ( GameObject ) Resources.Load ( "Tile/arrowMarker" );
		_tileMarkerPrefab = ( GameObject ) Resources.Load ( "Tile/tileMarker" );
		_tileMarkerInstant = ( GameObject ) Instantiate ( _tileMarkerPrefab, transform.root.position + Vector3.forward * 0.5f, _tileMarkerPrefab.transform.rotation );
		_tileMarkerInstant.name = "marker";
		_tileMarkerInstant.transform.parent = transform.root;
		
		_tileMarkerArrowInstant = ( GameObject ) Instantiate ( _tileMarkerArrow, transform.root.position, _tileMarkerArrow.transform.rotation );
		_tileMarkerArrowInstant.transform.parent = transform.root;
		_tileMarkerArrowInstant.transform.localPosition = new Vector3 ( 0f, 1.5f, -0.5f );
	}
	
	void Update () 
	{
		if ( _blockMove ) return;
		updateMyChildButtons ();
		
		if ( _mouseDownOnMe )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
			{
				if ( _lastMousePosition != Input.mousePosition )
				{
#else
			if ( Input.touchCount == 1 )
			{
				if ( Input.touches[0].phase == TouchPhase.Moved )
				{
#endif
					GlobalVariables.DRAGGING_OBJECT = true;
							
					Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
					if ( hitPosition != Vector3.zero )
					{
						int xHit = Mathf.RoundToInt ( hitPosition.x );
						int zHit = Mathf.RoundToInt ( hitPosition.z );
						
						if ( LevelControl.getInstance ().isTileInLevelBoudaries ( xHit, zHit ))
						{
							float additionalAdd = 0f;
							if ( zHit >= LevelControl.LEVEL_HEIGHT ) additionalAdd = 0.5f + zHit - LevelControl.LEVEL_HEIGHT;
							transform.root.position = new Vector3 ((float) xHit, (float) ( LevelControl.LEVEL_HEIGHT - zHit ) + 3f + additionalAdd, (float) zHit - 0.5f );		
							placeObjectOnGrid ();
						}
					}
				}
#if UNITY_EDITOR
				_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
			}
			else
			{
				_mouseDownOnMe = false;
				GlobalVariables.DRAGGING_OBJECT = false;
				placeObjectOnGrid ();
			}
		}
	}
			
	public void destoryMarkers ()
	{
		Destroy ( _tileMarkerArrowInstant );
		Destroy ( _tileMarkerInstant );
	}
			
	public void updateMyChildButtons ()
	{
		int x = Mathf.RoundToInt ( transform.root.position.x );
		int z = Mathf.RoundToInt ( transform.root.position.z + 0.5f );
		
		bool mayBePlaced = false;
		bool tutorialModeSoOnlyBackTile = false;
		if ( GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, x, z, transform.root.gameObject, _myIComponent.myID, true ))
		{
			if ( GlobalVariables.TUTORIAL_MENU )
			{
				tutorialModeSoOnlyBackTile = true;
			}
			
			mayBePlaced = true;
		}
		
		if ( transform.Find ( "yesButton" ) != null )
		{
			if ( ! mayBePlaced )
			{
				if ( ! tutorialModeSoOnlyBackTile )
				{
					transform.Find ( "yesButton" ).renderer.material.mainTexture = UIControl.getInstance ().textureYesGreyedOutUp;
					transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureYesGreyedOutUp;
					transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureYesGreyedOutDown;
		
					transform.Find ( "yesButton" ).GetComponent < BoxCollider > ().enabled = false;
				}
					
				changeColorOfMarker ( _cantMoveMarkerColor );
			}
			else
			{
				if ( ! tutorialModeSoOnlyBackTile )
				{
					transform.Find ( "yesButton" ).renderer.material.mainTexture = UIControl.getInstance ().textureYesUp;
					transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureYesUp;
					transform.Find ( "yesButton" ).GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureYesDown;
		
					transform.Find ( "yesButton" ).GetComponent < BoxCollider > ().enabled = true;
				}
						
				changeColorOfMarker ( _canMoveMarkerColor );
			}
		}
	}
			
	private void changeColorOfMarker ( Color color )
	{
		_tileMarkerInstant.renderer.material.color = color;
		GameObject.Find ( "tileMarkerCharacters(Clone)" ).renderer.material = _tileMarkerInstant.renderer.material;
	}
			
	private void placeObjectOnGrid ()
	{
		updateMyChildButtons ();
				
		int x = Mathf.RoundToInt ( transform.root.position.x );
		int z = Mathf.RoundToInt ( transform.root.position.z + 0.5f );
				
		transform.root.position = new Vector3 ( x, (float) ( LevelControl.LEVEL_HEIGHT - z ) + 3f, z - 0.5f );
		
		if ( ! GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _lastPositionOnPlaced[0], _lastPositionOnPlaced[1], transform.root.gameObject, _myIComponent.myID, false ))
		{
			//means it is from UI like redirector
			Debug.Log ( "Coundnt fill tile for object: " + transform.root.gameObject.name + " on position: " + x + " | " + z );				
		}
		
		// I must do it again as if object was draged to same _myIComponent.position it could clean its _myIComponent.position
		GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, x, z, transform.root.gameObject, _myIComponent.myID, false );
				
		_myIComponent.position[0] = _lastPositionOnPlaced[0] = x;
		_myIComponent.position[1] = _lastPositionOnPlaced[1] = z;
		
		if ( Array.IndexOf ( GameElements.REDIRECTORS, _myIComponent.myID ) != -1 )
		{
			Main.getInstance ().handlePutRedirector ( this );
		}
	}
			
	void OnMouseDown ()
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		
		handleTouched ();
	}
			
	private void handleTouched ()
	{
				
		if ( _blockMove ) return;
#if UNITY_EDITOR
		_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
		Main.getInstance ().handlePutRedirector ( this );
		_mouseDownOnMe = true;
	}
			
	public void externallyTurnOffMouseDownOnMe ()
	{
		_mouseDownOnMe = false;	
	}
			
	public void blockMove ()
	{
		_blockMove = true;	
	}
			
	public void externallyInvokeMouseDownOnMe ()
	{
		Main.getInstance ().handlePutRedirector ( this );
		updateMyChildButtons ();
				
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, false, true, true );
				
		placeObjectOnGrid ();
	}
			
	public void revokeFromCurrentPosition ()
	{
		if ( ! GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], transform.root.gameObject, _myIComponent.myID, false ))
		{
			Debug.Log ( "Coundnt fill tile for object: " + transform.root.gameObject.name + " on position: " + _myIComponent.position[0] + " | " + _myIComponent.position[1] );
		}
	}
}
