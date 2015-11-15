using UnityEngine;
using System.Collections;

public class FL_MOMTapReadyElement : MonoBehaviour 
{
	//*************************************************************//
	public int myElementID;
	//*************************************************************//
	private bool touched = false;
	private FLStorageContainerClass _currentFLStorageContainerClass;
	private int[] _myInitialPosition;
	//*************************************************************//
	void Start ()
	{
		gameObject.GetComponent < SelectedComponenent > ().setSelectedForBouncing ( true );
		//=================================Daves Edit====================================
		if(gameObject.GetComponent <IComponent> ().myID == 64)
		{
			//GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION ++;
		}
		//=================================Daves Edit====================================
	}
	
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( touched ) return;
		gameObject.GetComponent < SelectedComponenent > ().setSelectedForBouncing ( false );
		
		_myInitialPosition = new int[2] { Mathf.RoundToInt ( transform.parent.position.x ), Mathf.RoundToInt ( transform.parent.position.z )};
		
		touched = true;
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true, true );
		_currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().handleFindStoreForReadyElement ( myElementID );

		switch ( myElementID )
		{
		case GameElements.ICON_RECHARGEOCORES:
			int numberOfReadyBatteries = SaveDataManager.getValue ( SaveDataManager.BATTERIES_AT_MOM_READY_TO_TAP );
			numberOfReadyBatteries--;
			SaveDataManager.save ( SaveDataManager.BATTERIES_AT_MOM_READY_TO_TAP, numberOfReadyBatteries );
			break;
		case GameElements.ICON_REDIRECTORS:
			int numberOfReadyRedirectors = SaveDataManager.getValue ( SaveDataManager.REDIRECTORS_AT_MOM_READY_TO_TAP );
			numberOfReadyRedirectors--;
			SaveDataManager.save ( SaveDataManager.REDIRECTORS_AT_MOM_READY_TO_TAP, numberOfReadyRedirectors );
			break;
		}

		if ( _currentFLStorageContainerClass != null )
		{
			Vector3 positionToGo = new Vector3 ((float) _currentFLStorageContainerClass.position[0], (float) ( FLLevelControl.LEVEL_HEIGHT - _currentFLStorageContainerClass.position[1] ) + 4f, (float) _currentFLStorageContainerClass.position[1] - 0.5f );
			iTween.MoveTo ( transform.parent.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeInBack, "position", positionToGo ));
			StartCoroutine ( "onComplete" );
		}
		else
		{
			FLMain.getInstance ().handleObjectDestory ( myElementID, transform.parent.gameObject, _myInitialPosition );
		}
		//=================================Daves Edit====================================
		if(gameObject.GetComponent <IComponent> ().myID == 64 && GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION > 0)
		{
			GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION --;
		}
		//=================================Daves Edit====================================
	}
	
	private IEnumerator onComplete ()
	{
		yield return new WaitForSeconds ( 0.5f );
		_currentFLStorageContainerClass.amount++;
		
		switch ( _currentFLStorageContainerClass.type )
		{
			case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
				GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS++;
				SaveDataManager.save ( SaveDataManager.RECHARGEOCORES_IN_CONTAINERS, GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS );
				break;
			case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
				GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS++;
				SaveDataManager.save ( SaveDataManager.REDIRECTORS_IN_CONTAINERS, GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS );
				break;
		}
		
		FLMain.getInstance ().handleObjectDestory ( myElementID, transform.parent.gameObject, _myInitialPosition );
	}
}
