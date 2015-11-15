using UnityEngine;
using System.Collections;

public class LabTapedForTutorial02Control : MonoBehaviour 
{

	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		GameGlobalVariables.BLOCK_LAB_ENTERED = false;
		GameGlobalVariables.LAB_ENTERED = 2;
		SaveDataManager.save ( SaveDataManager.LABORATORY_ENTERED, GameGlobalVariables.LAB_ENTERED );

		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( false );

		Vector3 positionToGo = new Vector3 ( -6.5f + 12f, Camera.main.transform.position.y, 4f + 9.25f );
		Camera.main.transform.position = positionToGo;

		TutorialsManager.getInstance ().StartTutorial ();

		GetComponent < JumpingArrowAbove > ().destroyJumpingArrow ();
		GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );

		Destroy ( this );
	}
}
