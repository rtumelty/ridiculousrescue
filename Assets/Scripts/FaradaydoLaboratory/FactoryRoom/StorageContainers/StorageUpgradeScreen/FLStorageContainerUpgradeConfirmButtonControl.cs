using UnityEngine;
using System.Collections;

public class FLStorageContainerUpgradeConfirmButtonControl : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private GameObject _premiumCurrencyPrefab;
	private GameObject _mySelectedContainer;
	public GameObject _myNotEnoughResourcesScreen;
	//*************************************************************//
	void Awake ()
	{
		_premiumCurrencyPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/buyPremiumCurrencyUIScreen" );
		_myNotEnoughResourcesScreen = ( GameObject ) Resources.Load ( "UI/Laboratory/notEnoughResourcesUIPanel" );
	}
	
	void OnMouseUp ()
	{
		handleTouched ();
	}

	private void closeBackground ()
	{
		FLUIControl.currentBlackOutUI.SendMessage ("handleTouched");
	}
	
	private void noResources ()
	{
		_myNotEnoughResourcesScreen =  FLUIControl.getInstance ().createPopup ( _myNotEnoughResourcesScreen,true );
		if ( _myNotEnoughResourcesScreen != null )
		{
			_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().fromContainer = true;
		}
		//print ("active");
	}
	
	private void handleTouched ()
	{
		//print (myStorageContainerClass);
		transform.parent.GetComponent<FLStorageContainerUpgradeScreenControl>().allowed = true;
		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myStorageContainerClass = myStorageContainerClass;
		GameObject.Find ("textGeneral").GetComponent<GameTextControl> ().myKey = "ui_sign_upgrade_in_progress";
		GameObject.Find ("textGeneral").GetComponent<GameTextControl> ().addText = "";
		//=====================================Daves Edit==========================================
		switch (myStorageContainerClass.type) 
		{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
			print ("1");
			//print (FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost.ToString ());
			if ( ResourcesManager.getInstance ().handleMinusResources ( FLStorageContainerClass.LEVELS_STATS_METAL [myStorageContainerClass.level + 1].cost, 0, 0))
			{
				GoogleAnalytics.instance.LogScreen ( "Start upgrading METAL storage - to level " + ( myStorageContainerClass.level + 1 ).ToString ());

				SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
				myStorageContainerClass.upgrading = true;
				myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float)FLStorageContainerClass.LEVELS_STATS_METAL [myStorageContainerClass.level + 1].buildTime;
				SaveDataManager.save (SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position [0].ToString () + "," + myStorageContainerClass.position [1].ToString (), (int)(System.DateTime.UtcNow.Ticks / 10000000));
				_mySelectedContainer = GameObject.Find( "textMetal" );
				_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 2].cost.ToString ();
			}
			else
			{
				closeBackground();
				noResources();
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myContainerUpgradeCost = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost;
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().activeElement = 1;
				myStorageContainerClass  = GameGlobalVariables.lastStorageContainerClass;
			}
			break;

			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
			print ("2");
			if ( ResourcesManager.getInstance ().handleMinusResources (0, FLStorageContainerClass.LEVELS_STATS_PLASTIC [myStorageContainerClass.level + 1].cost, 0))
			{
				GoogleAnalytics.instance.LogScreen ( "Start upgrading PLASTIC storage - to level " + ( myStorageContainerClass.level + 1 ).ToString ());

				SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
				myStorageContainerClass.upgrading = true;
				myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float)FLStorageContainerClass.LEVELS_STATS_PLASTIC [myStorageContainerClass.level + 1].buildTime;
				SaveDataManager.save (SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position [0].ToString () + "," + myStorageContainerClass.position [1].ToString (), (int)(System.DateTime.UtcNow.Ticks / 10000000));
				_mySelectedContainer = GameObject.Find( "textPlastic" );
				_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 2].cost.ToString ();
			}
			else
			{
				closeBackground();
				noResources();
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myContainerUpgradeCost = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost;
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().activeElement = 2;
				myStorageContainerClass  = GameGlobalVariables.lastStorageContainerClass;
			}
			break;

			case FLStorageContainerClass.STORAGE_TYPE_VINES:
			print ("3");
			if ( ResourcesManager.getInstance ().handleMinusResources (0, FLStorageContainerClass.LEVELS_STATS_VINES [myStorageContainerClass.level + 1].cost, 0))
			{
				GoogleAnalytics.instance.LogScreen ( "Start upgrading VINES storage - to level " + ( myStorageContainerClass.level + 1 ).ToString ());

				SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
				myStorageContainerClass.upgrading = true;
				myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float)FLStorageContainerClass.LEVELS_STATS_VINES [myStorageContainerClass.level + 1].buildTime;
				SaveDataManager.save (SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position [0].ToString () + "," + myStorageContainerClass.position [1].ToString (), (int)(System.DateTime.UtcNow.Ticks / 10000000));
					
				_mySelectedContainer = GameObject.Find( "textPlastic" );
				_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 2].cost.ToString ();
			}
			else
			{
				closeBackground();
				noResources();
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myContainerUpgradeCost = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].cost;
				//this 2 value was 3 before, changed to reflect the plastic cost for vine upgrade to storage.
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().activeElement = 2;
				myStorageContainerClass  = GameGlobalVariables.lastStorageContainerClass;
			}
			break;

			default:
			print ("4");
				SoundManager.getInstance ().playSound (SoundManager.CANCEL_BUTTON);
	
				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel (true);
	
				FLUIControl.currentBlackOutUI.SendMessage ("handleTouched");
				FLUIControl.currentPopupUI.transform.position += Vector3.down * 5f;
				FLUIControl.currentPopupUI = null;
	
				FLUIControl.getInstance ().createPopup (_premiumCurrencyPrefab);
					break;
				}
		//=====================================Daves Edit==========================================
		/*if ( ResourcesManager.getInstance ().handleMinusResources ( FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].cost ))
		{
			SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
			
			myStorageContainerClass.upgrading = true;
			myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float) FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].buildTime;
			SaveDataManager.save ( SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position[0].ToString () + "," + myStorageContainerClass.position[1].ToString (), (int) ( System.DateTime.UtcNow.Ticks / 10000000 ));
		}
		else
		{
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
			
			FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
			
			FLUIControl.currentBlackOutUI.SendMessage ( "handleTouched" );
			FLUIControl.currentPopupUI.transform.position += Vector3.down * 5f;
			FLUIControl.currentPopupUI = null;
			
			FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
		}*/
	}

	public void handleFromPremium()
	{
		switch (myStorageContainerClass.type) 
		{
		case FLStorageContainerClass.STORAGE_TYPE_METAL:
		{

			_mySelectedContainer = GameObject.Find( "textMetal" );
			_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 2].cost.ToString ();
		}
			break;
			
		case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
		{

			_mySelectedContainer = GameObject.Find( "textPlastic" );
			_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 2].cost.ToString ();
		}
			break;
			
		case FLStorageContainerClass.STORAGE_TYPE_VINES:
		{

			
			_mySelectedContainer = GameObject.Find( "textPlastic" );
			_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 2].cost.ToString ();
		}
			break;
		}

	}
}
