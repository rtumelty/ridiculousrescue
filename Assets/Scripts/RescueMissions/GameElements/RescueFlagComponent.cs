using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RescueFlagComponent : MonoBehaviour 
{
	//*************************************************************//
	private IComponent _myIComponent;
	//*************************************************************//
	void Start ()
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
	}
	
	void OnMouseUp ()
	{
		if ( GlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myIComponent.position[0]][_myIComponent.position[1]] == null ) return;
		else
		{
			if ( Array.IndexOf ( GameElements.TO_BE_RESCUED, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][_myIComponent.position[0]][_myIComponent.position[1]] ) != -1 )
			{
				LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][_myIComponent.position[0]][_myIComponent.position[1]].transform.Find ( "tile" ).SendMessage ( "OnMouseUp" );
			}
		}
	}
}
