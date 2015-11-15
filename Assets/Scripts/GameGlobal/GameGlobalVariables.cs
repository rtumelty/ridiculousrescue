using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGlobalVariables
{
	//*************************************************************//	
	public const string VERSION = "0.6.06";  
	//*************************************************************//
	public static bool RELEASE = true;
	//*************************************************************//
	public static bool CUT_DOWN_GAME = true;
	//*************************************************************//
	public static bool TEST_BUILD = true;
	//*************************************************************//
	public static bool SHOW_ADS = true;
	//*************************************************************//
	public static bool SHOW_LEVEL_NUMBERS = false;
#if UNITY_EDITOR
	public static bool SHOW_CONSOLE = true;
#else
	public static bool SHOW_CONSOLE = false;
#endif
	//*************************************************************//
	public const int RESCUE = 0;
	public const int LABORATORY = 1;
	public const int MINING = 2;
	public const int TRAIN = 3;
	public const int INTRO = 4;
	public static int CURRENT_GAME_PART = RESCUE;

	public const int GOLD_CAP = 99999;
	//*************************************************************//
	public static bool UNLOCK_ALL_LEVELS = false;
	//*************************************************************//
	public static bool DUMMY = false;
	//*************************************************************//
	public static bool CHARACTER_RECENTLY_CHANGED = false;
	public static bool ACTIVE_FROM_ENEMY = false;
	public static int LAB_ENTERED = 0;
	public static int MINE_ENTERED = 0;
	public static int NUMBER_OF_MINE_LEVELS = 0;
	public static int I_WAS_IN_MINES = 0;
	public static bool BLOCK_LAB_ENTERED = true;
	public static int INTRO_PLAYED = 0;
	public static bool BACK_FROM_LEVEL = false;
	public static int WORLD_I_WAS_IN = 0;
	public static bool ACTIVE_TENTACLE = false;
	public static GameObject mostRecentLevel;
	public static FLStorageContainerClass lastStorageContainerClass;
	//*************************************************************//
	public static class Stats
	{ 
		//============== Daves edit for RR =================
		public static int COST_OF_MAP_UNLOCK = 200;
		public static int LIVES_AVAILABLE = 5;
		public static int MAX_LIVES = 5;
		public static int GOLD_COST_OF_LIFE = 5;
		public static int GOLD_AVAILABLE = 30;
		public static int COST_OF_CONTINUE = 5;
		public static int TIME_FOR_LIFE_GENERATION = 1200;//seconds
		public static int TIME_FOR_LIFE_GENERATION_LEFT = 0;
		public static int MOVES_BOUGHT_FOR_CONTINUE = 5;
		//============== Daves edit for RR =================
		//for RR
		public static int MOVES = 0;
		public static int SCORE = 0;

		public static class Scores
		{
			public static int FOR_RESCUE_TOY = 100;
			public static int FOR_DESTRYING_TENTACLE = 50;
			public static int FOR_DESTRYING_SLUG = 100;
			public static int FOR_DESTROYING_ROCK = 100;
			public static int FOR_DESTRYING_GOLD_ROCK = 100;
		}

		public static int RECHARGEOCORES_IN_CONSTRUCTION = 0;
		public static int PREMIUM_CURRENCY = 200;
		public static int METAL_IN_CONTAINERS = 30;
		public static int PLASTIC_IN_CONTAINERS = 18;
		public static int VINES_IN_CONTAINERS = 6;
		public static int RECHARGEOCORES_IN_CONTAINERS = 0;
		public static int REDIRECTORS_IN_CONTAINERS = 0;
		
		public static class NewResources
		{
			public static int PREMIUM_CURRENCY = 0;
			public static int METAL = 0;
			public static int PLASTIC = 0;
			public static int VINES = 0;
			public static int RECHARGEOCORES = 0;
			public static int REDIRECTORS = 0;
			
			public static void reset ()
			{
				PREMIUM_CURRENCY = 0;
				METAL = 0;
				PLASTIC = 0;
				VINES = 0;
				RECHARGEOCORES = 0;
				REDIRECTORS = 0;
			}
		}
		
		public static void reset ()
		{
			SCORE = 0;

			PREMIUM_CURRENCY = 200;
			METAL_IN_CONTAINERS = 12;
			PLASTIC_IN_CONTAINERS = 18;
			VINES_IN_CONTAINERS = 6;
			RECHARGEOCORES_IN_CONTAINERS = 0;
			REDIRECTORS_IN_CONTAINERS = 0;
		}
	}
	
	public static class AdditionalMoves 
	{
		public static int ADDITIONAL_MOVES = 0;
		
		public static void reset ()
		{
			ADDITIONAL_MOVES = 0;
		}
	}
	
	public static class FontMaterials
	{
		public static Material WHITE_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_white" );
		public static Material BLACK_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_black" );
		public static Material RED_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_red" );
		public static Material WHITE_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_white" );
		public static Material BLACK_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_black" );
		public static Material BLACK_TEXT_02 = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_black1" );
		public static Material RED_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_red" );
		public static Material RED_TEXT_02 = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_red1" );
		public static Material GREY_TEXT =  (Material ) Resources.Load ( "Fonts/AdLibBT Regular_grey" );
		
		public static Material BLACK_BIG_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_black1" );
		public static Material WHITE_BIG_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_white1" );
		
		public static Material BLACK_BIG_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_black1" );
		public static Material WHITE_BIG_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_white1" );
	}
	
	public static class Missions
	{
		//*************************************************************//
		public class LevelClass
		{
			//*************************************************************//
			public class RequiredElement
			{
				public int elementID;
				public int amountRequiered;
				
				public RequiredElement ( int elementIDValue, int amountRequieredValue )
				{
					elementID = elementIDValue;
					amountRequiered = amountRequieredValue;
				}
			}
			//*************************************************************//
			public string myName;
			public int type;
			public bool iAmFinished;
			public int starsEarned;

			public int moves;
			public int movesLimitForStar;
			public float timeLimitForStar;
			public float scoreLimitForStar;

			public List < RequiredElement > requiredElements;
			public LevelControl.SpecificLevelData mySpecificLevelData;
			
			public LevelClass ( string myNameValue, int typeValue = FLMissionScreenNodeManager.TYPE_REGULAR_NODE, int movesValue = 20, int movesLimitForStarValue = 10, float timeLimitForStarValue = 60f, int scoreLimitForStarValue = 2000 )
			{ 
				myName = myNameValue;
				type = typeValue;

				timeLimitForStar = timeLimitForStarValue;

				moves = movesValue; 
				movesLimitForStar = movesLimitForStarValue;

				scoreLimitForStar = scoreLimitForStarValue;
			}
		}
		//*************************************************************//
		public class MiningLevelClass
		{
			public string myName;
			public float coolDownTimeLeft;
			public int starsEarned;
			public bool iAmFinished;
			public int type;
			public MNLevelControl.SpecificLevelData mySpecificLevelData;
			
			public MiningLevelClass ( string myNameValue, int typeValue = FLMissionScreenNodeManager.TYPE_MINING_NODE )
			{
				myName = myNameValue;
				type = typeValue;
			}
		}
		//*************************************************************//
		public class TrainLevelClass
		{
			public string myName;
			public int starsEarned;
			public bool iAmFinished;
			//==================================Daves Edit=====================================
			public int star04;
			public int type;
			
			public TrainLevelClass ( string myNameValue, int star04Value = 0, int typeValue = FLMissionScreenNodeManager.TYPE_TRAIN_NODE )
			{
				myName = myNameValue;
				star04 = star04Value;
				type = typeValue;
			}
			//==================================Daves Edit=====================================
			
		}
		//*************************************************************//
		public class WorldClass
		{
			public string name;
			public List < LevelClass > levels;
			public List < MiningLevelClass > miningLevels;
			public List < LevelClass > bonusLevels;
			public List < TrainLevelClass > trainLevels;
			
			public WorldClass ( string nameValue, List < LevelClass > levelsValue, List < LevelClass > bonusValue, List < MiningLevelClass > miningLevelsValue, List < TrainLevelClass > trainLevelsValue = null )
			{
				name = nameValue;
				levels = levelsValue;
				bonusLevels = bonusValue;
				miningLevels = miningLevelsValue;
				trainLevels = trainLevelsValue;
			}
		}
		//*************************************************************//
		public const int WORLD_DURACELLIUM_MINES = 0;
		public const int WORLD_OTHER = 1;
		//*************************************************************//
		public static List < WorldClass > WORLDS;
		//*************************************************************//
		public static void fillWorldsAndLeveles ()
		{
			WORLDS = new List < WorldClass > ();
			// Duracellium Mines
			List < LevelClass > leveles = new List < LevelClass > ();
			List < MiningLevelClass > miningLeveles = new List < MiningLevelClass > ();
			List < TrainLevelClass > trainLeveles = new List < TrainLevelClass > ();
			leveles.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 15, 8, 12f, 700 ));
			leveles.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 20, 10, 12f, 1000 ));
			leveles.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 20, 7, 20f, 1300 ));
			leveles.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 25, 15, 40f, 1000 ));
			leveles.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 24, 12, 30f, 1200 ));
			leveles.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 35, 25, 50f, 1000 ));
			leveles.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 24, 16, 55f, 800 ));
			leveles.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 30, 20, 92f, 1000 ));
			leveles.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 24, 18, 50f, 600 ));
			leveles.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 30, 20, 110f, 1000 ));
			leveles.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 38, 30, 150f, 800 ));

			List < LevelClass > levelesBonus = new List < LevelClass > ();
			
			levelesBonus.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 25, 20, 90f, 500 ));
			levelesBonus.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 15, 10, 55f, 500 ));
			levelesBonus.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 32, 25, 110f, 700 ));
			levelesBonus.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 28, 20, 90f, 800 ));
			levelesBonus.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 16, 12, 35f, 400 ));
			levelesBonus.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 25, 20, 90f, 500 ));
			 
			miningLeveles.Add ( new MiningLevelClass ( "1" ));
			trainLeveles.Add ( new TrainLevelClass ( "1", 80));

			WORLDS.Add ( new WorldClass ( "ui_sign_worldname_map1", leveles, levelesBonus, miningLeveles, trainLeveles ));

			// Some Other World 
			leveles = new List < LevelClass > ();
			leveles.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));

			levelesBonus = new List < LevelClass > ();

			levelesBonus.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));

			miningLeveles = new List < MiningLevelClass > ();
			trainLeveles = new List < TrainLevelClass > ();

			miningLeveles.Add ( new MiningLevelClass ( "1" ));
			trainLeveles.Add ( new TrainLevelClass ( "1",80 ));
			leveles.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 10, 8, 18f, 200 ));
			leveles.Add ( new LevelClass ( "13", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 24, 16, 60f, 800 ));
			leveles.Add ( new LevelClass ( "14", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 14, 12, 30f, 200 ));
			leveles.Add ( new LevelClass ( "15", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 10, 6, 25f, 400 ));
			leveles.Add ( new LevelClass ( "16", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 20, 15, 45f, 500 ));
			leveles.Add ( new LevelClass ( "17", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 30, 20, 90f, 1000 ));
			leveles.Add ( new LevelClass ( "18", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 15, 10, 25f, 500 ));
			leveles.Add ( new LevelClass ( "19", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 15, 10, 50f, 500 ));
			leveles.Add ( new LevelClass ( "20", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 30, 25, 110f, 500 ));
			leveles.Add ( new LevelClass ( "21", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 30, 15, 60f, 1500 ));
			leveles.Add ( new LevelClass ( "22", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 15, 8, 45f, 700 ));
			leveles.Add ( new LevelClass ( "23", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 35, 28, 105f, 700 ));
			leveles.Add ( new LevelClass ( "24", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 35, 28, 140f, 700 ));
			leveles.Add ( new LevelClass ( "25", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 24, 20, 85f, 400 ));

			levelesBonus.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 26, 22, 80f, 400 ));
			levelesBonus.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 40, 30, 120f, 1000 ));
			levelesBonus.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 40, 30, 120f, 1000 ));
			levelesBonus.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 28, 24, 80f, 400 ));
			levelesBonus.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 20, 14, 70f, 600 ));
			levelesBonus.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 50, 40, 180f, 1000 ));

			miningLeveles.Add ( new MiningLevelClass ( "2" ));
			trainLeveles.Add ( new TrainLevelClass ( "2",110 ));
			trainLeveles.Add ( new TrainLevelClass ( "3",110 ));

			WORLDS.Add ( new WorldClass ( "ui_sign_worldname_map2", leveles, levelesBonus, miningLeveles, trainLeveles ));
			//**************************************************************************************************************//
			leveles.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "13", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "14", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "15", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "16", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "17", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "18", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "19", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "20", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "21", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "22", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "23", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "24", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "25", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			leveles.Add ( new LevelClass ( "26", FLMissionScreenNodeManager.TYPE_REGULAR_NODE ));
			
			levelesBonus.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			levelesBonus.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_BONUS_NODE ));
			
			miningLeveles.Add ( new MiningLevelClass ( "2" ));
			trainLeveles.Add ( new TrainLevelClass ( "2",110 ));
			trainLeveles.Add ( new TrainLevelClass ( "3",110 ));
			WORLDS.Add ( new WorldClass ( "ui_sign_worldname_map3", leveles, levelesBonus, miningLeveles, trainLeveles ));
		}
	}
}
