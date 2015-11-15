using UnityEngine;
using System.Collections;

public class MobileConsoleController : MonoBehaviour 
{
	private Rect _buttonOpenClose;
	private bool _isVisible = false;
	
	void OnGUI()
	{
#if ! UNITY_EDITOR
		//if ( ! GameGlobalVariables.TEST_BUILD ) return;
#endif
		if ( ! GameGlobalVariables.SHOW_CONSOLE ) return;

		if ( GUI.Button ( new Rect ( 0f, Screen.height - 90f, 90f, 90f ), "Console" )) 
		{
			_isVisible = ! _isVisible;
			ConsoleManager.getInstance ().tunrOnOff ( _isVisible );
		}
	}
}
