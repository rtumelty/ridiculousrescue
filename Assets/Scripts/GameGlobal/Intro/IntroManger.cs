using UnityEngine;
using System.Collections;

public class IntroManger : MonoBehaviour 
{
	//*************************************************************//
	public GameObject space;
	public GameObject signSpace;
	public GameObject signSpaceTap;
	public GameObject blackHole;
	public GameObject map;
	public GameObject lab;
	public GameObject missionRoom;
	public GameObject signMap;
	public GameObject signLab;
	public GameObject signFaradaydo01;
	public GameObject signFaradaydo02;
	public GameObject signCora01;
	public GameObject signCora02;

	public GameObject faradaydo;
	public GameObject madra;
	public GameObject cora;

	public GameObject mom;
	public GameObject battery;
	public Texture2D momReady;
	public GameObject flap;
	public GameObject buttonSkip;

	public AudioClip sound01;
	public AudioClip sound02;
	public AudioClip sound03FaradaydoConstructing;
	public AudioClip sound04MadraEntering;
	public AudioClip sound05MadraPeting;
	public AudioClip sound06MadraExiting;
	public AudioClip sound07CoraEntering;
	public AudioClip sound08CoraExiting;
	public AudioClip sound09FaradaydoExiting;
	//public AudioClip sound10CoraHmm;
	//public AudioClip sound11FaraHMM;

	public AudioSource audioSource;
	//*************************************************************//
	private bool _checkForTap01;
	private bool _checkForTap02;
	private bool _checkForTap03;
	private bool _checkForTap04;
	private bool _checkForTap05;
	private bool _checkForTap06;
	private bool section5 = false;
	private bool section6 = false;
	private bool loop1Stopped = false;
	private bool loop2Stopped = false;

