using UnityEngine;
using System.Collections;

public class LabVisit02DialogRepeatButtonControl : MonoBehaviour 
{
	void Awake ()
	{
		//GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true );
	}

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

		Vector3 positionToGo = new Vector3 ( -6.5f + 12f, Camera.main.transform.position.y, 4f + 9.25f );
		Camera.main.transform.position = positionToGo;

		TutorialsManager.getInstance ().StartTutorial ();

		gameObject.SetActive ( false );
	}
}
