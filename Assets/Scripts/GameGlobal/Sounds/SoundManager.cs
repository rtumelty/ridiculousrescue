using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	//*************************************************************//
	public const int MISSION_MUSIC = 0;
	public const int MAP_MUSIC = 1;
	public const int MINING_MUSIC = 2;
	//*************************************************************//
	public static bool MUSIC_ON
	{
		set
		{
			SoundManager.getInstance ().turnMusicOnOff ( value );
			_MUSIC_ON = value;
		}
		get
		{
			return _MUSIC_ON;
		}
	}
#if UNITY_EDITOR
	private static bool _MUSIC_ON = true;
#else
	private static bool _MUSIC_ON = true;
#endif
	public static bool SFX_ON = true;
	//*************************************************************//
	public const int TENTACLE_DRAINER_ATTACK = 0;
	public const int CHARACTER_ATTACK = 1;
	public const int CHARACTER_ATTACK_FAIL = 2;
	public const int CHARACTER_TROLEY_MOVE = 3;
	public const int DESTROYABLE_DEMOLISH = 4;
	public const int REDIRECTOR_BUILD = 5;
	public const int CONFIRM_BUTTON = 6;
	public const int CANCEL_BUTTON = 7;
	public const int REDIRECTOR_POWER_UP = 8;
	public const int POWER_PLUG_CONNECTED = 9;
	public const int REDIRECTOR_ROTATE = 10;
	public const int OBJECT_SELECT = 11;
	public const int HEADER_TAP = 12;
	public const int RESCUE_TOY_AT_MARKER = 13;
	public const int MISSION_COMPLETE = 14;
	public const int MISSION_FAILED = 15;
	public const int TENTACLE_DRAINER_DEFATED = 16;
	public const int BUM = 17;
	public const int ELECTROCUTED = 18;
	public const int LEVEL_NODE_UNLOCKED = 19;
	public const int X_MARK = 20;
	public const int BUTTONS_APAERE = 21;
	public const int STAR_01 = 22;
	public const int STAR_02 = 23;
	public const int STAR_03 = 24;
	public const int LOCKING_MECH = 25;
	public const int MACHINE_CLOSES_PLUS_LOCK = 26;
	public const int MACHINE_CLOSES = 27;
	public const int MACHINE_OPENS = 28;
	public const int MACHINE_OPENS_END = 29;
	public const int TOY_DEPOSITED = 30;
	public const int CHCH = 31;
	public const int CHARACTER_TROLEY_MOVE2 = 32;

	public const int BAT_DEFEAT = 40;
	public const int BAT_MOUNT = 41;
	public const int BAT_SWOOP = 42;
	public const int BOZ_DRILL = 43;
	public const int BOZ_FAIL = 44;
	public const int BOZ_WIN = 45;
	public const int CORA_CLAP = 46;
	public const int CORA_MISS = 47;
	public const int CORA_SELECT1 = 48;
	public const int CORA_SELECT2 = 49;
	public const int CORA_SWIPE_VERB = 50;
	public const int TENTACLE_DRAG = 51;
	public const int TENTACLE_POPUP = 52;
	public const int TRAIN_HORN = 53;
	public const int TRAIN_HORN_SHORT = 54;
	public const int TRAIN_LOOP = 55;
	public const int TRAIN_STRUGGLE = 56;
	public const int MADRA_BITE = 57;
	public const int MADRA_GROWL = 58;
	public const int BAT_FLAP = 59;
	public const int BAT_BITE = 60;
	public const int SLUG_GET_READY = 61;
	public const int TENTACLE_GET_READY = 62;
	public const int SLUG_DEFEAT = 63;
	public const int BOZ_JUMP_TO_DRILL = 64;
	public const int BOZ_DRILLING = 65;
	public const int BOZ_DESTROYED_ROCK = 66;
	public const int SCORE_PLINK = 67;
	//*************************************************************//
	private const int SOUND_ID_TENTACLE_DRAINER_ATTACK_01 = 0;
	private const int SOUND_ID_MADRA_ATTACK_01 = 1;
	private const int SOUND_ID_MADRA_ATTACK_02 = 2;
	private const int SOUND_ID_MADRA_ATTACK_FAIL_01 = 3;
	private const int SOUND_ID_MADRA_ATTACK_FAIL_02 = 4;
	private const int SOUND_ID_CHARACTER_TROLEY_MOVE_01 = 5;
	private const int SOUND_ID_CRACKED_ROCK_01_DEMOLISH_01 = 6;
	private const int SOUND_ID_REDIRECTOR_BUILD_01 = 7;
	private const int SOUND_ID_CONFRIM_BUTTON_01 = 8;
	private const int SOUND_ID_CANCEL_BUTTON_01 = 9;
	private const int SOUND_ID_REDIRECTOR_POWER_UP_01 = 10;
	private const int SOUND_ID_POWER_PLUG_CONNECTED_01 = 11;
	private const int SOUND_ID_REDIRECTOR_ROTATE_01 = 12;
	private const int SOUND_ID_OBJECT_SELECT_01 = 13;
	private const int SOUND_ID_CORA_SELECT_01 = 14;
	private const int SOUND_ID_MADRA_SELECT_01 = 15;
	private const int SOUND_ID_FARADAYDO_SELECT_01 = 16;
	private const int SOUND_ID_HEADER_TAP_01 = 17;
	private const int SOUND_ID_RESCUE_TOY_AT_MARKER_01 = 18;
	private const int SOUND_ID_MISSION_COMPLETE_01 = 19;
	private const int SOUND_ID_MISSION_FAILED_01 = 20;
	private const int SOUND_ID_TENTACLE_DRAINER_DEFATED_01 = 21;
	private const int SOUND_ID_BUM = 22;
	private const int SOUND_ID_ELECTROCUTED = 23;
	private const int SOUND_ID_LEVEL_NODE_UNLOCKED = 24;
	private const int SOUND_ID_X_MARK = 25;
	private const int SOUND_ID_BUTTONS_APAERE = 26;
	private const int SOUND_ID_STAR_01_APAERE = 27;
	private const int SOUND_ID_STAR_02_APAERE = 28;
	private const int SOUND_ID_STAR_03_APAERE = 29;
	private const int SOUND_ID_TR_LOCKING_MECH = 30;
	private const int SOUND_ID_TR_CLOSES_PLUS_LOCK = 31;
	private const int SOUND_ID_TR_MACHINE_CLOSES = 32;
	private const int SOUND_ID_TR_MACHINE_OPENS = 33;
	private const int SOUND_ID_TR_MACHINE_OPENS_END = 34;
	private const int SOUND_ID_TR_TOY_DEPOSITED = 35;
	private const int SOUND_ID_CHCH = 36;
	private const int SOUND_ID_CHARACTER_TROLEY_MOVE_02 = 37;

	private const int SOUND_ID_BAT_DEFEAT = 40;
	private const int SOUND_ID_BAT_MOUNT = 41;
	private const int SOUND_ID_BAT_SWOOP = 42;
	private const int SOUND_ID_BOZ_DRILL = 43;
	private const int SOUND_ID_BOZ_FAIL = 44;
	private const int SOUND_ID_BOZ_WIN = 45;
	private const int SOUND_ID_CORA_CLAP = 46;
	private const int SOUND_ID_CORA_MISS = 47;
	private const int SOUND_ID_CORA_SELECT1 = 48;
	private const int SOUND_ID_CORA_SELECT2 = 49;
	private const int SOUND_ID_CORA_SWIPE_VERB = 50;
	private const int SOUND_ID_TENTACLE_DRAG = 51;
	private const int SOUND_ID_TENTACLE_POPUP = 52;
	private const int SOUND_ID_TRAIN_HORN = 53;
	private const int SOUND_ID_TRAIN_HORN_DRY = 54;
	private const int SOUND_ID_TRAIN_LOOP = 55;
	private const int SOUND_ID_TRAIN_STRUGGLE = 56;
	private const int SOUND_ID_MADRA_BITE = 57;
	private const int SOUND_ID_MADRA_GROWL = 58;
	private const int SOUND_ID_BAT_FLAP = 59;
	private const int SOUND_ID_BAT_BITE = 60;
	private const int SOUND_ID_SLUG_GET_READY = 61;
	private const int SOUND_ID_TENTACLE_GET_READY = 62;
	private const int SOUND_ID_SLUG_DEFEAT = 63;
	private const int SOUND_ID_BOZ_JUMP_TO_DRILL = 66;
	private const int SOUND_ID_BOZ_DRILLING = 64;
	private const int SOUND_ID_BOZ_DESTROYED_ROCK = 65;
	private const int SOUND_ID_SCOREPLINK = 67;
	//*************************************************************//
	public AudioClip[] gameSounds;
	//*************************************************************//
	public AudioSource _mainAudioSource;
	private AudioSource _musicAudioSource;
	private AudioClip _currentAudioClip;
	private float _currentAudiClipCountTime;
	private float _currentMusicFadesAudiClipCountTime;
	private bool _doNotUpdateMusicVolume = true;
	private bool _miningLayerMusicPlayed = false;
	private bool _onceBeforePlayedMusic = false;
	private bool _doNotVolumeUpMusic = false;

	//*************************************************************//	
	private static SoundManager _meInstance;
	public static SoundManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_SoundObject" ).GetComponent < SoundManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	
	void Awake ()
	{
		if ( Camera.main != null ) _mainAudioSource = Camera.main.audio;
		if ( Camera.main.transform.Find ( "MusicSource" )) _musicAudioSource = Camera.main.transform.Find ( "MusicSource" ).audio;
		
		if ( SaveDataManager.keyExists ( SaveDataManager.MUSIC_ON_OFF_PREFIX ))
		{
			int musicSaveValue = SaveDataManager.getValue ( SaveDataManager.MUSIC_ON_OFF_PREFIX );
			MUSIC_ON = musicSaveValue == 1 ? true : false;
		}
		
		if ( SaveDataManager.keyExists ( SaveDataManager.SFX_ON_OFF_PREFIX ))
		{
			int sfxSaveValue = SaveDataManager.getValue ( SaveDataManager.SFX_ON_OFF_PREFIX );
			SFX_ON = sfxSaveValue == 1 ? true : false;
		}
		
		turnMusicOnOff ( MUSIC_ON );
		
		if ( _musicAudioSource ) _musicAudioSource.volume = 0f;
	}
	
	void Update ()
	{
		_currentAudiClipCountTime -= Time.deltaTime;
		_currentMusicFadesAudiClipCountTime -= Time.deltaTime;
		
		if ( _musicAudioSource )
		{
			if ( ! _doNotUpdateMusicVolume && ( _currentMusicFadesAudiClipCountTime <= 0f ) && ( _musicAudioSource.volume != 1f ))
			{
				 _musicAudioSource.volume = 1f;
				_doNotUpdateMusicVolume = true;
			}
			
			if ( ! _doNotVolumeUpMusic && _musicAudioSource.volume < 1f )
			{
				_musicAudioSource.volume = Mathf.Lerp ( _musicAudioSource.volume, 1f, 0.004f );
			}
		}
	}
	
	public void stopAllSFX ()
	{
		_mainAudioSource.Stop ();
	}

	public void stopAllSFXTillNewScene ()
	{
		_mainAudioSource.volume = 0f; 
	}
	
	public void turnMusicOnOff ( bool isOn )
	{
		if ( _musicAudioSource )
		{
			_musicAudioSource.enabled = isOn;
			if ( isOn ) _musicAudioSource.Play ();
			else _musicAudioSource.Stop ();
		}
		
		SaveDataManager.save ( SaveDataManager.MUSIC_ON_OFF_PREFIX, isOn ? 1 : 0 );
	}

	public void silenceMusicTillNewScene ()
	{
		_musicAudioSource.volume = 0f;
		_musicAudioSource.Stop ();
		_doNotVolumeUpMusic = true;
	}
	
	public void switchMusicTo ( int music )
	{
		_musicAudioSource = Camera.main.transform.Find ( "MusicSource" ).audio;
		if ( _musicAudioSource )
		{
			AudioClip musicAudioClip = null;
			switch ( music )
			{
				case MISSION_MUSIC:
					musicAudioClip = ( AudioClip ) Resources.Load ( "Sounds/Music/duracelliummines_1" );
					break;
				case MAP_MUSIC:
					musicAudioClip = ( AudioClip ) Resources.Load ( "Sounds/Music/map_1_mines" );
					break;
				case MINING_MUSIC:
					musicAudioClip = ( AudioClip ) Resources.Load ( "Sounds/Music/TR Mining Full track" );
					break;
			}
			
			Resources.UnloadUnusedAssets ();
			
			_musicAudioSource.Stop ();
			_musicAudioSource.clip = musicAudioClip;
			_musicAudioSource.Play ();
		}
	}
	
	public void playSound ( int soundID, int objectID = -1, bool force = false )
	{
		if ( ! SFX_ON ) return;

		AudioClip soundToBePlayed = null;
		bool soundThatFadesOutMusic = false;
		switch ( soundID )
		{
			case CHCH:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_CHCH];
					break;
				}
				break;
			}
			case BOZ_JUMP_TO_DRILL:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BOZ_JUMP_TO_DRILL];
					break;
				}
				break;
			}
			case BOZ_DRILLING:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BOZ_DRILLING];
					break;
				}
				break;
			}
			case BOZ_DESTROYED_ROCK:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BOZ_DESTROYED_ROCK];
					break;
				}
				break;
			}
