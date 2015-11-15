using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class LevelControl : MonoBehaviour 
{
	public GameObject back02;
	//*************************************************************//
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
		
		return charactersID;
	}
	//*************************************************************//
	public class SpecificLevelData
	{
		public int[] charactersPower = new int[3] { 0, 0, 0 };
		public int redirectors = 0;
	}
	//*************************************************************//
	public static string SELECTED_LEVEL_NAME = "NULL";
	public static int LEVEL_ID = 1;
	public static GameGlobalVariables.Missions.LevelClass CURRENT_LEVEL_CLASS
	{
		set
		{
			_CURRENT_LEVEL_CLASS = value;
		}
		get
		{
			return _CURRENT_LEVEL_CLASS;
		}
	}
	private static GameGlobalVariables.Missions.LevelClass _CURRENT_LEVEL_CLASS;
	public static List < CharacterData > CHARACTERS;
	//*************************************************************//
	public const int NUMBER_OF_LAYERS = 5;
	public const int GRID_LAYER_NORMAL = 0;
	public const int GRID_LAYER_RESCUEZONE = 1;
	public const int GRID_LAYER_REDIRECTORS = 2;
	public const int GRID_LAYER_TIPS = 3;
	public const int GRID_LAYER_BEAM = 4;
	public static int LEVEL_WIDTH = 12; 
	public const int LEVEL_HEIGHT = 8;
	//*************************************************************//
	public bool redirectorsAreNotNeeded = false;
	public Texture2D[] gameElements;
	public int[][][] levelGrid;
	public GameObject[][][] gameElementsOnLevel;
	public List < CharacterData > charactersOnLevel;
	public List < MNEnemyComponent > enemiesOnLevel;
	public List < RedirectorComponent > redirectorsOnLevel;
	public GameObject rescuerOnLevel;
	public GameObject toBeRescuedOnLevel;
	public GameObject coresOnLevel;
	public GameObject flagOnLevel;
	public GameObject lightOnLevel;
	public bool coresAreOnLevel = false;
	//*************************************************************//
	private GameObject _tileInteractivePrefab;
	private GameObject _tileInteractiveTrolleyPrefab;
	private GameObject _tileInteractivePrefab3x1;

	private PrefabPool _tileInteractivePrefabPool;
	private PrefabPool _slugPrefabPool;

	private bool _levelsLoaded = false;
	private SpecificLevelData _currentSpecifcLevelData;
	private List < Texture2D > _backgroundTextures;
	//*************************************************************//	
	private static LevelControl _meInstance;
	public static LevelControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < LevelControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		putAditionalSolidObjectsToTheElementsArray ();
		
		_backgroundTextures = new List < Texture2D > ();
		object[] textureObjects = Resources.LoadAll ( "Textures/Background", typeof ( Texture2D ));
		foreach ( object textureObject in textureObjects )
		{
			if ( textureObject is Texture2D )
			{
				_backgroundTextures.Add (( Texture2D ) textureObject );
			}
		}
		
		_currentSpecifcLevelData = new SpecificLevelData ();
		
		levelGrid = new int[NUMBER_OF_LAYERS][][];
		gameElementsOnLevel = new GameObject[NUMBER_OF_LAYERS][][];
		
		charactersOnLevel = new List < CharacterData > ();
		enemiesOnLevel= new List < MNEnemyComponent > ();
		redirectorsOnLevel = new List < RedirectorComponent > ();
		
		for ( int i = 0; i < NUMBER_OF_LAYERS; i++ )
		{
			levelGrid[i] = new int[LEVEL_WIDTH * 20][];
			gameElementsOnLevel[i] = new GameObject[LEVEL_WIDTH * 20][];
			for ( int j = 0; j < LEVEL_WIDTH * 20; j++ )
			{
				levelGrid[i][j] = new int[LEVEL_HEIGHT];
				gameElementsOnLevel[i][j] = new GameObject[LEVEL_HEIGHT];
			}
		}
		
		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractive" );
		_tileInteractiveTrolleyPrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractiveTrolley" );
		_tileInteractivePrefab3x1 = ( GameObject ) Resources.Load ( "Tile/tileInteractive3x1" );

		_tileInteractivePrefabPool = PrefabPool.GetPool ("_tileInteractivePrefab");
		_slugPrefabPool = PrefabPool.GetPool ("_slugPrefab");

		loadLevel ();
	}

	private void putAditionalSolidObjectsToTheElementsArray ()
	{
		List < Texture2D> solidTextures = new List < Texture2D > ();
		
		object[] textureObjects = Resources.LoadAll ( "Textures/Envi/SolidObjects", typeof ( Texture2D ));
		foreach ( object textureObject in textureObjects )
		{
			if ( textureObject is Texture2D )
			{
				solidTextures.Add (( Texture2D ) textureObject );
			}
		}
		
		textureObjects = Resources.LoadAll ( "Textures/Envi/SolidObjects02", typeof ( Texture2D ));
		foreach ( object textureObject in textureObjects )
		{
			if ( textureObject is Texture2D )
			{
				solidTextures.Add (( Texture2D ) textureObject );
			}
		}
		
		int i = 0;
		foreach ( Texture2D texture in solidTextures )
		{
			LevelControl.getInstance ().gameElements[1000 + i] = texture;
			i++;
		}
	}
	
	private IEnumerator loadLeveltoThisGrid ( string name, int[][] grid )
	{
		GlobalVariables.LOADING_SAVING_MENU = true;
		WWWForm requestForm = new WWWForm ();
		requestForm.AddField ( "mode", "GET" );
		requestForm.AddField ( "NAME", name );
		
		WWW request = new WWW ( "http://serwer1356769.home.pl/toylaz/connection_sql.php", requestForm );
		yield return request;
		
		if ( request.text.Length < 400 )
		{
			loadLevelLocalToThisGrid ( 1, levelGrid[LevelControl.GRID_LAYER_NORMAL] );
			Debug.Log ( "errorLoading" );
		}
		else
		{
			XmlDocument levelXml = new XmlDocument ();
			
			string response = request.text;
			
			levelXml.LoadXml ( WWW.UnEscapeURL ( response ));
			XmlNode levelNode = levelXml.ChildNodes[1].ChildNodes[0];
			XmlNode specifcDataNode = levelXml.ChildNodes[1].ChildNodes[2];
			XmlNode backgroundNode = levelXml.ChildNodes[1].ChildNodes[3];
			
			foreach ( XmlNode tileNode in levelNode.ChildNodes )
			{
				int x = 0;
				int z = 0;
				int tip = 0;
				int ID = 0;
				
				int.TryParse ( tileNode.Attributes[0].InnerText, out x );
				int.TryParse ( tileNode.Attributes[1].InnerText, out z );
				int.TryParse ( tileNode.Attributes[2].InnerText, out tip );
				int.TryParse ( tileNode.InnerText, out ID );
				
				if ( ID != GameElements.EMPTY )
				{
					grid[x][z] = ID;
				}
				
				if ( tip != GameElements.EMPTY ) levelGrid[GRID_LAYER_TIPS][x][z] = tip;
			}
			
			int.TryParse ( specifcDataNode.ChildNodes[0].InnerText, out _currentSpecifcLevelData.charactersPower[0] );
			int.TryParse ( specifcDataNode.ChildNodes[1].InnerText, out _currentSpecifcLevelData.charactersPower[1] );
			int.TryParse ( specifcDataNode.ChildNodes[2].InnerText, out _currentSpecifcLevelData.charactersPower[2] );
			int.TryParse ( specifcDataNode.ChildNodes[3].InnerText, out _currentSpecifcLevelData.redirectors );
			
			foreach ( Texture2D texture in _backgroundTextures )
			{
				if ( texture.name == backgroundNode.InnerText )
				{
					GameObject.Find ( "Background" ).renderer.material.mainTexture = texture;
				}
			}
		
			finalTouchOfLoadingLevels ();
		}
	}

	public CharacterData getCharacterFromPosition ( int[] position )
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( ToolsJerry.compareTiles ( character.position, position ))
			{
				return character;
			}
		}
		
		return null;
	}

	public void loadLevelLocalToThisGrid ( int levelID, int[][] grid )
	{
		XmlDocument levelXml = new XmlDocument ();

		if ( CURRENT_LEVEL_CLASS != null && CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE )
		{
			levelXml.LoadXml ( SelectLevel.ALL_LEVELS[levelID] );
		}
		else if ( CURRENT_LEVEL_CLASS != null && CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE )
		{
			levelXml.LoadXml ( SelectLevel.ALL_BONUS_LEVELS[levelID] );
		}
		else
		{
			levelXml.LoadXml ( SelectLevel.ALL_LEVELS[levelID] );
		}

		XmlNode levelNode = levelXml.ChildNodes[1].ChildNodes[0];
		XmlNode specifcDataNode = levelXml.ChildNodes[1].ChildNodes[2];
		XmlNode backgroundNode = levelXml.ChildNodes[1].ChildNodes[3];

		//Resets default values that were incorrectly climbing.
		LEVEL_WIDTH = 12;
		ZoomAndLevelDrag.MAXIMUM_CAMERA_X = 0;
		MNSpawningEnemiesManager.MAXIMUM_NUMBER_OF_ENEMIES = 10;

		int numberOfInnerSections = int.Parse ( backgroundNode.InnerText );
//		print ( "innersections: " + numberOfInnerSections );
//		print ("WIDTH" + LevelControl.LEVEL_WIDTH);
		LevelControl.LEVEL_WIDTH += 12 * ( numberOfInnerSections - 1 );

//		print ( numberOfInnerSections );
//		print ( LevelControl.LEVEL_WIDTH );

		ZoomAndLevelDrag.MAXIMUM_CAMERA_X += (12f * ( numberOfInnerSections - 1 )) - 12;
		MNSpawningEnemiesManager.MAXIMUM_NUMBER_OF_ENEMIES += 10 * ( numberOfInnerSections - 1 );

		for ( int i = 1; i < numberOfInnerSections; i++ )
		{
			GameObject back02Instance = ( GameObject ) Instantiate ( back02, new Vector3 ( 5.5f + 12f * i, 0f + 0.01f * i, 3.5f ), back02.transform.rotation );
		}

		foreach ( XmlNode tileNode in levelNode.ChildNodes )
		{
			int x = 0;
			int z = 0;
			int tip = 0;
			int ID = 0;
			
			int.TryParse ( tileNode.Attributes[0].InnerText, out x );
			int.TryParse ( tileNode.Attributes[1].InnerText, out z );
			int.TryParse ( tileNode.Attributes[2].InnerText, out tip );
			int.TryParse ( tileNode.InnerText, out ID );
			
			if ( ID != GameElements.EMPTY )
			{
				grid[x][z] = ID;
			}
			
			if ( tip != GameElements.EMPTY ) levelGrid[GRID_LAYER_TIPS][x][z] = tip;
		}
	
		int.TryParse ( specifcDataNode.ChildNodes[0].InnerText, out _currentSpecifcLevelData.charactersPower[0] );
		int.TryParse ( specifcDataNode.ChildNodes[1].InnerText, out _currentSpecifcLevelData.charactersPower[1] );
		int.TryParse ( specifcDataNode.ChildNodes[2].InnerText, out _currentSpecifcLevelData.charactersPower[2] );
		int.TryParse ( specifcDataNode.ChildNodes[3].InnerText, out _currentSpecifcLevelData.redirectors );

		finalTouchOfLoadingLevels ();
	}
	
	private void loadLevel ()
	{
#if UNITY_EDITOR
		//LEVEL_ID = 4;
#endif
		//*************************************************************//
		// LEVEL SELECTED
		//*************************************************************//
		if ( SELECTED_LEVEL_NAME != "NULL" )
		{	
			int selectedLevelID = 0;
			if ( int.TryParse ( SELECTED_LEVEL_NAME, out selectedLevelID ))
			{
				loadLevelLocalToThisGrid ( selectedLevelID, levelGrid[LevelControl.GRID_LAYER_NORMAL] );
			}
			else
			{
				StartCoroutine ( loadLeveltoThisGrid ( SELECTED_LEVEL_NAME, levelGrid[LevelControl.GRID_LAYER_NORMAL] ));
			}
		}
		else
		{
			string levelName = LEVEL_ID.ToString ();
			
			if ( GameGlobalVariables.RELEASE ) loadLevelLocalToThisGrid ( LEVEL_ID, levelGrid[LevelControl.GRID_LAYER_NORMAL] );
			else StartCoroutine ( loadLeveltoThisGrid ( levelName, levelGrid[LevelControl.GRID_LAYER_NORMAL] ));
		}
	}
	
	private void finalTouchOfLoadingLevels ()
	{
		if ( _levelsLoaded ) return;
		
		if ( CURRENT_LEVEL_CLASS != null )
		{
			UIControl.getInstance ().setTheMoves ();
			CURRENT_LEVEL_CLASS.mySpecificLevelData = _currentSpecifcLevelData;
		}
		if ( CHARACTERS == null )
		{
			CHARACTERS = new List < CharacterData > ();
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_CORA_1_IDLE, "Cora", -1, -1, 5, 1, 1, 1, 0, 0, 0, 0, 1 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MADRA_1_IDLE, "Madra", -1, -1, 9, 3, 1, 1, 3, 0, 0, 0, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_FARADAYDO_1_IDLE, "Faradaydo", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_1_IDLE, "Miner 1", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_2_IDLE, "Miner 2", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_3_IDLE, "Miner 3", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_4_IDLE, "Miner 4", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_5_IDLE, "Miner 5", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_6_IDLE, "Miner 6", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_JOSE_1_IDLE, "Jose", -1, -1, 5, 0, 1, 1, 0, 2, 2, 2, 0, 50 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_BOZ_1_IDLE, "Boz", -1, -1, 5, 0, 1, 1, 0, 2, 2, 2, 0, 50 ));
		}
		
		_levelsLoaded = true;

		fillLevelWithAssets ();
		
		sortCharactersOnLevelToMakeFaradayadoFirst ();
		setPowerToCharacters ();
		GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT = _currentSpecifcLevelData.redirectors;
		GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT += GameGlobalVariables.AdditionalMoves.ADDITIONAL_MOVES;

		UIControl.getInstance ().createButtonsCharacters ( charactersOnLevel );

		if ( LevelControl.CURRENT_LEVEL_CLASS == null )
		{
			GameGlobalVariables.Missions.fillWorldsAndLeveles ();
			if ( LevelControl.LEVEL_ID > 0 && LevelControl.LEVEL_ID < 12 ) LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[0].levels[LevelControl.LEVEL_ID - 1];
			else if ( LevelControl.LEVEL_ID >= 12 ) LevelControl.CURRENT_LEVEL_CLASS = GameGlobalVariables.Missions.WORLDS[1].levels[LevelControl.LEVEL_ID - 1];
		}

		if ( LevelControl.LEVEL_ID != 1 || LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE )
		{
			//OpeningSequenceManger.getInstance ().destroy ();
			StartSequenceManager.getInstance ().startStartSequence ();
		}
		else
		{
			//OpeningSequenceManger.getInstance ().startOpeningSequence ();
		}

		if ( LEVEL_ID <= 3 )
		{
			MNSpawningEnemiesManager.getInstance ().startSpawn = false;
		}

		GoogleAnalytics.instance.LogScreen ( "Rescue - Start - Level " + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.SELECTED_LEVEL_NAME );

		MNSpawningEnemiesManager.getInstance ().updateHolesOnLevel ();

		GlobalVariables.LOADING_SAVING_MENU = false;

		GameGlobalVariables.AdditionalMoves.reset ();
	}	
	
	private void fillLevelWithAssets ()
	{
		//fill the rescue zone

		levelGrid[GRID_LAYER_RESCUEZONE][0][0] = GameElements.ENVI_RESCUEZONE_1;
		levelGrid[GRID_LAYER_RESCUEZONE][1][0] = GameElements.ENVI_RESCUEZONE_1;
		levelGrid[GRID_LAYER_RESCUEZONE][0][1] = GameElements.ENVI_RESCUEZONE_1;
		levelGrid[GRID_LAYER_RESCUEZONE][1][1] = GameElements.ENVI_RESCUEZONE_1;
		levelGrid[GRID_LAYER_RESCUEZONE][0][2] = GameElements.ENVI_RESCUEZONE_1;
		levelGrid[GRID_LAYER_RESCUEZONE][1][2] = GameElements.ENVI_RESCUEZONE_1;

		coresAreOnLevel = false;
		for ( int i = 0; i < LEVEL_WIDTH; i++ )
		{
			for ( int j = 0; j < LEVEL_HEIGHT; j++ )
			{
				if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_MADRA_1_IDLE )
				{
					GameObject madraPrefab = ( GameObject ) Resources.Load ( "Spine/Madra/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( madraPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), madraPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					foreach ( CharacterData characterData in CHARACTERS )
					{
						if ( characterData.myID == levelGrid[GRID_LAYER_NORMAL][i][j] )
						{
							charactersOnLevel.Add ( characterData.getClone ());
							charactersOnLevel[charactersOnLevel.Count - 1].position[0] = i;
							charactersOnLevel[charactersOnLevel.Count - 1].position[1] = j;
							
							currentIComponent.myCharacterData = charactersOnLevel[charactersOnLevel.Count - 1];
							
							interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
							interactiveObjectInstant.AddComponent < MoveComponent > ();
							interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
							interactiveObjectMesh.AddComponent < CharacterReloactionComponent > ();
							
							if ( charactersOnLevel[charactersOnLevel.Count - 1].characterValues[CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE] > 0 )
							{
								interactiveObjectMesh.AddComponent < AttackerComponent > ();
							}
							
							interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
							
							if ( Array.IndexOf ( GameElements.RESCUERS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
							{
								interactiveObjectMesh.AddComponent < RescuerComponent > ();
								rescuerOnLevel = interactiveObjectInstant;
							}
						}
					}
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_MADRA_1_DRAINED )
				{
					GameObject madraPrefab = ( GameObject ) Resources.Load ( "Spine/Madra/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( madraPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), madraPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					interactiveObjectMesh.AddComponent < ToBeRescuedComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					
					toBeRescuedOnLevel = interactiveObjectInstant;
					currentIComponent.myCharacterData = getCharacter ( levelGrid[GRID_LAYER_NORMAL][i][j] );

					/*GameObject flagObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ) - 0.5f, ( float ) j + 0.7f - 0.5f ), _tileInteractivePrefab.transform.rotation );
					flagObject.transform.Find ( "tile" ).transform.localScale = Vector3.one * 1.5f;
					flagObject.transform.Find ( "tile" ).transform.localPosition = Vector3.back * 0.32f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < RescueFlagComponent > ();
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().enabled = true;
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().size *= 0.7f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < IComponent > ();
					flagObject.transform.Find ( "tile" ).gameObject.GetComponent < IComponent > ().position = new int[2] { i, j };
					
					flagOnLevel = flagObject;

					StaticObjectWithAnimation currentStaticObjectWithAnimation = flagObject.transform.Find ( "tile" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
					currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Whiteflag" );*/
					
					if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) ResoultScreen.TO_BE_RESCUED_TOY_NAME = currentIComponent.myCharacterData.name;
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_FARADAYDO_1_IDLE )
				{
					GameObject faradaydoPrefab = ( GameObject ) Resources.Load ( "Spine/Fara/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( faradaydoPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), faradaydoPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;

					interactiveObjectMesh.GetComponent < BoxCollider > ().size = new Vector3 ( 1f, 1.45f, 1f );
					interactiveObjectMesh.GetComponent < BoxCollider > ().center = new Vector3 ( 0f, 0.95f, 00f );

					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };

					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					foreach ( CharacterData characterData in CHARACTERS )
					{
						if ( characterData.myID == levelGrid[GRID_LAYER_NORMAL][i][j] )
						{
							charactersOnLevel.Add ( characterData.getClone ());
							charactersOnLevel[charactersOnLevel.Count - 1].position[0] = i;
							charactersOnLevel[charactersOnLevel.Count - 1].position[1] = j;
							
							currentIComponent.myCharacterData = charactersOnLevel[charactersOnLevel.Count - 1];
							
							interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
							interactiveObjectInstant.AddComponent < MoveComponent > ();
							interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
							interactiveObjectMesh.AddComponent < CharacterReloactionComponent > ();
							
							if ( charactersOnLevel[charactersOnLevel.Count - 1].characterValues[CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE] > 0 )
							{
								interactiveObjectMesh.AddComponent < AttackerComponent > ();
							}
							
							interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
							
							if ( Array.IndexOf ( GameElements.RESCUERS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
							{
								interactiveObjectMesh.AddComponent < RescuerComponent > ();
								rescuerOnLevel = interactiveObjectInstant;
							}
						}
					}

				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_FARADAYDO_1_DRAINED )
				{
					GameObject faradaydoPrefab = ( GameObject ) Resources.Load ( "Spine/Fara/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( faradaydoPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), faradaydoPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;

					interactiveObjectMesh.AddComponent < ToBeRescuedComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					
					toBeRescuedOnLevel = interactiveObjectInstant;
					currentIComponent.myCharacterData = getCharacter ( levelGrid[GRID_LAYER_NORMAL][i][j] );

					/*GameObject flagObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ) - 0.5f, ( float ) j + 0.7f - 0.5f ), _tileInteractivePrefab.transform.rotation );
					flagObject.transform.Find ( "tile" ).transform.localScale = Vector3.one * 1.5f;
					flagObject.transform.Find ( "tile" ).transform.localPosition = Vector3.back * 0.32f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < RescueFlagComponent > ();
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().enabled = true;
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().size *= 0.7f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < IComponent > ();
					flagObject.transform.Find ( "tile" ).gameObject.GetComponent < IComponent > ().position = new int[2] { i, j };
					
					flagOnLevel = flagObject;

					StaticObjectWithAnimation currentStaticObjectWithAnimation = flagObject.transform.Find ( "tile" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
					currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Whiteflag" );*/

					if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) ResoultScreen.TO_BE_RESCUED_TOY_NAME = currentIComponent.myCharacterData.name;
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_JOSE_IDLE_DRAINED )
				{
					GameObject josePrefab = ( GameObject ) Resources.Load ( "Spine/Jose/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( josePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), josePrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					interactiveObjectMesh.AddComponent < ToBeRescuedComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					
					toBeRescuedOnLevel = interactiveObjectInstant;
					currentIComponent.myCharacterData = getCharacter ( levelGrid[GRID_LAYER_NORMAL][i][j] );
					
					/*GameObject flagObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ) - 0.5f, ( float ) j + 0.7f - 0.5f ), _tileInteractivePrefab.transform.rotation );
					flagObject.transform.Find ( "tile" ).transform.localScale = Vector3.one * 1.5f;
					flagObject.transform.Find ( "tile" ).transform.localPosition = Vector3.back * 0.32f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < RescueFlagComponent > ();
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().enabled = true;
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().size *= 0.7f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < IComponent > ();
					flagObject.transform.Find ( "tile" ).gameObject.GetComponent < IComponent > ().position = new int[2] { i, j };
					
					flagOnLevel = flagObject;
					
					StaticObjectWithAnimation currentStaticObjectWithAnimation = flagObject.transform.Find ( "tile" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
					currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Whiteflag" );*/
					
					if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) ResoultScreen.TO_BE_RESCUED_TOY_NAME = currentIComponent.myCharacterData.name;
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_CORA_1_IDLE )
				{
					GameObject coraPrefab = ( GameObject ) Resources.Load ( "Spine/Cora/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( coraPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), coraPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					foreach ( CharacterData characterData in CHARACTERS )
					{
						if ( characterData.myID == levelGrid[GRID_LAYER_NORMAL][i][j] )
						{
							charactersOnLevel.Add ( characterData.getClone ());
							charactersOnLevel[charactersOnLevel.Count - 1].position[0] = i;
							charactersOnLevel[charactersOnLevel.Count - 1].position[1] = j;
							
							currentIComponent.myCharacterData = charactersOnLevel[charactersOnLevel.Count - 1];
							
							interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
							interactiveObjectInstant.AddComponent < MoveComponent > ();
							interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
							interactiveObjectMesh.AddComponent < CharacterReloactionComponent > ();
							
							if ( charactersOnLevel[charactersOnLevel.Count - 1].characterValues[CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE] > 0 )
							{
								interactiveObjectMesh.AddComponent < AttackerComponent > ();
							}
							
							interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
							
							if ( Array.IndexOf ( GameElements.RESCUERS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
							{
								interactiveObjectMesh.AddComponent < RescuerComponent > ();
								rescuerOnLevel = interactiveObjectInstant;
							}
						}
					}
				}
				else if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS_DRAINED, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1  )
				{
					GameObject minerPrefab = ( GameObject ) Resources.Load ( "Spine/Miner01/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( minerPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), minerPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					interactiveObjectMesh.AddComponent < ToBeRescuedComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					
					toBeRescuedOnLevel = interactiveObjectInstant;
					currentIComponent.myCharacterData = getCharacter ( levelGrid[GRID_LAYER_NORMAL][i][j] );
					
					/*GameObject flagObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ) - 0.5f, ( float ) j + 0.7f - 0.5f ), _tileInteractivePrefab.transform.rotation );
					flagObject.transform.Find ( "tile" ).transform.localScale = Vector3.one * 1.5f;
					flagObject.transform.Find ( "tile" ).transform.localPosition = Vector3.back * 0.32f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < RescueFlagComponent > ();
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().enabled = true;
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().size *= 0.7f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < IComponent > ();
					flagObject.transform.Find ( "tile" ).gameObject.GetComponent < IComponent > ().position = new int[2] { i, j };
					
					flagOnLevel = flagObject;
					
					StaticObjectWithAnimation currentStaticObjectWithAnimation = flagObject.transform.Find ( "tile" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
					currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Whiteflag" );*/
					
					if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) ResoultScreen.TO_BE_RESCUED_TOY_NAME = currentIComponent.myCharacterData.name;
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_BOZ_IDLE_DRAINED )
				{
					GameObject bozPrefab = ( GameObject ) Resources.Load ( "Spine/Boz/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( bozPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), bozPrefab.transform.rotation );
					GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					currentIComponent.position = new int[2] { i, j };
					
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					interactiveObjectMesh.AddComponent < ToBeRescuedComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					
					toBeRescuedOnLevel = interactiveObjectInstant;
					currentIComponent.myCharacterData = getCharacter ( levelGrid[GRID_LAYER_NORMAL][i][j] );
					
					/*GameObject flagObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ) - 0.5f, ( float ) j + 0.7f - 0.5f ), _tileInteractivePrefab.transform.rotation );
					flagObject.transform.Find ( "tile" ).transform.localScale = Vector3.one * 1.5f;
					flagObject.transform.Find ( "tile" ).transform.localPosition = Vector3.back * 0.32f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < RescueFlagComponent > ();
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().enabled = true;
					flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().size *= 0.7f;
					flagObject.transform.Find ( "tile" ).gameObject.AddComponent < IComponent > ();
					flagObject.transform.Find ( "tile" ).gameObject.GetComponent < IComponent > ().position = new int[2] { i, j };
					
					flagOnLevel = flagObject;
					
					StaticObjectWithAnimation currentStaticObjectWithAnimation = flagObject.transform.Find ( "tile" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
					currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Whiteflag" );*/
					
					if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) ResoultScreen.TO_BE_RESCUED_TOY_NAME = currentIComponent.myCharacterData.name;
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.EMPTY && levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.UNWALKABLE )
				{
					GameObject interactiveObjectInstant = null;
					GameObject interactiveObjectMesh = null;
					if ( Array.IndexOf ( GameElements.TWO_TILES_OBJECTS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), _tileInteractivePrefab.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
						interactiveObjectMesh.transform.localScale = new Vector3 ( interactiveObjectMesh.transform.localScale.x, interactiveObjectMesh.transform.localScale.y, 2f );
						interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x, interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f );
					}
					else if ( GameElements.isInBorders ( levelGrid[GRID_LAYER_NORMAL][i][j], GameElements.ONE_TILES_OBJECTS_BORDER ))
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), _tileInteractivePrefab.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
						interactiveObjectMesh.transform.localScale = Vector3.one;
						interactiveObjectMesh.transform.localPosition = Vector3.zero + Vector3.back * 0.5f;
					}
					else if ( Array.IndexOf ( GameElements.SIZE_3X1, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab3x1, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), _tileInteractivePrefab3x1.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					}
					else if ( Array.IndexOf ( GameElements.TWO_BY_TWO_TILES_OBJECTS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), _tileInteractivePrefab.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
						interactiveObjectMesh.transform.localScale = Vector3.one * 2f;
						interactiveObjectMesh.transform.localPosition = Vector3.zero + Vector3.back * 1f;
						interactiveObjectMesh.GetComponent < BoxCollider > ().size *= 0.8f;
					}
					else if ( Array.IndexOf ( GameElements.THREE_BY_THREE_TILES_OBJECTS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), _tileInteractivePrefab.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
						interactiveObjectMesh.transform.localScale = Vector3.one * 3f;
						interactiveObjectMesh.transform.localPosition = Vector3.zero + Vector3.back * 1.5f;
						interactiveObjectMesh.GetComponent < BoxCollider > ().size *= 0.55f;
						interactiveObjectMesh.GetComponent < BoxCollider > ().center += Vector3.forward * 0.2f;
						
						if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_CORA_1_IDLE )
						{
							GameObject secondInteractiveObjectMesh = ( GameObject ) Instantiate ( interactiveObjectMesh, interactiveObjectMesh.transform.position, interactiveObjectMesh.transform.rotation );
							secondInteractiveObjectMesh.collider.enabled = false;
							secondInteractiveObjectMesh.name = "troleyBack";
							secondInteractiveObjectMesh.transform.parent = interactiveObjectMesh.transform;
							secondInteractiveObjectMesh.transform.localPosition = new Vector3 ( 0f, -0.5f, 0f );
							secondInteractiveObjectMesh.renderer.enabled = false;
						}
					}
					else
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - .5f ), _tileInteractivePrefab.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
					}
					
					interactiveObjectMesh.tag = GlobalVariables.Tags.INTERACTIVE;
					
					interactiveObjectMesh.renderer.material.mainTexture = gameElements[levelGrid[GRID_LAYER_NORMAL][i][j]];
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					
					if ( Array.IndexOf ( GameElements.SIZE_3X1, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						currentIComponent.aditionalTiles = new int[2][];
						currentIComponent.aditionalTiles[0] = new int[2] { 1, 0 };
						currentIComponent.aditionalTiles[1] = new int[2] { 2, 0 };
						foreach ( int[] additionalTile in currentIComponent.aditionalTiles )
						{
							levelGrid[GRID_LAYER_NORMAL][i + additionalTile[0]][j + additionalTile[1]] = GameElements.UNWALKABLE;
							gameElementsOnLevel[GRID_LAYER_NORMAL][i + additionalTile[0]][j + additionalTile[1]] = interactiveObjectInstant;
						}
					}
					
					// if electirity passes object and you can wlak on it, just remove it from registers and put it a little bit lower
					if ( GameElements.isInBorders ( levelGrid[GRID_LAYER_NORMAL][i][j], GameElements.WALKABLE_BORDERS ) && GameElements.isInBorders ( levelGrid[GRID_LAYER_NORMAL][i][j], GameElements.ELECTRICITY_PASSES_BORDERS ))
					{
						levelGrid[GRID_LAYER_NORMAL][i][j] = GameElements.EMPTY;
						gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = null;
						interactiveObjectInstant.transform.position += Vector3.down * 0.9f;
					}
					
					if ( GameElements.isInBorders ( levelGrid[GRID_LAYER_NORMAL][i][j], GameElements.ELECTRICITY_PASSES_BORDERS ))
					{
						interactiveObjectInstant.transform.position += Vector3.down * 0.9f;
					}
						
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					
					if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.ENVI_CORES_01 )
					{
						coresAreOnLevel = true;
						
						coresOnLevel = interactiveObjectInstant;
						levelGrid[GRID_LAYER_REDIRECTORS][i][j] = levelGrid[GRID_LAYER_NORMAL][i][j];
						gameElementsOnLevel[GRID_LAYER_REDIRECTORS][i][j] = gameElementsOnLevel[GRID_LAYER_NORMAL][i][j];
						
						CoresComponent currentCoresComponent = interactiveObjectMesh.AddComponent < CoresComponent > ();
						currentCoresComponent.connected = true;
						currentCoresComponent.isCores = true;
						
						interactiveObjectMesh.AddComponent < SelectedComponenent > ();
						
						//TipOnClickComponent currentTipOnClickComponent = interactiveObjectMesh.AddComponent < TipOnClickComponent > ();
						//currentTipOnClickComponent.myTipTexture = gameElements[GameElements.UI_TIP_BUILDER];
						//currentTipOnClickComponent.tipID = GameElements.UI_TIP_BUILDER;
					}
					else if ( Array.IndexOf ( GameElements.REDIRECTORS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						levelGrid[GRID_LAYER_REDIRECTORS][i][j] = levelGrid[GRID_LAYER_NORMAL][i][j];
						gameElementsOnLevel[GRID_LAYER_REDIRECTORS][i][j] = gameElementsOnLevel[GRID_LAYER_NORMAL][i][j];
						levelGrid[GRID_LAYER_NORMAL][i][j] = GameElements.EMPTY;
						gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = null;
						
						interactiveObjectMesh.transform.localScale = Vector3.one;
						interactiveObjectMesh.transform.localPosition = Vector3.zero + Vector3.back * 0.5f;
						
						interactiveObjectInstant.name += "_REDIRECTOR";
						interactiveObjectInstant.transform.position += Vector3.down * 0.5f;
						RedirectorComponent currentRedirectorComponent = interactiveObjectMesh.AddComponent < RedirectorComponent > ();
						currentRedirectorComponent.forceBeingBuild ();
						redirectorsOnLevel.Add ( currentRedirectorComponent );
					}
					else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.ENVI_POWERPLUG_01 )
					{
						levelGrid[GRID_LAYER_REDIRECTORS][i][j] = levelGrid[GRID_LAYER_NORMAL][i][j];
						gameElementsOnLevel[GRID_LAYER_REDIRECTORS][i][j] = gameElementsOnLevel[GRID_LAYER_NORMAL][i][j];

						lightOnLevel = interactiveObjectInstant;

						PlugComponent currentPlugComponent = interactiveObjectMesh.AddComponent < PlugComponent > ();
						currentPlugComponent.connected = false;
						
						StaticObjectWithAnimation currentStaticObjectWithAnimation = interactiveObjectMesh.AddComponent < StaticObjectWithAnimation > ();
						currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Envi/Energy/PowerPlugAnimation", true );
					}
					else if ( Array.IndexOf ( GameElements.DESTROYABLE_OBJECTS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectMesh.collider.enabled = true;
						interactiveObjectMesh.AddComponent < DestroyableObjectComponent > ();
						interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					}
					if ( Array.IndexOf ( GameElements.DESTROYABLE_OBJECTS_RESOURCES, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectMesh.collider.enabled = true;
						interactiveObjectMesh.AddComponent < MNDestroyableObjectComponent > ();
						interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					}
					else if ( Array.IndexOf ( GameElements.ENEMIES, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 && levelGrid[GRID_LAYER_NORMAL][i][j] != 77 )
					{
						interactiveObjectMesh.transform.localScale = new Vector3 ( 2f, interactiveObjectMesh.transform.localScale.y, interactiveObjectMesh.transform.localScale.z );
						interactiveObjectMesh.GetComponent < BoxCollider > ().size = new Vector3 ( 0.45f, 1f, 0.48f );
						interactiveObjectMesh.GetComponent < BoxCollider > ().center = new Vector3 ( -0.02f, 0f, 0.25f );

						interactiveObjectMesh.collider.enabled = true;
						interactiveObjectMesh.AddComponent < SelectedComponenent > ();
						MNEnemyComponent currentEnemyComponent = interactiveObjectMesh.AddComponent < MNEnemyComponent > ();
						interactiveObjectMesh.AddComponent < EnemyAnimationComponent > ();
						enemiesOnLevel.Add ( currentEnemyComponent );
						
						switch ( levelGrid[GRID_LAYER_NORMAL][i][j] )
						{
							case GameElements.ENEM_TENTACLEDRAINER_01:
							case GameElements.ENEM_TENTACLEDRAINER_02:
								GameObject rockObjectPrefab = ( GameObject ) Resources.Load ( "Tile/tentacleRock" );
								GameObject rockObject = ( GameObject ) Instantiate ( rockObjectPrefab, interactiveObjectInstant.transform.position + Vector3.up * 0.25f + Vector3.forward * 1f, rockObjectPrefab.transform.rotation );
								rockObject.AddComponent < EnemyTentacleRockControl > ().parentGameObject = interactiveObjectInstant;
								interactiveObjectMesh.transform.localPosition = new Vector3 ( 0f, 0f, -1.1f );
								currentEnemyComponent.myEnemyData = new EnemyData ( levelGrid[GRID_LAYER_NORMAL][i][j], "Tentacle Drainer", 3, 0, 0, 1, 2, 2, 2 );
								currentIComponent.myEnemyData = currentEnemyComponent.myEnemyData;
								break;
						}
					}
					else if ( Array.IndexOf ( GameElements.TO_BE_RESCUED, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						/*
						TipOnClickComponent currentTipOnClickComponent = interactiveObjectMesh.AddComponent < TipOnClickComponent > ();
						currentTipOnClickComponent.myTipTexture = gameElements[GameElements.UI_TIP_RESCUER];
						currentTipOnClickComponent.tipID = GameElements.UI_TIP_RESCUER;
						*/

						interactiveObjectMesh.AddComponent < ToBeRescuedComponent > ();
						interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
						if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) interactiveObjectMesh.transform.localScale = Vector3.one * 2f ;
						interactiveObjectMesh.AddComponent < SelectedComponenent > ();

						if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) interactiveObjectMesh.GetComponent < BoxCollider > ().center = new Vector3 ( 0f, 0f, 0.16f );
						if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) interactiveObjectMesh.GetComponent < BoxCollider > ().size = new Vector3 ( 0.55f, 1f, 0.55f );

						toBeRescuedOnLevel = interactiveObjectInstant;
						currentIComponent.myCharacterData = getCharacter ( levelGrid[GRID_LAYER_NORMAL][i][j] );

						if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.POWER_MOTOR ) ResoultScreen.TO_BE_RESCUED_TOY_NAME = currentIComponent.myCharacterData.name;
					
						/*GameObject flagObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ) - 0.5f, ( float ) j + 0.7f - 0.5f ), _tileInteractivePrefab.transform.rotation );
						flagObject.transform.Find ( "tile" ).transform.localScale = Vector3.one * 1.5f;
						flagObject.transform.Find ( "tile" ).transform.localPosition = Vector3.back * 0.32f;
						flagObject.transform.Find ( "tile" ).gameObject.AddComponent < RescueFlagComponent > ();
						flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().enabled = true;
						flagObject.transform.Find ( "tile" ).GetComponent < BoxCollider > ().size *= 0.7f;
						flagObject.transform.Find ( "tile" ).gameObject.AddComponent < IComponent > ();
						flagObject.transform.Find ( "tile" ).gameObject.GetComponent < IComponent > ().position = new int[2] { i, j };
					
						flagOnLevel = flagObject;

						StaticObjectWithAnimation currentStaticObjectWithAnimation = flagObject.transform.Find ( "tile" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
						currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Whiteflag" );*/
					}
					else if ( Array.IndexOf ( GameElements.TROLEYS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectMesh.transform.localScale = Vector3.one;
					}
					else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.ENVI_RESCUEZONE_1 )
					{
						levelGrid[GRID_LAYER_RESCUEZONE][i][j] = levelGrid[GRID_LAYER_NORMAL][i][j];
						gameElementsOnLevel[GRID_LAYER_RESCUEZONE][i][j] = gameElementsOnLevel[GRID_LAYER_NORMAL][i][j];
						
						levelGrid[GRID_LAYER_NORMAL][i][j] = GameElements.EMPTY;
						gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = null;
						
						interactiveObjectInstant.transform.position += Vector3.down * 0.9f;
					}
					
					if ( Array.IndexOf ( GameElements.CHARACTERS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectMesh.collider.enabled = true;
						interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
						
						foreach ( CharacterData characterData in CHARACTERS )
						{
							if ( characterData.myID == levelGrid[GRID_LAYER_NORMAL][i][j] )
							{
								charactersOnLevel.Add ( characterData.getClone ());
								charactersOnLevel[charactersOnLevel.Count - 1].position[0] = i;
								charactersOnLevel[charactersOnLevel.Count - 1].position[1] = j;
								
								currentIComponent.myCharacterData = charactersOnLevel[charactersOnLevel.Count - 1];
								
								interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
								interactiveObjectInstant.AddComponent < MoveComponent > ();
								interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
								interactiveObjectMesh.AddComponent < CharacterReloactionComponent > ();
								
								if ( charactersOnLevel[charactersOnLevel.Count - 1].characterValues[CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE] > 0 )
								{
									interactiveObjectMesh.AddComponent < AttackerComponent > ();
								}
								
								interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
								
								if ( Array.IndexOf ( GameElements.RESCUERS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
								{
									interactiveObjectMesh.AddComponent < RescuerComponent > ();
									rescuerOnLevel = interactiveObjectInstant;
								}
							}
						}
					}
				}
			}
		}		
		
		if ( ! coresAreOnLevel ) GlobalVariables.PLUG_CONNECTED = true;	
	}
	
	private void sortCharactersOnLevelToMakeFaradayadoFirst ()
	{
		int i = 0;
		foreach ( CharacterData characterData in charactersOnLevel )
		{
			if ( characterData.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
			{
				if ( i != 0 )
				{
					charactersOnLevel.RemoveAt ( i );
					charactersOnLevel.Insert ( 0, characterData );
					break;
				}
			}
					
			i++;
		}
	}
	
	private void setPowerToCharacters ()
	{

	}
	
	public GameObject getCharacterObjectFromLevel ( int characterID )
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( character.myID == characterID )
			{
				return LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]];
			}
		}
		
		return null;
	}
	
	public GameObject getSelectedCharacterObjectFromLevel ()
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( character.selected )
			{
				return LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]];
			}
		}
		
		return null;
	}
	
	public CharacterData getCharacter ( int characterID )
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( character.myID == characterID )
			{
				return character;
			}
		}
		
		if ( Array.IndexOf ( GameElements.DRAINED_CHARACTERS, characterID ) != -1 )
		{
			switch ( characterID )
			{
				case GameElements.CHAR_FARADAYDO_1_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_MADRA_1_DRAINED:
				foreach ( CharacterData character in CHARACTERS )
				{
					if ( character.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
					{
						return character;
					}
				}
				break;
				case GameElements.CHAR_MINER_1_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_MINER_1_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_MINER_2_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_MINER_2_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_MINER_3_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_MINER_3_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_MINER_4_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_MINER_4_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_MINER_5_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_MINER_5_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_MINER_6_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_MINER_6_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_JOSE_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_JOSE_1_IDLE )
						{
							return character;
						}
					}
					break;
				case GameElements.CHAR_BOZ_IDLE_DRAINED:
					foreach ( CharacterData character in CHARACTERS )
					{
						if ( character.myID == GameElements.CHAR_BOZ_1_IDLE )
						{
							return character;
						}
					}
					break;
			}
		}
		
		return null;
	}
	
	public CharacterData getSelectedCharacter ()
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( character.selected )
			{
				return character;
			}
		}
		
		return null;
	}
	
	public GameObject createObjectOnPosition ( int objectID, int[] position )
	{
		if ( objectID == GameElements.ENEM_SLUG_01 )
		{
//			print ("Wuuuuuuuuuuuuuuuuuuuuuut");
//			GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _slugPrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _slugPrefab.transform.rotation );
			GameObject interactiveObjectInstant = _slugPrefabPool.Spawn(new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), Quaternion.Euler(0, 180, 0));

			IComponent currentIComponent = interactiveObjectInstant.AddComponent < IComponent > ();
			currentIComponent.position = ToolsJerry.cloneTile ( position );
			currentIComponent.myID = objectID;
			
			interactiveObjectInstant.AddComponent < SelectedComponenent > ();
			MNEnemyComponent currentEnemyComponent = interactiveObjectInstant.AddComponent < MNEnemyComponent > ();
			interactiveObjectInstant.AddComponent < EnemyAnimationComponent > ();
			enemiesOnLevel.Add ( currentEnemyComponent );
			
			interactiveObjectInstant.AddComponent < MNSlugMovementComponent > ();
			
			currentIComponent.myEnemyData = new EnemyData ( objectID, "Techno Slug", 3, 0, 0, 1, 1, 2, 2 );
			interactiveObjectInstant.tag = GlobalVariables.Tags.INTERACTIVE;
			
			return interactiveObjectInstant;
		}
		else
		{
			GameObject interactiveObjectInstant = null;
			GameObject interactiveObjectMesh = null;
			if ( Array.IndexOf ( GameElements.TWO_TILES_OBJECTS, objectID ) != -1 )
			{
				interactiveObjectInstant = _tileInteractivePrefabPool.Spawn(new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), Quaternion.Euler(0, 180, 0));
//				interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
				interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
				interactiveObjectMesh.transform.localScale = new Vector3 ( interactiveObjectMesh.transform.localScale.x, interactiveObjectMesh.transform.localScale.y, 2f );
				interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x, interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f );
			}
			else if ( Array.IndexOf ( GameElements.TROLEYS, objectID ) != -1 )
			{
				interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractiveTrolleyPrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractiveTrolleyPrefab.transform.rotation );
				interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
			}
			else if ( Array.IndexOf ( GameElements.TWO_BY_TWO_TILES_OBJECTS, objectID ) != -1 )
			{
				interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1]), ( float ) position[1]  - 0.5f ), _tileInteractivePrefab.transform.rotation );
				interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
				interactiveObjectMesh.transform.localScale = Vector3.one * 2f;
				interactiveObjectMesh.transform.localPosition = Vector3.zero + Vector3.back * 1f;
				interactiveObjectMesh.GetComponent < BoxCollider > ().size *= 0.8f;
			}
			else
			{
				interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
				interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
			}
			
			IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
			currentIComponent.position = ToolsJerry.cloneTile ( position );
			currentIComponent.myID = objectID;
			
			bool doNotRegisterToTile = false;
			if ( Array.IndexOf ( GameElements.ENEMIES, objectID ) != -1 )
			{
				interactiveObjectMesh.transform.localScale = new Vector3 ( 0, 0, 0 );
				interactiveObjectMesh.GetComponent < BoxCollider > ().size = new Vector3 ( 0.5f, 1f, 1f );
				
				interactiveObjectMesh.collider.enabled = true;
				interactiveObjectMesh.AddComponent < SelectedComponenent > ();
				MNEnemyComponent currentEnemyComponent = interactiveObjectMesh.AddComponent < MNEnemyComponent > ();
				interactiveObjectMesh.AddComponent < EnemyAnimationComponent > ();
				enemiesOnLevel.Add ( currentEnemyComponent );
				
				switch ( objectID )
				{
				case GameElements.ENEM_TENTACLEDRAINER_01:
				case GameElements.ENEM_TENTACLEDRAINER_02:
					/*
					GameObject rockObjectPrefab = ( GameObject ) Resources.Load ( "Tile/tentacleRock" );
					GameObject rockObject = ( GameObject ) Instantiate ( rockObjectPrefab, interactiveObjectInstant.transform.position + Vector3.up * 0.25f + Vector3.forward * 1f, rockObjectPrefab.transform.rotation );
					*/
					PrefabPool prefabPool =  PrefabPool.GetPool("tentacleRock");
					GameObject rockObject = prefabPool.Spawn(interactiveObjectInstant.transform.position + Vector3.down * 0.25f + Vector3.forward * 1f, Quaternion.Euler(0, 180, 0));

					rockObject.AddComponent < EnemyTentacleRockControl > ().parentGameObject = interactiveObjectInstant;
					interactiveObjectMesh.transform.localPosition = new Vector3 ( 0f, 0f, -1.1f );
					iTween.ScaleTo ( interactiveObjectMesh, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeInElastic, "scale", new Vector3 ( 2f, 1f, 2f )));
					StartCoroutine(FixCollider(interactiveObjectMesh));
					GameObject myParticles = Instantiate ( Main.getInstance ().rockParticles, interactiveObjectMesh.transform.position + Vector3.up * 5f, Quaternion.identity ) as GameObject;
					myParticles.GetComponent<ParticlesControl>().fromVines = true;
					currentIComponent.myEnemyData = new EnemyData ( objectID, "Tentacle Drainer", 3, 0, 0, 1, 1, 2, 2 );

					break;
				case GameElements.ENEM_SLUG_01:
//					print ("Wuuuuuuuuut");
					interactiveObjectInstant.AddComponent < MNSlugMovementComponent > ();
					doNotRegisterToTile = true;
					currentIComponent.myEnemyData = new EnemyData ( objectID, "Techno Slug", 3, 0, 0, 1, 1, 2, 2 );
					interactiveObjectMesh.transform.localPosition = Vector3.zero + Vector3.back * 0.5f;
					interactiveObjectMesh.GetComponent < BoxCollider > ().size = Vector3.one * 0.8f;
					break;
				}
			}
			
			interactiveObjectMesh.tag = GlobalVariables.Tags.INTERACTIVE;
			
			interactiveObjectMesh.renderer.material.mainTexture = gameElements[objectID];
			
			if ( ! doNotRegisterToTile )
			{
				gameElementsOnLevel[GRID_LAYER_NORMAL][position[0]][position[1]] = interactiveObjectInstant;
				levelGrid[GRID_LAYER_NORMAL][position[0]][position[1]] = objectID;
			}
			
			return interactiveObjectInstant;
		}

		/*
		GameObject interactiveObjectInstant = null;
		GameObject interactiveObjectMesh = null;
		if ( Array.IndexOf ( GameElements.TWO_TILES_OBJECTS, objectID ) != -1 )
		{
			interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
			interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
			interactiveObjectMesh.transform.localScale = new Vector3 ( interactiveObjectMesh.transform.localScale.x, interactiveObjectMesh.transform.localScale.y, 2f );
			interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x, interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f );
		}
		else if ( Array.IndexOf ( GameElements.TROLEYS, objectID ) != -1 )
		{
			interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractiveTrolleyPrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractiveTrolleyPrefab.transform.rotation );
			interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
		}
		else
		{
			interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
			interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
		}
		
		interactiveObjectMesh.tag = GlobalVariables.Tags.INTERACTIVE;
		
		interactiveObjectMesh.renderer.material.mainTexture = gameElements[objectID];
		gameElementsOnLevel[GRID_LAYER_NORMAL][position[0]][position[1]] = interactiveObjectInstant;
		levelGrid[GRID_LAYER_NORMAL][position[0]][position[1]] = objectID;
		
		IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
		currentIComponent.myID = objectID;
		
		return interactiveObjectInstant;
		*/
	}


	IEnumerator FixCollider(GameObject interactiveObjectMesh)
	{
		yield return new WaitForSeconds (0.31f);
		interactiveObjectMesh.GetComponent < BoxCollider > ().size = new Vector3 ( 0.5f, 1f, 0.5f );
		interactiveObjectMesh.GetComponent < BoxCollider > ().center = new Vector3 ( 0f, 0f, 0.24f );
	}


	public bool isTileInLevelBoudaries ( int x, int z )
	{
		if (( x < 0 ) || ( x > LEVEL_WIDTH - 1 ) || ( z < 0 ) || ( z > LEVEL_HEIGHT - 1 ))
		{
			return false;
		}
		return true;
	}
	
	private bool isTileEmptyForRedirector ( int x, int z )
	{
		if ( isTileInLevelBoudaries ( x, z ))
		{			
			if (( levelGrid[GRID_LAYER_NORMAL][x][z] == GameElements.EMPTY ) && ( levelGrid[GRID_LAYER_REDIRECTORS][x][z] == GameElements.EMPTY ))
			{
				return true;
			}
			
			if ( GameElements.isInBorders ( levelGrid[GRID_LAYER_NORMAL][x][z], GameElements.ELECTRICITY_PASSES_BORDERS ))
			{
				return true;	
			}
			
			if (( Array.IndexOf ( GameElements.CHARACTERS, levelGrid[GRID_LAYER_NORMAL][x][z] ) != -1 ) && ( levelGrid[GRID_LAYER_REDIRECTORS][x][z] == GameElements.EMPTY ))
			{
				return true;
			}
			
			if (( Array.IndexOf ( GameElements.DRAINED_CHARACTERS, levelGrid[GRID_LAYER_NORMAL][x][z] ) != -1 ) && ( levelGrid[GRID_LAYER_REDIRECTORS][x][z] == GameElements.EMPTY ))
			{
				return true;
			}
		}
		
		return false;
	}
	
	public bool allPathIsWalkableForCharacter ( int characterID, GameObject characterObject, int[][] path )
	{
		bool mayGo = true;
		int[] previousTile = null;

		if ( path == null )
		{
			mayGo = false;
		}
		else
		{
			foreach ( int[] tile in path )
			{
				if ( tile == null )
				{
					if ( Array.IndexOf ( GameElements.CHARACTERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][previousTile[0]][previousTile[1]] ) != -1 )
					{
						if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][previousTile[0]][previousTile[1]] != characterID )
						{
							if ( ! LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][previousTile[0]][previousTile[1]].transform.Find ( "tile" ).GetComponent < IComponent > ().myCharacterData.moving )
							{
								mayGo = false;
							}
						}
					}
					
					break;
				}
				/*
				else if ( previousTile != null )
				{
					if ( Vector3.Distance ( new Vector3 ((float) tile[0], 0f, (float) tile[1] ), new Vector3 ((float) previousTile[0], 0f, (float) previousTile[1] )) > 1.3f )
					{
						mayGo = false;
						break;
					}
				}
				*/
				previousTile = ToolsJerry.cloneTile ( tile );
				if (( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][tile[0]][tile[1]] == GameElements.EMPTY ) || ( Array.IndexOf ( GameElements.BUILDERS, characterID ) != -1 ))
				{
					if ( Array.IndexOf ( GameElements.CHARACTERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][tile[0]][tile[1]] ) != -1 )
					{
						mayGo = true;
					}
					else if ( ! GridReservationManager.getInstance ().fillTileWithMe ( characterID, tile[0], tile[1], characterObject, characterID, true ))
					{
						mayGo = false;
						break;
					}
				}
				else
				{
					mayGo = false;
					break;
				}
			}
		}
		
		return mayGo;
	}
	
	public bool isTileEmptyForRescuerWithTroley ( int x, int z )
	{
		if ( isTileInLevelBoudaries ( x, z ))
		{
			switch ( levelGrid[GRID_LAYER_NORMAL][x][z] )
			{
				case GameElements.EMPTY:
				case GameElements.ENVI_RESCUEZONE_1:
					if (( levelGrid[GRID_LAYER_BEAM][x][z] == GameElements.EMPTY ) && ( levelGrid[GRID_LAYER_REDIRECTORS][x][z] == GameElements.EMPTY ))
					{
						return true;
					}
					else if (( gameElementsOnLevel[GRID_LAYER_BEAM][x][z] == null ) && ( gameElementsOnLevel[GRID_LAYER_NORMAL][x][z] == null ) && ( gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x][z] == null ))
					{
						return true;
					}
			
					break;
			}
			
			if (( gameElementsOnLevel[GRID_LAYER_BEAM][x][z] == null ) && ( gameElementsOnLevel[GRID_LAYER_NORMAL][x][z] == null ) && ( gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x][z] == null ))
			{
				return true;
			}
			
			if ( Array.IndexOf ( GameElements.TROLEYS, levelGrid[GRID_LAYER_NORMAL][x][z] ) != -1 )
			{
				return true;
			}
			
			if ( levelGrid[GRID_LAYER_NORMAL][x][z] == GameElements.CHAR_CORA_1_IDLE )
			{
				return true;
			}
			
			if ( Array.IndexOf ( GameElements.CHARACTERS, levelGrid[GRID_LAYER_NORMAL][x][z] ) != -1 )
			{
				RescuerComponent.CHARACTER_HIT = gameElementsOnLevel[GRID_LAYER_NORMAL][x][z];
			}
		}
		
		return false;
	}
	
	public Transform findEnergyObjectOnPath ( int x, int z, int axis )
	{
		bool foundObjectOnPath = false;
		
		int incrementX = 0;
		int incrementZ = 0;
		
		switch ( axis )
		{
			case RedirectorComponent.DIRECTION_DOWN:
				incrementX = 0;
				incrementZ = -1;
				break;
			case RedirectorComponent.DIRECTION_UP:
				incrementX = 0;
				incrementZ = 1;
				break;
			case RedirectorComponent.DIRECTION_RIGHT:
				incrementX = 1;
				incrementZ = 0;
				break;
			case RedirectorComponent.DIRECTION_LEFT:
				incrementX = -1;
				incrementZ = 0;
				break;
		}
		
		int incrementedX = 0;
		int incrementedZ = 0;
		
		while ( ! foundObjectOnPath )
		{
			incrementedZ += incrementZ;
			incrementedX += incrementX;
			
			if ( isTileEmptyForRedirector ( x + incrementedX, z + incrementedZ ))
			{
				// do nothing and seek more
			}
			else if ( isTileInLevelBoudaries ( x + incrementedX, z + incrementedZ ))
			{
				if ( gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ] )
				{
					switch ( levelGrid[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ] )
					{
						case GameElements.ENVI_REDIRECTOR_01:
							switch ( axis )
							{
								case RedirectorComponent.DIRECTION_UP:
								case RedirectorComponent.DIRECTION_RIGHT:
									return gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ].transform;
							}
							break;
						case GameElements.ENVI_REDIRECTOR_02:
							switch ( axis )
							{
								case RedirectorComponent.DIRECTION_UP:
								case RedirectorComponent.DIRECTION_LEFT:
									return gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ].transform;
							}
							break;
						case GameElements.ENVI_REDIRECTOR_03:
							switch ( axis )
							{
								case RedirectorComponent.DIRECTION_DOWN:
								case RedirectorComponent.DIRECTION_LEFT:
									return gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ].transform;
							}
							break;
						case GameElements.ENVI_REDIRECTOR_04:
							switch ( axis )
							{
								case RedirectorComponent.DIRECTION_DOWN:
								case RedirectorComponent.DIRECTION_RIGHT:
									return gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ].transform;
							}
							break;
						case GameElements.ENVI_CORES_01:
						case GameElements.ENVI_POWERPLUG_01:
							return gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ].transform;
					}
					
					foundObjectOnPath = true;
				}
				else
				{
					foundObjectOnPath = true;
				}
			}
			else
			{
				foundObjectOnPath = true;
			}
		}
		
		return null;
	}
	
	public bool anyMovePerformed ()
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( character.totalMovesPerfromed > 0 ) return true;
		}
		
		return false;
	}
	
	public Transform findAnyObjectOnPathForRedirectorAndElectrify ( int x, int z, int axis )
	{
		bool foundObjectOnPath = false;
		
		int incrementX = 0;
		int incrementZ = 0;
		
		switch ( axis )
		{
			case RedirectorComponent.DIRECTION_DOWN:
				incrementX = 0;
				incrementZ = -1;
				break;
			case RedirectorComponent.DIRECTION_UP:
				incrementX = 0;
				incrementZ = 1;
				break;
			case RedirectorComponent.DIRECTION_RIGHT:
				incrementX = 1;
				incrementZ = 0;
				break;
			case RedirectorComponent.DIRECTION_LEFT:
				incrementX = -1;
				incrementZ = 0;
				break;
		}
		
		int incrementedX = 0;
		int incrementedZ = 0;
		
		while ( ! foundObjectOnPath )
		{
			incrementedZ += incrementZ;
			incrementedX += incrementX;
			
			if ( isTileEmptyForRedirector ( x + incrementedX, z + incrementedZ ))
			{
				// do nothing and seek more
			}
			else if ( isTileInLevelBoudaries ( x + incrementedX, z + incrementedZ ))
			{
				if ( gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ] )
				{
					return gameElementsOnLevel[GRID_LAYER_REDIRECTORS][x + incrementedX][z + incrementedZ].transform;
				}
				else if ( gameElementsOnLevel[GRID_LAYER_NORMAL][x + incrementedX][z + incrementedZ] )
				{					
					return gameElementsOnLevel[GRID_LAYER_NORMAL][x + incrementedX][z + incrementedZ].transform;
				}
				else
				{
					foundObjectOnPath = true;
				}
			}
			else
			{
				foundObjectOnPath = true;
			}
		}
		
		return null;
	}
}
