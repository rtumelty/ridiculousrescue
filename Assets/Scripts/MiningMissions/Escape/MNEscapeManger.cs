using UnityEngine;
using System.Collections;

public class MNEscapeManger : MonoBehaviour 
{
	//*************************************************************//
	private bool _endScreenStarted = false;
	//*************************************************************//

	void Update () 
	{
		bool allOnEscapeArea = true;
		foreach ( CharacterData character in MNLevelControl.getInstance ().charactersOnLevel )
		{
			if ( ToolsJerry.compareTiles ( character.position, new int[2] { 0, 0 } ))
			{

			}
			else if ( ToolsJerry.compareTiles ( character.position, new int[2] { 0, 1 } ))
			{
				
			}
			else if ( ToolsJerry.compareTiles ( character.position, new int[2] { 0, 2 } ))
			{
				
			}
			else if ( ToolsJerry.compareTiles ( character.position, new int[2] { 1, 0 } ))
			{
				
			}
			else if ( ToolsJerry.compareTiles ( character.position, new int[2] { 1, 1 } ))
			{
				
			}
			else if ( ToolsJerry.compareTiles ( character.position, new int[2] { 1, 2 } ))
			{
			}
			else
			{
				allOnEscapeArea = false;
				break;
			}
		}



		if ( allOnEscapeArea && ! _endScreenStarted )
		{
			_endScreenStarted = true;

			bool ommitCeleberation = false;

			if ( MNResoultScreen.getInstance ().charOutOfPower ()) ommitCeleberation = true;
			else if ( MNResoultScreen.getInstance ().noMaterialsGathered ()) ommitCeleberation = true;

			if ( ! ommitCeleberation )
			{
				foreach ( CharacterData character in MNLevelControl.getInstance ().charactersOnLevel )
				{
					MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]].transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelectedForCelebration ( true, 7 );
					iTween.Stop ( MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]] );
					MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][character.position[0]][character.position[1]].GetComponent < MNMoveComponent > ().isMoving = false;
				
				}

				SoundManager.getInstance ().playSound ( SoundManager.RESCUE_TOY_AT_MARKER );
			}

			MNMain.getInstance ().checkForWinConditions ();
		}
	}
}
