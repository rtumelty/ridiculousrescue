using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ConsoleManager : MonoBehaviour 
{
	public delegate void CallBackFunction ();
	public GUISkin guiSkin;
	
	private Vector2 _scrollPosition;
	private const string CONSOLE_INPUT_FIELD_NAME = "consoleInputField";
	
	public const string INT = "int";
	public const string FLOAT = "float";
	public const string STRING = "string";
	public const string BOOL = "bool";
	
	public const string DEV_PASSWORD = "sinuscosinus";
	
	public const string NO_PASSWORD = "no_password";
	public static string DUMMY_VALUE = "dummyValue";
	
	public const string CALL_BACK_AND_VALUE = "callBackAndValue";
	public const string ONLY_VALUE = "onlyValue";
	public const string ONLY_CALL_BACK = "onlyCallBack";
	
	// BEGIN build in commands
	public const string HELP = "help";
	public const string CLEAN = "clean";
	public const string VERSION = "version";
	public const string REBOOT = "reboot";
	public const string PAUSE = "pause";
	public const string PLAY = "play";
	// END build in commands
	
	private ArrayList _commandsArray;
	
	private bool _showConsole = false;
	private string _externalConsoleString = "";
	private string _internalConsoleString = "";
	private string _consoleInputString = "";
	private Rect _consoleRect;
	private Rect _buttonRect;
	private Rect _buttonRectSearch;
	private float _heightOfInputArea = 30f;
	private Rect _inputAreaRect;
	private float _buttonExecuteHieght = 30f;
	private bool _enterButtonUp = true;
	private int _commandsHistoryCount = 0;
	
	private List<string> _commandsHistory;
	
	private static ConsoleManager _meInstance;
	public static ConsoleManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_ConsoleObject" ).GetComponent < ConsoleManager > ();
		}
		
		return _meInstance;
	}
	
	
	// BEGIN OVERLOADED METHODS FOR PREPARING COMMANDS
	public static ArrayList prepareCommand ( ArrayList allCommands, string name, Pointer < int > pointerValue, string valueType, ConsoleManager.CallBackFunction callBack, string commandType, string password = NO_PASSWORD )
	{
		ArrayList command = new ArrayList ();
		command.Add ( name );
		command.Add ( pointerValue );
		command.Add ( valueType );
		command.Add ( callBack );
		command.Add ( commandType );
		command.Add ( password );
		
		allCommands.Add ( command );
		return new ArrayList ();
	}
	
	public static ArrayList prepareCommand ( ArrayList allCommands, string name, Pointer < float > pointerValue, string valueType, ConsoleManager.CallBackFunction callBack, string commandType, string password = NO_PASSWORD )
	{
		ArrayList command = new ArrayList ();
		command.Add ( name );
		command.Add ( pointerValue );
		command.Add ( valueType );
		command.Add ( callBack );
		command.Add ( commandType );
		command.Add ( password );
		
		allCommands.Add ( command );
		return new ArrayList ();
	}
	
	public static ArrayList prepareCommand ( ArrayList allCommands, string name, Pointer < string > pointerValue, string valueType, ConsoleManager.CallBackFunction callBack, string commandType, string password = NO_PASSWORD )
	{
		ArrayList command = new ArrayList ();
		command.Add ( name );
		command.Add ( pointerValue );
		command.Add ( valueType );
		command.Add ( callBack );
		command.Add ( commandType );
		command.Add ( password );
		
		allCommands.Add ( command );
		return new ArrayList ();
	}
	
	public static ArrayList prepareCommand ( ArrayList allCommands, string name, Pointer < bool > pointerValue, string valueType, ConsoleManager.CallBackFunction callBack, string commandType, string password = NO_PASSWORD )
	{
		ArrayList command = new ArrayList ();
		command.Add ( name );
		command.Add ( pointerValue );
		command.Add ( valueType );
		command.Add ( callBack );
		command.Add ( commandType );
		command.Add ( password );
		
		allCommands.Add ( command );
		return new ArrayList ();
	}
	// END OVERLOADED METHODS FOR PREPARING COMMANDS
	
	void Awake () 
	{
		setRect ( new Rect ( 0, 0, Screen.width, Screen.height / 2f ), 30f );
		_commandsHistory = new List < string > ();
		ConsoleCustomFunctions.getInstance ().CustomStart ();
		
		ArrayList allCommands = new ArrayList ();
		ConsoleManager.prepareCommand ( allCommands, "unlocklevels", new Pointer < bool > (()=>GameGlobalVariables.UNLOCK_ALL_LEVELS, x=>{GameGlobalVariables.UNLOCK_ALL_LEVELS=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().missionsRebootCallBack, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "showfps", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().fpsOn, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "quicktest", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().resetForTest, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "rescuelevel", new Pointer < string > (()=>LevelControl.SELECTED_LEVEL_NAME, x=>{LevelControl.SELECTED_LEVEL_NAME=x;}), ConsoleManager.STRING, ConsoleCustomFunctions.getInstance ().loadRescueLevel, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "enterlab", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().enterLab, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "entertrain", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().enterTrain, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "spine", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().enterSpineTest, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "reset", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().resetSavedData, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "metal", new Pointer < int > (()=>GameGlobalVariables.Stats.METAL_IN_CONTAINERS, x=>{GameGlobalVariables.Stats.METAL_IN_CONTAINERS=x;}), ConsoleManager.INT, ConsoleCustomFunctions.getInstance ().updateStorageMetal, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "plastic", new Pointer < int > (()=>GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS, x=>{GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS=x;}), ConsoleManager.INT, ConsoleCustomFunctions.getInstance ().updateStoragePlastic, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "vines", new Pointer < int > (()=>GameGlobalVariables.Stats.VINES_IN_CONTAINERS, x=>{GameGlobalVariables.Stats.VINES_IN_CONTAINERS=x;}), ConsoleManager.INT, ConsoleCustomFunctions.getInstance ().updateStorageVines, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "roc", new Pointer < int > (()=>GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS, x=>{GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS=x;}), ConsoleManager.INT, ConsoleCustomFunctions.getInstance ().updateStorageRechargeocores, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "redirectors", new Pointer < int > (()=>GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS, x=>{GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS=x;}), ConsoleManager.INT, ConsoleCustomFunctions.getInstance ().updateStorageRedirectors, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "money", new Pointer < int > (()=>GameGlobalVariables.Stats.PREMIUM_CURRENCY, x=>{GameGlobalVariables.Stats.PREMIUM_CURRENCY=x;}), ConsoleManager.INT, ConsoleCustomFunctions.getInstance ().voidCallBack, ConsoleManager.ONLY_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "finishrescue", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().finishRescueLevel, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "ft", new Pointer < bool > (()=>GameGlobalVariables.DUMMY, x=>{GameGlobalVariables.DUMMY=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().finishTrainLevel, ConsoleManager.ONLY_CALL_BACK, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "shownumbers", new Pointer < bool > (()=>GameGlobalVariables.SHOW_LEVEL_NUMBERS, x=>{GameGlobalVariables.SHOW_LEVEL_NUMBERS=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().showNumbers, ConsoleManager.CALL_BACK_AND_VALUE, ConsoleManager.NO_PASSWORD );
		ConsoleManager.prepareCommand ( allCommands, "console", new Pointer < bool > (()=>GameGlobalVariables.SHOW_CONSOLE, x=>{GameGlobalVariables.SHOW_CONSOLE=x;}), ConsoleManager.BOOL, ConsoleCustomFunctions.getInstance ().voidCallBack, ConsoleManager.ONLY_VALUE, ConsoleManager.NO_PASSWORD );

		ConsoleManager.getInstance ().initConsole ( allCommands );
	}
	
	public void initConsole ( ArrayList commandsArray  )
	{
		_commandsArray = commandsArray;
	}
	
	public void setRect ( Rect newConsoleRect, float inputFieldHeight )
	{
		_heightOfInputArea = inputFieldHeight;
		_consoleRect = new Rect ( newConsoleRect.x, newConsoleRect.y, newConsoleRect.width, newConsoleRect.height - _heightOfInputArea - _buttonExecuteHieght );
		_inputAreaRect = new Rect ( _consoleRect.x, _consoleRect.y + _consoleRect.height, _consoleRect.width, _heightOfInputArea );
		_buttonRect = new Rect ( _consoleRect.x, _consoleRect.y + _consoleRect.height + _buttonExecuteHieght, _consoleRect.width - 100f, _buttonExecuteHieght );
		_buttonRectSearch = new Rect ( _consoleRect.width - 100f, _consoleRect.y + _consoleRect.height + _buttonExecuteHieght, 100f, _buttonExecuteHieght );
	}
	
	public void setString ( string newConsoleString )
	{
		_externalConsoleString = newConsoleString;
	}
	
	public void addToExternalString(string sParam)
	{
		_externalConsoleString += "\n" + sParam;
	}
	
	public void tunrOnOff ( bool isOn )
	{
		_showConsole = isOn;
	}
	
	private void executeCommand ( string command )
	{
		if ( ! _enterButtonUp ) return;
		_enterButtonUp = false;
		
		addToInternalConsole ( command );
		_commandsHistory.Add ( command );
		
		string[] inputCommandArray = command.Split ( new string[]{ " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries );
		
		if ( inputCommandArray.Length == 0 ) return;
		
		for ( int i = 0; i < _commandsArray.Count; i++ )
		{
			// lets look if the input string is the command name...
			if ( inputCommandArray[0] == ( string ) (( ArrayList ) _commandsArray[i])[0] )
			{
				// lets look if the command is NOT protected...
				if ((( string ) (( ArrayList ) _commandsArray[i])[5] ) == NO_PASSWORD )
				{
					bool withCallback = true;
					switch ((( string ) (( ArrayList ) _commandsArray[i])[4] ))
					{
						case ONLY_CALL_BACK:
							break;
						case CALL_BACK_AND_VALUE:
							withCallback = true;
							break;
						case ONLY_VALUE:
							withCallback = false;
							break;
					}
					
					switch ((( string ) (( ArrayList ) _commandsArray[i])[4] ))
					{
						case ONLY_CALL_BACK:
							(((( ArrayList ) _commandsArray[i])[3] ) as CallBackFunction ) ();
							break;
						case CALL_BACK_AND_VALUE:
						case ONLY_VALUE:
							// lets look if the input string has value...
							if ( inputCommandArray.Length > 1 )
							{
								switch (( string ) (( ArrayList ) _commandsArray[i])[2] )
								{
									case INT:
										(( Pointer < int > ) (( ArrayList ) _commandsArray[i])[1]).Value = int.Parse ( inputCommandArray[1] );
										break;
									case FLOAT:
										(( Pointer < float >) (( ArrayList ) _commandsArray[i])[1]).Value = float.Parse ( inputCommandArray[1] );
										break;
									case STRING:
										(( Pointer < string >) (( ArrayList ) _commandsArray[i])[1]).Value = inputCommandArray[1];
										break;
									case BOOL:
										(( Pointer < bool >) (( ArrayList ) _commandsArray[i])[1]).Value = inputCommandArray[1] == "0" ? false : true;
										break;
								}
								
								addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " value set to: " + inputCommandArray[1] );
								if ( withCallback ) (((( ArrayList ) _commandsArray[i])[3] ) as CallBackFunction ) ();
							}
							else
							{
								switch (( string ) (( ArrayList ) _commandsArray[i])[2] )
								{
									case INT:
										addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " is type of INT and has value: " + (((Pointer<int>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()));
										break;
									case FLOAT:
										addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " is type of FLOAT and has value: " + (((Pointer<float>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()));
										break;
									case STRING:
										addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " is type of STRING and has value: " + (((Pointer<string>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()));
										break;
									case BOOL:
										addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " is type of BOOL and has value: " + (((Pointer<bool>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()));
										break;
								}
							}
							break;
					}
					
				}
				// command is protected...
				else
				{	
					int minusIndexForOnlyCallback = 0;
					bool withCallback = true;
					switch ((( string ) (( ArrayList ) _commandsArray[i])[4] ))
					{
						case ONLY_CALL_BACK:
							minusIndexForOnlyCallback = 1;
							break;
						case CALL_BACK_AND_VALUE:
							withCallback = true;
							minusIndexForOnlyCallback = 0;
							break;
						case ONLY_VALUE:
							withCallback = false;
							minusIndexForOnlyCallback = 0;
							break;
					}
					
					if ( inputCommandArray.Length > 2 - minusIndexForOnlyCallback )
					{
						if ( inputCommandArray[2  - minusIndexForOnlyCallback] == ( string ) (( ArrayList ) _commandsArray[i])[5] )
						{
							switch ((( string ) (( ArrayList ) _commandsArray[i])[4] ))
							{
								case ONLY_CALL_BACK:
									(((( ArrayList ) _commandsArray[i])[3] ) as CallBackFunction ) ();
									break;
								case ONLY_VALUE:
								case CALL_BACK_AND_VALUE:
									switch (( string ) (( ArrayList ) _commandsArray[i])[2] )
									{
										case INT:
											(( Pointer < int > ) (( ArrayList ) _commandsArray[i])[1]).Value = int.Parse ( inputCommandArray[1] );
											break;
										case FLOAT:
											(( Pointer < float >) (( ArrayList ) _commandsArray[i])[1]).Value = float.Parse ( inputCommandArray[1] );
											break;
										case STRING:
											(( Pointer < string >) (( ArrayList ) _commandsArray[i])[1]).Value = inputCommandArray[1];
											break;
										case BOOL:
											(( Pointer < bool >) (( ArrayList ) _commandsArray[i])[1]).Value = inputCommandArray[1] == "0" ? false : true;
											break;
									}
									
									addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " value set to: " + inputCommandArray[1] );
									if ( withCallback ) (((( ArrayList ) _commandsArray[i])[3] ) as CallBackFunction ) ();
									break;
							}
						}
						else
						{
							addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " is protected. Wrong password" );
						}
					}
					else
					{
						addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] + " is protected. Please enter password as second value..." );
					}
				}
			}
			else
			{
				tryBuildInCommands ( inputCommandArray );
			}
		}
	}
	
	private void tryBuildInCommands ( string[] inputCommandArray )
	{
		switch ( inputCommandArray[0] )
		{
			case HELP:
				printAllComands ();
				break;
			case CLEAN:
				_internalConsoleString = "";
				break;
			case VERSION:
				addToInternalConsole ( GameGlobalVariables.VERSION );
				break;
			case REBOOT:
				Application.LoadLevel ( Application.loadedLevel );
				break;
			case PAUSE:
				Time.timeScale = 0f;
				break;
			case PLAY:
				print ( inputCommandArray[1] );
				Time.timeScale =  float.Parse ( inputCommandArray[1] );
				break;
		}
	}
	
	private void printAllComands ()
	{
		addToInternalConsole ( "-----------------" );
		addToInternalConsole ( "Buildin commands:" );
		addToInternalConsole ( "-----------------" );
		addToInternalConsole ( HELP );
		addToInternalConsole ( CLEAN );
		addToInternalConsole ( VERSION );
		addToInternalConsole ( REBOOT );
		addToInternalConsole ( PAUSE );
		addToInternalConsole ( PLAY + " VALUE: FLOAT [0 = pause, 1 = normal speed, 2 = double speed]" );
		addToInternalConsole ( "-----------------" );
		addToInternalConsole ( "Custom commands:" );
		addToInternalConsole ( "-----------------" );
		for ( int i = 0; i < _commandsArray.Count; i++ )
		{
			addToInternalConsole (( string ) (( ArrayList ) _commandsArray[i])[0] );
			switch (( string ) (( ArrayList ) _commandsArray[i])[2] )
			{
				case INT:
					addToInternalConsole ( " | actual value: " + (((Pointer<int>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()), true );
					addToInternalConsole ( " | values need to be INT", true );
					break;
				case FLOAT:
					addToInternalConsole ( " | actual value: " + (((Pointer<float>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()), true );
					addToInternalConsole ( " | values need to be FLOAT", true );
					break;
				case STRING:
					addToInternalConsole ( " | actual value: " + (((Pointer<string>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()), true );
					addToInternalConsole ( " | values need to be STRING", true );
					break;
				case BOOL:
					addToInternalConsole ( " | actual value: " + (((Pointer<bool>)(( ArrayList ) _commandsArray[i])[1]).Value.ToString()), true );
					addToInternalConsole ( " | values need to be BOOL [ 0, 1 ]", true );
					break;
			}
		}
	}
	
	private void addToInternalConsole ( string addString, bool withOutNewLine = false )
	{
		_internalConsoleString += ( withOutNewLine ? "" : "\n" ) + addString;
	}
	
	void OnGUI ()
	{
		GUI.skin = guiSkin;

		if ( ! GameGlobalVariables.SHOW_CONSOLE ) return;

		if ( _showConsole )
		{
			int numberOfLines = _externalConsoleString.Split ( "\n"[0] ).Length;
			numberOfLines += _internalConsoleString.Split ( "\n"[0] ).Length;
			
			_scrollPosition = GUI.BeginScrollView ( _consoleRect, _scrollPosition, new Rect ( _consoleRect.x, _consoleRect.y, _consoleRect.width - 20f, _consoleRect.height + 9f * numberOfLines ));
			GUI.TextArea ( new Rect ( _consoleRect.x, _consoleRect.y, _consoleRect.width, _consoleRect.height + 9f * numberOfLines ) , _externalConsoleString + _internalConsoleString );
			GUI.EndScrollView ();
			GUI.SetNextControlName( CONSOLE_INPUT_FIELD_NAME );
			_consoleInputString = GUI.TextField ( _inputAreaRect, _consoleInputString );
			
			if ( _enterButtonUp )
			{
				Event currentEvent = Event.current;
				
				if ( currentEvent.keyCode == KeyCode.UpArrow )
				{
					if ( GUI.GetNameOfFocusedControl () == CONSOLE_INPUT_FIELD_NAME )
					{
						if ( _commandsHistory.Count - _commandsHistoryCount > 0 )
						{
							_enterButtonUp = false;
							_consoleInputString = _commandsHistory[_commandsHistory.Count - 1 - _commandsHistoryCount];
							_commandsHistoryCount++;
							if ( _commandsHistoryCount > _commandsHistory.Count - 1 ) _commandsHistoryCount = _commandsHistory.Count - 1;
							StartCoroutine ( "delayBeforeMayNextInput", 0.2f );
						}
					}
				}
				
				if ( currentEvent.keyCode == KeyCode.DownArrow )
				{
					if ( GUI.GetNameOfFocusedControl () == CONSOLE_INPUT_FIELD_NAME )
					{
						if ( _commandsHistory.Count - _commandsHistoryCount > 0 )
						{
							_enterButtonUp = false;
							_commandsHistoryCount--;
							if ( _commandsHistoryCount < 0 ) _commandsHistoryCount = 0;
							_consoleInputString = _commandsHistory[_commandsHistory.Count - 1 - _commandsHistoryCount];
							StartCoroutine ( "delayBeforeMayNextInput", 0.2f );
						}
					}
				}
				
				if ( GUI.Button ( _buttonRect, "RUN" ))
				{
					actionOnEnter ();
				}
				else if ( GUI.Button ( _buttonRectSearch, "Search" ))
				{
					_enterButtonUp = false;
					_commandsHistoryCount = 0;
					_consoleInputString = findMatchingCommandName ( _consoleInputString );
					StartCoroutine ( "delayBeforeMayNextInput", 0.2f );
				}
				else if( currentEvent.type == EventType.KeyDown ) 
				{
					if ( currentEvent.Equals ( Event.KeyboardEvent ( "enter" )))
					{
						if ( GUI.GetNameOfFocusedControl () == CONSOLE_INPUT_FIELD_NAME )
						{
							 actionOnEnter ();
						}
					}
					else if ( currentEvent.keyCode == KeyCode.Tab )
					{
						if ( GUI.GetNameOfFocusedControl () == CONSOLE_INPUT_FIELD_NAME )
						{
							_enterButtonUp = false;
							_commandsHistoryCount = 0;
							_consoleInputString = findMatchingCommandName ( _consoleInputString );
							StartCoroutine ( "delayBeforeMayNextInput", 0.2f );
						}
					}
				}
			}
		}
	}
	
	private string findMatchingCommandName ( string consoleInputStringValue )
	{
		string[] inputCommandArray = consoleInputStringValue.Split ( new string[]{ " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries );
		for ( int i = 0; i < _commandsArray.Count; i++ )
		{
			// lets look if the input string is in the command name...
			if ((( string ) (( ArrayList ) _commandsArray[i])[0] ).Contains ( inputCommandArray[0] ))
			{
				return (( string ) (( ArrayList ) _commandsArray[i])[0] );
			}
		}
		
		return consoleInputStringValue;
	}
	
	private void actionOnEnter ()
	{
		executeCommand ( _consoleInputString );
		_commandsHistoryCount = 0;
		_consoleInputString = "";
		StartCoroutine ( "delayBeforeMayNextInput", 0.4f );
	}
	
	private IEnumerator delayBeforeMayNextInput ( float delay )
	{
		yield return new WaitForSeconds(delay);
		_enterButtonUp = true;
	}
}
