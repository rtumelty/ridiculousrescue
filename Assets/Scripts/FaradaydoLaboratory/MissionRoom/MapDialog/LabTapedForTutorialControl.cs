using UnityEngine;
using System.Collections;

public class LabTapedForTutorialControl : MonoBehaviour 
{

	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		GameGlobalVariables.BLOCK_LAB_ENTERED = false;
		GameGlobalVariables.LAB_ENTERED = 1;
		SaveDataManager.save ( SaveDataManager.LABORATORY_ENTERED, GameGlobalVariables.LAB_ENTERED );

		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( false );

		TutorialsManager.getInstance ().StartTutorial ();

		GetComponent < JumpingArrowAbove > ().destroyJumpingArrow ();
		GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );

		Destroy ( this );
	}
}
