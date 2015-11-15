using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyComponent : ObjectTapControl 
{
	//*************************************************************//	
	public delegate void HandleAttackExecuted ( bool succsessful );
	public delegate void HandleFinishedAnimationMove ( bool succsessful );
	//*************************************************************//	
	public EnemyData myEnemyData;
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleEnemySelected;
	private HandleAttackExecuted _handleAttackExecuted;
	private GameObject _myAttackUIComboInstant;
	private CharacterData _currentAttackingCharacter;
	private IComponent _myIComponent;
	private SelectedComponenent _mySelectedComponent;
	private float _lookRightLocalScale;
	private float _countTimeToUnblockInteract = 0f;
	private bool _startCountingtoUnblock = false;
	//=======================Daves Work======================
	private bool holdLeft = false;
	private bool holdRight = false;
	private bool toTheLeft = false;
	private bool toTheRight = false;
	private int tentacleRaised = 0;
	private int[] characterClossest = null;
	private GameObject myParticles;
	private GameObject myParticlesInstance;
	private CharacterData myTarget;
	//=======================Daves Work======================

	//*************************************************************//	
	void Start () 
	{
		myParticles = (GameObject)Resources.Load ("Particles/elecSparks");
		_myHandleEnemySelected = handleAttackedByCharacted;
		_handleAttackExecuted = handleAttackedButtonPressed;
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_mySelectedComponent = gameObject.GetComponent < SelectedComponenent > ();
		myParticlesInstance = (GameObject)Instantiate (myParticles, transform.position, transform.rotation);
		//myParticles.transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);
		myParticlesInstance.SetActive(false);
		
		_lookRightLocalScale = transform.localScale.x;
	}

	void OnDestroy ()
	{
		GlobalVariables.READY_FOR_NEXT_TURN = true;
		print ("Ok Again");
		MNSpawningEnemiesManager.getInstance ().spawnRandomEnemy ();
		if(myParticlesInstance != null)
		{
			Destroy (myParticlesInstance);
		}
	}

	void Update ()
	{
		//=======================Daves Work======================
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
					SoundManager.getInstance().playSound(SoundManager.TENTACLE_GET_READY);
				}
				GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_HOLD_LEFT);
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
					SoundManager.getInstance().playSound(SoundManager.TENTACLE_GET_READY);
				}
				GetComponent < EnemyAnimationComponent > ().playAnimation (EnemyAnimationComponent.ATTACK_ANIMATION_HOLD);
			}
			
		}
		//=======================Daves Work======================
		if ( _startCountingtoUnblock )
		{
			_countTimeToUnblockInteract -= Time.deltaTime;
			
			if ( _countTimeToUnblockInteract <= 0f )
			{
				_countTimeToUnblockInteract = 0f;
				_currentAttackingCharacter.interactAction = false;
				_startCountingtoUnblock = false;
			}
		}
	}
	
	void OnMouseUp ()
	{
		if (( UIControl.currentAttackBarUI != null ) && _currentAttackingCharacter != null && ( _currentAttackingCharacter.attacking )) UIControl.currentAttackBarUI.SendMessage ( "OnMouseUp" );
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		if ( selectedCharacter.myID == GameElements.CHAR_MADRA_1_IDLE )
		{
			if ( GetComponent < TipOnClickComponent > ()) GetComponent < TipOnClickComponent > ().deActivate ();
		}
		if ( GlobalVariables.checkForMenus ()) return;

		handleTouched ();
	}
	
	private void handleTouched ()
	{	
		if ( _alreadyTouched ) return;

		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		//==============================Daves Edit==================================
		if ( selectedCharacter.interactAction && !selectedCharacter._interactTrolley) return;//This was blocking auto select when enemies were clicked.
		CameraMovement.getInstance ().centreCam ( 0.8f );

		if ( Array.IndexOf ( GameElements.ATTACKERS, selectedCharacter.myID ) != -1 )
		{
			_alreadyTouched = true;
		}
		else
		{
			GameObject madraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_MADRA_1_IDLE );
			madraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
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

		if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][characterToAttack.position[0]][characterToAttack.position[1]] != GameElements.EMPTY )
		{
			return;
		}

		characterToAttack.interactAction = true;
		characterToAttack.attacking = true;
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToAttack.position[0]][characterToAttack.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForAttack ( true );
		
		int differenceX = _myIComponent.position[0] - characterToAttack.position[0];
		int differenceZ = _myIComponent.position[1] - characterToAttack.position[1];
		SoundManager.getInstance ().playSound (SoundManager.MADRA_GROWL);
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


		iTween.Stop ( transform.parent.gameObject );
		_mySelectedComponent.setSelected ( false );
		
		if ( differenceX > 0 ) transform.localScale = new Vector3 ( -_lookRightLocalScale, transform.localScale.y, transform.localScale.z );
		else transform.localScale = new Vector3 ( _lookRightLocalScale, transform.localScale.y, transform.localScale.z );
				
		StartCoroutine ( "waitBeforeCreateAttackButton", characterToAttack );
	}

	//=======================Daves Work======================
	private IEnumerator getTentacleReady (bool fromAttack) 
	{
		if(fromAttack)
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
		//myTarget = LevelControl.getInstance ().getSelectedCharacter ();
		//characterClossest = ToolsJerry.findClossestCharacterTileAroundThisTileRescue ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.myID, transform.parent.gameObject, _myIComponent.myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ATTACK_RANGE] );
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
		myParticlesInstance.SetActive (false);
		tentacleRaised = 0;
	}
	//=======================Daves Work======================

	private IEnumerator waitBeforeCreateAttackButton ( CharacterData characterToAttack )
	{
		yield return new WaitForSeconds ( 0.1f );
		_currentAttackingCharacter = characterToAttack;

		/*
		UIControl.currentAttackBarUI = _myAttackUIComboInstant = ( GameObject ) Instantiate ( Main.getInstance ().attackUIComboPrefab, transform.position + Vector3.up * 2f + Vector3.forward * 1.3f, Main.getInstance ().attackUIComboPrefab.transform.rotation );
		_myAttackUIComboInstant.GetComponent < AttackUIComboControl > ().initAttackObjects ( _handleAttackExecuted, myEnemyData );

		if ( GlobalVariables.TUTORIAL_MENU )
		{
			if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS )
			{
				_myAttackUIComboInstant.AddComponent < TutorialObjectTapComponent > ();
			}
		}
		*/
		 
		StartCoroutine ( handleAttackedButtonPressedPre ( true ));
	}

	private IEnumerator handleAttackedButtonPressedPre ( bool succsessful )
	{
		yield return new WaitForSeconds ( 0.9f );
		handleAttackedButtonPressed ( succsessful );
	}
	
	public void handleAttackedButtonPressed ( bool succsessful )
	{
		int differenceX = _myIComponent.position[0] - _currentAttackingCharacter.position[0];
		int differenceZ = _myIComponent.position[1] - _currentAttackingCharacter.position[1];

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

		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForAttack ( false );

		callBackWhenFinishedMove ( succsessful );
	}
	
	private void callBackWhenFinishedMove ( bool succsessful )
	{
		if ( ! _mySelectedComponent ) return;
		_countTimeToUnblockInteract = 2f;
		_startCountingtoUnblock = true;
		
		_currentAttackingCharacter.attacking = false;
				
		//SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_ATTACK, _currentAttackingCharacter.myID );
		
		if ( succsessful )
		{
			if ( GlobalVariables.TUTORIAL_MENU )
			{
				if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS )
				{
					//TutorialsManager.getInstance ().goToNextStep ();
				}
			}

			Main.getInstance ().removeOneMove ();
			_mySelectedComponent.setSelected ( false );
			
			myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ENDURANCE] -= _currentAttackingCharacter.characterValues[CharacterData.CHARACTER_ACTION_TYPE_DAMAGE];
			
			if ( myEnemyData.enemyValues[EnemyData.ENEMY_ACTION_TYPE_ENDURANCE] <= 0 )
			{
				if(myEnemyData.myID == GameElements.ENEM_SLUG_01)
				{
					SoundManager.getInstance ().playSound ( SoundManager.TENTACLE_DRAINER_DEFATED, _myIComponent.myID );
				}
				else
				{
					SoundManager.getInstance().playSound(SoundManager.SLUG_DEFEAT);
				}
				holdLeft = false;
				holdRight = false;
				myParticlesInstance.SetActive(false);
				GetComponent < EnemyAnimationComponent > ().currentAttackingCharacter = _currentAttackingCharacter;
				GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.DESTROY_ANIMATION );
				if(LevelControl.CURRENT_LEVEL_CLASS.myName == "6")
				{
					TutorialComponent.getInstance().takeAStep ++;
				}

				if(_currentAttackingCharacter.myID == GameElements.CHAR_MADRA_1_IDLE)
				{
					SoundManager.getInstance().playSound(SoundManager.MADRA_BITE);
				}
				else if(_currentAttackingCharacter.myID == GameElements.CHAR_BOZ_1_IDLE)
				{
					print ("BozAttack SFX here");
				}
				SoundManager.getInstance().playSound(SoundManager.TENTACLE_DRAINER_DEFATED);
				
				Instantiate ( Main.getInstance ().tentacleParticles, transform.position + Vector3.up * 5f, Quaternion.identity ); 
			}

			_alreadyTouched = false;
		}
		else
		{


			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		
			if ( GlobalVariables.TUTORIAL_MENU )
			{
				if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS )
				{
					if ( _myAttackUIComboInstant )
					{
						_myAttackUIComboInstant.SendMessage ( "externalCallForDestroy", SendMessageOptions.DontRequireReceiver );
						StartCoroutine ( "wiatBeforeInvokeRepeatTutorilStep" );
					}
				}
			}
			
			//_mySelectedComponent.setSelectedForAttack ( true );
			StartCoroutine ( "waitBeforeReplyAttack" );
		}
	}
	
	private IEnumerator wiatBeforeInvokeRepeatTutorilStep ()
	{
		yield return new WaitForSeconds ( 0.25f );
		TutorialsManager.getInstance ().invokeRepeatStep ();
	}

	private IEnumerator waitBeforeReplyAttack ()
	{
		yield return new WaitForSeconds ( 0.9f );
		_mySelectedComponent.setSelected ( false );
		holdLeft = false;
		holdRight = false;
		myParticlesInstance.SetActive (false);
		GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION );
		tentacleRaised = 0;
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ELECTROCUTED_ANIMATION, 1f );
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().electrickShock ( false );
		
		Handheld.Vibrate ();
		
		SoundManager.getInstance ().playSound ( SoundManager.ELECTROCUTED );
		
		SoundManager.getInstance ().playSound ( SoundManager.TENTACLE_DRAINER_ATTACK, _myIComponent.myID );
		
		//_mySelectedComponent.moveHereAndCallThis ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.position, callBackWhenStrikeBackFinished, false );
			
		GameObject powerParticles = ( GameObject ) Instantiate ( Main.getInstance ().particlesPower, LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_currentAttackingCharacter.position[0]][_currentAttackingCharacter.position[1]].transform.position, Quaternion.identity );
		powerParticles.AddComponent < AutoDestruct > ();

		print ( false );
		_alreadyTouched = false;
		yield return new WaitForSeconds ( 0.7f );
		SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_ATTACK_FAIL, _currentAttackingCharacter.myID );
	}
}
