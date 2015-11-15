using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SuccessScreenControl : MonoBehaviour 
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
	public Texture2D star01Full;
	public Texture2D star01Half;
	public Texture2D star02Full;
	public Texture2D star02Half;
	public Texture2D star03Full;
	public Texture2D star03Half;
	public String part01;
	public String part02;
	public Texture2D tickPositive;
	public Texture2D tickNegative;

	public bool star01 = false;
	public bool star02 = false;
	public bool star03 = false;

	//*************************************************************//
	private GameObject faraPlatform;
	private GameObject coraPlatform;
	private GameObject josePlatform;
	private GameObject madraPlatform;
	private GameObject bozPlatform;
	private GameObject minerPlatform;
	private GameObject motorPlatform;
	//*************************************************************//
	private TextMesh[] _remainingMovesTexts;
	private TextMesh[] _scoreBoardTexts;
	
	private Renderer[] _charactersIcon;
	
	private Renderer _star01;
	private Renderer _star02;
	private Renderer _star03;
	
	//private Renderer _tick01;
	//private Renderer _tick02;
	//private Renderer _tick03;
	
	//private GameObject spotlight01;
	//private GameObject spotlight02;
	//private GameObject spotlight03;

	private GameObject _slot01;
	private GameObject _slot02;
	private GameObject _slot03;
	private int platformCharID = 0;
	private GameObject beginerScreen;
	private GameObject beginerText;
	private GameObject sign;
	private GameObject beginerSign;
	private GameObject buttonLab;
	private GameObject buttonRestart;

	private TextMesh platformText;
	//*************************************************************//	
	private static SuccessScreenControl _meInstance;
	public static SuccessScreenControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraResoultScreen" ).transform.Find ( "SuccessScreen" ).GetComponent < SuccessScreenControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		/*
		if ( GameGlobalVariables.LAB_ENTERED == 0 )
		{
			switch ( LevelControl.LEVEL_ID )
			{
				case 1:
				case 2:
				case 3:
					transform.Find ( "buttonRestart" ).gameObject.SetActive ( false );
					Destroy ( transform.Find ( "buttonLab" ).GetComponent < MainScreenPLayButtonControl > ());
					transform.Find ( "buttonLab" ).gameObject.AddComponent < GoToNextLevelButton > ();
					break;
				case 4:
					transform.Find ( "buttonRestart" ).gameObject.SetActive ( false );
					break;
			}
		}
		*/
	}
	
	public void CustomStart (bool fromBeginerLevel = false, int beginerLevelID = -1) 
	{
		buttonLab = GameObject.Find("buttonLab");
		buttonRestart = GameObject.Find ("buttonRestart");
		platformText = transform.Find ("platform2").transform.Find ("platformText").gameObject.GetComponent<TextMesh>();
		platformText.text = "YaRP";

		//============================================RR result screen logic============================
		_slot01 = transform.Find ( "slot01" ).gameObject;
		_slot02 = transform.Find ( "slot02" ).gameObject;
		_slot03 = transform.Find ( "slot03" ).gameObject;

		_scoreBoardTexts = new TextMesh[3];
		_scoreBoardTexts[0] = _slot01.transform.Find ( "textMovesLeft01" ).GetComponent < TextMesh > ();
		_scoreBoardTexts[1] = _slot02.transform.Find ( "textMovesLeft02" ).GetComponent < TextMesh > ();
		_scoreBoardTexts[2] = _slot03.transform.Find ( "textMovesLeft03" ).GetComponent < TextMesh > ();

		_scoreBoardTexts[0].text = LevelControl.CURRENT_LEVEL_CLASS.movesLimitForStar.ToString();
		_scoreBoardTexts[1].text = LevelControl.CURRENT_LEVEL_CLASS.timeLimitForStar.ToString();
		_scoreBoardTexts [2].text = LevelControl.CURRENT_LEVEL_CLASS.scoreLimitForStar.ToString ();													//LevelControl.CURRENT_LEVEL_CLASS.scoreLimitForStar.ToString ();

		_star01 = _slot01.transform.Find ( "star01" ).gameObject.renderer;
		_star02 = _slot02.transform.Find ( "star02" ).gameObject.renderer;
		_star03 = _slot03.transform.Find ( "star03" ).gameObject.renderer;
		
		
		
		_star01.gameObject.SetActive ( false );
		_star02.gameObject.SetActive ( false );
		_star03.gameObject.SetActive ( false );

		StarsValue starValue01 = StarsValue.DONT_COUNT;
		StarsValue starValue02 = StarsValue.DONT_COUNT;
		StarsValue starValue03 = StarsValue.DONT_COUNT;

		int numberOfStarsAwarded = 0;
		if(GameGlobalVariables.Stats.MOVES + LevelControl.CURRENT_LEVEL_CLASS.movesLimitForStar >= LevelControl.CURRENT_LEVEL_CLASS.moves)
		{
//			print ("Moves Check = " + GameGlobalVariables.Stats.MOVES + " + " + LevelControl.CURRENT_LEVEL_CLASS.movesLimitForStar + " ? " + LevelControl.CURRENT_LEVEL_CLASS.moves);
			numberOfStarsAwarded ++;
			starValue01 = StarsValue.FULL;
		}
		if(TimerControl.getInstance().timerBeaten())
		{
//			print ("WAS BEATEN = " + TimerControl.getInstance().timerBeaten());
			numberOfStarsAwarded ++;
			starValue02 = StarsValue.FULL;
		}

		if(GameGlobalVariables.Stats.SCORE >= LevelControl.CURRENT_LEVEL_CLASS.scoreLimitForStar)
		{
//			print ("Score Obtained" + GameGlobalVariables.Stats.SCORE + " ?> " + LevelControl.CURRENT_LEVEL_CLASS.scoreLimitForStar);
			numberOfStarsAwarded ++;
			starValue03 = StarsValue.FULL;
		}

		transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( false );
		transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( false );

		StartCoroutine ( startAnimationSequence ( starValue01, starValue02, starValue03, numberOfStarsAwarded ));
		//============================================RR result screen logic============================

		/*
		SaveDataManager.save ( SaveDataManager.LEVEL_FINISHED_PREFIX + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.CURRENT_LEVEL_CLASS.myName, 1 );
		_slot01 = transform.Find ( "slot01" ).gameObject;
		_slot02 = transform.Find ( "slot02" ).gameObject;
		_slot03 = transform.Find ( "slot03" ).gameObject;
		_slot01.SetActive (false); 
		_slot02.SetActive (false); 
		_slot03.SetActive (false); 

		beginerScreen = GameObject.Find ("award_panel_beginer");
		beginerText = beginerScreen.transform.Find ("beginerSlot").transform.Find ("textBeginer").gameObject;
		beginerText.SetActive (false);
		sign = GameObject.Find ("sign");
		sign.SetActive (false);
		beginerSign = GameObject.Find ("beginerSign");
		beginerSign.SetActive (false);
		beginerScreen.SetActive (false);

		buttonLab = GameObject.Find("buttonLab");
		buttonRestart = GameObject.Find ("buttonRestart");
		buttonLab.SetActive (false);
		buttonRestart.SetActive (false);

		if ( LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 0 || LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 0 ||  LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 0 || LevelControl.CURRENT_LEVEL_CLASS.star04Slot == 0 ) _slot01.SetActive ( true );
		if ( LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 1 || LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 1 ||  LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 1 || LevelControl.CURRENT_LEVEL_CLASS.star04Slot == 1 ) _slot02.SetActive ( true );
		if ( LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 2 || LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 2 ||  LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 2 || LevelControl.CURRENT_LEVEL_CLASS.star04Slot == 2 ) _slot03.SetActive ( true );

		if ( ! _slot02.activeSelf )
		{
			_slot01.transform.position = new Vector3(_slot01.transform.position.x, _slot01.transform.position.y - 0.4f, _slot01.transform.position.z);
			_slot03.transform.position = new Vector3(_slot03.transform.position.x, _slot03.transform.position.y + 0.4f, _slot03.transform.position.z);
		} 

		int numberOfStarSlots = 0;

		if ( _slot01.activeSelf ) numberOfStarSlots++;
		if ( _slot02.activeSelf ) numberOfStarSlots++;
		if ( _slot03.activeSelf ) numberOfStarSlots++;
		_remainingMovesTexts = new TextMesh[3];
		_remainingMovesTexts[0] = _slot01.transform.Find ( "textMovesLeft01" ).GetComponent < TextMesh > ();
		_remainingMovesTexts[1] = _slot02.transform.Find ( "textMovesLeft02" ).GetComponent < TextMesh > ();
		_remainingMovesTexts[2] = _slot03.transform.Find ( "textMovesLeft03" ).GetComponent < TextMesh > ();
		
		_charactersIcon = new Renderer[3];
		_charactersIcon[0] = _slot01.transform.Find ( "character01Icon" ).gameObject.renderer;
		_charactersIcon[1] = _slot02.transform.Find ( "character02Icon" ).gameObject.renderer;
		_charactersIcon[2] = _slot03.transform.Find ( "character03Icon" ).gameObject.renderer;
		
		_star01 = _slot01.transform.Find ( "star01" ).gameObject.renderer;
		_star02 = _slot02.transform.Find ( "star02" ).gameObject.renderer;
		_star03 = _slot03.transform.Find ( "star03" ).gameObject.renderer;
		
		
		
		_star01.gameObject.SetActive ( false );
		_star02.gameObject.SetActive ( false );
		_star03.gameObject.SetActive ( false );
		
		
		_charactersIcon[0].gameObject.SetActive ( false );
		_charactersIcon[1].gameObject.SetActive ( false );
		_charactersIcon[2].gameObject.SetActive ( false );
		

		StarsValue startValue01 = StarsValue.DONT_COUNT;
		StarsValue startValue02 = StarsValue.DONT_COUNT;
		StarsValue startValue03 = StarsValue.DONT_COUNT;
*/
		//================== Set Rescued Toy on Platform ====================
		GameObject myPlatform = GameObject.Find("SuccessScreen").transform.Find("platform2").gameObject;
		faraPlatform = myPlatform.transform.Find ("fara").gameObject;
		coraPlatform = myPlatform.transform.Find ("cora").gameObject;
		madraPlatform = myPlatform.transform.Find ("madra").gameObject;
		josePlatform = myPlatform.transform.Find ("jose").gameObject;
		bozPlatform = myPlatform.transform.Find ("boz").gameObject;
		minerPlatform = myPlatform.transform.Find ("miners").gameObject;
		motorPlatform = myPlatform.transform.Find ("powerMotor").gameObject;

		faraPlatform.SetActive (false);
		coraPlatform.SetActive (false);
		madraPlatform.SetActive (false);
		josePlatform.SetActive (false);
		bozPlatform.SetActive (false);
		minerPlatform.SetActive (false);
		motorPlatform.SetActive (false);
		foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
		{
			if(character.GetComponent<ToBeRescuedComponent>() != null)
			{
				if(character.GetComponent<IComponent>() != null)
				{
					platformCharID = character.GetComponent<IComponent>().myID;
				}
			}
		}
		if(platformCharID == GameElements.CHAR_FARADAYDO_1_DRAINED)
		{
			platformText.text = "FARADAYDO";
			faraPlatform.SetActive(true);
			faraPlatform.transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "idle";
		}
		else if(platformCharID == GameElements.CHAR_CORA_1_DRAINED)
		{
			platformText.text = "CORA";
			coraPlatform.SetActive(true);
			coraPlatform.transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "idle";
		}
		else if(platformCharID == GameElements.CHAR_MADRA_1_DRAINED)
		{
			platformText.text = "MADRA";
			madraPlatform.SetActive(true);
			madraPlatform.transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "idle";
		}
		else if(platformCharID == GameElements.CHAR_JOSE_IDLE_DRAINED)
		{
			platformText.text = "JOSE";
			josePlatform.SetActive(true);
			josePlatform.GetComponent<SkeletonAnimation>().animationName = "Stand_idle_animation";
		}
		else if(platformCharID == GameElements.CHAR_BOZ_IDLE_DRAINED)
		{
			platformText.text = "BOZ";
			bozPlatform.SetActive(true);
			bozPlatform.transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "idle";
		}
		else if(platformCharID == GameElements.CHAR_MINER_1_IDLE_DRAINED)
		{
			platformText.text = "MINER";
			minerPlatform.SetActive(true);
			minerPlatform.transform.Find("tile").transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "miner_01_stand_idleanim";
		}
		else if(platformCharID == GameElements.CHAR_MINER_2_IDLE_DRAINED)
		{
			platformText.text = "MINER";
			minerPlatform.SetActive(true);
			minerPlatform.transform.Find("tile").transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "miner_02_stand_idleanim";
		}
		else if(platformCharID == GameElements.CHAR_MINER_3_IDLE_DRAINED)
		{
			platformText.text = "MINER";
			minerPlatform.SetActive(true);
			minerPlatform.transform.Find("tile").transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "miner_03_stand_idleanim";
		}
		else if(platformCharID == GameElements.CHAR_MINER_4_IDLE_DRAINED)
		{
			platformText.text = "MINER";
			minerPlatform.SetActive(true);
			minerPlatform.transform.Find("tile").transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "miner_04_stand_idleanim";
		}
		else if(platformCharID == GameElements.CHAR_MINER_5_IDLE_DRAINED)
		{
			platformText.text = "MINER";
			minerPlatform.SetActive(true);
			minerPlatform.transform.Find("tile").transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "miner_05_stand_idleanim";
		}
		else if(platformCharID == GameElements.CHAR_MINER_6_IDLE_DRAINED)
		{
			platformText.text = "MINER";
			minerPlatform.SetActive(true);
			minerPlatform.transform.Find("tile").transform.Find("front").GetComponent<SkeletonAnimation>().animationName = "miner_06_stand_idleanim";
		}
		/*
		else
		{
			motorPlatform.SetActive(true);
		}
		//===================================================================

		if(fromBeginerLevel == false)
		{
			for (int i = 0; i < LevelControl.getInstance ().charactersOnLevel.Count; i++) 
			{
				_charactersIcon [i].gameObject.SetActive (true);
				switch (LevelControl.getInstance ().charactersOnLevel [i].myID) 
				{



					case GameElements.CHAR_FARADAYDO_1_IDLE:

					if (LevelControl.CURRENT_LEVEL_CLASS.star01 != 0) 
					{
						_remainingMovesTexts [LevelControl.CURRENT_LEVEL_CLASS.star01Slot].text = LevelControl.CURRENT_LEVEL_CLASS.star01.ToString ();
						//======================Align Text Spaces=======================
						if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 2)
						{
							Transform myPos3 = transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
							myPos3.position = new Vector3 (myPos3.position.x - 0.075f, myPos3.position.y, myPos3.position.z);
							Transform myPos3a = transform.Find ( "slot03" ).Find ( "textMovesLeft03b" );
							myPos3a.position = new Vector3 (myPos3a.position.x - 0.15f, myPos3a.position.y, myPos3a.position.z);
						}
						
						else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 1)
						{
							Transform myPos2 = transform.Find ( "slot02" ).Find ( "textMovesLeft02" );
							myPos2.position = new Vector3 (myPos2.position.x - 0.075f, myPos2.position.y, myPos2.position.z);
							Transform myPos2a = transform.Find ( "slot02" ).Find ( "textMovesLeft02b" );
							myPos2a.position = new Vector3 (myPos2a.position.x - 0.15f, myPos2a.position.y, myPos2a.position.z);
						}
						else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 0)
						{
							Transform myPos1 = transform.Find ( "slot01" ).Find ( "textMovesLeft01" );
							myPos1.position = new Vector3 (myPos1.position.x - 0.075f, myPos1.position.y, myPos1.position.z);
							Transform myPos1a = transform.Find ( "slot01" ).Find ( "textMovesLeft01b" );
							myPos1a.position = new Vector3 (myPos1a.position.x - 0.15f, myPos1a.position.y, myPos1a.position.z);
						}
						//======================Align Text Spaces=======================

						_charactersIcon [LevelControl.CURRENT_LEVEL_CLASS.star01Slot].material.mainTexture = LevelControl.getInstance ().gameElements [GameElements.UI_FARADAYDO_ICON];
						_charactersIcon [LevelControl.CURRENT_LEVEL_CLASS.star01Slot].gameObject.SetActive (true);

						switch (LevelControl.CURRENT_LEVEL_CLASS.star01Slot) 
						{
						case 0:
								startValue01 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star01 ? StarsValue.FULL :  StarsValue.NON;
								break;
						case 1:
								startValue02 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star01 ? StarsValue.FULL :  StarsValue.NON;
								break;
						case 2:
								startValue03 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star01 ? StarsValue.FULL : StarsValue.NON;
								break;
						}
						if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 2)
						{
							transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos3a = transform.Find ( "slot03" ).Find ( "textMovesLeft03a" );
							myPos3a.position = new Vector3 (myPos3a.position.x - 0.075f, myPos3a.position.y, myPos3a.position.z);
						}
						
						else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 1)
						{
							transform.Find ( "slot02" ).Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos2a = transform.Find ( "slot02" ).Find ( "textMovesLeft02a" );
							myPos2a.position = new Vector3 (myPos2a.position.x - 0.075f, myPos2a.position.y, myPos2a.position.z);
						}
						
						else if(LevelControl.CURRENT_LEVEL_CLASS.star01 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star01Slot == 0)
						{
							transform.Find ( "slot01" ).Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos1a = transform.Find ( "slot01" ).Find ( "textMovesLeft01a" );
							myPos1a.position = new Vector3 (myPos1a.position.x - 0.075f, myPos1a.position.y, myPos1a.position.z);
						}
						//=========================Grammar Correction==========================
					}
						break;



					case GameElements.CHAR_CORA_1_IDLE:
					if (LevelControl.CURRENT_LEVEL_CLASS.star02 != 0) 
					{
						_remainingMovesTexts [LevelControl.CURRENT_LEVEL_CLASS.star02Slot].text = LevelControl.CURRENT_LEVEL_CLASS.star02.ToString ();

						//======================Align Text Spaces=======================
						if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 2)
						{
							Transform myPos3 = transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
							myPos3.position = new Vector3 (myPos3.position.x - 0.075f, myPos3.position.y, myPos3.position.z);
							Transform myPos3a = transform.Find ( "slot03" ).Find ( "textMovesLeft03b" );
							myPos3a.position = new Vector3 (myPos3a.position.x - 0.15f, myPos3a.position.y, myPos3a.position.z);
						}

						else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 1)
						{
							Transform myPos2 = transform.Find ( "slot02" ).Find ( "textMovesLeft02" );
							myPos2.position = new Vector3 (myPos2.position.x - 0.075f, myPos2.position.y, myPos2.position.z);
							Transform myPos2a = transform.Find ( "slot02" ).Find ( "textMovesLeft02b" );
							myPos2a.position = new Vector3 (myPos2a.position.x - 0.15f, myPos2a.position.y, myPos2a.position.z);
						}
						else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 0)
						{
							Transform myPos1 = transform.Find ( "slot01" ).Find ( "textMovesLeft01" );
							myPos1.position = new Vector3 (myPos1.position.x - 0.075f, myPos1.position.y, myPos1.position.z);
							Transform myPos1a = transform.Find ( "slot01" ).Find ( "textMovesLeft01b" );
							myPos1a.position = new Vector3 (myPos1a.position.x - 0.15f, myPos1a.position.y, myPos1a.position.z);
						}
						//======================Align Text Spaces=======================
						_charactersIcon [LevelControl.CURRENT_LEVEL_CLASS.star02Slot].material.mainTexture = LevelControl.getInstance ().gameElements [GameElements.UI_CORA_ICON];
						_charactersIcon [LevelControl.CURRENT_LEVEL_CLASS.star02Slot].gameObject.SetActive (true);

						switch (LevelControl.CURRENT_LEVEL_CLASS.star02Slot) 
						{
						case 0:
								startValue01 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star02 ? StarsValue.FULL :  StarsValue.NON;
								break;
						case 1:
								startValue02 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star02 ? StarsValue.FULL :  StarsValue.NON;
								break;
						case 2:
								startValue03 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star02 ? StarsValue.FULL :  StarsValue.NON;
								break;
						}
						//=========================Grammar Correction==========================
						if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 2)
						{
							transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos3a = transform.Find ( "slot03" ).Find ( "textMovesLeft03a" );
							myPos3a.position = new Vector3 (myPos3a.position.x - 0.075f, myPos3a.position.y, myPos3a.position.z);
						}

						else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 1)
						{
							transform.Find ( "slot02" ).Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos2a = transform.Find ( "slot02" ).Find ( "textMovesLeft02a" );
							myPos2a.position = new Vector3 (myPos2a.position.x - 0.075f, myPos2a.position.y, myPos2a.position.z);
						}

						else if(LevelControl.CURRENT_LEVEL_CLASS.star02 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star02Slot == 0)
						{
							transform.Find ( "slot01" ).Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos1a = transform.Find ( "slot01" ).Find ( "textMovesLeft01a" );
							myPos1a.position = new Vector3 (myPos1a.position.x - 0.075f, myPos1a.position.y, myPos1a.position.z);
						}
						//=========================Grammar Correction==========================
					}
						break;



					case GameElements.CHAR_MADRA_1_IDLE:
					if (LevelControl.CURRENT_LEVEL_CLASS.star03 != 0) 
					{
						_remainingMovesTexts [LevelControl.CURRENT_LEVEL_CLASS.star03Slot].text = LevelControl.CURRENT_LEVEL_CLASS.star03.ToString ();
						//======================Align Text Spaces=======================
						if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 2)
						{
							Transform myPos3 = transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
							myPos3.position = new Vector3 (myPos3.position.x - 0.075f, myPos3.position.y, myPos3.position.z);
							Transform myPos3a = transform.Find ( "slot03" ).Find ( "textMovesLeft03b" );
							myPos3a.position = new Vector3 (myPos3a.position.x - 0.15f, myPos3a.position.y, myPos3a.position.z);
						}
						
						else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 1)
						{
							Transform myPos2 = transform.Find ( "slot02" ).Find ( "textMovesLeft02" );
							myPos2.position = new Vector3 (myPos2.position.x - 0.075f, myPos2.position.y, myPos2.position.z);
							Transform myPos2a = transform.Find ( "slot02" ).Find ( "textMovesLeft02b" );
							myPos2a.position = new Vector3 (myPos2a.position.x - 0.15f, myPos2a.position.y, myPos2a.position.z);
						}
						else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 10 && LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 0)
						{
							Transform myPos1 = transform.Find ( "slot01" ).Find ( "textMovesLeft01" );
							myPos1.position = new Vector3 (myPos1.position.x - 0.075f, myPos1.position.y, myPos1.position.z);
							Transform myPos1a = transform.Find ( "slot01" ).Find ( "textMovesLeft01b" );
							myPos1a.position = new Vector3 (myPos1a.position.x - 0.15f, myPos1a.position.y, myPos1a.position.z);
						}
						//======================Align Text Spaces=======================

						_charactersIcon [LevelControl.CURRENT_LEVEL_CLASS.star03Slot].material.mainTexture = LevelControl.getInstance ().gameElements [GameElements.UI_MADRA_ICON];
						_charactersIcon [LevelControl.CURRENT_LEVEL_CLASS.star03Slot].gameObject.SetActive (true);

						switch (LevelControl.CURRENT_LEVEL_CLASS.star03Slot) 
						{
						case 0:
								startValue01 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star03 ? StarsValue.FULL : StarsValue.NON;
								break;
						case 1:
								startValue02 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star03 ? StarsValue.FULL : StarsValue.NON;
								break;
						case 2:
								startValue03 = LevelControl.getInstance ().charactersOnLevel [i].totalMovesPerfromed <= LevelControl.CURRENT_LEVEL_CLASS.star03 ? StarsValue.FULL : StarsValue.NON;
								break;
						}
						//=========================Grammar Correction==========================
						if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 2)
						{
							transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos3a = transform.Find ( "slot03" ).Find ( "textMovesLeft03a" );
							myPos3a.position = new Vector3 (myPos3a.position.x - 0.075f, myPos3a.position.y, myPos3a.position.z);
						}
						
						else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 1)
						{
							transform.Find ( "slot02" ).Find ( "textMovesLeft02a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos2a = transform.Find ( "slot02" ).Find ( "textMovesLeft02a" );
							myPos2a.position = new Vector3 (myPos2a.position.x - 0.075f, myPos2a.position.y, myPos2a.position.z);
						}
						
						else if(LevelControl.CURRENT_LEVEL_CLASS.star03 < 2 && LevelControl.CURRENT_LEVEL_CLASS.star03Slot == 0)
						{
							transform.Find ( "slot01" ).Find ( "textMovesLeft01a" ).GetComponent < GameTextControl > ().myKey = "ui_less_than_moves_03";
							Transform myPos1a = transform.Find ( "slot01" ).Find ( "textMovesLeft01a" );
							myPos1a.position = new Vector3 (myPos1a.position.x - 0.075f, myPos1a.position.y, myPos1a.position.z);
						}
						//=========================Grammar Correction==========================
					}
						break;
				}
			}
		

		
			if ( LevelControl.CURRENT_LEVEL_CLASS.star04 != 0 )
			{  
				_charactersIcon[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].gameObject.SetActive ( true );
				_charactersIcon[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].material.mainTexture = LevelControl.getInstance ().gameElements[GameElements.ICON_TIMER];
				_remainingMovesTexts[LevelControl.CURRENT_LEVEL_CLASS.star04Slot].text = TimeScaleManager.getTimeString ( LevelControl.CURRENT_LEVEL_CLASS.star04 );
				transform.Find ( "slot03" ).Find ( "textMovesLeft03b" ).GetComponent < TextMesh > ().text = "";
				Transform myTimeAward = transform.Find ( "slot03" ).Find ( "textMovesLeft03" );
				print (myTimeAward.position.x);
				transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).GetComponent<GameTextControl>().myKey = "ui_less_than_seconds_02";
				myTimeAward.position = new Vector3 (myTimeAward.position.x + 0.075f, myTimeAward.position.y ,myTimeAward.position.z);
				//transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position = new Vector3(transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.x +0.075f, transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.y, transform.Find ( "slot03" ).Find ( "textMovesLeft03" ).position.z);
				transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position = new Vector3(transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.x -0.215f, transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.y, transform.Find ( "slot03" ).Find ( "textMovesLeft03a" ).position.z);
				print (myTimeAward.position.x);
				transform.Find ( "slot03" ).Find ( "textInfo03" ).GetComponent < GameTextControl > ().myKey = "android_secs";

				switch ( LevelControl.CURRENT_LEVEL_CLASS.star04Slot )
				{
					case 0:
						startValue01 = TimerControl.getInstance ().getCurrentTime () <= LevelControl.CURRENT_LEVEL_CLASS.star04 ? StarsValue.FULL : StarsValue.NON;
						break;
					case 1:
						startValue02 = TimerControl.getInstance ().getCurrentTime () <= LevelControl.CURRENT_LEVEL_CLASS.star04 ? StarsValue.FULL : StarsValue.NON;
						break;
					case 2:
						startValue03 = TimerControl.getInstance ().getCurrentTime () <= LevelControl.CURRENT_LEVEL_CLASS.star04 ? StarsValue.FULL : StarsValue.NON;
						break;
				}
			}

			StartCoroutine ( startAnimationSequence ( startValue01, startValue02, startValue03, numberOfStarSlots ));
		}
		else if(fromBeginerLevel == true)
		{
			beginerScreen.SetActive(true);
			if(beginerLevelID == 1 && LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.RESCUE)
			{
				beginerText.GetComponent<GameTextControl>().myKey = "ui_sign_rescue1_message";
			}
			else if (beginerLevelID == 2 && LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.RESCUE)
			{
				beginerText.GetComponent<GameTextControl>().myKey = "ui_sign_rescue2_message";
			}
			StartCoroutine ("BeginerSequence");
		}
*/

		StartCoroutine ("BeginerSequence");
	}

	private void BeginerSequence ()
	{
		/*
		yield return new WaitForSeconds (0f);
		SoundManager.getInstance ().playSound (SoundManager.MISSION_COMPLETE);
		LevelControl.CURRENT_LEVEL_CLASS.iAmFinished = true;

		yield return new WaitForSeconds (0.15f);
		beginerSign.SetActive (true);
		iTween.ScaleFrom ( beginerSign, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", beginerSign.transform.localScale * 3f ));

		yield return new WaitForSeconds (0.5f);
		beginerText.SetActive (true);
		iTween.ScaleFrom ( beginerText, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", beginerText.transform.localScale * 3f ));

		yield return new WaitForSeconds (0.5f);
		buttonLab.SetActive (true);
		buttonRestart.SetActive (true);
		iTween.ScaleFrom ( buttonLab, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", buttonLab.transform.localScale * 3f ));
		iTween.ScaleFrom ( buttonRestart, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", buttonRestart.transform.localScale * 3f ));
		*/
	}

	private IEnumerator startAnimationSequence ( StarsValue startValue01, StarsValue startValue02, StarsValue startValue03, int numberOfStars  )
	{
		SoundManager.getInstance ().playSound (SoundManager.MISSION_COMPLETE);
		int starsEarned = countStarsForThisLevel ( startValue01, startValue02, startValue03 );
		int alreadyEarnedStars = SaveDataManager.getValue ( SaveDataManager.LEVEL_STARS_PREFIX + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.CURRENT_LEVEL_CLASS.myName );
		
		if ( alreadyEarnedStars < starsEarned )
		{
			LevelControl.CURRENT_LEVEL_CLASS.starsEarned = starsEarned;
			SaveDataManager.save ( SaveDataManager.LEVEL_STARS_PREFIX + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.CURRENT_LEVEL_CLASS.myName, starsEarned );
		}
		yield return new WaitForSeconds ( 0.15f );
		//sign.SetActive (true);
		//iTween.ScaleFrom ( sign, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", sign.transform.localScale * 3f ));
		if ( startValue01 != StarsValue.DONT_COUNT )
		{
			yield return new WaitForSeconds ( 0.5f );

			if ( startValue01 == StarsValue.FULL )
			{
				switch ( numberOfStars )
				{
					case 1:
						SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
						break;
					case 2:
						if ( startValue02 == StarsValue.FULL || startValue03 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						else SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						break;
					case 3:
						if ( startValue02 == StarsValue.FULL && startValue03 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						else if ( startValue02 == StarsValue.FULL && startValue03 != StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						else if ( startValue02 != StarsValue.FULL && startValue03 != StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						else if ( startValue02 != StarsValue.FULL && startValue03 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						break;
				}
				//_tick01.material.mainTexture = tickPositive;
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
				//_tick01.material.mainTexture = tickNegative;
			}
			switch ( startValue01 )
			{
				case StarsValue.HALF:
					_star01.gameObject.SetActive ( true );
					iTween.ScaleFrom ( _star01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star01.transform.localScale * 3f ));
					_star01.material.mainTexture = star01Half;
					break;
				case StarsValue.FULL:
					switch ( numberOfStars )
				{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				}
					_star01.gameObject.SetActive ( true );
					iTween.ScaleFrom ( _star01.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star01.transform.localScale * 3f ));
					_star01.material.mainTexture = star01Full;
				//	spotlight01.gameObject.SetActive ( true );
					break;
			}
		}
		if (( startValue02 != StarsValue.DONT_COUNT ) && ( startValue02 != StarsValue.DONT_COUNT ))
		{
			yield return new WaitForSeconds ( 0.5f );

			if ( startValue02 == StarsValue.FULL )
			{
				switch ( numberOfStars )
				{
					case 1:
						SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
						break;
					case 2:
						if ( startValue01 == StarsValue.FULL || startValue03 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						else SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						break;
					case 3:
						if ( startValue01 == StarsValue.FULL && startValue03 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						else if ( startValue01 == StarsValue.FULL && startValue03 != StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						else if ( startValue01 != StarsValue.FULL && startValue03 != StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						else if ( startValue01 != StarsValue.FULL && startValue03 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						break;
				}

				//_tick02.material.mainTexture = tickPositive;
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
				//_tick02.material.mainTexture = tickNegative;
			}
			switch ( startValue02 )
			{
				case StarsValue.HALF:
					_star02.gameObject.SetActive ( true );
					iTween.ScaleFrom ( _star02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star02.transform.localScale * 3f ));
					_star02.material.mainTexture = star02Half;
					break;
				case StarsValue.FULL:
					_star02.gameObject.SetActive ( true );
					iTween.ScaleFrom ( _star02.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star02.transform.localScale * 3f ));
					_star02.material.mainTexture = star02Full;
					//spotlight02.gameObject.SetActive ( true );
					break;
			}
		}
		if (( startValue03 != StarsValue.DONT_COUNT ) && ( startValue03 != StarsValue.DONT_COUNT ))
		{
			yield return new WaitForSeconds ( 0.5f );
			SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
			if ( startValue03 == StarsValue.FULL )
			{
				switch ( numberOfStars )
				{
					case 1:
						SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
						break;
					case 2:
						if ( startValue01 == StarsValue.FULL || startValue02 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
						else SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
						break;
					case 3:
						if ( startValue01 == StarsValue.FULL && startValue02 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
						else if ( startValue01 == StarsValue.FULL && startValue02 != StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						else if ( startValue01 != StarsValue.FULL && startValue02 != StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
						else if ( startValue01 != StarsValue.FULL && startValue02 == StarsValue.FULL ) SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
						break;
				}

				//_tick03.material.mainTexture = tickPositive;
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.X_MARK, -1, true );
				//_tick03.material.mainTexture = tickNegative;
			}

			switch ( startValue03 )
			{
				case StarsValue.HALF:
					_star03.gameObject.SetActive ( true );
					iTween.ScaleFrom ( _star03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star03.transform.localScale * 3f ));
					_star03.material.mainTexture = star03Half;
					break;
				case StarsValue.FULL:
					_star03.gameObject.SetActive ( true );
					iTween.ScaleFrom ( _star03.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", _star03.transform.localScale * 3f ));
					_star03.material.mainTexture = star03Full;
					//spotlight03.gameObject.SetActive ( true );
					break;
			}
		}
		yield return new WaitForSeconds ( 0.5f );
		SoundManager.getInstance ().playSound ( SoundManager.BUTTONS_APAERE, -1, true );

		LevelControl.CURRENT_LEVEL_CLASS.iAmFinished = true;

		//SaveDataManager.save ( SaveDataManager.LEVEL_FINISHED_PREFIX + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.CURRENT_LEVEL_CLASS.myName, 1 );
		if ( GameGlobalVariables.LAB_ENTERED == 0 )
		{
			switch ( LevelControl.LEVEL_ID )
			{
				case 1:
				case 2:
				case 3:
					transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( true );
					transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( true );
				//	Destroy ( transform.Find ( "buttonLab" ).GetComponent < MainScreenPLayButtonControl > ());
					break;
				case 4:
					transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( true );
					transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( true );
					break;
				default:
					transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( true );
					transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( true );
					break;
			}
		}
		else
		{
			transform.Find("ScaleSolver").transform.Find ( "buttonRestart" ).gameObject.SetActive ( true );
			transform.Find("ScaleSolver").transform.Find ( "buttonLab" ).gameObject.SetActive ( true );
		}

		LevelControl.CURRENT_LEVEL_CLASS.iAmFinished = true;
		SaveDataManager.save ( SaveDataManager.LEVEL_FINISHED_PREFIX + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.CURRENT_LEVEL_CLASS.myName, 1 );

		buttonLab.SetActive (true);
		buttonRestart.SetActive (true);
		iTween.ScaleFrom ( buttonLab, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", buttonLab.transform.localScale * 3f ));
		iTween.ScaleFrom ( buttonRestart, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", buttonRestart.transform.localScale * 3f ));
	}
	
	
	private int countStarsForThisLevel ( StarsValue star01Value, StarsValue star02Value, StarsValue star03Value )
	{
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
