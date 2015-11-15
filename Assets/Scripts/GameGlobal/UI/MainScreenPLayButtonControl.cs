using UnityEngine;
using System.Collections;

public class MainScreenPLayButtonControl : MonoBehaviour 
{
	//*************************************************************//	
	public string WhatToLoadLevel = "00Instructions";
	public bool doNotLoadScreen = false;
	public bool loadLevel01 = false;
	public bool fromFailScreen = false;
	//*************************************************************//	
	void OnMouseUp ()
	{
		if ( WhatToLoadLevel != "01" ) gameObject.collider.enabled = false;
		
		handleTouched ();
	}
	void Update()
	{
		if(Input.GetKeyDown("t"))
		{
			SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
		}
	}
	private IEnumerator waitForClick()
	{
		yield return new WaitForSeconds (0.4f);
		Application.LoadLevel ( "00ChooseLevel" );
	}
	private void handleTouched ()
	{
		if(fromFailScreen == true)
		{
			GameGlobalVariables.Stats.LIVES_AVAILABLE -= 1;
			SaveDataManager.save(SaveDataManager.LIVES_AVAILABLE, GameGlobalVariables.Stats.LIVES_AVAILABLE);
			NotificationsManager.Instance.ScheduleLivesNotification ();
		}
		if(GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING)
		{
			GameGlobalVariables.I_WAS_IN_MINES = 1;
			SaveDataManager.save(SaveDataManager.MINES_ENTERED, GameGlobalVariables.I_WAS_IN_MINES);
		}
		if ( WhatToLoadLevel == "00ChooseGamePart" )
		{
			/*
		PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "3" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "TR" + "3" );
		PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( -783 ).ToString ());
			//PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 22 ).ToString ());
		//PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "MN" + "2" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "26" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "26" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 26 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "25" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "25" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 25 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "24" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "24" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 24 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "23" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "23" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 23 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "22" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "22" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 22 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "21" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "21" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 21 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "20" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "20" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 20 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "19" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "19" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 19 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "18" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "18" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 18 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "17" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "17" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 17 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "16" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "16" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 16 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "15" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "15" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 15 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "14" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "14" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 14 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "13" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "13" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 13 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "12" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "12" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 12 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LOCK_ON_LEVEL_DESTROYED_PREFIX + "13" );

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "TR" + "2" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "2" );

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( -782 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + "2" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "MN" + "2" );

			PlayerPrefs.DeleteKey ( SaveDataManager.WORLD_COMPLETED_PREFIX + ( 2 ).ToString ());
		PlayerPrefs.DeleteKey ( SaveDataManager.WORLD_I_WAS_ON_PREFIX );


			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "3" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "TR" + "3" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( -783 ).ToString ());

			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "26" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "26" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 26 ).ToString ());
			
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "25" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_FINISHED_PREFIX + "25" );
			PlayerPrefs.DeleteKey ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + ( 25 ).ToString ());
*/
			GameGlobalVariables.LAB_ENTERED = SaveDataManager.getValue ( SaveDataManager.LABORATORY_ENTERED );

			/*if ( ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "2" ) && ( GameGlobalVariables.LAB_ENTERED == 0 || loadLevel01 ))
			{
				LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
				LoadingScreenControl.getInstance ().turnOnLoadingScreen ();

				if ( SaveDataManager.keyExists ( SaveDataManager.INTRO_PLAYED_PREFIX ) || loadLevel01 ) 
				{
					SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
					StartCoroutine("waitForClick");
				}
				else Application.LoadLevel ( "00Intro" );
			}
			else
			{*/
				LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
				LoadingScreenControl.getInstance ().turnOnLoadingScreen ();

				FLMissionRoomManager.AFTER_INTRO = false;

				Application.LoadLevel ( "FL00ChooseLevel" );
			//}
			return;
		}
		else if (( WhatToLoadLevel == "MN01" ) || ( WhatToLoadLevel == "MN00Instructions" ))
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.MINING );
		}
		else if (( WhatToLoadLevel == "FL01" ) || ( WhatToLoadLevel == "FL00ChooseLevel" ))
		{
#if UNITY_ANDROID
			/*
			if ( showAdds )
			{
				if ( LevelControl.LEVEL_ID > 4 )
				{
					Debug.Log(
					ChartboostCall.getInstance ().showAdd ();
				}
			}*/
#endif

			FLMissionRoomManager.AFTER_INTRO = false;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
		}

		LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
		
		Application.LoadLevel ( WhatToLoadLevel );
	}
}
