using UnityEngine;
using System.Collections;

public class LoadingScreenControl : MonoBehaviour 
{
	//*************************************************************//
	public static string CURRENT_TIP_KEY = "loading_tip_rescue_001";
	public static Texture2D CURRENT_LOADING_SCREEN_TEXTURE;
	//*************************************************************//
	private bool isTrainlvl = false;
	private GameTextControl _text;
	private Material _loadingScreenMaterial;
	private Texture2D _loadingScreenTexture01;
	private Texture2D _loadingScreenTexture02;
	//==============================Daves Edit=================================
	private Texture2D _loadingScreenTexture03;
	//==============================Daves Edit=================================
	//*************************************************************//
	private static LoadingScreenControl _meInstance;
	public static LoadingScreenControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraLoading" ).GetComponent < LoadingScreenControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	void Awake ()
	{
		_loadingScreenTexture01 = ( Texture2D ) Resources.Load ( "Textures/UI_v3/Loading/ui_loadingscreen_01" );
		_loadingScreenTexture02 = ( Texture2D ) Resources.Load ( "Textures/UI_v3/Loading/ui_loadingscreen_02" );
		//==============================Daves Edit=================================
		_loadingScreenTexture03 = ( Texture2D ) Resources.Load ( "Textures/UI_v3/Loading/ui_loadingscreen_03" );
		//==============================Daves Edit=================================
		
		_text = transform.Find ( "text" ).GetComponent < GameTextControl > ();
		_loadingScreenMaterial = transform.Find ( "loadingScreen" ).renderer.material;

		_text.myKey = CURRENT_TIP_KEY;
		_text.lineLength = 40;
		
		_loadingScreenMaterial.mainTexture = CURRENT_LOADING_SCREEN_TEXTURE;
	}
	
	public void turnOnLoadingScreen ()
	{
		camera.depth = 150;
		if(Camera.main != null)
		{
			Camera.main.depth = 0;
		}
		else
		{
			TRCameraMovement.getInstance ().resetCam ();
			Camera.main.depth = 0;
		}
		Debug.Log("Show preloader");
		AndroidNativeUtility.ShowPreloader("Loading...", "Please wait");
	}
	
	public void changeTipForGamePart ( int gamePart )
	{
		switch ( gamePart )
		{
			//==============================Daves Edit=================================
			case GameGlobalVariables.TRAIN:
				CURRENT_TIP_KEY = "loading_tip_train_00" + UnityEngine.Random.Range ( 1, 4 ).ToString ();
				CURRENT_LOADING_SCREEN_TEXTURE = _loadingScreenTexture03;
				isTrainlvl = true;
				break;
			//==============================Daves Edit=================================
			case GameGlobalVariables.LABORATORY:
			case GameGlobalVariables.RESCUE:
				CURRENT_TIP_KEY = "loading_tip_rescue_00" + UnityEngine.Random.Range ( 1, 4 ).ToString ();
				CURRENT_LOADING_SCREEN_TEXTURE = _loadingScreenTexture02;
				break;
			case GameGlobalVariables.MINING:
				CURRENT_TIP_KEY = "loading_tip_mining_00" + UnityEngine.Random.Range ( 1, 4 ).ToString ();
				CURRENT_LOADING_SCREEN_TEXTURE = _loadingScreenTexture02;
				break;
			/*
			case GameGlobalVariables.LABORATORY:
				if ( FLMissionRoomManager.AFTER_INTRO )
				{
					CURRENT_TIP_KEY = "loading_tip_after_intro00" + UnityEngine.Random.Range ( 1, 1 ).ToString ();
					CURRENT_LOADING_SCREEN_TEXTURE = _loadingScreenTexture01;
				}
				else
				{
					CURRENT_TIP_KEY = "loading_tip_laboratory_00" + UnityEngine.Random.Range ( 1, 4 ).ToString ();
					CURRENT_LOADING_SCREEN_TEXTURE = _loadingScreenTexture01;
				}
			break;
			*/
		}
										//==============================Daves Edit=================================
		if ( ! FLMissionRoomManager.AFTER_INTRO /* here --> */ && isTrainlvl == false /* <-- here  */)
		{
			if ( Random.Range ( 0, 100 ) > 70 ) CURRENT_TIP_KEY = "loading_tip_general_00" + UnityEngine.Random.Range ( 1, 1 ).ToString ();
			isTrainlvl = false;
		}

		_text.myKey = CURRENT_TIP_KEY;

		_text.lineLength = 40;
		_loadingScreenMaterial.mainTexture = CURRENT_LOADING_SCREEN_TEXTURE;
	}
}
