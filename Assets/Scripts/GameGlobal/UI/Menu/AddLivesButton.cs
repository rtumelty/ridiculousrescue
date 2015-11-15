using UnityEngine;
using System.Collections;

public class AddLivesButton : MonoBehaviour 
{
	public int numberOfLivesToAdd = 0;
	public int myGoldCost = 0;
	private GameObject _buyGoldPrefab;


	void Awake ()
	{
		_buyGoldPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/getGoldUIPanel" );
		if(Camera.main.transform.Find("UI").GetComponent<UIControl>() == null)
		{
			GlobalVariables.NO_LIFE_RESTART = false;
		}
	}

	void OnMouseUp ()
	{
		if ( FLGlobalVariables.TUTORIAL_MENU || FLMissionRoomManager.AFTER_INTRO ) return;
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		handleTouched ();
	}
	
	private void handleTouched (/*Transform atThisPoint = null*/)
	{

		if (myGoldCost <= GameGlobalVariables.Stats.METAL_IN_CONTAINERS)
		{
			print ("COST " + GameGlobalVariables.Stats.GOLD_COST_OF_LIFE);
			print ("HAVE " + GameGlobalVariables.Stats.METAL_IN_CONTAINERS);
			print ("first bit");
			SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
			if(GameGlobalVariables.Stats.LIVES_AVAILABLE < GameGlobalVariables.Stats.MAX_LIVES && GameGlobalVariables.Stats.METAL_IN_CONTAINERS >= myGoldCost)
			{
				GameGlobalVariables.Stats.LIVES_AVAILABLE += numberOfLivesToAdd;
				SaveDataManager.save ( SaveDataManager.LIVES_AVAILABLE, GameGlobalVariables.Stats.LIVES_AVAILABLE);
				GameGlobalVariables.Stats.METAL_IN_CONTAINERS -= myGoldCost;
				SaveDataManager.save (SaveDataManager.METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS);

				if(GameGlobalVariables.Stats.LIVES_AVAILABLE > GameGlobalVariables.Stats.MAX_LIVES)
				{
					GameGlobalVariables.Stats.LIVES_AVAILABLE = GameGlobalVariables.Stats.MAX_LIVES;
				}
				if(GlobalVariables.NO_LIFE_RESTART == true)
				Destroy (UIControl.currentBlackOutUI);
				Destroy (UIControl.currentPopupUI);
				UIControl.currentPopupUI = null;
				GlobalVariables.POPUP_UI_SCREEN = false;
			}

		}
		else if (GlobalVariables.NO_LIFE_RESTART == true )
		{
			Destroy (UIControl.currentBlackOutUI);
			Destroy (UIControl.currentPopupUI);
			UIControl.currentPopupUI = null;
			//UIControl.getInstance ().createPopup (UIControl.getInstance ()._getLivesUI, true, "NULL", transform.parent.transform.parent);
			UIControl.getInstance().createPopup(UIControl.getInstance()._getGoldUIAlt, true);
			
			/*if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
			{
				print ("FFS2");
				FLUIControl.getInstance ().unselectCurrentGameElement ();
				FLUIControl.getInstance ().destoryCurrentUIElement ();
			}
			SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
			GameObject menuScreen = FLUIControl.getInstance ().createPopup ( _buyGoldPrefab );*/
			
		}
		else 
		{
			if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
			{
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Camera.main.transform.Find("world").transform.Find("MapUIPrefab").transform.Find("UIRight").transform.Find("GoldAddButton").SendMessage("Activate");
				GameObject.Find("GoldAddButton").SendMessage("Activate", transform.parent.transform.parent);
			}
			else if(Camera.main.transform.Find("UI").GetComponent<UIControl>() != null)
			{
				UIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();	
				print (transform.parent.transform.parent);
				GameObject.Find("GoldAddButton").SendMessage("handleTouched", transform.parent.transform.parent);
			}

			/*if(Camera.main.transform.Find("UI").GetComponent<FLUIControl>() != null && ! FLGlobalVariables.TUTORIAL_MENU)
			{
				print ("FFS2");
				FLUIControl.getInstance ().unselectCurrentGameElement ();
				FLUIControl.getInstance ().destoryCurrentUIElement ();
			}
			SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
			GameObject menuScreen = FLUIControl.getInstance ().createPopup ( _buyGoldPrefab );*/

		}
	}
}
