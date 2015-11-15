using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridReservationManager : MonoBehaviour 
{
	//*************************************************************//	
	public List<int[]> myReservedSpacesForChars;
	public int[] myPos;
	private static GridReservationManager _meInstance;
	private float countDown = 0.5f;
	public static GridReservationManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_LevelObject" ).GetComponent < GridReservationManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//

	void Update()
	{
		countDown -= Time.deltaTime;
		if(countDown > 0)
		{
			return;
		}
		countDown = 0.5f;
		myReservedSpacesForChars = new List<int[]>();
		foreach(GameObject toon in GameObject.FindGameObjectsWithTag("Character"))
		{
			myPos = toon.GetComponent<IComponent>().position;
//			Debug.Log("Updated" + myPos[0] + " " + myPos[1]);
			//myPos = new int[2];
			//myPos[0] = toon.position[0];
			//myPos[1] = toon.position[1];
			myReservedSpacesForChars.Add(myPos);
		}
	}


	public bool fillTileWithMe ( int ID, int x, int z, GameObject objectToBePut, int idOfRequestingObject, bool justCheck = false, int additionalLayer = -1 )
	{
		if ( LevelControl.getInstance ().isTileInLevelBoudaries ( x, z ))
		{
			int[][] currentGridLayer = null;
			GameObject[][] currentGameObjectLayer = null;
			if ( Array.IndexOf ( GameElements.REDIRECTORS, idOfRequestingObject ) != -1 )
			{
				if ( additionalLayer == LevelControl.GRID_LAYER_BEAM )
				{
					currentGridLayer = LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM];
					currentGameObjectLayer = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_BEAM];
				}
				else
				{
					currentGridLayer = LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS];
					currentGameObjectLayer = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_REDIRECTORS];
				}
			}
			else
			{
				currentGridLayer = LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL];
				currentGameObjectLayer = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL];
			}
			
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
							LevelControl.getInstance ().levelGrid[additionalLayer][x][z] = GameElements.EMPTY;
							LevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] = null;
						}
					}
					
					return true;
				}
			}
			else if ((( currentGridLayer[x][z] == GameElements.EMPTY ) || ( currentGameObjectLayer[x][z] == objectToBePut ) || ( currentGameObjectLayer[x][z] == null )) && (( additionalLayer == -1 ) || ( LevelControl.getInstance ().levelGrid[additionalLayer][x][z] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] == objectToBePut ) || ( LevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] == null ))) 
			{
				if ( Array.IndexOf ( GameElements.CHARACTERS, idOfRequestingObject ) != -1 )
				{
					if ( Array.IndexOf ( GameElements.BUILDERS, idOfRequestingObject ) == -1 )
					{
						if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][x][z] != GameElements.EMPTY )
						{
							return false;
						}
					}
				}
				
				if ( Array.IndexOf ( GameElements.REDIRECTORS, idOfRequestingObject ) != -1 )
				{
					if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x][z] == GameElements.EMPTY || ( Array.IndexOf ( GameElements.BUILDERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x][z] ) != -1 ))
					{
						// do nothing
					}
					else if ( additionalLayer != LevelControl.GRID_LAYER_BEAM ) return false;
				}
				
				if ( ! justCheck )
				{
					currentGridLayer[x][z] = ID;
					currentGameObjectLayer[x][z] = objectToBePut;
					
					if ( additionalLayer != -1 )
					{
						LevelControl.getInstance ().levelGrid[additionalLayer][x][z] = ID;
						LevelControl.getInstance ().gameElementsOnLevel[additionalLayer][x][z] = objectToBePut;
					}
				}
				return true;
			}
		}
//		print ("It was false?");
		return false;
	}
}
