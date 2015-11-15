using UnityEngine;
using System.Collections;

public class GameElements
{
	//************************************************//
	public static int[] TWO_TILES_OBJECTS = new int[] { 31, 32 };
	public static int[] TWO_BY_TWO_TILES_OBJECTS = new int[] { 40, 41, 43, 77 };
	public static int[] THREE_BY_THREE_TILES_OBJECTS = new int[] { 39 };
	public static int[] ONE_TILES_OBJECTS = new int[] { 35, 36, 37, 38 };
	public static int[] ONE_TILES_OBJECTS_BORDER = new int[] { 1000, 1499 };
	
	public static int[] REDIRECTORS = new int[] { 35, 36, 37, 38 };
	public static int[] DESTROYABLE_OBJECTS = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
	public static int[] DESTROYABLE_OBJECTS_RESOURCES = new int[] { 71, 72, 73, 74, 75, 76, 92, 93, 94 };
	public static int[] CRACKED_ROCKS_OBJECTS = new int[] { 1, 2, 3, 4 };
	public static int[] ENEMIES = new int[] { 31, 32, 77 };
	public static int[] ENEMIES_TENTACLES = new int[] { 31, 32 };
	public static int[] CHARACTERS = new int[] { 39, 40, 41, 43, 82, 83, 84, 85, 86, 87, 88 };
	public static int[] DRAINED_CHARACTERS = new int[] { 42, 53, 59, 60, 77, 78, 79, 80, 81, 89, 90 };
	public static int[] CHARACTERS_MINERS_DRAINED = new int[] { 42, 77, 78, 79, 80, 81 };
	public static int[] CHARACTERS_MINERS = new int[] { 43, 82, 83, 84, 85, 86 };
	public static int[] RESCUERS = new int[] { 39 };
	public static int[] TO_BE_RESCUED = new int[] { 42, 53, 60, 77, 78, 79, 80, 81, 89, 90, 91 };
	public static int[] BUILDERS = new int[] { 40, 88 };
	public static int[] ATTACKERS = new int[] { 41, 88 };
	public static int[] TROLEYS = new int[] { 44, 58 };
	public static int[] SIZE_3X1 = new int[] { 68, 69 };
	public static int[] ELECTRICITY_PASSES_BORDERS = new int[] { 1000, 1055 };
	public static int[] WALKABLE_BORDERS = new int[] { 1024, 1033 };
	public static int[] HOLES_BORDERS = new int[] { 1017, 1018 };
	
