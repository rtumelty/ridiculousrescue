using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MNLevelControl : MonoBehaviour 
{
	//*************************************************************//
	public class SpecificLevelData
	{
		public int[] charactersPower = new int[3] { 0, 0, 0 };
		public int redirectors = 0;
		public int oneStarLimit = 20;
		public int twoStarLimit = 15;
		public int threeStarLimit = 10;
	}
	//*************************************************************//
	public static string SELECTED_LEVEL_NAME = "NULL";
	public static int LEVEL_ID = 1;
	public static GameGlobalVariables.Missions.MiningLevelClass CURRENT_LEVEL_CLASS;
	public static List < CharacterData > CHARACTERS;
	//*************************************************************//
	public const int NUMBER_OF_LAYERS = 4;
	public const int GRID_LAYER_NORMAL = 0;
	public const int GRID_LAYER_REDIRECTORS = 1;
	public const int GRID_LAYER_TIPS = 2;
	public const int GRID_LAYER_SLUG_LONGER_PATH = 3;
	public const int LEVEL_WIDTH = 12;
	public const int LEVEL_HEIGHT = 8;
	//*************************************************************//
	public Texture2D[] gameElements;
	public int[][][] levelGrid;
	public GameObject[][][] gameElementsOnLevel;
	public List < CharacterData > charactersOnLevel;
	public List < MNEnemyComponent > enemiesOnLevel;
	public int totalRockPartsTobeCleared;
	public int totalRocksPartsCleared;
	public int totalPlasticOreRocksDestroyed;
	public int objectsOfValue = 0;
	public int destroyedObjectsOfValue = 0;
	//*************************************************************//
	private GameObject _tileInteractivePrefab;
	private GameObject _tileInteractivePrefab3x1;
	private bool _levelsLoaded = false;
	private SpecificLevelData _currentSpecifcLevelData;
	private List < Texture2D > _backgroundTextures;
	private GameObject _slugPrefab;
	private int randomMine;
	private Dictionary <int, string > myDictionary;
	private int myVariant = 0;
	//*************************************************************//	
	private static MNLevelControl _meInstance;
	public static MNLevelControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < MNLevelControl > ();
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
		
		for ( int i = 0; i < NUMBER_OF_LAYERS; i++ )
		{
			levelGrid[i] = new int[LEVEL_WIDTH][];
			gameElementsOnLevel[i] = new GameObject[LEVEL_WIDTH][];
			for ( int j = 0; j < LEVEL_WIDTH; j++ )
			{
				levelGrid[i][j] = new int[LEVEL_HEIGHT];
				gameElementsOnLevel[i][j] = new GameObject[LEVEL_HEIGHT];
			}
		}
		
		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractive" );
		_tileInteractivePrefab3x1 = ( GameObject ) Resources.Load ( "Tile/tileInteractive3x1" );

		_slugPrefab = ( GameObject ) Resources.Load ( "Spine/Slug/spine" );

		loadLevel ();		
		//print (LEVEL_ID);
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
			MNLevelControl.getInstance ().gameElements[1000 + i] = texture;
			i++;
		}
	}
	
	public void loadLevelLocalToThisGrid ( int levelID, int[][] grid )
	{
		/*
		XmlDocument levelXml = new XmlDocument ();
//		print ("this far");
		if(SaveDataManager.getValue ( SaveDataManager.MINES_ENTERED) == 0)
		{
			myVariant = 1;
	//		print ("tutorial level 1 loaded");
		}
		else if(LEVEL_ID == 1 &&  SaveDataManager.getValue ( SaveDataManager.MINES_ENTERED) != 0)
		{
	//		print ("tutorial level 1 avoided");
			myVariant = UnityEngine.Random.Range(2, SelectLevel.myVariantTotals[levelID] + 1);
		}
		else
		{
	//		print ("Other world mines");
			myVariant = UnityEngine.Random.Range(1, SelectLevel.myVariantTotals[levelID] + 1);
		}
		//print ("myID " + levelID);
		//print ("myVarient " + myVariant);
		Dictionary < int, string > myTest = new Dictionary<int, string> ();
		myTest = SelectLevel.ALL_MINE_VARIANTS[levelID];
		string mytestStr = myTest [myVariant];
		levelXml.LoadXml (mytestStr);
		//=======Dave Work===========

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
		
		if ( specifcDataNode.ChildNodes.Count > 4 )
		{
			int.TryParse ( specifcDataNode.ChildNodes[4].InnerText, out _currentSpecifcLevelData.oneStarLimit );
			int.TryParse ( specifcDataNode.ChildNodes[5].InnerText, out _currentSpecifcLevelData.twoStarLimit );
			int.TryParse ( specifcDataNode.ChildNodes[6].InnerText, out _currentSpecifcLevelData.threeStarLimit );
		}
		
		foreach ( Texture2D texture in _backgroundTextures )
		{
			if ( texture.name == backgroundNode.InnerText )
			{
				GameObject.Find ( "Background" ).renderer.material.mainTexture = texture;
			}
		}
		
		finalTouchOfLoadingLevels ();
		*/
	}
	
	private void loadLevel ()
	{
		//=================Daves Edit======================
		randomMine = UnityEngine.Random.Range(1,GameGlobalVariables.NUMBER_OF_MINE_LEVELS + 1);
		//print ("Random mine" + randomMine);
		if(SaveDataManager.getValue ( SaveDataManager.MINES_ENTERED) == 0)
		{
			loadLevelLocalToThisGrid ( 1, levelGrid[MNLevelControl.GRID_LAYER_NORMAL] );
			//print ("First time");
			//print ("levelID " + LEVEL_ID);
		}
		else
		{
			//LEVEL_ID = randomMine;
			loadLevelLocalToThisGrid ( LEVEL_ID , levelGrid[MNLevelControl.GRID_LAYER_NORMAL] );
			//print ("Another time");
		}
		//=================Daves Edit======================
	}
	
	private void finalTouchOfLoadingLevels ()
	{
		if ( _levelsLoaded ) return;
		
		if ( CURRENT_LEVEL_CLASS != null ) CURRENT_LEVEL_CLASS.mySpecificLevelData = _currentSpecifcLevelData;
		
		if ( CHARACTERS == null )
		{
			CHARACTERS = new List < CharacterData > ();
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_CORA_1_IDLE, "Cora", -1, -1, 5, 0, 1, 1, 0, 0, 0, 0, 1, 20 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MADRA_1_IDLE, "Madra", -1, -1, 9, 3, 1, 1, 3, 0, 0, 0, 0, 10 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_FARADAYDO_1_IDLE, "Faradaydo", -1, -1, 15, 0, 1, 1, 0, 2, 2, 2, 0, 40 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_1_IDLE, "Miner", -1, -1, 15, 0, 1, 1, 0, 2, 2, 2, 0, 50 ));
			//=====================Daves Work==================
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_BOZ_1_IDLE, "Boz", -1, -1, 15, 3, 1, 1, 3, 2, 2, 2, 0, 40 ));
			//=====================Daves Work==================
		}
		
		_levelsLoaded = true;

		fillLevelWithAssets ();
		
		sortCharactersOnLevelToMakeFaradayadoFirst ();
		setPowerToCharacters ();
		MNUIControl.getInstance ().createButtonsCharacters ( charactersOnLevel );
		
		MNSpawningEnemiesManager.getInstance ().updateHolesOnLevel ();
		
		GlobalVariables.LOADING_SAVING_MENU = false;
	}
	
	private void fillLevelWithAssets ()
	{
		for ( int i = 0; i < LEVEL_WIDTH; i++ )
		{
			for ( int j = 0; j < LEVEL_HEIGHT; j++ )
			{
				if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_FARADAYDO_1_IDLE )
				{
					GameObject faradaydoPrefab = ( GameObject ) Resources.Load ( "Spine/Fara/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( faradaydoPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), faradaydoPrefab.transform.rotation );
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
							interactiveObjectInstant.AddComponent < MNMoveComponent > ();
							interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
							interactiveObjectMesh.AddComponent < MNCharacterReloactionComponent > ();
							
							interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
							interactiveObjectMesh.AddComponent < MNAttackerComponent > ();
						}
					}
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_MADRA_1_IDLE )
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
							interactiveObjectInstant.AddComponent < MNMoveComponent > ();
							interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
							interactiveObjectMesh.AddComponent < MNCharacterReloactionComponent > ();
							
							interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
							interactiveObjectMesh.AddComponent < MNAttackerComponent > ();
						}
					}
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_BOZ_1_IDLE )
				{
					GameObject bozPrefab = ( GameObject ) Resources.Load ( "Spine/Boz/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( bozPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), bozPrefab.transform.rotation );
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
							interactiveObjectInstant.AddComponent < MNMoveComponent > ();
							interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
							interactiveObjectMesh.AddComponent < MNCharacterReloactionComponent > ();
							
							interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
							interactiveObjectMesh.AddComponent < MNAttackerComponent > ();
						}
					}
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
						interactiveObjectMesh.GetComponent < BoxCollider > ().size *= 0.7f;
					}
					else
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), _tileInteractivePrefab.transform.rotation );
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
					
					if ( Array.IndexOf ( GameElements.DESTROYABLE_OBJECTS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						if ( levelGrid[GRID_LAYER_TIPS][i][j] == GameElements.UI_TIP_DEMOLISHER )
						{
							TipOnClickComponent currentTipOnClickComponent = interactiveObjectMesh.AddComponent < TipOnClickComponent > ();
							currentTipOnClickComponent.myTipTexture = gameElements[GameElements.UI_TIP_DEMOLISHER];
							currentTipOnClickComponent.tipID = GameElements.UI_TIP_DEMOLISHER;
						}
						
						switch ( levelGrid[GRID_LAYER_NORMAL][i][j] )
						{
							case GameElements.ENVI_METAL_1_ALONE:
							case GameElements.ENVI_PLASTIC_1_ALONE:
							case GameElements.ENVI_TECHNOEGG_1_ALONE:
								totalRockPartsTobeCleared += 3;
								break;
						}
						
						interactiveObjectMesh.collider.enabled = true;
						interactiveObjectMesh.AddComponent < MNDestroyableObjectComponent > ();
						interactiveObjectMesh.AddComponent < SelectedComponenent > ();
					}
					else if ( Array.IndexOf ( GameElements.ENEMIES, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectMesh.transform.localScale = new Vector3 ( 2f, interactiveObjectMesh.transform.localScale.y, interactiveObjectMesh.transform.localScale.z );
						interactiveObjectMesh.GetComponent < BoxCollider > ().size = new Vector3 ( 0.5f, 1f, 1f );
											
						/*
						if ( levelGrid[GRID_LAYER_TIPS][i][j] == GameElements.UI_TIP_ATTACKER )
						{
							TipOnClickComponent currentTipOnClickComponent = interactiveObjectMesh.AddComponent < TipOnClickComponent > ();
							currentTipOnClickComponent.myTipTexture = gameElements[GameElements.UI_TIP_ATTACKER];
							currentTipOnClickComponent.tipID = GameElements.UI_TIP_ATTACKER;
						}
						*/
						
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
								currentIComponent.myEnemyData = new EnemyData ( levelGrid[GRID_LAYER_NORMAL][i][j], "Tentacle Drainer", 3, 0, 0, 1, 1, 2, 2 );
								break;
							case GameElements.ENEM_SLUG_01:
								interactiveObjectInstant.AddComponent < MNSlugMovementComponent > ();
								currentIComponent.myEnemyData = new EnemyData ( levelGrid[GRID_LAYER_NORMAL][i][j], "Techno Slug", 3, 0, 0, 1, 1, 2, 2 );
								break;
						}
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
								interactiveObjectInstant.AddComponent < MNMoveComponent > ();
								interactiveObjectInstant.AddComponent < ProduceFloatingNumberComponent > ();
								interactiveObjectMesh.AddComponent < MNCharacterReloactionComponent > ();
								
								interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
								interactiveObjectMesh.AddComponent < MNAttackerComponent > ();
							}
						}
					}
				}
			}
		}		
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
		CharacterData currentCharacter = null;
		for ( int i = 0; i < 3; i++ )
		{
			switch ( i )
			{
				case 0:
					currentCharacter = getCharacter ( GameElements.CHAR_FARADAYDO_1_IDLE );
					break;
				case 1:
					currentCharacter = getCharacter ( GameElements.CHAR_CORA_1_IDLE );
					break;
				case 2:
					currentCharacter = getCharacter ( GameElements.CHAR_MADRA_1_IDLE );
					break;
			}
			
			if ( currentCharacter != null )
			{
				currentCharacter.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] = _currentSpecifcLevelData.charactersPower[i];
			}
		}
	}
	
	public GameObject getCharacterObjectFromLevel ( int characterID )
	{
		foreach ( CharacterData character in charactersOnLevel )
		{
			if ( character.myID == characterID )
			{
				return MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]];
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
				return MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]];
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
			}
		}
		
		return null;
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
			GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _slugPrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _slugPrefab.transform.rotation );

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
				interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
				interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
				interactiveObjectMesh.transform.localScale = new Vector3 ( interactiveObjectMesh.transform.localScale.x, interactiveObjectMesh.transform.localScale.y, 2f );
				interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x, interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f );
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
				interactiveObjectMesh.transform.localScale = new Vector3 ( 2f, interactiveObjectMesh.transform.localScale.y, interactiveObjectMesh.transform.localScale.z );
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
						GameObject rockObjectPrefab = ( GameObject ) Resources.Load ( "Tile/tentacleRock" );
						GameObject rockObject = ( GameObject ) Instantiate ( rockObjectPrefab, interactiveObjectInstant.transform.position + Vector3.up * 0.25f + Vector3.forward * 1f, rockObjectPrefab.transform.rotation );
						rockObject.AddComponent < EnemyTentacleRockControl > ().parentGameObject = interactiveObjectInstant;
						interactiveObjectMesh.transform.localPosition = new Vector3 ( 0f, 0f, -1.1f );
						currentIComponent.myEnemyData = new EnemyData ( objectID, "Tentacle Drainer", 3, 0, 0, 1, 1, 2, 2 );
						break;
					case GameElements.ENEM_SLUG_01:
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
	}
	
	public bool isTileInLevelBoudaries ( int x, int z )
	{
		if (( x < 0 ) || ( x > LEVEL_WIDTH - 1 ) || ( z < 0 ) || ( z > LEVEL_HEIGHT - 1 )) return false;
		return true;
	}
	
	public bool allPathIsWalkableForCharacter ( int characterID, GameObject characterObject, int[][] path )
	{
		bool mayGo = true;
		int[] previousTile = null;
		foreach ( int[] tile in path )
		{
			if ( tile == null )
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][previousTile[0]][previousTile[1]] ) != -1 )
				{
					if ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][previousTile[0]][previousTile[1]] != characterID )
					{
						mayGo = false;
					}
				}
				
				break;
			}
			previousTile = ToolsJerry.cloneTile ( tile );
			if (( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_REDIRECTORS][tile[0]][tile[1]] == GameElements.EMPTY ) || ( Array.IndexOf ( GameElements.BUILDERS, characterID ) != -1 ))
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][tile[0]][tile[1]] ) != -1 )
				{
					mayGo = true;
				}
				else if ( ! MNGridReservationManager.getInstance ().fillTileWithMe ( characterID, tile[0], tile[1], characterObject, characterID, true ))
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
		
		return mayGo;
	}
}
/* 

 */