using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNSpawningEnemiesManager : MonoBehaviour 
{
	//*************************************************************//
	public static int MAXIMUM_NUMBER_OF_ENEMIES = 10;
	public static List < int[] > RESERVED_TILES;
	//*************************************************************//
	public bool startSpawn = true;
	//*************************************************************//
	private float _countTimeToNextSpawn = 5f;
	private float _countMinutes = 0f;
	private List < int[] > _holesTiles;
	private List < bool > _holesTilesOccupied;
	private GameObject _slugPrefab;
	private int levelNum = 1000;
	//*************************************************************//
	private static MNSpawningEnemiesManager _meInstance;
	public static MNSpawningEnemiesManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_enemySpawningObject" ).GetComponent < MNSpawningEnemiesManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		// to prevent from delay of loading slug in runtime
		_slugPrefab = ( GameObject ) Resources.Load ( "Spine/Slug/spine" );

		GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _slugPrefab, new Vector3 ( -100f, -100f, -100f ), _slugPrefab.transform.rotation );
		
		IComponent currentIComponent = interactiveObjectInstant.AddComponent < IComponent > ();
		currentIComponent.myID = GameElements.ENEM_SLUG_01;

		interactiveObjectInstant.AddComponent < SelectedComponenent > ();
		MNEnemyComponent currentEnemyComponent = interactiveObjectInstant.AddComponent < MNEnemyComponent > ();
		interactiveObjectInstant.AddComponent < EnemyAnimationComponent > ();
		
		interactiveObjectInstant.tag = GlobalVariables.Tags.INTERACTIVE;
	}

	public void updateHolesOnLevel ()
	{
//		print ("Holes Updated?");
		RESERVED_TILES = new List < int[] > ();
		_holesTiles = new List < int[] > ();
		_holesTilesOccupied = new List < bool > ();


		RESERVED_TILES.Add ( new int[] { 0, 0 } );
		RESERVED_TILES.Add ( new int[] { 1, 0 } );
		RESERVED_TILES.Add ( new int[] { 2, 0 } );
		RESERVED_TILES.Add ( new int[] { 0, 1 } );
		RESERVED_TILES.Add ( new int[] { 1, 1 } );
		RESERVED_TILES.Add ( new int[] { 2, 1 } );
		RESERVED_TILES.Add ( new int[] { 0, 2 } );
		RESERVED_TILES.Add ( new int[] { 1, 2 } );
		RESERVED_TILES.Add ( new int[] { 2, 2 } );
		RESERVED_TILES.Add ( new int[] { 0, 3 } );
		RESERVED_TILES.Add ( new int[] { 1, 3 } );
		RESERVED_TILES.Add ( new int[] { 2, 3 } );

		for ( int i = 0; i < LevelControl.LEVEL_WIDTH; i++ )
		{
//			print ("HoleStep 1");
			for ( int j = 0; j < LevelControl.LEVEL_HEIGHT; j++ )
			{
//				print ("HoleStep 2");
				if ( GameElements.isInBorders ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][i][j], GameElements.HOLES_BORDERS ))
				{
//					print ("HoleStep 3");
					_holesTiles.Add ( new int[2] { i, j });
					_holesTilesOccupied.Add ( false );

					RESERVED_TILES.Add ( new int[] { i - 1, j } );
					RESERVED_TILES.Add ( new int[] { i - 1, j + 1 } );
					RESERVED_TILES.Add ( new int[] { i, j + 1 } );
					RESERVED_TILES.Add ( new int[] { i + 1, j + 1 } );
					RESERVED_TILES.Add ( new int[] { i + 1, j } );
					RESERVED_TILES.Add ( new int[] { i + 1, j - 1 } );
					RESERVED_TILES.Add ( new int[] { i, j - 1 } );
					RESERVED_TILES.Add ( new int[] { i - 1, j - 1 } );
				}
			}
		}
//		print("# of holes" + _holesTiles.Count);
	}
	
	void Update () 
	{
		if ( ! startSpawn ) return; 

		_countMinutes += Time.deltaTime;
		_countTimeToNextSpawn -= Time.deltaTime;
		
		if ( _countTimeToNextSpawn <= 0f )
		{
			/*if ( getCountEnmiesOnLevel () < MAXIMUM_NUMBER_OF_ENEMIES )
			{
				spawnRandomEnemy ();
			}*/
			
			if ( _countMinutes < 60f )
			{
				_countTimeToNextSpawn = 5f;
			}
			else if (( _countMinutes >= 60f ) && ( _countMinutes < 120f ))
			{
				_countTimeToNextSpawn = 3.5f;
			}
			else if ( _countMinutes >= 120f )
			{
				_countTimeToNextSpawn = 1.5f;
			}
		}
	}
	
	private int getCountEnmiesOnLevel ()
	{
		int number = 0;
		if ( LevelControl.getInstance ().enemiesOnLevel == null ) return number;
		foreach ( MNEnemyComponent enemy in LevelControl.getInstance ().enemiesOnLevel )
		{
			if ( enemy != null )
			{
				number++;
			}
		}
		
		return number;
	}
	
	public void spawnRandomEnemy (bool allowSlug = false)
	{
		if(int.TryParse(LevelControl.CURRENT_LEVEL_CLASS.myName, out levelNum))
		{
			if(levelNum <= 5)
			{
//				print ("WHAM");
				return;
			}
		}
		if ( getCountEnmiesOnLevel () > MAXIMUM_NUMBER_OF_ENEMIES )
		{
			return;
		}
//		print ("Spawning");
		int randomEnemyID = 0;
		if(allowSlug == true)
		{
			randomEnemyID = GameElements.ENEMIES[UnityEngine.Random.Range ( 0, GameElements.ENEMIES.Length )];
//			print ("Random Id" + randomEnemyID);
		}
		else
		{
			randomEnemyID = 31;
		}

		switch ( randomEnemyID )
		{
			case GameElements.ENEM_TENTACLEDRAINER_01:
			case GameElements.ENEM_TENTACLEDRAINER_02:
				List < int[] > freeTiles = new List < int[] > (); 
				for ( int i = 0; i < LevelControl.LEVEL_WIDTH; i++ )
				{
					for ( int j = 0; j < LevelControl.LEVEL_HEIGHT; j++ )
					{
						if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][i][j] == GameElements.EMPTY )
						{
							bool freeTileRealy = true;
							foreach ( int[] tile in RESERVED_TILES )
							{
								if ( ToolsJerry.compareTiles ( tile, new int[2] { i, j }))
								{
									freeTileRealy = false;
									break;
								}
							}

							if ( freeTileRealy ) freeTiles.Add ( new int[2] { i, j });
						}
					}
				}
				
				int[] randomTile = freeTiles[UnityEngine.Random.Range ( 0, freeTiles.Count )];
				LevelControl.getInstance ().createObjectOnPosition ( randomEnemyID, randomTile );
				break;
			case GameElements.ENEM_SLUG_01:
