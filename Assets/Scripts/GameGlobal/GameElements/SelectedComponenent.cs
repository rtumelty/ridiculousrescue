using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SelectedComponenent : MonoBehaviour 
{
	//*************************************************************//
	public static SelectedComponenent CURRENT_SELECTED_OBJECT;
	//*************************************************************//
	public GameObject followTarget;
	//*************************************************************//
	private EnemyComponent.HandleFinishedAnimationMove _myCallBackWhenFinishedMove;
	private bool _moveSucesfull = false;
	private Vector3 _initialScale;
	private Vector3 _initialPosition;
	private Quaternion _initialParentRotation;
	private bool _iAmSelected = false;
	private bool _iAmSelectedPermanently = false;
	private bool _fadeDown = false;
	private float _colorLight = 1f;
	private Material _myMaterial;
	private int _countPoping = 0;
	private int _countJumping = 0;
	private int _customNumberOfJumps;
	private bool _withAlphaFade = true;
	private bool _uiHighlight = false;
	private bool _pulsing;
	private bool awakeHasPlayed = false;
	private CharacterData myLastChar;
	//======================Daves Edit====================================
	public bool _activated;
	//======================Daves Edit====================================
	private float _customPulsingSizeFactor;
	private float _customJumpTime = 0.35f;
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	private GameObject _tileMarkPrefab;
	private GameObject _tileMarkCoraPrefab;
	private GameObject _tileMarkInstant;
	private GameObject myPassenger;
	private bool _goParent = false;
	//*************************************************************//
	void Awake () 
	{
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );
		_tileMarkPrefab = ( GameObject ) Resources.Load ( "Tile/tileMarkerCharacters" );
		_tileMarkCoraPrefab = ( GameObject ) Resources.Load ( "Tile/tileMarkerCora" );
		_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		if ( transform.parent ) _initialParentRotation = VectorTools.cloneQuaternion ( transform.parent.rotation );
		_myMaterial = renderer != null ? renderer.material : null;
		//if(GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY)
		//{
		//	transform.root.parent = GameObject.Find("Background").transform;
	//	}
	}
	
	void Start ()
	{
		if ( GetComponent < IComponent > ())
		{
			if ( Array.IndexOf ( GameElements.OPERATIONS_ON_PARENT, GetComponent < IComponent > ().myID ) != -1 )
			{
				_initialScale = VectorTools.cloneVector3 ( transform.parent.localScale );
				_goParent = true;
			}
		}
	}

	void OnMouseDown()
	{
		//======================Daves Edit====================================
		if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && !FLGlobalVariables.TUTORIAL_MENU)
		{
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
		}
		if (Application.loadedLevelName == "FL01" || Application.loadedLevelName == "FL00ChooseLevel")
		{
			_activated = true;
		}

		if(GetComponent< IComponent >() !=null &&( GetComponent < IComponent >().myID == 31 || GetComponent < IComponent >().myID == 32))
		{
			if(GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING)
			{
				GetComponent < MNEnemyComponent >().StartCoroutine ("getTentacleReady",true);
			}
			else if(GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE)
			{
//				GetComponent < EnemyComponent >().StartCoroutine ("getTentacleReady",true);
			}
		}
		//======================Daves Edit====================================
	}
	
	public void updateInitialValues ()
	{
		if ( GetComponent < IComponent > ())
		{
			if ( Array.IndexOf ( GameElements.OPERATIONS_ON_PARENT, GetComponent < IComponent > ().myID ) != -1 )
			{
				_initialScale = VectorTools.cloneVector3 ( transform.parent.localScale );
				_goParent = true;
			}
		}
		else _initialScale = VectorTools.cloneVector3 ( transform.localScale );
		if ( transform.parent ) _initialParentRotation = VectorTools.cloneQuaternion ( transform.parent.rotation );
	}
	
	void Update () 
	{
		if ( followTarget != null )
		{
			transform.position = new Vector3 ( Mathf.RoundToInt ( followTarget.transform.position.x ),  followTarget.transform.position.y - 0.75f, Mathf.RoundToInt ( followTarget.transform.position.z + 0.5f ));
		}
		
		if ( _iAmSelected )
		{
			if ( _countPoping >= 1 )
			{
				if ( _colorLight <= 0.55f ) _fadeDown = false;
				else if ( _colorLight >= 0.95f ) _fadeDown = true;
			}
			
			if ( _withAlphaFade )
			{
				if ( _fadeDown )
				{
					_colorLight = Mathf.Lerp ( _colorLight, 0.5f, 0.07f );
				}
				else
				{
					_colorLight = Mathf.Lerp ( _colorLight, 1f, 0.07f );
				}
				
				if ( _myMaterial != null ) _myMaterial.color = new Color ( _colorLight, _colorLight, _colorLight, 1f );
			}
		}
		else if ( _pulsing )
		{
			if ( _colorLight <= 0.05f ) _fadeDown = false;
			else if ( _colorLight >= 0.95f ) _fadeDown = true;
		
			if ( _fadeDown )
			{
				_colorLight = Mathf.Lerp ( _colorLight, 0.0f, 0.07f );
			}
			else
			{
				_colorLight = Mathf.Lerp ( _colorLight, 1f, 0.07f );
			}
			
			if ( _myMaterial != null ) _myMaterial.color = new Color ( 1f, 1f, 1f, _colorLight );
		}
	}
	//*************************************************************//
	public void setSelected ( bool isOn, bool force = false, bool withAlphaFade = true, bool permanent = false )
	{
		if (( ! force ) && ( _iAmSelected == isOn )) return;

		_iAmSelectedPermanently = permanent;
		_withAlphaFade = withAlphaFade;
		_iAmSelected = isOn;
		_countPoping = 0;
		
		if ( isOn )
		{
			CURRENT_SELECTED_OBJECT = this;
			SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT );
			if ( _myMaterial != null ) _myMaterial.shader = Shader.Find ( "Transparent/Diffuse" );	
			onCompleteTweenAnimationDown ();
		}
		else
		{
			if ( _goParent ) iTween.Stop ( this.transform.parent.gameObject );
			else iTween.Stop ( this.gameObject );
			
			if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
			else transform.localScale = VectorTools.cloneVector3 ( _initialScale );

			if ( _myMaterial != null ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
//===============================Daves Edit==============================
			Invoke ("stopTween",1.43f);
		}
	}

	public void stopTween()
	{
		if ( _goParent )
		{
			iTween.Stop ( this.transform.parent.gameObject );
		}

		if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
		else transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		
		if ( _myMaterial != null ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
	}
//===============================Daves Edit==============================	
	public void resetObject ()
	{
		if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
		else transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		
		if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
	}
	
	private void onCompleteTweenAnimationUp ()
	{
		_fadeDown = true;
		iTween.ScaleTo ( _goParent ? transform.parent.gameObject : this.gameObject, iTween.Hash ( "time", _customJumpTime, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale, "oncompletetarget", this.gameObject,  "oncomplete", "onCompleteTweenAnimationDown"));
	}
	
	private void onCompleteTweenAnimationDown ()
	{
		//print ("huh");
		if (( _countPoping >= 1 ) && ( ! _uiHighlight ))
		{
			if ( GetComponent < IComponent > ().myCharacterData != null && ! GetComponent < IComponent > ().myCharacterData.coraSlidingTroley )
			{
				if ( _goParent ) iTween.Stop ( this.transform.parent.gameObject );
				else iTween.Stop ( this.gameObject );
				
				iTween.ScaleTo ( _goParent ? transform.parent.gameObject : this.gameObject, iTween.Hash ( "time", _customJumpTime, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale, "oncompletetarget", this.gameObject ));
				if ( ! _iAmSelectedPermanently ) setSelected ( false );
			}
			return;
		}

		_fadeDown = false;
		_countPoping++;

		iTween.ScaleTo ( _goParent ? transform.parent.gameObject : this.gameObject, iTween.Hash ( "time", _customJumpTime, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale * 1.2f, "oncompletetarget", this.gameObject,  "oncomplete", "onCompleteTweenAnimationUp" ));
	}
	//*************************************************************//
	public void setSelectedForUIHighlight ( bool isOn, float customTime = 0.35f, bool withArrow = false )
	{
		_countPoping = 0;
		_uiHighlight = true;
		_customJumpTime = customTime;

		if ( withArrow )
		{
			_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * 1.7f + Vector3.up, Quaternion.identity );
			_jumpingArrowInstant.transform.parent = transform;
		}

		if ( isOn )
		{
			//===================================Daves Edit===================================
			if(awakeHasPlayed == true)
			{
				SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT );
			}
			awakeHasPlayed = true;
			//===================================Daves Edit===================================
			onCompleteTweenAnimationDown ();
//			print("this? ? ?");
		}
		else
		{
			Destroy ( _jumpingArrowInstant );

			iTween.Stop ( this.gameObject );
			if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
			else transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		}
	}
	//******************************************************************************************************//
	public void setSelectedForPulsing ( bool isOn, float pulsingSizeFactor = 1f, bool alphaFade = true )
	{
		if ( isOn )
		{
			_pulsing = alphaFade;
			_customJumpTime = 1.4f;
			_customPulsingSizeFactor = pulsingSizeFactor;
			_colorLight = 0.0f;
			onCompleteTweenAnimationDownPulse ();
//			print ("or this");
		}
		else
		{
			iTween.Stop ( this.gameObject );
			transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		}
	}
	
	private void onCompleteTweenAnimationDownPulse ()
	{
		_fadeDown = true;
		print ("called");
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", _customJumpTime, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale * _customPulsingSizeFactor, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationPulse"));
	}
	
	private void onCompleteTweenAnimationPulse ()
	{
		transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		onCompleteTweenAnimationDownPulse ();
		print ("this");
	}
	//******************************************************************************************************//
	public void setSelectedForPulsingCharacterMark ( bool isOn, float pulsingSizeFactor = 1f )
	{
		if ( isOn )
		{
			//_pulsing = true;
			_customJumpTime = 1.4f;
			_customPulsingSizeFactor = pulsingSizeFactor;
			_colorLight = 0.0f;
			onCompleteTweenAnimationDownPulseCharacterMark ();
		}
		else
		{
			iTween.Stop ( this.gameObject );
			transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		}
	}
	
	private void onCompleteTweenAnimationDownPulseCharacterMark ()
	{
//		print ("wah");
		_fadeDown = true;
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", _customJumpTime, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale * _customPulsingSizeFactor, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationPulseCharacterMark"));
	}
	
	private void onCompleteTweenAnimationPulseCharacterMark ()
	{
		_fadeDown = false;
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", _customJumpTime, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationDownPulseCharacterMark"));
	}
	//******************************************************************************************************//
	public void setSelectedForTutorial ( bool isOn, bool force = false, bool withAlphaFade = true, bool permanent = false, bool withArrow = true )
	{
		if (( ! force ) && ( _iAmSelected == isOn )) return;
		
		_iAmSelectedPermanently = permanent;
		_withAlphaFade = withAlphaFade;
		_iAmSelected = isOn;
		_countPoping = 0;

		if ( isOn )
		{
			if ( withArrow )
			{
				_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * 1.7f + Vector3.up, Quaternion.identity );
				_jumpingArrowInstant.transform.parent = transform;
			}
			
			SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT );
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Transparent/Diffuse" );
			onCompleteTweenAnimationDown ();
			print("this? ? ?");
		}
		else
		{
			Destroy ( _jumpingArrowInstant );

			iTween.Stop ( this.gameObject );
			if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
			else transform.localScale = VectorTools.cloneVector3 ( _initialScale );

			if ( _goParent ) iTween.Stop ( this.gameObject.transform.parent.gameObject );
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
		}
	}
	
	public void setSelectedForAttack ( bool isOn )
	{
		if ( isOn )
		{
			_customNumberOfJumps = 1;
			_initialPosition = VectorTools.cloneVector3 ( transform.position );
			onCompleteTweenAnimationJumpDown ();
		}
		else
		{
			iTween.Stop ( this.gameObject );
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
		}
	}
	
	public void setSelectedForBouncing ( bool isOn )
	{
		if ( isOn )
		{
			_initialPosition = VectorTools.cloneVector3 ( transform.position );
			onCompleteTweenAnimationJumpDownBouncing ();
		}
		else
		{
			iTween.Stop ( this.gameObject );
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
		}
	}
	//******************************************************************************************************//
	public void setSelectedForPassingCharacter ( bool isOn, Vector3 bumpToPosition )
	{
		if ( isOn )
		{
			if ( GlobalVariables.CHARACTER_PASSING_ANIMATION && bumpToPosition != Vector3.zero ) return;
			//=====================================Daves Edit======================================
			//This Line was breaking the characters away from the tile they should be on, I've commented it out and can't find any issues caused by its absence.
			//transform.parent.position = new Vector3 ((float) GetComponent < IComponent > ().position[0], (float) ( LevelControl.LEVEL_HEIGHT - GetComponent < IComponent > ().position[1] ), (float) GetComponent < IComponent > ().position[1] - 0.5f );

			GlobalVariables.CHARACTER_PASSING_ANIMATION = true;
			_initialPosition = VectorTools.cloneVector3 ( transform.position );
			if ( bumpToPosition != Vector3.zero ) moveHereAndCallThis ( bumpToPosition, null, true, true );
			else
			{
				if(!GlobalVariables.CORA_ALREADY_WITH_TOY_ON_TROLEY)
				{
					iTween.ShakeRotation ( transform.parent.gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeOutElastic, "amount", new Vector3 ( 0f, -35f, 0f ), "oncompletetarget", this.gameObject, "oncomplete", "onCompleteShake"));
				}
			}
			//=====================================Daves Edit======================================
			StopCoroutine ( "waitBeforeUnblockCharacterPassingAnimation" );
			StartCoroutine ( "waitBeforeUnblockCharacterPassingAnimation" );
		}
		else
		{
			iTween.Stop ( this.gameObject );
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
		}
	}
	
	private void onCompleteShake ()
	{
		transform.parent.rotation = VectorTools.cloneQuaternion ( _initialParentRotation );
	}
	
	private IEnumerator waitBeforeUnblockCharacterPassingAnimation ()
	{
		yield return new WaitForSeconds ( 1.1f );
		GlobalVariables.CHARACTER_PASSING_ANIMATION = false;
	}
	//******************************************************************************************************//
	public void setSelectedForBuilding ( bool isOn )
	{
		if ( isOn )
		{
			_initialPosition = VectorTools.cloneVector3 ( transform.position );
			onCompleteTweenAnimationJumpDownBuidling ();
		}
		else
		{
			iTween.Stop ( this.gameObject );
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
		}
	}
	
	public void setSelectedForCelebration ( bool isOn, int numberOfJumps = 3 )
	{
		if ( isOn )
		{
			_customNumberOfJumps = numberOfJumps;

			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE ) GlobalVariables.CHARACTER_CELEBRATING = true;
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING ) MNGlobalVariables.CHARACTER_CELEBRATING = true;
			
			GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.JUMP_ANIMATION );
			_initialPosition = VectorTools.cloneVector3 ( transform.position );
			GetComponent < IComponent > ().myCharacterData.blocked = true;
			onCompleteTweenAnimationJumpDownCelebration ();
		}
		else
		{
			GetComponent < IComponent > ().myCharacterData.blocked = false;
			GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.DOWN );
			iTween.Stop ( this.gameObject );
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
		}
	}
	
	public void borrowTileMarkerForThisObject ( GameObject targetObject )
	{
		if ( _tileMarkInstant != null )
		{
			_tileMarkInstant.GetComponent < SelectedComponenent > ().followTarget = targetObject;
			_tileMarkInstant.renderer.material.color = Color.white;
			_tileMarkInstant.renderer.material.mainTexture = ( Texture2D ) Resources.Load ( "Textures/UI_v2/char_highlight_active" );
		}
	}

	IEnumerator ResetSelectedCharFlag ()
	{
		yield return new WaitForSeconds (0.5f);
		GameGlobalVariables.CHARACTER_RECENTLY_CHANGED = false;
	}
	public void setSelectedCharacter ( bool isOn, bool fromAwake = false, bool fromTrolley = false )
	{
		bool cora = false;
		if ( isOn )
		{
			if ( Array.IndexOf ( GameElements.TO_BE_RESCUED, GetComponent < IComponent > ().myID ) == -1 )
			{
				if ( ! GlobalVariables.TOY_LAZARUS_SEQUENCE ) 
				{
					if(fromAwake == false)
					{
						SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT, gameObject.GetComponent < IComponent > ().myID );
					}
				}
				foreach ( CharacterData characterData in LevelControl.getInstance ().charactersOnLevel )
				{
					if (( characterData.myID != GetComponent < IComponent > ().myID ) && ( ! GlobalVariables.TOY_LAZARUS_SEQUENCE ))
					{
						characterData.selected = false;
						GameGlobalVariables.CHARACTER_RECENTLY_CHANGED = true;
						StartCoroutine ("ResetSelectedCharFlag");
						if ( characterData.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							Transform toBeRescued = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "toBeRescued" );
							
							if ( toBeRescued != null )
							{
								toBeRescued.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( false );
							}
						}
						
						LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( false );
					}
					else
					{

						characterData.selected = true;
						print ("gotID" + characterData.myID);
						if ( characterData.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							cora = true;
							myLastChar = characterData;
							Main.getInstance()._lastChar = myLastChar;
							Transform toBeRescued = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "toBeRescued" );
							
							if ( toBeRescued != null )
							{
								Debug.Log(toBeRescued);
								toBeRescued.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( true );///////////////Snarf
							}
						}
						else 
						{
							myLastChar = null;
						}
					}
				}
				
				if ( ! GlobalVariables.TOY_LAZARUS_SEQUENCE ) 
				{
					if ( _tileMarkInstant == null )
					{
						if (cora) {
							CoraMarkerScale scaleControl = FindObjectOfType<CoraMarkerScale>() as CoraMarkerScale;
							if (scaleControl != null) scaleControl.UpdateScaleAndPosition(CoraMarkerScale.Direction.NONE);
							
							_tileMarkInstant = ( GameObject ) Instantiate ( _tileMarkCoraPrefab, transform.parent.position + Vector3.down * 0.25f, _tileMarkPrefab.transform.rotation );
						}
						else
							_tileMarkInstant = ( GameObject ) Instantiate ( _tileMarkPrefab, transform.parent.position + Vector3.down * 0.25f + Vector3.back * 0.5f, _tileMarkPrefab.transform.rotation );
						SelectedComponenent currentSelectedComponenent = _tileMarkInstant.AddComponent < SelectedComponenent > ();

						if (!cora) currentSelectedComponenent.setSelectedForPulsingCharacterMark ( true, 1.2f );

						currentSelectedComponenent.followTarget = this.transform.parent.gameObject;
						GetComponent < IComponent > ().myCharacterData.myTileMarker = _tileMarkInstant;
					}
				}
				else
				{
					Destroy ( _tileMarkInstant );
					return;
				}
			}
			
			_countPoping = 0;
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );

			if(fromTrolley == false)
			{
				onCompleteTweenAnimationDown ();
			}
