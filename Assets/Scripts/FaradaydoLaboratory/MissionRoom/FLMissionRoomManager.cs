using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class FLMissionRoomManager : MonoBehaviour 
{
	public struct ShowMapObjectClass
	{
		public GameObject blackHole;
		public bool onlyzoomout;
		public bool heighLightMiningNode;
	}
	//*************************************************************//
	public const int NUMBER_OF_WORLDS = 3;
	public static bool AFTER_INTRO = false;
	public static bool DO_NOT_TRIGGER_MAP_DIALOG = false;
	//*************************************************************//
	public Texture2D finishedLevelTexture;
	public Texture2D finishedLevelTrainTexture;
	public Texture2D finishedLevelMiningTexture;
	public Texture2D finishedLevelBonusTexture;
	public GameObject missionBriefPrefab;
	public GameObject missionPreScreen;
	public GameObject TRMissionPreScreen;
	public GameObject mineMissionPreSreen;
	public GameObject dangerScreenPrefab;
	public Texture2D activeButton;
	public Texture2D inActiveButton;
	
	public Texture2D levelNonStars3;
	public Texture2D levelOneStar3;
	public Texture2D levelTwoStars3;
	public Texture2D levelThreeStars3;
	public Texture2D levelNonStars2;
	public Texture2D levelOneStar2;
	public Texture2D levelTwoStars2;
	public Texture2D levelNonStars1;
	public Texture2D levelOneStar1;
	
	public Texture2D miningLevelReady;
	public Texture2D miningLevelNotReady;
	
	public GameObject missionTableOnLevel;
	public GameObject allWorldsObject;
	
	public float rotationYForWorlds = 180f;
	public int swipeDirection = 1;

	public bool onMap = false;

	public GameObject lockOnLevel26;
	//==========================Daves Edit=============================
	private GameObject theLab;
	private GameObject myDefaultPos;
	private GameObject myTempPos;
	//==========================Daves Edit=============================
	//*************************************************************//
	private int[] _missionTablePosition;
	private int _currentWorldID = 0;
	private GameObject _missionTablePrefab;
	
	private bool _blockSwipingMissions = false;
	private bool _blockRotating = false;
	
	private float _swipeSpeed;

	private bool _animationOfMapToLab = false;
	private bool mapWasFirst = false;
	private int stepCounter = 0;

	private bool _waitTillWorldRotated = false;
	private int _currentWorldOnSign = 0;

	private ShowMapObjectClass _showMapObject;
	private bool _hideMapInitiated = false;

	//*************************************************************//
	private static FLMissionRoomManager _meInstance;
	public static FLMissionRoomManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MissionRoomManagerObject" ).GetComponent < FLMissionRoomManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	
	void Awake ()
	{
		_showMapObject = new ShowMapObjectClass ();

		//myTempPos = GameObject.Find ("TempBackground");
		theLab = GameObject.Find ("Background");
		myDefaultPos = GameObject.Find ("DefaultPos");
		allWorldsObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).gameObject;
		allWorldsObject.transform.Find ( "0" ).Find ( "levels" ).gameObject.SetActive ( true );

		GameGlobalVariables.Missions.fillWorldsAndLeveles ();

		_currentWorldID = SaveDataManager.getValue ( SaveDataManager.WORLD_I_WAS_ON_PREFIX );
		_currentWorldOnSign = _currentWorldID;

		switch ( _currentWorldID )
		{
		case 0:
			Camera.main.transform.Find ( "world" ).Find ( "sign" ).Find ( "textWorldName" ).GetComponent < GameTextControl > ().myKey = GameGlobalVariables.Missions.WORLDS[0].name;
			break;
		case 1:
			Camera.main.transform.Find ( "world" ).Find ( "sign" ).Find ( "textWorldName" ).GetComponent < GameTextControl > ().myKey = GameGlobalVariables.Missions.WORLDS[1].name;
			break;
		case 2:
			Camera.main.transform.Find ( "world" ).Find ( "sign" ).Find ( "textWorldName" ).GetComponent < GameTextControl > ().myKey = GameGlobalVariables.Missions.WORLDS[2].name;
			break;
		}

		if ( _currentWorldID == 1 )
		{
			rotationYForWorlds = 198 + 1f * 3f;
			allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 198 + 1f * 3f, 0f ));
		}
		else if ( _currentWorldID == 2 )
		{
			rotationYForWorlds = 214f + 3f * 2f;
			allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 214f + 3f * 2f, 0f ));
		}
		 
		if ( AFTER_INTRO || GameGlobalVariables.BLOCK_LAB_ENTERED || GameGlobalVariables.CUT_DOWN_GAME )
		{
			showMissionLevelesScreen ( true, true, true );
		}

		if ( SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "5" ))
		{
			GameGlobalVariables.BLOCK_LAB_ENTERED = false;
		}

		manageMissionsScreen ();
	}

	IEnumerator Start ()
	{
		yield return new WaitForSeconds (0.5f);

		if(GameGlobalVariables.LAB_ENTERED >= 3)
		{
			stepCounter ++;
			Camera.main.transform.Find ( "world" ).localPosition = new Vector3 ( 0f, 0f, 6f );
			foreach(GameObject moveable in GameObject.FindGameObjectsWithTag("ForMovement"))
			{
				moveable.transform.position -= new Vector3 (50, 0, 0 );
			}
		}

		setMapRotation ();

		yield return new WaitForSeconds ( 0.2f );
		if ( ! DO_NOT_TRIGGER_MAP_DIALOG ) FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( UnlockLevelSequenceManager.LEVEL_I_AM_ON, UnlockLevelSequenceManager.LEVEL_NODE_I_AM_ON_GAME_OBJECT );

		if ( UnlockLevelSequenceManager.LEVEL_I_AM_ON > 4 )
		{
			GameGlobalVariables.BLOCK_LAB_ENTERED = false;
		}
		//theLab = GameObject.Find ("Background");
		//myDefaultPos = theLab.transform;
		//myTempPos = GameObject.Find("TempBackground");
	}

	public void setMapRotation()
	{
		//======================================Daves Work=========================================
		if ( _currentWorldID == 0 )
		{
			allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 180f, 0f ));
		}
		else if ( _currentWorldID == 1 )
		{
			allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 198 + 1f * 3f, 0f ));
		}
		else if ( _currentWorldID == 2 )
		{
			allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 214f + 3f * 2f, 0f ));
		}
		//======================================Daves Work=========================================
	}
	
	public void CustomStart ()
	{
		_missionTablePosition = new int[2] { 12 + 3, 8 + 2 };
		_missionTablePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractiveMissionTable" );
		_missionTablePrefab.tag = ("ForMovement");
		
		missionTableOnLevel = ( GameObject ) Instantiate ( _missionTablePrefab, new Vector3 ((float) _missionTablePosition[0], (float) ( FLLevelControl.LEVEL_HEIGHT - _missionTablePosition[1] ), (float) _missionTablePosition[1] - 0.5f ), _missionTablePrefab.transform.rotation );
		GameObject interactiveTileInstantMesh = missionTableOnLevel.transform.Find ( "tile" ).gameObject;
		
		IComponent currentIComponent = interactiveTileInstantMesh.AddComponent < IComponent > ();
		currentIComponent.myID = GameElements.MISSION_TABLE_OBJECT;
		
		interactiveTileInstantMesh.AddComponent < SelectedComponenent > ();
		interactiveTileInstantMesh.AddComponent < FLMissionTableControl > ();
		
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0]][_missionTablePosition[1]] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 1][_missionTablePosition[1]] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 2][_missionTablePosition[1]] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 3][_missionTablePosition[1]] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 4][_missionTablePosition[1]] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 5][_missionTablePosition[1]] = GameElements.UNWALKABLE;
		
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0]][_missionTablePosition[1] + 1] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 1][_missionTablePosition[1] + 1] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 2][_missionTablePosition[1] + 1] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 3][_missionTablePosition[1] + 1] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 4][_missionTablePosition[1] + 1] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 5][_missionTablePosition[1] + 1] = GameElements.UNWALKABLE;
	
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0]][_missionTablePosition[1] + 2] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 1][_missionTablePosition[1] + 2] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 2][_missionTablePosition[1] + 2] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 3][_missionTablePosition[1] + 2] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 4][_missionTablePosition[1] + 2] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][_missionTablePosition[0] + 5][_missionTablePosition[1] + 2] = GameElements.UNWALKABLE;
	}

	public void setCurrentWorld ( int worldValue )
	{
		_currentWorldID = worldValue;
		SaveDataManager.save ( SaveDataManager.WORLD_I_WAS_ON_PREFIX, _currentWorldID );
	}
	
	public void manageMissionsScreen ()
	{
		LockScreenControl.NUMBER_OF_STARS = 0;

		int finishedLevels = 0;

		bool unlockLevel06 = false;

		if ( SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "6" ))
		{
			unlockLevel06 = true;
		}

		for ( int i = 0; i < NUMBER_OF_WORLDS; i++ )
		{
			foreach ( Transform levelIconTransform in allWorldsObject.transform.Find ( i.ToString ()).Find ( "levels" ))
			{
				if ( levelIconTransform.gameObject.name.Contains ( "level" ))
				{
					if ( GameGlobalVariables.SHOW_LEVEL_NUMBERS ) levelIconTransform.Find ( "textNumber" ).renderer.enabled = true;

					int levelID = 0;
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "level", "" );
					int.TryParse ( levelIDString, out levelID );

					FLMissionScreenLevelIconControl currentFLMissionScreenLevelIconControl = levelIconTransform.gameObject.GetComponent < FLMissionScreenLevelIconControl > ();
					if ( currentFLMissionScreenLevelIconControl == null ) currentFLMissionScreenLevelIconControl = levelIconTransform.gameObject.AddComponent < FLMissionScreenLevelIconControl > ();
					currentFLMissionScreenLevelIconControl.myLevelClass = GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1];
					currentFLMissionScreenLevelIconControl.myLevelClass.requiredElements = new List < GameGlobalVariables.Missions.LevelClass.RequiredElement > ();
					currentFLMissionScreenLevelIconControl.myLevelClass.requiredElements.Add ( new GameGlobalVariables.Missions.LevelClass.RequiredElement ( GameElements.ICON_RECHARGEOCORES, 1 ));

					if ( levelID == 1 )
					{
						XmlDocument levelXml = new XmlDocument ();
						levelXml.LoadXml ( SelectLevel.ALL_LEVELS[levelID] );
						
						int numberOfRedirectors = 0;
						int.TryParse ( levelXml.ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText, out numberOfRedirectors );
						if ( numberOfRedirectors != 0 ) currentFLMissionScreenLevelIconControl.myLevelClass.requiredElements.Add ( new GameGlobalVariables.Missions.LevelClass.RequiredElement ( GameElements.ICON_REDIRECTORS, numberOfRedirectors ));
						
						LevelControl.SpecificLevelData currentSpecificLevelData = new LevelControl.SpecificLevelData ();
						currentFLMissionScreenLevelIconControl.myLevelClass.mySpecificLevelData = currentSpecificLevelData;
					}
				}
				else if ( levelIconTransform.gameObject.name.Contains ( "mining" ))
				{
					int levelID = 0;
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "mining", "" );
					int.TryParse ( levelIDString, out levelID );
					FLMissionScreenMiningLevelIconControl currentFLMissionScreenMiningLevelIconControl = levelIconTransform.gameObject.GetComponent < FLMissionScreenMiningLevelIconControl > ();
					if ( currentFLMissionScreenMiningLevelIconControl == null ) currentFLMissionScreenMiningLevelIconControl = levelIconTransform.gameObject.AddComponent < FLMissionScreenMiningLevelIconControl > ();
					currentFLMissionScreenMiningLevelIconControl.myLevelClass = GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1];
				}
				else if ( levelIconTransform.gameObject.name.Contains ( "train" ))
				{
					int levelID = 0;
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "train", "" );
					int.TryParse ( levelIDString, out levelID );
					FLMissionScreenTrainLevelIconControl currentFLMissionScreenTrainLevelIconControl = levelIconTransform.gameObject.GetComponent < FLMissionScreenTrainLevelIconControl > ();
					if ( currentFLMissionScreenTrainLevelIconControl == null ) currentFLMissionScreenTrainLevelIconControl = levelIconTransform.gameObject.AddComponent < FLMissionScreenTrainLevelIconControl > ();
					currentFLMissionScreenTrainLevelIconControl.myLevelClass = GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1];
				}
				else if ( levelIconTransform.gameObject.name.Contains ( "bonus" ))
				{
					if ( GameGlobalVariables.SHOW_LEVEL_NUMBERS ) levelIconTransform.Find ( "textNumber" ).renderer.enabled = true;

					int levelID = 0;
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "bonus", "" );
					int.TryParse ( levelIDString, out levelID );

					FLMissionScreenLevelIconControl currentFLMissionScreenLevelIconControl = levelIconTransform.gameObject.GetComponent < FLMissionScreenLevelIconControl > ();
					if ( currentFLMissionScreenLevelIconControl == null ) currentFLMissionScreenLevelIconControl = levelIconTransform.gameObject.AddComponent < FLMissionScreenLevelIconControl > ();

					currentFLMissionScreenLevelIconControl.myLevelClass = GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1];
					currentFLMissionScreenLevelIconControl.myLevelClass.type = FLMissionScreenNodeManager.TYPE_BONUS_NODE;
					currentFLMissionScreenLevelIconControl.myLevelClass.requiredElements = new List < GameGlobalVariables.Missions.LevelClass.RequiredElement > ();
					currentFLMissionScreenLevelIconControl.myLevelClass.requiredElements.Add ( new GameGlobalVariables.Missions.LevelClass.RequiredElement ( GameElements.ICON_RECHARGEOCORES, 1 ));

					/*
					XmlDocument levelXml = new XmlDocument ();
					levelXml.LoadXml ( SelectLevel.ALL_BONUS_LEVELS[levelID] );
					
					int numberOfRedirectors = 0;
					int.TryParse ( levelXml.ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText, out numberOfRedirectors );
					if ( numberOfRedirectors != 0 ) currentFLMissionScreenLevelIconControl.myLevelClass.requiredElements.Add ( new GameGlobalVariables.Missions.LevelClass.RequiredElement ( GameElements.ICON_REDIRECTORS, numberOfRedirectors ));
					
					LevelControl.SpecificLevelData currentSpecificLevelData = new LevelControl.SpecificLevelData ();
					currentFLMissionScreenLevelIconControl.myLevelClass.mySpecificLevelData = currentSpecificLevelData;
					*/
				}
			}

			foreach ( Transform levelIconTransform in allWorldsObject.transform.Find ( i.ToString ()).Find ( "levels" ))
			{
				if ( levelIconTransform.gameObject.name.Contains ( "level" ))
				{
					//==============================Daves Work==============================
					levelIconTransform.Find ( "stars" ).renderer.enabled = true;

					//==============================Daves Work==============================
					int levelID = 0;
					
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "level", "" );
					int.TryParse ( levelIDString, out levelID );
					if ( SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].myName ) == 1 )
					{
						GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].iAmFinished = true;
					}

					GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].starsEarned = SaveDataManager.getValue ( SaveDataManager.LEVEL_STARS_PREFIX + GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].myName );

					if ( levelID != 26 || i != 2 )
					{
						LockScreenControl.NUMBER_OF_STARS += GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].starsEarned;
					}

					switch ( GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].starsEarned )
					{
						case 3:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelThreeStars3;
							break;
						case 2:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelTwoStars3;
							break;
						case 1:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelOneStar3;
							break;
						case 0:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelNonStars3;
							break;
					}

					levelIconTransform.Find ( "textNumber" ).GetComponent < TextMesh > ().text = GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].myName;
					if (( GameGlobalVariables.UNLOCK_ALL_LEVELS ) || ( GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].iAmFinished ) || ( levelID == 1 ))
					{
						levelIconTransform.GetComponent < FLMissionScreenLevelIconControl > ().mayStart = true;
						levelIconTransform.renderer.enabled = true;

						switch ( levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().type )
						{
							case FLMissionScreenNodeManager.TYPE_REGULAR_NODE:
								levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_BONUS_NODE:
								levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_MINING_NODE:
								levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_TRAIN_NODE:
								levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;

								break;
						}
					}

					if ( levelID == 12 && i == 1 )
					{
						int worldCompletd = SaveDataManager.getValue ( SaveDataManager.WORLD_COMPLETED_PREFIX + "1" );

						if ( worldCompletd == 1 )
						{
							levelIconTransform.GetComponent < FLMissionScreenLevelIconControl > ().mayStart = true;
							levelIconTransform.renderer.enabled = true;

							UnlockLevelSequenceManager.LEVEL_I_AM_ON = 12;
							UnlockLevelSequenceManager.LEVEL_NODE_I_AM_ON_GAME_OBJECT = levelIconTransform.gameObject;

							switch ( levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().type )
							{
							case FLMissionScreenNodeManager.TYPE_REGULAR_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_BONUS_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_MINING_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_TRAIN_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;

								break;
							}
						}
					}

					if ( levelID == 26 && i == 2 )
					{
						int worldCompletd = SaveDataManager.getValue ( SaveDataManager.WORLD_COMPLETED_PREFIX + "2" );
						
						if ( worldCompletd == 1 )
						{
							lockOnLevel26.SetActive ( true );
							Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "2" ).Find ( "levels" ).Find ( "_clouds" ).Find ( "could01" ).gameObject.SetActive ( false );

							if ( lockOnLevel26.GetComponent < SelectedComponenent > () == null )
							{
								lockOnLevel26.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
							}
							else 
							{
								lockOnLevel26.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
							}

							levelIconTransform.GetComponent < FLMissionScreenLevelIconControl > ().mayStart = true;
							levelIconTransform.renderer.enabled = false;
							
							UnlockLevelSequenceManager.LEVEL_I_AM_ON = 26;
							UnlockLevelSequenceManager.LEVEL_NODE_I_AM_ON_GAME_OBJECT = levelIconTransform.gameObject;
							
							switch ( levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().type )
							{
							case FLMissionScreenNodeManager.TYPE_REGULAR_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_BONUS_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_MINING_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;

								break;
							case FLMissionScreenNodeManager.TYPE_TRAIN_NODE:

								levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;


								levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;

								break;
							}
						}
						else
						{
							levelIconTransform.GetComponent < FLMissionScreenLevelIconControl > ().mayStart = false;
							levelIconTransform.renderer.enabled = false;
						}
					}
					
					if ( GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].iAmFinished )
					{
						finishedLevels++;
						foreach ( FLMissionScreenNodeManager nodeManger in levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().unlockingNodes )
						{
							if ( nodeManger != null ) nodeManger.startUnlockingProcedure ();
						}
					}
					else if ( ! GameGlobalVariables.CUT_DOWN_GAME && levelID == 6 && unlockLevel06 )
					{
						levelIconTransform.gameObject.AddComponent < UnlockLevelSequenceManager > ();
					}

				}
				else if ( levelIconTransform.gameObject.name.Contains ( "mining" ))
				{
					int levelID = 0;
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "mining", "" );
					int.TryParse ( levelIDString, out levelID );


					if ( SaveDataManager.getValue ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + GameGlobalVariables.Missions.WORLDS[i].levels[levelID - 1].myName ) == 1 )
					{
						GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1].iAmFinished = true;
					}

					GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1].starsEarned = SaveDataManager.getValue ( SaveDataManager.MINING_LEVEL_STARS_PREFIX + GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1].myName );
					switch ( GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1].starsEarned )
					{
						case 3:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelThreeStars3;

							break;
						case 2:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelTwoStars3;

							break;
						case 1:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelOneStar3;

							break;
						case 0:
							levelIconTransform.Find ( "stars" ).renderer.enabled = false;
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelNonStars3;

							break;
					}

					levelIconTransform.GetComponent < FLMissionScreenMiningLevelIconControl > ().turnOnCoolDown ();

					/*
					int startTime = SaveDataManager.getValue ( SaveDataManager.MINING_LEVEL_COOLDOWN_TIME_PREFIX + GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1].myName );
					if ( startTime == 0 )
					{
						levelIconTransform.GetComponent < FLMissionScreenMiningLevelIconControl > ().mayStart = true;
						levelIconTransform.GetComponent < FLMissionScreenMiningLevelIconControl > ().coolingDown = false;
						//levelIconTransform.renderer.material.mainTexture = miningLevelReady;
					}
					else
					{
						levelIconTransform.GetComponent < FLMissionScreenMiningLevelIconControl > ().mayStart = false;
						levelIconTransform.GetComponent < FLMissionScreenMiningLevelIconControl > ().coolingDown = true;
						//levelIconTransform.renderer.material.mainTexture = miningLevelNotReady;
					}
					*/

					if ( GameGlobalVariables.Missions.WORLDS[i].miningLevels[levelID - 1].iAmFinished )
					{
						finishedLevels++;
						foreach ( FLMissionScreenNodeManager nodeManger in levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().unlockingNodes )
						{
							nodeManger.startUnlockingProcedure ();
						}
					}
				}
				else if ( levelIconTransform.gameObject.name.Contains ( "train" ))
				{
					int levelID = 0;
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "train", "" );
					int.TryParse ( levelIDString, out levelID );

					if ( SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].myName ) == 1 )
					{
						GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].iAmFinished = true;
					}

					GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].starsEarned = SaveDataManager.getValue ( SaveDataManager.TRAIN_LEVEL_STARS_PREFIX + GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].myName );

					LockScreenControl.NUMBER_OF_STARS += GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].starsEarned;

					switch ( GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].starsEarned )
					{
					case 3:
						levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelThreeStars3;

						break;
					case 2:

						levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelTwoStars3;

							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelTwoStars3;

						break;
					case 1:

						levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelOneStar3;


							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelOneStar3;

						break;
					case 0:

						levelIconTransform.Find ( "stars" ).renderer.enabled = true;
						levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelNonStars3;

							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelNonStars3;

						break;
					}

					if ( GameGlobalVariables.Missions.WORLDS[i].trainLevels[levelID - 1].iAmFinished )
					{
						foreach ( FLMissionScreenNodeManager nodeManger in levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().unlockingNodes )
						{
							nodeManger.startUnlockingProcedure ();
						}
					}
				}
				else if ( levelIconTransform.gameObject.name.Contains ( "bonus" ))
				{
					int levelID = 0;
					
					string levelIDString = levelIconTransform.gameObject.name.Replace ( "bonus", "" );
					int.TryParse ( levelIDString, out levelID );

					if ( SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "B" + GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].myName ) == 1 )
					{
						GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].iAmFinished = true;
					}
					
					GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].starsEarned = SaveDataManager.getValue ( SaveDataManager.LEVEL_STARS_PREFIX + "B" + GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].myName );

					LockScreenControl.NUMBER_OF_STARS += GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].starsEarned;

					switch ( GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].starsEarned )
					{
						case 3:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelThreeStars3;
							break;
						case 2:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelTwoStars3;
							break;
						case 1:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelOneStar3;
							break;
						case 0:
							levelIconTransform.Find ( "stars" ).renderer.material.mainTexture = levelNonStars3;
							break;
					}
					
					levelIconTransform.Find ( "textNumber" ).GetComponent < TextMesh > ().text = "B" + GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].myName;
					
					if (( GameGlobalVariables.UNLOCK_ALL_LEVELS ) || ( GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].iAmFinished ))
					{
						levelIconTransform.GetComponent < FLMissionScreenLevelIconControl > ().mayStart = true;
						levelIconTransform.renderer.enabled = true;
						
						switch ( levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().type )
						{
						case FLMissionScreenNodeManager.TYPE_REGULAR_NODE:

							levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;

							levelIconTransform.renderer.material.mainTexture = finishedLevelTexture;

							break;
						case FLMissionScreenNodeManager.TYPE_BONUS_NODE:

							levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;

							levelIconTransform.renderer.material.mainTexture = finishedLevelBonusTexture;

							break;
						case FLMissionScreenNodeManager.TYPE_MINING_NODE:

							levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;

							levelIconTransform.renderer.material.mainTexture = finishedLevelMiningTexture;

							break;
						case FLMissionScreenNodeManager.TYPE_TRAIN_NODE:

							levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;

							levelIconTransform.renderer.material.mainTexture = finishedLevelTrainTexture;

							break;
						}
					}
					
					if ( GameGlobalVariables.Missions.WORLDS[i].bonusLevels[levelID - 1].iAmFinished )
					{
						foreach ( FLMissionScreenNodeManager nodeManger in levelIconTransform.GetComponent < FLMissionScreenNodeManager > ().unlockingNodes )
						{
							nodeManger.startUnlockingProcedure ();
						}
					}
				}
			}
		}

		if ( finishedLevels == 0 || AFTER_INTRO )
		{
			if ( allWorldsObject.transform.Find ( "0" ).Find ( "levels" ).Find ( "level01" ).gameObject.GetComponent < JumpingArrowAbove > () == null )
			{
				allWorldsObject.transform.Find ( "0" ).Find ( "levels" ).Find ( "level01" ).gameObject.AddComponent < JumpingArrowAbove > ();
			}
		}
	}
	
	public void rotateWorld ( float howMuchRotate )
	{
		Debug.Log ( "_blockRotating: " + _blockRotating + " | _waitTillWorldRotated: " + _waitTillWorldRotated + " | howMuchRotate: " + howMuchRotate );
		if (_blockRotating)	return;

		if ( Mathf.Abs ( howMuchRotate ) > 0.005f ) FLGlobalVariables.MAP_DRAGGED = true;
#if UNITY_ANDROID
		_swipeSpeed = 50f + Mathf.Abs ( howMuchRotate ) * 420f;
#elif UNITY_IPHONE
		_swipeSpeed = 50f + Mathf.Abs ( howMuchRotate ) * 320f;
#endif

		if ( ! _waitTillWorldRotated )
		{
			if ( howMuchRotate > 0f )
			{
				Debug.Log ( "Swipe map to right" );

				_waitTillWorldRotated = true;
				swipeDirection = 0;
				_currentWorldID++;
			}
			else if ( howMuchRotate < 0f )
			{
				Debug.Log ( "Swipe map to left" );

				_waitTillWorldRotated = true;
				swipeDirection = 0;
				_currentWorldID--;
			}

			if ( _currentWorldID < 0 ) { _currentWorldID = 0; _waitTillWorldRotated = false; }
			else if ( _currentWorldID > 2 ) { _currentWorldID = 2; _waitTillWorldRotated = false; }
		}

		float maxWorldY = 214f + 3f * 2f;

		if (allWorldsObject.transform.rotation.eulerAngles.y < maxWorldY && allWorldsObject.transform.rotation.eulerAngles.y > 180f) 
		{
						if (allWorldsObject.transform.rotation.eulerAngles.y + howMuchRotate < 180f)
								howMuchRotate = 180f - allWorldsObject.transform.rotation.eulerAngles.y;
						if (allWorldsObject.transform.rotation.eulerAngles.y + howMuchRotate > maxWorldY)
								howMuchRotate = maxWorldY - allWorldsObject.transform.rotation.eulerAngles.y;
						allWorldsObject.transform.Rotate (new Vector3 (0f, howMuchRotate, 0f));
				} else if (allWorldsObject.transform.rotation.eulerAngles.y <= 180f && howMuchRotate > 0f) {
						allWorldsObject.transform.Rotate (new Vector3 (0f, howMuchRotate, 0f));
				} else if (allWorldsObject.transform.rotation.eulerAngles.y >= maxWorldY && howMuchRotate < 0f) {
						allWorldsObject.transform.Rotate (new Vector3 (0f, howMuchRotate, 0f));
				}
				//====================================Daves Edit===================================
	}

	private void onFinishedMoveSignUp ()
	{
		switch ( _currentWorldID )
		{
		case 0:
			iTween.MoveTo ( Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject.transform.localPosition + Vector3.down * 3f, "islocal", true, "oncompletetarget", this.gameObject, "oncomplete", "onFinishedMoveSignDown" ));
			Camera.main.transform.Find ( "world" ).Find ( "sign" ).Find ( "textWorldName" ).GetComponent < GameTextControl > ().myKey = GameGlobalVariables.Missions.WORLDS[0].name;
			break;
		case 1:
			iTween.MoveTo ( Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject.transform.localPosition + Vector3.down * 3f, "islocal", true, "oncompletetarget", this.gameObject, "oncomplete", "onFinishedMoveSignDown" ));
			Camera.main.transform.Find ( "world" ).Find ( "sign" ).Find ( "textWorldName" ).GetComponent < GameTextControl > ().myKey = GameGlobalVariables.Missions.WORLDS[1].name;
			break;
		case 2:
			iTween.MoveTo ( Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject.transform.localPosition + Vector3.down * 3f, "islocal", true, "oncompletetarget", this.gameObject, "oncomplete", "onFinishedMoveSignDown" ));
			Camera.main.transform.Find ( "world" ).Find ( "sign" ).Find ( "textWorldName" ).GetComponent < GameTextControl > ().myKey = GameGlobalVariables.Missions.WORLDS[2].name;
			break;
		}
	}
	
	void Update ()
	{
		switch ( _currentWorldID )
		{
		case 0:
			if ( _currentWorldOnSign != 0 )
			{
				_currentWorldOnSign = 0;

				iTween.MoveTo ( Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject.transform.localPosition + Vector3.up * 3f, "islocal", true, "oncompletetarget", this.gameObject, "oncomplete", "onFinishedMoveSignUp" ));
			}
			break;
		case 1:
			if ( _currentWorldOnSign != 1 )
			{
				_currentWorldOnSign = 1;

				iTween.MoveTo ( Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject.transform.localPosition + Vector3.up * 3f, "islocal", true, "oncompletetarget", this.gameObject, "oncomplete", "onFinishedMoveSignUp" ));
			}
			break;
		case 2:
			if ( _currentWorldOnSign != 2 )
			{
				_currentWorldOnSign = 2;

				iTween.MoveTo ( Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", Camera.main.transform.Find ( "world" ).Find ( "sign" ).gameObject.transform.localPosition + Vector3.up * 3f, "islocal", true, "oncompletetarget", this.gameObject, "oncomplete", "onFinishedMoveSignUp" ));
			}
			break;
		}

		if ( ! _blockSwipingMissions ) return;
		if ( _swipeSpeed <= 1 ) _swipeSpeed = 10f;
		allWorldsObject.transform.Rotate ( new Vector3 ( 0f, swipeDirection * _swipeSpeed * Time.deltaTime, 0f ));
		
		if ( swipeDirection == 1 && allWorldsObject.transform.rotation.eulerAngles.y >= rotationYForWorlds )
		{
			_blockSwipingMissions = false;
		}
		else if ( swipeDirection == -1 && allWorldsObject.transform.rotation.eulerAngles.y <= rotationYForWorlds )
		{
			_blockSwipingMissions = false;
		}
		else _waitTillWorldRotated = false;

		if ( ! _blockSwipingMissions )
		{
			_waitTillWorldRotated = false;
			allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, rotationYForWorlds, 0f ));
		}
		else
		{
			NewLevelsSequenceManager.getInstance ().updateSeqence ( allWorldsObject.transform.rotation.eulerAngles.y );
		}
	}
	
	public bool checkIfReachedEndOfWorlds ()
	{
		float maxWorldY = 214f + 3f * 2f;

		if ( _currentWorldID == 0 && allWorldsObject.transform.rotation.eulerAngles.y <= 180f ) return true;
		else if ( _currentWorldID == 1 && allWorldsObject.transform.rotation.eulerAngles.y >= maxWorldY ) return true;
		
		return false;
	}
	
	public int getCurrentWorldID ()
	{
		return _currentWorldID;
	}
	
	public void changeWorldInDirection ( Vector3 direction, float customSwipeSpeed = -1f )
	{
		float maxWorldY = 214f + 3f * ( 2f );

		if ( customSwipeSpeed != -1f )
		{
			_swipeSpeed = customSwipeSpeed;
		}

		if (( direction.x > 0f ) && ( _currentWorldID > 0 ))
		{
			_currentWorldID--;
			if ( _currentWorldID < 0 )
			{
				_currentWorldID = NUMBER_OF_WORLDS - 1;
			}			
			
			swipeDirection = -1;
			rotationYForWorlds = 180f;
		}
		else if (( direction.x < 0f ) && ( _currentWorldID < NUMBER_OF_WORLDS - 1 ))
		{
			_currentWorldID++;
			if ( _currentWorldID > NUMBER_OF_WORLDS - 1 )
			{
				_currentWorldID = 0;
			}
			
			swipeDirection = 1;
			rotationYForWorlds = maxWorldY;
		}
	}

	public void swipeToCurrentWorld ()
	{
		if ( _currentWorldID == 0 )
		{
			if ( allWorldsObject.transform.rotation.eulerAngles.y > 180f ) { swipeDirection = -1; _blockSwipingMissions = true; }
			else if (  allWorldsObject.transform.rotation.eulerAngles.y < 180f ) { swipeDirection = 1; _blockSwipingMissions = true; }
			rotationYForWorlds = 180f;
		}
		else if ( _currentWorldID == 1 )
		{
			if ( allWorldsObject.transform.rotation.eulerAngles.y > 198f + 2f * 2f ) { swipeDirection = -1; _blockSwipingMissions = true; }
			else if (  allWorldsObject.transform.rotation.eulerAngles.y < 198f + 1f * 3f ) { swipeDirection = 1; _blockSwipingMissions = true; }
			rotationYForWorlds = 198 + 1f * 3f;
		}
		else if ( _currentWorldID == 2 )
		{
			if ( allWorldsObject.transform.rotation.eulerAngles.y > 214f + 3f * 2f ) { swipeDirection = -1; _blockSwipingMissions = true; }
			else if (  allWorldsObject.transform.rotation.eulerAngles.y < 214f + 3f * 2f ) { swipeDirection = 1; _blockSwipingMissions = true; }
			rotationYForWorlds = 214f + 3f * 2f;
		}
	}
	
	public void blockOrUnblockSwiping ( bool isOn )
	{
		_blockSwipingMissions = isOn;
	}
	
	public void blockOrUnblockRotating ( bool isOn )
	{
		_blockRotating = isOn;
	}

	public void rememberWorldIWasOn ()
	{
		SaveDataManager.save ( SaveDataManager.WORLD_I_WAS_ON_PREFIX, _currentWorldID );
	}
	
	public void showMissionLevelesScreen ( bool isOn, bool withAnimation = true, bool onlyZommOut = false, bool haighLightMiningNode = false, bool force = false )
	{
		if ( _animationOfMapToLab ) return;

		onMap = isOn;

		GameObject blackHole = Camera.main.transform.Find ( "blackHole" ).gameObject;
		_showMapObject.blackHole = blackHole;
		_showMapObject.heighLightMiningNode = haighLightMiningNode;
		_showMapObject.onlyzoomout = onlyZommOut;
		
		if ( isOn )
		{
			_hideMapInitiated = false;
			FLGlobalVariables.MISSION_SCREEN = true;
			if ( withAnimation )
			{
				StartCoroutine ( "showMap", _showMapObject );
			}
			else 
			{
				Camera.main.transform.Find ( "world" ).localPosition = new Vector3 ( 0f, 0f, 6f );
				GameObject blackHole02 = Camera.main.transform.Find ( "dud" ).gameObject;
				_showMapObject.blackHole = blackHole02;
				StartCoroutine ( "showMap", _showMapObject );
			}
		}
		else
		{
			FLGlobalVariables.MISSION_SCREEN = false;
			StartCoroutine ( hideMap ( blackHole, 50 * stepCounter ));
		}
	}

	private IEnumerator showMap ( ShowMapObjectClass objectArgument )
	{
		if ( objectArgument.heighLightMiningNode )
		{
			if ( ! GameGlobalVariables.CUT_DOWN_GAME && FLGlobalVariables.AFTER_LAB_VISIT_02 )
			{
				FLGlobalVariables.AFTER_LAB_VISIT_02 = false;
				FLMissionTableControl.getInstance ().gameObject.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );
				Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "level06" ).gameObject.AddComponent < UnlockLevelSequenceManager > ();
				FLUIControl.getInstance ().transform.Find ( "triggerDialogLabVisit02" ).gameObject.SetActive ( false );
			}

		    setCurrentWorld ( 0 );
			allWorldsObject.transform.rotation = Quaternion.Euler ( 0f, 180f, 0f );

			GameObject haighLightObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "mining01" ).gameObject;

			if ( haighLightObject.GetComponent < SelectedComponenent > () == null )
			{
				haighLightObject.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
			}
			else 
			{
				haighLightObject.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
			}
			
			if ( haighLightObject.GetComponent < JumpingArrowAbove > () == null ) haighLightObject.AddComponent < JumpingArrowAbove > ().doNotJump = true;
		}

		_animationOfMapToLab = true;

		GameObject blackHole = objectArgument.blackHole;

		blackHole.SetActive ( true );
		blackHole.transform.localPosition = new Vector3 ( 0f, -1.481007f, 4.47f );
		if ( objectArgument.onlyzoomout ) blackHole.transform.localScale = new Vector3 ( 0.28f, 0.06f, 0.28f );
		else iTween.ScaleTo ( blackHole, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutExpo, "scale", new Vector3 ( 0.28f, 0.06f, 0.28f ), "islocal", true ));

		if ( ! objectArgument.onlyzoomout ) yield return new WaitForSeconds ( 1f );

		blackHole.transform.Find ( "cap" ).gameObject.SetActive ( true );
		Camera.main.transform.Find ( "world" ).localPosition = new Vector3 ( 0f, 0f, 6f );
		blackHole.transform.localPosition = new Vector3 ( 0f, -1.481007f, 4.47f );
		yield return new WaitForSeconds ( 0.35f );
		blackHole.transform.Find ( "cap" ).gameObject.SetActive ( false );
		iTween.ScaleTo ( blackHole, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutExpo, "scale", new Vector3 ( 24.84f, 0.06f, 24.84f ), "islocal", true ));
		yield return new WaitForSeconds ( 1f );
		blackHole.transform.localPosition = new Vector3 ( 0f, -1.481007f, 4.47f );
		blackHole.SetActive ( false );
		_animationOfMapToLab = false;
		foreach(GameObject moveable in GameObject.FindGameObjectsWithTag("ForMovement"))
		{
			moveable.transform.position -= new Vector3 (50, 0, 0 );
		}
		stepCounter ++;
	}

	private IEnumerator hideMap ( GameObject blackHole, int amountToMove = 50)
	{
		if ( theLab != null  ) 
		{
			//theLab.transform.localPosition = new Vector3 (transform.localPosition.x - 2.4f,transform.localPosition.y,transform.localPosition.z - 1.9f);
			foreach(GameObject moveable in GameObject.FindGameObjectsWithTag("ForMovement"))
			{
				moveable.transform.position += new Vector3 (amountToMove, 0, 0 );
			}
			stepCounter = 0;
		}
		_animationOfMapToLab = true;
		blackHole.SetActive ( true );
		blackHole.transform.localPosition = new Vector3 ( -4.51f, -3.28f, 4.47f );
		blackHole.transform.localScale = new Vector3 ( 24.84f, 0.06f, 24.84f );
		iTween.ScaleTo ( blackHole, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutExpo, "scale", new Vector3 ( 0.28f, 0.06f, 0.28f ), "islocal", true ));
		yield return new WaitForSeconds ( 1f );
		blackHole.transform.Find ( "cap" ).gameObject.SetActive ( true );
		Camera.main.transform.Find ( "world" ).localPosition = new Vector3 ( 0f, 17f, 6f );

		if ( FLMissionScreenMapDialogManager.CURRENT_MAP_DIALOG_SCREEN != null )
		{
			Destroy ( FLMissionScreenMapDialogManager.CURRENT_MAP_DIALOG_SCREEN );
			FLGlobalVariables.DIALOG_ON_MAP = false;
			FLMissionScreenMapDialogManager.getInstance ().trunOnButton ();
		}

		yield return new WaitForSeconds ( 0.35f );
		blackHole.transform.Find ( "cap" ).gameObject.SetActive ( false );
		iTween.ScaleTo ( blackHole, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo, "scale", new Vector3 ( 24.84f, 0.06f, 24.84f ), "islocal", true ));
		yield return new WaitForSeconds ( 0.45f );
		blackHole.transform.localPosition = new Vector3 ( -4.51f, -3.28f, 4.47f );
		blackHole.SetActive ( false );
		_animationOfMapToLab = false;
	}

	public bool checkIfAnimationOfMapToLabFinished ()
	{
		return ! _animationOfMapToLab;
	}

	public void forceFinishShowMapAnimation ()
	{
		if ( _hideMapInitiated ) return;
		_hideMapInitiated = true;

		GameObject blackHole = Camera.main.transform.Find ( "blackHole" ).gameObject;

		iTween.Stop ( blackHole );

		StopCoroutine ( "showMap" );
		StartCoroutine ( "hideMap" );

		blackHole.transform.localPosition = new Vector3 ( 0f, -1.481007f, 4.47f );
		blackHole.SetActive ( false );
		_animationOfMapToLab = false;
		foreach(GameObject moveable in GameObject.FindGameObjectsWithTag("ForMovement"))
		{
			moveable.transform.position += new Vector3 ( 0f, 0f, 0f );
		}
	}
}
