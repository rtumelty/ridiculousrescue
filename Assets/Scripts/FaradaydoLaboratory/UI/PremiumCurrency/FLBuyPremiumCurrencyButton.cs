using UnityEngine;
using System.Collections;

public class FLBuyPremiumCurrencyButton : MonoBehaviour 
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
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY && FLGlobalVariables.MISSION_SCREEN ) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && !FLGlobalVariables.TUTORIAL_MENU)
		{
			FLUIControl.getInstance ().unselectCurrentGameElement ();
			FLUIControl.getInstance ().destoryCurrentUIElement ();
		}
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			UIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( true );
			FLUIControl.getInstance ().createPopup ( _premiumCurrencyPrefab );
		}
	}
}
