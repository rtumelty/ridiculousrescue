using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class TRResoultScreen : MonoBehaviour 
{
	//*************************************************************//	
	public static bool WIN_RESOULT = false;
	public static string TO_BE_RESCUED_TOY_NAME = "";
	public static string CHARACTER_NAME = "";
	public static string SPECIFIC_FAILED_INFO_KEY = "";
	//*************************************************************//
	public GameObject bozDrain;
	public GameObject coraDrain;
	public GameObject faraDrain;
	public GameObject trainDrain;
	public GameObject joseDrain;
	public GameObject madraDrain;
	public CharacterData myCharacter;
	public GameObject myCharacterHolder;
	public GameObject panelChars;
	public GameObject powerFail;
	public GameObject speedFail;
	public Texture2D character;
	public Texture2D character1;
	public Texture2D character2;

	public Texture2D succsessSign;
	public Texture2D failedSign;
	public Texture2D succsessLight;
	public Texture2D failedLight;
	
	public Texture2D nonStars;
	public Texture2D oneStar;
	public Texture2D twoStars;
	public Texture2D threeStars;
	//*************************************************************//	
	private TextMesh _starOneText;
	private TextMesh _starTwoText;
	private TextMesh _starThreeText;
	private Material _starsMaterial;
	private TextMesh _missionStatusText;
	private TextMesh _missionFailText;
	private TRSuccessScreenControl myTRSuccessScreenControl;
	public bool itsOver = false;
	public bool alreadyResultOpen = false;
	//*************************************************************//	
	private static TRResoultScreen _meInstance;
	public static TRResoultScreen getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraResoultScreen" ).GetComponent < TRResoultScreen > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		/*
		if ( GameGlobalVariables.LAB_ENTERED == 0 )
		{
			switch ( LevelControl.LEVEL_ID )
			{
				case 1:
				case 2:
				case 3:
					transform.Find ( "FailedScreen" ).Find ( "buttonRestart" ).gameObject.SetActive ( false );
					Destroy ( transform.Find ( "FailedScreen" ).Find ( "buttonLab" ).GetComponent < MainScreenPLayButtonControl > ());
					transform.Find ( "FailedScreen" ).Find ( "buttonLab" ).gameObject.AddComponent < GoToNextLevelButton > ();
					break;
				case 4:
					transform.Find ( "FailedScreen" ).Find ( "buttonRestart" ).gameObject.SetActive ( false );
					break;
			}
		}
		*/
	}

	void Start ()
	{
		powerFail = GameObject.Find ( "powerFail" );
		speedFail = GameObject.Find ( "speedFail" );
		bozDrain = GameObject.Find("platform").transform.Find("boz").gameObject;
		coraDrain = GameObject.Find("platform").transform.Find("cora").gameObject;
		faraDrain = GameObject.Find("platform").transform.Find("fara").gameObject;
		trainDrain = GameObject.Find("platform").transform.Find("trainHolder").gameObject;
		joseDrain = GameObject.Find("platform").transform.Find("jose").gameObject;
		madraDrain = GameObject.Find("platform").transform.Find("madra").gameObject;

		bozDrain.SetActive (false);
		coraDrain.SetActive (false);
		faraDrain.SetActive (false);
		trainDrain.SetActive (false);
		joseDrain.SetActive (false);
		madraDrain.SetActive (false);
	}
	public void startResoultScreen ( bool win )
	{
		if(alreadyResultOpen == true)
		{
			return;
		}
		alreadyResultOpen = true;
		Time.timeScale = 1f;
		//_missionStatusText = transform.Find ( "FailedScreen" ).Find ( "textMissionStatus" ).GetComponent < TextMesh > ();

		camera.depth = 100;
		if(Camera.main != null)
		{
			Camera.main.depth = 0;
		}
		else
		{
			GameObject.Find("camHolderStatic").GetComponent< Camera > ().depth = 0;
		}

		//FlurryAnalytics.Instance ().LogEvent ( "Finish Level Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));
		//GoogleAnalytics.instance.LogScreen ( "Finish Level Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));

		if ( win )
		{
			int numberOfTrainSuccess = SaveDataManager.getValue ( SaveDataManager.TRAIN_SUCCESS );
			numberOfTrainSuccess++;
			SaveDataManager.save ( SaveDataManager.TRAIN_SUCCESS, numberOfTrainSuccess );
			GoogleAnalytics.instance.LogScreen ( "Train - Win - Level " + TRLevelControl.SELECTED_LEVEL_NAME );

			itsOver = true;
			//FlurryAnalytics.Instance ().LogEvent ( "Win - level " + LevelControl.SELECTED_LEVEL_NAME + " - Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));
			//if ( LevelControl.CURRENT_LEVEL_CLASS != null ) GoogleAnalytics.instance.LogScreen ( "Win - level " + ( TRLevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE ? "" : "B" ) + LevelControl.SELECTED_LEVEL_NAME + " - Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));
			
			GameGlobalVariables.Missions.fillWorldsAndLeveles ();

			transform.Find ( "FailedScreen" ).gameObject.SetActive ( false );
			SaveDataManager.save ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + TRLevelControl.CURRENT_LEVEL_CLASS.myName, 1 );

			if ( TRLevelControl.CURRENT_LEVEL_CLASS.myName == "1" )
			{
				SaveDataManager.save ( SaveDataManager.WORLD_COMPLETED_PREFIX + "1", 1 );
			}

			if ( TRLevelControl.CURRENT_LEVEL_CLASS.myName == "3" )
			{
				SaveDataManager.save ( SaveDataManager.WORLD_COMPLETED_PREFIX + "2", 1 );
			}

			transform.Find ( "FailedScreen" ).gameObject.SetActive ( false );
			transform.Find ( "SuccessScreen" ).gameObject.SetActive ( true );
			myTRSuccessScreenControl = transform.Find ("SuccessScreen").GetComponent <TRSuccessScreenControl> ();
			myTRSuccessScreenControl.CustomStart();
		}
		else
		{
			int numberOfTrainFailed = SaveDataManager.getValue ( SaveDataManager.TRAIN_FAILED );
			numberOfTrainFailed++;
			SaveDataManager.save ( SaveDataManager.TRAIN_FAILED, numberOfTrainFailed );
			GoogleAnalytics.instance.LogScreen ( "Train - Win - Level " + TRLevelControl.SELECTED_LEVEL_NAME );

			itsOver = true;
			SoundManager.getInstance().silenceMusicTillNewScene();
			ChChChSoundManger.getInstance ().mySource.volume = 0.0f;
			SoundManager.getInstance ().playSound ( SoundManager.MISSION_FAILED );
			_missionFailText = GameObject.Find( "textMissionFailCause" ).GetComponent < TextMesh > ();
			//_missionStatusText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission_failed";
			transform.Find ( "FailedScreen" ).gameObject.SetActive ( true );
			transform.Find ( "SuccessScreen" ).gameObject.SetActive ( false );
		
			if(myCharacter != null)
			{
				//powerFail.SetActive( true );
				//speedFail.SetActive( false );
				if(myCharacter.myID == GameElements.CHAR_FARADAYDO_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "train_failed_faradaydo_no_power";
					faraDrain.SetActive(true);
				}
				if(myCharacter.myID == GameElements.CHAR_JOSE_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "train_failed_jose_no_power";
					joseDrain.SetActive(true);
					joseDrain.GetComponent<SkeletonAnimation>().animationName = "deactive";
				}
				if(myCharacter.myID == GameElements.CHAR_CORA_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "train_failed_cora_no_power";
					coraDrain.SetActive(true);
				}
			}
			else
			{
				trainDrain.SetActive(true);
				_missionFailText.GetComponent < GameTextControl > ().myKey = "train_failed";
				//powerFail.SetActive( false );
				//speedFail.SetActive( true );
				myCharacterHolder =  GameObject.Find( "character" );
			}
		}
	}
}







