	public static int[] OPERATIONS_ON_PARENT = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 31, 32, 33, 34, 35, 36, 37, 39, 40, 41, 42, 43, 53, 777779, 777780, 777781 };
	//************************************************//
	public const int EMPTY = 0;
	//************************************************//
	public const int BEAM = 777777;
	public const int UNWALKABLE = 777778;
	
	public const int MOM_OBJECT = 777779;
	public const int STORAGE_OBJECT = 777780;
	public const int MISSION_TABLE_OBJECT = 777781;

	public const int SLUG_DUMMY_TILE_FOR_LONGER_PATH = 777782;
	//************************************************//
	public const int ENVI_CRACKED_1_LEFT = 1;
	public const int ENVI_CRACKED_1_MID = 2;
	public const int ENVI_CRACKED_1_RIGHT = 3;
	public const int ENVI_CRACKED_1_ALONE = 4;
	
	public const int ENVI_CRACKED_2_LEFT = 5;
	public const int ENVI_CRACKED_2_MID = 6;
	public const int ENVI_CRACKED_2_RIGHT = 7;
	public const int ENVI_CRACKED_2_ALONE = 8;
	
	public const int ENVI_CRACKED_3_LEFT = 9;
	public const int ENVI_CRACKED_3_MID = 10;
	public const int ENVI_CRACKED_3_RIGHT = 11;
	public const int ENVI_CRACKED_3_ALONE = 12;
	//************************************************//
	public const int ENVI_SOLIDROCKS_1_CENTER = 13;
	public const int ENVI_SOLIDROCKS_1_DOWN_END = 14;
	public const int ENVI_SOLIDROCKS_1_LEFT = 15;
	public const int ENVI_SOLIDROCKS_1_LEFT_DOWN = 16;
	public const int ENVI_SOLIDROCKS_1_LEFT_MID = 17;
	public const int ENVI_SOLIDROCKS_1_LEFT_UP = 18;
	public const int ENVI_SOLIDROCKS_1_MID = 19;
	public const int ENVI_SOLIDROCKS_1_MID_RIGHT = 20;
	public const int ENVI_SOLIDROCKS_1_RIGHT = 21;
	public const int ENVI_SOLIDROCKS_1_RIGHT_DOWN = 22;
	public const int ENVI_SOLIDROCKS_1_RIGHT_UP = 23;
	public const int ENVI_SOLIDROCKS_1_STANDALONE = 24;
	public const int ENVI_SOLIDROCKS_1_TOP_MID = 25;
	public const int ENVI_SOLIDROCKS_1_UP_END = 26;
	public const int ENVI_SOLIDROCKS_1_UP_MID = 27;
	
	public const int ENVI_SOLIDROCKS_2_MID = 28;
	public const int ENVI_SOLIDROCKS_2_UP_END = 29;
	
	public const int ENVI_SOLIDROCKS_3_MID = 30;
	//************************************************//
	public const int ENEM_TENTACLEDRAINER_01 = 31;
	public const int ENEM_TENTACLEDRAINER_02 = 32;
	public const int ENEM_SLUG_01 = 77;
	//************************************************//
	public const int ENVI_POWERPLUG_01 = 33;
	public const int ENVI_CORES_01 = 34;
	public const int ENVI_REDIRECTOR_01 = 35;
	public const int ENVI_REDIRECTOR_02 = 36;
	public const int ENVI_REDIRECTOR_03 = 37;
	public const int ENVI_REDIRECTOR_04 = 38;
	//************************************************//
	public const int CHAR_JOSE_1_IDLE = 87;
	public const int CHAR_JOSE_IDLE_DRAINED = 89;
	public const int CHAR_BOZ_1_IDLE = 88;
	public const int CHAR_BOZ_IDLE_DRAINED = 90;
	public const int CHAR_CORA_1_IDLE = 39;
	public const int CHAR_CORA_1_DRAINED = 59;
	public const int CHAR_FARADAYDO_1_IDLE = 40;
	public const int CHAR_FARADAYDO_1_DRAINED = 53;
	public const int CHAR_MADRA_1_IDLE = 41;
	public const int CHAR_MADRA_1_DRAINED = 60;
	public const int CHAR_MINER_1_IDLE_DRAINED = 42;
	public const int CHAR_MINER_1_IDLE = 43;
	public const int CHAR_MINER_2_IDLE_DRAINED = 77;
	public const int CHAR_MINER_2_IDLE = 82;
	public const int CHAR_MINER_3_IDLE_DRAINED = 78;
	public const int CHAR_MINER_3_IDLE = 83;
	public const int CHAR_MINER_4_IDLE_DRAINED = 79;
	public const int CHAR_MINER_4_IDLE = 84;
	public const int CHAR_MINER_5_IDLE_DRAINED = 80;
	public const int CHAR_MINER_5_IDLE = 85;
	public const int CHAR_MINER_6_IDLE_DRAINED = 81;
	public const int CHAR_MINER_6_IDLE = 86;
	//************************************************//
	public const int ITEM_TROLEY_LEFT_1 = 44;
	public const int ITEM_TROLEY_RIGHT_1 = 58;
	public const int ENVI_RESCUEZONE_1 = 45;
	//************************************************//
	public const int UI_CORA_ICON = 46;
	public const int UI_MADRA_ICON = 47;
	public const int UI_FARADAYDO_ICON = 48;
	public const int UI_MINER_ICON = 67;
	public const int UI_JOSE_ICON = 89;
	public const int UI_BOZ_ICON = 90;
	//************************************************//
	public const int UI_TIP_RESCUER = 49;
	public const int UI_TIP_BUILDER = 50;
	public const int UI_TIP_ATTACKER = 51;
	public const int UI_TIP_DEMOLISHER = 52;
	//************************************************//
	public const int ICON_METAL = 61;
	public const int ICON_PLASTIC = 62;
	public const int ICON_VINES = 63;
	public const int ICON_RECHARGEOCORES = 64;
	public const int ICON_REDIRECTORS = 65;
	public const int ICON_TECHNOSEED = 66;
	public const int ICON_TIMER = 70;
	//************************************************//
	public const int ENVI_DRILLER_LEFT = 68;
	public const int ENVI_DRILLER_RIGHT = 69;
	//************************************************//
	public const int ENVI_METAL_1_ALONE = 71;
	public const int ENVI_METAL_2_ALONE = 72;
	public const int ENVI_METAL_3_ALONE = 73;
	
	public const int ENVI_PLASTIC_1_ALONE = 74;
	public const int ENVI_PLASTIC_2_ALONE = 75;
	public const int ENVI_PLASTIC_3_ALONE = 76;

	public const int ENVI_TECHNOEGG_1_ALONE = 92;
	public const int ENVI_TECHNOEGG_2_ALONE = 93;
	public const int ENVI_TECHNOEGG_3_ALONE = 94;
	//************************************************//
	public const int POWER_MOTOR = 91;
	//************************************************//
	public static bool isInBorders ( int ID, int[] borders )
	{
		if ( ID >= borders[0] && ID <= borders[1] ) return true;
		else return false;
	}
}
