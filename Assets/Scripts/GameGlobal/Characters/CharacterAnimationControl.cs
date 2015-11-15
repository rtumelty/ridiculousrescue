using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterAnimationControl : MonoBehaviour 
{
	//*************************************************************//	
	public bool resultScreenChar = false;
	public const int UP = 0;
	public const int DOWN = 1;
	public const int RIGHT = 2;
	public const int LEFT = 3;
	public const int INTERACT_UP = 4;
	public const int INTERACT_DOWN = 5;
	public const int INTERACT_RIGHT = 6;
	public const int INTERACT_LEFT = 7;
	public const int CELEBRATION = 8;
	public const int POWERLESS = 9;
	public const int MOVE_RIGHT = 10;
	public const int MOVE_LEFT = 11;
	public const int MOVE_DOWN = 12;
	public const int MOVE_UP = 13;
	public const int INTERACT_RIGHT_ANIMATION = 14;
	public const int INTERACT_LEFT_ANIMATION_01 = 15;
	public const int INTERACT_LEFT_ANIMATION_02 = 16;
	public const int INTERACT_UP_ANIMATION = 17;
	public const int INTERACT_DOWN_ANIMATION = 18;
	public const int JUMP_ANIMATION = 19;
	public const int IMPACT_RIGHT_ANIMATION = 20;
	public const int IMPACT_LEFT_ANIMATION = 21;
	public const int IMPACT_UP_ANIMATION = 22;
	public const int IMPACT_DOWN_ANIMATION = 23;
	public const int INTERACT_UP_TROLEY_ANIMATION = 24;
	public const int IMPACT_UP_TROLEY_ANIMATION = 25;
	public const int IDLE_01_ANIMATION = 26;
	public const int IDLE_02_ANIMATION = 27;
	public const int IDLE_03_ANIMATION = 28;
	public const int INTERACT_01_ANIMATION = 29;
	public const int INTERACT_02_ANIMATION = 30;
	public const int ELECTROCUTED_ANIMATION = 31;
	public const int ATTACK_IDLE_RIGHT_ANIMATION = 32;
	public const int ATTACK_IDLE_FRONT_ANIMATION = 33;
	public const int ATTACK_IDLE_BACK_ANIMATION = 34;
	public const int ATTACK_FRONT_ANIMATION = 35;
	public const int ATTACK_BACK_ANIMATION = 36;
	public const int IDLE_SCARED = 37;
	public const int JUMP_TO_DRILL = 38;
	public const int JUMP_TO_SQUISH = 39;
	public const int BUMP_UP = 40;
	public const int BUMP_DOWN = 41;
	public const int BUMP_RIGHT = 42;
	public const int POWER_UP = 43;
	//*************************************************************//
	public const string FARADAYDO_IDLE = "idle";
	public const string FARADAYDO_IDLE_SPECIAL_01 = "idle_special1";
	public const string FARADAYDO_IDLE_SPECIAL_02 = "idle_special2";
	public const string FARADAYDO_JUMP = "jump";
	public const string FARADAYDO_MOVE_DOWN = "front_walk";
	public const string FARADAYDO_MOVE_UP = "back";
	public const string FARADAYDO_ELECTROCUDED = "electrocuted";
	public const string FARADAYDO_SCARED = "scared";
	public const string FARADAYDO_BUILDING = "building";
	public const string FARADAYDO_BREAKING = "breaking";
	public const string FARADAYDO_MOVE_RIGHT = "walking";
	public const string FARADAYDO_DEACTIVE = "deactive";
	public const string FARADAYDO_COWER = "cower";

	private GameObject _faradaydoFront;
	private GameObject _faradaydoSide;
	//*************************************************************//
	public const string MADRA_IDLE = "idle";
	public const string MADRA_IDLE_SPECIAL_01 = "idle special1";
	public const string MADRA_WALK_UP = "walk back";
	public const string MADRA_WALK_DOWN = "walk front";
	public const string MADRA_WALK_RIGHT = "walk4";
	public const string MADRA_STANDING = "standing";
	public const string MADRA_ELECTROCUDED = "electrocuted";
	public const string MADRA_JUMP = "celebrate";
	public const string MADRA_ATTACK_FRONT_JUMP = "attack front jump";
	public const string MADRA_ATTACK_FRONT_IDLE = "attack front idle";
	public const string MADRA_ATTACK_BACK_JUMP = "attack back jump";
	public const string MADRA_ATTACK_BACK_IDLE = "attack back idle";
	public const string MADRA_ATTACK_RIGHT_JUMP = "attack_jump";
	public const string MADRA_ATTACK_RIGHT_IDLE = "attack_idle";
	public const string MADRA_DEACTIVE = "deactivated";
	public const string MADRA_BUMP_BACK = "bump back";
	public const string MADRA_BUMP_FRONT = "bump front";
	public const string MADRA_BUMP_SIDE = "bump";

	private GameObject _madraFront;
	private GameObject _madraSide;
	//*************************************************************//
	public const string CORA_IDLE = "idle";
	public const string CORA_IDLE_SPECIAL_01 = "idle_special1";
	public const string CORA_IDLE_SPECIAL_02 = "idle_special2";
	public const string CORA_ELECTROCUDED = "electrocuted";
	public const string CORA_SLIDE_UP = "slide_back";
	public const string CORA_SLIDE_UP_IMPACT = "slide_back_bump";
	public const string CORA_SLIDE_BACK_IDLE = "slide_back_idle";
	public const string CORA_SLIDE_DOWN = "slide_front";
	public const string CORA_SLIDE_DOWN_IMPACT = "slide_front_bump";
	public const string CORA_SLIDE_FRONT_IDLE = "slide_front_idle";
	public const string CORA_WALK_UP = "walk_back";
	public const string CORA_WALK_DOWN = "walk_front";
	public const string CORA_SLIDE_RIGHT_IDLE = "trolley_idle";
	public const string CORA_SLIDE_RIGHT_IMPACT = "trolley_stop";
	public const string CORA_SLIDE_RIGHT = "trolley_walk";
	public const string CORA_WALK_RIGHT = "cora_walk";
	public const string CORA_WALK_WITH_TROLLEY = "trolley_walk";
	public const string CORA_JUMP = "idle2";
	public const string CORA_DEACTIVE = "deactivated";
	public const string CORA_BOOT_UP = "deactivated_old";
	public const string CORA_TROLEY_SLIDE_UP = "slide_back";
	public const string CORA_TROLEY_SLIDE_UP_IMPACT = "slide_back_bump";

	private GameObject _coraFront;
	private GameObject _coraSide;
	private GameObject _coraTroley;
	//*************************************************************//
	public const string MINER01_DEACTIVE = "miner_01_deactive";
	public const string MINER02_DEACTIVE = "miner_02_deactive";
	public const string MINER03_DEACTIVE = "miner_03_deactive";
	public const string MINER04_DEACTIVE = "miner_04_deactive";
	public const string MINER05_DEACTIVE = "miner_05_deactive";
	public const string MINER06_DEACTIVE = "miner_06_deactive";

	public const string MINER01_JUMP = "miner_01_celebrate";
	public const string MINER02_JUMP = "miner_02_celebrate";
	public const string MINER03_JUMP = "miner_03_celebrate";
	public const string MINER04_JUMP = "miner_04_celebrate";
	public const string MINER05_JUMP = "miner_05_celebrate";
	public const string MINER06_JUMP = "miner_06_celebrate";

	public const string MINER01_IDLE = "miner_01_stand_idleanim";
	public const string MINER02_IDLE = "miner_02_stand_idleanim";
	public const string MINER03_IDLE = "miner_03_stand_idleanim";
	public const string MINER04_IDLE = "miner_04_stand_idleanim";
	public const string MINER05_IDLE = "miner_05_stand_idleanim";
	public const string MINER06_IDLE = "miner_06_stand_idleanim";

	private GameObject _minerFront;
	private GameObject _minerSide;
	//*************************************************************//
	public const string BOZ_DEACTIVE = "deactive";
	public const string BOZ_IDLE = "idle";
	public const string BOZ_JUMP = "jumping";
	public const string BOZ_ELECTROCUDED = "electrocuted";
	public const string BOZ_DRILL = "drillling";
	public const string BOZ_WALK_UP = "walk_back";
	public const string BOZ_WALK_DOWN = "walk_front";
	public const string BOZ_WALK_RIGHT = "walking";
	public const string BOZ_JUMP_TO_DRILL = "jump_to_drill";
	public const string BOZ_JUMP_TO_SQUISH = "jump_to_squish";
	public const string BOZ_BUMP_UP = "bump_back";
	public const string BOZ_BUMP_DOWN = "bump_front";
	public const string BOZ_BUMP_RIGHT = "bump";

	private GameObject _bozFront;
	private GameObject _bozSide;
	//*************************************************************//
	public const string JOSE_DEACTIVE = "deactive";
	public const string JOSE_IDLE = "Stand_idle_animation";
	public const string JOSE_JUMP = "Jump";

	private GameObject _joseFront;
	private GameObject _joseSide;
	//*************************************************************//
	private Material _myMaterial;
	private IComponent _myIComponent;
	private GameObject _troleyBackObject;
	private float _countTimeToNextFrame = 0f;
	private int _frameID = 0;
	private int _currentAnimationID = -1;
	private bool _playAnimation = false;
	private string _myPathToTextures = "";
	private Vector3 _initialScale;
	
	private List < Texture2D > _additionalTextures;
	private List < Texture2D > _currentAnimationTextures;

	private int[] _myAnimationIDs;
	private int _myFramePauseForIdle02;
	private int _myFramePauseForIdle03;
	private float _myTimePauseForIdle02;
	private float _myTimePauseForIdle03;
	private float _countTimeToSpecialIdle = 10f;
	private float _timeToPlayAnimation = -1f;
	
	private bool _playUntilChangeState = false;
	private bool _forcePlayAnimation = false;
	private int _countPlayingAnimation = 0;
	private bool _onlyOneTurn = false;

	private bool _blockAnimationForIntro = false;

	private bool _countTimeToStopSpecialIdle = false;
	private float _timeToStopSpecialIdle;
	//*************************************************************//	

	void Start ()
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();

		_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		
		_currentAnimationTextures = new List < Texture2D > ();
		_additionalTextures = new List < Texture2D > ();

		if ( _myIComponent.myID == GameElements.CHAR_FARADAYDO_1_IDLE || _myIComponent.myID == GameElements.CHAR_FARADAYDO_1_DRAINED  )
		{
			_faradaydoFront = transform.Find ( "front" ).gameObject;
			_faradaydoSide = transform.Find ( "side" ).gameObject;
		}
		else if ( _myIComponent.myID == GameElements.CHAR_MADRA_1_IDLE || _myIComponent.myID == GameElements.CHAR_MADRA_1_DRAINED  )
		{
			_madraFront = transform.Find ( "front" ).gameObject;
			_madraSide = transform.Find ( "side" ).gameObject;
		}
		else if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			_coraFront = transform.Find ( "front" ).gameObject;
			_coraSide = transform.Find ( "side" ).gameObject;
			if(resultScreenChar == false)
			{
				_coraTroley = transform.Find ( "trolley" ).gameObject;
			}
		}
		else if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS_DRAINED, _myIComponent.myID ) != -1 || Array.IndexOf ( GameElements.CHARACTERS_MINERS, _myIComponent.myID ) != -1 )
		{
			_minerFront = transform.Find ( "front" ).gameObject;
			_minerSide = transform.Find ( "side" ).gameObject;
		}
		else if ( _myIComponent.myID == GameElements.CHAR_BOZ_1_IDLE || _myIComponent.myID == GameElements.CHAR_BOZ_IDLE_DRAINED  )
		{
			_bozFront = transform.Find ( "front" ).gameObject;
			_bozSide = transform.Find ( "side" ).gameObject;
		}
		else if ( _myIComponent.myID == GameElements.CHAR_JOSE_IDLE_DRAINED  )
		{
//			print ("BLARP");
			_joseFront = transform.Find ( "front" ).gameObject;
			_joseSide = transform.Find ( "side" ).gameObject;
		}
		else 
		{
			if ( renderer ) _myMaterial = renderer.material;

			if ( transform.Find ( "troleyBack" ) != null ) _troleyBackObject = transform.Find ( "troleyBack" ).gameObject;
			//=============================Daves Work===============================
			_myAnimationIDs = new int[38];
			//=============================Daves Work===============================

			switch ( _myIComponent.myID )
			{
				case GameElements.CHAR_CORA_1_IDLE:
					_myPathToTextures = "Textures/Characters_v2/Cora";
					_myFramePauseForIdle02 = 8;
					_myFramePauseForIdle03 = 9;
					_myTimePauseForIdle02 = 1f;
					_myTimePauseForIdle03 = 0.0f;
					break;
				case GameElements.CHAR_FARADAYDO_1_DRAINED:
				case GameElements.CHAR_FARADAYDO_1_IDLE:
					_myFramePauseForIdle02 = 2;
					_myFramePauseForIdle03 = 2;
					_myTimePauseForIdle02 = 0.5f;
					_myTimePauseForIdle03 = 0.5f;
					_myPathToTextures = "Textures/Characters_v2/Faradaydo";
					break;
				case GameElements.CHAR_MADRA_1_DRAINED:
				case GameElements.CHAR_MADRA_1_IDLE:
					_myFramePauseForIdle02 = 3;
					_myFramePauseForIdle03 = 3;
					_myTimePauseForIdle02 = 1f;
					_myTimePauseForIdle03 = 1f;
					_myPathToTextures = "Textures/Characters_v2/Madra";
					break;
				case GameElements.CHAR_JOSE_IDLE_DRAINED:
				case GameElements.CHAR_JOSE_1_IDLE:
					_myPathToTextures = "Textures/Characters_v2/Jose";
					break;
				case GameElements.CHAR_BOZ_IDLE_DRAINED:
				case GameElements.CHAR_BOZ_1_IDLE:
					_myPathToTextures = "Textures/Characters_v2/Boz";
					break;
				case GameElements.CHAR_MINER_1_IDLE_DRAINED:
				case GameElements.CHAR_MINER_2_IDLE_DRAINED:
				case GameElements.CHAR_MINER_3_IDLE_DRAINED:
				case GameElements.CHAR_MINER_4_IDLE_DRAINED:
				case GameElements.CHAR_MINER_5_IDLE_DRAINED:
				case GameElements.CHAR_MINER_6_IDLE_DRAINED:
					_myFramePauseForIdle02 = 10;
					_myFramePauseForIdle03 = 10;
					_myTimePauseForIdle02 = 2f;
					_myTimePauseForIdle03 = 0.5f;
					_myPathToTextures = "Textures/Characters_v2/Miner";
					break;
			}

			if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
			{
				_myAnimationIDs[MOVE_RIGHT - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_RIGHT, _myPathToTextures + "/WalkRight" );
				_myAnimationIDs[INTERACT_RIGHT_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_RIGHT_ANIMATION, _myPathToTextures + "/InteractRight/0" );
				_myAnimationIDs[IDLE_01_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_01_ANIMATION, _myPathToTextures + "/idle/1" );
				_myAnimationIDs[IDLE_02_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_02_ANIMATION, _myPathToTextures + "/idle/2" );
				_myAnimationIDs[IDLE_03_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_03_ANIMATION, _myPathToTextures + "/idle/3" );
				_myAnimationIDs[ELECTROCUTED_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ELECTROCUTED_ANIMATION, _myPathToTextures + "/electrocuted" );
			}
			else
			{
				_myAnimationIDs[MOVE_RIGHT - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_RIGHT, _myPathToTextures + "/WalkRight" );
				_myAnimationIDs[MOVE_UP - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_UP, _myPathToTextures + "/WalkUp" );
				_myAnimationIDs[MOVE_DOWN - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_DOWN, _myPathToTextures + "/WalkDown" );
				_myAnimationIDs[INTERACT_RIGHT_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_RIGHT_ANIMATION, _myPathToTextures + "/InteractRight/0" );
				_myAnimationIDs[INTERACT_LEFT_ANIMATION_02 - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_LEFT_ANIMATION_02, _myPathToTextures + "/InteractLeft/1" );
				_myAnimationIDs[INTERACT_UP_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_UP_ANIMATION, _myPathToTextures + "/InteractUp/0" );
				_myAnimationIDs[INTERACT_DOWN_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_DOWN_ANIMATION, _myPathToTextures + "/InteractDown/0" );
				_myAnimationIDs[JUMP_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, JUMP_ANIMATION, _myPathToTextures + "/Jump" );
				_myAnimationIDs[IMPACT_RIGHT_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IMPACT_RIGHT_ANIMATION, _myPathToTextures + "/ImpactRight" );
				_myAnimationIDs[IMPACT_UP_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IMPACT_UP_ANIMATION, _myPathToTextures + "/ImpactUp" );
				_myAnimationIDs[IMPACT_DOWN_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IMPACT_DOWN_ANIMATION, _myPathToTextures + "/ImpactDown" );
				_myAnimationIDs[INTERACT_UP_TROLEY_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_UP_TROLEY_ANIMATION, _myPathToTextures + "/InteractUpTroley" );
				_myAnimationIDs[IMPACT_UP_TROLEY_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IMPACT_UP_TROLEY_ANIMATION, _myPathToTextures + "/ImpactUpTroley" );
				_myAnimationIDs[IDLE_01_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_01_ANIMATION, _myPathToTextures + "/idle/1" );
				_myAnimationIDs[IDLE_02_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_02_ANIMATION, _myPathToTextures + "/idle/2" );
				_myAnimationIDs[IDLE_03_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_03_ANIMATION, _myPathToTextures + "/idle/3" );
				_myAnimationIDs[INTERACT_01_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_01_ANIMATION, _myPathToTextures + "/interact_01" );
				_myAnimationIDs[INTERACT_02_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, INTERACT_02_ANIMATION, _myPathToTextures + "/interact_02" );
				_myAnimationIDs[ELECTROCUTED_ANIMATION - 10] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ELECTROCUTED_ANIMATION, _myPathToTextures + "/electrocuted" );
			}
		}

		if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS_DRAINED, _myIComponent.myID ) == -1 &&  _myIComponent.myID != GameElements.CHAR_JOSE_IDLE_DRAINED && _myIComponent.myID != GameElements.CHAR_BOZ_IDLE_DRAINED )
		{
			if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN || _myIComponent.myID != GameElements.CHAR_MADRA_1_IDLE ) playAnimation ( IDLE_01_ANIMATION );
		}

		if ( _myIComponent.myID == GameElements.CHAR_FARADAYDO_1_DRAINED )
		{
			playAnimation ( POWERLESS );
		}

		if ( _myIComponent.myID == GameElements.CHAR_MADRA_1_DRAINED )
		{
			playAnimation ( POWERLESS );
		}

		if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS_DRAINED, _myIComponent.myID ) != -1  )
		{
			playAnimation ( POWERLESS );
		}

		if ( _myIComponent.myID == GameElements.CHAR_BOZ_IDLE_DRAINED )
		{
			playAnimation ( POWERLESS );
		}
		if ( _myIComponent.myID == GameElements.CHAR_JOSE_IDLE_DRAINED )
		{
//			print ("Blarper");
			playAnimation ( POWERLESS );
		}
	}

	public void playAnimation ( int animationID, float timeToPlay = -1f, int direction = 1, bool onlyOneTurn = false )
	{
		_currentAnimationID = animationID;
		_timeToPlayAnimation = timeToPlay;
		_onlyOneTurn = onlyOneTurn;
		_countTimeToStopSpecialIdle = false;
		if ( _myIComponent.myID == GameElements.CHAR_MADRA_1_IDLE )
		{
			switch ( _currentAnimationID )
			{
			case ELECTROCUTED_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
				break;
			case INTERACT_01_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_JUMP;
				break;
			case ATTACK_IDLE_RIGHT_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_IDLE;
				break;
			case ATTACK_FRONT_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_FRONT_JUMP;
				break;
			case ATTACK_IDLE_FRONT_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_FRONT_IDLE;
				break;
			case ATTACK_BACK_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_BACK_JUMP;
				break;
			case ATTACK_IDLE_BACK_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_BACK_IDLE;
				break;
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				_currentAnimationID = -1;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				break;
			case IDLE_02_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 0.8f;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE_SPECIAL_01;
				break;
			case MOVE_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				
				if ( _madraSide.activeSelf && _madraSide.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_RIGHT )
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				else
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_JUMP;
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				break;
			case MOVE_LEFT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				if ( _madraSide.activeSelf && _madraSide.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_RIGHT )
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				else
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_JUMP;
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				break;
			case MOVE_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _madraFront.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_DOWN )
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_DOWN;
				}
				else
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_DOWN;
				}
				break;
			case MOVE_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _madraFront.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_UP )
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_UP;
				}
				else
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_UP;
				}
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_JUMP;
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_DEACTIVE;
				break;
			case BUMP_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_BUMP_BACK;
				break;
			case BUMP_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_BUMP_FRONT;
				break;
			case BUMP_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_BUMP_SIDE;
				break;
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_MADRA_1_DRAINED )
		{
			switch ( _currentAnimationID )
			{
			case ELECTROCUTED_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
				break;
			case INTERACT_01_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_JUMP;
				break;
			case ATTACK_IDLE_RIGHT_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_IDLE;
				break;
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				_currentAnimationID = -1;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
				break;
			case IDLE_02_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 0.8f;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE_SPECIAL_01;
				break;
			case MOVE_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				
				if ( _madraSide.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_RIGHT )
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				else
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_JUMP;
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				break;
			case MOVE_LEFT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				if ( _madraSide.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_RIGHT )
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				else
				{
					_madraFront.SetActive ( false );
					_madraSide.SetActive ( true );
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_ATTACK_RIGHT_JUMP;
					_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				}
				break;
			case MOVE_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _madraFront.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_DOWN )
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_DOWN;
				}
				else
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_DOWN;
				}
				break;
			case MOVE_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _madraFront.GetComponent < SkeletonAnimation > ().animationName == MADRA_WALK_UP )
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_UP;
				}
				else
				{
					_madraFront.SetActive ( true );
					_madraSide.SetActive ( false );
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
					_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_UP;
				}
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( true );
				_madraSide.SetActive ( false );
				_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_JUMP;
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_madraFront.SetActive ( false );
				_madraSide.SetActive ( true );
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_WALK_RIGHT;
				_madraSide.GetComponent < SkeletonAnimation > ().animationName = MADRA_DEACTIVE;
				break;
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
		{
			switch ( _currentAnimationID )
			{
			case ELECTROCUTED_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_ELECTROCUDED;
				break;
			case INTERACT_02_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( false );
				_faradaydoSide.SetActive ( true );
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BUILDING;
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
				break;
			case INTERACT_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( false );
				_faradaydoSide.SetActive ( true );
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BUILDING;
				break;
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				_currentAnimationID = -1;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_ELECTROCUDED;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				break;
			case IDLE_02_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 0.8f;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE_SPECIAL_01;
				break;
			case IDLE_03_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.5f;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE_SPECIAL_02;
				break;
			case MOVE_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );

				if ( _faradaydoSide.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_RIGHT )
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				else
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				break;
			case MOVE_LEFT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				if ( _faradaydoSide.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_RIGHT )
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				else
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				break;
			case MOVE_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _faradaydoFront.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_DOWN )
				{
					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_DOWN;
				}
				else
				{

					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_DOWN;
				}
				break;
			case MOVE_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _faradaydoFront.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_UP )
				{
					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_UP;
				}
				else
				{

					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_UP;
				}
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_JUMP;
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( false );
				_faradaydoSide.SetActive ( true );
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_DEACTIVE;
				break;
				//==========================Daves Work============================
			case IDLE_SCARED:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.SetActive ( true );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_SCARED;
				break;
				//==========================Daves Work============================
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_FARADAYDO_1_DRAINED )
		{
			switch ( _currentAnimationID )
			{
			case ELECTROCUTED_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_ELECTROCUDED;
				break;
			case INTERACT_02_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( false );
				_faradaydoSide.SetActive ( true );
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BUILDING;
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
				break;
			case INTERACT_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( false );
				_faradaydoSide.SetActive ( true );
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BUILDING;
				break;
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				_currentAnimationID = -1;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_ELECTROCUDED;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				break;
			case IDLE_02_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 0.8f;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE_SPECIAL_01;
				break;
			case IDLE_03_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.5f;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE_SPECIAL_02;
				break;
			case MOVE_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				
				if ( _faradaydoSide.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_RIGHT )
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				else
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				break;
			case MOVE_LEFT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				if ( _faradaydoSide.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_RIGHT )
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				else
				{
					_faradaydoFront.SetActive ( false );
					_faradaydoSide.SetActive ( true );
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
					_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_RIGHT;
				}
				break;
			case MOVE_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _faradaydoFront.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_DOWN )
				{
					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_DOWN;
				}
				else
				{
					
					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_DOWN;
				}
				break;
			case MOVE_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _faradaydoFront.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_MOVE_UP )
				{
					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_UP;
				}
				else
				{
					
					_faradaydoFront.SetActive ( true );
					_faradaydoSide.SetActive ( false );
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
					_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_MOVE_UP;
				}
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( true );
				_faradaydoSide.SetActive ( false );
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
				_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_JUMP;
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_faradaydoFront.SetActive ( false );
				_faradaydoSide.SetActive ( true );
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_BREAKING;
				_faradaydoSide.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_DEACTIVE;
				break;
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			switch ( _currentAnimationID )
			{
			case ELECTROCUTED_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.0f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_ELECTROCUDED;
				_coraTroley.SetActive ( false );
				break;
			case INTERACT_UP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_UP;
				_coraTroley.SetActive ( true );
				break;
			case INTERACT_DOWN_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_DOWN;
				_coraTroley.SetActive ( false );
				break;
			case INTERACT_RIGHT_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( false );
				_coraSide.SetActive ( true );
				_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_RIGHT;
				_coraTroley.SetActive ( false );
				break;
			case INTERACT_LEFT_ANIMATION_01:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( false );
				_coraSide.SetActive ( true );
				_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_RIGHT;
				_coraTroley.SetActive ( false );
				break;
			case INTERACT_LEFT_ANIMATION_02:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( false );
				_coraSide.SetActive ( true );
				_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_WITH_TROLLEY;
				_coraTroley.SetActive ( false );
				break;
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_IDLE;
				if(resultScreenChar == false)
				{
					_coraTroley.SetActive ( false );
				}
				break;
			case IDLE_02_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 0.8f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_IDLE_SPECIAL_01;
				if(resultScreenChar == false)
				{
					_coraTroley.SetActive ( false );
				}
				break;
			case IDLE_03_ANIMATION:
				_playAnimation = true;
				_countTimeToStopSpecialIdle = true;
				_timeToStopSpecialIdle = 1.5f;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_IDLE_SPECIAL_02;
				if(resultScreenChar == false)
				{
					_coraTroley.SetActive ( false );
				}
				break;
			case MOVE_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraTroley.SetActive ( false );
				if ( _coraSide.activeSelf && _coraSide.GetComponent < SkeletonAnimation > ().animationName == CORA_WALK_RIGHT )
				{
					_coraFront.SetActive ( false );
					_coraSide.SetActive ( true );
					_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_RIGHT;
				}
				else
				{
					_coraFront.SetActive ( false );
					_coraSide.SetActive ( true );
					_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_RIGHT;
				}
				break;
			case MOVE_LEFT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraTroley.SetActive ( false );
				if ( _coraSide.activeSelf &&  _coraSide.GetComponent < SkeletonAnimation > ().animationName == CORA_WALK_RIGHT )
				{
					_coraFront.SetActive ( false );
					_coraSide.SetActive ( true );
					_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_RIGHT;
				}
				else
				{
					_coraFront.SetActive ( false );
					_coraSide.SetActive ( true );
					_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_RIGHT;
				}
				break;
			case MOVE_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraTroley.SetActive ( false );
				if ( _coraFront.activeSelf && _coraFront.GetComponent < SkeletonAnimation > ().animationName == CORA_WALK_DOWN )
				{
					_coraFront.SetActive ( true );
					_coraSide.SetActive ( false );
					_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_DOWN;
				}
				else
				{
					_coraFront.SetActive ( true );
					_coraSide.SetActive ( false );
					_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_DOWN;
				}
				break;
			case MOVE_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraTroley.SetActive ( false );
				if ( _coraFront.activeSelf &&  _coraFront.GetComponent < SkeletonAnimation > ().animationName == CORA_WALK_UP )
				{
					_coraFront.SetActive ( true );
					_coraSide.SetActive ( false );
					_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_UP;
				}
				else
				{
					_coraFront.SetActive ( true );
					_coraSide.SetActive ( false );
					_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_UP;
				}
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_JUMP;
				_coraTroley.SetActive ( false );
				break;
			case IMPACT_UP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_UP_IMPACT;
				_coraTroley.SetActive ( true );
				break;
			case IMPACT_DOWN_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_DOWN_IMPACT;
				_coraTroley.SetActive ( false );
				break;
			case IMPACT_RIGHT_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( false );
				_coraSide.SetActive ( true );
				_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_RIGHT_IMPACT;
				_coraTroley.SetActive ( false );
				break;
			case IMPACT_LEFT_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( false );
				_coraSide.SetActive ( true );
				_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_RIGHT_IMPACT;
				_coraTroley.SetActive ( false );
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_IDLE;
				_coraFront.GetComponent<SkeletonAnimation> ().animationName = CORA_DEACTIVE;
				break;
			case POWER_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_coraFront.SetActive ( true );
				_coraSide.SetActive ( false );
				_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_DEACTIVE;
				_coraFront.GetComponent<SkeletonAnimation> ().animationName = CORA_BOOT_UP;
				break;
			}
		}
		else if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS_DRAINED, _myIComponent.myID ) != -1 )
		{
			switch ( _currentAnimationID )
			{
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_minerFront.SetActive ( true );
				_minerSide.SetActive ( false );
				
				switch ( _myIComponent.myID )
				{
				case GameElements.CHAR_MINER_1_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER01_IDLE;
					break;
				case GameElements.CHAR_MINER_2_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER02_IDLE;
					break;
				case GameElements.CHAR_MINER_3_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER03_IDLE;
					break;
				case GameElements.CHAR_MINER_4_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER04_IDLE;
					break;
				case GameElements.CHAR_MINER_5_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER05_IDLE;
					break;
				case GameElements.CHAR_MINER_6_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER06_IDLE;
					break;
				}
				
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_minerFront.SetActive ( true );
				_minerSide.SetActive ( false );

				switch ( _myIComponent.myID )
				{
				case GameElements.CHAR_MINER_1_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER01_JUMP;
					break;
				case GameElements.CHAR_MINER_2_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER02_JUMP;
					break;
				case GameElements.CHAR_MINER_3_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER03_JUMP;
					break;
				case GameElements.CHAR_MINER_4_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER04_JUMP;
					break;
				case GameElements.CHAR_MINER_5_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER05_JUMP;
					break;
				case GameElements.CHAR_MINER_6_IDLE_DRAINED:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER06_JUMP;
					break;
				}

				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_minerFront.SetActive ( false );
				_minerSide.SetActive ( true );

				switch ( _myIComponent.myID )
				{
				case GameElements.CHAR_MINER_1_IDLE_DRAINED:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER01_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_2_IDLE_DRAINED:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER02_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_3_IDLE_DRAINED:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER03_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_4_IDLE_DRAINED:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER04_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_5_IDLE_DRAINED:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER05_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_6_IDLE_DRAINED:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER06_DEACTIVE;
					break;
				}
				break;
			}
		}
		else if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS, _myIComponent.myID ) != -1 )
		{
			switch ( _currentAnimationID )
			{
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_minerFront.SetActive ( true );
				_minerSide.SetActive ( false );
				
				switch ( _myIComponent.myID )
				{
				case GameElements.CHAR_MINER_1_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER01_IDLE;
					break;
				case GameElements.CHAR_MINER_2_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER02_IDLE;
					break;
				case GameElements.CHAR_MINER_3_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER03_IDLE;
					break;
				case GameElements.CHAR_MINER_4_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER04_IDLE;
					break;
				case GameElements.CHAR_MINER_5_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER05_IDLE;
					break;
				case GameElements.CHAR_MINER_6_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER06_IDLE;
					break;
				}
				
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_minerFront.SetActive ( true );
				_minerSide.SetActive ( false );
				
				switch ( _myIComponent.myID )
				{
				case GameElements.CHAR_MINER_1_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER01_JUMP;
					break;
				case GameElements.CHAR_MINER_2_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER02_JUMP;
					break;
				case GameElements.CHAR_MINER_3_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER03_JUMP;
					break;
				case GameElements.CHAR_MINER_4_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER04_JUMP;
					break;
				case GameElements.CHAR_MINER_5_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER05_JUMP;
					break;
				case GameElements.CHAR_MINER_6_IDLE:
					_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER06_JUMP;
					break;
				}
				
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_minerFront.SetActive ( false );
				_minerSide.SetActive ( true );
				
				switch ( _myIComponent.myID )
				{
				case GameElements.CHAR_MINER_1_IDLE:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER01_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_2_IDLE:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER02_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_3_IDLE:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER03_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_4_IDLE:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER04_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_5_IDLE:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER05_DEACTIVE;
					break;
				case GameElements.CHAR_MINER_6_IDLE:
					_minerSide.GetComponent < SkeletonAnimation > ().animationName = MINER06_DEACTIVE;
					break;
				}
				break;
			}
		}
		else if(_myIComponent.myID == GameElements.CHAR_JOSE_IDLE_DRAINED)
		{
			switch(_currentAnimationID)
			{
			case POWERLESS:
//				print ("BLARPYBLARP");
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_joseFront.SetActive ( false );
				_joseSide.SetActive ( true );
				_joseSide.GetComponent < SkeletonAnimation > ().animationName = JOSE_DEACTIVE;
			break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_joseFront.SetActive ( true );
				_joseSide.SetActive ( false );
				_joseFront.GetComponent < SkeletonAnimation > ().animationName = JOSE_JUMP;
				break;
			}
		}
		else if(_myIComponent.myID == GameElements.CHAR_JOSE_1_IDLE)
		{
			switch(_currentAnimationID)
			{
			case CELEBRATION:
				print ("HappYBLARP");
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_joseFront.SetActive ( true );
				_joseSide.SetActive ( false );
				_joseFront.GetComponent < SkeletonAnimation > ().animationName = JOSE_IDLE;
				_joseFront.GetComponent < SkeletonAnimation > ().animationName = JOSE_JUMP;
				break;
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_BOZ_IDLE_DRAINED )
		{
			switch ( _currentAnimationID )
			{
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_IDLE;
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_JUMP;
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_DEACTIVE;
				break;
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			switch ( _currentAnimationID )
			{
			case BUMP_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_BUMP_UP;
				break;
			case BUMP_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_BUMP_DOWN;
				break;
			case BUMP_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_BUMP_RIGHT;
				break;
			case JUMP_TO_SQUISH:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_JUMP_TO_SQUISH;
				break;
			case JUMP_TO_DRILL:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_JUMP_TO_DRILL;
				break;
			case IDLE_01_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_IDLE;
				break;
			case JUMP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_JUMP;
				break;
			case POWERLESS:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_DEACTIVE;
				break;

			case ELECTROCUTED_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_ELECTROCUDED;
				break;
			case INTERACT_UP_ANIMATION:
				_playAnimation = true;
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( true );
				_bozSide.SetActive ( false );
				_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_DRILL;
				break;
			case MOVE_RIGHT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_WALK_RIGHT;
				break;
			case MOVE_LEFT:
				_playAnimation = true;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_bozFront.SetActive ( false );
				_bozSide.SetActive ( true );
				_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_WALK_RIGHT;
				break;
			case MOVE_DOWN:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _bozFront.activeSelf && _bozFront.GetComponent < SkeletonAnimation > ().animationName == BOZ_WALK_DOWN )
				{
					_bozFront.SetActive ( true );
					_bozSide.SetActive ( false );
					_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_WALK_DOWN;
				}
				else
				{
					_bozFront.SetActive ( true );
					_bozSide.SetActive ( false );
					_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_WALK_DOWN;
				}
				break;
			case MOVE_UP:
				_playAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				if ( _bozFront.activeSelf && _bozFront.GetComponent < SkeletonAnimation > ().animationName == BOZ_WALK_UP )
				{
					_bozFront.SetActive ( true );
					_bozSide.SetActive ( false );
					_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_WALK_UP;
				}
				else
				{
					_bozFront.SetActive ( true );
					_bozSide.SetActive ( false );
					_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_WALK_UP;
				}
				break;
			}
		}
		else
		{
			switch ( _currentAnimationID )
			{
			case ELECTROCUTED_ANIMATION:
				transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
				_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ELECTROCUTED_ANIMATION - 10]];
				if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( DOWN );
					}
					else 
					{
						if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.LABORATORY && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN )
						{
							if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
							{
								if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
							}
						}
					
						_frameID = 0;
						_playAnimation = true;
						_countTimeToNextFrame = 0f; // For Madra attack sequence
					}
					break;
				case INTERACT_02_ANIMATION:
					transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_02_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_RIGHT );
					}
					else 
					{
						if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.LABORATORY && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN )
						{
							if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
							{
								if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
							}
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case INTERACT_01_ANIMATION:
					transform.localScale = new Vector3 ( direction > 0 ? _initialScale.x : -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_01_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_RIGHT );
					}
					else 
					{
						if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.LABORATORY && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN )
						{
							if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
							{
								if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
							}
						}
					
						_frameID = 0;
						_playAnimation = true;
						_countTimeToNextFrame = 0f; // For Madra attack sequence
					}
					break;
				case IDLE_01_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_01_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( DOWN );
					}
					else 
					{
						if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.LABORATORY && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN )
						{
							if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
							{
								if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
							}
						}
					
						_frameID = 0;
						_playAnimation = true;
						_countTimeToNextFrame = 0f; // For Madra attack sequence
					}
					break;
				case IDLE_02_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_02_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( DOWN );
					}
					else 
					{
						if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.LABORATORY && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN )
						{
							if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
							{
								if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
							}
						}
						
						_frameID = 0;
						_playAnimation = true;
						_countTimeToNextFrame = 0f; // For Madra attack sequence
					}
					break;
				case IDLE_03_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_03_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( DOWN );
					}
					else 
					{
						if ( GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.LABORATORY && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.TRAIN )
						{
							if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
							{
								if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
							}
						}
						
						_frameID = 0;
						_playAnimation = true;
						_countTimeToNextFrame = 0f; // For Madra attack sequence
					}
					break;
				case MOVE_RIGHT:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_RIGHT - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( RIGHT );
					}
					else 
					{
						_countTimeToNextFrame = 0f;
					
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_playAnimation = true;
					}
					break;
				case MOVE_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_RIGHT - 10]];
					
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( LEFT );
					}
					else 
					{
						_countTimeToNextFrame = 0f;
					
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_playAnimation = true;
					}
					break;
				case MOVE_DOWN:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_DOWN - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( DOWN );
					}
					else 
					{
						_countTimeToNextFrame = 0f;
					
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_playAnimation = true;
					}
					break;
				case MOVE_UP:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_UP - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( UP );
					}
					else 
					{
						_countTimeToNextFrame = 0f;
					
					 	if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_playAnimation = true;
					}
					break;
				case INTERACT_RIGHT_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_RIGHT_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_RIGHT );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case INTERACT_LEFT_ANIMATION_01:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_RIGHT_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_LEFT );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case INTERACT_LEFT_ANIMATION_02:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_LEFT_ANIMATION_02 - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_LEFT );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
						_playUntilChangeState = true;
					}
					break;
				case INTERACT_UP_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_UP_ANIMATION - 10]];
					if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
					{
						_additionalTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_UP_TROLEY_ANIMATION - 10]];
					}
				
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_UP );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = true;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case INTERACT_DOWN_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[INTERACT_DOWN_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_DOWN );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
						
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case JUMP_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[JUMP_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( CELEBRATION );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case IMPACT_RIGHT_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IMPACT_RIGHT_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_RIGHT );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case IMPACT_LEFT_ANIMATION:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IMPACT_RIGHT_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_LEFT );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case IMPACT_UP_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IMPACT_UP_ANIMATION - 10]];
					if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
					{
						_additionalTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IMPACT_UP_TROLEY_ANIMATION - 10]];
					}
				
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_UP );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = true;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
				case IMPACT_DOWN_ANIMATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IMPACT_DOWN_ANIMATION - 10]];
					if ( _currentAnimationTextures.Count == 0 )
					{
						changeState ( INTERACT_DOWN );
					}
					else 
					{
						if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
						}
					
						_frameID = 0;
						_playAnimation = true;
					}
					break;
			}
		}
	}
	
	public void reloadTextures ()
	{
		Start ();
	}
	
	public void changeState ( int state ) 
	{
		_playAnimation = false;
		_playUntilChangeState = false;

		_myIComponent = gameObject.GetComponent < IComponent > ();

		if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			_coraFront = transform.Find ( "front" ).gameObject;
			_coraSide = transform.Find ( "side" ).gameObject;
			_coraTroley = transform.Find ( "trolley" ).gameObject;
		}

		switch ( _myIComponent.myID )
		{
			case GameElements.CHAR_CORA_1_IDLE:
				CoraMarkerScale scaleControl;
				switch ( state )
				{
					case UP:
					case DOWN:
					case RIGHT:
					case LEFT:
						_playAnimation = true;
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_coraFront.SetActive ( true );
						_coraSide.SetActive ( false );
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_DOWN;
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_IDLE;
						_coraTroley.SetActive ( false );
						break;
					case INTERACT_UP:
						_playAnimation = true;
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_coraFront.SetActive ( true );
						_coraSide.SetActive ( false );
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_DOWN;
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_BACK_IDLE;
						_coraTroley.SetActive ( true );
						
						break;
					case INTERACT_DOWN:
						_playAnimation = true;
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_coraFront.SetActive ( true );
						_coraSide.SetActive ( false );
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_DOWN;
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_FRONT_IDLE;
						_coraTroley.SetActive ( false );
				
						break;
					case INTERACT_RIGHT:
						_playAnimation = true;
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_coraFront.SetActive ( false );
						_coraSide.SetActive ( true );
						//print ("call 23");
						_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_RIGHT;
						_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_RIGHT_IDLE;
						_coraTroley.SetActive ( false );
				
						break;
					case INTERACT_LEFT:
						_playAnimation = true;
						transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
						_coraFront.SetActive ( false );
						_coraSide.SetActive ( true );
						//print ("call 24");
						_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_WALK_RIGHT;
						_coraSide.GetComponent < SkeletonAnimation > ().animationName = CORA_SLIDE_RIGHT_IDLE;
						_coraTroley.SetActive ( false );
				
						break;
					case CELEBRATION:
						_playAnimation = true;
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_coraFront.SetActive ( true );
						_coraSide.SetActive ( false );
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_IDLE;
						_coraFront.GetComponent < SkeletonAnimation > ().animationName = CORA_JUMP;
						_coraTroley.SetActive ( false );
						break;
					case POWERLESS:
						_playAnimation = false;
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_cora_idle_drained" );
						break;
				}
				break;
			case GameElements.CHAR_FARADAYDO_1_DRAINED:
			case GameElements.CHAR_FARADAYDO_1_IDLE:
				if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.INTRO )
				{
					switch ( state )
					{
						case RIGHT:
							_blockAnimationForIntro = true;
							transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
							if ( _myMaterial ) _myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Faradaydo/WalkRight/char_faradaydo_walkright_0003" );
							break;
						case LEFT:
							_blockAnimationForIntro = true;
							transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
							if ( _myMaterial ) _myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Faradaydo/WalkRight/char_faradaydo_walkright_0003" );
							break;
					}
				}
				else
				{
					switch ( state )
					{
						case UP:
						case DOWN:
						case RIGHT:
						case LEFT:
							transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
							_faradaydoFront.SetActive ( true );
							_faradaydoSide.SetActive ( false );
							_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_ELECTROCUDED;
							_faradaydoFront.GetComponent < SkeletonAnimation > ().animationName = FARADAYDO_IDLE;
							break;
						case INTERACT_RIGHT:
							transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
							if ( _myMaterial ) _myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_faradaydo_buildright" );
							break;
						case INTERACT_LEFT:
							transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
							if ( _myMaterial ) _myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_faradaydo_buildright" );
							break;
						case CELEBRATION:
							transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
							if ( _myMaterial ) _myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_faradaydo_celebrate_01" );
							break;
						case POWERLESS:
							transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
							if ( _myMaterial ) _myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_faradaydo_idle_drainedright" );
							break;
					}
					
				}
				break;
			case GameElements.CHAR_MADRA_1_DRAINED:
			case GameElements.CHAR_MADRA_1_IDLE:
				switch ( state )
				{
					case UP:
					case DOWN:
					case RIGHT:
					case LEFT:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_madraFront.SetActive ( true );
						_madraSide.SetActive ( false );
						_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_ELECTROCUDED;
						_madraFront.GetComponent < SkeletonAnimation > ().animationName = MADRA_IDLE;
						break;
					case INTERACT_RIGHT:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_madra_attackright" );
						break;
					case INTERACT_LEFT:
						transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_madra_attackright" );
						break;
					case CELEBRATION:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_madra_1_idle" );
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_madra_idle_drained" );
						break;
				}
				break;
			case GameElements.CHAR_MINER_1_IDLE_DRAINED:
				switch ( state )
				{
					case DOWN:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER01_IDLE;
						break;
					case CELEBRATION:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER01_JUMP;
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Miner/Static/char_miner_1_idle_drainedright" );
						break;
				}
				break;
			case GameElements.CHAR_MINER_2_IDLE_DRAINED:
				switch ( state )
				{
					case DOWN:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER02_IDLE;
						break;
					case CELEBRATION:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER02_JUMP;
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Miner/Static/char_miner_2_idle_drainedright" );
						break;
				}
				break;
			case GameElements.CHAR_MINER_3_IDLE_DRAINED:
				switch ( state )
				{
					case DOWN:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER03_IDLE;
						break;
					case CELEBRATION:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER03_JUMP;
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Miner/Static/char_miner_3_idle_drainedright" );
						break;
				}
				break;
			case GameElements.CHAR_MINER_4_IDLE_DRAINED:
				switch ( state )
				{
					case DOWN:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER04_IDLE;
						break;
					case CELEBRATION:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER04_JUMP;
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Miner/Static/char_miner_4_idle_drainedright" );
						break;
				}
				break;
			case GameElements.CHAR_MINER_5_IDLE_DRAINED:
				switch ( state )
				{
					case DOWN:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER05_IDLE;
						break;
					case CELEBRATION:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER05_JUMP;
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Miner/Static/char_miner_5_idle_drainedright" );
						break;
				}
				break;
			case GameElements.CHAR_MINER_6_IDLE_DRAINED:
				switch ( state )
				{
					case DOWN:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER06_IDLE;
						break;
					case CELEBRATION:
						_minerFront.SetActive ( true );
						_minerSide.SetActive ( false );
						_minerFront.GetComponent < SkeletonAnimation > ().animationName = MINER06_JUMP;
						break;
					case POWERLESS:
						transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
						_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Miner/Static/char_miner_6_idle_drainedright" );
						break;
				}
				break;
			case GameElements.CHAR_JOSE_IDLE_DRAINED:
			case GameElements.CHAR_JOSE_1_IDLE:
				/*switch ( state )
				{
				case UP:
				case DOWN:
				case RIGHT:
				case LEFT:
					transform.localScale = new Vector3 ( _initialScale.x * 0.75f, _initialScale.y* 0.75f, _initialScale.z * 0.75f );
					_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Jose/jose_front" );
					break;
				case INTERACT_RIGHT:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_faradaydo_buildright" );
					break;
				case INTERACT_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_faradaydo_buildright" );
					break;
				case CELEBRATION:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Jose/jose_jump" );
					break;
				case POWERLESS:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_myMaterial.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/Jose/jose_fall" );
					break;
				}
				break;*/
			case GameElements.CHAR_BOZ_1_IDLE:
			case GameElements.CHAR_BOZ_IDLE_DRAINED:
				/*switch ( state )
				{
				case UP:
				case DOWN:
				case RIGHT:
				case LEFT:
					_bozFront.SetActive ( true );
					_bozSide.SetActive ( false );
					_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_IDLE;
					break;
				case CELEBRATION:
					_bozFront.SetActive ( true );
					_bozSide.SetActive ( false );
					_bozFront.GetComponent < SkeletonAnimation > ().animationName = BOZ_JUMP;
					break;
				case POWERLESS:
					_bozFront.SetActive ( false );
					_bozSide.SetActive ( true );
					_bozSide.GetComponent < SkeletonAnimation > ().animationName = BOZ_DEACTIVE;
					break;
				}*/
				break;
		}
		
		if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			if ( state == INTERACT_UP )
			{
				if ( _troleyBackObject ) 
				{
					_troleyBackObject.renderer.enabled = true;
					_troleyBackObject.transform.localPosition = new Vector3 ( 0f, -0.2f, 0f );
					_troleyBackObject.renderer.material.mainTexture = ( Texture2D ) Resources.Load ( "Textures/Characters_v2/char_cora_1_trolleyup_back" );
				}
			}
			else
			{
				if ( _troleyBackObject ) _troleyBackObject.renderer.enabled = false;
			}
		}
	}

	public void unblockForIntro ()
	{
		_blockAnimationForIntro = false;
	}
	
	void Update () 
	{
		////if(GetComponent < >)
		if ( ! _playAnimation ) return;
		_countTimeToNextFrame += Time.deltaTime;

		if ( _countTimeToStopSpecialIdle )
		{
			_timeToStopSpecialIdle -= Time.deltaTime;

			if ( _timeToStopSpecialIdle <= 0f )
			{
				_countTimeToStopSpecialIdle = false;
				playAnimation ( IDLE_01_ANIMATION );
			}
		}

		if ( _currentAnimationID == -1 )
		{
			_countTimeToSpecialIdle -= Time.deltaTime;
		}
		else
		{
			_timeToPlayAnimation -= Time.deltaTime;
		}

		if ( _myIComponent.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
		{
			if ( _faradaydoFront.activeSelf && _faradaydoFront.GetComponent < SkeletonAnimation > ().animationName == FARADAYDO_IDLE )
			{
				_countTimeToSpecialIdle -= Time.deltaTime;
			}

			if ( _countTimeToSpecialIdle <= 0f && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.INTRO )
			{
				_countTimeToSpecialIdle = 10f;
				
				if ( UnityEngine.Random.Range ( 0, 2 ) == 0 )
				{
					playAnimation ( IDLE_02_ANIMATION );
				}
				else
				{
					playAnimation ( IDLE_03_ANIMATION );
				}
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			if ( _coraFront.activeSelf && _coraFront.GetComponent < SkeletonAnimation > ().animationName == CORA_IDLE )
			{
				_countTimeToSpecialIdle -= Time.deltaTime;
			}
			
			if ( _countTimeToSpecialIdle <= 0f && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.INTRO )
			{
				_countTimeToSpecialIdle = 10f;
				
				if ( UnityEngine.Random.Range ( 0, 2 ) == 0 )
				{
					playAnimation ( IDLE_02_ANIMATION );
				}
				else
				{
					playAnimation ( IDLE_03_ANIMATION );
				}
			}
		}
		else if ( _myIComponent.myID == GameElements.CHAR_MADRA_1_IDLE )
		{
			if ( _madraFront.activeSelf && _madraFront.GetComponent < SkeletonAnimation > ().animationName == MADRA_IDLE )
			{
				_countTimeToSpecialIdle -= Time.deltaTime;
			}
			
			if ( _countTimeToSpecialIdle <= 0f && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.INTRO )
			{
				_countTimeToSpecialIdle = 10f;
				playAnimation ( IDLE_02_ANIMATION );
			}
		}
		else if ( Array.IndexOf ( GameElements.CHARACTERS_MINERS, _myIComponent.myID ) != -1 )
		{
		}
		else if ( _myIComponent.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
		}
		else
		{
			if ( _countTimeToNextFrame >= ( 1f / 24f ))
			{
				_countTimeToNextFrame = 0f;
				
				if ( ! _blockAnimationForIntro && _currentAnimationID == -1 && ( Array.IndexOf ( GameElements.DRAINED_CHARACTERS, _myIComponent.myID ) == -1 || _forcePlayAnimation ))
				{
					_frameID++;				
					
					if ( _frameID >= _currentAnimationTextures.Count ) _frameID = 0;
					if ( _currentAnimationTextures.Count > 0 ) _myMaterial.mainTexture = _currentAnimationTextures[_frameID];
					
					if ( _countTimeToSpecialIdle <= 0f && GameGlobalVariables.CURRENT_GAME_PART != GameGlobalVariables.INTRO )
					{
						_countTimeToSpecialIdle = 10f;
						_frameID = 0;
						
						if ( UnityEngine.Random.Range ( 0, 2 ) == 0 )
						{
							_currentAnimationID = IDLE_02_ANIMATION;
							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_02_ANIMATION - 10]];
						}
						else
						{
							_currentAnimationID = IDLE_03_ANIMATION;
							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_03_ANIMATION - 10]];
							_countPlayingAnimation = 1;
						}
					}
				}
				else if ( ! _blockAnimationForIntro && Array.IndexOf ( GameElements.DRAINED_CHARACTERS, _myIComponent.myID ) == -1 || _forcePlayAnimation )
				{
					if ( _frameID >= _currentAnimationTextures.Count )
					{	
						if ( _timeToPlayAnimation > 0f || _playUntilChangeState || _currentAnimationID == MOVE_UP || _currentAnimationID == MOVE_DOWN || _currentAnimationID == MOVE_RIGHT || _currentAnimationID == MOVE_LEFT )
						{
							
						}
						else
						{
							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_01_ANIMATION - 10]];
							_currentAnimationID = -1;
						}
						
						_frameID = 0;
					}
					
					if ( _currentAnimationTextures.Count > 0 ) _myMaterial.mainTexture = _currentAnimationTextures[_frameID];
					
					if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
					{
						if ( _currentAnimationID == INTERACT_UP_ANIMATION )
						{
							_troleyBackObject.transform.localPosition = new Vector3 ( 0f, -0.2f, 0f );
							_troleyBackObject.renderer.material.mainTexture = _additionalTextures[_frameID];
						}
						else if ( _currentAnimationID == IMPACT_UP_ANIMATION )
						{
							_troleyBackObject.transform.localPosition = new Vector3 ( 0f, -0.2f, 0f );
							_troleyBackObject.renderer.material.mainTexture = _additionalTextures[_frameID];
						}				
					}
					
					_frameID++;
					
					if ( _myIComponent.myID == GameElements.CHAR_CORA_1_IDLE )
					{
						if ( _currentAnimationID == IDLE_03_ANIMATION )
						{
							if ( _frameID >= _currentAnimationTextures.Count )
							{
								if ( _countPlayingAnimation == 1 )
								{
									_frameID = 0;
									_countPlayingAnimation++;
								}
							}
						}
					}
					
					if ( _myIComponent.myID == GameElements.CHAR_MADRA_1_IDLE )
					{
						if ( _currentAnimationID == INTERACT_01_ANIMATION )
						{
							if ( _frameID >= _currentAnimationTextures.Count )
							{
								if ( ! _onlyOneTurn ) _countTimeToNextFrame = -100f;
								else playAnimation ( IDLE_01_ANIMATION );
							}
						}
					}
					
					if ( _currentAnimationID == IDLE_02_ANIMATION && _frameID == _myFramePauseForIdle02 ) _countTimeToNextFrame = -_myTimePauseForIdle02;
					if ( _currentAnimationID == IDLE_03_ANIMATION && _frameID == _myFramePauseForIdle03 ) _countTimeToNextFrame = -_myTimePauseForIdle03;
					
					if ( _frameID >= _currentAnimationTextures.Count )
					{	
						if ( _timeToPlayAnimation > 0f || _playUntilChangeState || _currentAnimationID == MOVE_UP || _currentAnimationID == MOVE_DOWN || _currentAnimationID == MOVE_RIGHT || _currentAnimationID == MOVE_LEFT )
						{
							
						}
						else
						{

							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_01_ANIMATION - 10]];
							_currentAnimationID = -1;
						}
						
						_frameID = 0;
					}
				}
			}
		}
	}
}
