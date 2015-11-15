using UnityEngine;
using System.Collections;

public class TapBuyNowButtonControl : MonoBehaviour 
{
	//*************************************************************//
	private GameObject _premiumCurrencyPrefab;
	//*************************************************************//
	void Awake ()
	{
		_premiumCurrencyPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/buyPremiumCurrencyUIScreen" );
	}

	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( 250 ))
		{
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );

			FLGlobalVariables.POPUP_UI_SCREEN = true;
			FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
			if ( FLUIControl.currentPopupUI ) Destroy ( FLUIControl.currentPopupUI );

			FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
		}
		else
		{
			SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
			FLMissionScreenMapDialogManager.getInstance ().startUnlockingProcedure ();
		}
	}
}
