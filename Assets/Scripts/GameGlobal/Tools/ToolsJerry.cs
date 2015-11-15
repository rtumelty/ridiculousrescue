using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ToolsJerry 
{
	public static float CUSTOM_PITCH = 1f;
	public const int OMMIT_IDS = -7;
	
	public static int[] findClossestCharacterTileAroundThisTileMining ( int x, int y, int objectsID, GameObject objectsGameObject, int edgeRadius = 1 )
	{
		int[] returnTile = new int[2] { -1, -1 };
		float currentClossestDistanceForThisTile = 777f;
		if ( objectsGameObject )
		{
			for ( int addX = -edgeRadius; addX <= edgeRadius; addX++ )
			{
				for ( int addY = -edgeRadius; addY <= edgeRadius; addY++ )
				{
					if (( addX != 0 ) || ( addY != 0 ))
					{
						if (( addX == 0 ) || ( addY == 0 ))
						{
							if ( LevelControl.getInstance ().isTileInLevelBoudaries ( x + addX, y + addY ))
							{
								int[][] path = AStar.search ( new int[2] { x, y }, new int[2] { x + addX, y + addY }, false, objectsID, objectsGameObject );
								
								if (( path != null ) && ( ToolsJerry.compareTiles ( path[0], new int[2] { x, y } )))
								{
									if ( Array.IndexOf ( GameElements.CHARACTERS, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] ) != -1 )
									{
										CharacterData currentCharacter = LevelControl.getInstance ().getCharacterFromPosition ( new int[2] { x + addX, y + addY });
										if ( currentCharacter != null && currentCharacter.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0 )
										{
											float newDistance = Vector3.Distance ( new Vector3 ( objectsGameObject.transform.position.x, 0f, objectsGameObject.transform.position.y ), new Vector3 (( float ) ( x + addX ), 0f, ( float ) ( y + addY )));
											if ( newDistance < currentClossestDistanceForThisTile )
											{
												currentClossestDistanceForThisTile = newDistance;
												returnTile = new int[2] { x + addX, y + addY };
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		
		return returnTile;
	}
	
	public static int[] findClossestEmptyTileAroundThisTileNoAStar ( int x, int y, int[] starTtile, int objectsID, GameObject objectsGameObject, int edgeRadius = 1 )
	{
		int[] returnTile = new int[2] { -1, -1 };
		float currentClossestDistanceForThisTile = 777f;
		
		if ( objectsGameObject )
		{
			for ( int addX = -edgeRadius; addX <= edgeRadius; addX++ )
			{
				for ( int addY = -edgeRadius; addY <= edgeRadius; addY++ )
				{
					if (( addX != 0 ) || ( addY != 0 ))
					{
						if ( LevelControl.getInstance ().isTileInLevelBoudaries ( x + addX, y + addY ))
						{
							if ((( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == objectsGameObject )) && 
								( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][x + addX][y + addY] == GameElements.EMPTY ) && 
								( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][x + addX][y + addY] == GameElements.EMPTY ))
							{
								float newDistance = Vector3.Distance ( new Vector3 ( objectsGameObject.transform.position.x, 0f, objectsGameObject.transform.position.y ), new Vector3 (( float ) ( x + addX ), 0f, ( float ) ( y + addY )));
							
								if ( newDistance < currentClossestDistanceForThisTile )
								{
									currentClossestDistanceForThisTile = newDistance;
									returnTile = new int[2] { x + addX, y + addY };
								}
							}
						}
					}
				}
			}
		}
		
		return returnTile;
	}
	
	public static int[] findClossestEmptyTileAroundThisTile ( int x, int y, int[] starTtile, int objectsID, GameObject objectsGameObject, int edgeRadius = 1, int[] preferenceTile = null, int[] ommitThisTile = null )
	{
		bool fourAndFive = false;
		if ( x == 4 && y == 5 )
		{
			fourAndFive = true;
		}

		int[] returnTile = new int[2] { -1, -1 };
		float currentClossestDistanceForThisTile = 777f;
		bool prefreneceTileIsOk = false;
		
		if ( preferenceTile != null )
		{
			if ( LevelControl.getInstance ().isTileInLevelBoudaries ( preferenceTile[0], preferenceTile[1] ))
			{
				int[][] path = AStar.search ( starTtile, preferenceTile, false, objectsID, objectsGameObject );
			
				if (( path != null ) && ( ToolsJerry.compareTiles ( path[0], starTtile )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( objectsID, objectsGameObject, path ))
				{
					if (( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][preferenceTile[0]][preferenceTile[1]] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][preferenceTile[0]][preferenceTile[1]] == objectsGameObject ))
					{
						if ((( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][preferenceTile[0]][preferenceTile[1]] == GameElements.EMPTY ) && ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][preferenceTile[0]][preferenceTile[1]] == GameElements.EMPTY )) || ( Array.IndexOf ( GameElements.BUILDERS, objectsID ) != -1 ))
						{
							prefreneceTileIsOk = true;
							returnTile = new int[2] { preferenceTile[0], preferenceTile[1] };
						}
					}
				}
			}
		}
		
		if ( ! prefreneceTileIsOk )
		{
			if ( objectsGameObject )
			{
				for ( int addX = -edgeRadius; addX <= edgeRadius; addX++ )
				{
					for ( int addY = -edgeRadius; addY <= edgeRadius; addY++ )
					{
						if (( addX != 0 ) || ( addY != 0 ))
						{
							if (( addX == 0 ) || ( addY == 0 ))
							{
								if ( LevelControl.getInstance ().isTileInLevelBoudaries ( x + addX, y + addY ))
								{
									if ( ToolsJerry.compareTiles ( starTtile, new int[2] { x + addX, y + addY } ))
									{
										returnTile = new int[2] { x + addX, y + addY };
										return returnTile;
									}
									
									bool samePositionOfStartThatLookingForAroundTile = true;
									if ( ToolsJerry.compareTiles ( starTtile, new int[2] { x, y } ))
									{
										samePositionOfStartThatLookingForAroundTile = true;
									}

									int[][] path = null;
									if ( ! samePositionOfStartThatLookingForAroundTile ) path = AStar.search ( starTtile, new int[2] { x, y }, true, objectsID, objectsGameObject );
									else path = AStar.search ( starTtile, new int[2] { x + addX, y + addY }, false, objectsID, objectsGameObject );
								
									if (( path != null ) && ( ToolsJerry.compareTiles ( path[0], starTtile )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( objectsID, objectsGameObject, path ))
									{
										if (( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == objectsGameObject ))
										{
											if ((( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_REDIRECTORS][x + addX][y + addY] == GameElements.EMPTY ) && ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_BEAM][x + addX][y + addY] == GameElements.EMPTY )) || ( Array.IndexOf ( GameElements.BUILDERS, objectsID ) != -1 ))
											{
												float newDistance = Vector3.Distance ( new Vector3 ( objectsGameObject.transform.position.x, 0f, objectsGameObject.transform.position.z ), new Vector3 (( float ) ( x + addX ), 0f, ( float ) ( y + addY )));
												if ( newDistance < currentClossestDistanceForThisTile )
												{
													currentClossestDistanceForThisTile = newDistance;
													returnTile = new int[2] { x + addX, y + addY };
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		
		return returnTile;
	}
	
	public static int[] findClossestEmptyTileAroundThisTileMining ( int x, int y, int[] starTtile, int objectsID, GameObject objectsGameObject, int edgeRadius = 1, int[] preferenceTile = null )
	{
		int[] returnTile = new int[2] { -1, -1 };
		float currentClossestDistanceForThisTile = 777f;
		bool prefreneceTileIsOk = false;

		if ( preferenceTile != null )
		{
			if ( LevelControl.getInstance ().isTileInLevelBoudaries ( preferenceTile[0], preferenceTile[1] ))
			{
				int[][] path = AStar.search ( starTtile, preferenceTile, false, objectsID, objectsGameObject );
			
				if (( path != null ) && ( ToolsJerry.compareTiles ( path[0], starTtile )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( objectsID, objectsGameObject, path ))
				{
					if (( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][preferenceTile[0]][preferenceTile[1]] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][preferenceTile[0]][preferenceTile[1]] == objectsGameObject ))
					{
						prefreneceTileIsOk = true;
						returnTile = new int[2] { preferenceTile[0], preferenceTile[1] };
					}
				}
			}
		}

		if ( ! prefreneceTileIsOk )
		{
			if ( objectsGameObject )
			{
				for ( int addX = -edgeRadius; addX <= edgeRadius; addX++ )
				{
					for ( int addY = -edgeRadius; addY <= edgeRadius; addY++ )
					{
						if (( addX != 0 ) || ( addY != 0 ))
						{
							if (( addX == 0 ) || ( addY == 0 ))
							{
								if ( LevelControl.getInstance ().isTileInLevelBoudaries ( x + addX, y + addY ))
								{
									if ( ToolsJerry.compareTiles ( starTtile, new int[2] { x + addX, y + addY } ))
									{
										returnTile = new int[2] { x + addX, y + addY };
										return returnTile;
									}
									
									bool samePositionOfStartThatLookingForAroundTile = true;
									if ( ToolsJerry.compareTiles ( starTtile, new int[2] { x, y } ))
									{
										samePositionOfStartThatLookingForAroundTile = true;
									}

									int[][] path = null;
									if ( ! samePositionOfStartThatLookingForAroundTile ) path = AStar.search ( starTtile, new int[2] { x, y }, true, objectsID, objectsGameObject );
									else path = AStar.search ( starTtile, new int[2] { x + addX, y + addY }, false, objectsID, objectsGameObject );
								
									if (( path != null ) && ( ToolsJerry.compareTiles ( path[0], starTtile )) && LevelControl.getInstance ().allPathIsWalkableForCharacter ( objectsID, objectsGameObject, path ))
									{
										if (( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == objectsGameObject ))
										{
											float newDistance = Vector3.Distance ( new Vector3 ( objectsGameObject.transform.position.x, 0f, objectsGameObject.transform.position.z ), new Vector3 (( float ) ( x + addX ), 0f, ( float ) ( y + addY )));

											if ( newDistance < currentClossestDistanceForThisTile )
											{
												currentClossestDistanceForThisTile = newDistance;
												returnTile = new int[2] { x + addX, y + addY };
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		
		return returnTile;
	}
	
	public static int[] findClossestEmptyTileAroundThisTileNoAStarMining ( int x, int y, int[] starTtile, int objectsID, GameObject objectsGameObject, int edgeRadius = 1 )
	{
		int[] returnTile = new int[2] { -1, -1 };
		float currentClossestDistanceForThisTile = 777f;
		
		if ( objectsGameObject )
		{
			for ( int addX = -edgeRadius; addX <= edgeRadius; addX++ )
			{
				for ( int addY = -edgeRadius; addY <= edgeRadius; addY++ )
				{
					if (( addX != 0 ) || ( addY != 0 ))
					{
						//if (( addX == 0 ) || ( addY == 0 ))
						//{
							if ( LevelControl.getInstance ().isTileInLevelBoudaries ( x + addX, y + addY ))
							{
								if (( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == GameElements.EMPTY ) || ( LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == objectsGameObject ))
								{
									float newDistance = Vector3.Distance ( new Vector3 ( objectsGameObject.transform.position.x, 0f, objectsGameObject.transform.position.y ), new Vector3 (( float ) ( x + addX ), 0f, ( float ) ( y + addY )));
								
									if ( newDistance < currentClossestDistanceForThisTile )
									{
										currentClossestDistanceForThisTile = newDistance;
										returnTile = new int[2] { x + addX, y + addY };
									}
								}
							}
						//}
					}
				}
			}
		}
		
		return returnTile;
	}
	
	public static int[] findClossestEmptyTileAroundThisTileLaboratory ( int x, int y, int[] starTtile, int objectsID, GameObject objectsGameObject, int edgeRadius = 1, int[] preferenceTile = null )
	{
		int[] returnTile = new int[2] { -1, -1 };
		float currentClossestDistanceForThisTile = 777f;
		bool prefreneceTileIsOk = false;
		
		if ( preferenceTile != null )
		{
			if ( FLLevelControl.getInstance ().isTileInLevelBoudaries ( preferenceTile[0], preferenceTile[1] ))
			{
				int[][] path = AStar.search ( starTtile, preferenceTile, false, objectsID, objectsGameObject );
			
				if (( path != null ) && ( ToolsJerry.compareTiles ( path[0], starTtile )) && FLLevelControl.getInstance ().allPathIsWalkableForCharacter ( objectsID, objectsGameObject, path ))
				{
					if (( FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][preferenceTile[0]][preferenceTile[1]] == GameElements.EMPTY ) || ( FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][preferenceTile[0]][preferenceTile[1]] == objectsGameObject ))
					{
						prefreneceTileIsOk = true;
						returnTile = new int[2] { preferenceTile[0], preferenceTile[1] };
					}
				}
			}
		}
		
		if ( ! prefreneceTileIsOk )
		{
			if ( objectsGameObject )
			{
				for ( int addX = -edgeRadius; addX <= edgeRadius; addX++ )
				{
					for ( int addY = -edgeRadius; addY <= edgeRadius; addY++ )
					{
						if (( addX != 0 ) || ( addY != 0 ))
						{
							if ( FLLevelControl.getInstance ().isTileInLevelBoudaries ( x + addX, y + addY ))
							{
								if ( ToolsJerry.compareTiles ( starTtile, new int[2] { x + addX, y + addY } ))
								{
									returnTile = new int[2] { x + addX, y + addY };
									break;
								}
								
								if (( FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == GameElements.EMPTY ) || ( FLLevelControl.getInstance ().gameElementsOnLevel[FLLevelControl.GRID_LAYER_NORMAL][x + addX][y + addY] == objectsGameObject ))
								{
									float newDistance = Vector3.Distance ( new Vector3 ( objectsGameObject.transform.position.x, 0f, objectsGameObject.transform.position.z ), new Vector3 (( float ) ( x + addX ), 0f, ( float ) ( y + addY )));
									if ( newDistance < currentClossestDistanceForThisTile )
									{
										currentClossestDistanceForThisTile = newDistance;
										returnTile = new int[2] { x + addX, y + addY };
									}
								}
							}
						}
					}
				}
			}
		}
		
		return returnTile;
	}
	
	public static int[][] getAllTilesFromPointToPoint ( Vector3 position1, Vector3 position2 )
	{
		List < int[] > allTilesToReturn = new List < int[] > ();
		
		int[] position1Tile = new int[2] { Mathf.RoundToInt ( position1.x ), Mathf.RoundToInt ( position1.z )};
		int[] position2Tile = new int[2] { Mathf.RoundToInt ( position2.x ), Mathf.RoundToInt ( position2.z )};
		
		int xAdd = 0;
		int zAdd = 0;
		
		if ( position2Tile[0] - position1Tile[0] < 0 ) xAdd = -1;
		else if ( position2Tile[0] - position1Tile[0] > 0 ) xAdd = 1;
		else if ( position2Tile[1] - position1Tile[1] < 0 ) zAdd = -1;
		else if ( position2Tile[1] - position1Tile[1] > 0 ) zAdd = 1;
		
		if ( xAdd == 0 && zAdd == 0 ) return null;
		else if ( xAdd != 0 && zAdd != 0 )
		{
			return null;
		}	
		else if ( position1Tile[0] != position2Tile[0] && position1Tile[1] != position2Tile[1] )
		{
			MonoBehaviour.print ( "not rowne: " + position1Tile[0] + " - " + position2Tile[0] + " | " + position1Tile[1] + " - " + position2Tile[1]);
			
			if ( xAdd != 0 ) position2Tile[0] = position1Tile[0];
			else if ( zAdd != 0 ) position2Tile[1] = position1Tile[1];
			
			//return null;
		}
		
		while ( ! ToolsJerry.compareTiles ( position1Tile, position2Tile ))
		{
			position1Tile[0] += xAdd;
			position1Tile[1] += zAdd;
			
			if ( ! ToolsJerry.compareTiles ( position1Tile, position2Tile ))
			{
				allTilesToReturn.Add ( ToolsJerry.cloneTile ( position1Tile ));
			}
			
			if (( position1Tile[0] < -1 ) || ( position1Tile[0] > LevelControl.LEVEL_WIDTH + 1 ) || ( position1Tile[1] < -1 ) || ( position1Tile[1] > LevelControl.LEVEL_HEIGHT + 1 ))
			{
				return null;
			}
		}
		
		return allTilesToReturn.ToArray ();
	}
	
	public static bool compareTiles ( int[] pTileA, int[] pTileB )
	{
		return (( pTileA[0] == pTileB[0] ) && ( pTileA[1] == pTileB[1] ));
	}
	
	public static int[] cloneTile ( int[] pTile )
	{
		int[] returnTile = new int[pTile.Length];
		for ( int i = 0; i < pTile.Length; i++ )
		{
			returnTile[i] = pTile[i];
		}
		
		return returnTile;
	}
	
	public static int[][] cloneTiles ( int[][] pTiles )
	{
		int[][] returnTiles = new int[pTiles.Length][];
		for ( int i = 0; i < pTiles.Length; i++ )
		{
			if ( pTiles[i] != null )
			{
				returnTiles[i] = new int[2];
				returnTiles[i][0] = pTiles[i][0];
				returnTiles[i][1] = pTiles[i][1];
			}
			else break;
		}
		
		return returnTiles;
	}
	
	public static bool comparePathes ( int[][] pathA, int[][] pathB )
	{
		if ( pathA != null & pathB != null )
		{
			if ( pathA.Length == pathB.Length )
			{
				for ( int i = 0; i < pathA.Length; i++ )
				{
					if ( compareTiles ( pathA[i], pathB[i] ))
					{
						continue;
					}
					else
					{
						return false;
					}
				}
			}
			else return false;
		}
		
		return true;
	}
}
