using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameTextManager : MonoBehaviour 
{
	//*************************************************************//
	// Rescue Mission Failed Screen
	public const string KEY_SCREEN_RESCUEMISSION_FAILED_GENERAL = "screen_rescuemission_failed_general";
	public const string KEY_SCREEN_RESCUEMISSION_FAILED_REDIRECTORS = "screen_rescuemission_failed_outofredirectors";
	public const string KEY_SCREEN_RESCUEMISSION_FAILED_CORA = "screen_rescuemission_failed_failedtobringrescuetoytomarker";
	public const string KEY_SCREEN_RESCUEMISSION_FAILED_ENGINEER = "screen_rescuemission_failed_failedtopowerlazarus";
	//*************************************************************//
	public static int CURRENT_LANAGUAGE
	{
		set
		{
			_CURRENT_LANAGUAGE = value;
			if ( _CURRENT_LANAGUAGE == 8 ) _CURRENT_LANAGUAGE = 2;
			if ( _CURRENT_LANAGUAGE == 1 ) _CURRENT_LANAGUAGE = 7;
			GameTextManager.getInstance ().updateGameTexts ();
		}
		get
		{
			return _CURRENT_LANAGUAGE;
		}
	}
	private static int _CURRENT_LANAGUAGE = LANGUAGE_ENGLISH;
	//*************************************************************//
	private const int KEY = 0;
	public const int LANGUAGE_ENGLISH = 2;
	public const int LANGUAGE_JAPANESE = 3;
	public const int LANGUAGE_SPANISH = 4;
	public const int LANGUAGE_GERMAN = 5;
	public const int LANGUAGE_FRENCH = 6;
	public const int LANGUAGE_POLISH = 7;
	//*************************************************************//
	private TextAsset _gameTextAsset;
	private static Dictionary < string, Dictionary < int, string >> _GAME_TEXT_DICTIONARY;
	private List < GameTextControl > _allGameTexts;
	//*************************************************************//	
	private static GameTextManager _meInstance;
	public static GameTextManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < GameTextManager > ();
		}
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
//		SoundManager.getInstance ().turnMusicOnOff (true);
//		print("Save data cleared");
//		PlayerPrefs.DeleteAll ();
		_allGameTexts = new List < GameTextControl > ();
		if ( _GAME_TEXT_DICTIONARY != null ) return;
		_GAME_TEXT_DICTIONARY = new Dictionary < string, Dictionary < int, string >> ();
		
		_gameTextAsset = ( TextAsset ) Resources.Load ( "GameText/gameText" );
		string[] lines = _gameTextAsset.text.Split ( new Char[] { '\n' });
		
		foreach ( string line in lines )
		{
			string[] words = line.Split ( new Char[] { ',' });

			if ( words[KEY] == "" ) continue;
			if ( words.Length < 9 )
			{
				print ( line );
				print ( "ENETRESJSZON!!!" );
				continue;
			}
			int differneceBetwenNormalLenghtAndActual = words.Length - 9;
			if ( differneceBetwenNormalLenghtAndActual > 0 )
			{
				print ( line );
				print ( "AHHHHHHHHHHHHHHHH, PRECINEKAJSOSZON!!!" );
			}
			
			if ( _GAME_TEXT_DICTIONARY.ContainsKey ( words[KEY] )) continue;
			Dictionary < int, string > dictionaryWithLanguagesTexts = new Dictionary < int, string > ();
			dictionaryWithLanguagesTexts.Add ( LANGUAGE_ENGLISH, words[LANGUAGE_ENGLISH] );
			dictionaryWithLanguagesTexts.Add ( LANGUAGE_JAPANESE, words[LANGUAGE_JAPANESE] );
			dictionaryWithLanguagesTexts.Add ( LANGUAGE_SPANISH, words[LANGUAGE_SPANISH] );
			dictionaryWithLanguagesTexts.Add ( LANGUAGE_GERMAN, words[LANGUAGE_GERMAN] );
			dictionaryWithLanguagesTexts.Add ( LANGUAGE_FRENCH, words[LANGUAGE_FRENCH] );
			dictionaryWithLanguagesTexts.Add ( LANGUAGE_POLISH, words[LANGUAGE_POLISH] );

			_GAME_TEXT_DICTIONARY.Add ( words[KEY], dictionaryWithLanguagesTexts );
		}
	}
	
	public string getText ( string key, string characterName = "" )
	{
		if ( ! _GAME_TEXT_DICTIONARY.ContainsKey ( key ))
		{
			//print ( key );
			return "";
		}

		string stringToReturn = _GAME_TEXT_DICTIONARY[key][CURRENT_LANAGUAGE];
		if ( stringToReturn.Contains ( "{CHARACTER NAME}" ))
		{
			stringToReturn = stringToReturn.Replace ( "{CHARACTER NAME}", characterName );
		}
		
		if ( stringToReturn.Contains ( "{X MACHINE}" ))
		{
			if ( FLGlobalVariables.RequiredMachinesForNextLevel.RECHARGEOCORES > 0 ) stringToReturn = stringToReturn.Replace ( "{X MACHINE}", FLGlobalVariables.RequiredMachinesForNextLevel.RECHARGEOCORES.ToString () + " " + getText ( "ui_name_factory_storage_rechargeocores" ));
			else stringToReturn = stringToReturn.Replace ( "{X MACHINE} and", "" );
		}
		
		if ( stringToReturn.Contains ( "{Y MACHINE}" ))
		{
			if ( FLGlobalVariables.RequiredMachinesForNextLevel.REDIRECTORS > 0 ) stringToReturn = stringToReturn.Replace ( "{Y MACHINE}", FLGlobalVariables.RequiredMachinesForNextLevel.REDIRECTORS.ToString () + " " + getText ( "ui_name_factory_storage_redirectors" ));
			else
			{
				stringToReturn = stringToReturn.Replace ( "and {Y MACHINE}", "" );
				stringToReturn = stringToReturn.Replace ( "{Y MACHINE}", "" );
			}
		}
		
		stringToReturn = stringToReturn.Replace ( ";", "," );
		stringToReturn = stringToReturn.Replace ( "</br>", "\n" );
		stringToReturn = stringToReturn.Replace ( "\"\"\"", "\"" );
		
		return stringToReturn;
	}
	
	public string getCurrentLanguageString ()
	{
		switch ( CURRENT_LANAGUAGE )
		{
			case LANGUAGE_ENGLISH:
				return "English";
			case LANGUAGE_JAPANESE:
				return "Japanese";
			case LANGUAGE_SPANISH:
				return "Spanish";
			case LANGUAGE_GERMAN:
				return "German";
			case LANGUAGE_FRENCH:
				return "French";
			case LANGUAGE_POLISH:
				return "Polish";
		}
		
		return "";
	}
	
	public void registerGameText ( GameTextControl gameText )
	{
		_allGameTexts.Add ( gameText );
	}
	
	public void updateGameTexts ()
	{
		foreach ( GameTextControl gameText in _allGameTexts )
		{
			if ( gameText == null )
			{
				continue;
			}
			
			gameText.updateText ();
		}
	}
}
