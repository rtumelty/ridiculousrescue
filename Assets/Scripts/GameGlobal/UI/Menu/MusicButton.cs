using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour 
{
	//*************************************************************//
	private Texture2D _textureOn;
	private Texture2D _textureOff;
	//*************************************************************//
	void Awake ()
	{
		_textureOn = ( Texture2D ) Resources.Load ( "Textures/UI_v3/ui_button_music_on" );
		_textureOff = ( Texture2D ) Resources.Load ( "Textures/UI_v3/ui_button_music_off" );
		
		if ( SoundManager.MUSIC_ON )
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
		SoundManager.MUSIC_ON = ! SoundManager.MUSIC_ON;
		
		if ( SoundManager.MUSIC_ON )
		{
			renderer.material.mainTexture = _textureOn;
			SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		}
		else
		{
			renderer.material.mainTexture = _textureOff;
			SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		}
		
		if ( SoundManager.MUSIC_ON )
		{
			//FlurryAnalytics.Instance ().LogEvent ( "Turning music ON" );
			GoogleAnalytics.instance.LogScreen ( "Turning music ON" );
		}
		else
		{
			//FlurryAnalytics.Instance ().LogEvent ( "Turning music OFF" );
			GoogleAnalytics.instance.LogScreen ( "Turning music OFF" );
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
