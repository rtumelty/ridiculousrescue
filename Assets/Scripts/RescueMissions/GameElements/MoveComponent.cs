using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoveComponent : MonoBehaviour 
{
	//*************************************************************//
	public delegate void HandleChoosenCharacter ( CharacterData characterChoosen, bool onlyUnblocking = false );
	//*************************************************************//	
	public bool isMoving = false;
	//*************************************************************//	
	private int[][] _myPath;
	private int _nodeID = 0;
	private IComponent _myIComponent;
	private Main.HandleChoosenCharacter _myCallBackOnFinish;
	private bool _electricShockExecuted = false;
	private int[] _tileJustBefore;
	private int[] _target;
	private bool _ommitLastNode;
	private float divisionForSpeed = 6f;
	//*************************************************************//
	void Awake ()
	{
		_myIComponent = transform.Find ( "tile" ).GetComponent < IComponent > ();
		_myIComponent.myCharacterData = LevelControl.getInstance ().getCharacter ( _myIComponent.myID );
	}
	
	void Start ()
	{
		_tileJustBefore = ToolsJerry.cloneTile ( _myIComponent.position );
	}
	
	public void initMove ( int[][] path, int[] target, bool ommitLastNode, CharacterData characterData, Main.HandleChoosenCharacter myCallBackOnFinish ) 
	{
		if(GlobalVariables.RESTRICT_MADRA_TUTORIAL_LEVEL_3 == true)
		{
			if(target[0] < 8|| target[1] < 6)
			{
				return;
			}
		}
		iTween.Stop ( this.gameObject );

		if (( _myPath != null ) && ( _myPath[_nodeID] != null ))
		{

			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			//GlobalVariables.READY_FOR_NEXT_TURN = true;
		}
		else
		{
			//GlobalVariables.READY_FOR_NEXT_TURN = true;
		}
		//Pure test booooy
		/*else
		{
			print ("This blocker?");
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
		}*/
		
		_target = target;
		_ommitLastNode = ommitLastNode;
		
		_myCallBackOnFinish = myCallBackOnFinish;
		if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( null, true );
		
		_nodeID = 1;

		if (( path != null ) && ( path[_nodeID] != null ) && ( ToolsJerry.compareTiles ( path[0], _myIComponent.position )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( _myIComponent.myID, this.gameObject, path ))
		{
			_myPath = path;
			switchTextures ( _myIComponent.position, _myPath[_nodeID] );
			
			if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]] != null )
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]] ) != -1 )
				{
					if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]] != this.gameObject )
					{
						_electricShockExecuted = false;
						_myIComponent.myCharacterData.blocked = false;
						isMoving = false;
						transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
						Vector3 positionToBumpTo = new Vector3 (((float) path[_nodeID][0] ) + ((float) ( _myIComponent.position[0] - path[_nodeID][0] )) / 2f, (float) ( LevelControl.LEVEL_HEIGHT - path[_nodeID][1] ), ((float) path[_nodeID][1] ) + ((float) ( _myIComponent.position[1] - path[_nodeID][1] )) / 2f + 0.33f - 0.5f );
						transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, positionToBumpTo );
						LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][path[_nodeID][0]][path[_nodeID][1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, Vector3.zero );
						
						if ( _myIComponent.myCharacterData.interactAction )
						{
							_myIComponent.myCharacterData.interactAction = false;
							Main.getInstance ().revokeRedirectorCallBack ();
							if ( _myIComponent.myCharacterData.myCallBack != null ) _myIComponent.myCharacterData.myCallBack ( null, true );
						}
						return;
					}
				}
			}
			
			_tileJustBefore = ToolsJerry.cloneTile ( _myIComponent.position );
			
			isMoving = true;
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( LevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
//			print ("TIMES");
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition / divisionForSpeed, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
		}
		else
		{
			if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][_myIComponent.position[0]][_myIComponent.position[1]] == GameElements.EMPTY || Array.IndexOf ( GameElements.BUILDERS, _myIComponent.myID ) != -1 )
			{
				if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( _myIComponent.myCharacterData );
				else MessageCenter.getInstance ().createMarkInPosition ( target );
			}
		}
