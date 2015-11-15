using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TRSuccessScreenControl : MonoBehaviour 
{
	//*************************************************************//	
	public enum StarsValue
	{
		DONT_COUNT,
		NON,
		HALF,
		FULL
	}
	//*************************************************************//	
	public const int LIMIT_PLASTIC_FOR_TICK = 4;
	//*************************************************************//	
	public Texture2D star01Full;
	public Texture2D star01Half;
	public Texture2D star02Full;
	public Texture2D star02Half;
	public Texture2D star03Full;
	public Texture2D star03Half;
	
	public Texture2D tickPositive;
	public Texture2D tickNegative;
	//*************************************************************//	
	/*private GameObject faraPlatform;
	private GameObject coraPlatform;
	private GameObject josePlatform;
	private GameObject madraPlatform;
	private GameObject bozPlatform;
	private GameObject minerPlatform;
	private GameObject motorPlatform;
	private int platformCharID = 0;
	private Renderer[] _charactersIcon;*/
	//*************************************************************//	
	private GameTextControl _text1;
	private GameTextControl _text2;
	private GameTextControl _text3;
	private TextMesh _timeText;
	/*private TextMesh _textUp01;
	private TextMesh _textUp02;
	private TextMesh _textUp03;
	private TextMesh _textLow01;
	private TextMesh _textLow02;
	private TextMesh _textLow03;

	public static string UPPER_TEXT_01 = "train_stars_power";
	public static string UPPER_TEXT_02 = "train_stars_power";
	public static string UPPER_TEXT_03 = "train_stars_time";*/
	
	private Renderer _star01;
	private Renderer _star02;
	private Renderer _star03;
	
	private Renderer _tick01;
	private Renderer _tick02;
	private Renderer _tick03;
/*	
	private GameObject spotlight01;
	private GameObject spotlight02;
	private GameObject spotlight03;
*/
	public bool startValuesRetrieved = false;
	public int i = 1;
	public int faradaydoStartPower;
	public int faradaydoPower;
	public int joseStartPower;
	public int josePower;
	
	public float finishedTime = 0;
	public float maxTime = 0;
	//*************************************************************//	
	private static MNSuccessScreenControl _meInstance;
	public static MNSuccessScreenControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraResoultScreen" ).transform.Find ( "SuccessScreen" ).GetComponent < MNSuccessScreenControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		/*
		//if ( GameGlobalVariables.RELEASE )
		{
			if ( GameGlobalVariables.LAB_ENTERED == 0 )
			{
				switch ( MNLevelControl.LEVEL_ID )
				{
					case 1:
					case 2: 
						transform.Find ( "buttonRestart" ).gameObject.SetActive ( false );
						Destroy ( transform.Find ( "buttonLab" ).GetComponent < MainScreenPLayButtonControl > ());
						transform.Find ( "buttonLab" ).gameObject.AddComponent < GoToNextLevelButton > ();
						break;
					case 3:
						transform.Find ( "buttonRestart" ).gameObject.SetActive ( false );
						break;
				}
			}
		}
		*/
	}

	void Update ()
	{
		if (TRLevelControl.getInstance ()._levelsLoaded && !startValuesRetrieved) 
		{
			faradaydoStartPower = TRLevelControl.getInstance ().getCharacter (GameElements.CHAR_FARADAYDO_1_IDLE).characterValues [CharacterData.CHARACTER_ACTION_TYPE_POWER];
			joseStartPower = TRLevelControl.getInstance ().getCharacter (GameElements.CHAR_JOSE_1_IDLE).characterValues [CharacterData.CHARACTER_ACTION_TYPE_POWER];
			startValuesRetrieved = true;
		}
	}

	public void CustomStart () 
	{
		_text1 = GameObject.Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ();
		_text1.myKey = "ui_keep_full_power";
		_text2 = GameObject.Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ();
		_text2.myKey = "ui_keep_full_power";
		_text3 = GameObject.Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ();
		_text3.myKey = "ui_less_than_seconds_02";
		maxTime = TRLevelControl.CURRENT_LEVEL_CLASS.star04;
		_timeText = GameObject.Find ("textMovesLeft03").GetComponent < TextMesh > (); 
		_timeText.text = TimeScaleManager.getTimeString ( TRLevelControl.CURRENT_LEVEL_CLASS.star04 );
		
		/*_textUp01 = GameObject.Find ( "textUpper01" ).GetComponent < TextMesh > ();
		_textUp02 = GameObject.Find ( "textUpper02" ).GetComponent < TextMesh > ();
		_textUp03 = GameObject.Find ( "textUpper03" ).GetComponent < TextMesh > ();

		_textLow01 = GameObject.Find ( "textLower01" ).GetComponent < TextMesh > ();
		_textLow02 = GameObject.Find ( "textLower02" ).GetComponent < TextMesh > ();
		_textLow03 = GameObject.Find ( "textLower03" ).GetComponent < TextMesh > ();

		_textUp01.text = _text1.myKey;
		_textUp02.text = _text2.myKey;
		_textUp03.text = _text3.myKey;

		_textLow01.text = "5/5";
		_textLow02.text = "5/5";
		_textLow03.text = maxTime.ToString() + "s";
*/

		_star01 = GameObject.Find ( "star01" ).gameObject.renderer;
		_star02 = GameObject.Find ( "star02" ).gameObject.renderer;
		_star03 = GameObject.Find ( "star03" ).gameObject.renderer;
	/*	
		_tick01 = GameObject.Find ( "tick01" ).gameObject.renderer;
		_tick02 = GameObject.Find ( "tick02" ).gameObject.renderer;
		_tick03 = GameObject.Find ( "tick03" ).gameObject.renderer;
		
		spotlight01 = GameObject.Find ( "spotlight01" ).gameObject;
		spotlight02 = GameObject.Find ( "spotlight02" ).gameObject;
		spotlight03 = GameObject.Find ( "spotlight03" ).gameObject;
		*/
		//*************************************************************//
		transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( false );
		transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( false );
		
		_star01.gameObject.SetActive ( false );
		_star02.gameObject.SetActive ( false );
		_star03.gameObject.SetActive ( false );
	/*	
		_tick01.gameObject.SetActive ( false );
		_tick02.gameObject.SetActive ( false );
		_tick03.gameObject.SetActive ( false );
		
		spotlight01.gameObject.SetActive ( false );
		spotlight02.gameObject.SetActive ( false );
		spotlight03.gameObject.SetActive ( false );
		*/

		finishedTime = TRSpeedAndTrackOMetersManager.getInstance ()._finishedTime;

		faradaydoPower = TRLevelControl.getInstance().getCharacter( GameElements.CHAR_FARADAYDO_1_IDLE ).characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER];
		josePower = TRLevelControl.getInstance().getCharacter( GameElements.CHAR_JOSE_1_IDLE ).characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER];


		StartCoroutine ( startAnimationSequence ( faradaydoPower, faradaydoStartPower, josePower, joseStartPower, finishedTime, maxTime ));
	}

	private IEnumerator startAnimationSequence ( int faradaydoPower,int faradaydoStartPower, int josePower, int joseStartPower, float finishedTime, float maxTime )
	{
		SoundManager.getInstance ().silenceMusicTillNewScene ();
		SoundManager.getInstance ().playSound (SoundManager.MISSION_COMPLETE);
		ChChChSoundManger.getInstance ().mySource.volume = 0.0f;
		int starCounter = 1;
		StarsValue startValue01 = StarsValue.NON;
		StarsValue startValue02 = StarsValue.NON;
		StarsValue startValue03 = StarsValue.NON;
		
		bool tick01_01Positive = faradaydoPower >= faradaydoStartPower;
		//*************************************************************//
		/*yield return new WaitForSeconds ( 0.15f );
		SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
		_tick01.gameObject.SetActive ( true );
		iTween.ScaleFrom ( _tick01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick01.transform.localScale * 3f ));
		if ( tick01_01Positive ) _tick01.material.mainTexture = tickPositive;
		else _tick01.material.mainTexture = tickNegative;*/
		//*************************************************************//
		
		if ( tick01_01Positive ) startValue01 = StarsValue.FULL;
		else startValue01 = StarsValue.NON;
		
		//*************************************************************//
		bool tick02_01Positive = josePower >= joseStartPower;
		/*yield return new WaitForSeconds ( 0.5f );
		SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
		_tick02.gameObject.SetActive ( true );
		iTween.ScaleFrom ( _tick02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick02.transform.localScale * 3f ));
		if ( tick02_01Positive ) _tick02.material.mainTexture = tickPositive;
		else _tick02.material.mainTexture = tickNegative;*/
		//*************************************************************//
		
		if ( tick02_01Positive ) startValue02 = StarsValue.FULL;
		else startValue02 = StarsValue.NON;
		
		//*************************************************************//
		bool tick03Positive = finishedTime <= maxTime;
		/*yield return new WaitForSeconds ( 0.5f );
		SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
		_tick03.gameObject.SetActive ( true );
		iTween.ScaleFrom ( _tick03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick03.transform.localScale * 3f ));
		if ( tick03Positive ) _tick03.material.mainTexture = tickPositive;
		else _tick03.material.mainTexture = tickNegative;*/
		//*************************************************************//
		
		if ( tick03Positive ) startValue03 = StarsValue.FULL;
		else startValue03 = StarsValue.NON; 
		
		//*************************************************************//
		yield return new WaitForSeconds ( 0.5f );
		if ( startValue01 != StarsValue.NON )
		{


			switch ( startValue01 )
			{
			case StarsValue.HALF:
				_star01.gameObject.SetActive ( true );
				iTween.ScaleFrom ( _star01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star01.transform.localScale * 3f ));
				_star01.material.mainTexture = star01Half;
				SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
				//print ("first + one");

				break;
			case StarsValue.FULL:
				_star01.gameObject.SetActive ( true );
				iTween.ScaleFrom ( _star01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star01.transform.localScale * 3f ));
				_star01.material.mainTexture = star02Full;
				SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
				starCounter++;
				//print ("first + one");
				//spotlight01.gameObject.SetActive ( true );
				break;
			}
			//_tick01.gameObject.SetActive ( true );
			//iTween.ScaleFrom ( _tick01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick01.transform.localScale * 3f ));
			//if ( tick01_01Positive ) _tick01.material.mainTexture = tickPositive;
			//else _tick01.material.mainTexture = tickNegative;
		}
		else
		{
			//print ("no star faradaydo");
			//_tick01.gameObject.SetActive ( true );
			SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
			//_tick01.material.mainTexture = tickNegative;
			//iTween.ScaleFrom ( _tick01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick01.transform.localScale * 3f ));
		}
		//*************************************************************//
		yield return new WaitForSeconds ( 0.5f );
		if ( startValue02 != StarsValue.NON )
		{


			switch ( startValue02 )
			{
			case StarsValue.HALF:
				_star02.gameObject.SetActive ( true );
				iTween.ScaleFrom ( _star02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star02.transform.localScale * 3f ));
				_star02.material.mainTexture = star02Half;

				SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
				break;
			case StarsValue.FULL:
				_star02.gameObject.SetActive ( true );
				iTween.ScaleFrom ( _star02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star02.transform.localScale * 3f ));
				_star02.material.mainTexture = star02Full;
				//spotlight02.gameObject.SetActive ( true );

				if(starCounter == 2)
				{
					SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
					starCounter++;
				//	print ("second + one");
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
					starCounter++;
					//print ("second + one");
				}
				break;
			}
			//_tick02.gameObject.SetActive ( true );
			//iTween.ScaleFrom ( _tick02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick02.transform.localScale * 3f ));
			//if ( tick02_01Positive ) _tick02.material.mainTexture = tickPositive;
			//else _tick02.material.mainTexture = tickNegative;
		}
		else
		{
			//print ("no star jose");
			//_tick02.gameObject.SetActive ( true );
			//_tick02.material.mainTexture = tickNegative;
			SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
			//iTween.ScaleFrom ( _tick02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick02.transform.localScale * 3f ));
		}
		//*************************************************************//
		yield return new WaitForSeconds ( 0.5f );
		if ( startValue03 != StarsValue.NON )
		{

			switch ( startValue03 )
			{
			case StarsValue.HALF:
				_star03.gameObject.SetActive ( true );
				iTween.ScaleFrom ( _star03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star03.transform.localScale * 3f ));
				_star03.material.mainTexture = star03Half;
				//print ("is half");
				SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
				break;
			case StarsValue.FULL:
				_star03.gameObject.SetActive ( true );
				iTween.ScaleFrom ( _star03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star03.transform.localScale * 3f ));
				_star03.material.mainTexture = star02Full;
				//spotlight03.gameObject.SetActive ( true );
				//print ("is full");
				if(starCounter == 3)
				{
					SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
					starCounter++;
					//print ("third + one");
				}
				else if(starCounter == 2)
				{
					SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
					starCounter++;
					//print ("third + one");
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
					starCounter++;
					//print ("third + one");
				}
				break;
			}
			//_tick03.gameObject.SetActive ( true );
			//iTween.ScaleFrom ( _tick03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick03.transform.localScale * 3f ));
			//if ( tick03Positive ) _tick03.material.mainTexture = tickPositive;
			//else _tick03.material.mainTexture = tickNegative;
		}
		else
		{
			//_tick03.gameObject.SetActive ( true );
			//_tick03.material.mainTexture = tickNegative;
			SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
			//iTween.ScaleFrom ( _tick03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _tick03.transform.localScale * 3f ));
		}
		//*************************************************************//
		yield return new WaitForSeconds ( 0.5f );
		SoundManager.getInstance ().playSound ( SoundManager.BUTTONS_APAERE, -1, true );
		int alreadyEarnedStars = SaveDataManager.getValue ( SaveDataManager.TRAIN_LEVEL_STARS_PREFIX + ( TRLevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE ? "" : "B" ) + TRLevelControl.CURRENT_LEVEL_CLASS.myName );
		int starsEarned = countStarsForThisLevel ( startValue01, startValue02, startValue03 );
		if ( alreadyEarnedStars < starsEarned )
		{
			TRLevelControl.CURRENT_LEVEL_CLASS.starsEarned = starsEarned;
			SaveDataManager.save ( SaveDataManager.TRAIN_LEVEL_STARS_PREFIX + TRLevelControl.CURRENT_LEVEL_CLASS.myName, starsEarned );
		}
		
		//SaveDataManager.save ( SaveDataManager. + MNLevelControl.CURRENT_LEVEL_CLASS.myName, 1 );
		
		transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( true );
		transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( true );
		
		//*************************************************************//
		iTween.ScaleFrom ( transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).localScale * 3f ));
		iTween.ScaleFrom ( transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).localScale * 3f ));
		//*************************************************************//
	}

	private int countStarsForThisLevel ( StarsValue star01Value, StarsValue star02Value, StarsValue star03Value )
	{
//		print ((int)star01Value+" "+(int)star02Value+" "+(int)star03Value);
		float valueOfStars = 0f;
		
		switch ( star01Value )
		{
		case StarsValue.HALF:
			valueOfStars += 0.5f;
			break;
		case StarsValue.FULL:
			valueOfStars += 1f;
			break;
		}
		
		switch ( star02Value )
		{
		case StarsValue.HALF:
			valueOfStars += 0.5f;
			break;
		case StarsValue.FULL:
			valueOfStars += 1f;
			break;
		}
		
		switch ( star03Value )
		{
		case StarsValue.HALF:
			valueOfStars += 0.5f;
			break;
		case StarsValue.FULL:
			valueOfStars += 1f;
			break;
		}
		
		return (int) valueOfStars;
	}
}