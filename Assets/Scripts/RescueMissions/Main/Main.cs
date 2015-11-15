using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour 
{
	//*************************************************************//
	public delegate void HandleChoosenCharacter ( CharacterData characterChoosen, bool onlyUnblocking = false );
	//*************************************************************//	
	public GameObject rockParticles;
	public GameObject tentacleParticles;
	public GameObject attackUIComboPrefab;
	public GameObject particlesPower;
	//*************************************************************//	
	private int _currentActionType;
	private DragComponent _currentRedirectorDragComponent;
	private IComponent _currentObjectIComponent;
	private CharacterData _currentCharacter;
	public CharacterData _lastChar;
	private IComponent _currentCharacterIComponent;
	private CharacterReloactionComponent.HandleRelocate _currentRelocateCallBackWithPosition;
	private bool _missionFailedScreenShowed = false;
	//*************************************************************//	
	private static Main _meInstance;
	public static Main getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < Main > ();
		}
		
		return _meInstance;
	}

	//*************************************************************//
	
	void Awake () 
	{
#if UNITY_EDITOR
		//Camera.main.orthographicSize = 7f;
#endif		
		GlobalVariables.READY_FOR_NEXT_TURN = true;
//		print ("Ok Again");
		GlobalVariables.resetStaticValues ();
		AStar.init ( LevelControl.LEVEL_WIDTH, LevelControl.LEVEL_HEIGHT );
		
		GameGlobalVariables.CURRENT_GAME_PART = GameGlobalVariables.RESCUE;	

		GameGlobalVariables.Stats.SCORE = 0;
	}

	public CharacterData CurrentChar ()
	{
		return _currentCharacter; 
	}

	public void setChar (CharacterData newChar)
	{
		_currentCharacter = newChar; 
	}

	void Update () 
	{
	
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))

#else
		if (Input.touchCount != 0) {
		if(Input.touches[0].phase == TouchPhase.Began)

#endif
		{
			if(GlobalVariables.READY_FOR_NEXT_TURN == false)
			{
				if(CheckInputType.getInstance().lastType != CheckInputType.DUALTOUCH_TYPE && CheckInputType.getInstance().lastType !=CheckInputType.DRAG_TYPE)
				{
					print ("YO");
					MovesWarningCreator.getInstance().StartCoroutine("ShowWarning");
				}
			}
		}
#if UNITY_ANDROID && !UNITY_EDITOR
		}
#endif


//		print ("Ready is " + GlobalVariables.READY_FOR_NEXT_TURN);
#if UNITY_EDITOR
		/*
		Vector3 hitPositionTrace = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
		if ( LevelControl.getInstance ().isTileInLevelBoudaries ( Mathf.RoundToInt ( hitPositionTrace.x ), Mathf.RoundToInt ( hitPositionTrace.z )))
		{
			//GameObject objectunderMouse = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )];
			GameObject objectunderMouse02 = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_BEAM][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )];
			//print ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )] + " | "+ (( objectunderMouse != null ) ? objectunderMouse.name : "null" ) );
			print ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][Mathf.RoundToInt ( hitPositionTrace.x )][Mathf.RoundToInt ( hitPositionTrace.z )] + " | "+ (( objectunderMouse02 != null ) ? objectunderMouse02.name : "null" ) );
		}
		*/
#endif	
		if ( GlobalVariables.POPUP_UI_SCREEN || GlobalVariables.UI_CLICKED || GlobalVariables.CHARACTER_PASSING_ANIMATION ) return;
		if(CheckInputType.getInstance().touchType != CheckInputType.TAP_TYPE)
		{
			return;
		}

#if UNITY_EDITOR
		if (( _currentCharacter != null ) && ! _currentCharacter.interactAction && ! _currentCharacter.blocked && ( Input.GetMouseButtonUp ( 0 )))
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
			Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
#else
		if (( _currentCharacter != null ) && ! _currentCharacter.interactAction && ! _currentCharacter.blocked  && ( Input.touchCount > 0 ) && ( Input.touches[0].phase == TouchPhase.Ended ))
		{
			GameObject hitGameObject = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
			Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.touches[0].position );
