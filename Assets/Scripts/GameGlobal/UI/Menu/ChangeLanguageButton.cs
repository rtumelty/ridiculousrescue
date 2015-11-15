using UnityEngine;
using System.Collections;

public class ChangeLanguageButton : MonoBehaviour 
{
	//*************************************************************//
	public bool goRight = true;
	//*************************************************************//
	private TextMesh _langaugePanelText;
	//*************************************************************//
	void Awake ()
	{
		_langaugePanelText = transform.parent.Find ( "languagePanel" ).Find ( "text" ).GetComponent < TextMesh > ();
		_langaugePanelText.text = GameTextManager.getInstance ().getCurrentLanguageString ();
	}
	
	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		if ( goRight )
		{
			GameTextManager.CURRENT_LANAGUAGE++;			
		}
		else
		{
			GameTextManager.CURRENT_LANAGUAGE--;
		}
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		
		_langaugePanelText.text = GameTextManager.getInstance ().getCurrentLanguageString ();
	}
}
