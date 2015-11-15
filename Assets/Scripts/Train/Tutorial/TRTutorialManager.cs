using UnityEngine;
using System.Collections;

public class TRTutorialManager : MonoBehaviour 
{
	//*************************************************************//	
	private GameObject _tutorialComboUIPrefab;
	private int _currentTutorialID = 0;
	//*************************************************************//	
	private static TRTutorialManager _meInstance;
	public static TRTutorialManager getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		_meInstance = this;

		_tutorialComboUIPrefab = ( GameObject ) Resources.Load ( "UI/TRTutorialComboUIDemi" );
	}

	void Start ()
	{
		StartTutorial ();
	}

	public void StartTutorial ()
	{
		switch ( TRLevelControl.LEVEL_ID )
		{
		case 1:
			if ( SaveDataManager.getValue ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + "1" ) != 1 )
			{
				GameObject tutorialUIComboObject = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
				tutorialUIComboObject.transform.parent = Camera.main.transform;
				tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;

				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_help_the_toys";

				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TRTapFrameControl > ();
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;

				Time.timeScale = 0.15f;

				_currentTutorialID = 1;

				SaveDataManager.save ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + "1", 1 );
			}
			else
			{
				TRTriggerDialogueControl.getInstance ().turnOn ();
			}
			break;
		case 2:
			if ( SaveDataManager.getValue ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + "2" ) != 1 )
			{
				GameObject tutorialUIComboObject = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
				tutorialUIComboObject.transform.parent = Camera.main.transform;
				tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
				
				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_help_the_toys";
				
				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TRTapFrameControl > ();
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;
				
				Time.timeScale = 0.15f;
				
				_currentTutorialID = 2;
				
				SaveDataManager.save ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + "2", 1 );
			}
			else
			{
				TRTriggerDialogueControl.getInstance ().turnOn ();
			}
			break;
		case 3:
			if ( SaveDataManager.getValue ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + "3" ) != 1 )
			{
				GameObject tutorialUIComboObject = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
				tutorialUIComboObject.transform.parent = Camera.main.transform;
				tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
				
				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_help_the_toys";
				
				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TRTapFrameControl > ();
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;
				
				Time.timeScale = 0.15f;
				
				_currentTutorialID = 3;
				
				SaveDataManager.save ( SaveDataManager.TRAIN_TUTORIAL_PLAYED + "3", 1 );
			}
			else
			{
				TRTriggerDialogueControl.getInstance ().turnOn ();
			}
			break;
		}

	}

	public int getCurrentTutorialID ()
	{
		return _currentTutorialID;
	}
}
