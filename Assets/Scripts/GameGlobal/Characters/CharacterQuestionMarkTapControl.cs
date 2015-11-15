using UnityEngine;
using System.Collections;

public class CharacterQuestionMarkTapControl : MonoBehaviour 
{
	//*************************************************************//
	public CharacterTapTutorialControl myCharacterTapTutorialControl;
	//*************************************************************//
	void Start ()
	{
		 gameObject.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1.1f );
	}
	
	public void OnMouseUp ()
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GlobalVariables.checkForMenus ()) return;
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		myCharacterTapTutorialControl.OnMouseUp ();
	}
}
