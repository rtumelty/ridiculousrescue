using UnityEngine;
using System.Collections;

public class FLMissionTableControl : MonoBehaviour 
{
	//*************************************************************//
	private bool _checkIfMapDragged = false;
	//*************************************************************//
	private static FLMissionTableControl _meInstance;
	public static FLMissionTableControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		_meInstance = this;
	}

	void OnMouseUp ()
	{
		if ( _checkIfMapDragged )
		{
			if ( FLGlobalVariables.SCREEN_DRAGGED )
			{
				FLGlobalVariables.SCREEN_DRAGGED = false;
				_checkIfMapDragged = false;
				return;
			}
		}

		if ( FLGlobalVariables.checkForMenus ()) return;
		if ( ! FLMissionRoomManager.getInstance ().checkIfAnimationOfMapToLabFinished ()) return;
		handleTouched ();
	}

	void OnMouseDown ()
	{
		_checkIfMapDragged = true;
	}
	
	private void handleTouched ()
	{
		FLMain.getInstance ().unselectCurrentCharacter ();
		FLUIControl.getInstance ().unselectCurrentGameElement ();

		FLUIControl.currentSelectedGameElement = this.gameObject;
		
		FLUIControl.getInstance ().destoryCurrentUIElement ();
		
		FLMissionRoomManager.getInstance ().showMissionLevelesScreen ( true, true );

		if ( FLGlobalVariables.TUTORIAL_MENU )
		{
			Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "level05" ).gameObject.GetComponent < FLMissionScreenNodeManager > ().startUnlockingProcedure ( true );
		}

		if ( ! GameGlobalVariables.CUT_DOWN_GAME && FLGlobalVariables.AFTER_LAB_VISIT_02 )
		{
			FLGlobalVariables.AFTER_LAB_VISIT_02 = false;
			gameObject.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( false );
			Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "level06" ).gameObject.AddComponent < UnlockLevelSequenceManager > ();
			FLUIControl.getInstance ().transform.Find ( "triggerDialogLabVisit02" ).gameObject.SetActive ( false );
		}
		else
		{
			gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true );
		}
	}
}
