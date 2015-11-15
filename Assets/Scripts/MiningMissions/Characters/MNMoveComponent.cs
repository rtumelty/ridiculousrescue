using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNMoveComponent : MonoBehaviour 
{
	//*************************************************************//	
	public bool isMoving = false;
	//*************************************************************//	
	private int[][] _myPath;
	private int _nodeID = 0;
	private IComponent _myIComponent;
	private Main.HandleChoosenCharacter _myCallBackOnFinish;
	private int[] _target;
	private bool _ommitLastNode;
	//*************************************************************//
	void Awake ()
	{
		_myIComponent = transform.Find ( "tile" ).GetComponent < IComponent > ();
		_myIComponent.myCharacterData = MNLevelControl.getInstance ().getCharacter ( _myIComponent.myID );
	}
	
	void Update ()
	{
		//print (_myPath);
		if ( isMoving )
		{
			if (( _myPath != null ) && ( _myPath[_nodeID] != null ))
			{
				if ( ! MNGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  ))
				{
					iTween.Stop ( gameObject );
					isMoving = false;
					transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
					MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
					_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
					_myIComponent.position = ToolsJerry.cloneTile ( _myIComponent.myCharacterData.position );
					MNGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
					transform.position = new Vector3 ((float) _myIComponent.position[0], (float) ( MNLevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
				}
			}
		}
	}
	
	public void initMove ( int[][] path, int[] target, bool ommitLastNode, CharacterData characterData, Main.HandleChoosenCharacter myCallBackOnFinish ) 
	{
		iTween.Stop ( this.gameObject );
		
		if (( _myPath != null ) && ( _myPath[_nodeID] != null ))
		{
			MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			MNGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
		}
		
		_myCallBackOnFinish = myCallBackOnFinish;
		if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( null, true );

		_nodeID = 1;
		_ommitLastNode = ommitLastNode;
		_target = target;
		
		if (( path != null ) && ( path[_nodeID] != null ) && ( ToolsJerry.compareTiles ( path[0], _myIComponent.position )) && MNLevelControl.getInstance ().allPathIsWalkableForCharacter ( _myIComponent.myID, this.gameObject, path ))
		{
			_myPath = path;
			switchTextures ( _myIComponent.position, _myPath[_nodeID] );
			
			if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]] != null )
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]] ) != -1 )
				{
					if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]] != this.gameObject )
					{
						_myIComponent.myCharacterData.blocked = false;
						isMoving = false;
						transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
						Vector3 positionToBumpTo = new Vector3 (((float) path[_nodeID][0] ) + ((float) ( _myIComponent.position[0] - path[_nodeID][0] )) / 2f, (float) ( MNLevelControl.LEVEL_HEIGHT - path[_nodeID][1] ), ((float) path[_nodeID][1] ) + ((float) ( _myIComponent.position[1] - path[_nodeID][1] )) / 2f + 0.33f - 0.5f );
						transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, positionToBumpTo );
						MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, Vector3.zero );
						
						if ( _myIComponent.myCharacterData.interactAction )
						{
							_myIComponent.myCharacterData.interactAction = false;
							if ( _myIComponent.myCharacterData.myCallBack != null ) _myIComponent.myCharacterData.myCallBack ( null, true );
						}
						return;
					}
				}
			}
			
			isMoving = true;
			MNGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( MNLevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition / 4f, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
		
			characterData.moving = true;
		}
		else
		{
			if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( _myIComponent.myCharacterData );
			else MessageCenter.getInstance ().createMarkInPosition ( target );
			
			characterData.moving = false;
		}
	}
	
	private void switchTextures ( int[] positionA, int[] positionB )
	{
		int xValue = positionB[0] - positionA[0];
		int zValue = positionB[1] - positionA[1];
		
		if ( xValue > 0 )
		{
			transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		}
		else if ( xValue < 0 )
		{
			transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_LEFT );
		}
		else if ( zValue > 0 )
		{
			transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_UP );
		}
		else if ( zValue < 0 )
		{
			transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_DOWN );
		}
	}	
	
	private void onCompleteTweenAnimation ()
	{
		if (( _myPath == null ) || ( _myPath[_nodeID] == null )) return;
		
		int[][] path = AStar.search ( _myIComponent.position, _target, _ommitLastNode, _myIComponent.myID, this.gameObject );
		_nodeID = 1;
		_myPath = path;
		
		if ( _myPath[_nodeID + 1] != null )
		{
			switchTextures ( _myPath[_nodeID], _myPath[_nodeID + 1] );
			if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] != null )
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] ) != -1 )
				{
					if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] != this.gameObject )
					{
						_myIComponent.myCharacterData.blocked = false;
						
						MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
						_myIComponent.myCharacterData.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
						_myIComponent.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
						isMoving = false;
						transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
						Vector3 positionToBumpTo = new Vector3 (((float) _myPath[_nodeID + 1][0] ) + ((float) ( _myIComponent.position[0] - _myPath[_nodeID + 1][0] )) / 2f, (float) ( MNLevelControl.LEVEL_HEIGHT - _myPath[_nodeID + 1][1] ), ((float) _myPath[_nodeID + 1][1] ) + ((float) ( _myIComponent.position[1] - _myPath[_nodeID + 1][1] )) / 2f + 0.33f - 0.5f );
						transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, positionToBumpTo );
						MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, Vector3.zero );
						
						if ( _myIComponent.myCharacterData.interactAction )
						{
							_myIComponent.myCharacterData.interactAction = false;
							_myIComponent.myCharacterData.myCallBack ( null, true );
						}
						
						return;
					}
				}
			}
			
			MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
			_myIComponent.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
			_nodeID++;
			MNGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( MNLevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition / 4f, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
			
			_myIComponent.myCharacterData.moving = true;
		}
		else if ( isMoving )
		{
			_myIComponent.myCharacterData.moving = false;
			_myIComponent.myCharacterData.blocked = false;
			MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
			_myIComponent.position = ToolsJerry.cloneTile ( _myIComponent.myCharacterData.position );
			if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( _myIComponent.myCharacterData );
			isMoving = false;
			if ( ! _myIComponent.myCharacterData.interactAction ) transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		}
	}
}
