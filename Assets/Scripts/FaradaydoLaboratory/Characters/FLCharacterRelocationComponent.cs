using UnityEngine;
using System.Collections;

public class FLCharacterRelocationComponent : MonoBehaviour 
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
		//=================================Daves Edit=======================================
		//This was causing unwanted SFX to play on worldmap launch.
		gameObject.GetComponent < SelectedComponenent > ().FL_setSelectedCharacter ( true,true );
	}
	
	public void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		if ( gameObject.GetComponent < CharacterTapTutorialControl > () != null ) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		FLMain.getInstance ().unselectCurrentCharacter ();
		gameObject.GetComponent < SelectedComponenent > ().FL_setSelectedCharacter ( true );
		FLMain.getInstance ().handleCharacterChosen ( _myIComponent.myCharacterData, handleRelocateToPosition );
	}
	
	private bool handleRelocateToPosition ( Vector3 position )
	{
		if ( blockMove ) return false;
		if ( _myIComponent.myCharacterData.blocked ) return false;
		if ( GlobalVariables.checkForMenus  ()) return false;
		
		int[] targetTile = new int[2] { Mathf.RoundToInt ( position.x ), Mathf.RoundToInt ( position.z )};
		int[][] returnedPath = AStar.search ( _myIComponent.position, targetTile, false, _myIComponent.myID, this.transform.root.gameObject );
		
		gameObject.GetComponent < SelectedComponenent > ().resetObject ();
		
		FLMoveComponent moveComponent = gameObject.transform.root.GetComponent < FLMoveComponent > ();
		moveComponent.initMove ( returnedPath, targetTile, false, _myIComponent.myCharacterData, null );
		
		return true;
	}
}
