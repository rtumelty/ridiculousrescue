using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ToyLazarusSequenceControl : MonoBehaviour 
{
	//*************************************************************//	
	public GameObject lightEmmiter01;
	public GameObject lightTarget01;
	public Texture2D[] animationOfRoofClosing;
	public Texture2D[] animationOfAntenas;
	public Texture2D backDoneTexture;
	public Renderer[] antenaLightsRenders;
	public GameObject[] antenaLightsFlashesObjects;
	public Transform[] pathForBounce;
	//*************************************************************//	
	private int stepID = 0;
	private int statrtedStepID = -1;
	private List < GameObject > charactersObject;
	private GameObject _coraObject;
	private bool _skipped = false;
	private Material _roofMaterial;
	private GameObject _rocObject;
	private int _animationFrame;
	//*************************************************************//	
	private static ToyLazarusSequenceControl _meInstance;
	public static ToyLazarusSequenceControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "BackgroundToyLazarus" ).GetComponent < ToyLazarusSequenceControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	public void initToyLazarusSequence ( int toBeRescuedID ) 
	{
		Camera.main.orthographicSize = ZoomAndLevelDrag.MAXIMUM_CAMERA_ZOOM;

		lightEmmiter01.gameObject.SetActive ( true );

		_roofMaterial = transform.Find ( "roof" ).renderer.material;
		_rocObject = transform.Find ( "roc" ).gameObject;

		SoundManager.getInstance ().stopAllSFX ();
		
		if ( ! LevelControl.getInstance ().coresAreOnLevel ) 
		{
			transform.Find ( "LightAndBeams" ).gameObject.SetActive ( false );
			//lightEmmiter01.transform.localPosition = new Vector3 ( 0.42f, 14.18f, 0.20f );
			//lightTarget01.transform.localPosition = new Vector3 ( 7.77f, 14.18f, -0.39f );
		}

		Camera.main.transform.Find ( "UI" ).transform.position = new Vector3 ( 0f, 60f, 0f );
		
		charactersObject = new List < GameObject > ();
		
		int characterPositionFactor = 0;
		for ( int i = 0; i < LevelControl.getInstance ().charactersOnLevel.Count; i++ )
		{
			if ( LevelControl.getInstance ().charactersOnLevel[i].myID != GameElements.CHAR_CORA_1_IDLE )
			{
				charactersObject.Add ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][LevelControl.getInstance ().charactersOnLevel[i].position[0]][LevelControl.getInstance ().charactersOnLevel[i].position[1]] );
				iTween.Stop ( charactersObject[charactersObject.Count - 1].transform.Find ( "tile" ).gameObject );
				iTween.Stop ( charactersObject[charactersObject.Count - 1].gameObject );
				charactersObject[charactersObject.Count - 1].transform.position = new Vector3 ( -16.2f + 2f * characterPositionFactor, 4.5f, 4f );
				characterPositionFactor++;
			}
			else
			{
				_coraObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][LevelControl.getInstance ().charactersOnLevel[i].position[0]][LevelControl.getInstance ().charactersOnLevel[i].position[1]];
				iTween.Stop ( _coraObject.transform.Find ( "tile" ).gameObject );
				iTween.Stop ( _coraObject );
				_coraObject.transform.Find ( "tile" ).transform.localPosition = new Vector3 ( 1.421687f, -0.4152609f, -0.4099993f );
				LevelControl.getInstance ().toBeRescuedOnLevel.transform.position = _coraObject.transform.position + Vector3.left * 0.05f + Vector3.up * 0.5f;
				LevelControl.getInstance ().toBeRescuedOnLevel.SetActive ( true );
				LevelControl.getInstance ().toBeRescuedOnLevel.transform.parent = _coraObject.transform;
				LevelControl.getInstance ().toBeRescuedOnLevel.name = "toBeRescued";
			}
		}
		
		GlobalVariables.TOY_LAZARUS_SEQUENCE = true;
		
		foreach ( GameObject characterObject in charactersObject )
		{
			characterObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().reloadTextures ();
			characterObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.DOWN );
			characterObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( true );
		}

		_coraObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( true );
	}
	
	void Update () 
	{
		if ( ! GlobalVariables.TOY_LAZARUS_SEQUENCE ) return;
		if ( statrtedStepID == stepID ) return;
		if ( _skipped ) return;
		switch ( stepID )
		{
			case 0:
				SoundManager.getInstance ().silenceMusicTillNewScene ();
				if ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "troley" ) != null ) Destroy ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "troley" ).gameObject );
			
				if ( ! GameGlobalVariables.BLOCK_LAB_ENTERED )
				{
					if ( ! GameGlobalVariables.CUT_DOWN_GAME )
					{
						GameGlobalVariables.Stats.NewResources.RECHARGEOCORES--;
					}
				}
				Camera.main.gameObject.AddComponent < ToyLazarusSequenceSlideCamera > ();
				StaticObjectWithAnimation currentStaticObjectWithAnimation = transform.Find ( "LightAndBeams" ).gameObject.AddComponent < StaticObjectWithAnimation > ();
				currentStaticObjectWithAnimation.uploadAnimation ( "Textures/Envi/Energy/PowerPlugAnimation" );		
				break;
			case 1:
				//SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_TROLEY_MOVE, GameElements.CHAR_CORA_1_IDLE );
				_coraObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.INTERACT_LEFT );
				_coraObject.transform.Find ( "tile" ).localScale = VectorTools.cloneVector3 ( RescuerComponent.CORA_WITHOUT_TROLEY_SCALE );
				_coraObject.AddComponent < ToyLazarusSequenceCoraMove > ();
				break;
			case 2:
				SoundManager.getInstance ().playSound ( SoundManager.MACHINE_OPENS );
				InvokeRepeating ( "changeFrameForRoof", 0f, 0.4f );
				break;
			case 3:
				SoundManager.getInstance ().playSound ( SoundManager.MACHINE_CLOSES_PLUS_LOCK );
				iTween.MoveFrom ( transform.Find ( "roof" ).gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeOutBounce, "position", transform.Find ( "roof" ).position + Vector3.forward * 1.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteShake"));
				break;
			case 4:
				iTween.MoveTo ( _rocObject, iTween.Hash ( "time", 1.5f, "position", new Vector3 ( 0f, 1f, -0.075f ), "oncompletetarget", this.gameObject, "oncomplete", "goToNextStep", "islocal", true  ));
			    break;
			case 5:
			{
				GameObject roofPart = transform.Find ( "roofPart" ).gameObject;
				roofPart.renderer.enabled = true;
				iTween.MoveTo ( roofPart, iTween.Hash ( "time", 1.5f, "position", new Vector3 ( roofPart.transform.localPosition.x, roofPart.transform.localPosition.y, -0.168f ), "islocal", true ));
				_animationFrame = 0;
				InvokeRepeating ( "changeFrameForAntenas", 0f, 0.4f );
				break;
			}
			case 6:
				StartCoroutine ( "waitBeforeNextStep", 1.5f );
				break;
			case 7:
				//SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_TROLEY_MOVE, GameElements.CHAR_CORA_1_IDLE );
				SoundManager.getInstance ().playSound ( SoundManager.REDIRECTOR_POWER_UP, GameElements.ENVI_REDIRECTOR_01 );
				iTween.ShakePosition ( Camera.main.gameObject, iTween.Hash ( "amount", Vector3.one * 0.17f, "time", 2f, "easetype", iTween.EaseType.easeInOutBack ));
				StartCoroutine ( "waitBeforeNextStep", 2.2f );
				transform.Find ( "lightFromWindow" ).renderer.enabled = true;

				foreach ( GameObject flash in antenaLightsFlashesObjects )
				{
					flash.renderer.enabled = true;
				}
				break;
			case 8:
			{
				_rocObject.renderer.enabled = false;
				foreach ( GameObject flash in antenaLightsFlashesObjects )
				{
					flash.renderer.enabled = false;
				}
				
				GameObject roofPart = transform.Find ( "roofPart" ).gameObject;
				roofPart.renderer.enabled = true;
				iTween.MoveTo ( roofPart, iTween.Hash ( "time", 1.5f, "position", new Vector3 ( roofPart.transform.localPosition.x, roofPart.transform.localPosition.y, -0.139f ), "islocal", true ));
				_animationFrame = animationOfAntenas.Length - 1;
				InvokeRepeating ( "changeFrameForAntenasReverse", 0f, 0.4f );

				transform.Find ( "lightFromWindow" ).renderer.enabled = false;
				transform.Find ( "back" ).renderer.material.mainTexture = backDoneTexture;
				LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.DOWN );
				StartCoroutine ( "waitBeforeNextStep", 1.5f );
				break;
			}
			case 9:
			{
				_animationFrame = animationOfRoofClosing.Length - 1;
				InvokeRepeating ( "changeFrameForRoofRevearse", 0f, 0.4f );
				SoundManager.getInstance ().playSound ( SoundManager.MACHINE_OPENS_END );
				StartCoroutine ( "waitBeforeNextStep", 1.1f );
				break;
			}
			case 10:
				SoundManager.getInstance ().playSound ( SoundManager.MISSION_COMPLETE );
				foreach ( GameObject characterObject in charactersObject )
				{
					characterObject.AddComponent < ToyLazarusSequenceAnimateCelebrate > ();
				}
			
				_coraObject.AddComponent < ToyLazarusSequenceAnimateCelebrate > ();
				LevelControl.getInstance ().toBeRescuedOnLevel.AddComponent < ToyLazarusSequenceAnimateCelebrate > ();
				StartCoroutine ( "waitBeforeNextStep", 1.5f );
				break;
			case 11:
				ResoultScreen.getInstance ().CustomStart ();
				print ("Here?");
				break;
		}
		
		statrtedStepID = stepID;
	}

	private void onCompleteShake ()
	{
		goToNextStep ();
	}

	private void changeFrameForRoof ()
	{
		_roofMaterial.mainTexture = animationOfRoofClosing[_animationFrame];
		_animationFrame++;

		if ( _animationFrame >= animationOfRoofClosing.Length )
		{
			CancelInvoke ( "changeFrameForRoof" );
			goToNextStep ();
		}
	}

	private void changeFrameForRoofRevearse ()
	{
		_roofMaterial.mainTexture = animationOfRoofClosing[_animationFrame];
		_animationFrame--;
		
		if ( _animationFrame < 0 )
		{
			CancelInvoke ( "changeFrameForRoofRevearse" );
		}
	}

	private void changeFrameForAntenas ()
	{
		foreach ( Renderer anteanRenderer in antenaLightsRenders )
		{
			anteanRenderer.enabled = true;
			anteanRenderer.material.mainTexture = animationOfAntenas[_animationFrame];
		}

		_animationFrame++;

		if ( _animationFrame >= animationOfAntenas.Length )
		{
			CancelInvoke ( "changeFrameForAntenas" );
			goToNextStep ();
		}
	}

	private void changeFrameForAntenasReverse ()
	{
		foreach ( Renderer anteanRenderer in antenaLightsRenders )
		{
			anteanRenderer.enabled = true;
			anteanRenderer.material.mainTexture = animationOfAntenas[_animationFrame];
		}
		
		_animationFrame--;
		
		if ( _animationFrame < 0 )
		{
			foreach ( Renderer anteanRenderer in antenaLightsRenders )
			{
				anteanRenderer.enabled = false;
			}

			GameObject roofPart = transform.Find ( "roofPart" ).gameObject;
			roofPart.renderer.enabled = false;
			
			CancelInvoke ( "changeFrameForAntenasReverse" );
		}
	}
	
	private IEnumerator waitBeforeNextStep ( float delay )
	{
		yield return new WaitForSeconds ( delay );
		goToNextStep ();
	}
	
	public void goToNextStep ()
	{
		stepID++;
	}
	
	public void skipAll ()
	{
		print ("Here?");
		_skipped = true;
		ResoultScreen.getInstance ().CustomStart ();
	}
}
