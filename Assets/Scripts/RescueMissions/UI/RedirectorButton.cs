using UnityEngine;
using System.Collections;

public class RedirectorButton : MonoBehaviour 
{
	//*************************************************************//
	public bool redirectorIsOnlevel = false;
	public TextMesh redirectorsNumberTextmesh;
	public GameObject currentRedirectorOnLevel;
	//*************************************************************//
	private int _currentRedirectorGameElementsID = 0;
	private GameObject _tileInteractivePrefab;
	//*************************************************************//
	private static RedirectorButton _meInstance;
	public static RedirectorButton getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = Camera.main.transform.Find ( "UI" ).Find ( "RedirectionButton" ).GetComponent < RedirectorButton > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractiveWithAutoPanObjects" );
		_currentRedirectorGameElementsID = GameElements.ENVI_REDIRECTOR_01;
		redirectorsNumberTextmesh.text = GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT.ToString ();
	}
	
	void Update ()
	{
		redirectorsNumberTextmesh.text = GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT.ToString ();		
	}
	
	void OnMouseUp ()
	{
		//if ( GlobalVariables.PLUG_CONNECTED ) return;
		if ( GlobalVariables.checkForMenus ()) return;
		if ( GlobalVariables.NUMBER_OF_REDIRECTORS_LEFT <= 0 ) return;
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( redirectorIsOnlevel ) return;
		
		CharacterData currentSelectedCharacter = LevelControl.getInstance ().getSelectedCharacter ();
		if ( currentSelectedCharacter.interactAction )
		{
			return;
		}
		
		redirectorIsOnlevel = true;
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true, false );
		
		UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		
		GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 ( transform.position.x, transform.position.y - 1f, transform.position.z ), _tileInteractivePrefab.transform.rotation );
		interactiveObjectInstant.transform.parent = Camera.main.transform;
		interactiveObjectInstant.transform.localPosition = new Vector3 ( -4.39f, 0.1f, 2f );		
		interactiveObjectInstant.transform.parent = null;
		
		int xPosition = Mathf.RoundToInt ( interactiveObjectInstant.transform.position.x );
		int zPosition = Mathf.RoundToInt ( interactiveObjectInstant.transform.position.z );
		
		interactiveObjectInstant.transform.position = new Vector3 ((float) xPosition, (float) ( LevelControl.LEVEL_HEIGHT - zPosition ) + 3f, (float) zPosition - 0.5f );
		GameObject interactiveObjectMesh = interactiveObjectInstant.transform.Find ( "tile" ).gameObject;
		interactiveObjectMesh.transform.localScale = Vector3.one;
		interactiveObjectMesh.transform.position -= Vector3.forward * 0.33f;
		interactiveObjectMesh.renderer.material.mainTexture = LevelControl.getInstance ().gameElements[GameElements.ENVI_REDIRECTOR_01];
		interactiveObjectInstant.name += "_REDIRECTOR";
		
		IComponent curretnIComponent = interactiveObjectMesh.AddComponent < IComponent > ();
		DragComponent currentDragComponent = interactiveObjectMesh.AddComponent < DragComponent > ();
		interactiveObjectMesh.AddComponent < SelectedComponenent > ();
		
		curretnIComponent.myID = _currentRedirectorGameElementsID;
		currentDragComponent.externallyInvokeMouseDownOnMe ();	
		
		interactiveObjectMesh.AddComponent < RedirectorComponent > ();
		
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		currentRedirectorOnLevel = interactiveObjectInstant;
		
		LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][currentSelectedCharacter.position[0]][currentSelectedCharacter.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( interactiveObjectInstant );
	}
}
