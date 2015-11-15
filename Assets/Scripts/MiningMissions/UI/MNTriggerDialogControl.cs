using UnityEngine;
using System.Collections;

public class MNTriggerDialogControl : MonoBehaviour 
{
	//*************************************************************//	
	private static MNTriggerDialogControl _meInstance;
	public static MNTriggerDialogControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//

	void Awake ()
	{
		_meInstance = this;

		GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
	}

	void OnMouseUp ()
	{
		if ( MNGlobalVariables.checkForMenus ()) return;
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		collider.enabled = false;
		renderer.enabled = false;

		switch ( MNLevelControl.LEVEL_ID )
		{
		case 1:
			TutorialsManager.getInstance ().triggerGeneralDialogForMiningTutorial ();
			break;
		case 2:
			TutorialsManager.getInstance ().triggerGeneralDialogForMining02Tutorial ();
			break;
		}
	}

	public void turnOn ()
	{
		collider.enabled = true;
		renderer.enabled = true;
	}
}
