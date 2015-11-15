using UnityEngine;
using System.Collections;

public class FLMissionScreenConfirmButtonControl : MonoBehaviour 
{
	//*************************************************************//
	public GameGlobalVariables.Missions.LevelClass myLevelClass;
	public GameGlobalVariables.Missions.TrainLevelClass myTrainLevelClass;
	public GameGlobalVariables.Missions.MiningLevelClass myMineClass;
	public bool playTrain = false;
	public bool playMine = false;
	public static string MINING_LEVEL_PLAYED = "NULL";
	//*************************************************************//
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if(GameGlobalVariables.Stats.LIVES_AVAILABLE < 1)
		{
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
			Destroy(FLUIControl.currentBlackOutUI);
			GameObject menuScreen = FLUIControl.getInstance ().createPopup ( FLUIControl.getInstance()._buyLivesPrefab,true);
			print ("Play Blocked");
			return;
		}

		if(playTrain == false && playMine == false)
		{
			MemoryManager.getInstance ().clean ();

			FLGlobalVariables.MISSION_SCREEN = false;
			FLGlobalVariables.POPUP_UI_SCREEN = false;

			LevelControl.SELECTED_LEVEL_NAME = myLevelClass.myName;
			int.TryParse ( LevelControl.SELECTED_LEVEL_NAME, out LevelControl.LEVEL_ID );

			if ( LevelControl.LEVEL_ID < 10 ) LevelControl.SELECTED_LEVEL_NAME = "0" + LevelControl.LEVEL_ID.ToString ();

			LevelControl.CURRENT_LEVEL_CLASS = myLevelClass;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();

			FLMissionRoomManager.getInstance ().rememberWorldIWasOn ();

			if ( ! FLMissionRoomManager.AFTER_INTRO && LevelControl.SELECTED_LEVEL_NAME == "01" && myLevelClass.type != FLMissionScreenNodeManager.TYPE_BONUS_NODE )
			{
				Application.LoadLevel ( "00Intro" );
				return;
			}

			Application.LoadLevel ( "01" );
		}
		else if(playTrain == true)
		{
			FLGlobalVariables.MISSION_SCREEN = false;
			FLGlobalVariables.POPUP_UI_SCREEN = false;

			TRLevelControl.SELECTED_LEVEL_NAME = myTrainLevelClass.myName;
			int.TryParse ( TRLevelControl.SELECTED_LEVEL_NAME, out TRLevelControl.LEVEL_ID );
			TRLevelControl.CURRENT_LEVEL_CLASS = myTrainLevelClass;
			
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.TRAIN );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			FLMissionRoomManager.getInstance ().rememberWorldIWasOn ();
			
			MemoryManager.getInstance ().clean ();
			
			Application.LoadLevel ( "00MemoryCleaner" );
		}

		else if(playMine == true)
		{
			FLGlobalVariables.MISSION_SCREEN = false;
			FLGlobalVariables.POPUP_UI_SCREEN = false;
			
			MNLevelControl.SELECTED_LEVEL_NAME = myMineClass.myName;
			
			int.TryParse ( MNLevelControl.SELECTED_LEVEL_NAME, out MNLevelControl.LEVEL_ID );
			MNLevelControl.CURRENT_LEVEL_CLASS = myMineClass;
			
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.MINING );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			FLMissionRoomManager.getInstance ().rememberWorldIWasOn ();
			
			MINING_LEVEL_PLAYED = myMineClass.myName;
			
			GameGlobalVariables.MINE_ENTERED = 1;
			Application.LoadLevel ( "MN01" );
		}
	}
}
