using UnityEngine;
using System.Collections;

public class MNAttackerComponent : MonoBehaviour 
{
	//*************************************************************//	
	private IComponent _myIComponent;
	//*************************************************************//	
	IEnumerator Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		
		yield return new WaitForSeconds ( 0.1f );
		
		if ( _myIComponent.myCharacterData.myID == GameElements.CHAR_MADRA_1_IDLE || _myIComponent.myCharacterData.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			gameObject.SendMessage ( "handleTouched" );
		}
	}
	
	void OnMouseUp ()
	{
		if (( MNUIControl.currentAttackBarUI != null ) && ( _myIComponent.myCharacterData.attacking )) MNUIControl.currentAttackBarUI.SendMessage ( "OnMouseUp" );
		if ( MNGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		MNMain.getInstance ().handleAttackerChosen ( _myIComponent, _myIComponent.myCharacterData );
	}
}
