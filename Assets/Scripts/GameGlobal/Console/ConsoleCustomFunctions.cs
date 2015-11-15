using UnityEngine;
using System.Collections;

public class ConsoleCustomFunctions : MonoBehaviour
{
	public ConsoleManager.CallBackFunction rebootCallBack;
	public ConsoleManager.CallBackFunction voidCallBack;
	public ConsoleManager.CallBackFunction missionsRebootCallBack;
	public bool toggle = false;
	
	private static ConsoleCustomFunctions _meInstance;
	public static ConsoleCustomFunctions getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_ConsoleObject" ).GetComponent < ConsoleCustomFunctions > ();
		}
		
		return _meInstance;
	}
	
	public void CustomStart ()
	{
		rebootCallBack = this.reboot;
		voidCallBack = this.empty;
		missionsRebootCallBack = this.missionsReboot;
	}
	
	public void reboot ()
	{
		Application.LoadLevel ( Application.loadedLevel );
	}

	public void fpsOn ()
	{
		toggle = !toggle;
		if(GameObject.Find("FPS") != null)
		{
			GameObject.Find("FPS").GetComponent < FPSCounter > ().display = toggle;
		}
	}

	public void empty ()
	{
		
	}

	public void loadRescueLevel ()
	{
		int.TryParse ( LevelControl.SELECTED_LEVEL_NAME, out LevelControl.LEVEL_ID );
		Application.LoadLevel ( "01" );
	}
	
	public void enterLab ()
	{
		PlayerPrefs.DeleteKey ( SaveDataManager.MOM_SLOTS_NUMBER_PREFIX + "0,10" );
		Application.LoadLevel ( "FL00ChooseLevel" );
	}

	public void enterTrain ()
	{
		TRLevelControl.LEVEL_ID = 2;
		Application.LoadLevel ( "TR01" );
	}

	public void enterSpineTest ()
	{
		Application.LoadLevel ( "spine" );
	}
	
	public void missionsReboot ()
	{
		if ( Application.loadedLevel != 4 ) Application.LoadLevel ( "FL00ChooseLevel" );
		else FLMissionRoomManager.getInstance ().manageMissionsScreen ();
	}
	
	public void add2Moves ()
	{
	}

	public void showNumbers ()
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLMissionRoomManager.getInstance ().manageMissionsScreen ();
		}
	}
	
	public void resetForTest ()
	{
		GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS = 3;
		GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS = 30;
		GameGlobalVariables.LAB_ENTERED = 0;
		GameGlobalVariables.MINE_ENTERED = 0;
		UnlockLevelSequenceManager.LEVEL_I_AM_ON = 0;
		SaveDataManager.save ( SaveDataManager.LABORATORY_ENTERED, GameGlobalVariables.LAB_ENTERED );

		SaveDataManager.saveValues ();
		Application.LoadLevel ( "00" );
	}
	
	public void resetSavedData ()
	{ 
		PlayerPrefs.DeleteAll ();
		GameGlobalVariables.UNLOCK_ALL_LEVELS = false;
		GameGlobalVariables.LAB_ENTERED = 0;
		GameGlobalVariables.Stats.reset ();
		Application.LoadLevel ( "00" );
	}
	
	public void finishRescueLevel ()
	{
		GlobalVariables.PLUG_CONNECTED = true;
		GlobalVariables.TOY_RESCUED = true;
		Main.getInstance ().checkForWinConditions ();
	}

	public void finishTrainLevel ()
	{
		TRResoultScreen.getInstance ().startResoultScreen ( true );
	}
	
	public void updateStorageMetal ()
	{ 
		FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( GameElements.ICON_METAL );
		currentFLStorageContainerClass.amount = GameGlobalVariables.Stats.METAL_IN_CONTAINERS;
	}
	
	public void updateStoragePlastic ()
	{ 
		FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( GameElements.ICON_PLASTIC );
		currentFLStorageContainerClass.amount = GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS;
	}
	
	public void updateStorageVines ()
	{ 
		FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( GameElements.ICON_VINES );
		currentFLStorageContainerClass.amount = GameGlobalVariables.Stats.VINES_IN_CONTAINERS;
	}
	
	public void updateStorageRechargeocores ()
	{ 
		FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( GameElements.ICON_RECHARGEOCORES );
		currentFLStorageContainerClass.amount = GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS;
	}
	
	public void updateStorageRedirectors ()
	{ 
		FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( GameElements.ICON_REDIRECTORS );
		currentFLStorageContainerClass.amount = GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS;
	}
}