//		print ("MyPath" + _myPath);
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
		
		//int[][] path = AStar.search ( _myIComponent.position, _target, _ommitLastNode, _myIComponent.myID, this.gameObject );
		//_nodeID = 1;
		//_myPath = path;
		
		if ( /*_myPath != null && */  _myPath[_nodeID + 1] != null /*&& Tools.compareTiles ( _myIComponent.position, _myPath[0] )&& LevelControl.getInstance ().allPathIsWalkableForCharacter ( _myIComponent.myID, this.gameObject, _myPath )*/)
		{
			_tileJustBefore = ToolsJerry.cloneTile ( _myIComponent.position );
			
			switchTextures ( _myPath[_nodeID], _myPath[_nodeID + 1] );
			if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] != null )
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] ) != -1 )
				{
					if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] != this.gameObject )
					{
						_electricShockExecuted = false;
						_myIComponent.myCharacterData.blocked = false;
						
						GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
						_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
						_myIComponent.position = ToolsJerry.cloneTile ( _myIComponent.myCharacterData.position );
						isMoving = false;
						transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
						transform.position = new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT -  _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
						Vector3 positionToBumpTo = new Vector3 (((float) _myPath[_nodeID + 1][0] ) + ((float) ( _myIComponent.position[0] - _myPath[_nodeID + 1][0] )) / 2f, (float) ( LevelControl.LEVEL_HEIGHT - _myPath[_nodeID + 1][1] ), ((float) _myPath[_nodeID + 1][1] ) + ((float) ( _myIComponent.position[1] - _myPath[_nodeID + 1][1] )) / 2f + 0.33f - 0.5f );
						transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, positionToBumpTo );
						LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, Vector3.zero );

						if ( _myIComponent.myCharacterData.interactAction )
						{
							_myIComponent.myCharacterData.interactAction = false;
							Main.getInstance ().revokeRedirectorCallBack ();
							_myIComponent.myCharacterData.myCallBack ( null, true );
						}
						
						return;
					}
				}
			}

			if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][_myPath[_nodeID + 1][0]][_myPath[_nodeID + 1][1]] != GameElements.EMPTY )
			{
				_electricShockExecuted = false;
				_myIComponent.myCharacterData.blocked = false;
				
				GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
				_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
				_myIComponent.position = ToolsJerry.cloneTile ( _myIComponent.myCharacterData.position );
				isMoving = false;
				transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
				transform.position = new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT -  _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
				if ( _myIComponent.myCharacterData.interactAction )
				{
					_myIComponent.myCharacterData.interactAction = false;
					Main.getInstance ().revokeRedirectorCallBack ();
					_myIComponent.myCharacterData.myCallBack ( null, true );
				}
				return;
			}
			
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
			_myIComponent.position = ToolsJerry.cloneTile ( _myIComponent.myCharacterData.position );
			_nodeID++;
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( LevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition / divisionForSpeed, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
		}
		else if ( isMoving )
		{
			_electricShockExecuted = false;
			_myIComponent.myCharacterData.blocked = false;
			
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
			_myIComponent.position = ToolsJerry.cloneTile ( _myIComponent.myCharacterData.position );
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			
			if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][_myIComponent.position[0]][_myIComponent.position[1]] == GameElements.EMPTY || Array.IndexOf ( GameElements.BUILDERS, _myIComponent.myID ) != -1 )
			{
				if ( _myCallBackOnFinish != null && Vector3.Distance ( new Vector3 ((float) _myIComponent.position[0], 0f, (float) _myIComponent.position[1]), new Vector3 ((float) _target[0], 0f, (float) _target[1])) <= 1.5f )
				{
					_myCallBackOnFinish ( _myIComponent.myCharacterData );
				}
			}
			
			isMoving = false;
			if(GameGlobalVariables.ACTIVE_FROM_ENEMY == false)
			{
				GlobalVariables.READY_FOR_NEXT_TURN = true;
			}
