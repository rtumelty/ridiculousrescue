using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNUIControl : MonoBehaviour 
{
	//*************************************************************//
	public static GameObject currentPopupUI;
	public static GameObject currentBlackOutUI;
	public static GameObject currentAttackBarUI;
	//*************************************************************//
	public Texture2D textureYesUp;
	public Texture2D textureYesDown;
	public Texture2D textureYesGreyedOutUp;
	public Texture2D textureYesGreyedOutDown;
	public Texture2D textureNoUp;
	public Texture2D textureNoDown;
	public Texture2D textureUIButtonSelectedUp;
	public Texture2D textureUIButtonSelectedDown;
	public Texture2D textureUIButtonNotSelectedUp;
	public Texture2D textureUIButtonNotSelectedDown;
	public Texture2D textureUIClockTimePassed;
	public Texture2D textureIconNext;
	public Texture2D textureIconForward;
	public Texture2D attackGreenBoxDown;
	public Texture2D attackGreenBoxUp;
	public Texture2D[] powerBarTextures;
	public Material transparentMaterial;
	//*************************************************************//	
	public delegate bool ButtonCallBack ();
	public delegate bool ButtonCharacterCallBack ( CharacterData character );
	//*************************************************************//	
	private GameObject _buttonMenuPrefab;
	private List < GameObject > _buttonsCharacters;
	private float _countTimeOutConnection;
	private GameObject _characterBoxComboUIPrefab;
	private GameObject _blackOutUIPrefab;
	private bool initialZoomOutDone = false;
	//*************************************************************//	
	private static MNUIControl _meInstance;
	public static MNUIControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "Camera" ).transform.Find ( "UI" ).GetComponent < MNUIControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	void Awake () 
	{
		_blackOutUIPrefab = ( GameObject ) Resources.Load ( "UI/PopupBlackOutBackUI" );
		_characterBoxComboUIPrefab = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar01");
		_countTimeOutConnection = 15f;
		_buttonsCharacters = new List < GameObject > ();
		_buttonMenuPrefab = ( GameObject ) Resources.Load ( "UI/MNButtonPrefab" );
	}

	void Update ()
	{
		if ( MNGlobalVariables.TUTORIAL_MENU )
		{
			if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().maximumCameraZoom )
			{
				Camera.main.orthographicSize = Mathf.Lerp ( Camera.main.orthographicSize, MNZoomAndLevelDrag.MAXIMUM_CAMERA_ZOOM, 0.12f );
				Camera.main.transform.position = Vector3.Lerp ( Camera.main.transform.position, new Vector3 ( 5.5f, Camera.main.transform.position.y, 4f ), 0.12f );
			}
		}
		else if(GameGlobalVariables.MINE_ENTERED > 0 && initialZoomOutDone == false)
		{
			//print ("shrinky?");
			Camera.main.orthographicSize = Mathf.Lerp ( Camera.main.orthographicSize, MNZoomAndLevelDrag.MAXIMUM_CAMERA_ZOOM, 0.12f );
			if(Camera.main.orthographicSize > 5.99)
			{
				initialZoomOutDone = true;
			}
		}
	}
	
	public GameObject createPopup ( GameObject popupPrefab )
	{
		if ( MNUIControl.currentPopupUI != null ) return null;
		
		MNGlobalVariables.POPUP_UI_SCREEN = true;
		blockClicksForAMomentAfterUIClicked ();
		
		GameObject popup = ( GameObject ) Instantiate ( popupPrefab, Camera.main.transform.position + Vector3.down * 4f + Vector3.forward * 12f, popupPrefab.transform.rotation );
		popup.transform.parent = Camera.main.transform;
		popup.AddComponent < PopupDelayer > ();
		MNUIControl.currentPopupUI = popup;
		
		GameObject blackOutScreen = ( GameObject ) Instantiate ( _blackOutUIPrefab, Camera.main.transform.position + Vector3.down * 5.5f, _blackOutUIPrefab.transform.rotation );
		blackOutScreen.transform.parent = Camera.main.transform;
		MNUIControl.currentBlackOutUI = blackOutScreen;
		
		return popup;
	}
	
	public void createButtonsCharacters ( List < CharacterData > charactersData )
	{
		for ( int i = 0; i < charactersData.Count; i++ )
		{
			if ( charactersData.Count == 3 )
			{
				_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 2.1f + Vector3.up + Vector3.left * 0.25f + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			}
			else if ( charactersData.Count == 2 )
			{
				_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 2.1f + Vector3.left * 2.65f + Vector3.up + Vector3.left * 0.25f + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			}
			else if ( charactersData.Count == 1 )
			{
				_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 2.1f + Vector3.left * 4.15f + Vector3.up + Vector3.left * 0.25f + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			}

			CharacterButton currentCharacterButton = _buttonsCharacters[_buttonsCharacters.Count - 1].AddComponent < CharacterButton > ();
			
			switch ( charactersData[i].myID )
			{
				case GameElements.CHAR_CORA_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[GameElements.UI_CORA_ICON];
					break;
				case GameElements.CHAR_MADRA_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[GameElements.UI_MADRA_ICON];
					break;
				case GameElements.CHAR_FARADAYDO_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[GameElements.UI_FARADAYDO_ICON];
					break;
				case GameElements.CHAR_BOZ_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[GameElements.UI_BOZ_ICON];
					break;
			}
			
			currentCharacterButton.setCharacterData ( charactersData[i] );
			charactersData[i].myCharacterButton = currentCharacterButton;
			_buttonsCharacters[_buttonsCharacters.Count - 1].transform.parent = this.transform;
		}
	}
	
	public void LateUpdate () 
	{
		if ( MNGlobalVariables.START_SEQUENCE || MNGlobalVariables.TOY_LAZARUS_SEQUENCE ) return;
		
		float uiSizeFactor = Camera.main.orthographicSize / 4.5f;
		transform.localScale = new Vector3 ( 24f * uiSizeFactor, 1f, 1f * uiSizeFactor );
		transform.localPosition = new Vector3 ( 0f, Camera.main.orthographicSize - 0.5f * uiSizeFactor, 6f );
	}
	
	public void blockClicksForAMomentAfterUIClicked ()
	{
		MNGlobalVariables.UI_CLICKED = true;
		StopCoroutine ( "unblockAfterDelay" );
		StartCoroutine ( "unblockAfterDelay" );
	}
	
	private IEnumerator unblockAfterDelay ()
	{
		yield return new WaitForSeconds ( 0.25f );
		MNGlobalVariables.UI_CLICKED = false;	
	}
	
	void OnGUI ()
	{
		if ( GameGlobalVariables.RELEASE ) return;
		if ( MNGlobalVariables.LOADING_SAVING_MENU )
		{
			_countTimeOutConnection -= Time.deltaTime;
			
			if ( _countTimeOutConnection <= 0f )
			{
				MNLevelControl.getInstance ().loadLevelLocalToThisGrid ( 1, MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL] );
				MNGlobalVariables.LOADING_SAVING_MENU = false;
				return;
			}
			
			GUI.TextArea ( new Rect ( 10f, 70f, Screen.width - 20f, Screen.height - 20f ), "Loading Levels From Online Database... Please Wait" );
		}
	}
	
	public void createCharacterBox ( int characterID )
	{
		MNGlobalVariables.CHARACTER_BOX = true;
		GameObject tutorialUIComboObject = ( GameObject ) Instantiate ( _characterBoxComboUIPrefab, Vector3.zero, _characterBoxComboUIPrefab.transform.rotation );
		tutorialUIComboObject.transform.parent = Camera.main.transform;
		tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, -2.5f, 2f );
		iTween.ScaleFrom ( tutorialUIComboObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo, "scale", Vector3.zero, "islocal", true ));
		
		tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "character_out_of_moves";
		if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
		{
			tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = MNLevelControl.getInstance ().gameElements[characterID];
			if ( characterID == GameElements.CHAR_CORA_1_IDLE )
			{
				tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
				tutorialUIComboObject.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
			}
		}
		
		tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < FrameTapControl > ();
		tutorialUIComboObject.transform.Find ( "tapText" ).GetComponent < TextMesh > ().text = "Tap Here!";
	}
}
