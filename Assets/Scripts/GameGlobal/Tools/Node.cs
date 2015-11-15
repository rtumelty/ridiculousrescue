using UnityEngine;
using System;
using System.Collections;

/**
 * Framework Class
 * @author stellar
 * @c#_port Hubert and Jerry 
 */

public class Node /*: IComparable < Node >*/
{
	public float f = 0;
	public float g = 0;
	public float h = 0;
	public Node parent = null;
	public float terrainCost = 0;
	public float cost = 0;
	public int positionX;
	public int positionY;

	public GridReservationManager gridReservationManager;

	public bool walkable ( int objectType, GameObject gameObjectOfElementSeekingPath, int[] target )
	{
		return checkWalkable( objectType, gameObjectOfElementSeekingPath, target );			
	}
	
	public Node (int positionYValue, int positionXValue )
	{
		this.positionY = positionYValue;
        this.positionX = positionXValue;
	}

	private bool checkWalkable ( int objectType, GameObject gameObjectOfElementSeekingPath, int[] target )
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( ToolsJerry.compareTiles ( target, new int[2] { positionX, positionY } ))
			{
				return true;
			}
			if ( gameObjectOfElementSeekingPath == LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][positionX][positionY] )
			{
				return true;
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			if ( gameObjectOfElementSeekingPath == FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][positionX][positionY] ) return true;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			if ( ToolsJerry.compareTiles ( target, new int[2] { positionX, positionY } )) return true;
			if ( gameObjectOfElementSeekingPath == MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionX][positionY] ) return true;
		}
		
		terrainCost = 0;
		
		int lLevelGridValue = 0;
		int lLevelGridValueredirectors = 0;
		int lLevelGridValueBeam = 0;
		int lLevelGridValueSlugPath = 0;

		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			lLevelGridValue = LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][positionX][positionY];
			lLevelGridValueredirectors = LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][positionX][positionY];
			lLevelGridValueBeam = LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][positionX][positionY];
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY ) 
		{
			lLevelGridValue = FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][positionX][positionY];
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			lLevelGridValue = MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][positionX][positionY];
			lLevelGridValueSlugPath = MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_SLUG_LONGER_PATH][positionX][positionY];
		}
		
		if ( lLevelGridValueBeam != GameElements.EMPTY )
		{
			if ( Array.IndexOf ( GameElements.BUILDERS, objectType ) == -1 )
			{
				terrainCost = 150;
			}
		}
		
		switch ( lLevelGridValue )
		{
			case GameElements.EMPTY:
				if ( lLevelGridValueredirectors != GameElements.EMPTY )
				{
					if ( Array.IndexOf ( GameElements.BUILDERS, objectType ) != -1 )
					{
						return true;
					}
					else return false;
				}
				else return true;
		}

		if ( objectType == GameElements.ENEM_SLUG_01 )
		{
			if ( lLevelGridValueSlugPath == GameElements.SLUG_DUMMY_TILE_FOR_LONGER_PATH )
			{
				terrainCost = 150;
			}
		}
		
		if ( Array.IndexOf ( GameElements.CHARACTERS, lLevelGridValue ) != -1 )
		{
			if ( lLevelGridValueredirectors != GameElements.EMPTY )
			{
				if ( Array.IndexOf ( GameElements.BUILDERS, objectType ) != -1 )
				{
					terrainCost = 150;
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{	
				if ( Array.IndexOf ( GameElements.ENEMIES, objectType ) == -1 )
				{
					if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
					{
						MoveComponent currentMoveComponent = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][positionX][positionY].GetComponent < MoveComponent > ();
						if ( currentMoveComponent != null && ! currentMoveComponent.isMoving )
						{
							terrainCost = 150;
						}
					
					}
					else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY ) 
					{
						FLMoveComponent currentFLMoveComponent = FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][positionX][positionY].GetComponent < FLMoveComponent > ();
						if ( currentFLMoveComponent != null && ! currentFLMoveComponent.isMoving )
						{
							terrainCost = 150;
						}
					}
					else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
					{
						MNMoveComponent currentMNMoveComponent = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][positionX][positionY].GetComponent < MNMoveComponent > ();
						if ( currentMNMoveComponent != null && ! currentMNMoveComponent.isMoving )
						{
							terrainCost = 150;
						}
					}
					return true;
				}
				else
				{
					return true;
				}
			}
		}
		
		if ( Array.IndexOf ( GameElements.ENEMIES, objectType ) != -1 )
		{
			if ( Array.IndexOf ( GameElements.ENEMIES, lLevelGridValue ) != -1 || Array.IndexOf ( GameElements.CHARACTERS, lLevelGridValue ) != -1 )
			{
				return true;
			}
		}
		return false;
	}
}

