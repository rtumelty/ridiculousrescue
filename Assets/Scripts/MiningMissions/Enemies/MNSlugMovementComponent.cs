using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNSlugMovementComponent : MonoBehaviour 
{
	//*************************************************************//	
	public bool isMoving = false;

	public int[] position1;
	public int[] position2;
	public int[] position3;
	public int[] position4;
	//*************************************************************//	
	private int[][] _myPath;
	private int _nodeID = 0;
	private IComponent _myIComponent;
	private int[] _target;
	private bool _mayMove = false;
	private int _currentTargetPosition = 0;
	//*************************************************************//
	void Start ()
	{
		_myIComponent = transform.GetComponent < IComponent > ();
	}
	
	void Update ()
	{
		if ( _myIComponent.myEnemyData.dieInProgress ) return;

		if ( isMoving )
		{
			if (( _myPath != null ) && ( _myPath[_nodeID] != null ))
			{
				if ( ! GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  ))
				{
					if ( Array.IndexOf ( GameElements.CHARACTERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][_myPath[_nodeID][0]][_myPath[_nodeID][1]] ) != -1 )
					{
						CharacterData characterToAttack = LevelControl.getInstance ().getCharacterFromPosition ( _myPath[_nodeID] );
						if ( characterToAttack != null && ! characterToAttack.interactAction && ! characterToAttack.moving )
						{
							isMoving = false;
							iTween.Stop ( transform.gameObject );
							transform.position = new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
							transform.GetComponent < MNEnemyComponent > ().fromTapFromPlayer = false;
							Main.getInstance ().interactWithParticularCharacter ( characterToAttack, gameObject.GetComponent < MNEnemyComponent > ().handleAttackedByCharacted, CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE, _myIComponent ); 
						}
					}

					iTween.Stop ( gameObject );
					isMoving = false;
					transform.GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION );
					GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
					_myIComponent.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
					GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
					transform.position = new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
				}
			}
		}

		if ( ! _mayMove ) return;

		if ( ! isMoving && ! _myIComponent.myEnemyData.attacking )
		{
			if ( ToolsJerry.compareTiles ( _myIComponent.position, position1 ))
			{
				_currentTargetPosition = 2;
				int[][] path = AStar.search ( _myIComponent.position, position2, false, _myIComponent.myID, this.gameObject );
				if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
				{
					int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position2[0], position2[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
					path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
					initMove ( path, aroundTileB ); 
					print ( aroundTileB[0] + " | " + aroundTileB[1] );
				}
				else
				{
					initMove ( path, position2 );
				}
			}
			else if ( ToolsJerry.compareTiles ( _myIComponent.position, position2 ))
			{
				_currentTargetPosition = 3;
				int[][] path = AStar.search ( _myIComponent.position, position3, false, _myIComponent.myID, this.gameObject );
				if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
				{
					int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position3[0], position3[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
					path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
					initMove ( path, aroundTileB ); 
					print ( aroundTileB[0] + " | " + aroundTileB[1] );
				}
				else
				{
					initMove ( path, position3 );
				}
			}
			else if ( ToolsJerry.compareTiles ( _myIComponent.position, position3 ))
			{
				_currentTargetPosition = 4;
				int[][] path = AStar.search ( _myIComponent.position, position4, false, _myIComponent.myID, this.gameObject );
				if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
				{
					int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position4[0], position4[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
					path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
					initMove ( path, aroundTileB ); 
					print ( aroundTileB[0] + " | " + aroundTileB[1] );
				}
				else
				{
					initMove ( path, position4 );
				}
			}
			else if ( ToolsJerry.compareTiles ( _myIComponent.position, position4 ))
			{
				_currentTargetPosition = 1;
				int[][] path = AStar.search ( _myIComponent.position, position1, false, _myIComponent.myID, this.gameObject );
				if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
				{
					int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position1[0], position1[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
					path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
					initMove ( path, aroundTileB ); 
					print ( aroundTileB[0] + " | " + aroundTileB[1] );
				}
				else
				{
					initMove ( path, position1 );
				}
			}
			else
			{
				int[][] path = null;
				switch ( _currentTargetPosition )
				{
				case 1:
					path = AStar.search ( _myIComponent.position, position1, false, _myIComponent.myID, this.gameObject );
					if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
					{
						int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position1[0], position1[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
						path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
						initMove ( path, aroundTileB ); 
						print ( aroundTileB[0] + " | " + aroundTileB[1] );
					}
					else
					{
						initMove ( path, position1 );
					}
					break;
				case 2:
					path = AStar.search ( _myIComponent.position, position2, false, _myIComponent.myID, this.gameObject );
					if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
					{
						int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position2[0], position2[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
						path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
						initMove ( path, aroundTileB ); 
						print ( aroundTileB[0] + " | " + aroundTileB[1] );
					}
					else
					{
						initMove ( path, position2 );
					}
					break;
				case 3:
					path = AStar.search ( _myIComponent.position, position3, false, _myIComponent.myID, this.gameObject );
					if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
					{
						int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position3[0], position3[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
						path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
						initMove ( path, aroundTileB ); 
						print ( aroundTileB[0] + " | " + aroundTileB[1] );
					}
					else
					{
						initMove ( path, position3 );
					}
					break;
				case 4:
					path = AStar.search ( _myIComponent.position, position4, false, _myIComponent.myID, this.gameObject );
					if (( path == null ) || ( path[1] == null ) || ( ! ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
					{
						int[] aroundTileB = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( position4[0], position4[1], _myIComponent.position, _myIComponent.myID, this.gameObject, 1 );
						path = AStar.search ( _myIComponent.position, aroundTileB, false, _myIComponent.myID, this.gameObject );
						initMove ( path, aroundTileB ); 
						print ( aroundTileB[0] + " | " + aroundTileB[1] );
					}
					else
					{
						initMove ( path, position4 );
					}
					break;
				}
			}
		}
	}
	
	public void initMove ( int[][] path, int[] target ) 
	{
		_mayMove = true;
		iTween.Stop ( this.gameObject );
		
		if (( _myPath != null ) && ( _myPath[_nodeID] != null ))
		{
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
		}
		
		_target = target;
		
		_nodeID = 1;
		
		if (( path != null ) && ( path[_nodeID] != null ) && ( ToolsJerry.compareTiles ( path[0], _myIComponent.position )))
		{
			_myPath = path;
			switchTextures ( _myIComponent.position, _myPath[_nodeID] );
			
			isMoving = true;
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( LevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
		}
	}
	
	private void switchTextures ( int[] position1, int[] position2 )
	{
		int xValue = position2[0] - position1[0];
		int zValue = position2[1] - position1[1];
		
		if ( xValue > 0 )
		{
			transform.GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.MOVE_ANIMATION_RIGHT );
		}
		else if ( xValue < 0 )
		{
			transform.GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.MOVE_ANIMATION_LEFT );
		}
		else if ( zValue > 0 )
		{
			transform.GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.MOVE_ANIMATION_UP );
		}
		else if ( zValue < 0 )
		{
			transform.GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.MOVE_ANIMATION_DOWN );
		}
	}	
	
	private void onCompleteTweenAnimation ()
	{
		if (( _myPath == null ) || ( _myPath[_nodeID] == null )) return;
		
		int[][] path = AStar.search ( _myIComponent.position, _target, false, _myIComponent.myID, this.gameObject );
		_nodeID = 1;
		_myPath = path;
		
		if ( _myPath != null && _myPath[_nodeID + 1] != null && ToolsJerry.compareTiles ( _myIComponent.position, _myPath[0] ))
		{
			switchTextures ( _myPath[_nodeID], _myPath[_nodeID + 1] );
			
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
			_nodeID++;
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( LevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
		}
		else if ( isMoving )
		{
			GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.position = new int[2] { Mathf.RoundToInt ( transform.position.x ), Mathf.RoundToInt ( transform.position.z + 0.5f )};
			GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			
			isMoving = false;
			transform.GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION );
		}
	}
}
