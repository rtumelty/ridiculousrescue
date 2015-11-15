using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RescuerComponent : ObjectTapControl 
{
	//*************************************************************//
	public const int DIRECTION_UP = 0;
	public const int DIRECTION_DOWN = 1;
	public const int DIRECTION_RIGHT = 2;
	public const int DIRECTION_LEFT = 3;
	//*************************************************************//
	public static Vector3 CORA_WITHOUT_TROLEY_SCALE = new Vector3 ( 3f, 3f, 3f );
	public static Vector3 CORA_WITH_TROLEY_SCALE = new Vector3 ( 3f, 3f, 3f );
	
	public static GameObject CHARACTER_HIT;
	//*************************************************************//
	private bool _withToyTobeRescued = false;
	private GameObject _tileMarkerPrefab;
	private IComponent _myIComponent;
	private bool _mouseDownOnMe = false;
	private GameObject _toBeRescuedObject;
	public int[] _positionOfToBeRescued;
	private int _toBeRescuedID;
	private bool _playedCelebrationAnimation = false;
	private float myMagnitudeMeasure = 0f;
	private float myCounter = 0f;
	private int _lastDirectionState;
#if UNITY_EDITOR
	private Vector3 _lastMousePosition;
#endif
	private int _countMoves = 0;
	private bool freshSwipeHasBegun = false;
	//*************************************************************//	
	IEnumerator Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		//Camera.main.GetComponent<CameraMovement> ().centreOnMe (gameObject.transform);

		yield return new WaitForSeconds ( 0.1f );
		if ( _myIComponent.myCharacterData.myID == GameElements.CHAR_CORA_1_IDLE && LevelControl.LEVEL_ID == 3 )
		{
			gameObject.SendMessage ( "handleTouchedFromStart" );
		}
	}
	
	void Update () 
	{
		myCounter -= Time.deltaTime;
//		print (freshSwipeHasBegun);

		if ( GlobalVariables.CHARACTER_IN_MOVE )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButtonDown ( 0 ))
			{
				//_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
			}

			if ( Input.GetMouseButton ( 0 ))
			{
#else
			if ( Input.touchCount == 1 )
			{
#endif
			}
			else
			{
				GlobalVariables.CORA_SWIPED = false;
				_mouseDownOnMe = false;
			}
			
			return;
		}
			
		if ( ! _myIComponent.myCharacterData.selected )
		{
			return;
		}

		if ( _withToyTobeRescued )
		{
			if (( ! _playedCelebrationAnimation ) && ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_RESCUEZONE][_positionOfToBeRescued[0]][_positionOfToBeRescued[1]] != GameElements.EMPTY ))
			{
				if ( ! GlobalVariables.TUTORIAL_MENU )
				{
					if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_DUMMY )
					{
						TutorialsManager.getInstance ().goToNextStep ();
					
						if ( LevelControl.LEVEL_ID != 1 ) return;
						else
						{
							Destroy ( TutorialsManager.getInstance ().getCurrentTutorialUICombo ());
						}
					}

					if ( LevelControl.LEVEL_ID == 3 )
					{
						if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_STAY_SIGN_TILL_WIN_LEVEL )
						{
							GameObject _myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
							Destroy ( _myFrameUICombo );
							
							if ( _countMoves <= 3 ) TutorialsManager.getInstance ().goToNextStep ();
							else TutorialsManager.getInstance ().goToNextStep ( 0, 1 );
							return;
						}
					}

					leaveTroley ();
					
					_toBeRescuedObject.transform.Find ( "tile" ).GetComponent < ToBeRescuedComponent > ().iAmRescued = true;	
					_toBeRescuedObject.transform.Find ( "tile" ).collider.enabled = false;
							
					SoundManager.getInstance ().playSound ( SoundManager.RESCUE_TOY_AT_MARKER );
					_playedCelebrationAnimation = true;
					TimerControl.getInstance().startTimer(false);
					GlobalVariables.CORA_SWIPED = false;
					_alreadyTouched = true;
					GlobalVariables.TOY_RESCUED = true;
					
					gameObject.GetComponent < SelectedComponenent > ().setSelectedForCelebration ( true );
					Main.getInstance ().checkForWinConditions ();
