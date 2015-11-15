using UnityEngine;
using System.Collections;

public class RestartScreenButton : MonoBehaviour {

	void OnMouseUp ()
	{
		HandleTouched ();
	}

	void HandleTouched ()
	{
		GameGlobalVariables.Stats.LIVES_AVAILABLE -= 1;
		SaveDataManager.save(SaveDataManager.LIVES_AVAILABLE, GameGlobalVariables.Stats.LIVES_AVAILABLE);
		NotificationsManager.Instance.ScheduleLivesNotification ();

		if ( GameGlobalVariables.RELEASE )
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			Application.LoadLevel ( "00ChooseLevel" );
		}
		else
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			Application.LoadLevel ( "01" );
		}
	}
}
