using UnityEngine;
using System.Collections;

public class ToBeRescuedComponent : MonoBehaviour 
{
	//*************************************************************//
	public bool iAmRescued = false;
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleChoosenCharacter;
	private bool _iAmOnTroley;
	private IComponent _myIComponent;
	//*************************************************************//	
	IEnumerator Start () 
	{
		_myHandleChoosenCharacter = handlePutOnTroley;
		_myIComponent = gameObject.GetComponent < IComponent > ();

		yield return new WaitForSeconds ( 0.5f );
		GameObject coraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		coraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
		GetComponent < SelectedComponenent > ().setSelected ( true, true );
		Main.getInstance ().interactWithCurrentCharacter ( _myHandleChoosenCharacter, CharacterData.CHARACTER_ACTION_TYPE_RESCUING, _myIComponent );
	}
		
	void OnMouseUp ()
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		if ( GlobalVariables.OPENING_SEQUENCE ) return;

		if ( LevelControl.LEVEL_ID == 3 )
		{
			GameObject coraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
			coraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
		}

		CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		if ( selectedCharacter.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			if ( GetComponent < TipOnClickComponent > ()) GetComponent < TipOnClickComponent > ().deActivate ();
		}
		else
		{
			GameObject coraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
			coraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
		}
		
		if ( GlobalVariables.checkForMenus ()) return;
		if ( _iAmOnTroley || iAmRescued )
		{
			return;
		}
		
		handleTouched ();
	}
	
	private void unBlockOnTroley ()
	{
		_iAmOnTroley = false;
		collider.enabled = true;
	}
	
	private void handleTouched ()
	{
		if ( GlobalVariables.TUTORIAL_MENU )
		{
			GameObject coraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
			coraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
		}
		else
		{
			CharacterData selectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
			if ( selectedCharacter.myID != GameElements.CHAR_CORA_1_IDLE )
			{
				GameObject coraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
				coraObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
		}
		GetComponent < SelectedComponenent > ().setSelected ( true, true );
		Main.getInstance ().interactWithCurrentCharacter ( _myHandleChoosenCharacter, CharacterData.CHARACTER_ACTION_TYPE_RESCUING, _myIComponent );
	}
	
	public void handlePutOnTroley ( CharacterData characterThatIsRescuingMe, bool onlyUnblocking = false )
	{
		if ( onlyUnblocking )
		{
			return;
		}
		
		if ( transform.parent.Find ( "troley" ) != null ) Destroy ( transform.parent.Find ( "troley" ).gameObject );
		
		_iAmOnTroley = true;
		
		GridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], transform.root.gameObject, _myIComponent.myID, false );
		transform.root.gameObject.name = "toBeRescued";
		transform.root.parent = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterThatIsRescuingMe.position[0]][characterThatIsRescuingMe.position[1]].transform;

		GetComponent < SelectedComponenent > ().updateInitialValues ();

		RescuerComponent currentRescuerComponent = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][characterThatIsRescuingMe.position[0]][characterThatIsRescuingMe.position[1]].transform.Find ( "tile" ).GetComponent < RescuerComponent > ();
		currentRescuerComponent.handleChangePositionForTroley ( _myIComponent.position, _myIComponent.myID );
		
		collider.enabled = false;
	}
}
