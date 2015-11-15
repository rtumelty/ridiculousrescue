using UnityEngine;
using System.Collections;

public class FLMissionScreenMiningLevelIconControl : MonoBehaviour 
{
	//*************************************************************//
	//public static string MINING_LEVEL_PLAYED = "NULL";
	//*************************************************************//
	public GameGlobalVariables.Missions.MiningLevelClass myLevelClass;
	public bool mayStart = false;
	public bool coolingDown = false;
	public bool unlocked = false;
	private GameObject _myMissionBriefScreen;
	//*************************************************************//
	private float _countCoolDown = 0f;
	private GameObject _myWarningSign;
	private float _totalCooldownTime = 5f * 60f;
	private GameObject _myDangerScreen;
	private bool _checkIfMapDragged = false;
	//*************************************************************//
	void Start ()
	{
		_myWarningSign = transform.Find ( "warningSign" ).gameObject;

		_myWarningSign.SetActive ( false );
		coolingDown = false;
		mayStart = true;

		if ( ! SaveDataManager.keyExists ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName ))
		{
			//SaveDataManager.save ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName, 0 );
		}
		else
		{
			//int coolDownTime = SaveDataManager.getValue ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName );
			//if ( coolDownTime > 0 )
			//{
			//	SaveDataManager.save ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName, (int) ( System.DateTime.UtcNow.Ticks / 10000000 ));
			//	coolingDown = true;
			//	mayStart = false;
			//}
		}
	}
	
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

		if ( ! unlocked )
		{
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
			renderer.enabled = false;
			return;
		}

		if ( FLGlobalVariables.POPUP_UI_SCREEN ) return;
		if ( ! mayStart )
		{
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
			
			_myDangerScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().dangerScreenPrefab );
		
			if ( _myDangerScreen != null )
			{
				_myDangerScreen.GetComponent < FLDangerScreenControl > ().timeString = TimeScaleManager.getTimeString ((int) _countCoolDown );
				FLUIControl.currentReservingUIElmentObject = this.gameObject;
			}
			
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

		MNLevelControl.SELECTED_LEVEL_NAME = myLevelClass.myName;

		int.TryParse ( MNLevelControl.SELECTED_LEVEL_NAME, out MNLevelControl.LEVEL_ID );
		MNLevelControl.CURRENT_LEVEL_CLASS = myLevelClass;

		LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.MINING );
		LoadingScreenControl.getInstance ().turnOnLoadingScreen ();

		FLMissionRoomManager.getInstance ().rememberWorldIWasOn ();

		MINING_LEVEL_PLAYED = myLevelClass.myName;

		GameGlobalVariables.MINE_ENTERED = 1;
		Application.LoadLevel ( "MN01" );*/
	}

	public void turnOnCoolDown ()
	{
		/*if ( MINING_LEVEL_PLAYED == myLevelClass.myName )
		{
			//MINING_LEVEL_PLAYED = "NULL";
			//SaveDataManager.save ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName, (int) ( System.DateTime.UtcNow.Ticks / 10000000 ));
			//coolingDown = true;
			//mayStart = false;
		}*/
	}
	
	void Update ()
	{
		if ( ! unlocked )
		{
			_myWarningSign.SetActive ( false );
			return;
		}

		/*
		if ( coolingDown )
		{
			int startTime = SaveDataManager.getValue ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName );
			int deltaTime = (int) ( System.DateTime.UtcNow.Ticks / 10000000 ) - startTime;
			
			_countCoolDown = _totalCooldownTime - (float) deltaTime ;
			
			if ( _countCoolDown <= 0f )
			{
				coolingDown = false;
				SaveDataManager.save ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + myLevelClass.myName, 0 );
			}
			
			if ( _myDangerScreen != null )
			{
				_myDangerScreen.GetComponent < FLDangerScreenControl > ().timeString = TimeScaleManager.getTimeString ((int) _countCoolDown );
			}
		}
		
		if ( _countCoolDown <= 0f )
		{
			renderer.material.mainTexture = FLMissionRoomManager.getInstance ().miningLevelReady;
			mayStart = true;
			_myWarningSign.SetActive ( false );
		}
		else
		{
			renderer.material.mainTexture = FLMissionRoomManager.getInstance ().miningLevelNotReady;
			mayStart = false;
			_myWarningSign.SetActive ( true );
			_myWarningSign.transform.Find ( "textTime" ).GetComponent < TextMesh > ().text = TimeScaleManager.getTimeString ((int) _countCoolDown );
		}
		*/
	}

	private void FixMyUiElements()
	{
		_myMissionBriefScreen = FLUIControl.getInstance ().createPopup ( FLMissionRoomManager.getInstance ().mineMissionPreSreen );
		if ( _myMissionBriefScreen != null )
		{
			FLUIControl.currentReservingUIElmentObject = this.gameObject;
		}
		_myMissionBriefScreen.transform.Find("scaleSolver").transform.Find("buttonConfirm").GetComponent<FLMissionScreenConfirmButtonControl>().myMineClass = myLevelClass;
		_myMissionBriefScreen.transform.Find ("scaleSolver").transform.Find ("buttonConfirm").GetComponent<FLMissionScreenConfirmButtonControl> ().playMine = true;
		GameObject missionText = _myMissionBriefScreen.transform.Find("levelTitle").transform.Find("textMissionStatus").gameObject;
		if ( myLevelClass.type == FLMissionScreenNodeManager.TYPE_MINING_NODE)
		{
			missionText.GetComponent < GameTextControl > ().myKey = "ui_sign_mine_mission_number";
		}

		missionText.GetComponent < GameTextControl > ().addText = " " + myLevelClass.myName;
		/*if(myLevelClass.type == FLMissionScreenNodeManager.TYPE_MINING_NODE)
		{
			GameObject myPreScreen = GameObject.Find ("MissionPreScreen(Clone)");

			print ("+ TIMER");	
		}*/
	}
}
