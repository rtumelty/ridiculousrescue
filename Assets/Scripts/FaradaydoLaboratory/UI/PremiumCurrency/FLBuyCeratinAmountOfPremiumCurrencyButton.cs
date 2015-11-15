using UnityEngine;
using System.Collections;

public class FLBuyCeratinAmountOfPremiumCurrencyButton : MonoBehaviour 
{
	/*	//************************************************************* //	
	public int numberOfSeeds;
	public string productName;
	private GameObject myImage;
	private GameObject myMaxText;
	//************************************************************* //	
	void Start ()
	{
		myMaxText = transform.parent.Find ("maxed").gameObject;
		myImage = this.gameObject;
		InvokeRepeating ("checkCurrency",0,0.25f);
		myMaxText.SetActive (false);
	}

	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( GameGlobalVariables.Stats.PREMIUM_CURRENCY + numberOfSeeds < 99999 )
		{
			if ( FLGlobalVariables.TRANSACTION ) return;
			FLGlobalVariables.TRANSACTION = true;

			SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
#if UNITY_ANDROID
			InAppBillingManager.getInstance ().initPurchase ( productName, callBackOnFinishedTransaction );
#else
			InAppBillingManagerIOS.getInstance ().initPurchase ( productName, callBackOnFinishedTransaction );
			//callBackOnFinishedTransaction ( true );
#endif
			switch ( productName )
			{
			case "01seeds":
				GoogleAnalytics.instance.LogScreen ( "Button Click - Pile of Seeds" );
				break;
			case "02seeds":
				GoogleAnalytics.instance.LogScreen ( "Button Click - Bag of Seeds" );
				break;
			case "03seeds":
				GoogleAnalytics.instance.LogScreen ( "Button Click - Box of Seeds" );
				break;
			case "04seeds":
				GoogleAnalytics.instance.LogScreen ( "Button Click - Toybox of Seeds" );
				break;
			case "05seeds":
				GoogleAnalytics.instance.LogScreen ( "Button Click - Chest of Seeds" );
				break;
			case "06seeds":
				GoogleAnalytics.instance.LogScreen ( "Button Click - Crate of Seeds" );
				break;
			}

		}
		else
		{
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		}
	}

	private void callBackOnFinishedTransaction ( bool success, bool timeOut = false )
	{
		FLGlobalVariables.TRANSACTION = false;
		if ( success )
		{
			GameGlobalVariables.Stats.PREMIUM_CURRENCY += numberOfSeeds;
			SaveDataManager.saveValues ();

			if ( FLUIControl.currentBlackOutUI )
			{
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
			}

			if ( FLUIControl.currentPopupUI )
			{
				FLUIControl.currentPopupUI.AddComponent < HideUIElement > ();
				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( false );
			}

			FLGlobalVariables.POPUP_UI_SCREEN = false;
		}
		else
		{

		}
	}

	public void checkCurrency()
	{
		if(GameGlobalVariables.Stats.PREMIUM_CURRENCY + numberOfSeeds > 99999)
		{
			myImage.renderer.material.color = new Color(1,1,1,0.35f);
			myMaxText.SetActive(true);
		}
		else
		{
			myImage.renderer.material.color = new Color(1,1,1,1);
			myMaxText.SetActive(false);
		}
	}
*/
}