//			print("this? this? this?");
		}
		else if ( GetComponent < IComponent > ().myCharacterData != null && ! GetComponent < IComponent > ().myCharacterData.selected )
		{
			if ( Array.IndexOf ( GameElements.TO_BE_RESCUED, GetComponent < IComponent > ().myID ) == -1 )
			{
				if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
				else transform.localScale = VectorTools.cloneVector3 ( _initialScale );
			}

			Destroy ( _tileMarkInstant );
			
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
		}
	}
	
	public void MN_setSelectedCharacter ( bool isOn )
	{
		if ( isOn )
		{
			SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT, gameObject.GetComponent < IComponent > ().myID );
				
			foreach ( CharacterData characterData in MNLevelControl.getInstance ().charactersOnLevel )
			{
				if ( characterData.myID != GetComponent < IComponent > ().myID )
				{
					characterData.selected = false;
					MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedCharacter ( false );
				}
				else
				{
					characterData.selected = true;
				}
			}
			
			if ( _tileMarkInstant == null)
			{
				_tileMarkInstant = ( GameObject ) Instantiate ( _tileMarkPrefab, transform.parent.position + Vector3.down * 0.25f + Vector3.back * 0.5f, _tileMarkPrefab.transform.rotation );
				SelectedComponenent currentSelectedComponenent = _tileMarkInstant.AddComponent < SelectedComponenent > ();
				currentSelectedComponenent.setSelectedForPulsingCharacterMark ( true, 1.5f );
				currentSelectedComponenent.followTarget = this.transform.parent.gameObject;
			}
			
			_countPoping = 0;
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
			
			onCompleteTweenAnimationDown ();
			print("this? ? ?");
		}
		else if ( ! GetComponent < IComponent > ().myCharacterData.selected )
		{
			if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
			else transform.localScale = VectorTools.cloneVector3 ( _initialScale );
			
			Destroy ( _tileMarkInstant );
			
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
		}
	}
	
	public void FL_setSelectedCharacter ( bool isOn, bool fromAwake = false )
	{
		if ( isOn )
		{
			if(fromAwake == false)
			{
				SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT, gameObject.GetComponent < IComponent > ().myID );
			}
			foreach ( CharacterData characterData in FLLevelControl.getInstance ().charactersOnLevel )
			{
				if ( characterData.myID != GetComponent < IComponent > ().myID )
				{
					characterData.selected = false;
					FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][characterData.position[0]][characterData.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().FL_setSelectedCharacter ( false );
				}
				else
				{
					characterData.selected = true;
				}
			}
			//======================Daves Edit====================================

			if ( ! GlobalVariables.TOY_LAZARUS_SEQUENCE ) 
			{
				if ( _tileMarkInstant == null && _activated == true)
				{
					_tileMarkInstant = ( GameObject ) Instantiate ( _tileMarkPrefab, transform.parent.position + Vector3.down * 0.25f + Vector3.back * 0.5f, _tileMarkPrefab.transform.rotation );
					SelectedComponenent currentSelectedComponenent = _tileMarkInstant.AddComponent < SelectedComponenent > ();
					currentSelectedComponenent.setSelectedForPulsingCharacterMark ( true, 1.5f );
					currentSelectedComponenent.followTarget = this.transform.parent.gameObject;
					_activated = false;
				}
			}
			else
			{
				Destroy ( _tileMarkInstant );
				return;
			}
		
			_countPoping = 0;
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
			
			onCompleteTweenAnimationDown ();
//			print("this? ? ?");
		}
		else if ( GetComponent < IComponent > ().myCharacterData != null && ! GetComponent < IComponent > ().myCharacterData.selected )
		{
			if ( Array.IndexOf ( GameElements.TO_BE_RESCUED, GetComponent < IComponent > ().myID ) == -1 )
			{
				if ( _goParent ) transform.parent.localScale = VectorTools.cloneVector3 ( _initialScale );
				else transform.localScale = VectorTools.cloneVector3 ( _initialScale );
			}
			
			Destroy ( _tileMarkInstant );
			
			if ( _myMaterial ) _myMaterial.shader = Shader.Find ( "Unlit/Transparent" );
		}
		//======================Daves Edit====================================
	}

	public void FL_setSelectedCharacterOnlyEffect ( bool isOn )
	{
		if ( isOn )
		{
			SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT, gameObject.GetComponent < IComponent > ().myID );
			_countPoping = 0;
			onCompleteTweenAnimationDown ();
			print("this? ? ?");
		}
		else
		{
			transform.localScale = VectorTools.cloneVector3 ( _initialScale );
			iTween.Stop ( this.gameObject );
		}
	}
	
	public void moveHereAndCallThis ( Vector3 positionToMove, EnemyComponent.HandleFinishedAnimationMove callBackWhenFinidhedMove, bool success, bool withBoune = false )
	{
		_initialPosition = VectorTools.cloneVector3 ( transform.position );
		
		_myCallBackWhenFinishedMove = callBackWhenFinidhedMove;
		_moveSucesfull = success;
		
		if ( withBoune ) iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.linear, "position", positionToMove + Vector3.forward * 0.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationMoveToPositionWithBounce" ));
		else iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeInOutCubic, "position", positionToMove + Vector3.forward * 0.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationMoveToPosition" ));
	}
	
	private void onCompleteTweenAnimationMoveToPosition ()
	{
		if ( _myCallBackWhenFinishedMove != null ) _myCallBackWhenFinishedMove ( _moveSucesfull );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeInOutCubic, "position", _initialPosition, "oncompletetarget", this.gameObject, "oncomplete", "centerPosition" ));		
	}
	
	private void onCompleteTweenAnimationMoveToPositionWithBounce ()
	{
		if ( _myCallBackWhenFinishedMove != null ) _myCallBackWhenFinishedMove ( _moveSucesfull );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeOutElastic, "position", _initialPosition, "oncompletetarget", this.gameObject, "oncomplete", "centerPosition" ));		
	}

	private void centerPosition ()
	{
		transform.parent.position = new Vector3 ((float) GetComponent < IComponent > ().position[0], (float) ( LevelControl.LEVEL_HEIGHT -  GetComponent < IComponent > ().position[1] ), (float) GetComponent < IComponent > ().position[1] - 0.5f );
	}
	
	//*************************************************************//
	private void onCompleteTweenAnimationJumpUp ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "oncompletetarget", this.gameObject,  "oncomplete", "onCompleteTweenAnimationJumpDown" ));
	}
	
	private void onCompleteTweenAnimationJumpUpBouncing ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.55f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpDownBouncing" ));
	}
	
	private void onCompleteTweenAnimationJumpUpCelebration ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.13f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpDownCelebration"));
	}
	
	private void onCompleteTweenAnimationJumpUpBuidling ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpDownBuidling"));
	}
	
	private void onCompleteTweenAnimationJumpDownBuidling ()
	{
		if ( _countJumping >= 3 )
		{
			_countJumping = 0;
			return;
		}
		
		_countJumping++;
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.15f, "easetype", iTween.EaseType.easeOutCubic, "position", _initialPosition + Vector3.forward, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpUpBuidling"));
	}
	
	private void onCompleteTweenAnimationJumpDown ()
	{
		if ( _countJumping >= _customNumberOfJumps )
		{
			_countJumping = 0;
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE ) GlobalVariables.CHARACTER_CELEBRATING = false;
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING ) MNGlobalVariables.CHARACTER_CELEBRATING = false;
			return;
		}
		
		_countJumping++;
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", UnityEngine.Random.Range ( 0.07f, 0.2f ), "easetype", iTween.EaseType.easeOutCubic, "position", _initialPosition + Vector3.forward, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpUp"));
	}
	
	private void onCompleteTweenAnimationJumpDownBouncing ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", UnityEngine.Random.Range ( 0.2f, 0.7f ), "easetype", iTween.EaseType.easeOutCubic, "position", _initialPosition + Vector3.forward * 0.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpUpBouncing"));
	}
	
	private void onCompleteTweenAnimationJumpDownCelebration ()
	{
		if ( _countJumping >= _customNumberOfJumps )
		{
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE ) GlobalVariables.CHARACTER_CELEBRATING = false;
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING ) MNGlobalVariables.CHARACTER_CELEBRATING = false;

			GetComponent < IComponent > ().myCharacterData.blocked = false;
			GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.DOWN );
			_countJumping = 0;
			return;
		}
		
		_countJumping++;
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", UnityEngine.Random.Range ( 0.04f, 0.07f ), "easetype", iTween.EaseType.easeOutCubic, "position", _initialPosition + Vector3.forward, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationJumpUpCelebration"));
	}

	public void electrickShock ( bool withMessageToMoveOneTile = true, float time = 1f )
	{
		iTween.ShakePosition ( transform.parent.gameObject, iTween.Hash ( "amount", Vector3.one * 0.17f, "time", time, "easetype", iTween.EaseType.easeInOutBack, "oncomplete", withMessageToMoveOneTile ? "onCompleteElectricShock" : "NULL"  ));
	}
}
