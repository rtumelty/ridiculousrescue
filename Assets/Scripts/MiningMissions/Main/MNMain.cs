using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNMain : MonoBehaviour 
{
	//*************************************************************//	
	public GameObject rockParticles;
	public GameObject tentacleParticles;
	public GameObject attackUIComboPrefab;
	public GameObject particlesPower;
	//*************************************************************//	
	private int _currentActionType;
	private IComponent _currentObjectIComponent;
	private CharacterData _currentCharacter;
	private IComponent _currentCharacterIComponent;
	private CharacterReloactionComponent.HandleRelocate _currentRelocateCallBackWithPosition;
	private bool _missionFailedScreenShowed = false;
	private bool _ommitNextTap = false;
	//*************************************************************//	
	private static MNMain _meInstance;
	public static MNMain getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < MNMain > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//

	//This prevents level blocker when Fara breaks last block while Madra is attacking
	IEnumerator waitForInactiveCharsForPopUp ()
	{
		yield return new WaitForSeconds (0f);
		bool waitForInactiveCharacters = false;
		foreach (CharacterData character in MNLevelControl.CHARACTERS)
		{
			print ("Step 2");
			if(character.interactAction == true)
			{
				waitForInactiveCharacters = true;
			}
		}
		if(waitForInactiveCharacters == false)
		{
			print ("Step 3");
			TutorialsManager.getInstance ().triggerGeneralDialogForMiningTutorialOnlyExit ();
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
			StartCoroutine("waitForInactiveCharsForPopUp");
			print ("Loop Step");
		}
	}

	void Awake () 
	{
#if UNITY_EDITOR
		//Camera.main.orthographicSize = 7f;
#endif		
		MNGlobalVariables.resetStaticValues ();
		AStar.init ( MNLevelControl.LEVEL_WIDTH, MNLevelControl.LEVEL_HEIGHT );
		
		GameGlobalVariables.CURRENT_GAME_PART = GameGlobalVariables.MINING;
		
		SoundManager.getInstance ().switchMusicTo ( SoundManager.MINING_MUSIC );

		int numberOfMiningPLayesd = SaveDataManager.getValue ( SaveDataManager.MINING_PLAYED );
		numberOfMiningPLayesd++;
		SaveDataManager.save ( SaveDataManager.MINING_PLAYED, numberOfMiningPLayesd );
		
		GoogleAnalytics.instance.LogScreen ( "Mining - Start - Level " + MNLevelControl.SELECTED_LEVEL_NAME );
	}

	void Start ()
	{
		TutorialsManager.getInstance ().StartTutorial ();
	}

	void Update () 
	{
#if UNITY_EDITOR
		/*
		Vector3 hitPositionTrace = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
		if ( MNLevelControl.getInstance ().isTileInLevelBoudaries ( Mathf.RoundToInt ( hitPositionTrace.x ), Mathf.RoundToInt ( hitPositionTrace.z )))
		{
			//GameObject objectunderMouse = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )];
			//GameObject objectunderMouse02 = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_BEAM][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )];
			//print ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )] + " | "+ (( objectunderMouse != null ) ? objectunderMouse.name : "null" ) );
			//print ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_BEAM][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )] + " | "+ (( objectunderMouse02 != null ) ? objectunderMouse02.name : "null" ) );
		}
		*/
#endif
		
#if UNITY_EDITOR
		if ( Input.GetMouseButton ( 0 ))
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
#else
		if ( Input.touchCount > 0 )
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
#endif
			if ( hitGameObject.tag == MNGlobalVariables.Tags.RESOURCES )
			{
				hitGameObject.SendMessage ( "handleTouched" );
				_ommitNextTap = true;
			}
		}
		
		if ( MNGlobalVariables.POPUP_UI_SCREEN || MNGlobalVariables.UI_CLICKED || MNGlobalVariables.CHARACTER_PASSING_ANIMATION ) return;
#if UNITY_EDITOR

		if (( _currentCharacter != null ) && _currentCharacter.countTimeToNextPossibleMoveAfterAttack <= 0f && _currentCharacter.myCurrentAttackBar == null && ! _currentCharacter.interactAction && ! _currentCharacter.attacking && ( Input.GetMouseButtonUp ( 0 )) && ! _ommitNextTap )
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
			Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
#else
		if (( _currentCharacter != null ) && ! _currentCharacter.interactAction && ( Input.touchCount > 0 ) && ( Input.touches[0].phase == TouchPhase.Ended ) && ! _ommitNextTap )
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
			Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.touches[0].position );
