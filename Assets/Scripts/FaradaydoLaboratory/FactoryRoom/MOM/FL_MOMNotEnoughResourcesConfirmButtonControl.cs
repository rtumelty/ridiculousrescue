using UnityEngine;
using System.Collections;

public class FL_MOMNotEnoughResourcesConfirmButtonControl : MonoBehaviour 
{
	//*************************************************************//
	public int myElementID;
	public int cost;
	public FL_MOMClass myMomClass;
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private GameObject _premiumCurrencyPrefab;
	//*************************************************************//
	void Awake ()
	{
		_premiumCurrencyPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/buyPremiumCurrencyUIScreen" );
	}

	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		if(myElementID == 64 && (GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION + GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS) < 3)	
		{
			if ( ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( cost ))
			{
				SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
				
				transform.parent.gameObject.AddComponent < HideUIElement > ();
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				
				int metalAdd = -( GameGlobalVariables.Stats.METAL_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal );
				int plasticAdd = -( GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic );
				int vinesAdd = -( GameGlobalVariables.Stats.VINES_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines );
				
				ResourcesManager.getInstance ().handleAddResources ( metalAdd > 0 ? metalAdd : 0 , plasticAdd > 0 ? plasticAdd : 0, vinesAdd > 0 ? vinesAdd : 0	);
				
				myMomClass.myMomPanelControl.startProduction ( myElementID );
				GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION++;
				print ("building" + GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION);
				print ("made" + GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS);
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());

				FLGlobalVariables.POPUP_UI_SCREEN = false;

				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
				FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab, true );
			}
		}
		else if(myElementID == 65)
		{
			if ( ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( cost ))
			{
				SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
				
				transform.parent.gameObject.AddComponent < HideUIElement > ();
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				
				int metalAdd = -( GameGlobalVariables.Stats.METAL_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal );
				int plasticAdd = -( GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic );
				int vinesAdd = -( GameGlobalVariables.Stats.VINES_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines );
				
				ResourcesManager.getInstance ().handleAddResources ( metalAdd > 0 ? metalAdd : 0 , plasticAdd > 0 ? plasticAdd : 0, vinesAdd > 0 ? vinesAdd : 0	);
				
				myMomClass.myMomPanelControl.startProduction ( myElementID );
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				
				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
				FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab, true );
			}
		}
		if(transform.parent.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().fromContainer == true)
		{
			if ( ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( cost ))
			{
				SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
				
				transform.parent.gameObject.AddComponent < HideUIElement > ();
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				int activeElement = transform.parent.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().activeElement;
				myStorageContainerClass = GameGlobalVariables.lastStorageContainerClass;
				switch(activeElement)
				{
					case 1:
					{
						int metalAdd = -(transform.parent.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myContainerUpgradeCost);
						int plasticAdd = 0;
						int vinesAdd = 0;
						ResourcesManager.getInstance ().handleAddResources ( metalAdd > 0 ? metalAdd : 0 , plasticAdd > 0 ? plasticAdd : 0, vinesAdd > 0 ? vinesAdd : 0	);
						GoogleAnalytics.instance.LogScreen ( "Start upgrading METAL storage - to level " + ( myStorageContainerClass.level + 1 ).ToString ());
						
						SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
						myStorageContainerClass.upgrading = true;
						myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float)FLStorageContainerClass.LEVELS_STATS_METAL [myStorageContainerClass.level + 1].buildTime;
						SaveDataManager.save (SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position [0].ToString () + "," + myStorageContainerClass.position [1].ToString (), (int)(System.DateTime.UtcNow.Ticks / 10000000));
					}
						break;
					case 2:
					{
						int plasticAdd = -(transform.parent.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myContainerUpgradeCost);
						int metalAdd = 0;
						int vinesAdd = 0;
						ResourcesManager.getInstance ().handleAddResources ( metalAdd > 0 ? metalAdd : 0 , plasticAdd > 0 ? plasticAdd : 0, vinesAdd > 0 ? vinesAdd : 0	);
						GoogleAnalytics.instance.LogScreen ( "Start upgrading PLASTIC storage - to level " + ( myStorageContainerClass.level + 1 ).ToString ());
						
						SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
						myStorageContainerClass.upgrading = true;
						myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float)FLStorageContainerClass.LEVELS_STATS_PLASTIC [myStorageContainerClass.level + 1].buildTime;
						SaveDataManager.save (SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position [0].ToString () + "," + myStorageContainerClass.position [1].ToString (), (int)(System.DateTime.UtcNow.Ticks / 10000000));
					}
						break;
					case 3:
					{
						int vinesAdd = -(transform.parent.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myContainerUpgradeCost);
						int plasticAdd = 0;
						int metalAdd = 0;
						ResourcesManager.getInstance ().handleAddResources ( metalAdd > 0 ? metalAdd : 0 , plasticAdd > 0 ? plasticAdd : 0, vinesAdd > 0 ? vinesAdd : 0	);
						GoogleAnalytics.instance.LogScreen ( "Start upgrading VINES storage - to level " + ( myStorageContainerClass.level + 1 ).ToString ());
						
						SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
						myStorageContainerClass.upgrading = true;
						myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal = (float)FLStorageContainerClass.LEVELS_STATS_VINES [myStorageContainerClass.level + 1].buildTime;
						SaveDataManager.save (SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position [0].ToString () + "," + myStorageContainerClass.position [1].ToString (), (int)(System.DateTime.UtcNow.Ticks / 10000000));
					}
						break;
				}
			}
			else
			{
				SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				
				FLGlobalVariables.POPUP_UI_SCREEN = false;
				
				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
				FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab, true );
			}
		}
	}
}
