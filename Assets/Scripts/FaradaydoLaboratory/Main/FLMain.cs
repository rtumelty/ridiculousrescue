using UnityEngine;
using System.Collections;

public class FLMain : MonoBehaviour 
{
	//*************************************************************//	
	public GameObject particlesTechnogrowth;
	//*************************************************************//	
	private CharacterData _currentCharacter;
	private IComponent _currentCharacterIComponent;
	private FLCharacterRelocationComponent.HandleRelocate _currentRelocateCallBackWithPosition;
	//*************************************************************//	
	private static FLMain _meInstance;
	public static FLMain getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < FLMain > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	
	void Awake () 
	{
		if ( GameGlobalVariables.CUT_DOWN_GAME )
		{
			Camera.main.transform.Find ( "world" ).Find ( "backButton" ).gameObject.SetActive ( false );
			Destroy ( Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "lab" ).gameObject.GetComponent < FLMissionScreenLabNodeControl > ());
		}

		GlobalVariables.resetStaticValues ();
		AStar.init ( FLLevelControl.LEVEL_WIDTH, FLLevelControl.LEVEL_HEIGHT );
		
		GameGlobalVariables.CURRENT_GAME_PART = GameGlobalVariables.LABORATORY;
		GameGlobalVariables.LAB_ENTERED = SaveDataManager.getValue ( SaveDataManager.LABORATORY_ENTERED );

		if ( ! FLMissionRoomManager.AFTER_INTRO )
		{
			if ( ! GameGlobalVariables.BLOCK_LAB_ENTERED )
			{
				GameGlobalVariables.LAB_ENTERED++;
			}
		}

		SaveDataManager.save ( SaveDataManager.LABORATORY_ENTERED, GameGlobalVariables.LAB_ENTERED );
		
		SoundManager.getInstance ().switchMusicTo ( SoundManager.MAP_MUSIC );
	}
	
	void Start ()
	{
		FLFactoryRoomManager.getInstance ().CustomStart ();
		FLMissionRoomManager.getInstance ().CustomStart ();
		TutorialsManager.getInstance ().StartTutorial ();
	}
	
	void Update ()
	{
		if ( FLGlobalVariables.POPUP_UI_SCREEN || FLGlobalVariables.UI_CLICKED ) return;
#if UNITY_EDITOR
		if (( _currentCharacter != null ) && ( Input.GetMouseButtonUp ( 0 )))
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
			Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
#else
		if (( _currentCharacter != null ) && ( Input.touchCount > 0 ) && ( Input.touches[0].phase == TouchPhase.Ended ))
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
			Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.touches[0].position );
#endif		
			int[] positionToCheck = null;
			positionToCheck = new int[2] { Mathf.RoundToInt ( hitPosition.x ), Mathf.RoundToInt ( hitPosition.z )};
				
			if ( hitGameObject != null )
			{
				FLCharacterRelocationComponent currentCharacterReloactionComponent = hitGameObject.GetComponent < FLCharacterRelocationComponent > ();
				
				// I AM CLICKING ALREADY SELECTED CHARACTER THE SAME
				if (( currentCharacterReloactionComponent != null ) && ( FLLevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]] && FLLevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]].gameObject == currentCharacterReloactionComponent.transform.root.gameObject ))
				{
					// do nothing
				}
				else
				{
					// I AM CLICKING FREE SPACE FOR THAT CHACRACTER
					if ( FLGridReservationManager.getInstance ().fillTileWithMe ( _currentCharacter.myID, positionToCheck[0], positionToCheck[1], FLLevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacterIComponent.position[0]][_currentCharacterIComponent.position[1]], _currentCharacter.myID, true ))
					{
						_currentRelocateCallBackWithPosition ( hitPosition );
									
						int[][] returnedPath = AStar.search ( _currentCharacterIComponent.position, positionToCheck, false, _currentCharacterIComponent.myID, _currentCharacterIComponent.transform.root.gameObject );
						
						if (( returnedPath != null ) && ( ToolsJerry.compareTiles ( returnedPath[0], _currentCharacterIComponent.position )) && FLLevelControl.getInstance ().allPathIsWalkableForCharacter ( _currentCharacter.myID, _currentCharacterIComponent.transform.root.gameObject, returnedPath )) 
						{
							MessageCenter.getInstance ().createGoodMarkInPosition ( positionToCheck );
						}
					}
				}
			}
		}
	}	
	
	public void handleObjectDestory ( int ID, GameObject objectToBeDestoryed, int[] position )
	{
		if ( ! FLGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, position[0], position[1], objectToBeDestoryed, ID ))
		{
			print ( 0 );
		}
		
		GameObject particlesInstant = ( GameObject ) Instantiate ( particlesTechnogrowth, objectToBeDestoryed.transform.position, Quaternion.identity ); 
		particlesInstant.AddComponent < AutoDestruct > ();
				
		Destroy ( objectToBeDestoryed );
	}
	
	void OnGUI ()
	{
		if ( GameGlobalVariables.RELEASE ) return;
		GUI.Label ( new Rect ( Screen.width - 220f, Screen.height - 30f, 150f, 50f ), GameGlobalVariables.VERSION );
	}
	
	public void unselectCurrentCharacter ()
	{
		if ( _currentCharacterIComponent == null ) return;
		
		FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][_currentCharacterIComponent.position[0]][_currentCharacterIComponent.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().FL_setSelectedCharacter ( false );		
		_currentCharacter = null;
		_currentCharacterIComponent = null;
	}
	
	public void handleCharacterChosen ( CharacterData characterData, FLCharacterRelocationComponent.HandleRelocate relocateCallBackWithPosition )
	{
		_currentCharacterIComponent = FLLevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < IComponent > ();
		_currentCharacter = characterData;
		if ( relocateCallBackWithPosition != null ) _currentRelocateCallBackWithPosition = relocateCallBackWithPosition;
	}
}