//			print ("Made A Slug");
			if(_holesTiles.Count < 1)
			{
				return;
			}
				int randomHoleID = UnityEngine.Random.Range ( 0, _holesTiles.Count );
				if ( _holesTilesOccupied.Count > 0 && _holesTilesOccupied[randomHoleID] )
				{
					randomEnemyID = GameElements.ENEMIES[UnityEngine.Random.Range ( 0, GameElements.ENEMIES_TENTACLES.Length )];
					List < int[] > freeTilesAgain = new List < int[] > (); 
					for ( int i = 6; i < LevelControl.LEVEL_WIDTH; i++ )
					{
						for ( int j = 0; j < LevelControl.LEVEL_HEIGHT; j++ )
						{
							if ( LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL][i][j] == GameElements.EMPTY )
							{
								bool freeTileRealy = true;
								foreach ( int[] tile in RESERVED_TILES )
								{
									if ( ToolsJerry.compareTiles ( tile, new int[2] { i, j }))
									{
										freeTileRealy = false;
										break;
									}
								}
								
								if ( freeTileRealy ) freeTilesAgain.Add ( new int[2] { i, j });
							}
						}
					}
					
					int[] randomTileAgain = freeTilesAgain[UnityEngine.Random.Range ( 0, freeTilesAgain.Count )];
					LevelControl.getInstance ().createObjectOnPosition ( randomEnemyID, randomTileAgain );
					return;
				}
				int[] randomTileHole = _holesTiles[randomHoleID];
				_holesTilesOccupied[randomHoleID] = true;
				GameObject slugObject = LevelControl.getInstance ().createObjectOnPosition ( randomEnemyID, randomTileHole );
				slugObject.GetComponent < IComponent > ().myEnemyData.myHoleID = randomHoleID;

				int[] freeTile = new int[2] { randomTileHole[0] - 1, randomTileHole[1] };
			
				if (( LevelControl.getInstance ().isTileInLevelBoudaries ( freeTile[0], freeTile[1] )) && ( GridReservationManager.getInstance ().fillTileWithMe ( randomEnemyID, freeTile[0], freeTile[1], slugObject, randomEnemyID, true )))
				{
					if ( ! GridReservationManager.getInstance ().fillTileWithMe ( randomEnemyID, freeTile[0], freeTile[1], slugObject, randomEnemyID, false ))
					{
						print ( "Cannot register slug!" );
					}
				
					slugObject.GetComponent < IComponent > ().position = freeTile;
				
					Vector3 positionToGo = new Vector3 ((float) freeTile[0], (float) ( LevelControl.LEVEL_HEIGHT - freeTile[1] ), (float) freeTile[1] - 0.5f );
					iTween.MoveTo ( slugObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutCirc, "position", positionToGo ));
					StartCoroutine ( "startMovingAfterTime", slugObject );
					
				}
				else
				{
					_holesTilesOccupied[randomHoleID] = false;
					Destroy ( slugObject );
				}
			
				break;
		}
	}

	public void unRegisterHole ( int holeID )
	{
		if ( holeID == -1 ) return;
		_holesTilesOccupied[holeID] = false;
	}
	
	private IEnumerator startMovingAfterTime ( GameObject slugObject )
	{
		yield return new WaitForSeconds ( 1f );
		IComponent currentIComponent = slugObject.GetComponent < IComponent > ();

		slugObject.GetComponent < MNSlugMovementComponent > ().position1 = ToolsJerry.cloneTile ( currentIComponent.position );
		slugObject.GetComponent < MNSlugMovementComponent > ().position2 = ToolsJerry.cloneTile ( new int[2] { currentIComponent.position[0] + 1, currentIComponent.position[1] + 1 });
		slugObject.GetComponent < MNSlugMovementComponent > ().position3 = ToolsJerry.cloneTile ( new int[2] { currentIComponent.position[0] + 2, currentIComponent.position[1] });
		slugObject.GetComponent < MNSlugMovementComponent > ().position4 = ToolsJerry.cloneTile ( new int[2] { currentIComponent.position[0] + 1, currentIComponent.position[1] - 1 });

		int[][] path = AStar.search ( currentIComponent.position, slugObject.GetComponent < MNSlugMovementComponent > ().position2, false, currentIComponent.myID, slugObject );
		slugObject.GetComponent < MNSlugMovementComponent > ().initMove ( path, slugObject.GetComponent < MNSlugMovementComponent > ().position2 );
	}
}
