using UnityEngine;
using System.Collections;

public class TriggerDialogueControl : MonoBehaviour 
{
	//*************************************************************//	
	private GameObject myTutorialBbox;
	private bool startLooking = false;
	public bool waitOnSequenceEnd = false;
	private bool questionClicked = true;
	private static TriggerDialogueControl _meInstance;
	public static TriggerDialogueControl getInstance ()
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
		if ( GlobalVariables.checkForMenus ()) return;
		if ( waitOnSequenceEnd == true) return;
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		collider.enabled = false;
		renderer.enabled = false;
		Camera.main.gameObject.AddComponent < StartSequenceStartCameraZoom > ();
		Camera.main.gameObject.GetComponent < StartSequenceStartCameraZoom > ().fromQuestionClick = true;
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			TutorialsManager.getInstance ().StartSmallTutorial ();
			print ("fhdg");
		}
		else
		{
			TutorialsManager.getInstance ().StartTutorial ();
			print ("fhdaddg");
		}
	}

	public IEnumerator releaseQuestionMarkButton ()
	{
		print ("MAKE IT SO!");
		yield return new WaitForSeconds (1.6f);
		waitOnSequenceEnd = false;
	}
	
	public void turnOn ()
	{
		StartCoroutine ("activate");
	}

	public IEnumerator activate ()
	{
		yield return new WaitForSeconds (1.6f);
		collider.enabled = true;
		renderer.enabled = true;
	}
}
