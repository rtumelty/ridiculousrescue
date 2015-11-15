using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLMissionScreenMapDialogManager : MonoBehaviour 
{
	public class DialogStepsClass
	{
		public int repeatSequenceNumberBack;
		public int characterID;
		public string frameTextKey;
		public Vector3 framePosition;
		public int type;
		public GameObject tapObject;
		public bool arrowFromAbove = true;
		public List < GameObject > objectsToBeDestroyed = new List < GameObject > ();
		public List < int[] > targetTiles = new List < int[] > ();
		public List < int[] > showTargetTiles = new List < int[] > ();
		public int[] startPosition;
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
		public GameObject createLockOnIt = null;
		public bool noDialog = false;
		public GameObject unlockThisNode = null;
		public bool noAutomiaticShowDialog = false;
		public GameObject heighlightNode = null;
		public GameObject unHeighlightNode = null;
		public bool onlyForSeenDialog = false;
		public bool rotateToMap02 = false;
		public bool rotateToMap03 = false;
		public float waitTillNextStep = 0f;
		public int customLineLenght = 0;
		public GameObject turnOnAndHighlightThisObject = null;
		public GameObject turnOffAndUNHighlightThisObject = null;
		public GameObject setToInactiveThisObject = null;
	} 
	//*************************************************************//
	public static GameObject CURRENT_LOCKED_LEVEL_NODE = null;
	public static string CURRENT_LOCKED_LEVEL_NODE_NAME = "13";
	public static GameObject CURRENT_MAP_DIALOG_SCREEN;
	//*************************************************************//
	public GameObject[] tutorialComboUIPrefabs;
	public GameObject tutorialUIComboObject;
	public GameObject lockPrefab;
	public Texture2D buttonNormal;
	public Texture2D buttonGrayedOut;
	public Texture2D starNormal;
	public Texture2D starGrayedOut;

	public GameObject lockOnLevel26;
	//*************************************************************//
	private List < DialogStepsClass > _currentDialogs;
	private GameObject _currentDialogObject;
	private int _currentLevel;
	private GameObject _currentLevelNode;
	private GameObject _currentObjectToUnHeighLight;
	//*************************************************************//
	private static FLMissionScreenMapDialogManager _meInstance;
	public static FLMissionScreenMapDialogManager getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		_meInstance = this;

		tutorialComboUIPrefabs = new GameObject[4];
		tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_FULL] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIFull");
		tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_DEMI] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIDemi");
		tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar01");
		tutorialComboUIPrefabs[TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02] = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar02");

		renderer.enabled = false;
		collider.enabled = false;

		if ( SaveDataManager.keyExists ( SaveDataManager.LOCK_ON_LEVEL_DESTROYED_PREFIX + "13" ))
		{
			Destroy ( Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "_clouds" ).gameObject );
		}
	}

	void Update ()
	{
		if ( _currentObjectToUnHeighLight != null )
		{
			if ( _currentObjectToUnHeighLight.GetComponent < SelectedComponenent > ()) _currentObjectToUnHeighLight.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );
			if ( _currentObjectToUnHeighLight.GetComponent < JumpingArrowAbove > ())
			{
				_currentObjectToUnHeighLight.GetComponent < JumpingArrowAbove > ().destroyJumpingArrow ();
				Destroy ( _currentObjectToUnHeighLight.GetComponent < JumpingArrowAbove > ());
				_currentObjectToUnHeighLight = null;
			}
		}
	}

	private void prepareDialogSteps ( int level, bool forTextOnlyrepeat = false )
	{
		DialogStepsClass currentDialog = null;
		switch ( level )
		{
		case 1:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_01_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case 2:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_02_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case 3:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_level_03_dialogue_01_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_03_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_level_03_dialogue_03_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case 4:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_level_04_dialogue_01_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_04_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case -777:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_lab_visit01_dialogue_01_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_lab_visit01_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_lab_visit01_dialogue_03_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case 5:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_05_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_05_dialogue_02_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case -778:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_minelevel_01_dialogue_01_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_minelevel_01_dialogue_02_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case -779:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_labvisit_02_dialogue_01_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case 6:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_06_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			break;
		case 7:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_07_08_09_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.noAutomiaticShowDialog = true;
			_currentDialogs.Add ( currentDialog );
			break;
		case 8:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_07_08_09_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.noAutomiaticShowDialog = true;
			_currentDialogs.Add ( currentDialog );
			break;
		case 9:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_07_08_09_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.noAutomiaticShowDialog = true;
			_currentDialogs.Add ( currentDialog );
			break;
		case 10:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_10_dialogue_01_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			break;
		case 11:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;
			currentDialog.noDialog = true;
			_currentDialogs.Add ( currentDialog );
			break;
		case -780:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_01_dialogue_01_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;
			currentDialog.onlyForSeenDialog = true;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_01_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f ); 
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_01_dialogue_03_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.unlockThisNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_01_dialogue_04_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			break;
		case 12:
			_currentDialogs = new List < DialogStepsClass > ();
			if ( ! forTextOnlyrepeat )
			{
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.waitTillNextStep = 3.5f;
				_currentDialogs.Add ( currentDialog );
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.rotateToMap02 = true;
				currentDialog.waitTillNextStep = 1f;
				_currentDialogs.Add ( currentDialog );
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.waitTillNextStep = 1f;
				_currentDialogs.Add ( currentDialog );
			}

			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_12_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "level12" ).gameObject;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_12_dialogue_02_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_12_dialogue_03_Jose"; 
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_12_dialogue_04_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 13:
			if ( SaveDataManager.keyExists ( SaveDataManager.LOCK_ON_LEVEL_DESTROYED_PREFIX + "13" ))
			{
				_currentDialogs = new List < DialogStepsClass > ();
				currentDialog = new DialogStepsClass ();
				currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
				currentDialog.frameTextKey = "map_level_13_dialogue_01_Jose";
				currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
				currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
				_currentDialogs.Add ( currentDialog );
				currentDialog = new DialogStepsClass ();
				currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
				currentDialog.frameTextKey = "map_level_13_dialogue_02_Faradaydo";
				currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
				currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
				_currentDialogs.Add ( currentDialog );
				currentDialog = new DialogStepsClass ();
				currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
				currentDialog.frameTextKey = "map_level_13_dialogue_03_Jose"; 
				currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
				currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
				_currentDialogs.Add ( currentDialog );
			}
			break;
		case 14:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_14_dialogue_01_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_14_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MADRA_1_IDLE;
			currentDialog.frameTextKey = "map_level_14_dialogue_03_Madra"; 
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 15:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_15_dialogue_01_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_15_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 16:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_16_dialogue_01_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.unHeighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "level16" ).gameObject;
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_16_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.unHeighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "level16" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_16_dialogue_03_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.unHeighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "level16" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			break;
		case 17:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_17_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_17_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.customLineLenght = 45;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_17_dialogue_03_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 18:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_18_dialogue_01_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_18_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case -781:
			_currentDialogs = new List < DialogStepsClass > ();
			if ( ! forTextOnlyrepeat )
			{
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.waitTillNextStep = 3.5f;
				_currentDialogs.Add ( currentDialog );
			}
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_BOZ_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_02_dialogue_01_Boz";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.unlockThisNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_02_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_02_dialogue_03_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 19:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_19_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_19_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 20:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_level_20_dialogue_01_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_20_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_level_20_dialogue_03_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 21:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_21_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_21_dialogue_02_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 22:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_BOZ_1_IDLE;
			currentDialog.frameTextKey = "map_level_22_dialogue_01_Boz";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_22_dialogue_02_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case -782:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MINER_1_IDLE;
			currentDialog.frameTextKey = "map_minelevel_02_dialogue_01_Miner";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_minelevel_02_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_BOZ_1_IDLE;
			currentDialog.frameTextKey = "map_minelevel_02_dialogue_03_Boz";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 23:
			_currentDialogs = new List < DialogStepsClass > ();
			if ( ! forTextOnlyrepeat )
			{
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.waitTillNextStep = 3f;
				_currentDialogs.Add ( currentDialog );
			}
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_MADRA_1_IDLE;
			currentDialog.frameTextKey = "map_level_23_dialogue_01_Madra";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_23_dialogue_02_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 24:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_24_dialogue_01_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train03" ).gameObject;
			currentDialog.turnOnAndHighlightThisObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "jumpingArrowGap" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_BOZ_1_IDLE;
			currentDialog.frameTextKey = "map_level_24_dialogue_02_Boz";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.turnOffAndUNHighlightThisObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "jumpingArrowGap" ).gameObject;
			currentDialog.turnOnAndHighlightThisObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "powerMotor" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_24_dialogue_03_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.turnOffAndUNHighlightThisObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "powerMotor" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			break;
		case 25:
			_currentDialogs = new List < DialogStepsClass > ();
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_25_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.createLockOnIt = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train03" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_level_25_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case -783:
			_currentDialogs = new List < DialogStepsClass > ();
			if ( ! forTextOnlyrepeat )
			{
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.waitTillNextStep = 3.5f;
				_currentDialogs.Add ( currentDialog );
			}
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_03_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			currentDialog.heighlightNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train03" ).gameObject;
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_JOSE_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_03_dialogue_02_Jose";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_trainlevel_03_dialogue_03_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		case 26:
			_currentDialogs = new List < DialogStepsClass > ();
			if ( ! forTextOnlyrepeat )
			{
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.rotateToMap03 = true;
				currentDialog.waitTillNextStep = 1f;
				_currentDialogs.Add ( currentDialog );
				currentDialog = new DialogStepsClass ();
				currentDialog.noDialog = true;
				currentDialog.waitTillNextStep = 1f;
				_currentDialogs.Add ( currentDialog );
			}
			
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_26_dialogue_01_Cora";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.turnOnAndHighlightThisObject = lockOnLevel26;
			currentDialog.setToInactiveThisObject = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "2" ).Find ( "levels" ).Find ( "_clouds" ).Find ( "could01" ).gameObject;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_26_dialogue_02_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_CORA_1_IDLE;
			currentDialog.frameTextKey = "map_level_26_dialogue_03_Cora"; 
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			currentDialog = new DialogStepsClass ();
			currentDialog.characterID = GameElements.CHAR_FARADAYDO_1_IDLE;
			currentDialog.frameTextKey = "map_level_26_dialogue_03_Faradaydo";
			currentDialog.tutorialBoxType = TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02;
			currentDialog.framePosition = new Vector3 ( 0f, 3.8f, 0.11f );
			_currentDialogs.Add ( currentDialog );
			break;
		}
	}

	private void createTutorialBox ( DialogStepsClass tutorialStep )
	{
		if ( ! tutorialStep.noDialog )
		{
			if ( FLUIControl.currentPopupUI != null )
			{
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				
				FLUIControl.currentReservingUIElmentObject = null;
				Destroy ( FLUIControl.currentPopupUI );
			}

			tutorialUIComboObject = ( GameObject ) Instantiate ( tutorialComboUIPrefabs[tutorialStep.tutorialBoxType], Vector3.zero, tutorialComboUIPrefabs[0].transform.rotation );

			CURRENT_MAP_DIALOG_SCREEN = tutorialUIComboObject;

			tutorialUIComboObject.transform.parent = Camera.main.transform;
			tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, tutorialStep.framePosition.y, tutorialStep.framePosition.z ) + Vector3.forward * 3f;
		
			tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = tutorialStep.frameTextKey;

			tutorialUIComboObject.transform.Find ( "frameText" ).position += Vector3.back * 0.3f;
			tutorialUIComboObject.transform.Find ( "frameText" ).localScale *= 0.8f;

			switch ( tutorialStep.tutorialBoxType )
			{
			case TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_01:
			case TutorialsManager.TUTORIAL_COMBO_UI_CHARACTER_02:
				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = 35;
				break;
			case TutorialsManager.TUTORIAL_COMBO_UI_DEMI:
			case TutorialsManager.TUTORIAL_COMBO_UI_FULL:
				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = 40;
				break;
			}

			if ( tutorialStep.customLineLenght != 0 )
			{
				tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().lineLength = tutorialStep.customLineLenght;
			}

			if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
			{
				tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[tutorialStep.characterID];
				if ( tutorialStep.characterID == GameElements.CHAR_CORA_1_IDLE )
				{
					//tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
					//tutorialUIComboObject.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
				}
			}

			tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < DialogFrameTapControl > ();
			tutorialUIComboObject.transform.Find ( "tapText" ).renderer.enabled = true;
			tutorialUIComboObject.transform.Find ( "frame" ).collider.enabled = true;

			TutorialsManager.getInstance ().setAlphaToZero ( tutorialUIComboObject );
		}

		if ( tutorialStep.createLockOnIt != null && tutorialStep.createLockOnIt.transform.Find ( "lock(Clone)" ) == null )
		{
			GameObject lockInstant = ( GameObject ) Instantiate ( lockPrefab, tutorialStep.createLockOnIt.transform.position + Vector3.up * 1f, lockPrefab.transform.rotation );
			lockInstant.AddComponent < TapLockControl > ();
			lockInstant.transform.parent = tutorialStep.createLockOnIt.transform.transform;
		}

		if ( tutorialStep.unlockThisNode != null )
		{
			tutorialStep.unlockThisNode.AddComponent < UnlockLevelSequenceManager > ().forceSequence = true;
			if ( tutorialStep.unlockThisNode.transform.Find ( "lock(Clone)" )) Destroy ( tutorialStep.unlockThisNode.transform.Find ( "lock(Clone)" ).gameObject );
		}

		if ( tutorialStep.turnOnAndHighlightThisObject != null )
		{
			tutorialStep.turnOnAndHighlightThisObject.SetActive ( true );
			if ( tutorialStep.turnOnAndHighlightThisObject.GetComponent < SelectedComponenent > () == null )
			{
				tutorialStep.turnOnAndHighlightThisObject.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
			}
			else 
			{
				tutorialStep.turnOnAndHighlightThisObject.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
			}
		}

		if ( tutorialStep.turnOffAndUNHighlightThisObject != null )
		{
			tutorialStep.turnOffAndUNHighlightThisObject.SetActive ( false );
			tutorialStep.turnOffAndUNHighlightThisObject.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false  );
		}

		if ( tutorialStep.setToInactiveThisObject != null )
		{
			tutorialStep.setToInactiveThisObject.SetActive ( false );
		}

		if ( tutorialStep.heighlightNode != null && ! tutorialStep.onlyForSeenDialog )
		{
			if ( tutorialStep.heighlightNode.GetComponent < SelectedComponenent > () == null )
			{
				tutorialStep.heighlightNode.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
			}
			else 
			{
				tutorialStep.heighlightNode.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
			}
			
			if ( tutorialStep.heighlightNode.GetComponent < JumpingArrowAbove > () == null ) tutorialStep.heighlightNode.AddComponent < JumpingArrowAbove > ().doNotJump = true;
		}

		if ( tutorialStep.unHeighlightNode != null )
		{
			_currentObjectToUnHeighLight = tutorialStep.unHeighlightNode;
		}

		if ( tutorialStep.rotateToMap02 )
		{
			//FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 198 + 1f * 3f, 0f ));
			FLMissionRoomManager.getInstance ().setCurrentWorld ( 1 );
		}

		if ( tutorialStep.rotateToMap03 )
		{
			//FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 198 + 1f * 3f, 0f ));
			FLMissionRoomManager.getInstance ().setCurrentWorld ( 2 );
		}

		if ( tutorialStep.waitTillNextStep != 0f )
		{
			StartCoroutine ( "waitTillnextStep", tutorialStep.waitTillNextStep );
		}
	}

	private IEnumerator waitTillnextStep ( float delay )
	{
		yield return new WaitForSeconds ( delay );
		goToNextStep ();
	}

	public void goToNextStep ()
	{
		if ( _currentDialogs.Count > 0 ) _currentDialogs.RemoveAt ( 0 );
		if ( _currentDialogs.Count > 0 )
		{
			FLGlobalVariables.DIALOG_ON_MAP = true;
			createTutorialBox ( _currentDialogs[0] );
		}
		else
		{
			FLGlobalVariables.DIALOG_ON_MAP = false;

			renderer.enabled = true;
			collider.enabled = true;
			GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1.2f );

			/*
			if ( _currentLevelNode.GetComponent < UnlockLevelSequenceManager > () != null )
			{
				Destroy ( _currentLevelNode.GetComponent < UnlockLevelSequenceManager > ());
				_currentLevelNode.AddComponent < UnlockLevelSequenceManager > ().forceSequence = true;
			}
			else
			{
				_currentLevelNode.AddComponent < UnlockLevelSequenceManager > ().forceSequence = true;
			}
			*/
		}
	}
	
	public void triggerDialog ( int level, GameObject levelNode ) 
	{
		if(levelNode != null)
		{
			PersistThroughSceneComponent.lastActiveNode = levelNode.name;
		}

		_currentLevel = level;
		_currentLevelNode = levelNode;

		prepareDialogSteps ( level );

		if ( ! GameGlobalVariables.CUT_DOWN_GAME && level == -777 )
		{
			levelNode.AddComponent < LabTapedForTutorialControl > ();
		}

		if ( ! GameGlobalVariables.CUT_DOWN_GAME && level == -779 )
		{
			levelNode.AddComponent < LabTapedForTutorial02Control > ();
		}

		if ( level == 13 )
		{
			if ( ! SaveDataManager.keyExists ( SaveDataManager.LOCK_ON_LEVEL_DESTROYED_PREFIX + "13" ))
			{
				GameObject lockInstant = ( GameObject ) Instantiate ( lockPrefab, levelNode.transform.position + Vector3.up * 1f, lockPrefab.transform.rotation );
				lockInstant.AddComponent < TriggerLockScreenControl > ();
				lockInstant.transform.parent = levelNode.transform.transform;
			}
		}

		if ( SaveDataManager.keyExists ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + level.ToString ()))
		{
			if ( _currentDialogs != null && _currentDialogs.Count > 0 )
			{
				if ( ! _currentDialogs[0].noDialog )
				{
					renderer.enabled = true;
					collider.enabled = true;

					GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1.2f );
				}
				//specific for level 12 dialog
				else
				{
					if ( _currentDialogs[0].waitTillNextStep != 0f )
					{
						if ( ! SaveDataManager.keyExists ( SaveDataManager.UNLOCK_LEVEL12_PLAYED ))
						{
							createTutorialBox ( _currentDialogs[0] );
							SaveDataManager.save ( SaveDataManager.UNLOCK_LEVEL12_PLAYED, 1 );
						}
						else
						{
							renderer.enabled = true;
							collider.enabled = true;
							
							GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1.2f );
						}
					}
				}
				//end

				if ( _currentDialogs[0].createLockOnIt != null && _currentDialogs[0].createLockOnIt.transform.Find ( "lock(Clone)" ) == null )
				{
					GameObject lockInstant = ( GameObject ) Instantiate ( lockPrefab, _currentDialogs[0].createLockOnIt.transform.position + Vector3.up * 1f, lockPrefab.transform.rotation );
					lockInstant.AddComponent < TapLockControl > ();
					lockInstant.transform.parent = _currentDialogs[0].createLockOnIt.transform;
				}

				if ( _currentDialogs[0].heighlightNode != null )
				{
					if ( _currentDialogs[0].heighlightNode.GetComponent < SelectedComponenent > () == null )
					{
						_currentDialogs[0].heighlightNode.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
					}
					else 
					{
						_currentDialogs[0].heighlightNode.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
					}

					if ( _currentDialogs[0].heighlightNode.GetComponent < JumpingArrowAbove > () == null ) _currentDialogs[0].heighlightNode.AddComponent < JumpingArrowAbove > ().doNotJump = true;
				}
			}
		}
		else
		{
			renderer.enabled = false;
			collider.enabled = false;

			if ( _currentDialogs != null && _currentDialogs.Count > 0 )
			{
				if ( ! _currentDialogs[0].noAutomiaticShowDialog )
				{
					SaveDataManager.save ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + level.ToString (), 1 );

					createTutorialBox ( _currentDialogs[0] );
				}
				else
				{
					if ( ! _currentDialogs[0].noDialog )
					{
						renderer.enabled = true;
						collider.enabled = true;
						
						GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1.2f );
					}
				}
			}
		}

		if ( level == 1 ) levelNode = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "level01" ).gameObject;
		if ( levelNode != null )
		{
			if ( levelNode.GetComponent < SelectedComponenent > () == null )
			{
				levelNode.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
			}
			else 
			{
				levelNode.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
			}
			if(levelNode.GetComponent <JumpingArrowAbove>() == null)
			{
				levelNode.AddComponent <JumpingArrowAbove>();
			}
		}
		else
		{
			GameObject myNode = GameObject.Find(PersistThroughSceneComponent.lastActiveNode).gameObject;
			print ( "___ " + myNode.name );
			if(myNode.GetComponent < SelectedComponenent > () == null )
			{
				myNode.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
			}
			else
			{
				myNode.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
			}
		}
	}

	void OnMouseUp ()
	{
		if ( FLGlobalVariables.POPUP_UI_SCREEN || FLGlobalVariables.TUTORIAL_MENU ) return;
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		prepareDialogSteps ( _currentLevel, true );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( _currentDialogs != null && _currentDialogs.Count > 0 )
		{
			createTutorialBox ( _currentDialogs[0] );

			renderer.enabled = false;
			collider.enabled = false;
		}
	}

	public void trunOffButton ()
	{
		renderer.enabled = false;
		collider.enabled = false;
	}

	public void trunOnButton ()
	{
		renderer.enabled = true;
		collider.enabled = true;
	}
	
	public void startUnlockingProcedure ()
	{
		FLGlobalVariables.POPUP_UI_SCREEN = false;
		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		if ( FLUIControl.currentPopupUI ) Destroy ( FLUIControl.currentPopupUI );

		FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
		Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
		
		FLUIControl.currentReservingUIElmentObject = null;
		FLUIControl.currentPopupUI = null;
		
		StartCoroutine ( "unlockProcedure" );
	}

	private IEnumerator unlockProcedure ()
	{
		SaveDataManager.save ( SaveDataManager.LOCK_ON_LEVEL_DESTROYED_PREFIX + "13", 1 );
		yield return new WaitForSeconds ( 0.3f );
		foreach ( Transform cloud in Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "_clouds" ))
		{
			int randomPosition = UnityEngine.Random.Range ( 0, 4 );

			iTween.Stop ( cloud.gameObject );
			iTween.MoveTo ( cloud.gameObject, iTween.Hash ( "time", 3.6f, "easetype", iTween.EaseType.easeOutCirc, "position", cloud.position + ( randomPosition == 0 ? Vector3.left * 20f : randomPosition == 1 ? Vector3.right * 20f : randomPosition == 2 ? Vector3.forward * 20f : Vector3.back * 20f )));
		}

		CURRENT_LOCKED_LEVEL_NODE.renderer.enabled = false;
		CURRENT_LOCKED_LEVEL_NODE.transform.Find ( "lock" ).renderer.enabled = false;
		CURRENT_LOCKED_LEVEL_NODE.transform.Find ( "particles" ).gameObject.SetActive ( true );

		yield return new WaitForSeconds ( 1.3f );
		Destroy ( Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "_clouds" ).gameObject );
		CURRENT_LOCKED_LEVEL_NODE.transform.parent.gameObject.AddComponent < UnlockLevelSequenceManager > ();

		Destroy ( CURRENT_LOCKED_LEVEL_NODE.gameObject );
	}
}
