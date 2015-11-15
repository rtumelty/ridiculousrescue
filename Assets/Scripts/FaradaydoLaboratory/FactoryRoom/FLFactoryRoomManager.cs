using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLFactoryRoomManager : MonoBehaviour 
{
	//*************************************************************//
	public Texture2D[] powerBarPaneltextures;
	
	public List < Texture2D > rechargeOCoreContainerTexture;
	public List < Texture2D > redirectorContainerTexture;
	public Texture2D momTextureOff;
	public Texture2D momTextureWorking;
	public Texture2D momTextureReady;
	
	public Texture2D buttonGrayedOutTexture;
	public Texture2D buttonGreenTexture;

	public Texture2D buttonGrayedOutTextureOnMouseUp;
	public Texture2D buttonGreenTextureOnMouseUp;
	
	public Texture2D momWorkingSlotTexture;
	public Texture2D momNotWorkingSlotTexture;
	
	public GameObject storageComboUIShortPrefab;
	//=========================================Daves Edit===========================================
	public GameObject storageComboUIShortMetalPrefab;
	public GameObject storageComboUIShortPlasticPrefab;
	public GameObject storageComboUIShortVinesPrefab;
	public GameObject storageComboUIStorageUpgradeScreenPlasticPrefab;
	//=========================================Daves Edit===========================================
	public GameObject storageComboUIPanelPrefab;
	public GameObject storageComboUIStorageInfoScreenPrefab;
	public GameObject storageComboUIStorageUpgradeScreenPrefab;
	public GameObject momUIComboPrefab;
	public GameObject momUIToolTipPrefab;
	public GameObject momNotEnoughResourcesPrefab;
	
	public List < FLStorageContainerClass > storageContainersOnLevel;
	public List < FL_MOMClass > momsOnLevel;
	
	public GameObject currentMoMRechargeOCoreObject;
	//*************************************************************//
	private GameObject _tileInteractivePrefab;
	private GameObject _tileInteractiveMomPrefab;
	//*************************************************************//
	private static FLFactoryRoomManager _meInstance;
	public static FLFactoryRoomManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_FactoryRoomManagerObject" ).GetComponent < FLFactoryRoomManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	void Awake ()
	{
		rechargeOCoreContainerTexture = new List < Texture2D > ();
		redirectorContainerTexture = new List < Texture2D > ();
		
		UnityEngine.Object[] textureObjectsMachineStand = Resources.LoadAll ( "Textures/FactoryRoom/StorageContainers/RechargeOCore", typeof ( Texture2D ));
		foreach ( UnityEngine.Object textureObject in textureObjectsMachineStand )
		{
			if ( textureObject is Texture2D )
			{
				rechargeOCoreContainerTexture.Add (( Texture2D ) textureObject );
			}
		}
		
		textureObjectsMachineStand = Resources.LoadAll ( "Textures/FactoryRoom/StorageContainers/Redirector", typeof ( Texture2D ));
		foreach ( UnityEngine.Object textureObject in textureObjectsMachineStand )
		{
			if ( textureObject is Texture2D )
			{
				redirectorContainerTexture.Add (( Texture2D ) textureObject );
			}
		}
	}
	
	public void CustomStart ()
	{
		storageComboUIShortPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/StorageComboUIShort");
		//=========================================Daves Edit===========================================

		storageComboUIShortMetalPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/StorageComboUIShortMetal");
		storageComboUIShortPlasticPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/StorageComboUIShortPlastic");
		storageComboUIShortVinesPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/StorageComboUIShortVines");
		storageComboUIStorageUpgradeScreenPlasticPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/containerUIUpgradeScreenPlastic" );
		//=========================================Daves Edit===========================================
		storageComboUIPanelPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/containerUIPanel" );
		storageComboUIStorageInfoScreenPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/containerUIInfoScreen" );
		storageComboUIStorageUpgradeScreenPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/containerUIUpgradeScreen" );
		momUIComboPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/momUICombo" );
		momUIToolTipPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/momToolTipUIComboPanel" );
		momNotEnoughResourcesPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/notEnoughResourcesUIPanel" );
		
		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractiveStorage" );
		_tileInteractiveMomPrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractiveMom" );
		
		setTheStatsForStorgaeContainers ();
		setTheCostValuesForElements ();
		createObjects ();
	}
	
	private void setTheStatsForStorgaeContainers ()
	{
		FLStorageContainerClass.LEVELS_STATS_METAL = new FLStorageContainerClass.Stats[10];
		FLStorageContainerClass.LEVELS_STATS_METAL[0] = new FLStorageContainerClass.Stats ( 12, 0, 0 );
		FLStorageContainerClass.LEVELS_STATS_METAL[1] = new FLStorageContainerClass.Stats ( 20, 12, 1 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[2] = new FLStorageContainerClass.Stats ( 28, 20, 2 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[3] = new FLStorageContainerClass.Stats ( 36, 28, 3 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[4] = new FLStorageContainerClass.Stats ( 44, 36, 4 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[5] = new FLStorageContainerClass.Stats ( 52, 44, 5 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[6] = new FLStorageContainerClass.Stats ( 60, 52, 6 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[7] = new FLStorageContainerClass.Stats ( 68, 60, 7 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[8] = new FLStorageContainerClass.Stats ( 76, 68, 8 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_METAL[9] = new FLStorageContainerClass.Stats ( 84, 76, 9 * 60 * 60 );

		FLStorageContainerClass.LEVELS_STATS_PLASTIC = new FLStorageContainerClass.Stats[10];
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[0] = new FLStorageContainerClass.Stats ( 18, 0, 0 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[1] = new FLStorageContainerClass.Stats ( 38, 18, 1 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[2] = new FLStorageContainerClass.Stats ( 58, 38, 2 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[3] = new FLStorageContainerClass.Stats ( 78, 58, 3 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[4] = new FLStorageContainerClass.Stats ( 98, 78, 4 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[5] = new FLStorageContainerClass.Stats ( 118, 98, 5 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[6] = new FLStorageContainerClass.Stats ( 138, 118, 6 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[7] = new FLStorageContainerClass.Stats ( 158, 138, 7 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[8] = new FLStorageContainerClass.Stats ( 178, 158, 8 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_PLASTIC[9] = new FLStorageContainerClass.Stats ( 198, 178, 9 * 60 * 60 );

		FLStorageContainerClass.LEVELS_STATS_VINES = new FLStorageContainerClass.Stats[10];
		FLStorageContainerClass.LEVELS_STATS_VINES[0] = new FLStorageContainerClass.Stats ( 6, 0, 0 );
		FLStorageContainerClass.LEVELS_STATS_VINES[1] = new FLStorageContainerClass.Stats ( 9, 18, 1 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[2] = new FLStorageContainerClass.Stats ( 12, 38, 2 * 60 * 60);
		FLStorageContainerClass.LEVELS_STATS_VINES[3] = new FLStorageContainerClass.Stats ( 15, 58, 3 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[4] = new FLStorageContainerClass.Stats ( 18, 78, 4 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[5] = new FLStorageContainerClass.Stats ( 21, 98, 5 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[6] = new FLStorageContainerClass.Stats ( 24, 118, 6 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[7] = new FLStorageContainerClass.Stats ( 27, 138, 7 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[8] = new FLStorageContainerClass.Stats ( 30, 158, 8 * 60 * 60 );
		FLStorageContainerClass.LEVELS_STATS_VINES[9] = new FLStorageContainerClass.Stats ( 33, 178, 9 * 60 * 60 );

		FLStorageContainerClass.MACHINE_STAND_STATS = new Dictionary < string, FLStorageContainerClass.Stats > ();
		FLStorageContainerClass.MACHINE_STAND_STATS.Add ( FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE, new FLStorageContainerClass.Stats ( 3, 20, 8 * 60 * 60 ));
		FLStorageContainerClass.MACHINE_STAND_STATS.Add ( FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR, new FLStorageContainerClass.Stats ( 99, 20, 8 * 60 * 60 ));
	}
	
	private void setTheCostValuesForElements ()
	{
		FLElementsConstructionCosts.COSTS_VALUES = new Dictionary<int, FLElementsConstructionCosts.Cost > ();
		FLElementsConstructionCosts.COSTS_VALUES.Add ( GameElements.ICON_RECHARGEOCORES , new FLElementsConstructionCosts.Cost ( 4, 6, 2, 25, 60 * 30 ));
		FLElementsConstructionCosts.COSTS_VALUES.Add ( GameElements.ICON_REDIRECTORS, new FLElementsConstructionCosts.Cost ( 2, 4, 1, 15, 60 * 10 ));
	}
	
	private void createObjects ()
	{
		FLStorageContainerClass.INFO_TEXT_KEYS = new Dictionary < string, string > ();
		FLStorageContainerClass.INFO_TEXT_KEYS.Add ( FLStorageContainerClass.STORAGE_TYPE_METAL, "metal_container_info_text" );
		FLStorageContainerClass.INFO_TEXT_KEYS.Add ( FLStorageContainerClass.STORAGE_TYPE_PLASTIC, "plastic_container_info_text");
		FLStorageContainerClass.INFO_TEXT_KEYS.Add ( FLStorageContainerClass.STORAGE_TYPE_VINES, "vines_container_info_text" );
		FLStorageContainerClass.INFO_TEXT_KEYS.Add ( FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE, "rechargerocores_container_info_text" );
		FLStorageContainerClass.INFO_TEXT_KEYS.Add ( FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR, "redirectors_container_info_text" );
		
		storageContainersOnLevel = new List < FLStorageContainerClass > ();
		
		int levelOfCurrentStorage = SaveDataManager.getValue ( SaveDataManager.STORAGE_CONTAINER_LEVEL_PREFIX + "0,14" );
		storageContainersOnLevel.Add ( new FLStorageContainerClass ( levelOfCurrentStorage, FLStorageContainerClass.STORAGE_TYPE_METAL, GameGlobalVariables.Stats.METAL_IN_CONTAINERS, new int[2] { 0, 14 }, FLLevelControl.getInstance ().gameElements[GameElements.ICON_METAL] ));
			
		levelOfCurrentStorage = SaveDataManager.getValue ( SaveDataManager.STORAGE_CONTAINER_LEVEL_PREFIX + "2,14" );
		storageContainersOnLevel.Add ( new FLStorageContainerClass ( levelOfCurrentStorage, FLStorageContainerClass.STORAGE_TYPE_PLASTIC, GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS, new int[2] { 2, 14 }, FLLevelControl.getInstance ().gameElements[GameElements.ICON_PLASTIC] ));
		
		levelOfCurrentStorage = SaveDataManager.getValue ( SaveDataManager.STORAGE_CONTAINER_LEVEL_PREFIX + "4,14" );
		storageContainersOnLevel.Add ( new FLStorageContainerClass ( levelOfCurrentStorage, FLStorageContainerClass.STORAGE_TYPE_VINES, GameGlobalVariables.Stats.VINES_IN_CONTAINERS, new int[2] { 4, 14 }, FLLevelControl.getInstance ().gameElements[GameElements.ICON_VINES] ));
		
		storageContainersOnLevel.Add ( new FLStorageContainerClass ( 0, FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE, GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS, new int[2] { 6, 14 }, FLLevelControl.getInstance ().gameElements[GameElements.ICON_RECHARGEOCORES] ));
		storageContainersOnLevel.Add ( new FLStorageContainerClass ( 0, FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR, GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS, new int[2] { 8, 14 }, FLLevelControl.getInstance ().gameElements[GameElements.ICON_REDIRECTORS] ));
		
		foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
		{
			string path = "";
			switch ( contatiner.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
					path = "Textures/FactoryRoom/StorageContainers/Metal";
					break;
				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
					path = "Textures/FactoryRoom/StorageContainers/Plastic";
					break;
				case FLStorageContainerClass.STORAGE_TYPE_VINES:
					path = "Textures/FactoryRoom/StorageContainers/Vines";
					break;
			}
			
			contatiner.myCurrentLevelTextures = new Texture2D[5];
			UnityEngine.Object[] textureObjects = Resources.LoadAll ( path + "/lvl" + ( contatiner.level + 1 ).ToString (), typeof ( Texture2D ));
			for ( int i = 0; i <  textureObjects.Length; i++ )
			{
				if ( textureObjects[i] is Texture2D )
				{
					contatiner.myCurrentLevelTextures[i] = ( Texture2D ) textureObjects[i];
				}
			}
			
			GameObject interactiveTileInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 ((float) contatiner.position[0], (float) ( FLLevelControl.LEVEL_HEIGHT - contatiner.position[1] ), (float) contatiner.position[1] - 0.5f ), _tileInteractivePrefab.transform.rotation );
			interactiveTileInstant.tag = ("ForMovement");
			GameObject interactiveTileInstantMesh = interactiveTileInstant.transform.Find ( "tile" ).gameObject;
			contatiner.containerObject = interactiveTileInstant;
			
			interactiveTileInstantMesh.AddComponent < SelectedComponenent > ();

			//======================Daves Edit====================================
			interactiveTileInstantMesh.AddComponent < FLInteractiveDestroySelectedCharHighlight > ();
			//======================Daves Edit====================================

			IComponent currentIComponent = interactiveTileInstantMesh.AddComponent < IComponent > ();
			currentIComponent.myID = GameElements.STORAGE_OBJECT;
			
			FLStorageContainerControl currentStorageContainerControl = interactiveTileInstantMesh.AddComponent < FLStorageContainerControl > ();
			currentStorageContainerControl.myStorageContainerClass = contatiner;
			
			int startTimeForThisStorage = SaveDataManager.getValue ( SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + contatiner.position[0].ToString () + "," + contatiner.position[1].ToString ());

			if ( startTimeForThisStorage != 0 ) contatiner.upgrading = true;

			switch ( contatiner.type )
			{
			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				contatiner.timeTotal = ( float ) FLStorageContainerClass.LEVELS_STATS_PLASTIC[contatiner.level + 1].buildTime;
				break;
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
				contatiner.timeTotal = ( float ) FLStorageContainerClass.LEVELS_STATS_METAL[contatiner.level + 1].buildTime;
				break;
			case FLStorageContainerClass.STORAGE_TYPE_VINES:
				contatiner.timeTotal = ( float ) FLStorageContainerClass.LEVELS_STATS_VINES[contatiner.level + 1].buildTime;
				break;
			}

			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0]][contatiner.position[1]] = GameElements.UNWALKABLE;
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0] + 1][contatiner.position[1]] = GameElements.UNWALKABLE;
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0]][contatiner.position[1] + 1] = GameElements.UNWALKABLE;
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0] + 1][contatiner.position[1] + 1] = GameElements.UNWALKABLE;
			
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0]][contatiner.position[1]] = interactiveTileInstant;
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0] + 1][contatiner.position[1]] = interactiveTileInstant;
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0]][contatiner.position[1] + 1] = interactiveTileInstant;
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][contatiner.position[0] + 1][contatiner.position[1] + 1] = interactiveTileInstant;
		}
		
		momsOnLevel = new List < FL_MOMClass > ();
		
		int unblockedSlotsForThisMom = SaveDataManager.getValue ( SaveDataManager.MOM_SLOTS_NUMBER_PREFIX + "0,10" );
		if ( unblockedSlotsForThisMom == 0 ) unblockedSlotsForThisMom = 2;
		momsOnLevel.Add ( new FL_MOMClass ( 6, unblockedSlotsForThisMom, new int[2] { 0, 10 } ));
		SaveDataManager.save ( SaveDataManager.MOM_SLOTS_NUMBER_PREFIX + "0,10", unblockedSlotsForThisMom );
		
		foreach ( FL_MOMClass mom in momsOnLevel )
		{
			for ( int i = 0; i < mom.numberOfSlotsUnblocked; i++ )
			{
				int elementInTheSlot = SaveDataManager.getValue ( SaveDataManager.MOM_SLOTS_ELEMENT_PREFIX + ( i ).ToString () + mom.position[0].ToString () + "," + mom.position[1].ToString ());
				if ( elementInTheSlot != GameElements.EMPTY )
				{
					mom.myProductionSlots.Add ( new FL_MOMControl.ProductionSlot ( elementInTheSlot, FLElementsConstructionCosts.COSTS_VALUES[elementInTheSlot].time ));
				}
			}
			
			GameObject interactiveTileInstant = ( GameObject ) Instantiate ( _tileInteractiveMomPrefab, new Vector3 ((float) mom.position[0], (float) ( FLLevelControl.LEVEL_HEIGHT - mom.position[1] + 4.5f ), (float) mom.position[1] - 0.5f ), _tileInteractiveMomPrefab.transform.rotation );
			interactiveTileInstant.tag = ("ForMovement");
			GameObject interactiveTileInstantMesh = interactiveTileInstant.transform.Find ( "tile" ).gameObject;
			mom.momObject = interactiveTileInstant;
			
			interactiveTileInstantMesh.renderer.material.mainTexture = momTextureOff;
			
			interactiveTileInstantMesh.AddComponent < SelectedComponenent > ();
			interactiveTileInstantMesh.AddComponent < FL_MOMAnimationComponent > ();

			//======================Daves Edit====================================
			interactiveTileInstantMesh.AddComponent < FLInteractiveDestroySelectedCharHighlight > ();
			//======================Daves Edit====================================
			
			IComponent currentIComponent = interactiveTileInstantMesh.AddComponent < IComponent > ();
			currentIComponent.myID = GameElements.MOM_OBJECT;
			
			FL_MOMControl currentFL_MOMControl = interactiveTileInstantMesh.AddComponent < FL_MOMControl > ();
			currentFL_MOMControl.myMomClass = mom;
			
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0]][mom.position[1]] = GameElements.UNWALKABLE;
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0] + 1][mom.position[1]] = GameElements.UNWALKABLE;
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0]][mom.position[1] + 1] = GameElements.UNWALKABLE;
			FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0] + 1][mom.position[1] + 1] = GameElements.UNWALKABLE;
			
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0]][mom.position[1]] = interactiveTileInstant;
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0] + 1][mom.position[1]] = interactiveTileInstant;
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0]][mom.position[1] + 1] = interactiveTileInstant;
			FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][mom.position[0] + 1][mom.position[1] + 1] = interactiveTileInstant;
		}
	}	
	
	public FLStorageContainerClass handleFindStoreForReadyElement ( int elementID )
	{
		switch ( elementID )
		{
			case GameElements.ICON_RECHARGEOCORES:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
							if (( contatiner.amount < FLStorageContainerClass.MACHINE_STAND_STATS[contatiner.type].capacity ) || ( FLGlobalVariables.TUTORIAL_MENU ))
							{
								return ( contatiner );
							}
							break;
					}
				}
				break;
			case GameElements.ICON_REDIRECTORS:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
							if ( contatiner.amount < FLStorageContainerClass.MACHINE_STAND_STATS[contatiner.type].capacity )
							{
								return ( contatiner );
							}
							break;
					}
				}
				break;			
		}
		
		return ( null );
	}
	
	public FLStorageContainerClass findStorageOfThisElement ( int elementID )
	{
		switch ( elementID )
		{
			case GameElements.ICON_RECHARGEOCORES:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
							return ( contatiner );
					}
				}
				break;
			case GameElements.ICON_REDIRECTORS:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
							return ( contatiner );
					}
				}
				break;
			case GameElements.ICON_METAL:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_METAL:
							return ( contatiner );
					}
				}
				break;
			case GameElements.ICON_PLASTIC:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
							return ( contatiner );
					}
				}
				break;
			case GameElements.ICON_VINES:
				foreach ( FLStorageContainerClass contatiner in storageContainersOnLevel )
				{
					switch ( contatiner.type )
					{
						case FLStorageContainerClass.STORAGE_TYPE_VINES:
							return ( contatiner );
					}
				}
				break;
		}
		
		return ( null );
	}
}
