using UnityEngine;
using System.Collections;

public class TRGlobalVariables
{	
	//*************************************************************//	
	public static bool TOY_RESCUED = false;
	public static bool PLUG_CONNECTED = false;
	//*************************************************************//	
	public static bool MENU_FOR_REDIRECTOR = false;
	public static bool CHARACTER_IN_MOVE = false;
	public static bool CORA_SWIPED = false;
	public static bool CORA_ALREADY_WITH_TOY_ON_TROLEY = false;
	public static bool CHARACTER_CELEBRATING = false;
	public static bool CHARACTER_PASSING_ANIMATION = false;
	public static bool SCREEN_DRAGGING = false;
	public static bool MENU_FOR_TIP = false;
	public static bool UI_CLICKED = false;
	public static bool LOADING_SAVING_MENU = false;
	public static bool TUTORIAL_MENU = false;
	public static bool TOY_LAZARUS_SEQUENCE = false;
	public static bool START_SEQUENCE = false;
	public static bool CHARACTER_BOX = false;
	public static bool POPUP_UI_SCREEN = false;
	//*************************************************************//	
	public static bool checkForMenus ()
	{
		return ( CHARACTER_PASSING_ANIMATION || POPUP_UI_SCREEN || MENU_FOR_REDIRECTOR || CHARACTER_IN_MOVE || CHARACTER_CELEBRATING || SCREEN_DRAGGING || MENU_FOR_TIP || UI_CLICKED || LOADING_SAVING_MENU || TUTORIAL_MENU || TOY_LAZARUS_SEQUENCE || START_SEQUENCE || CHARACTER_BOX );
	}
	//*************************************************************//
	public const float HEIGHT_OF_SELECTED_OBJECT = 20f;
	public const float HEIGHT_OF_UI_OBJECT = 2f;
	//*************************************************************//	
	public static bool DRAGGING_OBJECT = false;
	//*************************************************************//	
	public static int NUMBER_OF_REDIRECTORS_LEFT = 0;
	//*************************************************************//	
	public static class Layers
	{
		public static int GROUND = 8;
	}
	
	public static class Tags
	{
		public static string UI = "UI";
		public static string INTERACTIVE = "Interactive";
		public static string RESOURCES = "MiningResources";
		public static string CHARACTER = "Character";
		public static string STONE = "Stone";
	}
	
	public static void resetStaticValues ()
	{
		TOY_RESCUED = false;
		PLUG_CONNECTED = false;
		MENU_FOR_REDIRECTOR = false;
		DRAGGING_OBJECT = false;
		CHARACTER_IN_MOVE = false;
		CORA_SWIPED = false;
		CORA_ALREADY_WITH_TOY_ON_TROLEY = false;
		CHARACTER_CELEBRATING = false;
		SCREEN_DRAGGING = false;
		MENU_FOR_TIP = false;
		UI_CLICKED = false;
		LOADING_SAVING_MENU = false;
		TUTORIAL_MENU = false;
		TOY_LAZARUS_SEQUENCE = false;
		START_SEQUENCE = false;
		CHARACTER_BOX = false;
		POPUP_UI_SCREEN = false;
		CHARACTER_PASSING_ANIMATION = false;
		
		ResoultScreen.WIN_RESOULT = false;
	}
}
