using UnityEngine;
using System.Collections;

public class TapUnlockWithBuyButtonControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		/*if ( FLGlobalVariables.TRANSACTION ) return;
		FLGlobalVariables.TRANSACTION = true;

#if UNITY_ANDROID
		InAppBillingManager.getInstance ().initPurchase ( "unlocklevel13", callBackOnFinishedTransaction );
#else
		InAppBillingManagerIOS.getInstance ().initPurchase ( "unlocklevel13ios", callBackOnFinishedTransaction );
		//callBackOnFinishedTransaction ( true );
#endif*/
		GoogleAnalytics.instance.LogScreen ( "Button Click - Unlock Level 13" );
		if(GameGlobalVariables.Stats.METAL_IN_CONTAINERS >= GameGlobalVariables.Stats.COST_OF_MAP_UNLOCK)
		{
			GameGlobalVariables.Stats.METAL_IN_CONTAINERS -= GameGlobalVariables.Stats.COST_OF_MAP_UNLOCK;
			SaveDataManager.save (SaveDataManager.METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS);
			GoogleAnalytics.instance.LogScreen ( "User bought UNLOCK LEVEL 13 and unlocked level 13" );
			FLMissionScreenMapDialogManager.getInstance ().startUnlockingProcedure ();
		}
		else if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
		{
			FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
			Camera.main.transform.Find("world").transform.Find("MapUIPrefab").transform.Find("UIRight").transform.Find("GoldAddButton").SendMessage("Activate");
			GameObject.Find("GoldAddButton").SendMessage("Activate", transform.parent.transform.parent);
		}
	}

	private void callBackOnFinishedTransaction ( bool success, bool timeOut = false )
	{
		FLGlobalVariables.TRANSACTION = false;
		if ( success )
		{
			GoogleAnalytics.instance.LogScreen ( "User bought UNLOCK LEVEL 13 and unlocked level 13" );
			FLMissionScreenMapDialogManager.getInstance ().startUnlockingProcedure ();
		}
		else
		{
			
		}
	}
}
