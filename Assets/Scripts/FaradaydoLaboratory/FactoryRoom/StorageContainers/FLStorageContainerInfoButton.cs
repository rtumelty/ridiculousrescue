using UnityEngine;
using System.Collections;

public class FLStorageContainerInfoButton : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private GameObject _myComboUIInfoScreen;
	//*************************************************************//
	
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		_myComboUIInfoScreen = FLUIControl.getInstance ().createPopup ( FLFactoryRoomManager.getInstance ().storageComboUIStorageInfoScreenPrefab );
		if ( _myComboUIInfoScreen != null ) _myComboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().myStorageContainerClass = myStorageContainerClass;
	}
}
