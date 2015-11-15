using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterTapTutorialControl : MonoBehaviour 
{
	//*************************************************************//
	private List < string > _myTextKeys;
	private GameObject _questionMarkPrefab;
	private GameObject _questionMarkInstant;
	private GameObject _tutorialUIInstant;
	private IComponent _myIComponent;
	private bool _firstTap = true;
	private int _lastKeyIndex;
	//*************************************************************//
	void Awake () 
	{
		_questionMarkPrefab = ( GameObject ) Resources.Load ( "UI/questionMark" );
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_myTextKeys = new List < string > ();
		
		switch ( _myIComponent.myID )
		{
			case GameElements.CHAR_FARADAYDO_1_IDLE:
				//_myTextKeys.Add ( "laboratory_tutorial_2_text_8_faradaydo_requirementsnotmet" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_1_faradaydo" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_2_faradaydo" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_3_faradaydo" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_4_faradaydo" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_5_faradaydo" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_6_faradaydo" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_7_faradaydo" );
				break;

			case GameElements.CHAR_CORA_1_IDLE:
				//_myTextKeys.Add ( "laboratory_tutorial_2_text_2_cora_requirementsnotmet" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_1_cora" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_3_cora" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_4_cora" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_5_cora" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_6_cora" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_7_cora" );
				_myTextKeys.Add ( "laboratory_tutorial_2_text_8_for_lab_visit_02_cora" );
				break;

			case GameElements.CHAR_MINER_4_IDLE:
				_myTextKeys.Add ( "laboratory_text_garage_miner_1_1" );
				_myTextKeys.Add ( "laboratory_text_garage_miner_1_2" );
				_myTextKeys.Add ( "laboratory_text_garage_miner_1_3" );
				break;
		}
		
		createQuestionMark ();
	}
	
	public void createQuestionMark ()
	{
		if ( _questionMarkInstant != null ) return;
		
		_questionMarkInstant = ( GameObject ) Instantiate ( _questionMarkPrefab, transform.position + Vector3.forward * 2f + Vector3.up * 2f + Vector3.left * 1f, _questionMarkPrefab.transform.rotation );
		_questionMarkInstant.transform.parent = transform.parent;
		CharacterQuestionMarkTapControl currentCharacterQuestionMarkTapControl = _questionMarkInstant.AddComponent < CharacterQuestionMarkTapControl > ();
		currentCharacterQuestionMarkTapControl.myCharacterTapTutorialControl = this;
	}
	
	public void OnMouseUp ()
	{
		if(Camera.main.GetComponent<FLUIControl>() != null)
		{
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
		}
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GlobalVariables.checkForMenus ()) return;
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		Destroy ( FLUIControl.currentCharacterTutorialFrame );
		
		if ( _questionMarkInstant ) _questionMarkInstant.AddComponent < HideUIElement > ();
		
		int randomBoxID = UnityEngine.Random.Range ( 0, 2 );
		if ( randomBoxID == 0 ) FLUIControl.currentCharacterTutorialFrame = _tutorialUIInstant = ( GameObject ) Instantiate ( TutorialsManager.getInstance ().tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01], Vector3.zero, TutorialsManager.getInstance ().tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01].transform.rotation );
		else FLUIControl.currentCharacterTutorialFrame = _tutorialUIInstant = ( GameObject ) Instantiate ( TutorialsManager.getInstance ().tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02], Vector3.zero, TutorialsManager.getInstance ().tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02].transform.rotation );
		
		_tutorialUIInstant.transform.parent = Camera.main.transform;
		_tutorialUIInstant.transform.localPosition = new Vector3 ( 0f, 1.5f, 9f );
		
		//iTween.MoveFrom ( _tutorialUIInstant, iTween.Hash ( "time", 0.6f, "easetype", iTween.EaseType.easeOutExpo, "position", _tutorialUIInstant.transform.position + Vector3.forward * 10f ));

		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.AFTER_LAB_VISIT_02 ) 
		{
			_tutorialUIInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = _myTextKeys[_myTextKeys.Count - 1];
		}
		else if ( _firstTap && GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && GameGlobalVariables.LAB_ENTERED == 2 ) 
		{
			_firstTap = false;
			_tutorialUIInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = _myTextKeys[0];
		}
		else
		{
			int keyIndex = UnityEngine.Random.Range ( 0, _myTextKeys.Count );
			while (  keyIndex == _lastKeyIndex )
			{
				keyIndex = UnityEngine.Random.Range ( 0, _myTextKeys.Count );
			}
			
			_lastKeyIndex = keyIndex;
			
			_tutorialUIInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = _myTextKeys[keyIndex];
		}
		
		_tutorialUIInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = 28;
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( _tutorialUIInstant.transform.Find ( "speaker" ) != null ) 
			{
				_tutorialUIInstant.transform.Find ( "speaker" ).renderer.material.mainTexture = LevelControl.getInstance ().gameElements[_myIComponent.myID];
				if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
				{
					//_tutorialUIInstant.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
					//_tutorialUIInstant.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
				}
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			if ( _tutorialUIInstant.transform.Find ( "speaker" ) != null ) 
			{
				_tutorialUIInstant.transform.Find ( "speaker" ).renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[_myIComponent.myID];
				if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
				{
					//_tutorialUIInstant.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
					//_tutorialUIInstant.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
				}
			}
		}
		//====================================Daves Edit===================================
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			if ( _tutorialUIInstant.transform.Find ( "speaker" ) != null ) 
			{
				_tutorialUIInstant.transform.Find ( "speaker" ).renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[_myIComponent.myID];
				if ( _myIComponent.myID == GameElements.CHAR_MINER_4_IDLE )
				{
					_tutorialUIInstant.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
					_tutorialUIInstant.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
				}
			}
		}
		//====================================Daves Edit===================================

		
		//iTween.ScaleFrom ( _tutorialUIInstant, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo, "scale", Vector3.zero, "islocal", true ));
		_tutorialUIInstant.transform.Find ( "frame" ).gameObject.AddComponent < CharacterTutorialTapFrameControl > ();
		_tutorialUIInstant.transform.Find ( "frame" ).collider.enabled = true;
		
		TutorialsManager.getInstance ().setAlphaToZero ( _tutorialUIInstant );
	}
}
