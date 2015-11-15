using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLGridReservationManager : MonoBehaviour 
{
	//*************************************************************//	
	private static FLGridReservationManager _meInstance;
	public static FLGridReservationManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < FLGridReservationManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	
	public bool fillTileWithMe ( int ID, int x, int z, GameObject objectToBePut, int idOfRequestingObject, bool justCheck = false, int additionalLayer = -1 )
	{
		if ( FLLevelControl.getInstance ().isTileInLevelBoudaries ( x, z ))
		{
			int[][] currentGridLayer = null;
			GameObject[][] currentGameObjectLayer = null;
			currentGridLayer = FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL];
			currentGameObjectLayer = FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL];
			
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
							FLLevelControl.getInstance ().levelGrid[additionalLayer][x][z] = GameElements.EMPTY;
							FLLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] = null;
						}
					}
					return true;
				}
			}
			else if ((( currentGridLayer[x][z] == GameElements.EMPTY ) || ( currentGameObjectLayer[x][z] == objectToBePut ) || ( currentGameObjectLayer[x][z] == null )) && (( additionalLayer == -1 ) || ( FLLevelControl.getInstance ().levelGrid[additionalLayer][x][z] == GameElements.EMPTY ) || ( FLLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] == objectToBePut ) || ( FLLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] == null ))) 
			{
				if ( ! justCheck )
				{
					currentGridLayer[x][z] = ID;
					currentGameObjectLayer[x][z] = objectToBePut;
					
					if ( additionalLayer != -1 )
					{
						FLLevelControl.getInstance ().levelGrid[additionalLayer][x][z] = ID;
						FLLevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] = objectToBePut;
					}
				}
				return true;
			}
		}
		
		return false;
	}
}
