using UnityEngine;
using System.Collections;

public class FL_MOMBuyWithPremiumButton : MonoBehaviour 
{
	//*************************************************************//
	public static int NEW_SLOT_COST_03 = 100;
	public static int NEW_SLOT_COST_04 = 150;
	public static int NEW_SLOT_COST_05 = 200;
	public static int NEW_SLOT_COST_06 = 250;
	//*************************************************************//
	public FL_MOMClass myMomClass;
	public bool buyElement = true;
	//*************************************************************//
	private GameObject _premiumCurrencyPrefab;
	private float _countUpdate = 0f;
	//*************************************************************//
	void Awake ()
	{
		_premiumCurrencyPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/buyPremiumCurrencyUIScreen" );
	}
	
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if( myMomClass.myProductionSlots.Count > 0 && myMomClass.myProductionSlots[0].elementID == 64)
		{
			//GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION --;
		}

		FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();

		myMomClass.myMomPanelControl.update ();

		if ( buyElement )
		{
			if (( myMomClass.myProductionSlots != null ) && ( myMomClass.myProductionSlots.Count > 0 ) && ( myMomClass.myProductionSlots[0] != null ))
			{
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( FLElementsConstructionCosts.COSTS_VALUES[myMomClass.myProductionSlots[0].elementID].premiumCurrency ))
				{
					SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
					FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
					FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
					myMomClass.myMomControl.externallyCreateReadyElement ();
				}
			}
		}
		else
		{
			switch ( myMomClass.numberOfSlotsUnblocked )
			{
			case 2:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_03 ))
				{
					SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
					FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
					FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
					myMomClass.myMomPanelControl.buyNewSlot ();
				}
				break;
			case 3:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_04 ))
				{
					SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
					FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
					FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
					myMomClass.myMomPanelControl.buyNewSlot ();
				}
				break;
			case 4:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_05 ))
				{
					SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
					FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
					FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
					myMomClass.myMomPanelControl.buyNewSlot ();
				}
				break;
			case 5:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_06 ))
				{
					SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
					FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
					FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
				}
				else
				{
					SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
					myMomClass.myMomPanelControl.buyNewSlot ();
				}
				break;
			}
		}
	}
	
	void Update ()
	{
		if ( ! buyElement )
		{
			switch ( myMomClass.numberOfSlotsUnblocked )
			{
			case 2:
				transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = NEW_SLOT_COST_03.ToString ();
				break;
			case 3:
				transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = NEW_SLOT_COST_04.ToString ();
				break;
			case 4:
				transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = NEW_SLOT_COST_05.ToString ();
				break;
			case 5:
				transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = NEW_SLOT_COST_06.ToString ();
				break;
			}
		}

		_countUpdate -= Time.deltaTime;

		if ( _countUpdate > 0f ) return;

		_countUpdate = 1f;

		if ( buyElement )
		{
			if (( myMomClass != null ) && ( myMomClass.myProductionSlots != null ) && ( myMomClass.myProductionSlots.Count > 0 ) && ( myMomClass.myProductionSlots[0] != null ))
			{
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( FLElementsConstructionCosts.COSTS_VALUES[myMomClass.myProductionSlots[0].elementID].premiumCurrency, true ))
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}
			}
		}
		else
		{
			switch ( myMomClass.numberOfSlotsUnblocked )
			{
			case 2:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_03, true ))
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}
				break;
			case 3:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_04, true ))
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}
				break;
			case 4:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_05, true ))
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}
				break;
			case 5:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( NEW_SLOT_COST_06, true ))
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					transform.Find ( "textAmount" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}
				break;
			}
		}
	}
}
