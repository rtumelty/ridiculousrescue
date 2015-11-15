using UnityEngine;
using System.Collections;

public class CharacterButton : MonoBehaviour 
{
	//*************************************************************//	
	public int initialPower;
	//*************************************************************//	
	private CharacterData _myCharacterData;
	private TextMesh _myText01;
	//private Vector3 _initialScale;
	private bool _selected = false;
	private Material _myBackMaterial;
	private Material _myPowerBarMaterial;
	private bool _noPowerNotification = false;
	private Vector3 _initialScaleButtonBack;
	private GameObject _buttonBack;
	//*************************************************************//	
	void Start () 
	{
		//_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE /*|| GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN */)
		{
			_myText01 = transform.Find ( "3DText01" ).GetComponent < TextMesh > ();
		}
		_myBackMaterial = renderer.material;
		_myPowerBarMaterial = transform.Find ( "powerBar" ).renderer.material;

		_buttonBack = transform.Find ( "ButtonBack2" ).gameObject;
		_initialScaleButtonBack = VectorTools.cloneVector3 ( _buttonBack.transform.localScale );
	}
	
	void Update () 
	{
		if ( ! _selected && _myCharacterData.selected )
		{
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
			{
				_myBackMaterial.mainTexture = UIControl.getInstance ().textureUIButtonSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureUIButtonSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureUIButtonSelectedDown;

				if ( _myCharacterData.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
				{
					if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_DUMMY )
					{
						TutorialsManager.getInstance ().goToNextStep ();
					}
				}

				CameraMovement.getInstance ().followOnXAxis ( LevelControl.getInstance ().getCharacterObjectFromLevel ( _myCharacterData.myID ).transform );
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
			{
				_myBackMaterial.mainTexture = UIControl.getInstance ().textureUIButtonSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureUIButtonSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureUIButtonSelectedDown;
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
			{
				_myBackMaterial.mainTexture = MNUIControl.getInstance ().textureUIButtonSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = MNUIControl.getInstance ().textureUIButtonSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = MNUIControl.getInstance ().textureUIButtonSelectedDown;
			}
			
			_selected = true;
		}
		else if ( _selected && ! _myCharacterData.selected )
		{
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
			{
				_myBackMaterial.mainTexture = UIControl.getInstance ().textureUIButtonNotSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureUIButtonNotSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureUIButtonNotSelectedDown;
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
			{
				_myBackMaterial.mainTexture = UIControl.getInstance ().textureUIButtonNotSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureUIButtonNotSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureUIButtonNotSelectedDown;
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
			{
				_myBackMaterial.mainTexture = MNUIControl.getInstance ().textureUIButtonNotSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = MNUIControl.getInstance ().textureUIButtonNotSelectedUp;
				GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = MNUIControl.getInstance ().textureUIButtonNotSelectedDown;
			}
			
			_selected = false;
		}

		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING || _myText01 != null && _myText01.text != _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER].ToString ())
		{
			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE || GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN)
			{
				iTween.ScaleFrom ( _myText01.gameObject, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _myText01.transform.localScale * 2f ));
				_myText01.text = _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER].ToString ();
			}

			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
			{
				if ( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] == 0 )
				{
					_myPowerBarMaterial.mainTexture = UIControl.getInstance ().powerBarTextures[0];
				}
				else _myPowerBarMaterial.mainTexture = UIControl.getInstance ().powerBarTextures[(int) (((( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] * 100 ) / initialPower ) / 12 ) + 1 )];
			}
			else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
			{
				if ( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] == 0 )
				{
					if ( ! _noPowerNotification )
					{
						_noPowerNotification = true;

						iTween.ScaleFrom ( _buttonBack, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _initialScaleButtonBack * 1.4f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteNotification" ));
					}

					transform.Find ( "powerBar" ).renderer.enabled = false;
				}
				else
				{
					iTween.Stop ( _buttonBack );
					_buttonBack.transform.localScale = VectorTools.cloneVector3 ( _initialScaleButtonBack );
					_noPowerNotification = false;
					transform.Find ( "powerBar" ).renderer.enabled = true;
				}

				_myPowerBarMaterial.mainTexture = MNUIControl.getInstance ().powerBarTextures[(int) ((( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] * 100 ) / initialPower ) / 25 )];
			}
			/*else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
			{
				_myPowerBarMaterial.mainTexture = TRUIControl.getInstance ().powerBarTextures[(int) ((( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] * 100 ) / initialPower ) / 10 )];
			}*/
		}
		else if(GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN)
		{
			if ( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] == 0 )
			{
				if ( ! _noPowerNotification )
				{
					_noPowerNotification = true;
					
					iTween.ScaleFrom ( _buttonBack, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _initialScaleButtonBack * 1.4f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteNotification" ));
				}
				
				transform.Find ( "powerBar" ).renderer.enabled = false;
			}
			else
			{
				iTween.Stop ( _buttonBack );
				_buttonBack.transform.localScale = VectorTools.cloneVector3 ( _initialScaleButtonBack );
				_noPowerNotification = false;
				transform.Find ( "powerBar" ).renderer.enabled = true;
			}
			
			_myPowerBarMaterial.mainTexture = TRUIControl.getInstance ().powerBarTextures[(int) ((( _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] * 100 ) / initialPower ) / 25 )];
		}
	}

	private void onCompleteNotification ()
	{
		iTween.ScaleFrom ( _buttonBack, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _buttonBack.transform.localScale * 1.4f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteNotification" ));
	}
	
	public void highlightEnergy ()
	{
		if ( _myText01 != null ) iTween.ScaleFrom ( _myText01.gameObject, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _myText01.transform.localScale * 2f ));
	}
	
	public void setCharacterData ( CharacterData characterData )
	{
		_myCharacterData = characterData;
		initialPower = _myCharacterData.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER];
	}
	
	void OnMouseUp ()
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GlobalVariables.checkForMenus ()) return;
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && MNGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{	
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myCharacterData.position[0]][_myCharacterData.position[1]].transform.Find ( "tile" ).SendMessage ( "handleTouched", SendMessageOptions.DontRequireReceiver );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			MNLevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myCharacterData.position[0]][_myCharacterData.position[1]].transform.Find ( "tile" ).SendMessage ( "handleTouched", SendMessageOptions.DontRequireReceiver );
		}
	}
}
