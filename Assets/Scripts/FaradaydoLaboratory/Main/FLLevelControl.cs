using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class FLLevelControl : MonoBehaviour 
{
	//*************************************************************//
	public static List < CharacterData > CHARACTERS;
	//*************************************************************//
	public const int NUMBER_OF_LAYERS = 1;
	public const int GRID_LAYER_NORMAL = 0;
	public const int LEVEL_WIDTH = 12 * 5;
	public const int LEVEL_HEIGHT = 8 * 2;
	//*************************************************************//
	public Texture2D[] gameElements;
	public int[][][] levelGrid;
	public GameObject[][][] gameElementsOnLevel;
	public List < CharacterData > charactersOnLevel;
	public List < CharacterTapTutorialControl > tutorialTapComponentsOnLevel;
	//*************************************************************//
	private GameObject _tileInteractivePrefab;
	//*************************************************************//	
	private static FLLevelControl _meInstance;
	public static FLLevelControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < FLLevelControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		tutorialTapComponentsOnLevel = new List < CharacterTapTutorialControl > ();
		
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
		
		if ( CHARACTERS == null )
		{
			CHARACTERS = new List < CharacterData > ();
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_CORA_1_IDLE, "Cora", -1, -1, 5, 1, 1, 1, 0, 0, 0, 0, 1 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MADRA_1_IDLE, "Madra", -1, -1, 9, 3, 1, 1, 3, 0, 0, 0, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_FARADAYDO_1_IDLE, "Faradaydo", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_1_IDLE, "Miner", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
			//===============================Daves Edit=====================================
			CHARACTERS.Add ( new CharacterData ( GameElements.CHAR_MINER_4_IDLE, "Miner", -1, -1, 15, 1, 1, 1, 0, 2, 2, 2, 0 ));
		}
	}
	
	public void fillLevelWithAssets ()
	{
		for ( int i = 0; i < LEVEL_WIDTH; i++ )
		{
			for ( int j = 0; j < LEVEL_HEIGHT; j++ )
			{
				if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_CORA_1_IDLE )
				{
					GameObject coraPrefab = ( GameObject ) Resources.Load ( "Spine/Cora/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( coraPrefab, new Vector3 (( float ) i - 3.5f, ( LEVEL_HEIGHT - j ), ( float ) j  + 0.75f ), coraPrefab.transform.rotation );
					interactiveObjectInstant.tag = ("ForMovement");
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
						}
					}
					
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
					interactiveObjectInstant.AddComponent < FLMoveComponent > ();
					interactiveObjectMesh.AddComponent < FLCharacterRelocationComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					
					if ( GameGlobalVariables.LAB_ENTERED >= 1 )
					{
						tutorialTapComponentsOnLevel.Add ( interactiveObjectMesh.AddComponent < CharacterTapTutorialControl > ());
					}
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_FARADAYDO_1_IDLE )
				{
					GameObject faradaydoPrefab = ( GameObject ) Resources.Load ( "Spine/Fara/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( faradaydoPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), faradaydoPrefab.transform.rotation );
					interactiveObjectInstant.tag = ("ForMovement");
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
						}
					}

					interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
					interactiveObjectInstant.AddComponent < FLMoveComponent > ();
					interactiveObjectMesh.AddComponent < FLCharacterRelocationComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
					
					if ( GameGlobalVariables.LAB_ENTERED >= 1 )
					{
						tutorialTapComponentsOnLevel.Add ( interactiveObjectMesh.AddComponent < CharacterTapTutorialControl > ());
					}
				}
				else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_MADRA_1_IDLE )
				{
					GameObject madraPrefab = ( GameObject ) Resources.Load ( "Spine/Madra/prefab" );
					GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( madraPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j  - 0.5f ), madraPrefab.transform.rotation );
					interactiveObjectInstant.tag = ("ForMovement");
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
						}
					}
					
					interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
					interactiveObjectInstant.AddComponent < FLMoveComponent > ();
					interactiveObjectMesh.AddComponent < FLCharacterRelocationComponent > ();
					interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();

					interactiveObjectInstant.AddComponent < FLFollowingPlayerFromRoomToRoomComponent > ();
				}
				else if (( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.EMPTY ) && ( levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.UNWALKABLE ))
				{
					GameObject interactiveObjectInstant = null;
					GameObject interactiveObjectMesh = null;
					if ( Array.IndexOf ( GameElements.TWO_TILES_OBJECTS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j - 0.5f ), _tileInteractivePrefab.transform.rotation );
						interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
						interactiveObjectMesh.transform.localScale = new Vector3 ( interactiveObjectMesh.transform.localScale.x, interactiveObjectMesh.transform.localScale.y, 2f );
						interactiveObjectMesh.transform.position = new Vector3 ( interactiveObjectMesh.transform.position.x, interactiveObjectMesh.transform.position.y, interactiveObjectMesh.transform.position.z + 0.5f );
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
						if(levelGrid[GRID_LAYER_NORMAL][i][j] != GameElements.CHAR_MINER_4_IDLE)
						{
							interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j - 0.5f  ), _tileInteractivePrefab.transform.rotation );
							interactiveObjectInstant.tag = ("ForMovement");
							interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
						}
						else
						{
							GameObject minerPrefab = ( GameObject ) Resources.Load ( "Spine/Miner01/prefab" );
							interactiveObjectInstant = ( GameObject ) Instantiate ( minerPrefab, new Vector3 (( float ) i, ( LEVEL_HEIGHT - j ), ( float ) j - 0.5f  ), minerPrefab.transform.rotation );
							interactiveObjectInstant.tag = ("ForMovement");
							interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
							interactiveObjectMesh.tag = GlobalVariables.Tags.CHARACTER;
							//interactiveObjectMesh.transform.localScale = Vector3.one * 3f;
						}
					}
					
					interactiveObjectMesh.tag = FLGlobalVariables.Tags.INTERACTIVE;
					
					if(interactiveObjectMesh.renderer != null)
					{
						interactiveObjectMesh.renderer.material.mainTexture = gameElements[levelGrid[GRID_LAYER_NORMAL][i][j]];
					}
					gameElementsOnLevel[GRID_LAYER_NORMAL][i][j] = interactiveObjectInstant;
					
					IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
					currentIComponent.myID = levelGrid[GRID_LAYER_NORMAL][i][j];
					
					if ( Array.IndexOf ( GameElements.CHARACTERS, levelGrid[GRID_LAYER_NORMAL][i][j] ) != -1 )
					{
						interactiveObjectMesh.collider.enabled = true;
						interactiveObjectMesh.tag = FLGlobalVariables.Tags.CHARACTER;
						
						foreach ( CharacterData characterData in CHARACTERS )
						{
							if ( characterData.myID == levelGrid[GRID_LAYER_NORMAL][i][j] )
							{
								charactersOnLevel.Add ( characterData.getClone ());
								charactersOnLevel[charactersOnLevel.Count - 1].position[0] = i;
								charactersOnLevel[charactersOnLevel.Count - 1].position[1] = j;
								
								interactiveObjectMesh.AddComponent < SelectedComponenent > ();								
								interactiveObjectInstant.AddComponent < FLMoveComponent > ();
								interactiveObjectMesh.AddComponent < FLCharacterRelocationComponent > ();
								interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
								if(characterData.myID == 84)
								{
									interactiveObjectMesh.AddComponent < TestAnim > ();
								}
								interactiveObjectMesh.GetComponent < IComponent > ().myCharacterData = charactersOnLevel[charactersOnLevel.Count - 1];
							
								if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_MADRA_1_IDLE )
								{
									interactiveObjectInstant.AddComponent < FLFollowingPlayerFromRoomToRoomComponent > ();

								}
								else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_FARADAYDO_1_IDLE )
								{
									if ( GameGlobalVariables.LAB_ENTERED >= 1 )
									{
										tutorialTapComponentsOnLevel.Add ( interactiveObjectMesh.AddComponent < CharacterTapTutorialControl > ());
									}
								}
								else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_CORA_1_IDLE )
								{
									if ( GameGlobalVariables.LAB_ENTERED >= 1 )
									{
										tutorialTapComponentsOnLevel.Add ( interactiveObjectMesh.AddComponent < CharacterTapTutorialControl > ());
									}
								}
								else if ( levelGrid[GRID_LAYER_NORMAL][i][j] == GameElements.CHAR_MINER_4_IDLE )
								{
									if ( GameGlobalVariables.LAB_ENTERED >= 1 )
									{
										tutorialTapComponentsOnLevel.Add ( interactiveObjectMesh.AddComponent < CharacterTapTutorialControl > ());
									}
								}
							}							
						}
					}
				}
			}
		}
	}
	
	public GameObject createFreeObjectOnPosition ( int objectID, int[] position )
	{
		GameObject interactiveObjectInstant = null;
		GameObject interactiveObjectMesh = null;
		
		interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ) + 6.5f, ( float ) position[1] ), _tileInteractivePrefab.transform.rotation );
		interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
		interactiveObjectMesh.transform.localPosition = Vector3.zero;
		interactiveObjectMesh.transform.localScale = Vector3.one;
		
		interactiveObjectMesh.tag = GlobalVariables.Tags.INTERACTIVE;
		interactiveObjectMesh.renderer.material.mainTexture = gameElements[objectID];
		
		interactiveObjectMesh.AddComponent < SelectedComponenent > ();
		IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
		currentIComponent.myID = objectID;
		
		interactiveObjectMesh.collider.enabled = true;
		
		return interactiveObjectInstant;
	}
	
	public GameObject createObjectOnPosition ( int objectID, int[] position )
	{
		GameObject interactiveObjectInstant = null;
		GameObject interactiveObjectMesh = null;
		
		interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) position[0], ( LEVEL_HEIGHT - position[1] ), ( float ) position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
		interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
		interactiveObjectMesh.transform.localPosition = Vector3.zero;
		interactiveObjectMesh.transform.localScale = Vector3.one;
		
		interactiveObjectMesh.tag = GlobalVariables.Tags.INTERACTIVE;
		
		if ( levelGrid[GRID_LAYER_NORMAL][position[0]][position[1]] != GameElements.EMPTY )
		{
			int[] emptyTile = ToolsJerry.findClossestEmptyTileAroundThisTileLaboratory ( position[0], position[1], position, objectID, interactiveObjectInstant, 1 ); 
			if ( emptyTile[0] != -1 )
			{
				interactiveObjectInstant.transform.position = new Vector3 (( float ) emptyTile[0], ( LEVEL_HEIGHT - emptyTile[1] ), ( float ) emptyTile[1] - 0.5f );
				position = ToolsJerry.cloneTile ( emptyTile );
			}
			else
			{
				Destroy ( interactiveObjectInstant );
				return null;
			}
		}
		
		interactiveObjectMesh.renderer.material.mainTexture = gameElements[objectID];
		
		interactiveObjectMesh.AddComponent < SelectedComponenent > ();
		IComponent currentIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
		currentIComponent.myID = objectID;
		
		interactiveObjectMesh.collider.enabled = true;
		
		FLGridReservationManager.getInstance ().fillTileWithMe ( objectID, position[0], position[1], interactiveObjectInstant, objectID );
		return interactiveObjectInstant;
	}
	
	public bool isTileInLevelBoudaries ( int x, int z )
	{
		if (( x < 0 ) || ( x > LEVEL_WIDTH - 1 ) || ( z < 0 ) || ( z > LEVEL_HEIGHT - 1 )) return false;
		return true;
	}
	
	public bool allPathIsWalkableForCharacter ( int characterID, GameObject characterObject, int[][] path )
	{
		bool mayGo = true;
		foreach ( int[] tile in path )
		{
			if ( tile == null ) break;
			if ( Array.IndexOf ( GameElements.CHARACTERS, levelGrid[LevelControl.GRID_LAYER_NORMAL][tile[0]][tile[1]] ) != -1 )
			{
				mayGo = true;
			}
			else if ( ! FLGridReservationManager.getInstance ().fillTileWithMe ( characterID, tile[0], tile[1], characterObject, characterID, true ))
			{
				mayGo = false;
				break;
			}
		}
		
		return mayGo;
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
}
