using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class TRLevelControl : MonoBehaviour 
{
	//*************************************************************//
	public static string SELECTED_LEVEL_NAME = "NULL";
	public static int LEVEL_ID = 1;
	public static GameGlobalVariables.Missions.TrainLevelClass CURRENT_LEVEL_CLASS;
	//*************************************************************//
	public const int NUMBER_OF_LAYERS = 3;
	public const int GRID_LAYER_NORMAL = 0;
	public const int GRID_LAYER_REDIRECTORS = 1;
	public const int GRID_LAYER_TIPS = 2;
	public const int LEVEL_WIDTH = 12;
	public const int LEVEL_HEIGHT = 8;
	//*************************************************************//
	public Texture2D[] gameElements;
	public int[][][] levelGrid;
	public GameObject[][][] gameElementsOnLevel;
	public List < CharacterData > charactersOnLevel;
	public int totalRockPartsTobeCleared;
	public int totalRocksPartsCleared;
	public Texture2D tentacleGrabTexture;
	//*************************************************************//
	private GameObject _tileInteractivePrefab;
	private GameObject _tileInteractivePrefab3x1;
	public bool _levelsLoaded = false;
	private List < Texture2D > _backgroundTextures;

	public static List < CharacterData > CHARACTERS;
	//*************************************************************//	
	private static TRLevelControl _meInstance;
	public static TRLevelControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < TRLevelControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		int numberOfTrainPLayesd = SaveDataManager.getValue ( SaveDataManager.TRAIN_PLAYED );
		numberOfTrainPLayesd++;
		SaveDataManager.save ( SaveDataManager.TRAIN_PLAYED, numberOfTrainPLayesd );
		GoogleAnalytics.instance.LogScreen ( "Train - Start - Level " + TRLevelControl.SELECTED_LEVEL_NAME );

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
		
		levelGrid = new int[NUMBER_OF_LAYERS][][];
		gameElementsOnLevel = new GameObject[NUMBER_OF_LAYERS][][];
		
		charactersOnLevel = new List < CharacterData > ();
		
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

		GameGlobalVariables.CURRENT_GAME_PART = GameGlobalVariables.TRAIN;

		loadLevel ();	
		/*if(LEVEL_ID == 1)
		{
			GameObject.Find("Boz").SetActive(false);
		}*/
		print (LEVEL_ID);
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
			TRLevelControl.getInstance ().gameElements[1000 + i] = texture;
			i++;
		}
	}
	
	public void loadLevelLocalToThisGrid ( int levelID, int[][] grid )
	{
		/*
		XmlDocument levelXml = new XmlDocument ();
		levelXml.LoadXml ( SelectLevel.ALL_MINING_LEVELS[levelID] );
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
	
		foreach ( Texture2D texture in _backgroundTextures )
		{
			if ( texture.name == backgroundNode.InnerText )
			{
				GameObject.Find ( "Background" ).renderer.material.mainTexture = texture;
			}
		}
		*/

		StartCoroutine ( "finalTouchOfLoadingLevels" );
	}
	
	private void loadLevel ()
	{
		loadLevelLocalToThisGrid ( LEVEL_ID, levelGrid[TRLevelControl.GRID_LAYER_NORMAL] );
	}
	
	private IEnumerator finalTouchOfLoadingLevels ()
	{
		if ( ! _levelsLoaded )
		{
		
			yield return new WaitForSeconds ( 0.2f );

			_levelsLoaded = true;

			fillLevelWithAssets ();
			//=====================================Daves Edit====================================
			//charactersOnLevel.Add ( new CharacterData ( GameElements.CHAR_MADRA_1_IDLE, "Madra", -1, -1, 10, 3, 1, 1, 3, 0, 0, 0, 0, 10 ));
			charactersOnLevel.Add ( new CharacterData ( GameElements.CHAR_FARADAYDO_1_IDLE, "Faradaydo", -1, -1, 5, 0, 1, 1, 0, 2, 2, 2, 0, 40 ));
			if(TRLevelControl.LEVEL_ID >= 2)
			{
				charactersOnLevel.Add ( new CharacterData ( GameElements.CHAR_CORA_1_IDLE, "Cora", -1, -1, 10, 0, 1, 1, 0, 0, 0, 0, 1, 20 ));
			}
			charactersOnLevel.Add ( new CharacterData ( GameElements.CHAR_JOSE_1_IDLE, "Jose", -1, -1, 5, 0, 1, 1, 0, 2, 2, 2, 0, 50 ));
			//charactersOnLevel.Add ( new CharacterData ( GameElements.CHAR_BOZ_1_IDLE, "Boz", -1, -1, 5, 0, 1, 1, 0, 2, 2, 2, 0, 50 ));

			TRUIControl.getInstance ().createButtonsCharacters ( charactersOnLevel );

			if ( TRLevelControl.CURRENT_LEVEL_CLASS != null && CURRENT_LEVEL_CLASS.star04 != 0 )
			{
				TRUIControl.getInstance ().manageTimer ( true );
			}
			else
			{
				TRUIControl.getInstance ().manageTimer ( false );
			}
			//=====================================Daves Edit====================================
			GlobalVariables.LOADING_SAVING_MENU = false;
		}
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
	
	private void fillLevelWithAssets ()
	{
		for ( int i = 0; i < LEVEL_WIDTH; i++ )
		{
			for ( int j = 0; j < LEVEL_HEIGHT; j++ )
			{
				if ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.EMPTY && levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.UNWALKABLE )
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
					
				}
			}
		}		
	}
	
	public bool isTileInLevelBoudaries ( int x, int z )
	{
		if (( x < 0 ) || ( x > LEVEL_WIDTH - 1 ) || ( z < 0 ) || ( z > LEVEL_HEIGHT - 1 )) return false;
		return true;
	}
}
