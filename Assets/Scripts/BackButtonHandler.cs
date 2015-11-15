using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class BackButtonHandler : MonoBehaviour {

	static BackButtonHandler instance = null;
	public static BackButtonHandler Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject();
				instance = go.AddComponent<BackButtonHandler>();
			} 

			return instance;
		}
	}

	public enum GameState {
		Undefined,
		LevelScreen,
		Menu,
		Settings
	}

	GameObject _menuPrefab;
	GameObject _settingsPrefab;

	public GameState gameState = GameState.Undefined;

	public GameObject menuObject;
	public GameObject settingsObject;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);

		_menuPrefab = ( GameObject ) Resources.Load ( "UI/menuUIScreen" );
		_settingsPrefab = ( GameObject ) Resources.Load ( "UI/settingsUIScreen" );
	}
	
	// Update is called once per frame
	void Update () {
		if (Chartboost.isImpressionVisible ()) {
			return;
		
		}

		else if (Input.GetKeyUp (KeyCode.Escape)) {
			GameState previousState;

			switch (gameState) {
			case GameState.LevelScreen:
				previousState = gameState;

				gameState = GameState.Menu;

				
				if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
				{
					print ("FFS2");
					FLUIControl.getInstance ().unselectCurrentGameElement ();
					FLUIControl.getInstance ().destoryCurrentUIElement ();
				}
				
				SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
				
				if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
				{
					if ( GlobalVariables.MENU_FOR_TIP ) TipOnClickComponent.CURRENT_TIP.deActivate ();
					
					menuObject = UIControl.getInstance ().createPopup ( _menuPrefab );
					/*
					if ( GlobalVariables.TUTORIAL_MENU )
					{
						if ( menuScreen != null )
						{
							menuScreen.transform.Find ( "returnToLabButton" ).gameObject.SetActive ( false );
							menuScreen.transform.Find ( "settingsButton" ).position += Vector3.forward * 0.8f;
							menuScreen.transform.Find ( "exitButton" ).position += Vector3.forward * 0.4f;
						}
					}
					*/
				}
				else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
				{
					menuObject = FLUIControl.getInstance ().createPopup ( _menuPrefab );
					if ( menuObject != null )
					{
						menuObject.transform.Find ( "returnToLabButton" ).gameObject.SetActive ( false );
						menuObject.transform.Find ( "settingsButton" ).position += Vector3.forward * 0.8f;
						menuObject.transform.Find ( "exitButton" ).position += Vector3.forward * 0.4f;
					}
				}
				else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
				{
					MNUIControl.getInstance ().createPopup ( _menuPrefab );			
				}
				else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
				{
					TRUIControl.getInstance ().createPopup ( _menuPrefab );			
				}
				break;
			case GameState.Menu:
				if (menuObject != null) { 
					
					Debug.Log(menuObject);
					ScreenCloseButton closeMenu = menuObject.GetComponentInChildren<ScreenCloseButton>() as ScreenCloseButton;
					closeMenu.Close();
				}
				gameState = GameState.LevelScreen;
				break;
			case GameState.Settings:
				if (settingsObject != null) {
					Debug.Log(settingsObject);
					ScreenCloseButton closeSettings = settingsObject.GetComponentInChildren<ScreenCloseButton>() as ScreenCloseButton;
					closeSettings.Close();
				}
				
				gameState = GameState.LevelScreen;
				break;
			default:
				break;
			}
		}
	}

	void OnLevelWasLoaded(int levelLoaded) {
		switch (levelLoaded) {
		case 2:
		case 4:
		case 3:
		case 5:
		case 6:
		case 7:
			gameState = GameState.LevelScreen;
			break;
		default:
			gameState = GameState.Undefined;
			break;
		}
	}
}
