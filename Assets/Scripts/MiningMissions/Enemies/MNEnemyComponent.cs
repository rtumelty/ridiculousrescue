using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNEnemyComponent : ObjectTapControl 
{
	//*************************************************************//	
	public delegate void HandleFinishedAnimationMove ( bool succsessful );
	public EnemyData myEnemyData;
	public bool fromTapFromPlayer = false;
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleEnemySelected;
	private EnemyComponent.HandleAttackExecuted _handleAttackExecuted;
	private GameObject _myAttackUIComboInstant;
	private CharacterData _currentAttackingCharacter;
	private IComponent _myIComponent;
	private SelectedComponenent _mySelectedComponent;
	private float _lookRightLocalScale;
	private float _countTimeToUnblockInteract = 0f;
	private bool _startCountingtoUnblock = false;
	private float _countToCheckCharacterAround = 2f;
	private float _countTimeToNextAttack = 0f;
	//=======================Daves Work======================
	private bool holdLeft = false;
	private bool holdRight = false;
	private bool toTheLeft = false;
	private bool toTheRight = false;
	private int tentacleRaised = 0;
	private int[] characterClossest = null;
	private GameObject myParticles;
	private GameObject myParticlesInstance;
	private GameObject[] charsInLevel;
	private List <GameObject> myTargets;
	private CharacterData myTarget;
	private GameObject madraObject;

	private int myAudiolooper = 0;
	private bool imLoadingRef = false;

	private Vector3 _initialPositionOfBoz;
	private GameObject _bozObject;
	private bool _duringAttack = false;
	//=======================Daves Work======================
	//*************************************************************//	
	void Awake ()
	{
		myParticles = (GameObject)Resources.Load ("Particles/elecSparks");
	}

	void OnDestroy ()
	{
		if(myParticlesInstance != null)
		{
			Destroy (myParticlesInstance);
		}
	}

	void Start () 
	{
		_myHandleEnemySelected = handleAttackedByCharacted;
		_handleAttackExecuted = handleAttackedButtonPressed;
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_mySelectedComponent = gameObject.GetComponent < SelectedComponenent > ();
		if(_myIComponent.myID == GameElements.ENEM_TENTACLEDRAINER_01 || _myIComponent.myID == GameElements.ENEM_TENTACLEDRAINER_02)
		{
			myParticlesInstance = (GameObject)Instantiate (myParticles, transform.position, transform.rotation);
			//myParticles.transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);
			myParticlesInstance.SetActive(false);
		}
		_lookRightLocalScale = transform.localScale.x;
		//=======================Daves Work======================
		myTargets = new List<GameObject> ();
		foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
		{
			myTargets.Add(character);
		}
	}

	void tentacleAudioLooper()
	{
		SoundManager.getInstance().playSound(SoundManager.TENTACLE_GET_READY);
	}

	void slugAudioLooper()
	{
		SoundManager.getInstance().playSound(SoundManager.SLUG_GET_READY);
	}

	void Update ()
	{
		/*
		if(holdLeft == true)
		{
			int myRangeX = _myIComponent.position[0] - myTarget.position[0];
			int myRangeZ = _myIComponent.position[1] - myTarget.position[1];
			if( myRangeX >= -1 && myRangeX <= 1 && myRangeZ >= -1 && myRangeZ <= 1 )
			{
				myParticlesInstance.SetActive(true);
				if(tentacleRaised < 1)
				{
					GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_CHARGE_LEFT);
					tentacleRaised = 1;
				}
				GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_HOLD_LEFT);

				if(myAudiolooper < 1)
				{
					myAudiolooper = 1;
					Invoke("tentacleAudioLooper",0);
				}
			}
		}
		else if(holdRight == true)
		{
			int myRangeX = _myIComponent.position[0] - myTarget.position[0];
			int myRangeZ = _myIComponent.position[1] - myTarget.position[1];
			if( myRangeX >= -1 && myRangeX <= 1 && myRangeZ >= -1 && myRangeZ <= 1 )
			{
				myParticlesInstance.SetActive(true);
				if(tentacleRaised < 1)
				{
					GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_CHARGE);
					tentacleRaised = 1;
				}
				GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_HOLD);

				if(myAudiolooper < 1)
				{
					myAudiolooper = 1;
					Invoke("tentacleAudioLooper",0);
				}
			}
		}*/
		//=======================Daves Work======================
		if ( _startCountingtoUnblock && ! _currentAttackingCharacter.doNotUnblockThisCharacter )
		{
			_countTimeToUnblockInteract -= Time.deltaTime;
			
			if ( _countTimeToUnblockInteract <= 0f )
			{
				_countTimeToUnblockInteract = 0f;
				_currentAttackingCharacter.interactAction = false;
				_currentAttackingCharacter.attacking = false;
				_startCountingtoUnblock = false;
			}
		}
		
		_countToCheckCharacterAround -= Time.deltaTime;
		_countTimeToNextAttack -= Time.deltaTime;

		/*if ( _myIComponent.myID != GameElements.ENEM_SLUG_01 )
		{
			if ( _countToCheckCharacterAround <= 0f )
			{
				_countToCheckCharacterAround = 0.5f;
				int[] characterClossest = null;

				if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 )
				{
					characterClossest = ToolsJerry.findClossestCharacterTileAroundThisTileMining ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.myID, transform.gameObject, _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] );
				}
				else 
				{
					characterClossest = ToolsJerry.findClossestCharacterTileAroundThisTileMining ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.myID, transform.parent.gameObject, _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] );
				}
				if ( characterClossest[0] != -1 )
				{
					if ( _countTimeToNextAttack <= 0f )
					{		
						CharacterData characterToAttack = LevelControl.getInstance ().getCharacterFromPosition ( characterClossest );
						if(characterToAttack.myID == 40 && ! characterToAttack.moving)
						{
							//plays Fara's cower animation.
							//LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_SCARED );
						}
						if ( characterToAttack != null && ! characterToAttack.interactAction && ! characterToAttack.attacking && ! characterToAttack.moving && characterToAttack.countTimeToNextPossibleAttack <= 0f )
						{
							if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 ) 
							{
							}		
							else
							{
								//=========================Dave Work==========================
								switch ( _myIComponent.myID )
								{
								case GameElements.ENEM_TENTACLEDRAINER_01:
									//GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_CHARGE);
									//GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION );
									//StartCoroutine("getTentacleReady");
									if(_myIComponent.position[0] - characterToAttack.position[0] > 0)
									{
										toTheLeft = true;
									}
									StartCoroutine("getTentacleReady",false);
									break;
								case GameElements.ENEM_TENTACLEDRAINER_02:
									//GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_CHARGE);
									//GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION );

									if(_myIComponent.position[0] - characterToAttack.position[0] > 0)
									{
										toTheLeft = true;
									}
									StartCoroutine("getTentacleReady",false);
									break;
								}
								//=========================Dave Work==========================
							}
						}
					}
				}
			}
		}*/
		if(_myIComponent.myID == GameElements.ENEM_SLUG_01 && imLoadingRef != true && _duringAttack == true)
		{

			if ( _countToCheckCharacterAround <= 0f )
			{
				_countToCheckCharacterAround = 0.5f;
				//int[] characterClossest = null;
				
				if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 )
				{
					characterClossest = ToolsJerry.findClossestCharacterTileAroundThisTileMining ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.myID, transform.gameObject, _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] );
				}
				else 
				{
					characterClossest = ToolsJerry.findClossestCharacterTileAroundThisTileMining ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.myID, transform.parent.gameObject, _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] );
				}
				if ( characterClossest[0] != -1 )
				{
					CharacterData characterToAttack = LevelControl.getInstance ().getCharacterFromPosition ( characterClossest );
					if(characterToAttack.myID == 40 && ! characterToAttack.moving)
					{
						//print ("happened");
						LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_SCARED );
					}
				}
			}
		}
	}
	void OnMouseDown ()
	{
		return;
		if(GlobalVariables.READY_FOR_NEXT_TURN == false)
		{
			print ("YO");
			MovesWarningCreator.getInstance().StartCoroutine("ShowWarning");
		}
	}

	void OnMouseUp ()
	{
		if (( UIControl.currentAttackBarUI != null ) && _currentAttackingCharacter != null && ( _currentAttackingCharacter.attacking )) UIControl.currentAttackBarUI.SendMessage ( "OnMouseUp" );
		if ( _duringAttack ) return;
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		if (CheckInputType.getInstance().touchType != CheckInputType.TAP_TYPE)
		{
//			print ("touchType = " + CheckInputType.getInstance().touchType);
			return;
		}
//		print ("touchType = " + CheckInputType.getInstance().touchType);
		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		if ( selectedCharacter.myID == GameElements.CHAR_MADRA_1_IDLE )
		{
			if ( GetComponent < TipOnClickComponent > ()) GetComponent < TipOnClickComponent > ().deActivate ();
		}
		else
		{
			madraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_MADRA_1_IDLE );

			if ( madraObject != null )
			{
				madraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
			else if ( selectedCharacter.myID != GameElements.CHAR_BOZ_1_IDLE )
			{
				GameObject bozObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_BOZ_1_IDLE );
				bozObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
		}

		if ( GlobalVariables.checkForMenus ())
		{
			print ("Was Returned");
			return;
		}
		if(GlobalVariables.READY_FOR_NEXT_TURN == false)
		{
			if(CheckInputType.getInstance().lastType != CheckInputType.DUALTOUCH_TYPE && CheckInputType.getInstance().lastType !=CheckInputType.DRAG_TYPE)
			{
				print ("YO");
				MovesWarningCreator.getInstance().StartCoroutine("ShowWarning");
			}
			return;
		}
		if(GlobalVariables.READY_FOR_NEXT_TURN == false)
		{
			//DAVE LOCATOR
			return;
		}
		//GlobalVariables.READY_FOR_NEXT_TURN = false;
//		print ("This One");
		handleTouched ();
		GameGlobalVariables.ACTIVE_FROM_ENEMY = true;
	}
	
	private void handleTouched ()
	{	

		if ( _alreadyTouched ) return;
		fromTapFromPlayer = true;
		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		if ( selectedCharacter.interactAction ) return;
		if ( selectedCharacter.attacking ) return;
		if ( _duringAttack ) return;

		if ( Array.IndexOf ( GameElements.ATTACKERS, selectedCharacter.myID ) != -1 )
		{
			_alreadyTouched = true;
		}

		if ( SelectedComponenent.CURRENT_SELECTED_OBJECT ) SelectedComponenent.CURRENT_SELECTED_OBJECT.setSelected ( false );

		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true );
		Main.getInstance ().interactWithCurrentCharacter ( _myHandleEnemySelected, CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE, _myIComponent );
	}
	
	public void handleAttackedByCharacted ( CharacterData characterToAttack, bool onlyUnblocking = false )
	{
		if ( onlyUnblocking )
		{
			_alreadyTouched = false;
			return;
		}

		if ( GlobalVariables.MENU_FOR_RESOULT_SCREEN ) return;
		if ( _duringAttack ) return;

		if ( characterToAttack.myCurrentAttackBar != null )
		{
			_myIComponent.myEnemyData.attacking = false;
			return;
		}

		_duringAttack = true;
		_countTimeToNextAttack = 9f;

		_startCountingtoUnblock = false;

		_currentAttackingCharacter = characterToAttack;
		characterToAttack.doNotUnblockThisCharacter = true;
		characterToAttack.interactAction = true;
		characterToAttack.attacking = true;

		_myIComponent.myEnemyData.attacking = true;	

		int differenceX = _myIComponent.position[0] - characterToAttack.position[0];
		int differenceZ = _myIComponent.position[1] - characterToAttack.position[1];

		if ( characterToAttack.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			_bozObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]];

			_initialPositionOfBoz = VectorTools.cloneVector3 ( _bozObject.transform.position );

			if ( differenceX > 0 )
			{
				_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_RIGHT, 2f, differenceX );
			}
			else if ( differenceX < 0 )
			{
				_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_RIGHT, 2f, differenceX );
			}
			else if ( differenceZ < 0 )
			{
				_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_DOWN, 2f, differenceX );
			}
			else
			{
				_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.BUMP_UP, 2f, differenceX );
			}

			StartCoroutine ( "waitBeofreChangeToIdleBoz" );
		}
		else
		{
			if ( characterToAttack.myID == GameElements.CHAR_MADRA_1_IDLE ) SoundManager.getInstance().playSound(SoundManager.MADRA_GROWL);

			if(_myIComponent.myID == GameElements.ENEM_SLUG_01)
			{
				Invoke("slugAudioLooper",0);
			}
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForAttack ( true );
			
			if ( characterToAttack.characterValues[CharacterData.CHARACTER_ACTION_TYPE_ATTACK_RATE] > 0 )
			{
				if ( differenceX > 0 )
				{  
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ATTACK_IDLE_RIGHT_ANIMATION, 2f, differenceX );
				}
				else if ( differenceX < 0 )
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ATTACK_IDLE_RIGHT_ANIMATION, 2f, differenceX );
				}
				else if ( differenceZ < 0 )
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ATTACK_IDLE_FRONT_ANIMATION );
				}
				else
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ATTACK_IDLE_BACK_ANIMATION );
				}
			}
			else
			{
				if ( differenceX > 0 ) 
				{
					if(characterToAttack.myID != 40)
					{
						LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.RIGHT );
					}
				}
				else 
				{
					if(characterToAttack.myID != 40)
					{
						LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.LEFT );
					}
				}
			}
		}

		if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 ) iTween.Stop ( transform.gameObject );
		else iTween.Stop ( transform.parent.gameObject );

		_mySelectedComponent.setSelected ( false );
		
		switch ( _myIComponent.myID )
		{
			case GameElements.ENEM_TENTACLEDRAINER_01:
			case GameElements.ENEM_TENTACLEDRAINER_02:
				/*if ( differenceX > 0 ) 
				{
					transform.localScale = new Vector3 ( -_lookRightLocalScale, transform.localScale.y, transform.localScale.z );
				}
				else 
				{	
					transform.localScale = new Vector3 ( _lookRightLocalScale, transform.localScale.y, transform.localScale.z );
				}*/
				break;
			case GameElements.ENEM_SLUG_01:
				if ( differenceX > 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION_LEFT );
				else if ( differenceX < 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION_RIGHT );
				else if ( differenceZ > 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION_DOWN );
				else if ( differenceZ < 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION_UP );
				break;
		}
		
		StartCoroutine ( "waitBeforeCreateAttackButton", characterToAttack );
	}

	private IEnumerator waitBeofreChangeToIdleBoz ()
	{
		_bozObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]];
		yield return new WaitForSeconds ( 0.25f );
		_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
	}

	private void finishedJumpOnTentacleAbove ()
	{
		_bozObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]];
		iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.2f, "easetype", iTween.EaseType.linear, "position", this.transform.position + Vector3.forward * -0.5f + Vector3.up, "oncompletetarget", this.gameObject, "oncomplete", "finishedJumpOnTentacleAboveAfterArc" ));
	}

	private void finishedJumpOnTentacleAboveAfterArc ()
	{
		StartCoroutine ( "waitBeofreChangeAnimationToDrill" );
	}

	private IEnumerator waitBeofreChangeAnimationToDrill ()
	{
		yield return new WaitForSeconds ( 0.5f );
		_bozObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]];
		_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
	}
	
	private IEnumerator waitBeforeCreateAttackButton ( CharacterData characterToAttack )
	{
		if ( _currentAttackingCharacter.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			yield return new WaitForSeconds ( 0.2f );
		}
		else 
		{
			yield return new WaitForSeconds ( 1f );
		}
		switch ( _myIComponent.myID )
		{
		case GameElements.ENEM_TENTACLEDRAINER_01:
		case GameElements.ENEM_TENTACLEDRAINER_02:
			handleAttackedButtonPressed ( true );
			break;
		case GameElements.ENEM_SLUG_01:
			handleAttackedButtonPressed ( fromTapFromPlayer );
			break;
		}
	}
	
	public void handleAttackedButtonPressed ( bool succsessful )
	{
		_currentAttackingCharacter.countTimeToNextPossibleMoveAfterAttack = 1.5f;

		StopCoroutine ( "waitBeofreChangeToIdleBoz" );

		int differenceX = _myIComponent.position[0] - _currentAttackingCharacter.position[0];
		int differenceZ = _myIComponent.position[1] - _currentAttackingCharacter.position[1];

		if ( succsessful && _currentAttackingCharacter.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			if ( transform.parent )
			{
				_bozObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( transform.parent.gameObject );
			}
			else
			{
				_bozObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( gameObject );
			}

			_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.JUMP_TO_SQUISH, 2f, differenceX );
			SoundManager.getInstance ().playSound ( SoundManager.BOZ_JUMP_TO_DRILL );
			iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.2f, "easetype", iTween.EaseType.linear, "position", this.transform.position + Vector3.forward * 1.5f + Vector3.up, "oncompletetarget", this.gameObject, "oncomplete", "finishedJumpOnTentacleAbove" ));
		}
		else if ( _currentAttackingCharacter.myID != GameElements.CHAR_BOZ_1_IDLE )
		{
			if ( Array.IndexOf ( GameElements.ATTACKERS, _currentAttackingCharacter.myID ) != -1 )
			{
				if ( differenceX > 0 )
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_01_ANIMATION, 2f, differenceX );
				}
				else if ( differenceX < 0 )
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_01_ANIMATION, 2f, differenceX );
				}
				else if ( differenceZ < 0 )
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ATTACK_FRONT_ANIMATION );
				}
				else
				{
					LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ATTACK_BACK_ANIMATION );
				}
			}

			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForAttack ( false );
		}

		callBackWhenFinishedMove ( succsessful );

	}
	
	private void callBackWhenFinishedMove ( bool succsessful )
	{
		if ( ! _mySelectedComponent ) return;

		_currentAttackingCharacter.doNotUnblockThisCharacter = false;
		_countTimeToUnblockInteract = 0.7f;
		_startCountingtoUnblock = true;
		_currentAttackingCharacter.countTimeToNextPossibleAttack = 3f;

		if ( _currentAttackingCharacter.myID == GameElements.CHAR_BOZ_1_IDLE )
		{

		}
		else 
		{
			_currentAttackingCharacter.attacking = false;
		}
		
		//SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_ATTACK, _currentAttackingCharacter.myID );

		if ( succsessful )
		{
			_myIComponent.myEnemyData.attacking = false;
			_countTimeToUnblockInteract = 0.5f;

			if ( GlobalVariables.TUTORIAL_MENU )
			{
				if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS )
				{
					TutorialsManager.getInstance ().goToNextStep ();
				}
			}
			
			_mySelectedComponent.setSelected ( false );

			_myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ENDURANCE] -= _currentAttackingCharacter.characterValues[CharacterData.CHARACTER_ACTION_TYPE_DAMAGE];
			
			if ( _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ENDURANCE] <= 0 )
			{
				GetComponent < EnemyAnimationComponent > ().currentAttackingCharacter = _currentAttackingCharacter;
				holdLeft = false;
				holdRight = false;
				if(myParticlesInstance != null)
				{
					myParticlesInstance.SetActive(false);
				}

				_myIComponent.myEnemyData.dieInProgress = true;
				Instantiate ( Main.getInstance ().tentacleParticles, transform.position, Quaternion.identity );
				CancelInvoke("tentacleAudioLooper");
				CancelInvoke("slugAudioLooper");
				myAudiolooper = 0;
				if(_currentAttackingCharacter.myID == GameElements.CHAR_MADRA_1_IDLE)
				{
					SoundManager.getInstance().playSound(SoundManager.MADRA_BITE);
				}
				else if(_currentAttackingCharacter.myID == GameElements.CHAR_BOZ_1_IDLE)
				{
					print ("BozAttack SFX here");
				}
				if(_myIComponent.myID == GameElements.ENEM_SLUG_01)
				{
					SoundManager.getInstance ().playSound ( SoundManager.TENTACLE_DRAINER_DEFATED, _myIComponent.myID );
				}
				else
				{
					SoundManager.getInstance ().playSound (SoundManager.TENTACLE_DRAINER_DEFATED, _myIComponent.myID);
				}
			}
			
			StartCoroutine ( wiatBeforeMoveCharacter ( _currentAttackingCharacter ));
			
			_alreadyTouched = false;
		}
		else
		{
			if ( GlobalVariables.TUTORIAL_MENU )
			{
				if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS )
				{
					//Jerry this should be gone? null reference
					/*if ( _myAttackUIComboInstant )
					{
						_myAttackUIComboInstant.SendMessage ( "externalCallForDestroy", SendMessageOptions.DontRequireReceiver );
						StartCoroutine ( "wiatBeforeInvokeRepeatTutorilStep" );
					}*/
				}
			}
			
			if ( Array.IndexOf ( GameElements.ATTACKERS, _currentAttackingCharacter.myID ) == -1 )
			{
				MiningResourcesControl.getInstance ().createRandomResourcesFromCollectedAroundPosition ( _currentAttackingCharacter.position );
			}
			
			StartCoroutine ( "waitBeforeReplyAttack" );
		}
	}

	private IEnumerator wiatBeforeMoveCharacter ( CharacterData character, bool fromSlug = false )
	{
		if ( Array.IndexOf ( GameElements.ATTACKERS, character.myID ) == -1 || fromSlug )
		{
			yield return new WaitForSeconds ( 0.1f );
			_duringAttack = false;

			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]].transform.Find ( "tile" ).GetComponent < CharacterReloactionComponent > ().handleMoveToClossestFreeTile ();
		}
		else 
		{
			MNSpawningEnemiesManager.getInstance ().unRegisterHole ( _myIComponent.myEnemyData.myHoleID );


			if ( character.myID == GameElements.CHAR_BOZ_1_IDLE )
			{
				yield return new WaitForSeconds ( 0.4f );
				print ("FeCK");
				GameGlobalVariables.Stats.SCORE += GameGlobalVariables.Stats.Scores.FOR_DESTRYING_TENTACLE;

				
				PointController points = PrefabPool.GetPool ("pointPrefab").Spawn (transform.position + Vector3.up * 2).GetComponent<PointController>()  as PointController;
				points.Init ("50", Color.green);


				GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.DESTROY_ANIMATION );
				_bozObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]];
				
				GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, character.position[0], character.position[1], _bozObject, character.myID );

				if ( this.transform.parent ) GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.transform.parent.gameObject, _myIComponent.myID );
				else GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.gameObject, _myIComponent.myID );

				GridReservationManager.getInstance ().fillTileWithMe ( character.myID, _myIComponent.position[0], _myIComponent.position[1], _bozObject, character.myID );
				character.position = ToolsJerry.cloneTile ( _myIComponent.position );
				_bozObject.transform.Find ( "tile" ).GetComponent < IComponent > ().position = ToolsJerry.cloneTile ( _myIComponent.position );
				yield return new WaitForSeconds ( 0.1f );
				iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.05f, "easetype", iTween.EaseType.linear, "position", new Vector3 ((float) _myIComponent.position[0], (float) ( LevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f )));
				SoundManager.getInstance ().playSound ( SoundManager.BOZ_DESTROYED_ROCK );
				yield return new WaitForSeconds ( 0.03f );
				_bozObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( _bozObject );
				_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
				character.interactAction = false;
			}
			else
			{
				GameGlobalVariables.Stats.SCORE += GameGlobalVariables.Stats.Scores.FOR_DESTRYING_TENTACLE;

				
				PointController points = PrefabPool.GetPool ("pointPrefab").Spawn (transform.position + Vector3.up * 2).GetComponent<PointController>()  as PointController;
				points.Init ("50", Color.green);

				GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.DESTROY_ANIMATION );
				character.interactAction = false;
				GlobalVariables.READY_FOR_NEXT_TURN = true;
