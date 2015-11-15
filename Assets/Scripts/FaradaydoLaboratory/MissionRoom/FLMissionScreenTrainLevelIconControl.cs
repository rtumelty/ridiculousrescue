using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class FLMissionScreenTrainLevelIconControl : MonoBehaviour 
{
	//*************************************************************//
	public GameGlobalVariables.Missions.TrainLevelClass myLevelClass;
	public bool mayStart = false;
	private GameObject _myMissionBriefScreen;
	private Renderer[] _charactersIcon;
	private GameObject _slot01;
	private GameObject _slot02;
	private GameObject _slot03;
	private TextMesh[] _remainingMovesTexts;
	private List < int > charactersInSelected;
	//*************************************************************//
	private bool _checkIfMapDragged = false;
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

		if ( FLGlobalVariables.POPUP_UI_SCREEN ) return;
		if ( ! mayStart )
		{
			renderer.enabled = false;
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
		FixMyUiElements ();
		/*FLGlobalVariables.MISSION_SCREEN = false;
		FLGlobalVariables.POPUP_UI_SCREEN = false;
		
		TRLevelControl.SELECTED_LEVEL_NAME = myLevelClass.myName;
		int.TryParse ( TRLevelControl.SELECTED_LEVEL_NAME, out TRLevelControl.LEVEL_ID );
		TRLevelControl.CURRENT_LEVEL_CLASS = myLevelClass;
		
		LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.TRAIN );
		LoadingScreenControl.getInstance ().turnOnLoadingScreen ();

		FLMissionRoomManager.getInstance ().rememberWorldIWasOn ();

		MemoryManager.getInstance ().clean ();

		Application.LoadLevel ( "00MemoryCleaner" );*/
	}

	private void FixMyUiElements()
	{
		if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE)
		{
			_myMissionBriefScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().TRMissionPreScreen );
		}
		else
		{
			_myMissionBriefScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().missionPreScreen );
		}

		if ( _myMissionBriefScreen != null )
		{
			FLUIControl.currentReservingUIElmentObject = this.gameObject;
		}
		_myMissionBriefScreen.transform.Find("scaleSolver").transform.Find("buttonConfirm").GetComponent<FLMissionScreenConfirmButtonControl>().myTrainLevelClass = myLevelClass;
		_myMissionBriefScreen.transform.Find ("scaleSolver").transform.Find ("buttonConfirm").GetComponent<FLMissionScreenConfirmButtonControl> ().playTrain = true;

		GameObject missionText = _myMissionBriefScreen.transform.Find("levelTitle").transform.Find("textMissionStatus").gameObject;
		if ( myLevelClass.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE )
		{
			missionText.GetComponent < GameTextControl > ().myKey = "ui_sign_train_mission";
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
		
		/*_slot01.SetActive (false); 
		_slot02.SetActive (false); 
		_slot03.SetActive (false); */
		
	
		if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE)
		{
			GameObject myPreScreen = GameObject.Find ("MissionPreScreen(Clone)");


			_charactersIcon [0].material.mainTexture = FLLevelControl.getInstance ().gameElements [GameElements.UI_FARADAYDO_ICON];
			GameObject myTextSlot01 = _slot01.transform.Find ("textMovesLeft01a").gameObject;
			myTextSlot01.GetComponent<GameTextControl>().myKey = "ui_keep_full_power";
			_slot01.transform.Find("textMovesLeft01").gameObject.SetActive(false);
			_slot01.transform.Find("textMovesLeft01b").gameObject.SetActive(false);
			//myTextSlot01.transform.position = new Vector3(myTextSlot01.transform.position.x - 0.28f, myTextSlot01.transform.position.y,  myTextSlot01.transform.position.z);


			_charactersIcon [1].material.mainTexture = FLLevelControl.getInstance ().gameElements [GameElements.UI_JOSE_ICON];
			GameObject myTextSlot02 = _slot02.transform.Find ("textMovesLeft02a").gameObject;
			myTextSlot02.GetComponent<GameTextControl>().myKey = "ui_keep_full_power";
			_slot02.transform.Find("textMovesLeft02").gameObject.SetActive(false);
			_slot02.transform.Find("textMovesLeft02b").gameObject.SetActive(false);
			//myTextSlot02.transform.position = new Vector3(myTextSlot02.transform.position.x - 0.28f, myTextSlot02.transform.position.y,  myTextSlot02.transform.position.z);
		  

			print ("+ TIMER");
			//_charactersIcon[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].gameObject.transform.parent.gameObject.SetActive ( true );
			_charactersIcon[2].material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_TIMER];
			//_remainingMovesTexts[2].text = TimeScaleManager.getTimeString ( myLevelClass.star04 );
			GameObject timeObject = _myMissionBriefScreen.transform.Find( "slot03" ).Find ( "textMovesLeft03" ).gameObject;
			//timeObject.transform.position = new Vector3(timeObject.transform.position.x + 0.35f, timeObject.transform.position.y, timeObject.transform.position.z);
			_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03b" ).gameObject.SetActive(false);
			GameObject myTextSlot03 = _slot03.transform.Find ("textMovesLeft03a").gameObject;
			myTextSlot03.GetComponent<GameTextControl>().myKey = "ui_less_than_seconds_02";
			myTextSlot03.GetComponent<GameTextControl>().addPreText = TimeScaleManager.getTimeString ( myLevelClass.star04 ) + " ";
		//	_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position = new Vector3(_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.x + 0.35f, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.y, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.z);
		//	_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position = new Vector3(_myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.x + 0.075f, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.y, _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.z);
			Transform myTimeAward = _myMissionBriefScreen.transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
			print (myTimeAward.position.x);
			myTimeAward.position = new Vector3 (myTimeAward.position.x + 0.075f, myTimeAward.position.y ,myTimeAward.position.z);
			print (myTimeAward.position.x);

		}
	}
}

