using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNDestroyableObjectComponent : ObjectTapControl 
{
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleChoosenCharacter;
	private IComponent _myIComponent;
	private int _hitNumber = 0;
	private bool clickedAfter = false;
	private GameObject _bozObject;
	private int myID;
	//*************************************************************//	
	void Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_myHandleChoosenCharacter = handleDemolish;
		myID = transform.parent.Find("tile").GetComponent < IComponent > ().myID;
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

		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();

		if ( Array.IndexOf ( GameElements.BUILDERS, selectedCharacter.myID ) != -1 )
		{
			if ( GetComponent < TipOnClickComponent > ()) GetComponent < TipOnClickComponent > ().deActivate ();
		}
		else
		{
			GameObject faradaydoObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );

			if ( faradaydoObject != null )
			{
				faradaydoObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
			else
			{
				GameObject bozObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_BOZ_1_IDLE );
				bozObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
		}

		if ( GlobalVariables.checkForMenus ()) return;
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
		if ( GlobalVariables.TUTORIAL_MENU )
		{
			clickedAfter = true;
		}
		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		if ( selectedCharacter.interactAction ) return;
		if ( Array.IndexOf ( GameElements.BUILDERS, selectedCharacter.myID ) != -1 )
		{
			_alreadyTouched = true;
		}

		if ( SelectedComponenent.CURRENT_SELECTED_OBJECT ) SelectedComponenent.CURRENT_SELECTED_OBJECT.setSelected ( false );

		if ( GlobalVariables.TUTORIAL_MENU ) gameObject.GetComponent < SelectedComponenent > ().setSelected ( true,true );
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
		if(characterToBuild.myID == 40)
		{
			SoundManager.getInstance ().playSound ( SoundManager.DESTROYABLE_DEMOLISH, _myIComponent.myID );
		}
		StartCoroutine ( "startBuildProcedure", characterToBuild );
		if(LevelControl.CURRENT_LEVEL_CLASS.myName == "5")
		{
			TutorialComponent.getInstance().takeAStep ++;
		}
	}
	
	private IEnumerator startBuildProcedure ( CharacterData characterToBuild )
	{	
		print ("Score Added");
		GameGlobalVariables.Stats.SCORE += GameGlobalVariables.Stats.Scores.FOR_DESTRYING_GOLD_ROCK;
		PointController points = PrefabPool.GetPool ("pointPrefab").Spawn (transform.position + Vector3.up * 2).GetComponent<PointController>() as PointController;
		points.Init ("100", Color.yellow);

		Main.getInstance ().removeOneMove ();


		int differenceX = _myIComponent.position[0] - characterToBuild.position[0];
		
		characterToBuild.interactAction = true;

		bool isBoz = false;

		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_02_ANIMATION, 1.6f, differenceX );

		gameObject.GetComponent < SelectedComponenent > ().electrickShock ();

		yield return new WaitForSeconds ( 1.6f );
		GlobalVariables.READY_FOR_NEXT_TURN = true;
		print ("Ok Again");
		MNSpawningEnemiesManager.getInstance ().spawnRandomEnemy ();
		_hitNumber++;


		if ( Array.IndexOf ( GameElements.CRACKED_ROCKS_OBJECTS, _myIComponent.myID ) != -1 )
		{
			_hitNumber = 3;
		}

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
					
					case GameElements.ENVI_METAL_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_METAL_2_ALONE];
						break;
					case GameElements.ENVI_PLASTIC_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_PLASTIC_2_ALONE];
						break;
					case GameElements.ENVI_TECHNOEGG_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_TECHNOEGG_2_ALONE];
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
					case GameElements.ENVI_METAL_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_METAL_3_ALONE];
						break;
					case GameElements.ENVI_PLASTIC_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_PLASTIC_3_ALONE];
						break;
					case GameElements.ENVI_TECHNOEGG_1_ALONE:
						myTextureDestroyedLevel1 = LevelControl.getInstance ().gameElements[GameElements.ENVI_TECHNOEGG_3_ALONE];
						break;
				}
				
				renderer.material.mainTexture = myTextureDestroyedLevel1;
				break;
			case 3:
				break;
		}
		
		switch ( _myIComponent.myID )
		{
			case GameElements.ENVI_METAL_1_ALONE:
				Instantiate ( Main.getInstance ().rockParticles, transform.root.position + Vector3.up, Quaternion.identity );
				int numberOfRandomResourcesMetal = UnityEngine.Random.Range ( MiningResourcesControl.MINIMUM_METAL_RESOURCES, MiningResourcesControl.MAXIMUM_METAL_RESOURCES );
				MiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_METAL, numberOfRandomResourcesMetal, _myIComponent.position );
				break;
			case GameElements.ENVI_PLASTIC_1_ALONE:
				Instantiate ( Main.getInstance ().rockParticles, transform.root.position + Vector3.up, Quaternion.identity );
				int numberOfRandomResourcesPlastic = UnityEngine.Random.Range ( MiningResourcesControl.MINIMUM_PLASTIC_RESOURCES, MiningResourcesControl.MAXIMUM_PLASTIC_RESOURCES );
				MiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_PLASTIC, numberOfRandomResourcesPlastic, _myIComponent.position );
				break;
			case GameElements.ENVI_TECHNOEGG_1_ALONE:
				Instantiate ( Main.getInstance ().tentacleParticles, transform.root.position + Vector3.up, Quaternion.identity );
				if ( _hitNumber == 3 )
				{
					int numberOfRandomResourcesVines = UnityEngine.Random.Range ( MiningResourcesControl.MINIMUM_VINES_RESOURCES, MiningResourcesControl.MAXIMUM_VINES_RESOURCES );
					MiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_VINES, numberOfRandomResourcesVines, _myIComponent.position );

					if ( UnityEngine.Random.Range ( 0, 2 ) < 1 )
					{
						MiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_TECHNOSEED, 1, _myIComponent.position );
					}
				}
				break;
			default:
			{
			//print (_hitNumber);
			//print ("boosh3");
				Instantiate ( Main.getInstance ().rockParticles, transform.root.position + Vector3.up, Quaternion.identity );
			}
				break;
		}

		characterToBuild.interactAction = false;
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( false );
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		
		if ( _hitNumber == 3 )
		{
			Main.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.root.gameObject );
		}
		
		_alreadyTouched = false;
	}

	private void finishedJumpOnRockAbove ()
	{
		iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.1f, "easetype", iTween.EaseType.linear, "position", this.transform.position + Vector3.forward * 0.5f + Vector3.up ));
	}
}