#endif		
			
			int[] positionToCheck = null;
			if ( hitGameObject.tag == GlobalVariables.Tags.INTERACTIVE )
			{
				positionToCheck = new int[2] { Mathf.RoundToInt ( hitGameObject.transform.root.position.x ), Mathf.RoundToInt ( hitGameObject.transform.root.position.z + 0.5f )};
			}
			else
			{
				positionToCheck = new int[2] { Mathf.RoundToInt ( hitPosition.x ), Mathf.RoundToInt ( hitPosition.z )};
			}
			
			if ( ! GlobalVariables.checkForMenus ())
			{
				if ( LevelControl.getInstance ().isTileInLevelBoudaries ( positionToCheck[0], positionToCheck[1]))
				{
					if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]] != null )
					{
						if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].transform.Find ( "tile" ).gameObject.tag == GlobalVariables.Tags.INTERACTIVE )
						{
							LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][positionToCheck[0]][positionToCheck[1]].transform.Find ( "tile" ).gameObject.SendMessage ( "OnMouseUp", SendMessageOptions.DontRequireReceiver );
						}
					}
				}
			}
				
			if ( hitGameObject != null )
			{
				AttackerComponent currentAttackerComponent = hitGameObject.GetComponent < AttackerComponent > ();
				CharacterReloactionComponent currentCharacterReloactionComponent = hitGameObject.GetComponent < CharacterReloactionComponent > ();
				
				// I AM CLICKING ALREADY SELECTED CHARACTER THE SAME
				if (( currentAttackerComponent != null ) && ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]] && LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]].gameObject == currentAttackerComponent.transform.root.gameObject ))
				{
					// do nothing;
						print ("SHITEHAWK 1");
				}
				// I AM CLICKING ALREADY SELECTED CHARACTER THE SAME
				else if (( currentCharacterReloactionComponent != null ) && ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]] && LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]].gameObject == currentCharacterReloactionComponent.transform.root.gameObject ))
				{
					// do nothing
						print ("SHITEHAWK 2");
				}
				else
				{
					// I AM CLICKING FREE SPACE FOR THAT CHACRACTER
				/*if(GlobalVariables.READY_FOR_NEXT_TURN == true)
				{
					return;
				}*/
					if ( GridReservationManager.getInstance ().fillTileWithMe ( _currentCharacter.myID, positionToCheck[0], positionToCheck[1], LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacterIComponent.position[0]][_currentCharacterIComponent.position[1]], _currentCharacter.myID, true ))
					{
						if ( ! GlobalVariables.TUTORIAL_MENU )
						{
							int[][] returnedPath = 	AStar.search ( _currentCharacterIComponent.position, positionToCheck, false, _currentCharacterIComponent.myID, _currentCharacterIComponent.transform.root.gameObject );
							
							if (( returnedPath != null ) && ( ToolsJerry.compareTiles ( returnedPath[0], _currentCharacterIComponent.position )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( _currentCharacter.myID, _currentCharacterIComponent.transform.root.gameObject, returnedPath ) && ( ! GlobalVariables.SCREEN_DRAGGING && ! GlobalVariables.UI_CLICKED ))
							{
								if(GlobalVariables.READY_FOR_NEXT_TURN == false)
								{
									//DAVE LOCATOR
									return;
								}
								//GlobalVariables.READY_FOR_NEXT_TURN = false;
								print ("This One");
								Main.getInstance ().removeOneMove ();
									//Lock players from being used now// DAVE SOLO
								MessageCenter.getInstance ().createGoodMarkInPosition ( positionToCheck );
								_currentRelocateCallBackWithPosition ( hitPosition );
								if ( SelectedComponenent.CURRENT_SELECTED_OBJECT ) SelectedComponenent.CURRENT_SELECTED_OBJECT.setSelected ( false );
							}
							else if (( returnedPath == null ) || ( ! ToolsJerry.compareTiles ( returnedPath[0], _currentCharacterIComponent.position )))
							{
								MessageCenter.getInstance ().createMarkInPosition ( positionToCheck );
							}
						}
						else
						{
							int[][] returnedPath = 	AStar.search ( _currentCharacterIComponent.position, positionToCheck, false, _currentCharacterIComponent.myID, _currentCharacterIComponent.transform.root.gameObject );
							
							if (( returnedPath != null ) && ( ToolsJerry.compareTiles ( returnedPath[0], _currentCharacterIComponent.position )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( _currentCharacter.myID, _currentCharacterIComponent.transform.root.gameObject, returnedPath ) && ( ! GlobalVariables.SCREEN_DRAGGING && ! GlobalVariables.UI_CLICKED ))
							{
								if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_TAP_SCREEN )
								{
									GameObject myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
									TutorialsManager.getInstance ().disapeareTutorialBox ( myFrameUICombo );
									StartCoroutine ( "destroyOnComplete" );

									MessageCenter.getInstance ().createGoodMarkInPosition ( positionToCheck );
									_currentRelocateCallBackWithPosition ( hitPosition );
								}
								else if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER_BY_TAP )
								{
									if ( ToolsJerry.compareTiles ( positionToCheck, TutorialsManager.getInstance ().getCurrentTutorialStep ().targetTiles[0] ))
									{
										MessageCenter.getInstance ().createGoodMarkInPosition ( positionToCheck );
										_currentRelocateCallBackWithPosition ( hitPosition );
									}
								}
							}
						}
					}
				}
			}
				
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}	
	}

	private IEnumerator destroyOnComplete ()
	{
		GameObject myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
		yield return new WaitForSeconds ( 0.1f );
		Destroy ( myFrameUICombo );
		TutorialsManager.getInstance ().goToNextStep ();
	}
			
	private void cancelSelectedCharacterAndEnemy ()
	{	
		if(LevelControl.getInstance() != null)
		{
			foreach ( MNEnemyComponent enemyComponent in LevelControl.getInstance ().enemiesOnLevel )
			{
				if ( enemyComponent == null ) continue;
				enemyComponent.GetComponent < SelectedComponenent > ().setSelected ( false );
			}
		}
	}
			
	public void unselectCurrentCharacter ()
	{
			print ("happened?");
		if ( _currentCharacterIComponent == null ) return;
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacterIComponent.position[0]][_currentCharacterIComponent.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( false );		
	}
	
	public void deselectAllChars()
	{
			print ("happened?");
		_currentCharacter.selected = false;
		_currentCharacter = null;
	}

	public CharacterData getCurrentCharacter()
	{
		return _currentCharacter;
	}

	public void checkForWinConditions ()
	{
		//	print ("WIN CHECK 9");
		if ( /*GlobalVariables.PLUG_CONNECTED &&*/ GlobalVariables.TOY_RESCUED )
		{
			Destroy(FindObjectOfType<CoraMarkerScale>().gameObject);

			InvokeRepeating ( "repatCheckForEndOfCelebretingAnimation", 0.1f, 0.5f );
		}
	}
			
	private void repatCheckForEndOfCelebretingAnimation ()
	{
		if ( GlobalVariables.CHARACTER_CELEBRATING ) return;

//		print ("WIN CHECK 10");
		CancelInvoke ( "repatCheckForEndOfCelebretingAnimation" );
		ResoultScreen.SPECIFIC_FAILED_INFO_KEY = "ui_sign_result_toy_rescued";
		ResoultScreen.WIN_RESOULT = true;
			
		TimerControl.getInstance ().startTimer ( false );
		
		if ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "tile" ).GetComponent < IComponent > ().myID != GameElements.POWER_MOTOR )
		{
			CharacterData toBerescuedCharacterData = LevelControl.getInstance ().getCharacter ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "tile" ).GetComponent < IComponent > ().myID );
			ResoultScreen.getInstance ().character = LevelControl.getInstance ().gameElements[toBerescuedCharacterData.myID];		
		}
		ScoreHandler.getInstance ().DoScoreSequence ();
		//ToyLazarusSequenceControl.getInstance ().initToyLazarusSequence ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "tile" ).GetComponent < IComponent > ().myID );
	}

	public void removeOneMove ()
	{
		GameGlobalVariables.Stats.MOVES--;
		//LevelControl.getInstance ().getSelectedCharacterObjectFromLevel ().GetComponent < ProduceFloatingNumberComponent > ().produceFloatinNumber ( 1 );
	}
	
	public void showFaliedScreen ()
	{
		if ( _missionFailedScreenShowed ) return;
		_missionFailedScreenShowed = true;
		
		ResoultScreen.WIN_RESOULT = false;
		TimerControl.getInstance ().pause(true);
		MNSpawningEnemiesManager.getInstance ().startSpawn = false;
		StartCoroutine ("SlightPause");
	}

	IEnumerator SlightPause ()
	{
		yield return new WaitForSeconds (0f);
		ResoultScreen.getInstance ().CustomStart ();
	}

	public void restorePlay ()
	{
		_missionFailedScreenShowed = false;
		ResoultScreen.WIN_RESOULT = false;
		ResoultScreen.getInstance ().RestoreCamera ();
		TimerControl.getInstance ().pause(false);
		MNSpawningEnemiesManager.getInstance ().startSpawn = true;
	}
		
	public void allowAgainShowFailedScreen ()
	{
		_missionFailedScreenShowed = false;
	}
	
	public bool buildRedirectorCallBack ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		GlobalVariables.MENU_FOR_REDIRECTOR = false;
			
		CharacterData currentSelectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		currentSelectedCharacter.interactAction = true;
		GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT--;

		if ( ! GameGlobalVariables.CUT_DOWN_GAME )
		{
			GameGlobalVariables.Stats.NewResources.REDIRECTORS--;	
		}

		_currentRedirectorDragComponent.GetComponent < RedirectorComponent > ().handlePlacedOnLevel ();
		
		return true;
	}
	
	public bool revokeRedirectorCallBack ()
	{
		if ( _currentRedirectorDragComponent == null ) return false;
		
		CharacterData characterSelected = LevelControl.getInstance ().getSelectedCharacter ();
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterSelected.position[0]][characterSelected.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterSelected.position[0]][characterSelected.position[1]] );
			
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		GlobalVariables.MENU_FOR_REDIRECTOR = false;
		_currentRedirectorDragComponent.revokeFromCurrentPosition ();
		Destroy ( _currentRedirectorDragComponent.transform.root.gameObject );
		
		GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT++;
		GameGlobalVariables.Stats.NewResources.REDIRECTORS++;
		
		RedirectorButton.getInstance ().redirectorIsOnlevel = false;
		return true;
	}
		
	public bool revokeRedirectorCallBackWithoutAdding ()
	{
		CharacterData characterSelected = LevelControl.getInstance ().getSelectedCharacter ();
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterSelected.position[0]][characterSelected.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterSelected.position[0]][characterSelected.position[1]] );
			
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		GlobalVariables.MENU_FOR_REDIRECTOR = false;
		_currentRedirectorDragComponent.revokeFromCurrentPosition ();
		Destroy ( _currentRedirectorDragComponent.transform.root.gameObject );
		
		RedirectorButton.getInstance ().redirectorIsOnlevel = false;
		return true;
	}
	
	public bool noCharacterFoundCallBack ()
	{
		if ( _currentRedirectorDragComponent ) 
		{
			_currentRedirectorDragComponent.revokeFromCurrentPosition ();
			Destroy ( _currentRedirectorDragComponent.transform.root.gameObject );
			GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT++;
			GameGlobalVariables.Stats.NewResources.REDIRECTORS++;
			RedirectorButton.getInstance ().redirectorIsOnlevel = false;
		}
		
		return true;
	}
		
	public bool checkPathClearBetweenRescuerAndToBeRescued ()
	{
		if ( GlobalVariables.CORA_ALREADY_WITH_TOY_ON_TROLEY ) return true;
				
		int[] positionOfRescuer = new int[2] { Mathf.RoundToInt ( LevelControl.getInstance ().rescuerOnLevel.transform.position.x ), Mathf.RoundToInt ( LevelControl.getInstance ().rescuerOnLevel.transform.position.z )}; 
		int[] positionOfToBeRescued = new int[2] { Mathf.RoundToInt ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.position.x ), Mathf.RoundToInt ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.position.z )}; 
	
		int[][] returnedPath = 	AStar.search ( positionOfRescuer, positionOfToBeRescued, true, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][positionOfRescuer[0]][positionOfRescuer[1]], LevelControl.getInstance ().rescuerOnLevel );
			
		if (( returnedPath == null ) || ! LevelControl.getInstance ().allPathIsWalkableForCharacter ( LevelControl.getInstance ().rescuerOnLevel.transform.Find ( "tile" ).GetComponent < IComponent > ().myID, LevelControl.getInstance ().rescuerOnLevel, returnedPath ))
		{
			return false;
		}
		else
		{
			return true;
		}
	}
			
	public void checkForOtherCharacterWithSkill ( CharacterData character, int actionType, bool withBuildRateActionType = false )
	{
		if ( character.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] <= 0 )
		{
			if ( ! checkIfOtherCharacterWithSuchSkill ( actionType ))
			{
				bool continueShowingFailedInfo = false;
				switch ( actionType )
				{
					case CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE:
						if ( withBuildRateActionType )
						{
							if ( ! GlobalVariables.PLUG_CONNECTED ) continueShowingFailedInfo = true;
						}
						break;
				}
				
				if ( continueShowingFailedInfo )
				{				
					ResoultScreen.SPECIFIC_FAILED_INFO_KEY = ( actionType == CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE || actionType == CharacterData.CHARACTER_ACTION_TYPE_DEMOLISH_RATE ) ? GameTextManager.KEY_SCREEN_RESCUEMISSION_FAILED_ENGINEER : "";
					ResoultScreen.CHARACTER_NAME = character.name;
					ResoultScreen.WIN_RESOULT = false;
					ResoultScreen.getInstance ().character = LevelControl.getInstance ().gameElements[GameElements.CHAR_FARADAYDO_1_DRAINED];
					Main.getInstance ().showFaliedScreen ();
				}
			}
		}
	}			
		
	public bool chosenCharacterCallBack ( CharacterData character )
	{
		if ( character.blocked ) return false;

		int[] currentPositionOfCharacter = ToolsJerry.cloneTile ( character.position );
		int[][] returnedPath = null;
		
		int[] returnedClossestTile = ToolsJerry.findClossestEmptyTileAroundThisTile ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], character.position, character.myID, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]], 1, character.position );
		returnedPath = AStar.search ( currentPositionOfCharacter, returnedClossestTile, false, character.myID, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]] );

		MoveComponent moveComponent = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]].GetComponent < MoveComponent > ();
		moveComponent.initMove ( returnedPath, returnedClossestTile, false, character, character.myCallBack );
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().resetObject ();	
			
		return true;
	}
		
	public bool checkIfOtherCharacterWithSuchSkill ( int actionType )
	{
		foreach ( CharacterData characterData in LevelControl.getInstance ().charactersOnLevel )
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
		
		if ( ! GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, x, z, objectToBeDestoryed, ID ))
		{
			print ( "CanNOT fill tile: " + x + " | " + z + " | " + ID + " | " + objectToBeDestoryed.name );
		}
