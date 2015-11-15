using UnityEngine;
using System.Collections;

public class LabWallsMovementControl : MonoBehaviour 
{
	//*************************************************************//
	private int _lastRoomIdOnUpdate = -1;
	//*************************************************************//
	void Update () 
	{
		if ( _lastRoomIdOnUpdate == FLUIControl.getInstance ().currentCameraAboveRoom ()) return;
		Vector3 positionToGo = Vector3.zero;
		switch ( FLUIControl.getInstance ().currentCameraAboveRoom ())
		{
			case FLNavigateButton.ROOM_ID_FACTORY:
				_lastRoomIdOnUpdate = FLNavigateButton.ROOM_ID_FACTORY;
				positionToGo = new Vector3 ( -0.1790485f, 11.076454f, 2.659571f ); 
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeInOutBack, "position", positionToGo, "islocal", true ));
				break;
			case FLNavigateButton.ROOM_ID_MISSION:
				positionToGo = new Vector3 ( -0.682518f, 11.076454f, 2.659571f ); 
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeInOutBack, "position", positionToGo, "islocal", true ));
				_lastRoomIdOnUpdate = FLNavigateButton.ROOM_ID_MISSION;
				break;
			case FLNavigateButton.ROOM_ID_PLAY:
				positionToGo = new Vector3 ( -0.682518f, 11.076454f, 3.322029f ); 
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeInOutBack, "position", positionToGo, "islocal", true ));
				_lastRoomIdOnUpdate = FLNavigateButton.ROOM_ID_PLAY;
				break;
			case FLNavigateButton.ROOM_ID_GARAGE:
				positionToGo = new Vector3 ( -1.079992f, 11.076454f, 2.659571f ); 
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeInOutBack, "position", positionToGo, "islocal", true ));
				_lastRoomIdOnUpdate = FLNavigateButton.ROOM_ID_GARAGE;
				break;
		}
	}
}
