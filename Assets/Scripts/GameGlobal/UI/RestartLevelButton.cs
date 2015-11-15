using UnityEngine;
using System.Collections;

public class RestartLevelButton : MonoBehaviour 
{
	void OnMouseUp ()
	{
		//gameObject.collider.enabled = false;
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( UIControl.currentPopupUI != null )
			{
				return;
			}
		}
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{

		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			print ("Have " + GameGlobalVariables.Stats.MOVES);
			print("Cap " + LevelControl.CURRENT_LEVEL_CLASS.moves);
			if(GameGlobalVariables.Stats.LIVES_AVAILABLE > 0 && GameGlobalVariables.Stats.MOVES < LevelControl.CURRENT_LEVEL_CLASS.moves)
			{
				UIControl.getInstance().createPopup(UIControl.getInstance().restartScreen, true, "NULL", Camera.main.transform.Find("UI").transform.Find("UIPivot").gameObject.transform);
			}
			else if(Camera.main.transform.Find("UI").GetComponent<UIControl>() != null && GameGlobalVariables.Stats.LIVES_AVAILABLE <= 1 && GameGlobalVariables.Stats.MOVES < LevelControl.CURRENT_LEVEL_CLASS.moves)
			{
				print ("WTF");
				UIControl.getInstance().createPopup(UIControl.getInstance()._getLivesUI, true, "NULL", Camera.main.transform.Find("UI").transform.Find("UIPivot").gameObject.transform);
				UIControl.getInstance().currencyHeaderBar.SetActive(true);
			}
			else
			{
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
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.MINING );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			Application.LoadLevel ( "MN01" );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.TRAIN );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			Application.LoadLevel ( "TR01" );
		}
	}
}
