using UnityEngine;
using System.Collections;

public class StartSequenceManager : MonoBehaviour 
{
	//*************************************************************//	
	private int stepID = 0;
	private int statrtedStepID = -1;
	//*************************************************************//	
	private static StartSequenceManager _meInstance;
	public static StartSequenceManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_StartSequence" ).GetComponent < StartSequenceManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	public void startStartSequence () 
	{
		//StartCoroutine ( "waitSomeTimeBeforeRealStart" );
	}
	
	private IEnumerator waitSomeTimeBeforeRealStart ()
	{
		// just for pause 
		GlobalVariables.POPUP_UI_SCREEN = true;
		yield return new WaitForSeconds ( 1.3f );
		GlobalVariables.POPUP_UI_SCREEN = false;
		GlobalVariables.START_SEQUENCE = true;
	}
	
	void Update () 
	{
		if ( ! GlobalVariables.START_SEQUENCE ) return;
		if ( statrtedStepID == stepID ) return;
		switch ( stepID )
		{
			case 0:
				Camera.main.gameObject.AddComponent < StartSequenceStartCameraZoom > ();
				break;
			case 1:
				TutorialsManager.getInstance ().StartTutorial ();				
				GlobalVariables.START_SEQUENCE = false;
				break;
		}
		
		statrtedStepID = stepID;
	}
	
	public void goToNextStep ()
	{
		stepID++;
	}
}
