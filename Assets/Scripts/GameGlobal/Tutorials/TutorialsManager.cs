using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialsManager : MonoBehaviour 
{
	//*************************************************************//	
	public class TutorialStepsClass
	{
		public int myID;
		public int repeatSequenceNumberBack;
		public int characterID;
		public string frameTextKey;
		public Vector3 framePosition;
		public int type;
		public GameObject tapObject;
		public bool arrowFromAbove = true;
		public List < GameObject > objectsToBeDestroyed = new List < GameObject > ();
		public List < int[] > targetTiles = new List < int[] > ();
		public List < int[] > targetTiles02 = new List < int[] > ();
		public List < int[] > showTargetTiles = new List < int[] > ();
		public List < int[] > showTargetTiles02 = new List < int[] > ();
		public int[] startPosition;
		public int[] startPosition02;
		public GameObject characterObject;
		public GameObject dragObject;
		public List < GameObject > objectsToHighLight = new List < GameObject > ();
		public int roomID;
		public float arrowBigFactor = 1f;
		public int tutorialBoxType;
		public bool highLightWithHand = false;
		public Texture2D textureAfterText = null;
		public bool preservePreviousTutorialBox = false;
		public bool dontDestroyThisTutorialBox = false;
		public bool destroyThisPreservedTutorialBoxOnEnd = false;
		public int[] forcePositionOnEnd = null;
		public float waitBeforeShowSlideArrow = 0f;
		public int[] fillTileForAMoment = null;
		public int[] refillTile = null;
		public bool triggerLabVisit02AfterACtions = false;
		public bool maximumCameraZoom = false;
		public int customNumberOfCharsInTheLine = 0;
		public bool turnOffPreviousHeighlightOnEnd = false;
		public Texture2D texture01;
		public Texture2D texture02;
		public bool showNextStepOnWinLevel = false;
		public int customLineLength = 0;
		public float delayShowTutorialBox = 0f;
		public bool dontShowNextStep = false;
		public float handMove = 1f;
		public bool fromTutorial = true;
		public bool closeMoMPanel = false;
		public bool fromMiningTutorial = false;
		public bool heighLightWithArrow = false;
		public float waitBeforeShowHandAnttaptoContinue;
		public bool doNoShowTapFrame = false;
		public bool targetIsRescueToy = false;
		public bool dontTurnOnTutorialMode = false;
		public float ifNoMoveAfterSecShowHand = 0f;
	}
	//*************************************************************//
	public const int TUTORIAL_OBJECT_TYPE_JUST_TAP = 0;
	public const int TUTORIAL_OBJECT_TYPE_TAP_OBJECT = 1;
	public const int TUTORIAL_OBJECT_TYPE_TAP_ATTACK_BUTTON = 2;
	public const int TUTORIAL_OBJECT_TYPE_JUST_TAP_AND_REPEAT = 3;
	public const int TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS = 4;
	public const int TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER = 5;
	public const int TUTORIAL_OBJECT_TYPE_SELECT_CHARACTER_AND_TAP_OBJECT = 6;
	public const int TUTORIAL_OBJECT_TYPE_BUILD_REDIRECTOR = 8;
	public const int TUTORIAL_OBJECT_TYPE_DRAG_REDIRECTOR_BEFORE_ROTATE = 9;
	public const int TUTORIAL_OBJECT_TYPE_ROTATE_REDIRECTOR = 10;
	public const int TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT = 11;
	public const int TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI = 12;
	public const int TUTORIAL_OBJECT_TYPE_TAP_OBJECT_LAB = 13;
	public const int TUTORIAL_OBJECT_TYPE_DRAG_MOM_OBJECT = 14;
	public const int TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI_THAT_ACTIVATES_WHEN_SHOW_UP = 15;
	public const int TUTORIAL_OBJECT_TYPE_JUST_TAP_FIRST_READY_MOM_OBJECT = 16;
	public const int TUTORIAL_OBJECT_TYPE_SWIPE_TO_ROOM = 17;
	public const int TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS_WITH_FRAME_TEXT = 18;
	public const int TUTORIAL_OBJECT_TYPE_TAP_SCREEN = 19;
	public const int TUTORIAL_OBJECT_TYPE_WIN_LEVEL = 20;
	public const int TUTORIAL_OBJECT_TYPE_DUMMY = 21;
	public const int TUTORIAL_OBJECT_TYPE_TAP_AND_SHOW_OBJECT = 22;
	public const int TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02 = 23;
	public const int TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER_BY_TAP = 24;
	public const int TUTORIAL_OBJECT_TYPE_STAY_SIGN_TILL_WIN_LEVEL = 25;
	public const int TUTORIAL_OBJECT_TYPE_CLOSE_MOM_UI = 26;
	public const int TUTORIAL_OBJECT_TYPE_REMOVE_MAP_ARROW = 27;
	//*************************************************************//
	public const int TUTORIAL_COMBO_UI_FULL = 0;
	public const int TUTORIAL_COMBO_UI_DEMI = 1;
	public const int TUTORIAL_COMBO_UI_CHARACTER_01 = 2;
	public const int TUTORIAL_COMBO_UI_CHARACTER_02 = 3;
	public const int TUTORIAL_COMBO_UI_DEMI_WITH_TEXTURES = 4;
	public const int TUTORIAL_COMBO_UI_FULL_BIG = 5;
	public const int TUTORIAL_COMBO_UI_DEMI_USE_ONLY_MOVES = 6;
	public const int TUTORIAL_COMBO_UI_FULL_CONTROLS = 7;
	public const int TUTORIAL_COMBO_UI_FULL_MOVES = 8;
	public const int TUTORIAL_COMBO_UI_FULL_CHALLENGE = 9;
	//*************************************************************//
	public GameObject[] tutorialComboUIPrefabs;
	public GameObject objectToBeDestoryedOnNextStep;
	//*************************************************************//
	private List < TutorialStepsClass > _currentTutorialSteps;
	private List < TutorialStepsClass > _currentTutorialRepeatSteps;
	private TutorialStepsClass _currentTutorialStep;
	private int _currentTutorialStepID = 0;
	private GameObject _currentTutorialUICombo;
	private bool _waitUntilThisUIComboIsOff = false;
	private bool _waitUntilSomeTimeBeforeNextStep = false;
	private List < SelectedComponenent > _currentHighlightedSelectedComponenents;
	private bool _lab02Visited = false;
	private bool _turnOnTriggerGEneralDialgoButton = false;
	private float _countTimeToStartTutorialBox = 0f;
	private bool _countTimeStarted = false;
	private bool _firstRunOfTutorial12 = true;
	//*************************************************************//	
	private static TutorialsManager _meInstance;
	public static TutorialsManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_TutorialsObject" ).GetComponent < TutorialsManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	
	void Awake ()
	{		
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		_currentTutorialRepeatSteps = new List < TutorialStepsClass > ();
		tutorialComboUIPrefabs = new GameObject[10];
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_FULL] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIFull");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_DEMI] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIDemi");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_CHARACTER_01] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar01");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_CHARACTER_02] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar02");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_DEMI_WITH_TEXTURES] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIDemiWithTextures");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_FULL_BIG] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIFullBig");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_DEMI_USE_ONLY_MOVES] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIDemiToEarnStars");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_FULL_CONTROLS] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIFullConrols");
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_FULL_MOVES] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIFullMoves" );
		tutorialComboUIPrefabs[TUTORIAL_COMBO_UI_FULL_CHALLENGE] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIFullChallenge" );
	}


	public void StartSmallTutorial () 
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "rescue_tutorial_general_01";
		tutorialStep.framePosition = new Vector3 ( -3f, 3.5f, 0f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_SHOW_OBJECT;
		tutorialStep.tapObject = LevelControl.getInstance ().toBeRescuedOnLevel;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		if ( LevelControl.getInstance ().coresAreOnLevel )
		{
			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
			tutorialStep.frameTextKey = "rescue_tutorial_general_02";
			tutorialStep.framePosition = new Vector3 ( -3f, 3.5f, 0f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_SHOW_OBJECT;
			tutorialStep.tapObject = LevelControl.getInstance ().lightOnLevel;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
		}
		GlobalVariables.TUTORIAL_MENU = true;
		_turnOnTriggerGEneralDialgoButton = true;
	}

	public void StartTutorial () 
	{
		return;
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && ( LevelControl.CURRENT_LEVEL_CLASS == null || LevelControl.CURRENT_LEVEL_CLASS.type != FLMissionScreenNodeManager.TYPE_BONUS_NODE ))
		{
			print ("Starting Tut " + LevelControl.LEVEL_ID);
			switch ( LevelControl.LEVEL_ID )
			{
				case 1:
					startLevel01Tutorial ();
					break;
				case 2:
					startLevel02Tutorial ();
					break;
				case 3:
					startLevel03Tutorial ();
					break;
				case 4:
					startLevel04Tutorial ();
					break;
				case 5:
					startLevel05Tutorial ();
					break;
				case 6:
					startLevel06Tutorial ();
					break;
				case 7:
					startLevel07Tutorial ();
					break;
				case 8:
					startLevel08Tutorial ();
					break;
				case 9:
					startLevel09Tutorial ();
					break;
				case 10:
					startLevel10Tutorial ();
					break;
				case 11:
					startLevel11Tutorial ();
					break;
				case 12:
					startLevel12Tutorial ();
					break;
				case 13:
					startLevel13Tutorial ();
					break;
				case 14:
					startLevel14Tutorial ();
					break;
				case 15:
					startLevel15Tutorial ();
					break;
				case 16:
					startLevel16Tutorial ();
					break;
				case 17:
					startLevel17Tutorial ();
					break;
				case 18:
					startLevel18Tutorial ();
					break;
				case 19:
					startLevel19Tutorial ();
					break;
				case 20:
					startLevel20Tutorial ();
					break;
				case 21:
					startLevel21Tutorial ();
					break;
				case 22:
					startLevel22Tutorial ();
					break;
				case 23:
					startLevel23Tutorial ();
					break;
				case 24:
					startLevel24Tutorial ();
					break;
				case 25:
					startLevel25Tutorial ();
					break;
				default:
					TimerControl.getInstance ().startTimer ( true );
				print ("here once");
					break;
			}

		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			switch ( GameGlobalVariables.LAB_ENTERED )
			{
				case 0:
					// do nothing
					break;
				case 1:
					if ( ! GameGlobalVariables.CUT_DOWN_GAME && ! FLMissionRoomManager.AFTER_INTRO && ! FLMissionRoomManager.getInstance ().onMap  ) startLab01Tutorial ();
					break;
				case 2:
					if ( ! GameGlobalVariables.CUT_DOWN_GAME && SaveDataManager.getValue ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + "1" ) == 1 && ! FLMissionRoomManager.getInstance ().onMap  ) startLab02Tutorial ();
					break;
				default:
					if ( ! GameGlobalVariables.CUT_DOWN_GAME ) FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( true, false );
					break;
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			switch ( MNLevelControl.LEVEL_ID )
			{
			case 0:
				// do nothing
				break;
			case 1:
				if(SaveDataManager.getValue ( SaveDataManager.MINES_ENTERED) == 0)
				{
					MNSpawningEnemiesManager.getInstance ().startSpawn = false;
					startMining01Tutorial ();
				}
				else
				{
					startIntroSequence ();
				}
				break;
			case 2:
				startMining02Tutorial ();
				break;
			}
		}
		else if(LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE)
		{
			TriggerDialogueControl.getInstance().turnOn ();
		}
	}

	public void triggerDialogForMiningTutorialTooMuchMetal ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "mine_level_storage_full_metal";
		tutorialStep.framePosition = new Vector3 ( 0f, 0f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_currentTutorialStepID = 0;
	}

	public void triggerDialogForMiningTutorialTooMuchPlastic ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "mine_level_storage_full_plastic";
		tutorialStep.framePosition = new Vector3 ( 0f, 0f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_currentTutorialStepID = 0;
	}

	public void triggerDialogForMiningTutorialTooMuchVines ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "mine_level_storage_full_vines";
		tutorialStep.framePosition = new Vector3 ( 0f, 0f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_currentTutorialStepID = 0;
	}

	private void startMining01Tutorial ()
	{
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_01_Miner";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.objectsToHighLight = new List < GameObject > ();
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][6][1] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][10][0] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][9][2] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][9][4] );
		tutorialStep.maximumCameraZoom = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_02_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		tutorialStep.maximumCameraZoom = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_03_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;

		tutorialStep.objectsToHighLight = new List < GameObject > ();
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][6][1] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][10][0] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][9][2] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][9][4] );

		tutorialStep.heighLightWithArrow = true;
		tutorialStep.fromMiningTutorial = true;

		tutorialStep.maximumCameraZoom = true;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_04_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;

		tutorialStep.heighLightWithArrow = true;

		tutorialStep.objectsToHighLight = new List < GameObject > ();
		tutorialStep.objectsToHighLight.Add ( GameObject.Find ( "Background" ).transform.Find ( "exitArea" ).gameObject );
		
		tutorialStep.maximumCameraZoom = true;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_turnOnTriggerGEneralDialgoButton = true;
	}

	private void startMining02Tutorial ()
	{
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_BOZ_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_02_tutorialdialogue_01_Boz";
		tutorialStep.framePosition = new Vector3 ( 0f, 0f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_turnOnTriggerGEneralDialgoButton = true;
	}


	private void startIntroSequence ()
	{
		_turnOnTriggerGEneralDialgoButton = true;
		MNTriggerDialogControl.getInstance ().turnOn ();
	}

	public void triggerGeneralDialogForMiningTutorialOnlyExit ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_05_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;

		tutorialStep.heighLightWithArrow = true;

		tutorialStep.objectsToHighLight = new List < GameObject > ();
		tutorialStep.objectsToHighLight.Add ( GameObject.Find ( "Background" ).transform.Find ( "exitArea" ).gameObject );
		
		tutorialStep.maximumCameraZoom = true;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_currentTutorialStepID = 0;
		_turnOnTriggerGEneralDialgoButton = true;
	}

	public void triggerGeneralDialogForMiningTutorial ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_03_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
		
		tutorialStep.objectsToHighLight = new List < GameObject > ();
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][6][1] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][10][0] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][9][2] );
		tutorialStep.objectsToHighLight.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][9][4] );

		tutorialStep.heighLightWithArrow = true;
		tutorialStep.fromMiningTutorial = true;

		tutorialStep.maximumCameraZoom = true;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_01_tutorialdialogue_04_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;

		tutorialStep.heighLightWithArrow = true;

		tutorialStep.objectsToHighLight = new List < GameObject > ();
		tutorialStep.objectsToHighLight.Add ( GameObject.Find ( "Background" ).transform.Find ( "exitArea" ).gameObject );
		tutorialStep.maximumCameraZoom = true;
		tutorialStep.customNumberOfCharsInTheLine = 75;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_currentTutorialStepID = 0;
		_turnOnTriggerGEneralDialgoButton = true;
	}

	public void triggerGeneralDialogForMining02Tutorial ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_BOZ_1_IDLE;
		tutorialStep.frameTextKey = "minelevel_02_tutorialdialogue_01_Boz";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		MNGlobalVariables.TUTORIAL_MENU = true;
		_currentTutorialStepID = 0;
		_turnOnTriggerGEneralDialgoButton = true;
	}

	private void startLab02Tutorial ()
	{
		_lab02Visited = false;

		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "labvisit_02_dialogue_01_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, -1.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.objectsToHighLight = new List < GameObject > ();

		foreach ( FLStorageContainerClass contatiner in FLFactoryRoomManager.getInstance ().storageContainersOnLevel )
		{
			if ( contatiner.type == FLStorageContainerClass.STORAGE_TYPE_PLASTIC )
			{
				tutorialStep.objectsToHighLight.Add ( contatiner.containerObject );
				break;
			}
		}
		tutorialStep.turnOffPreviousHeighlightOnEnd = true;
		tutorialStep.delayShowTutorialBox = 1.5f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "labvisit_02_dialogue_02_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, -1.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.objectsToHighLight = new List < GameObject > ();
		
		foreach ( FLStorageContainerClass contatiner in FLFactoryRoomManager.getInstance ().storageContainersOnLevel )
		{
			if ( contatiner.type == FLStorageContainerClass.STORAGE_TYPE_VINES )
			{
				tutorialStep.objectsToHighLight.Add ( contatiner.containerObject );
				break;
			}
		}
		tutorialStep.turnOffPreviousHeighlightOnEnd = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "labvisit_02_dialogue_03_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, -1.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.objectsToHighLight = new List < GameObject > ();
		
		foreach ( FLStorageContainerClass contatiner in FLFactoryRoomManager.getInstance ().storageContainersOnLevel )
		{
			if ( contatiner.type == FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE )
			{
				tutorialStep.objectsToHighLight.Add ( contatiner.containerObject );
				break;
			}
		}
		tutorialStep.turnOffPreviousHeighlightOnEnd = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MINER_1_IDLE;
		tutorialStep.frameTextKey = "labvisit_02_dialogue_04_Miner";
		tutorialStep.framePosition = new Vector3 ( 0f, -1.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		tutorialStep.customLineLength = 28;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "labvisit_02_dialogue_05_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, -1.5f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		if (! GameGlobalVariables.CUT_DOWN_GAME ) tutorialStep.triggerLabVisit02AfterACtions = true;
		//Daves Step
		//tutorialStep.type = TUTORIAL_OBJECT_TYPE_REMOVE_MAP_ARROW;
		if(GameObject.Find ("jumpingArrowMap(Clone)") != null)
		{
			Destroy (GameObject.Find ("jumpingArrowMap(Clone)"));
		}
		//Daves Step
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		FLGlobalVariables.TUTORIAL_MENU = true;
	}
	
	private void startLab01Tutorial ()
	{
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_1_faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.delayShowTutorialBox = 2.5f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_2_general";
		tutorialStep.framePosition = new Vector3 ( 0f, -2.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.tapObject = FLUIControl.getInstance ().transform.Find ( "FactoryRoomButton" ).gameObject;
		tutorialStep.arrowBigFactor = 2f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 03
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_3_faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, -2.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 04
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_4_faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.arrowBigFactor = 2f;
		tutorialStep.objectsToHighLight = new List < GameObject > ();
		
		tutorialStep.objectsToHighLight.Add ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject );

		_currentTutorialSteps.Add ( tutorialStep );
		tutorialStep.tapObject = FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject;
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 05
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_5_general";
		tutorialStep.framePosition = new Vector3 ( 0f, 3f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_DRAG_MOM_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.arrowBigFactor = 2f;
		tutorialStep.fromTutorial = true;
		List < int[] > targetTiles = new List<int[]> ();
		targetTiles.Add ( new int[2] { Mathf.RoundToInt ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.position.x ), Mathf.RoundToInt ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.position.z + 2f )});
		tutorialStep.targetTiles = targetTiles;	
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 06
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_6_general";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI_THAT_ACTIVATES_WHEN_SHOW_UP;
		tutorialStep.arrowBigFactor = 2f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 07
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_7_general";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP_FIRST_READY_MOM_OBJECT;
		tutorialStep.closeMoMPanel = true;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.arrowBigFactor = 2f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 08
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_8_faradaydo";
		tutorialStep.framePosition = new Vector3 ( 0f, -2.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		//tutorialStep.type = TUTORIAL_OBJECT_TYPE_CLOSE_MOM_UI;
		tutorialStep.tapObject = FLUIControl.getInstance ().transform.Find ( "MissionRoomButton" ).gameObject;
		tutorialStep.arrowBigFactor = 2f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 09
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_9_general";
		tutorialStep.framePosition = new Vector3 ( 0f, 3.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT_LAB;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_BIG;
		tutorialStep.arrowBigFactor = 2f;
		tutorialStep.arrowFromAbove = false;
		tutorialStep.customLineLength = 60;
		//Daves Step
		//tutorialStep.type = TUTORIAL_OBJECT_TYPE_REMOVE_MAP_ARROW;
		if(GameObject.Find ("jumpingArrowMap(Clone)") != null)
		{
			Destroy (GameObject.Find ("jumpingArrowMap(Clone)"));
		}
		//Daves Step
		tutorialStep.tapObject = FLMissionRoomManager.getInstance ().missionTableOnLevel;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		FLGlobalVariables.TUTORIAL_MENU = true;
	}

	public void turnOnDragMomObjectTutorial ()
	{
		_currentTutorialSteps = new List < TutorialStepsClass > ();
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "laboratory_tutorial_1_text_5_general";
		tutorialStep.framePosition = new Vector3 ( 0f, 3f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_DUMMY;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.arrowBigFactor = 2f;
		tutorialStep.fromTutorial = false;
		tutorialStep.delayShowTutorialBox = 2.0f;
		List < int[] > targetTiles = new List<int[]> ();
		targetTiles.Add ( new int[2] { Mathf.RoundToInt ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.position.x ), Mathf.RoundToInt ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.position.z + 2f )});
		tutorialStep.targetTiles = targetTiles;	
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		_currentTutorialStepID = 0;

		_waitUntilSomeTimeBeforeNextStep = false;
		_waitUntilThisUIComboIsOff = false ;

		//*************************************************************//
		FLGlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel01Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 0.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.waitBeforeShowHandAnttaptoContinue = 4f;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_1_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_CONTROLS;
		tutorialStep.highLightWithHand = true;
		tutorialStep.handMove = 0f;
		tapObject = null;
		tapObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][9][3];
		tutorialStep.tapObject = tapObject;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 06
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_2_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.2f, 3.99f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_SHOW_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_MOVES;
		tutorialStep.doNoShowTapFrame = true;
		tutorialStep.tapObject = UIControl.getInstance ().getFirstCharacterButton ().transform.Find ( "powerBar" ).gameObject;
		tutorialStep.arrowFromAbove = false;
		tutorialStep.arrowBigFactor = 2.0f;
		
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_3_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_CONTROLS;
		characterObject = null;
		characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		tutorialStep.characterObject = characterObject;
		tutorialStep.targetTiles.Add ( new int[2] { 7, 3 } );
		tutorialStep.targetIsRescueToy = true;
		tutorialStep.showTargetTiles.Add ( new int[2] { 7, 3 } );
		tutorialStep.startPosition = new int[2] { 9, 3 };
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 7
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_4_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 3.3f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_FULL_CHALLENGE;
		characterObject = null;
		characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		tutorialStep.characterObject = characterObject;
		tutorialStep.targetTiles.Add ( new int[2] { 7, 2 } );
		tutorialStep.showTargetTiles.Add ( new int[2] { 7, 1 } );
		tutorialStep.startPosition = new int[2] { 7, 3 };
		tutorialStep.dontDestroyThisTutorialBox = true;
		tutorialStep.dontTurnOnTutorialMode = true;
		tutorialStep.ifNoMoveAfterSecShowHand = 5f;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 8
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		characterObject = null;
		characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		tutorialStep.characterObject = characterObject;
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_DUMMY;
		tutorialStep.preservePreviousTutorialBox = true;
		tutorialStep.destroyThisPreservedTutorialBoxOnEnd = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}
	
	private void startLevel02Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_2_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.fillTileForAMoment = new int[2] { 6, 1 }; 
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 03
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_2_text_1_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.highLightWithHand = true;
		tutorialStep.handMove = 0f;
		tapObject = null;
		tapObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_MADRA_1_IDLE );
		tutorialStep.tapObject = tapObject;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 04
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_2_text_2_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tapObject = null;
		tapObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][6][2];
		tutorialStep.tapObject = tapObject;
		tutorialStep.highLightWithHand = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 05
		//*************************************************************//
		/*
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_2_text_3_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_ATTACK_BUTTON;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.refillTile = new int[2] { 6, 1 }; 
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		*/
		//*************************************************************//
		// step 06
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_2_text_4_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 1.5f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.forcePositionOnEnd = new int[2];
		tutorialStep.forcePositionOnEnd[0] = 8;
		tutorialStep.forcePositionOnEnd[1] = 3;

		tutorialStep.delayShowTutorialBox = 2.1f;

		tapObject = null;
		tapObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][9][3];
		tutorialStep.tapObject = tapObject;
		tutorialStep.highLightWithHand = true;
		tutorialStep.refillTile = new int[2] { 6, 1 }; 
		tutorialStep.characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_2_text_5_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		tutorialStep.characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		tutorialStep.targetTiles.Add ( new int[2] { 7, 3 } );
		tutorialStep.showTargetTiles.Add ( new int[2] { 6, 3 } );
		tutorialStep.showTargetTiles.Add ( new int[2] { 6, 0 } );
		tutorialStep.showTargetTiles.Add ( new int[2] { 0, 0 } );
		tutorialStep.startPosition = new int[2] { 9, 3 };
		tutorialStep.waitBeforeShowSlideArrow = 5f;
		tutorialStep.textureAfterText = LevelControl.getInstance ().gameElements[GameElements.ENVI_RESCUEZONE_1];
		tutorialStep.dontDestroyThisTutorialBox = true;
		tutorialStep.objectsToHighLight.Add ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_RESCUEZONE][0][0] );
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_5_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		characterObject = null;
		characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		tutorialStep.characterObject = characterObject;
		tutorialStep.waitBeforeShowSlideArrow = 5f;
		tutorialStep.targetTiles.Add ( new int[2] { 6, 1 } );
		tutorialStep.showTargetTiles.Add ( new int[2] { 6, 0 } ); 
		tutorialStep.showTargetTiles.Add ( new int[2] { 0, 0 } );
		tutorialStep.startPosition = new int[2] { 6, 3 };
		tutorialStep.preservePreviousTutorialBox = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_1_text_5_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		characterObject = null;
		characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		tutorialStep.characterObject = characterObject;
		tutorialStep.waitBeforeShowSlideArrow = 5f;
		tutorialStep.targetTiles.Add ( new int[2] { 1, 0 } );
		tutorialStep.showTargetTiles.Add ( new int[2] { 0, 0 } );
		tutorialStep.startPosition = new int[2] { 6, 0 };
		tutorialStep.preservePreviousTutorialBox = true;
		tutorialStep.destroyThisPreservedTutorialBoxOnEnd = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}
	
	private void startLevel03Tutorial ()
	{
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_3_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.9f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "NULL";
		tutorialStep.framePosition = new Vector3 ( 0f, 0.0f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_AND_SHOW_OBJECT;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI_USE_ONLY_MOVES;

		tutorialStep.tapObject = UIControl.getInstance ().getFirstCharacterButton ().transform.Find ( "powerBar" ).gameObject;
		tutorialStep.arrowFromAbove = false;
		tutorialStep.arrowBigFactor = 2.0f;

		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_3_text_2_General";
		tutorialStep.framePosition = new Vector3 ( 0f, 3f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_STAY_SIGN_TILL_WIN_LEVEL;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_3_text_3_condition_True_Cora";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.9f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		tutorialStep.dontShowNextStep = true;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_3_text_3_condition_False_Cora";
		tutorialStep.framePosition = new Vector3 ( 0f, 2.9f, 0.11f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}
	
	private void startLevel04Tutorial ()
	{
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_4_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( 0f, -2.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_4_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( 0f, -2.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MADRA_1_IDLE;
		tutorialStep.frameTextKey = "level_4_text_3_Madra";
		tutorialStep.framePosition = new Vector3 ( 0f, -2.0f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel05Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_5_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 02
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_5_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		// step 03
		//*************************************************************//
		if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][5][0] != null )
		{
			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.frameTextKey = "level_5_text_1_General";
			tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_SELECT_CHARACTER_AND_TAP_OBJECT;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
			tutorialStep.characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );
			tutorialStep.tapObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][5][0];
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
		}
		//*************************************************************//
		// step 04
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}
	
	private void startLevel06Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_6_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_6_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_6_text_4_General";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel07Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_7_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_7_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel08Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_8_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, -2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;

	}

	private void startLevel09Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_9_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_9_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MADRA_1_IDLE;
		tutorialStep.frameTextKey = "level_9_text_3_Madra";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel10Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_10_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_10_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel11Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_11_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_11_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel12Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_12_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_12_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_12_text_3_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		if ( _firstRunOfTutorial12 )
		{
			_firstRunOfTutorial12 = false;

			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			tutorialStep.frameTextKey = "level_12_text_1_General";
			tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_TAP_OBJECT;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
			tutorialStep.tapObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
			//*************************************************************//

			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			tutorialStep.frameTextKey = "level_12_text_2_General";
			tutorialStep.framePosition = new Vector3 ( 0f, -2.2f, 2f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_SELECT_CHARACTER_AND_TAP_OBJECT;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
			tutorialStep.characterObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );
			tutorialStep.tapObject = RedirectorButton.getInstance ().gameObject;
			tutorialStep.arrowFromAbove = false;
			tutorialStep.arrowBigFactor = 2.0f;
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
			//*************************************************************//
			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			tutorialStep.frameTextKey = "level_12_text_3_General";
			tutorialStep.framePosition = new Vector3 ( 0f, 2.2f, 2f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_DRAG_REDIRECTOR_BEFORE_ROTATE;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
			tutorialStep.targetTiles.Add ( new int[2] { 1, 2 });
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
			//*************************************************************//
			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			tutorialStep.frameTextKey = "level_12_text_4_General";
			tutorialStep.framePosition = new Vector3 ( 0f, 2.2f, 2f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_ROTATE_REDIRECTOR;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
			//*************************************************************//
			tutorialStep = new TutorialStepsClass ();
			ID++;
			tutorialStep.myID = ID;
			tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			tutorialStep.frameTextKey = "level_12_text_5_General";
			tutorialStep.framePosition = new Vector3 ( 0f, 2.2f, 2f );
			tutorialStep.type = TUTORIAL_OBJECT_TYPE_BUILD_REDIRECTOR;
			tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_DEMI;
			_currentTutorialSteps.Add ( tutorialStep );
			_currentTutorialRepeatSteps.Add ( null );
		}
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_12_text_4_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_12_text_5_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel13Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_13_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MADRA_1_IDLE;
		tutorialStep.frameTextKey = "level_13_text_2_Madra";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel14Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_14_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_14_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel15Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_15_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_15_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MADRA_1_IDLE;
		tutorialStep.frameTextKey = "level_15_text_3_Madra";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel16Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_16_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_16_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_16_text_3_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, -2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel17Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_17_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_17_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel18Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_18_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_18_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}


	private void startLevel19Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_19_text_1_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_19_text_2_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel20Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_20_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_20_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel21Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_21_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_MADRA_1_IDLE;
		tutorialStep.frameTextKey = "level_21_text_2_Madra";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel22Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_22_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_22_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_22_text_3_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel23Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_23_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel24Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_24_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_24_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private void startLevel25Tutorial ()
	{
		GameObject tapObject = null;
		GameObject characterObject = null;
		TutorialStepsClass tutorialStep = null;
		int ID = -1;
		//*************************************************************//
		// step 01
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_25_text_1_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
		tutorialStep.frameTextKey = "level_25_text_2_Faradaydo";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_02;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		tutorialStep = new TutorialStepsClass ();
		ID++;
		tutorialStep.myID = ID;
		tutorialStep.characterID = GameElements.CHAR_CORA_1_IDLE;
		tutorialStep.frameTextKey = "level_25_text_3_Cora";
		tutorialStep.framePosition = new Vector3 ( -3f, 2.2f, 2f );
		tutorialStep.type = TUTORIAL_OBJECT_TYPE_JUST_TAP;
		tutorialStep.tutorialBoxType = TUTORIAL_COMBO_UI_CHARACTER_01;
		_currentTutorialSteps.Add ( tutorialStep );
		_currentTutorialRepeatSteps.Add ( null );
		//*************************************************************//
		GlobalVariables.TUTORIAL_MENU = true;
	}

	private IEnumerator waitBeoreTurnOnTapText ( GameObject tutorialUIComboObject )
	{
		yield return new WaitForSeconds ( 4f );
		if ( tutorialUIComboObject )
		{
			tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = true;
			tutorialUIComboObject.transform.Find ( "tapText" ).Find ( "tapText(Clone)" ).renderer.enabled = true;
		}
	}

	private void createTutorialObject ( TutorialStepsClass tutorialStep )
	{
		GameObject tutorialUIComboObject = null;
		if ( tutorialStep.type != TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS && tutorialStep.type != TUTORIAL_OBJECT_TYPE_DUMMY && ! tutorialStep.preservePreviousTutorialBox )
		{
			tutorialUIComboObject = ( GameObject ) Instantiate ( tutorialComboUIPrefabs[tutorialStep.tutorialBoxType], Vector3.zero, tutorialComboUIPrefabs[0].transform.rotation );

			//tutorialUIComboObject.GetComponent < ScaleUIElementControl > ().scalePosition = true;

			tutorialUIComboObject.transform.parent = Camera.main.transform;
			tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, tutorialStep.framePosition.y, tutorialStep.framePosition.z ) + Vector3.forward * 3f;
			if ( tutorialStep.textureAfterText != null )
			{
				tutorialUIComboObject.transform.Find ( "textureAfterText" ).renderer.material.mainTexture = tutorialStep.textureAfterText;
				tutorialUIComboObject.transform.Find ( "textureAfterText" ).renderer.enabled = true;
			}

			if ( tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ()) tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = tutorialStep.frameTextKey;
			
			switch ( tutorialStep.tutorialBoxType )
			{
				case TUTORIAL_COMBO_UI_CHARACTER_01:
				case TUTORIAL_COMBO_UI_CHARACTER_02:
					tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = 35;
					break;
				case TUTORIAL_COMBO_UI_DEMI:
				case TUTORIAL_COMBO_UI_FULL:
					if ( tutorialStep.customNumberOfCharsInTheLine != 0 ) tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = tutorialStep.customNumberOfCharsInTheLine;
					else tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = 50;
					break;
				case TUTORIAL_COMBO_UI_DEMI_WITH_TEXTURES:
					tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = 50;
					tutorialUIComboObject.transform.Find ( "texture01" ).renderer.material.mainTexture = tutorialStep.texture01;
					tutorialUIComboObject.transform.Find ( "texture02" ).renderer.material.mainTexture = tutorialStep.texture02;
					break;
			}

			if ( tutorialStep.customLineLength != 0 )
			{
				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = tutorialStep.customLineLength;
			}

			
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
			{
				if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
				{
					tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = LevelControl.getInstance ().gameElements[tutorialStep.characterID];
					if ( tutorialStep.characterID == GameElements.CHAR_CORA_1_IDLE )
					{
						//tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
						//tutorialUIComboObject.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
					}
				}
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
			{
				if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
				{
					tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[tutorialStep.characterID];
					if ( tutorialStep.characterID == GameElements.CHAR_CORA_1_IDLE )
					{
						//tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
						//tutorialUIComboObject.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
					}
				}
			}
		
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
			{
				if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
				{
					tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[tutorialStep.characterID];
					if ( tutorialStep.characterID == GameElements.CHAR_CORA_1_IDLE )
					{
						//tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
						//tutorialUIComboObject.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
					}
				}

				if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
				{
					tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[tutorialStep.characterID];
					if ( tutorialStep.characterID == GameElements.CHAR_BOZ_1_IDLE )
					{
						tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale = new Vector3 ( tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale.x * 1.2f, tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale.y, tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale.z );
					}
				}
			}
			
			setAlphaToZero ( tutorialUIComboObject );
			
			//iTween.MoveFrom ( tutorialUIComboObject, iTween.Hash ( "time", 0.6f, "easetype", iTween.EaseType.easeOutExpo, "position", tutorialUIComboObject.transform.position + Vector3.forward * 10f ));
		}
		else if ( tutorialStep.preservePreviousTutorialBox )
		{
			tutorialUIComboObject = getCurrentTutorialUICombo ();
		}

		if ( tutorialStep.refillTile != null )
		{
			LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][tutorialStep.refillTile[0]][tutorialStep.refillTile[1]] = GameElements.EMPTY;
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][tutorialStep.refillTile[0]][tutorialStep.refillTile[1]] = null;
			tutorialStep.refillTile = null;
		}
		
		switch ( tutorialStep.type )
		{
			case TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT:
				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TutorialFrameTapControl > ();
				_currentHighlightedSelectedComponenents = new List < SelectedComponenent > ();
				foreach ( GameObject toHighlightObject in tutorialStep.objectsToHighLight )
				{
					_currentHighlightedSelectedComponenents.Add ( toHighlightObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ());
				}
			
				foreach ( SelectedComponenent toHighlightObjectsSelectedComponent in _currentHighlightedSelectedComponenents )
				{
					toHighlightObjectsSelectedComponent.setSelectedForTutorial ( true, true, true, true );
				}
			
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = true;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;
				break;
			case TUTORIAL_OBJECT_TYPE_TAP_AND_HIGHLIGHT02:
				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TutorialFrameTapControl > ();
				_currentHighlightedSelectedComponenents = new List < SelectedComponenent > ();
				foreach ( GameObject toHighlightObject in tutorialStep.objectsToHighLight )
				{
				//======================================Daves Edit========================================
					if(toHighlightObject != null)
					{
						if (  toHighlightObject.transform.Find ( "tile" ) != null )
						{
							_currentHighlightedSelectedComponenents.Add ( toHighlightObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ());
						}
						else
						{
							_currentHighlightedSelectedComponenents.Add ( toHighlightObject.GetComponent < SelectedComponenent > ());
						}
					}
				//======================================Daves Edit========================================
				}

			if ( tutorialStep.fromMiningTutorial )
			{
				for ( int i = 0; i < MNLevelControl.LEVEL_WIDTH; i++ )
				{
					for ( int j = 0; j < MNLevelControl.LEVEL_HEIGHT; j++ )
					{
						switch ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][i][j] )
						{
						case GameElements.ENVI_PLASTIC_1_ALONE:
						case GameElements.ENVI_METAL_1_ALONE:
						case GameElements.ENVI_TECHNOEGG_1_ALONE:
							if (  MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][i][j].transform.Find ( "tile" ) != null )
							{
								_currentHighlightedSelectedComponenents.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][i][j].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ());
							}
							else
							{
								_currentHighlightedSelectedComponenents.Add ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][i][j].GetComponent < SelectedComponenent > ());
							}
							break;
						}
						
					}
				}
			}
				
				foreach ( SelectedComponenent toHighlightObjectsSelectedComponent in _currentHighlightedSelectedComponenents )
				{
					if(toHighlightObjectsSelectedComponent != null)
					{
						toHighlightObjectsSelectedComponent.setSelectedForUIHighlight ( true, 1f, tutorialStep.heighLightWithArrow );
					}
				}
				
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = true;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;
				break;		
			case TUTORIAL_OBJECT_TYPE_JUST_TAP_AND_REPEAT:
			case TUTORIAL_OBJECT_TYPE_JUST_TAP:
			case TUTORIAL_OBJECT_TYPE_WIN_LEVEL:
				if ( tutorialStep.fillTileForAMoment != null )
				{
					LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][tutorialStep.fillTileForAMoment[0]][tutorialStep.fillTileForAMoment[1]] = GameElements.UNWALKABLE;
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][tutorialStep.fillTileForAMoment[0]][tutorialStep.fillTileForAMoment[1]] = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_RESCUEZONE][0][0];
					tutorialStep.fillTileForAMoment = null;
				}

				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TutorialFrameTapControl > ();

				if ( tutorialStep.waitBeforeShowHandAnttaptoContinue == 0f ) 
				{
					tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = true;
				}
				else
				{
					tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
					StartCoroutine ( "waitBeoreTurnOnTapText", tutorialUIComboObject );
				}

				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;
				GlobalVariables.TUTORIAL_MENU = true;
			break;
			case TUTORIAL_OBJECT_TYPE_TAP_OBJECT:
				tutorialStep.tapObject.transform.Find ( "tile" ).gameObject.AddComponent < TutorialObjectTapComponent > ();
				tutorialStep.tapObject.transform.Find ( "tile" ).collider.enabled = true;
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;			
			case TUTORIAL_OBJECT_TYPE_JUST_TAP_FIRST_READY_MOM_OBJECT:
			case TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI_THAT_ACTIVATES_WHEN_SHOW_UP:
			case TUTORIAL_OBJECT_TYPE_TAP_ATTACK_BUTTON:
				if ( tutorialStep.closeMoMPanel )
				{
					Destroy ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].myMomPanelControl.gameObject );
				}

				if ( tutorialStep.refillTile != null )
				{
					LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][tutorialStep.refillTile[0]][tutorialStep.refillTile[1]] = GameElements.EMPTY;
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][tutorialStep.refillTile[0]][tutorialStep.refillTile[1]] = null;
					tutorialStep.refillTile = null;
				}
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_TAP_SCREEN:
				tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TutorialFrameTapControl > ().screenTap = true;
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS_WITH_FRAME_TEXT:
			case TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS:
				foreach ( GameObject objectToBeDestroyed in tutorialStep.objectsToBeDestroyed )
				{
					objectToBeDestroyed.transform.Find ( "tile" ).gameObject.AddComponent < TutorialObjectToBeDestoryedComponent > ();
				}
				if ( tutorialUIComboObject != null )
				{
					tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
					tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				}
				break;
			case TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER:
				TutorialMoveCharacterComponent currentTutorialMoveCharacterComponent = tutorialStep.characterObject.transform.Find ( "tile" ).gameObject.AddComponent < TutorialMoveCharacterComponent > ();
				currentTutorialMoveCharacterComponent.ifNoMoveAfterSecShowHand = tutorialStep.ifNoMoveAfterSecShowHand;
				
			print ( currentTutorialMoveCharacterComponent.ifNoMoveAfterSecShowHand + " | " + tutorialStep.ifNoMoveAfterSecShowHand );

			currentTutorialMoveCharacterComponent.targetIsRescueToy = tutorialStep.targetIsRescueToy;
				currentTutorialMoveCharacterComponent.targetTiles = tutorialStep.targetTiles;
				currentTutorialMoveCharacterComponent.showTargetTiles = tutorialStep.showTargetTiles;
				currentTutorialMoveCharacterComponent.startPosition = tutorialStep.startPosition;

				currentTutorialMoveCharacterComponent.targetTiles02 = tutorialStep.targetTiles02;
				currentTutorialMoveCharacterComponent.showTargetTiles02 = tutorialStep.showTargetTiles02;
				currentTutorialMoveCharacterComponent.startPosition02 = tutorialStep.startPosition02;

				_currentHighlightedSelectedComponenents = new List < SelectedComponenent > ();
				foreach ( GameObject toHighlightObject in tutorialStep.objectsToHighLight )
				{
					_currentHighlightedSelectedComponenents.Add ( toHighlightObject.transform.Find ( "tile" ).gameObject.AddComponent < SelectedComponenent > ());
				}
				
				foreach ( SelectedComponenent toHighlightObjectsSelectedComponent in _currentHighlightedSelectedComponenents )
				{
					toHighlightObjectsSelectedComponent.setSelectedForPulsingCharacterMark ( true, 1.5f );
				}

				if ( tutorialStep.dontTurnOnTutorialMode )
				{
					GlobalVariables.TUTORIAL_MENU = false;
				}

				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER_BY_TAP:
				TutorialMoveCharacterByTapComponent currentTutorialMoveCharacterByTapComponent = tutorialStep.characterObject.transform.Find ( "tile" ).gameObject.AddComponent < TutorialMoveCharacterByTapComponent > ();
				currentTutorialMoveCharacterByTapComponent.targetTiles = tutorialStep.targetTiles;
				
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_SELECT_CHARACTER_AND_TAP_OBJECT:
				tutorialStep.characterObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( true );
				tutorialStep.characterObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );

				if ( tutorialStep.tapObject != null )
				{
					TutorialObjectTapComponent currentTutorialObjectTapComponent = null;
					if ( tutorialStep.tapObject.transform.Find ( "tile" ) != null ) currentTutorialObjectTapComponent = tutorialStep.tapObject.transform.Find ( "tile" ).gameObject.AddComponent < TutorialObjectTapComponent > ();
					else currentTutorialObjectTapComponent = tutorialStep.tapObject.AddComponent < TutorialObjectTapComponent > ();

					currentTutorialObjectTapComponent.arrowFromAbove = tutorialStep.arrowFromAbove;
				}
				else
				{
					Destroy ( tutorialUIComboObject );
					TriggerDialogueControl.getInstance ().turnOn ();
					GlobalVariables.TUTORIAL_MENU = false;
				}

				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_TAP_AND_SHOW_OBJECT:
				TutorialObjectTapComponent currentTutorialObjectTapComponent02 = tutorialStep.tapObject.AddComponent < TutorialObjectTapComponent > ();
				currentTutorialObjectTapComponent02.arrowFromAbove = tutorialStep.arrowFromAbove;
				currentTutorialObjectTapComponent02.doNotInteract = true;
				if ( ! tutorialStep.doNoShowTapFrame )
				{
					tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < TutorialFrameTapControl > ();
					tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = true;
					tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;
				}
				else
				{
					tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
					tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				}
				break;
			case TUTORIAL_OBJECT_TYPE_TAP_OBJECT_UI:
				tutorialStep.tapObject.AddComponent < TutorialUIObjectTapComponent > ();
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_TAP_OBJECT_LAB:
				TutorialLaboratoryObjectTapComponent currentTutorialLaboratoryObjectTapComponent = tutorialStep.tapObject.transform.Find ( "tile" ).gameObject.AddComponent < TutorialLaboratoryObjectTapComponent > ();
				currentTutorialLaboratoryObjectTapComponent.arrowFromAbove = tutorialStep.arrowFromAbove;
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				FLMissionScreenMapDialogManager.getInstance ().trunOffButton ();
				break;
			case TUTORIAL_OBJECT_TYPE_DRAG_MOM_OBJECT:
				FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
				FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelected ( false );

				StartCoroutine ( "waitBeforeAddingTutorialScript" );
				
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_BUILD_REDIRECTOR:
				RedirectorButton.getInstance ().currentRedirectorOnLevel.transform.Find ( "tile" ).gameObject.AddComponent < TutorialCheckIfRediretorBuildedComponent > ();
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_DRAG_REDIRECTOR_BEFORE_ROTATE:
				TutorialDragRedirectorBeforeRotateComponenet currentTutorialDragRedirectorBeforeRotateComponenet = RedirectorButton.getInstance ().currentRedirectorOnLevel.transform.Find ( "tile" ).gameObject.AddComponent < TutorialDragRedirectorBeforeRotateComponenet > ();
				currentTutorialDragRedirectorBeforeRotateComponenet.targetTiles = tutorialStep.targetTiles;
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_ROTATE_REDIRECTOR:
				RedirectorButton.getInstance ().currentRedirectorOnLevel.transform.Find ( "tile" ).gameObject.AddComponent < TutorialCheckIfRedirectorRotatedComponent > ();
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_SWIPE_TO_ROOM:
				TutorialChangeRoomComponenet currentTutorialChangeRoomComponenet = tutorialStep.tapObject.AddComponent < TutorialChangeRoomComponenet > ();
				currentTutorialChangeRoomComponenet.targetTiles = tutorialStep.targetTiles;
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				break;
			case TUTORIAL_OBJECT_TYPE_STAY_SIGN_TILL_WIN_LEVEL:
				tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = false;
				tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = false;
				GlobalVariables.TUTORIAL_MENU = false;
				break;
			case TUTORIAL_OBJECT_TYPE_DUMMY:
				if ( tutorialStep.fromTutorial == false )
			    {
					FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
					FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelected ( false );
					
					StartCoroutine ( "waitBeforeAddingTutorialScript" );

					FLGlobalVariables.TUTORIAL_MENU = false;
				}

				GlobalVariables.TUTORIAL_MENU = false;
				break;
			case TUTORIAL_OBJECT_TYPE_CLOSE_MOM_UI:
				FLUIControl.currentSelectedGameElement = null;
				//FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject.transform.Find ( "tile" ).SendMessage ( "ToggleFocus", false );
				print ("This happened now ");
				break;
			//Daves Step
			case TUTORIAL_OBJECT_TYPE_REMOVE_MAP_ARROW:
				if(GameObject.Find ("jumpingArrowMap(Clone)") != null)
				{
					Destroy (GameObject.Find ("jumpingArrowMap(Clone)"));
				}
				break;
			//Daves Step
			default:
				TriggerDialogueControl.getInstance ().turnOn ();
				break;
		}
		
		_currentTutorialUICombo = tutorialUIComboObject;
	}


	private IEnumerator waitBeforeAddingTutorialScript ()
	{
		yield return new WaitForSeconds ( 0.1f );
		iTween.Stop ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject );
		yield return new WaitForSeconds ( 0.4f );
		iTween.Stop ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].momObject );

		TutorialDragMoMObjectComponenet currentTutorialDragMoMObjectComponenet = FLFactoryRoomManager.getInstance ().currentMoMRechargeOCoreObject.AddComponent < TutorialDragMoMObjectComponenet > ();
		currentTutorialDragMoMObjectComponenet.targetTiles = _currentTutorialStep.targetTiles;
		currentTutorialDragMoMObjectComponenet.fromTutorial = _currentTutorialStep.fromTutorial;
 	}
	
	public void setAlphaToZero ( GameObject objectToBeAlphedOut )
	{
		foreach ( Transform child in objectToBeAlphedOut.transform )
		{
			if ( child.renderer != null ) child.renderer.material.color = new Color ( child.renderer.material.color.r, child.renderer.material.color.g, child.renderer.material.color.b, 0f );
			if ( child.GetComponent < TextMesh > () != null )
			{
				foreach ( Transform childInChild in child )
				{
					childInChild.renderer.material.color = new Color ( childInChild.renderer.material.color.r, childInChild.renderer.material.color.g, childInChild.renderer.material.color.b, 0f );
				}
			}
		}
	}
	
	public void disapeareTutorialBox ( GameObject tutorialBoxObject )
	{
		if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && ( TutorialsManager.getInstance ().getCurrentTutorialStep ().dontDestroyThisTutorialBox || TutorialsManager.getInstance ().getCurrentTutorialStep ().preservePreviousTutorialBox )) return;

		foreach ( Transform child in tutorialBoxObject.transform )
		{
			if ( child.GetComponent < AlphaApear > ()) child.GetComponent < AlphaApear > ().startFadeOut ();
			if ( child.GetComponent < TextMesh > () != null )
			{
				foreach ( Transform childInChild in child )
				{
					childInChild.GetComponent < AlphaApear > ().startFadeOut ();
				}
			}
		}
	}
	
	public void goToNextStep ( int numberBack = 0, int skipNumberOfTutorialSteps = 0 )
	{
		GlobalVariables.TUTORIAL_MENU = true;
		if ( numberBack != 0 )
		{
			_currentTutorialStepID -= numberBack;
		}
		else
		{
			if ( skipNumberOfTutorialSteps == 0 )
			{
				if ( ! _currentTutorialStep.dontShowNextStep )
				{
					_currentTutorialStepID++; 
				}
				else
				{
					_currentTutorialStepID++;
					_currentTutorialSteps.Remove ( _currentTutorialSteps[_currentTutorialSteps.Count - 1] );
				}
			}
			else _currentTutorialStepID += ( skipNumberOfTutorialSteps + 1 );
		}

		if ( _currentTutorialStep.turnOffPreviousHeighlightOnEnd )
		{
			foreach ( SelectedComponenent toHighlightObjectsSelectedComponent in _currentHighlightedSelectedComponenents )
			{
				toHighlightObjectsSelectedComponent.setSelectedForTutorial ( false, true  );
			}
		}

		if ( _currentHighlightedSelectedComponenents != null && (( ! _currentTutorialStep.dontDestroyThisTutorialBox && ! _currentTutorialStep.preservePreviousTutorialBox ) || _currentTutorialStep.destroyThisPreservedTutorialBoxOnEnd ))
		{
			foreach ( SelectedComponenent toHighlightObjectsSelectedComponent in _currentHighlightedSelectedComponenents )
			{
				if(toHighlightObjectsSelectedComponent != null)
				{
					toHighlightObjectsSelectedComponent.setSelectedForTutorial ( false, true );
					toHighlightObjectsSelectedComponent.setSelectedForUIHighlight ( false );
				}
			}
		}
		_currentHighlightedSelectedComponenents = null;
		
		_waitUntilThisUIComboIsOff = false;

		if ( objectToBeDestoryedOnNextStep != null )
		{
			Destroy ( objectToBeDestoryedOnNextStep );
		}
	}
	
	public void goToNextStepFully ()
	{
		_currentTutorialUICombo.AddComponent < HideUIElement > ();
		StartCoroutine ( "destroyOnComplete", false );
	}
	
	private IEnumerator destroyOnComplete ()
	{
		yield return new WaitForSeconds ( 0.3f );
		Destroy ( _currentTutorialUICombo );
		goToNextStep ();
	}
	
	public GameObject getCurrentTutorialUICombo ()
	{
		return _currentTutorialUICombo;
	}
	
	public TutorialStepsClass getCurrentTutorialStep ()
	{
		return _currentTutorialStep;
	}
	
	public void invokeRepeatStep ()
	{
		_waitUntilThisUIComboIsOff = true;
		createTutorialObject ( _currentTutorialRepeatSteps[_currentTutorialStepID + 1] );
		_currentTutorialStep = _currentTutorialRepeatSteps[_currentTutorialStepID + 1];
	}
	
	void Update () 
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && ! GlobalVariables.TUTORIAL_MENU ) return;
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && ! FLGlobalVariables.TUTORIAL_MENU ) return;
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && ! MNGlobalVariables.TUTORIAL_MENU) return;

		if ( _countTimeStarted )
		{
			_countTimeToStartTutorialBox -= Time.deltaTime;
			if ( _countTimeToStartTutorialBox <= 0f )
			{
				_countTimeStarted = false;
				_waitUntilSomeTimeBeforeNextStep = true;
				createTutorialObject ( _currentTutorialSteps[_currentTutorialStepID] );
				_currentTutorialStep = _currentTutorialSteps[_currentTutorialStepID];
				
				if ( _currentTutorialStep.type == TUTORIAL_OBJECT_TYPE_DUMMY )
				{
					if ( _currentTutorialStep.fromTutorial )
					{
						TimerControl.getInstance ().startTimer ( true );
					}
				}
				
				StartCoroutine ( "waitUntilSomeTimeBeforeNextStep" );
			}
		}

		if ( ! _waitUntilSomeTimeBeforeNextStep && ! _waitUntilThisUIComboIsOff && _currentTutorialStepID < _currentTutorialSteps.Count && _currentTutorialSteps[_currentTutorialStepID] != _currentTutorialStep )
		{
			if ( ! _countTimeStarted && _currentTutorialSteps[_currentTutorialStepID].delayShowTutorialBox != 0f )
			{
				_countTimeToStartTutorialBox = _currentTutorialSteps[_currentTutorialStepID].delayShowTutorialBox;
				_countTimeStarted = true;
			}
			else if ( _currentTutorialSteps[_currentTutorialStepID].delayShowTutorialBox == 0f )
			{
				_waitUntilSomeTimeBeforeNextStep = true;
				createTutorialObject ( _currentTutorialSteps[_currentTutorialStepID] );
				_currentTutorialStep = _currentTutorialSteps[_currentTutorialStepID];

				if ( _currentTutorialStep.type == TUTORIAL_OBJECT_TYPE_DUMMY )
				{
					if ( _currentTutorialStep.fromTutorial == true )
					{
						TimerControl.getInstance ().startTimer ( true );
					}
				}

				StartCoroutine ( "waitUntilSomeTimeBeforeNextStep" );
			}
		}
		else if ( _currentTutorialStepID >= _currentTutorialSteps.Count )
		{
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
			{
				GlobalVariables.TUTORIAL_MENU = false;
				TimerControl.getInstance ().startTimer ( true );

				TriggerDialogueControl.getInstance ().turnOn ();
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
			{
				if ( _currentTutorialSteps[_currentTutorialSteps.Count - 1].triggerLabVisit02AfterACtions && ! _lab02Visited )
				{
					_lab02Visited = true;
					FLUIControl.getInstance ().triggerActionOnLab02VistTutorial ();
				}
				
				FLGlobalVariables.TUTORIAL_MENU = false;
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
			{
				MNGlobalVariables.TUTORIAL_MENU = false;
				MNSpawningEnemiesManager.getInstance ().startSpawn = true;

				if ( _turnOnTriggerGEneralDialgoButton )
				{
					_turnOnTriggerGEneralDialgoButton = false;
					MNTriggerDialogControl.getInstance ().turnOn ();
				}
			}
		}
	}
	
	private IEnumerator waitUntilSomeTimeBeforeNextStep ()
	{
		yield return new WaitForSeconds ( 0.5f );
		_waitUntilSomeTimeBeforeNextStep = false;
	}
}