//			print ("Boom");
		if(ID == GameElements.ENEM_TENTACLEDRAINER_01 || ID == GameElements.ENEM_TENTACLEDRAINER_02)
		{
				EnemyData myEnemyData = objectToBeDestoryed.transform.Find("tile").GetComponent<IComponent>().myEnemyData;
				myEnemyData.dieInProgress = false;
				myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] = 2;
				/*print (myEnemyData.dieInProgress);
				print (myEnemyData.attacking);
				print (myEnemyData.name);
				print (myEnemyData.myID);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ENDURANCE]);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_MOVING]);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE]);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_DAMAGE]);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_LENGHT_TIME_BAR]);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_MOVING_RANGE]);
				print (myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_TIME_BAR_SPEED]);*/
			objectToBeDestoryed.SetActive(false);
		}
		else
		{
			Destroy ( objectToBeDestoryed );
		}
	}
	
	public void handlePutRedirector ( DragComponent redirectorsDragComponent )
	{
		if ( _currentRedirectorDragComponent == redirectorsDragComponent ) return;
		_currentRedirectorDragComponent = redirectorsDragComponent;
		GlobalVariables.MENU_FOR_REDIRECTOR = true;
		
		List < GameObject > setOfButtons = new List < GameObject > ();
		setOfButtons.Add ( UIControl.getInstance ().createButton ( "yesButton", -0.7f , GlobalVariables.HEIGHT_OF_UI_OBJECT + 9f, -1f, Vector3.one * ZoomAndLevelDrag.UI_SIZE_FACTOR, UIControl.getInstance ().textureYesUp, UIControl.getInstance ().textureYesDown, buildRedirectorCallBack, _currentRedirectorDragComponent.gameObject ));
		if ( ! GlobalVariables.TUTORIAL_MENU ) setOfButtons.Add ( UIControl.getInstance ().createButton ( "noButton", 0.7f, GlobalVariables.HEIGHT_OF_UI_OBJECT + 9f, -1f, Vector3.one * ZoomAndLevelDrag.UI_SIZE_FACTOR, UIControl.getInstance ().textureNoUp, UIControl.getInstance ().textureNoDown, revokeRedirectorCallBackWithoutAdding, _currentRedirectorDragComponent.gameObject ));
		
		foreach ( GameObject button in setOfButtons )
		{
			button.GetComponent < ButtonManager > ().mySetOfButtons = setOfButtons;
		}
	}
	
	public void handleAttackerChosen ( IComponent attackerIComponent, CharacterData characterDataOfAttacker )
	{
		_currentCharacterIComponent = attackerIComponent;
		_currentCharacter = characterDataOfAttacker;
	}
			
	public void handleCharacterChosen ( CharacterData characterData, CharacterReloactionComponent.HandleRelocate relocateCallBackWithPosition )
	{
		_currentCharacterIComponent = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < IComponent > ();
		_currentCharacter = characterData;
		if ( relocateCallBackWithPosition != null ) _currentRelocateCallBackWithPosition = relocateCallBackWithPosition;
	}
			
	public void handleCharacterChosenGeneral ( CharacterData characterData, CharacterReloactionComponent.HandleRelocate relocateCallBackWithPosition )
	{
		_currentCharacterIComponent = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < IComponent > ();
		_currentCharacter = characterData;
		if ( relocateCallBackWithPosition != null ) _currentRelocateCallBackWithPosition = relocateCallBackWithPosition;
	}
	
	public void handleEnemySelected ( HandleChoosenCharacter callBackOnChoosenCharacter, IComponent iComponentOfObject )
	{
		_currentObjectIComponent = iComponentOfObject;
		
		foreach ( MNEnemyComponent enemyComponent in LevelControl.getInstance ().enemiesOnLevel )
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
	
	public void interactWithCurrentCharacter ( HandleChoosenCharacter callBackOnChoosenCharacter, int actionType, IComponent iComponentOfObject )
	{
		if ( _currentCharacter == null ) return;

		_currentActionType = actionType;
		_currentObjectIComponent = iComponentOfObject;
		_currentCharacter.myCallBack = callBackOnChoosenCharacter;
			
		bool mayDoAction = false;
		int[] returnedClossestTile = ToolsJerry.findClossestEmptyTileAroundThisTile ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], _currentCharacter.position, _currentCharacter.myID, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentCharacter.position[0]][_currentCharacter.position[1]], 1 );
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
				UIControl.getInstance ().createCharacterBox ( _currentCharacter.myID );
				_currentCharacter.myCharacterButton.highlightEnergy ();
			}
		}
			
		if ( ! mayDoAction )
		{
			MessageCenter.getInstance ().createMarkInPosition ( _currentObjectIComponent.position );
			if ( _currentCharacter.myCallBack != null )
			{
				_currentCharacter.myCallBack ( null, true );
			}
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
	
	public void handleChooseCharacter ( HandleChoosenCharacter callBackOnChoosenCharacter, int actionType, IComponent iComponentOfObject )
	{	
		_currentActionType = actionType;
		_currentObjectIComponent = iComponentOfObject;		
			
		List < GameObject > setOfButtons = new List < GameObject > ();
		
		int numberOfSelectedCharacters = 0;
		bool foundOnlyOneCharacterSoAutomaticExecute = false;
		
		if ( actionType == CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE )
		{
			for ( int i = 0; i < LevelControl.getInstance ().charactersOnLevel.Count; i++ )
			{
				if ( LevelControl.getInstance ().charactersOnLevel[i].characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
				{
					if ( LevelControl.getInstance ().charactersOnLevel[i].characterValues[_currentActionType] > 0 )
					{
						if ( ! LevelControl.getInstance ().charactersOnLevel[i].blocked )
						{
							int[] currentPositionOfCharacter = ToolsJerry.cloneTile ( LevelControl.getInstance ().charactersOnLevel[i].position );
							int[] foundTileAroundComponent = ToolsJerry.findClossestEmptyTileAroundThisTile ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], LevelControl.getInstance ().charactersOnLevel[i].position, LevelControl.getInstance ().charactersOnLevel[i].myID, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]], 1 );
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
		
		for ( int i = 0; i < LevelControl.getInstance ().charactersOnLevel.Count; i++ )
		{
			if ( LevelControl.getInstance ().charactersOnLevel[i].characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
			{
				if ( LevelControl.getInstance ().charactersOnLevel[i].characterValues[_currentActionType] > 0 )
				{
					if ( ! LevelControl.getInstance ().charactersOnLevel[i].blocked )
					{
						int[] currentPositionOfCharacter = ToolsJerry.cloneTile ( LevelControl.getInstance ().charactersOnLevel[i].position );
						int[] foundTileAroundComponent = ToolsJerry.findClossestEmptyTileAroundThisTile ( _currentObjectIComponent.position[0], _currentObjectIComponent.position[1], currentPositionOfCharacter, LevelControl.getInstance ().charactersOnLevel[i].myID, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][currentPositionOfCharacter[0]][currentPositionOfCharacter[1]], 1 );
						if (( foundTileAroundComponent[0] != -1 ) || ( ToolsJerry.compareTiles ( _currentObjectIComponent.position, currentPositionOfCharacter )))
						{
							numberOfSelectedCharacters++;
							
							if ( foundOnlyOneCharacterSoAutomaticExecute )
							{
								_currentCharacter = LevelControl.getInstance ().charactersOnLevel[i];
								_currentCharacter.myCallBack = callBackOnChoosenCharacter;
								chosenCharacterCallBack ( _currentCharacter );
							}
							else setOfButtons.Add ( UIControl.getInstance ().createButtonCharacter ( -2f + i * 2f, -3f, GlobalVariables.HEIGHT_OF_UI_OBJECT, Vector3.one * ZoomAndLevelDrag.UI_SIZE_FACTOR, LevelControl.getInstance ().gameElements[LevelControl.getInstance ().charactersOnLevel[i].myID], chosenCharacterCallBack, LevelControl.getInstance ().charactersOnLevel[i] ));
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
			noCharacterFoundCallBack ();
		}
		else
		{
			if ( ! foundOnlyOneCharacterSoAutomaticExecute ) setOfButtons.Add ( UIControl.getInstance ().createButton ( "cancelButton", 0f, -3f, GlobalVariables.HEIGHT_OF_UI_OBJECT + 1f, new Vector3 ( 50f, 50f, 1f ) * ZoomAndLevelDrag.UI_SIZE_FACTOR, null, null, noCharacterFoundCallBack ));
		}
		
		foreach ( GameObject button in setOfButtons )
		{
			button.GetComponent < ButtonManager > ().mySetOfButtons = setOfButtons;
		}
	}
}