#endif		
			
			int[] positionToCheck = null;
			if ( hitGameObject.tag == MNGlobalVariables.Tags.INTERACTIVE )
			{
				positionToCheck = new int[2] { Mathf.RoundToInt ( hitGameObject.transform.root.position.x ), Mathf.RoundToInt ( hitGameObject.transform.root.position.z + 0.5f )};
			}
			else
			{
				positionToCheck = new int[2] { Mathf.RoundToInt ( hitPosition.x ), Mathf.RoundToInt ( hitPosition.z )};
			}
			
			if ( ! MNGlobalVariables.checkForMenus ())
			{
				if ( MNLevelControl.getInstance ().isTileInLevelBoudaries ( positionToCheck[0], positionToCheck[1]))
				{
					if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]] != null )
					{
						if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].transform.Find ( "tile" ) != null && MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].transform.Find ( "tile" ).gameObject.tag == MNGlobalVariables.Tags.INTERACTIVE )
						{
							MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].transform.Find ( "tile" ).gameObject.SendMessage ( "OnMouseUp", SendMessageOptions.DontRequireReceiver );
						}
						else if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].transform.Find ( "tile" ) == null && MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].gameObject.tag == MNGlobalVariables.Tags.INTERACTIVE )
						{
							MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].gameObject.SendMessage ( "OnMouseUp", SendMessageOptions.DontRequireReceiver );
						}
					}
				}
			}
				
			if ( hitGameObject != null && hitGameObject.tag != MNGlobalVariables.Tags.RESOURCES )
			{
				AttackerComponent currentAttackerComponent = hitGameObject.GetComponent < AttackerComponent > ();
				MNCharacterReloactionComponent currentCharacterReloactionComponent = hitGameObject.GetComponent < MNCharacterReloactionComponent > ();
				
				// I AM CLICKING ALREADY SELECTED CHARACTER THE SAME
				if (( currentAttackerComponent != null ) && ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]] && MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]].gameObject == currentAttackerComponent.transform.root.gameObject ))
				{
					// do nothing;
				}
				// I AM CLICKING ALREADY SELECTED CHARACTER THE SAME
				else if (( currentCharacterReloactionComponent != null ) && ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]] && MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]].gameObject == currentCharacterReloactionComponent.transform.root.gameObject ))
				{
					// do nothing
				}
				else
				{
					// I AM CLICKING FREE SPACE FOR THAT CHACRACTER
					if ( MNGridReservationManager.getInstance ().fillTileWithMe ( _currentCharacter.myID, positionToCheck[0], positionToCheck[1], MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacterIComponent.position[0]][_currentCharacterIComponent.position[1]], _currentCharacter.myID, true ))
					{
						if ( ! MNGlobalVariables.TUTORIAL_MENU )
						{
							int[][] returnedPath = 	AStar.search ( _currentCharacterIComponent.position, positionToCheck, false, _currentCharacterIComponent.myID, _currentCharacterIComponent.transform.root.gameObject );
							
							if (( returnedPath != null ) && ( ToolsJerry.compareTiles ( returnedPath[0], _currentCharacterIComponent.position )) && MNLevelControl.getInstance ().allPathIsWalkableForCharacter ( _currentCharacter.myID, _currentCharacterIComponent.transform.root.gameObject, returnedPath ) && ( ! MNGlobalVariables.SCREEN_DRAGGING && ! MNGlobalVariables.UI_CLICKED ))
							{
								MessageCenter.getInstance ().createGoodMarkInPosition ( positionToCheck );
								_currentRelocateCallBackWithPosition ( hitPosition );
								if ( SelectedComponenent.CURRENT_SELECTED_OBJECT ) SelectedComponenent.CURRENT_SELECTED_OBJECT.setSelected ( false );
							}
							else if (( returnedPath == null ) || ( ! ToolsJerry.compareTiles ( returnedPath[0], _currentCharacterIComponent.position )))
							{
								MessageCenter.getInstance ().createMarkInPosition ( positionToCheck );
							}
						}
					}
				}
			}
				
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
#if UNITY_EDITOR
		else if ( Input.GetMouseButtonUp ( 0 ))
		{
#else
		else if (( Input.touchCount > 0 ) && ( Input.touches[0].phase == TouchPhase.Ended ))
		{
#endif		
			_ommitNextTap = false;			
		}
	}
			
	private void cancelSelectedCharacterAndEnemy ()
	{	
		foreach ( MNEnemyComponent enemyComponent in MNLevelControl.getInstance ().enemiesOnLevel )
		{
			if ( enemyComponent == null ) continue;
			enemyComponent.GetComponent < SelectedComponenent > ().setSelected ( false );
		}
	}
			
	public void unselectCurrentCharacter ()
	{
		if ( _currentCharacterIComponent == null ) return;
		MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacterIComponent.position[0]][_currentCharacterIComponent.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().MN_setSelectedCharacter ( false );		
	}

	public void checkForWinConditions ()
	{
		InvokeRepeating ( "repatCheckForEndOfCelebretingAnimation", 0f, 0.5f );
	}
	
	private void repatCheckForEndOfCelebretingAnimation ()
	{
		if ( MNGlobalVariables.CHARACTER_CELEBRATING ) return;

		CancelInvoke ( "repatCheckForEndOfCelebretingAnimation" );

		MNResoultScreen.getInstance ().startResoultScreen ();
	}
	
	public void showFaliedScreen ()
	{
		if ( _missionFailedScreenShowed ) return;
		_missionFailedScreenShowed = true;
		
		MNResoultScreen.getInstance ().startResoultScreen ();
	}
	
	public bool chosenCharacterCallBack ( CharacterData character )
	{
		if ( character.blocked ) return false;
		int[] currentPositionOfCharacter = ToolsJerry.cloneTile ( character.position );

		int[] returnedClossestTile = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], character.position, character.myID, MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]], 1, character.position );
		
		int[][] returnedPath = AStar.search ( currentPositionOfCharacter, returnedClossestTile, false, character.myID, MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]] );
		
		MNMoveComponent moveComponent = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]].GetComponent < MNMoveComponent > ();
		moveComponent.initMove ( returnedPath, returnedClossestTile, false, character, character.myCallBack );
		
		return true;
	}
		
	public bool checkIfOtherCharacterWithSuchSkill ( int actionType )
	{
		foreach ( CharacterData characterData in MNLevelControl.getInstance ().charactersOnLevel )
		{
			if ( characterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
			{
				if ( characterData.characterValues[actionType] > 0 )
				{
					return true;
				}		
			}
		}
		
		return false;
	}
		
	void OnGUI ()
	{
		if ( GameGlobalVariables.RELEASE ) return;
		GUI.Label ( new Rect ( Screen.width - 220f, Screen.height - 30f, 150f, 50f ), GameGlobalVariables.VERSION );
	}
	
	public void handleObjectDestory ( int ID, GameObject objectToBeDestoryed )
	{
		int x = Mathf.RoundToInt ( objectToBeDestoryed.transform.position.x );
		int z = Mathf.RoundToInt ( objectToBeDestoryed.transform.position.z + 0.5f );
		
		if ( ! MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, x, z, objectToBeDestoryed, ID ))
		{
			print ( "CanNOT fill tile: " + x + " | " + z + " | " + ID + " | " + objectToBeDestoryed.name );
		}
				
		Destroy ( objectToBeDestoryed );
	}
	
	public void handleAttackerChosen ( IComponent attackerIComponent, CharacterData characterDataOfAttacker )
	{
		_currentCharacterIComponent = attackerIComponent;
		_currentCharacter = characterDataOfAttacker;
	}
			
	public void handleCharacterChosen ( CharacterData characterData, CharacterReloactionComponent.HandleRelocate relocateCallBackWithPosition )
	{
		_currentCharacterIComponent = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < IComponent > ();
		_currentCharacter = characterData;
		if ( relocateCallBackWithPosition != null ) _currentRelocateCallBackWithPosition = relocateCallBackWithPosition;
	}
			
	public void handleCharacterChosenGeneral ( CharacterData characterData, CharacterReloactionComponent.HandleRelocate relocateCallBackWithPosition )
	{
		_currentCharacterIComponent = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < IComponent > ();
		_currentCharacter = characterData;
		if ( relocateCallBackWithPosition != null ) _currentRelocateCallBackWithPosition = relocateCallBackWithPosition;
	}
	
	public void handleEnemySelected ( Main.HandleChoosenCharacter callBackOnChoosenCharacter, IComponent iComponentOfObject )
	{
		_currentObjectIComponent = iComponentOfObject;
		
		foreach ( MNEnemyComponent enemyComponent in MNLevelControl.getInstance ().enemiesOnLevel )
		{
			if ( enemyComponent == null ) continue;
			if ( iComponentOfObject.gameObject != enemyComponent.gameObject )
			{
				//enemyComponent.setMayBeAttacked ( false );
				enemyComponent.GetComponent < SelectedComponenent > ().setSelected ( false );
			}
		}
		
		if ( _currentCharacter != null ) chosenCharacterCallBack ( _currentCharacter );  
	}
	
	public void interactWithCurrentCharacter ( Main.HandleChoosenCharacter callBackOnChoosenCharacter, int actionType, IComponent iComponentOfObject )
	{
		if ( _currentCharacter == null ) return;
				
		_currentActionType = actionType;
		_currentObjectIComponent = iComponentOfObject;
		_currentCharacter.myCallBack = callBackOnChoosenCharacter;
			
		bool mayDoAction = false;
		int[] returnedClossestTile = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], _currentCharacter.position, _currentCharacter.myID, MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]], 1 );
		
		if ( returnedClossestTile[0] != -1 ) 
		{
			if ( _currentCharacter.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
			{
				if ( _currentCharacter.characterValues[_currentActionType] > 0 )
				{
					if ( ! _currentCharacter.blocked && ! _currentCharacter.interactAction )
					{
						mayDoAction = true;
						chosenCharacterCallBack ( _currentCharacter );
					}
				}
			}
			else
			{
				MNUIControl.getInstance ().createCharacterBox ( _currentCharacter.myID );
				_currentCharacter.myCharacterButton.highlightEnergy ();
			}
		}
			
		if ( ! mayDoAction )
		{
			MessageCenter.getInstance ().createMarkInPosition ( _currentObjectIComponent.position );
			if ( _currentCharacter.myCallBack != null ) _currentCharacter.myCallBack ( null, true );
		}	
	}
		
	public void interactWithParticularCharacter ( CharacterData characterToInteractWith, Main.HandleChoosenCharacter callBackOnChoosenCharacter, int actionType, IComponent iComponentOfObject )
	{
		if ( characterToInteractWith == null ) return;
				
		_currentActionType = actionType;
		_currentObjectIComponent = iComponentOfObject;
		characterToInteractWith.myCallBack = callBackOnChoosenCharacter;
			
		bool mayDoAction = false;
		int[] returnedClossestTile = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], characterToInteractWith.position, characterToInteractWith.myID, MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterToInteractWith.position[0]][characterToInteractWith.position[1]], 1 );
		if ( returnedClossestTile[0] != -1 ) 
		{
			if ( characterToInteractWith.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
			{
				if ( characterToInteractWith.characterValues[_currentActionType] > 0 )
				{
					if ( ! characterToInteractWith.blocked && ! characterToInteractWith.interactAction )
					{
						mayDoAction = true;
						chosenCharacterCallBack ( characterToInteractWith );
					}
				}
				else
				{
					if ( ! characterToInteractWith.blocked && ! characterToInteractWith.interactAction )
					{
						mayDoAction = true;
						chosenCharacterCallBack ( characterToInteractWith );
					}
				}
			}
		}
			
		if ( ! mayDoAction )
		{
			MessageCenter.getInstance ().createMarkInPosition ( _currentObjectIComponent.position );
			if ( characterToInteractWith.myCallBack != null ) characterToInteractWith.myCallBack ( null, true );
		}	
	}
	
	public void handleChooseCharacter ( Main.HandleChoosenCharacter callBackOnChoosenCharacter, int actionType, IComponent iComponentOfObject )
	{	
		_currentActionType = actionType;
		_currentObjectIComponent = iComponentOfObject;		
			
		List < GameObject > setOfButtons = new List < GameObject > ();
		
		int numberOfSelectedCharacters = 0;
		bool foundOnlyOneCharacterSoAutomaticExecute = false;
		
		if ( actionType == CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE )
		{
			for ( int i = 0; i < MNLevelControl.getInstance ().charactersOnLevel.Count; i++ )
			{
				if ( MNLevelControl.getInstance ().charactersOnLevel[i].characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
				{
					if ( MNLevelControl.getInstance ().charactersOnLevel[i].characterValues[_currentActionType] > 0 )
					{
						if ( ! MNLevelControl.getInstance ().charactersOnLevel[i].blocked )
						{
							int[] currentPositionOfCharacter = ToolsJerry.cloneTile ( MNLevelControl.getInstance ().charactersOnLevel[i].position );
							int[] foundTileAroundComponent = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], MNLevelControl.getInstance ().charactersOnLevel[i].position, MNLevelControl.getInstance ().charactersOnLevel[i].myID, MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]], 1 );
							if (( foundTileAroundComponent[0] != -1 ) || ( ToolsJerry.compareTiles ( _currentObjectIComponent.position, currentPositionOfCharacter )))
							{
								numberOfSelectedCharacters++;
							}
						}
					}
				}
			}
			
			if ( numberOfSelectedCharacters == 1 )
			{
				foundOnlyOneCharacterSoAutomaticExecute = true;
			}
		}
		
		numberOfSelectedCharacters = 0;
		
		for ( int i = 0; i < MNLevelControl.getInstance ().charactersOnLevel.Count; i++ )
		{
			if ( MNLevelControl.getInstance ().charactersOnLevel[i].characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
			{
				if ( MNLevelControl.getInstance ().charactersOnLevel[i].characterValues[_currentActionType] > 0 )
				{
					if ( ! MNLevelControl.getInstance ().charactersOnLevel[i].blocked )
					{
						int[] currentPositionOfCharacter = ToolsJerry.cloneTile ( MNLevelControl.getInstance ().charactersOnLevel[i].position );
						int[] foundTileAroundComponent = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], currentPositionOfCharacter, MNLevelControl.getInstance ().charactersOnLevel[i].myID, MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]], 1 );
						if (( foundTileAroundComponent[0] != -1 ) || ( ToolsJerry.compareTiles ( _currentObjectIComponent.position, currentPositionOfCharacter )))
						{
							numberOfSelectedCharacters++;
							
							if ( foundOnlyOneCharacterSoAutomaticExecute )
							{
								_currentCharacter = MNLevelControl.getInstance ().charactersOnLevel[i];
								_currentCharacter.myCallBack = callBackOnChoosenCharacter;
								chosenCharacterCallBack ( _currentCharacter );
							}
						}
					}
				}
			}
		}
		
		if ( numberOfSelectedCharacters == 0 )
		{
			if ( _currentCharacter.interactAction )
			{
				_currentCharacter.interactAction = false;
			}
				
			MessageCenter.getInstance ().createMarkInPosition ( _currentObjectIComponent.position );
		}
		
		foreach ( GameObject button in setOfButtons )
		{
			button.GetComponent < ButtonManager > ().mySetOfButtons = setOfButtons;
		}
	}
}
