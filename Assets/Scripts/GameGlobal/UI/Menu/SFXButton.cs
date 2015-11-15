using UnityEngine;
using System.Collections;

public class SFXButton : MonoBehaviour 
{
	//*************************************************************//
	private Texture2D _textureOn;
	private Texture2D _textureOff;
	//*************************************************************//
	void Awake ()
	{
		_textureOn = ( Texture2D ) Resources.Load ( "Textures/UI_v3/ui_button_sfx_on" );
		_textureOff = ( Texture2D ) Resources.Load ( "Textures/UI_v3/ui_button_sfx_off" );
		
		if ( SoundManager.SFX_ON )
		{
			renderer.material.mainTexture = _textureOn;
		}
		else
		{
			renderer.material.mainTexture = _textureOff;
		}
	}
	
	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		SoundManager.SFX_ON = ! SoundManager.SFX_ON;
		
		SaveDataManager.save ( SaveDataManager.SFX_ON_OFF_PREFIX, SoundManager.SFX_ON ? 1 : 0 );
		
		if ( SoundManager.SFX_ON )
		{
			//FlurryAnalytics.Instance ().LogEvent ( "Turning SFX ON" );
			GoogleAnalytics.instance.LogScreen ( "Turning SFX ON" );
		}
		else
		{
			//FlurryAnalytics.Instance ().LogEvent ( "Turning SFX OFF" );
			GoogleAnalytics.instance.LogScreen ( "Turning SFX OFF" );
		}
		
		if ( SoundManager.SFX_ON )
		{
			renderer.material.mainTexture = _textureOn;
			SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		}
		else
		{
			renderer.material.mainTexture = _textureOff;
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		}
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			FLUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
	}
}
