using UnityEngine;
using System.Collections;

public class MessageCenter : MonoBehaviour
{
	//*************************************************************//
	private GameObject _tileMarkerPrefab;
	private GameObject _errorMarkPrefab;
	private GameObject _errorDialog;
	private bool allowNewErrorPopUp = true;
	private GameObject _goodMarkPrefab;
	private Color _cantMoveMarkerColor = new Color ( 1f, 0.2f, 0.1f, 0.3f );
	private float _countToUnselect;
	private int[] _currentMarkPosition;
	//*************************************************************//	
	private static MessageCenter _meInstance;
	public static MessageCenter getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < MessageCenter > ();
		}

		return _meInstance;
	}
	//*************************************************************//
	
	void Awake () 
	{
		_tileMarkerPrefab = ( GameObject ) Resources.Load ( "Tile/tileMarker" );
		_errorMarkPrefab = ( GameObject ) Resources.Load ( "ErrorMark/errorMark" );
		_errorDialog = (GameObject)Resources.Load ("UI/clearAPathUI");
		_goodMarkPrefab = ( GameObject ) Resources.Load ( "GoodMark/goodMark" );
	}
	
	void Update () 
	{
		if ( _currentMarkPosition == null ) return;
		if ( _countToUnselect > 0f )
		{
			_countToUnselect -= Time.deltaTime;
		}
		else
		{
			unselectObjectAfterSeconds ( _currentMarkPosition );
		}
	}
	
	public void createMarkInPosition ( int[] markPosition )
	{   
		if (( _currentMarkPosition != null ) && ( ! ToolsJerry.compareTiles ( _currentMarkPosition, markPosition )))
		{
			unselectObjectAfterSeconds ( _currentMarkPosition );
		}
		
		_countToUnselect = 1.2f;
		_currentMarkPosition = ToolsJerry.cloneTile ( markPosition );
		
		Vector3 gridSqurePosition  = new Vector3 ((float) markPosition[0], (float) ( LevelControl.LEVEL_HEIGHT - markPosition[1] ) - 0.5f, (float) markPosition[1] );
		GameObject currentErrorMarkInstant = null;
		currentErrorMarkInstant = ( GameObject ) Instantiate ( _errorMarkPrefab, gridSqurePosition + Vector3.up * 2f, _errorMarkPrefab.transform.rotation );
		
		GameObject currentSqureMarkInstant = ( GameObject ) Instantiate ( _tileMarkerPrefab, gridSqurePosition, Quaternion.identity );
		currentSqureMarkInstant.renderer.material.color = _cantMoveMarkerColor;
		ErrorMarkControl errorMarkControl = currentErrorMarkInstant.GetComponent < ErrorMarkControl > ();
		errorMarkControl.gridSqureMark = currentSqureMarkInstant;

		GlobalVariables.READY_FOR_NEXT_TURN = true;
		print ("Ok Again");
		GameGlobalVariables.ACTIVE_FROM_ENEMY = false;
	
		if(allowNewErrorPopUp == true)
	    {
			allowNewErrorPopUp = false;
			GameObject currentErrorDialog = (GameObject)Instantiate (_errorDialog, Camera.main.transform.position + Vector3.down * 2f, Quaternion.identity);
			currentErrorDialog.transform.parent = Camera.main.transform;
			StartCoroutine("ShowDialog", currentErrorDialog);
		}
	}

	public IEnumerator ShowDialog (GameObject myObject)
	{
		yield return new WaitForSeconds (0.5f);
		GlobalVariables.READY_FOR_NEXT_TURN = true;
		yield return new WaitForSeconds (2.5f);
		allowNewErrorPopUp = true;
		Destroy (myObject);
	}
	
	public void createGoodMarkInPosition ( int[] markPosition )
	{   
		Vector3 gridSqurePosition  = new Vector3 ((float) markPosition[0], (float) ( LevelControl.LEVEL_HEIGHT - markPosition[1] ) - 0.5f, (float) markPosition[1] );
		Instantiate ( _goodMarkPrefab, gridSqurePosition, _goodMarkPrefab.transform.rotation );
	}
	
	public void startUnselectProcedureInPosition ( int[] markPosition )
	{
		if (( _currentMarkPosition != null ) && ( ! ToolsJerry.compareTiles ( _currentMarkPosition, markPosition )))
		{
			unselectObjectAfterSeconds ( _currentMarkPosition );
		}
		
		_countToUnselect = 1.2f;
		_currentMarkPosition = ToolsJerry.cloneTile ( markPosition );
	}
	
	private void unselectObjectAfterSeconds ( int[] markPosition )
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]] == null ) return;
			if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > () != null )
			{
				LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelected ( false );
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			if ( FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]] == null ) return;
			if ( FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > () != null )
			{
				FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelected ( false );
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]] == null ) return;
			if ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > () != null )
			{
				MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][markPosition[0]][markPosition[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelected ( false );
			}
		}
	}
}
