using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class FLMissionScreenLevelIconControl : MonoBehaviour 
{
	//*************************************************************//
	public GameGlobalVariables.Missions.LevelClass myLevelClass;
	public bool mayStart = false;
	//*************************************************************//
	private GameObject _myMissionBriefScreen;
	private bool _checkIfMapDragged = false;
	private int levelValue = -1;
	private List < GameObject > _elements;
	private bool usePreScreen = true;
	private Renderer[] _charactersIcon;
	private GameObject _slot01;
	private GameObject _slot02;
	private GameObject _slot03;
	private TextMesh[] _remainingMovesTexts;
	private int slotTracker = 0;
	private bool isTimerSlot = false;
	private List < int > charactersInSelected;

	private GameTextControl[] _scoreBoardTexts;
	private Renderer _star01;
	private Renderer _star02;
	private Renderer _star03;
	//*************************************************************//
	
	void OnMouseUp ()
	{
		if ( _checkIfMapDragged )
		{
			if ( FLGlobalVariables.MAP_DRAGGED )
			{
				_checkIfMapDragged = false;
				return;
			}
		}


		for(int i = 0; i < myLevelClass.requiredElements.Count; i ++)
		{
			switch ( myLevelClass.requiredElements[i].elementID )
			{
			case GameElements.ICON_RECHARGEOCORES:
				if ( GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS - myLevelClass.requiredElements[i].amountRequiered < 0 && ! GameGlobalVariables.BLOCK_LAB_ENTERED )
				{
					if(int.TryParse(myLevelClass.myName, out levelValue))
					{
						if(levelValue != 3 && levelValue != 4 && myLevelClass.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE )
						{
							usePreScreen = false;
						}
						else if (myLevelClass.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE)
						{
							usePreScreen = false;
						}
					}
				}	
				break;
			case GameElements.ICON_REDIRECTORS:
				if ( GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS - myLevelClass.requiredElements[i].amountRequiered < 0 )
				{
					if(int.TryParse(myLevelClass.myName, out levelValue))
					{
						if(levelValue != 3 && levelValue != 4 && myLevelClass.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE )
						{
							usePreScreen = false;
						}
						else if (myLevelClass.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE)
						{
							usePreScreen = false;
						}
					}
				}	
				break;
			}
		}
		//usePreScreen = false;
		if ( FLGlobalVariables.POPUP_UI_SCREEN ) return;
		if (( ! mayStart ) || ( FLGlobalVariables.TUTORIAL_MENU && myLevelClass.myName != "5" ))
		{
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
			return;
		}
		
		if ( FLUIControl.currentReservingUIElmentObject == this.gameObject )
		{
			return;
		}
		
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		handleTouched ();
	}

	void OnMouseDown ()
	{
		_checkIfMapDragged = true;
	}
	
	private void handleTouched ()
	{
		LevelControl.SELECTED_LEVEL_NAME = myLevelClass.myName;
		int.TryParse ( LevelControl.SELECTED_LEVEL_NAME, out LevelControl.LEVEL_ID );
		
		if ( LevelControl.LEVEL_ID < 10 ) LevelControl.SELECTED_LEVEL_NAME = "0" + LevelControl.LEVEL_ID.ToString ();
		//print ("DIS" + usePreScreen);
		//PersistThroughSceneComponent.lastActiveNode = gameObject.name;
		if(int.TryParse(myLevelClass.myName, out levelValue))
		{
			//print ("levelValue" +levelValue);
			if(levelValue > 0 && levelValue < 3 && myLevelClass.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE )
			{
				MemoryManager.getInstance ().clean ();
				
				FLGlobalVariables.MISSION_SCREEN = false;
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				
				/*LevelControl.SELECTED_LEVEL_NAME = myLevelClass.myName;
				int.TryParse ( LevelControl.SELECTED_LEVEL_NAME, out LevelControl.LEVEL_ID );
				
				if ( LevelControl.LEVEL_ID < 10 ) LevelControl.SELECTED_LEVEL_NAME = "0" + LevelControl.LEVEL_ID.ToString ();*/
				
				LevelControl.CURRENT_LEVEL_CLASS = myLevelClass;
				LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
				LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
				
				FLMissionRoomManager.getInstance ().rememberWorldIWasOn ();
				
				/*if ( ! FLMissionRoomManager.AFTER_INTRO && LevelControl.SELECTED_LEVEL_NAME == "01" && myLevelClass.type != FLMissionScreenNodeManager.TYPE_BONUS_NODE )
				{
					Application.LoadLevel ( "00Intro" );
					return;
				}*/
				
				Application.LoadLevel ( "01" );
			}
			else
			{
//				print ("DIS" + usePreScreen);
				if(usePreScreen == true)
				{
					FixMyUiElements();
				}
				else
				{
					_myMissionBriefScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().missionBriefPrefab );
					if ( _myMissionBriefScreen != null )
					{
						_myMissionBriefScreen.GetComponent < FLMissionBriefScreenControl > ().myLevelClass = myLevelClass;
						FLUIControl.currentReservingUIElmentObject = this.gameObject;
					}
				}
			}
		}
		else
		{
			print ("OR DIS" + usePreScreen);
			if(usePreScreen == true)
			{
				FixMyUiElements();
			}
			else
			{
				_myMissionBriefScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().missionBriefPrefab );
				if ( _myMissionBriefScreen != null )
				{
					_myMissionBriefScreen.GetComponent < FLMissionBriefScreenControl > ().myLevelClass = myLevelClass;
					FLUIControl.currentReservingUIElmentObject = this.gameObject;
				}
			}
		}
		usePreScreen = true;
	}

	private void FixMyUiElements()
	{
		_myMissionBriefScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().missionPreScreen );
		if ( _myMissionBriefScreen != null )
		{
			FLUIControl.currentReservingUIElmentObject = this.gameObject;
		}
		_myMissionBriefScreen.transform.Find("scaleSolver").transform.Find("buttonConfirm").GetComponent<FLMissionScreenConfirmButtonControl>().myLevelClass = myLevelClass;
		GameObject missionText = _myMissionBriefScreen.transform.Find("levelTitle").transform.Find("textMissionStatus").gameObject;
		if ( myLevelClass.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE )
		{
			missionText.GetComponent < GameTextControl > ().myKey = "ui_sign_bonus_mission";
		}
		else
		{
			missionText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission";
		}
		missionText.GetComponent < GameTextControl > ().addText = " " + myLevelClass.myName;

		//=========================Arrange Reward Slots========================================
		_slot01 = _myMissionBriefScreen.transform.Find ( "slot01" ).gameObject;
		_slot02 = _myMissionBriefScreen.transform.Find ( "slot02" ).gameObject;
		_slot03 = _myMissionBriefScreen.transform.Find ( "slot03" ).gameObject;

		_charactersIcon = new Renderer[3];
		_charactersIcon[0] = _slot01.transform.Find ( "character01Icon" ).gameObject.renderer;
		_charactersIcon[1] = _slot02.transform.Find ( "character02Icon" ).gameObject.renderer;
		_charactersIcon[2] = _slot03.transform.Find ( "character03Icon" ).gameObject.renderer;

		_remainingMovesTexts = new TextMesh[3];
		_remainingMovesTexts[0] = _slot01.transform.Find ( "textMovesLeft01" ).GetComponent < TextMesh > ();
		_remainingMovesTexts[1] = _slot02.transform.Find ( "textMovesLeft02" ).GetComponent < TextMesh > ();
		_remainingMovesTexts[2] = _slot03.transform.Find ( "textMovesLeft03" ).GetComponent < TextMesh > ();

		//_slot01.SetActive (false); 
		//_slot02.SetActive (false); 
		//_slot03.SetActive (false); 

		if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE)
		{
			charactersInSelected = getCharactersFromThisLevelBonus (myLevelClass.myName);
			if ( LevelControl.LEVEL_ID > 0 && LevelControl.LEVEL_ID < 7 ) 
			{
				LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[0].bonusLevels[int.Parse(myLevelClass.myName) - 1];
			}
			else if ( LevelControl.LEVEL_ID >= 7 && LevelControl.LEVEL_ID < 13 )
			{
				LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[1].bonusLevels[int.Parse(myLevelClass.myName) -1];
			}
		}
		else if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE)
		{
			charactersInSelected = getCharactersFromThisLevel (myLevelClass.myName);
			if ( LevelControl.LEVEL_ID > 0 && LevelControl.LEVEL_ID < 12 ) 
			{
				LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[0].levels[int.Parse(myLevelClass.myName) - 1];
			}
			else if ( LevelControl.LEVEL_ID >= 12 )
			{
				LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[1].levels[int.Parse(myLevelClass.myName) -1];
			}
		}

		//else if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE)
		//{
		//	charactersInSelected = getCharactersFromThisLevelTrain (myLevelClass.myName);
		//}
		sortCharactersOnLevelToMakeFaradayadoFirst ();


		GameObject myPreScreen = GameObject.Find ("MissionPreScreen(Clone)");

		//================RR PRE MISSION SCREEN======================
		if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE || myLevelClass.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE)
		{
		_scoreBoardTexts = new GameTextControl[3];
		_scoreBoardTexts[0] = _slot01.transform.Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ();
		_scoreBoardTexts[1] = _slot02.transform.Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ();
		_scoreBoardTexts[2] = _slot03.transform.Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ();

		Transform info1 = _slot01.transform.Find ( "textMovesLeft01a" ).transform;
		Transform info2 = _slot02.transform.Find ( "textMovesLeft02a" ).transform;
		Transform info3 = _slot03.transform.Find ( "textMovesLeft03a" ).transform;

		/*_scoreBoardTexts[0].transform.position = new Vector3(_scoreBoardTexts[0].transform.position.x - 1.1f, _scoreBoardTexts[0].transform.position.y, _scoreBoardTexts[0].transform.position.z);
		info1.transform.position = new Vector3(info1.transform.position.x - 0.9f, info1.transform.position.y, info1.transform.position.z);

		_scoreBoardTexts[1].transform.position = new Vector3(_scoreBoardTexts[1].transform.position.x - 1.1f, _scoreBoardTexts[1].transform.position.y, _scoreBoardTexts[1].transform.position.z);
		info2.transform.position = new Vector3(info2.transform.position.x - 0.9f, info2.transform.position.y, info2.transform.position.z);

		_scoreBoardTexts[2].transform.position = new Vector3(_scoreBoardTexts[2].transform.position.x - 1.1f, _scoreBoardTexts[2].transform.position.y, _scoreBoardTexts[2].transform.position.z);
		info3.transform.position = new Vector3(info3.transform.position.x - 0.9f, info3.transform.position.y, info3.transform.position.z);*/


		_scoreBoardTexts[0].addPreText = LevelControl.CURRENT_LEVEL_CLASS.movesLimitForStar.ToString() + " ";
		_scoreBoardTexts[1].addPreText = LevelControl.CURRENT_LEVEL_CLASS.timeLimitForStar.ToString() + " ";
		_scoreBoardTexts [2].addPreText = ScoreFormatManager.returnFormatedScoreString( (int)LevelControl.CURRENT_LEVEL_CLASS.scoreLimitForStar) + " ";
		
		_charactersIcon[0].enabled = false;
		_charactersIcon[1].enabled = false;
		_charactersIcon[2].enabled = false;
		}
		//================RR PRE MISSION SCREEN======================
		/*
		for (int i = 0; i < charactersInSelected.Count; i++) 
		{

			switch (charactersInSelected[i]) 
			{
				
				
				
			case GameElements.CHAR_FARADAYDO_1_IDLE:

				if (LevelControl.CURRENT_LEVEL_CLASS.star01 != 0) 
				{
					_charactersIcon [i].gameObject.transform.parent.gameObject.SetActive (true);
					slotTracker ++;
					_remainingMovesTexts [i].text = LevelControl.CURRENT_LEVEL_CLASS.star01.ToString ();
					//======================Align Text Spaces=======================
					if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 10 && i == 2)
					{
						Transform myPos3 = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
						myPos3.position = new Vector3 (myPos3.position.x - 0.075f, myPos3.position.y, myPos3.position.z);
						Transform myPos3a = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03b" );
						myPos3a.position = new Vector3 (myPos3a.position.x - 0.15f, myPos3a.position.y, myPos3a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 10 && i == 1)
					{
						Transform myPos2 = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02" );
						myPos2.position = new Vector3 (myPos2.position.x - 0.075f, myPos2.position.y, myPos2.position.z);
						Transform myPos2a = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02b" );
						myPos2a.position = new Vector3 (myPos2a.position.x - 0.15f, myPos2a.position.y, myPos2a.position.z);
					}
					else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 10 && i == 0)
					{
						Transform myPos1 = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01" );
						myPos1.position = new Vector3 (myPos1.position.x - 0.075f, myPos1.position.y, myPos1.position.z);
						Transform myPos1a = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01b" );
						myPos1a.position = new Vector3 (myPos1a.position.x - 0.15f, myPos1a.position.y, myPos1a.position.z);
					}
					//======================Align Text Spaces=======================

					_charactersIcon [i].material.mainTexture = FLLevelControl.getInstance ().gameElements [GameElements.UI_FARADAYDO_ICON];
					_charactersIcon [i].gameObject.SetActive (true);
					//=========================Grammar Correction==========================
					if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 2 && i == 2)
					{
						_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03b" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos3a = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" );
						myPos3a.position = new Vector3 (myPos3a.position.x - 0.1f, myPos3a.position.y, myPos3a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 2 && i == 1)
					{
						_myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos2a = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02a" );
						myPos2a.position = new Vector3 (myPos2a.position.x - 0.1f, myPos2a.position.y, myPos2a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 2 && i == 0)
					{
						_myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos1a = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01a" );
						myPos1a.position = new Vector3 (myPos1a.position.x - 0.1f, myPos1a.position.y, myPos1a.position.z);
					}
					//=========================Grammar Correction==========================
				}
				break;
				
				
				
			case GameElements.CHAR_CORA_1_IDLE:

				if (LevelControl.CURRENT_LEVEL_CLASS.star02 != 0) 
				{
					_charactersIcon [i].gameObject.transform.parent.gameObject.SetActive (true);
					slotTracker ++;
					_remainingMovesTexts [i].text = LevelControl.CURRENT_LEVEL_CLASS.star02.ToString ();
					
					//======================Align Text Spaces=======================
					if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 10 && i == 2)
					{
						Transform myPos3 = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
						myPos3.position = new Vector3 (myPos3.position.x - 0.075f, myPos3.position.y, myPos3.position.z);
						Transform myPos3a = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03b" );
						myPos3a.position = new Vector3 (myPos3a.position.x - 0.15f, myPos3a.position.y, myPos3a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 10 && i == 1)
					{
						Transform myPos2 = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02" );
						myPos2.position = new Vector3 (myPos2.position.x - 0.075f, myPos2.position.y, myPos2.position.z);
						Transform myPos2a = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02b" );
						myPos2a.position = new Vector3 (myPos2a.position.x - 0.15f, myPos2a.position.y, myPos2a.position.z);
					}
					else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 10 && i == 0)
					{
						Transform myPos1 = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01" );
						myPos1.position = new Vector3 (myPos1.position.x - 0.075f, myPos1.position.y, myPos1.position.z);
						Transform myPos1a = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01b" );
						myPos1a.position = new Vector3 (myPos1a.position.x - 0.15f, myPos1a.position.y, myPos1a.position.z);
					}
					//======================Align Text Spaces=======================
					_charactersIcon [i].material.mainTexture = FLLevelControl.getInstance ().gameElements [GameElements.UI_CORA_ICON];
					_charactersIcon [i].gameObject.SetActive (true);

					//=========================Grammar Correction==========================
					if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 2 && i == 2)
					{
						_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos3a = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" );
						myPos3a.position = new Vector3 (myPos3a.position.x - 0.1f, myPos3a.position.y, myPos3a.position.z);
					}

					else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 2 && i == 1)
					{
						_myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos2a = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02a" );
						myPos2a.position = new Vector3 (myPos2a.position.x - 0.1f, myPos2a.position.y, myPos2a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 2 && i == 0)
					{
						_myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos1a = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01a" );
						myPos1a.position = new Vector3 (myPos1a.position.x - 0.1f, myPos1a.position.y, myPos1a.position.z);
					}
					//=========================Grammar Correction==========================
				}
				break;
			case GameElements.CHAR_MADRA_1_IDLE:
				if (LevelControl.CURRENT_LEVEL_CLASS.star03 != 0) 
				{
					_charactersIcon [i].gameObject.transform.parent.gameObject.SetActive (true);
					slotTracker ++;
					_remainingMovesTexts [i].text = LevelControl.CURRENT_LEVEL_CLASS.star03.ToString ();
					//======================Align Text Spaces=======================
					if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 10 && i == 2)
					{
						Transform myPos3 = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
						myPos3.position = new Vector3 (myPos3.position.x - 0.075f, myPos3.position.y, myPos3.position.z);
						Transform myPos3a = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03b" );
						myPos3a.position = new Vector3 (myPos3a.position.x - 0.15f, myPos3a.position.y, myPos3a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 10 && i == 1)
					{
						Transform myPos2 = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02" );
						myPos2.position = new Vector3 (myPos2.position.x - 0.075f, myPos2.position.y, myPos2.position.z);
						Transform myPos2a = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02b" );
						myPos2a.position = new Vector3 (myPos2a.position.x - 0.15f, myPos2a.position.y, myPos2a.position.z);
					}
					else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 10 && i == 0)
					{
						Transform myPos1 = myPreScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01" );
						myPos1.position = new Vector3 (myPos1.position.x - 0.075f, myPos1.position.y, myPos1.position.z);
						Transform myPos1a = myPreScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01b" );
						myPos1a.position = new Vector3 (myPos1a.position.x - 0.15f, myPos1a.position.y, myPos1a.position.z);
					}
					//======================Align Text Spaces=======================
					
					_charactersIcon [i].material.mainTexture = FLLevelControl.getInstance ().gameElements [GameElements.UI_MADRA_ICON];
					_charactersIcon [i].gameObject.SetActive (true);
					//=========================Grammar Correction==========================
					if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 2 && i == 2)
					{
						_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos3a = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" );
						myPos3a.position = new Vector3 (myPos3a.position.x - 0.1f, myPos3a.position.y, myPos3a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 2 && i == 1)
					{
						_myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos2a = _myMissionBriefScreen.transform.Find ( "slot02" ).Find ( "textMovesLeft02a" );
						myPos2a.position = new Vector3 (myPos2a.position.x - 0.1f, myPos2a.position.y, myPos2a.position.z);
					}
					
					else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 2 && i == 0)
					{
						_myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
						Transform myPos1a = _myMissionBriefScreen.transform.Find ( "slot01" ).Find ( "textMovesLeft01a" );
						myPos1a.position = new Vector3 (myPos1a.position.x - 0.1f, myPos1a.position.y, myPos1a.position.z);
					}
					//=========================Grammar Correction==========================
				}
				break;
			}

		}
		
		
		
		if ( LevelControl.CURRENT_LEVEL_CLASS.star04 != 0 )
		{  
			slotTracker ++;
			isTimerSlot = true;
			_charactersIcon[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].gameObject.transform.parent.gameObject.SetActive ( true );
			_charactersIcon[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_TIMER];
			_remainingMovesTexts[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].text = TimeScaleManager.getTimeString ( LevelControl.CURRENT_LEVEL_CLASS.star04 );
			_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03b" ).gameObject.SetActive(false);
			_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent<GameTextControl>().myKey = "ui_less_than_seconds_02";
			_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position = new Vector3(_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.x -0.215f, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.y, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.z);
			_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position = new Vector3(_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.x +0.075f, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.y, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.z);
			Transform myTimeAward = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" );

			myTimeAward.position = new Vector3 (myTimeAward.position.x + 0.075f, myTimeAward.position.y ,myTimeAward.position.z);

			//_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textInfo03" ).GetComponent < GameTextControl > ().myKey = "android_secs";
		}

		if(slotTracker == 1 && isTimerSlot == false)
		{
			_slot01.transform.position = new Vector3(_slot01.transform.position.x, _slot01.transform.position.y, _slot01.transform.position.z + -1.5f);
		}
		else if(slotTracker == 2 && isTimerSlot == false)
		{
			_slot01.transform.position = new Vector3(_slot01.transform.position.x, _slot01.transform.position.y, _slot01.transform.position.z + -0.55f);
			_slot02.transform.position = new Vector3(_slot02.transform.position.x, _slot02.transform.position.y, _slot02.transform.position.z + -1.1f);
		}
		slotTracker = 0;
		isTimerSlot = false;
		*/
	}
	public static List < int > getCharactersFromThisLevel ( string levelIDName )
	{
		List < int > charactersID = new List < int > ();
		
		XmlDocument levelXml = new XmlDocument ();
		int levelID = 0;
		int.TryParse ( levelIDName, out levelID );
		levelXml.LoadXml ( SelectLevel.ALL_LEVELS[levelID] );
		XmlNode levelNode = levelXml.ChildNodes[1].ChildNodes[0];
		foreach ( XmlNode tileNode in levelNode.ChildNodes )
		{
			int x = 0;
			int z = 0;
			int ID = 0;
			
			int.TryParse ( tileNode.Attributes[0].InnerText, out x );
			int.TryParse ( tileNode.Attributes[1].InnerText, out z );
			int.TryParse ( tileNode.InnerText, out ID );
			
			if ( Array.IndexOf ( GameElements.CHARACTERS, ID ) != -1 )
			{
				charactersID.Add ( ID );
			}
		}
//		print ("ISTHISHAPPENING");
		return charactersID;
	}

	public static List < int > getCharactersFromThisLevelBonus ( string levelIDName )
	{
		List < int > charactersID = new List < int > ();
		
		XmlDocument levelXml = new XmlDocument ();
		int levelID = 0;
		int.TryParse ( levelIDName, out levelID );
		levelXml.LoadXml ( SelectLevel.ALL_BONUS_LEVELS[levelID] );
		XmlNode levelNode = levelXml.ChildNodes[1].ChildNodes[0];
		foreach ( XmlNode tileNode in levelNode.ChildNodes )
		{
			int x = 0;
			int z = 0;
			int ID = 0;
			
			int.TryParse ( tileNode.Attributes[0].InnerText, out x );
			int.TryParse ( tileNode.Attributes[1].InnerText, out z );
			int.TryParse ( tileNode.InnerText, out ID );
			
			if ( Array.IndexOf ( GameElements.CHARACTERS, ID ) != -1 )
			{
				charactersID.Add ( ID );
			}
		}
		//		print ("ISTHISHAPPENING");
		return charactersID;
	}
	public static List < int > getCharactersFromThisLevelTrain ( string levelIDName )
	{
		List < int > charactersID = new List < int > ();
		
		XmlDocument levelXml = new XmlDocument ();
		int levelID = 0;
		int.TryParse ( levelIDName, out levelID );
		//levelXml.LoadXml ( SelectLevel.[levelID] );
		XmlNode levelNode = levelXml.ChildNodes[1].ChildNodes[0];
		foreach ( XmlNode tileNode in levelNode.ChildNodes )
		{
			int x = 0;
			int z = 0;
			int ID = 0;
			
			int.TryParse ( tileNode.Attributes[0].InnerText, out x );
			int.TryParse ( tileNode.Attributes[1].InnerText, out z );
			int.TryParse ( tileNode.InnerText, out ID );
			
			if ( Array.IndexOf ( GameElements.CHARACTERS, ID ) != -1 )
			{
				charactersID.Add ( ID );
			}
		}
		//		print ("ISTHISHAPPENING");
		return charactersID;
	}

	private void sortCharactersOnLevelToMakeFaradayadoFirst ()
	{
		int i = 0;
		foreach ( int characterID in charactersInSelected )
		{
			if (characterID == GameElements.CHAR_FARADAYDO_1_IDLE )
			{
				if ( i != 0 )
				{
					charactersInSelected.RemoveAt ( i );
					charactersInSelected.Insert ( 0, characterID );
					break;
				}
			}
			
			i++;
		}
	}
}
