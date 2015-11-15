using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TRUIControl : MonoBehaviour 
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
	private float uiSizeFactor = 0;
	private List < GameObject > _buttonsCharacters;
	private float _countTimeOutConnection;
	private GameObject _characterBoxComboUIPrefab;
	private GameObject _blackOutUIPrefab;
	//=====================================Daves Edit====================================
	private GameObject _timerPanel;	
	//=====================================Daves Edit====================================
	//*************************************************************//	
	private static TRUIControl _meInstance;
	public static TRUIControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "Camera" ).transform.Find ( "UI" ).GetComponent < TRUIControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	void Awake () 
	{
		_timerPanel = transform.Find ( "TimerPanel" ).gameObject;

		_blackOutUIPrefab = ( GameObject ) Resources.Load ( "UI/PopupBlackOutBackUI" );
		_characterBoxComboUIPrefab = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar01");
		_countTimeOutConnection = 15f;
		_buttonsCharacters = new List < GameObject > ();
		_buttonMenuPrefab = (GameObject)Resources.Load ("UI/MNButtonPrefab");
	}

	public GameObject createPopup ( GameObject popupPrefab )
	{
		if ( TRUIControl.currentPopupUI != null ) return null;
		
		TRGlobalVariables.POPUP_UI_SCREEN = true;
		blockClicksForAMomentAfterUIClicked ();
		
		GameObject popup = ( GameObject ) Instantiate ( popupPrefab, Camera.main.transform.position + Vector3.down * 4f + Vector3.forward * 12f, popupPrefab.transform.rotation );
		popup.transform.parent = Camera.main.transform;
		popup.AddComponent < PopupDelayer > ();
		TRUIControl.currentPopupUI = popup;
		
		GameObject blackOutScreen = ( GameObject ) Instantiate ( _blackOutUIPrefab, Camera.main.transform.position + Vector3.down * 5.5f, _blackOutUIPrefab.transform.rotation );
		blackOutScreen.transform.parent = Camera.main.transform;
		TRUIControl.currentBlackOutUI = blackOutScreen;
		
		return popup;
	}
	//=====================================Daves Edit====================================
	public void manageTimer ( bool isOn )
	{
		if ( isOn )
		{
			_timerPanel.transform.localPosition = new Vector3 ( 0.083f, _timerPanel.transform.localPosition.y, -0.04f );
		}
		else
		{
			_timerPanel.transform.localPosition = new Vector3 ( 0.18f, _timerPanel.transform.localPosition.y, 2f );
		}
	}
	//=====================================Daves Edit====================================
	public void createButtonsCharacters ( List < CharacterData > charactersData )
	{
		for ( int i = 0; i < charactersData.Count; i++ )
		{
																																				//=====================================Daves Edit====================================
			_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 1.94f + Vector3.up + Vector3.left * ( TRLevelControl.LEVEL_ID > 1 ? 1.00f : 2.00f ) + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			CharacterButton currentCharacterButton = _buttonsCharacters[_buttonsCharacters.Count - 1].AddComponent < CharacterButton > ();
			
			switch ( charactersData[i].myID )
			{
				case GameElements.CHAR_CORA_1_IDLE:
					TREventsManager.getInstance ().coraCharacterData = charactersData[i];
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = TRLevelControl.getInstance ().gameElements[GameElements.UI_CORA_ICON];
					break;
				case GameElements.CHAR_MADRA_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = TRLevelControl.getInstance ().gameElements[GameElements.UI_MADRA_ICON];
					break;
				case GameElements.CHAR_FARADAYDO_1_IDLE:
					TREventsManager.getInstance ().faradyadoCharacterData = charactersData[i];
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = TRLevelControl.getInstance ().gameElements[GameElements.UI_FARADAYDO_ICON];
					break;
				case GameElements.CHAR_JOSE_1_IDLE:
					TREventsManager.getInstance ().joseCharacterData = charactersData[i];
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = TRLevelControl.getInstance ().gameElements[GameElements.UI_JOSE_ICON];
					break;
				case GameElements.CHAR_BOZ_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = TRLevelControl.getInstance ().gameElements[GameElements.UI_BOZ_ICON];
					break;
			}
			
			currentCharacterButton.setCharacterData ( charactersData[i] );
			charactersData[i].myCharacterButton = currentCharacterButton;
			_buttonsCharacters[_buttonsCharacters.Count - 1].transform.parent = this.transform;
		}

		TREventsManager.getInstance ().start = true;
	}
	
	public void LateUpdate () 
	{
		if ( TRGlobalVariables.START_SEQUENCE || TRGlobalVariables.TOY_LAZARUS_SEQUENCE ) return;

		if (Camera.main != null) 
		{
			uiSizeFactor = Camera.main.orthographicSize / 4.5f;
			transform.localScale = new Vector3 ( 24f * uiSizeFactor, 1f, 1f * uiSizeFactor );
			transform.localPosition = new Vector3 ( 0f, Camera.main.orthographicSize - 0.5f * uiSizeFactor, 6f );
		}
		else
		{
			uiSizeFactor = GameObject.Find("camHolderStatic").GetComponent< Camera > ().orthographicSize / 4.5f;
			transform.localScale = new Vector3 ( 24f * uiSizeFactor, 1f, 1f * uiSizeFactor );
			transform.localPosition = new Vector3 ( 0f, GameObject.Find("camHolderStatic").GetComponent< Camera > ().orthographicSize - 0.5f * uiSizeFactor, 6f );
		}
	}
	
	public void blockClicksForAMomentAfterUIClicked ()
	{
		TRGlobalVariables.UI_CLICKED = true;
		StopCoroutine ( "unblockAfterDelay" );
		StartCoroutine ( "unblockAfterDelay" );
	}
	
	private IEnumerator unblockAfterDelay ()
	{
		yield return new WaitForSeconds ( 0.25f );
		TRGlobalVariables.UI_CLICKED = false;	
	}
	
	void OnGUI ()
	{
		if ( GameGlobalVariables.RELEASE ) return;
		if ( TRGlobalVariables.LOADING_SAVING_MENU )
		{
			_countTimeOutConnection -= Time.deltaTime;
			
			if ( _countTimeOutConnection <= 0f )
			{
				TRLevelControl.getInstance ().loadLevelLocalToThisGrid ( 1, TRLevelControl.getInstance ().levelGrid[TRLevelControl.GRID_LAYER_NORMAL] );
				TRGlobalVariables.LOADING_SAVING_MENU = false;
				return;
			}
			
			GUI.TextArea ( new Rect ( 10f, 70f, Screen.width - 20f, Screen.height - 20f ), "Loading Levels From Online Database... Please Wait" );
		}
	}
	
	public void createCharacterBox ( int characterID )
	{
		TRGlobalVariables.CHARACTER_BOX = true;
		GameObject tutorialUIComboObject = ( GameObject ) Instantiate ( _characterBoxComboUIPrefab, Vector3.zero, _characterBoxComboUIPrefab.transform.rotation );
		tutorialUIComboObject.transform.parent = Camera.main.transform;
		tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, -2.5f, 2f );
		iTween.ScaleFrom ( tutorialUIComboObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo, "scale", Vector3.zero, "islocal", true ));
		
		tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "character_out_of_moves";
		if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
		{
			tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = TRLevelControl.getInstance ().gameElements[characterID];
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
