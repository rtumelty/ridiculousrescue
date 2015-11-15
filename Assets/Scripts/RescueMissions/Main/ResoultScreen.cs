using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class ResoultScreen : MonoBehaviour 
{
	//*************************************************************//	
	public static bool WIN_RESOULT = false;
	public static string TO_BE_RESCUED_TOY_NAME = "";
	public static string CHARACTER_NAME = "";
	public static string SPECIFIC_FAILED_INFO_KEY = "";
	//*************************************************************//	
	public Texture2D character;
	public GameObject madraSpine;
	public GameObject faraSpine;
	public GameObject bozSpine;
	public GameObject coraSpine;
	public Texture2D succsessSign;
	public Texture2D failedSign;
	public Texture2D succsessLight;
	public Texture2D failedLight;
	
	public Texture2D nonStars;
	public Texture2D oneStar;
	public Texture2D twoStars;
	public Texture2D threeStars;

	public bool resoultScreenIsOn = false;
	//*************************************************************//	
	private TextMesh _starOneText;
	private TextMesh _starTwoText;
	private TextMesh _starThreeText;
	private Material _starsMaterial;
	private TextMesh _missionStatusText;
	public int timeResultScreenIsActive = 0;
	public GameObject winScreen;

	//*************************************************************//	
	private static ResoultScreen _meInstance;
	public static ResoultScreen getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraResoultScreen" ).GetComponent < ResoultScreen > ();
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
		GameObject failPlatform = GameObject.Find ("FailedScreen").transform.Find("platform").gameObject;
		madraSpine = failPlatform.transform.Find ( "madra" ).gameObject;
		faraSpine = failPlatform.transform.Find ( "fara" ).gameObject;
		bozSpine = failPlatform.transform.Find ( "boz" ).gameObject;
		coraSpine = failPlatform.transform.Find ( "cora" ).gameObject;
		
		madraSpine.SetActive(false);
		faraSpine.SetActive(false);
		bozSpine.SetActive(false);
		//coraSpine.SetActive(false);
		winScreen = transform.Find ( "SuccessScreen" ).gameObject;
	}
	
	public void RestoreCamera ()
	{
		camera.depth = 0;
		Camera.main.depth = 100;
	}
	public void RestoreWinScreen ()
	{
		winScreen.SetActive (true);
	}

	public void CustomStart ()
	{
		GlobalVariables.MENU_FOR_RESOULT_SCREEN = true;
		resoultScreenIsOn = true;
		//_missionStatusText = transform.Find ( "FailedScreen" ).Find ( "textMissionStatus" ).GetComponent < TextMesh > ();
		
		camera.depth = 100;
		Camera.main.depth = 0;
		
		//transform.Find ( "FailedScreen" ).Find ( "character" ).renderer.material.mainTexture = character;
		
		//FlurryAnalytics.Instance ().LogEvent ( "Finish Level Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));
		//GoogleAnalytics.instance.LogScreen ( "Finish Level Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));
		
		if ( WIN_RESOULT )
		{
			//FlurryAnalytics.Instance ().LogEvent ( "Win - level " + LevelControl.SELECTED_LEVEL_NAME + " - Timer: " + TimeScaleManager.getTimeString ( TimerControl.getInstance ().getCurrentTime ()));
			if ( LevelControl.CURRENT_LEVEL_CLASS != null ) GoogleAnalytics.instance.LogScreen ( "Rescue - Win - Level " + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.SELECTED_LEVEL_NAME );

			GameGlobalVariables.Missions.fillWorldsAndLeveles ();

			if ( LevelControl.CURRENT_LEVEL_CLASS == null )
			{
				LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[0].levels[LevelControl.LEVEL_ID - 1];
				LevelControl.CURRENT_LEVEL_CLASS.requiredElements = new List < GameGlobalVariables.Missions.LevelClass.RequiredElement > ();
				LevelControl.CURRENT_LEVEL_CLASS.requiredElements.Add ( new GameGlobalVariables.Missions.LevelClass.RequiredElement ( GameElements.ICON_RECHARGEOCORES, 1 ));
				XmlDocument levelXml = new XmlDocument ();
				levelXml.LoadXml ( SelectLevel.ALL_LEVELS[LevelControl.LEVEL_ID] );
				
				int numberOfRedirectors = 0;
				int.TryParse ( levelXml.ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText, out numberOfRedirectors );
				if ( numberOfRedirectors != 0 ) LevelControl.CURRENT_LEVEL_CLASS.requiredElements.Add ( new GameGlobalVariables.Missions.LevelClass.RequiredElement ( GameElements.ICON_REDIRECTORS, numberOfRedirectors ));
			}

			if ( LevelControl.LEVEL_ID < 5 /*4*/ )
			{
				GameGlobalVariables.BLOCK_LAB_ENTERED = true;
			}
			else
			{
				GameGlobalVariables.BLOCK_LAB_ENTERED = false;
			}

			transform.Find ( "FailedScreen" ).gameObject.SetActive ( false );
			if(LevelControl.LEVEL_ID == 1 && LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.RESCUE || LevelControl.LEVEL_ID == 2 && LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.RESCUE)
			{
				SuccessScreenControl.getInstance ().CustomStart (true,LevelControl.LEVEL_ID);
			}
			else
			{
				SuccessScreenControl.getInstance ().CustomStart ();
			}
		}
		else
		{
//			print ("MORNING");
			coraSpine.transform.GetComponent<CharacterAnimationControl>().playAnimation(CharacterAnimationControl.POWERLESS);
			//FlurryAnalytics.Instance ().LogEvent ( "Failed - level " + LevelControl.SELECTED_LEVEL_NAME );
			if ( LevelControl.CURRENT_LEVEL_CLASS != null ) GoogleAnalytics.instance.LogScreen ( "Rescue - Fail - Level " + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.SELECTED_LEVEL_NAME );

			/*
			if ( GameGlobalVariables.LAB_ENTERED == 0 )
			{
				switch ( LevelControl.LEVEL_ID )
				{
					case 1:
					case 2:
					case 3:
					case 4:
						transform.Find ( "FailedScreen" ).Find ( "buttonRestart" ).gameObject.SetActive ( true );
						transform.Find ( "FailedScreen" ).Find ( "buttonLab" ).gameObject.SetActive ( false );
						break;
				}
			}
			*/

			foreach ( CharacterData character in LevelControl.getInstance ().charactersOnLevel )
			{
				if ( character.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] < 1 )
				{
					if ( character.myID ==  GameElements.CHAR_FARADAYDO_1_IDLE ) 
					{
						faraSpine.SetActive(true);
					}
					if ( character.myID == GameElements.CHAR_CORA_1_IDLE ) 
					{
						coraSpine.SetActive(true);
					}
					if ( character.myID ==  GameElements.CHAR_MADRA_1_IDLE ) 
					{
						//madraSpine.SetActive(true);
					}
					if ( character.myID ==  GameElements.CHAR_BOZ_1_IDLE ) 
					{
						bozSpine.SetActive(true);
					}
				}
			}
			winScreen.SetActive ( false );
			
			//_missionStatusText.GetComponent < GameTextControl > ().myKey = SPECIFIC_FAILED_INFO_KEY;
			
			GameGlobalVariables.Stats.NewResources.reset ();
			SoundManager.getInstance ().playSound ( SoundManager.MISSION_FAILED );
			//transform.Find ( "FailedScreen" ).Find ( "sign" ).renderer.material.mainTexture = failedSign;
			//transform.Find ( "FailedScreen" ).Find ( "light" ).renderer.material.mainTexture = failedLight;
			
			//transform.Find ( "FailedScreen" ).Find ( "3DText01" ).GetComponent < GameTextControl > ().myKey = SPECIFIC_FAILED_INFO_KEY;
			//transform.Find ( "FailedScreen" ).Find ( "3DText01" ).GetComponent < GameTextControl > ().characterName = CHARACTER_NAME;
			//transform.Find ( "FailedScreen" ).Find ( "3DText01" ).GetComponent < GameTextControl > ().lineLength = 70;
		}
	}

	IEnumerator CoraFailScreenAnim ()
	{
		yield return new WaitForSeconds (0f);
		coraSpine.transform.GetComponent<CharacterAnimationControl>().playAnimation(CharacterAnimationControl.POWER_UP);
		yield return new WaitForSeconds (1.5f);
		coraSpine.transform.GetComponent<CharacterAnimationControl>().playAnimation(CharacterAnimationControl.IDLE_01_ANIMATION);
	}
}

