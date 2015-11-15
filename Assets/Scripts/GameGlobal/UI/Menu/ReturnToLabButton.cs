using UnityEngine;
using System.Collections;

public class ReturnToLabButton : MonoBehaviour 
{
	//*************************************************************//	
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		//FlurryAnalytics.Instance ().LogEvent ( "Return to map button on - level " + LevelControl.SELECTED_LEVEL_NAME );
		if ( LevelControl.CURRENT_LEVEL_CLASS != null ) GoogleAnalytics.instance.LogScreen ( "Return to map button on - level " + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.SELECTED_LEVEL_NAME );
		if(GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN)
		{
			if((LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.RESCUE || LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.LABORATORY) && GameGlobalVariables.Stats.MOVES < LevelControl.CURRENT_LEVEL_CLASS.moves)
			{
				print ("Correct Call");
				Destroy(UIControl.currentPopupUI);
				UIControl.currentPopupUI = null;
				UIControl.getInstance().createPopup(UIControl.getInstance().backToMapScreen, true, "NULL", Camera.main.transform.Find("UI").transform.Find("UIPivot").gameObject.transform);

			}
			else
			{
				print ("Other Call");
				LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
				LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
				
				GlobalVariables.POPUP_UI_SCREEN = false;
				
				GameGlobalVariables.Stats.NewResources.reset ();
				FLMissionRoomManager.AFTER_INTRO = false;
				Application.LoadLevel ( "FL00ChooseLevel" );
			}
		}
		else
		{
			print ("Other Call");
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			GlobalVariables.POPUP_UI_SCREEN = false;
			
			GameGlobalVariables.Stats.NewResources.reset ();
			FLMissionRoomManager.AFTER_INTRO = false;
			Application.LoadLevel ( "FL00ChooseLevel" );
		}
	}
}