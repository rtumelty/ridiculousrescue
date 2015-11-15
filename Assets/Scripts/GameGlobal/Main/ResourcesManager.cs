using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour 
{
	//*************************************************************//	
	public static int CURRENT_MAX_PLASTIC = 0;
	public static int CURRENT_MAX_METAL = 0;
	public static int CURRENT_MAX_VINES = 0;
	//*************************************************************//	
	private static ResourcesManager _meInstance;
	public static ResourcesManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_ResourcesManager" ).GetComponent < ResourcesManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		if ( SaveDataManager.saveFileExists ())
		{
			SaveDataManager.setAllValues ();
		}
		else
		{
			SaveDataManager.saveValues ();
		}
		//commented out for redundancy
		//GameGlobalVariables.Stats.GOLD_AVAILABLE += GameGlobalVariables.Stats.NewResources.METAL;
		GameGlobalVariables.Stats.NewResources.reset ();

		if ( GameGlobalVariables.CUT_DOWN_GAME )
		{
			GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS = 3;
			GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS = 30;
		}

		SaveDataManager.saveValues ();
	}
	
	public bool handleBuyWithPremiumCurrency ( int cost, bool justCheck = false )
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.TUTORIAL_MENU ) return true;
		
		if ( GameGlobalVariables.Stats.PREMIUM_CURRENCY - cost < 0 )
		{
			//print ( "not enough money" );
			return false;
		}
		else
		{
			if ( ! justCheck )
			{
				GameGlobalVariables.Stats.PREMIUM_CURRENCY -= cost;
				SaveDataManager.save ( SaveDataManager.PREMIUM_CURRENCY, GameGlobalVariables.Stats.PREMIUM_CURRENCY );
			}
		}
		
		return true;
	}
	
	public bool handleMinusResources ( int metal, int plastic, int vines, bool justCheck = false)
	{
		if ( GameGlobalVariables.Stats.METAL_IN_CONTAINERS - metal < 0 )
		{
			return false;
		}
		
		if ( GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS - plastic < 0 )
		{
			return false;
		}
		
		if ( GameGlobalVariables.Stats.VINES_IN_CONTAINERS - vines < 0 )
		{
			return false;
		}

		if ( ! justCheck )
		{
			foreach ( FLStorageContainerClass contatiner in FLFactoryRoomManager.getInstance ().storageContainersOnLevel )
			{
				switch ( contatiner.type )
				{
					case FLStorageContainerClass.STORAGE_TYPE_METAL:
						GameGlobalVariables.Stats.METAL_IN_CONTAINERS -= metal;
						contatiner.amount -= metal;
						break;

					case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
						GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS -= plastic;
						contatiner.amount -= plastic;
						break;

					case FLStorageContainerClass.STORAGE_TYPE_VINES:
						GameGlobalVariables.Stats.VINES_IN_CONTAINERS -= vines;
						contatiner.amount -= vines;
						break;
				}
			}
		}
		
		SaveDataManager.save ( SaveDataManager.METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS );
		SaveDataManager.save ( SaveDataManager.PLASTIC_IN_CONTAINERS, GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS );
		SaveDataManager.save ( SaveDataManager.VINES_IN_CONTAINERS, GameGlobalVariables.Stats.VINES_IN_CONTAINERS );
		
		return true;
	}
	
	public bool handleAddResources ( int metal, int plastic, int vines )
	{
		foreach ( FLStorageContainerClass contatiner in FLFactoryRoomManager.getInstance ().storageContainersOnLevel )
		{
			switch ( contatiner.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
					GameGlobalVariables.Stats.METAL_IN_CONTAINERS += metal;
					contatiner.amount += metal;
					if ( contatiner.amount > FLStorageContainerClass.LEVELS_STATS_METAL[contatiner.level].capacity )
					{
						GameGlobalVariables.Stats.METAL_IN_CONTAINERS = contatiner.amount = FLStorageContainerClass.LEVELS_STATS_METAL[contatiner.level].capacity;
					}
					break;
				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
					GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS += plastic;
					contatiner.amount += plastic;
					if ( contatiner.amount > FLStorageContainerClass.LEVELS_STATS_PLASTIC[contatiner.level].capacity )
					{
						GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS = contatiner.amount = FLStorageContainerClass.LEVELS_STATS_PLASTIC[contatiner.level].capacity;
					}
					break;
				case FLStorageContainerClass.STORAGE_TYPE_VINES:
					GameGlobalVariables.Stats.VINES_IN_CONTAINERS += vines;
					contatiner.amount += vines;
					if ( contatiner.amount > FLStorageContainerClass.LEVELS_STATS_VINES[contatiner.level].capacity )
					{
						GameGlobalVariables.Stats.VINES_IN_CONTAINERS = contatiner.amount = FLStorageContainerClass.LEVELS_STATS_VINES[contatiner.level].capacity;
					}
					break;
			}
		}
		
		return true;
	}
}
