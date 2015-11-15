using UnityEngine;
using System.Collections;

public class FLFollowingPlayerFromRoomToRoomComponent : MonoBehaviour 
{
	//*************************************************************//
	private int _iAmInRoom;
	private IComponent _myIComponent;
	//*************************************************************//
	void Awake () 
	{
		_myIComponent = transform.Find ( "tile" ).GetComponent < IComponent > ();
	}
	
	void Update () 
	{
		if ( FLUIControl.getInstance ().currentCameraAboveRoom () == FLNavigateButton.ROOM_ID_FACTORY )
		{
			if ( _iAmInRoom != FLNavigateButton.ROOM_ID_FACTORY )
			{
				int[] emptyTile = ToolsJerry.findClossestEmptyTileAroundThisTileLaboratory ( 5, 11, _myIComponent.position, _myIComponent.myID, this.gameObject, 1, new int[2] { 5, 11 });   
				int[][] path = AStar.search ( _myIComponent.position, emptyTile, false, _myIComponent.myID, this.gameObject );
				if ( gameObject.GetComponent < FLMoveComponent > ().initMove ( path, emptyTile, false, _myIComponent.myCharacterData, null ))
				{
					_iAmInRoom = FLNavigateButton.ROOM_ID_FACTORY;
				}
			}
		}
		else if ( FLUIControl.getInstance ().currentCameraAboveRoom () == FLNavigateButton.ROOM_ID_MISSION )
		{
			if ( _iAmInRoom != FLNavigateButton.ROOM_ID_MISSION )
			{
				int[] emptyTile = ToolsJerry.findClossestEmptyTileAroundThisTileLaboratory ( 19, 9, _myIComponent.position, _myIComponent.myID, this.gameObject, 1, new int[2] { 19, 10 });   
				int[][] path = AStar.search ( _myIComponent.position, emptyTile, false, _myIComponent.myID, this.gameObject );
				if ( gameObject.GetComponent < FLMoveComponent > ().initMove ( path, emptyTile, false, _myIComponent.myCharacterData, null ))
				{
					_iAmInRoom = FLNavigateButton.ROOM_ID_MISSION;
				}
			}
		}
		else if ( FLUIControl.getInstance ().currentCameraAboveRoom () == FLNavigateButton.ROOM_ID_PLAY )
		{
			if ( _iAmInRoom != FLNavigateButton.ROOM_ID_PLAY )
			{
				int[] emptyTile = ToolsJerry.findClossestEmptyTileAroundThisTileLaboratory ( 17, 4, _myIComponent.position, _myIComponent.myID, this.gameObject, 1, new int[2] { 17, 4 });   
				int[][] path = AStar.search ( _myIComponent.position, emptyTile, false, _myIComponent.myID, this.gameObject );
				if ( gameObject.GetComponent < FLMoveComponent > ().initMove ( path, emptyTile, false, _myIComponent.myCharacterData, null ))
				{
					_iAmInRoom = FLNavigateButton.ROOM_ID_PLAY;
				}
			}
		}
		else if ( FLUIControl.getInstance ().currentCameraAboveRoom () == FLNavigateButton.ROOM_ID_GARAGE )
		{
			if ( _iAmInRoom != FLNavigateButton.ROOM_ID_GARAGE )
			{
				int[] emptyTile = ToolsJerry.findClossestEmptyTileAroundThisTileLaboratory ( 25, 11, _myIComponent.position, _myIComponent.myID, this.gameObject, 1, new int[2] { 25, 11 });   
				int[][] path = AStar.search ( _myIComponent.position, emptyTile, false, _myIComponent.myID, this.gameObject );
				if ( gameObject.GetComponent < FLMoveComponent > ().initMove ( path, emptyTile, false, _myIComponent.myCharacterData, null ))
				{
					_iAmInRoom = FLNavigateButton.ROOM_ID_GARAGE;
				}
			}
		}
	}
}