//				print ("Ok Again");
				GameGlobalVariables.ACTIVE_FROM_ENEMY = false;
				if(MNSpawningEnemiesManager.getInstance () != null)
				{
					MNSpawningEnemiesManager.getInstance ().spawnRandomEnemy ();
				}
				Main.getInstance ().removeOneMove ();
			}
		}
		if(LevelControl.CURRENT_LEVEL_CLASS.myName == "6")
		{
			TutorialComponent.getInstance().takeAStep ++;
		}
		yield return new WaitForSeconds (1f);
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );

	}
	
	private IEnumerator wiatBeforeInvokeRepeatTutorilStep ()
	{
		yield return new WaitForSeconds ( 0.25f );
		TutorialsManager.getInstance ().invokeRepeatStep ();
	}

	//=========================Daves Edit=============================
	private IEnumerator getTentacleReady (bool fromAttack) 
	{
		if(fromAttack == true)
		{
			foreach(CharacterData character in LevelControl.getInstance().charactersOnLevel)
			{
				if(character.myID == GameElements.CHAR_MADRA_1_IDLE)
				{
					myTarget = character;
				}
			}
		}
		else
		{
			myTarget = LevelControl.getInstance ().getSelectedCharacter ();
		}
		characterClossest = ToolsJerry.findClossestCharacterTileAroundThisTileMining ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.myID, transform.parent.gameObject, _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] );
		int rangeX = _myIComponent.position[0] - myTarget.position[0];
		int rangeZ = _myIComponent.position[1] - myTarget.position[1];
		if( rangeX > 0 )
		{
			toTheLeft = true;
		}
		else if(rangeX <= 0 )
		{
			toTheRight = true;
		}
		if(toTheLeft == true)
		{

			holdLeft = true;
		}
		else if(toTheRight == true)
		{
			holdRight = true;
		}
		if(fromAttack == false)
		{
			yield return new WaitForSeconds ( 3.80f );
		}
		else
		{
			yield return new WaitForSeconds ( 10.0f );
		}
		holdLeft = false;
		holdRight = false;
		tentacleRaised = 0;
		CancelInvoke ("tentacleAudioLooper");
		myAudiolooper = 0;
		myParticlesInstance.SetActive(false);
	}
	//=========================Daves Edit=============================

	private void loopAttackReady()
	{
		if(toTheLeft == true)
		{
			GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_HOLD_LEFT);
		}
		else
		{
			GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_HOLD);
		}
	}

	private IEnumerator waitBeforeReplyAttack ()
	{
		_mySelectedComponent.setSelected ( false );
		
		int differenceX = _myIComponent.position[0] - _currentAttackingCharacter.position[0];
		int differenceZ = _myIComponent.position[1] - _currentAttackingCharacter.position[1];
		
		switch ( _myIComponent.myID )
		{
			case GameElements.ENEM_TENTACLEDRAINER_01:
			case GameElements.ENEM_TENTACLEDRAINER_02:
				holdLeft = false;
				holdRight = false;
				myParticlesInstance.SetActive(false);
				CancelInvoke("tentacleAudioLooper");
				GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION );
				break;
			case GameElements.ENEM_SLUG_01:
			CancelInvoke("slugAudioLooper");
				if ( differenceX > 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION_LEFT );
				else if ( differenceX < 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION_RIGHT );
				else if ( differenceZ > 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION_DOWN );
				else GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION_UP );
				break;
		}
		
		SoundManager.getInstance ().playSound ( SoundManager.TENTACLE_DRAINER_ATTACK, _myIComponent.myID );

		if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 )
		{
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ELECTROCUTED_ANIMATION, 1f );
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().electrickShock ();
			SoundManager.getInstance ().playSound ( SoundManager.ELECTROCUTED );
			Handheld.Vibrate ();
		}
		else
		{
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ELECTROCUTED_ANIMATION, 1f );
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().electrickShock ();
			SoundManager.getInstance ().playSound ( SoundManager.ELECTROCUTED );
			Handheld.Vibrate ();
		}

		GameObject powerParticles = ( GameObject ) Instantiate ( Main.getInstance ().particlesPower, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.position, Quaternion.identity );
		powerParticles.AddComponent < AutoDestruct > ();
		
		_alreadyTouched = false;
		yield return new WaitForSeconds ( 0.7f );

		if ( _currentAttackingCharacter.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			_currentAttackingCharacter.attacking = false;
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		}

		_myIComponent.myEnemyData.attacking = false;
		_duringAttack = false;

		if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 )
		{
			GetComponent < MNSlugMovementComponent > ().isMoving = false;

			if ( differenceX > 0 ) GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION_LEFT );
			else GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.IDLE_ANIMATION_RIGHT );
		}

		SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_ATTACK_FAIL, _currentAttackingCharacter.myID );

		StartCoroutine ( wiatBeforeMoveCharacter ( _currentAttackingCharacter, true ));
	}
}
