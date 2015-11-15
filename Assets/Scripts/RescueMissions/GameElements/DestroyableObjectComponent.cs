using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DestroyableObjectComponent : ObjectTapControl 
{
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleChoosenCharacter;
	private IComponent _myIComponent;
	private int _hitNumber = 0;
	//*************************************************************//	
	void Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_myHandleChoosenCharacter = handleDemolish;
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
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		if ( GlobalVariables.checkForMenus ()) return;
		if (CheckInputType.getInstance().touchType != CheckInputType.TAP_TYPE)
		{
			return;
		}
		if(GlobalVariables.READY_FOR_NEXT_TURN == false)
		{
			return;
		}
		//GlobalVariables.READY_FOR_NEXT_TURN = false;
		print ("This One");
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( _alreadyTouched ) return;

		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		//==============================Daves Edit==================================
		if ( selectedCharacter.interactAction && !selectedCharacter._interactTrolley) return;//This was blocking auto select when destructables were clicked, added second condition.
		//==============================Daves Edit==================================
		//CameraMovement.getInstance ().centreCam ( 0.8f );
		if ( Array.IndexOf ( GameElements.BUILDERS, selectedCharacter.myID ) != -1 )
		{
			_alreadyTouched = true;
		}
		else
		{
			GameObject faradaydoObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );
			faradaydoObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
		}

		if ( SelectedComponenent.CURRENT_SELECTED_OBJECT ) SelectedComponenent.CURRENT_SELECTED_OBJECT.setSelected ( false );

		if ( GlobalVariables.TUTORIAL_MENU ) gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true );
		else gameObject.GetComponent < SelectedComponenent > ().setSelected ( true );

		Main.getInstance ().interactWithCurrentCharacter ( _myHandleChoosenCharacter, CharacterData.CHARACTER_ACTION_TYPE_DEMOLISH_RATE, _myIComponent );
	}
	
	public void handleDemolish ( CharacterData characterToBuild, bool onlyUnblocking = false )
	{
		if ( onlyUnblocking )
		{
			_alreadyTouched = false;
			return;
		}
		
		SoundManager.getInstance ().playSound ( SoundManager.DESTROYABLE_DEMOLISH, _myIComponent.myID );
		StartCoroutine ( "startBuildProcedure", characterToBuild );
		if(LevelControl.CURRENT_LEVEL_CLASS.myName == "5")
		{
			TutorialComponent.getInstance().takeAStep ++;
		}
	}
	
	private IEnumerator startBuildProcedure ( CharacterData characterToBuild )
	{	
//		print ("Score Added");
		GameGlobalVariables.Stats.SCORE += GameGlobalVariables.Stats.Scores.FOR_DESTROYING_ROCK;
		PointController points = PrefabPool.GetPool ("pointPrefab").Spawn (transform.position + Vector3.up * 2).GetComponent<PointController>()  as PointController;
		points.Init ("100", Color.white);

		Main.getInstance ().removeOneMove ();


		int differenceX = _myIComponent.position[0] - characterToBuild.position[0];
		
		characterToBuild.interactAction = true;
			
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_02_ANIMATION, 1.6f, differenceX );
		
		gameObject.GetComponent < SelectedComponenent > ().electrickShock ();
		yield return new WaitForSeconds ( 1.6f );
		MNSpawningEnemiesManager.getInstance().spawnRandomEnemy();
		GlobalVariables.READY_FOR_NEXT_TURN = true;
		print ("Ok Again");
		if ( GlobalVariables.TUTORIAL_MENU )
		{
			_hitNumber = 3;
		}
		else _hitNumber++;
		
		_hitNumber = 3;

		Texture2D myTextureDestroyedLevel1 = null;
		switch ( _hitNumber )
		{
			case 1:
				switch ( _myIComponent.myID )
				{
					case GameElements.ENVI_CRACKED_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_ALONE];
						break;
					case GameElements.ENVI_CRACKED_1_LEFT:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_LEFT];
						break;
					case GameElements.ENVI_CRACKED_1_MID:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_MID];
						break;
					case GameElements.ENVI_CRACKED_1_RIGHT:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_RIGHT];
						break;
				}
				
				renderer.material.mainTexture = myTextureDestroyedLevel1;
				break;
			case 2:
				switch ( _myIComponent.myID )
				{
					case GameElements.ENVI_CRACKED_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_ALONE];
						break;
					case GameElements.ENVI_CRACKED_1_LEFT:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_LEFT];
						break;
					case GameElements.ENVI_CRACKED_1_MID:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_MID];
						break;
					case GameElements.ENVI_CRACKED_1_RIGHT:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_RIGHT];
						break;
				}
				
				renderer.material.mainTexture = myTextureDestroyedLevel1;
				break;
			case 3:
				break;
		}
		
		Instantiate ( Main.getInstance ().rockParticles, transform.root.position + Vector3.up * 5f, Quaternion.identity );
		
		characterToBuild.interactAction = false;
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( false );
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		
		if ( _hitNumber == 3 )
		{
			Main.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.root.gameObject );
		}
		
		if ( characterToBuild.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] <= 0 )
		{
			Main.getInstance ().checkForOtherCharacterWithSkill ( characterToBuild, CharacterData.CHARACTER_ACTION_TYPE_BUILD_RATE, true );
		}
		
		_alreadyTouched = false;
	}
}
