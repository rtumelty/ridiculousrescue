using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNGridReservationManager : MonoBehaviour 
{
	//*************************************************************//	
	private static MNGridReservationManager _meInstance;
	public static MNGridReservationManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < MNGridReservationManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	
	public bool fillTileWithMe ( int ID, int x, int z, GameObject objectToBePut, int idOfRequestingObject, bool justCheck = false, int additionalLayer = -1 )
	{
		if ( MNLevelControl.getInstance ().isTileInLevelBoudaries ( x, z ))
		{
			int[][] currentGridLayer = null;
			GameObject[][] currentGameObjectLayer = null;
			currentGridLayer = MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL];
			currentGameObjectLayer = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL];
			
			if ( ID == GameElements.EMPTY )
			{
				if (( currentGameObjectLayer[x][z] == objectToBePut ) || ( currentGameObjectLayer[x][z] == null ))
				{
					if ( ! justCheck )
					{
						currentGridLayer[x][z] = GameElements.EMPTY;
						currentGameObjectLayer[x][z] = null;
						if ( additionalLayer != -1 )
						{
							MNLevelControl.getInstance ().levelGrid[additionalLayer][x][z] = GameElements.EMPTY;
							MNLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] = null;
						}
					}
					
					return true;
				}
			}
			else if ((( currentGridLayer[x][z] == GameElements.EMPTY ) || ( currentGameObjectLayer[x][z] == objectToBePut ) || ( currentGameObjectLayer[x][z] == null )) && (( additionalLayer == -1 ) || ( MNLevelControl.getInstance ().levelGrid[additionalLayer][x][z] == GameElements.EMPTY ) || ( MNLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] == objectToBePut ) || ( MNLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] == null ))) 
			{
				if ( ! justCheck )
				{
					currentGridLayer[x][z] = ID;
					currentGameObjectLayer[x][z] = objectToBePut;
					
					if ( additionalLayer != -1 )
					{
						MNLevelControl.getInstance ().levelGrid[additionalLayer][x][z] = ID;
						MNLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] = objectToBePut;
					}
				}
				return true;
			}
		}
		
		return false;
	}
}
