using UnityEngine;
using System.Collections;

public class ReurnToLabScreenButton : MonoBehaviour {

	void OnMouseUp()
	{
		HandleTouched ();
	}

	void HandleTouched()
	{
		GameGlobalVariables.Stats.LIVES_AVAILABLE -= 1;
		SaveDataManager.save(SaveDataManager.LIVES_AVAILABLE, GameGlobalVariables.Stats.LIVES_AVAILABLE);
		NotificationsManager.Instance.ScheduleLivesNotification ();

		LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
		LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
		
		GlobalVariables.POPUP_UI_SCREEN = false;
		
		GameGlobalVariables.Stats.NewResources.reset ();
		FLMissionRoomManager.AFTER_INTRO = false;
		Application.LoadLevel ( "FL00ChooseLevel" );
	}
}
