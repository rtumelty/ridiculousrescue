using UnityEngine;
using System.Collections;

public class FLGlobalVariables
{
	//*************************************************************//	
	public static bool UI_CLICKED = false;
	public static bool SCREEN_DRAGGING = false;
	public static bool SCREEN_DRAGGED = false;
	public static bool POPUP_UI_SCREEN = false;
	public static bool DRAGGING_OBJECT = false;
	public static bool MISSION_SCREEN = false;
	public static bool CHARACTER_IN_MOVE = false;
	public static bool TRANSACTION = false;
	public static bool DIALOG_ON_MAP = false;
	public static bool AFTER_LAB_VISIT_02 = false;
	public static int CONTAINER_UI_OPEN = 0;
	public static bool MAP_DRAGGED = false;
	public static bool TUTORIAL_MENU 
	{
		set
		{
			_TUTORIAL_MENU = value;
		}
		get
		{
			return _TUTORIAL_MENU;
		}
	}
	private static bool _TUTORIAL_MENU = false;
	public static int CHARACTER_IN_MOVE_MENU_ID = 0;
	//*************************************************************//	
	public static bool checkForMenus ( int IDToCheck = -1 )
	{
		if ( CHARACTER_IN_MOVE && CHARACTER_IN_MOVE_MENU_ID == IDToCheck ) return false;
		return ( DIALOG_ON_MAP || UI_CLICKED || SCREEN_DRAGGING || POPUP_UI_SCREEN || DRAGGING_OBJECT || MISSION_SCREEN || CHARACTER_IN_MOVE || TUTORIAL_MENU || TRANSACTION );
	}
	//*************************************************************//
	public static class Layers
	{
		public static int GROUND = 8;
	}
	
	public static class Tags
	{
		public static string UI = "UI";
		public static string INTERACTIVE = "Interactive";
		public static string CHARACTER = "Character";
	}
	
	public static class RequiredMachinesForNextLevel
	{
		public static int RECHARGEOCORES = 0;
		public static int REDIRECTORS = 0;
	}
	
	public static void resetStaticValues ()
	{
		UI_CLICKED = false;
		SCREEN_DRAGGING = false;
		POPUP_UI_SCREEN = false;
		DRAGGING_OBJECT = false;
		MISSION_SCREEN = false;
		CHARACTER_IN_MOVE = false;
		DIALOG_ON_MAP = false;
		AFTER_LAB_VISIT_02 = false;
		TUTORIAL_MENU = false;
		TRANSACTION = false;

		MAP_DRAGGED = false;
		SCREEN_DRAGGED = false;
	}
}