//=======================================Daves Work===================================
			case BAT_DEFEAT:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BAT_DEFEAT];
					break;
				}
				break;
			}
			case BAT_MOUNT:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BAT_MOUNT];
					break;
				}
				break;
			}
			case BAT_SWOOP:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BAT_SWOOP];
					break;
				}
				break;
			}
			case BOZ_DRILL:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BOZ_DRILL];
					break;
				}
				break;
			}
			case BOZ_FAIL:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BOZ_FAIL];
					break;
				}
				break;
			}
			case BOZ_WIN:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BOZ_WIN];
					break;
				}
				break;
			}
			case CORA_CLAP:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_CORA_CLAP];
					break;
				}
				break;
			}
			case CORA_MISS:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_CORA_MISS];
					break;
				}
				break;
			}
			case CORA_SELECT1:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_CORA_SELECT1];
					break;
				}
				break;
			}
			case CORA_SELECT2:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_CORA_SELECT2];
					break;
				}
				break;
			}
			case CORA_SWIPE_VERB:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_CORA_SWIPE_VERB];
					break;
				}
				break;
			}
			case TENTACLE_DRAG:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TENTACLE_DRAG];
					break;
				}
				break;
			}
			case TENTACLE_POPUP:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TENTACLE_POPUP];
					break;
				}
				break;
			}
			case TRAIN_HORN:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TRAIN_HORN];
					break;
				}
				break;
			}
			case TRAIN_HORN_SHORT:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TRAIN_HORN_DRY];
					break;
				}
				break;
			}
			case TRAIN_LOOP:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TRAIN_LOOP];
					break;
				}
				break;
			}
			case TRAIN_STRUGGLE:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TRAIN_STRUGGLE];
					break;
				}
				break;
			}
		case MADRA_BITE:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_MADRA_BITE];
				break;
			}
			break;
		}
		case MADRA_GROWL:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_MADRA_GROWL];
				break;
			}
			break;
		}
		case BAT_FLAP:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_BAT_FLAP];
				break;
			}
			break;
		}
		case BAT_BITE:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_BAT_BITE];
				break;
			}
			break;
		}
		case SLUG_GET_READY:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_SLUG_GET_READY];
				break;
			}
			break;
		}
		case TENTACLE_GET_READY:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_TENTACLE_GET_READY];
				break;
			}
			break;
		}
		case SLUG_DEFEAT:
		{
			int randomSoundID = Random.Range ( 0, 1 );
			switch ( randomSoundID )
			{
			case 0:
				soundToBePlayed = gameSounds[SOUND_ID_SLUG_DEFEAT];
				break;
			}
			break;
		}
