using UnityEngine;
using System.Collections;

public class AttackerComponent : MonoBehaviour 
{
	//*************************************************************//	
	private IComponent _myIComponent;
	//*************************************************************//	
	IEnumerator Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		
		yield return new WaitForSeconds ( 0.1f );
		//===============================Daves Edit=================================
		//This was also causing undesired SFX in the level.
		if ( _myIComponent.myCharacterData.myID == GameElements.CHAR_MADRA_1_IDLE )
		{
			//gameObject.SendMessage ( "handleTouchedFromStart" );
		}
		//===============================Daves Edit=================================
	}
	
	void OnMouseUp ()
	{
		if (( UIControl.currentAttackBarUI != null ) && ( _myIComponent.myCharacterData.attacking )) UIControl.currentAttackBarUI.SendMessage ( "OnMouseUp" );
		if ( GlobalVariables.checkForMenus ()) return;
		if (CheckInputType.getInstance().touchType != CheckInputType.TAP_TYPE)
		{
			return;
		}
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		Main.getInstance ().handleAttackerChosen ( _myIComponent, _myIComponent.myCharacterData );
//		print ("Whaaaaaaatttt");
	}
}