	private Vector3 _labPosition;
	private Vector3 _missionRoomPosition;
	//*************************************************************//
	private static IntroManger _meInstance;
	public static IntroManger getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		_meInstance = this;
		GameGlobalVariables.CURRENT_GAME_PART = GameGlobalVariables.INTRO;
	}

	IEnumerator Start () 
	{
		GameGlobalVariables.INTRO_PLAYED = SaveDataManager.getValue ( SaveDataManager.INTRO_PLAYED_PREFIX );
		GameGlobalVariables.INTRO_PLAYED++;
		SaveDataManager.save ( SaveDataManager.INTRO_PLAYED_PREFIX, GameGlobalVariables.INTRO_PLAYED );

		//if ( GameGlobalVariables.INTRO_PLAYED > 1 )
		//{
			buttonSkip.SetActive ( true );
		//}

		if ( GameGlobalVariables.INTRO_PLAYED == 1 )
		{
			FLMissionRoomManager.AFTER_INTRO = true;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
		}
		else
		{
			FLMissionRoomManager.AFTER_INTRO = false;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
		}

		_missionRoomPosition = new Vector3 ( 0.8769306f, 177.2187f, -16.95727f );
		_labPosition = new Vector3 ( 6.2f, 176.8814f, -17.68f ); 
		map.SetActive ( false );
		signMap.SetActive ( false );
		signLab.SetActive ( false );
		signFaradaydo01.SetActive ( false );
		signFaradaydo02.SetActive ( false );
		signCora01.SetActive ( false );
		signCora02.SetActive ( false );
		battery.SetActive ( false );

		audioSource.clip = sound01;
		audioSource.loop = false;
		audioSource.Play ();

		space.animation["intro_space"].speed = 1f;
		space.animation.CrossFade ( "intro_space", 1f );

		yield return new WaitForSeconds ( 3.5f );
		signSpace.SetActive ( true );
		signSpaceTap.SetActive ( false );
		iTween.MoveFrom ( signSpace, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "position", signSpace.transform.position + Vector3.back * 0.3f ));
		yield return new WaitForSeconds ( 2.5f );
		signSpaceTap.SetActive ( true );
		_checkForTap01 = true;
	}

	private IEnumerator part2 ()
	{
		audioSource.clip = sound02;
		audioSource.loop = true;
		audioSource.Play ();

		signSpace.SetActive ( false );
		iTween.ScaleTo ( blackHole, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 0.14f, 0.005f, 0.14f )));
		yield return new WaitForSeconds ( 1f );
		iTween.ScaleTo ( blackHole, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 31f, 0.005f, 31f )));
		blackHole.transform.position = new Vector3 ( -2.4f, 183f, -19f );
		map.SetActive ( true );
		space.SetActive ( false );
		yield return new WaitForSeconds ( 1f );
		signMap.SetActive ( true );
		yield return new WaitForSeconds ( 1f );
		signLab.SetActive ( true );
		iTween.ScaleFrom ( signLab, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutCubic, "scale", signLab.transform.localScale * 0.2f ));
		_checkForTap02 = true;
	}

	private IEnumerator part3 ()
	{
		audioSource.PlayOneShot ( sound03FaradaydoConstructing );
		
		signMap.SetActive ( false );
		signLab.SetActive ( false );
		iTween.ScaleTo ( map, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 4.11f, 2.87f, 3.26f )));
		iTween.MoveTo ( map, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.linear, "position", new Vector3 ( 12.27f, 177.7f, -11.05f )));
		yield return new WaitForSeconds ( 1f );
		map.SetActive ( false );
		lab.transform.position = VectorTools.cloneVector3 ( _labPosition );
		iTween.ScaleFrom ( blackHole, iTween.Hash ( "time", 4.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 0.14f, 0.005f, 0.14f )));
		yield return new WaitForSeconds ( 0.2f );
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_01_ANIMATION, 3f, -1 );		                                                                                           
		yield return new WaitForSeconds ( 3f );
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.LEFT );
		yield return new WaitForSeconds ( 0.2f );
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().unblockForIntro ();
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_02_ANIMATION );
		yield return new WaitForSeconds ( 0.4f );
		flap.SetActive ( false );
		signFaradaydo01.SetActive ( true );
		iTween.ScaleFrom ( signFaradaydo01, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", signFaradaydo01.transform.localScale * 0.2f ));

		Destroy ( mom.GetComponent < StaticObjectWithAnimation > ());
		mom.renderer.material.mainTexture = momReady;
		battery.SetActive ( true );
		battery.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForBouncing ( true );
		_checkForTap03 = true;
	}

	private IEnumerator part4 ()
	{
		signFaradaydo01.SetActive ( false );
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.RIGHT );
		madra.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_LEFT );
		iTween.MoveTo ( madra, iTween.Hash ( "time", 2.5f, "easetype", iTween.EaseType.linear, "position", new Vector3 ( 0.1354256f, 2.449688f, 0.1370018f ), "islocal", true ));
		yield return new WaitForSeconds ( .5f );
		audioSource.PlayOneShot ( sound05MadraPeting );
		yield return new WaitForSeconds ( .65f );
		audioSource.PlayOneShot ( sound04MadraEntering );
		yield return new WaitForSeconds ( 1.35f );
		madra.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_02_ANIMATION );
		signFaradaydo02.SetActive ( true );
		//audioSource.PlayOneShot (sound11FaraHMM);
		_checkForTap04 = true;
		yield return new WaitForSeconds ( 0.9f );
		StartCoroutine ("pantLoop1");
	}

	private IEnumerator pantLoop1 ()
	{
		if(section5 == false)
		{
			audioSource.PlayOneShot ( sound05MadraPeting );
			yield return new WaitForSeconds ( 2.351f );
			if(loop1Stopped == false)
			{
				StartCoroutine ("pantLoop1");
			}
		}
	}
	private IEnumerator pantLoop2 ()
	{
		if(section6 == false && section5 == true)
		{
			audioSource.PlayOneShot ( sound05MadraPeting );
			yield return new WaitForSeconds ( 2.351f );
			if(loop2Stopped == false)
			{
				StartCoroutine ("pantLoop2");
			}
		}
	}

	private IEnumerator part5 ()
	{
		section5 = true;
		StopCoroutine ("pantLoop1");

		SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT, GameElements.CHAR_FARADAYDO_1_IDLE );

		//audioSource.PlayOneShot (sound11FaraHMM);
		signFaradaydo02.SetActive ( false );
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().unblockForIntro ();
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_03_ANIMATION );
		yield return new WaitForSeconds ( 1.3f );
		audioSource.PlayOneShot ( sound09FaradaydoExiting );
		faradaydo.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		iTween.MoveTo ( faradaydo, iTween.Hash ( "time", 2.5f, "easetype", iTween.EaseType.linear, "position", new Vector3 ( -0.1937376f, 0.8438273f, 0.02915447f ), "islocal", true ));
		yield return new WaitForSeconds ( 1.5f );
		madra.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		loop2Stopped = true;
		StartCoroutine ("pantLoop2");
		yield return new WaitForSeconds ( 2.35f );

		madra.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_02_ANIMATION );
		yield return new WaitForSeconds ( 0.4f );
		StopCoroutine("pantLoop2");
		//loop2Stopped = true;
		yield return new WaitForSeconds ( 0.5f );
		StartCoroutine ("pantLoop2");
		audioSource.PlayOneShot ( sound06MadraExiting );
		madra.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		iTween.MoveTo ( madra, iTween.Hash ( "time", 2.5f, "easetype", iTween.EaseType.linear, "position", new Vector3 ( -0.2783511f, 0.2846887f, 0.004077836f ), "islocal", true ));
		yield return new WaitForSeconds ( 2.5f );
		lab.SetActive ( false );
		battery.SetActive ( false );
		missionRoom.transform.position = VectorTools.cloneVector3 ( _missionRoomPosition );
		iTween.ScaleFrom ( blackHole, iTween.Hash ( "time", 4.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 0.14f, 0.005f, 0.14f )));
		yield return new WaitForSeconds ( 0.5f );
		audioSource.PlayOneShot ( sound07CoraEntering );
		cora.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		iTween.MoveTo ( cora, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.linear, "position", new Vector3 ( -0.1174448f, 2.753193f, 0.34f ), "islocal", true ));
		yield return new WaitForSeconds ( 2f );
		signCora01.SetActive ( true );
		cora.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_02_ANIMATION );
		//audioSource.PlayOneShot (sound10CoraHmm);

		SoundManager.getInstance ().playSound ( SoundManager.OBJECT_SELECT, GameElements.CHAR_CORA_1_IDLE );

		_checkForTap05 = true;
	}

	private IEnumerator part6 ()
	{
		audioSource.PlayOneShot ( sound08CoraExiting );
		signCora01.SetActive ( false );
		cora.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		iTween.MoveTo ( cora, iTween.Hash ( "time", 3f, "easetype", iTween.EaseType.linear, "position", new Vector3 ( -0.890833f, 2.753193f, 0.3893746f ), "islocal", true ));
		yield return new WaitForSeconds ( 2f );
		signCora02.SetActive ( true );
		_checkForTap06 = true;
	}

	private IEnumerator part7 ()
	{
		signCora02.SetActive ( false );
		yield return new WaitForSeconds ( 0.5f );
		iTween.ScaleTo ( missionRoom, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 8.784201f, 0.7320165f, 5.856132f )));
		iTween.MoveTo ( missionRoom, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutCubic, "scale", new Vector3 ( 1.230051f, 177.2187f, -17.60466f )));
		yield return new WaitForSeconds ( 1f );

		if ( GameGlobalVariables.INTRO_PLAYED == 1 )
		{
			FLMissionRoomManager.AFTER_INTRO = true;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			Application.LoadLevel ( "FL00ChooseLevel" );
		}
		else
		{
			FLMissionRoomManager.AFTER_INTRO = false;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			Application.LoadLevel ( "00ChooseLevel" );
		}

	}

	public void skip ()
	{
		if ( GameGlobalVariables.INTRO_PLAYED == 1 )
		{
			FLMissionRoomManager.AFTER_INTRO = true;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			Application.LoadLevel ( "FL00ChooseLevel" );
		}
		else
		{
			FLMissionRoomManager.AFTER_INTRO = false;
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			Application.LoadLevel ( "00ChooseLevel" );
		}
	}
	
	void Update () 
	{
		if ( _checkForTap01 )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
#else
			if ( Input.touchCount > 0 )
#endif
			{
				_checkForTap01 = false;
				StartCoroutine ( "part2" );
			}
		}

		if ( _checkForTap02 )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
#else
			if ( Input.touchCount > 0 )
#endif
			{
				_checkForTap02 = false;
				StartCoroutine ( "part3" );
			}
		}

		if ( _checkForTap03 )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
#else
			if ( Input.touchCount > 0 )
#endif
			{
				_checkForTap03 = false;
				StartCoroutine ( "part4" );
			}
		}

		if ( _checkForTap04 )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
#else
			if ( Input.touchCount > 0 )
#endif
			{
				_checkForTap04 = false;
				StartCoroutine ( "part5" );
			}
		}

		if ( _checkForTap05 )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
#else
			if ( Input.touchCount > 0 )
#endif
			{
				_checkForTap05 = false;
				StartCoroutine ( "part6" );
			}
		}

		if ( _checkForTap06 )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
#else
			if ( Input.touchCount > 0 )
#endif
			{
				_checkForTap06 = false;
				StartCoroutine ( "part7" );
			}
		}
	}
}