//=======================================Daves Work===================================
			case TOY_DEPOSITED:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TR_TOY_DEPOSITED];
					break;
				}
				break;
			}
			case MACHINE_OPENS_END:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TR_MACHINE_OPENS_END];
					break;
				}
				break;
			}
			case MACHINE_OPENS:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_TR_MACHINE_OPENS];
					break;
				}
				break;
			}
			case MACHINE_CLOSES:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_TR_MACHINE_CLOSES];
						break;
				}
				break;
			}
			case MACHINE_CLOSES_PLUS_LOCK:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_TR_CLOSES_PLUS_LOCK];
						break;
				}

				break;
			}
			case LOCKING_MECH:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_TR_LOCKING_MECH];
						break;
				}
				break;
			}
			case TENTACLE_DRAINER_ATTACK:
				switch ( objectID )
				{
					case GameElements.ENEM_TENTACLEDRAINER_01:
					case GameElements.ENEM_TENTACLEDRAINER_02:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_TENTACLE_DRAINER_ATTACK_01];
								break;
						}
						break;
				}
				break;
			case CHARACTER_ATTACK:
				switch ( objectID )
				{
					case GameElements.CHAR_MADRA_1_IDLE:
						int randomSoundID = Random.Range ( 0, 2 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[MADRA_BITE];
								break;
							case 1:
								soundToBePlayed = gameSounds[MADRA_BITE];
								break;
						}
						break;
				}
				break;
			case CHARACTER_ATTACK_FAIL:
				switch ( objectID )
				{
					case GameElements.CHAR_MADRA_1_IDLE:
						int randomSoundID = Random.Range ( 0, 2 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_MADRA_ATTACK_FAIL_01];
								break;
							case 1:
								soundToBePlayed = gameSounds[SOUND_ID_MADRA_ATTACK_FAIL_02];
								break;
						}
						break;
				}
				break;
			case CHARACTER_TROLEY_MOVE:
				switch ( objectID )
				{
					case GameElements.CHAR_CORA_1_IDLE:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_CHARACTER_TROLEY_MOVE_01];
								break;
						}
						break;
				}
				break;
			case CHARACTER_TROLEY_MOVE2:
				switch ( objectID )
				{
					case GameElements.CHAR_CORA_1_IDLE:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_CHARACTER_TROLEY_MOVE_02];
								break;
						}
						break;
				}
				break;
			case DESTROYABLE_DEMOLISH:
				switch ( objectID )
				{
					case GameElements.ENVI_CRACKED_1_ALONE:
					case GameElements.ENVI_CRACKED_1_LEFT:
					case GameElements.ENVI_CRACKED_1_MID:
					case GameElements.ENVI_CRACKED_1_RIGHT:
					case GameElements.ENVI_METAL_1_ALONE:
					case GameElements.ENVI_PLASTIC_1_ALONE:
					case GameElements.ENVI_TECHNOEGG_1_ALONE:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_CRACKED_ROCK_01_DEMOLISH_01];
								break;
						}
						break;
				}
				break;
			case REDIRECTOR_BUILD:
				switch ( objectID )
				{
					case GameElements.ENVI_REDIRECTOR_01:
					case GameElements.ENVI_REDIRECTOR_02:
					case GameElements.ENVI_REDIRECTOR_03:
					case GameElements.ENVI_REDIRECTOR_04:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_REDIRECTOR_BUILD_01];
								break;
						}
						break;
				}
				break;
			case CONFIRM_BUTTON:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_CONFRIM_BUTTON_01];
						break;
				}
				break;
			}
			case CANCEL_BUTTON:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_CANCEL_BUTTON_01];
						break;
				}
				break;
			}
			case REDIRECTOR_POWER_UP:
				switch ( objectID )
				{
					case GameElements.ENVI_REDIRECTOR_01:
					case GameElements.ENVI_REDIRECTOR_02:
					case GameElements.ENVI_REDIRECTOR_03:
					case GameElements.ENVI_REDIRECTOR_04:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_REDIRECTOR_POWER_UP_01];
								break;
						}
						break;
				}
				break;
			case POWER_PLUG_CONNECTED:
			{
				soundThatFadesOutMusic = true;
				_doNotUpdateMusicVolume = false;
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_POWER_PLUG_CONNECTED_01];
						break;
				}
			
				GameObject soundObject = new GameObject ( "sound" );
				soundObject.transform.position = _mainAudioSource.transform.position;
				soundObject.transform.parent = _mainAudioSource.transform;
				soundObject.AddComponent < DestroyAfterTime > ().time = 4f;
				soundObject.AddComponent < AudioSource > ().PlayOneShot ( soundToBePlayed );
				return;
			}
			case REDIRECTOR_ROTATE:
				switch ( objectID )
				{
					case GameElements.ENVI_REDIRECTOR_01:
					case GameElements.ENVI_REDIRECTOR_02:
					case GameElements.ENVI_REDIRECTOR_03:
					case GameElements.ENVI_REDIRECTOR_04:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_REDIRECTOR_ROTATE_01];
								break;
						}
						break;
				}
				break;
			case OBJECT_SELECT:
				switch ( objectID )
				{
					case GameElements.CHAR_CORA_1_IDLE:
					{
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_CORA_SELECT_01];
								break;
						}
						break;
					}
					case GameElements.CHAR_MADRA_1_IDLE:
					{
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_MADRA_SELECT_01];
								break;
						}
						break;
					}
					case GameElements.CHAR_FARADAYDO_1_IDLE:
					{
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_FARADAYDO_SELECT_01];
								break;
						}
						break;
					}
					default:
						soundToBePlayed = gameSounds[SOUND_ID_OBJECT_SELECT_01];
						break;
				}
				break;
			case HEADER_TAP:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_HEADER_TAP_01];
						break;
				}
				break;
			}
			case RESCUE_TOY_AT_MARKER:
			{
				soundThatFadesOutMusic = true;
				_doNotUpdateMusicVolume = false;
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_RESCUE_TOY_AT_MARKER_01];
						break;
				}
			
				GameObject soundObject = new GameObject ( "sound" );
				soundObject.transform.position = _mainAudioSource.transform.position;
				soundObject.transform.parent = _mainAudioSource.transform;
				soundObject.AddComponent < DestroyAfterTime > ().time = 4f;
				soundObject.AddComponent < AudioSource > ().PlayOneShot ( soundToBePlayed );
				return;
			}
			case MISSION_COMPLETE:
			{
				_musicAudioSource.clip = null;
				soundThatFadesOutMusic = true;
				_doNotUpdateMusicVolume = false;
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_MISSION_COMPLETE_01];
						break;
				}
			
				break;
			}
			case MISSION_FAILED:
			{
				_musicAudioSource.clip = null;
				soundThatFadesOutMusic = true;
				_doNotUpdateMusicVolume = false;
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_MISSION_FAILED_01];
						break;
				}
				break;
			}
			case TENTACLE_DRAINER_DEFATED:
				soundToBePlayed = gameSounds[SOUND_ID_TENTACLE_DRAINER_DEFATED_01];
				switch ( objectID )
				{
					case GameElements.ENEM_TENTACLEDRAINER_01:
					case GameElements.ENEM_TENTACLEDRAINER_02:
						int randomSoundID = Random.Range ( 0, 1 );
						switch ( randomSoundID )
						{
							case 0:
								soundToBePlayed = gameSounds[SOUND_ID_TENTACLE_DRAINER_DEFATED_01];
								break;
						}
						break;
				}
				break;
			case BUM:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_BUM];
						break;
				}
				break;
			}
			case ELECTROCUTED:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_ELECTROCUTED];
						break;
				}
				break;
			}
			case LEVEL_NODE_UNLOCKED:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_LEVEL_NODE_UNLOCKED];
						break;
				}
				break;
			}
			case X_MARK:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_X_MARK];
					break;
				}
				break;
			}
			case BUTTONS_APAERE:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_BUTTONS_APAERE];
					break;
				}
				break;
			}
			case SCORE_PLINK:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_SCOREPLINK];
					break;
				}
				break;
			}
			case STAR_01:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
					case 0:
						soundToBePlayed = gameSounds[SOUND_ID_STAR_01_APAERE];
						break;
				}

				GameObject soundObject = new GameObject ( "sound" );
				soundObject.transform.position = _mainAudioSource.transform.position;
				soundObject.transform.parent = _mainAudioSource.transform;
				soundObject.AddComponent < DestroyAfterTime > ().time = 4f;
				soundObject.AddComponent < AudioSource > ().PlayOneShot ( soundToBePlayed );
				soundObject.audio.pitch = 1f;
				soundObject.audio.priority = 0;
				return;
			}
			case STAR_02:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_STAR_02_APAERE];
					break;
				}
				GameObject soundObject = new GameObject ( "sound" );
				soundObject.transform.position = _mainAudioSource.transform.position;
				soundObject.transform.parent = _mainAudioSource.transform;
				soundObject.AddComponent < DestroyAfterTime > ().time = 4f;
				soundObject.AddComponent < AudioSource > ().PlayOneShot ( soundToBePlayed );
				soundObject.audio.pitch = 1f;
				soundObject.audio.priority = 0;
				return;
			}
			case STAR_03:
			{
				int randomSoundID = Random.Range ( 0, 1 );
				switch ( randomSoundID )
				{
				case 0:
					soundToBePlayed = gameSounds[SOUND_ID_STAR_03_APAERE];
					break;
				}
				GameObject soundObject = new GameObject ( "sound" );
				soundObject.transform.position = _mainAudioSource.transform.position;
				soundObject.transform.parent = _mainAudioSource.transform;
				soundObject.AddComponent < DestroyAfterTime > ().time = 4f;
				soundObject.AddComponent < AudioSource > ().PlayOneShot ( soundToBePlayed );
				soundObject.audio.pitch = 2f;
				soundObject.audio.priority = 0;
				return;
			}
		}
		
		if ( soundToBePlayed != null )
		{

			if ( _currentAudioClip != null )
			{
				if ( soundToBePlayed.name != _currentAudioClip.name )
				{
					_currentAudioClip = soundToBePlayed;
					_currentAudiClipCountTime = _currentAudioClip.length;
					_mainAudioSource.PlayOneShot ( soundToBePlayed );

				}
				else
				{
					if (( _currentAudiClipCountTime <= 0f ) || ( force ))
					{
						_currentAudioClip = soundToBePlayed;
						_currentAudiClipCountTime = _currentAudioClip.length;
						_mainAudioSource.PlayOneShot ( soundToBePlayed );
					}
				}
			}
			else
			{
				_currentAudioClip = soundToBePlayed;
				_currentAudiClipCountTime = _currentAudioClip.length;
				_mainAudioSource.PlayOneShot ( soundToBePlayed );
			}
		}
		
		if ( soundThatFadesOutMusic )
		{
			_currentMusicFadesAudiClipCountTime = soundToBePlayed.length;
			_musicAudioSource.volume = 0f;
		}
	}
}
