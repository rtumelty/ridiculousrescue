using UnityEngine;
using System.Collections;

public class MNCharacterReloactionComponent : MonoBehaviour 
{
	//*************************************************************//
	public delegate bool HandleRelocate ( Vector3 position );
	//*************************************************************//	
	public bool blockMove;
	//*************************************************************//	
	private IComponent _myIComponent;
	//*************************************************************//	
	void Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
	}

	void Update ()
	{
		if ( _myIComponent.myCharacterData.countTimeToNextPossibleAttack > 0f )
		{
			_myIComponent.myCharacterData.countTimeToNextPossibleAttack -= Time.deltaTime;
		}

		if ( _myIComponent.myCharacterData.countTimeToNextPossibleMoveAfterAttack > 0f )
		{
			_myIComponent.myCharacterData.countTimeToNextPossibleMoveAfterAttack -= Time.deltaTime;
		}
	}
	
	public void OnMouseUp ()
	{
		if ( MNGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		MNMain.getInstance ().unselectCurrentCharacter ();
		gameObject.GetComponent < SelectedComponenent > ().MN_setSelectedCharacter ( true );
		MNMain.getInstance ().handleCharacterChosen ( _myIComponent.myCharacterData, handleRelocateToPosition );
	}
	
	private bool handleRelocateToPosition ( Vector3 position )
	{
		if ( blockMove ) return false;
		if ( _myIComponent.myCharacterData.blocked ) return false;
		if ( MNGlobalVariables.checkForMenus ()) return false;
		int[] targetTile = new int[2] { Mathf.RoundToInt ( position.x ), Mathf.RoundToInt ( position.z )};
		int[][] returnedPath = AStar.search ( _myIComponent.position, targetTile, false, _myIComponent.myID, this.transform.root.gameObject );
		
		MNMoveComponent moveComponent = gameObject.transform.root.GetComponent < MNMoveComponent > ();
		moveComponent.initMove ( returnedPath, targetTile, false, _myIComponent.myCharacterData, null );
		
		return true;
	}
	
	public void handleMoveToClossestFreeTile ()
	{
		int[] targetTile = ToolsJerry.findClossestEmptyTileAroundThisTileMining ( _myIComponent.position[0], _myIComponent.position[1], _myIComponent.position, _myIComponent.myID, transform.parent.gameObject, 1 );
		int[][] returnedPath = AStar.search ( _myIComponent.position, targetTile, false, _myIComponent.myID, this.transform.root.gameObject );
		
		gameObject.GetComponent < SelectedComponenent > ().resetObject ();
		
		MNMoveComponent moveComponent = gameObject.transform.root.GetComponent < MNMoveComponent > ();
		moveComponent.initMove ( returnedPath, targetTile, false, _myIComponent.myCharacterData, null );
	}
}