//			print ("Single call for tentacle spawn & unlock character turn");
			if ( ! _myIComponent.myCharacterData.interactAction )
			{
				transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
			}
		}
	}
	
	void Update ()
	{
		if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][_myIComponent.position[0]][_myIComponent.position[1]] != GameElements.EMPTY )
		{
			if ( ! GlobalVariables.TOY_LAZARUS_SEQUENCE )
			{
				if ( Array.IndexOf ( GameElements.BUILDERS, _myIComponent.myCharacterData.myID ) == -1 )
				{
					executeElectricShock ();
				}
			}
		}
	}	
	
	private void executeElectricShock ()
	{
		if ( _electricShockExecuted ) return;
		
		_electricShockExecuted = true;
		_myIComponent.myCharacterData.blocked = true;

		/*
		bool justCheck = false;

		if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE && GlobalVariables.CORA_ALREADY_WITH_TOY_ON_TROLEY )
		{
			justCheck = true;
		}

		if ( ! GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _tileJustBefore[0], _tileJustBefore[1], this.gameObject, _myIComponent.myID, justCheck ))
		{
			if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_tileJustBefore[0]][_tileJustBefore[1]] != null )
			{
				LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_tileJustBefore[0]][_tileJustBefore[1]].SendMessage ( "onCompleteElectricShock" );
				LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_tileJustBefore[0]][_tileJustBefore[1]] = null;
				LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][_tileJustBefore[0]][_tileJustBefore[1]] = GameElements.EMPTY;
			}
			
			if ( ! GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _tileJustBefore[0], _tileJustBefore[1], this.gameObject, _myIComponent.myID, justCheck ))
			{
				_tileJustBefore = Tools.findClossestEmptyTileAroundThisTile ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 2 );
			}
		}
		
		if ( isMoving )
		{
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};;
			_myIComponent.position = Tools.cloneTile ( _myIComponent.myCharacterData.position );
		}
		*/
		
		if ( Array.IndexOf ( GameElements.RESCUERS, _myIComponent.myID ) != -1 )
		{
			if ( _myIComponent.myCharacterData.interactAction )
			{
				transform.Find ( "tile" ).GetComponent < RescuerComponent > ().leaveTroley ();
			}
		}
		
		transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ELECTROCUTED_ANIMATION, 1f );
		
		_myPath = null;
		
		transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().electrickShock ();
		
		Handheld.Vibrate ();
		
		SoundManager.getInstance ().playSound ( SoundManager.ELECTROCUTED );
		GlobalVariables.READY_FOR_NEXT_TURN = true;
		print ("Ok Again");
	}
	
	// called from SelectedComponent after electic shock
	private void onCompleteElectricShock ()
	{
		int[] clossestEmptyTile = null;
		
		if ( ! ToolsJerry.compareTiles ( _tileJustBefore, _myIComponent.position ))
		{
			clossestEmptyTile = ToolsJerry.findClossestEmptyTileAroundThisTile ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 2, _tileJustBefore );
		
			if ( clossestEmptyTile[0] == -1 )
			{
				clossestEmptyTile = ToolsJerry.findClossestEmptyTileAroundThisTile ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 2 );
			}
		}
		else
		{
			clossestEmptyTile = ToolsJerry.findClossestEmptyTileAroundThisTile ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 2 );
		}
		
		if ( clossestEmptyTile[0] == -1 )
		{
			_electricShockExecuted = false;
			return;
		}

		transform.position = new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT -  _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
		
		_myPath = null;
		int[][] path = AStar.search ( _myIComponent.position, clossestEmptyTile, false, _myIComponent.myID, gameObject );
		
		if ( path != null )	isMoving = true;
		else isMoving = false;
		
		initMove ( path, clossestEmptyTile, false, _myIComponent.myCharacterData, null );
	}
}