//					print ("WIN CHECK 8");
					//GameGlobalVariables.Stats.SCORE += GameGlobalVariables.Stats.Scores.FOR_RESCUE_TOY * GameGlobalVariables.Stats.MOVES;

					if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_STAY_SIGN_TILL_WIN_LEVEL )
					{
						if ( LevelControl.LEVEL_ID == 8 )
						{
							GameObject _myFrameUICombo = TutorialsManager.getInstance ().getCurrentTutorialUICombo ();
							Destroy ( _myFrameUICombo );
						}
					}				
				}
			}
		}

		/*if(freshSwipeHasBegun == false)
		{
			return;
		}*/

		if ( ! _alreadyTouched )
		{
			if ( _myIComponent.myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] <= 0 )
			{
				ResoultScreen.SPECIFIC_FAILED_INFO_KEY = GameTextManager.KEY_SCREEN_RESCUEMISSION_FAILED_CORA;
				ResoultScreen.CHARACTER_NAME = _myIComponent.myCharacterData.name;
				ResoultScreen.getInstance ().character = LevelControl.getInstance ().gameElements[GameElements.CHAR_CORA_1_DRAINED];
				Main.getInstance ().showFaliedScreen ();
			}
		}
		else return;
		
		if (( _withToyTobeRescued ) && ( _mouseDownOnMe ))
		{
			GlobalVariables.CORA_SWIPED = true;
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
			{
				if ( _lastMousePosition != Input.mousePosition )
				{
						//print ("Last" + _lastMousePosition);
						//print ("Current" + Input.mousePosition);
						//print (" Difference" + Vector3.Distance ( _lastMousePosition, Input.mousePosition ));
					if ( Vector3.Distance ( _lastMousePosition, Input.mousePosition ) > 40f )
					{
						int xDirection = (int) ( Input.mousePosition.x - _lastMousePosition.x );
						int zDirection = (int) ( Input.mousePosition.y - _lastMousePosition.y );
#else
			if ( Input.touchCount == 1 )
			{																						//This 0 value was 18 before edit.
				if ( Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].deltaPosition.magnitude > 0f)
				{
									myMagnitudeMeasure += Input.touches[0].deltaPosition.magnitude;

									int xDirection = (int) Input.touches[0].deltaPosition.x;
									int zDirection = (int) Input.touches[0].deltaPosition.y;

					if ( myMagnitudeMeasure >= 40f )
					{
						myMagnitudeMeasure = 0f;
#endif
	
						if ( GlobalVariables.TUTORIAL_MENU && LevelControl.LEVEL_ID != 8 )
						{
							int xDirectionTutorial = 0;
							int zDirectionTutorial = 0;

							if (  gameObject.GetComponent < TutorialMoveCharacterComponent > () != null )
							{
								xDirectionTutorial = (int) ( gameObject.GetComponent < TutorialMoveCharacterComponent > ().targetTiles[0][0] - _positionOfToBeRescued[0]);
								zDirectionTutorial = (int) ( gameObject.GetComponent < TutorialMoveCharacterComponent > ().targetTiles[0][1] - _positionOfToBeRescued[1] );
							}
							if ( Mathf.Abs ( xDirection ) > Mathf.Abs ( zDirection ))
							{
								if (( xDirection < 0 ) && ( xDirectionTutorial < 0 )) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_LEFT );
								else if ( xDirectionTutorial > 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_RIGHT );
							}
							else
							{
								if (( zDirection < 0 ) && ( zDirectionTutorial < 0 )) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_DOWN );
								else if ( zDirectionTutorial > 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_UP );
							}				
										
						}					
						else
						{
							if ( Mathf.Abs ( xDirection ) > Mathf.Abs ( zDirection ))
							{
								if ( xDirection < 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_LEFT );
								else  findEmptyTileForMeWithTroleyInDirection ( DIRECTION_RIGHT );
							}
							else
							{
								if ( zDirection < 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_DOWN );
								else  findEmptyTileForMeWithTroleyInDirection ( DIRECTION_UP );
							}
						}
					}
				}
			}
			else
			{
				GlobalVariables.CORA_SWIPED = false;
				_mouseDownOnMe = false;
			}
						
#if UNITY_EDITOR
			//_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
		}
		else if (( _withToyTobeRescued ) && ( ! _mouseDownOnMe ))
		{
							return; //prevents swipe during screen drag.
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
			{
				if ( _lastMousePosition != Input.mousePosition )
				{
					if ( Vector3.Distance ( _lastMousePosition, Input.mousePosition ) > 100f )
					{
						int xDirection = (int) ( Input.mousePosition.x - _lastMousePosition.x );
						int zDirection = (int) ( Input.mousePosition.y - _lastMousePosition.y );
						Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
#else
			if ( Input.touchCount == 1 )
			{
				if ( Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].deltaPosition.magnitude > 0f)
				{
					myMagnitudeMeasure += Input.touches[0].deltaPosition.magnitude;
					
					int xDirection = (int) Input.touches[0].deltaPosition.x;
					int zDirection = (int) Input.touches[0].deltaPosition.y;
					Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.touches[0].position );
					
					if ( myMagnitudeMeasure >= 40f )
					{	
						myMagnitudeMeasure = 0f;
#endif
						if ( Vector3.Distance ( new Vector3 ( hitPosition.x, 0f, hitPosition.z ), new Vector3 ( transform.position.x, 0f, transform.position.z )) <= 2.5f )
						{

							if ( GlobalVariables.TUTORIAL_MENU )
							{
								int xDirectionTutorial = 0;
								int zDirectionTutorial = 0;

								if (  gameObject.GetComponent < TutorialMoveCharacterComponent > () != null )
								{
									xDirectionTutorial = (int) ( gameObject.GetComponent < TutorialMoveCharacterComponent > ().targetTiles[0][0] - _positionOfToBeRescued[0]);
									zDirectionTutorial = (int) ( gameObject.GetComponent < TutorialMoveCharacterComponent > ().targetTiles[0][1] - _positionOfToBeRescued[1] );
								}

								if ( Mathf.Abs ( xDirection ) > Mathf.Abs ( zDirection ))
								{
									if (( xDirection < 0 ) && ( xDirectionTutorial < 0 )) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_LEFT );
									else if ( xDirectionTutorial > 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_RIGHT );
								}
								else
								{
									if (( zDirection < 0 ) && ( zDirectionTutorial < 0 )) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_DOWN );
									else if ( zDirectionTutorial > 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_UP );
								}				
							}					
							else
							{
								if ( Mathf.Abs ( xDirection ) > Mathf.Abs ( zDirection ))
								{
									if ( xDirection < 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_LEFT );
									else  findEmptyTileForMeWithTroleyInDirection ( DIRECTION_RIGHT );
								}
								else
								{
									if ( zDirection < 0 ) findEmptyTileForMeWithTroleyInDirection ( DIRECTION_DOWN );
									else  findEmptyTileForMeWithTroleyInDirection ( DIRECTION_UP );
								}
							}
						}
					}
				}
			}
		}	
	}
					
	public void leaveTroley ()
	{
		gameObject.GetComponent < CharacterReloactionComponent > ().blockMove = false;
		_myIComponent.myCharacterData.interactAction = false;
//=======================================Daves Edit==========================================
		_myIComponent.myCharacterData._interactTrolley = false;
//=======================================Daves Edit==========================================
		_myIComponent.myCharacterData.blocked = false;
									
		_toBeRescuedObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().stopTween ();
		
		_toBeRescuedObject.transform.localPosition = new Vector3 ( 0f, 0.5f, 0f );
		_toBeRescuedObject.transform.parent = null;

		transform.localPosition = new Vector3 ( 0f, 0.45f, 0f );
		gameObject.GetComponent < SelectedComponenent > ().updateInitialValues ();
		transform.parent.position = new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f );
		
		GameObject troleyObject = LevelControl.getInstance ().createObjectOnPosition ( GameElements.ITEM_TROLEY_LEFT_1, _positionOfToBeRescued );
		troleyObject.name = "troley";
		_toBeRescuedObject.SetActive ( true );
		_toBeRescuedObject.transform.position += Vector3.up;

		troleyObject.transform.parent = _toBeRescuedObject.transform;

		GlobalVariables.CORA_ALREADY_WITH_TOY_ON_TROLEY = _withToyTobeRescued = false;				
		_toBeRescuedObject.transform.Find ( "tile" ).gameObject.SendMessage ( "unBlockOnTroley" );
	}
				
	private void findEmptyTileForMeWithTroleyInDirection ( int direction )
	{
		int addX = 0;
		int addZ = 0;
		
		switch ( direction )
		{
			case DIRECTION_UP:
				addZ = 1;
				addX = 0;
				break;
			case DIRECTION_DOWN:
				addZ = -1;
				addX = 0;
				break;
			case DIRECTION_RIGHT:
				addZ = 0;
				addX = 1;
				break;
			case DIRECTION_LEFT:
				addZ = 0;
				addX = -1;
				break;
		}
		
		int addedX = addX;
		int addedZ = addZ;
		
		int[] emptyTile = new int[2] { -1, -1 };
		
		while (( LevelControl.getInstance ().isTileInLevelBoudaries ( _positionOfToBeRescued[0] + addedX, _positionOfToBeRescued[1] + addedZ )) && ( LevelControl.getInstance ().isTileEmptyForRescuerWithTroley ( _positionOfToBeRescued[0] + addedX, _positionOfToBeRescued[1] + addedZ )))
		{
			emptyTile = new int[2] { _positionOfToBeRescued[0] + addedX, _positionOfToBeRescued[1] + addedZ };
			addedX += addX;
			addedZ += addZ;
		}
		if ( emptyTile[0] != -1 )
		{
			registerTilesAndAnimateCoreWithTroleyInDirection ( emptyTile, direction );
//			print ("updating 1");
		}
		else 
		{
			Vector3 _initialPosition = transform.position;
			if(gameObject.transform.parent.gameObject.GetComponent <iTween>() == null)
			{
				if(transform.Find("front").gameObject.activeSelf == true)
				{
					print ("UpShake");
					iTween.PunchPosition ( gameObject.transform.parent.gameObject, iTween.Hash("y", 0.8f, "time", 0.8f));
				}
				if(transform.Find("side").gameObject .activeSelf == true)
				{
					print ("SideShake");
					iTween.PunchPosition ( gameObject.transform.parent.gameObject, iTween.Hash("x", 0.8f, "time", 0.8f));
				}
			}
			print ("I'm Jammed!");
			CHARACTER_HIT = null;
		}
	}
				
	private void registerTilesAndAnimateCoreWithTroleyInDirection ( int[] targetTile, int direction, bool withActualMove = true )
	{	
		if(GlobalVariables.READY_FOR_NEXT_TURN == false)
		{
										/*if(myCounter < 0)
			{
				MovesWarningCreator.getInstance().StartCoroutine("ShowWarning");
				myCounter = 1.5f;
			}*/
			return;

		}
		if(withActualMove)
		{
			if(GlobalVariables.READY_FOR_NEXT_TURN == false || GameGlobalVariables.Stats.MOVES < 1)
			{
				//DAVE LOCATOR
				return;
			}
			//GlobalVariables.READY_FOR_NEXT_TURN = false;
//			print ("This One");
		}
		ZoomAndLevelDrag.getInstance ().allowFollow = true;
		_myIComponent = gameObject.GetComponent < IComponent > ();
		
		GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], transform.root.gameObject, _myIComponent.myID );
		GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _positionOfToBeRescued[0], _positionOfToBeRescued[1], _toBeRescuedObject, _myIComponent.myID  );			
		
		_toBeRescuedObject.transform.localPosition = new Vector3 ( 0f, 0.5f, 0f );			
		if ( ! withActualMove )
		{
			gameObject.GetComponent < SelectedComponenent > ().updateInitialValues ();
		}
		else
		{
			_myIComponent.myCharacterData.coraSlidingTroley = true;
		}
						
		Vector3 positionToGo = new Vector3 ((float) targetTile[0], (float) ( LevelControl.LEVEL_HEIGHT - targetTile[1] ), (float) targetTile[1] - 0.5f ); 		
		float distanceToTarget = Vector3.Distance ( new Vector3 ( positionToGo.x, 0f, positionToGo.z ), new Vector3 ( transform.root.position.x, 0f, transform.root.position.z ));
				
		CoraMarkerScale scaleControl;
		switch ( direction )
		{
			case DIRECTION_UP:
				_toBeRescuedObject.SetActive ( true );
				_toBeRescuedObject.transform.localPosition = new Vector3 ( 0f, .5f, 0f );
				transform.localPosition = new Vector3 ( 0f, -0.5f, -0.38f );

				scaleControl = FindObjectOfType<CoraMarkerScale>() as CoraMarkerScale;
				if (scaleControl != null) scaleControl.UpdateScaleAndPosition(CoraMarkerScale.Direction.UP);

				gameObject.GetComponent < BoxCollider > ().center = new Vector3 ( 0f, 0.87f, 0.11f );
							
				GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, targetTile[0], targetTile[1] - 1, transform.root.gameObject, _myIComponent.myID  );
				GridReservationManager.getInstance ().fillTileWithMe ( _toBeRescuedID, targetTile[0], targetTile[1], _toBeRescuedObject, _myIComponent.myID  );
				
				_myIComponent.position[0] = _myIComponent.myCharacterData.position[0] = targetTile[0];
				_myIComponent.position[1] = _myIComponent.myCharacterData.position[1] = targetTile[1] - 1;
				
				if ( withActualMove ) gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_UP_ANIMATION, 0.038f * distanceToTarget );
				else gameObject.GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.INTERACT_UP );
				_lastDirectionState = CharacterAnimationControl.INTERACT_UP;
				break;
			case DIRECTION_DOWN:
				_toBeRescuedObject.SetActive ( true );
				_toBeRescuedObject.transform.localPosition = new Vector3 ( 0f, 0.5f, -0.29f );
							
				transform.localPosition = new Vector3 ( 0f, 0.6f, 0f );
				
				gameObject.GetComponent < BoxCollider > ().center = new Vector3 ( 0.09f, 1.05f, 0.2f );
							
				scaleControl = FindObjectOfType<CoraMarkerScale>() as CoraMarkerScale;
				if (scaleControl != null) scaleControl.UpdateScaleAndPosition(CoraMarkerScale.Direction.DOWN);
				
				GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, targetTile[0], targetTile[1] + 1, transform.root.gameObject, _myIComponent.myID  );
				GridReservationManager.getInstance ().fillTileWithMe ( _toBeRescuedID, targetTile[0], targetTile[1], _toBeRescuedObject, _myIComponent.myID  );
				
				_myIComponent.position[0] = _myIComponent.myCharacterData.position[0] = targetTile[0];
				_myIComponent.position[1] = _myIComponent.myCharacterData.position[1] = targetTile[1] + 1;
				
				if ( withActualMove ) gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_DOWN_ANIMATION, 0.038f * distanceToTarget );
				else gameObject.GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.INTERACT_DOWN );
				_lastDirectionState = CharacterAnimationControl.INTERACT_DOWN;
				break;
			case DIRECTION_RIGHT:
				_toBeRescuedObject.SetActive ( true );
				_toBeRescuedObject.transform.localPosition = new Vector3 ( 0f, .6f, 0f );
							
				transform.localPosition = new Vector3 ( -1.2f, .45f, 0.07f );
				
				gameObject.GetComponent < BoxCollider > ().center = new Vector3 ( 0.4f, 1.2f, 0.2f );

				scaleControl = FindObjectOfType<CoraMarkerScale>() as CoraMarkerScale;
				if (scaleControl != null) scaleControl.UpdateScaleAndPosition(CoraMarkerScale.Direction.RIGHT);
													
				GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, targetTile[0] - 1, targetTile[1], transform.root.gameObject, _myIComponent.myID  );
				GridReservationManager.getInstance ().fillTileWithMe ( _toBeRescuedID, targetTile[0], targetTile[1], _toBeRescuedObject, _myIComponent.myID  );
				
				_myIComponent.position[0] = _myIComponent.myCharacterData.position[0] = targetTile[0] - 1;
				_myIComponent.position[1] = _myIComponent.myCharacterData.position[1] = targetTile[1];
				
				if ( withActualMove ) gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_RIGHT_ANIMATION, 0.038f * distanceToTarget );
				else gameObject.GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.INTERACT_RIGHT );
				_lastDirectionState = CharacterAnimationControl.INTERACT_RIGHT;
				break;
			case DIRECTION_LEFT:
				_toBeRescuedObject.SetActive ( true );
				_toBeRescuedObject.transform.localPosition = new Vector3 ( 0f, .6f, 0f );
							
				transform.localPosition = new Vector3 ( 1.2f, 0.45f, 0.92f );
				
				gameObject.GetComponent < BoxCollider > ().center = new Vector3 ( 0.57f, 0.84f, 0.2f );
							
				scaleControl = FindObjectOfType<CoraMarkerScale>() as CoraMarkerScale;
				if (scaleControl != null) scaleControl.UpdateScaleAndPosition(CoraMarkerScale.Direction.LEFT);

				GridReservationManager.getInstance ().fillTileWithMe ( _myIComponent.myID, targetTile[0] + 1, targetTile[1], transform.root.gameObject, _myIComponent.myID  );
				GridReservationManager.getInstance ().fillTileWithMe ( _toBeRescuedID, targetTile[0], targetTile[1], _toBeRescuedObject, _myIComponent.myID  );
				
				_myIComponent.position[0] = _myIComponent.myCharacterData.position[0] = targetTile[0] + 1;
				_myIComponent.position[1] = _myIComponent.myCharacterData.position[1] = targetTile[1];
				
				if ( withActualMove ) gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_LEFT_ANIMATION_01, 0.038f * distanceToTarget );
				else gameObject.GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.INTERACT_LEFT );
				_lastDirectionState = CharacterAnimationControl.INTERACT_LEFT;
				break;
		}
		
		_positionOfToBeRescued[0] = targetTile[0];
		_positionOfToBeRescued[1] = targetTile[1];	
		
		if ( withActualMove )
		{
			if ( distanceToTarget < 2f )
			{
				
				SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_TROLEY_MOVE, _myIComponent.myID, true );
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_TROLEY_MOVE2, _myIComponent.myID, true );
			}
						
			iTween.MoveTo ( transform.root.gameObject, iTween.Hash ( "time", 0.04f * distanceToTarget, "easetype", iTween.EaseType.linear, "position", positionToGo, "oncomplete", "rescuerMoveComplete", "oncompletetarget", this.gameObject ));
			
			GlobalVariables.CHARACTER_IN_MOVE = true;
							
			if ( Camera.main.orthographicSize < ZoomAndLevelDrag.MAXIMUM_CAMERA_ZOOM - 0.1f )
			{
				if ( LevelControl.LEVEL_ID != 1 ) CameraMovement.getInstance ().followObjectForSeconds ( transform.parent, 0.8f );
			}
		}
		else
		{
			transform.root.position = new Vector3 ((float) targetTile[0], (float) ( LevelControl.LEVEL_HEIGHT - targetTile[1] ), (float) targetTile[1] - 0.5f );			
		}
	}
					
	public void rescuerMoveComplete ()
	{
		Main.getInstance ().removeOneMove ();
		//freshSwipeHasBegun = false;
		GlobalVariables.READY_FOR_NEXT_TURN = true;
//		print ("Ok Again");
//		print ("Arrived at destination");
		if(MNSpawningEnemiesManager.getInstance ()!= null)
		{
			MNSpawningEnemiesManager.getInstance ().spawnRandomEnemy (true);
		}
		switch ( _lastDirectionState )
		{
			case CharacterAnimationControl.INTERACT_RIGHT:
				gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IMPACT_RIGHT_ANIMATION );
				break;
			case CharacterAnimationControl.INTERACT_LEFT:
				gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IMPACT_LEFT_ANIMATION );
				break;
			case CharacterAnimationControl.INTERACT_DOWN:
				gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IMPACT_DOWN_ANIMATION );
				break;
			case CharacterAnimationControl.INTERACT_UP:
				gameObject.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IMPACT_UP_ANIMATION );
				break;
		}
		
		if ( CHARACTER_HIT != null )
		{
			CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForPassingCharacter ( true, Vector3.zero );
			
			//int differenceX = _myIComponent.myCharacterData.position[0]  - CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < IComponent > ().position[0];
			//int differenceZ = _myIComponent.myCharacterData.position[1]  - CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < IComponent > ().position[1];

			//if ( differenceX > 0f )
			//{
			//	CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_RIGHT, -1, differenceX );
			//}
			//else if ( differenceX < 0 )
			//{
			//	CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_RIGHT, -1, differenceX );
			//}
			//else if ( differenceZ < 0 )
			//{
				CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_DOWN );
			//}
			//else
			//{
			//	CHARACTER_HIT.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_UP );
			//							
			//}

			StartCoroutine ( "playIdleOnHitCharacter", CHARACTER_HIT );
			CHARACTER_HIT = null;			
		}

		if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_DUMMY )
		{
			if ( LevelControl.LEVEL_ID == 8 )
			{
				TutorialsManager.getInstance ().goToNextStep ();
			}
		}
		
		if ( LevelControl.LEVEL_ID == 8 )
		{
			if ( _positionOfToBeRescued[0] == 7 && _positionOfToBeRescued[1] == 0 )
			{
				GameObject madraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_MADRA_1_IDLE );
				//madraObject.AddComponent < MoveMadraPointer > ();
			}
		}
		
		if ( LevelControl.LEVEL_ID == 3 )
		{
			_countMoves++;
		}

		if ( gameObject.GetComponent < TutorialMoveCharacterComponent > ())
		{
			gameObject.GetComponent < TutorialMoveCharacterComponent > ().finishExternaly ();
		}

		iTween.MoveFrom ( _toBeRescuedObject, iTween.Hash ( "time", 0.12f, "easetype", iTween.EaseType.linear, "position", _toBeRescuedObject.transform.position + Vector3.forward * 0.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationRescueToyMove"));

		StartCoroutine ( "onFinishimpactChangeState" );
	}

	private IEnumerator playIdleOnHitCharacter ( GameObject charcterObject )
	{
		yield return new WaitForSeconds ( 0.4f );
		charcterObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
	}

	private void onCompleteTweenAnimationRescueToyMove ()
	{

	}
					
	private IEnumerator onFinishimpactChangeState ()
	{
		yield return new WaitForSeconds ( 0.33f );
		GlobalVariables.CHARACTER_IN_MOVE = false;
		gameObject.GetComponent < CharacterAnimationControl > ().changeState ( _lastDirectionState );
		_myIComponent.myCharacterData.coraSlidingTroley = false;
	}

	public void OnMouseDown ()
	{
		if ( GlobalVariables.checkForMenus ()) return;
		//freshSwipeHasBegun = true;
		if ( ! _myIComponent.myCharacterData.selected && _withToyTobeRescued ) 
		{
			gameObject.SendMessage ( "handleTouched" );	
		}
#if UNITY_EDITOR
		_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
		_mouseDownOnMe = true;
	}
				
	public void forceOnMouseDown ()
	{	
		if ( ! _myIComponent.myCharacterData.selected && _withToyTobeRescued ) 
		{
			gameObject.SendMessage ( "handleTouched" );	
		}

#if UNITY_EDITOR
		_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif	
		_mouseDownOnMe = true;
	}
				
	public void OnMouseUp ()
	{
		if ( GlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
				
	private void handleTouched ()
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		Main.getInstance ().handleCharacterChosenGeneral ( _myIComponent.myCharacterData, null );
	}
				
	public bool handleChangePositionForTroley ( int[] positionOfToBeRescued, int toBeRescuedID )
	{
		gameObject.GetComponent < BoxCollider > ().size = Vector3.one * 2.75f;
						
		gameObject.GetComponent < CharacterReloactionComponent > ().blockMove = true;
		_toBeRescuedID = toBeRescuedID;
		gameObject.GetComponent < IComponent > ().myCharacterData.interactAction = true;
		//=====================================Daves Edit========================================
		gameObject.GetComponent < IComponent > ().myCharacterData._interactTrolley = true;
		//=====================================Daves Edit========================================
		_toBeRescuedObject = transform.root.Find ( "toBeRescued" ).gameObject;
		_positionOfToBeRescued = positionOfToBeRescued; // intetionaly keeping reference
					
		int xDiference = positionOfToBeRescued[0] - gameObject.GetComponent < IComponent > ().position[0];
		int zDiference = positionOfToBeRescued[1] - gameObject.GetComponent < IComponent > ().position[1];
//									print ("updating 2");
		if ( xDiference > 0 )
		{
			registerTilesAndAnimateCoreWithTroleyInDirection ( positionOfToBeRescued, DIRECTION_RIGHT, false );
		}
		else if ( xDiference < 0 )
		{
			registerTilesAndAnimateCoreWithTroleyInDirection ( positionOfToBeRescued, DIRECTION_LEFT, false );
		}
		else if ( zDiference > 0 )
		{
			registerTilesAndAnimateCoreWithTroleyInDirection ( positionOfToBeRescued, DIRECTION_UP, false );
		}
		else if ( zDiference < 0 )
		{
			registerTilesAndAnimateCoreWithTroleyInDirection ( positionOfToBeRescued, DIRECTION_DOWN, false );
		}
		
		//GridReservationManager.getInstance ().fillTileWithMe ( _toBeRescuedID, positionOfToBeRescued[0], positionOfToBeRescued[1], _toBeRescuedObject, _myIComponent.myID  );			
					
		collider.enabled = true;
		GlobalVariables.CORA_ALREADY_WITH_TOY_ON_TROLEY = _withToyTobeRescued = true;
		
		return false;
	}
}
