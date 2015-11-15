using UnityEngine;
using System.Collections;

public class FLMoveComponent : MonoBehaviour 
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
		_myIComponent.myCharacterData = FLLevelControl.getInstance ().getCharacter ( _myIComponent.myID );
		_myIComponent.myCharacterData.position = ToolsJerry.cloneTile ( _myIComponent.position );
	}
	
	public bool initMove ( int[][] path, int[] target, bool ommitLastNode, CharacterData characterData, Main.HandleChoosenCharacter myCallBackOnFinish ) 
	{
		if (( _myPath != null ) && ( _myPath[_nodeID] != null ))
		{
			FLGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			FLGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
		}
		
		_myCallBackOnFinish = myCallBackOnFinish;
		
		_nodeID = 1;
		_ommitLastNode = ommitLastNode;
		_target = target;
		
		if (( path != null ) && ( path[_nodeID] != null ) && ( ToolsJerry.compareTiles ( path[0], _myIComponent.position )) && FLLevelControl.getInstance ().allPathIsWalkableForCharacter ( _myIComponent.myID, this.gameObject, path ))
		{
			_myPath = path;
			
			switchTextures ( _myIComponent.position, _myPath[_nodeID] );
			isMoving = true;
			FLGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( FLLevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition / 4f, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
			
			return true;
		}
		else
		{
			if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( _myIComponent.myCharacterData );
			else MessageCenter.getInstance ().createMarkInPosition ( target );
			
			return false;
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
			
			FLGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
			_myIComponent.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
			_nodeID++;
			FLGridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, _myPath[_nodeID][0], _myPath[_nodeID][1], this.gameObject, _myIComponent.myID  );
			
			Vector3 positionToGo = new Vector3 ((float) _myPath[_nodeID][0], (float) ( FLLevelControl.LEVEL_HEIGHT - _myPath[_nodeID][1] ), (float) _myPath[_nodeID][1] - 0.5f );
			float distanceToNewPosition = Vector3.Distance ( transform.position, positionToGo );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", distanceToNewPosition / 4f, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "onCompleteTweenAnimation" ));
		}
		else if ( isMoving )
		{
			_myIComponent.myCharacterData.blocked = false;
			
			FLGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID  );
			_myIComponent.myCharacterData.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
			_myIComponent.position = ToolsJerry.cloneTile ( _myPath[_nodeID] );
			if ( _myCallBackOnFinish != null ) _myCallBackOnFinish ( _myIComponent.myCharacterData );
			isMoving = false;
			transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		}
	}
}
