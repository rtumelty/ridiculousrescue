using UnityEngine;
using System.Collections;

public class TRTriggerDialogueControl : MonoBehaviour 
{
	//*************************************************************//	
	private static TRTriggerDialogueControl _meInstance;
	public static TRTriggerDialogueControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	
	void Awake ()
	{
		_meInstance = this;

		GetComponent < SelectedComponenent > ().updateInitialValues ();
		GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
	}
	
	void OnMouseUp ()
	{
		if ( TRGlobalVariables.checkForMenus ()) return;
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		collider.enabled = false;
		renderer.enabled = false;

		PlayerPrefs.DeleteKey ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + TRLevelControl.LEVEL_ID.ToString ());

		MemoryManager.getInstance ().clean ();
		
		Application.LoadLevel ( "00MemoryCleaner" );
	}
	
	public void turnOn ()
	{
		collider.enabled = true;
		renderer.enabled = true;
	}
}
