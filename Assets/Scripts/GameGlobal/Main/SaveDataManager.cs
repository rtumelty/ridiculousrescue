using UnityEngine;
using System.Collections;

public class SaveDataManager
{
	//*************************************************************//
	public const string METAL_IN_CONTAINERS = "metal";
	public const string PLASTIC_IN_CONTAINERS = "plastic";
	public const string VINES_IN_CONTAINERS = "vines";
	public const string RECHARGEOCORES_IN_CONTAINERS = "rechargeocores";
	public const string REDIRECTORS_IN_CONTAINERS = "redirectors";
	public const string PREMIUM_CURRENCY = "premium";
	public const string LABORATORY_ENTERED = "labEnters";
	//========Daves Edit=========
	public const string TIME_FOR_LIFE_LEFT = "lifeTimer";
	public const string LEFTOVER_TIME = "leftTime";
	public const string LAST_RECORDED_TIME = "lastRecoredTime";
	public const string MINES_ENTERED = "mineEntered";
	public const string LIVES_AVAILABLE = "livesAvailable";
	//========Daves Edit=========
	public const string LEVEL_FINISHED_PREFIX = "level";
	public const string LEVEL_MINING_FINISHED_PREFIX = "levelMining";

	public const string LEVEL_STARS_PREFIX = "levelStars";
	public const string MINING_LEVEL_STARS_PREFIX = "miningLevelStars";
	public const string MINING_LEVEL_COOLDOWN_TIME_PREFIX = "miningLevelCooldownTime";
	public const string TRAIN_LEVEL_STARS_PREFIX = "trainLevelStars";
	
	public const string MOM_SLOTS_NUMBER_PREFIX = "momSlots";
	public const string MOM_SLOT_0_TIME_PREFIX = "momSlot0Time";
	public const string MOM_SLOTS_ELEMENT_PREFIX = "momSlotsElement";
	
	public const string STORAGE_CONTAINER_LEVEL_PREFIX = "storageLevel";
	public const string STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX = "storageLevelUpTime";
	
	public const string BATTERIES_TIME_TO_NEXT_PREFIX = "batteriesTimeToNext";
	public const string BATTERIES_AMOUNT_PREFIX = "batteriesAmount";
	
	public const string LEVEL_WAS_UNLOCKED_PREFIX = "levelWasUnlocked";
	
	public const string MUSIC_ON_OFF_PREFIX = "musicOnOff";
	public const string SFX_ON_OFF_PREFIX = "sfxOnOff";

	public const string WORLD_COMPLETED_PREFIX = "worldCompleted";
	public const string WORLD_I_WAS_ON_PREFIX = "worldIWasOn";

	public const string INTRO_PLAYED_PREFIX = "introPlayed";

	public const string LEVEL_MAP_DIALOG_PLAYED_PREFIX = "levelMapDialogPlayed";

	public const string LOCK_ON_LEVEL_DESTROYED_PREFIX = "lockOnLevelDestroyed";
	public const string UNLOCK_LEVEL12_PLAYED = "unlockLevel12Played";

	public const string TRAIN_TUTORIAL_PLAYED = "trainTutorialPlayed";

	public const string MINING_PLAYED = "miningPlayed";
	public const string MINING_FAILED = "miningFailed";
	public const string MINING_SUCCESS = "miningSuccess";

	public const string TRAIN_PLAYED = "trainPlayed";
	public const string TRAIN_FAILED = "trainFailed";
	public const string TRAIN_SUCCESS = "trainSuccess";

	public const string BATTERIES_AT_MOM_READY_TO_TAP = "batteriesAtMomReadyToTap";
	public const string REDIRECTORS_AT_MOM_READY_TO_TAP = "redirectorsAtMomReadyToTap";
	//*************************************************************//
	
	public static void saveValues ()
	{
		PlayerPrefs.SetInt ( METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS );
		PlayerPrefs.SetInt ( PLASTIC_IN_CONTAINERS, GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS );
		PlayerPrefs.SetInt ( VINES_IN_CONTAINERS, GameGlobalVariables.Stats.VINES_IN_CONTAINERS );
		PlayerPrefs.SetInt ( RECHARGEOCORES_IN_CONTAINERS, GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS );
		PlayerPrefs.SetInt ( REDIRECTORS_IN_CONTAINERS, GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS );
		PlayerPrefs.SetInt ( PREMIUM_CURRENCY, GameGlobalVariables.Stats.PREMIUM_CURRENCY );
		PlayerPrefs.SetInt (TIME_FOR_LIFE_LEFT, GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION_LEFT);
		PlayerPrefs.Save ();
	}
	
	public static void save ( string key, int intValue )
	{
		PlayerPrefs.SetInt ( key, intValue );
		PlayerPrefs.Save ();
	}
	
	public static void save ( string key, float floatValue )
	{
		PlayerPrefs.SetFloat ( key, floatValue );
		PlayerPrefs.Save ();
	}
	
	public static void save ( string key, string stringValue )
	{
		PlayerPrefs.SetString ( key, stringValue );
		PlayerPrefs.Save ();
	}
	
	public static int getValue ( string key )
	{
		if ( ! PlayerPrefs.HasKey ( key )) return 0;
		return PlayerPrefs.GetInt ( key );
	}
	
	public static bool saveFileExists ()
	{
		if ( PlayerPrefs.HasKey ( METAL_IN_CONTAINERS )) return true;
		else return false;
	}
	
	public static bool keyExists ( string key )
	{
		return PlayerPrefs.HasKey ( key );
	}
	
	public static void setAllValues ()
	{
		if ( ! saveFileExists ()) return;
		GameGlobalVariables.Stats.METAL_IN_CONTAINERS = PlayerPrefs.GetInt ( METAL_IN_CONTAINERS );
		GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS = PlayerPrefs.GetInt ( PLASTIC_IN_CONTAINERS );
		GameGlobalVariables.Stats.VINES_IN_CONTAINERS = PlayerPrefs.GetInt ( VINES_IN_CONTAINERS );
		GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS = PlayerPrefs.GetInt ( RECHARGEOCORES_IN_CONTAINERS );
		GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS = PlayerPrefs.GetInt ( REDIRECTORS_IN_CONTAINERS );
		GameGlobalVariables.Stats.PREMIUM_CURRENCY = PlayerPrefs.GetInt ( PREMIUM_CURRENCY );
	}
}
