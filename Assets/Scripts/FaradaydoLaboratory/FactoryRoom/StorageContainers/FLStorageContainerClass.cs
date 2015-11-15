using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLStorageContainerClass
{
	//*************************************************************//	
	public const string STORAGE_TYPE_METAL = "Metal";
	public const string STORAGE_TYPE_PLASTIC = "Plastic";
	public const string STORAGE_TYPE_VINES = "Vines";
	public const string STORAGE_TYPE_RECHARGEOCORE = "Recharg-O-Cores";
	public const string STORAGE_TYPE_REDIRECTOR = "Redirectors";
	//*************************************************************//	
	public const int MAXIMUM_LEVEL = 9;
	public static Stats[] LEVELS_STATS_VINES;
	public static Stats[] LEVELS_STATS_METAL;
	public static Stats[] LEVELS_STATS_PLASTIC;
	public static Dictionary < string, Stats > MACHINE_STAND_STATS;
	public static Dictionary < string, string > INFO_TEXT_KEYS;
	//*************************************************************//
	public static bool isMachineStand ( string type )
	{
		switch ( type )
		{
			case STORAGE_TYPE_RECHARGEOCORE:
			case STORAGE_TYPE_REDIRECTOR:
				return true;
		}
		
		return false;
	}
	//*************************************************************//	
	public class Stats
	{
		public int capacity;
		public int cost;
		public int buildTime;
		
		public Stats ( int capacityValue, int costValue, int buildTimeValue )
		{
			capacity = capacityValue;
			cost = costValue;
			buildTime = buildTimeValue;
		}
	}
	//*************************************************************//	
	public int level;
	public string type;
	public int amount;
	public int[] position;
	public Texture2D iconTexture;
	public GameObject containerObject;
	public bool upgrading = false;
	public float upgradeTime;
	public float timeTotal;
	public Texture2D[] myCurrentLevelTextures; 
	//*************************************************************//	
	public FLStorageContainerClass ( int levelValue, string typeValue, int amountValue, int[] positionValue, Texture2D iconTextureValue )
	{
		level = levelValue;
		type = typeValue;
		amount = amountValue;
		position = positionValue;
		iconTexture = iconTextureValue;
	}
}
